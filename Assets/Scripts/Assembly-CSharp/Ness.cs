using System;
using UnityEngine;

public class Ness : EnemyBase
{
	private bool castingHeal;

	private bool protectPaula;

	private int stage;

	private int animFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Ness";
		fileName = "ness";
		checkDesc = "";
		maxHp = 1800;
		hp = maxHp;
		hpPos = new Vector2(2f, 140f);
		hpWidth = 200;
		atk = 20;
		def = 4;
		displayedDef = 15;
		flavorTxt = new string[4] { "* Ness stares intently at\n  your actions.", "* Paula stands by.", "* The atmosphere is tense.", "* ..." };
		satisfyTxt = flavorTxt;
		dyingTxt = new string[1] { "* Ness is starting to\n  worry." };
		actNames = new string[3] { "Talk", "Reflect", DUALHEAL_NAME };
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 250;
		gold = 20;
		attacks = new int[1] { 67 };
		defaultChatPos = new Vector2(182f, 126f);
		defaultChatSize = "RightWide";
	}

	protected override void Update()
	{
		if (hp > 0)
		{
			base.Update();
		}
		else if (gotHit)
		{
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
			}
			if ((stage == 4 && frames == 1) || (stage < 4 && frames == 20))
			{
				Util.FindObjectOfType<Paula>().ActivatePhase2();
				Util.FindObjectOfType<Paula>().CombineParts();
				Util.FindObjectOfType<Paula>().GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_gasp");
			}
			else if (stage < 4 && frames == 1)
			{
				Util.FindObjectOfType<Paula>().GetPart("body").GetChild(0)
					.GetComponent<SpriteRenderer>()
					.sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_head_openeye");
			}
			if (frames <= 60)
			{
				if (GameManager.GetOptions().lowGraphics.value == 0)
				{
					GameObject.Find("NessBGNameSoICanFindIt").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float)frames / 60f);
				}
				else
				{
					BattleBGPiece[] array = Util.FindObjectsOfType<BattleBGPiece>();
					foreach (BattleBGPiece obj in array)
					{
						Color color = obj.GetComponent<SpriteRenderer>().color;
						obj.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, 1f - (float)frames / 60f);
					}
				}
			}
			if (frames != 60 && frames == 90)
			{
				killed = true;
				gotHit = false;
			}
			base.obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
		}
		if (!gotHit)
		{
			animFrames++;
			GetPart("body").transform.localPosition = new Vector3(0.646f, 2.561f) + new Vector3(Mathf.Sin((float)(animFrames * 12) * (MathF.PI / 180f)) / 36f, Mathf.Sin((float)(animFrames * 24) * (MathF.PI / 180f)) / 48f);
		}
		if (protectPaula)
		{
			base.obj.transform.localPosition = Vector3.Lerp(base.obj.transform.localPosition, mainPos, 0.5f);
		}
		if ((bool)Util.FindObjectOfType<AttackBase>() && castingHeal && Util.FindObjectOfType<AttackBase>().HasStarted())
		{
			castingHeal = false;
			GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_head" + (ReduceDamage() ? "_regret" : GetHead()));
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "SN!Debug")
		{
			stage = 4;
			satisfied = 100;
			GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_head_regret");
			return new string[1] { "* Magical redemption" };
		}
		if (GetActNames()[i] == "Talk")
		{
			if (stage == 1)
			{
				satisfied += 25;
				stage++;
				return new string[2] { "* You told Ness how badly you\n  feel about what you've done.", "* He doesn't seem very\n  convinced." };
			}
			if (stage == 3)
			{
				satisfied += 25;
				stage++;
				GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_head_regret");
				Util.FindObjectOfType<Paula>().GetPart("body").GetChild(0)
					.GetComponent<SpriteRenderer>()
					.sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_head_listening");
				return new string[2] { "* You talked to Ness about\n  your control over the\n  situation,^05 and the sensation.", "* He starts to understand.\n* Ness's ATTACK dropped!" };
			}
			return new string[1] { "* But you didn't know what\n  to talk about." };
		}
		if (GetActNames()[i] == "Reflect")
		{
			if (stage == 0)
			{
				satisfied += 25;
				stage++;
				return new string[2] { "* You thought about what led\n  you here...", "* You were disgusted with your\n  actions." };
			}
			if (stage == 2)
			{
				satisfied += 25;
				stage++;
				return new string[2] { "* You thought about Ness and\n  his own part in this...", "* Perhaps he's an essence holder\n  like Paula." };
			}
			return new string[1] { "* You tried to think...\n* But nothing came to mind." };
		}
		return base.PerformAct(i);
	}

	public override int GetNextAttack()
	{
		if (CanSpare())
		{
			return -1;
		}
		return base.GetNextAttack();
	}

	public override string GetRandomFlavorText()
	{
		if (CanSpare())
		{
			return "* Ness and Paula are sparing\n  you.";
		}
		return base.GetRandomFlavorText();
	}

	public bool ReduceDamage()
	{
		return stage >= 4;
	}

	public override bool CanSpare()
	{
		return Util.FindObjectOfType<Paula>().CanSpare();
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		return new string[1] { "* But she couldn't think\n  of anything to do." };
	}

	public override void Chat()
	{
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		protectPaula = false;
		if (CanSpare() && rawDmg != 0f)
		{
			rawDmg = ((partyMember == 0) ? 3621 : 0);
		}
		else
		{
			GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_head" + (ReduceDamage() ? "_regret" : GetHead()));
		}
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0)
		{
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
			Util.FindObjectOfType<BattleManager>().ForceNoFight();
			Util.FindObjectOfType<BattleManager>().ForceNoSpare();
			Util.FindObjectOfType<BattleManager>().StopMusic();
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_kill_0");
			moveBody = 20;
		}
	}

	private string GetHead()
	{
		if (!((float)hp / (float)maxHp <= 0.2f))
		{
			return "";
		}
		return "_worry";
	}

	public void ProtectPaula()
	{
		protectPaula = true;
		mainPos.x = 5f;
		hpPos.x = Mathf.Abs(hpPos.x);
		GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_head");
	}

	public void CastHeal()
	{
		GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Ness/spr_b_ness_head_psi");
		castingHeal = true;
	}

	public override bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		if (partyMember == 0)
		{
			return true;
		}
		if (CanSpare())
		{
			return false;
		}
		switch (attackType)
		{
		case 0:
			if (hp - CalculateDamage(0, 20f) - CalculateDamage(1, 20f) - CalculateDamage(2, 20f) <= 0)
			{
				return false;
			}
			break;
		case 1:
			if (hp - CalculateDamage(partyMember, 40f, forceMagic: true) <= 0)
			{
				return false;
			}
			break;
		}
		return true;
	}
}
