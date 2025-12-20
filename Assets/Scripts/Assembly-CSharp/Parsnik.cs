using System;
using UnityEngine;

public class Parsnik : EnemyBase
{
	private bool greened;

	private bool eatingGreens;

	private int lastAct = -1;

	private Sprite[] sprites;

	private Sprite[][] snakes;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Parsnik";
		fileName = "parsnik";
		checkDesc = "* This cobrafied carrot has\n  a headful of tasty snakes.";
		maxHp = 150;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 6;
		def = 8;
		flavorTxt = new string[5] { "* Parsnik has a hissy fit.", "* Parsnik's snakes shift to\n  change hairstyles.^05\n* Mohawk.^05 Ponytail.^05 Undercut.", "* Parsnik completely closes its\n  mouth.^05\n* It looks short and weird.", "* Snakes play with a beach ball.", "* Smells like tasty snakes." };
		dyingTxt = new string[1] { "* The snakes are wilting." };
		satisfyTxt = new string[1] { "* Parsnik seems satisfied." };
		chatter = new string[3] { "Hisssss", "Herssss", "Theirsss" };
		actNames = new string[3] { "Hiss", "S!Devour", "Snack" };
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 4;
		gold = 4;
		attacks = new int[1] { 48 };
		sprites = new Sprite[4]
		{
			Resources.Load<Sprite>("battle/enemies/Parsnik/spr_b_parsnik_head_0"),
			Resources.Load<Sprite>("battle/enemies/Parsnik/spr_b_parsnik_head_1"),
			Resources.Load<Sprite>("battle/enemies/Parsnik/spr_b_parsnik_head_2"),
			Resources.Load<Sprite>("battle/enemies/Parsnik/spr_b_parsnik_head_3")
		};
		snakes = new Sprite[5][];
		for (int i = 0; i < 5; i++)
		{
			snakes[i] = new Sprite[4];
			for (int j = 0; j < 4; j++)
			{
				snakes[i][j] = Resources.Load<Sprite>("battle/enemies/Parsnik/spr_b_parsnik_snake" + (i + 1) + "_" + j);
			}
		}
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		frames++;
		GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[frames / 12 % 4];
		GetPart("hair").transform.localPosition = new Vector3(-0.025f, Mathf.Lerp(1.56f, 1.626f, (Mathf.Cos((float)(frames * 12) * ((float)Math.PI / 180f)) + 1f) / 2f));
		for (int i = 0; i < 5; i++)
		{
			Transform transform = GetPart("hair").transform.Find("snake" + i);
			int num = frames + 2 * i;
			int num2 = ((i == 1 || i == 2) ? 5 : 6);
			if (i != 2)
			{
				bool flag = i > 0 && i < 4;
				float num3 = Mathf.Sin((float)(num * (flag ? 12 : 9)) * ((float)Math.PI / 180f));
				float z = (flag ? 2.6f : 6f) * num3;
				transform.localEulerAngles = new Vector3(0f, 0f, z);
			}
			transform.GetComponent<SpriteRenderer>().sprite = snakes[i][num / num2 % 4];
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
		if (GetActNames()[i] == "Check")
		{
			return new string[1] { "* PARSNIK - 30 ATK 28 DEF\n" + checkDesc };
		}
		if (GetActNames()[i] == "Hiss")
		{
			return new string[2] { "* You hissed at Parsnik.^15\n* ...", "* Nothing happened." };
		}
		if (GetActNames()[i] == "S!Devour")
		{
			if (satisfied >= 100 || IsTired())
			{
				Spare();
				UnityEngine.Object.FindObjectOfType<GameManager>().Heal(0, 5);
				UnityEngine.Object.FindObjectOfType<GameManager>().Heal(1, 5);
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_swallow");
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayTimedHealSound();
				return new string[1] { "* You and Susie start eating\n  snakes like spaghetti.\n* You each recovered 5 HP!" };
			}
			return new string[1] { "* You and Susie tried to\n  eat Parsnik,^10 but it wasn't\n  weakened enough." };
		}
		if (GetActNames()[i] == "Snack")
		{
			eatingGreens = true;
			return new string[1] { "* Parsnik mishears you and fires\n  a series of tasty snakes." };
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
				return new string[1] { "* Susie weakened Parsnik\n  slightly." };
			}
			return new string[1] { "* Parsnik is already\n  sufficiently weakened.\n* The snakes quiver in fear." };
		}
		return base.PerformAssistAct(i);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > -1 && lastAct != 1)
		{
			string[] array = new string[3] { "Don't \nBe Rude", "SHIT", "Eat Your \nGreen \nTasty \nSnakes" };
			if (greened)
			{
				array[2] = "Ate \nYour \nGreen \nSnakes";
			}
			text = new string[1] { array[lastAct] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
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

	public override bool IsTired()
	{
		if (!tired)
		{
			return (float)hp / (float)maxHp <= 0.2f;
		}
		return true;
	}
}
