using UnityEngine;

public class Dogamy : EnemyBase
{
	public enum DogiDialogue
	{
		Neutral1 = 0,
		Neutral2 = 1,
		Neutral3 = 2,
		Neutral4 = 3,
		PetImmediateM = 4,
		PetImmediateF = 5,
		Resniff = 6,
		PetM = 7,
		PetF = 8
	}

	private Sprite[] sprites;

	private int bodyFrames;

	private int response;

	private bool useDogaressaVariant;

	private DogiDialogue dialogueType;

	private bool rolled;

	private bool resniffed;

	private bool pet;

	private bool sparedOnce;

	protected override void Awake()
	{
		base.Awake();
		base.Awake();
		enemyName = "Dogamy";
		fileName = "dogamy";
		checkDesc = "* Husband of Dogaressa.\n* Knows only what he smells.";
		maxHp = 240;
		hp = maxHp;
		atk = 14;
		def = 6;
		displayedDef = 4;
		canSpareViaFight = false;
		chatter = new string[1] { "error\nno\nbrute\nforce" };
		flavorTxt = new string[4] { "* The Dogs keep shifting their\n  axes to protect each other.", "* The Dogs are practicing for\n  the next couples contest...^05\n  whenever that is.", "* The Dogs are saying sickly\n  sweet things to each other.", "* The Dogs are still hesitant\n  to fight you." };
		dyingTxt = flavorTxt;
		satisfyTxt = flavorTxt;
		actNames = new string[3] { "Pet", "Re-sniff", "Roll Around" };
		hurtSound = "sounds/snd_ehurt1";
		defaultChatSize = "LeftSmall";
		exp = 45;
		gold = 20;
		attacks = new int[1] { 85 };
		int[] array = new int[9] { 0, 1, 2, 3, 4, 5, 5, 6, 0 };
		sprites = new Sprite[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			sprites[i] = Resources.Load<Sprite>("battle/Enemies/Dogamy/spr_b_dogamy_" + array[i]);
		}
		hpPos = new Vector2(0f, 187f);
		hpWidth = 202;
		defaultChatPos = new Vector2(-161f, 98f);
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		bodyFrames = (bodyFrames + 1) % (20 + sprites.Length * 7);
		if (GetBodyFrames() > 20)
		{
			int num = (GetBodyFrames() - 20) / 7;
			if (num >= sprites.Length)
			{
				num = sprites.Length - 1;
			}
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[num];
		}
		else if (GetBodyFrames() == 0)
		{
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
		GetPart("axe").GetComponent<SpriteRenderer>().enabled = !Object.FindObjectOfType<Dogaressa>().IsShaking() && !Object.FindObjectOfType<Dogaressa>().IsDone() && !killed;
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Pet")
		{
			if (!Object.FindObjectOfType<Dogaressa>().IsKilled())
			{
				if (resniffed)
				{
					response = 3;
					if (satisfied < 100)
					{
						if (Object.FindObjectOfType<Dogaressa>().GetSatisfactionLevel() >= 100)
						{
							satisfied = 75;
						}
						AddActPoints(25);
						if (satisfied < 100)
						{
							satisfied = 100;
						}
					}
					return new string[1] { "* You pet Dogamy." };
				}
				response = 1;
				return new string[1] { "* The Dogs are too suspicious\n  of your smell." };
			}
			return new string[1] { "* Dogamy just growls at\n  you." };
		}
		if (GetActNames()[i] == "Re-sniff")
		{
			if (!Object.FindObjectOfType<Dogaressa>().IsKilled())
			{
				if (!resniffed)
				{
					if (rolled)
					{
						AddActPoints(50);
						Object.FindObjectOfType<Dogaressa>().AddActPoints(50);
						response = 2;
						resniffed = true;
					}
					return new string[2]
					{
						"* The Dogs sniff you...",
						rolled ? "* After rolling in the dirt,^05\n  you smell all right!" : "* You smell weird!\n* They think you might\n  actually be a human!"
					};
				}
				return new string[1] { "* The Dogs already know you\n  smell fine." };
			}
			return new string[1] { "* Dogamy won't even lift\n  up his snout." };
		}
		if (GetActNames()[i] == "Roll Around")
		{
			rolled = true;
			return new string[2] { "* You roll around in the\n  dirt and snow.", "* You kind of smell like\n  a weird puppy." };
		}
		return base.PerformAct(i);
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		int num = ((!Object.FindObjectOfType<Dogaressa>().IsKilled()) ? 1 : 2);
		return base.CalculateDamage(partyMember, rawDmg, forceMagic) * num;
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (Object.FindObjectOfType<Dogaressa>().IsDone())
		{
			text = ((response != 4) ? new string[1] { (Random.Range(0, 2) == 0) ? "Whine." : "Whimper." } : new string[1] { "W...^10\nwhat...?" });
		}
		else
		{
			if (response == 0)
			{
				dialogueType = (DogiDialogue)Random.Range(0, 4);
			}
			else if (response == 1)
			{
				dialogueType = (useDogaressaVariant ? DogiDialogue.PetImmediateF : DogiDialogue.PetImmediateM);
			}
			else if (response == 2)
			{
				dialogueType = DogiDialogue.Resniff;
			}
			else if (response == 3)
			{
				dialogueType = (useDogaressaVariant ? DogiDialogue.PetF : DogiDialogue.PetM);
			}
			text[0] = (new string[9] { "Take my \nwife... \n's fleas.", "Don't \ntouch my \nhot dog.", "No. 2 \nNuzzle \nChamps \n'98!!", "Let's \nkick \nhuman \ntail!!", "Paws off \nyou \nsmelly \nhuman.", "Stop! \nDon't \ntouch \nher!", "What! \nSmells \nlike a \n...", "Wow!!! \nPet by \nanother \npup!!!", "What \nabout \nme......\n........" })[(int)dialogueType];
		}
		response = 0;
		useDogaressaVariant = false;
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public void SetResponseFromDogaressa(int response = -1)
	{
		useDogaressaVariant = true;
		if (response > -1)
		{
			this.response = response;
		}
	}

	public override string GetRandomFlavorText()
	{
		if (sparedOnce)
		{
			return "* Dogamy is flabbergasted.";
		}
		if (Object.FindObjectOfType<Dogaressa>().IsKilled())
		{
			return "* Dogamy is brokenhearted.";
		}
		if (CanSpare() && Object.FindObjectOfType<Dogaressa>().CanSpare())
		{
			return "* The Dogs' minds have been\n  expanded.";
		}
		if (resniffed)
		{
			return "* The Dogs' think that you\n  may be a lost puppy and\n  are confused.";
		}
		if (rolled)
		{
			return "* The Dogs may want to\n  smell you.";
		}
		return base.GetRandomFlavorText();
	}

	public int GetBodyFrames()
	{
		if (Object.FindObjectOfType<Dogamy>().IsShaking() || Object.FindObjectOfType<Dogamy>().IsDone() || Object.FindObjectOfType<Dogaressa>().IsShaking() || Object.FindObjectOfType<Dogaressa>().IsDone())
		{
			return 0;
		}
		return bodyFrames;
	}

	public bool CanPet()
	{
		return resniffed;
	}

	public override bool CanSpare()
	{
		if (satisfied < 100 || Object.FindObjectOfType<Dogaressa>().GetSatisfactionLevel() < 100)
		{
			return Object.FindObjectOfType<Dogaressa>().IsKilled();
		}
		return true;
	}

	public DogiDialogue GetDialogue()
	{
		return dialogueType;
	}

	public override void Spare(bool sleepMist = false)
	{
		if (Object.FindObjectOfType<Dogaressa>().IsKilled())
		{
			if (!sparedOnce)
			{
				response = 4;
				sparedOnce = true;
			}
			else
			{
				base.Spare(sleepMist);
			}
		}
		else
		{
			base.Spare(sleepMist);
		}
	}
}
