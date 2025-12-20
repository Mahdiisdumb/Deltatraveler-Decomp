using System;
using System.Collections.Generic;
using UnityEngine;

public class LetterEndCutscene : CutsceneBase
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
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if ((bool)txt)
			{
				if (AtLine(1))
				{
					RevokePlayerControl();
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.right);
				}
				else if (AtLine(3))
				{
					ChangeDirection(sans, Vector2.down);
				}
				else if (AtLine(5))
				{
					txt.ForceAdvanceCurrentLine();
					PlayAnimation(papyrus, "Pissed");
				}
				else if (AtLine(7))
				{
					papyrus.enabled = false;
				}
				else if (AtLine(8))
				{
					txt.ForceAdvanceCurrentLine();
				}
				else if (AtLine(9))
				{
					papyrus.enabled = true;
					PlayAnimation(papyrus, "idle");
				}
				else if (AtLine(10))
				{
					PlayAnimation(papyrus, "pose");
				}
				else if (AtLine(11))
				{
					PlayAnimation(papyrus, "idle");
					ChangeDirection(papyrus, Vector2.up);
				}
				return;
			}
			if (frames == 0)
			{
				frames++;
				ChangeDirection(kris, Vector2.up);
				SetMoveAnim(kris, isMoving: true);
				SetMoveAnim(sans, isMoving: true);
				if (!leave)
				{
					ChangeDirection(susie, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
					SetMoveAnim(susie, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
				}
				ChangeDirection(papyrus, Vector2.up);
				SetMoveAnim(papyrus, isMoving: true);
			}
			bool flag = MoveTo(kris, new Vector3(kris.transform.position.x, -2.35f), 6f);
			bool flag2 = false;
			bool flag3 = false;
			bool flag4 = MoveTo(sans, new Vector3(sans.transform.position.x, 0.92f), 4f);
			if (!leave)
			{
				flag2 = MoveTo(susie, new Vector3(susie.transform.position.x, -2.14f), 6f);
				flag3 = MoveTo(noelle, new Vector3(noelle.transform.position.x, -2.14f), 6f);
			}
			if (!flag)
			{
				SetMoveAnim(kris, isMoving: false);
			}
			if (!flag2)
			{
				SetMoveAnim(susie, isMoving: false);
			}
			if (!flag3)
			{
				SetMoveAnim(noelle, isMoving: false);
			}
			if (!flag4)
			{
				SetMoveAnim(sans, isMoving: false);
			}
			if (papyrus.transform.position.y != 0.87f)
			{
				MoveTo(papyrus, new Vector3(papyrus.transform.position.x, 0.87f), 10f);
				return;
			}
			if (papyrus.transform.position.x != 0f)
			{
				ChangeDirection(papyrus, Vector2.left);
				MoveTo(papyrus, new Vector3(0f, 0.87f), 10f);
				if (papyrus.transform.position.x < 1f)
				{
					ChangeDirection(sans, Vector2.left);
				}
				return;
			}
			ChangeDirection(papyrus, Vector2.down);
			SetMoveAnim(papyrus, isMoving: false);
			if (!flag && !flag2 && !flag3)
			{
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames == 10)
			{
				letter.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_letteropen_0");
			}
			if (frames == 30)
			{
				PlaySFX("sounds/snd_noise");
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_letteropen_1");
			}
			if (frames == 45)
			{
				PlaySFX("sounds/snd_encounter");
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_letteropen_2");
			}
			if (frames >= 50 && frames < 60)
			{
				if (frames == 50)
				{
					letter.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					letter.GetComponent<UFLetter>().SetSprite(0);
					letter.position = new Vector3(0f, 0.046f);
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_slide_0");
				}
				float num = (float)(frames - 50) / 9f;
				if (num < 1f)
				{
					num = Mathf.Sin(num * MathF.PI * 0.5f);
				}
				papyrus.transform.position = new Vector3(Mathf.Lerp(0f, -1.81f, num), papyrus.transform.position.y);
				letter.position += new Vector3(0f, speed / 48f);
				speed -= 0.5f;
			}
			else if (frames >= 60 && letter.position.y > -0.39f)
			{
				if (frames == 60)
				{
					gm.StopMusic();
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_slide_1");
					letter.GetComponent<UFLetter>().StartGeneratingBones();
					SetSprite(susie, leave ? "spr_su_surprise_right" : "spr_su_surprise_up", susie.transform.position.x > 0f);
					if (!oblit)
					{
						SetSprite(noelle, "spr_no_surprise_up");
					}
				}
				letter.position -= new Vector3(0f, 1f / 6f);
				if (letter.position.y < -0.39f)
				{
					letter.position = new Vector3(0f, -0.39f);
					letter.GetComponent<UFLetter>().MakeLetterEmpty();
				}
			}
			if (frames == 110)
			{
				letter.GetComponent<UFLetter>().StopGeneratingBones();
			}
			if (frames == 155)
			{
				gm.PlayMusic("music/mus_him", 0.45f);
				SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_slide_2");
				StartText(new string[12]
				{
					"...", "* Y'know,^05 should I even\n  be surprised???", "* That's basically a\n  BOMB!!!", "SANS,^05 THIS ISN'T A \nFAIR OR CLEVER TRAP.", "IT IS YET ANOTHER \nASSAULT...", "I WOULD'VE EXPECTED \nYOU TO HAVE,^10 SAY...", "WRITTEN A LOVELY,^05 \nFLATTERING LETTER \nAS A TRICK!", "* oh, ^05papyrus.", "* do you think i'd just\n  let them by after\n  what i've been through?", "* after what they've done\n  to me?",
					"WHAT THE HELL IS \nTHAT SUPPOSED TO \nMEAN???", "* oh,^05 you wouldn't know."
				}, new string[12]
				{
					"snd_txtpap", "snd_txtsus", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsans", "snd_txtsans", "snd_txtsans",
					"snd_txtpap", "snd_txtsans"
				}, new int[1], new string[12]
				{
					"ufpap_worry", "su_annoyed", "su_pissed", "ufpap_worry", "ufpap_worry", "ufpap_side", "ufpap_evil", "ufsans_side", "ufsans_closed", "ufsans_empty",
					"ufpap_mad", "ufsans_closed"
				});
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2)
		{
			if ((!txt || txt.GetCurrentStringNum() >= 3) && leave)
			{
				if (!MoveTo(kris, new Vector3(-1.78f, kris.transform.position.y), txt ? 2 : 4))
				{
					SetMoveAnim(kris, isMoving: false);
				}
				else
				{
					if (kris.transform.position.x < 0f)
					{
						ChangeDirection(kris, Vector2.right);
					}
					SetMoveAnim(kris, isMoving: true, txt ? 0.5f : 1f);
				}
			}
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					susie.EnableAnimator();
				}
				else if (AtLine(3))
				{
					SetSprite(susie, leave ? "spr_su_throw_ready" : "spr_su_point_up_0");
				}
				else if (AtLine(4))
				{
					susie.EnableAnimator();
					noelle.EnableAnimator();
					ChangeDirection(papyrus, Vector2.right);
					PlayAnimation(papyrus, "idle");
				}
				else if (AtLine(11))
				{
					PlayAnimation(papyrus, "Pissed");
				}
				if (leave && txt.GetCurrentStringNum() >= 2)
				{
					if (!MoveTo(susie, new Vector3(susie.transform.position.x, papyrus.transform.position.y), 6f))
					{
						ChangeDirection(susie, Vector2.right);
						SetMoveAnim(susie, isMoving: false);
					}
					else
					{
						ChangeDirection(susie, Vector2.down);
						SetMoveAnim(susie, isMoving: true);
					}
				}
				return;
			}
			if (sans.transform.position.y > -0.32f)
			{
				if (leave)
				{
					ChangeDirection(susie, Vector2.right);
					SetMoveAnim(susie, isMoving: false);
				}
				PlayAnimation(papyrus, "idle");
				ChangeDirection(sans, Vector2.down);
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
				ChangeDirection(susie, Vector2.right);
				ChangeDirection(noelle, Vector2.right);
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
					ChangeDirection(papyrus, leave ? Vector2.left : Vector2.down);
					ChangeDirection(kris, Vector2.up);
					if (!leave)
					{
						ChangeDirection(susie, Vector2.up);
						ChangeDirection(noelle, Vector2.up);
					}
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
					SetSprite(papyrus, "overworld/npcs/underfell/spr_ufpap_down_mad_0");
				}
				return;
			}
			if (MoveTo(papyrus, new Vector3(7.1f, 0.87f), 16f))
			{
				ChangeDirection(kris, Vector2.right);
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
				if (leave)
				{
					ChangeDirection(kris, Vector2.up);
					ChangeDirection(susie, Vector2.down);
				}
				else
				{
					ChangeDirection(kris, susie.transform.position - kris.transform.position);
					ChangeDirection(susie, kris.transform.position - susie.transform.position);
					ChangeDirection(noelle, susie.transform.position - noelle.transform.position);
				}
				List<string> list = new List<string> { "* The hell did he\n  mean by \"they?\"", "* He wasn't talking about\n  us,^05 right?", "* Cuz we only met this\n  guy like...^10 10 minutes\n  ago." };
				List<string> list2 = new List<string> { "snd_txtsus", "snd_txtsus", "snd_txtsus" };
				List<string> list3 = new List<string> { "su_annoyed", "su_side_sweat", "su_neutral" };
				if (!oblit)
				{
					list.Add("* Maybe he's confusing\n  us with someone else...?");
					list2.Add("snd_txtnoe");
					list3.Add("no_thinking");
				}
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
				int num2 = 8;
				runFrames++;
				if (runFrames > 10)
				{
					num2 = ((runFrames > 60) ? 12 : 10);
				}
				MoveTo(susie, new Vector3(8.33f, susie.transform.position.y), num2);
				if (runFrames == 10 && susRun)
				{
					PlayAnimation(susie, "run", 1.5f);
				}
				if (susie.transform.position.y > 0.22f)
				{
					susie.transform.position -= new Vector3(0f, (float)num2 / 48f);
				}
				if (frames == 0)
				{
					UnityEngine.Object.Destroy(Util.FindObjectOfType<LetterScenarioHandler>().gameObject);
					SetMoveAnim(susie, isMoving: true, 1.5f);
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(susie, Vector2.right);
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
		Util.GameManager().SetFlag(197, 1);
		gm.SetFlag(84, 9);
		StartText(new string[12]
		{
			"SANS!!!^05\nTHAT DIDN'T DO \nANYTHING!", "* hmm.", "* boss,^05 i need you to\n  understand.", "* you do a lotta talk,\n  but not enough\n  persuasion.", "WHAT DID I SAY \nABOUT CALLING \nME--^10 ", "* you can't seem to focus.", "* you wouldn't be able\n  to stand whatever is\n  in that envelope.", "* Y'know,^05 if you wanted\n  to convince us that\n  the letter is safe,^10 ", "NO, NO.^10\nHE'S RIGHT.", "I MUST PROVE MYSELF \nWORTHY!",
			"FINE,^05 SANS!!!^05\nI ACCEPT YOUR\nCHALLENGE!", "* (This can only end\n  badly...)"
		}, new string[12]
		{
			"snd_txtpap", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtpap", "snd_txtsans", "snd_txtsans", "snd_txtsus", "snd_txtpap", "snd_txtpap",
			"snd_txtpap", "snd_txtsus"
		}, new int[1], new string[12]
		{
			"ufpap_mad", "ufsans_closed", "ufsans_side", "ufsans_side", "ufpap_mad", "ufsans_neutral", "ufsans_empty", "su_side", "ufpap_side", "ufpap_mad",
			"ufpap_evil", "su_inquisitive"
		});
		susRun = GameManager.GetOptions().runAnimations.value == 1;
		letter = Util.FindObjectOfType<UFLetter>().transform;
	}
}
