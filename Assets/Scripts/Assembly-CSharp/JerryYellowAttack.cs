using System;
using System.Collections.Generic;
using UnityEngine;

public class JerryYellowAttack : AttackBase
{
	private GameObject prefab;

	private Jerry jerry;

	private Sprite[] spinFrames = new Sprite[3];

	protected int numOfBullets = 25;

	protected bool[] bigBullets;

	protected bool[] redBullets;

	private int numOfBulletsSpawned;

	protected int spawnRate = 4;

	protected int fallRate = 6;

	protected float bulletSpeed = 1.3f;

	private List<JerrySlashYellow> bullets = new List<JerrySlashYellow>();

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(185f, 140f);
		jerry = UnityEngine.Object.FindObjectOfType<Jerry>();
		prefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashYellow");
		for (int i = 0; i < 3; i++)
		{
			spinFrames[i] = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_yellowspin_" + i);
		}
		bigBullets = new bool[numOfBullets];
		redBullets = new bool[numOfBullets];
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
			if (frames == 1)
			{
				jerry.SetPose(1);
				jerry.PlaySFX("sounds/snd_boost");
				UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(5);
			}
			if (frames == 18)
			{
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = false;
				jerry.GetPart("body").Find("headband").GetComponent<SpriteRenderer>()
					.enabled = false;
				Util.GameManager().PlayGlobalSFX("sounds/snd_criticalswing");
				jerry.GetPart("body").GetComponent<SpriteRenderer>().color = Color.white;
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_yellowspin_start_0");
			}
			if (frames == 20)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_yellowspin_start_1");
			}
			if (frames <= 20)
			{
				float b = Mathf.Sin((float)frames * 10.588235f * ((float)Math.PI / 180f));
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, b);
				return;
			}
			jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = spinFrames[frames % 3];
			if (frames % 6 == 1)
			{
				jerry.PlaySFX("sounds/snd_wing", 1f, UnityEngine.Random.Range(0.8f, 1f));
			}
			if (frames % spawnRate == 1)
			{
				float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
				Vector3 position = new Vector3(Mathf.Sin(f) * 2.05f, 1.37f + Mathf.Cos(f) * 0.73f);
				JerrySlashYellow component = UnityEngine.Object.Instantiate(prefab, position, Quaternion.identity, base.transform).GetComponent<JerrySlashYellow>();
				component.Activate(position.x < 0f, UnityEngine.Random.Range(-10f, 60f), UnityEngine.Random.Range(25, 40), bulletSpeed, bigBullets[numOfBulletsSpawned], redBullets[numOfBulletsSpawned]);
				bullets.Add(component);
				numOfBulletsSpawned++;
				if (numOfBulletsSpawned >= numOfBullets)
				{
					state = 1;
					frames = 0;
					jerry.SetPose(0);
				}
			}
			return;
		}
		frames++;
		if (frames >= 15)
		{
			if (bullets.Count > 0 && frames % fallRate == 0)
			{
				bullets[0].StartFalling();
				bullets.RemoveAt(0);
			}
			else if (bullets.Count == 0 && !UnityEngine.Object.FindObjectOfType<JerrySlashYellow>())
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
