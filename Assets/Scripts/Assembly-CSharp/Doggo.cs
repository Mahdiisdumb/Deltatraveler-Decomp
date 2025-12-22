using UnityEngine;

public class Doggo : EnemyBase
{
	private bool saidFirstLine;

	private bool approached;

	private bool hitLastAttack = true;

	private int approachCount;

	private int rageFrames;

	private int response = -1;

	private int lastFlavor = -1;

	private bool acknowledgeUnhostile;

	private bool pettedOnce;

	private bool assuredAfterUnhostile;

	private Sprite[] eyeSprites;

	private Sprite[] headSprites;

	private int bodyFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Doggo";
		fileName = "doggo";
		checkDesc = "* A bit too afraid of movement\n  nowadays.";
		maxHp = 320;
		hp = maxHp;
		atk = 13;
		def = 10;
		displayedDef = 7;
		hostile = true;
		hpToUnhostile = 0;
		canSpareViaFight = false;
		chatter = new string[1] { "error\nno\nbrute\nforce" };
		flavorTxt = new string[2] { "* Doggo is constantly searching\n  for movement.", "* Doggo looks back and forth\n  anxiously." };
		dyingTxt = flavorTxt;
		satisfyTxt = flavorTxt;
		actNames = new string[3] { "Pet", "Approach", "N!Assure" };
		hurtSound = "sounds/snd_doghurt1";
		defaultChatSize = "RightSmall";
		exp = 30;
		gold = 15;
		eyeSprites = new Sprite[13];
		for (int i = 0; i < 7; i++)
		{
			if (i != 6)
			{
				eyeSprites[i] = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_eyes_" + i);
			}
			switch (i)
			{
			case 6:
				eyeSprites[10] = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_eyes_" + i);
				break;
			case 1:
				eyeSprites[11] = eyeSprites[i];
				eyeSprites[12] = eyeSprites[i];
				break;
			case 5:
				eyeSprites[6] = eyeSprites[i];
				eyeSprites[7] = eyeSprites[i];
				break;
			case 4:
				eyeSprites[8] = eyeSprites[i];
				break;
			case 3:
				eyeSprites[9] = eyeSprites[i];
				break;
			}
		}
		headSprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_head_mad_0"),
			Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_head_mad_1")
		};
		attacks = new int[1] { 81 };
		defaultChatPos = new Vector2(72f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		bodyFrames++;
		int num = bodyFrames / 3 % 13;
		GetPart("head").Find("eyes").GetComponent<SpriteRenderer>().sprite = eyeSprites[num];
		float num2 = (float)(bodyFrames % 30) / 15f;
		if (num2 > 1f)
		{
			num2 = 2f - num2;
		}
		GetPart("arms").transform.localPosition = new Vector3(0f, Mathf.Lerp(2.743f, 2.5f, num2));
		if (!hostile)
		{
			return;
		}
		if (approached)
		{
			rageFrames++;
			if (rageFrames % 7 == 0)
			{
				GetPart("head").GetComponent<SpriteRenderer>().flipX = !GetPart("head").GetComponent<SpriteRenderer>().flipX;
			}
		}
		else
		{
			int num3 = bodyFrames / 4 % 2;
			GetPart("head").GetComponent<SpriteRenderer>().sprite = headSprites[num3];
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Pet")
		{
			if (!hostile)
			{
				AddActPoints(25);
				if (!pettedOnce)
				{
					response = 0;
					pettedOnce = true;
					return new string[1] { "* You pet Doggo." };
				}
				return new string[1] { "* You thoroughly pet Doggo." };
			}
			return new string[1] { (!hitLastAttack) ? "* You reach your hand,^05 but then\n  wonder if approaching him might\n  be better,^05 so you don't pet." : "* Doggo is too suspicious of\n  your movements." };
		}
		if (GetActNames()[i] == "Approach")
		{
			if (!hostile)
			{
				return new string[1] { "* Doggo already trusts you." };
			}
			if (!approached && hitLastAttack)
			{
				return new string[1] { "* You worried that Doggo would\n  attack you instantly,^05 so\n  you didn't." };
			}
			if (!approached)
			{
				approached = true;
				response = 1;
				GetPart("arms").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_arms_rage");
				GetPart("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_head_rage");
				GetPart("head").Find("eyes").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
			}
			approachCount++;
			if (approachCount == 1)
			{
				return new string[1] { "* You slowly approached Doggo.\n* He heard you approach and\n  prepares to swing." };
			}
			if (approachCount == 2)
			{
				return new string[1] { "* You continue to approach Doggo.\n* He can't tell what your\n  motivations are." };
			}
			if (approachCount == 3)
			{
				Unhostile();
				AddActPoints(50);
				return new string[2] { "* You are now very close to\n  Doggo.", "* Something about this intimate\n  distance calms him down." };
			}
		}
		else if (GetActNames()[i] == "N!Assure")
		{
			if (hostile)
			{
				if (approached)
				{
					response = 2;
					Unhostile();
					AddActPoints(50);
					return new string[4] { "* Noelle spoke calmly to Doggo.", "no_happy`snd_txtnoe`* Hey,^05 we're not gonna\n  hurt you.", "no_happy`snd_txtnoe`* Maybe we could be\n  friends...?", "* Doggo seems to have calmed\n  down,^05 but isn't entirely\n  convinced yet." };
				}
				return new string[2] { "no_confused_side`snd_txtnoe`* (Wait,^05 Kris,^05 we don't\n  want to freak him\n  out!)", "no_happy`snd_txtnoe`* (Maybe we should make\n  our presence known\n  first...)" };
			}
			if (!assuredAfterUnhostile)
			{
				response = 2;
				assuredAfterUnhostile = true;
				AddActPoints(25);
				return new string[2] { "* Noelle spoke calmly to Doggo.", "no_happy`snd_txtnoe`* Maybe we could...^05 play\n  fetch after this...?" };
			}
			return new string[2] { "no_thinking`snd_txtnoe`* (I don't know what\n  else to say...)", "no_happy`snd_txtnoe`* (Sorry,^05 Kris.)" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		switch (i)
		{
		case 1:
			if (!hostile)
			{
				if (!tired)
				{
					tired = true;
					Hit(3, 1f, playSound: true);
					Util.GameManager().SetSessionFlag(4, 1);
					return new string[2] { "* Susie snuck behind Doggo's\n  sentry station and shook it.", "* Doggo jumped and hit his\n  head on the ceiling." };
				}
				return new string[1] { "* Susie shook Doggo's station\n  again,^05 but nothing happened." };
			}
			return new string[2] { "su_annoyed`snd_txtsus`* (Kris,^05 I don't want him\n  to stab any of us.)", "su_smirk_sweat`snd_txtsus`* (You prolly need to\n  calm him down.)" };
		case 2:
			if (!hostile)
			{
				response = 0;
				pettedOnce = true;
				AddActPoints(25);
				return new string[1] { "* Noelle pet Doggo." };
			}
			return new string[2] { "no_confused_side`snd_txtnoe`* (Wait,^05 Kris,^05 we don't\n  want to freak him\n  out!)", "no_happy`snd_txtnoe`* (Maybe we should make\n  our presence known\n  first...)" };
		default:
			return base.PerformAssistAct_Old(i);
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp < maxHp / 2 && !approached && hostile)
		{
			approached = true;
			approachCount++;
			response = 1;
			GetPart("arms").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_arms_rage");
			GetPart("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_head_rage");
			GetPart("head").Find("eyes").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0f);
		}
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (partyMember == 3)
		{
			if (hp == 1)
			{
				Util.GameManager().SetSessionFlag(5, 1);
			}
			return 1;
		}
		return base.CalculateDamage(partyMember, rawDmg, forceMagic);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		bool flag = hostile;
		if (response > -1)
		{
			if (hostile)
			{
				saidFirstLine = true;
				if (response == 1)
				{
					text = new string[1] { (new string[3] { "error \nbark", "GET \nAWAY,^05 \nFIEND!", "WHAT \nDO YOU \nWANT?!" })[approachCount] };
					lastFlavor = 2;
				}
				else if (approached)
				{
					text = new string[1] { hitLastAttack ? "WHY ARE \nYOU \nHERE??" : "WHERE'D \nYOU GO\n???" };
				}
			}
			else
			{
				flag = true;
				if (!acknowledgeUnhostile)
				{
					acknowledgeUnhostile = true;
					text = new string[1] { (approachCount >= 3) ? "(Are \nthey...\nnice?)" : "You're \ngonna \nneed to \nshow me" };
				}
				else if (response == 0)
				{
					text = new string[1] { (satisfied >= 100) ? "Oh \nyeah..." : "Maybe... \na lil' \nmore..." };
				}
				else if (response == 2)
				{
					text = new string[1] { "That \nsounds...\nkind of \nexciting" };
				}
				if (lastFlavor == -1)
				{
					lastFlavor = response;
				}
				response = -1;
			}
		}
		else if (!saidFirstLine && hostile)
		{
			saidFirstLine = true;
			text = new string[1] { "Don't \nmove an \ninch!" };
		}
		else if (hostile)
		{
			text = new string[1] { hitLastAttack ? "I hit \nsomething\n?????" : "Will it \nmove \nthis \ntime?" };
		}
		hitLastAttack = false;
		if (flag)
		{
			speed = 1;
			base.Chat(text, type, sound, pos, canSkip, speed);
			chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
		}
	}

	public void HitLastAttack()
	{
		hitLastAttack = true;
	}

	public override string GetRandomFlavorText()
	{
		if (satisfied >= 100)
		{
			if (!pettedOnce)
			{
				return "* Doggo loves fetch!!!^30\n\n* Wait what error???";
			}
			return "* Doggo has been pet.";
		}
		if (lastFlavor == 0)
		{
			return "* Doggo needs more pats.";
		}
		if (lastFlavor == 2)
		{
			lastFlavor = -1;
			if (!hostile)
			{
				return "* Doggo feels a bit better.";
			}
			return "* Doggo is freaking out!!!";
		}
		if (!hostile)
		{
			return "* Doggo still isn't convinced.";
		}
		if (!hitLastAttack)
		{
			return "* Doggo can't seem to\n  find anything.";
		}
		return base.GetRandomFlavorText();
	}

	public int GetProgress()
	{
		if (hostile)
		{
			if (!approached)
			{
				return 0;
			}
			return 1;
		}
		return 2;
	}

	public override void Unhostile()
	{
		if (hostile)
		{
			response = 3;
			def = -500;
			if (hp > 0)
			{
				Util.GameManager().SetSessionFlag(3, 1);
			}
			GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_body");
			GetPart("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_head_normal");
			GetPart("head").GetComponent<SpriteRenderer>().flipX = false;
			GetPart("head").Find("eyes").localPosition = Vector3.zero;
			GetPart("head").Find("eyes").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			GetPart("tail").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f);
			GetPart("arms").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Doggo/spr_b_doggo_arms");
		}
		base.Unhostile();
	}
}
