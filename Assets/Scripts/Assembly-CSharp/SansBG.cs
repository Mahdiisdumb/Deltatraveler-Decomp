using System;
using UnityEngine;

public class SansBG : MonoBehaviour
{
	private readonly string[] LAYERS = new string[2] { "BGTileBottomLayer", "BGTileTopLayer" };

	private int frames;

	private bool fadeIn;

	private int fadeFrames;

	private bool moveBones;

	private Transform frontBones;

	private Transform backBones;

	private bool frozen;

	private void Awake()
	{
		frontBones = base.transform.Find("BonesFront");
		backBones = base.transform.Find("BonesBack");
	}

	private void Update()
	{
		if (frozen)
		{
			return;
		}
		if (moveBones)
		{
			frontBones.localPosition -= new Vector3(1f / 12f, 0f);
			if (frontBones.localPosition.x < -8f)
			{
				frontBones.localPosition += new Vector3(11f / 24f, 0f);
			}
			backBones.localPosition += new Vector3(1f / 24f, 0f);
			if (backBones.localPosition.x > 8f)
			{
				backBones.localPosition -= new Vector3(0.5f, 0f);
			}
		}
		if (fadeIn && fadeFrames < 90)
		{
			fadeFrames++;
		}
		else if (!fadeIn && fadeFrames > 0)
		{
			fadeFrames--;
		}
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f - (float)fadeFrames / 90f);
		frames = (frames + 1) % 96;
		float t = (0f - Mathf.Cos((float)frames * 3.75f * ((float)Math.PI / 180f)) + 1f) / 2f;
		base.transform.Find("TopGradient").localScale = new Vector3(1.25f, Mathf.Lerp(1f / 12f, 1f, t), 1f);
		base.transform.Find("BottomGradient").localScale = new Vector3(1.25f, Mathf.Lerp(1f, 0.5f, t), 1f);
		for (int i = 0; i < 6; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num = frames + i * 16;
				if (j == 1)
				{
					num += 9;
				}
				t = Mathf.Sin((float)num * 3.75f * ((float)Math.PI / 180f));
				Transform child = base.transform.Find(LAYERS[j]).GetChild(i);
				child.localPosition = new Vector3(child.localPosition.x, 2.35f + 8f * t / 48f);
			}
		}
	}

	public void SetFreeze(bool frozen)
	{
		this.frozen = frozen;
	}

	public void FadeIn(bool moveBones = true)
	{
		fadeIn = true;
		this.moveBones = moveBones;
	}

	public void FadeOut()
	{
		fadeIn = false;
	}
}
