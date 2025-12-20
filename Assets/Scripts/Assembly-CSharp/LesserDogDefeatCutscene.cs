using System.Collections.Generic;
using UnityEngine;

public class LesserDogDefeatCutscene : CutsceneBase
{
	private string oblitEnd;

	private int endState;

	private bool selecting;

	private readonly string[] SUSIE_END_LINES = new string[2] { "* Don't do that shit\n  again,^05 or what comes\n  next'll be MUCH worse.", "* Got it?" };

	private readonly string[] SUSIE_END_PORTRAITS = new string[2] { "su_annoyed", "su_annoyed" };

	private readonly string[] SUSIE_YOUNEEDME_LINES = new string[4] { "* What if you NEED me\n  to get past someone?", "* What then?", "* You can't rewind time\n  and do it over again.", "* So I'll say this." };

	private readonly string[] SUSIE_YOUNEEDME_PORTRAITS = new string[4] { "su_teeth", "su_teeth_eyes", "su_smile_side", "su_neutral" };

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			gm.PlayMusic("zoneMusic");
			EndCutscene();
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					PlaySFX("sounds/snd_wing");
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
					ChangeDirection(susie, Vector2.left);
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "Sorry", Vector3.zero);
					select.SetupChoice(Vector2.right, "Shut up", new Vector3(-32f, 0f));
					select.SetupChoice(Vector2.up, "You hit me???", new Vector3(-14f, 0f));
					select.Activate(this, 0, txt.gameObject);
					selecting = true;
				}
			}
		}
		else if (state == 2)
		{
			if (!txt)
			{
				EndOfObliterationHandle();
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(5))
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(susie, "spr_su_throw_ready", true);
				}
				else if (AtLine(6))
				{
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
					ChangeDirection(susie, Vector2.left);
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "Sorry", Vector3.zero);
					select.SetupChoice(Vector2.right, "I want\nyou dead", new Vector3(-32f, 0f));
					select.SetupChoice(Vector2.up, "Yes", Vector3.zero);
					select.SetupChoice(Vector2.down, "Wanted to see what happened", new Vector3(-120f, 0f));
					select.Activate(this, 1, txt.gameObject);
					selecting = true;
				}
			}
		}
		else
		{
			if (state != 4)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(4) || AtLine(10))
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(susie, "spr_su_throw_ready", true);
				}
				else if (AtLine(5) || AtLine(11))
				{
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
					ChangeDirection(susie, (txt.GetCurrentStringNum() == 5) ? Vector2.up : Vector2.left);
				}
				else if (AtLine(9))
				{
					SetSprite(susie, "spr_su_wtf", true);
				}
			}
			else
			{
				EndOfObliterationHandle();
			}
		}
	}

	private void EndOfObliterationHandle()
	{
		RestorePlayerControl();
		if (oblitEnd != "")
		{
			new GameObject("LesserDogDefeatCutsceneOblitEnd").AddComponent<TextBox>().CreateBox(new string[2] { "* (...)", oblitEnd }, true);
			gm.PlayMusic("zoneMusic");
			EndCutscene(false);
		}
		else
		{
			gm.PlayMusic("zoneMusic");
			EndCutscene();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selecting = false;
		switch (id)
		{
		case 0:
		{
			List<string> list3 = new List<string>();
			List<string> list4 = new List<string>();
			if (index == Vector2.right)
			{
				ChangeDirection(susie, Vector2.up);
				list3.AddRange(new string[6] { "* ...", "* Okay,^10 \"Kris.\"", "* Whatever you say.", "* But lemme ask you\n  this...", "* Why'd you try to\n  kill me?", "* Do you just want\n  rid of me that badly?" });
				list4.AddRange(new string[6] { "su_depressed", "su_depressed_smile", "su_depressed_smile", "su_depressed", "su_depressed", "su_depressed" });
			}
			else
			{
				if (index == Vector2.left)
				{
					list3.Add("* Yeah,^05 you BETTER be\n  saying sorry.");
					list4.Add("su_pissed");
				}
				else
				{
					list3.AddRange(new string[2] { "* Whatever.^05\n* You asked for it when\n  you attacked that dog.", "* Honestly,^05 you kinda\n  just asked for it\n  in general." });
					list4.AddRange(new string[2] { "su_side", "su_depressed" });
				}
				list3.AddRange(SUSIE_END_LINES);
				list4.AddRange(SUSIE_END_PORTRAITS);
			}
			StartText(list3.ToArray(), new string[1] { "snd_txtsus" }, new int[1], list4.ToArray());
			state = 2;
			if (index == Vector2.right)
			{
				txt.EnableSelectionAtEnd();
				state = 3;
			}
			break;
		}
		case 1:
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			if (index == Vector2.right)
			{
				PlaySFX("sounds/snd_sussurprise");
				SetSprite(susie, "spr_su_wtf", true);
				list.AddRange(new string[12]
				{
					"* OH REALLY NOW???", "* ALRIGHT.", "* You want me dead,^05\n  huh???", "* Bet you also want\n  Noelle and Kris dead\n  too,^05 huh???", "* Alright,^05 punk,^05 I\n  see you.", "* You didn't need to\n  do this,^05 y'know.", "* Ya could've just laid\n  low and used me and\n  everyone else for murder.", "* So y'know what?", "* I'm ready to\n  jeopardize EVERYTHING.", "* Don't expect me to\n  take your shit from\n  now on.",
					"* I'm done.", "* (Susie is now done.)"
				});
				list2.AddRange(new string[12]
				{
					"su_excited", "su_excited", "su_excited", "su_serious", "su_teeth", "su_depressed", "su_depressed", "su_depressed_smile", "su_wtf", "su_angry",
					"su_annoyed", ""
				});
			}
			else
			{
				if (index == Vector2.left)
				{
					list.Add("* Yeah,^05 you BETTER be\n  saying sorry.");
					list2.Add("su_pissed");
				}
				else if (index == Vector2.up)
				{
					list.AddRange(new string[4] { "* Well that's too bad.", "* I'm not going anywhere,^05\n  cuz I've got places\n  to be.", "* And YOU'RE gonna get\n  me there.", "* And besides..." });
					list2.AddRange(new string[4] { "su_confident", "su_smile", "su_teeth_eyes", "su_smile_side" });
				}
				else if (index == Vector2.down)
				{
					list.AddRange(new string[3] { "* Well what happened is\n  that I DIDN'T die.", "* So I guess that's a\n  good thing for now.", "* But making reckless\n  choices like that could\n  ruin your road ahead." });
					list2.AddRange(new string[3] { "su_annoyed", "su_side", "su_annoyed" });
				}
				if (index != Vector2.left)
				{
					list.AddRange(SUSIE_YOUNEEDME_LINES);
					list2.AddRange(SUSIE_YOUNEEDME_PORTRAITS);
				}
				list.AddRange(SUSIE_END_LINES);
				list2.AddRange(SUSIE_END_PORTRAITS);
			}
			StartText(list.ToArray(), new string[12]
			{
				"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus",
				"snd_txtsus", "snd_text"
			}, new int[1], list2.ToArray());
			state = 2;
			if (index == Vector2.right)
			{
				gm.SetFlag(1, "depressed_smile");
				gm.SetFlag(257, 1);
				state = 4;
			}
			break;
		}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		if (endState == 3)
		{
			endState = 1;
			gm.SetFlag(253, 1);
		}
		if (endState == 1)
		{
			Object.FindObjectOfType<LesserDogSentry>().KilledDog();
			if (!gm.NoelleInParty())
			{
				Util.GameManager().SetFlag(1, "side_sweat");
				gm.SetPartyMembers(true, false);
				Util.GameManager().PlayGlobalSFX("sounds/snd_ominous");
				oblitEnd = EndBattleHandler.GetSnowdinSecondHalfString();
				if ((int)gm.GetFlag(256) == 1)
				{
					gm.StopMusic();
					RevokePlayerControl();
					cam.SetFollowPlayer(true);
					kris.transform.position = new Vector3(2.9f, 0.83f);
					susie.transform.position = new Vector3(4.52f, 1.09f);
					ChangeDirection(kris, Vector2.right);
					SetSprite(susie, "spr_su_kneel", true);
					StartText(new string[2] { "* Nnngh...", "* Okay,^05 punk.^05\n* What the HELL is\n  your problem???" }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_depressed", "su_pissed" });
					txt.EnableSelectionAtEnd();
					state = 1;
				}
				else
				{
					EndOfObliterationHandle();
				}
				return;
			}
			gm.StopMusic();
			if ((int)gm.GetFlag(261) == 1)
			{
				if ((int)gm.GetFlag(87) >= 5)
				{
					StartText(new string[3] { "* Should I even be\n  surprised?", "* You're the same one\n  that was willing to\n  kill begging humans.", "* Of course you'd do\n  this to a poor\n  defenseless dog." }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_disappointed", "su_side", "su_annoyed" });
					return;
				}
				StartText(new string[7] { "* The hell is wrong\n  with you??!", "* Can you PLEASE not\n  do shit like that???", "* Really makes me\n  reconsider traveling\n  alongside you.", "* Susie,^05 what about\n  the...", "* The what?", "* Oh,^05 that.", "* Ugh..." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[7] { "su_pissed", "su_pissed", "su_annoyed", "no_thinking", "su_annoyed", "su_surprised", "su_depressed" });
			}
			else if ((int)gm.GetFlag(87) >= 5)
			{
				StartText(new string[3] { "* Should I even be\n  surprised?", "* You're the same one\n  that was willing to\n  kill begging humans.", "* Of course you'd do\n  this to a poor\n  defenseless dog." }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_disappointed", "su_side", "su_annoyed" });
			}
			else
			{
				StartText(new string[7] { "* Kris,^05 we REALLY didn't\n  need to kill that\n  dog.", "* Honestly kind of\n  horrific you chose to\n  do that.", "* Really makes me\n  reconsider traveling\n  alongside you.", "* Susie,^05 what about\n  the...", "* The what?", "* Oh,^05 that.", "* Ugh..." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[7] { "su_annoyed", "su_annoyed", "su_annoyed", "no_thinking", "su_annoyed", "su_surprised", "su_depressed" });
			}
		}
		else if (endState == 2)
		{
			Object.FindObjectOfType<LesserDogSentry>().SparedDog();
			if ((int)Util.GameManager().GetFlag(12) == 1)
			{
				gm.StopMusic();
				WeirdChecker.Abort(gm);
				StartText(new string[7] { "* So...", "* It didn't take a\n  couple of kids for\n  you to stop.", "* It took a really\n  sad dog.", "* ...", "* (Humans are fucking\n  weird...)", "* Well,^05 whatever.", "* Let's go." }, new string[1] { "snd_txtsus" }, new int[1], new string[7] { "su_annoyed", "su_annoyed", "su_annoyed", "su_side", "su_side_sweat", "su_smirk", "su_smirk" });
			}
			else
			{
				EndCutscene();
			}
		}
	}
}
