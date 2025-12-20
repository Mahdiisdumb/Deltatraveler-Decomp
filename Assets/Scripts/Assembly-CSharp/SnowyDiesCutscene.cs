using UnityEngine;

public class SnowyDiesCutscene : CutsceneBase
{
	private bool oblit;

	private bool noelleMove;

	private int rageFrames;

	private bool ended;

	private void Update()
	{
		if (state == 0)
		{
			if ((bool)txt)
			{
				if (AtLine(2) || AtLine(7))
				{
					PlayAnimation(susie, "idle");
					ChangeDirection(susie, Vector2.left);
					susie.UseUnhappySprites();
				}
				else if (AtLine(3))
				{
					ChangeDirection(noelle, Vector2.right);
				}
				else if (AtLine(5))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(6))
				{
					PlayAnimation(susie, "Embarrassed");
				}
			}
			else if (oblit)
			{
				gm.StopMusic();
				ChangeDirection(noelle, Vector2.up);
				state = 2;
				StartText(new string[9] { "* ...", "* That was Snowy,^05\n  Susie.", "* We just killed one\n  of our classmates.", "* Noelle,^05 that could've\n  been^05--", "* That was Snowy!!", "* The only blue snowdrake\n  in town,^05 not counting\n  his dad?", "* He had the same\n  voice,^05 Susie.", "* And I know that\n  you know,^05 Susie.", "* ..." }, new string[9] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[9] { "no_mad_closed", "no_mad", "no_mad_closed", "su_concerned", "no_angry", "no_mad", "no_mad", "no_mad", "su_dejected" });
			}
			else
			{
				state = 1;
				StartText(new string[6] { "* But weren't him and\n  his dad the only\n  blue ones in town?", "* And the way he\n  looked when we...", "* Well we aren't gonna\n  know now.", "* Even if it was\n  him, he'd prolly get\n  killed by someone else.", "* ...", "* Anyway,^05 let's get going\n  before we get killed\n  ourselves." }, new string[6] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[6] { "no_shocked", "no_depressed_side", "su_annoyed", "su_dejected", "no_depressed", "su_annoyed" });
			}
		}
		else if (state == 1)
		{
			if (!txt && !MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				Util.FindObjectOfType<SectionTitleCard>().Activate();
				EndCutscene();
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					txt.ForceAdvanceCurrentLine();
				}
				else if (AtLine(5))
				{
					noelleMove = true;
					ChangeDirection(noelle, Vector2.right);
					SetMoveAnim(noelle, isMoving: true);
					noelle.SetCustomSpritesetPrefix("pissed");
				}
				else if (AtLine(9))
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
					PlayAnimation(susie, "idle");
					ChangeDirection(susie, Vector2.up);
				}
				if (noelleMove)
				{
					rageFrames++;
					MoveTo(noelle, susie.transform.position, 2f);
					if (rageFrames == 5 && txt.GetCurrentStringNum() < 9)
					{
						SetSprite(susie, "spr_su_surprise_right", flipX: true);
					}
					if (rageFrames >= 10)
					{
						SetMoveAnim(noelle, isMoving: false);
						noelleMove = false;
					}
				}
			}
			else
			{
				if (noelleMove)
				{
					SetMoveAnim(noelle, isMoving: false);
				}
				frames++;
				if (frames == 1)
				{
					ChangeDirection(noelle, Vector2.left);
					noelle.SetCustomSpritesetPrefix("");
				}
				if (frames == 30)
				{
					StartText(new string[2] { "* I'm going to go\n  ahead of you two.", "* I can't take this\n  anymore." }, new string[2] { "snd_txtnoe", "snd_txtnoe" }, new int[1], new string[2] { "no_depressedx", "no_depressedx" });
					frames = 0;
					state = 3;
				}
			}
		}
		else if (state == 3 && !txt)
		{
			if (MoveTo(noelle, new Vector3(17.6f, -2.33f), 4f))
			{
				ChangeDirection(noelle, Vector2.right);
				SetMoveAnim(noelle, isMoving: true);
				if (noelle.transform.position.x > 10.58f)
				{
					ChangeDirection(kris, Vector2.right);
				}
				else
				{
					ChangeDirection(kris, Vector2.up);
				}
				return;
			}
			frames++;
			if (frames == 10)
			{
				ChangeDirection(susie, Vector2.right);
			}
			if (frames == 40)
			{
				StartText(new string[10] { "* Damn it...", "* Well,^05 Kris.", "* Sorry to do this\n  to you,^05 but...", "* I need to catch\n  up with Noelle.", "* I'm not gonna be\n  responsible for her\n  dying here.", "* Besides...", "* You're already \"strong\n  enough,\"^05 right?", "* Don't really need my\n  help right now.", "* ...", "* Good luck,^05 I guess." }, new string[1] { "snd_txtsus" }, new int[1], new string[10] { "su_depressed", "su_neutral", "su_side", "su_annoyed", "su_dejected", "su_side", "su_smirk_sweat", "su_smile_sweat", "su_dejected", "su_dejected" });
				frames = 0;
				state = 4;
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
				if (AtLine(2) || AtLine(6))
				{
					susie.ChangeDirection(Vector2.down);
					kris.ChangeDirection(Vector2.up);
				}
				else if (AtLine(5) || AtLine(10))
				{
					susie.ChangeDirection(Vector2.right);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(susie, isMoving: true);
			}
			MoveTo(susie, new Vector3(17.6f, -2.33f), 6f);
			if (frames == 10)
			{
				ChangeDirection(kris, Vector2.right);
			}
			if (frames >= 30 && !ended && !MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				Util.FindObjectOfType<SectionTitleCard>().Activate();
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				gm.EnablePlayerMovement();
				gm.SetCheckpoint(76);
				ended = true;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		if ((int)gm.GetFlag(13) == 7)
		{
			gm.SetFlag(13, 8);
			gm.SetFlag(87, 8);
			oblit = true;
			gm.SetPartyMembers(susie: false, noelle: false);
		}
		noelle.ChangeDirection(Vector2.up);
		noelle.UseUnhappySprites();
		StartText(new string[7] { "* ...", "* ...^10 You good,^05 Noelle?", "* Susie,^05 we didn't just...", "* ... kill Snowy,^05 did\n  we?", "* Uhh...^10 nah.", "* He was part of like...^05\n  a bird species,^05 right?", "* That was prolly some\n  other killer snowdrake." }, new string[7] { "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[7] { "no_depressed_look", "su_smile_sweat", "no_depressed_side", "no_depressed", "su_annoyed", "su_smirk_sweat", "su_neutral" });
	}
}
