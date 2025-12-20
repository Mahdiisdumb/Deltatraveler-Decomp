using System;
using System.Collections.Generic;
using UnityEngine;

public class Section3DreamSequencePt2 : CutsceneBase
{
	private bool doorClosed;

	private bool selecting;

	private bool canInterrupt;

	private bool oblitVariant;

	private bool aborted;

	private bool susieKnowsOfAbort;

	private int abortOffset;

	private int reasonChoice;

	private Animator bed;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames >= 20)
			{
				if (!MoveTo(kris, new Vector3(-3.32f, -5.13f), 3f) && !doorClosed)
				{
					doorClosed = true;
					SetMoveAnim(kris, false);
					PlaySFX("sounds/snd_doorclose");
					SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_0");
				}
				else if (!doorClosed)
				{
					SetMoveAnim(kris, true, 0.75f);
				}
				MoveTo(cam, new Vector3(0f, -5f, -10f), 4f);
			}
			if (frames == 50)
			{
				PlayAnimation(noelle, "idle");
			}
			if (frames == 70)
			{
				PlayAnimation(susie, "idle");
				ChangeDirection(susie, Vector2.right);
			}
			if (frames >= 90)
			{
				if (!MoveTo(susie, new Vector3(-4.61f, -4.93f), 4f))
				{
					SetMoveAnim(susie, false);
				}
				else
				{
					SetMoveAnim(susie, true);
				}
				if (!MoveTo(noelle, new Vector3(-2.12f, -4.93f), 4f))
				{
					SetMoveAnim(noelle, false);
				}
				else
				{
					SetMoveAnim(noelle, true);
				}
			}
			if (frames != 135)
			{
				return;
			}
			if (oblitVariant)
			{
				ChangeDirection(kris, Vector2.left);
				List<string> list = new List<string> { "* So,^05 Kris.", "* You said \"I can't\n  control myself\" back in\n  that human town...", "* Back when we killed\n  that snake." };
				List<string> list2 = new List<string> { "snd_txtsus", "snd_txtsus", "snd_txtsus" };
				List<string> list3 = new List<string> { "su_side", "su_neutral", "su_neutral" };
				if ((int)gm.GetFlag(87) == 4 && aborted)
				{
					list.Add("* Even though you stopped\n  immediately,^05 we still\n  killed a lot of people.");
					list2.Add("snd_txtsus");
					list3.Add("su_depressed");
				}
				else if ((int)gm.GetFlag(87) == 5 && aborted)
				{
					list.AddRange(new string[2] { "* You stopped after\n  getting bonked in the\n  head...", "* But we still killed\n  a lot of people." });
					list2.AddRange(new string[2] { "snd_txtsus", "snd_txtsus" });
					list3.AddRange(new string[2] { "su_side", "su_depressed" });
					if ((int)gm.GetFlag(117) == 0)
					{
						list.Add("* Including the cult guy...");
						list2.Add("snd_txtsus");
						list3.Add("su_depressed");
					}
				}
				if ((int)gm.GetFlag(87) >= 6)
				{
					list.AddRange(new string[2] { "* And now we've killed\n  an entire cave of\n  animals...", "* A whole cult..." });
					list2.AddRange(new string[2] { "snd_txtsus", "snd_txtsus" });
					list3.AddRange(new string[2] { "su_annoyed", "su_annoyed" });
				}
				if (aborted && (int)gm.GetFlag(117) == 0 && ((int)gm.GetFlag(87) == 5 || (int)gm.GetFlag(87) == 6))
				{
					list.Add(((int)gm.GetFlag(150) == 1) ? "* I'm still thankful that\n  you spared Ness and\n  Paula,^05 but still." : "* I'm at least glad we\n  stopped before we could\n  kill \"the children.\"");
					list2.Add("snd_txtsus");
					list3.Add(((int)gm.GetFlag(150) == 1) ? "su_side_sweat" : "su_dejected");
				}
				if ((int)gm.GetFlag(87) >= 7)
				{
					list.Add("* Two actual children...");
					list2.Add("snd_txtsus");
					list3.Add("su_dejected");
					if (aborted && (int)gm.GetFlag(87) == 7)
					{
						list.Add("* But hey,^05 at least\n  you spared Snowy,^05 I\n  guess...");
						list2.Add("snd_txtsus");
						list3.Add("su_side");
					}
				}
				if ((int)gm.GetFlag(87) >= 8)
				{
					list.Add("* And this world's Snowy.");
					list2.Add("snd_txtsus");
					list3.Add("su_depressed");
					if (aborted)
					{
						if ((int)gm.GetFlag(87) == 8 && (int)gm.GetFlag(205) == 1)
						{
							list.AddRange(new string[2] { "* Huh...?^05\n* You spared enemies\n  before coming here?", "* Well,^05 I guess it's\n  nice that us leaving\n  made you stop killing???" });
							list2.AddRange(new string[2] { "snd_txtsus", "snd_txtsus" });
							list3.AddRange(new string[2] { "su_neutral", "su_smirk_sweat" });
						}
						else
						{
							list.AddRange(new string[4] { "* Now,^05 I know you spared\n  that one weird snowdrake\n  we fought...", "* But you didn't spare\n  anything from before\n  that point.", "* And I doubt you\n  spared enemies while\n  we were gone.", "* But I guess it's\n  better to do it now\n  than never." });
							list2.AddRange(new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
							list3.AddRange(new string[4] { "su_side", "su_annoyed", "su_annoyed", "su_side" });
						}
					}
				}
				if (aborted)
				{
					list.AddRange(new string[8] { "* Regardless...", "* Since you have been\n  willing to spare\n  people...", "* I'm willing to try\n  to go on as if\n  nothing happened.", "* After all,^05 we won't\n  see these places again\n  after we get home.", "* But I'm still curious...", "* Why?", "* Why do this?", "* Why do all the killing\n  in the first place?" });
					list2.AddRange(new string[2] { "snd_txtsus", "snd_txtsus" });
					list3.AddRange(new string[8] { "su_annoyed", "su_side", "su_neutral", "su_smirk_sweat", "su_side", "su_neutral", "su_neutral", "su_neutral" });
				}
				else
				{
					list.AddRange(new string[14]
					{
						"* I just...", "* I don't get it,^04 Kris.", "* I can't believe you\n  of all people would\n  just...^15 slaughter.", "* And not just that,^05\n  but take advantage of\n  me and Noelle.", "* Using our friendship\n  to make your killing\n  spree easier.", "* I SHOULD abandon you,^05\n  regardless of your\n  bullshit excuses.", "* But...^10 we've been\n  through so much that...", "* ...it feels wrong\n  leaving you like this.", "* You could get into a\n  hell of a lot of danger\n  if you keep going.", "* And I can't just sit\n  by and let that happen.",
						"* So,^05 I have to ask...", "* Why?", "* Why do this?", "* What the hell brought\n  you to this point?"
					});
					list2.AddRange(new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
					list3.AddRange(new string[14]
					{
						"su_annoyed", "su_depressed", "su_annoyed", "su_panic", "su_pissed", "su_serious", "su_dejected", "su_depressed", "su_sad", "su_dejected",
						"su_side", "su_neutral", "su_neutral", "su_side_sweat"
					});
				}
				StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
			}
			else
			{
				StartText(new string[3] { "* Kris...?", "* Yeesh,^05 dude,^05 what\n  happened to you?", "* Looks like you had\n  one hell of a\n  nightmare." }, new string[3] { "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], new string[3] { "no_shocked", "su_annoyed", "su_side" });
			}
			txt.EnableSelectionAtEnd();
			frames = 0;
			state = 1;
		}
		else if (state == 1 && (bool)txt)
		{
			if (txt.CanLoadSelection() && !selecting)
			{
				selecting = true;
				InitiateDeltaSelection();
				if (oblitVariant)
				{
					select.SetupChoice(Vector2.up, "Boredom", Vector3.zero);
					select.SetupChoice(Vector2.left, "Curiosity", Vector3.zero);
					select.SetupChoice(Vector2.right, "...", new Vector3(30f, 0f));
					select.SetupChoice(Vector2.down, "Content", Vector3.zero);
					select.Activate(this, (!aborted) ? 1 : 2, txt.gameObject);
				}
				else
				{
					select.SetupChoice(Vector2.left, "It was horrible", Vector3.zero);
					select.SetupChoice(Vector2.right, "I'm fine", new Vector3(-46f, 0f));
					select.Activate(this, 0, txt.gameObject);
				}
			}
		}
		else if (state == 2 && !txt)
		{
			frames++;
			if (frames < 90)
			{
				Vector3 vector = ((UnityEngine.Random.Range(0, 5) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
				kris.transform.position = new Vector3(-3.32f, -5.13f) + vector / 24f;
			}
			else if (frames < 100)
			{
				if (frames == 90)
				{
					ChangeDirection(kris, Vector2.left);
					SetMoveAnim(kris, true, 2f);
				}
				kris.transform.position = new Vector3(Mathf.Lerp(-3.32f, -4.166f, (float)(frames - 90) / 10f), -5.13f);
			}
			else if (frames <= 120)
			{
				if (frames == 100)
				{
					SetSprite(noelle, "spr_no_surprise_left");
					PlaySFX("sounds/snd_swallow");
					SetSprite(kris, "spr_kr_hugsusie_0");
					susie.GetComponent<SpriteRenderer>().enabled = false;
				}
				float num = (float)(frames - 100) / 20f;
				kris.transform.position = new Vector3(Mathf.Lerp(-4.166f, -4.625f, Mathf.Sin(num * (float)Math.PI * 0.5f)), -5.13f);
				if (frames == 120)
				{
					SetSprite(kris, "spr_kr_hugsusie_1");
				}
			}
			else if (frames == 140)
			{
				gm.PlayMusic("music/mus_angel");
			}
			else if (frames == 180)
			{
				StartText(new string[2] { "* K-^05KRIS?????", "* I????^10\n* DON'T DO???^10\n* HUGS???" }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_wideeye", "su_wtf" });
			}
			else if (frames == 181)
			{
				SetSprite(noelle, "spr_no_think_left_sad");
			}
			else if (frames == 205)
			{
				SetSprite(kris, "spr_kr_hugsusie_2");
			}
			else if (frames == 235)
			{
				StartText(new string[3] { "* ...^05 I mean...", "* Umm,^05 I guess it's\n  fine???", "* Uhh..." }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_concerned", "su_flustered", "su_sad" });
				state = 3;
				frames = 0;
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(2) && doorClosed)
				{
					doorClosed = false;
					SetSprite(kris, "spr_kr_hugsusie_3");
				}
				return;
			}
			frames++;
			if (frames == 20)
			{
				PlaySFX("sounds/snd_swallow");
				SetSprite(kris, "spr_kr_hugsusie_4");
			}
			else if (frames == 50)
			{
				StartText(new string[2] { "* There,^05 there...", "* ..." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_worriedsmile", "su_sad" });
			}
			else if (frames == 80)
			{
				SetSprite(noelle, "spr_no_think_left_sad");
				SetSprite(kris, "spr_kr_hugsusie_5");
			}
			else if (frames == 120)
			{
				PlaySFX("sounds/snd_wing");
				SetSprite(kris, "spr_kr_hugsusie_6");
			}
			else if (frames == 150)
			{
				StartText(new string[6]
				{
					"* Dude,^05 it's gonna be\n  fine.",
					"* Quit tearing up,^05 or\n  I'M gonna start crying\n  too.",
					"* And you don't wanna\n  see me cry.",
					"* Kris...",
					oblitVariant ? "* I'm still wondering..." : "* We should probably\n  take a minute to\n  talk about this.",
					oblitVariant ? "* What did you mean\n  by \"no control?\"" : "* An outburst like this...^05\n  It's not like you."
				}, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[1], new string[6] { "su_side", "su_annoyed", "su_smile", "no_depressed_look", "no_depressed_side", "no_depressed" });
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					susie.transform.position = new Vector3(-4.916f, -4.795f);
					susie.GetComponent<SpriteRenderer>().enabled = true;
					susie.EnableAnimator();
					SetMoveAnim(kris, false);
					gm.SetSessionFlag(6, 0);
					kris.EnableAnimator();
					ChangeDirection(kris, Vector2.right);
					kris.transform.position = new Vector3(-4.214f, -4.963f);
				}
				return;
			}
			frames++;
			if (frames == 40)
			{
				ChangeDirection(kris, Vector2.left);
				noelle.EnableAnimator();
			}
			if (frames == 80)
			{
				noelle.UseHappySprites();
				StartText(new string[3] { "* Umm...^05 sure,^05 we can\n  give you a minute.", "* We can talk about\n  this downstairs,^05 if\n  you really want to.", "* Just don't take forever\n  again,^05 okay?" }, new string[3] { "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[3] { "su_smile_side", "no_happy", "su_annoyed" });
				state = 5;
				frames = 0;
			}
		}
		else if (state == 5 && !txt)
		{
			if (MoveTo(susie, new Vector3(-9.53f, -4.93f), 4f))
			{
				ChangeDirection(susie, Vector2.left);
				SetMoveAnim(susie, true);
			}
			if (noelle.transform.position.y > -5.84f)
			{
				MoveTo(noelle, new Vector3(noelle.transform.position.x, -5.84f), 4f);
				ChangeDirection(noelle, Vector2.down);
				SetMoveAnim(noelle, true);
			}
			else if (MoveTo(noelle, new Vector3(-7.57f, -5.84f), 5f))
			{
				ChangeDirection(noelle, Vector2.left);
			}
			if (kris.transform.position.x < -3.32f)
			{
				MoveTo(kris, new Vector3(-3.32f, kris.transform.position.y), 4f);
				ChangeDirection(kris, Vector2.right);
				SetMoveAnim(kris, true);
				return;
			}
			if (MoveTo(kris, new Vector3(-3.32f, -4.73f), 4f))
			{
				ChangeDirection(kris, Vector2.up);
				return;
			}
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(kris, false);
				gm.StopMusic(60f);
			}
			if (frames == 30)
			{
				doorClosed = false;
				PlaySFX("sounds/snd_dooropen");
				SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_1");
			}
			if (frames == 60)
			{
				state = 6;
				frames = 0;
				SetMoveAnim(kris, true);
			}
		}
		else if (state == 6)
		{
			if (!doorClosed)
			{
				if (!MoveTo(cam, new Vector3(0f, 0f, -10f), 4f))
				{
					doorClosed = true;
					PlaySFX("sounds/snd_doorclose");
					SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_0");
				}
				else if (!MoveTo(kris, new Vector3(-3.32f, -2.46f), 4f))
				{
					SetMoveAnim(kris, false);
				}
				return;
			}
			frames++;
			if (frames == 90)
			{
				PlayAnimation(kris, "RemoveSoul_WalkUp");
			}
			if (frames < 120 && (UTInput.GetAxis("Horizontal") != 0f || UTInput.GetAxis("Vertical") != 0f || UTInput.GetButtonDown("Z")))
			{
				frames = 0;
				state = 7;
				PlaySFX("sounds/snd_noise");
				PlayAnimation(kris, "RemoveSoulCollapse");
			}
			if (frames == 128)
			{
				PlaySFX("sounds/snd_bump");
				gm.PlayGlobalSFX("sounds/snd_hurt");
			}
			if (frames >= 128 && frames <= 131)
			{
				int num2 = ((frames % 2 == 0) ? 1 : (-1));
				int num3 = 131 - frames;
				kris.transform.position = new Vector3(-3.32f, -2.46f) + new Vector3((float)(num3 * num2) / 24f, 0f);
			}
			if (frames == 154)
			{
				PlaySFX("sounds/snd_grab");
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
			}
			if (frames == 244)
			{
				frames = 0;
				state = 8;
				PlayAnimation(kris, "StumbleWalkUpSoul");
			}
		}
		else if (state == 7)
		{
			frames++;
			if (frames == 64)
			{
				PlaySFX("sounds/snd_grab");
				gm.PlayGlobalSFX("sounds/snd_hurt");
			}
			if (frames == 88 || frames == 100 || frames == 114)
			{
				PlaySFX("sounds/snd_bump");
			}
			if (frames == 126)
			{
				PlaySFX("sounds/snd_grab");
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
			}
			if (frames == 214)
			{
				frames = 0;
				state = 8;
				PlayAnimation(kris, "StumbleWalkUpSoul");
			}
		}
		else if (state == 8)
		{
			frames++;
			if (frames <= 150)
			{
				kris.transform.position = Vector3.Lerp(new Vector3(-3.32f, -2.46f), new Vector3(-3.74f, 0.3f), (float)frames / 150f);
				if (frames % 30 == 15)
				{
					int num4 = 2;
					if (frames % 60 == 15)
					{
						num4 = 1;
					}
					PlaySFX("sounds/snd_step" + num4);
				}
			}
			if (frames == 150)
			{
				PlayAnimation(kris, "StumbleWalkUpSoul", 0f);
			}
			if (frames == 200)
			{
				SetSprite(kris, "spr_kr_removesoul_collapse_24");
			}
			if (frames >= 215 && frames <= 230)
			{
				if (frames == 215)
				{
					SetSprite(kris, "spr_kr_removesoul_collapse_25");
					PlaySFX("sounds/snd_jump");
				}
				kris.transform.position = Vector3.Lerp(new Vector3(-3.74f, 0.3f), new Vector3(-3.89f, 2.2f), (float)(frames - 215) / 15f) + new Vector3(0f, Mathf.Sin((float)((frames - 215) * 12) * ((float)Math.PI / 180f)));
				if (frames == 230)
				{
					kris.GetComponent<SpriteRenderer>().enabled = false;
					bed.enabled = true;
					bed.SetFloat("speed", 0f);
				}
			}
			if (frames == 245)
			{
				bed.SetFloat("speed", 1f);
			}
			if (frames == 270)
			{
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 320)
			{
				PlaySFX("sounds/snd_wing");
				bed.enabled = false;
				SetSprite(bed, "overworld/snow_objects/spr_bunny_singlebed_nocover_putsoulin_8");
			}
			if (frames == 360)
			{
				PlaySFX("sounds/snd_heavyswing");
				SetSprite(bed, "overworld/snow_objects/spr_bunny_singlebed_soul");
				SetSprite(kris, "spr_kr_hop_down");
				kris.GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames >= 360 && frames <= 368)
			{
				kris.transform.position = Vector3.Lerp(new Vector3(-3.57f, 1.85f), new Vector3(-2.6f, 1.19f), (float)(frames - 360) / 8f);
				if (frames == 368)
				{
					gm.PlayGlobalSFX("sounds/snd_noise");
					SetSprite(kris, "spr_kr_removesoul_collapse_3", true);
				}
			}
			if (frames == 420)
			{
				PlaySFX("sounds/snd_step1");
				SetSprite(kris, "spr_kr_stumblewalk_down_0", true);
			}
			if (frames >= 480 && frames <= 660)
			{
				if (frames == 480)
				{
					PlayAnimation(kris, "StumbleWalkDown");
					kris.GetComponent<SpriteRenderer>().flipX = false;
				}
				kris.transform.position = Vector3.Lerp(new Vector3(-2.6f, 1.19f), new Vector3(-3.32f, -2.46f), (float)(frames - 480) / 180f);
				if (frames % 30 == 15)
				{
					int num5 = 2;
					if (frames % 60 == 15)
					{
						num5 = 1;
					}
					PlaySFX("sounds/snd_step" + num5);
				}
				if (frames == 660)
				{
					PlayAnimation(kris, "StumbleWalkDown", 0f);
				}
			}
			if (frames == 700)
			{
				PlayAnimation(kris, "HairFlipZombie");
			}
			if (frames == 715)
			{
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 745)
			{
				gm.SetFlag(0, "eye");
				gm.SetFlag(204, 1);
				PlayerPrefs.SetInt("KrisEye", 1);
				SpriteRenderer component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/lostcore/EyeFlash"), new Vector3(-3.4032f, -2.1054f), Quaternion.identity, base.transform).GetComponent<SpriteRenderer>();
				component.color = Color.red;
				component.sortingOrder = 460;
			}
			if (frames == 775)
			{
				PlaySFX("sounds/snd_dooropen");
				SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_1");
			}
			if (frames == 795)
			{
				frames = 0;
				state = 9;
				doorClosed = false;
				ChangeDirection(kris, Vector2.down);
				PlayAnimation(kris, "walk");
				SetMoveAnim(kris, true, 0.75f);
			}
		}
		else if (state == 9)
		{
			if (!doorClosed && !MoveTo(kris, new Vector3(-3.32f, -4.96f), 6f))
			{
				ChangeDirection(kris, Vector3.left);
				doorClosed = true;
				PlaySFX("sounds/snd_doorclose");
				SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_0");
			}
			else if (doorClosed && !MoveTo(kris, new Vector3(-8.27f, -4.96f), 6f))
			{
				frames++;
				if (frames == 90)
				{
					CutsceneHandler.GetCutscene(75).StartCutscene();
					EndCutscene(false);
				}
			}
		}
		else if (state == 10)
		{
			if ((bool)txt && abortOffset != -1)
			{
				if (reasonChoice == 1)
				{
					if (AtLine(1))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(3))
					{
						SetSprite(susie, "spr_su_throw_ready");
					}
					else if (AtLine(4))
					{
						susie.EnableAnimator();
					}
				}
				else if (reasonChoice == 2)
				{
					if (AtLine(1))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(2))
					{
						SetSprite(susie, "spr_su_throw_ready");
					}
					else if (AtLine(4))
					{
						susie.EnableAnimator();
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
					}
				}
				else if (reasonChoice == 3)
				{
					if (AtLine(1))
					{
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(3))
					{
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
					}
				}
				if (abortOffset > 2 && AtLine(-1 + abortOffset))
				{
					SetSprite(noelle, "spr_no_think_left_sad");
				}
				if (AtLine(2 + abortOffset) || AtLine(7 + abortOffset))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(4 + abortOffset))
				{
					PlayAnimation(susie, "Embarrassed");
				}
				else if (AtLine(5 + abortOffset))
				{
					PlayAnimation(susie, "idle");
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(8 + abortOffset))
				{
					ChangeDirection(susie, Vector2.right);
				}
			}
			else
			{
				if ((bool)txt)
				{
					return;
				}
				frames++;
				if (frames == 20)
				{
					SetSprite(susie, "spr_su_hugready");
					abortOffset = -1;
				}
				if (frames == 40)
				{
					StartText(new string[2] { "* I guess we can\n  hug on this?", "* The end of merciless\n  slaughter,^05 I guess." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_smile_side", "su_dejected" });
				}
				if (frames < 50 && frames > 40)
				{
					if (frames == 41)
					{
						ChangeDirection(kris, Vector2.left);
						SetMoveAnim(kris, true, 2f);
					}
					kris.transform.position = new Vector3(Mathf.Lerp(-3.32f, -4.166f, (float)(frames - 40) / 10f), -5.13f);
				}
				else if (frames <= 70 && frames >= 50)
				{
					if (frames == 50)
					{
						SetSprite(noelle, "spr_no_left_shocked_0");
						PlaySFX("sounds/snd_swallow");
						SetSprite(kris, "spr_kr_hugsusie_0");
						susie.GetComponent<SpriteRenderer>().enabled = false;
					}
					float num6 = (float)(frames - 50) / 20f;
					kris.transform.position = new Vector3(Mathf.Lerp(-4.166f, -4.625f, Mathf.Sin(num6 * (float)Math.PI * 0.5f)), -5.13f);
					if (frames == 70)
					{
						SetSprite(kris, "spr_kr_hugsusie_1");
					}
				}
				if (frames == 110)
				{
					PlaySFX("sounds/snd_swallow");
					SetSprite(kris, "spr_kr_hugsusie_4");
				}
				if (frames == 125)
				{
					doorClosed = false;
					StartText(new string[2] { "* Okay,^05 dude,^05 chill out\n  at least a LITTLE.", "* ..." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_worriedsmile", "su_sad" });
					state = 3;
					frames = 50;
				}
			}
		}
		else if (state == 11)
		{
			if ((bool)txt)
			{
				if (reasonChoice == 1)
				{
					if (AtLine(1))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(3))
					{
						SetSprite(susie, "spr_su_throw_ready");
					}
					else if (AtLine(4))
					{
						susie.EnableAnimator();
						SetSprite(kris, "spr_kr_up_0");
						ChangeDirection(kris, Vector2.up);
					}
				}
				else if (reasonChoice == 2)
				{
					if (AtLine(1))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(2))
					{
						SetSprite(susie, "spr_su_throw_ready");
					}
					else if (AtLine(4))
					{
						susie.EnableAnimator();
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
						SetSprite(kris, "spr_kr_up_0");
						ChangeDirection(kris, Vector2.up);
					}
				}
				else if (reasonChoice == 3)
				{
					if (AtLine(1))
					{
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(3))
					{
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
						SetSprite(kris, "spr_kr_up_0");
						ChangeDirection(kris, Vector2.up);
					}
				}
				else if (AtLine(5))
				{
					SetSprite(kris, "spr_kr_up_0");
					ChangeDirection(kris, Vector2.up);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.EnableAnimator();
				}
				Vector3 vector2 = ((UnityEngine.Random.Range(0, 10) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
				kris.transform.position = new Vector3(-3.32f, -5.13f) + vector2 / 24f;
				if (frames == 45)
				{
					SetSprite(noelle, "spr_no_think_left_sad");
				}
				if (frames == 60)
				{
					StartText(new string[7] { "* ... Kris?", "* Are you...?", "* Kris,^05 now's not the\n  time to be moping.", "* Murderers don't get to\n  mope about the killing\n  they did.", "* Be glad we aren't\n  killing YOU.", "* SUSIE!!!^05\n* Don't say that!", "* Well it's true!!!" }, new string[7] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[7] { "no_afraid", "no_afraid", "su_annoyed", "su_side", "su_annoyed", "no_angry", "su_angry" });
					frames = 0;
					state = 12;
				}
			}
		}
		else if (state == 12)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(5))
				{
					ChangeDirection(susie, Vector2.right);
					SetSprite(susie, "spr_su_throw_ready");
					SetSprite(noelle, "spr_no_surprise_left");
				}
				else if (AtLine(6))
				{
					SetSprite(noelle, "pissed/spr_no_right_0_pissed", true);
					SetSprite(susie, "spr_su_surprise_right");
				}
				else if (AtLine(7))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				int max = ((txt.GetCurrentStringNum() >= 5) ? 6 : 10);
				Vector3 vector3 = ((UnityEngine.Random.Range(0, max) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
				kris.transform.position = new Vector3(-3.32f, -5.13f) + vector3 / 24f;
				return;
			}
			frames++;
			if (frames == 1)
			{
				susie.EnableAnimator();
				SetSprite(noelle, "spr_no_left_shocked_0");
				SetSprite(kris, "spr_kr_balled");
				PlaySFX("sounds/snd_noise");
			}
			Vector3 vector4 = ((UnityEngine.Random.Range(0, 6) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
			kris.transform.position = new Vector3(-3.32f, -5.13f) + vector4 / 24f;
			if (frames == 30)
			{
				SetSprite(noelle, "spr_no_think_left_sad");
			}
			if (frames == 70)
			{
				SetSprite(susie, "spr_su_left_worried_0", true);
				StartText(new string[3] { "* ...", "* Kris...^10 I...", "* ..." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[3] { "su_concerned", "su_sad", "su_sad_tears" });
				frames = 0;
				state = 13;
			}
		}
		else if (state == 13)
		{
			Vector3 vector5 = ((UnityEngine.Random.Range(0, 6) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
			kris.transform.position = new Vector3(-3.32f, -5.13f) + vector5 / 24f;
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					SetSprite(susie, "spr_su_up_0");
				}
				return;
			}
			frames++;
			int max2 = ((frames >= 90) ? 10 : 20);
			vector5 = ((UnityEngine.Random.Range(0, max2) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
			susie.transform.position = new Vector3(-4.61f, -4.93f) + vector5 / 24f;
			if (frames == 90)
			{
				SetSprite(susie, "spr_su_kneel");
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 120)
			{
				StartText(new string[9] { "* God damn it dude,^08\n  stop crying...", "* Kris,^08 we killed a\n  lot of people...", "* What the hell do\n  we do???", "* Kris...^05 Susie...", "* Maybe...^05 we should talk\n  about that control thing.", "* That seems to be\n  the thing we should\n  deal with first.", "* ...", "* I...^05 guess so.", "* If that's cool with\n  you,^05 Kris." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[4] { 1, 1, 1, 0 }, new string[9] { "su_crying", "su_crying", "su_crying", "no_sad", "no_depressed", "no_depressed_side", "su_depressed_eyeliner", "su_cried", "su_cried_look_away" });
				txt.gameObject.AddComponent<ShakingText>().StartShake(5);
				state = 14;
				frames = 0;
			}
		}
		else if (state == 14)
		{
			if ((bool)txt)
			{
				int max3 = ((txt.GetCurrentStringNum() >= 5) ? 10 : 6);
				int num7 = 0;
				if (txt.GetCurrentStringNum() < 5)
				{
					num7 = 10;
				}
				else if (txt.GetCurrentStringNum() < 7)
				{
					num7 = 20;
				}
				Vector3 vector6 = Vector3.zero;
				if (num7 > 0)
				{
					vector6 = ((UnityEngine.Random.Range(0, num7) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
				}
				susie.transform.position = new Vector3(-4.61f, -4.93f) + vector6 / 24f;
				vector6 = ((UnityEngine.Random.Range(0, max3) == 0) ? new Vector3((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1), 0f) : Vector3.zero);
				kris.transform.position = new Vector3(-3.32f, -5.13f) + vector6 / 24f;
				if (AtLine(4))
				{
					txt.GetComponent<ShakingText>().Stop();
				}
				else if (AtLine(7))
				{
					txt.GetComponent<ShakingText>().StartShake(50);
				}
				else if (AtLine(9))
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(susie, "spr_su_left_worried_0", true);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				kris.transform.position = new Vector3(-3.32f, -5.13f);
			}
			if (frames == 45)
			{
				PlaySFX("sounds/snd_wing");
				kris.EnableAnimator();
			}
			if (frames == 90)
			{
				gm.SetSessionFlag(6, 0);
			}
			if (frames == 130)
			{
				ChangeDirection(kris, Vector2.right);
			}
			if (frames == 190)
			{
				noelle.EnableAnimator();
				susie.EnableAnimator();
				susie.GetComponent<SpriteRenderer>().flipX = false;
				StartText(new string[7] { "* Talk about it\n  downstairs?", "* Yeah,^05 we can meet\n  you downstairs to talk\n  about this...", "* If that makes you\n  more comfortable.", "* Kris,^05 you better not\n  climb out the window\n  or anything.", "* I'm not gonna let\n  you get away after\n  making me cry.", "* ...", "* See ya in a sec." }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[7] { "no_thinking", "no_neutral", "no_happy", "su_pissed_eyeliner", "su_pissed_eyeliner", "su_depressed_eyeliner", "su_depressed_eyeliner" });
				state = 15;
				frames = 0;
			}
		}
		else
		{
			if (state != 15)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					SetSprite(susie, "spr_su_throw_ready");
					ChangeDirection(kris, Vector2.left);
				}
				else if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.up);
					susie.EnableAnimator();
				}
			}
			else
			{
				ChangeDirection(kris, Vector2.up);
				state = 5;
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selecting = false;
		if (id == 0)
		{
			if (index == Vector2.left)
			{
				ChangeDirection(kris, Vector2.left);
				StartText(new string[2] { "* ...", "* Well...^10 do you wanna\n  talk about it or\n  something?" }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_side", "su_smirk_sweat" });
			}
			else
			{
				ChangeDirection(kris, Vector2.up);
				StartText(new string[3] { "* ... Kris,^05 I'm not an\n  idiot.", "* I can SEE how\n  upset you are.", "* The hell happened?" }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_annoyed", "su_annoyed", "su_side" });
			}
			state = 2;
			return;
		}
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		if (index == Vector2.left)
		{
			gm.SetFlag(218, 1);
			reasonChoice = 1;
			list.AddRange(new string[4] { "* Curiosity???", "* Dude,^05 is traveling the\n  multiverse not cool\n  enough???", "* You have to add blood\n  and guts to that list??", "* Is this a game\n  to you or something?" });
			list2.AddRange(new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
			list3.AddRange(new string[4] { "su_panic", "su_pissed", "su_pissed", "su_annoyed" });
		}
		else if (index == Vector2.up)
		{
			gm.SetFlag(218, 2);
			reasonChoice = 2;
			list.AddRange(new string[6] { "* What do you mean\n  you were BORED???", "* We're seeing a bunch\n  of weird places.", "* Meeting a bunch of\n  weird people.", "* Some of 'em are a\n  bit lame,^05 but still!!!", "* What,^05 is this all like...\n^10  a game to you?", "* That you wanna spice\n  it up with blood\n  and guts?" });
			list2.AddRange(new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
			list3.AddRange(new string[6] { "su_panic", "su_pissed", "su_pissed", "su_sus", "su_annoyed", "su_side" });
		}
		else if (index == Vector2.down)
		{
			gm.SetFlag(218, GameManager.UsingRecordingSoftware() ? 4 : 3);
			reasonChoice = 3;
			SetSprite(noelle, "spr_no_left_shocked_0");
			list.AddRange(new string[5] { "* ...", "* None of us are\n  even holding a camera.", "* That is like,^05 actually\n  fucking weird.", "* Is this some kind\n  of a game to you\n  or something?", "* Cuz that is NOT funny." });
			list2.AddRange(new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
			list3.AddRange(new string[5] { "su_inquisitive", "su_inquisitive", "su_disappointed", "su_disappointed", "su_disappointed" });
		}
		else
		{
			SetSprite(noelle, "spr_no_think_left_sad");
			if (aborted)
			{
				list.AddRange(new string[2] { "* ...", "* Eh,^05 whatever,^05 I guess..." });
				list2.AddRange(new string[1] { "snd_txtsus" });
				list3.AddRange(new string[2] { "su_depressed", "su_dejected" });
			}
			else
			{
				list.AddRange(new string[5] { "* Kris...", "* You can't just not\n  say anything.", "* I know you want\n  to say something.", "* This is serious,^05 dude.", "* Is this a game to\n  you or something?" });
				list2.AddRange(new string[5] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" });
				list3.AddRange(new string[5] { "no_depressed", "no_depressed_look", "no_depressed_side", "su_annoyed", "su_disappointed" });
			}
		}
		if (aborted)
		{
			if (index != Vector2.right)
			{
				list.AddRange(new string[2] { "* Susie...", "* ...^05 Sorry." });
				list2.AddRange(new string[2] { "snd_txtnoe", "snd_txtsus" });
				list3.AddRange(new string[2] { "no_depressed_look", "su_sad" });
			}
			abortOffset = list.Count;
			list.AddRange(new string[9] { "* At this point,^05 though,^05\n  it doesn't really matter\n  why.", "* What we've done is\n  in the past now.", "* No real way of\n  going back to fix\n  it...", "* Or if there is,^05\n  it isn't showing up.", "* Either way,^05 I'd rather\n  encourage you to kill\n  LESS people than more.", "* So I'll quit pushing\n  you on this,^05 I guess.", "* ...", "* I can tell you're\n  upset over this whole\n  thing,^05 so...^10 uhh...", "* ..." });
			list2.AddRange(new string[1] { "snd_txtsus" });
			list3.AddRange(new string[9] { "su_dejected", "su_neutral", "su_dejected", "su_smirk_sweat", "su_annoyed", "su_side", "su_neutral", "su_dejected", "su_sad" });
		}
		StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
		frames = 0;
		state = (aborted ? 10 : 11);
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		bed = GameObject.Find("Bed").GetComponent<Animator>();
		if ((int)gm.GetFlag(87) >= 4 && WeirdChecker.HasCommittedBloodshed(gm))
		{
			oblitVariant = true;
			aborted = (int)gm.GetFlag(12) == 0;
			if ((int)gm.GetFlag(87) != 8 && aborted)
			{
				susieKnowsOfAbort = true;
			}
		}
		SetMoveAnim(kris, false);
		ChangeDirection(kris, Vector2.down);
		PlaySFX("sounds/snd_dooropen");
		SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_1");
	}
}
