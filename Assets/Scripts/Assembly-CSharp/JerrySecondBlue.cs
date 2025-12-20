using System;
using System.Collections.Generic;
using UnityEngine;

public class JerrySecondBlue : AttackBase
{
	private Jerry jerry;

	private GameObject prefab;

	private Transform course;

	private Transform bsWall;

	private float bsWallVelocity = 0.04f;

	private Transform[] blueSpawnSpots;

	private bool[] hasSpawned;

	private int at;

	private float swordVelocity;

	private Vector3 direction = Vector3.left;

	private Transform sword;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(405f, 140f);
		prefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashBlue");
		course = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySecondBlue"), base.transform).transform;
		List<Transform> list = new List<Transform>();
		SpriteRenderer[] componentsInChildren = course.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.gameObject.name.StartsWith("WallBlock"))
			{
				bsWall = spriteRenderer.transform;
				spriteRenderer.color = UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
			}
			else if (spriteRenderer.gameObject.name.StartsWith("BlueSpawnSpot"))
			{
				spriteRenderer.enabled = false;
				list.Add(spriteRenderer.transform);
			}
		}
		blueSpawnSpots = list.ToArray();
		hasSpawned = new bool[blueSpawnSpots.Length];
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
				bb.StartMovement(new Vector2(405f, 180f));
				frames = 0;
				state = 1;
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
			course.transform.position -= new Vector3(1f / 12f, 0f);
			if (bsWall.position.x <= 6f)
			{
				bsWall.localPosition += new Vector3(0f - bsWallVelocity, 0f);
			}
			for (int i = 0; i < blueSpawnSpots.Length; i++)
			{
				if (blueSpawnSpots[i].position.x <= 7f && !hasSpawned[i])
				{
					hasSpawned[i] = true;
					UnityEngine.Object.Instantiate(prefab, blueSpawnSpots[i].position, Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-178f, -182f))).GetComponent<JerrySlashBlue>().SetSpeed(8f);
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
				sword.position = new Vector3(Mathf.Lerp(10.28f, 2.09f, num3), -2.75f);
				sword.localScale = new Vector3(-1f, Mathf.Lerp(1f, 1.75f, num3), 1f);
			}
			if (frames >= 420 && frames <= 450)
			{
				if (frames == 420)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_spearappear_choppy");
				}
				float num4 = (float)(frames - 420) / 30f;
				num4 = num4 * num4 * num4 * (num4 * (6f * num4 - 15f) + 10f);
				float t = Mathf.Sin((float)((frames - 420) * 6) * ((float)Math.PI / 180f));
				sword.position = Vector3.Lerp(new Vector3(2.09f, -2.75f), new Vector3(6.98f, Mathf.Lerp(-1.44f, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y, (float)(frames - 445) / 5f)), num4);
				sword.localScale = new Vector3(-1f, Mathf.Lerp(1.75f, 1f, num4), 1f);
				sword.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(90f, 115f, t));
			}
			if (frames >= 450)
			{
				if (frames == 450)
				{
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_spearthrow");
				}
				if (UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x < sword.position.x)
				{
					direction = (UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - sword.position).normalized;
					direction.x = 0f - Mathf.Abs(direction.x);
					if (Mathf.Abs(direction.x) < 2f / 3f)
					{
						direction.x = -2f / 3f;
					}
					direction.y /= 3f;
				}
				swordVelocity += 1f / 48f;
				sword.position += direction * swordVelocity;
			}
			if (frames == 520)
			{
				jerry.SetPose(0);
			}
			if (frames >= 520)
			{
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color32(0, 60, byte.MaxValue, (byte)(int)Mathf.Lerp(0f, 255f, (float)(frames - 520) / 30f));
			}
			if (frames >= 550)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
