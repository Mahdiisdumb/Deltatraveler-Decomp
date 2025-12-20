using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SansMazeCatchCutscene : CutsceneBase
{
	private Animator sans;

	private Animator qc;

	private SpriteRenderer soul;

	private int shakeFrames = 90;

	private int freak = 10;

	private bool oblit;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 15)
			{
				RevokePlayerControl();
				fade.FadeIn(1);
				sans.transform.position = new Vector3(1.686f, -16.11f);
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 30)
			{
				StartText(new string[1] { "* Found you." }, new string[1] { "" }, new int[1] { 2 }, new string[1] { "ufsans_empty" }, 0);
				txt.GetTextUT().SetLetterSpacing(15.3825f);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				ChangeDirection(kris, Vector2.down);
				SetSprite(kris, "spr_kr_surprise_down_holdingtorch");
				SetSprite(susie, "spr_su_freaked");
				Vector3 vector = noelle.transform.position - sans.transform.position;
				SetSprite(noelle, (vector.x > vector.y) ? "spr_no_surprise_left" : "spr_no_surprise");
				PlaySFX("sounds/snd_noelle_scared");
			}
			GameObject.Find("wind").GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, (float)frames / 30f);
			if (frames < 10)
			{
				return;
			}
			if (frames == 10)
			{
				ChangeDirection(kris, Vector2.up);
				kris.EnableAnimator();
				SetMoveAnim(kris, isMoving: true, 1.5f);
				ChangeDirection(susie, Vector2.up);
				susie.EnableAnimator();
				SetMoveAnim(susie, isMoving: true, 1.5f);
				ChangeDirection(noelle, new Vector2(2.648f - noelle.transform.position.x, 0f));
				noelle.EnableAnimator();
				SetMoveAnim(noelle, isMoving: true, 2f);
			}
			bool flag = MoveTo(kris, new Vector3(1.71f, -11.62f), 8f);
			bool flag2 = MoveTo(susie, new Vector3(1.712f, -12.17f), 8f);
			bool flag3 = false;
			MoveTo(cam, new Vector3(1.72f, -12.77f, -10f), 2f);
			bool flag4 = true;
			if (!flag)
			{
				ChangeDirection(kris, Vector2.down);
				SetMoveAnim(kris, isMoving: false);
			}
			if (!flag2)
			{
				ChangeDirection(susie, Vector2.down);
				SetMoveAnim(susie, isMoving: false);
			}
			if (noelle.transform.position.x != 2.648f)
			{
				MoveTo(noelle, new Vector3(2.648f, noelle.transform.position.y), 12f);
				flag4 = false;
				flag3 = true;
			}
			else if (MoveTo(noelle, new Vector3(2.648f, -11.45f), 12f))
			{
				ChangeDirection(noelle, Vector2.up);
				flag4 = false;
				flag3 = true;
			}
			else
			{
				ChangeDirection(noelle, Vector2.down);
				SetMoveAnim(noelle, isMoving: false);
				SetSprite(noelle, "spr_no_surprise");
			}
			if (flag4 && MoveTo(sans, new Vector3(1.686f, -13.79f), 2f))
			{
				SetMoveAnim(sans, isMoving: true);
				return;
			}
			SetMoveAnim(sans, isMoving: false);
			MonoBehaviour.print(flag + " " + flag2 + " " + flag3);
			if (!flag && !flag2 && !flag3)
			{
				gm.PlayMusic("music/mus_him", 0.45f);
				StartText(new string[9] { "* Back the hell away,^05\n  freak!", "* nah.", "* i've got you right where\n  i want you.", "* W-^05what do you want\n  from us???", "* y'know,^05 every time i've\n  held onto hope,^05 it's just\n  eroded away.", "* it's about time you felt\n  what i felt.", "* That doesn't answer the\n  question,^05 dumbass!!!", "* like i'd tell you about\n  how i felt.", "* you should know." }, new string[9] { "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtnoe", "snd_txtsans", "snd_txtsans", "snd_txtsus", "snd_txtsans", "snd_txtsans" }, new int[1], new string[9] { "su_annoyed_sweat", "ufsans_closed", "ufsans_empty", "no_afraid_open", "ufsans_side", "ufsans_closed", "su_angry", "ufsans_neutral", "ufsans_empty" }, 1);
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlayAnimation(susie, "Kick");
				ChangeDirection(kris, Vector2.right);
				SetMoveAnim(kris, isMoving: true);
			}
			if (!MoveTo(kris, new Vector3(0.731f, -11.72f), 8f))
			{
				SetMoveAnim(kris, isMoving: false);
			}
			susie.transform.position = Vector3.Lerp(new Vector3(1.712f, -12.17f), new Vector3(1.712f, -11.5f), (float)frames / 6f);
			if (frames == 6)
			{
				PlaySFX("sounds/snd_crash");
				SetSprite(noelle, "spr_no_surprise_left");
			}
			if (frames != 15)
			{
				return;
			}
			List<string> list = new List<string>
			{
				"* HEY!!!^05\n* RABBIT LADY!!!\n^05* OPEN THE DOOR!!!!!",
				"* she's not gonna help.",
				"* no one will.",
				"* you can't even help\n  yourselves against me.",
				"* You sure you wanna\n  try us?",
				(Util.GameManager().GetLV() >= 7) ? "* We've...^05 got quite the\n  body count already." : "* Dimension travellers\n  that can take on\n  a giant mech?",
				"* S-^05Susie,^05 don't--"
			};
			List<string> list2 = new List<string> { "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsus", "snd_txtsus", "snd_txtnoe" };
			List<string> list3 = new List<string>
			{
				"su_wtf",
				"ufsans_side",
				"ufsans_closed",
				"ufsans_empty",
				"su_depressed_smile",
				(Util.GameManager().GetLV() >= 7) ? "su_smile_sweat" : "su_teeth_eyes",
				"no_scared"
			};
			if (Util.GameManager().GetLV() >= 7)
			{
				list.AddRange(new string[2] { "* now you're interesting me.", "* how many have you killed?" });
				list2.AddRange(new string[2] { "snd_txtsans", "snd_txtsans" });
				list3.AddRange(new string[2] { "ufsans_side", "ufsans_neutral" });
				if ((int)Util.GameManager().GetFlag(87) >= 5)
				{
					list.AddRange(new string[7] { "* You don't wanna know.", "* yeah i do.^05\n* tell me.", "* Umm...^10 I don't even\n  know...", "* yeah,^05 i think your\n  dusty,^05 stained clothes\n  speak to that.", "* so i truly am\n  doing a service.", "* the entirety of reality\n  would benefit from your\n  deaths.", "* time to die." });
					list2.AddRange(new string[6] { "snd_txtsus", "snd_txtsans", "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtsans" });
					list3.AddRange(new string[7] { "su_smirk", "ufsans_neutral", "su_side_sweat", "ufsans_side", "ufsans_closed", "ufsans_closed", "ufsans_empty" });
				}
				else
				{
					list.AddRange(new string[4] { "* I dunno.^05\n* How many have YOU\n  killed?", "* 37 people.", "* 40 in a minute.", "* time to die." });
					list2.AddRange(new string[4] { "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtsans" });
					list3.AddRange(new string[4] { "su_side_sweat", "ufsans_neutral", "ufsans_empty", "ufsans_empty" });
				}
			}
			else
			{
				list.AddRange(new string[3] { "* you're really getting my\n  expectations up,^05 but\n  i don't believe you.", "* now enough talking.", "* time to die." });
				list2.AddRange(new string[3] { "snd_txtsans", "snd_txtsans", "snd_txtsans" });
				list3.AddRange(new string[3] { "ufsans_neutral", "ufsans_closed", "ufsans_empty" });
			}
			freak = list.Count;
			StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray(), 1);
			state = 3;
			frames = 0;
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(kris, Vector2.down);
				}
				else if (AtLine(3))
				{
					noelle.EnableAnimator();
					ChangeDirection(noelle, Vector2.down);
					ChangeDirection(susie, Vector2.up);
					PlayAnimation(susie, "idle");
				}
				else if (AtLine(5))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(6))
				{
					SetSprite(susie, "spr_su_shrug");
				}
				else if (AtLine(7))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.right);
					SetSprite(noelle, "spr_no_left_shocked_0");
				}
				else if (AtLine(8))
				{
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(susie, Vector2.down);
					noelle.EnableAnimator();
					ChangeDirection(noelle, Vector2.down);
				}
				else if (AtLine(freak))
				{
					SetSprite(noelle, "spr_no_surprise");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				susie.GetComponent<SpriteRenderer>().enabled = false;
				noelle.GetComponent<SpriteRenderer>().enabled = false;
				SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				Collider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<Collider2D>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
				AudioSource[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
				foreach (AudioSource audioSource in componentsInChildren3)
				{
					if (audioSource.isPlaying)
					{
						audioSource.enabled = false;
					}
				}
				TilemapRenderer[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
				for (int i = 0; i < componentsInChildren4.Length; i++)
				{
					componentsInChildren4[i].enabled = false;
				}
				SpriteMask[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<SpriteMask>();
				for (int i = 0; i < componentsInChildren5.Length; i++)
				{
					componentsInChildren5[i].enabled = false;
				}
				soul = Object.Instantiate(Resources.Load<GameObject>("overworld/OWSoul"), kris.transform).GetComponent<SpriteRenderer>();
				soul.transform.localScale = new Vector2(0.5f, 0.5f);
				soul.GetComponent<SpriteRenderer>().sortingOrder = 300;
				gm.StopMusic();
			}
			if (frames == 1 || frames == 9 || frames == 17)
			{
				PlaySFX("sounds/snd_noise");
				soul.GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames == 5 || frames == 13)
			{
				soul.GetComponent<SpriteRenderer>().enabled = false;
			}
			if (frames == 21)
			{
				susie.GetComponent<SpriteRenderer>().enabled = true;
				noelle.GetComponent<SpriteRenderer>().enabled = true;
				SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = true;
				}
				Collider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<Collider2D>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = true;
				}
				AudioSource[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
				for (int i = 0; i < componentsInChildren3.Length; i++)
				{
					componentsInChildren3[i].enabled = true;
				}
				TilemapRenderer[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
				for (int i = 0; i < componentsInChildren4.Length; i++)
				{
					componentsInChildren4[i].enabled = true;
				}
				SpriteMask[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<SpriteMask>();
				for (int i = 0; i < componentsInChildren5.Length; i++)
				{
					componentsInChildren5[i].enabled = true;
				}
				Object.Destroy(soul.gameObject);
				qc.transform.position = new Vector3(1.686f, -16.31f);
			}
			if (frames >= 21 && frames <= 27)
			{
				qc.transform.position = new Vector3(1.686f, Mathf.Lerp(-16.31f, -13.8f, (float)(frames - 21) / 6f));
				if (frames == 27)
				{
					SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_attack_sans_1");
					SetSprite(kris, "spr_kr_surprise_down_holdingtorch");
					SetSprite(susie, "spr_su_freaked");
					sans.GetComponent<SpriteRenderer>().enabled = false;
					PlaySFX("sounds/snd_grab");
				}
			}
			if (frames >= 27 && frames <= 30)
			{
				int num = ((frames % 2 == 0) ? 1 : (-1));
				int num2 = 30 - frames;
				qc.transform.position = new Vector3(1.686f, -13.8f) + new Vector3((float)(num2 * num) / 24f, 0f);
			}
			if (frames == 35)
			{
				StartText(new string[2] { "* GOTCHA!!!", "* Get the hell away from\n  them you damn--" }, new string[2] { "snd_text", "snd_text" }, new int[1], new string[2] { "", "" }, 1);
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)txt)
			{
				if (shakeFrames > 2)
				{
					shakeFrames--;
				}
				if (Random.Range(0, shakeFrames) == 0)
				{
					qc.transform.position = new Vector3(1.686f, -13.8f) + new Vector3((float)((Random.Range(0, 2) != 0) ? 1 : (-1)) / 24f, 0f);
				}
				else
				{
					qc.transform.position = new Vector3(1.686f, -13.8f);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				sans.GetComponent<SpriteRenderer>().enabled = true;
				sans.enabled = false;
				PlaySFX("sounds/snd_damage");
				SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_push_qc");
				SetSprite(qc, "overworld/npcs/underfell/spr_qc_hit");
			}
			if (frames <= 4)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				int num4 = 4 - frames;
				sans.transform.position = new Vector3(1.686f, -13.79f) + new Vector3((float)(num4 * num3) / 24f, 0f);
			}
			if (frames <= 15)
			{
				qc.transform.position = new Vector3(1.686f, Mathf.Lerp(-13.8f, -15.39f, Mathf.Sqrt((float)frames / 15f)));
			}
			if (frames == 20)
			{
				PlayAnimation(sans, "Vanish");
				PlaySFX("sounds/snd_shadowpendant");
			}
			if (frames >= 70 && frames <= 90)
			{
				if (frames == 70)
				{
					kris.EnableAnimator();
					susie.EnableAnimator();
					noelle.EnableAnimator();
					SetMoveAnim(kris, isMoving: true);
					SetMoveAnim(susie, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
				}
				MoveTo(kris, new Vector3(0.731f, -300f), 3f);
				MoveTo(susie, new Vector3(1.712f, -300f), 3f);
				MoveTo(noelle, new Vector3(2.648f, -300f), 3f);
				if (frames == 90)
				{
					SetMoveAnim(kris, isMoving: false);
					SetMoveAnim(susie, isMoving: false);
					SetMoveAnim(noelle, isMoving: false);
					StartText(new string[11]
					{
						"* Q.C.!!!^05\n* Are you okay?", "* Yeah,^05 I'm fine,^05 but...", "* He got away.", "* Feels like every time I get\n  a chance to kill him,^05 he\n  runs away.", "* So much for his \"kill or\n  be killed\" rhetoric.", "* Uhh...^05 thanks for saving\n  us,^05 but uhh...", "* We've got like four\n  of those ladder parts\n  now.", "* Do you know if we\n  need to get more?", "* Oh,^05 right!^05\n* I believe that's all of them!", "* I'm carrying the ones I\n  found right now,^05 so let's go\n  ahead and rebuild the ladder.",
						"* Meet me over at the dead\n  end to the <color=#FFFF00FF>east of here</color>."
					}, new string[11]
					{
						"snd_txtnoe", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_text",
						"snd_text"
					}, new int[1], new string[13]
					{
						"no_afraid", "", "", "", "", "su_neutral", "su_side", "su_neutral", "", "",
						"", "", ""
					}, 0);
					state = 5;
					frames = 0;
				}
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (AtLine(9))
				{
					qc.enabled = true;
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(qc, Vector2.right);
				SetMoveAnim(qc, isMoving: true);
			}
			qc.transform.position += new Vector3(0.125f, 0f);
			if (frames != 45)
			{
				return;
			}
			Object.Destroy(qc.gameObject);
			List<string> list4 = new List<string> { "* ... Descriptive...", "* I'm sure she means\n  there's a cliff we can\n  build the ladder to.", "* I guess...", "* But uhh,^05 what about\n  that whole \"I'll keep\n  a leash on him\" bit?" };
			List<string> list5 = new List<string> { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" };
			List<string> list6 = new List<string> { "su_inquisitive", "no_curious", "su_annoyed", "su_side" };
			if (!oblit)
			{
				list4.AddRange(new string[4] { "* Maybe...^10 umm...", "* Oh,^05 Sans left after\n  the letter attack!", "* Maybe he ran away\n  while Papyrus was\n  distracted.", "* They'll BOTH run away\n  after I beat him\n  up." });
				list5.AddRange(new string[5] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" });
				list6.AddRange(new string[5] { "no_thinking", "no_awe", "no_confused_side", "su_angry", "su_annoyed" });
			}
			else
			{
				bool flag5 = (int)Util.GameManager().GetFlag(199) == 1;
				list4.AddRange(new string[7]
				{
					"* ...^05 What do you mean?",
					"* Oh,^05 right.^05\n* You weren't there.",
					flag5 ? "* Kris almost got attacked\n  by Sans with a weird\n  letter bomb of bones." : "* Sans tried tricking Kris\n  to open a weird letter\n  bomb of bones.",
					flag5 ? "* I knocked Kris out of\n  the way,^05 but..." : "* It didn't work,^05 but...",
					"* Papyrus said that he'd\n  keep an eye on Sans.",
					"* And Sans chasing us\n  means his eyes were\n  kept on SHIT!!!",
					"* He's about to see\n  my fists next time\n  we see him."
				});
				list5.AddRange(new string[8] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
				list6.AddRange(new string[8] { "no_thinking", "su_surprised", "su_annoyed", "su_side", "su_neutral", "su_wtf", "su_angry", "su_pissed" });
			}
			bool flag6 = false;
			for (int j = 0; j < 3; j++)
			{
				if ((float)gm.GetHP(j) / (float)gm.GetMaxHP(j) <= 0.5f)
				{
					flag6 = true;
					break;
				}
			}
			if (flag6 && gm.GetFlagInt(12) == 0)
			{
				list4.AddRange(new string[6] { "* Now let's...", "* ... Actually,^05 I feel\n  kinda bad right now.", "* Not sure if we\n  should go on without\n  healing first.", "* That might be a\n  good idea.", "* Maybe we should talk\n  to Sans...?", "* Well,^05 OUR Sans,^05 that\n  is." });
				list5.AddRange(new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" });
				list6.AddRange(new string[5] { "su_inquisitive", "su_neutral", "no_curious", "no_happy", "no_weird" });
			}
			else
			{
				list4.Add("* Now let's go.");
			}
			StartText(list4.ToArray(), list5.ToArray(), new int[1], list6.ToArray());
			state = 6;
			frames = 0;
		}
		else
		{
			if (state != 6)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
				}
				if (!oblit)
				{
					if (AtLine(5))
					{
						ChangeDirection(noelle, Vector2.up);
					}
					else if (AtLine(6))
					{
						ChangeDirection(noelle, Vector2.left);
					}
					else if (AtLine(8))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(9))
					{
						ChangeDirection(susie, Vector2.left);
						susie.EnableAnimator();
					}
				}
				else if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(7))
				{
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(10))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(12))
				{
					ChangeDirection(susie, Vector2.left);
					susie.EnableAnimator();
				}
			}
			else if (!MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				RestorePlayerControl();
				gm.PlayMusic("zoneMusic");
				ChangeDirection(kris, Vector2.down);
				gm.SetCheckpoint(87, new Vector3(1.69f, -12.52f));
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(20f, -1f), Quaternion.identity);
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		sans = GameObject.Find("Sans").GetComponent<Animator>();
		ChangeDirection(sans, Vector2.up);
		qc = GameObject.Find("QC-cutscene").GetComponent<Animator>();
		ChangeDirection(qc, Vector2.up);
		Util.FindObjectOfType<DeepMazeEventHandler>().StopChase();
		gm.SetFlag(84, 10);
		gm.SetFlag(208, 2);
		gm.UnlockMenu();
		Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/underfell/QCEnd"), GameObject.Find("NPC").transform);
		oblit = (int)Util.GameManager().GetFlag(87) >= 8;
		GameObject.Find("wind").GetComponent<AudioSource>().volume = 0f;
		gm.StopMusic();
		PlaySFX("sounds/snd_noise");
		fade.FadeOut(1);
	}
}
