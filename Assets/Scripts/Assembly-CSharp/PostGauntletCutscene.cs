using UnityEngine;

public class PostGauntletCutscene : CutsceneBase
{
	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 75)
			{
				SetSprite(kris, "spr_kr_sit");
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 90)
			{
				bool flag = Util.GameManager().GetFlagInt(87) >= 7;
				if (flag)
				{
					StartText(new string[6] { "* There we go.^05\n* They're awake.", "* Careful where you're\n  throwing people next\n  time,^05 Kris.", "* ...", "* I got a bad feeling\n  about what's ahead...", "* But that's kinda just\n  been this whole\n  adventure.", "* Let's just get going." }, new string[1] { "snd_txtsus" }, new int[1], new string[6] { "su_smirk", "su_annoyed", "su_dejected", "su_side", "su_side_sweat", "su_annoyed" });
				}
				else
				{
					StartText(new string[6] { "* There we go.^05\n* They're awake.", "* Careful where you're\n  throwing people next\n  time,^05 Kris.", "* Fahaha!^05\n* Says YOU,^05 Susie!", "* You were the one\n  who ran into me\n  and made me fall!", "* HEY!!!", "* ..." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], new string[6] { "su_smirk", "su_annoyed", "no_playful", "no_happy", "su_wtf", "su_dejected" });
				}
				state = ((!flag) ? 1 : 5);
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.left);
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
				else if (AtLine(3))
				{
					SetSprite(noelle, "spr_no_laugh_0");
				}
				else if (AtLine(5))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(6))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.up);
					noelle.EnableAnimator();
				}
				return;
			}
			frames++;
			if (frames == 40)
			{
				PlayAnimation(susie, "Embarrassed");
			}
			if (frames == 60)
			{
				StartText(new string[7] { "* Sorry....", "* I-^05it's okay!", "* ...", "* Say,^05 what happened\n  to Sans?", "*\ti'm over here.", "* HUH???\n^05* YOU COME BACK FOR\n  MORE??!", "*\tnah.^05\n*\tlook over the big ol' mound." }, new string[7] { "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsans", "snd_txtsus", "snd_txtsans" }, new int[1], new string[7] { "su_depressed", "no_surprised_happy", "no_silent_side", "no_confused", "", "su_angry", "" });
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					ChangeDirection(noelle, Vector2.up);
				}
				else if (AtLine(4))
				{
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(5))
				{
					SetSprite(kris, "spr_kr_surprise");
					SetSprite(susie, "spr_su_surprise_right");
					SetSprite(noelle, "spr_no_surprise");
				}
				else if (AtLine(6))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				kris.EnableAnimator();
				susie.EnableAnimator();
				noelle.EnableAnimator();
				PlayAnimation(susie, "idle");
				ChangeDirection(kris, Vector2.up);
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
			}
			cam.transform.position = new Vector3(1.8f, Mathf.Lerp(0f, 6.81f, (float)frames / 60f), -10f);
			if (frames == 90)
			{
				StartText(new string[1] { "*\theya.^05\n*\thow was the fall?" }, new string[1] { "snd_txtsans" }, new int[1], new string[1] { "sans_wink" }, 1);
				state = 3;
				frames = 0;
			}
		}
		else if (state == 3 && !txt)
		{
			frames++;
			cam.transform.position = new Vector3(1.8f, Mathf.Lerp(6.81f, 0f, (float)frames / 30f), -10f);
			if (frames == 30)
			{
				StartText(new string[11]
				{
					"* Oh.", "* Not THAT Sans!", "* The one that...^10\n  bounced down with us.", "* Oh shoot,^05 you're right!", "* God,^05 I'm REALLY getting\n  sick of this crazy\n  place.", "* Can we just find\n  a grey door and\n  go back to,^05 y'know...", "* The NICER underground\n  place we were in?", "* That Sans better not\n  show his ugly mug\n  around us again.", "*\toh,^05 MY ugly mug?", "* NOT YOU!!!",
					"* Let's just go."
				}, new string[11]
				{
					"snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsans", "snd_txtsus",
					"snd_txtsus"
				}, new int[1], new string[11]
				{
					"su_side", "no_playful", "no_weird", "su_surprised", "su_annoyed", "su_annoyed", "su_annoyed", "su_angry", "sans_side", "su_wtf",
					"su_annoyed"
				});
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(4))
				{
					SetSprite(susie, "spr_su_surprise_right");
				}
				else if (AtLine(5))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(8))
				{
					ChangeDirection(susie, Vector2.left);
				}
				else if (AtLine(9))
				{
					ChangeDirection(kris, Vector2.up);
					ChangeDirection(susie, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
				}
				else if (AtLine(10))
				{
					SetSprite(susie, "spr_su_point_up_0");
				}
				else if (AtLine(11))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
					ChangeDirection(susie, Vector2.left);
					susie.EnableAnimator();
				}
			}
			else if (!MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				EndCutscene();
			}
		}
		else if (state == 5)
		{
			if (!txt)
			{
				state = 4;
				kris.EnableAnimator();
			}
			else if (AtLine(2))
			{
				susie.EnableAnimator();
				ChangeDirection(susie, Vector2.left);
				susie.GetComponent<SpriteRenderer>().flipX = false;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		gm.SetFlag(246, 1);
		fade.FadeIn(60, Color.black);
		kris.transform.position = new Vector3(1.8f, -1.09f);
		SetSprite(kris, "spr_kr_ko");
		susie.transform.position = new Vector3(3.37f, -1.03f);
		SetSprite(susie, "spr_su_kneel", true);
		noelle.transform.position = new Vector3(5.33f, -0.8f);
		ChangeDirection(noelle, Vector2.left);
		cam.transform.position = new Vector3(1.8f, 0f, -10f);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		gm.SetFlag(1, "inquisitive");
		gm.SetFlag(2, "shocked");
	}
}
