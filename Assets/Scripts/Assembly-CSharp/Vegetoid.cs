using UnityEngine;

public class Vegetoid : EnemyBase
{
	private bool greened;

	private bool eatingGreens;

	private int lastAct = -1;

	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Vegetoid";
		fileName = "vegetoid";
		checkDesc = "* Serving Size: 1 Monster.\n* Not monitored by the USDA.";
		maxHp = 115;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 6;
		def = 7;
		flavorTxt = new string[4] { "* Vegetoid gave a mysterious\n  smile.", "* Vegetoid cackles softly.", "* Vegetoid's here for your\n  health.", "* It smells like steamed\n  carrots and peas." };
		dyingTxt = new string[1] { "* Vegetoid seems kind of\n  bruised." };
		satisfyTxt = flavorTxt;
		chatter = new string[4] { "Contains \nVitamin \nA", "Part Of \nA \nComplete \nBreakfast", "Farmed \nLocally, \nVery \nLocally", "Fresh \nMorning \nTaste" };
		actNames = new string[3] { "Talk", "S!Devour", "Dinner" };
		defaultChatSize = "RightSmall";
		exp = 4;
		gold = 4;
		attacks = new int[2] { 17, 18 };
		sprites = new Sprite[4]
		{
			Resources.Load<Sprite>("battle/enemies/Vegetoid/spr_b_vegetoid_0"),
			Resources.Load<Sprite>("battle/enemies/Vegetoid/spr_b_vegetoid_1"),
			Resources.Load<Sprite>("battle/enemies/Vegetoid/spr_b_vegetoid_2"),
			Resources.Load<Sprite>("battle/enemies/Vegetoid/spr_b_vegetoid_1")
		};
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit)
		{
			frames++;
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[frames / 10 % 4];
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0)
		{
			eatingGreens = false;
		}
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i - 1;
		if (GetActNames()[i] == "Talk")
		{
			return new string[2] { "* You talked to Vegetoid.^15\n* ...", "* Nothing happened." };
		}
		if (GetActNames()[i] == "S!Devour")
		{
			if (CanSpare())
			{
				Spare();
				Object.FindObjectOfType<GameManager>().Heal(0, 5);
				Object.FindObjectOfType<GameManager>().Heal(1, 5);
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_swallow");
				Object.FindObjectOfType<GameManager>().PlayTimedHealSound();
				return new string[1] { "* You and Susie took a\n  bite out of Vegetoid.\n* You each recovered 5 HP!" };
			}
			return new string[1] { "* You and Susie tried to\n  eat Vegetoid,^10 but it wasn't\n  weakened enough." };
		}
		if (GetActNames()[i] == "Dinner")
		{
			eatingGreens = true;
			return new string[1] { "* You pat your stomach.\n* Vegetoid offers a healthy\n  meal." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (spared)
		{
			return base.PerformAssistAct(i);
		}
		if (i == 1)
		{
			if (satisfied < 100)
			{
				AddActPoints(20);
				return new string[1] { "* Susie weakened Vegetoid\n  slightly." };
			}
			return new string[1] { "* Vegetoid is already\n  sufficiently weakened.\n* It is ripe for consumption." };
		}
		return base.PerformAssistAct(i);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > -1 && lastAct != 1)
		{
			string[] array = new string[3] { "Plants \nCan't \nTalk \nDummy", "Fart", "Eat \nYour \nGreens" };
			if (greened)
			{
				array[2] = "Ate \nYour \nGreens";
			}
			text = new string[1] { array[lastAct] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override int GetNextAttack()
	{
		if (Object.FindObjectsOfType<Vegetoid>().Length == 2 && !Object.FindObjectsOfType<Vegetoid>()[0].IsDone() && !Object.FindObjectsOfType<Vegetoid>()[1].IsDone())
		{
			return 20;
		}
		if ((bool)Object.FindObjectOfType<Loox>() && !Object.FindObjectOfType<Loox>().IsDone())
		{
			return 21;
		}
		return base.GetNextAttack();
	}

	public override bool[] GetTargets()
	{
		if (eatingGreens)
		{
			return new bool[3] { true, false, false };
		}
		return base.GetTargets();
	}

	public bool ExpectingGreensEaten()
	{
		return eatingGreens;
	}

	public void DisableEatingGreens()
	{
		eatingGreens = false;
	}

	public void EatGreens()
	{
		if (!greened)
		{
			greened = true;
			if (satisfied < 100)
			{
				AddActPoints(100);
			}
		}
	}
}
