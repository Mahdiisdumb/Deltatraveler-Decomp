using System.Collections.Generic;
using UnityEngine;

public class QCNPC : InteractTextBox
{
	protected override void Awake()
	{
		base.Awake();
		if ((int)Util.GameManager().GetFlag(208) == 2)
		{
			InteractTextBox component = GameObject.Find("SittingBunnies").GetComponent<InteractTextBox>();
			InteractTextBox component2 = GameObject.Find("QCSis").GetComponent<InteractTextBox>();
			InteractTextBox component3 = GameObject.Find("Fridge").GetComponent<InteractTextBox>();
			component3.ModifyContents(new string[5]
			{
				component3.GetLines()[0],
				"* Please don't take anything\n  from the fridge!",
				component3.GetLines()[2],
				component3.GetLines()[3],
				component3.GetLines()[4]
			}, component3.GetSounds(), new int[1], component3.GetPortraits());
			if ((int)Util.GameManager().GetFlag(209) != 0)
			{
				if ((int)Util.GameManager().GetFlag(87) >= 7)
				{
					component.ModifyContents(new string[4] { "* Huh?^05\n* No, I'm aware the forest is\n  now safer without the beasts.", "* But I get this strange feeling\n  that still things aren't safe.", "* I feel like it's something\n  with you guys...", "* I'd prefer if we never speak\n  again." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
					component.ModifySecondaryContents(new string[1] { "* Please leave us alone." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
					component2.ModifyContents(new string[3] { "* I wonder why she feels so\n  uncomfortable going outside.", "* It's so strange.^05\n* I've never seen her be so\n  anti-human before.", "* I hope she's okay." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				}
				else if ((int)Util.GameManager().GetFlag(229) == 1 || (int)Util.GameManager().GetFlag(229) == 2)
				{
					component.ModifyContents(new string[5] { "* We tried to go out for a\n  walk earlier,^05 but out of\n  the shadows...", "* A feral snowdrake attacked\n  us.", "* I didn't know there would\n  be more after the ones you\n  guys dealt with.", "* Looks like no more walks for\n  us anymore...", "* ..." }, new string[5] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtnoe" }, new int[1], new string[5] { "", "", "", "", "no_depressed" });
					component.DisableSecondaryLines();
					component2.ModifyContents(new string[4] { "* Weren't the feral snowdrakes the\n  ones guarding the ladders in\n  the first place?", "* How can there be more...?", "* Unless you let one of them go.", "* Was there not a better option\n  of disposing of them?" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				}
				else
				{
					Object.Destroy(component.gameObject);
					GameObject.Find("TV").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (The TV is turned off.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
					component2.ModifyContents(new string[3] { "* It truly feels like things\n  are healing here.", "* My sister can get us supplies\n  again,^05 the other two are taking\n  walks...", "* It would be a miracle if\n  somehow the barrier broke too,^05\n  but that might be TOO hopeful." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				}
			}
			component2.ModifySecondaryContents(new string[5] { "* No,^05 we aren't worried about\n  getting killed while my\n  sister's gone.", "* Y'see,^05 we found this really\n  powerful human weapon a long\n  time ago...", "* We have it as a last resort,^05\n  if nothing else works.", "* It's why we didn't get\n  mauled by the Snowdrakes,^05\n  because the blast scared them.", "* I don't know what it's called,^05\n  but I like calling it a\n  <color=#FF0000FF>shotgun</color>." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			Object.Destroy(GameObject.Find("TableItems"));
			Object.Destroy(base.gameObject);
			return;
		}
		int num = 0;
		bool num2 = (int)Util.GameManager().GetFlag(227) == 1;
		bool flag = (int)Util.GameManager().GetFlag(228) == 1;
		if (num2)
		{
			num++;
		}
		if (flag)
		{
			num++;
		}
		List<string> list = new List<string>();
		switch (num)
		{
		case 0:
			list.Add("* Y'all need some guidance first?^05\n* I've got a few ideas on where\n  the last three are.");
			break;
		case 1:
			list.Add("* Looks like we need two more\n  pieces before we've got all\n  of them!");
			break;
		default:
			list.Add("* Keep up the good work,^05 y'all!\n^05* We only need one more piece!");
			break;
		}
		if (!num2)
		{
			list.Add("* I think there might be one\n^05  in the southwest of the forest,^05\n  in a pretty long deadend.");
			list.Add("* Try not to get cornered by\n  a bunch of snowdrakes.");
			if (!flag)
			{
				list.Add("* There's another in a shorter\n  deadend near the center of\n  the forest.");
				list.Add("* I've always seen it guarded\n  by a snowdrake,^05 so you'd either\n  have to lure it away...");
				list.Add("* ... Or try to kill it.");
			}
		}
		else if (!flag)
		{
			list.Add("* There's one in a pretty short\n  deadend near the center of\n  the forest.");
			list.Add("* I've always seen it guarded\n  by a snowdrake,^05 so you'd either\n  have to lure it away...");
			list.Add("* ... Or try to kill it.");
		}
		if (num >= 2)
		{
			list.Add("* I think the last one is on\n  a cliff southeast of here,^05\n  but that's a strange one.");
			list.Add("* It's never guarded by anything,\n  but everytime I'm brave enough\n  to grab it, it's gone.");
			list.Add("* It might be a trap,^05 so when\n  you grab it,^05 get ready to run.");
			list.Add("* If you're being chased,^05 yell\n  out for me when you get\n  here.");
			list.Add("* I'll take care of them...");
		}
		else
		{
			list.Add("* There's also another one near\n  a cliff southeast of here,^05\n  but that's a strange one.");
			list.Add("* It's never guarded by anything,\n  but everytime I'm brave enough\n  to grab it, it's gone.");
			list.Add("* It might be a trap,^05 so I'd say\n  grab that one last.");
		}
		list.Add("* Good luck out there,^05 you three!");
		lines = list.ToArray();
	}
}
