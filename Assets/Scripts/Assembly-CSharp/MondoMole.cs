using System;
using UnityEngine;

public class MondoMole : EnemyBase
{
	private bool geno;

	private bool playingDefeatAnim;

	private int defeatFrames;

	private int animFrames;

	private Vector3 basePos = Vector3.zero;

	private bool toBulletBoard;

	private bool beesArePissed;

	private bool lectured;

	private int whackAMoleSpots = 2;

	private int extremeAddition;

	private int psiDisruptLevel;

	private int lastAct = -1;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Mondo Mole";
		fileName = "mondo";
		checkDesc = "* The guardian of Lilliput Steps.";
		maxHp = 1000;
		hp = maxHp;
		hpPos = new Vector2(2f, 69f);
		hpWidth = 150;
		atk = 35;
		def = 5;
		displayedDef = 25;
		flavorTxt = new string[4] { "* Mondo Mole appears to be\n  salivating.", "* The smell is indescribable.", "* Mondo Mole attentively watches\n  your movements.", "* Mondo Mole towers over you." };
		dyingTxt = new string[1] { "* Mondo Mole has low HP." };
		actNames = new string[3] { "Play Rough", "N!Lecture", "S!Disrupt" };
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 3;
		gold = 30;
		geno = (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) >= 6;
		attacks = new int[4] { 54, 55, 56, 56 };
	}

	protected override void Start()
	{
		base.Start();
		basePos = obj.transform.position;
	}

	protected override void Update()
	{
		if (hp > 0 || !geno)
		{
			base.Update();
			if (!gotHit && (bool)obj)
			{
				animFrames++;
				float num = Mathf.Sin((float)(animFrames * 8) * ((float)Math.PI / 180f));
				float num2 = Mathf.Sin((float)(animFrames * 4) * ((float)Math.PI / 180f));
				GetPart("body").localScale = new Vector3(1f + num * 0.02f, 1f - num * 0.02f);
				GetPart("left").localPosition = new Vector3(-0.937f, 3f) + new Vector3(num * -0.05f, 0f - Mathf.Abs(num2 * 0.05f));
				GetPart("right").localPosition = new Vector3(0.937f, 2.583f) + new Vector3(num * 0.05f, 0f - Mathf.Abs(num2 * 0.05f));
				obj.transform.position = Vector3.Lerp(obj.transform.position, toBulletBoard ? new Vector3(0f, -4.05f) : basePos, 0.5f);
			}
		}
		else
		{
			if (!gotHit)
			{
				return;
			}
			frames++;
			if (frames % 3 == 0)
			{
				if (moveBody < 0)
				{
					moveBody *= -1;
				}
				else if (moveBody > 0)
				{
					moveBody -= 2;
					moveBody *= -1;
				}
				else
				{
					gotHit = false;
				}
			}
			obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
		}
	}

	public bool IsLectured()
	{
		return lectured;
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i - 1;
		if (GetActNames()[i] == "Play Rough")
		{
			int points = ((Util.GameManager().GetMiniPartyMember() == 1) ? 10 : 5);
			extremeAddition += 2;
			AddActPoints(points);
			if (satisfied >= 100)
			{
				Spare();
			}
			return new string[2] { "* You threw caution to the\n  wind and encouraged the\n  mole to play with you.", "* Its attacks became extreme\n  this next turn!" };
		}
		if (GetActNames()[i] == "N!Lecture")
		{
			if (!lectured)
			{
				def -= 5;
				lectured = true;
				AddActPoints(20);
				if (satisfied >= 100)
				{
					Spare();
				}
				return new string[2] { "* You and Noelle yelled at\n  Mondo Mole to keep its\n  distance.", "* Its ATTACK and DEFENSE dropped!" };
			}
			return new string[2] { "* You and Noelle yelled at\n  Mondo Mole to keep its\n  distance.", "* It didn't work..." };
		}
		if (GetActNames()[i] == "S!Disrupt")
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_weirdeffect");
			string text = "* Seems like it won't do\n  anymore good.";
			if (psiDisruptLevel < 3)
			{
				psiDisruptLevel++;
				text = "* Its PSI power became weaker.";
			}
			return new string[2] { "* You and Susie disrupted\n  Mondo Mole's senses.", text };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
		{
			int points = 5;
			string text = "* Its attacks became more\n  difficult this turn!";
			extremeAddition++;
			if (extremeAddition > 0)
			{
				text = "* Its attacks became even\n  more extreme!";
			}
			AddActPoints(points);
			if (satisfied >= 100)
			{
				Spare();
			}
			return new string[1] { "* Susie roared at Mondo Mole.\n" + text };
		}
		case 2:
			return new string[1] { "no_confused`snd_txtnoe`* Okay,^05 there is\n  NO WAY I'm going\n  up to that thing." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void TurnToDust()
	{
		if (geno)
		{
			playingDefeatAnim = true;
		}
		CombineParts();
	}

	public override void Spare(bool sleepMist = false)
	{
		CombineParts();
		spared = true;
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0 && geno)
		{
			exp = 250;
			killed = false;
			UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
			UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
			UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
			UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Mondo Mole/spr_b_mondo_die_0");
		}
	}

	public override int GetNextAttack()
	{
		if (hp <= 0 && geno)
		{
			return 77;
		}
		if (hp < maxHp)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_enemy_psi");
			if ((bool)UnityEngine.Object.FindObjectOfType<EnemyHPBar>())
			{
				UnityEngine.Object.DestroyImmediate(UnityEngine.Object.FindObjectOfType<EnemyHPBar>().gameObject);
			}
			Hit(0, -(75 - psiDisruptLevel * 18), false);
		}
		return base.GetNextAttack();
	}

	public void SetToBulletBoard(bool toBulletBoard)
	{
		this.toBulletBoard = toBulletBoard;
		GetPart("left").GetComponent<SpriteRenderer>().enabled = !toBulletBoard;
		GetPart("right").GetComponent<SpriteRenderer>().enabled = !toBulletBoard;
	}

	public override string GetRandomFlavorText()
	{
		extremeAddition = 0;
		return base.GetRandomFlavorText();
	}

	public int GetMaxWhackAMoleSpots()
	{
		if (whackAMoleSpots < 10 + GetDifficultyLevel())
		{
			whackAMoleSpots++;
			if (GetDifficultyLevel() >= 3)
			{
				whackAMoleSpots++;
			}
		}
		return whackAMoleSpots;
	}

	public int GetDifficultyLevel()
	{
		int num = maxHp / 6;
		int num2 = (maxHp - hp) / num;
		if (extremeAddition > 0)
		{
			num2 += extremeAddition;
			if (num2 > 6)
			{
				num2 = 6;
			}
		}
		return num2;
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (!text[0].Contains("error"))
		{
			if (geno)
			{
				killed = true;
			}
			base.Chat(text, type, sound, pos, canSkip, speed);
			chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
		}
	}
}
