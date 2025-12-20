using UnityEngine;

public class Dummy : EnemyBase
{
	private bool talkedTo;

	private int timesSpared;

	private bool leaving;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Dummy";
		checkDesc = "^10* A cotton heart and a button eye^10\n* You are the apple of my eye";
		actNames = new string[2]
		{
			EnemyBase.CHECK_NAME,
			"Talk"
		};
		flavorTxt = new string[2] { "* Dummy looks like it's\n  going to fall over.", "* Dummy stands around\n  absentmindedly." };
		satisfyTxt = flavorTxt;
		dyingTxt = flavorTxt;
		chatter = new string[1] { ".^10.^10.^10.^10." };
		fileName = "dummy";
		maxHp = 15;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 0;
		def = 0;
		canSpareViaFight = false;
		susieMiniACTs[0].SetName("Talk");
		defaultChatSize = "RightSmall";
		defaultChatPos = new Vector2(50f, 51f);
		exp = 0;
		gold = 0;
		attacks = new int[1] { -1 };
	}

	protected override void Update()
	{
		base.Update();
		if (leaving)
		{
			Vector3 position = base.transform.position;
			base.transform.position = new Vector3(position.x, position.y + 0.1f, position.z);
		}
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		int num = base.CalculateDamage(partyMember, rawDmg, forceMagic);
		if (num < 15 && rawDmg > 0f)
		{
			return 15;
		}
		return num;
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
	}

	public override string[] PerformAct(int i)
	{
		if (i == 1)
		{
			if (!talkedTo)
			{
				talkedTo = true;
				AddActPoints(100);
				if ((int)Util.GameManager().GetFlag(108) == 1)
				{
					Spare();
					return new string[3] { "* You talk to the DUMMY.^10\n* ...", "* It doesn't seem much for\n  conversation.", "* TORIEL seems happy with you." };
				}
				return new string[3] { "* You talk to the DUMMY.^10\n* ...", "* It doesn't seem much for\n  conversation.", "* Susie looks confused." };
			}
			return new string[3] { "* You talk to the DUMMY.^10\n* ...", "* It doesn't seem much for\n  conversation.", "su_inquisitive`snd_txtsus`* Why are we talking\n  to it AGAIN???" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		if (i == 1)
		{
			if (!talkedTo)
			{
				talkedTo = true;
				AddActPoints(100);
				return new string[3] { "* Susie talked to the DUMMY.", "su_smile_sweat`snd_txtsus`* You uhh... look pretty\n  beat-up-able.", "* It doesn't seem much for\n  conversation." };
			}
			return new string[1] { "su_inquisitive`snd_txtsus`* ..." };
		}
		return base.PerformAssistAct_Old(i);
	}

	public override void Chat()
	{
		base.Chat();
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override string GetRandomFlavorText()
	{
		int num = Random.Range(0, flavorTxt.Length);
		return flavorTxt[num];
	}

	public override int GetNextAttack()
	{
		timesSpared++;
		if (timesSpared > 6)
		{
			spared = true;
			return 78;
		}
		return 79;
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

	public void SetLeaving()
	{
		leaving = true;
	}
}
