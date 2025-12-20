using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class SansPapUFFirstCutscene : CutsceneBase
{
	private Animator papyrus;

	private Animator sansuf;

	private Animator sans;

	private Transform snowy;

	private Vector3 krisToPos;

	private Vector3 susieToPos;

	private Vector3 noelleToPos;

	private Vector3 krisFromPos;

	private Vector3 susieFromPos;

	private Vector3 noelleFromPos;

	private float krisOrigY;

	private bool doSonaStuff = true;

	private bool noelleDepressed;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 40)
			{
				papyrus.SetFloat("speed", 1f);
				papyrus.Play("idle");
				papyrus.SetFloat("dirX", -1f);
				papyrus.enabled = false;
				papyrus.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/underfell/spr_ufpap_left_mad_0");
				papyrus.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				kris.SetSelfAnimControl(setAnimControl: false);
				susie.SetSelfAnimControl(setAnimControl: false);
				noelle.SetSelfAnimControl(setAnimControl: false);
				kris.ChangeDirection(Vector2.right);
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
				StartText(new string[11]
				{
					"WHO GOES THERE?", "... I HAVEN'T SEEN \nYOUR FACES AROUND \nHERE BEFORE...", "EXCEPT FOR THE \nDEER LADY.", "WHICH IS STRANGE.", "* Uhh...^10 who the hell\n  are you?", "I AM KNOWN BY \nMANY NAMES,^05 BUT \nYOU MAY CALL ME...", "THE GREAT PAPYRUS!!!^10\nTHE CLEVEREST KILLER \nIN THE UNDERGROUND!", "* ...", "* Okay,^05 well,^05 you keep\n  doing what you're\n  doing...", "* We're gonna head\n  over to Hotland.",
					"OH,^05 OKAY."
				}, new string[11]
				{
					"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtsus", "snd_txtsus",
					"snd_txtpap"
				}, new int[2], new string[11]
				{
					"ufpap_mad", "ufpap_side", "ufpap_neutral", "ufpap_worry", "su_side", "ufpap_side", "ufpap_evil", "su_inquisitive", "su_annoyed", "su_smirk_sweat",
					"ufpap_neutral"
				});
				state = 1;
				frames = 0;
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && !papyrus.enabled)
				{
					papyrus.enabled = true;
				}
				else if (txt.GetCurrentStringNum() == 6)
				{
					papyrus.SetFloat("dirX", 0f);
					papyrus.SetFloat("dirY", 1f);
				}
				else if (txt.GetCurrentStringNum() == 7)
				{
					papyrus.Play("Pose");
				}
				else if (txt.GetCurrentStringNum() == 8)
				{
					susie.ChangeDirection(Vector2.up);
				}
				else if (txt.GetCurrentStringNum() == 9)
				{
					susie.ChangeDirection(Vector2.right);
				}
				else if (txt.GetCurrentStringNum() == 11)
				{
					papyrus.Play("idle");
					papyrus.SetFloat("dirX", -1f);
					papyrus.SetFloat("dirY", 0f);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					papyrus.Play("Write");
					kris.GetComponent<Animator>().SetBool("isMoving", value: true);
					kris.GetComponent<Animator>().SetFloat("speed", 1f);
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.GetComponent<Animator>().SetFloat("speed", 1f);
					noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
					noelle.GetComponent<Animator>().SetFloat("speed", 1f);
					krisFromPos = kris.transform.position;
					susieFromPos = susie.transform.position;
					noelleFromPos = noelle.transform.position;
					krisToPos = kris.transform.position + new Vector3(4.69f, 0f);
					if (krisToPos.y > -0.54f)
					{
						krisToPos.y = -0.54f;
					}
					susieToPos = susie.transform.position + new Vector3(4.69f, 0f);
					if (susieToPos.y > -0.54f + susie.GetPositionOffset().y)
					{
						susieToPos.y = -0.54f + susie.GetPositionOffset().y;
					}
					noelleToPos = noelle.transform.position + new Vector3(4.69f, 0f);
					if (noelleToPos.y > -0.54f + noelle.GetPositionOffset().y)
					{
						noelleToPos.y = -0.54f + noelle.GetPositionOffset().y;
					}
				}
				kris.transform.position = Vector3.Lerp(krisFromPos, krisToPos, (float)frames / 50f);
				noelle.transform.position = Vector3.Lerp(noelleFromPos, noelleToPos, (float)frames / 50f);
				susie.transform.position = Vector3.Lerp(susieFromPos, susieToPos, (float)frames / 50f);
				if (frames == 35)
				{
					papyrus.SetFloat("speed", 0f);
				}
				if (frames == 50)
				{
					papyrus.Play("idle");
					papyrus.SetFloat("dirX", 0f);
					papyrus.SetFloat("dirY", 1f);
					kris.GetComponent<Animator>().SetBool("isMoving", value: false);
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
					Util.GameManager().StopMusic(30f);
					StartText(new string[9]
					{
						"WAIT A SECOND.^10\nSTOP RIGHT THERE.",
						noelleDepressed ? "* ...?" : "* Is...^10 something wrong?",
						"YOU,^05 IN THE GREEN.",
						"SOMETHING ABOUT \nYOU SEEMS...",
						"FAMILIAR.",
						"* Does \"Kris\" ring any\n  bells?",
						"NO,^05 BUT...",
						"LET ME CHECK \nMY RECORDS...",
						"I GET THE FEELING \nTHAT THEY MAY BE \nA..."
					}, new string[9] { "snd_txtpap", "snd_txtnoe", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[2], new string[9]
					{
						"ufpap_side",
						noelleDepressed ? "no_depressedx" : "no_shocked",
						"ufpap_mad",
						"ufpap_side",
						"ufpap_side",
						"su_side_sweat",
						"ufpap_side",
						"ufpap_neutral",
						"ufpap_side"
					});
					state = 2;
					frames = 0;
				}
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					kris.ChangeDirection(Vector2.up);
					susie.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
				}
				else if (txt.GetCurrentStringNum() == 3)
				{
					papyrus.SetFloat("dirX", 0f);
					papyrus.SetFloat("dirY", -1f);
				}
				else if (txt.GetCurrentStringNum() == 3 || txt.GetCurrentStringNum() == 7)
				{
					papyrus.SetFloat("dirX", 0f);
					papyrus.SetFloat("dirY", 1f);
				}
				else if (txt.GetCurrentStringNum() == 6)
				{
					papyrus.SetFloat("dirX", 0f);
					papyrus.SetFloat("dirY", -1f);
					susie.GetComponent<Animator>().Play("Embarrassed");
				}
				else if (txt.GetCurrentStringNum() == 8)
				{
					susie.GetComponent<Animator>().Play("idle");
					papyrus.Play("Write");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					sansuf.GetComponent<AudioSource>().volume = 0f;
					sansuf.GetComponent<AudioSource>().Play();
				}
				sansuf.transform.position = new Vector3(Mathf.Lerp(17.87f, 11.56f, (float)frames / 25f), kris.transform.position.y);
				sansuf.GetComponent<AudioSource>().volume = (float)frames / 25f;
				if (frames >= 25 && frames < 60)
				{
					if (frames == 25)
					{
						GameObject.Find("White").GetComponent<SpriteRenderer>().enabled = true;
						PlaySFX("sounds/snd_grab");
						Util.GameManager().PlayGlobalSFX("sounds/snd_damage");
						sansuf.GetComponent<AudioSource>().Stop();
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
						susie.GetComponent<SpriteRenderer>().enabled = false;
						noelle.GetComponent<SpriteRenderer>().enabled = false;
						sansuf.GetComponent<SpriteRenderer>().enabled = false;
						papyrus.GetComponent<SpriteRenderer>().enabled = false;
						GameObject.Find("Sentry").GetComponent<SpriteRenderer>().enabled = false;
						TilemapRenderer[] array = Util.FindObjectsOfType<TilemapRenderer>();
						for (int i = 0; i < array.Length; i++)
						{
							array[i].enabled = false;
						}
						kris.DisableAnimator();
						kris.SetSprite("spr_kr_grabbed_sans_0");
						cam.SetFollowPlayer(follow: false);
					}
					if (frames <= 35)
					{
						float t = Mathf.Sin((float)(frames - 25) / 10f * MathF.PI * 0.5f);
						kris.transform.position = new Vector3(Mathf.Lerp(10.94f, 10.44f, t), kris.transform.position.y);
					}
					else
					{
						if (frames == 36 || frames == 46 || frames == 56)
						{
							Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
						}
						float num = 0f;
						if (frames >= 36 && frames <= 39)
						{
							int num2 = ((frames % 2 == 0) ? 1 : (-1));
							num = (float)(39 - frames) * (float)num2 / 24f;
						}
						if (frames >= 46 && frames <= 49)
						{
							int num3 = ((frames % 2 == 0) ? 1 : (-1));
							num = (float)(49 - frames) * (float)num3 / 24f;
						}
						if (frames >= 56 && frames <= 59)
						{
							int num4 = ((frames % 2 == 0) ? 1 : (-1));
							num = (float)(59 - frames) * (float)num4 / 24f;
						}
						kris.transform.position = new Vector3(10.44f + num, kris.transform.position.y);
					}
				}
				else if (frames >= 65)
				{
					if (frames == 65)
					{
						GameObject.Find("White").GetComponent<SpriteRenderer>().enabled = false;
						Util.GameManager().PlayGlobalSFX("sounds/snd_damage");
						sansuf.GetComponent<AudioSource>().Stop();
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
						susie.GetComponent<SpriteRenderer>().enabled = true;
						noelle.GetComponent<SpriteRenderer>().enabled = true;
						sansuf.GetComponent<SpriteRenderer>().enabled = true;
						papyrus.GetComponent<SpriteRenderer>().enabled = true;
						GameObject.Find("Sentry").GetComponent<SpriteRenderer>().enabled = true;
						TilemapRenderer[] array = Util.FindObjectsOfType<TilemapRenderer>();
						for (int i = 0; i < array.Length; i++)
						{
							array[i].enabled = true;
						}
						papyrus.enabled = false;
						papyrus.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/underfell/spr_ufpap_punch");
						papyrus.transform.position = new Vector3(10.61f, kris.transform.position.y + 0.73f);
						sansuf.enabled = false;
						sansuf.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/underfell/spr_ufsans_reach");
						kris.SetSprite("spr_kr_ko");
						susie.DisableAnimator();
						noelle.DisableAnimator();
						susie.SetSprite("spr_su_surprise_right");
						if (!noelleDepressed)
						{
							noelle.SetSprite("spr_no_surprise");
						}
						else
						{
							gm.SetFlag(172, 1);
							noelle.ChangeDirection(Vector2.right);
						}
						susie.transform.position = new Vector3(8f, kris.transform.position.y) + susie.GetPositionOffset();
						noelle.transform.position = new Vector3(6.5f, kris.transform.position.y) + noelle.GetPositionOffset();
						krisOrigY = kris.transform.position.y;
					}
					float num5 = (float)(frames - 65) / 15f;
					if (num5 >= 1f)
					{
						sansuf.enabled = true;
						sansuf.Play("idle");
						sansuf.SetFloat("dirX", -1f);
					}
					else
					{
						num5 = Mathf.Sin(num5 * MathF.PI * 0.5f);
					}
					sansuf.transform.position = new Vector3(Mathf.Lerp(10.94f, 13.01f, num5), krisOrigY - 0.137f);
					float num6 = (float)(frames - 65) / 25f;
					if (num6 < 1f)
					{
						num6 = Mathf.Sin(num6 * MathF.PI * 0.5f);
					}
					kris.transform.position = Vector3.Lerp(new Vector3(10.44f, krisOrigY), new Vector3(9.45f, krisOrigY - 1.02f), num6);
				}
				if (frames == 110)
				{
					papyrus.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/underfell/spr_ufpap_right_mad_0");
					sansuf.GetComponent<AudioSource>().Stop();
					StartText(new string[4] { "SANS!!!^15\nWHAT THE HELL ARE \nYOU DOING?!?!", "YOU ARE SO \nUNREFINED AND \nBARBARIC!!!", "* ...but boss,^05 it was just\n  standing there...", "OH MY GOD,^05 DO NOT \nCALL ME THAT!!!" }, new string[4] { "snd_txtpap", "snd_txtpap", "snd_txtsans", "snd_txtpap" }, new int[1], new string[4] { "ufpap_mad", "ufpap_mad", "ufsans_neutral", "ufpap_mad" });
					state = 3;
				}
			}
		}
		if (state == 3)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					susie.EnableAnimator();
					noelle.EnableAnimator();
					susie.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.right);
					gm.PlayMusic("music/mus_papyrus", 0.85f);
				}
				else if (txt.GetCurrentStringNum() == 4)
				{
					papyrus.enabled = true;
					papyrus.Play("Pissed");
				}
			}
			else
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_wtf");
				StartText(new string[8] { "* What the HELL are\n  you two talking about???", "* Did you just try\n  to kill Kris?!", "I WASN'T,^05 BUT \nSANS CERTAINLY DID!", "HE WAS TRYING TO \nSTEAL THEIR HUMAN \nSOUL.", "BUT HE SEEMS TO \nHAVE FORGOTTEN THAT \nI NEED TO DO THAT!!!", "I MUST BECOME A \nMEMBER OF THE ROYAL \nGUARD!!", "I MUST... ^15ASSIST IN \nTHE ERADICATION OF \nHUMANITY!", "YES..." }, new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[8] { "su_wtf", "su_wtf", "ufpap_mad", "ufpap_side", "ufpap_mad", "ufpap_evil", "ufpap_laugh", "ufpap_side" });
				state = 4;
			}
		}
		if (state == 4)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					papyrus.Play("idle");
					papyrus.SetFloat("dirX", -1f);
					papyrus.SetFloat("dirY", 0f);
				}
				else if (txt.GetCurrentStringNum() == 4)
				{
					susie.EnableAnimator();
				}
				else if (txt.GetCurrentStringNum() == 6)
				{
					papyrus.Play("Pose");
				}
			}
			else
			{
				papyrus.Play("idle");
				StartText(new string[5]
				{
					noelleDepressed ? "* ... Then why are you\n  sparing us." : "* W-wait,^05 then...\n^05  why are you sparing\n  us...?",
					"IT WOULD BE TOO\nSIMPLE TO KILL YOU  \nNOW...",
					"IT IS BUT A MANNER\nOF CULTURE!",
					"I MUST CATCH YOU\nIN ONE OF MY MANY\nTRAPS AND PUZZLES!",
					"I AM NOT A WILD \nMONSTER LIKE... ^10\nWELL..."
				}, new string[5] { "snd_txtnoe", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[5]
				{
					noelleDepressed ? "no_depressedx" : "no_shocked",
					"ufpap_side",
					"ufpap_evil",
					"ufpap_laugh",
					"ufpap_side"
				});
				state = 5;
			}
		}
		if (state == 5)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 4)
				{
					sansuf.SetFloat("dirX", -1f);
					sansuf.SetFloat("dirY", 0f);
				}
			}
			else
			{
				sansuf.SetBool("isMoving", value: true);
				sansuf.SetFloat("speed", 0.35f);
				frames = 0;
				state = 6;
			}
		}
		if (state == 6)
		{
			frames++;
			sansuf.transform.position += new Vector3(-1f, -0.5f) / 48f;
			if (frames == 20)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_freaked");
			}
			else if (frames > 30)
			{
				if (frames == 31)
				{
					susie.SetSprite("spr_su_threaten_stick");
					PlaySFX("sounds/snd_weaponpull");
				}
				susie.transform.position += new Vector3((float)(6 / (frames - 30)) / 48f, 0f);
			}
			if (frames == 40)
			{
				StartText(new string[9] { "SANS!!!", "* c'mon boss, gimme some\n  slack.", "* i deal with my\n  victims to the point.", "WE ARE NOT \nDISCUSSING THIS \nMATTER RIGHT NOW!!!", "YOU WILL ACCOMPANY \nME THIS VERY \nINSTANT!", "THERE IS A TRAP \nTHAT I NEED TO \nFINE-TUNE.", "AS FOR YOU THREE...", "COME FORTH...^10\nIF YOU DARE!", "NYEHEHEHEHEHE\nHEHEHEHEH!!!" }, new string[9] { "snd_txtpap", "snd_txtsans", "snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[9] { "ufpap_mad", "ufsans_neutral", "ufsans_empty", "ufpap_mad", "ufpap_mad", "ufpap_side", "ufpap_side", "ufpap_evil", "ufpap_evil" });
				papyrus.Play("Pissed");
				sansuf.SetBool("isMoving", value: false);
				frames = 0;
				state = 7;
			}
		}
		if (state == 7)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					sansuf.SetFloat("dirX", 0f);
					sansuf.SetFloat("dirY", 1f);
				}
				else if (txt.GetCurrentStringNum() == 4 && papyrus.enabled)
				{
					papyrus.Play("idle");
					papyrus.enabled = false;
					papyrus.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/underfell/spr_ufpap_down_mad_0");
					susie.EnableAnimator();
					PlaySFX("sounds/snd_smallswing");
				}
				else if (txt.GetCurrentStringNum() == 7)
				{
					papyrus.enabled = true;
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					papyrus.SetBool("isMoving", value: true);
					papyrus.SetFloat("speed", 1f);
					papyrus.SetFloat("dirX", 1f);
					sansuf.SetBool("isMoving", value: true);
					sansuf.SetFloat("dirX", 1f);
					sansuf.SetFloat("dirY", 0f);
					sansuf.SetFloat("speed", 1f);
					gm.StopMusic(60f);
				}
				if (frames >= 20)
				{
					cam.transform.position = Vector3.Lerp(new Vector3(10f, 0f, -10f), cam.GetClampedPos() - new Vector3(1f, 0f), (float)(frames - 20) / 45f);
				}
				if (frames == 30)
				{
					kris.SetSprite("spr_kr_sit");
					PlaySFX("sounds/snd_wing");
					susie.ChangeDirection(Vector2.down);
				}
				if (frames == 60)
				{
					kris.EnableAnimator();
					PlaySFX("sounds/snd_wing");
				}
				if (frames == 80)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wtf");
					List<string> list = new List<string> { "* That double crossing\n  piece of...!" };
					List<string> list2 = new List<string> { "snd_txtsus" };
					List<string> list3 = new List<string> { "su_pissed" };
					if (noelleDepressed)
					{
						list.AddRange(new string[10] { "* I bet he was waiting\n  to do that the whole\n  time.", "* ...", "* ... But,^05 he could've\n  done this at any time.", "* And like...^05 he isn't\n  so straight forward\n  like this.", "* He usually says really\n  stupid stuff too.", "* We're prolly in some\n  weird crazy world.", "* We need to keep our\n  guards up here.", "* ...", "* Though...^05 we can't stand\n  around defending\n  ourselves either.", "* We've gotta keep moving." });
						list2.AddRange(new string[10] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" });
						list3.AddRange(new string[10] { "su_pissed", "su_annoyed", "su_dejected", "su_side", "su_annoyed", "su_smile_sweat", "su_side_sweat", "no_depressedx", "su_side", "su_neutral" });
					}
					else
					{
						list.AddRange(new string[8] { "* Susie,^05 are you sure he's\n  the one that we've seen?", "* He doesn't have the\n  same outfit...", "* And did you see that\n  strange crack under his\n  eye?", "* ... I guess you're right.", "* But the moment he\n  lays his hands on Kris,^05\n  he's dead.", "* Well,^05 if Kris'll let\n  you, that is.", "* I'm sure they will.", "* But...^05 we've gotta keep\n  moving." });
						list2.AddRange(new string[8] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" });
						list3.AddRange(new string[8] { "no_shocked", "no_thinking", "no_curious", "su_side", "su_annoyed", "no_happy", "su_smile", "su_side" });
					}
					list.Add("* Let's \"go forth,\"^05\n  I guess.");
					list2.Add("snd_txtsus");
					list3.Add("su_dejected");
					StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
					state = 9;
					frames = 0;
				}
			}
		}
		if (state == 9)
		{
			if ((bool)txt)
			{
				if (noelleDepressed)
				{
					if (txt.GetCurrentStringNum() == 2)
					{
						susie.EnableAnimator();
						susie.ChangeDirection(Vector2.up);
					}
					else if (txt.GetCurrentStringNum() == 5 || txt.GetCurrentStringNum() == 7 || txt.GetCurrentStringNum() == 11)
					{
						susie.GetComponent<Animator>().Play("idle");
						susie.ChangeDirection(Vector2.down);
					}
					else if (txt.GetCurrentStringNum() == 6)
					{
						susie.ChangeDirection(Vector2.right);
					}
					else if (txt.GetCurrentStringNum() == 8)
					{
						susie.ChangeDirection(Vector2.left);
					}
					else if (txt.GetCurrentStringNum() == 10)
					{
						susie.GetComponent<Animator>().Play("Embarrassed");
					}
				}
				else if (txt.GetCurrentStringNum() == 2)
				{
					susie.EnableAnimator();
					susie.ChangeDirection(Vector2.up);
				}
				else if (txt.GetCurrentStringNum() == 4)
				{
					noelle.ChangeDirection(Vector2.left);
				}
				else if (txt.GetCurrentStringNum() == 5)
				{
					noelle.ChangeDirection(Vector2.right);
					susie.ChangeDirection(Vector2.left);
				}
				else if (txt.GetCurrentStringNum() == 6 && susie.GetComponent<Animator>().enabled)
				{
					susie.ChangeDirection(Vector2.right);
					susie.UseHappySprites();
					susie.DisableAnimator();
					susie.SetSprite("spr_su_throw_ready");
					susie.GetComponent<SpriteRenderer>().flipX = true;
				}
				else if (txt.GetCurrentStringNum() == 7 && noelle.GetComponent<Animator>().enabled)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_laugh_0");
				}
				else if (txt.GetCurrentStringNum() == 8 && !susie.GetComponent<Animator>().enabled)
				{
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
				else if (txt.GetCurrentStringNum() == 9 && !noelle.GetComponent<Animator>().enabled)
				{
					noelle.EnableAnimator();
					noelle.UseHappySprites();
					susie.ChangeDirection(Vector2.down);
					susie.UseUnhappySprites();
				}
			}
			else
			{
				state = 10;
				PlaySFX("sounds/snd_hurtdragon");
				snowy.transform.position = new Vector3(-0.37f, -1.04f);
			}
		}
		if (state == 10)
		{
			if (Mathf.Abs(snowy.transform.position.x - kris.transform.position.x) > 0.25f)
			{
				frames++;
				snowy.transform.position = Vector3.MoveTowards(snowy.transform.position, kris.transform.position, 5f / 24f);
				if (frames == 10)
				{
					kris.ChangeDirection(Vector2.left);
					susie.ChangeDirection(Vector2.left);
					noelle.ChangeDirection(Vector2.left);
				}
				if (frames == 30)
				{
					StartText(new string[1] { "* WAIT WHAT THE HELL" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_shocked" });
					susie.DisableAnimator();
					susie.SetSprite("spr_su_surprise_right");
					susie.GetComponent<SpriteRenderer>().flipX = true;
					if (!noelleDepressed)
					{
						noelle.DisableAnimator();
						noelle.SetSprite("spr_no_surprise");
					}
				}
			}
			else
			{
				if ((bool)txt)
				{
					UnityEngine.Object.Destroy(txt.gameObject);
				}
				noelle.EnableAnimator();
				susie.EnableAnimator();
				susie.GetComponent<SpriteRenderer>().flipX = false;
				snowy.position = new Vector3(0f, 10f);
				snowy.GetComponent<OverworldSnowy>().ForceDisable();
				kris.InitiateBattle(56);
				EndCutscene(enablePlayerMovement: false);
			}
		}
		if ((state == 7 && !txt) || state > 7)
		{
			if ((bool)papyrus)
			{
				papyrus.transform.position = Vector3.MoveTowards(papyrus.transform.position, new Vector3(18.35f, -2.79f), 1f / 12f);
			}
			if ((bool)sansuf)
			{
				sansuf.transform.position = Vector3.MoveTowards(sansuf.transform.position, new Vector3(18.35f, -2.79f), 1f / 12f);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		PlaySFX("sounds/snd_encounter");
		papyrus = GameObject.Find("Papyrus").GetComponent<Animator>();
		papyrus.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
		papyrus.SetFloat("speed", 0f);
		gm.SetPartyMember(3, -1);
		noelleDepressed = (int)gm.GetFlag(172) != 0;
		gm.SetFlag(64, 2);
		gm.SetFlag(155, 1);
		gm.SetFlag(167, 1);
		gm.SetFlag(1, "side_sweat");
		if (!noelleDepressed)
		{
			gm.SetFlag(2, "shocked");
		}
		sansuf = GameObject.Find("SansUF").GetComponent<Animator>();
		sansuf.Play("AttackLeft");
		snowy = Util.FindObjectOfType<OverworldSnowy>().transform;
		UnityEngine.Object.Destroy(GameObject.Find("CutsceneColliders"));
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
	}
}
