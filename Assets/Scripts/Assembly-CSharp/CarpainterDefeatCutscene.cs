using System.Collections.Generic;
using UnityEngine;

public class CarpainterDefeatCutscene : CutsceneBase
{
	private bool carpainterLives = true;

	private bool itemSound;

	private int itemSoundAt = 14;

	private void Update()
	{
		if (!isPlaying || state != 0)
		{
			return;
		}
		if ((bool)txt)
		{
			if (!itemSound && txt.GetCurrentStringNum() == itemSoundAt)
			{
				itemSound = true;
				gm.PlayGlobalSFX("sounds/snd_item");
			}
			return;
		}
		if (carpainterLives)
		{
			gm.PlayMusic("music/mus_zombiepaper");
		}
		kris.ChangeDirection(Vector2.down);
		gm.SetCheckpoint(56, new Vector3(24.15f, -22.53f));
		EndCutscene();
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		Object.Destroy(GameObject.Find("Lightning"));
		Object.Destroy(GameObject.Find("ReflectBall"));
		gm.StopMusic();
		gm.SetFlag(84, 4);
		int num = int.Parse(par[0].ToString());
		bool flag = (int)gm.GetFlag(13) >= 5;
		if (!flag && (int)gm.GetFlag(12) == 1)
		{
			WeirdChecker.Abort(gm);
			flag = false;
		}
		carpainterLives = !(num == 1 && flag);
		cam.SetFollowPlayer(follow: true);
		if (carpainterLives)
		{
			gm.SetFlag(117, 1);
			if (flag)
			{
				WeirdChecker.Abort(gm);
			}
			Util.FindObjectOfType<CarpainterNPC>().SetSprite(0);
			List<string> list = new List<string>
			{
				"", "* Do you see the Mani Mani\n  statue behind me...", "* Since I got the statue,^05\n  I have been doing peculiar\n  things.", "* Please forgive me,^05 if you can.", "* I just wanted to have a normal,^05\n  peaceful life.", "* I apologize^10 to everyone.", "* Hey,^05 so long as you're\n  genuine with your\n  apology.", "* If you're actually sorry,^05\n  you should get out\n  the paint thinner.", "* Hey,^05 so we're trying\n  to get to the Lilliput\n  Steps.", "* We were told that\n  you have a bomb for\n  that.",
				"* Hand it over and I'll\n  accept your apology.", "* That sounds like a deal.", "* Here's that bomb.^05\n* It should do enough damage\n  to break the seal.", "* (Susie got the Bomb.)", "* Aw,^05 sweet.", "* Say,^05 uhh...\n^05* Where do we set this\n  off again?", "* The cave entrance is to the\n  east of town.^05\n* It's barricaded.", "* Okay,^05 sweet.", "* Let's go."
			};
			List<string> list2 = new List<string>
			{
				"snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtnoe", "snd_txtpau", "snd_txtsus", "snd_txtsus",
				"snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_text", "snd_txtsus", "snd_txtsus"
			};
			List<string> list3 = new List<string>
			{
				"", "", "", "", "", "", "no_happy", "pau_sussy", "su_neutral", "su_side",
				"su_annoyed", "", "", "", "su_smile", "su_smile_sweat", "", "su_smile", "su_happy"
			};
			if (!gm.PartySlotFilled(3))
			{
				list.RemoveAt(7);
				list2.RemoveAt(7);
				list3.RemoveAt(7);
				list[8] = "* Surely you have\n  something to get\n  past it.";
				list[10] = "* I happen to have a\n  bomb for this purpose.";
				itemSoundAt--;
			}
			if (num == 1)
			{
				if ((int)gm.GetFlag(87) >= 4)
				{
					list[0] = "* Well, would you look\n  at that.^05\n* You lived.";
					list3[0] = "su_confident";
				}
				else
				{
					list[0] = "* Haha!^05\n* Take that,^05 idiot!";
					list3[0] = "su_teeth_eyes";
				}
			}
			else if (flag)
			{
				list[0] = "* (Well,^05 hey,^05 we didn't\n  kill him...)";
				list3[0] = "su_smile_sweat";
			}
			else
			{
				list[0] = "* See,^05 look at how awful\n  your cult is.";
				list3[0] = "su_confident";
			}
			StartText(list.ToArray(), list2.ToArray(), new int[18], list3.ToArray(), 1);
		}
		else
		{
			itemSoundAt = 1;
			Util.FindObjectOfType<CarpainterNPC>().SetSprite(4);
			StartText(new string[2] { "* (Susie got the Bomb.)", "* ..." }, new string[2] { "snd_text", "" }, new int[2], new string[2] { "", "su_depressed" }, 1);
		}
	}
}
