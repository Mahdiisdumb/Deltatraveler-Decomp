using System;
using UnityEngine;

public class JerryFinisherAttack : AttackBase
{
	private bool reachedVerse;

	private MusicPlayer musicPlayer;

	private Jerry jerry;

	private int holdFrames;

	private int maxHoldFrames = 12;

	private Transform sword;

	private Transform swordMask;

	private GameObject finaleSlashPrefab;

	private int firstSlashFrames;

	private int firstSlashRate = 8;

	private GameObject redSlashPrefab;

	private Transform swordSpinning;

	private SpriteRenderer spinSword;

	private bool[] bulletHasSpawned = new bool[11];

	private float angleAcceleration;

	private float angle;

	private int spinSoundFrames;

	private int spinSoundRate = 11;

	private int stumbleFrames;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		UnityEngine.Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		UnityEngine.Object.FindObjectOfType<PartyPanels>().RaiseHeads(true, false, false);
		UnityEngine.Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		musicPlayer = UnityEngine.Object.FindObjectOfType<BattleManager>().GetComponent<MusicPlayer>();
		jerry = UnityEngine.Object.FindObjectOfType<Jerry>();
		sword = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySword"), new Vector3(10f, 10f), Quaternion.identity, base.transform).transform;
		sword.GetComponent<SpriteRenderer>().color = Color.red;
		finaleSlashPrefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerryFinaleSlash");
		redSlashPrefab = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashRed");
		swordMask = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySwordMask")).transform;
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (!reachedVerse && !musicPlayer.GetSource().clip.name.EndsWith("intro") && musicPlayer.GetSource().time >= 14f)
		{
			reachedVerse = true;
		}
		if (state == 2 || state == 3)
		{
			if (UTInput.GetButtonDown("Z"))
			{
				maxHoldFrames = 11;
				holdFrames = maxHoldFrames;
			}
			if (UTInput.GetButton("Z"))
			{
				holdFrames++;
				if (!reachedVerse)
				{
					musicPlayer.ChangeMusic("music/mus_armstrong", false, true);
					musicPlayer.GetSource().time = 14f;
					reachedVerse = true;
				}
				if (holdFrames >= maxHoldFrames)
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SmallFist")).GetComponent<SmallFistAttack>().AssignValues(jerry, 0, 1f, 1);
					jerry.ReduceHPFinale();
					holdFrames = 0;
					if (maxHoldFrames > 3)
					{
						maxHoldFrames--;
					}
					if (jerry.GetHP() == 0)
					{
						UnityEngine.Object.FindObjectOfType<PetalGenerator>().SetRate(30);
						jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_dmg");
						musicPlayer.Stop();
						frames = 0;
						state = 4;
						Util.GameManager().PlayGlobalSFX("sounds/snd_damage");
						sword.GetComponent<SpriteRenderer>().color = Color.red;
						if ((bool)spinSword)
						{
							spinSword.color = new Color(1f, 0f, 0f, 0f);
						}
					}
				}
			}
		}
		if (state == 0)
		{
			frames++;
			if (frames >= 5 && frames <= 30)
			{
				float num = (float)(frames - 5) / 25f;
				num = Mathf.Sin(num * (float)Math.PI * 0.5f);
				sword.position = Vector3.Lerp(new Vector3(-1.09f, 2.84f), new Vector3(-1.03f, 9f), num);
				sword.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-76f, -170f, num));
			}
			else if (frames > 45)
			{
				float num2 = (float)(frames - 45) / 25f;
				num2 *= num2;
				sword.position = new Vector3(4f, Mathf.Lerp(9f, 2.5f, num2));
				sword.eulerAngles = new Vector3(0f, 0f, 180f);
				sword.GetComponent<SpriteRenderer>().flipX = true;
				sword.GetComponent<SpriteRenderer>().sortingOrder = 20;
				sword.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleOutsideMask;
			}
			if (frames == 1)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_smug_0");
				jerry.GetPart("body").Find("headband").localPosition -= new Vector3(0f, 1f / 24f);
				jerry.GetPart("sword").localPosition -= new Vector3(0f, 1f / 12f);
				jerry.SetFace(null);
			}
			else if (frames == 5)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_smug_1");
				jerry.GetPart("sword").GetComponent<SpriteRenderer>().enabled = false;
				jerry.GetPart("body").Find("headband").localPosition += new Vector3(0f, 0.125f);
				Util.GameManager().PlayGlobalSFX("sounds/snd_heavyswing");
			}
			else if (frames == 8)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_toss_sword_up_smug_2");
				jerry.GetPart("body").Find("headband").localPosition -= new Vector3(0f, 1f / 24f);
			}
			else if (frames == 30)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_unarmed");
				jerry.SetFace("eyeshut_smug");
				jerry.GetPart("body").Find("face").localPosition += new Vector3(0f, 1f / 24f);
			}
			if (frames == 70)
			{
				UnityEngine.Object.FindObjectOfType<BattleCamera>().BlastShake();
				Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
			}
			if (frames == 85)
			{
				jerry.Chat(new string[4] { "You won't be able to \ntake me down!", "Come as close as \nyou'd like,^05 human...", "But you'll die \ntrying.", "Now enough talk.^05\nHit me with your \nbest shot!" }, "RightWide", "snd_text", Vector2.zero, true, 1);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)UnityEngine.Object.FindObjectOfType<TextBubble>())
			{
				if (UnityEngine.Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2)
				{
					jerry.SetFace("smirk_full");
				}
				else if (UnityEngine.Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 3)
				{
					jerry.SetFace(null);
				}
			}
			else
			{
				UnityEngine.Object.FindObjectOfType<BattleManager>().StartText("* Mash or hold ^Z to defeat\n  Jerry!", new Vector2(-4f, -134f), "snd_txtbtl");
				UnityEngine.Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
				state = 2;
			}
		}
		else if (state == 2 && jerry.GetHP() < 250)
		{
			state = 3;
			UnityEngine.Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = true;
			UnityEngine.Object.FindObjectOfType<SOUL>().SetControllable(true);
			UnityEngine.Object.FindObjectOfType<PartyPanels>().SetTargets(true, false, false);
			bb.StartMovement(new Vector2(185f, 140f));
			UnityEngine.Object.FindObjectOfType<BattleManager>().ResetText();
		}
		else if (state == 3)
		{
			frames++;
			if (frames >= 10 && frames < 30)
			{
				int num3 = (frames - 10) / 4;
				int num4 = ((frames / 2 % 2 != 0) ? 1 : (-1));
				sword.transform.position = new Vector3(4f + (float)(num3 * num4) / 48f, 2.5f);
			}
			else if (frames >= 30 && frames < 60)
			{
				if (frames == 30)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_spearappear_choppy");
				}
				float t = (float)(frames - 30) / 20f;
				float t2 = (float)(frames - 40) / 20f;
				sword.transform.position = new Vector3(4f, Mathf.Lerp(2.5f, 5f, t));
				sword.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(180f, 200f, t2));
			}
			else if (frames >= 60 && frames < 120)
			{
				float num5 = (float)(frames - 60) / 45f;
				num5 *= num5;
				sword.transform.position = new Vector3(Mathf.Lerp(4f, -8f, num5), 5f);
				sword.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(200f, 160f, num5));
				firstSlashFrames++;
				if (firstSlashFrames > firstSlashRate && sword.transform.position.x > -6f)
				{
					firstSlashFrames = 0;
					if (firstSlashRate > 4)
					{
						firstSlashRate--;
					}
					JerryFinaleSlash component = UnityEngine.Object.Instantiate(finaleSlashPrefab, new Vector3(sword.transform.position.x, UnityEngine.Random.Range(4.5f, 3.5f)), Quaternion.identity).GetComponent<JerryFinaleSlash>();
					component.transform.right = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - component.transform.position;
					component.SetSpeed(12f);
				}
			}
			else if (frames >= 120 && frames < 170)
			{
				if (frames == 120)
				{
					UnityEngine.Object.Destroy(swordMask.gameObject);
					sword.transform.eulerAngles = Vector3.zero;
					sword.GetComponent<SpriteRenderer>().sortingOrder = 210;
					sword.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_bigcut");
				}
				float t3 = (float)(frames - 120) / 40f;
				sword.transform.position = new Vector3(Mathf.Lerp(-7f, 7f, t3), -5f);
				if (frames % 4 == 0 && Mathf.Abs(sword.transform.position.x) <= 6f)
				{
					firstSlashFrames = 0;
					if (firstSlashRate > 4)
					{
						firstSlashRate--;
					}
					JerryFinaleSlash component2 = UnityEngine.Object.Instantiate(finaleSlashPrefab, sword.transform.position + new Vector3(0f, 0.5f), Quaternion.identity).GetComponent<JerryFinaleSlash>();
					component2.transform.eulerAngles = new Vector3(0f, 0f, 90f);
					component2.SetSpeed(10f);
				}
			}
			else if (frames >= 170 && frames < 220)
			{
				if (frames == 170)
				{
					sword.transform.eulerAngles = new Vector3(0f, 0f, 180f);
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_bigcut");
				}
				float t4 = (float)(frames - 170) / 40f;
				sword.transform.position = new Vector3(Mathf.Lerp(7f, -7f, t4), 1.84f);
				if (frames % 4 == 0 && Mathf.Abs(sword.transform.position.x) <= 6f)
				{
					firstSlashFrames = 0;
					if (firstSlashRate > 4)
					{
						firstSlashRate--;
					}
					JerryFinaleSlash component3 = UnityEngine.Object.Instantiate(finaleSlashPrefab, sword.transform.position - new Vector3(0f, 0.5f), Quaternion.identity).GetComponent<JerryFinaleSlash>();
					component3.transform.eulerAngles = new Vector3(0f, 0f, -90f);
					component3.SetSpeed(10f);
				}
			}
			else if (frames >= 220 && frames < 270)
			{
				if (frames == 220)
				{
					sword.transform.eulerAngles = Vector3.zero;
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_bigcut");
				}
				float t5 = (float)(frames - 220) / 40f;
				sword.transform.position = new Vector3(Mathf.Lerp(-7f, 7f, t5), -5f);
				if (frames % 4 == 0 && Mathf.Abs(sword.transform.position.x) <= 6f)
				{
					firstSlashFrames = 0;
					if (firstSlashRate > 4)
					{
						firstSlashRate--;
					}
					JerryFinaleSlash component4 = UnityEngine.Object.Instantiate(finaleSlashPrefab, sword.transform.position + new Vector3(0f, 0.5f), Quaternion.identity).GetComponent<JerryFinaleSlash>();
					component4.transform.eulerAngles = new Vector3(0f, 0f, 90f);
					component4.SetSpeed(12f);
				}
			}
			else if (frames >= 270 && frames < 320)
			{
				if (frames == 270)
				{
					sword.transform.eulerAngles = new Vector3(0f, 0f, 180f);
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_bigcut");
				}
				float t6 = (float)(frames - 270) / 40f;
				sword.transform.position = new Vector3(Mathf.Lerp(7f, -7f, t6), 1.84f);
				if (frames % 4 == 0 && Mathf.Abs(sword.transform.position.x) <= 6f)
				{
					firstSlashFrames = 0;
					if (firstSlashRate > 4)
					{
						firstSlashRate--;
					}
					JerryFinaleSlash component5 = UnityEngine.Object.Instantiate(finaleSlashPrefab, sword.transform.position - new Vector3(0f, 0.5f), Quaternion.identity).GetComponent<JerryFinaleSlash>();
					component5.transform.eulerAngles = new Vector3(0f, 0f, -90f);
					component5.SetSpeed(12f);
				}
			}
			else if (frames >= 320 && frames < 450)
			{
				if (frames == 320)
				{
					sword.transform.eulerAngles = Vector3.zero;
					sword.GetComponent<SpriteRenderer>().flipX = false;
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_spearappear");
				}
				if (frames <= 350)
				{
					float num6 = (float)(frames - 320) / 30f;
					num6 = Mathf.Sin(num6 * (float)Math.PI * 0.5f);
					float num7 = (float)(frames - 335) / 15f;
					num7 = num7 * num7 * num7 * (num7 * (6f * num7 - 15f) + 10f);
					sword.transform.position = new Vector3(Mathf.Lerp(7f, 2.75f, num6), -1.66f);
					sword.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, -15f, num7));
				}
				else if (frames <= 370)
				{
					if (frames == 351)
					{
						Util.GameManager().PlayGlobalSFX("sounds/snd_criticalswing");
						sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_swipe");
					}
					float num8 = (float)(frames - 350) / 25f;
					num8 = num8 * num8 * num8 * (num8 * (6f * num8 - 15f) + 10f);
					sword.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-15f, 180f, num8));
				}
				else if (frames <= 385)
				{
					float num9 = (float)(frames - 370) / 15f;
					num9 = Mathf.Sin(num9 * (float)Math.PI * 0.5f);
					float num10 = (float)(frames - 370) / 26f;
					num10 = num10 * num10 * num10 * (num10 * (6f * num10 - 15f) + 10f);
					sword.transform.position = new Vector3(Mathf.Lerp(2.75f, 5.81f, num9), -1.66f);
					sword.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(180f, 195f, num10));
				}
				else if (frames <= 400)
				{
					if (frames == 386)
					{
						Util.GameManager().PlayGlobalSFX("sounds/snd_criticalswing");
						sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_swipe");
					}
					float num11 = (float)(frames - 385) / 15f;
					num11 = num11 * num11 * num11 * (num11 * (6f * num11 - 15f) + 10f);
					float num12 = Mathf.Lerp(195f, 0f, num11);
					for (int i = 0; i < 9; i++)
					{
						int num13 = 150 - i * 15;
						if (!bulletHasSpawned[i] && num12 <= (float)num13)
						{
							Vector3 vector = new Vector3(0f - Mathf.Sin((float)num13 * ((float)Math.PI / 180f)), 0f - Mathf.Cos((float)num13 * ((float)Math.PI / 180f))) * 0.5f;
							JerryFinaleSlash component6 = UnityEngine.Object.Instantiate(finaleSlashPrefab, sword.transform.position + vector, Quaternion.identity).GetComponent<JerryFinaleSlash>();
							component6.transform.eulerAngles = new Vector3(0f, 0f, num13 + 90);
							component6.SetSpeed(12f);
							bulletHasSpawned[i] = true;
						}
					}
					sword.transform.eulerAngles = new Vector3(0f, 0f, num12);
				}
				else if (frames <= 420)
				{
					float t7 = (float)(frames - 400) / 20f;
					sword.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, 90f, t7));
					sword.transform.position = new Vector3(5.81f, Mathf.Lerp(sword.position.y, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y, t7));
				}
				else
				{
					float num14 = (float)(frames - 420) / 30f;
					num14 *= num14;
					sword.position = new Vector3(Mathf.Lerp(5.81f, -9f, num14), sword.position.y);
				}
			}
			else
			{
				if (frames < 450)
				{
					return;
				}
				if (frames == 450)
				{
					sword.GetComponent<SpriteRenderer>().sortingOrder = 20;
					sword.eulerAngles = Vector3.zero;
				}
				if (frames <= 470)
				{
					float num15 = (float)(frames - 450) / 20f;
					num15 = Mathf.Sin(num15 * (float)Math.PI * 0.5f);
					sword.position = new Vector3(Mathf.Lerp(-7f, 0f, num15), 1.41f);
					if (frames == 470)
					{
						swordSpinning = new GameObject("TheRealSwordThatsSpinning").transform;
						swordSpinning.position = new Vector3(0f, 2.65f);
						swordSpinning.parent = base.transform;
						sword.parent = swordSpinning;
						spinSword = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySpinSword"), new Vector3(0f, 2.65f), Quaternion.identity).GetComponent<SpriteRenderer>();
						spinSword.transform.localScale = new Vector3(2f, 1f, 1f);
						spinSword.color = new Color(1f, 0f, 0f, 0f);
						spinSword.sortingOrder = 20;
					}
				}
				else if (frames <= 515)
				{
					angleAcceleration += 1.3333334f;
					if (frames >= 500)
					{
						sword.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f - (float)(frames - 500) / 15f);
						spinSword.color = new Color(1f, 0f, 0f, (float)(frames - 500) / 15f);
					}
				}
				else if (frames % 9 == 1)
				{
					bool flag = UnityEngine.Random.Range(0, 2) == 0;
					float num16 = UnityEngine.Random.Range(0f, 1f);
					float num17 = 1f - num16;
					float startAngle = Mathf.Lerp(60f, 0f, num16) + UnityEngine.Random.Range(-3f, 3f);
					UnityEngine.Object.Instantiate(redSlashPrefab, Vector3.Lerp(new Vector3(flag ? (-1.68f) : 1.68f, 4.42f), new Vector3(flag ? (-2.4f) : 2.4f, 1.21f), 1f - num17 * num17), Quaternion.identity).GetComponent<JerrySlashRed>().Activate(flag, startAngle, UnityEngine.Random.Range(2.5f, 3.5f) + num16, (int)Mathf.Lerp(60f, 30f, num16), 1.5f);
				}
				if (!swordSpinning)
				{
					return;
				}
				angle += angleAcceleration;
				swordSpinning.eulerAngles = new Vector3(0f, 0f, angle);
				spinSoundFrames++;
				if (spinSoundFrames >= spinSoundRate)
				{
					sword.GetComponent<JerrySword>().PlaySFX("sounds/snd_wing", (float)(40 - spinSoundRate) / 37f);
					if (spinSoundRate > 3)
					{
						spinSoundRate -= 2;
					}
					spinSoundFrames = 0;
				}
			}
		}
		else if (state == 4)
		{
			frames++;
			int num18 = 20 - frames / 6;
			int num19 = ((frames / 6 % 2 != 0) ? 1 : (-1));
			if (num18 >= 0)
			{
				jerry.GetEnemyObject().transform.position = new Vector3((float)(num18 * num19) / 24f, 0.29f);
			}
			if (frames == 130)
			{
				UnityEngine.Object.Destroy(UnityEngine.Object.FindObjectOfType<JerryFinaleHPBar>().gameObject);
				UnityEngine.Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
				sword.GetComponent<BoxCollider2D>().enabled = false;
				UnityEngine.Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
				bb.StartMovement(bbSize, bbPos);
			}
			if (frames <= 160)
			{
				sword.GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, 1f - (float)(frames - 130) / 30f);
				if (frames == 160)
				{
					state = 5;
					frames = 0;
					jerry.Chat(new string[2] { "Y-^10you...", "B-^10but \nI..." }, "RightSmall", "snd_text", Vector2.zero, true, 2);
				}
			}
		}
		else if (state == 5)
		{
			if ((bool)jerry.GetTextBubble())
			{
				stumbleFrames++;
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_dying_" + stumbleFrames / 10 % 2);
				return;
			}
			frames++;
			if (frames == 1)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_dying_2");
			}
			if (frames < 10)
			{
				jerry.GetEnemyObject().transform.position = new Vector3(0f, 0.29f) + new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 24f;
				return;
			}
			if (frames == 10)
			{
				jerry.GetPart("body").GetComponent<SpriteRenderer>().enabled = false;
				jerry.GetPart("body").Find("headband").GetComponent<SpriteRenderer>()
					.enabled = false;
				jerry.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = true;
				jerry.PlaySFX("sounds/snd_damage");
				UnityEngine.Object.FindObjectOfType<BattleCamera>().BlastShake();
			}
			int num20 = 14 - (frames - 10);
			int num21 = ((frames % 2 != 0) ? 1 : (-1));
			if (num20 >= 0)
			{
				jerry.GetEnemyObject().transform.position = new Vector3((float)(num20 * num21) / 24f, 0.29f);
			}
			if (frames == 30)
			{
				stumbleFrames = 0;
				jerry.FinaleDeath();
				UnityEngine.Object.FindObjectOfType<PetalGenerator>().Deactivate();
			}
			if (frames >= 75)
			{
				if (frames == 75)
				{
					UnityEngine.Object.FindObjectOfType<PartyPanels>().RaiseHeads(true, false, false);
				}
				stumbleFrames++;
				UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_heavybreathing_" + stumbleFrames / 12 % 2);
				if (stumbleFrames == 48)
				{
					state = 6;
					UnityEngine.Object.FindObjectOfType<BattleManager>().StartText("kr_susgrin`snd_txtkrs`*^01 ^01W^01e^01'^01r^01e^01.^01.^01.^10 d^01o^01n^01e^01 ^01h^01e^01r^01e^01.^01.^01.", new Vector2(-4f, -134f), "snd_txtkrs");
				}
			}
		}
		else if (state == 6 && UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleText().Exists())
		{
			stumbleFrames++;
			UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_heavybreathing_" + stumbleFrames / 12 % 2);
			if (!UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying() && UTInput.GetButtonDown("Z"))
			{
				UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_heavybreathing_0");
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		UnityEngine.Object.FindObjectOfType<SOUL>().SetControllable(false);
	}
}
