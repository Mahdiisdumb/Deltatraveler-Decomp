using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterPickUpCutscene : CutsceneBase
{
	private bool leave;

	private bool oblit;

	private float speed = 4f;

	private int krisRunFrames;

	private Transform letter;

	private Animator papyrus;

	private Animator sans;

	private int runFrames;

	private bool susRun;

	private void Update()
	{
		if (state == 0)
		{
			bool num = MoveTo(kris, new Vector3(0f, 0.62f), 4f);
			bool flag = MoveTo(susie, new Vector3(-0.7f, 1.01f), 4f);
			bool flag2 = MoveTo(noelle, new Vector3(-3.68f, noelle.transform.position.y), 4f);
			if (!num)
			{
				ChangeDirection(kris, Vector2.down);
				SetMoveAnim(kris, isMoving: false);
			}
			if (!flag)
			{
				ChangeDirection(susie, Vector2.down);
				SetMoveAnim(susie, isMoving: false);
			}
			if (!flag2)
			{
				ChangeDirection(noelle, Vector2.right);
				SetMoveAnim(noelle, isMoving: false);
			}
			if (!num && !flag && !flag2)
			{
				state = 1;
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames == 10)
			{
				letter.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				SetSprite(kris, "spr_kr_openletter_0");
			}
			if (frames == 30)
			{
				PlaySFX("sounds/snd_noise");
				SetSprite(kris, "spr_kr_openletter_1");
			}
			if (frames == 45)
			{
				SetSprite(susie, "spr_su_surprise_right");
			}
			if (frames == 50)
			{
				PlaySFX("sounds/snd_attack");
				PlayAnimation(susie, "AttackStick");
				gm.StopMusic();
			}
			if (frames >= 52)
			{
				if (frames == 52)
				{
					letter.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					letter.GetComponent<UFLetter>().SetSprite(0);
					letter.position = new Vector3(0f, 0.046f);
					gm.PlayGlobalSFX("sounds/snd_damage");
					SetSprite(kris, "spr_kr_ko");
				}
				float num2 = (float)(frames - 52) / 20f;
				if (num2 < 1f)
				{
					num2 = Mathf.Sin(num2 * MathF.PI * 0.5f);
				}
				kris.transform.position = new Vector3(Mathf.Lerp(0f, 2f, num2), kris.transform.position.y);
				if (frames < 62)
				{
					letter.position += new Vector3(0f, speed / 48f);
					speed -= 0.5f;
				}
			}
			if (frames >= 62)
			{
				if (letter.position.y > -0.39f)
				{
					if (frames == 62)
					{
						letter.GetComponent<UFLetter>().StartGeneratingBones();
						PlayAnimation(susie, "walk");
						ChangeDirection(susie, Vector2.left);
						SetMoveAnim(susie, isMoving: true, 3f);
						if (!oblit)
						{
							SetSprite(noelle, "spr_no_surprise");
						}
					}
					letter.position -= new Vector3(0f, 1f / 6f);
					if (letter.position.y < -0.39f)
					{
						letter.position = new Vector3(0f, -0.39f);
						letter.GetComponent<UFLetter>().MakeLetterEmpty();
					}
				}
				if (!MoveTo(susie, new Vector3(-3f, susie.transform.position.y), 10f))
				{
					ChangeDirection(susie, Vector2.right);
					SetMoveAnim(susie, isMoving: false);
					SetSprite(susie, "spr_su_surprise_right");
				}
			}
			if (frames == 92)
			{
				SetSprite(kris, "spr_kr_sit_injured", flipX: true);
			}
			if (frames == 112)
			{
				letter.GetComponent<UFLetter>().StopGeneratingBones();
			}
			if (frames == 157)
			{
				gm.PlayMusic("music/mus_him", 0.45f);
				SetSprite(susie, "spr_su_wtf");
				StartText(new string[12]
				{
					"* WHAT THE HELL WAS\n  THAT???", "* \"Letter\" head-ass.^05\n* THAT WAS BASICALLY A\n  BOMB!!!", "UHMMM,^10 SANS...", "I WAS EXPECTING A \nBIT MORE THAN,^05 WELL,^10\nAN ASSAULT.", "I FIGURED THAT IT \nWOULD'VE BEEN A \nTRICK!", "LIKE MAYBE YOU \nWRITE A VERY NICE, \nFLATTERING LETTER.", "THEN BACKSTAB THEM \nWHILE THEY'RE \nDISTRACTED!!", "* oh, ^05papyrus.", "* do you think i'd just\n  let them by after\n  what i've been through?", "* after what they've done\n  to me?",
					"WHAT THE HELL IS \nTHAT SUPPOSED TO \nMEAN???", "* oh,^05 you wouldn't know."
				}, new string[12]
				{
					"snd_txtsus", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsans", "snd_txtsans", "snd_txtsans",
					"snd_txtpap", "snd_txtsans"
				}, new int[1], new string[12]
				{
					"su_wtf", "su_pissed", "ufpap_worry", "ufpap_side", "ufpap_neutral", "ufpap_side", "ufpap_evil", "ufsans_side", "ufsans_closed", "ufsans_empty",
					"ufpap_mad", "ufsans_closed"
				});
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2)
		{
			if (!txt || txt.GetCurrentStringNum() >= 3)
			{
				krisRunFrames++;
				if (krisRunFrames == 20)
				{
					ChangeDirection(kris, Vector2.right);
					PlayAnimation(kris, "idle");
				}
				else if (krisRunFrames >= 40)
				{
					if (!MoveTo(kris, new Vector3(-1.78f, kris.transform.position.y), txt ? 2 : 4))
					{
						SetMoveAnim(kris, isMoving: false);
					}
					else
					{
						SetMoveAnim(kris, isMoving: true, txt ? 0.5f : 1f);
					}
				}
			}
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(susie, "spr_su_throw_ready");
				}
				else if (AtLine(3))
				{
					PlayAnimation(susie, "idle");
					if (!oblit)
					{
						SetSprite(noelle, "spr_no_right_shocked_0");
					}
					ChangeDirection(papyrus, Vector2.up);
					ChangeDirection(sans, Vector2.down);
					SetSprite(kris, "spr_kr_sit");
				}
				else if (AtLine(4))
				{
					ChangeDirection(papyrus, Vector2.left);
				}
				else if (AtLine(5))
				{
					ChangeDirection(papyrus, Vector2.up);
				}
				else if (AtLine(11))
				{
					PlayAnimation(papyrus, "Pissed");
				}
				return;
			}
			if (sans.transform.position.y > -0.32f)
			{
				PlayAnimation(papyrus, "idle");
				SetMoveAnim(sans, isMoving: true);
				MoveTo(sans, new Vector3(sans.transform.position.x, -0.32f), 4f);
				return;
			}
			if (sans.transform.position.x < 7.06f)
			{
				ChangeDirection(sans, Vector2.right);
				MoveTo(sans, new Vector3(7.06f, -0.32f), 4f);
				if (sans.transform.position.x > 4.5f)
				{
					ChangeDirection(papyrus, Vector2.right);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(kris, Vector2.right);
				PlayAnimation(kris, "idle");
				SetMoveAnim(kris, isMoving: false);
			}
			if (frames == 30)
			{
				state = 3;
				frames = 0;
				ChangeDirection(papyrus, Vector2.up);
				StartText(new string[11]
				{
					"...", "WELL,^05 THAT WAS \nRATHER UNEXPECTED.", "BUT DO NOT BE \nTHAT ALARMED!!!", "I SHALL KEEP HIM \nON THE STRONGEST OF \nLEASHES!!!", "WELL,^10 METAPHORICALLY.", "FROM HERE ON,^05 YOU \nWILL ENCOUNTER FAIR \nCHALLENGES!", "THEY WILL NOT BE \nSUDDEN ASSAULTS \nYOU COULDN'T PREDICT!", "I WILL MAKE SURE \nOF THAT.", "IN ANY CASE!!!", "AS LONG AS YOU \nSEE ROAD,^05 YOU'LL \nSEE TRAPS!",
					"NYEH!^10 HEH!^10 HEH HEH\n!"
				}, new string[1] { "snd_txtpap" }, new int[1], new string[11]
				{
					"ufpap_sus", "ufpap_side", "ufpap_neutral", "ufpap_neutral", "ufpap_side", "ufpap_neutral", "ufpap_neutral", "ufpap_side", "ufpap_evil", "ufpap_evil",
					"ufpap_evil"
				});
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(papyrus, Vector2.left);
				}
				else if (AtLine(3))
				{
					PlayAnimation(papyrus, "Pose");
				}
				else if (AtLine(5))
				{
					PlayAnimation(papyrus, "idle");
					ChangeDirection(papyrus, Vector2.up);
				}
				else if (AtLine(6))
				{
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_left_mad_0");
				}
				return;
			}
			if (MoveTo(papyrus, new Vector3(7.1f, -0.778f), 6f))
			{
				papyrus.enabled = true;
				ChangeDirection(papyrus, Vector2.right);
				SetMoveAnim(papyrus, isMoving: true);
				return;
			}
			frames++;
			if (frames == 1)
			{
				gm.StopMusic(60f);
			}
			if (frames == 20)
			{
				PlayAnimation(noelle, "idle");
				List<string> list = new List<string> { "* The hell did he\n  mean by \"they?\"", "* He wasn't talking about\n  us,^05 right?", "* Cuz we only met this\n  guy like...^10 10 minutes\n  ago." };
				List<string> list2 = new List<string> { "snd_txtsus", "snd_txtsus", "snd_txtsus" };
				List<string> list3 = new List<string> { "su_annoyed", "su_side_sweat", "su_neutral" };
				if (!oblit)
				{
					list.Add("* Maybe he's confusing\n  us with someone else...?");
					list2.Add("snd_txtnoe");
					list3.Add("no_thinking");
				}
				ChangeDirection(kris, Vector2.left);
				list.Add(leave ? "* Whatever.^05\n* See ya up ahead." : "* Whatever.^05\n* Let's get going.");
				list2.Add("snd_txtsus");
				list3.Add("su_annoyed");
				StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
				state = 4;
				frames = 0;
			}
		}
		else
		{
			if (state != 4 || (bool)txt)
			{
				return;
			}
			if (leave)
			{
				int num3 = 8;
				runFrames++;
				if (runFrames > 10)
				{
					num3 = ((runFrames > 60) ? 12 : 10);
				}
				MoveTo(susie, new Vector3(8.33f, susie.transform.position.y), num3);
				if (susie.transform.position.y > 0.22f)
				{
					susie.transform.position -= new Vector3(0f, (float)num3 / 48f);
				}
				if (runFrames == 10 && susRun)
				{
					PlayAnimation(susie, "run", 1.5f);
				}
				if (frames == 0)
				{
					UnityEngine.Object.Destroy(Util.FindObjectOfType<LetterScenarioHandler>().gameObject);
					SetMoveAnim(susie, isMoving: true, 1.5f);
					ChangeDirection(kris, Vector2.down);
					gm.PlayMusic("zoneMusic");
					kris.SetCollision(onoff: true);
					kris.SetSelfAnimControl(setAnimControl: true);
					gm.EnablePlayerMovement();
					frames++;
				}
			}
			else
			{
				UnityEngine.Object.Destroy(Util.FindObjectOfType<LetterScenarioHandler>().gameObject);
				RestorePlayerControl();
				ChangeDirection(kris, Vector2.down);
				gm.PlayMusic("zoneMusic");
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		GameObject.Find("LoadingZone").GetComponent<LoadingZone>().SetForceActivationTrigger(forceActivationTrigger: false);
		papyrus = GameObject.Find("Papyrus").GetComponent<Animator>();
		sans = GameObject.Find("Sans").GetComponent<Animator>();
		oblit = (int)Util.GameManager().GetFlag(172) == 1;
		leave = !Util.GameManager().SusieInParty();
		UnityEngine.Object.Destroy(GameObject.Find("CutsceneZone (1)"));
		RevokePlayerControl();
		Util.GameManager().SetFlag(197, 1);
		gm.SetFlag(84, 9);
		ChangeDirection(kris, new Vector3(0f, 0.62f) - kris.transform.position);
		ChangeDirection(susie, new Vector3(-0.7f, 1.01f) - susie.transform.position);
		ChangeDirection(noelle, Vector2.left);
		SetMoveAnim(kris, isMoving: true);
		SetMoveAnim(susie, isMoving: true);
		SetMoveAnim(noelle, isMoving: true);
		susRun = GameManager.GetOptions().runAnimations.value == 1;
		letter = Util.FindObjectOfType<UFLetter>().transform;
	}
}
