using UnityEngine;

public class Kris : EnemyBase
{
	private int chungusLevel;

	private int lastAct = -1;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Kris";
		checkDesc = "* They're only here for\n  testing.";
		actNames = new string[5] { "Talk;Speak to them`", "Compliment;Be nice :)`", "Threaten;Rage >:(`", "Autowin;You can flee tho`50", "Big Chungus;`10" };
		flavorTxt = new string[3] { "* Kris stares blankly at you.", "* Smells like crayons.", "* Kris is clutching their\n  fists." };
		dyingTxt = new string[1] { "* Kris is losing their grip." };
		chatter = new string[1] { "AAAAAA\n^Z^Z^Z^Z^Z^Z\nV V V V V V" };
		fileName = "kris";
		maxHp = 2000;
		hp = maxHp;
		hpPos = new Vector2(150f, 102f);
		atk = 10;
		def = 10;
		hasSoul = true;
		exp = Util.GameManager().GetLVExp() - Util.GameManager().GetEXP();
		susieMiniACTs = new MiniACT[2]
		{
			EnemyBase.SACTION_DEFAULT,
			new MiniACT("SusieTest", 1, "Fuck you!")
		};
		noelleMiniACTs = new MiniACT[2]
		{
			EnemyBase.NACTION_DEFAULT,
			new MiniACT("NoelleTest", 1, "", 20)
		};
		chungusLevel = 0;
		hpWidth = 200;
		actNames[1] = EnemyBase.MakeSpecialActString("N", actNames[1]);
		actNames[2] = EnemyBase.MakeSpecialActString("S", actNames[2]);
		actNames[3] = EnemyBase.MakeSpecialActString("SN", actNames[3]);
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
			return new string[4] { "* You talked to Kris.", "* ...", "* Doesn't seem much for\n  conversation.", "* Sarah is happy with this." };
		case 2:
			AddActPoints(15);
			return new string[1] { "* You and Noelle complimented\n  Kris on their shirt." };
		case 3:
			tired = true;
			return new string[1] { "* You and Susie threatened to\n  kill Kris.\n* Kris became TIRED." };
		case 4:
			if (chungusLevel <= 3)
			{
				chungusLevel = 4;
				AddActPoints(100);
				return new string[1] { "* Everyone brought to Kris\n  <color=#FFFF00FF>FOUR WHOLE CHUNGUSSIES</color>." };
			}
			return new string[1] { "* Chungus Level at maximum." };
		case 5:
			if (chungusLevel <= 3)
			{
				chungusLevel++;
				AddActPoints(25);
				return Localizer.FormatArray(new string[1] { "* Chungus Level - {0}" }, chungusLevel.ToString());
			}
			return new string[1] { "* Chungus Level at maximum." };
		default:
			return base.PerformAct(i);
		}
	}

	public override string[] PerformAssistAct(int partyMember, int i)
	{
		if (partyMember == 1 && i == 1)
		{
			AddActPoints(50);
			return new string[1] { "* The test worked!" };
		}
		if (partyMember == 2 && i == 1)
		{
			AddActPoints(51);
			return new string[1] { "* The test worked!\n  (but for Noelle!)" };
		}
		return PerformAssistAct_Old(partyMember);
	}

	public override string[] PerformAssistAct_Old(int i)
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
			return base.PerformAssistAct_Old(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > 0)
		{
			string[] array = new string[5] { "You literally said \n\"Blah blah blah\" \nthree times in \na row.", "Thanks!", "Not unless I \ndo it first!", "Ch... cheating?", "Whoa!" };
			if (lastAct != 5 || chungusLevel >= 4)
			{
				text = new string[1] { array[lastAct - 1] };
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
			return "* The Chungus Level is blowing\n  Kris's mind.";
		}
		return base.GetRandomFlavorText();
	}
}
