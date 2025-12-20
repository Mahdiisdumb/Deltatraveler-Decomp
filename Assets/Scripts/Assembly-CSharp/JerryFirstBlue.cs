using System;
using System.Collections.Generic;
using UnityEngine;

public class JerryFirstBlue : AttackBase
{
	private Jerry jerry;

	private GameObject prefab;

	private Transform course;

	private Transform candyPlatform;

	private float candyVelocity = 1f / 48f;

	private Transform[] blueSpawnSpots;

	private bool[] hasSpawned;

	private int at;

	private Transform sword;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(185f, 140f);
		prefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashBlue");
		course = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerryFirstBlue"), base.transform).transform;
		List<Transform> list = new List<Transform>();
		SpriteRenderer[] componentsInChildren = course.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.gameObject.name.StartsWith("WallBlock"))
			{
				spriteRenderer.color = UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
			}
			else if (spriteRenderer.gameObject.name.StartsWith("BlueSpawnSpot"))
			{
				list.Add(spriteRenderer.transform);
			}
		}
		blueSpawnSpots = list.ToArray();
		hasSpawned = new bool[blueSpawnSpots.Length];
		candyPlatform = course.Find("CandyPlatform");
		sword = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySword"), new Vector3(10f, 10f), Quaternion.identity, base.transform).transform;
		sword.GetComponent<SpriteRenderer>().color = new Color32(0, 60, byte.MaxValue, byte.MaxValue);
		jerry = UnityEngine.Object.FindObjectOfType<Jerry>();
		attackAllTargets = false;
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
				num2 *= num2;
				sword.position = Vector3.Lerp(new Vector3(-1.03f, 4.76f), new Vector3(7.75f, 1.21f), num2);
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
				bb.StartMovement(new Vector2(355f, 140f));
				UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
				Util.GameManager().PlayGlobalSFX("sounds/snd_spearappear_choppy");
			}
			else if (frames == 8)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_2");
				jerry.GetPart("body").Find("headband").localPosition -= new Vector3(0f, 1f / 24f);
			}
			else if (frames == 20)
			{
				sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_spearthrow");
			}
			else if (frames == 30)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_unarmed");
			}
			else if (frames == 45)
			{
				bb.StartMovement(new Vector2(355f, 180f));
				frames = 0;
				state = 1;
				course.transform.position = new Vector3(-2.7f, 0f);
				sword.position = new Vector3(10.28f, -2.75f);
				sword.eulerAngles = new Vector3(0f, 0f, 90f);
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			frames++;
			MonoBehaviour.print(frames);
			course.transform.position -= new Vector3(0.0625f, 0f);
			if (candyPlatform.position.x <= 8f)
			{
				candyPlatform.localPosition += new Vector3(candyVelocity, 0f);
			}
			if (frames >= 400)
			{
				candyVelocity -= 1f / 192f;
			}
			for (int i = at; i < blueSpawnSpots.Length; i++)
			{
				if (blueSpawnSpots[i].position.x <= 7f && !hasSpawned[i])
				{
					hasSpawned[i] = true;
					UnityEngine.Object.Instantiate(prefab, blueSpawnSpots[i].position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-178f, -182f))).GetComponent<JerrySlashBlue>().SetSpeed(8f);
					at++;
				}
			}
			if (frames == 120)
			{
				sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_spearthrow");
			}
			if (frames >= 120 && frames <= 180)
			{
				float num3 = (float)(frames - 120) / 60f;
				num3 = Mathf.Sin(num3 * (float)Math.PI * 0.5f);
				sword.position = new Vector3(Mathf.Lerp(10.28f, 1.82f, num3), -2.75f);
				sword.localScale = new Vector3(-1f, Mathf.Lerp(1f, 1.5f, num3), 1f);
			}
			if (frames >= 320)
			{
				float num4 = (float)(frames - 320) / 70f;
				num4 *= num4;
				sword.position = new Vector3(Mathf.Lerp(1.82f, -10.28f, num4), -2.75f);
				sword.localScale = new Vector3(-1f, Mathf.Lerp(1.5f, 1f, num4), 1f);
			}
			if (frames == 420)
			{
				jerry.SetPose(0);
			}
			if (frames >= 420)
			{
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color32(0, 60, byte.MaxValue, (byte)(int)Mathf.Lerp(0f, 255f, (float)(frames - 420) / 30f));
			}
			if (frames >= 450)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
