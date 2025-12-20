using UnityEngine;

public class PorkyDuoCultistDefeatCutscene : CutsceneBase
{
	private int endState;

	private bool geno;

	private bool abortedGeno;

	private int selectIndex;

	private bool selecting;

	private bool bringUpIceShockUse;

	private float yVelocity = 0.1f;

	private Transform soul;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			int num = ((geno && endState == 1) ? 40 : 20);
			if (frames == num)
			{
				noelle.ChangeDirection(Vector2.left);
				kris.ChangeDirection(Vector2.right);
				if (geno)
				{
					if (endState == 1)
					{
						if (bringUpIceShockUse)
						{
							StartText(new string[4] { "* Kris...", "* ... Why...", "* Why are you acting\n  like this?", "* And...^05 why did you\n  make only ME do it?" }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[4] { "no_depressed", "no_depressed", "no_depressed_look", "no_depressed" }, 0);
							txt.EnableSelectionAtEnd();
							state = 4;
						}
						else
						{
							StartText(new string[8] { "* Kris...", "* ... Why...", "* Why are you acting\n  like this?", "* ...", "* ...?", "* What...?", "* And it just...^05\n  stopped everything?", "* Is that why you\n  collapsed in there?" }, new string[8] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[8] { "no_depressed", "no_depressed", "no_depressed_look", "no_depressed", "no_depressed_side", "no_curious", "no_thinking", "no_curious" }, 0);
							state = 5;
						}
						noelle.ChangeDirection(Vector2.up);
					}
					else
					{
						StartText(new string[6] { "* ... Kris?", "* Is there a reason\n  why you decided to\n  stop killing now?", "* Is it because Susie's\n  gone or something?", "* Or maybe you kept\n  dying to them BECAUSE\n  of Susie being gone?", "* ...^05 I'm joking!^05\n* This isn't a game\n  or anything.", "* I'm just glad that\n  you did that." }, new string[8] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[6] { "no_shocked", "no_confused_side", "no_curious", "no_happy", "no_playful", "no_relief" }, 0);
						state = 5;
					}
				}
				else if (abortedGeno)
				{
					StartText(new string[1] { "* Well,^05 that was...^05\n  something." }, new string[1] { "snd_txtnoe" }, new int[18], new string[1] { "no_playful" }, 0);
					state = 5;
				}
				else
				{
					noelle.UseHappySprites();
					if ((int)gm.GetFlag(105) == 1)
					{
						StartText(new string[4] { "* Wow,^05 that was some...\n  interesting magic,^05\n  Paula.", "* It's even more\n  powerful than my\n  own...", "* Those were the\n  psychic moves that\n  I was talking about.", "* Oh,^05 that makes sense,^05\n  I guess." }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtpau", "snd_txtnoe" }, new int[18], new string[4] { "no_surprised_happy", "no_blush", "pau_confident", "no_happy" }, 0);
					}
					else if (endState == 2)
					{
						StartText(new string[5] { "* Well,^05 that was...^05\n  something.", "* Yeah...", "* Hey,^05 don't forget that\n  I can use psychic\n  abilities.", "* I know two moves\n  that don't hurt enemies,\n^05  y'know.", "* Thanks for reminding\n  us..." }, new string[5] { "snd_txtnoe", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtnoe" }, new int[18], new string[5] { "no_playful", "pau_dejected", "pau_confident", "pau_embarrassed", "no_happy" }, 0);
					}
					else
					{
						StartText(new string[4] { "* Well,^05 that was...^05\n  something.", "* Yeah...", "* Hey,^05 don't forget that\n  I can use psychic\n  abilities.", "* Thanks for reminding\n  us..." }, new string[5] { "snd_txtnoe", "snd_txtpau", "snd_txtpau", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[4] { "no_playful", "pau_dejected", "pau_confident", "no_happy" }, 0);
					}
					state = 1;
				}
				frames = 0;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				susie.GetComponent<SpriteRenderer>().enabled = true;
				susie.ChangeDirection(Vector2.left);
				susie.GetComponent<Animator>().Play("walk");
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
			}
			if (susie.transform.position.x != 89.5f)
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(89.5f, 32.18f), 0.125f);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
				StartText(new string[2] { "* Damn it,^05 he got\n  away.", "* ... Why didn't you\n  do ANYTHING,^05 Ralsei???" }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_neutral", "su_pissed" }, 0);
				state = 2;
				frames = 0;
			}
			if (frames == 20)
			{
				noelle.ChangeDirection(Vector2.right);
				noelle.UseUnhappySprites();
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.ChangeDirection(Vector2.left);
				noelle.ChangeDirection(Vector2.left);
			}
			if (frames == 30)
			{
				Util.FindObjectOfType<RalseiSmokinAFatOne>().GetComponent<Animator>().enabled = false;
				Util.FindObjectOfType<RalseiSmokinAFatOne>().GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_ralsei_lookover");
			}
			if (frames == 45)
			{
				StartText(new string[3] { "* Am I even relevant\n  to this story?", "* Whatever.", "* Let's just get\n  going." }, new string[3] { "snd_txtral", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "ral_concerned_doobie", "su_annoyed", "su_pissed" }, 0);
				state = 3;
			}
		}
		if (state == 3)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					Util.FindObjectOfType<RalseiSmokinAFatOne>().GetComponent<Animator>().enabled = true;
				}
			}
			else
			{
				susie.SetSelfAnimControl(setAnimControl: true);
				noelle.SetSelfAnimControl(setAnimControl: true);
				kris.SetSelfAnimControl(setAnimControl: true);
				gm.PlayMusic("zoneMusic");
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(92.91f, 22.05f), Quaternion.identity);
				EndCutscene();
			}
		}
		if (state == 4 && txt.CanLoadSelection() && !selecting)
		{
			selecting = true;
			InitiateDeltaSelection();
			if (selectIndex == 0)
			{
				select.SetupChoice(Vector2.left, "There is\nsomething\nweird\nhappening", Vector3.zero);
				select.SetupChoice(Vector2.right, "For your\nown good", new Vector3(-32f, 0f));
			}
			else if (selectIndex == 1)
			{
				select.SetupChoice(Vector2.left, "Nothing", Vector3.zero);
				select.SetupChoice(Vector2.right, "You must\nbecome\nstronger", new Vector3(-40f, 0f));
			}
			else if (selectIndex == 2)
			{
				select.SetupChoice(Vector2.left, "Nevermind", Vector3.zero);
				select.SetupChoice(Vector2.right, "We will\neradicate\nthem all", new Vector3(-40f, 0f));
			}
			select.Activate(this, selectIndex, txt.gameObject);
		}
		if (state == 5)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 4)
				{
					kris.ChangeDirection(Vector2.up);
				}
				if (txt.GetCurrentStringNum() == 5)
				{
					noelle.ChangeDirection(Vector2.left);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					susie.GetComponent<SpriteRenderer>().enabled = true;
					susie.ChangeDirection(Vector2.left);
					susie.GetComponent<Animator>().Play("walk");
					susie.GetComponent<Animator>().SetFloat("speed", 1f);
				}
				if (susie.transform.position.x != 89.5f)
				{
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(89.5f, 32.18f), 0.125f);
				}
				else
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					string text = "* And I couldn't find\n  Paula anywhere.";
					string text2 = "su_side_sweat";
					if (selectIndex == 3)
					{
						text = "* Uhh,^05 Noelle?^05\n* Is everything okay?";
						text2 = "su_concerned";
					}
					else if (geno && endState == 1)
					{
						text = "* And of course this is\n  what I come back to.";
						text2 = "su_annoyed";
					}
					StartText(new string[3] { "* Damn it,^05 he got\n  away.", text, "* ...^10 Wait a sec,^05\n  what's that?" }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_neutral", text2, "su_side" }, 0);
					state = 6;
					frames = 0;
				}
				if (frames == 20)
				{
					kris.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.right);
					noelle.UseUnhappySprites();
				}
			}
		}
		if (state == 6)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && geno && endState == 1 && selectIndex < 3)
				{
					susie.ChangeDirection(Vector2.down);
				}
				if (txt.GetCurrentStringNum() == 3)
				{
					susie.ChangeDirection(Vector2.left);
				}
			}
			else if (susie.transform.position != new Vector3(87.09f, 32.18f))
			{
				noelle.ChangeDirection(Vector2.down);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(87.09f, 32.18f), 1f / 12f);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.ChangeDirection(Vector2.down);
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					susie.DisableAnimator();
					susie.SetSprite("spr_su_kneel");
					susie.GetComponent<SpriteRenderer>().flipX = true;
				}
				if (frames == 30)
				{
					StartText(new string[3] { "* It's some kind of\n  weird lightning badge.", "* ...^05 Something's telling\n  me that this is\n  important.", "* Maybe you could use\n  this for something,^05\n  Kris?" }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side", "su_inquisitive", "su_smirk" }, 0);
					state = 7;
					frames = 0;
				}
			}
		}
		if (state == 7)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					susie.ChangeDirection(Vector2.up);
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
			}
			else if (susie.transform.position != new Vector3(86.23f, 32.98f))
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(86.23f, 32.98f), 1f / 12f);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
				gm.PlayGlobalSFX("sounds/snd_item");
				StartText(new string[3] { "* (Susie gave you a lightning\n  badge.)", "* (You pinned the badge onto\n  your shirt.)", "* Let's go see if\n  we can do anything\n  with it in town." }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[18], new string[3] { "", "", "su_side" }, 0);
				state = 8;
				frames = 0;
			}
		}
		if (state == 8 && !txt)
		{
			susie.SetSelfAnimControl(setAnimControl: true);
			noelle.SetSelfAnimControl(setAnimControl: true);
			kris.SetSelfAnimControl(setAnimControl: true);
			gm.PlayMusic("zoneMusic");
			EndCutscene();
		}
		if (state == 9 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic();
				kris.GetComponent<Animator>().Play("RemoveSoul_WalkUp");
			}
			if (frames >= 38 && frames <= 41)
			{
				int num2 = ((frames % 2 == 0) ? 1 : (-1));
				int num3 = 41 - frames;
				kris.transform.position = new Vector3(86.245f, 33.759f) + new Vector3((float)(num3 * num2) / 24f, 0f);
			}
			if (frames >= 48 && frames <= 51)
			{
				int num4 = ((frames % 2 == 0) ? 1 : (-1));
				int num5 = 51 - frames;
				kris.transform.position = new Vector3(86.245f, 33.759f) + new Vector3((float)(num5 * num4) / 24f, 0f);
			}
			if (frames >= 58 && frames <= 61)
			{
				int num6 = ((frames % 2 == 0) ? 1 : (-1));
				int num7 = 61 - frames;
				kris.transform.position = new Vector3(86.245f, 33.759f) + new Vector3((float)(num7 * num6) / 24f, 0f);
			}
			if (frames >= 64 && frames <= 67)
			{
				int num8 = ((frames % 2 == 0) ? 1 : (-1));
				int num9 = 67 - frames;
				kris.transform.position = new Vector3(86.245f, 33.759f) + new Vector3((float)(num9 * num8) / 24f, 0f);
				noelle.transform.position = new Vector3(87.42f, 33.35f) + new Vector3((float)(num9 * num8) / 24f, 0f);
			}
			if (frames == 38 || frames == 48 || frames == 58)
			{
				PlaySFX("sounds/snd_bump");
			}
			if (frames == 64)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), new Vector3(86.245f, 33.759f), Quaternion.identity);
				PlaySFX("sounds/snd_grab");
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_surprise");
			}
			if (frames == 90)
			{
				StartText(new string[6] { "* K-^05Kris???", "* Freeze it.", "* ...", "* Please.", "* ...", "* Okay..." }, new string[6] { "snd_txtnoe", "snd_txtkrs", "snd_txtnoe", "snd_txtkrs", "snd_txtnoe", "snd_txtnoe" }, new int[18]
				{
					0, 2, 0, 2, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0
				}, new string[6] { "no_speechless", "kr_injured", "no_shocked", "kr_injured", "no_sad", "no_depressed" }, 0);
				state = 10;
				frames = 0;
				txt.gameObject.AddComponent<ShakingText>();
			}
		}
		if (state == 10)
		{
			if ((bool)txt)
			{
				if ((txt.GetCurrentStringNum() == 2 || txt.GetCurrentStringNum() == 4) && !txt.GetComponent<ShakingText>().IsPlaying())
				{
					txt.GetComponent<ShakingText>().StartShake(0);
				}
				else if (txt.GetCurrentStringNum() != 2 && txt.GetCurrentStringNum() != 4 && txt.GetComponent<ShakingText>().IsPlaying())
				{
					if (txt.GetCurrentStringNum() == 3)
					{
						noelle.EnableAnimator();
					}
					else if (txt.GetCurrentStringNum() == 5)
					{
						noelle.ChangeDirection(Vector2.up);
					}
					txt.GetComponent<ShakingText>().Stop();
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.DisableAnimator();
					kris.SetSprite("spr_kr_up_removesoul_8_injured");
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_freeze_soul_0");
				}
				if (frames == 20)
				{
					kris.SetSprite("spr_kr_sit_soulless");
					PlaySFX("sounds/snd_noise");
				}
				if (frames >= 20 && frames <= 23)
				{
					int num10 = ((frames % 2 == 0) ? 1 : (-1));
					int num11 = 23 - frames;
					kris.transform.position = new Vector3(86.245f, 33.759f) + new Vector3((float)(num11 * num10) / 24f, 0f);
				}
				if (frames == 35)
				{
					noelle.EnableAnimator();
					noelle.GetComponent<Animator>().Play("FreezeSoul");
					PlaySFX("sounds/snd_petrify");
				}
				if (frames == 90)
				{
					StartText(new string[6] { "* ...^05 Now what?", "* Throw it.", "* Far away.", "* Are you sure...?^05\n* This doesn't seem\n  like--", "* Just do it.", "* ...^05 Okay." }, new string[6] { "snd_txtnoe", "snd_txtkrs", "snd_txtkrs", "snd_txtnoe", "snd_txtkrs", "snd_txtnoe" }, new int[18]
					{
						0, 2, 2, 0, 2, 0, 0, 0, 0, 0,
						0, 0, 0, 0, 0, 0, 0, 0
					}, new string[6] { "no_depressed", "kr_injured", "kr_evil_injured", "no_depressed_look", "kr_injured", "no_depressed" }, 0);
					txt.gameObject.AddComponent<ShakingText>();
					state = 11;
					frames = 0;
				}
			}
		}
		if (state != 11)
		{
			return;
		}
		if ((bool)txt)
		{
			if ((txt.GetCurrentStringNum() == 2 || txt.GetCurrentStringNum() == 5) && !txt.GetComponent<ShakingText>().IsPlaying())
			{
				txt.GetComponent<ShakingText>().StartShake(0);
			}
			else if ((txt.GetCurrentStringNum() == 4 || txt.GetCurrentStringNum() == 6) && txt.GetComponent<ShakingText>().IsPlaying())
			{
				txt.GetComponent<ShakingText>().Stop();
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			noelle.DisableAnimator();
			noelle.SetSprite("spr_no_cast_right");
			PlaySFX("sounds/snd_heavyswing");
			susie.transform.position = new Vector3(94.5f, 40.03f);
			susie.ChangeDirection(Vector2.down);
			susie.GetComponent<Animator>().SetBool("isMoving", value: true);
			susie.GetComponent<Animator>().Play("walk");
			susie.GetComponent<Animator>().SetFloat("speed", 1f);
			soul = new GameObject("Stupid", typeof(FakeActionSOUL), typeof(SpriteRenderer)).transform;
			soul.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/eb_objects/spr_frozen_soul");
			soul.GetComponent<SpriteRenderer>().sortingOrder = 100;
			soul.position = new Vector3(87.42f, 33.35f);
		}
		if (frames <= 30)
		{
			susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(94.5f, 0f), 0.125f);
			if (frames == 30)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_surprise_right");
			}
		}
		if (frames >= 30 && frames <= 33)
		{
			int num12 = ((frames % 2 == 0) ? 1 : (-1));
			int num13 = 33 - frames;
			susie.transform.position = new Vector3(94.5f, susie.transform.position.y) + new Vector3((float)(num13 * num12) / 24f, 0f);
		}
		if (frames <= 40)
		{
			cam.transform.position = new Vector3(Mathf.Lerp(86.245f, 95.84f, (float)frames / 40f), cam.transform.position.y, -10f);
			soul.position = new Vector3(Mathf.Lerp(87.42f, 98.351f, (float)frames / 40f), soul.position.y + yVelocity);
			yVelocity -= 0.005f;
			if (frames == 40)
			{
				PlaySFX("sounds/snd_glassbreak");
			}
		}
		if (frames == 45)
		{
			gm.Death(0);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selecting = false;
		if (id == 0)
		{
			noelle.ChangeDirection(Vector2.left);
			if (index == Vector2.left)
			{
				StartText(new string[4] { "* ...?", "* What...?", "* And it just...^05\n  stopped everything?", "* Is that why you\n  collapsed in there?" }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[4] { "no_depressed_side", "no_curious", "no_thinking", "no_curious" }, 0);
				kris.ChangeDirection(Vector2.up);
				state = 5;
			}
			else
			{
				StartText(new string[2] { "* What?", "* Kris...?^05\n* What does that mean?" }, new string[3] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[3] { "no_shocked", "no_silent", "no_depressed_look" }, 0);
				txt.EnableSelectionAtEnd();
				selectIndex++;
			}
		}
		if (id == 1)
		{
			if (index == Vector2.left)
			{
				noelle.ChangeDirection(Vector2.up);
				StartText(new string[3] { "* ...", "* Okay...", "* Don't scare me like\n  that!" }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[3] { "no_depressed", "no_mad_closed", "no_angry" }, 0);
				state = 5;
			}
			else
			{
				gm.PlayMusic("music/mus_adrenaline", 0.25f);
				StartText(new string[1] { "* Y-^05you're not wanting\n  ME to..." }, new string[1] { "snd_txtnoe" }, new int[18], new string[1] { "no_shocked" }, 0);
				txt.EnableSelectionAtEnd();
				selectIndex++;
			}
		}
		if (id == 2)
		{
			if (index == Vector2.left)
			{
				gm.StopMusic();
				noelle.ChangeDirection(Vector2.up);
				StartText(new string[3] { "* ...", "* Okay...", "* Don't scare me like\n  that!" }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[3] { "no_depressed", "no_mad_closed", "no_angry" }, 0);
				state = 5;
			}
			else
			{
				kris.ChangeDirection(Vector2.up);
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_left_shocked_0");
				cam.SetFollowPlayer(follow: false);
				StartText(new string[2] { "* ...", "* ... I..." }, new string[3] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[3] { "no_shocked", "no_shocked", "no_shocked" }, 0);
				state = 9;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		susie = GameObject.Find("Susie").GetComponent<OverworldPartyMember>();
		endState = int.Parse(par[0].ToString());
		gm.SetFlag(106, endState);
		MonoBehaviour.print((int)gm.GetFlag(13));
		geno = (int)gm.GetFlag(13) >= 5;
		abortedGeno = (int)gm.GetFlag(87) >= 5;
		if (geno)
		{
			if (endState != 1)
			{
				WeirdChecker.Abort(gm);
			}
			else
			{
				GameObject.Find("DeadCultists").transform.position = Vector3.zero;
				string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
				SpriteRenderer[] componentsInChildren = GameObject.Find("DeadCultists").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill" + text);
				}
				bringUpIceShockUse = (int)gm.GetFlag(129) == 1;
			}
		}
		gm.SetPartyMembers(susie: true, noelle: true);
		GameObject.Find("CultistCutscene1").transform.position = new Vector3(0f, 500f);
		GameObject.Find("CultistCutscene2").transform.position = new Vector3(0f, 500f);
		GameObject.Find("Porky").transform.position = new Vector3(0f, 500f);
	}
}
