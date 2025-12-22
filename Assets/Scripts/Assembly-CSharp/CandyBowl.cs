using System.Collections.Generic;
using UnityEngine;

public class CandyBowl : InteractSelectionBase
{
	[SerializeField]
	private Sprite spill;

	private int bowlState;

	private void Awake()
	{
		bowlState = (int)Util.GameManager().GetFlag(14);
		if ((int)Util.GameManager().GetFlag(108) == 1)
		{
			lines[1] = "* Hey,^05 let's just take\n  all of it.";
		}
		ReplaceText();
	}

	public override void DoInteract()
	{
		if ((bool)txt || !enabled)
		{
			return;
		}
		txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
		if (Util.GameManager().GetFlagInt(286) == 1 && bowlState > 0)
		{
			if (Util.GameManager().NumItemFreeSpace(equipment: false) == 0)
			{
				List<string> list = new List<string> { "* You wanted to place the Egg\n  that you're carrying here...", "* But you felt that you should\n  make room in your ITEMs first." };
				sounds = new string[1] { "snd_text" };
				portraits = new string[1] { "" };
				if (bowlState == 2)
				{
					list.Insert(0, "* ...");
					list.Insert(0, "* Look at what you've done.");
				}
				lines = list.ToArray();
			}
			else
			{
				Util.GameManager().SetFlag(126, 1);
				if (bowlState == 1)
				{
					Util.GameManager().SetFlag(286, 0);
					Util.GameManager().AddItem(17);
					lines = new string[4] { "* You swapped the egg that\n  you were carrying for\n  another candy.", "* For some reason,^05 this one\n  looks different from the\n  others.", "* You got the Chocolate Candy.\n* It was added to your ITEMs.", "* The hell was that\n  doing in there?" };
					sounds = new string[4] { "snd_text", "snd_text", "snd_text", "snd_txtsus" };
					portraits = new string[4] { "", "", "", "su_inquisitive" };
				}
				else if (bowlState == 2)
				{
					Util.GameManager().SetFlag(286, 0);
					Util.GameManager().AddItem(17);
					bool num = (int)Util.GameManager().GetFlag(108) == 1;
					string text = (num ? "* So,^05 a chocolate formed\n  after you knocked over\n  that stupid bowl." : "* Kris,^05 you look really\n  pleased.");
					string text2 = (num ? "* You should be\n  relishing in the spoils\n  of DESTRUCTION." : "* Are you finally\n  relishing in the spoils\n  of DESTRUCTION?");
					lines = new string[7] { "* Look at what you've done.", "* ...", "* You put the Egg in the\n  center of the podium.", "* You looked away for a\n  brief moment,^05 then saw a\n  chocolate candy in its place.", "* You got the Chocolate Candy.\n* It was added to your ITEMs.", text, text2 };
					sounds = new string[7] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus" };
					speed = new int[10];
					portraits = new string[7] { "", "", "", "", "", "su_smirk", "su_teeth_eyes" };
				}
			}
		}
		txt.CreateBox(lines, sounds, speed, bowlState != 0, portraits);
		ReplaceText();
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		if (bowlState == 0)
		{
			txt.EnableSelectionAtEnd();
		}
	}

	private void ReplaceText()
	{
		if (bowlState == 1)
		{
			lines = new string[1] { "* You considered taking more\n  candy,^10 but the mere thought\n  of it made you feel bad." };
		}
		else if (bowlState == 2)
		{
			GetComponent<SpriteRenderer>().sprite = spill;
			lines = new string[1] { "* Look at what you've done." };
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			if (Util.GameManager().NumItemFreeSpace(equipment: false) > 1)
			{
				Util.GameManager().AddItem(10);
				Util.GameManager().AddItem(10);
				Util.GameManager().SetFlag(14, 1);
				bowlState = 1;
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				sounds = new string[3] { "snd_text", "snd_txtsus", "snd_text" };
				txt.CreateBox(new string[3] { "* You took a couple pieces\n  of candy.\n* They were added to your ITEMs.", "* Whatever.^05\n* At least I get\n  something.", "* (Press ^C to open the menu.)" }, sounds, speed, giveBackControl: true, new string[2] { "", "su_annoyed" });
			}
			else
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[2] { "* You didn't have enough space\n  to get candy for you and Susie.", "* How are you already\n  carrying so much???" }, sounds, speed, giveBackControl: true, new string[2] { "", "su_angry" });
			}
		}
		else if (index == Vector2.up)
		{
			int num = Util.GameManager().NumItemFreeSpace(equipment: false);
			if (num > 1)
			{
				bool flag = (int)Util.GameManager().GetFlag(108) == 1;
				int num2 = ((num < 4) ? num : 4);
				if (flag && num2 == 4)
				{
					num2 = 3;
					num = 3;
				}
				for (int i = 0; i < num2; i++)
				{
					Util.GameManager().AddItem(10);
				}
				string text = "* But after the fourth one,^10\n  the candy spilled on the\n  floor.";
				string text2 = "* Whoops.";
				string text3 = "su_wideeye";
				switch (num)
				{
				case 3:
					text = "* But after the third one,^10\n  the candy spilled on the\n  floor.";
					break;
				case 2:
					text = "* But after the second one,^10\n  the candy spilled on the\n  floor.";
					text2 = "* If you didn't have the\n  space,^05 you shouldn't've\n  even tried.";
					text3 = "su_annoyed";
					break;
				case 1:
					text = "* But after the first one,^10\n  the candy spilled on the\n  floor.";
					text2 = "* Really,^05 Kris?";
					if (flag)
					{
						text2 = "* Really, dude?";
					}
					text3 = "su_pissed";
					break;
				}
				string text4 = "* You tried to take as much\n  candy as you could before\n  anyone else saw.";
				if (flag)
				{
					text4 = "* Because of your small size,^05\n  you could barely reach the\n  bowl.";
					text = text.Replace("But after", "After taking").Replace("one,", "candy,");
				}
				Util.GameManager().SetFlag(14, 2);
				bowlState = 2;
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[3] { text4, text, text2 }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, speed, giveBackControl: true, new string[3] { "", "", text3 });
			}
			else
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[2] { "* You didn't have enough space\n  to get candy for you and Susie.", "* How are you already\n  carrying so much???" }, sounds, speed, giveBackControl: true, new string[2] { "", "su_angry" });
			}
		}
		else if (index == Vector2.right)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* You decided not to take some." }, sounds, speed, giveBackControl: true);
		}
		ReplaceText();
	}
}
