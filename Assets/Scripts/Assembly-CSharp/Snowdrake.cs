using UnityEngine;

public class Snowdrake : EnemyBase
{
	private bool saidJoke;

	private bool chilldrake;

	private bool saidSomething;

	private bool laughed;

	private int respond;

	private int headFrames;

	private int lastI = -1;

	private Sprite[] sprites = new Sprite[15];

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Snowdrake";
		fileName = "snowdrake";
		checkDesc = "* This Snowdrake appears to\n  be eerily familiar...";
		maxHp = 255;
		hp = maxHp;
		hpToUnhostile = maxHp / 4;
		hostile = true;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 12;
		def = 2;
		displayedDef = 6;
		chatter = new string[3] { "I'll\nleave \nyou to \nfreeze \nout here", "You'll\nturn \nto \"snow\"\nafter \nthis.", "Fights \nyou in \n\"cold\" \nblood." };
		flavorTxt = new string[3] { "* Snowdrake is too aggressive\n  to think about puns.", "* It smells like a wet pillow.", "* Snowdrake is staring into\n  the crowd." };
		dyingTxt = new string[1] { "* Snowdrake is flaking\n  apart." };
		satisfyTxt = flavorTxt;
		actNames = new string[3] { "S!Heckle", "SN!Laugh", "Joke" };
		hurtSound = "sounds/snd_hurtdragon";
		defaultChatSize = "RightSmall";
		exp = 18;
		gold = 8;
		attacks = new int[1] { 80 };
		for (int i = 0; i < 15; i++)
		{
			sprites[i] = Resources.Load<Sprite>("battle/enemies/Snowdrake/spr_snowdrake_head_" + i);
		}
		if ((int)Util.GameManager().GetFlag(180) == 1)
		{
			chilldrake = true;
			checkDesc = "* Rebel gone violent!\n* Looking for its friend Snowy.";
			actNames = new string[3] { "SN!Agree", "S!Clash", "Joke" };
			flavorTxt = new string[5] { "* Chilldrake is calling for\n  violence.", "* Chilldrake is chanting an\n  anarchist spell.", "* Chilldrake is eating its\n  own homework.", "* It smells like \"Ice\"\n  scented body-spray.", "* Chilldrake is wondering\n  where Snowy went." };
			chatter = new string[4] { "I'm gonna \nactually \nkill \nyou.", "Sleep \nwith one \neye \nopen...", "I'll make \nsure you \nfreeze \nto death", "*pretends \nto slice \nthroat*" };
			dyingTxt[0] = "* Chilldrake is flaking\n  apart.";
		}
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 107, 95f);
		if (!chilldrake)
		{
			Object.Destroy(GetPart("head-anchor").Find("head").Find("glasses").gameObject);
		}
		Object.Destroy(GetPart("head-anchor").Find("head").Find("feral").gameObject);
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		int num = headFrames % 84;
		float num2 = 0f;
		if (num < 50)
		{
			num2 = (float)num / 30f;
			if (num2 > 1f)
			{
				num2 = 1f;
			}
		}
		else
		{
			num2 = 1f - (float)(num - 50) / 30f;
			if (num2 < 0f)
			{
				num2 = 0f;
			}
		}
		GetPart("head-anchor").localPosition = new Vector3(-1f / 24f, Mathf.Lerp(0.957f, 0.707f, num2 * num2 * (3f - 2f * num2)) + 1.5208334f);
		GetPart("body").transform.localPosition = new Vector3(-1f / 24f, Mathf.Lerp(0.373f, 0.293f, num2 * num2 * (3f - 2f * num2)));
		if (headFrames >= 60)
		{
			int num3 = Mathf.FloorToInt(5f / 58f * (float)(headFrames - 60));
			if (num3 != lastI)
			{
				GetPart("head-anchor").Find("head").GetComponent<SpriteRenderer>().sprite = sprites[num3 + 4];
			}
			lastI = num3;
		}
		else if (headFrames == 0 || headFrames == 30)
		{
			GetPart("head-anchor").Find("head").GetComponent<SpriteRenderer>().sprite = sprites[1];
		}
		else if (headFrames == 8)
		{
			GetPart("head-anchor").Find("head").GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
		else if (headFrames == 39)
		{
			GetPart("head-anchor").Find("head").GetComponent<SpriteRenderer>().sprite = sprites[2];
		}
		headFrames = (headFrames + 1) % 168;
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (partyMember == 2 && (bool)Object.FindObjectOfType<IceShock>())
		{
			return base.CalculateDamage(partyMember, rawDmg, forceMagic) / 20;
		}
		return base.CalculateDamage(partyMember, rawDmg, forceMagic);
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (partyMember == 2 && (bool)Object.FindObjectOfType<IceShock>())
		{
			satisfied += 50;
			if (satisfied >= 100 && hostile)
			{
				Unhostile();
				respond = 5;
			}
		}
	}

	public override string GetName()
	{
		if (chilldrake)
		{
			return "Chilldrake";
		}
		return base.GetName();
	}

	public override bool CanSpare()
	{
		if (hp > hpToUnhostile)
		{
			return base.CanSpare();
		}
		return true;
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "SN!Laugh" || GetActNames()[i] == "SN!Agree")
		{
			respond = 1;
			if (saidSomething)
			{
				if (hostile)
				{
					if (!chilldrake)
					{
						return new string[1] { "* You all laugh at Snowdrake's\n  threat." };
					}
					return new string[2] { "* You agree with Chilldrake.\n* Susie and Noelle don't seem\n  very comfortable.", "su_side_sweat`snd_txtsus`* (Kris,^05 now isn't the\n  time to joke around.)" };
				}
				if (laughed)
				{
					Spare();
					if (!chilldrake)
					{
						return new string[3] { "* Before you could laugh,^05 Susie\n  spared Snowdrake!", "su_annoyed`snd_txtsus`* Okay,^05 I think we've\n  had enough.", "no_confused`snd_txtnoe`* ??????" };
					}
					return new string[1] { "* You all reaffirmed your\n  agreement.\n* Chilldrake ran away!" };
				}
				AddActPoints(100);
				laughed = true;
				if (!chilldrake)
				{
					return new string[2] { "* You all laugh at Snowdrake's\n  pun.", "su_annoyed`snd_txtsus`* (That was actually\n  pretty lame...)" };
				}
				return new string[1] { "* You all agree with Chilldrake.\n* Chilldrake feels neutered by\n  this." };
			}
			return new string[2]
			{
				chilldrake ? "* You agree with Chilldrake\n  before it says anything." : "* You laugh at Snowdrake before\n  it says anything.",
				"* Susie and Noelle just stare\n  at you."
			};
		}
		if (GetActNames()[i] == "S!Heckle" || GetActNames()[i] == "S!Clash")
		{
			respond = 2;
			if (saidSomething)
			{
				if (hostile)
				{
					Unhostile();
					if (!chilldrake)
					{
						return new string[2] { "* You and Susie boo the\n  Snowdrake.", "su_pissed`snd_txtsus`* You're so PATHETIC.\n* You're no different than\n  the Snowy I know." };
					}
					return new string[2] { "* You and Susie condemn\n  Chilldrake.", "su_pissed`snd_txtsus`* Jeez,^05 what's up with\n  all the terrorizing?!\n* Y'all suck." };
				}
				tired = true;
				if (!chilldrake)
				{
					return new string[3] { "* You and Susie boo the\n  Snowdrake.", "su_annoyed`snd_txtsus`* AND your jokes suck???\n* Literally unchanged.", "* Snowdrake felt discouraged and\n  became TIRED." };
				}
				return new string[3] { "* You and Susie tell Chilldrake\n  it's all wrong.", "su_annoyed`snd_txtsus`* God,^05 and you're so\n  tryhard,^05 too.", "* Chilldrake felt discouraged and\n  became TIRED." };
			}
			if (!chilldrake)
			{
				return new string[2] { "* You and Susie boo the\n  Snowdrake.", "no_shocked`snd_txtnoe`* ...^05 But he hasn't even\n  said anything..." };
			}
			return new string[2] { "* You and Susie condemn\n  Snowdrake.", "no_shocked`snd_txtnoe`* ...^05 But they haven't even\n  said anything..." };
		}
		if (GetActNames()[i] == "Joke")
		{
			respond = 3;
			if (hostile)
			{
				if (!chilldrake)
				{
					return new string[1] { "* You make a bad ice pun.\n* Snowdrake seems annoyed." };
				}
				return new string[1] { "* You make a bad ice pun.\n* Chilldrake seems annoyed." };
			}
			if (chilldrake)
			{
				AddActPoints(10);
				return new string[1] { "* You make a bad ice pun.\n* Chilldrake doesn't seem too\n  impressed." };
			}
			AddActPoints(20);
			return new string[1] { "* You make a bad ice pun.\n* Snowdrake seems to like it...?" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			if (!chilldrake)
			{
				return new string[2]
				{
					"* Susie points her weapon\n  at Snowdrake.",
					hostile ? "* Snowdrake looks ready to\n  snap the branch in half." : "* Snowdrake flinches..."
				};
			}
			return new string[2]
			{
				"* Susie points her weapon\n  at Chilldrake.",
				hostile ? "* Chilldrake looks ready to\n  snap the branch in half." : "* Chilldrake flinches,^05 but then\n  tries to look tough again."
			};
		case 2:
			if (!hostile)
			{
				AddActPoints(30);
				if (!chilldrake)
				{
					return new string[1] { "* Noelle flows snow through\n  the cold air.\n* Snowdrake seems impressed." };
				}
				return new string[1] { "* Noelle flows snow through\n  the cold air.\n* Chilldrake seems impressed." };
			}
			return new string[1] { "* Noelle flows snow through\n  the cold air.\n* Nothing happens." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (respond > 0 && (respond != 2 || !hostile))
		{
			if (hostile)
			{
				if (respond == 1 && !chilldrake)
				{
					text = new string[1] { "What are \nYOU \nlaughin' \nat?!?" };
				}
				else if (respond == 1 && chilldrake)
				{
					text = new string[1] { saidSomething ? "Then \nrun \ninto \nthe \nbullets" : "I \ndidn't \nSAY \nany-\nthing!!!" };
				}
				else if (respond == 3)
				{
					text = new string[1] { "This \nisn't a \ngame!!!" };
				}
				else if (respond == 4)
				{
					text = new string[1] { "This \nerror is \n\"cold\"" };
				}
			}
			else if (respond == 1)
			{
				text = new string[1] { chilldrake ? "Do you...\nreally\n..." : "See!?\nLaughs!\nDad was \nwrong!" };
			}
			else if (respond == 2)
			{
				text = new string[1] { chilldrake ? "WRONG!\nLet's \nfight \nand die." : "THIS \nwon't be \nfunny \neither!" };
			}
			else if (respond == 3)
			{
				text = new string[1] { chilldrake ? "That's... \nkinda \nSnowy's \nthing." : "Ha...\nHa...\nNice \ntry." };
			}
			else if (respond == 4 && !chilldrake)
			{
				text = new string[1] { "Why \ndon't we \n\"chill\" \nout?" };
			}
			else if (respond == 4 && chilldrake)
			{
				text = new string[1] { (hp <= hpToUnhostile) ? "Hey, \nI was \nJOKING!!!" : "What?\nWanna \nFIGHT \nabout \nit?" };
			}
			else if (respond == 5)
			{
				text = new string[1] { chilldrake ? "Whoa, \nthat's \nsuper \ncool!" : "Wow, \nthat ICE \nfeels \nNICE!" };
			}
		}
		else
		{
			saidSomething = true;
		}
		respond = 0;
		speed = 1;
		base.Chat(text, type, sound, pos, canSkip, speed);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override string GetRandomFlavorText()
	{
		if (laughed)
		{
			if (!chilldrake)
			{
				return "* Snowdrake is pleased with\n  its \"cool\" joke.";
			}
			return "* Chilldrake feels neutered\n  by your agreement.";
		}
		if (tired)
		{
			if (!chilldrake)
			{
				return "* Snowdrake is puffed up..";
			}
			return "* Chilldrake is puffed up..";
		}
		return base.GetRandomFlavorText();
	}

	public override void Unhostile()
	{
		if (hostile)
		{
			respond = 4;
			if (chilldrake)
			{
				chatter = new string[4] { "Brush my \nteeth? \nNo way \nin heck!", "NEVER do \nyour \nhomework\n!!", "No \nbedtime! \nOnly \nDEADTIME", "*turns \nmusic up \nall the \nway*" };
				flavorTxt[0] = "* Chilldrake starts a one-\n  monster riot.";
			}
			else
			{
				chatter = new string[7] { "Ice \nto meet \nyou.", "Ice puns \nare \n\"snow\" \nproblem", "Fights \nyou in \n\"cold\" \nblood.", "Better \nnot snow \n\"flake\" \nout!", "My fave \nice \ncereals:\n\"frosted\"", "Chill \nout...", "M.. m.. \nmacaroni \nand \n\"freeze\"" };
				flavorTxt = new string[3] { "* Snowdrake realized its own\n  name is a pun and is\n  freaking out.", "* It smells like a wet pillow.", "* Snowdrake is assessing the\n  crowd." };
			}
			satisfyTxt = flavorTxt;
			Object.Destroy(GetPart("head-anchor").Find("head").Find("pissed").gameObject);
		}
		base.Unhostile();
	}
}
