using System;
using System.Collections.Generic;
using UnityEngine;

public class SansPostfightCutscene : CutsceneBase
{
	private int endState;

	private bool frozen;

	private bool branch;

	private bool abort;

	private bool susieBackup;

	private bool realSpare;

	private Transform papyrus;

	private Transform stick;

	private float velocity;

	private SpriteRenderer greyDoor;

	private SpriteRenderer krisBW;

	private SpriteRenderer susieBW;

	private SpriteRenderer noelleBW;

	private int ki;

	private int si;

	private int ni;

	private int susieInitFrames;

	private bool susieRage;

	private float snoozeX;

	private int snoozer;

	private bool sansSleep;

	private Transform sans;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (sansSleep && state < 51)
		{
			int num = snoozer % 100;
			if (num == 0)
			{
				snoozeX = UnityEngine.Random.Range(0f, 1f) - 0.5f;
			}
			if (num == 10 || num == 30 || num == 50)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/OverworldZ"), sans.position + new Vector3(0f, 0.6f), Quaternion.identity).GetComponent<OverworldZ>().SetVelocity(snoozeX);
			}
			snoozer++;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 60)
			{
				if (endState == 2)
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
					if (abort)
					{
						StartText(new string[9] { "* ...", "* Susie...?", "* Kris...", "* It's taking everything\n  to not rip your\n  heart out right now.", "* This.^15\n* Is.^15\n* Bullshit.", "* Despite everything,^05 I\n  would've killed him\n  in an instant.", "* This is the ONE TIME\n  we'd be justified in\n  killing someone.", "* But no,^05 let the orphan\n  killer live 'cause you\n  also kill kids.", "* Susie..." }, new string[9] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[9] { "su_disappointed", "no_curious", "su_annoyed", "su_depressed", "su_disappointed", "su_pissed", "su_panic", "su_inquisitive", "no_depressed_side" }, 0);
						state = 19;
					}
					else if (realSpare)
					{
						StartText(new string[1] { "IS...^10 EVERYTHING \nTHAT YOU SAID...^10 \nTRUE?" }, new string[1] { "snd_txtpap" }, new int[1], new string[1] { "ufpap_dejected" }, 0);
						state = 24;
						ChangeDirection(kris, Vector2.down);
						ChangeDirection(susie, Vector2.down);
						ChangeDirection(noelle, Vector2.down);
					}
					else
					{
						StartText(new string[19]
						{
							"* Kris.", "* I'm gonna be honest.", "* I was this close\n  to killing him\n  myself.", "* I don't think he\n  deserves mercy.", "* I wish we could've\n  been able to at\n  least talk to him.", "* I feel...^05 this\n  resentment is more\n  than he lets on.", "* No.", "* People like that\n  aren't willing to\n  give mercy.", "* Their grudge goes\n  beyond whatever the\n  hell they show.", "* They would rather\n  shatter your heart\n  or...",
							"* ...^05 Threaten to throw\n  you off a roof.", "* What...?", "* And besides...", "* He killed me.", "* He GLOATED about\n  killing me.", "* And I...^10 s-^05she...\n  was just a kid.", "* ...^05 I think that alone\n  would be grounds to\n  kill him.", "* ...", "* Then what does that\n  mean for our Sans?"
						}, new string[19]
						{
							"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus",
							"snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe"
						}, new int[1], new string[19]
						{
							"su_neutral", "su_dejected", "su_depressed", "su_depressed", "no_thinking", "no_depressed", "su_annoyed", "su_side", "su_side", "su_neutral",
							"su_depressed", "no_curious", "su_annoyed", "su_pissed", "su_panic", "su_sad", "su_depressed", "no_depressed", "no_depressed_side"
						}, 0);
						state = 1;
					}
					frames = 0;
				}
				else if (frozen)
				{
					List<string> list = new List<string> { "* Noelle...?", "* How the hell did\n  that...", "* ...", "* I..." };
					List<string> list2 = new List<string> { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe" };
					List<string> list3 = new List<string> { "su_wideeye", "su_concerned", "no_shocked", "no_shocked" };
					if (branch)
					{
						list.AddRange(new string[8] { "* And how the hell\n  did Kris know that\n  would happen?", "* HMM???", "* Susie,^05 what are you--", "* Nah,^05 nahnahnahnahnah\n  nahnahnahnah.", "* This is ridiculous.", "* You attack me.", "* And then you get\n  Noelle to do some\n  weird freeze shit.", "* Clearly you don't\n  need my help!" });
						list2.AddRange(new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
						list3.AddRange(new string[8] { "su_pissed", "su_panic", "no_confused", "su_annoyed", "su_annoyed", "su_serious", "su_serious", "su_wtf" });
						StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray(), 0);
						state = 17;
						frames = 0;
					}
					else
					{
						if ((int)gm.GetFlag(87) >= 5)
						{
							list.AddRange(new string[3] { "* Eerie...", "* Is it because of\n  our power?", "* If that's the case..." });
							list2.AddRange(new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtnoe" });
							list3.AddRange(new string[3] { "su_side_sweat", "su_smirk_sweat", "no_depressed_side" });
						}
						else
						{
							list.AddRange(new string[2] { "* That's kinda cool,^05\n  actually.", "* But..." });
							list2.AddRange(new string[2] { "snd_txtsus", "snd_txtnoe" });
							list3.AddRange(new string[2] { "su_surprised", "no_depressed_side" });
						}
						list.Add("* Why did it only\n  freeze him?");
						list2.Add("snd_txtnoe");
						list3.Add("no_depressed");
						StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray(), 0);
						state = 15;
						frames = 0;
					}
				}
				else
				{
					StartText(new string[3] { "* ...", "* Susie?", "* Susie,^05 are you...?" }, new string[1] { "snd_txtnoe" }, new int[1], new string[3] { "no_shocked", "no_shocked", "no_afraid" }, 0);
					state = 13;
					frames = 0;
				}
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					ChangeDirection(susie, Vector2.left);
				}
				else if (AtLine(5))
				{
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.up);
				}
				else if (AtLine(7))
				{
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(10))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(12))
				{
					SetSprite(noelle, "spr_no_think_left_sad");
				}
				else if (AtLine(18))
				{
					noelle.EnableAnimator();
					ChangeDirection(noelle, Vector2.up);
				}
			}
			else
			{
				frames++;
				if (frames == 20)
				{
					ChangeDirection(susie, Vector2.right);
				}
				if (frames == 65)
				{
					ChangeDirection(susie, Vector2.up);
				}
				if (frames == 105)
				{
					StartText(new string[6] { "* ...", "* I...", "* ...", "* Ugh,^05 whatever!", "* Let's just get outta\n  here.", "* I don't want to\n  think about this\n  anymore." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[6] { "su_side_sweat", "su_smirk_sweat", "su_depressed", "su_pissed", "su_annoyed", "su_depressed" }, 0);
					state = 2;
					frames = 0;
					SetSprite(susie, "spr_su_lookup");
				}
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(noelle, "spr_no_think_left_sad");
				}
				else if (AtLine(3))
				{
					SetSprite(susie, "spr_su_lookup_side");
				}
				else if (AtLine(4))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(5))
				{
					susie.EnableAnimator();
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					SetMoveAnim(kris, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
					SetMoveAnim(susie, isMoving: true);
					noelle.EnableAnimator();
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(kris, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
				}
				kris.transform.position += new Vector3(0f, 1f / 12f);
				noelle.transform.position += new Vector3(0f, 1f / 12f);
				if (susie.transform.position.x > -1.71f)
				{
					susie.transform.position += new Vector3(-1f / 12f, 0f);
				}
				else
				{
					ChangeDirection(susie, Vector2.up);
					susie.transform.position += new Vector3(0f, 5f / 48f);
				}
				if (kris.transform.position.y >= 5.25f)
				{
					state = 3;
					frames = 0;
					SetSprite(kris, "spr_kr_surprise");
					SetSprite(susie, "spr_su_surprise_right");
					SetSprite(noelle, "spr_no_surprise_left");
					SetMoveAnim(kris, isMoving: false);
					SetMoveAnim(susie, isMoving: false);
					SetMoveAnim(noelle, isMoving: false);
					StartText(new string[4] { "* papyrus...^10\n* you don't...^10\n* understand...", "* Aight,^05 asshole,^05 I'm\n  killing you MYSELF!", "WAIT!!!^05 STOP!!!", "* P-^05Papyrus???" }, new string[4] { "snd_txtsans", "snd_txtsus", "snd_txtpap", "snd_txtnoe" }, new int[2] { 1, 0 }, new string[4] { "", "su_angry", "", "no_awe" }, 0);
				}
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					PlaySFX("sounds/snd_weaponpull");
					SetSprite(susie, "spr_su_threaten_stick");
				}
				else if (AtLine(3))
				{
					SetSprite(kris, "spr_kr_surprise_down");
					SetSprite(noelle, "spr_no_surprise");
				}
				else if (AtLine(4))
				{
					PlaySFX("sounds/snd_smallswing");
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.down);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.EnableAnimator();
					noelle.EnableAnimator();
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(noelle, Vector2.down);
					SetMoveAnim(papyrus, isMoving: true);
				}
				MoveTo(cam, new Vector3(0f, 2.5f, -10f), 4f);
				if (!MoveTo(papyrus, new Vector3(1f / 48f, 1.25f), 6f))
				{
					SetMoveAnim(papyrus, isMoving: false);
					StartText(new string[4] { "YES,^05 I DIDN'T DIE!!!", "A MIRACLE I GRABBED \nTHE LEDGE AFTER \nBEING KICKED.", "BUT...", "I WASN'T KICKED \nFOR NO REASON,^05 \nI IMAGINE." }, new string[4] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[4] { "ufpap_neutral", "ufpap_laugh", "ufpap_side", "ufpap_dejected" }, 1);
					state = 4;
					frames = 0;
				}
			}
		}
		else if (state == 4 && !txt)
		{
			if (frames == 0)
			{
				frames++;
				SetMoveAnim(papyrus, isMoving: true, 0.5f);
				gm.StopMusic(60f);
			}
			if (!MoveTo(papyrus, new Vector3(1f / 48f, 3.8f), 2f))
			{
				frames++;
				if (frames == 2)
				{
					SetMoveAnim(papyrus, isMoving: false);
				}
				if (frames == 10)
				{
					PlayAnimation(papyrus, "Kneel");
				}
				if (frames == 45)
				{
					gm.PlayMusic("music/mus_sansspare_intro");
				}
				if (frames == 75)
				{
					StartText(new string[14]
					{
						"SANS...", "I KNOW THAT YOU'RE \nASLEEP.", "THE FIRST TIME YOU \nSLEPT IN A LONG,^05 \nLONG TIME.", "BUT THE WAY YOU'VE \nBEEN ACTING...", "IT IS SO UNLIKE \nTHE BROTHER I \nGREW UP WITH.", "THE ONE THAT WOULD \nPLAY TERRIBLE \nJOKES...", "THE ONE WHO \nVALUED HIS FRIENDS \nAND MEMORIES...", "I KNOW THAT YOU \nWERE VERY RESERVED,^05 \nEVEN IN THE PAST.", "BUT EVER SINCE WE \nARRIVED HERE IN \nSNOWDIN...", "I WATCHED AS YOU \nCHANGED INTO THIS \nCOLD,^05 JADED KILLER.",
						"YOU BEGAN YOUR \nMURDEROUS SPREE.", "YOU PUSHED ME TO \nSEEK REVENGE ON \nTHE HUMANS.", "YOU BASHED MY HEAD \nIN FOR REFUSING TO \nFINISH OFF...", "..."
					}, new string[14]
					{
						"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap",
						"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap"
					}, new int[1], new string[14]
					{
						"ufpap_side", "ufpap_side", "ufpap_worry", "ufpap_sad", "ufpap_sad", "ufpap_sad", "ufpap_sad", "ufpap_side", "ufpap_sad", "ufpap_dejected",
						"ufpap_sad", "ufpap_sad", "ufpap_dejected", "ufpap_dejected_closed"
					}, 1);
					state = 5;
					frames = 0;
				}
			}
			else
			{
				cam.transform.position += new Vector3(0f, 1f / 48f);
				LookAt(kris, papyrus);
				LookAt(susie, papyrus);
				LookAt(noelle, papyrus);
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (AtLine(8))
				{
					SetSprite(noelle, "spr_no_think_left_sad");
				}
				if (AtLine(10))
				{
					SetSprite(kris, "spr_kr_think");
				}
				if (AtLine(14))
				{
					SetSprite(susie, "spr_su_left_worried_0", flipX: true);
				}
			}
			else
			{
				frames++;
				if (frames == 45)
				{
					SetSprite(susie, "spr_su_lookaway");
				}
				if (frames == 90)
				{
					StartText(new string[3] { "SANS...", "I CANNOT HELP BUT \nTHINK...", "DO YOU EVEN CARE \nABOUT ME \nANYMORE...?" }, new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[3] { "ufpap_dejected", "ufpap_dejected", "ufpap_dejected_closed" }, 1);
					state = 6;
					frames = 0;
				}
			}
		}
		else if (state == 6 && !txt)
		{
			frames++;
			if (frames == 45)
			{
				PlayAnimation(papyrus, "PreRemoveArmor");
			}
			if (frames == 120)
			{
				StartText(new string[1] { "PERHAPS WE'LL SEE IF \nI BECOME ONE WITH \nTHE SNOW OR NOT." }, new string[1] { "snd_txtpap" }, new int[1], new string[1] { "ufpap_dejected_closed" }, 1);
				state = 7;
				frames = 0;
			}
		}
		else if (state == 7 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				SetSprite(kris, "spr_kr_think_look");
				PlayAnimation(papyrus, "RemoveArmor");
			}
			if (frames == 100)
			{
				GameObject.Find("Armor").transform.position = new Vector3(0f, 3.302f);
				PlayAnimation(papyrus, "PutOnScarf");
			}
			if (frames == 190)
			{
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_up_0");
				StartText(new string[10] { "I OWE ALL OF \nYOU AN APOLOGY.", "NONE OF YOU \nDESERVED ANY OF \nTHIS.", "ESPECIALLY YOU,^05\nKRIS.", "MY ARROGANCE AND \nDOUBT TRULY BLINDED \nME.", "* ...", "* I guess...", "* Papyrus...", "* I'm so sorry...", "DO NOT APOLOGIZE,^05\nMS. NOELLE.", "THIS IS NOT YOUR \nBURDEN TO CARRY." }, new string[10] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtpap", "snd_txtpap" }, new int[1], new string[10] { "ufpap_sad", "ufpap_sad", "ufpap_worry", "ufpap_dejected", "su_depressed", "su_depressed", "no_sad", "no_depressed", "ufpap_sad", "ufpap_confident" }, 1);
				state = 8;
				frames = 0;
			}
			MonoBehaviour.print(frames);
		}
		else if (state == 8)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_pointatkris");
					SetSprite(kris, "spr_kr_think_look_turn");
				}
				else if (AtLine(7))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_lookatnoelle");
				}
				else if (AtLine(10))
				{
					PlayAnimation(papyrus, "WalkDownCasualSad", 0f);
				}
			}
			else if (!MoveTo(papyrus, new Vector3(1f / 48f, 2.76f, 2f), 2f))
			{
				frames++;
				if (frames == 1)
				{
					PlayAnimation(papyrus, "WalkDownCasualSad", 0f);
					noelle.EnableAnimator();
					SetSprite(susie, "spr_su_lookup");
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(noelle, Vector2.down);
				}
				if (frames == 40)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_down_closed_0");
					StartText(new string[10] { "STARTING TODAY,^05 I \nTURN OVER A NEW \nLEAF.", "I WANT TO START \nMAKING A POSITIVE \nMARK ON THE WORLD.", "AND THAT STARTS \nWITH CEASING MY \nGOALS OF BECOMING", "A ROYAL GUARDSMAN.", "AS COOL AS THE \nTITLE IS...", "IT IS WRITTEN IN \nBLOOD.", "AND CONTINUES TO \nGROW WORSE BY THE \nDAY.", "INSTEAD,^05 I AIM \nTO HELP THOSE \nIN NEED.", "I KNOW MANY PEOPLE \nAROUND HERE NEED \nTHAT KIND OF HELP.", "ESPECIALLY WITH HOW \nCOLD THE \nUNDERGROUND IS." }, new string[10] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[10] { "ufpap_confident", "ufpap_confident", "ufpap_dejected", "ufpap_confident", "ufpap_sad", "ufpap_dejected", "ufpap_dejected_closed", "ufpap_neutral", "ufpap_side", "ufpap_dejected" }, 1);
					state = 9;
					frames = 0;
				}
			}
			else
			{
				PlayAnimation(papyrus, "WalkDownCasualSad", 0.5f, startAtBeginning: false);
			}
		}
		else if (state == 9)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_confident");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_down_closed_0");
				}
				if (frames == 45)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_up_0");
					StartText(new string[4] { "I DO HOPE THAT \nOTHERS WILL ACCEPT \nMY CHANGE.", "* I'm sure you'll do\n  well,^05 Papyrus.", "...", "THANK YOU." }, new string[4] { "snd_txtpap", "snd_txtnoe", "snd_txtpap", "snd_txtpap" }, new int[1], new string[4] { "ufpap_dejected", "no_relief", "ufpap_dejected_closed", "ufpap_confident" }, 1);
					state = 10;
					frames = 0;
				}
			}
		}
		else if (state == 10 && !txt)
		{
			frames++;
			if (frames == 30)
			{
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_down_closed_0");
			}
			if (frames == 75)
			{
				StartText(new string[1] { "TAKE CARE,^10\nYOU THREE." }, new string[1] { "snd_txtpap" }, new int[1], new string[1] { "ufpap_confident" }, 1);
				state = 11;
				frames = 0;
			}
		}
		else if (state == 11 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				PlayAnimation(papyrus, "WalkDownCasual");
			}
			if (frames >= 30 && !MoveTo(cam, new Vector3(0f, 5.15f, -10f), 3f))
			{
				gm.StopMusic(90f);
				StartText(new string[5] { "* ...", "* Took him that long\n  to say sorry.", "* Susie.", "* I think we should\n  go.", "* Right." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[5] { "su_side", "su_dejected", "no_depressed", "no_happy", "su_smirk_sweat" }, 0);
				state = 12;
				frames = 0;
			}
			papyrus.position -= new Vector3(0f, 1f / 12f);
		}
		else if (state == 12)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					kris.EnableAnimator();
					SetSprite(susie, "spr_su_lookup_side");
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(5))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.right);
				}
			}
			else
			{
				SetMoveAnim(kris, isMoving: true);
				SetMoveAnim(noelle, isMoving: true);
				SetMoveAnim(susie, isMoving: true);
				state = 50;
				frames = 0;
			}
		}
		else if (state == 13 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				SetSprite(susie, "spr_su_postsans_0");
			}
			if (frames == 40)
			{
				SetSprite(susie, "spr_su_postsans_look");
			}
			if (frames == 85)
			{
				susie.EnableAnimator();
				PlayAnimation(susie, "idle");
				PlaySFX("sounds/snd_smallswing");
			}
			if (frames == 115)
			{
				StartText(new string[5] { "* ...", "* ...^15 Let's just go.", "* Susie,^05 I don't think\n  you're--", "* I said let's go.", "* ..." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[5] { "", "", "no_sad", "", "no_depressed" }, 0);
				state = 14;
				frames = 0;
			}
		}
		else if (state == 14 && !txt)
		{
			SetMoveAnim(kris, isMoving: true);
			SetMoveAnim(noelle, isMoving: true);
			SetMoveAnim(susie, isMoving: true);
			state = 50;
			frames = 0;
			gm.StopMusic(60f);
		}
		else if (state == 15 && !txt)
		{
			frames++;
			if (frames == 45)
			{
				StartText(new string[7] { "* ...", "* I dunno.", "* Maybe we ask whoever\n  the hell is putting\n  these doors up.", "* Speaking of...", "* We're done with this\n  guy.", "* Let's go.", "* ... Right!" }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[7] { "su_side", "su_smirk_sweat", "su_neutral", "su_annoyed", "su_annoyed", "su_annoyed", "no_awe" }, 0);
				state = 16;
				frames = 0;
			}
		}
		else if (state == 16)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					PlayAnimation(susie, "Embarrassed");
				}
				else if (AtLine(4))
				{
					PlayAnimation(susie, "idle");
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(5))
				{
					ChangeDirection(noelle, Vector2.left);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					SetMoveAnim(kris, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
					SetMoveAnim(susie, isMoving: true);
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(kris, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
				}
				kris.transform.position += new Vector3(0f, 1f / 12f);
				noelle.transform.position += new Vector3(0f, 1f / 12f);
				if (susie.transform.position.x > -1.71f)
				{
					susie.transform.position += new Vector3(-1f / 12f, 0f);
				}
				else
				{
					ChangeDirection(susie, Vector2.up);
					susie.transform.position += new Vector3(0f, 11f / 96f);
				}
				MoveTo(cam, new Vector3(0f, 6.34f, -10f), 4f);
				if (kris.transform.position.y >= 5.25f)
				{
					state = 50;
					frames = 0;
					gm.StopMusic(60f);
					ChangeDirection(susie, Vector2.right);
				}
			}
		}
		else if (state == 17)
		{
			if ((bool)txt)
			{
				if (AtLine(5))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(6))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(7))
				{
					susie.EnableAnimator();
				}
				else if (AtLine(8))
				{
					ChangeDirection(susie, Vector2.left);
				}
				else if (AtLine(12))
				{
					SetSprite(susie, "spr_su_wtf", flipX: true);
					SetSprite(kris, "spr_kr_surprise");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					Util.GameManager().ForceWeapon(1, -1);
					PlayAnimation(susie, "Throw");
					kris.EnableAnimator();
					SetMoveAnim(kris, isMoving: true, 2f);
					SetSprite(noelle, "spr_no_surprise_left");
					PlaySFX("sounds/snd_heavyswing");
				}
				if (frames <= 4)
				{
					kris.transform.position -= new Vector3(1f / 6f, 0f);
				}
				if (frames == 4)
				{
					SetMoveAnim(kris, isMoving: false);
				}
				if (frames <= 30)
				{
					float t = (float)frames / 30f;
					float num2 = Mathf.Sin((float)(frames * 6) * (MathF.PI / 180f)) * 2.47f;
					stick.position = new Vector3(Mathf.Lerp(-1.12f, -8.56f, t), 2.9f + num2);
					stick.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, 146f, t));
				}
				if (frames == 30)
				{
					PlaySFX("sounds/snd_splash");
				}
				if (frames == 40)
				{
					StartText(new string[10]
					{
						Items.ItemDrop(15),
						"* Susie,^05 why'd you..?!",
						"* Did you forget,^05 Noelle?",
						"* You should be fine\n  with this.",
						"* I...",
						"* ...",
						"* Okay.",
						"* Now let's get going.",
						"* (Don't want this thing\n  getting any bright\n  ideas.)",
						"* O-^05okay!"
					}, new string[10] { "snd_text", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[10] { "", "no_shocked", "su_angry", "su_pissed", "no_afraid", "no_depressed", "no_depressed_side", "su_annoyed", "su_side_sweat", "no_awe" }, 0);
					state = 18;
					frames = 0;
					SetSprite(noelle, "spr_no_think_right_panic", flipX: true);
				}
			}
		}
		else if (state == 18)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					SetSprite(susie, "spr_su_wtf");
					SetSprite(noelle, "spr_no_surprise_left");
					SetSprite(kris, "spr_kr_surprise");
				}
				else if (AtLine(4))
				{
					SetSprite(susie, "spr_su_throw_ready");
				}
				else if (AtLine(5))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.right);
					PlayAnimation(susie, "idle");
					SetSprite(kris, "spr_kr_think_look");
				}
				else if (AtLine(6))
				{
					noelle.EnableAnimator();
					ChangeDirection(noelle, Vector2.up);
				}
				else if (AtLine(9))
				{
					SetSprite(kris, "spr_kr_think");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.EnableAnimator();
					SetMoveAnim(kris, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
					SetMoveAnim(susie, isMoving: true);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(kris, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
				}
				kris.transform.position += new Vector3(0f, 1f / 12f);
				noelle.transform.position += new Vector3(0f, 1f / 12f);
				if (susie.transform.position.x < 1.71f)
				{
					susie.transform.position += new Vector3(1f / 12f, 0f);
				}
				else
				{
					ChangeDirection(susie, Vector2.up);
					susie.transform.position += new Vector3(0f, 11f / 96f);
				}
				MoveTo(cam, new Vector3(0f, 6.34f, -10f), 4f);
				if (kris.transform.position.y >= 5.25f)
				{
					state = 50;
					frames = 0;
					gm.StopMusic(60f);
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(kris, Vector2.right);
				}
			}
		}
		else if (state == 19)
		{
			if ((bool)txt)
			{
				if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.left);
				}
				else if (AtLineRepeat(7))
				{
					if (susieInitFrames < 4)
					{
						if (susieInitFrames == 0)
						{
							PlaySFX("sounds/snd_sussurprise");
							SetSprite(susie, "spr_su_wtf", flipX: true);
						}
						susieInitFrames++;
						MoveTo(susie, new Vector3(-0.792f, 2.164f), 2f);
					}
				}
				else if (AtLine(8))
				{
					SetSprite(susie, "spr_su_shrug_unhappy");
				}
			}
			else if (frames == 0 && MoveTo(susie, new Vector3(-0.792f, 2.164f), 6f))
			{
				susie.EnableAnimator();
				SetMoveAnim(susie, isMoving: true, 2f);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_grab");
					SetSprite(susie, "spr_su_grasp_kris_4", flipX: true);
					SetSprite(noelle, "spr_no_surprise_left");
					kris.GetComponent<SpriteRenderer>().enabled = false;
					susieInitFrames = 0;
				}
				if (frames >= 1 && frames <= 4)
				{
					int num3 = ((frames % 2 == 0) ? 1 : (-1));
					int num4 = 4 - frames;
					susie.transform.position = new Vector3(-0.792f, 2.164f) + new Vector3((float)(num4 * num3) / 24f, 0f);
				}
				if (frames == 10)
				{
					List<string> list4 = new List<string> { "* And not JUST an\n  orphan...", "* He killed ME,^05 Kris!!!", "* He killed sad,^05\n  defenseless,^05 orphan me!", "* Does it make you\n  happy letting her\n  killer live!?" };
					List<string> list5 = new List<string> { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" };
					List<string> list6 = new List<string> { "su_pissed", "su_panic", "su_pissed", "su_determined" };
					if (gm.GetFlagInt(256) == 1)
					{
						susieRage = true;
						list4.Add("* Oh wait!^05\n* YOU ALREADY TRIED\n  KILLING ME!!!");
						list5.Add("snd_txtsus");
						list6.Add("su_angry_hero");
					}
					list4.Add("* Susie!");
					list5.Add("snd_txtnoe");
					list6.Add("no_afraid_open");
					StartText(list4.ToArray(), list5.ToArray(), new int[1], list6.ToArray(), 0);
					state = 20;
					frames = 0;
					txt.gameObject.AddComponent<ShakingText>();
				}
			}
		}
		else if (state == 20)
		{
			if ((bool)txt)
			{
				if (AtLine(1))
				{
					txt.GetComponent<ShakingText>().StartShake(20);
				}
				else if (txt.GetCurrentSound() == "snd_txtsus")
				{
					txt.GetComponent<ShakingText>().StartShake(10);
				}
				else
				{
					txt.GetComponent<ShakingText>().Stop();
				}
				if (susieRage && txt.GetCurrentStringNum() >= 5 && susieInitFrames < 4)
				{
					if (susieInitFrames == 0)
					{
						Util.GameManager().PlayGlobalSFX("sounds/snd_damage");
						PlaySFX("sounds/snd_sussurprise");
					}
					susieInitFrames++;
					int num5 = ((susieInitFrames % 2 == 0) ? 1 : (-1));
					int num6 = 4 - susieInitFrames;
					susie.transform.position = new Vector3(-0.792f, 2.164f) + new Vector3((float)(num6 * num5) / 24f, 0f);
				}
				if (AtLine(susieRage ? 6 : 5))
				{
					SetSprite(noelle, "spr_no_panic_right", flipX: true);
				}
			}
			else
			{
				susie.transform.position = new Vector3(-0.792f, 2.164f);
				if (UnityEngine.Random.Range(0, 15) == 0 && frames <= 30)
				{
					susie.transform.position += new Vector3((float)((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1)) / 24f, 0f);
				}
				frames++;
				if (frames == 1)
				{
					SetSprite(noelle, "spr_no_think_right_panic", flipX: true);
				}
				if (frames == 30)
				{
					kris.GetComponent<SpriteRenderer>().enabled = true;
					SetSprite(kris, "spr_kr_sit_injured");
					SetSprite(susie, "spr_su_kneel", flipX: true);
					PlaySFX("sounds/snd_bump");
				}
				if (frames == 75)
				{
					SetSprite(noelle, "spr_no_think_left_sad");
					StartText(new string[6] { "* Susie...", "* If there's anything\n  to take away from\n  this...", "* We probably can't hurt\n  anyone else as bad\n  as before.", "* What,^05 like...", "* Like when we were\n  making people bleed out?", "* Mhm." }, new string[6] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[6] { "no_depressed_look", "no_depressed_side", "no_depressed_side", "su_sad_side", "su_depressed", "no_relief" }, 0);
					state = 21;
					frames = 0;
				}
			}
		}
		else if (state == 21 && !txt)
		{
			frames++;
			if (frames == 40)
			{
				PlaySFX("sounds/snd_wing");
				susie.EnableAnimator();
				SetMoveAnim(susie, isMoving: false);
				susie.GetComponent<SpriteRenderer>().flipX = false;
				ChangeDirection(susie, Vector2.up);
			}
			if (frames == 80)
			{
				SetSprite(susie, "spr_su_lookup");
			}
			if (frames == 120)
			{
				SetSprite(susie, "spr_su_lookup_side");
				StartText(new string[2] { "* Okay, then...", "* I guess we should\n  get going." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_dejected", "su_side" }, 0);
			}
			if (frames == 121)
			{
				SetSprite(susie, "spr_su_lookup");
			}
			if (frames == 150)
			{
				kris.EnableAnimator();
				PlaySFX("sounds/snd_wing");
			}
			if (frames >= 160 && frames < 170)
			{
				kris.transform.position -= new Vector3(1f / 24f, 0f);
				SetMoveAnim(kris, isMoving: true, 0.75f);
			}
			else
			{
				SetMoveAnim(kris, isMoving: false);
			}
			if (frames == 170)
			{
				StartText(new string[3] { "* But,^05 just know,^15 \"Kris\"...", "* Whatever the hell you\n  are...", "* I fucking hate you." }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_depressed", "su_depressed", "su_teeth" }, 0);
				state = 22;
				frames = 0;
			}
		}
		else if (state == 22)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(kris, "spr_kr_think_noeye");
				}
				else if (AtLine(3))
				{
					SetSprite(susie, "spr_su_lookup_glare");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					SetSprite(susie, "spr_su_lookup");
				}
				if (frames == 45)
				{
					StartText(new string[1] { "* Let's go." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_depressed" }, 0);
					state = 23;
					frames = 0;
				}
			}
		}
		else if (state == 23 && !txt)
		{
			kris.EnableAnimator();
			susie.EnableAnimator();
			noelle.EnableAnimator();
			ChangeDirection(kris, Vector2.up);
			ChangeDirection(susie, Vector2.up);
			ChangeDirection(noelle, Vector2.up);
			SetMoveAnim(kris, isMoving: true);
			SetMoveAnim(susie, isMoving: true);
			SetMoveAnim(noelle, isMoving: true);
			state = 50;
			gm.StopMusic(60f);
		}
		else if (state == 24 && !txt)
		{
			frames++;
			if (frames >= 20)
			{
				if (!MoveTo(susie, new Vector3(-2.211f, 1.294f), 6f))
				{
					ChangeDirection(susie, Vector2.right);
					SetMoveAnim(susie, isMoving: false);
				}
				else
				{
					ChangeDirection(susie, Vector2.left);
					SetMoveAnim(susie, isMoving: true);
				}
			}
			if (frames == 75)
			{
				StartText(new string[3] { "* ...", "* papyrus...", "* you weren't supposed to\n  hear any of that." }, new string[3] { "snd_txtsans", "snd_txtsans", "snd_txtsans" }, new int[1], new string[3] { "ufsans_shocked", "ufsans_shocked", "ufsans_sad" }, 0);
				ChangeDirection(kris, Vector2.up);
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
				state = 25;
				frames = 0;
			}
		}
		else if (state == 25)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_0");
				}
			}
			else if (!MoveTo(papyrus, new Vector3(1f / 48f, 1.35f), 8f))
			{
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_up_0");
				StartText(new string[9] { "AND WHY NOT?", "* w-^05well...", "* it's...^10 not for you to\n  worry about.", "OH,^05 IT'S EVERYTHING \nFOR ME TO WORRY \nABOUT.", "YOU ARE THE ONE \nTHAT STARTED A \nMURDEROUS SPREE.", "YOU PUSHED ME TO \nHELP WITH YOUR \nDIRTY WORK.", "TO THE POINT OF \nHARMING ME AND \nPASSERSBY.", "AND ALL THAT PAIN \nJUST TO END IT \nALL!", "IT IS COMPLETELY \nAND UTTERLY SELFISH." }, new string[9] { "snd_txtpap", "snd_txtsans", "snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[9] { "ufpap_sus", "ufsans_sad", "ufsans_depressed", "ufpap_mad", "ufpap_mad", "ufpap_mad", "ufpap_dejected", "ufpap_mad", "ufpap_disappointed" }, 0);
				state = 26;
				frames = 0;
			}
			else
			{
				PlayAnimation(papyrus, "WalkUpCasual", 2.75f, startAtBeginning: false);
			}
		}
		else if (state == 26)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_1");
				}
			}
			else
			{
				frames++;
				if (frames == 45)
				{
					StartText(new string[13]
					{
						"IN YOUR QUEST TO \nRID THE WORLD OF \nITS ISSUES...", "YOU BECAME PART OF \nTHE PROBLEM.", "ALL WHILE \nDISREGARDING WHAT \nYOU HAD LEFT.", "THROWING YOUR OWN \nBROTHER TO THE \nWAYSIDE.", "DAMAGING ANY GOOD \nTHIS WORLD HAS TO \nOFFER.", "ALL FOR REVENGE.^10\nFOR OBLIVION.", "LIKE,^10 GOODNESS ME,^05 \nBROTHER!", "DON'T YOU THINK I \nMISS THEM DEARLY,^05 \nTOO...?", "THIS WASN'T EASY \nFOR ME,^05 EITHER.", "BUT PERHAPS,^05 IT \nWOULD HAVE BEEN \nBETTER...",
						"TO JUST TAKE WHAT \nHAS BEEN GIVEN \nTO YOU.", "INSTEAD OF...", "THIS."
					}, new string[1] { "snd_txtpap" }, new int[1], new string[13]
					{
						"ufpap_side", "ufpap_sad", "ufpap_dejected", "ufpap_dejected_closed", "ufpap_dejected_closed", "ufpap_dejected", "ufpap_disappointed", "ufpap_disappointed_closed", "ufpap_dejected", "ufpap_dejected_closed",
						"ufpap_side", "ufpap_disappointed_closed", "ufpap_disappointed"
					}, 0);
					state = 27;
					frames = 0;
				}
			}
		}
		else if (state == 27 && !txt)
		{
			frames++;
			if (frames == 45)
			{
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_right_mad_0");
				StartText(new string[2] { "TO THINK YOU ALMOST \nENDED THE LIFE OF \nA CHILD.", "NOW I KNOW WHY \nYOU TRIED MAKING ME \nDO THE SAME THING." }, new string[1] { "snd_txtpap" }, new int[1], new string[2] { "ufpap_disappointed_closed", "ufpap_disappointed" }, 0);
				state = 28;
				frames = 0;
			}
		}
		else if (state == 28)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_up_0");
				}
			}
			else
			{
				frames++;
				if (frames == 20)
				{
					SetSprite(susie, "spr_su_lookaway", flipX: true);
				}
				if (frames == 60)
				{
					SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_0");
					StartText(new string[9] { "* papyrus...", "* i don't know what to\n  even do anymore...", "* everything i've done has\n  been so reprehensible.", "* how can i say that i\n  deserve a second chance?^10\n* let alone allowed to live.", "WELL...", "CONSIDERING HOW \nREVILED WE ARE...", "I'M NOT SURE.", "I DON'T THINK EVEN \nI COULD EVER FULLY \nFORGIVE YOU.", "HOWEVER..." }, new string[9] { "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[9] { "ufsans_sad", "ufsans_depressed", "ufsans_sad", "ufsans_depressed", "ufpap_side", "ufpap_worry", "ufpap_dejected", "ufpap_dejected_closed", "ufpap_confident" }, 0);
					state = 29;
					frames = 0;
				}
			}
		}
		else if (state == 29)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_1");
				}
			}
			else
			{
				if (frames == 0)
				{
					gm.StopMusic(60f);
					frames++;
				}
				if (!MoveTo(papyrus, new Vector3(1f / 48f, 3.09f), 2f))
				{
					frames++;
					if (frames == 2)
					{
						SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_up_0");
					}
					if (frames == 30)
					{
						SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_0");
						StartText(new string[1] { "THAT DOESN'T MEAN \nYOU SHOULD GIVE UP." }, new string[1] { "snd_txtpap" }, new int[1], new string[1] { "ufpap_relief" }, 0);
						state = 30;
						frames = 0;
					}
				}
				else
				{
					PlayAnimation(papyrus, "WalkUpCasual", 0.5f, startAtBeginning: false);
				}
			}
		}
		else if (state == 30 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.PlayMusic("music/mus_sansspare_real");
			}
			if (frames == 45)
			{
				SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_1");
				StartText(new string[6] { "* ...", "* but...^10 after everything\n  i've done...", "WHAT GOOD WOULD IT \nDO FOR YOU TO \nJUST GIVE UP?", "IF YOU WISH TO \nMAKE AMENDS,^10 YOU \nMUST TRY.", "BESIDES,^05 I HAVE \nMY OWN PATH OF \nATONEMENT TO WALK.", "SO...^10 WHY DON'T WE \nWALK IT TOGETHER?" }, new string[6] { "snd_txtsans", "snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[6] { "ufsans_depressed", "ufsans_sad", "ufpap_side", "ufpap_mad", "ufpap_dejected_closed", "ufpap_relief" }, 0);
				state = 31;
				frames = 0;
			}
		}
		else if (state == 31)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_0");
				}
			}
			else
			{
				frames++;
				if (frames == 30)
				{
					SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sad_1");
					StartText(new string[2] { "* i...^15 uhh...", "HERE,^05 WHY DON'T I \nHELP YOU UP." }, new string[2] { "snd_txtsans", "snd_txtpap" }, new int[1], new string[2] { "ufsans_depressed", "ufpap_side" }, 0);
					state = 32;
					frames = 0;
				}
			}
		}
		else if (state == 32 && !txt)
		{
			MoveTo(cam, new Vector3(0f, 4.4f, -10f), 2f);
			if (!MoveTo(papyrus, new Vector3(-0.686f, 4.547f), 2f))
			{
				frames++;
				if (frames == 1)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_0");
					sans.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (frames == 30)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_1");
					PlaySFX("sounds/snd_wing");
				}
				if (frames == 60)
				{
					StartText(new string[13]
					{
						"* but what about becoming\n  a royal guardsman?", "I REALIZED WHEN \nHEARING YOUR \nCONFESSION...", "I CANNOT SEEK OUT \nGUARDSMANSHIP \nANYMORE.", "SUCH A TITLE IS \nWRITTEN IN BLOOD.", "WHY WOULD I WANT \nTO SHED EVEN MORE...", "WHEN I CAN HELP \nPEOPLE INSTEAD?", "OR AT LEAST TRY \nTO...", "SO THAT OLD BATTLE \nBODY MUST GO.", "* ...", "* well,^05 heh...",
						"* hopefully undyne doesn't\n  kill you for this.", "I'D BE A LITTLE \nSHOCKED IF SHE DID.", "* well,^05 whatever you say."
					}, new string[13]
					{
						"snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsans", "snd_txtsans",
						"snd_txtsans", "snd_txtpap", "snd_txtsans"
					}, new int[1], new string[13]
					{
						"ufsans_sad_confused", "ufpap_side", "ufpap_sad", "ufpap_dejected", "ufpap_side", "ufpap_neutral", "ufpap_worry", "ufpap_confident", "ufsans_depressed", "ufsans_closed",
						"ufsans_sad_smile_side", "ufpap_worry", "ufsans_closed"
					}, 0);
					state = 33;
					frames = 0;
				}
			}
			else
			{
				PlayAnimation(papyrus, "WalkUpCasual", 0.5f, startAtBeginning: false);
			}
		}
		else if (state == 33)
		{
			if ((bool)txt)
			{
				if (AtLine(6))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_2");
				}
				else if (AtLine(10))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_3");
				}
			}
			else
			{
				frames++;
				if (frames == 40)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_4");
					StartText(new string[11]
					{
						"I APOLOGIZE ON OUR \nBEHALF FOR WHAT \nWE HAVE CAUSED YOU.", "I KNOW YOU DON'T \nOWE US \nFORGIVENESS...", "BUT YOU HAVE HELPED \nUS REALIZE OUR \nGRAVE REALITIES.", "WHETHER YOU \nINTENDED TO OR NOT.", "I ESPECIALLY \nAPOLOGIZE TO YOU,^05 \nKRIS.", "YOU GOT THE BLUNT \nEND OF OUR \nAGGRESSION.", "* ...", "* Papyrus...", "* ...^10 I wish you two\n  well...", "...",
						"THANK YOU,^10 MS. \nNOELLE."
					}, new string[11]
					{
						"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtpap",
						"snd_txtpap"
					}, new int[1], new string[11]
					{
						"ufpap_sad", "ufpap_dejected", "ufpap_relief", "ufpap_worry", "ufpap_sad", "ufpap_sad", "su_depressed", "no_depressed_side", "no_depressed", "ufpap_dejected",
						"ufpap_confident"
					}, 0);
					state = 34;
					frames = 0;
				}
			}
		}
		else if (state == 34)
		{
			if (!txt)
			{
				frames++;
				if (frames == 25)
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_3");
					StartText(new string[5] { "SANS...", "LET'S GO HOME.", "* roger that,^05 boss.", "...", "I'LL LET THAT ONE \nSLIDE...^10\nTHIS TIME." }, new string[5] { "snd_txtpap", "snd_txtpap", "snd_txtsans", "snd_txtpap", "snd_txtpap" }, new int[1], new string[5] { "ufpap_side", "ufpap_relief", "ufsans_closed", "ufpap_sus", "ufpap_side" }, 0);
					state = 35;
					frames = 0;
				}
			}
		}
		else if (state == 35)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_sans_stand_5");
				}
			}
			else
			{
				if (frames == 0)
				{
					PlayAnimation(papyrus, "WalkSans");
					frames++;
				}
				papyrus.position -= new Vector3(0f, 1f / 12f);
				MoveTo(cam, new Vector3(0f, 3.23f, -10f), 1f);
				if (papyrus.position.y <= -3.25f)
				{
					frames++;
					if (frames == 30)
					{
						gm.StopMusic(90f);
						SetSprite(susie, "spr_su_lookup");
						StartText(new string[6] { "* ...", "* I mean...", "* Who could ever forgive\n  that kinda scum.", "* Susie...", "* We should just go.", "* ...^10 Okay." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[6] { "su_depressed", "su_depressed", "su_teeth", "no_depressed_side", "no_depressed_side", "su_depressed" }, 0);
						state = 36;
						frames = 0;
					}
				}
				else
				{
					LookAt(kris, papyrus);
					LookAt(noelle, papyrus);
				}
			}
		}
		else if (state == 36)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(3))
				{
					SetSprite(susie, "spr_su_lookup_side");
				}
				else if (AtLine(4))
				{
					SetSprite(noelle, "spr_no_think_left_sad");
				}
				else if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.up);
					susie.EnableAnimator();
				}
			}
			else
			{
				noelle.EnableAnimator();
				ChangeDirection(kris, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
				SetMoveAnim(kris, isMoving: true);
				SetMoveAnim(noelle, isMoving: true);
				SetMoveAnim(susie, isMoving: true);
				state = 50;
				frames = 0;
			}
		}
		if (state == 50)
		{
			bool flag = true;
			if (MoveTo(kris, new Vector3(-1.71f, 5.26f), 4f))
			{
				flag = false;
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
				kris.ChangeDirection(Vector2.up);
			}
			if (MoveTo(susie, new Vector3(0f, 4.75f), 4f))
			{
				flag = false;
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
				susie.ChangeDirection(Vector2.up);
			}
			if (MoveTo(noelle, new Vector3(1.67f, 5.55f), 4f))
			{
				flag = false;
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
				noelle.ChangeDirection(Vector2.up);
			}
			if (MoveTo(cam, new Vector3(0f, 6.34f, -10f), 4f))
			{
				flag = false;
			}
			if (flag)
			{
				state = 51;
				frames = 0;
			}
		}
		else if (state == 51)
		{
			frames++;
			if (frames == 20)
			{
				greyDoor.sprite = Resources.Load<Sprite>("overworld/spr_grey_door_1");
				PlaySFX("sounds/snd_elecdoor_shutheavy");
			}
			if (frames >= 45 && frames <= 65)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 0f);
				kris.GetComponent<Animator>().Play("RunUp", 0, 0f);
				susie.GetComponent<Animator>().SetFloat("speed", 0f);
				susie.GetComponent<Animator>().Play("RunUp", 0, 0f);
				kris.transform.position = Vector3.Lerp(new Vector3(-1.71f, 5.26f), new Vector3(-1.71f, 4.4f), Mathf.Sin((float)((frames - 45) * 9) * (MathF.PI / 180f)));
				susie.transform.position = Vector3.Lerp(new Vector3(0f, 4.75f), new Vector3(0f, 4.75f - (susieBackup ? 0.86f : 0f)), Mathf.Sin((float)((frames - 45) * 9) * (MathF.PI / 180f)));
				noelle.transform.position = Vector3.Lerp(new Vector3(1.67f, 5.55f), new Vector3(1.67f, 4.69f), Mathf.Sin((float)((frames - 45) * 9) * (MathF.PI / 180f)));
			}
			else if (frames >= 65 && frames <= 75)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				kris.transform.position = Vector3.Lerp(new Vector3(-1.71f, 5.26f), new Vector3(-0.49f, 6.87f), (float)(frames - 65) / 10f);
				susie.transform.position = Vector3.Lerp(new Vector3(0f, 4.75f), new Vector3(0f, 6.86f), (float)(frames - 65) / 10f);
				noelle.transform.position = Vector3.Lerp(new Vector3(1.67f, 5.55f), new Vector3(0.56f, 6.98f), (float)(frames - 65) / 10f);
			}
			if (frames == 75)
			{
				kris.GetComponent<Animator>().Play("Fall", 0, 0f);
				susie.GetComponent<Animator>().Play("FallBack", 0, 0f);
				noelle.GetComponent<Animator>().Play("Fall", 0, 0f);
				state = 52;
				frames = 0;
			}
		}
		else if (state == 52)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.GetComponent<AudioSource>().Play();
			}
			if (frames <= 15)
			{
				float num7 = (float)frames / 15f;
				num7 = Mathf.Sin(num7 * MathF.PI * 0.5f);
				kris.transform.position = Vector3.Lerp(new Vector3(-0.49f, 6.87f), new Vector3(-1.42f, 7.54f), num7);
				susie.transform.position = Vector3.Lerp(new Vector3(0f, 6.86f), new Vector3(0f, 7.53f), num7);
				noelle.transform.position = Vector3.Lerp(new Vector3(0.56f, 6.98f), new Vector3(1.45f, 7.65f), num7);
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
				susie.transform.position += new Vector3(0f, -1f / 32f) * velocity;
				noelle.transform.position += new Vector3(1f / 96f, -1f / 32f) * velocity;
			}
			if (frames >= 40 && frames % 15 == 1)
			{
				SpriteRenderer component = new GameObject("GreyDoorBGSquare", typeof(SpriteRenderer), typeof(GreyDoorBGSquare)).GetComponent<SpriteRenderer>();
				component.sprite = Resources.Load<Sprite>("spr_pixel");
				component.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y);
			}
			if (frames == 180)
			{
				state = 53;
				frames = 0;
			}
		}
		else if (state == 53)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeIn(60, Color.white);
				GameObject.Find("Black").transform.position = Vector3.zero;
			}
			if (frames == 120)
			{
				Util.GameManager().ForceLoadArea(112);
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 52)
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
		endState = int.Parse(par[0].ToString());
		frozen = (int)gm.GetFlag(282) == 1;
		gm.SetFlag(84, 11);
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
		krisBW = GameObject.Find("KrisBW").GetComponent<SpriteRenderer>();
		susieBW = GameObject.Find("SusieBW").GetComponent<SpriteRenderer>();
		noelleBW = GameObject.Find("NoelleBW").GetComponent<SpriteRenderer>();
		gm.PlayMusic("music/mus_deep_noise");
		GameObject.Find("Mask").GetComponent<SpriteRenderer>().enabled = false;
		papyrus = GameObject.Find("Papyrus").transform;
		ChangeDirection(papyrus, Vector2.up);
		stick = GameObject.Find("Stick").transform;
		sans = GameObject.Find("Sans").transform;
		RevokePlayerControl();
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		cam.transform.position = new Vector3(0f, 5.27f, -10f);
		kris.transform.position = new Vector3(-1.71f, 2f);
		noelle.transform.position = new Vector3(1.71f, 2.12f);
		susie.transform.position = new Vector3(0f, 2.07f);
		ChangeDirection(kris, Vector2.up);
		ChangeDirection(susie, Vector2.up);
		ChangeDirection(noelle, Vector2.up);
		susie.GetComponent<SpriteRenderer>().flipX = false;
		noelle.GetComponent<SpriteRenderer>().flipX = false;
		SetMoveAnim(kris, isMoving: false);
		SetMoveAnim(susie, isMoving: false);
		SetMoveAnim(noelle, isMoving: false);
		PlayAnimation(kris, "idle");
		PlayAnimation(susie, "idle");
		PlayAnimation(noelle, "idle");
		gm.SetFlag(2, "relief");
		if (endState == 1)
		{
			if ((int)gm.GetFlag(12) == 1)
			{
				gm.PlayGlobalSFX("sounds/snd_ominous");
				if ((int)gm.GetFlag(257) == 1 && frozen)
				{
					branch = true;
				}
			}
			if (frozen)
			{
				sans.position = new Vector3(0f, 4.15f);
				SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_frozen");
				sans.GetComponent<SpriteRenderer>().sortingOrder = -1;
				gm.SetFlag(1, branch ? "disappointed" : "side_sweat");
			}
			else
			{
				UnityEngine.Object.Destroy(GameObject.Find("Sans"));
				susie.transform.position = new Vector3(0f, 4.02f);
				PlayAnimation(susie, "PostSansBreathe");
				susieBackup = true;
				gm.SetFlag(1, "depressed");
			}
			return;
		}
		gm.SetFlag(1, "side");
		sans.position = new Vector3(0f, 4.15f);
		sans.GetComponent<SpriteRenderer>().sortingOrder = -1;
		if (gm.GetFlagInt(87) >= 10)
		{
			abort = true;
			susieBackup = true;
			if (gm.GetFlagInt(12) == 1)
			{
				WeirdChecker.Abort(gm);
			}
			UnityEngine.Object.Destroy(sans.gameObject);
			gm.SetFlag(1, "depressed");
		}
		else if (gm.GetFlagInt(318) == 1)
		{
			SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_shock");
			papyrus.position = new Vector3(1f / 48f, 0.39f);
			SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_casual_up_0");
			realSpare = true;
			susieBackup = true;
			cam.transform.position = new Vector3(0f, 3.23f, -10f);
		}
		else
		{
			SetSprite(sans, "overworld/npcs/underfell/spr_ufsans_sleep");
			papyrus.position = new Vector3(1f / 48f, -6f);
			sansSleep = true;
		}
	}
}
