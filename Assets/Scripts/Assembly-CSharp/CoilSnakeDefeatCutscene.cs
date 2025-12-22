using UnityEngine;

public class CoilSnakeDefeatCutscene : CutsceneBase
{
	private bool selecting;

	private int endState;

	private bool onOblit;

	private Vector3 susiePos = Vector3.zero;

	private bool damage;

	private bool prevOblit;

	private void Update()
	{
		if (state == 0)
		{
			if (!txt)
			{
				if (onOblit && !prevOblit && endState == 1)
				{
					state = 1;
					susiePos = kris.transform.position + new Vector3(-0.879f, 0.165f);
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.GetComponent<Animator>().SetFloat("speed", 0.5f);
					if (susie.transform.position.x < susiePos.x)
					{
						susie.ChangeDirection(Vector2.right);
					}
					else
					{
						susie.ChangeDirection(Vector2.left);
					}
				}
				else
				{
					if ((int)gm.GetFlag(12) == 1)
					{
						WeirdChecker.Abort(Util.GameManager());
					}
					EndCutscene();
				}
			}
			else if (!txt && onOblit && endState == 1)
			{
				state = 1;
				susiePos = kris.transform.position + new Vector3(-0.879f, 0.165f);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.GetComponent<Animator>().SetFloat("speed", 0.5f);
				if (susie.transform.position.x < susiePos.x)
				{
					susie.ChangeDirection(Vector2.right);
				}
				else
				{
					susie.ChangeDirection(Vector2.left);
				}
			}
		}
		if (state == 1)
		{
			if (susie.transform.position != susiePos)
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, susiePos, 1f / 24f);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.GetComponent<SpriteRenderer>().enabled = false;
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					susie.GetComponent<Animator>().SetFloat("speed", 0f);
					susie.GetComponent<Animator>().Play("GraspKris");
				}
				if (frames == 20)
				{
					StartText(new string[1] { "* ... ^15Kris." }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_depressed" });
					state = 2;
					frames = 0;
				}
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				noelle.SetSprite("spr_no_surprise");
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				PlaySFX("sounds/snd_grab");
			}
			if (frames == 30)
			{
				StartText(new string[7] { "* What the HELL is\n  your problem???", "* We do NOT need to\n  be hitting this hard!", "* Especially something as\n  harmless as that snake!", "* ...", "* What the hell is\n  <color=#FF0000FF>\"I can't control myself\"</color>\n  supposed to mean???", "* YOU KNOW WHAT YOU'RE\n  DOING!!!", "* S-Susie,^05 stop!!!" }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[18], new string[7] { "su_serious", "su_serious", "su_serious", "su_serious", "su_angry_hero", "su_teeth_determined", "no_afraid_open" });
				state = 3;
				frames = 0;
			}
		}
		if (state == 3)
		{
			if (!txt)
			{
				if (damage)
				{
					damage = false;
					frames = 0;
					susie.transform.position = susiePos;
					susie.DisableAnimator();
					susie.SetSprite("spr_su_grasp_kris_letgo");
					kris.DisableAnimator();
					kris.GetComponent<SpriteRenderer>().enabled = true;
					kris.SetSprite("spr_kr_grasped");
					noelle.EnableAnimator();
				}
				frames++;
				float num = (float)frames / 5f;
				kris.GetComponent<SpriteRenderer>().sortingOrder = susie.GetComponent<SpriteRenderer>().sortingOrder - 1;
				if (frames <= 5)
				{
					kris.transform.position = susiePos + Vector3.Lerp(new Vector3(1.146f, -0.102000006f), new Vector3(1.146f, -0.26500002f), num * num);
				}
				else if (frames == 6)
				{
					kris.transform.position = susiePos + new Vector3(1.146f, -0.165f);
					kris.SetSprite("spr_kr_sit");
					kris.GetComponent<SpriteRenderer>().flipX = true;
					PlaySFX("sounds/snd_noise");
				}
				if (frames == 30)
				{
					susie.SetSprite("spr_su_left_sad_0");
					StartText(new string[7] { "* Noelle...", "* Susie,^05 there's\n  something...^10 wrong with\n  Kris.", "* Think about it...", "* They've been doing\n  things that they'd\n  never do before.", "* And it's not just here.", "* They've been acting\n  strange since at least\n  a few days ago.", "* ... They might be right." }, new string[7] { "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[7] { "su_concerned", "no_afraid", "no_depressed", "no_depressed", "no_depressed_side", "no_depressed_side", "no_depressed" });
					state = 4;
					frames = 0;
				}
			}
			else if (txt.GetCurrentStringNum() >= 6)
			{
				frames++;
				damage = true;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_damage");
				}
				if (frames <= 8)
				{
					int num2 = ((frames % 2 == 0) ? 1 : (-1));
					int num3 = 8 - frames;
					susie.transform.position = susiePos + new Vector3((float)(num3 * num2) / 24f, 0f);
				}
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				susie.EnableAnimator();
				susie.GetComponent<Animator>().Play("idle");
				susie.ChangeDirection(Vector2.up);
			}
			if (frames == 40)
			{
				StartText(new string[7] { "* ...", "* Kris.", "* You have some\n  explaining to do.", "* Next time we have a\n  break,^05 we're gonna talk\n  about this.", "* Right now,^05 we need\n  to keep going.", "* Just,^05 if you're able\n  to do something about\n  this,^05 please do.", "* I really don't wanna\n  go around spilling\n  blood everywhere." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[7] { "su_depressed", "su_side", "su_neutral", "su_side", "su_annoyed", "su_dejected", "su_dejected" });
				state = 5;
				frames = 0;
			}
		}
		if (state != 5)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2)
			{
				susie.ChangeDirection(Vector2.right);
				kris.GetComponent<SpriteRenderer>().flipX = false;
				kris.EnableAnimator();
				kris.ChangeDirection(Vector2.left);
			}
		}
		else if (!txt)
		{
			if (frames == 0)
			{
				gm.PlayGlobalSFX("sounds/snd_ominous");
				frames++;
			}
			if (cam.transform.position != cam.GetClampedPos())
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 1f / 24f);
				return;
			}
			gm.SetFlag(1, "depressed");
			gm.SetFlag(2, "depressed");
			kris.SetSelfAnimControl(setAnimControl: true);
			susie.SetSelfAnimControl(setAnimControl: true);
			noelle.SetSelfAnimControl(setAnimControl: true);
			cam.SetFollowPlayer(follow: true);
			EndCutscene();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (!(index == Vector2.left))
		{
			_ = index == Vector2.right;
		}
		state = 1;
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		gm.SetFlag(89, endState);
		onOblit = (int)gm.GetFlag(13) >= 4;
		prevOblit = (int)gm.GetFlag(13) == 3;
		if (endState == 1)
		{
			if (onOblit)
			{
				Util.FindObjectOfType<OverworldCoilSnake>().CreateDeadEnemy();
				gm.StopMusic();
				cam.SetFollowPlayer(follow: false);
				kris.ChangeDirection(Vector2.right);
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
				susie.UseUnhappySprites();
				susie.SetSelfAnimControl(setAnimControl: false);
				noelle.transform.position = new Vector3(noelle.transform.position.x, kris.transform.position.y + noelle.GetPositionOffset().y);
				noelle.UseUnhappySprites();
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_right_shocked_0");
				StartText(new string[5] { "* ...", "* ...", "* K-^05Kris...?", "* Did we really just...\n  ^15do that?", "* W-^05w-^05was that...\n  ^15blood??" }, new string[5] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[18], new string[5] { "no_shocked", "su_concerned", "su_concerned", "su_concerned", "no_afraid" });
			}
			else if (prevOblit)
			{
				StartText(new string[6] { "* Oh shoot,^05 it ran away???", "* Hell yea!", "* Wow,^05 Susie,^05 you're\n  really happy about that.", "* Is everything okay?", "* It is now.", "* ?????" }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[18], new string[6] { "su_surprised", "su_excited", "no_confused", "no_confused_side", "su_smirk", "no_happy" });
			}
			else
			{
				StartText(new string[4] { "* IT RAN AWAY???", "* GET BACK HERE YOU\n  LITTLE--", "* H-hey,^05 at least it's\n  out of the way.", "* I guess..." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[18], new string[4] { "su_angry", "su_wtf", "no_happy", "su_annoyed" });
			}
		}
		else if (endState == 2)
		{
			if (onOblit)
			{
				StartText(new string[7] { "* ...", "* For some reason...", "* I felt really anxious.", "* Why...?", "* Dunno.", "* I was kinda afraid that\n  the snake's guts would\n  come flying out.", "* ?????????" }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[18], new string[7] { "su_side_sweat", "su_side_sweat", "su_side_sweat", "no_curious", "su_neutral", "su_dejected", "no_speechless" });
			}
			else if (prevOblit)
			{
				WeirdChecker.Abort(Util.GameManager());
				EndCutscene();
			}
			else
			{
				EndCutscene();
			}
		}
		else
		{
			EndCutscene();
		}
	}
}
