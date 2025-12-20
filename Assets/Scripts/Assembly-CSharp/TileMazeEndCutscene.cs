using System.Collections.Generic;
using UnityEngine;

public class TileMazeEndCutscene : CutsceneBase
{
	private InteractPapyrusTextbox papyrus;

	private Animator sans;

	private int papyEnd;

	private int timerType;

	private bool depressed;

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
				int time = Util.FindObjectOfType<TileMaze>().GetTime();
				bool flag = (int)gm.GetFlag(239) == 1;
				gm.SetFlag(240, time);
				List<string> list = new List<string>();
				List<string> list2 = new List<string>();
				List<string> list3 = new List<string>();
				if (time <= 10)
				{
					timerType = 0;
					list.Add("WHAT!!!^05\nASTONISHING!!!");
					list2.Add("snd_txtpap");
					list3.Add("ufpap_mad");
				}
				else if (time <= 30)
				{
					timerType = 1;
					list.Add("IMPRESSIVE!");
					list2.Add("snd_txtpap");
					list3.Add("ufpap_neutral");
				}
				else if (time <= 60)
				{
					timerType = 2;
					list.Add("AND TIME!");
					list2.Add("snd_txtpap");
					list3.Add("ufpap_neutral");
				}
				else
				{
					timerType = 3;
					list.Add("IT'S ABOUT TIME!");
					list2.Add("snd_txtpap");
					list3.Add("ufpap_mad");
				}
				list.Add("IT TOOK YOU " + time + " \nSECONDS TO COMPLETE \nTHE PUZZLE!");
				list2.Add("snd_txtpap");
				list3.Add((timerType == 3) ? "ufpap_mad" : "ufpap_neutral");
				if (timerType == 0)
				{
					list.AddRange(new string[3] { "DID YOU PLAN OUT \nYOUR MOVEMENTS \nIN ADVANCE?", "OR PERHAPS DID \nYOU SECRETLY KNOW \nWHAT THE PUZZLE WAS?", "CLEARLY YOU ARE \nAN EXPERT ON COLORED \nTILES!" });
					list2.AddRange(new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" });
					list3.AddRange(new string[3] { "ufpap_laugh", "ufpap_side", "ufpap_neutral" });
				}
				else if (timerType == 1)
				{
					list.AddRange(new string[3] { "ALPHYS WANTED TO \nPUT A 30 SECOND \nTIMER ON THIS...", "BUT WE COMPROMISED \nWITH MORE DAMAGING \nTILES.", "BUT CLEARLY YOU \nWOULD NOT HAVE HAD \nISSUES WITH TIMERS!" });
					list2.AddRange(new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" });
					list3.AddRange(new string[3] { "ufpap_side", "ufpap_neutral", "ufpap_neutral" });
				}
				else if (timerType == 2)
				{
					list.AddRange(new string[4]
					{
						"A SHAME THAT YOU \nWOULD NOT HAVE \nMADE IT ON TIME.",
						"ALPHYS WANTED TO \nPUT A 30 SECOND \nTIMER ON THIS...",
						"BUT WE COMPROMISED \nWITH MORE DAMAGING \nTILES.",
						flag ? "PERHAPS IF YOU \nLISTENED TO THE \nDEER LADY..." : "PERHAPS IF YOU \nLISTENED TO MY \nINSTRUCTION..."
					});
					list2.AddRange(new string[4] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" });
					list3.AddRange(new string[4] { "ufpap_side", "ufpap_side", "ufpap_neutral", "ufpap_side" });
				}
				else
				{
					list.AddRange(new string[2]
					{
						flag ? "DID YOU NOT TALK \nTO YOUR FRIEND \nABOUT THE RULES?" : "WERE YOU NOT \nPAYING ATTENTION \nTO THE RULES?",
						"IF ONLY YOU \nUNDERSTOOD THE \nTILES' FUNCTIONS..."
					});
					list2.AddRange(new string[2] { "snd_txtpap", "snd_txtpap" });
					list3.AddRange(new string[2] { "ufpap_side", "ufpap_laugh" });
				}
				papyEnd = list.Count;
				list.AddRange(new string[8] { "* Alright,^05 whatever!!!", "* Your puzzles aren't\n  any match for us,^05\n  bonehead!", "* Just give up already!", "* boss,^05 i think it's about\n  time to stop playing\n  this game with them.", "NO,^05 NOT YET!!!", "I HAVE ONE MORE \nACE UP MY \nSPIKEY SLEEVE!", "PERHAPS IT MAY BE \nTOO DEADLY FOR \nYOU!", "IT ISN'T OVER YET!" });
				list2.AddRange(new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsans", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" });
				list3.AddRange(new string[8] { "su_pissed", "su_annoyed", "su_angry", "ufsans_closed", "ufpap_mad", "ufpap_mad", "ufpap_evil", "ufpap_evil" });
				StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(1 + papyEnd))
				{
					SetSprite(susie, "spr_su_throw_ready");
				}
				else if (AtLine(4 + papyEnd))
				{
					ChangeDirection(papyrus, Vector2.down);
					ChangeDirection(sans, Vector2.up);
				}
				else if (AtLine(5 + papyEnd))
				{
					kris.EnableAnimator();
					SetMoveAnim(kris, isMoving: false);
					ChangeDirection(kris, Vector2.right);
					PlayAnimation(papyrus, "Pissed");
					susie.EnableAnimator();
					susie.UseUnhappySprites();
					noelle.UseUnhappySprites();
				}
				else if (AtLine(7 + papyEnd))
				{
					susie.EnableAnimator();
					PlayAnimation(papyrus, "idle");
					ChangeDirection(papyrus, Vector2.left);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(sans, Vector2.left);
				SetMoveAnim(papyrus, isMoving: true, 1.5f);
				ChangeDirection(papyrus, Vector2.right);
			}
			if (frames <= 15)
			{
				papyrus.transform.position = Vector3.Lerp(new Vector3(4.171f, 1.13f), new Vector3(7.17f, -0.08f), (float)frames / 15f);
				if (frames == 15)
				{
					ChangeDirection(papyrus, Vector2.left);
					SetMoveAnim(papyrus, isMoving: true, 0.5f);
				}
			}
			else if (frames <= 90)
			{
				papyrus.transform.position = Vector3.Lerp(new Vector3(7.17f, -0.08f), new Vector3(6.55f, -0.08f), (float)(frames - 60) / 30f);
				if (frames == 90)
				{
					SetMoveAnim(papyrus, isMoving: false);
				}
			}
			if (frames == 120)
			{
				StartText(new string[1] { "SANS!!!" }, new string[1] { "snd_txtpap" }, new int[1], new string[1] { "ufpap_mad" });
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2 && !txt)
		{
			if (MoveTo(sans, new Vector2(7.34f, -0.67f), 4f))
			{
				MoveTo(papyrus, new Vector2(7.34f, -0.08f), 4f);
				SetMoveAnim(papyrus, isMoving: true);
				SetMoveAnim(sans, isMoving: true);
				ChangeDirection(papyrus, Vector2.right);
				ChangeDirection(sans, Vector2.right);
				return;
			}
			if (susie.transform.position.y > 0.3f)
			{
				susie.transform.position -= new Vector3(0f, 1f / 12f);
				noelle.transform.position -= new Vector3(0f, 1f / 12f);
				ChangeDirection(susie, Vector2.down);
				ChangeDirection(noelle, Vector2.down);
				SetMoveAnim(susie, isMoving: true);
				SetMoveAnim(noelle, isMoving: true);
				return;
			}
			if (susie.transform.position.x < -1.36f)
			{
				susie.transform.position += new Vector3(1f / 12f, 0f);
				noelle.transform.position += new Vector3(1f / 12f, 0f);
				ChangeDirection(susie, Vector2.right);
				ChangeDirection(noelle, Vector2.right);
				return;
			}
			SetMoveAnim(susie, isMoving: false);
			SetMoveAnim(noelle, isMoving: false);
			ChangeDirection(kris, Vector2.left);
			if (depressed)
			{
				StartText(new string[6] { "* Are we done with\n  them yet?", "* They seem like they're\n  close to their breaking\n  point.", "* Good.", "* I just wanna be\n  outta here at this\n  point.", "* This feels like hell.", "* ..." }, new string[6] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[6] { "su_annoyed", "no_thinking", "su_annoyed", "su_dejected", "su_depressed", "no_depressed_side" });
			}
			else
			{
				StartText(new string[5] { "* Why did that exit\n  feel so empty?", "* Prolly cuz he didn't\n  do his stupid laugh.", "* But who cares?", "* Feels like we're\n  almost done with these\n  bozos.", "* Let's go." }, new string[5] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[5] { "no_thinking", "su_side", "su_annoyed", "su_smirk_sweat", "su_neutral" });
			}
			state = 3;
		}
		else
		{
			if (state != 3)
			{
				return;
			}
			if ((bool)txt)
			{
				if (!depressed)
				{
					if (AtLine(2))
					{
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(3))
					{
						ChangeDirection(susie, Vector2.right);
					}
				}
			}
			else
			{
				gm.SetPartyMembers(susie: true, noelle: true);
				RestorePlayerControl();
				ChangeDirection(kris, Vector2.down);
				gm.PlayMusic("zoneMusic");
				gm.SetCheckpoint();
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		papyrus = Util.FindObjectOfType<InteractPapyrusTextbox>();
		sans = GameObject.Find("Sans").GetComponent<Animator>();
		RevokePlayerControl();
		SetSprite(kris, "spr_kr_pose");
		ChangeDirection(susie, Vector2.right);
		ChangeDirection(noelle, Vector2.right);
		PlayAnimation(susie, "idle");
		PlayAnimation(noelle, "idle");
		susie.UseHappySprites();
		noelle.UseHappySprites();
		gm.StopMusic();
		gm.SetFlag(1, "annoyed");
		gm.SetFlag(2, "thinking");
		depressed = Util.GameManager().GetFlagInt(87) >= 7;
		GameObject.Find("LoadingZone").GetComponent<LoadingZone>().SetForceActivationTrigger(forceActivationTrigger: false);
	}
}
