using System;
using System.Collections.Generic;
using UnityEngine;

public class PorkyDefeatCutscene : CutsceneBase
{
	private SpriteRenderer greyDoor;

	private Animator paula;

	private Animator ness;

	private float velocity;

	private SpriteRenderer krisBW;

	private SpriteRenderer susieBW;

	private SpriteRenderer noelleBW;

	private int ki;

	private int si;

	private int ni;

	private bool abortedOblit;

	private bool kill;

	private bool snakeReaction;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 45)
			{
				kris.ChangeDirection(Vector2.down);
				noelle.ChangeDirection(Vector2.up);
				if (!abortedOblit)
				{
					paula.SetFloat("dirX", 0f);
					paula.SetFloat("dirY", -1f);
				}
				if (!kill)
				{
					ness.SetFloat("dirX", 0f);
					ness.SetFloat("dirY", -1f);
				}
				if (kill)
				{
					if (abortedOblit)
					{
						StartText(new string[3] { "* ... I feel like...", "* I'd be more affected\n  by this if we didn't\n  like...", "* Do much worse." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[5], new string[3] { "su_dejected", "su_dejected", "su_depressed" });
					}
					else
					{
						StartText(new string[6] { "* What the hell is\n  up with these people?", "* Why did he like...^05\n  challenge us like\n  that...?", "* This is the second\n  time this happened.", "* Don't worry about it\n  too much.", "* It's just a robot.", "* ...^05 Alright." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtsus" }, new int[5], new string[6] { "su_side", "su_dejected", "su_depressed", "pau_embarrassed", "pau_embarrassed", "su_dejected" });
					}
				}
				else if (abortedOblit)
				{
					StartText(new string[5] { "* What the hell is\n  up with these guys???", "* This is the second\n  time this happened.", "* That's so weird...", "* What was Porky doing\n  in there...?", "* Sorry if I'm being\n  ^05rude,^05 but..." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtness", "snd_txtness", "snd_txtnoe" }, new int[5], new string[5] { "su_pissed", "su_annoyed", "ness_neutral", "ness_confused", "no_happy" });
				}
				else
				{
					StartText(new string[4] { "* What the hell is\n  up with these guys???", "* This is the second\n  time this happened.", "* That's so weird...", "* What was Porky doing\n  in there...?" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtness", "snd_txtness" }, new int[5], new string[4] { "su_pissed", "su_annoyed", "ness_neutral", "ness_confused" });
				}
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (kill)
			{
				if (frames == 1)
				{
					ness.SetBool("isMoving", true);
				}
				else if (frames == 45)
				{
					ness.SetBool("isMoving", false);
				}
				ness.transform.position = new Vector3(Mathf.Lerp(3.26f, 5.33f, (float)frames / 45f), -5.88f);
				cam.transform.position = new Vector3(Mathf.Lerp(10.32f, 8.3f, (float)frames / 45f), -6.75f, -10f);
			}
			if (!abortedOblit)
			{
				if (!kill)
				{
					if (frames == 1)
					{
						paula.SetFloat("dirX", 1f);
						paula.SetFloat("dirY", 0f);
						paula.SetBool("isMoving", true);
					}
					paula.GetComponent<SpriteRenderer>().sortingOrder = 29;
					if (paula.transform.position.x != 10.83f)
					{
						paula.transform.position = new Vector3(Mathf.MoveTowards(paula.transform.position.x, 10.83f, 0.125f), -5.25f);
					}
					else if (paula.transform.position.y != -4.87f)
					{
						paula.transform.position = new Vector3(10.83f, Mathf.MoveTowards(paula.transform.position.y, -4.87f, 0.125f));
					}
					else
					{
						paula.SetFloat("dirX", -1f);
						paula.SetFloat("dirY", 0f);
						ness.SetFloat("dirX", 1f);
						ness.SetFloat("dirY", 0f);
						paula.SetBool("isMoving", false);
						paula.enabled = false;
						paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_happy");
						paula.GetComponent<SpriteRenderer>().sortingOrder = 25;
						susie.ChangeDirection(Vector2.up);
						noelle.ChangeDirection(Vector2.up);
						kris.ChangeDirection(Vector2.right);
						StartText(new string[6] { "* Wow,^05 Ness,^05 that was\n  really cool!", "* Umm...^05 I'm Paula,^05 by\n  the way.", "* Oh!\n^05* I've been trying to\n  look for you!", "* Were you helping these\n  guys get here?", "* Yeah,^05 they were looking\n  for the grey door\n  here!", "* Oh,^05 so that's why\n  I was hearing about\n  that." }, new string[6] { "snd_txtpau", "snd_txtpau", "snd_txtness", "snd_txtness", "snd_txtpau", "snd_txtness" }, new int[5], new string[6] { "pau_happy", "pau_smile", "ness_surprised", "ness_neutral", "pau_smile", "ness_confused" });
						state = 2;
						frames = 0;
					}
				}
				else if (frames == 55 && kill)
				{
					susie.ChangeDirection(Vector2.left);
					noelle.ChangeDirection(Vector2.left);
					kris.ChangeDirection(Vector2.left);
					paula.enabled = false;
					paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_shocked");
					StartText(new string[10] { "* I thought I heard\n  Porky over here.", "* Did you guys take\n  care of him?", "* ... Is that...?", "* Are you Ness?", "* Yeah,^05 that's me!", "* I'm trying to find\n  someone named Paula.", "* Hey,^05 that's also\n  me!", "* I thought I was\n  gonna have to find\n  you!", "* I guess these three\n  took care of\n  everything?", "* Yeah,^05 and I helped\n  them get here,^05 too!" }, new string[10] { "snd_txtness", "snd_txtness", "snd_txtpau", "snd_txtpau", "snd_txtness", "snd_txtness", "snd_txtpau", "snd_txtpau", "snd_txtness", "snd_txtpau" }, new int[5], new string[10] { "ness_confused", "ness_neutral", "pau_confused", "pau_surprised", "ness_smile", "ness_neutral", "pau_happy", "pau_smile", "ness_smile", "pau_happy" });
					state = 2;
					frames = 0;
				}
			}
			if (abortedOblit && ((frames == 1 && !kill) || (frames == 55 && kill)))
			{
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				if (kill)
				{
					susie.ChangeDirection(Vector2.left);
					noelle.ChangeDirection(Vector2.left);
					kris.ChangeDirection(Vector2.left);
					susie.DisableAnimator();
					susie.SetSprite("spr_su_freaked");
					list.AddRange(new string[3] { "* I thought I heard\n  Porky over here.", "* Did you guys take\n  care of him?", "* Oh,^05 uhh,^05 yeah!" });
					list2.AddRange(new string[3] { "snd_txtness", "snd_txtness", "snd_txtsus" });
					list3.AddRange(new string[3] { "ness_confused", "ness_neutral", "su_excited" });
				}
				else
				{
					susie.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
					kris.ChangeDirection(Vector2.right);
				}
				list.AddRange(new string[14]
				{
					"* Who are you?", "* Oh!\n^05* My name is Ness.", "* I heard there were\n  killings happening over\n  here...", "* So I came to check\n  over here.", "* It seems like you\n  guys already took care\n  of it.", "* Huh...?", "* Well,^05 some people\n  thought a giant monster\n  did it.", "* And I heard a huge,^05\n  monster mole guarded\n  this place.", "* So I guess that it\n  did the killing.", "* You guys took care\n  of it,^05 did you?",
					"* And by proxy,^05 you\n  guys also freed Paula,^05\n  right?", "* Oh,^05 yeah,^05 right,^05\n  yeah!", "* We,^05 uhh...^05 did.", "* That's really cool."
				});
				list2.AddRange(new string[14]
				{
					"snd_txtnoe", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtsus", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness",
					"snd_txtness", "snd_txtsus", "snd_txtsus", "snd_txtness"
				});
				list3.AddRange(new string[14]
				{
					"no_confused", "ness_neutral", "ness_neutral", "ness_neutral", "ness_neutral", "su_side_sweat", "ness_confused", "ness_neutral", "ness_confused", "ness_neutral",
					"ness_neutral", "su_surprised", "su_smirk_sweat", "ness_fuzzy"
				});
				StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
				state = 2;
				frames = 0;
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (abortedOblit)
				{
					int num = txt.GetCurrentStringNum() - (kill ? 3 : 0);
					if (num == 0 && !susie.GetComponent<Animator>().enabled)
					{
						susie.EnableAnimator();
					}
					else if ((num == 2 && kill) || (num == 6 && !kill))
					{
						ness.SetFloat("dirX", 0f);
						ness.SetFloat("dirY", -1f);
					}
					else if ((num == 4 || num == 6) && kill)
					{
						ness.SetFloat("dirX", 1f);
						ness.SetFloat("dirY", 0f);
					}
					else if ((num == 5 || num == 10) && !kill)
					{
						ness.SetFloat("dirX", -1f);
						ness.SetFloat("dirY", 0f);
					}
				}
				else if (kill)
				{
					if (txt.GetCurrentStringNum() >= 3 && txt.GetCurrentStringNum() < 7)
					{
						if (!paula.enabled)
						{
							paula.enabled = true;
							paula.SetFloat("dirX", -1f);
							paula.SetFloat("dirY", 0f);
							paula.SetBool("isMoving", true);
						}
						if (paula.transform.position.x != 5.31f)
						{
							paula.transform.position = new Vector3(Mathf.MoveTowards(paula.transform.position.x, 5.31f, 0.125f), -5.25f);
						}
						else
						{
							paula.SetFloat("dirX", 0f);
							paula.SetFloat("dirY", -1f);
							paula.SetBool("isMoving", false);
						}
					}
					if (txt.GetCurrentStringNum() == 3 || txt.GetCurrentStringNum() == 6)
					{
						ness.SetFloat("dirX", 0f);
						ness.SetFloat("dirY", 1f);
					}
					else if (txt.GetCurrentStringNum() == 5)
					{
						ness.SetFloat("dirX", 0f);
						ness.SetFloat("dirY", -1f);
					}
					else if (txt.GetCurrentStringNum() == 7 && paula.enabled)
					{
						paula.enabled = false;
						paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_happy");
					}
					else if (txt.GetCurrentStringNum() == 8 && !paula.enabled)
					{
						paula.enabled = true;
					}
					else if (txt.GetCurrentStringNum() == 9)
					{
						ness.SetFloat("dirX", 1f);
						ness.SetFloat("dirY", 0f);
					}
					else if (txt.GetCurrentStringNum() == 10)
					{
						paula.SetFloat("dirX", 1f);
						paula.SetFloat("dirY", 0f);
					}
				}
				else if (txt.GetCurrentStringNum() == 2 && !paula.enabled)
				{
					paula.enabled = true;
				}
				else if (txt.GetCurrentStringNum() == 4)
				{
					ness.SetFloat("dirX", -1f);
					ness.SetFloat("dirY", 0f);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gm.PlayMusic("music/mus_sanctuary_post_intro");
					if (!kill)
					{
						ness.SetFloat("dirX", 0f);
						ness.SetFloat("dirY", -1f);
						paula.SetFloat("dirX", 0f);
						paula.SetFloat("dirY", -1f);
					}
					else
					{
						kris.ChangeDirection(Vector2.down);
						susie.ChangeDirection(Vector2.up);
						noelle.ChangeDirection(Vector2.up);
					}
					ness.SetBool("isMoving", true);
					paula.SetBool("isMoving", false);
				}
				ness.transform.position = Vector3.Lerp(kill ? new Vector3(5.33f, -5.88f) : new Vector3(9.75f, -4.87f), new Vector3(10.97f, -5.88f), (float)frames / (kill ? 60f : 30f));
				cam.transform.position = new Vector3(Mathf.Lerp(8.3f, 10.32f, (float)(kill ? frames : 60) / 60f), -6.75f, -10f);
				if (frames == (kill ? 60 : 30))
				{
					ness.SetFloat("dirX", -1f);
					ness.SetFloat("dirY", 0f);
					ness.SetBool("isMoving", false);
				}
				if (frames == (kill ? 75 : 45))
				{
					if (abortedOblit)
					{
						kris.ChangeDirection(Vector2.right);
						susie.ChangeDirection(Vector2.right);
						noelle.ChangeDirection(Vector2.right);
						susie.UseHappySprites();
						noelle.UseHappySprites();
						StartText(new string[8] { "* Also,^05 you guys helped\n  me get closer to\n  defeating Giygas.", "* Now I can absorb\n  the power of this\n  place.", "* Hey,^05 we're not called\n  the <color=#FFFF00FF>DELTA WARRIORS</color> for\n  nothing.", "* No we aren't!", "* (Just go with it!!!)", "* Well,^05 we might as\n  well get going.", "* Good luck with your\n  adventure,^05 dude.", "* Thanks a lot!" }, new string[8] { "snd_txtness", "snd_txtness", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtness" }, new int[5], new string[8] { "ness_smile", "ness_fuzzy", "su_confident", "no_playful", "su_angry", "su_side", "su_smile", "ness_fuzzy" });
					}
					else
					{
						kris.ChangeDirection(Vector2.right);
						susie.ChangeDirection(Vector2.right);
						noelle.ChangeDirection(Vector2.right);
						susie.UseHappySprites();
						noelle.UseHappySprites();
						List<string> list4 = new List<string> { "* Hey,^05 you guys helped\n  me get closer to\n  defeating Giygas." };
						List<string> list5 = new List<string> { "snd_txtness" };
						List<string> list6 = new List<string> { "ness_smile" };
						if ((int)gm.GetFlag(87) == 4 && (int)gm.GetFlag(89) == 1)
						{
							snakeReaction = true;
							list4.AddRange(new string[5] { "* I thought there\n  was gonna be an\n  issue...", "* Because I heard a\n  snake was slit open\n  around here.", "* There was a WHAT???", "* Not that it matters\n  now,^05 because these\n  three handled it.", "* Uhh...^10 yeah..." });
							list5.AddRange(new string[5] { "snd_txtness", "snd_txtness", "snd_txtpau", "snd_txtness", "snd_txtnoe" });
							list6.AddRange(new string[5] { "ness_confused", "ness_confused", "pau_wideeye", "ness_neutral", "no_weird" });
						}
						else
						{
							list4.AddRange(new string[2] { "* It seems like you\n  all took care of\n  everything.", "* Now I can absorb\n  the power of this\n  place." });
							list5.AddRange(new string[2] { "snd_txtness", "snd_txtness" });
							list6.AddRange(new string[2] { "ness_smile", "ness_fuzzy" });
						}
						list4.AddRange(new string[7] { "* Hey,^05 we're not called\n  the DELTA WARRIORS for\n  nothing.", "* No we aren't!", "* (Just go with it!!!)", "* Well,^05 we might as\n  well get going.", "* Good luck with your\n  adventure,^05 you two.", "* Thanks a lot!", "* Good luck getting\n  home!" });
						list5.AddRange(new string[7] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtness", "snd_txtpau" });
						list6.AddRange(new string[7] { "su_confident", "no_playful", "su_angry", "su_side", "su_smile", "ness_fuzzy", "pau_happy" });
						StartText(list4.ToArray(), list5.ToArray(), new int[1], list6.ToArray());
					}
					state = 3;
					frames = 0;
				}
			}
		}
		if (state == 3)
		{
			if ((bool)txt)
			{
				if (abortedOblit)
				{
					if (txt.GetCurrentStringNum() == 3 && susie.GetComponent<Animator>().enabled)
					{
						susie.DisableAnimator();
						susie.SetSprite("spr_su_pose");
					}
					else if (txt.GetCurrentStringNum() == 4)
					{
						kris.ChangeDirection(Vector2.down);
						noelle.ChangeDirection(Vector2.up);
					}
					else if (txt.GetCurrentStringNum() == 5 && !susie.GetComponent<Animator>().enabled)
					{
						susie.EnableAnimator();
						susie.ChangeDirection(Vector2.down);
					}
					else if (txt.GetCurrentStringNum() == 6)
					{
						kris.ChangeDirection(Vector2.right);
						susie.ChangeDirection(Vector2.right);
						noelle.ChangeDirection(Vector2.right);
					}
				}
				else
				{
					int num2 = txt.GetCurrentStringNum() - ((!snakeReaction) ? 1 : 4);
					if (num2 == 0 && paula.enabled && snakeReaction)
					{
						paula.enabled = false;
						paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_shocked");
						paula.GetComponent<SpriteRenderer>().flipX = true;
					}
					else if (num2 == 2 && !paula.enabled)
					{
						paula.enabled = true;
						paula.GetComponent<SpriteRenderer>().flipX = false;
						noelle.UseUnhappySprites();
					}
					else if (num2 == 3 && susie.GetComponent<Animator>().enabled)
					{
						susie.DisableAnimator();
						susie.SetSprite("spr_su_pose");
					}
					else if (num2 == 4)
					{
						kris.ChangeDirection(Vector2.down);
						noelle.ChangeDirection(Vector2.up);
					}
					else if (num2 == 5 && !susie.GetComponent<Animator>().enabled)
					{
						susie.EnableAnimator();
						susie.ChangeDirection(Vector2.down);
						noelle.UseHappySprites();
					}
					else if (num2 == 6)
					{
						kris.ChangeDirection(Vector2.right);
						susie.ChangeDirection(Vector2.right);
						noelle.ChangeDirection(Vector2.right);
					}
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					ness.SetBool("isMoving", true);
					if (!abortedOblit)
					{
						paula.enabled = true;
						paula.SetFloat("dirX", -1f);
						paula.SetFloat("dirY", 0f);
						paula.SetBool("isMoving", true);
					}
				}
				if (frames == 30)
				{
					kris.ChangeDirection(Vector2.down);
					susie.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
					StartText(new string[2] { "* Kris,^05 Noelle.", "* Let's get outta here." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[5], new string[2] { "su_confident", "su_smile" });
					state = 4;
					frames = 0;
				}
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic(45f);
				kris.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				noelle.GetComponent<Animator>().SetBool("isMoving", true);
				kris.ChangeDirection(Vector2.right);
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
			}
			bool flag = true;
			if (kris.transform.position != new Vector3(11.34f, -4.22f))
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(11.34f, -4.22f), 5f / 48f);
				flag = false;
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", false);
				kris.ChangeDirection(Vector2.up);
			}
			if (susie.transform.position != new Vector3(14.07f, -4.02f))
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(14.07f, -4.02f), 5f / 48f);
				flag = false;
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.ChangeDirection(Vector2.up);
			}
			if (noelle.transform.position != new Vector3(12.67f, -4.21f))
			{
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(12.67f, -4.21f), 5f / 48f);
				flag = false;
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				noelle.ChangeDirection(Vector2.up);
			}
			if (cam.transform.position != new Vector3(12.62f, -3.08f, -10f))
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(12.62f, -3.08f, -10f), 1f / 12f);
				flag = false;
			}
			if (flag)
			{
				state = 5;
				frames = 0;
			}
		}
		if (state == 5)
		{
			frames++;
			if (frames == 20)
			{
				GameObject.Find("NatureSounds").GetComponent<AudioSource>().Stop();
				greyDoor.sprite = Resources.Load<Sprite>("overworld/spr_grey_door_1");
				PlaySFX("sounds/snd_elecdoor_shutheavy");
			}
			if (frames >= 45 && frames <= 65)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 0f);
				kris.GetComponent<Animator>().Play("RunUp", 0, 0f);
				susie.GetComponent<Animator>().SetFloat("speed", 0f);
				susie.GetComponent<Animator>().Play("RunUp", 0, 0f);
				kris.transform.position = Vector3.Lerp(new Vector3(11.34f, -4.22f), new Vector3(11.34f, -5.08f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
				susie.transform.position = Vector3.Lerp(new Vector3(14.07f, -4.02f), new Vector3(14.07f, -4.88f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
				noelle.transform.position = Vector3.Lerp(new Vector3(12.67f, -4.21f), new Vector3(12.67f, -5.07f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
			}
			else if (frames >= 65 && frames <= 75)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				kris.transform.position = Vector3.Lerp(new Vector3(11.34f, -4.22f), new Vector3(11.92f, -2.82f), (float)(frames - 65) / 10f);
				susie.transform.position = Vector3.Lerp(new Vector3(14.07f, -4.02f), new Vector3(13.35f, -2.6f), (float)(frames - 65) / 10f);
				noelle.transform.position = Vector3.Lerp(new Vector3(12.67f, -4.21f), new Vector3(12.64f, -2.9f), (float)(frames - 65) / 10f);
			}
			if (frames == 75)
			{
				kris.GetComponent<Animator>().Play("Fall", 0, 0f);
				susie.GetComponent<Animator>().Play("FallBack", 0, 0f);
				noelle.GetComponent<Animator>().Play("Fall", 0, 0f);
				state = 6;
				frames = 0;
			}
		}
		if (state == 6)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.GetComponent<AudioSource>().Play();
			}
			if (frames <= 15)
			{
				float num3 = (float)frames / 15f;
				num3 = Mathf.Sin(num3 * (float)Math.PI * 0.5f);
				kris.transform.position = Vector3.Lerp(new Vector3(11.92f, -2.82f), new Vector3(10.92f, -2.1499999f), num3);
				susie.transform.position = Vector3.Lerp(new Vector3(13.35f, -2.6f), new Vector3(14.35f, -1.9299998f), num3);
				noelle.transform.position = Vector3.Lerp(new Vector3(12.64f, -2.9f), new Vector3(12.64f, -2.23f), num3);
			}
			if (frames < 50)
			{
				greyDoor.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f) * Mathf.Lerp(0f, 20f, (float)frames / 40f);
				greyDoor.GetComponent<AudioSource>().pitch = Mathf.Lerp(0.8f, 1.15f, (float)frames / 10f);
			}
			else if (frames > 50)
			{
				if (frames == 51)
				{
					velocity = 0f;
					greyDoor.GetComponent<AudioSource>().pitch = 0.8f;
					greyDoor.GetComponent<AudioSource>().volume = 0f;
					greyDoor.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_dtrans_drone");
					greyDoor.GetComponent<AudioSource>().loop = true;
					greyDoor.GetComponent<AudioSource>().Play();
				}
				if (frames < 120)
				{
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, (float)(frames - 50) / 60f);
				}
				else
				{
					if (frames == 120)
					{
						PlaySFX("sounds/snd_dtrans_lw");
						fade.FadeOut(60, Color.white);
					}
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, (float)(frames - 120) / 30f);
				}
				if (velocity < 0.5f)
				{
					velocity += 0.01f;
				}
				kris.transform.position += new Vector3(-1f / 96f, -1f / 32f) * velocity;
				susie.transform.position += new Vector3(1f / 96f, -1f / 32f) * velocity;
				noelle.transform.position += new Vector3(0f, -1f / 32f) * velocity;
			}
			if (frames >= 40 && frames % 15 == 1)
			{
				SpriteRenderer component = new GameObject("GreyDoorBGSquare", typeof(SpriteRenderer), typeof(GreyDoorBGSquare)).GetComponent<SpriteRenderer>();
				component.sprite = Resources.Load<Sprite>("spr_pixel");
				component.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y);
			}
			if (frames == 180)
			{
				state = 7;
				frames = 0;
			}
		}
		if (state == 7)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeIn(60, Color.white);
				GameObject.Find("Black").transform.position = Vector3.zero;
			}
			if (frames == 120)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().ForceLoadArea(74);
			}
		}
		if ((state == 3 && !txt) || state >= 4)
		{
			ness.transform.position -= new Vector3(1f / 12f, 0f);
			if (!abortedOblit)
			{
				paula.transform.position -= new Vector3(1f / 12f, 0f);
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 6)
		{
			kris.GetComponent<SpriteRenderer>().sortingOrder = 500;
			susie.GetComponent<SpriteRenderer>().sortingOrder = 500;
			noelle.GetComponent<SpriteRenderer>().sortingOrder = 500;
			Color color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)(frames - 50) / 45f);
			int num = int.Parse(kris.GetComponent<SpriteRenderer>().sprite.name.Substring(kris.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			int num2 = int.Parse(susie.GetComponent<SpriteRenderer>().sprite.name.Substring(susie.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			int num3 = int.Parse(noelle.GetComponent<SpriteRenderer>().sprite.name.Substring(noelle.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			if (ki != num)
			{
				ki = num;
				krisBW.sprite = Resources.Load<Sprite>("player/Kris/spr_kr_fall_bw_" + ki);
			}
			if (si != num2)
			{
				si = num2;
				susieBW.sprite = Resources.Load<Sprite>("player/Susie/spr_su_fall_back_bw_" + si);
			}
			if (ni != num3)
			{
				ni = num3;
				noelleBW.sprite = Resources.Load<Sprite>("player/Noelle/spr_no_fall_bw_" + ni);
			}
			krisBW.transform.position = kris.transform.position;
			susieBW.transform.position = susie.transform.position;
			noelleBW.transform.position = noelle.transform.position;
			krisBW.color = color;
			susieBW.color = color;
			noelleBW.color = color;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		abortedOblit = (int)gm.GetFlag(87) >= 5;
		if (par.Length != 0)
		{
			kill = (int)par[0] == 1;
		}
		else
		{
			kill = false;
			abortedOblit = false;
		}
		gm.SetMiniPartyMember(0);
		gm.StopMusic();
		paula = GameObject.Find("Paula").GetComponent<Animator>();
		ness = GameObject.Find("Ness").GetComponent<Animator>();
		ness.enabled = true;
		ness.SetFloat("dirX", 1f);
		UnityEngine.Object.Destroy(GameObject.Find("Porky"));
		if ((bool)UnityEngine.Object.FindObjectOfType<MoleFriend>())
		{
			UnityEngine.Object.FindObjectOfType<MoleFriend>().GetComponent<SpriteRenderer>().enabled = true;
		}
		GameObject.Find("NatureSounds").GetComponent<AudioSource>().Play();
		kris.transform.position = new Vector3(8.31f, -5.11f);
		kris.SetSelfAnimControl(false);
		kris.GetComponent<Animator>().SetBool("isMoving", false);
		kris.GetComponent<Animator>().Play("idle");
		kris.ChangeDirection(Vector2.right);
		susie.UseUnhappySprites();
		susie.EnableAnimator();
		susie.transform.position = new Vector3(7.54f, -6.73f);
		susie.SetSelfAnimControl(false);
		susie.GetComponent<Animator>().SetBool("isMoving", false);
		susie.GetComponent<Animator>().Play("idle");
		susie.ChangeDirection(Vector2.right);
		noelle.transform.position = new Vector3(8.31f, -8.17f);
		noelle.EnableAnimator();
		noelle.UseUnhappySprites();
		noelle.SetSelfAnimControl(false);
		noelle.GetComponent<Animator>().SetBool("isMoving", false);
		noelle.GetComponent<Animator>().Play("idle");
		noelle.ChangeDirection(Vector2.right);
		cam.SetFollowPlayer(false);
		cam.transform.position = new Vector3(10.32f, -6.75f, -10f);
		if (!abortedOblit)
		{
			paula.transform.position = new Vector3(6.83f, -5.25f);
			paula.SetFloat("speed", 1f);
			paula.SetBool("isMoving", false);
			paula.Play("idle");
			paula.SetFloat("dirX", 1f);
			paula.SetFloat("dirY", 0f);
		}
		if (!kill)
		{
			ness.transform.position = new Vector3(9.75f, -4.87f);
		}
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
		krisBW = GameObject.Find("KrisBW").GetComponent<SpriteRenderer>();
		susieBW = GameObject.Find("SusieBW").GetComponent<SpriteRenderer>();
		noelleBW = GameObject.Find("NoelleBW").GetComponent<SpriteRenderer>();
	}
}
