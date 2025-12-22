using System;
using UnityEngine;

public class EarthboundBG : MonoBehaviour
{
	public enum DistortionType
	{
		None = 0,
		Horizontal = 1,
		HorizontalInterlaced = 2,
		Vertical = 3,
		ReverseHorizontalInterlaced = 4
	}

	public enum PaletteCycleType
	{
		None = 0,
		Forward = 1,
		Reverse = 2,
		ForwardReverse = 3
	}

	[Serializable]
	public struct Background
	{
		public PaletteCycleType palCycleType;

		public int palCycleSpeed;

		public Color32[] palette;

		public BackgroundScroll[] scrolls;

		public BackgroundDistortion[] distortions;
	}

	[Serializable]
	public struct BackgroundScroll
	{
		public int duration;

		public int hAccel;

		public int hMovement;

		public int vAccel;

		public int vMovement;
	}

	[Serializable]
	public struct BackgroundDistortion
	{
		public DistortionType type;

		public int speed;

		public int rippleAmp;

		public int rippleAmpAccel;

		public int rippleFreq;

		public int rippleFreqAccel;
	}

	private MaterialPropertyBlock materialPropertyBlock;

	[SerializeField]
	public Background background;

	private float palCycleTime;

	private int currentScroll;

	private float scrollTime;

	private int currentDistortion;

	private float distortionTime;

	private void Awake()
	{
		UpdateBG();
	}

	private void Update()
	{
		if (background.palCycleType > PaletteCycleType.None)
		{
			palCycleTime += Time.deltaTime;
		}
		if (background.palCycleType == PaletteCycleType.Forward)
		{
			materialPropertyBlock.SetInt("_PaletteOffset", Mathf.FloorToInt((0f - palCycleTime) * 60f / (float)background.palCycleSpeed));
		}
		else if (background.palCycleType == PaletteCycleType.Reverse)
		{
			materialPropertyBlock.SetInt("_PaletteOffset", Mathf.FloorToInt(palCycleTime * 60f / (float)background.palCycleSpeed));
		}
		else
		{
			materialPropertyBlock.SetInt("_PaletteOffset", 0);
		}
		distortionTime += Time.deltaTime * 24f;
		materialPropertyBlock.SetFloat("_DistortionTime", distortionTime);
		scrollTime += Time.deltaTime * 24f;
		materialPropertyBlock.SetFloat("_ScrollTime", scrollTime);
		GetComponent<SpriteRenderer>().SetPropertyBlock(materialPropertyBlock);
	}

	private void UpdateDistortionInfo()
	{
		if (background.distortions.Length != 0)
		{
			BackgroundDistortion backgroundDistortion = background.distortions[currentDistortion];
			materialPropertyBlock.SetInt("_DistortionType", (int)backgroundDistortion.type);
			materialPropertyBlock.SetFloat("_DistortionSpeed", backgroundDistortion.speed);
			materialPropertyBlock.SetFloat("_RippleFreq", backgroundDistortion.rippleFreq);
			materialPropertyBlock.SetFloat("_RippleAmp", backgroundDistortion.rippleAmp);
		}
	}

	private void UpdateScrollInfo()
	{
		if (background.scrolls.Length != 0)
		{
			BackgroundScroll backgroundScroll = background.scrolls[currentScroll];
			materialPropertyBlock.SetFloat("_ScrollDuration", backgroundScroll.duration);
			materialPropertyBlock.SetFloat("_ScrollHAccel", backgroundScroll.hAccel);
			materialPropertyBlock.SetFloat("_ScrollHMovement", backgroundScroll.hMovement);
			materialPropertyBlock.SetFloat("_ScrollVAccel", backgroundScroll.vAccel);
			materialPropertyBlock.SetFloat("_ScrollVMovement", backgroundScroll.vMovement);
		}
	}

	private void UpdateBG()
	{
		if (materialPropertyBlock == null)
		{
			materialPropertyBlock = new MaterialPropertyBlock();
		}
		materialPropertyBlock.SetInt("_EnablePalette", (background.palCycleType > PaletteCycleType.None) ? 1 : 0);
		Vector4[] array = new Vector4[32];
		for (int i = 0; i < background.palette.Length * 2; i++)
		{
			int num = i;
			if (i >= background.palette.Length)
			{
				num = background.palette.Length * 2 - i - 1;
			}
			Debug.Log(num);
			Color color = background.palette[num];
			array[i] = new Vector4(color.r, color.g, color.b, color.a);
		}
		materialPropertyBlock.SetVectorArray("_Palette", array);
		materialPropertyBlock.SetInt("_PaletteCount", background.palette.Length * 2);
		UpdateDistortionInfo();
		UpdateScrollInfo();
		GetComponent<SpriteRenderer>().SetPropertyBlock(materialPropertyBlock);
	}
}
