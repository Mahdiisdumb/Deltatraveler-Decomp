using UnityEngine;

public class TrainingDummy : EnemyBase
{
	private bool lastAttackHit;

	private float krisAttack = -1f;

	private float susieAttack = -1f;

	private float noelleAttack = -1f;

	private float miniAttack = -1f;

	private bool rudeBuster;

	private bool iceShock;

	private bool psi;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Dummy";
		fileName = "dummy";
		checkDesc = "^10* A cotton heart and a button eye^10\n* You are the apple of my eye";
		maxHp = 1000;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 201;
		atk = 0;
		def = 0;
		flavorTxt = new string[5] { "* Critical hits result in a\n  score of 20!\n* Aim for the target!", "* Defending Kris while holding\n  a mini party member will earn\n  more TP!", "* QUAD hits are based on an\n  average, but hitting more crits\n  will multiply it slightly...", "* BASH reticles will always stick\n  to the target for at least\n  one frame.", "* Using PSI attacks will always\n  allow Kris a chance to attack!" };
		chatter = new string[1] { ".^10.^10.^10.^10." };
		defaultChatSize = "RightSmall";
		defaultChatPos = new Vector2(91f, 51f);
		exp = 0;
		gold = 0;
		actNames = new string[5] { "WeaponMenu", "IncreaseLV", "MaxLV", "ResetLV", "SN!Exit" };
		attacks = new int[1] { -1 };
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		switch (partyMember)
		{
		case 0:
			krisAttack = rawDmg;
			lastAttackHit = true;
			break;
		case 1:
			susieAttack = rawDmg;
			lastAttackHit = true;
			if ((bool)Util.FindObjectOfType<RudeBusterEffect>())
			{
				rudeBuster = true;
			}
			break;
		case 2:
			noelleAttack = rawDmg;
			lastAttackHit = true;
			if ((bool)Util.FindObjectOfType<IceShock>())
			{
				iceShock = true;
			}
			break;
		default:
			miniAttack = rawDmg;
			lastAttackHit = true;
			if ((bool)Util.FindObjectOfType<SpecialAttackEffect>())
			{
				psi = true;
			}
			break;
		}
		base.Hit(partyMember, rawDmg, playSound);
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "WeaponMenu")
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/acts/TrainingMenu"), GameObject.Find("BattleCanvas").transform, worldPositionStays: false);
			return new string[1] { "* Use LEFT and RIGHT to change\n  weapons. Setting Susie/Noelle\n  to None will deactivate them." };
		}
		if (GetActNames()[i] == "IncreaseLV")
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_levelup");
			if (Util.GameManager().GetLV() < 14)
			{
				Util.GameManager().SetEXP(Util.GameManager().GetLVExp());
			}
			Util.GameManager().HealAll(99);
			return new string[1] { "* You became stronger." };
		}
		if (GetActNames()[i] == "MaxLV")
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_levelup");
			Util.GameManager().SetEXP(5000);
			Util.GameManager().HealAll(99);
			return new string[1] { "* You became stronger." };
		}
		if (GetActNames()[i] == "ResetLV")
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_awkward");
			Util.GameManager().SetEXP(0);
			Util.GameManager().SetHP(1, 30);
			Util.GameManager().SetHP(2, 20);
			return new string[1] { "* You became weaker." };
		}
		if (GetActNames()[i].EndsWith("Exit"))
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_vineboom");
			aud.Play();
			Util.GameManager().SetDefaultValues();
			Util.GameManager().ForceLoadArea(6);
			return new string[1] { "* FUCK YOU BALTIMORE" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		return i switch
		{
			1 => new string[1] { "* Susie prepped her weapon..." }, 
			2 => new string[1] { "* Noelle's hands became cold..." }, 
			_ => base.PerformAssistAct_Old(i), 
		};
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAttackHit)
		{
			string text2 = "";
			if (krisAttack > 0f)
			{
				text2 = text2 + "Kris Score: " + krisAttack.ToString("F3") + "\n";
			}
			if (susieAttack > 0f)
			{
				text2 = text2 + "Susie Score: " + (rudeBuster ? "Rude" : susieAttack.ToString("F3")) + "\n";
			}
			if (noelleAttack > 0f)
			{
				text2 = text2 + "Noelle Score: " + (iceShock ? "Magic" : noelleAttack.ToString("F3")) + "\n";
			}
			if (miniAttack > 0f)
			{
				text2 = text2 + "Mini Score: " + (psi ? "PSI" : miniAttack.ToString("F3")) + "\n";
			}
			if (text2 == "")
			{
				text2 = "Everyone missed";
			}
			text = new string[1] { text2 };
			type = "RightWide";
			pos.x += 66f;
		}
		base.Chat(text, type, sound, pos, canSkip, speed);
		if (!lastAttackHit)
		{
			chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
		}
	}

	public override string GetRandomFlavorText()
	{
		hp = 1000;
		lastAttackHit = false;
		krisAttack = -1f;
		susieAttack = -1f;
		noelleAttack = -1f;
		miniAttack = -1f;
		rudeBuster = false;
		iceShock = false;
		psi = false;
		int num = Random.Range(0, flavorTxt.Length);
		return flavorTxt[num];
	}

	public override void Spare(bool sleepMist = false)
	{
		if ((int)Util.GameManager().GetFlag(108) == 0)
		{
			base.Spare(sleepMist);
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_main");
		}
		else
		{
			spared = true;
		}
	}

	public override void EnemyTurnEnd()
	{
		string text = "";
		if (Util.GameManager().SusieInParty())
		{
			text += "S";
		}
		if (Util.GameManager().NoelleInParty())
		{
			text += "N";
		}
		text += ((text.Length > 0) ? "!Exit" : "Exit");
		actNames[5] = text;
	}
}
