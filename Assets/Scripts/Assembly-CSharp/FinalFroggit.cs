using System;
using UnityEngine;

public class FinalFroggit : EnemyBase
{
	private int bodyFrames;

	private int lastAct = -1;

	private int attackOffset;

	private int attackType;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Final Froggit";
		fileName = "ffroggit";
		checkDesc = "* Its future looks brighter and\n  brighter.";
		maxHp = 100;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 7;
		def = 6;
		flavorTxt = new string[4] { "* Final Froggit knows exactly\n  why it's here.", "* Final Froggit jumps ominously\n  up and down.", "* The battlefield is filled\n  with the smell of\n  mustard seed.", "* You are intimidated by\n  Froggit's raw strength." };
		dyingTxt = new string[1] { "* Final Froggit stands its\n  ground." };
		satisfyTxt = new string[1] { "* Final Froggit seems reluctant\n  to fight you." };
		chatter = new string[4] { "Robbit, \nrobbit.", "Creak, \ncreak.", "Skip, \njump.", "Woof." };
		actNames = new string[3] { "Compliment", "Threat", "S!Mystify" };
		hurtSound = "sounds/snd_ehurt1";
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 4;
		gold = 4;
		attacks = new int[1] { 47 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0)
		{
			gold = 2;
		}
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i;
		if (GetActNames()[i] == "Check")
		{
			return new string[1] { "* FINAL FROGGIT - " + (30 + attackOffset) + " ATK " + (18 + def) + " DEF\n" + checkDesc };
		}
		if (GetActNames()[i] == "Compliment")
		{
			attackOffset--;
			return new string[1] { "* You compliment Final Froggit.\n* It understood you perfectly.\n* Its ATTACK dropped." };
		}
		if (GetActNames()[i] == "Threat")
		{
			def -= 2;
			return new string[1] { "* You threaten Final Froggit.\n* It understood you perfectly.\n* Its DEFENSE dropped." };
		}
		if (GetActNames()[i] == "S!Mystify")
		{
			if (satisfied < 100)
			{
				AddActPoints(100);
				return new string[2] { "* You and Susie did something\n  mysterious.", "* Final Froggit recognizes it\n  has more to learn from\n  this world." };
			}
			return new string[1] { "* You and Susie did something\n  mysterious.\n* But nothing happened." };
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
			return new string[2] { "* Susie punched her fist\n  into her hand.", "* Final Froggit recognizes this\n  threat.\n* It became TIRED." };
		}
		return base.PerformAssistAct(i);
	}

	public override string GetChatter()
	{
		attackType = UnityEngine.Random.Range(0, 2);
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			if (attackType != 0)
			{
				return chatter[0];
			}
			return chatter[2];
		}
		if (UnityEngine.Random.Range(0, 2) != 0)
		{
			return chatter[3];
		}
		return chatter[1];
	}

	public int GetAttackType()
	{
		return attackType;
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > 0)
		{
			string[] array = new string[3] { "Nod, \nnod.", "Shudder, \nshudder.", "(Thought\n-ful \ncroak)" };
			text = new string[1] { array[lastAct - 1] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public int GetAttackOffset()
	{
		return attackOffset;
	}

	protected override void Update()
	{
		if (!gotHit)
		{
			bodyFrames++;
			if (bodyFrames >= 38)
			{
				bodyFrames = 0;
			}
			float num = 0f - Mathf.Sin((float)bodyFrames * 18.947369f * ((float)Math.PI / 180f));
			float num2 = 0f - Mathf.Sin((float)bodyFrames * 9.473684f * ((float)Math.PI / 180f));
			GetPart("body").transform.localScale = new Vector3(1f, 1f + num * 0.0875f, 1f);
			float x = 0.138f + num2 * 0.148f;
			float y = 1.792f + 0.102f * num;
			GetPart("head").transform.localPosition = new Vector3(x, y);
			GetPart("head").transform.eulerAngles = new Vector3(0f, 0f, -3f * num2);
		}
		base.Update();
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
