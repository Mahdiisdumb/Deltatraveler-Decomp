using UnityEngine;

public class GreaterDog : EnemyBase
{
	private bool distracted;

	private bool communicator = true;

	private bool collar = true;

	private int collarSnatchHP = 2;

	private bool response;

	private int flavorText = -1;

	private int beckonTimes;

	private int doFunnyAct;

	private int hits;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Greater Dog";
		fileName = "gdog";
		checkDesc = "* The one trained to kill.";
		maxHp = 420;
		hp = maxHp;
		atk = 18;
		def = 10;
		displayedDef = 12;
		canSpareViaFight = false;
		chatter = new string[3] { "I shall \nslay you \nhuman \nscum.", "Give up \nand \ndie!", "I have \nyou \ncornered" };
		defaultChatSize = "RightSmall";
		defaultChatPos = new Vector2(191f, 60f);
		flavorTxt = new string[4] { "* GREATERDOG stands completely\n  still.", "* GREATERDOG is following\n  commands.", "* Smells like spoiled puppy\n  juice.", "* GREATERDOG glares into you." };
		dyingTxt = new string[1] { "* GREATERDOG remains steadfast." };
		satisfyTxt = new string[1] { "* Greater Dog is very tired." };
		actNames = new string[5] { "Pet", "N!Beckon", "S!Snatch", "SN!Distract", "Sneak" };
		hurtSpriteName = "_0";
		hostile = true;
		exp = 65;
		gold = 80;
		attacks = new int[1] { 107 };
		hpPos = new Vector2(0f, 122f);
		hpWidth = 202;
	}

	public override string[] PerformAct(int i)
	{
		if (!collar && !communicator)
		{
			distracted = true;
		}
		if (GetActNames()[i] == "SN!Debug")
		{
			Spare();
			return new string[1] { "* Deez nuts" };
		}
		if (GetActNames()[i].EndsWith("Pet"))
		{
			if (beckonTimes == 0)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
				Util.GameManager().Damage(0, 4);
				return new string[1] { "* You tried to pet\n  GREATERDOG,^05 but it stabs\n  you!" };
			}
			if (satisfied >= 100)
			{
				return new string[2] { "* You pet the dog.", "* You can see it trying\n  to fight off the sleep\n  falling upon it." };
			}
			return new string[1] { "* You tried to pet\n  GREATERDOG,^05 but it backs\n  away from you." };
		}
		if (GetActNames()[i].EndsWith("Beckon"))
		{
			if (collar || communicator)
			{
				return new string[1] { "* You and Noelle called\n  for the GREATERDOG,^05 but\n  it didn't budge." };
			}
			if (beckonTimes == 0)
			{
				flavorText = 3;
				beckonTimes++;
				AddActPoints(20);
				ChangeSprites("_3");
				return new string[3] { "* You and Noelle called\n  for the GREATERDOG.", "no_happy`snd_txtnoe`* C'mere,^05 little guy!", "* GREATERDOG perks up its ears." };
			}
			if (beckonTimes == 1)
			{
				flavorText = 4;
				beckonTimes++;
				AddActPoints(40);
				return new string[2] { "no_weird`snd_txtnoe`* C'mon...!\n* I know you're not\n  as bad as you look!", "* GREATERDOG seems to be\n  resisting the urge to\n  hop into your lap." };
			}
			if (beckonTimes == 2)
			{
				beckonTimes++;
				ChangeSprites("_4");
				Unhostile();
				AddActPoints(100);
				tired = true;
				attacks[0] = -1;
				playerMultiplier = 999f;
				displayedDef = -21;
				Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_dogsong", 0.8f);
				checkDesc = "* This dog is ready for\n  a well needed nap.";
				return new string[3] { "no_happy`snd_txtnoe`* C'mon,^05 who's a good\n  boy?", "* Without its harnesses holding\n  it back,^05 Greater Dog leaps\n  into your lap...", "* ... Completely disregarding\n  that it was trained to kill." };
			}
			if (beckonTimes == 3)
			{
				return new string[1] { "* Greater Dog is already\n  laying in your lap." };
			}
		}
		else
		{
			if (GetActNames()[i].EndsWith("Snatch"))
			{
				if (!collar && !communicator)
				{
					return new string[1] { "* But there was nothing\n  left to take." };
				}
				string arg = (communicator ? "communicator" : "collar");
				if (distracted)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/acts/GreaterDogSnatch"));
					return new string[1] { string.Format("* Aim to grab GREATERDOG's\n  {0}!\n^05* (Press ^Z to grab!)^10 ", arg) };
				}
				Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
				Util.GameManager().Damage(0, 3);
				Util.GameManager().Damage(1, 3);
				return new string[2]
				{
					string.Format("* You and Susie tried\n  snatching GREATERDOG's\n  {0}...", arg),
					"* But failed!^05\n* GREATERDOG stabs both of\n  you!"
				};
			}
			if (GetActNames()[i].EndsWith("Distract"))
			{
				if (satisfied < 100)
				{
					if (!collar && !communicator)
					{
						return new string[1] { "* Seems GREATERDOG is already\n  distracted by its lack\n  of collar and communicator." };
					}
					distracted = true;
					return new string[2] { "* Everyone frantically told\n  GREATERDOG to look behind\n  itself.", "* GREATERDOG seems to be\n  a bit distracted." };
				}
				return new string[1] { "* Seems Greater Dog is already\n  distracted by its sleepiness." };
			}
			if (GetActNames()[i].EndsWith("Sneak"))
			{
				if (satisfied < 100)
				{
					if (distracted)
					{
						if (collar)
						{
							flavorText = (communicator ? 1 : 2);
							doFunnyAct += (communicator ? 5 : 10);
							Util.GameManager().PlayGlobalSFX("sounds/snd_grab");
							distracted = false;
							collar = false;
							if (communicator)
							{
								ChangeSprites("_swapped");
							}
							else
							{
								ChangeSprites("_2");
							}
							return new string[2]
							{
								"* You snuck behind GREATERDOG\n  and removed its spiked\n  collar!",
								communicator ? "* But you couldn't reach\n  its communicator from there." : "* GREATERDOG is now vulnerable!"
							};
						}
						return new string[1] { communicator ? "* You snuck behind GREATERDOG,^05\n  but you couldn't reach\n  its communicator from there." : "* You snuck behind GREATERDOG,^05\n  but you couldn't really\n  do anything." };
					}
					Util.GameManager().PlayGlobalSFX("sounds/snd_hurt");
					Util.GameManager().Damage(0, 4);
					return new string[1] { "* You tried to sneak behind\n  GREATERDOG,^05 but it spotted\n  you and stabbed you!" };
				}
				return new string[2] { "* You can't sneak behind\n  what is laying in\n  your lap.", "su_smirk_sweat`snd_txtsus`* (It can just...^05 leave\n  its armor???)" };
			}
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			if (satisfied < 100)
			{
				return new string[1] { "* Susie barks at GREATERDOG!\n* Nothing happened." };
			}
			return new string[1] { "* Susie barks at Greater Dog!\n* Nothing happened." };
		case 2:
			if (doFunnyAct > 0)
			{
				response = true;
				AddActPoints(doFunnyAct);
				doFunnyAct = 0;
				return new string[2] { "* Noelle spoke softly to\n  GREATERDOG.", "* Seems like the dog is\n  starting to lower its\n  guard." };
			}
			return new string[2] { "* Noelle spoke softly to\n  GREATERDOG.", "* Nothing happened." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override string GetRandomFlavorText()
	{
		if (flavorText > -1)
		{
			int num = flavorText;
			flavorText = -1;
			return (new string[5] { "* GREATERDOG is growling because\n  it can no longer speak.", "* GREATERDOG doesn't feel as\n  threatening without its collar.", "* GREATERDOG is no longer bound\n  to its threatening appearance.", "* GREATERDOG's facade is falling\n  apart.", "* GREATERDOG is one \"good boy\"\n  away from becoming good\n  again." })[num];
		}
		return base.GetRandomFlavorText();
	}

	private void ChangeSprites(string spriteName)
	{
		Sprite sprite = Resources.Load<Sprite>("battle/enemies/Greater Dog/spr_b_gdog" + spriteName);
		GetPart("body").GetComponent<SpriteRenderer>().sprite = sprite;
		hurtSpriteName = spriteName;
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (rawDmg > 0f && hp > 0)
		{
			hits++;
		}
		base.Hit(partyMember, rawDmg, playSound);
	}

	public void SnatchCommunicator()
	{
		distracted = false;
		if (collar)
		{
			flavorText = 0;
			doFunnyAct += 5;
			ChangeSprites("_1");
		}
		else
		{
			doFunnyAct += 10;
			ChangeSprites("_2");
		}
		communicator = false;
	}

	public void SnatchCollar()
	{
		distracted = false;
		doFunnyAct += (communicator ? 5 : 10);
		flavorText = (communicator ? 1 : 2);
		collarSnatchHP--;
		if (collarSnatchHP == 0)
		{
			ChangeSprites("_2");
			collar = false;
		}
	}

	public bool HasCommunicator()
	{
		return communicator;
	}

	public bool HasCollar()
	{
		return collar;
	}

	public override void EnemyTurnStart()
	{
		hits++;
	}

	public override string GetName()
	{
		if (satisfied < 100)
		{
			return "GREATERDOG";
		}
		return base.GetName();
	}

	public override void TurnToDust()
	{
		base.TurnToDust();
		Util.GameManager().SetFlag(279, hits);
		Util.GameManager().SetFlag(280, (satisfied >= 100) ? 1 : 0);
	}

	public override bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		if (partyMember != 0 && satisfied >= 100)
		{
			return false;
		}
		return base.PartyMemberAcceptAttack(partyMember, attackType);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (communicator)
		{
			sound = "snd_txtmtt";
			if (response)
			{
				response = false;
				text = new string[2] { "ERROR\nEMOTION\nNOT\nFOUND", "Do NOT \nplay \nwith \nme!" };
			}
			base.Chat(text, type, sound, pos, canSkip, speed);
		}
	}

	public bool IsDistracted()
	{
		return distracted;
	}
}
