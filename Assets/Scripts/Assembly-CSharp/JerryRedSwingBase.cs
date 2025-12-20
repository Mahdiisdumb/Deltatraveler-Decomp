using System;
using System.Collections.Generic;
using UnityEngine;

public class JerryRedSwingBase : AttackBase
{
	protected KeyValuePair<int, int>[] actions = new KeyValuePair<int, int>[3]
	{
		new KeyValuePair<int, int>(0, 35),
		new KeyValuePair<int, int>(1, 60),
		new KeyValuePair<int, int>(3, 100)
	};

	private GameObject prefab;

	private Jerry jerry;

	private Sprite[] swingSprites = new Sprite[5];

	private Sprite[] spinSprites;

	protected float bulletSpeed = 1.25f;

	protected int bulletNumberPerSwing = 5;

	private int bulletsSpawnedThisTime;

	private int onAction;

	private Transform sword;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(185f, 140f);
		state = -1;
		jerry = UnityEngine.Object.FindObjectOfType<Jerry>();
		UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = Color.red;
		for (int i = 0; i < 5; i++)
		{
			swingSprites[i] = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_attack_left_" + i);
		}
		spinSprites = Resources.LoadAll<Sprite>("battle/enemies/Jerry/spr_b_jerry_spin_attack");
		prefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashRed");
		attackAllTargets = false;
	}

	private void OnDestroy()
	{
		if ((bool)jerry)
		{
			jerry.SetPose(0);
		}
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (state == -1)
		{
			SetNextState();
		}
		else if (state == 0 || state == 1)
		{
			bool flag = state == 0;
			frames++;
			if (frames == 1)
			{
				jerry.SetPose(0);
				jerry.GetPart("body").GetComponent<SpriteRenderer>().flipX = !flag;
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = swingSprites[0];
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().flipX = !flag;
				jerry.GetPart("sword").localPosition = new Vector3(flag ? 0.91f : (-0.91f), 2.35f);
				jerry.GetPart("sword").eulerAngles = new Vector3(0f, 0f, flag ? (-70) : 70);
			}
			else if (frames == 4)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = swingSprites[1];
				if (!flag)
				{
					jerry.GetPart("body").Find("headband").localPosition = new Vector3(0.594f, 1.25f);
				}
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = false;
				jerry.PlaySFX("sounds/snd_swipe");
			}
			else if (frames == 6)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = swingSprites[2];
				jerry.GetPart("body").Find("headband").localPosition = new Vector3(flag ? 1.06f : 0.38f, 1.33f);
			}
			else if (frames == 8)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = swingSprites[3];
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = true;
				jerry.GetPart("sword").localPosition = new Vector3(flag ? (-2.15f) : 2.15f, 0.32f);
				jerry.GetPart("sword").eulerAngles = new Vector3(0f, 0f, flag ? 96 : (-96));
			}
			else if (frames == 10)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = swingSprites[4];
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = true;
				jerry.GetPart("sword").localPosition += new Vector3(0f, 1f / 24f);
			}
			if (frames >= 4 && frames <= 8)
			{
				int num = bulletNumberPerSwing / 5;
				int num2 = bulletNumberPerSwing % 5;
				int num3 = num;
				if (frames - 4 < num2)
				{
					num3++;
				}
				for (int i = 0; i < num3; i++)
				{
					float num4 = (float)bulletsSpawnedThisTime / ((float)bulletNumberPerSwing - 1f);
					float startAngle = Mathf.Lerp(60f, 0f, num4) + UnityEngine.Random.Range(-3f, 3f);
					float num5 = 1f - num4;
					UnityEngine.Object.Instantiate(prefab, Vector3.Lerp(new Vector3(flag ? (-1.68f) : 1.68f, 4.78f), new Vector3(flag ? (-4.23f) : 4.23f, 1.21f), 1f - num5 * num5), Quaternion.identity).GetComponent<JerrySlashRed>().Activate(flag, startAngle, UnityEngine.Random.Range(2.5f, 3.5f) + num4, (int)Mathf.Lerp(60f, 30f, num4), bulletSpeed);
					bulletsSpawnedThisTime++;
				}
			}
			if (frames >= maxFrames && frames >= 10)
			{
				SetNextState();
			}
		}
		else if (state == 2)
		{
			frames++;
			if (frames <= 10)
			{
				float num6 = Mathf.Sin((float)(frames * 18) * ((float)Math.PI / 180f));
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color(1f, num6, num6);
			}
			if (frames == 1)
			{
				jerry.SetPose(1);
				jerry.PlaySFX("sounds/snd_bell_bounce_short");
			}
			else if (frames == 11)
			{
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = false;
				jerry.GetPart("body").Find("headband").GetComponent<SpriteRenderer>()
					.enabled = false;
				Util.GameManager().PlayGlobalSFX("sounds/snd_criticalswing");
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[1];
			}
			else if (frames == 14)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[2];
			}
			else if (frames == 16)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[3];
			}
			else if (frames == 18)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[4];
			}
			else if (frames == 20)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[5];
			}
			else if (frames == 22)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[6];
			}
			else if (frames == 24)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinSprites[7];
				jerry.GetPart("body").Find("headband").GetComponent<SpriteRenderer>()
					.enabled = true;
				jerry.GetPart("body").Find("headband").localPosition = new Vector3(1.289f, 1.237f);
			}
			if (frames >= 11 && frames <= 15)
			{
				int num7 = bulletNumberPerSwing / 5;
				int num8 = bulletNumberPerSwing % 5;
				int num9 = num7;
				if (frames - 11 < num8)
				{
					num9++;
				}
				for (int j = 0; j < num9; j++)
				{
					float num10 = (float)bulletsSpawnedThisTime / ((float)bulletNumberPerSwing - 1f);
					float startAngle2 = Mathf.Lerp(60f, 0f, num10) + UnityEngine.Random.Range(-3f, 3f);
					float num11 = 1f - num10;
					UnityEngine.Object.Instantiate(prefab, Vector3.Lerp(new Vector3(-1.68f, 4.78f), new Vector3(-4.23f, 1.21f), 1f - num11 * num11), Quaternion.identity).GetComponent<JerrySlashRed>().Activate(true, startAngle2, UnityEngine.Random.Range(2.5f, 3.5f) + num10, (int)Mathf.Lerp(60f, 30f, num10), bulletSpeed);
					bulletsSpawnedThisTime++;
				}
				if (frames == 15)
				{
					bulletsSpawnedThisTime = 0;
				}
			}
			else if (frames >= 16 && frames <= 20)
			{
				int num12 = bulletNumberPerSwing / 5;
				int num13 = bulletNumberPerSwing % 5;
				int num14 = num12;
				if (frames - 16 < num13)
				{
					num14++;
				}
				for (int k = 0; k < num14; k++)
				{
					float num15 = (float)bulletsSpawnedThisTime / ((float)bulletNumberPerSwing - 1f);
					num15 = 1f - num15;
					float startAngle3 = Mathf.Lerp(60f, 0f, num15) + UnityEngine.Random.Range(-3f, 3f);
					float num16 = 1f - num15;
					UnityEngine.Object.Instantiate(prefab, Vector3.Lerp(new Vector3(1.68f, 4.78f), new Vector3(4.23f, 1.21f), 1f - num16 * num16), Quaternion.identity).GetComponent<JerrySlashRed>().Activate(false, startAngle3, UnityEngine.Random.Range(2.5f, 3.5f) + num15, (int)Mathf.Lerp(60f, 30f, num15), bulletSpeed);
					bulletsSpawnedThisTime++;
				}
			}
			if (frames >= maxFrames && frames >= 22)
			{
				SetNextState();
			}
		}
		else
		{
			if (state != 3)
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				jerry.PlaySFX("sounds/snd_jump");
				jerry.SetPose(0);
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_sword_throw_0_hand");
				jerry.GetPart("body").Find("headband").localPosition = new Vector3(1.2083334f, 1.8333334f);
				jerry.GetPart("body").Find("theunders").GetComponent<SpriteRenderer>()
					.enabled = true;
				jerry.GetPart("sword").localPosition = new Vector3(1.612f, 2.814f);
			}
			if (frames <= 20)
			{
				float num17 = (float)frames / 20f;
				num17 = Mathf.Sin(num17 * (float)Math.PI * 0.5f);
				jerry.GetPart("body").parent.localPosition = new Vector3(0f, Mathf.Lerp(-0.5f, 2.25f, num17));
				jerry.GetPart("sword").up = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - jerry.GetPart("sword").position;
				if (frames == 20)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_bigcut");
					jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_sword_throw_1");
					jerry.GetPart("body").Find("theunders").GetComponent<SpriteRenderer>()
						.enabled = false;
					jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = false;
					sword = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySword"), jerry.GetPart("sword").position, jerry.GetPart("sword").rotation, base.transform).transform;
					sword.GetComponent<SpriteRenderer>().color = Color.red;
				}
			}
			if ((bool)sword)
			{
				sword.position += sword.up * (7f / 24f);
			}
			if (frames == 24)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_sword_throw_2");
			}
			if (frames >= 35 && frames <= 50)
			{
				float num18 = (float)(frames - 35) / 15f;
				num18 *= num18;
				jerry.GetPart("body").parent.localPosition = new Vector3(0f, Mathf.Lerp(2.25f, -0.5f, num18));
				if (frames == 50)
				{
					jerry.SetPose(0);
					jerry.GetPart("body").parent.localPosition = Vector3.zero;
					jerry.PlaySFX("sounds/snd_noise");
					jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 0f);
				}
			}
			if (frames >= 50 && frames <= 60)
			{
				float t = (Mathf.Cos((float)((frames - 50) * 18) * ((float)Math.PI / 180f)) + 1f) / 2f;
				jerry.GetEnemyObject().transform.localScale = new Vector3(Mathf.Lerp(1f, 1.1f, t), Mathf.Lerp(1f, 0.9f, t), 1f);
			}
			if (frames >= 60)
			{
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (float)(frames - 60) / 15f);
			}
			if (frames >= 75)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	protected void SetNextState()
	{
		if (onAction >= actions.Length)
		{
			UnityEngine.Object.Destroy(base.gameObject);
			return;
		}
		state = actions[onAction].Key;
		maxFrames = actions[onAction].Value;
		frames = 0;
		bulletsSpawnedThisTime = 0;
		onAction++;
	}
}
