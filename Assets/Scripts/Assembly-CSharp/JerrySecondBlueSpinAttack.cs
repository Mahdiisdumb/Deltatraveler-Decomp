using System;
using System.Collections.Generic;
using UnityEngine;

public class JerrySecondBlueSpinAttack : AttackBase
{
	private Jerry jerry;

	private GameObject slashPrefab;

	private GameObject platformPrefab;

	private Transform sword;

	private Transform swordSpinning;

	private SpriteRenderer spinSword;

	private float angle;

	private float angleAcceleration;

	private float soulPush;

	private SOUL soul;

	private List<float[]> safetyRanges = new List<float[]>();

	private List<Transform> platforms = new List<Transform>();

	private List<int> safetyRangeRegistered = new List<int>();

	private Transform spikeLeft;

	private Transform spikeRight;

	private int spinSoundFrames;

	private int spinSoundRate = 12;

	private float swordVelocity = 1f / 48f;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(215f, 140f);
		slashPrefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashBlue");
		platformPrefab = Resources.Load<GameObject>("battle/attacks/MedPlatform");
		sword = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySword"), new Vector3(10f, 10f), Quaternion.identity, base.transform).transform;
		sword.GetComponent<SpriteRenderer>().color = new Color32(0, 60, byte.MaxValue, byte.MaxValue);
		sword.GetComponent<BoxCollider2D>().enabled = false;
		jerry = UnityEngine.Object.FindObjectOfType<Jerry>();
		attackAllTargets = false;
		soul = UnityEngine.Object.FindObjectOfType<SOUL>();
		spikeLeft = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweySpikeInterior"), new Vector3(-10f, 0f), Quaternion.Euler(0f, 0f, -90f), base.transform).transform;
		spikeRight = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweySpikeInterior"), new Vector3(10f, 0f), Quaternion.Euler(0f, 0f, 90f), base.transform).transform;
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames >= 5 && frames <= 20)
			{
				float num = (float)(frames - 5) / 15f;
				num = Mathf.Sin(num * (float)Math.PI * 0.5f);
				sword.position = Vector3.Lerp(new Vector3(-1.09f, 2.84f), new Vector3(-1.03f, 4.76f), num);
				sword.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-76f, -112f, num));
			}
			else if (frames > 20)
			{
				float num2 = (float)(frames - 20) / 25f;
				num2 = num2 * num2 * num2 * (num2 * (6f * num2 - 15f) + 10f);
				sword.position = Vector3.Lerp(new Vector3(-1.03f, 4.76f), new Vector3(4.25f, -2.6f), num2) + new Vector3(0f, Mathf.Sin((float)(frames - 20) * 7.2f * ((float)Math.PI / 180f)) * ((float)frames / 45f));
				sword.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-112f, -360f, num2));
			}
			if (frames == 1)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_0");
				jerry.GetPart("body").Find("headband").localPosition -= new Vector3(0f, 1f / 24f);
				jerry.GetPart("sword").localPosition -= new Vector3(0f, 1f / 12f);
			}
			else if (frames == 5)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_1");
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = false;
				jerry.GetPart("body").Find("headband").localPosition += new Vector3(0f, 0.125f);
				soul.ChangeSOULMode(1);
				Util.GameManager().PlayGlobalSFX("sounds/snd_spearappear_choppy");
			}
			else if (frames == 8)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_2");
				jerry.GetPart("body").Find("headband").localPosition -= new Vector3(0f, 1f / 24f);
			}
			else if (frames == 30)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_unarmed");
			}
			else if (frames == 45)
			{
				frames = 0;
				state = 1;
				swordSpinning = new GameObject("TheRealSwordThatsSpinning").transform;
				swordSpinning.position = new Vector3(4.25f, -1.36f);
				swordSpinning.parent = base.transform;
				sword.parent = swordSpinning;
				bbSize = new Vector3(215f, 150f);
				spinSword = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySpinSword"), new Vector3(4.25f, -1.36f), Quaternion.identity).GetComponent<SpriteRenderer>();
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames <= 90)
			{
				angleAcceleration += 2f / 3f;
				if (frames >= 60)
				{
					sword.GetComponent<SpriteRenderer>().color = new Color(0f, 0.23529412f, 1f, 1f - (float)(frames - 60) / 30f);
					spinSword.color = new Color(0f, 0.23529412f, 1f, (float)(frames - 60) / 30f);
					soulPush = Mathf.Lerp(0f, 1f / 24f, (float)(frames - 60) / 30f);
				}
			}
			if (frames <= 110)
			{
				if (frames == 100)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_spearrise");
				}
				spikeLeft.position = new Vector3(Mathf.Lerp(-2.536f, -2.12f, (float)(frames - 100) / 10f), -1.25f);
				spikeRight.position = new Vector3(Mathf.Lerp(2.536f, 2.12f, (float)(frames - 100) / 10f), -1.25f);
			}
			angle += angleAcceleration;
			swordSpinning.eulerAngles = new Vector3(0f, 0f, angle);
			float num3 = Mathf.Abs(Mathf.Sin(angle * ((float)Math.PI / 180f)));
			swordSpinning.localScale = new Vector3(1f + num3 / 5f, 1f - num3 / 2f, 1f);
			if (frames >= 120)
			{
				if (frames % 10 == 0)
				{
					float max = -3f;
					float num4 = UnityEngine.Random.Range(-0.3f, max);
					if (safetyRanges.Count > 0 && UnityEngine.Random.Range(0, 10) != 0)
					{
						for (int i = 0; i < safetyRanges.Count; i++)
						{
							if (num4 < safetyRanges[i][0] && num4 > safetyRanges[i][1])
							{
								num4 = safetyRanges[i][1] - UnityEngine.Random.Range(0.05f, 0.1f);
							}
						}
					}
					if (UnityEngine.Random.Range(0, 12) == 0)
					{
						num4 = soul.transform.position.y;
					}
					UnityEngine.Object.Instantiate(slashPrefab, new Vector3(4.25f, num4), Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-182f, -178f))).GetComponent<JerrySlashBlue>().SetSpeed(7f);
				}
				if (frames % 40 == 0)
				{
					platforms.Add(UnityEngine.Object.Instantiate(platformPrefab, new Vector3(7.25f, (platforms.Count > 0) ? UnityEngine.Random.Range(-0.85f, -2.48f) : (-2f)), Quaternion.identity, base.transform).transform);
					platforms[platforms.Count - 1].GetComponent<Platform>().ChangeSize(35);
					platforms[platforms.Count - 1].GetComponent<Platform>().DisableGainTPOnLand();
					safetyRangeRegistered.Add(0);
				}
				for (int j = 0; j < platforms.Count; j++)
				{
					platforms[j].transform.position -= new Vector3(0.0625f, 0f);
					if (platforms[j].transform.position.x <= 4.1f && safetyRangeRegistered[j] == 0)
					{
						safetyRangeRegistered[j]++;
						float y = platforms[j].transform.position.y;
						safetyRanges.Add(new float[2]
						{
							y + 0.9f,
							y - 0.32f
						});
					}
					else if (platforms[j].transform.position.x <= -0.1f && safetyRangeRegistered[j] == 1)
					{
						safetyRangeRegistered[j]++;
						safetyRanges.RemoveAt(0);
					}
				}
			}
			spinSoundFrames++;
			if (spinSoundFrames >= spinSoundRate)
			{
				sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_wing", (float)(40 - spinSoundRate) / 37f);
				if (spinSoundRate > 3)
				{
					spinSoundRate--;
				}
				spinSoundFrames = 0;
			}
			if (frames == 400)
			{
				state = 2;
				frames = 0;
			}
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			for (int k = 0; k < platforms.Count; k++)
			{
				platforms[k].transform.position -= new Vector3(Mathf.Lerp(3f, 12f, (float)(frames - 10) / 35f) / 48f, 0f);
			}
			frames++;
			if (frames <= 65)
			{
				if (frames == 1)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_spearappear_choppy");
				}
				if (frames <= 15)
				{
					sword.GetComponent<SpriteRenderer>().color = new Color(0f, 0.23529412f, 1f, (float)frames / 15f);
					spinSword.color = new Color(0f, 0.23529412f, 1f, 1f - (float)frames / 15f);
					soulPush = Mathf.Lerp(1f / 24f, 0f, (float)frames / 15f);
				}
				float num5 = (float)frames / 65f;
				float t = Mathf.Pow(num5, 10f);
				if (num5 < 1f)
				{
					num5 = Mathf.Sin(num5 * (float)Math.PI * 0.5f);
				}
				angle = Mathf.Lerp(0f, 1170f, num5);
				swordSpinning.eulerAngles = new Vector3(0f, 0f, angle);
				float num6 = Mathf.Lerp(Mathf.Abs(Mathf.Sin(angle * ((float)Math.PI / 180f))), 0f, (float)frames / 65f);
				swordSpinning.localScale = new Vector3(1f + num6 / 5f, 1f - num6 / 2f, 1f);
				swordSpinning.position = new Vector3(swordSpinning.position.x, Mathf.Lerp(swordSpinning.position.y, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y, t));
			}
			if (frames > 65)
			{
				if (frames == 66)
				{
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_spearthrow");
					sword.parent = base.transform;
					sword.GetComponent<BoxCollider2D>().enabled = true;
				}
				swordVelocity += 1f / 48f;
				sword.position += new Vector3(0f - swordVelocity, 0f);
			}
			if (frames >= 100)
			{
				if (frames == 100)
				{
					jerry.SetPose(0);
				}
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color32(0, 60, byte.MaxValue, (byte)(int)Mathf.Lerp(0f, 255f, (float)(frames - 100) / 20f));
			}
			if (frames == 120)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	private void LateUpdate()
	{
		if (!soul.OnPlatform())
		{
			soul.SetPullForce(new Vector3(0f - soulPush, 0f));
		}
		else
		{
			soul.SetPullForce(Vector3.zero);
		}
	}

	private void OnDestroy()
	{
		if ((bool)soul)
		{
			soul.SetPullForce(Vector3.zero);
		}
	}
}
