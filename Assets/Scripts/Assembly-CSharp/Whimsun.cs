using UnityEngine;

public class Whimsun : EnemyBase
{
	private bool terrorized;

	private int floatFrames;

	private int spriteFrames;

	private Sprite[] sprites;

	private bool up = true;

	private int lastAct = -1;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Whimsun";
		fileName = "whimsun";
		checkDesc = "^10* This monster is too\n  sensitive to fight...";
		maxHp = 10;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 5;
		satisfied = 100;
		def = 0;
		flavorTxt = new string[4] { "* Whimsun avoids eye contact.", "* Whimsun continues to mutter\n  apologies.", "* Whimsun is fluttering.", "* It's starting to smell like\n  lavender and mothballs." };
		dyingTxt = new string[1] { "* Whimsun is having trouble\n  flying." };
		satisfyTxt = flavorTxt;
		chatter = new string[4] { "I'm \nsorry...", "I have \nno \nchoice..", "Forgive \nme...", "*sniff \nsniff*" };
		actNames = new string[2] { "Console", "Terrorize" };
		defaultChatSize = "RightSmall";
		exp = 2;
		gold = 0;
		attacks = new int[2] { 7, 8 };
		sprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Whimsun/spr_b_whimsun_0"),
			Resources.Load<Sprite>("battle/enemies/Whimsun/spr_b_whimsun_1")
		};
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 104, 172f);
	}

	protected override void Update()
	{
		if (!gotHit)
		{
			if (up)
			{
				floatFrames++;
				if (floatFrames == 32)
				{
					up = false;
				}
			}
			else
			{
				floatFrames--;
				if (floatFrames == 0)
				{
					up = true;
				}
			}
			GetEnemyObject().transform.Find("parts").transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0.3f), (float)floatFrames / 32f);
			spriteFrames++;
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[spriteFrames / 10 % 2];
		}
		base.Update();
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0)
		{
			gold = 2;
		}
	}

	public override string GetRandomFlavorText()
	{
		if (terrorized)
		{
			return "* Whimsun is hyperventilating.\n* Susie looks worried.";
		}
		return base.GetRandomFlavorText();
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i;
		if (GetActNames()[i] == "Console")
		{
			Spare();
			return new string[1] { "* Halfway through your first\n  word,^10 Whimsun burst into\n  tears and runs away." };
		}
		if (GetActNames()[i] == "Terrorize")
		{
			gold = 2;
			if (!terrorized)
			{
				terrorized = true;
				return new string[1] { "* You raise your arms and\n  wiggle your fingers.^10\n* Whimsun freaks out!" };
			}
			return new string[2] { "* You raise your arms and\n  wiggle your fingers.^10\n* Whimsun freaks out!", "su_worried`snd_txtsus`* Kris,^10 are you sure\n  this little guy's an\n  enemy?" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (i == 1)
		{
			if (!spared)
			{
				Spare();
				return new string[1] { "* Susie barely takes a step\n  forward.\n* Whimsun ran away!" };
			}
			return new string[1] { "su_inquisitive`snd_txtsus`* Okay...?" };
		}
		return base.PerformAssistAct(i);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (terrorized)
		{
			text = new string[1] { "I \ncan't \nhandle \nthis..." };
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}
}
