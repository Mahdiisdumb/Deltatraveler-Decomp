using System;
using UnityEngine;

public class GonerCreator : MonoBehaviour
{
	private int frames;

	private int mode = -1;

	private bool selecting;

	private float oldX;

	private int selectFrames;

	private bool selectBuffer;

	private int[] part = new int[3];

	private bool[] showPart = new bool[3];

	private float[] partAlpha = new float[3];

	private readonly int[] HIGHEST_PART = new int[3] { 7, 5, 4 };

	private readonly float PART_SPACING = 2.1666667f;

	private void Awake()
	{
		for (int i = 0; i < 3; i++)
		{
			Transform transform = base.transform.Find("Parts").Find(i.ToString()).transform;
			for (int j = 0; j < transform.childCount; j++)
			{
				transform.GetChild(j).transform.localPosition = new Vector3((float)j * PART_SPACING, 0f);
			}
		}
	}

	private void Update()
	{
		frames++;
		base.transform.Find("Parts").localPosition = new Vector3(Mathf.Sin((float)frames / 24f), Mathf.Cos((float)frames / 30f)) / 24f;
		for (int i = 0; i < 3; i++)
		{
			if (showPart[i] && partAlpha[i] < 1f)
			{
				partAlpha[i] += 0.1f;
			}
			else if (!showPart[i] && partAlpha[i] > 0f)
			{
				partAlpha[i] -= 0.1f;
			}
			Transform transform = base.transform.Find("Parts").Find(i.ToString()).transform;
			for (int j = 0; j < transform.childCount; j++)
			{
				SpriteRenderer[] componentsInChildren = transform.GetChild(j).GetComponentsInChildren<SpriteRenderer>();
				foreach (SpriteRenderer spriteRenderer in componentsInChildren)
				{
					if (spriteRenderer.gameObject.name == transform.GetChild(j).gameObject.name)
					{
						if (mode == i)
						{
							float num = 5.25f - Mathf.Abs(spriteRenderer.transform.localPosition.x + transform.localPosition.x);
							if (num < 0f)
							{
								num = 0f;
							}
							spriteRenderer.color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, num / 5.25f) * partAlpha[i]);
						}
						else if (j == part[i])
						{
							spriteRenderer.color = new Color(1f, 1f, 1f, partAlpha[i]);
						}
						else
						{
							spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
						}
						spriteRenderer.sortingOrder = 200 + i;
					}
					else
					{
						if (j == part[i] && (mode != i || (!selecting && mode == i)))
						{
							spriteRenderer.color = new Color(1f, 1f, 1f, 0.5f * partAlpha[i]);
							float t = Mathf.Abs(Mathf.Sin((float)frames * (180f / (float)(spriteRenderer.gameObject.name.EndsWith("0") ? 50 : 65)) * ((float)Math.PI / 180f)));
							float num2 = Mathf.Lerp(1f, 1.25f, t);
							spriteRenderer.transform.localScale = new Vector3(num2, num2, 1f);
						}
						else
						{
							spriteRenderer.color = new Color(1f, 1f, 1f, 0f);
						}
						spriteRenderer.sortingOrder = 150 + i;
					}
				}
			}
		}
		if (mode >= 0 && mode <= 2)
		{
			if (UTInput.GetAxis("Horizontal") != 0f && !selecting && PartsAreShown() && ((part[mode] < HIGHEST_PART[mode] && UTInput.GetAxis("Horizontal") > 0f) || (part[mode] > 0 && UTInput.GetAxis("Horizontal") < 0f)))
			{
				part[mode] += (int)UTInput.GetAxis("Horizontal");
				oldX = base.transform.Find("Parts").Find(mode.ToString()).transform.localPosition.x;
				selectFrames = 0;
				selecting = true;
			}
			if (UTInput.GetButtonDown("Z") && PartsAreShown())
			{
				selectBuffer = true;
			}
		}
		if (selecting)
		{
			selectFrames++;
			float b = (float)part[mode] * (0f - PART_SPACING);
			Transform transform2 = base.transform.Find("Parts").Find(mode.ToString()).transform;
			transform2.localPosition = new Vector3(Mathf.Lerp(oldX, b, (float)selectFrames / 5f), transform2.localPosition.y);
			if (selectFrames == 5)
			{
				selecting = false;
			}
		}
		if (selectBuffer && !selecting)
		{
			mode = -1;
			Hide();
			selectBuffer = false;
		}
	}

	public void StartMode(int mode, bool resetFrames = false)
	{
		this.mode = mode;
		for (int i = 0; i < ((mode < 2) ? (mode + 1) : 3); i++)
		{
			showPart[i] = true;
		}
		if (resetFrames)
		{
			frames = 0;
		}
	}

	public void Hide()
	{
		for (int i = 0; i < 3; i++)
		{
			showPart[i] = false;
		}
	}

	public void SetValues()
	{
		Util.GameManager().SetFlag(215, part[0]);
		Util.GameManager().SetFlag(216, part[1]);
		Util.GameManager().SetFlag(217, (part[2] == 4) ? 1 : 0);
		UnityEngine.Object.FindObjectOfType<GonerParts>().SetPrefixes();
	}

	public bool PartsAreHidden()
	{
		for (int i = 0; i < 3; i++)
		{
			if (partAlpha[i] > 0f)
			{
				return false;
			}
		}
		return true;
	}

	public bool PartsAreShown()
	{
		for (int i = 0; i < mode; i++)
		{
			if (partAlpha[i] < 1f)
			{
				return false;
			}
		}
		return true;
	}

	public int GetMode()
	{
		return mode;
	}
}
