using UnityEngine;

public class LesserDog : EnemyBase
{
	private bool weakerAttack;

	private bool noShield;

	private int tiredCount;

	private int progress;

	private bool oblit;

	private bool soloComfort;

	private int bodyFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Lesser Dog";
		fileName = "ldog";
		checkDesc = "* This soft, loving dog has\n  seen better days.";
		maxHp = 250;
		hp = maxHp;
		atk = 12;
		def = 0;
		displayedDef = 2;
		canSpareViaFight = false;
		chatter = new string[1] { "error\nno\nbrute\nforce" };
		flavorTxt = new string[3] { "* Lesser Dog is hiding behind\n  its shield.", "* Smells like dog chow.", "* Lesser Dog is afraid of your\n  weapons." };
		dyingTxt = flavorTxt;
		satisfyTxt = new string[1] { "* Lesser Dog finally feels safe." };
		actNames = new string[5] { "Pet", "Beckon", "SN!Play", "S!Ignore", "N!Comfort" };
		hurtSound = "sounds/snd_pombark";
		hurtSpriteName = "_shield";
		if (!Util.GameManager().NoelleInParty())
		{
			oblit = true;
			actNames = new string[5] { "Pet", "Beckon", "S!Play", "S!Ignore", "Comfort" };
		}
		exp = 40;
		gold = 20;
		attacks = new int[1] { 87 };
		playerMultiplier = Mathf.Lerp(0.4f, 0.35f, (float)Util.GameManager().GetLV() / 10f);
		hpPos = new Vector2(0f, 122f);
		hpWidth = 202;
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		if (!noShield && hp <= maxHp / 4 && !killed)
		{
			noShield = true;
			tired = true;
			PlaySFX("sounds/snd_dust");
			GetPart("shield").GetComponent<ParticleDuplicator>().Activate(includeBlack: true, Vector2.zero);
			GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Lesser Dog/spr_b_ldog_noshield");
			hurtSpriteName = "_dmg";
			playerMultiplier = 1f;
			Util.FindObjectOfType<BattleManager>().GetComponent<AudioSource>().pitch = 0.4f;
		}
		int maxExclusive = 15;
		if (noShield)
		{
			maxExclusive = 6;
		}
		else if (progress == 1)
		{
			maxExclusive = 50;
		}
		else if (progress > 1 && progress != 4)
		{
			maxExclusive = 100000;
		}
		GetPart("body").transform.localPosition = new Vector3((float)((Random.Range(0, maxExclusive) == 0) ? ((Random.Range(0, 2) != 0) ? 1 : (-1)) : 0) / 48f, 0f);
		if (progress > 4 && !noShield)
		{
			bodyFrames++;
			int num = bodyFrames / ((progress == 5) ? 15 : 10) % 2;
			if (!GetPart("body").GetComponent<SpriteRenderer>().sprite.name.EndsWith(num.ToString()))
			{
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Lesser Dog/spr_b_ldog_happy_" + num);
			}
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Pet")
		{
			if (progress >= 5)
			{
				progress++;
				if (gold < 80)
				{
					gold += 20;
				}
				return new string[1] { "* You pet the Lesser Dog.^05\n* It seems to really like it." };
			}
			return new string[1] { "* You try to pet the\n  Lesser Dog,^05 but it quickly\n  backs away from you." };
		}
		if (GetActNames()[i] == "Beckon")
		{
			if (!noShield && progress >= 2)
			{
				if (progress == 2)
				{
					GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Lesser Dog/spr_b_ldog_shield_down");
					progress++;
					AddActPoints(15);
					playerMultiplier = 0.5f;
					Util.FindObjectOfType<BattleManager>().GetComponent<AudioSource>().pitch = 0.45f;
					return new string[1] { "* You called for the Lesser Dog.\n* It lowers its shield,^05 but isn't\n  fully out yet." };
				}
				if (progress == 3)
				{
					GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Lesser Dog/spr_b_ldog_base");
					progress++;
					AddActPoints(25);
					playerMultiplier = 0.75f;
					Util.FindObjectOfType<BattleManager>().GetComponent<AudioSource>().pitch = 0.5f;
					return new string[1] { "* You called for the Lesser Dog.\n* It puts its shield to its\n  side,^05 ready for you." };
				}
				return new string[1] { "* You called for the Lesser Dog.\n* But nothing happened." };
			}
			return new string[1] { "* You called for the Lesser Dog.\n* But it doesn't budge." };
		}
		if (GetActNames()[i].EndsWith("Play"))
		{
			if (noShield)
			{
				if (!oblit)
				{
					return new string[2] { "su_disappointed`snd_txtsus`* Kris, you're stupid.", "no_thinking`snd_txtnoe`* I think I can make\n  it fall asleep with\n  Sleep Mist." };
				}
				return new string[1] { "su_depressed`snd_txtsus`* Kris, just do it." };
			}
			if (progress >= 4)
			{
				if (progress == 4)
				{
					progress++;
					playerMultiplier = 666f;
					AddActPoints(35);
					Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_battle", 1f);
					return new string[3] { "* You proposed a game of catch\n  with snowballs.", "* Everyone played with the\n  Lesser Dog.", "* Lesser Dog feels safe in your\n  presence." };
				}
				return new string[1] { "* The Lesser Dog is tuckered out." };
			}
			return new string[1] { "* You try to get the Lesser\n  Dog to play with you,^05 but\n  it doesn't trust you yet." };
		}
		if (GetActNames()[i].EndsWith("Ignore"))
		{
			if (noShield)
			{
				if (!oblit)
				{
					return new string[2] { "su_annoyed`snd_txtsus`* Kris, we're NOT gonna\n  be able to calm\n  it down like this.", "su_side`snd_txtsus`* Maybe you can make\n  Noelle make it fall\n  asleep." };
				}
				return new string[1] { "su_disappointed`snd_txtsus`* ..." };
			}
			if (progress == 0)
			{
				progress++;
				AddActPoints(10);
				return new string[2] { "* You stepped away from Lesser\n  Dog and dragged Susie with you.", "* Lesser Dog seems to be less\n  intimidated." };
			}
			return new string[2] { "* You stepped away from Lesser\n  Dog and dragged Susie with you.", "* But nothing happened." };
		}
		if (GetActNames()[i].EndsWith("Comfort"))
		{
			if (noShield)
			{
				if (!oblit)
				{
					return new string[2] { "no_thinking`snd_txtnoe`* Kris,^05 I don't think\n  we can comfort it\n  like this.", "no_thinking`snd_txtnoe`* I think I can make\n  it fall asleep with\n  Sleep Mist." };
				}
				return new string[2] { "* You try to comfort the\n  Lesser Dog.", "* But Lesser Dog is waiting\n  for you to put it out\n  of its misery." };
			}
			if (progress == 1)
			{
				if (oblit)
				{
					if (!soloComfort)
					{
						soloComfort = true;
						AddActPoints(5);
						return new string[2] { "* You try to comfort the\n  Lesser Dog.", "* Lesser Dog seems to be\n  shaking less." };
					}
					progress++;
					AddActPoints(10);
					return new string[2] { "* You try to comfort the\n  Lesser Dog.", "* Lesser Dog stops shaking." };
				}
				progress++;
				AddActPoints(15);
				return new string[3] { "* You tell Noelle to comfort\n  the Lesser Dog.", "no_happy`snd_txtnoe`* There,^05 there...^05\n* We aren't going to\n  hurt you.", "* Lesser Dog stops shaking." };
			}
			if (progress == 0)
			{
				if (!oblit)
				{
					return new string[2] { "* You try to comfort the\n  Lesser Dog,^05 but it keeps\n  shaking.", "no_thinking`snd_txtnoe`* (I wonder if he's\n  too intimidated by\n  Susie...)" };
				}
				return new string[1] { "* You try to comfort the\n  Lesser Dog,^05 but it keeps\n  shaking." };
			}
			return new string[1] { "* You try to comfort the\n  Lesser Dog,^05 but nothing\n  happens." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		switch (i)
		{
		case 1:
			weakerAttack = true;
			if (progress == 1 && !soloComfort)
			{
				progress = 0;
				AddActPoints(-10);
				return new string[3]
				{
					"* Susie stands tall over\n  Lesser Dog.",
					"* Lesser Dog feels intimidated\n  again!!!",
					oblit ? "su_inquisitive`snd_txtsus`* (The hell did I\n  even do???)" : "no_weird`snd_txtnoe`* (Maybe...^05 wait to do\n  that later,^05 Susie.)"
				};
			}
			return new string[2] { "* Susie stands tall over\n  Lesser Dog.", "* Lesser Dog's next attack became\n  weaker!" };
		case 2:
			PlaySFX("sounds/snd_hypnosis");
			if (tired)
			{
				return new string[1] { "* Noelle hums a soothing tune.\n* But Lesser Dog is already\n  tired!" };
			}
			tiredCount++;
			if (tiredCount == 1)
			{
				return new string[1] { "* Noelle hums a soothing tune.\n* Lesser Dog finds this to be\n  oddly soothing." };
			}
			if (tiredCount == 2)
			{
				return new string[1] { "* Noelle hums a soothing tune.\n* Lesser Dog seems to becoming\n  sleepier." };
			}
			if (tiredCount == 3)
			{
				tired = true;
				return new string[1] { "* Noelle hums a soothing tune.\n* Lesser Dog has become TIRED!" };
			}
			break;
		}
		return base.PerformAssistAct_Old(i);
	}

	public override void EnemyTurnEnd()
	{
		base.EnemyTurnEnd();
		weakerAttack = false;
	}

	public override void Spare(bool sleepMist = false)
	{
		if (sleepMist)
		{
			Util.GameManager().SetFlag(258, 1);
		}
		if (oblit)
		{
			Util.GameManager().SetFlag(244, 1);
		}
		base.Spare(sleepMist);
	}

	public override string GetRandomFlavorText()
	{
		if (noShield)
		{
			return (new string[2] { "* Lesser Dog is about to faint.", "* Lesser Dog thinks its about\n  to die." })[Random.Range(0, 2)];
		}
		if (progress == 4)
		{
			return "* Lesser Dog is now vulnerable.";
		}
		return base.GetRandomFlavorText();
	}

	public override void Chat()
	{
	}

	public override int GetNextAttack()
	{
		if (satisfied >= 100)
		{
			return -1;
		}
		return base.GetNextAttack();
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (killed && progress >= 5)
		{
			if (progress > 5)
			{
				gold = 0;
			}
			Util.GameManager().SetFlag(261, 1);
		}
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (CanSpare() && base.CalculateDamage(partyMember, rawDmg, forceMagic) < hp)
		{
			return hp;
		}
		return base.CalculateDamage(partyMember, rawDmg, forceMagic);
	}

	public override bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		if (partyMember == 0)
		{
			return true;
		}
		if (progress > 4)
		{
			return false;
		}
		if (!Util.GameManager().NoelleInParty())
		{
			return false;
		}
		return true;
	}
}
