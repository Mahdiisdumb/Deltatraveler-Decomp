using System.Collections.Generic;
using UnityEngine;

public class MondoMoleDefeatCutscene : CutsceneBase
{
	private bool moleTimeBaby;

	private MoleFriend mole;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.PlayGlobalSFX("sounds/snd_hypnosis");
			}
			Object.FindObjectOfType<MondoMoleNPC>().GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)(30 - frames) / 30f);
			if (frames == 40)
			{
				frames = 0;
				Object.Destroy(Object.FindObjectOfType<MondoMoleNPC>().gameObject);
				state = ((!moleTimeBaby) ? 1 : 2);
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (moleTimeBaby)
				{
					if (txt.GetCurrentStringNum() == 2 && noelle.GetComponent<Animator>().enabled)
					{
						noelle.DisableAnimator();
						noelle.SetSprite("spr_no_laugh_0");
					}
					else if (txt.GetCurrentStringNum() == 3 && !noelle.GetComponent<Animator>().enabled)
					{
						susie.ChangeDirection(Vector2.right);
						kris.ChangeDirection(Vector2.right);
						noelle.EnableAnimator();
					}
					else if (txt.GetCurrentStringNum() == 5)
					{
						susie.ChangeDirection(Vector2.left);
					}
				}
			}
			else if (cam.transform.position != cam.GetClampedPos())
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 1f / 24f);
			}
			else
			{
				susie.SetSelfAnimControl(true);
				susie.ChangeDirection(Vector2.left);
				noelle.SetSelfAnimControl(true);
				noelle.ChangeDirection(Vector2.left);
				kris.SetSelfAnimControl(true);
				kris.ChangeDirection(Vector2.down);
				cam.SetFollowPlayer(true);
				if ((bool)mole)
				{
					mole.SetSelfAnimControl(true);
				}
				Object.FindObjectOfType<SAVEPoint>().transform.position = new Vector3(56f, 0.368f);
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(50.07f, -7.45f), Quaternion.identity);
				EndCutscene();
			}
		}
		if (state == 2)
		{
			frames++;
			if (frames == 1)
			{
				mole.transform.position = new Vector3(46.25f, -1.37f);
				mole.GetComponent<Animator>().SetBool("isMoving", true);
				mole.ChangeDirection(Vector2.right);
			}
			if (mole.transform.position != new Vector3(49.78f, -0.39f))
			{
				mole.transform.position = Vector3.MoveTowards(mole.transform.position, new Vector3(49.78f, -0.39f), 1f / 12f);
			}
			else
			{
				mole.GetComponent<Animator>().SetBool("isMoving", false);
			}
			if (frames == 20)
			{
				kris.ChangeDirection(Vector2.left);
				susie.ChangeDirection(Vector2.left);
				noelle.ChangeDirection(Vector2.left);
			}
			if (frames == 60)
			{
				StartText(new string[9] { "* Uhh,^05 what?", "* Aww!\n* I think it wants\n  to follow us!", "* Okay...?", "* But like...\n^05* We're leaving soon.", "* I don't really see\n  the point.", "* I dunno.", "* There might be someone\n  that'd really like to\n  see a mole.", "* Honestly,^05 that sounds\n  like some stupid\n  side-quest shit.", "* Just do whatever\n  you wanna do,^05 Kris." }, new string[9] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtsus", "snd_txtsus" }, new int[1], new string[9] { "su_side", "no_happy", "su_inquisitive", "su_annoyed", "su_side", "pau_confident", "pau_confident", "su_annoyed", "su_neutral" }, 1);
				state = 1;
			}
		}
		if (state == 3)
		{
			frames++;
			if (frames == 40)
			{
				StartText(new string[8] { "* Y'know what.", "* It's starting to\n  feel numb,^05 but at\n  least it's over.", "* It's over...?", "* I mean,^05 we're almost\n  at the door.", "* It's not like someone\n  will just stupidly\n  try to get us.", "* Especially at the\n  last second.", "* ...^05 You're probably\n  right.", "* Let's go." }, new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe" }, new int[1], new string[8] { "su_side", "su_dejected", "no_thinking", "su_annoyed", "su_smirk_sweat", "su_annoyed", "no_depressed", "no_relief" }, 1);
				state = 4;
			}
		}
		if (state != 4)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 3)
			{
				noelle.ChangeDirection(Vector2.left);
			}
			else if (txt.GetCurrentStringNum() == 4)
			{
				susie.ChangeDirection(Vector2.right);
			}
			else if (txt.GetCurrentStringNum() == 7)
			{
				susie.ChangeDirection(Vector2.up);
				noelle.ChangeDirection(Vector2.up);
			}
		}
		else
		{
			state = 1;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		int num = int.Parse(par[0].ToString());
		bool flag = (int)gm.GetFlag(13) >= 6;
		bool flag2 = (int)gm.GetFlag(13) == 5;
		if (!WeirdChecker.HasKilled(gm))
		{
			gm.SetFlag(151, 1);
			mole = Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/MoleFriend")).GetComponent<MoleFriend>();
			mole.Deactivate();
			mole.SetSelfAnimControl(false);
			moleTimeBaby = true;
		}
		OverworldEnemyBase[] array = Object.FindObjectsOfType<OverworldEnemyBase>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ActivateHandled();
		}
		if (!flag || num != 1)
		{
			gm.PlayMusic("music/mus_wintercaves");
			if (flag2 || (num == 2 && flag))
			{
				WeirdChecker.Abort(gm);
			}
			List<string> list = new List<string>();
			if (num == 1)
			{
				list.Add("* You have done excellently\n  to defeat me.");
				if (flag2)
				{
					list.AddRange(new string[3] { "* Though the confusion on\n  your face intrigues me.", "* Did you expect to penetrate\n  my thick skin with your\n  weak power?", "* I can sense that your power\n  to draw blood has waned." });
				}
			}
			else
			{
				list.Add("* You have done excellently\n  to appease me.");
				if (flag2)
				{
					list.AddRange(new string[2] { "* You have also sufficiently\n  redeemed yourself.", "* Your power to draw blood\n  has waned." });
				}
			}
			if (flag2)
			{
				list.Add("* But regardless...");
			}
			list.AddRange(new string[4] { "* I can tell that you are\n  here for a reason beyond\n  your control.", "* I shall allow you to pass\n  into this \"Your Sanctuary\"\n  location.", "* You will not be able to\n  absorb its power,^05 but you will\n  be able to feel it.", "* Goodbye." });
			StartText(list.ToArray(), new string[1] { "snd_text" }, new int[1], new string[0], 1);
		}
		else
		{
			Object.FindObjectOfType<MondoMoleNPC>().CreateDeadEnemy(false);
			Object.Destroy(Object.FindObjectOfType<MondoMoleNPC>().gameObject);
			gm.StopMusic();
			WeirdChecker.AdvanceTo(gm, 6);
			state = 3;
		}
	}
}
