using System.Collections.Generic;
using UnityEngine;

public class Kris : EnemyBase
{
	private int chungusLevel;

	private int lastAct = -1;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("enemy_name", new string[1] { "Kris" });
		dictionary.Add("enemy_check_description", new string[1] { "* They're only here for\n  testing." });
		dictionary.Add("enemy_acts", new string[5] { "Talk;Speak to them`", "Compliment;Be nice :)`", "Threaten;Rage >:(`", "Autowin;You can flee tho`50", "Big Chungus;`10" });
		dictionary.Add("enemy_flavor_text", new string[3] { "* Kris stares blankly at you.", "* Smells like crayons.", "* Kris is clutching their\n  fists." });
		dictionary.Add("enemy_dying_text", new string[1] { "* Kris is losing their grip." });
		dictionary.Add("enemy_chatter", new string[1] { "Pee pee ^Z ^Z\nPoo Poo ^X ^X\nDeez nuts ^Ca^Ca" });
		dictionary.Add("act_talk", new string[4] { "* You talked to Kris.", "* ...", "* Doesn't seem much for\n  conversation.", "* Sarah is happy with this." });
		dictionary.Add("act_compliment", new string[1] { "* You and Noelle complimented\n  Kris on their shirt." });
		dictionary.Add("act_threaten", new string[1] { "* You and Susie threatened to\n  kill Kris.\n* Kris became TIRED." });
		dictionary.Add("act_autowin_chungussies", new string[1] { "* Everyone brought to Kris\n  <color=#FFFF00FF>FOUR WHOLE CHUNGUSSIES</color>." });
		dictionary.Add("act_autowin_nomorechungus", new string[1] { "* Chungus Level at maximum." });
		dictionary.Add("act_big_chungus", new string[1] { "* Chungus Level - {0}" });
		dictionary.Add("act_big_chungus_max", new string[1] { "* Chungus Level at maximum." });
		dictionary.Add("chatter_act_lines", new string[5] { "You literally said \n\"Blah blah blah\" \nthree times in \na row.", "Thanks!", "Not unless I \ndo it first!", "Ch... cheating?", "Whoa!" });
		dictionary.Add("flavor_chungus", new string[1] { "* The Chungus Level is blowing\n  Kris's mind." });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetInfoFromStrings();
		fileName = "kris";
		maxHp = 2000;
		hp = maxHp;
		hpPos = new Vector2(150f, 102f);
		atk = 10;
		def = 10;
		hasSoul = true;
		exp = Util.GameManager().GetLVExp() - Util.GameManager().GetEXP();
		chungusLevel = 0;
		hpWidth = 200;
		actNames[1] = EnemyBase.MakeSpecialActString("N", GetString("enemy_acts", 1));
		actNames[2] = EnemyBase.MakeSpecialActString("S", GetString("enemy_acts", 2));
		actNames[3] = EnemyBase.MakeSpecialActString("SN", GetString("enemy_acts", 3));
		attacks = new int[1] { 1 };
	}

	protected override void Update()
	{
		base.Update();
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i;
		switch (i)
		{
		case 1:
			return GetStringArray("act_talk");
		case 2:
			AddActPoints(15);
			return GetStringArray("act_compliment");
		case 3:
			tired = true;
			return GetStringArray("act_threaten");
		case 4:
			if (chungusLevel <= 3)
			{
				chungusLevel = 4;
				AddActPoints(100);
				return GetStringArray("act_autowin_chungussies");
			}
			return GetStringArray("act_autowin_nomorechungus");
		case 5:
			if (chungusLevel <= 3)
			{
				chungusLevel++;
				AddActPoints(25);
				return Localizer.FormatArray(GetStringArray("act_big_chungus"), chungusLevel.ToString());
			}
			return GetStringArray("act_big_chungus_max");
		default:
			return base.PerformAct(i);
		}
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			AddActPoints(10);
			return new string[1] { "* Susie talked about moss.\n* Both Kris's seemed to enjoy\n  this." };
		case 2:
		{
			string text = "* Noelle gave the evil Kris\n  a candycane.";
			if (chungusLevel <= 3)
			{
				chungusLevel++;
				AddActPoints(25);
				text = text + "\n* Chungus Level increased to " + chungusLevel;
			}
			return new string[1] { text };
		}
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > 0)
		{
			string[] stringArray = GetStringArray("chatter_act_lines");
			if (lastAct != 5 || chungusLevel >= 4)
			{
				text = new string[1] { stringArray[lastAct - 1] };
			}
		}
		base.Chat(text, type, "snd_txtkrs", pos, canSkip, speed);
		chatbox.transform.localPosition = new Vector2(187f + Mathf.Round(xDif * 48f), 118f);
	}

	public override string GetChatter()
	{
		if (GetHP() == 0)
		{
			return "...^15\nPlease don't take \nmy SOUL.";
		}
		return base.GetChatter();
	}

	public override string GetRandomFlavorText()
	{
		if (chungusLevel == 4)
		{
			return GetString("flavor_chungus", 0);
		}
		return base.GetRandomFlavorText();
	}
}
