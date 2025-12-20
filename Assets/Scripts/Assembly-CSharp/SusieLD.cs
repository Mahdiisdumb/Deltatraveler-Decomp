using UnityEngine;

public class SusieLD : EnemyBase
{
	private bool doAttack;

	private bool doneAttack;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Susie";
		fileName = "susie";
		checkDesc = "* Strong-headed \"mean girl\"\n  trying to help you.";
		maxHp = Util.GameManager().GetMaxHP(1);
		hp = maxHp;
		atk = Util.GameManager().GetATK(1);
		def = 0;
		displayedDef = Util.GameManager().GetDEF(1);
		canSpareViaFight = false;
		chatter = new string[1] { "Kris im morbing out" };
		flavorTxt = new string[1] { "* Susie is sparing you." };
		dyingTxt = flavorTxt;
		satisfyTxt = flavorTxt;
		hurtSound = "sounds/snd_sussurprise";
		exp = 0;
		gold = 0;
		attacks = new int[1] { 105 };
		satisfied = 100;
		hpPos = new Vector2(0f, 122f);
		hpWidth = 202;
	}

	protected override void Start()
	{
		base.Start();
		xDif = base.transform.position.x - obj.transform.localPosition.x;
		hpPos.x -= Mathf.Round(xDif * 48f);
		mainPos.x = 0f;
	}

	protected override void Update()
	{
		base.Update();
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (!doneAttack)
		{
			return 0;
		}
		int num = base.CalculateDamage(partyMember, rawDmg, forceMagic) * 2;
		if (num < hp)
		{
			return hp;
		}
		return num;
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (doneAttack)
		{
			base.Hit(partyMember, rawDmg, playSound);
			if (killed)
			{
				moveBody = -30;
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_dmg");
				attacks[0] = 106;
				Util.GameManager().SetFlag(256, 1);
			}
		}
	}

	public override void Chat()
	{
		if (doneAttack && attacks[0] != 106)
		{
			base.Chat();
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		sound = "snd_txtsus";
		pos = new Vector2(179f, 136f);
		base.Chat(text, type, sound, pos, canSkip, speed);
	}

	public override bool IsDone()
	{
		if (!Object.FindObjectOfType<LesserDog>().IsKilled() || Util.GameManager().NoelleInParty() || spared)
		{
			return true;
		}
		return false;
	}

	public override void EnemyTurnStart()
	{
		if (!IsDone())
		{
			doAttack = true;
			hp = Util.GameManager().GetHP(1);
			if (hp <= 0)
			{
				hp = 1;
			}
			Util.GameManager().SetPartyMembers(false, false);
			Object.FindObjectOfType<BattleManager>().UpdatePartyMembers();
			Object.FindObjectOfType<PartyPanels>().SetXPositions();
		}
		if (doneAttack)
		{
			tired = true;
			if (killed)
			{
				SeparateParts();
			}
			else
			{
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_annoyed");
			}
		}
	}

	public override void EnemyTurnEnd()
	{
		if (doAttack && !doneAttack)
		{
			chatter = new string[1] { "The hell are you \ndoing?^05\nLet's go." };
			attacks[0] = -1;
			doneAttack = true;
		}
	}

	public override void TurnToDust()
	{
		CombineParts();
	}

	public override bool CanSpare()
	{
		return doneAttack;
	}

	public override void Spare(bool sleepMist = false)
	{
		base.Spare(sleepMist);
		GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = GetPart("body").GetComponent<SpriteRenderer>().sprite;
	}
}
