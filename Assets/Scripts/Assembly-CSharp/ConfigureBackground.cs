using UnityEngine;

public class ConfigureBackground : MonoBehaviour
{
	[Header("Texture and Color Gradient")]
	[Tooltip("Use this to set the texture to manipulate")]
	public Texture2D sourceTexture;

	[Tooltip("Use this to configure the color palette to shift")]
	public Gradient colorShiftGradient;

	[Tooltip("Use this to set color palette shifting")]
	public bool colorShift = true;

	[Header("Configuration")]
	[Tooltip("Use this to adjust type of distortion")]
	[Space(25f)]
	[StringInList(new string[] { "No Effect", "Horizontal Offset Wave", "Horizontal Wave", "Horizontal and Vertical Wave", "Horizontal and Vertical Offset Wave", "Vertical Offset Wave", "Vertical Wave" })]
	public string typeOfDistortion;

	[Range(0f, 1f)]
	[Tooltip("Use this to adjust the opacity of this layer")]
	public float opacity;

	[Tooltip("Use this to adjust the blend mode of this layer")]
	[StringInList(new string[] { "Traditional", "Premultiplied", "Additive", "Soft-Additive", "Multiplicative", "2x-Multiplicative" })]
	public string typeOfBlend = "Traditional";

	private int blendSrc;

	private int blendDst;

	[Tooltip("Use this to adjust speed of distortion")]
	[Range(0f, 100f)]
	public float speed;

	[Tooltip("Use this to adjust size of tiling")]
	public Vector2 TilesSize = new Vector2(1f, 1f);

	[Tooltip("Use this to adjust speed of scrolling")]
	public Vector2 ScrollSpeed;

	[Tooltip("Use this to adjust amplitude of the vertex position")]
	public float amplitude;

	[Tooltip("Use this to adjust the frequency that the sinusoidal wave moves")]
	public float frequency;

	[Tooltip("Use this to adjust scale or power of the distortion")]
	public float scale;

	[Tooltip("Use this to change the spacing in the offset effects")]
	public float lineWidth;

	[Tooltip("Use this to adjust hsv manipulation")]
	public float bloomEffect;

	private Texture2D originalTexture;

	private Texture2D resultTexture;

	private Texture2D colorMap;

	private float min;

	private float max = 256f;

	private Renderer rend;

	private static readonly int RampTex = Shader.PropertyToID("_RampTex");

	private static readonly int ColorShift = Shader.PropertyToID("_ColorShift");

	private static readonly int Alpha = Shader.PropertyToID("_Alpha");

	private static readonly int Speed = Shader.PropertyToID("_Speed");

	private static readonly int Amplitude = Shader.PropertyToID("_Amplitude");

	private static readonly int Frequency = Shader.PropertyToID("_Frequency");

	private static readonly int Scale = Shader.PropertyToID("_Scale");

	private static readonly int LineWidth = Shader.PropertyToID("_LineWidth");

	private static readonly int InPatternRand = Shader.PropertyToID("in_PatternRand");

	private static readonly int MainTex = Shader.PropertyToID("_MainTex");

	private static readonly int Max = Shader.PropertyToID("_Max");

	private static readonly int Min = Shader.PropertyToID("_Min");

	private static readonly int DistortionType = Shader.PropertyToID("_DistortionType");

	private static readonly int BlendModeSrc = Shader.PropertyToID("_BlendModeSrc");

	private static readonly int MyDstMode = Shader.PropertyToID("_BlendModeDst");

	private void Awake()
	{
		rend = GetComponent<Renderer>();
		if (base.gameObject.name == "Layer1")
		{
			rend.sortingOrder = -1;
		}
		else
		{
			rend.sortingOrder = -2;
		}
		CreateImageEffect();
		if (colorShift)
		{
			rend.material.SetFloat(ColorShift, 1f);
		}
		else
		{
			rend.material.SetFloat(ColorShift, 0f);
		}
		rend.material.SetFloat(Alpha, opacity);
		rend.material.SetFloat(Speed, speed);
		rend.material.SetFloat(Amplitude, amplitude);
		rend.material.SetFloat(Frequency, frequency);
		rend.material.SetFloat(Scale, scale);
		rend.material.SetFloat(LineWidth, lineWidth);
		rend.material.SetFloat(InPatternRand, bloomEffect);
		switch (typeOfBlend)
		{
		case "Traditional":
			rend.material.SetFloat(BlendModeSrc, 5f);
			rend.material.SetFloat(MyDstMode, 10f);
			break;
		case "Premultiplied":
			rend.material.SetFloat(BlendModeSrc, 1f);
			rend.material.SetFloat(MyDstMode, 10f);
			break;
		case "Additive":
			rend.material.SetFloat(BlendModeSrc, 1f);
			rend.material.SetFloat(MyDstMode, 1f);
			break;
		case "Soft-Additive":
			rend.material.SetFloat(BlendModeSrc, 4f);
			rend.material.SetFloat(MyDstMode, 1f);
			break;
		case "Multiplicative":
			rend.material.SetFloat(BlendModeSrc, 2f);
			rend.material.SetFloat(MyDstMode, 0f);
			break;
		case "2x-Multiplicative":
			rend.material.SetFloat(BlendModeSrc, 2f);
			rend.material.SetFloat(MyDstMode, 3f);
			break;
		default:
			MonoBehaviour.print("Blend mode not correct");
			break;
		}
	}

	private void CreateImageEffect()
	{
		resultTexture = new Texture2D(sourceTexture.width, sourceTexture.height);
		colorMap = new Texture2D(200, 1);
		CreateColorMap();
		ConvertToGrayscale();
		Vector3 position = base.transform.position;
		int num = Mathf.RoundToInt(position.x);
		int num2 = Mathf.RoundToInt(position.y);
		int num3 = Mathf.FloorToInt(sourceTexture.width);
		int num4 = Mathf.FloorToInt(sourceTexture.height);
		for (int i = num2; i < num4; i++)
		{
			for (int j = num; j < num3; j++)
			{
				float num5 = resultTexture.GetPixel(j, i).r * 255f;
				if (num5 > min)
				{
					min = num5;
				}
				if (num5 < max)
				{
					max = num5;
				}
			}
		}
		rend.material.SetTexture(MainTex, resultTexture);
		rend.material.SetFloat(Max, max);
		rend.material.SetFloat(Min, min);
	}

	private void Update()
	{
		if (originalTexture != sourceTexture)
		{
			CreateImageEffect();
			originalTexture = sourceTexture;
		}
		rend.material.SetFloat(ColorShift, colorShift ? 1 : 0);
		rend.material.SetFloat(Alpha, opacity);
		rend.material.SetFloat(Speed, speed);
		rend.material.SetFloat(Amplitude, amplitude);
		rend.material.SetFloat(Frequency, frequency);
		rend.material.SetFloat(Scale, scale);
		rend.material.SetFloat(LineWidth, lineWidth);
		rend.material.SetFloat(InPatternRand, bloomEffect);
		switch (typeOfDistortion)
		{
		case "No Effect":
			rend.material.SetFloat(DistortionType, 0f);
			break;
		case "Horizontal Offset Wave":
			rend.material.SetFloat(DistortionType, 1f);
			break;
		case "Horizontal Wave":
			rend.material.SetFloat(DistortionType, 2f);
			break;
		case "Horizontal and Vertical Wave":
			rend.material.SetFloat(DistortionType, 3f);
			break;
		case "Horizontal and Vertical Offset Wave":
			rend.material.SetFloat(DistortionType, 4f);
			break;
		case "Vertical Offset Wave":
			rend.material.SetFloat(DistortionType, 5f);
			break;
		case "Vertical Wave":
			rend.material.SetFloat(DistortionType, 6f);
			break;
		default:
			rend.material.SetFloat(DistortionType, 0f);
			break;
		}
		switch (typeOfBlend)
		{
		case "Traditional":
			rend.material.SetFloat(BlendModeSrc, 5f);
			rend.material.SetFloat(MyDstMode, 10f);
			break;
		case "Premultiplied":
			rend.material.SetFloat(BlendModeSrc, 1f);
			rend.material.SetFloat(MyDstMode, 10f);
			break;
		case "Additive":
			rend.material.SetFloat(BlendModeSrc, 1f);
			rend.material.SetFloat(MyDstMode, 1f);
			break;
		case "Soft-Additive":
			rend.material.SetFloat(BlendModeSrc, 4f);
			rend.material.SetFloat(MyDstMode, 1f);
			break;
		case "Multiplicative":
			rend.material.SetFloat(BlendModeSrc, 2f);
			rend.material.SetFloat(MyDstMode, 0f);
			break;
		case "2x-Multiplicative":
			rend.material.SetFloat(BlendModeSrc, 2f);
			rend.material.SetFloat(MyDstMode, 3f);
			break;
		default:
			MonoBehaviour.print("Blend mode not correct");
			break;
		}
		float x = Time.time * ScrollSpeed.x % 1f;
		float y = Time.time * ScrollSpeed.y % 1f;
		Material material = rend.material;
		material.mainTextureOffset = new Vector2(x, y);
		material.mainTextureScale = new Vector2(TilesSize.x, TilesSize.y);
	}

	private void CreateColorMap()
	{
		for (int i = 0; i <= 100; i++)
		{
			float time = (float)i / 100f;
			colorMap.SetPixel(i, 1, colorShiftGradient.Evaluate(time));
		}
		for (int num = 100; num >= 0; num--)
		{
			float time2 = (float)num / 100f;
			colorMap.SetPixel(200 - num, 1, colorShiftGradient.Evaluate(time2));
		}
		colorMap.Apply();
		rend.material.SetTexture(RampTex, colorMap);
	}

	private void ConvertToGrayscale()
	{
		for (int i = 0; i < resultTexture.width; i++)
		{
			for (int j = 0; j < resultTexture.height; j++)
			{
				Color pixel = sourceTexture.GetPixel(i, j);
				float num = pixel.r * 0.3f + pixel.g * 0.59f + pixel.b * 0.11f;
				Color color = new Color(num, num, num, pixel.a);
				resultTexture.SetPixel(i, j, color);
			}
		}
		resultTexture.Apply();
	}
}
