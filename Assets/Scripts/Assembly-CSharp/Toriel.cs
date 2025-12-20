using UnityEngine;

public class Toriel : EnemyBase
{
	private int talk = -1;

	private bool fightBegin;

	private bool jumpedInFront;

	private bool jumpedOnce;

	private int curProgress;

	private int processedProgress;

	private bool prevProgressWasViolence;

	private int spareTally;

	private bool friskDownedOnce;

	private bool susieDownedOnce;

	private bool finalBlow;

	private string[][] diag = new string[5][]
	{
		new string[2] { "My child,^05 what on \nearth are you doing?", "Why are you guarding \nher...?" },
		new string[2] { "What are you trying \nto accomplish?", "I am trying to \nprotect you." },
		new string[3] { "Do you understand me?", "That monster is \ndangerous!", "Get away from her!" },
		new string[1] { "Stop looking at me \nlike that." },
		new string[1] { "I just... ^10don't get \nit..." }
	};

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Toriel";
		fileName = "toriel";
		checkDesc = "* Knows best for you.";
		maxHp = 440;
		hp = maxHp;
		hpPos = new Vector2(0f, 200f);
		atk = 5;
		def = 0;
		flavorTxt = new string[4] { "* Toriel prepares a magical\n  attack.", "* Toriel looks through you.", "* Toriel is acting aloof.", "* Toriel takes a deep breath." };
		chatter = new string[1] { "" };
		actNames = new string[2] { "Talk", "S!Defend Susie" };
		canSpareViaFight = false;
		hpWidth = 150;
		attacks = new int[5] { 41, 42, 43, 44, 45 };
		exp = 0;
		gold = 0;
		defaultChatPos = new Vector2(178f, 141f);
		defaultChatSize = "RightWide";
	}

	protected override void Update()
	{
		base.Update();
		if (Object.FindObjectOfType<GameManager>().GetCombinedHP() <= 0)
		{
			SetFace("gasp");
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (CalculateDamage(partyMember, rawDmg) >= hp)
		{
			rawDmg = 0f;
		}
		if (partyMember == 0 && rawDmg > 0f)
		{
			if (curProgress >= 5)
			{
				finalBlow = true;
			}
			if (processedProgress == curProgress)
			{
				curProgress++;
				prevProgressWasViolence = true;
			}
		}
		base.Hit(partyMember, rawDmg, playSound);
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Check")
		{
			return new string[1] { "* TORIEL - ATK 80 DEF 80\n" + checkDesc };
		}
		if (GetActNames()[i] == "Talk")
		{
			if (curProgress < 5)
			{
				talk++;
				if (talk != 0)
				{
					if (talk != 1)
					{
						return new string[1] { "* Ironically,^05 talking does not\n  seem to be the solution\n  to this situation." };
					}
					return new string[1] { "* You tried to think\n  of something to say\n  again,^05 but..." };
				}
				return new string[1] { "* You couldn't think of\n  any conversation\n  topics." };
			}
			return new string[1] { "* You felt that Susie should\n  be the one to explain." };
		}
		if (GetActNames()[i] == "S!Defend Susie")
		{
			jumpedInFront = true;
			if (processedProgress == curProgress)
			{
				curProgress++;
				prevProgressWasViolence = false;
			}
			if (!jumpedOnce)
			{
				jumpedOnce = true;
				return new string[2] { "* You jumped in front of Susie.", "* Only you will take damage\n  this turn." };
			}
			return new string[1] { "* You continue defending Susie." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (curProgress >= 5)
		{
			finalBlow = true;
			prevProgressWasViolence = false;
			Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
			return new string[6] { "su_dejected`snd_txtsus`* Look,^05 if I actually\n  wanted to kill this\n  kid...", "su_neutral`snd_txtsus`* I would've done it\n  already.", "su_dejected`snd_txtsus`* But I didn't,^05 because\n  they never did anything\n  wrong.", "su_dejected`snd_txtsus`* I know nothing about\n  what's going on here,^05\n  to be honest.", "su_side`snd_txtsus`* If you could just\n  let me talk...", "su_smirk`snd_txtsus`* Then I can tell you\n  what the hell my\n  deal is." };
		}
		return new string[1] { "* Susie tried to reason with\n  Toriel,^05 but she was\n  ignored." };
	}

	public void SetFace(string spriteName)
	{
		GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Toriel/spr_b_toriel_" + spriteName);
	}

	public override string GetRandomFlavorText()
	{
		jumpedInFront = false;
		if (!fightBegin)
		{
			fightBegin = true;
			return "* Toriel blocks the way!";
		}
		if (spareTally >= 3 && curProgress == 0)
		{
			return "* Sparing doesn't seem to\n  be the solution to\n  this situation.";
		}
		if (curProgress >= 5)
		{
			return "* Seems like Toriel might\n  listen to Susie now.";
		}
		return base.GetRandomFlavorText();
	}

	public override bool[] GetTargets()
	{
		if (!jumpedInFront)
		{
			return new bool[3] { false, true, false };
		}
		return new bool[3] { true, false, false };
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (text[0] != "")
		{
			base.Chat(text, type, sound, pos, canSkip, speed);
		}
		else if (processedProgress < curProgress && processedProgress < diag.Length && processedProgress < 5)
		{
			text = diag[processedProgress];
			if (processedProgress == 0 && prevProgressWasViolence)
			{
				text[1] = "Why are you hitting \nme?";
			}
			base.Chat(text, type, "snd_txttor", pos, canSkip, speed);
			if (processedProgress == 3)
			{
				SetFace("annoyed");
			}
			else if (processedProgress == 4)
			{
				SetFace("sad_side");
			}
			processedProgress++;
		}
		else if (finalBlow)
		{
			text = ((!prevProgressWasViolence) ? new string[7] { "... You're right.", "I am just so...^05\nafraid.", "I am afraid of \nASGORE's plans to \nsucceed.", "I have been afraid \nfor so long.", "Perhaps it is time \nfor me to finally \nopen up.", "I apologize for what \nI have dealt onto \nyou.", "I will be happy \nto hear your story." } : new string[8] { "No,^05 I understand \nnow.", "I am being \nunreasonable to harm \nsomeone that did not \ndo anything.", "I am just so...^05\nafraid.", "I am afraid of \nASGORE's plans to \nsucceed.", "I have been afraid \nfor so long.", "Perhaps it is time \nfor me to finally \nopen up.", "I apologize for what \nI have dealt onto \nyou.", "I will be happy \nto hear your story." });
			Object.FindObjectOfType<BattleManager>().StopMusic();
			base.Chat(text, type, "snd_txttor", pos, canSkip, speed);
			SetFace("surrender");
		}
	}

	public override int GetNextAttack()
	{
		if (finalBlow)
		{
			return 46;
		}
		return base.GetNextAttack();
	}

	public override void AttemptedSpare()
	{
		base.AttemptedSpare();
		spareTally++;
	}

	public bool FriskHasDowned()
	{
		if (!friskDownedOnce)
		{
			friskDownedOnce = true;
			return false;
		}
		return true;
	}

	public bool SusieHasDowned()
	{
		if (!susieDownedOnce)
		{
			susieDownedOnce = true;
			return false;
		}
		return true;
	}

	public override void TurnToDust()
	{
		Util.GameManager().LoadBunnyCheck();
	}

	public override bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		if (partyMember != 0)
		{
			return false;
		}
		return true;
	}
}
