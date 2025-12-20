using UnityEngine;

public class BreathingBG : MonoBehaviour
{
	[SerializeField]
	private int lowHeight = 56;

	[SerializeField]
	private int highHeight = 270;

	[SerializeField]
	private int segments = 8;

	[SerializeField]
	private int breatheLength = 120;

	private int frames;

	[SerializeField]
	private bool soulColor = true;

	private Color color = new Color32(0, 0, 0, 24);

	private bool generateBalls;

	private void Awake()
	{
		for (int i = 0; i < segments; i++)
		{
			GameObject obj = new GameObject("Segment" + i, typeof(SpriteRenderer));
			obj.transform.SetParent(base.transform, worldPositionStays: false);
			obj.transform.localScale = new Vector3(1f, Mathf.Lerp(1f / (float)segments, 1f, (float)i / (float)segments));
			SpriteRenderer component = obj.GetComponent<SpriteRenderer>();
			component.sprite = Resources.Load<Sprite>("spr_pixel_bot");
			component.color = color;
		}
	}

	private void Update()
	{
		frames = (frames + 1) % breatheLength;
		float num = (float)breatheLength / 2f;
		float num2 = (float)frames / num;
		if (frames > breatheLength / 2)
		{
			num2 = (float)(breatheLength - frames) / num;
		}
		num2 = num2 * num2 * num2 * (num2 * (6f * num2 - 15f) + 10f);
		base.transform.localScale = Vector3.Lerp(new Vector3(700f, lowHeight, 1f), new Vector3(700f, highHeight, 1f), num2);
		if ((bool)Util.FindObjectOfType<SOUL>() && soulColor)
		{
			color = Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().color;
			color.a = 8f / 85f;
		}
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer obj in componentsInChildren)
		{
			obj.color = Color.Lerp(obj.color, color, 0.025f);
		}
		if (generateBalls && frames % 3 == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/Balls"), new Vector3(Random.Range(-7f, 7f), -5.41f), Quaternion.identity);
		}
	}

	public void ToggleSOULColor(bool soulColor)
	{
		this.soulColor = soulColor;
		if (!soulColor)
		{
			color = new Color32(0, 0, 0, 24);
		}
	}

	public void SetColor(Color color)
	{
		this.color = color;
		if (generateBalls && color == new Color(0f, 0f, 0f, 0f))
		{
			generateBalls = false;
		}
	}

	public Color GetColor()
	{
		return color;
	}

	public void StartGeneratingBalls()
	{
		generateBalls = true;
	}
}
