using System;
using UnityEngine;

public class Whimsalot : EnemyBase
{
	private int lastAct = -1;

	private bool pray;

	private bool susieAct;

	private int attackOffset;

	private Sprite[] headSprites;

	private Sprite[] bodySprites;

	private Sprite[] wingSprites;

	private int bodyFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Whimsalot";
		fileName = "whimsalot";
		checkDesc = "* It finally stopped worrying.";
		maxHp = 90;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 5;
		def = 2;
		flavorTxt = new string[5] { "* Whimsalot locks eyes with you.", "* Whimsalot flutters silently.", "* Whimsalot shakes its head\n  dismissively.", "* Whimsalot spins its weapon\n  around.", "* Smells like nightshade and\n  bleach." };
		dyingTxt = new string[1] { "* Whimsalot's flying stutters." };
		satisfyTxt = new string[1] { "* Whimsalot doesn't want to\n  fight anymore." };
		chatter = new string[4] { "No \nregrets.", "I've \nmade my \nchoice.", "Not this \ntime.", "*Shine \nshine*" };
		actNames = new string[3] { "Console", "Terrorize", "Pray" };
		defaultChatSize = "RightSmall";
		exp = 2;
		gold = 3;
		canSpareViaFight = false;
		headSprites = new Sprite[4]
		{
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_head_0"),
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_head_1"),
			null,
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_head_2")
		};
		headSprites[2] = headSprites[0];
		bodySprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_body_0"),
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_body_1")
		};
		wingSprites = new Sprite[7]
		{
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_wing_0"),
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_wing_1"),
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_wing_2"),
			Resources.Load<Sprite>("battle/enemies/Whimsalot/spr_b_whimsalot_wing_3"),
			null,
			null,
			null
		};
		wingSprites[4] = wingSprites[3];
		wingSprites[5] = wingSprites[2];
		wingSprites[6] = wingSprites[1];
		attacks = new int[1] { 47 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 104, 66f);
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit)
		{
			bodyFrames++;
			float t = (0f - Mathf.Cos((float)(bodyFrames * 12) * ((float)Math.PI / 180f)) + 1f) / 2f;
			float t2 = (0f - Mathf.Cos((float)(bodyFrames * 24) * ((float)Math.PI / 180f)) + 1f) / 2f;
			GetPart("body").transform.localPosition = new Vector3(0.186f, Mathf.Lerp(0.439f, 0.646f, t));
			GetPart("head").transform.localPosition = new Vector3(0f, Mathf.Lerp(1.398f, 1.691f, t));
			GetPart("wings").transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, -0.104f, t));
			GetPart("wings").transform.Find("right").eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(13.915f, -42.498f, t2));
			GetPart("wings").transform.Find("left").eulerAngles = new Vector3(0f, 0f, 0f - Mathf.Lerp(13.915f, -42.498f, t2));
			GetPart("body").GetComponent<SpriteRenderer>().sprite = bodySprites[(bodyFrames + 25) / 15 % 2];
			GetPart("head").GetComponent<SpriteRenderer>().sprite = headSprites[bodyFrames / 10 % 4];
			GetPart("wings").transform.Find("right").GetComponent<SpriteRenderer>().sprite = wingSprites[bodyFrames / 15 % 7];
			GetPart("wings").transform.Find("left").GetComponent<SpriteRenderer>().sprite = wingSprites[bodyFrames / 15 % 7];
		}
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i;
		if (GetActNames()[i] == "Check")
		{
			return new string[1] { "* WHIMSALOT - " + (34 + attackOffset * 2) + " ATK 12 DEF\n" + checkDesc };
		}
		if (GetActNames()[i] == "Console")
		{
			return new string[1] { "* You reassure Whimsalot that\n  what it's doing is\n  alright." };
		}
		if (GetActNames()[i] == "Terrorize")
		{
			return new string[1] { "* You leap at Whimsalot in a\n  threatening manner." };
		}
		if (GetActNames()[i] == "Pray")
		{
			pray = true;
			if (satisfied < 100)
			{
				AddActPoints(50);
			}
			return new string[1] { "* You kneel and pray for safety.\n* Whimsalot remembers its\n  conscience." };
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
			susieAct = true;
			attackOffset += 2;
			return new string[2] { "* Susie made a horrifying\n  face at Whimsalot.", "* Whimsalot was not deterred.\n* Its ATTACK went up." };
		}
		return base.PerformAssistAct(i);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > 0)
		{
			string[] array = new string[3]
			{
				"I've \nmade \nmy \npeace.",
				"I'm not \nafraid \nof you.",
				(new string[2] { "Don't \ngive \nup!", "There's \nstill \nhope." })[UnityEngine.Random.Range(0, 2)]
			};
			text = new string[1] { array[lastAct - 1] };
			lastAct = -1;
		}
		else if (susieAct)
		{
			text = new string[1] { "I've put \nup with \nyou long \nenough." };
		}
		susieAct = false;
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public int GetAttackOffset()
	{
		return attackOffset;
	}

	public bool IsPraying()
	{
		if (pray)
		{
			pray = false;
			return true;
		}
		return false;
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
