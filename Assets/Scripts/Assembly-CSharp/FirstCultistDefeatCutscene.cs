using UnityEngine;

public class FirstCultistDefeatCutscene : CutsceneBase
{
	private Animator cultist;

	private Animator creepyLady;

	private int cutsceneMode;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (cutsceneMode == 0)
		{
			if (state == 0 && !txt)
			{
				frames++;
				if (frames == 1)
				{
					cultist.SetBool("isMoving", value: true);
				}
				if (frames == 15)
				{
					susie.ChangeDirection(Vector2.down);
					noelle.ChangeDirection(Vector2.up);
				}
				if (frames < 30)
				{
					cultist.transform.position = Vector3.Lerp(new Vector3(5.9f, -3.47f), new Vector3(0.06f, -1.59f), (float)frames / 30f);
				}
				else if (frames <= 40)
				{
					if (frames == 30)
					{
						kris.ChangeDirection(Vector2.up);
						susie.ChangeDirection(Vector2.left);
						cultist.SetFloat("dirX", 0f);
						cultist.SetFloat("dirY", 1f);
					}
					cultist.transform.position = Vector3.Lerp(new Vector3(0.06f, -1.59f), new Vector3(0.06f, -0.902f), (float)(frames - 30) / 10f);
					if (frames == 40)
					{
						cultist.GetComponent<SpriteRenderer>().enabled = false;
						PlaySFX("sounds/snd_escaped");
					}
				}
				if (frames == 70)
				{
					kris.ChangeDirection(Vector2.right);
					susie.ChangeDirection(Vector2.down);
					StartText(new string[4] { "* Painting the whole\n  world blue??", "* What on earth does\n  that mean?", "* I guess this place\n  is for some weird\n  cult thing?", "* We should go look\n  around." }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "no_confused", "no_confused_side", "su_side", "su_smirk" }, 1);
					frames = 0;
					state = 1;
				}
			}
			if (state == 1 && !txt)
			{
				if ((int)gm.GetFlag(87) == 4 && (int)gm.GetFlag(89) == 1)
				{
					frames = 0;
					state = 2;
					susie.ChangeDirection(Vector2.left);
					StartText(new string[4] { "* But uhh,^05 Kris.", "* You decided to spare\n  the first human we\n  fight?", "* ...", "* I guess it's better\n  than...^10 well...^10\n  doing the opposite." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_neutral", "su_annoyed", "su_side", "su_dejected" }, 1);
				}
				else if (cam.transform.position != cam.GetClampedPos())
				{
					cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
				}
				else
				{
					cultist.transform.position = new Vector3(0f, 500f);
					kris.SetSelfAnimControl(setAnimControl: true);
					susie.SetSelfAnimControl(setAnimControl: true);
					noelle.SetSelfAnimControl(setAnimControl: true);
					susie.UseHappySprites();
					noelle.UseHappySprites();
					kris.ChangeDirection(Vector2.down);
					cam.SetFollowPlayer(follow: true);
					gm.SetCheckpoint(56);
					EndCutscene();
				}
			}
			if (state == 2 && !txt)
			{
				frames++;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_escaped");
				}
				if (frames == 15)
				{
					cultist.enabled = false;
					cultist.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_grab_snake");
					cultist.GetComponent<SpriteRenderer>().enabled = true;
					kris.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
				}
				if (frames == 30)
				{
					susie.DisableAnimator();
					noelle.DisableAnimator();
					susie.SetSprite("spr_su_surprise_up");
					susie.GetComponent<SpriteRenderer>().flipX = true;
					noelle.SetSprite("spr_no_surprise_up");
					StartText(new string[11]
					{
						"* Ummm... excuse me???", "* W-^05what happened\n  here???", "* Uhhhhhhhh.....", "* A,^05 uhh,^05 a...^10\n  coyote came into the\n  cave,^05 and uhh...", "* Sliced it,^05 with...^10\n  its teeth.", "* I think.", "* I...^05 warded it\n  off afterwards.", "* .^02.^02.^02.^02.^02.^02.^02.^02.^02.^02.^02.^02.^02.^02.", "* Well,^05 you should've acted\n  quicker.", "* Someone's gonna have to clean\n  up that blood.",
						"* Don't worry,^05 I'll get someone\n  else to."
					}, new string[11]
					{
						"snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_text", "snd_text",
						"snd_text"
					}, new int[18], new string[11]
					{
						"", "", "su_shocked", "su_shocked", "su_smirk_sweat", "su_inquisitive", "su_smile_sweat", "", "", "",
						""
					}, 1);
					state = 3;
					frames = 0;
				}
			}
			if (state == 3)
			{
				if ((bool)txt)
				{
					if (txt.GetCurrentStringNum() == 5)
					{
						susie.EnableAnimator();
						susie.ChangeDirection(Vector2.left);
						susie.GetComponent<SpriteRenderer>().flipX = false;
					}
					if (txt.GetCurrentStringNum() == 8)
					{
						noelle.EnableAnimator();
					}
				}
				else if (!txt)
				{
					frames++;
					if (frames == 1)
					{
						PlaySFX("sounds/snd_escaped");
						cultist.GetComponent<SpriteRenderer>().enabled = false;
						cultist.transform.position = new Vector3(0f, 500f);
					}
					if (frames == 30)
					{
						kris.ChangeDirection(Vector2.right);
						susie.ChangeDirection(Vector2.down);
						noelle.ChangeDirection(Vector2.up);
						StartText(new string[1] { "* Let's just go." }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_neutral" }, 1);
						state = 4;
					}
				}
			}
			if (state == 4 && !txt)
			{
				if (cam.transform.position != cam.GetClampedPos())
				{
					cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
					return;
				}
				cultist.transform.position = new Vector3(0f, 500f);
				kris.SetSelfAnimControl(setAnimControl: true);
				susie.SetSelfAnimControl(setAnimControl: true);
				noelle.SetSelfAnimControl(setAnimControl: true);
				susie.UseHappySprites();
				kris.ChangeDirection(Vector2.down);
				cam.SetFollowPlayer(follow: true);
				gm.SetCheckpoint(56);
				EndCutscene();
			}
		}
		else if (cutsceneMode == 1)
		{
			if (state == 0)
			{
				frames++;
				if (frames == 30)
				{
					kris.ChangeDirection(Vector2.right);
					susie.ChangeDirection(Vector2.down);
					noelle.ChangeDirection(Vector2.up);
					StartText(new string[5] { "* Heheheh....\n^05* Looks like we taught\n  THEM a lesson!", "* Painting the whole\n  world blue??", "* What on earth does\n  that mean?", "* I guess this place\n  is for some weird\n  cult thing?", "* We should go look\n  around." }, new string[5] { "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_confident", "no_confused", "no_confused_side", "su_side", "su_smirk" }, 1);
					susie.DisableAnimator();
					susie.SetSprite("spr_su_pose");
					frames = 0;
					state = 1;
				}
			}
			if (state != 1)
			{
				return;
			}
			if (!txt)
			{
				if (cam.transform.position != cam.GetClampedPos())
				{
					cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
					return;
				}
				kris.SetSelfAnimControl(setAnimControl: true);
				susie.SetSelfAnimControl(setAnimControl: true);
				noelle.SetSelfAnimControl(setAnimControl: true);
				susie.UseHappySprites();
				noelle.UseHappySprites();
				kris.ChangeDirection(Vector2.down);
				cam.SetFollowPlayer(follow: true);
				gm.SetCheckpoint(56);
				EndCutscene();
			}
			else if (txt.GetCurrentStringNum() == 2)
			{
				susie.EnableAnimator();
			}
		}
		else
		{
			if (cutsceneMode != 2)
			{
				return;
			}
			if (state == 0)
			{
				frames++;
				if (frames == 45)
				{
					kris.ChangeDirection(Vector2.right);
					susie.ChangeDirection(Vector2.left);
					susie.EnableAnimator();
					noelle.ChangeDirection(Vector2.up);
					noelle.EnableAnimator();
					StartText(new string[2] { "* ...", "* Of course you\n  did that." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_disappointed", "su_disappointed" }, 1);
					frames = 0;
					state = 1;
				}
			}
			if (state == 1 && !txt)
			{
				frames++;
				if (frames == 1)
				{
					creepyLady.SetFloat("dirX", -1f);
					creepyLady.SetBool("isMoving", value: true);
					creepyLady.SetFloat("speed", 0.75f);
				}
				creepyLady.transform.position = Vector3.Lerp(new Vector3(9.51f, -3.35f), new Vector3(6.94f, -2.26f), (float)frames / 45f);
				if (frames == 25)
				{
					susie.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.right);
				}
				if (frames == 45)
				{
					creepyLady.SetBool("isMoving", value: false);
				}
				if (frames == 65)
				{
					StartText(new string[6] { "* Uhhhhhh.", "* No,^05 no,^05 I know what is\n  happening here.", "* Your LOVE has risen greatly.", "* What.", "* That means that you deserve\n  a reward for your hard work.", "* ?????????????" }, new string[6] { "snd_txtsus", "snd_text", "snd_text", "snd_txtsus", "snd_text", "snd_txtnoe" }, new int[18], new string[6] { "su_wideeye", "", "", "su_annoyed", "", "no_confused" }, 1);
					state = 2;
					frames = 0;
				}
			}
			if (state == 2 && !txt)
			{
				if (creepyLady.transform.position.x != 0.98f)
				{
					creepyLady.SetBool("isMoving", value: true);
					creepyLady.transform.position = Vector3.MoveTowards(creepyLady.transform.position, new Vector3(0.98f, -2.26f), 1f / 12f);
					if (creepyLady.transform.position.x < 3.07f)
					{
						susie.ChangeDirection(Vector2.down);
						noelle.ChangeDirection(Vector2.up);
					}
				}
				else
				{
					frames++;
					if (frames == 1)
					{
						susie.ChangeDirection(Vector2.left);
						noelle.ChangeDirection(Vector2.left);
						creepyLady.SetBool("isMoving", value: false);
					}
					if (frames == 30)
					{
						creepyLady.SetFloat("dirX", 0f);
						creepyLady.SetFloat("dirY", 1f);
					}
					if (frames == 45)
					{
						StartText(new string[1] { "* Your reward..." }, new string[1] { "" }, new int[1] { 3 }, new string[1] { "" }, 1);
						state = 3;
						frames = 0;
					}
				}
			}
			if (state == 3 && !txt)
			{
				frames++;
				if (frames == 1)
				{
					creepyLady.Play("Smash");
					PlaySFX("sounds/snd_frypan");
				}
				if (frames == 8)
				{
					GameObject.Find("Dark").GetComponent<SpriteRenderer>().enabled = true;
					gm.PlayGlobalSFX("sounds/snd_hurt");
					gm.SetFlag(102, 1);
					gm.SetFlag(0, "injured");
				}
				if (frames == 90)
				{
					StartText(new string[5] { "* My reward...", "* It's^10 what I deserve.", "* For being unable to stop this.", "* ...", "* Hopefully I won't wake up." }, new string[5] { "", "", "", "", "" }, new int[5] { 2, 2, 2, 2, 2 }, new string[5] { "", "", "", "", "" }, 1);
					Object.Destroy(GameObject.Find("menuBorder"));
					Object.Destroy(GameObject.Find("menuBox"));
					state = 4;
					frames = 0;
				}
			}
			if (state == 4 && !txt)
			{
				frames++;
				if (frames == 60)
				{
					gm.ForceLoadArea(59);
				}
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		int num = int.Parse(par[0].ToString());
		bool flag = (int)gm.GetFlag(13) == 4;
		gm.SetFlag(97, 1);
		gm.SetFlag(98, num);
		gm.SetFlag(99, 1);
		if (!flag && (int)gm.GetFlag(12) == 1)
		{
			WeirdChecker.Abort(gm);
		}
		if (num == 1)
		{
			if (flag)
			{
				WeirdChecker.AdvanceTo(gm, 5);
				cutsceneMode = 2;
			}
			else
			{
				cutsceneMode = 1;
			}
		}
		else
		{
			cutsceneMode = 0;
			if (flag)
			{
				WeirdChecker.Abort(gm);
			}
		}
		cultist = GameObject.Find("CultistCutscene").GetComponent<Animator>();
		cultist.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
		cultist.transform.GetComponent<SpriteRenderer>().sortingOrder = 9;
		creepyLady = GameObject.Find("CreepyLadyCutscene").GetComponent<Animator>();
		gm.SetFlag(84, 2);
		if (cutsceneMode == 2)
		{
			gm.SetFlag(84, 3);
			gm.SetFlag(99, 0);
			cultist.enabled = false;
			cultist.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill");
			noelle.DisableAnimator();
			noelle.SetSprite("spr_no_right_shocked_0");
			gm.StopMusic();
			return;
		}
		if (cutsceneMode == 1)
		{
			susie.EnableAnimator();
			cultist.transform.position = new Vector3(0f, -666f);
			return;
		}
		susie.EnableAnimator();
		StartText(new string[4] { "* ...", "* Well...^10 after that...", "* I might need to think about\n  this whole blue thing.", "* Excuse me." }, new string[4] { "snd_text", "snd_text", "snd_text", "snd_text" }, new int[18], new string[4] { "", "", "", "" }, 1);
	}
}
