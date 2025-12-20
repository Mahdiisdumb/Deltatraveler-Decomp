using System;
using UnityEngine;

public class GlydeAttack : AttackBase
{
	private Jerry jerry;

	private Glyde glyde;

	private Transform fakeJerry;

	private bool fakeJerryAttack;

	private int fakeJerryFrames;

	private int face;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(165f, 140f);
		maxFrames = 90000;
		glyde = UnityEngine.Object.FindObjectOfType<Glyde>();
		jerry = UnityEngine.Object.FindObjectOfType<Jerry>();
	}

	protected override void Update()
	{
		if (state == 0)
		{
			base.Update();
			if (isStarted)
			{
				if (frames % 13 == 1)
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/GlydeBackflash"), glyde.GetPart("antenna"));
				}
				if (frames == 120)
				{
					fakeJerryAttack = true;
					Util.GameManager().PlayGlobalSFX("sounds/snd_bigcut");
				}
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames >= 90 && frames <= 150)
			{
				float num = (float)(frames - 90) / 15f;
				jerry.transform.position = new Vector3(0f, Mathf.Lerp(8f, 0f, num * num));
				if (frames == 105)
				{
					jerry.PlaySFX("sounds/snd_noise");
				}
				if (frames >= 105 && frames <= 115)
				{
					float t = (Mathf.Cos((float)((frames - 105) * 18) * ((float)Math.PI / 180f)) + 1f) / 2f;
					jerry.GetEnemyObject().transform.localScale = new Vector3(Mathf.Lerp(1f, 1.1f, t), Mathf.Lerp(1f, 0.9f, t), 1f);
				}
				if (frames == 150)
				{
					jerry.Chat(new string[2] { "Finally...", "That egotistical \nasswipe is finally \ndead." }, "RightWide", "snd_text", new Vector2(191f, 60f), true, 0);
				}
			}
			else if (frames > 150 && !UnityEngine.Object.FindObjectOfType<TextBubble>())
			{
				frames = 0;
				state = 2;
			}
		}
		else if (state == 2)
		{
			frames++;
			if (frames == 15)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_land_1");
			}
			else if (frames == 25)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_land_2");
				jerry.GetPart("body").Find("headband").localPosition = new Vector3(1.0416666f, 1.25f);
				jerry.GetPart("sword").localPosition = new Vector3(1.1366667f, 2.27f);
			}
			else if (frames == 60)
			{
				state = 3;
				frames = 0;
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_introtalk_0");
				jerry.GetPart("body").Find("headband").localPosition = new Vector3(1.2083334f, 1.25f);
				jerry.GetPart("body").Find("headband").GetComponent<SpriteRenderer>()
					.sortingOrder = 24;
				jerry.GetPart("sword").localPosition = new Vector3(-2.66f, 0.57f);
				jerry.GetPart("sword").eulerAngles = new Vector3(0f, 0f, 63f);
				jerry.Chat(new string[6] { "And what's this?", "A human?", "Everything's falling \ninto place...", "With a human SOUL \ncaptured...", "Everyone will finally \ngive me the respect \nI deserve.", "It's time to die,^05 \nhuman." }, "RightWide", "snd_text", new Vector2(191f, 60f), true, 0);
			}
		}
		else if (state == 3)
		{
			if ((bool)UnityEngine.Object.FindObjectOfType<TextBubble>())
			{
				if (UnityEngine.Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 3 && face == 0)
				{
					face++;
					jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_introtalk_1");
				}
				else if (UnityEngine.Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 5 && face == 1)
				{
					face++;
					jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_introtalk_2");
				}
				else if (UnityEngine.Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 6 && face == 2)
				{
					face++;
					jerry.SetPose(0);
					jerry.PlaySFX("sounds/snd_weaponpull");
				}
			}
			else
			{
				UnityEngine.Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_jerry", 1f, true);
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		if (fakeJerryAttack)
		{
			fakeJerryFrames++;
			fakeJerry.position = new Vector3(Mathf.Lerp(10f, -10f, (float)fakeJerryFrames / 10f), 1.92f);
			if (fakeJerryFrames == 5)
			{
				glyde.ForceKill();
				UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
				state = 1;
				frames = 0;
			}
			if (fakeJerryFrames == 10)
			{
				fakeJerryAttack = false;
				UnityEngine.Object.Destroy(fakeJerry.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		glyde.TurnDark();
		fakeJerry = new GameObject("FakeJerry", typeof(SpriteRenderer)).transform;
		fakeJerry.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_swoopkill");
		fakeJerry.GetComponent<SpriteRenderer>().sortingOrder = 100;
		fakeJerry.position = new Vector3(10f, 1.92f);
		jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_land_0");
		jerry.GetPart("body").Find("headband").localPosition = new Vector3(1f, 1.25f);
		jerry.GetPart("body").Find("headband").GetComponent<SpriteRenderer>()
			.sortingOrder = 26;
		jerry.GetPart("sword").localPosition = new Vector3(1.22f, 2.27f);
		jerry.GetPart("sword").eulerAngles = new Vector3(0f, 0f, -127f);
	}
}
