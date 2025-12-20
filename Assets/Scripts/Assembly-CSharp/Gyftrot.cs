using System.Collections.Generic;
using UnityEngine;

public class Gyftrot : EnemyBase
{
	private int decorated = 4;

	private List<SpriteRenderer> decorations = new List<SpriteRenderer>();

	private int redecorateCount;

	private int reundecorateCount;

	private bool undecorateLastTime = true;

	private bool actedLastTime;

	private int rage;

	private bool fullyUndecoratedOnce;

	private bool gifted;

	private bool moneyGift;

	private bool giftRefusal;

	private bool susieUndecorateLast;

	private bool caneGet;

	private int damage;

	private bool[] undecoratedBySusie = new bool[4];

	private int headMoveFrames;

	private int headMoveMaxFrames = 45;

	private bool headMoving;

	private bool headMovingLeft;

	private float headMoveSpeed;

	private int bodyFrames;

	private int colorShift = 60;

	private Sprite[] headSprites;

	private Sprite[] mouthSprites = new Sprite[5];

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Gyftrot";
		fileName = "gyftrot";
		checkDesc = "* Local teens have made its\n  life a living hell.";
		maxHp = 310;
		hp = maxHp;
		atk = 16;
		def = 3;
		displayedDef = 7;
		canSpareViaFight = false;
		hostile = true;
		hpToUnhostile = 0;
		chatter = new string[3] { "Get \nthis \noff \nof me...", "Damn \nkids...", "Is \nthis \nfunny \nto you???" };
		flavorTxt = new string[3] { "* Gyftrot distrusts your\n  youthful demeanor.", "* Gyftrot tries vainly to\n  remove its decorations.", "* Ah,^05 the scent of fresh\n  pine needles." };
		dyingTxt = new string[1] { "* Gyftrot's antlers tremble." };
		satisfyTxt = new string[1] { "* Gyftrot's problems have\n  been taken away." };
		actNames = new string[3] { "Decorate", "N!Undecorate", "SN!Gift" };
		exp = 50;
		gold = 30;
		defaultChatSize = "RightSmall";
		defaultChatPos = new Vector2(149f, 75f);
		attacks = new int[1] { 86 };
		hpPos = new Vector2(0f, 122f);
		hpWidth = 202;
		headSprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Gyftrot/spr_b_gyftrot_head_0"),
			Resources.Load<Sprite>("battle/enemies/Gyftrot/spr_b_gyftrot_head_1")
		};
		for (int i = 0; i < 5; i++)
		{
			mouthSprites[i] = Resources.Load<Sprite>("battle/enemies/Gyftrot/spr_b_gyftrot_mouth_" + i);
		}
	}

	protected override void Start()
	{
		base.Start();
		string[] array = new string[4] { "googlyeyes", "photo", "candycane", "barbwire" };
		for (int num = 3; num >= 0; num--)
		{
			decorations.Add(GetPart("head").Find("decorations").Find(array[num]).GetComponent<SpriteRenderer>());
		}
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		bodyFrames++;
		int num = (bodyFrames + 162) % 180;
		int num2 = num / 3;
		if (num >= 60)
		{
			num2 = (75 - num) / 3;
		}
		if (num2 < 0)
		{
			num2 = 0;
		}
		else if (num2 > 4)
		{
			num2 = 4;
		}
		GetPart("head").Find("mouth").GetComponent<SpriteRenderer>().sprite = mouthSprites[num2];
		if (num == 17)
		{
			Object.Instantiate(Resources.Load<GameObject>("vfx/GyftrotBreath"), new Vector3(GetPart("head").position.x, 1.184f), Quaternion.identity);
		}
		if (bodyFrames % 67 <= 60)
		{
			GetPart("head").GetComponent<SpriteRenderer>().sprite = headSprites[0];
		}
		else
		{
			GetPart("head").GetComponent<SpriteRenderer>().sprite = headSprites[1];
		}
		headMoveFrames++;
		if (headMoveFrames >= headMoveMaxFrames)
		{
			if (headMoving)
			{
				headMoving = false;
				headMoveMaxFrames = Random.Range(40, 50);
			}
			else
			{
				headMoving = true;
				if (!headMovingLeft && GetPart("head").transform.localPosition.x >= 0.21f)
				{
					headMovingLeft = true;
				}
				else if (headMovingLeft && GetPart("head").transform.localPosition.x <= -0.21f)
				{
					headMovingLeft = false;
				}
				int num3 = Random.Range(1, 3);
				headMoveSpeed = (float)num3 / 48f;
				if (num3 == 1)
				{
					headMoveMaxFrames = Random.Range(13, 17);
				}
				else
				{
					headMoveMaxFrames = Random.Range(10, 15);
				}
			}
			headMoveFrames = 0;
		}
		if (headMoving)
		{
			if (headMovingLeft)
			{
				GetPart("head").transform.localPosition -= new Vector3(headMoveSpeed, 0f);
			}
			else
			{
				GetPart("head").transform.localPosition += new Vector3(headMoveSpeed, 0f);
			}
			if ((!headMovingLeft && GetPart("head").transform.localPosition.x >= 0.32f) || (headMovingLeft && GetPart("head").transform.localPosition.x <= -0.32f))
			{
				headMoving = false;
				headMoveMaxFrames = Random.Range(40, 50);
			}
		}
		if (!hostile || colorShift >= 60)
		{
			return;
		}
		colorShift++;
		SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (!decorations.Contains(spriteRenderer))
			{
				spriteRenderer.color = Color.Lerp(Color.white, new Color32(237, 28, 36, byte.MaxValue), (float)colorShift / 60f);
			}
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Decorate")
		{
			if (!gifted)
			{
				if (decorated >= 4)
				{
					if (redecorateCount <= 0)
					{
						return new string[1] { "* There's nothing around to add." };
					}
					return new string[1] { "su_annoyed`snd_txtsus`* You sure this is\n  the guy we should\n  be pranking...?" };
				}
				actedLastTime = true;
				decorations[decorated].color = new Color(1f, 1f, 1f, 1f);
				AddActPoints(-25);
				if (!hostile)
				{
					hostile = true;
					PlaySFX("sounds/snd_deathnoise");
				}
				decorated++;
				string text = "* You readd what you\n  removed before.";
				if (undecoratedBySusie[decorated - 1])
				{
					text = "* You took the last item\n  from Susie and added it.";
				}
				else if (decorated == 2)
				{
					text = "* You took the last item\n  from Noelle and added it.";
				}
				else if (undecorateLastTime)
				{
					text = "* You readd what you\n  last removed.";
				}
				if (undecorateLastTime)
				{
					redecorateCount++;
					if (exp < 80)
					{
						exp += 5;
						playerMultiplier += 0.25f;
					}
					undecorateLastTime = false;
				}
				if (decorated == 1)
				{
					if (Util.GameManager().GetHP(0) > 1)
					{
						Util.GameManager().Damage(0, 1);
					}
					Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
					return new string[1] { text + "\n^05* The barbed wire." };
				}
				if (decorated == 4)
				{
					if (fullyUndecoratedOnce)
					{
						tired = true;
					}
					return new string[2]
					{
						text,
						fullyUndecoratedOnce ? "* Gyftrot's anger turns into\n  exhaustion." : "* Gyftrot is back to square one."
					};
				}
				return new string[1] { text };
			}
			return new string[3] { "* You considered readding what\n  you last removed...", "* But after everything you've\n  been through,^05 you couldn't\n  do it.", "no_happy`snd_txtnoe`* Is everything okay,^05 Kris?" };
		}
		if (GetActNames()[i] == "N!Undecorate")
		{
			return Undecorate(susie: false);
		}
		if (GetActNames()[i] == "SN!Gift")
		{
			if (decorated > 0 || redecorateCount > 0)
			{
				actedLastTime = true;
				giftRefusal = true;
				return new string[1] { tired ? "* It looks like Gyftrot\n  is about to bash into\n  you head-first." : "* Gyftrot refuses your gift." };
			}
			if (!gifted)
			{
				actedLastTime = true;
				Object.Instantiate(Resources.Load<GameObject>("battle/enemies/Gyftrot/GyftrotGift"));
				int num = Util.GameManager().GetGold();
				if ((int)Util.GameManager().GetFlag(211) == 1 && num > 0)
				{
					Util.GameManager().RemoveGold(num);
					moneyGift = true;
					return new string[1] { "* You give all of your money\n  because you can't think\n  of a better gift." };
				}
				if (num > 35)
				{
					Util.GameManager().RemoveGold(35);
					moneyGift = true;
					return new string[1] { "* You give 35 G because\n  you can't think of an\n  appropriate gift." };
				}
				if (num > 0)
				{
					Util.GameManager().RemoveGold(num - 1);
					moneyGift = true;
					return new string[3]
					{
						"* You give your remaining money--",
						"su_annoyed`snd_txtsus`* At least keep\n  SOMETHING.",
						"* You give " + (num - 1) + " G because\n  you can't think of an\n  appropriate gift."
					};
				}
				return new string[1] { "* You give the cheapest gift\n  of all...^05\n* Friendship." };
			}
			if (Util.GameManager().GetGold() == 0 && moneyGift)
			{
				return new string[1] { "* You could give more,^05 but\n  you need it to get home." };
			}
			if (moneyGift)
			{
				return new string[1] { "* You probably shouldn't give\n  any more money." };
			}
			return new string[1] { "* You've already given it\n  your all." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		switch (i)
		{
		case 1:
			return Undecorate(susie: true);
		case 2:
			if (rage > 0)
			{
				rage--;
				return new string[1] { "* Noelle tried to calm Gyftrot\n  down.\n* Gyftrot's attacks became easier." };
			}
			return new string[1] { "* Noelle tried to calm Gyftrot\n  down.\n* Nothing happened." };
		default:
			return base.PerformAssistAct_Old(i);
		}
	}

	private string[] Undecorate(bool susie)
	{
		susieUndecorateLast = susie;
		if (decorated <= 0)
		{
			if (!susie)
			{
				return new string[3] { "* You try to undecorate...?", "no_confused`snd_txtnoe`* Kris,^05 you CAN'T remove\n  antlers.^05\n* I've tried.", "su_inquisitive`snd_txtsus`* You WHAT." };
			}
			return new string[1] { "su_side`snd_txtsus`* Yeah,^05 uhh...^05\n* Looks good." };
		}
		actedLastTime = true;
		if (!undecorateLastTime)
		{
			reundecorateCount++;
			undecorateLastTime = true;
		}
		string text = (susie ? "* Susie " : "* You and Noelle ");
		if ((redecorateCount < 2 || susie) && rage > 0)
		{
			rage--;
		}
		decorated--;
		AddActPoints(25);
		undecoratedBySusie[decorated] = susie;
		decorations[decorated].color = new Color(1f, 1f, 1f, 0f);
		if (decorated == 0)
		{
			fullyUndecoratedOnce = true;
			if (hostile)
			{
				Unhostile();
			}
		}
		if (decorated != 3)
		{
			if (decorated != 2)
			{
				if (decorated == 1)
				{
					if (!susie)
					{
						return new string[2] { "* You and Noelle removed the\n  striped cane that says \"I use\n  this tiny cane to walk\" on it.", "* Noelle pockets the cane." };
					}
					return new string[1] { "* Susie removed the striped cane\n  that says \"I use this tiny\n  cane to walk\" on it." };
				}
				if (decorated == 0)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
					if (susie)
					{
						if (Util.GameManager().GetHP(1) > 1)
						{
							Util.GameManager().Damage(1, 1);
						}
						return new string[1] { "* Susie removed some actual\n  barbed wire.\n* Ouch!" };
					}
					return new string[1] { "* You removed some actual\n  barbed wire.\n* Noelle heals your wound." };
				}
				return new string[1] { "* Removed Bepis" };
			}
			return new string[1] { text + "removed the\n  family photo of Snowy\n  and his father." };
		}
		return new string[1] { text + "removed the\n  googly eyes haphazardly\n  blocking Gyftrot's vision." };
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (partyMember == 2 && (bool)Util.FindObjectOfType<IceShock>())
		{
			return base.CalculateDamage(partyMember, rawDmg, forceMagic) / 10;
		}
		return base.CalculateDamage(partyMember, rawDmg, forceMagic);
	}

	public override string GetRandomFlavorText()
	{
		if ((float)hp / (float)maxHp <= 0.2f && (tired || redecorateCount >= 2))
		{
			return "* Gyftrot might prefer death\n  at this point.";
		}
		if (tired)
		{
			if (decorated != 0)
			{
				return "* Gyftrot is huffing and\n  puffing loudly.";
			}
			return "* Gyftrot is tempted to jump\n  off the cliff.";
		}
		if (redecorateCount < 2)
		{
			if (undecorateLastTime && decorated < 4 && decorated > 0)
			{
				return "* Gyftrot is slightly less\n  irritated.";
			}
			if (decorated == 4 && Random.Range(0, 4) == 0)
			{
				return "* Gyftrot stumbles blindly.";
			}
			if (!undecorateLastTime && redecorateCount == 1 && decorated < 4)
			{
				return "* Gyftrot looks slightly more\n  irritated.";
			}
			if (decorated == 0 && redecorateCount == 1)
			{
				return "* Gyftrot's problems have\n  been taken away.^05\n* Again.";
			}
			if (redecorateCount == 1)
			{
				return "* Gyftrot looks disappointed.";
			}
		}
		else if (redecorateCount >= 2)
		{
			if (decorated == 0)
			{
				return "* Is this truly the end?!";
			}
			if (!undecorateLastTime)
			{
				return "* Gyftrot is in hell.";
			}
			return "* Down it goes again.";
		}
		return base.GetRandomFlavorText();
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		speed = 1;
		if (tired)
		{
			text[0] = ((Random.Range(0, 2) == 0) ? "..." : "You \nmight \nas well \nkill me");
		}
		if (decorated == 4 && Random.Range(0, 4) == 0)
		{
			text[0] = "Can't \nsee...";
		}
		if (giftRefusal)
		{
			giftRefusal = false;
			text[0] = ((redecorateCount > 0) ? "I don't \nwant \nyour \ngift!" : "How do \nI know \nit's not \na trick?");
		}
		if (redecorateCount == 0 && actedLastTime)
		{
			if (gifted)
			{
				text[0] = (moneyGift ? "Aw, you \nshouldn' \nhave..." : "You even \nwrapped \nit...");
			}
			else if (decorated == 3)
			{
				text[0] = "I CAN \nSEE!!!";
			}
			else if (decorated > 0)
			{
				text[0] = "That's \na little \nbetter.";
			}
			else
			{
				text[0] = "A weight \nhas been \nlifted.";
			}
		}
		else if (redecorateCount == 1)
		{
			if (!undecorateLastTime)
			{
				if (decorated > 1)
				{
					text[0] = "Please...\nstop...";
				}
				else
				{
					text[0] = "WHAT ARE \nYOU \nDOING???";
				}
			}
			else if (decorated > 0)
			{
				text[0] = "Please \njust \nfinish \nit.";
			}
			else
			{
				text[0] = "Thank god";
			}
		}
		else if (redecorateCount >= 2)
		{
			if (!undecorateLastTime)
			{
				if (decorated > 1)
				{
					text[0] = "This is \nhell.";
				}
				else
				{
					text[0] = "Oh, the\ncycle\ncontinues";
				}
			}
			else if (decorated > 0)
			{
				text[0] = "Please \njust \nfinish \nit.";
			}
			else
			{
				text[0] = "Thank god";
			}
		}
		base.Chat(text, type, sound, pos, canSkip, speed);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override int GetNextAttack()
	{
		if (satisfied >= 100 && redecorateCount == 0)
		{
			return -1;
		}
		return base.GetNextAttack();
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		int num = hp;
		base.Hit(partyMember, rawDmg, playSound);
		damage += num - hp;
	}

	public override void EnemyTurnStart()
	{
		base.EnemyTurnStart();
		rage += damage / 50;
		damage = 0;
	}

	public override void Unhostile()
	{
		base.Unhostile();
		colorShift = 0;
	}

	public override void Spare(bool sleepMist = false)
	{
		if (sleepMist)
		{
			Util.GameManager().SetFlag(266, 1);
		}
		if (decorated == 0)
		{
			Util.GameManager().SetFlag(267, (!tired) ? 1 : 2);
		}
		if (redecorateCount > 0)
		{
			Util.GameManager().SetFlag(268, 1);
		}
		base.Spare(sleepMist);
	}

	public int GetRage()
	{
		return rage;
	}
}
