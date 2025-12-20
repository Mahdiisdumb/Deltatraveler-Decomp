using System;
using System.Collections.Generic;
using UnityEngine;

public class Section3DreamSequencePt1 : CutsceneBase
{
	private Animator krisDream;

	private bool oblitVariant;

	private int aborted;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeOut(45);
				gm.StopMusic(45f);
			}
			else if (frames == 45)
			{
				if ((bool)UnityEngine.Object.FindObjectOfType<SusieAndNoelleGoToBed>())
				{
					UnityEngine.Object.Destroy(UnityEngine.Object.FindObjectOfType<SusieAndNoelleGoToBed>().gameObject);
				}
				fade.FadeIn(1);
				GameObject.Find("DreamObjects").transform.position = Vector3.zero;
			}
			if (frames == 120 && oblitVariant && (int)gm.GetFlag(13) < 9 && (int)gm.GetFlag(12) == 1)
			{
				StartText(new string[1] { "\b  THE ENEMIES THAT YOU MISSED\n\b       HAVE HIDDEN AWAY." }, new string[1] { "" }, new int[1] { 2 }, new string[1] { "" }, 0);
				txt.MakeUnskippable();
				txt.EnableGasterText();
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
				WeirdChecker.Abort(gm);
				aborted = 3;
			}
			if (frames >= 150)
			{
				if (frames == 150)
				{
					GameObject.Find("Wind").GetComponent<AudioSource>().Play();
				}
				GameObject.Find("Wind").GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 0.5f, (float)(frames - 150) / 45f);
			}
			if (frames == 195)
			{
				if (!oblitVariant)
				{
					StartText(new string[1] { "* What did I do...?" }, new string[1] { "" }, new int[1] { 2 }, new string[1] { "" }, 1);
					UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
					UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
				}
				frames = 0;
				state = 1;
			}
		}
		else if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.PlayMusic("music/mus_kris", oblitVariant ? 0.6f : 1f);
				if (oblitVariant)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_dream_kneel_oblit");
					SetSprite(GameObject.Find("KrisDouble").transform, "player/Kris/spr_kr_dream_kneel_oblit");
				}
			}
			if (frames <= 90)
			{
				float num = (float)frames / 90f;
				for (int i = 0; i < 2; i++)
				{
					SpriteRenderer obj = ((i == 0) ? krisDream.GetComponent<SpriteRenderer>() : GameObject.Find("KrisDouble").GetComponent<SpriteRenderer>());
					obj.transform.localPosition = new Vector3(Mathf.Lerp((i != 0) ? 1 : (-1), 0f, num), 0f);
					obj.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, num * num);
				}
				if (frames == 90)
				{
					UnityEngine.Object.Destroy(GameObject.Find("KrisDouble"));
				}
			}
			if (frames != 120)
			{
				return;
			}
			if (oblitVariant)
			{
				List<string> list = new List<string> { "* What is wrong with me?" };
				if (aborted == 0)
				{
					list.AddRange(new string[11]
					{
						"* Why can't I stop killing\n  people?", "* This demon inside of me that\n  keeps the slaughter going...", "* It doesn't care about me.", "* It doesn't care about my\n  friends or anyone else.", "* It just wants to see\n  everything around me suffer.", "* How does Susie think of\n  me now?", "* Does she think I'm a freak?", "* A horrible person.", "* Because that's what I am.", "* I can't stop killing.",
						"* As hard as I try,^10 I can't\n  stop."
					});
				}
				else
				{
					list.AddRange(new string[3]
					{
						"* Why couldn't I stop killing\n  people?",
						"* This demon inside of me that\n  kept the slaughter going...",
						(aborted == 3) ? "* Sure,^10 it missed a few victims,^10\n  but I can't say for sure\n  of it's intentions." : "* Sure,^10 it spared a few victims,^10\n  but..."
					});
					if (aborted == 3)
					{
						list.Add("* And besides...");
					}
					if (aborted == 3 || aborted == 2)
					{
						list.Add("* We killed children.");
					}
					else
					{
						list.Add("* We killed a bunch of innocent\n  people.");
					}
					list.AddRange(new string[4] { "* I can't imagine what Susie\n  would say about this.", "* Will she think of me as\n  anything other than a freak?", "* Because that's what I am.", "* A freak with a high body\n  count,^10 regardless of who I\n  spared." });
				}
				list.AddRange(new string[8] { "* All of my friends will abandon\n  me.", "* I will die alone.", "* Surrounded by blood.", "* And I will never be shown\n  forgiveness.", "* Never.", "* Never.", "* Never.", "* Never..." });
				StartText(list.ToArray(), new string[1] { "" }, new int[1] { 2 }, new string[1] { "" }, 1);
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
				frames = 60;
				state = 4;
			}
			else
			{
				StartText(new string[10] { "* It feels like I'm drifting\n  without my input.", "* Doing things against my will.", "* Yet everyone thinks...", "* \"It's just you,^10 Kris.\"", "* What did I do to deserve\n  this?", "* Am I just...^10 cursed?", "* Why do I deserve to be\n  cursed with the removal of\n  my agency?", "* What about the me from all\n  these other worlds?", "* Do they also suffer this\n  curse?", "* Is this just...^20 my fate?" }, new string[1] { "" }, new int[1] { 2 }, new string[1] { "" }, 1);
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
				frames = 0;
				state = 2;
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				if (frames >= 120 && AtLine(2))
				{
					txt.gameObject.AddComponent<ShakingText>().StartShake(10);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				gm.StopMusic(60f);
			}
			if (frames == 60)
			{
				PlaySFX("sounds/snd_step1");
				PlayAnimation(krisDream, "RemoveSoulDream", 0f);
			}
			if (frames == 90)
			{
				StartText(new string[2] { "* I want to end it so badly.", "* All I want to do..." }, new string[1] { "" }, new int[1] { 2 }, new string[1] { "" }, 1);
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
			}
			if (frames == 91)
			{
				PlayAnimation(krisDream, "RemoveSoulDream");
			}
			if (frames == 143)
			{
				PlaySFX("sounds/snd_grab");
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), Vector3.zero, Quaternion.identity);
			}
			if (frames == 180)
			{
				StartText(new string[2] { "* Is rip it out...", "* And just..." }, new string[1] { "" }, new int[2] { 2, 3 }, new string[1] { "" }, 1);
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
			}
			if (frames > 180)
			{
				if (frames <= 255 && frames >= 225)
				{
					float t = (float)(frames - 225) / 30f;
					GameObject.Find("SoulDream").transform.position = new Vector3(Mathf.Lerp(0f, 2.35f, t), 0f);
					krisDream.transform.position = new Vector3(0f, Mathf.Sin((float)((frames - 225) * 6) * ((float)Math.PI / 180f)));
				}
				if (frames == 181)
				{
					PlayAnimation(krisDream, "SoulThrowDream");
					PlaySFX("sounds/snd_heavyswing");
				}
				else if (frames == 185)
				{
					gm.PlayGlobalSFX("sounds/snd_crash");
					GameObject.Find("SoulDream").GetComponent<Animator>().enabled = true;
				}
				else if (frames == 215)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_up_soul_yeet_5_dream");
				}
				else if (frames == 225)
				{
					PlaySFX("sounds/snd_jump");
					SetSprite(krisDream, "player/Kris/spr_kr_up_soul_yeet_6_dream");
				}
				else if (frames == 240)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_up_soul_yeet_7_dream");
				}
				else if (frames == 253)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_up_soul_yeet_8_dream");
				}
				else if (frames == 255)
				{
					GameObject.Find("Wind").GetComponent<AudioSource>().volume = 0f;
					GameObject.Find("SoulDream").GetComponent<Animator>().enabled = false;
					GameObject.Find("SoulDream").GetComponent<SpriteRenderer>().enabled = false;
					GameObject.Find("SoulDream").transform.position = Vector3.zero;
					GameObject.Find("DreamBG").GetComponent<SpriteRenderer>().color = Color.white;
					SetSprite(krisDream, "player/Kris/spr_kr_dream_dying_0");
				}
				else if (frames == 256)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_dream_dying_1");
					GameObject.Find("SoulDream").GetComponent<SpriteRenderer>().enabled = true;
					SetSprite(GameObject.Find("SoulDream").GetComponent<SpriteRenderer>(), "player/Kris/spr_soul_splatter_0");
				}
				else if (frames == 257)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_dream_dying_2");
					SetSprite(GameObject.Find("SoulDream").GetComponent<SpriteRenderer>(), "player/Kris/spr_soul_splatter_1");
				}
				if (frames >= 315 && frames <= 318)
				{
					int num2 = ((frames % 2 == 0) ? 1 : (-1));
					int num3 = 318 - frames;
					krisDream.transform.position = new Vector3((float)(num3 * num2) / 24f, 0f);
				}
				else if (frames == 375)
				{
					SetSprite(krisDream, "player/Kris/spr_kr_dream_dying_3");
					frames = 0;
					state = 3;
				}
			}
		}
		else if (state == 3)
		{
			frames++;
			krisDream.transform.position = new Vector3((float)UnityEngine.Random.Range(-1, 2) / 48f, (float)UnityEngine.Random.Range(-1, 2) / 48f);
			if (frames == 20)
			{
				StartText(new string[5] { "* Urgh...^20 AUGH!!!^20\n* I can't breathe...!", "* Susie...^20\n  Noelle...", "* ... Asriel...", "* S-^15someone,^20 anyone...", "* Please...^20  help me..." }, new string[1] { "" }, new int[1] { 3 }, new string[1] { "" }, 1);
				txt.gameObject.AddComponent<ShakingText>().StartShake(1);
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
			}
			if (frames > 20 && !txt)
			{
				UnityEngine.Object.Destroy(krisDream.gameObject);
				UnityEngine.Object.Destroy(GameObject.Find("SoulDream"));
				GameObject.Find("DreamBG").GetComponent<SpriteRenderer>().color = Color.black;
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4 && !txt)
		{
			frames++;
			if (frames <= 60)
			{
				GameObject.Find("Wind").GetComponent<AudioSource>().volume = Mathf.Lerp(0.5f, 0f, (float)(frames - 15) / 45f);
			}
			if (frames == 60)
			{
				StartText(new string[2] { "* Please...", "* I don't want to die alone..." }, new string[1] { "" }, new int[1] { 3 }, new string[1] { "" }, 1);
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
			}
			if (frames == 61)
			{
				susie.transform.position = new Vector3(-3.33f, -4.4f);
				noelle.transform.position = new Vector3(-1.87f, -4.4f);
				PlayAnimation(susie, "idle");
				PlayAnimation(noelle, "idle");
				SetMoveAnim(susie, false);
				SetMoveAnim(noelle, false);
				susie.UseUnhappySprites();
				noelle.UseUnhappySprites();
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.left);
				GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>().enabled = false;
				GameObject.Find("RAGE").GetComponent<SpriteRenderer>().enabled = false;
				GameObject.Find("LightsOff").GetComponent<SpriteRenderer>().enabled = false;
				GameObject.Find("Bed").transform.position = new Vector3(-4.124f, 1.433f);
				noelle.GetComponent<SpriteRenderer>().enabled = true;
				kris.transform.position = new Vector3(-2.665f, 0.419f);
				SetSprite(kris, "spr_kr_sleep_onground");
				if (oblitVariant)
				{
					gm.StopMusic(120f);
				}
				else
				{
					PlaySFX("sounds/snd_darkness");
				}
			}
			if (frames < 240 && oblitVariant)
			{
				krisDream.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float)(frames - 60) / 120f);
				GameObject.Find("Wind").GetComponent<AudioSource>().volume = Mathf.Lerp(0.5f, 0f, (float)(frames - 60) / 120f);
			}
			if (frames == 300)
			{
				PlaySFX("sounds/snd_knock");
			}
			if (frames == 330)
			{
				UnityEngine.Object.Destroy(GameObject.Find("DreamObjects"));
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 360)
			{
				if (oblitVariant)
				{
					StartText(new string[5]
					{
						"* Kris???",
						"* We've been knocking\n  for like 5 minutes\n  already!",
						"* Don't make us come\n  in there.",
						"* Kris,^05 we really need\n  to have that talk.",
						((int)gm.GetFlag(87) >= 8) ? "* If you don't want us\n  to leave without you\n  again,^05 come out here." : "* If you don't come\n  out here,^05 we're leaving\n  without you."
					}, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[5] { "su_angry", "su_wtf", "su_annoyed", "no_depressed_look", "su_annoyed" }, 0);
				}
				else
				{
					StartText(new string[7] { "* Kris???", "* We've been knocking\n  for like 5 minutes\n  already!", "* Don't make us come\n  in there.", "* Give them some time,^05\n  Susie.", "* They're...^05 really bad\n  at waking up.", "* I've BEEN waiting!!!", "* If you don't come\n  out here,^05 we're leaving\n  without you." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], new string[7] { "su_angry", "su_wtf", "su_annoyed", "no_shocked", "no_thinking", "su_wtf", "su_annoyed" }, 0);
				}
				state = 5;
				frames = 0;
			}
		}
		else
		{
			if (state != 5)
			{
				return;
			}
			if ((bool)txt)
			{
				if (!oblitVariant)
				{
					if (AtLine(6))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(7))
					{
						PlayAnimation(susie, "idle");
					}
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				SetSprite(noelle, "spr_no_sit_unhappy");
				ChangeDirection(susie, Vector2.left);
				SetMoveAnim(susie, true);
			}
			if (!MoveTo(susie, new Vector3(-5f, -4.2f), 4f))
			{
				SetMoveAnim(susie, false);
				SetSprite(susie, "spr_su_crossed_down");
			}
			if (frames == 30)
			{
				gm.SetSessionFlag(6, 1);
				kris.transform.position = new Vector3(-2.665f, 1.26f);
				SetSprite(kris, "spr_kr_sit_injured");
				PlaySFX("sounds/snd_wing");
			}
			if (frames >= 30 && frames <= 75)
			{
				float num4 = (float)(frames - 30) / 45f;
				num4 = num4 * num4 * num4 * (num4 * (6f * num4 - 15f) + 10f);
				GameObject.Find("Blanket").transform.position = new Vector3(-2.71f, Mathf.Lerp(0.4f, -0.5f, num4));
				if (frames == 60)
				{
					SetSprite(GameObject.Find("Blanket").transform, "overworld/snow_objects/spr_bunny_comforter_1");
					GameObject.Find("Blanket").GetComponent<SpriteRenderer>().sortingOrder = -100;
				}
			}
			if (frames == 90)
			{
				PlayAnimation(kris, "idle");
				ChangeDirection(kris, Vector2.down);
				kris.SetSelfAnimControl(true);
				kris.SetMovement(true);
				EndCutscene(false);
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 3 && (bool)UnityEngine.Object.FindObjectOfType<TextUT>() && (bool)UnityEngine.Object.FindObjectOfType<TextUT>().GetText())
		{
			UnityEngine.Object.FindObjectOfType<TextUT>().GetText().color = Color.black;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		krisDream = GameObject.Find("KrisDream").GetComponent<Animator>();
		kris.SetMovement(false);
		oblitVariant = (int)gm.GetFlag(87) > 4 || WeirdChecker.HasCommittedBloodshed(gm);
		if (oblitVariant && (int)gm.GetFlag(12) == 0)
		{
			aborted = (((int)gm.GetFlag(87) < 7) ? 1 : 2);
		}
		gm.SetHP(0, gm.GetMaxHP(0) + 10, true);
		gm.SetHP(1, gm.GetMaxHP(1) + 10, true);
		gm.SetHP(2, gm.GetMaxHP(2) + 10, true);
		gm.SetFlag(172, 0);
		StartText(new string[1] { "* (You decide to climb into\n  bed.)" }, new string[1] { "snd_text" }, new int[0], new string[1] { "" });
	}
}
