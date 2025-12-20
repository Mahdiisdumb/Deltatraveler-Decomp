using UnityEngine;

public class Dogaressa : EnemyBase
{
	private Sprite[] sprites;

	private bool pet;

	private int colorShift;

	protected override void Awake()
	{
		base.Awake();
		base.Awake();
		enemyName = "Dogaressa";
		fileName = "dogaressa";
		checkDesc = "* This puppy finds her\n  hubby lovely. SMELLS ONLY?";
		maxHp = 240;
		hp = maxHp;
		atk = 14;
		def = 6;
		displayedDef = 4;
		canSpareViaFight = false;
		hpToUnhostile = 0;
		chatter = new string[1] { "error\nno\nbrute\nforce" };
		flavorTxt = new string[1] { "* The Dogaressa is on the\n  warpath." };
		dyingTxt = flavorTxt;
		satisfyTxt = flavorTxt;
		actNames = new string[3] { "Pet", "Re-sniff", "Roll Around" };
		hurtSound = "sounds/snd_ehurt1";
		defaultChatSize = "RightSmall";
		exp = 45;
		gold = 20;
		attacks = new int[1] { 85 };
		int[] array = new int[9] { 0, 1, 2, 3, 4, 5, 5, 6, 0 };
		sprites = new Sprite[array.Length];
		for (int i = 0; i < array.Length; i++)
		{
			sprites[i] = Resources.Load<Sprite>("battle/Enemies/Dogaressa/spr_b_dogaressa_" + array[i]);
		}
		hpPos = new Vector2(0f, 187f);
		hpWidth = 202;
		defaultChatPos = new Vector2(164f, 98f);
	}

	private void LateUpdate()
	{
		if (gotHit)
		{
			return;
		}
		int bodyFrames = Object.FindObjectOfType<Dogamy>().GetBodyFrames();
		if (bodyFrames > 20)
		{
			int num = (bodyFrames - 20) / 7;
			if (num >= sprites.Length)
			{
				num = sprites.Length - 1;
			}
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[num];
		}
		else if (bodyFrames == 0)
		{
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
		if (hostile && colorShift < 60)
		{
			colorShift++;
			GetPart("body").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color32(237, 28, 36, byte.MaxValue), (float)colorShift / 60f);
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Pet")
		{
			if (!Object.FindObjectOfType<Dogamy>().IsKilled())
			{
				if (Object.FindObjectOfType<Dogamy>().CanPet())
				{
					Object.FindObjectOfType<Dogamy>().SetResponseFromDogaressa(3);
					if (satisfied < 100)
					{
						if (Object.FindObjectOfType<Dogamy>().GetSatisfactionLevel() >= 100)
						{
							satisfied = 75;
						}
						AddActPoints(25);
						if (satisfied < 100)
						{
							satisfied = 100;
						}
					}
					return new string[1] { "* You pet the Dogaressa." };
				}
				Object.FindObjectOfType<Dogamy>().SetResponseFromDogaressa(1);
				return new string[1] { "* The Dogaressa is too\n  suspicious of your smell." };
			}
			return new string[1] { "* The Dogaressa just growls\n  at you." };
		}
		if (GetActNames()[i] == "Re-sniff")
		{
			if (Object.FindObjectOfType<Dogamy>().IsKilled())
			{
				return new string[1] { "* The Dogaressa won't even\n  lift up her snout." };
			}
			return Object.FindObjectOfType<Dogamy>().PerformAct(i);
		}
		if (GetActNames()[i] == "Roll Around")
		{
			return Object.FindObjectOfType<Dogamy>().PerformAct(i);
		}
		return base.PerformAct(i);
	}

	public override bool CanSpare()
	{
		if (satisfied >= 100 && Object.FindObjectOfType<Dogamy>().GetSatisfactionLevel() >= 100)
		{
			return !hostile;
		}
		return false;
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (!Object.FindObjectOfType<Dogamy>().IsDone())
		{
			int dialogue = (int)Object.FindObjectOfType<Dogamy>().GetDialogue();
			text[0] = (new string[9] { "(Don't, \nactually \n...)", "(He \nmeans \nme.)", "(When \nwill \nthey do \nanother \none???)", "(Are \nthey \neven \nhuman?)", "(That's \nnot your \nhusband, \nOK?)", "(Beware \nof \ndog.)", "(...Okay, \nyou were \njust \npranking \nus.)", "(Well. \nDon't \nleave me \nout!)", "(A dog \nthat pets \ndogs... \nAmazing!)" })[dialogue];
		}
		else
		{
			if (Object.FindObjectOfType<Dogamy>().IsKilled() && !killed && !hostile)
			{
				renderSpareBar = false;
				hostile = true;
				PlaySFX("sounds/snd_deathnoise");
				GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().color = new Color32(237, 28, 36, byte.MaxValue);
			}
			text[0] = (new string[3] { "(Misery \nawaits \nyou.)", "(Kneel \nand \nsuffer!)", "(I'll \nchop you \nin \nhalf!)" })[Random.Range(0, 3)];
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}
}
