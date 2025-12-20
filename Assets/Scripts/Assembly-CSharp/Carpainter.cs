using System;
using UnityEngine;

public class Carpainter : EnemyBase
{
	private bool geno;

	private bool lookingForAvoid;

	private bool startedFight;

	private bool playingDefeatAnim;

	private int animFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Mr.Carpainter";
		fileName = "carpainter";
		checkDesc = "* The highest priest of\n  Happy-Happyism.";
		maxHp = 1000;
		hp = maxHp;
		hpPos = new Vector2(2f, 145f);
		hpWidth = 150;
		atk = 30;
		def = 12;
		flavorTxt = new string[3] { "* Mr. Carpainter points at\n  his paintbrush menacingly.", "* Smells like paint.", "* Mr. Carpainter gazes into\n  you." };
		dyingTxt = new string[1] { "* Mr. Carpainter appears to be\n  losing motivation." };
		satisfyTxt = flavorTxt;
		actNames = new string[3] { "Talk", "SN!Convince", "S!Refuse" };
		sActionName = "Talk";
		nActionName = "Talk";
		defaultChatSize = "RightSmall";
		exp = 1;
		gold = 18;
		geno = (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) >= 5;
		canSpareViaFight = geno;
		attacks = new int[2] { 38, 39 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 109, 121f);
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit)
		{
			animFrames++;
			float t = (Mathf.Cos((float)(animFrames * 12) * ((float)Math.PI / 180f)) + 1f) / 2f;
			float t2 = (Mathf.Cos((float)((animFrames - 3) * 12) * ((float)Math.PI / 180f)) + 1f) / 2f;
			float t3 = (Mathf.Cos((float)((animFrames - 6) * 12) * ((float)Math.PI / 180f)) + 1f) / 2f;
			GetPart("torso").transform.localPosition = new Vector3(0.059f, Mathf.Lerp(0.789f, 0.647f, t));
			GetPart("torso").transform.Find("arms").localPosition = new Vector3(-0.061f, Mathf.Lerp(0.879f, 0.824f, t2));
			GetPart("torso").transform.Find("arms").localScale = new Vector3(Mathf.Lerp(1f, 1.05f, t3), 1f, 1f);
			GetPart("legs").transform.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.8625f, t), 1f);
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "S!Refuse")
		{
			lookingForAvoid = true;
			return new string[2] { "su_annoyed`snd_txtsus`* Keep your cult shit\n  away from us.", "* (Susie encouraged you to <color=#FFFF00FF>avoid\n  getting hit</color> this turn!)" };
		}
		if (GetActNames()[i] != "Check")
		{
			int points = 5;
			string text = "* You tried to convince\n  Mr. Carpainter away from\n  Happy-Happyism.";
			string text2 = "* He seems to be listening.";
			if (GetActNames()[i].StartsWith("SN!Convince"))
			{
				points = ((UnityEngine.Object.FindObjectOfType<GameManager>().GetMiniPartyMember() == 1) ? 20 : 15);
				text = "* Everyone tried to convince\n  Mr. Carpainter away from\n  Happy-Happyism.";
			}
			else if (UnityEngine.Object.FindObjectOfType<GameManager>().GetMiniPartyMember() == 1)
			{
				text = "* Paula reminded Mr. Carpainter\n  of his sins.";
				points = 10;
			}
			AddActPoints(points);
			if (satisfied >= 100)
			{
				Spare();
				text2 = "* He became fully convinced!";
			}
			return new string[2] { text, text2 };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		string text = "* You defied the laws of the\n  game and used K-ACTION.";
		string text2 = "* He seems to be listening.";
		if (i == 1)
		{
			text = "* Susie threatened to beat\n  Carpainter up for leading\n  a cult.";
		}
		if (i == 2)
		{
			text = "* Noelle begged Carpainter to\n  think of the negatives\n  Happy-Happyism is doing.";
		}
		AddActPoints(5);
		if (satisfied >= 100)
		{
			Spare();
			text2 = "* He became fully convinced!";
		}
		return new string[2] { text, text2 };
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0 && rawDmg > 0f)
		{
			if (geno)
			{
				killed = false;
				UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
				exp = 100;
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_0");
			}
			else
			{
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_defeat");
			}
		}
	}

	public override void Spare(bool sleepMist = false)
	{
		CombineParts();
		spared = true;
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_defeat");
	}

	public override void TurnToDust()
	{
		if (geno)
		{
			playingDefeatAnim = true;
		}
		CombineParts();
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (!text[0].Contains("error"))
		{
			if (playingDefeatAnim)
			{
				killed = true;
			}
			base.Chat(text, type, sound, pos, canSkip, speed);
			chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
		}
	}

	public override int GetNextAttack()
	{
		if (playingDefeatAnim)
		{
			return 52;
		}
		return base.GetNextAttack();
	}

	public bool LookingForAvoid()
	{
		if (lookingForAvoid)
		{
			lookingForAvoid = false;
			return true;
		}
		return false;
	}

	public override void EnemyTurnEnd()
	{
		lookingForAvoid = false;
	}

	public override string GetRandomFlavorText()
	{
		if (!startedFight)
		{
			startedFight = true;
			return "* The Franklin Badge reflected\n  Mr. Carpainter's lightning!";
		}
		return base.GetRandomFlavorText();
	}
}
