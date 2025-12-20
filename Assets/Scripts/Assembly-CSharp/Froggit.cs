using UnityEngine;

public class Froggit : EnemyBase
{
	private int headFrames;

	private int nostrilFrames;

	private int bodyFrames;

	private int lastAct = -1;

	private Sprite[] headSprites;

	private Sprite[] bodySprites;

	private float attackType;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Froggit";
		fileName = "froggit";
		checkDesc = "^10* Life is difficult for\n  this enemy.";
		maxHp = 50;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 4;
		def = 6;
		flavorTxt = new string[4] { "* Froggit doesn't seem to\n  know why it's here.", "* Froggit hops to and fro.", "* The battlefield is filled\n  with the smell of\n  mustard seed.", "* You are intimidated by\n  Froggit's raw strength.^30\n* Only kidding." };
		dyingTxt = new string[1] { "* Froggit is trying to\n  run away." };
		satisfyTxt = new string[1] { "* Froggit seems reluctant\n  to fight you." };
		chatter = new string[4] { "Ribbit, \nribbit.", "Croak, \ncroak.", "Hop, \nhop.", "Meow." };
		actNames = new string[2] { "Compliment", "Threat" };
		hurtSound = "sounds/snd_ehurt1";
		defaultChatSize = "RightSmall";
		exp = 4;
		gold = 2;
		attacks = new int[2] { 4, 5 };
		headSprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Froggit/spr_b_froggit_head_0"),
			Resources.Load<Sprite>("battle/enemies/Froggit/spr_b_froggit_head_1")
		};
		bodySprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Froggit/spr_b_froggit_body_0"),
			Resources.Load<Sprite>("battle/enemies/Froggit/spr_b_froggit_body_1")
		};
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i;
		if (GetActNames()[i] == "Compliment")
		{
			if (satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[1] { "* Froggit didn't understand\n  what you said,^10 but was\n  flattered anyway." };
		}
		if (GetActNames()[i] == "Threat")
		{
			if (satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[2] { "* Froggit didn't understand\n  what you said,^10 but was\n  scared anyway.", "su_wideeye`snd_txtsus`* Kris???" };
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
			tired = true;
			return new string[2] { "* Susie punched her fist\n  into her hand.", "* Froggit couldn't handle\n  it and became TIRED." };
		}
		return base.PerformAssistAct(i);
	}

	public override string GetChatter()
	{
		attackType = Random.Range(0, 2);
		if (Random.Range(0, 2) == 0)
		{
			if (attackType != 0f)
			{
				return chatter[0];
			}
			return chatter[2];
		}
		if (Random.Range(0, 2) != 0)
		{
			return chatter[3];
		}
		return chatter[1];
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > 0)
		{
			string[] array = new string[2] { "(Blushes \ndeeply.)\nRibbit.", "Shiver, \nshiver." };
			text = new string[1] { array[lastAct - 1] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override int GetNextAttack()
	{
		if (Object.FindObjectsOfType<Froggit>().Length == 2 && !Object.FindObjectsOfType<Froggit>()[0].IsDone() && !Object.FindObjectsOfType<Froggit>()[1].IsDone())
		{
			return 6;
		}
		if ((bool)Object.FindObjectOfType<Whimsun>() && !Object.FindObjectOfType<Whimsun>().IsDone())
		{
			return 9;
		}
		if (attackType != 0f)
		{
			return 4;
		}
		return 5;
	}

	protected override void Update()
	{
		if (!gotHit)
		{
			headFrames++;
			nostrilFrames++;
			bodyFrames++;
			if (headFrames >= 65)
			{
				headFrames = 0;
			}
			if (headFrames < 11)
			{
				GetPart("head").transform.localPosition = Vector3.Lerp(new Vector3(0f, 1.69f), new Vector3(-0.09f, 1.65f), (float)headFrames / 11f);
			}
			else if (headFrames < 37)
			{
				GetPart("head").transform.localPosition = Vector3.Lerp(new Vector3(-0.09f, 1.65f), new Vector3(0.2f, 1.58f), (float)(headFrames - 11) / 26f);
			}
			else if (headFrames < 49)
			{
				GetPart("head").transform.localPosition = Vector3.Lerp(new Vector3(0.2f, 1.58f), new Vector3(0.08f, 1.56f), (float)(headFrames - 37) / 12f);
			}
			else if (headFrames < 65)
			{
				GetPart("head").transform.localPosition = Vector3.Lerp(new Vector3(0.08f, 1.56f), new Vector3(0f, 1.69f), (float)(headFrames - 49) / 16f);
			}
			GetPart("head").transform.GetComponent<SpriteRenderer>().sprite = headSprites[nostrilFrames / 48 % 2];
			GetPart("body").transform.GetComponent<SpriteRenderer>().sprite = bodySprites[bodyFrames / 24 % 2];
		}
		base.Update();
	}
}
