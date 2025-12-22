using UnityEngine;

public class DogiPostCutscene : CutsceneBase
{
	private InteractTextBox dogamy;

	private InteractTextBox dogaressa;

	private Vector3 camPos;

	private int endState;

	private bool replayTrollDeath;

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
				if (AtLine(2))
				{
					dogamy.SetTalkable(null);
					dogaressa.SetTalkable(txt);
				}
				else if (AtLine(4))
				{
					dogamy.SetTalkable(txt);
					dogaressa.SetTalkable(null);
				}
				return;
			}
			if (frames == 0)
			{
				PlayAnimation(dogamy, "Walk");
				PlayAnimation(dogaressa, "Walk");
				frames++;
			}
			if (dogamy.transform.parent.position.y != 2.02f)
			{
				MoveTo(dogamy.transform.parent, new Vector3(8.43f, 2.02f), 6f);
				return;
			}
			if (MoveTo(dogamy.transform.parent, new Vector3(-1.04f, 2.02f), 6f))
			{
				dogamy.GetComponent<SpriteRenderer>().flipX = true;
				return;
			}
			frames++;
			if (frames == 30)
			{
				ChangeDirection(susie, Vector2.left);
				ChangeDirection(kris, Vector2.right);
				ChangeDirection(noelle, Vector2.left);
				StartText(new string[7] { "* So like...", "* Did you just want\n  to desperately dodge\n  their attacks?", "* I'm not an expert\n  or anything,^05 but like...", "* You probably didn't\n  need to murder kids to\n  do that.", "* Seems like something\n  you could pay $10 to\n  do.", "* What are you talking\n  about,^05 Susie?", "* I dunno.^05\n* Let's just go." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[7] { "su_side", "su_annoyed", "su_side", "su_neutral", "su_smile_side", "no_thinking", "su_smirk_sweat" });
				Object.Destroy(dogamy.transform.parent.gameObject);
				state = 1;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.right);
				}
				else if (AtLine(7))
				{
					ChangeDirection(susie, Vector2.left);
				}
			}
			else if (!MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				gm.PlayMusic("zoneMusic");
				EndCutscene();
			}
		}
		else if (state == 2)
		{
			frames++;
			if (frames == 45)
			{
				ChangeDirection(susie, Vector2.left);
				ChangeDirection(kris, Vector2.right);
				ChangeDirection(noelle, Vector2.left);
				StartText(new string[6] { "* Yeesh...^05 you really do\n  just...^05 hit like a\n  truck on your own.", "* It's so...^05 scary how\n  you did that...", "* How the hell are we\n  even doing this much\n  harm?", "* Surely it isn't just\n  as simple as \"we're\n  getting stronger.\"", "* Cuz the only results\n  I'm seeing are dead\n  bodies.", "* I hope we can find\n  out what the hell\n  is happening soon." }, new string[6] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[6] { "su_concerned", "no_shocked", "su_inquisitive", "su_neutral", "su_depressed", "su_dejected" });
				state = 3;
			}
		}
		else if (state == 3 && !txt)
		{
			if (!MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				gm.PlayMusic("zoneMusic");
				EndCutscene();
			}
		}
		else if (state == 4)
		{
			frames++;
			if (frames == 30)
			{
				ChangeDirection(kris, Vector2.right);
				ChangeDirection(susie, Vector2.left);
				ChangeDirection(noelle, Vector2.left);
				if (replayTrollDeath)
				{
					StartText(new string[3] { "* That is actually\n  fucked up.", "* If you wanted to\n  kill them,^05 you should've\n  done BOTH of them.", "* No,^05 y'know--^10 " }, new string[1] { "snd_txtsus" }, new int[1], new string[3] { "su_disappointed", "su_pissed", "su_annoyed" });
				}
				else
				{
					StartText(new string[4] { "* That is actually\n  fucked up.", "* If you wanted to\n  kill them,^05 you should've\n  done BOTH of them.", "* No,^05 y'know what?^05\n* Rip that heart out.", "* ...^05 The hell are\n  you hesitating for?^05\n* You wanted this." }, new string[1] { "snd_txtsus" }, new int[1], new string[4] { "su_disappointed", "su_pissed", "su_annoyed", "su_smirk_sweat" });
				}
				state = 5;
				frames = 0;
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					susie.ChangeDirection(Vector2.up);
				}
				else if (AtLine(3))
				{
					SetSprite(susie, "spr_su_throw_ready", flipX: true);
					if (replayTrollDeath)
					{
						ChangeDirection(kris, Vector2.up);
						txt.ForceAdvanceCurrentLine();
					}
					else
					{
						SetSprite(kris, "spr_kr_surprise");
					}
				}
				else if (AtLine(4))
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.left);
					kris.EnableAnimator();
					ChangeDirection(kris, Vector2.up);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlayAnimation(kris, "RemoveSoul_WalkUp");
			}
			if (frames == 15 && replayTrollDeath)
			{
				susie.GetComponent<SpriteRenderer>().flipX = false;
				susie.EnableAnimator();
				ChangeDirection(susie, Vector2.left);
			}
			if (frames == 45)
			{
				SetSprite(noelle, "spr_no_surprise_left");
				if (replayTrollDeath)
				{
					SetSprite(susie, "spr_su_surprise_right", flipX: true);
				}
			}
			if (frames >= 38 && frames <= 41)
			{
				int num = ((frames % 2 == 0) ? 1 : (-1));
				int num2 = 41 - frames;
				kris.transform.position = new Vector3(7.302f, -1.63f) + new Vector3((float)(num2 * num) / 24f, 0f);
			}
			if (frames >= 48 && frames <= 51)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				int num4 = 51 - frames;
				kris.transform.position = new Vector3(7.302f, -1.63f) + new Vector3((float)(num4 * num3) / 24f, 0f);
			}
			if (frames >= 58 && frames <= 61)
			{
				int num5 = ((frames % 2 == 0) ? 1 : (-1));
				int num6 = 61 - frames;
				kris.transform.position = new Vector3(7.302f, -1.63f) + new Vector3((float)(num6 * num5) / 24f, 0f);
			}
			if (frames >= 64 && frames <= 67)
			{
				int num7 = ((frames % 2 == 0) ? 1 : (-1));
				int num8 = 67 - frames;
				kris.transform.position = new Vector3(7.302f, -1.63f) + new Vector3((float)(num8 * num7) / 24f, 0f);
			}
			if (frames == 38 || frames == 48 || frames == 58)
			{
				PlaySFX("sounds/snd_bump");
			}
			if (frames == 64)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
				PlaySFX("sounds/snd_grab");
			}
			if (frames == 110)
			{
				if (replayTrollDeath)
				{
					StartText(new string[7] { "* T-^05that's so\n  horrifying...!", "* Kris,^05 I didn't even\n  get to finish...", "* No,^05 I'm with you.", "* Let's play some\n  baseball.", "* Was about to say\n  something like that.", "* So,^05 go on.^05\n* Back up.", "* And uhh...^05 I guess\n  get out of the\n  way,^05 Noelle." }, new string[5] { "snd_txtnoe", "snd_txtsus", "snd_txtkrs", "snd_txtkrs", "snd_txtsus" }, new int[1], new string[7] { "no_afraid", "su_surprised", "kr_relieved_side", "kr_smug", "su_smile_sweat", "su_confident", "su_smirk_sweat" });
				}
				else
				{
					StartText(new string[6] { "* T-^05that's so\n  horrifying...!", "* What...^10 do you want\n  to do to it?", "* Let's play some\n  baseball.", "* What?", "* Back up real quick.", "* And uhh...^05 I guess\n  get out of the\n  way,^05 Noelle." }, new string[5] { "snd_txtnoe", "snd_txtkrs", "snd_txtsus", "snd_txtkrs", "snd_txtsus" }, new int[3] { 0, 1, 0 }, new string[6] { "no_afraid", "kr_g_1", "su_smile_side", "kr_weird", "su_annoyed", "su_smirk_sweat" });
				}
				state = 6;
				frames = 0;
			}
		}
		else if (state == 6)
		{
			if ((bool)txt)
			{
				if (replayTrollDeath)
				{
					if (AtLine(2))
					{
						susie.GetComponent<SpriteRenderer>().flipX = false;
						susie.EnableAnimator();
					}
					else if (AtLine(3))
					{
						txt.gameObject.AddComponent<ShakingText>().StartShake(25);
					}
					else if (AtLine(4))
					{
						SetSprite(noelle, "spr_no_left_shocked_0");
					}
					else if (AtLine(6))
					{
						SetSprite(susie, "spr_su_throw_ready", flipX: true);
						txt.GetComponent<ShakingText>().Stop();
					}
					else if (AtLine(7))
					{
						noelle.EnableAnimator();
						SetSprite(susie, "spr_su_shrug_unhappy");
					}
				}
				else if (AtLine(2))
				{
					txt.gameObject.AddComponent<ShakingText>().StartShake(5);
				}
				else if (AtLine(3))
				{
					txt.GetComponent<ShakingText>().Stop();
				}
				else if (AtLine(4))
				{
					SetSprite(kris, "spr_kr_up_removesoul_wtf");
					SetSprite(noelle, "spr_no_left_shocked_0");
					txt.gameObject.GetComponent<ShakingText>().StartShake(25);
				}
				else if (AtLine(5))
				{
					SetSprite(susie, "spr_su_throw_ready", flipX: true);
					txt.GetComponent<ShakingText>().Stop();
				}
				else if (AtLine(6))
				{
					noelle.EnableAnimator();
					SetSprite(susie, "spr_su_shrug_unhappy");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(susie, Vector2.right);
				ChangeDirection(noelle, Vector2.up);
				ChangeDirection(kris, Vector2.left);
				PlayAnimation(kris, "walk");
				kris.EnableAnimator();
				susie.EnableAnimator();
				SetMoveAnim(kris, isMoving: true);
				SetMoveAnim(susie, isMoving: true);
				SetMoveAnim(noelle, isMoving: true);
			}
			if (!MoveTo(kris, new Vector3(4.11f, -1.62f), 4f))
			{
				SetMoveAnim(kris, isMoving: false);
				ChangeDirection(kris, Vector2.right);
				if (replayTrollDeath && kris.GetComponent<Animator>().enabled)
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(kris, "spr_kr_pre_pitchsoul_0");
				}
			}
			if (!MoveTo(susie, new Vector3(9.43f, -1.47f), 4f))
			{
				SetMoveAnim(susie, isMoving: false);
				ChangeDirection(susie, Vector2.left);
			}
			if (!MoveTo(noelle, new Vector3(7.64f, 0.65f), 6f))
			{
				SetMoveAnim(noelle, isMoving: false);
				ChangeDirection(noelle, Vector2.down);
			}
			if (frames == 60)
			{
				if (replayTrollDeath)
				{
					StartText(new string[4] { "* Let's see you try\n  to hit this into\n  space.", "* You know I will,^05\n  Kris.", "* Alright.", "* Batter up." }, new string[4] { "snd_txtkrs", "snd_txtsus", "snd_txtkrs", "snd_txtkrs" }, new int[1], new string[4] { "kr_smirk", "su_smile", "kr_g_1", "kr_smug" });
				}
				else
				{
					StartText(new string[11]
					{
						"* You'll throw it at\n  me,^05 and I'll send\n  it to space.", "* Or at least see\n  if it breaks through\n  the ceiling.", "* Oh...", "* Are you sure we\n  wanna do this for\n  what I just did?", "* I remember what you\n  told us to do.", "* And like...", "* What you did was\n  stupidly cruel.", "* So we're doing this.", "* Unless you're fine\n  with going on as\n  is.", "* ...",
						"* Batter up."
					}, new string[10] { "snd_txtsus", "snd_txtsus", "snd_txtkrs", "snd_txtkrs", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtkrs" }, new int[1], new string[11]
					{
						"su_neutral", "su_smirk_sweat", "kr_dejected", "kr_sad", "su_neutral", "su_side", "su_depressed", "su_annoyed", "su_annoyed", "kr_g_1",
						"kr_smug"
					});
				}
				frames = 0;
				state = 7;
			}
		}
		else
		{
			if (state != 7)
			{
				return;
			}
			if ((bool)txt)
			{
				if (replayTrollDeath)
				{
					if (AtLine(2))
					{
						SetSprite(susie, "spr_su_threaten_stick", flipX: true);
					}
					else if (AtLine(3))
					{
						SetSprite(kris, "spr_kr_pre_pitchsoul_1");
					}
					else if (AtLine(4))
					{
						SetSprite(kris, "spr_kr_pitchsoul_0");
					}
				}
				else if (AtLine(3))
				{
					ChangeDirection(kris, Vector2.up);
				}
				else if (AtLine(4))
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(kris, "spr_kr_pre_pitchsoul_0");
				}
				else if (AtLine(5))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(8))
				{
					SetSprite(susie, "spr_su_threaten_stick", flipX: true);
				}
				else if (AtLine(10))
				{
					SetSprite(kris, "spr_kr_pre_pitchsoul_1");
				}
				else if (AtLine(11))
				{
					SetSprite(kris, "spr_kr_pitchsoul_0");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlayAnimation(kris, "PitchSoul");
				kris.EnableAnimator();
			}
			if (frames == 19)
			{
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 33)
			{
				gm.PlayGlobalSFX("sounds/snd_swing");
			}
			if (frames >= 37)
			{
				GameObject.Find("SoulToss").transform.position = new Vector3(Mathf.Lerp(5.01f, 7.54f, (float)(frames - 37) / 3f), kris.transform.position.y);
			}
			if (frames == 36)
			{
				susie.EnableAnimator();
				PlayAnimation(susie, "AttackStick");
				PlaySFX("sounds/snd_attack");
			}
			if (frames >= 40)
			{
				int num9 = frames / 2 % 2;
				SetSprite(GameObject.Find("SoulToss").transform, "player/Kris/spr_soul_pitch_hit_" + num9);
				if (frames == 40)
				{
					gm.PlayGlobalSFX("sounds/snd_homerun");
					susie.DisableAnimator();
					SetSprite(noelle, "spr_no_surprise");
				}
				float num10 = (float)(frames - 56) / 48f;
				cam.transform.position = camPos + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) * num10;
			}
			if (frames == 51)
			{
				gm.Death(6);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		dogamy = GameObject.Find("Dogamy").GetComponent<InteractTextBox>();
		dogaressa = GameObject.Find("Dogaressa").GetComponent<InteractTextBox>();
		endState = int.Parse(par[0].ToString());
		gm.SetPartyMembers(susie: true, noelle: true);
		if (endState == 2)
		{
			WeirdChecker.Abort(gm);
			StartText(new string[4] { ";MPdogamy; I still don't know if you're\n  a human or a puppy,^05 but...\n  You're nice???", ";MPdogaressa; (Most of the monsters here are\n  just mean,^05 so...)", ";MPdogaressa; (I guess we won't eliminate\n  you.)", ";MPdogamy; Thanks,^05 weird human-puppy\n  thing!" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			dogamy.SetTalkable(txt);
			return;
		}
		Object.Destroy(dogamy.transform.parent.gameObject);
		gm.SetFlag(1, "inquisitive");
		gm.SetFlag(2, "depressed_side");
		if (endState == 3)
		{
			gm.ModifyCheckpointLocation(84, Vector3.zero);
			state = 4;
			replayTrollDeath = (int)gm.GetPersistentFlag(8) == 1;
			if (!replayTrollDeath)
			{
				gm.SetPersistentFlag(8, 1);
			}
			camPos = cam.transform.position;
		}
		else
		{
			PlaySFX("sounds/snd_ominous");
			state = 2;
		}
	}
}
