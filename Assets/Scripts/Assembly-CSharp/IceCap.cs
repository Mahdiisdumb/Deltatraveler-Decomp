using UnityEngine;

public class IceCap : EnemyBase
{
	private bool ice;

	private bool pissed;

	private int response = -1;

	private int ignoreCount;

	private Sprite[] bodySprites = new Sprite[4];

	private Sprite[] rageSprites = new Sprite[4];

	private int bodyFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Ice Cap";
		fileName = "icecap";
		checkDesc = "* This teen is a little too\n  obsessed with its hat.";
		maxHp = 190;
		hp = maxHp;
		hpToUnhostile = 0;
		hostile = true;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 11;
		def = 3;
		displayedDef = 4;
		chatter = new string[3] { "Where's \nYOUR \nhat?", "Your \nhead \nlooks so \n...NAKED", "You look \nso LAME \nand \nBORING" };
		flavorTxt = new string[5] { "* Ice Cap also wants a hat\n  for its nose.", "* Ice Cap makes sure its hat\n  is still there.", "* Ice Cap is thinking about a\n  certain article of clothing.", "* Here comes that new clothes\n  smell.", "* Ice Cap wants more spikes on\n  its hat." };
		dyingTxt = new string[1] { "* Ice Cap's hat is loose." };
		satisfyTxt = flavorTxt;
		actNames = new string[4] { "Compliment", "Ignore", "SN!XIgnore", "Steal" };
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 12;
		gold = 6;
		attacks = new int[1] { 80 };
		for (int i = 0; i < 4; i++)
		{
			bodySprites[i] = Resources.Load<Sprite>("battle/enemies/Ice Cap/spr_b_icecap_" + i);
			rageSprites[i] = Resources.Load<Sprite>("battle/enemies/Ice Cap/spr_b_icecap_fury_" + i);
		}
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 111, 73f);
		if (Object.FindObjectOfType<BattleManager>().GetBattleID() == 71)
		{
			defaultChatPos.x -= 24f;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit && !ice)
		{
			bodyFrames++;
			if (pissed)
			{
				GetPart("body").GetComponent<SpriteRenderer>().sprite = rageSprites[bodyFrames / 8 % 4];
			}
			else
			{
				GetPart("body").GetComponent<SpriteRenderer>().sprite = bodySprites[bodyFrames / 10 % 4];
			}
		}
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (partyMember == 2 && (bool)Object.FindObjectOfType<IceShock>())
		{
			return base.CalculateDamage(partyMember, rawDmg, forceMagic) / 20;
		}
		return base.CalculateDamage(partyMember, rawDmg, forceMagic);
	}

	public override string[] PerformAct(int i)
	{
		if (ice && GetActNames()[i] == "Check")
		{
			return new string[1] { "* ICE - ATK 1 DEF 0\n* Without its cap..." };
		}
		if (GetActNames()[i] == "Compliment")
		{
			response = 0;
			if (ice)
			{
				if (satisfied < 100)
				{
					AddActPoints(100);
				}
				return new string[1] { "* You inform Ice Cap that\n  it still looks fine..." };
			}
			if (pissed)
			{
				response = -1;
				return new string[1] { "* You tried complimenting\n  Ice Cap to calm it down,^05\n  but it interrupted you." };
			}
			return new string[2]
			{
				"* You inform Ice Cap that\n  it has a great hat!",
				(ignoreCount >= 3) ? "* It sees your compliments\n  as insincere." : "* It begins to act smug."
			};
		}
		if (GetActNames()[i] == "Ignore" || GetActNames()[i] == "SN!XIgnore")
		{
			bool flag = GetActNames()[i] == "SN!XIgnore";
			string text = ((ignoreCount == 0) ? "* You manage to tear your\n  eyes away from Ice Cap's\n  hat." : "* You continue not looking\n  at Ice Cap's hat.");
			ignoreCount++;
			if (flag)
			{
				ignoreCount++;
				text = "* Everyone looked away from\n  Ice Cap's hat.";
			}
			response = 1;
			if (ignoreCount != 0)
			{
				if (ignoreCount != 1)
				{
					if (ignoreCount == 2 || (ignoreCount == 3 && flag))
					{
						pissed = true;
						bodyFrames = 0;
						GetPart("body").GetComponent<SpriteRenderer>().sprite = rageSprites[0];
						return new string[2] { text, "* Suddenly, Ice Cap starts\n  a riot in rage!" };
					}
					if (pissed)
					{
						pissed = false;
						bodyFrames = 0;
						GetPart("body").GetComponent<SpriteRenderer>().sprite = bodySprites[0];
					}
					return new string[2] { text, "* It seems defeated..." };
				}
				return new string[2] { text, "* It looks annoyed..." };
			}
			return new string[1] { "* Help babe" };
		}
		if (GetActNames()[i] == "Steal")
		{
			if ((ignoreCount >= 3 && !pissed) || (float)hp / (float)maxHp <= 0.2f)
			{
				fileName = "ice";
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ice Cap/spr_b_ice");
				Unhostile();
				ice = true;
				def = -200;
				flavorTxt = new string[2] { "* 'Ice Cap' is no more.", "* It smells like frozen despair." };
				satisfyTxt = new string[1] { "* Ice doesn't mind its identity." };
				dyingTxt = new string[1] { "* It's melting." };
				chatter = new string[4] { "I... \nI...", "What can \nI say...", "What's \nthe \npoint...", "So... \nCold..." };
				actNames = new string[2] { "Check", "Compliment" };
				return new string[2] { "* You tried to steal Ice Cap's\n  hat...", "* And succeeded!\n* (It melts in your hands...)" };
			}
			response = 2;
			return new string[2] { "* You tried to steal Ice Cap's\n  hat...", "* ... but it's not weakened\n  enough!" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			if (response != 1)
			{
				response = 3;
			}
			if (ice)
			{
				tired = true;
				return new string[2] { "* Susie insults Ice.", "* It feels dejected..." };
			}
			return new string[2]
			{
				"* Susie insults Ice Cap.",
				pissed ? "* It starts to snap back\n  at her!" : "* It looks bothered..."
			};
		case 2:
			if (!ice)
			{
				if (response != 1)
				{
					response = 4;
				}
				return new string[3]
				{
					"* Noelle tried to reason with\n  Ice Cap...",
					"* ... but it snapped back at\n  her!",
					pissed ? "no_silent`snd_txtnoe`* (Kris...^05 help...)" : "no_shocked`snd_txtnoe`* Hey, that's rude!"
				};
			}
			response = 0;
			if (satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[2] { "* Noelle complimented Ice on\n  its new look.", "no_happy`snd_txtnoe`* Hey,^05 you don't need\n  to be defined by\n  a hat!" };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (response > -1)
		{
			string text2 = "Umm...\nhat \nerror???";
			if (response == 0)
			{
				text2 = (ice ? (new string[4] { "Yeah... \nI like \nmy hair \ntoo.", "Hmm... \nHats are \nfor \nposers.", "So I can \nstill \nimpress \nyou?", "I wanted \nyou to \nsee me \nas cool." })[Random.Range(0, 4)] : ((ignoreCount < 3) ? (new string[3] { "Envious?\nTOO BAD!", "My hat's \ntoo loud \nfor me \nto hear \nyou.", "DUH!\nWho \nDOESN'T \nknow?" })[Random.Range(0, 3)] : "Why \nshould I \ntrust \nYOU?"));
			}
			else if (response == 1)
			{
				text2 = (pissed ? "LOOK \nAT MY \nDAMN \nHAT!!!\n!!!!!!" : ((ignoreCount < 3) ? (new string[2] { "HELLO???\nMy hat's \nup here.", "What?\nWhat are \nyou \ndoing?" })[Random.Range(0, 2)] : (new string[2] { "Fine!!!\nI don't \ncare!!!", "Better \na hatter \nthan a \nHATER." })[Random.Range(0, 2)]));
			}
			else if (response == 2 && !ice)
			{
				text2 = ((!pissed) ? (new string[2] { "I KNEW \nIT!!! \nTHIEF!!", "HELP!!! \nFASHION \nMOB!!!" })[Random.Range(0, 2)] : "GET YOUR \nHANDS \nOFF, \nFUCKER!");
			}
			else if (response == 3)
			{
				text2 = (ice ? "I guess \nI can't \nplease \neveryone \n..." : ((!pissed) ? "What a \nfreaking \nHATER!" : "YOUR POOR \nASS \nWOULDN'T \nKNOW \nFASHION!"));
			}
			else if (response == 4)
			{
				text2 = ((!pissed) ? "Says \nthe \nstupid \nX-mas \ndeer!" : "SAYS \nTHE \nSTUPID \nX-MAS \nDEER!!!");
			}
			text = new string[1] { text2 };
			response = -1;
		}
		else if (pissed)
		{
			return;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override string GetRandomFlavorText()
	{
		if (!ice)
		{
			if (pissed)
			{
				return "* Ice Cap is raging around you!";
			}
			if (ignoreCount == 1)
			{
				return "* Ice Cap is secretly checking\n  if you're looking at\n  its hat.";
			}
			if (ignoreCount >= 3)
			{
				return "* Ice Cap is desperate for\n  attention.";
			}
		}
		return base.GetRandomFlavorText();
	}

	public override string GetName()
	{
		if (ice)
		{
			return "Ice";
		}
		return base.GetName();
	}

	public bool IsIce()
	{
		return ice;
	}

	public bool IsPissed()
	{
		return pissed;
	}
}
