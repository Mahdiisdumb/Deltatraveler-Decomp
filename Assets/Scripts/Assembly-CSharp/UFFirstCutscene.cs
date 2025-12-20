using UnityEngine;

public class UFFirstCutscene : CutsceneBase
{
	private bool obligatory;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 75)
			{
				susie.SetSprite("spr_su_kneel");
				PlaySFX("sounds/snd_wing", 0.9f);
			}
			if (frames >= 75 && frames <= 78)
			{
				int num = ((frames % 2 == 0) ? 1 : (-1));
				int num2 = 78 - frames;
				susie.transform.position = new Vector3(4.28f, -0.33f) + new Vector3((float)(num2 * num) / 24f, 0f);
			}
			if (frames == 85)
			{
				kris.SetSprite("spr_kr_sit_injured");
				noelle.SetSprite("spr_no_kneel_left");
				PlaySFX("sounds/snd_wing", 0.9f);
			}
			if (frames >= 85 && frames <= 88)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				int num4 = 88 - frames;
				kris.transform.position = new Vector3(-0.08f, -1.06f) + new Vector3((float)(num4 * num3) / 24f, 0f);
				noelle.transform.position = new Vector3(1.23f, 0.19f) + new Vector3((float)(num4 * num3) / 24f, 0f);
			}
			if (frames == 90)
			{
				StartText(new string[2] { "* Ugh...", "* I swear,^05 everytime we\n  fall,^05 it's like we've\n  been sleeping for hours." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[2], new string[2] { "su_depressed", "su_neutral" });
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.EnableAnimator();
				kris.SetSelfAnimControl(false);
				susie.GetComponent<SpriteRenderer>().flipX = false;
				susie.EnableAnimator();
				susie.SetSelfAnimControl(false);
				noelle.EnableAnimator();
				noelle.SetSelfAnimControl(false);
				susie.UseUnhappySprites();
				noelle.UseUnhappySprites();
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 25)
			{
				kris.ChangeDirection(Vector2.left);
				noelle.ChangeDirection(Vector2.left);
				susie.ChangeDirection(Vector2.right);
			}
			if (frames == 45)
			{
				kris.ChangeDirection(Vector2.up);
				noelle.ChangeDirection(Vector2.right);
				susie.ChangeDirection(Vector2.left);
			}
			if (frames == 65)
			{
				kris.ChangeDirection(Vector2.down);
				noelle.ChangeDirection(Vector2.up);
				susie.ChangeDirection(Vector2.right);
			}
			if (frames == 105)
			{
				kris.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
				if ((int)gm.GetFlag(13) >= 7)
				{
					gm.SetFlag(167, 1);
					gm.SetFlag(155, 1);
					susie.ChangeDirection(Vector2.up);
					StartText(new string[4] { "* Hey,^05 we made it\n  back to that forest.", "* ...", "* ...^05 Yeah,^05 I'm not\n  feeling the best\n  either.", "* Let's just cut the\n  crap and make a\n  beeline to Hotland." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[2], new string[4] { "su_smirk", "su_neutral", "su_depressed", "su_depressed" });
					state = 4;
				}
				else
				{
					susie.ChangeDirection(Vector2.left);
					susie.DisableAnimator();
					susie.SetSprite("spr_su_pose");
					StartText(new string[7] { "* Hey,^05 we made it\n  back to that forest!", "* ... I'm not too sure.", "* Uhh...^10 what.", "* It's snowy.^05\n* It's a forest.\n^05* It's got a box.", "* Dunno how you'd get\n  much different than\n  that.", "* The trees weren't this...^10\n  dead.", "* And the ground feels...^05\n  strangely tepid." }, new string[7] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe" }, new int[2], new string[7] { "su_smile", "no_thinking", "su_inquisitive", "su_smile_sweat", "su_smirk", "no_depressed_side", "no_depressed" });
					state = 2;
				}
				frames = 0;
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3 && !susie.GetComponent<Animator>().enabled)
				{
					susie.EnableAnimator();
				}
				else if (txt.GetCurrentStringNum() == 5 && susie.GetComponent<Animator>().enabled)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_shrug");
				}
				else if (txt.GetCurrentStringNum() == 6 && !susie.GetComponent<Animator>().enabled)
				{
					susie.EnableAnimator();
					noelle.ChangeDirection(Vector2.up);
				}
				else if (txt.GetCurrentStringNum() == 7 && noelle.GetComponent<Animator>().enabled)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_kneel_right");
					PlaySFX("sounds/snd_wing");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					noelle.ChangeDirection(Vector2.right);
					noelle.EnableAnimator();
				}
				if (frames == 20)
				{
					susie.GetComponent<SpriteRenderer>().flipX = true;
					susie.DisableAnimator();
					susie.SetSprite("spr_su_kneel");
					PlaySFX("sounds/snd_wing");
				}
				if (frames == 60)
				{
					StartText(new string[7] { "* ...", "* You're right,^05 WHERE THE\n  HELL ARE WE???", "* Hmm...", "* I dunno,^05 maybe that\n  scientist guy is in\n  this world,^05 too.", "* You mean Alphys?", "* WHOEVER the royal\n  scientist is,^05 WE WILL\n  SEE THEM!!!", "* C'mon,^05 let's go." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[2], new string[7] { "su_wideeye", "su_wtf", "su_neutral", "su_smirk_sweat", "no_happy", "su_angry", "su_smile" });
					state = 3;
					frames = 0;
				}
			}
		}
		if (state == 3)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && !obligatory)
				{
					obligatory = true;
					susie.SetSprite("spr_su_pissed");
					susie.GetComponent<SpriteRenderer>().flipX = false;
					PlaySFX("sounds/snd_whip_hard");
				}
				else if (txt.GetCurrentStringNum() == 3 && !susie.GetComponent<Animator>().enabled)
				{
					susie.EnableAnimator();
					susie.ChangeDirection(Vector2.up);
				}
				else if (txt.GetCurrentStringNum() == 4)
				{
					susie.GetComponent<Animator>().Play("Embarrassed");
				}
				else if (txt.GetCurrentStringNum() == 5 && noelle.GetComponent<Animator>().enabled)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_laugh_0");
				}
				else if (txt.GetCurrentStringNum() == 6 && susie.GetComponent<Animator>().enabled)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wtf");
					susie.GetComponent<SpriteRenderer>().flipX = true;
				}
				else if (txt.GetCurrentStringNum() == 7 && obligatory)
				{
					obligatory = false;
					susie.SetSprite("spr_su_throw_ready");
					PlaySFX("sounds/snd_wing");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.left);
					susie.ChangeDirection(Vector2.left);
					susie.EnableAnimator();
					susie.GetComponent<Animator>().Play("idle");
					susie.GetComponent<SpriteRenderer>().flipX = false;
					susie.UseHappySprites();
					noelle.UseHappySprites();
				}
				if (cam.transform.position != cam.GetClampedPos())
				{
					cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
				}
				else
				{
					kris.ChangeDirection(Vector2.down);
					kris.SetSelfAnimControl(true);
					susie.SetSelfAnimControl(true);
					noelle.SetSelfAnimControl(true);
					cam.SetFollowPlayer(true);
					gm.SetCheckpoint(74);
					EndCutscene();
				}
			}
		}
		if (state != 4)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2)
			{
				susie.ChangeDirection(Vector2.left);
			}
			else if (txt.GetCurrentStringNum() == 3)
			{
				susie.ChangeDirection(Vector2.up);
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			noelle.EnableAnimator();
			noelle.ChangeDirection(Vector2.left);
			susie.ChangeDirection(Vector2.left);
		}
		if (cam.transform.position != cam.GetClampedPos())
		{
			cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
			return;
		}
		kris.ChangeDirection(Vector2.down);
		kris.SetSelfAnimControl(true);
		susie.SetSelfAnimControl(true);
		noelle.SetSelfAnimControl(true);
		cam.SetFollowPlayer(true);
		gm.SetCheckpoint(74);
		EndCutscene();
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetFlag(84, 5);
		gm.SetFlag(168, 1);
		gm.SetFlag(64, 2);
		kris.DisableAnimator();
		kris.transform.position = new Vector3(-0.08f, -1.06f);
		susie.DisableAnimator();
		susie.transform.position = new Vector3(4.28f, -0.33f);
		noelle.DisableAnimator();
		noelle.transform.position = new Vector3(1.23f, 0.19f);
		kris.SetSprite("spr_kr_ko");
		susie.SetSprite("spr_su_ko");
		susie.GetComponent<SpriteRenderer>().flipX = true;
		noelle.SetSprite("spr_no_collapsed");
		cam.SetFollowPlayer(false);
		cam.transform.position = new Vector3(2.12f, 0f, -10f);
		if (PlayerPrefs.GetInt("CompletionState", 0) < 2)
		{
			PlayerPrefs.SetInt("CompletionState", 2);
		}
		if (Object.FindObjectOfType<GameManager>().GetItemList().Contains(16))
		{
			Object.FindObjectOfType<GameManager>().RemoveItem(Object.FindObjectOfType<GameManager>().GetItemList().IndexOf(16));
		}
		fade.FadeIn(60);
	}
}
