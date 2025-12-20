using UnityEngine;

public class Napstablook : EnemyBase
{
	private int lastAct = -1;

	private int susieConvince;

	private bool krisHit;

	private int turnCount;

	private int hitAttempts;

	private int hitLinesState;

	private bool finalHit;

	private bool hardmode;

	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Napstablook";
		fileName = "napstablook";
		checkDesc = "* This monster doesn't seem to\n  have a sense of humor...";
		if ((int)Util.GameManager().GetFlag(108) == 1)
		{
			checkDesc = "* Missed the memo and is the\n  same difficulty as normal.";
			hardmode = true;
		}
		maxHp = 180;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 10;
		def = 10;
		flavorTxt = new string[4] { "* Napstablook is staring into\n  the distance.", "* Napstablook is wishing they\n  weren't here.", "* Napstablook is pretending to\n  sleep.", "* The faint odor of ectoplasm\n  permeates the vicinity." };
		dyingTxt = flavorTxt;
		satisfyTxt = flavorTxt;
		chatter = new string[3] { "i'm \nfine, \nthanks.", "just \npluggin \nalong...", "nnnnnn\nggghhh." };
		actNames = new string[3] { "Flirt", "Threat", "Cheer" };
		sActionName = "Talk";
		defaultChatSize = "RightSmall";
		defaultChatPos = new Vector2(135f, 57f);
		exp = 0;
		gold = 0;
		canSpareViaFight = false;
		attacks = new int[2] { 11, 14 };
		sprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Napstablook/spr_b_napstablook_0"),
			Resources.Load<Sprite>("battle/enemies/Napstablook/spr_b_napstablook_1")
		};
	}

	protected override void Update()
	{
		if (gotHit)
		{
			frames++;
			if (frames % (2 + (finalHit ? 3 : 0)) == 0)
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
				else
				{
					gotHit = false;
				}
			}
			if (finalHit)
			{
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
			}
		}
		else if (!finalHit)
		{
			frames++;
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[frames / 6 % 2];
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		int num = 0;
		int num2 = hp;
		predictedDmg[partyMember] = 0f;
		if (rawDmg < 35f)
		{
			hitAttempts++;
			rawDmg = 0f;
		}
		if (rawDmg > 0f)
		{
			if (partyMember == 1 && rawDmg >= 35f)
			{
				finalHit = true;
				rawDmg = 70f;
				CombineParts();
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_dmg");
				Object.FindObjectOfType<BattleManager>().StopMusic();
				Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
				aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
				aud.Play();
			}
			float num3 = 1.5f + (float)Object.FindObjectOfType<GameManager>().GetATK(partyMember) / 3f;
			num = Mathf.RoundToInt(rawDmg * num3 - (float)(def + buffs[1]));
			if (num <= 0)
			{
				num = 1;
			}
			hp -= num;
			frames = 0;
			gotHit = true;
			if (hp <= 0)
			{
				hp = 0;
			}
			moveBody = -10;
			if (finalHit)
			{
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
			}
		}
		else
		{
			num = (int)rawDmg;
			hp -= num;
			if (hp > num2 && num != 0)
			{
				aud.clip = Resources.Load<AudioClip>("sounds/snd_heal");
				aud.Play();
				if (hp > maxHp)
				{
					hp = maxHp;
				}
			}
		}
		if (!(rawDmg > 0f) || !(enemySOUL != null))
		{
			string text = "EnemyHP" + obj.transform.parent.gameObject.name[5];
			if (!GameObject.Find(text))
			{
				EnemyHPBar component = Object.Instantiate(Resources.Load<GameObject>("battle/enemies/EnemyHP"), GameObject.Find("BattleCanvas").transform).GetComponent<EnemyHPBar>();
				component.gameObject.name = "EnemyHP" + obj.transform.parent.gameObject.name[5];
				component.transform.localScale = new Vector2(1f, 1f);
				component.transform.localPosition = hpPos;
				component.StartHP(num, num2, maxHp, partyMember, hpWidth, false);
			}
			else
			{
				GameObject.Find(text).GetComponent<EnemyHPBar>().StartHP(num, num2, maxHp, partyMember, false);
			}
		}
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i;
		if (GetActNames()[i] == "Flirt")
		{
			return new string[1] { "* You tried to flirt with\n  Napstablook." };
		}
		if (GetActNames()[i] == "Threat")
		{
			return new string[1] { "* You give Napstablook a\n  cruel look." };
		}
		if (GetActNames()[i] == "Cheer")
		{
			return new string[1] { "* But you saw it as insincere." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (i == 1)
		{
			AddActPoints(25);
			lastAct = 10;
			susieConvince++;
			if (susieConvince != 1)
			{
				if (susieConvince != 2)
				{
					if (susieConvince != 3)
					{
						if (susieConvince == 4)
						{
							Object.FindObjectOfType<GameManager>().SetFlag(12, 0);
							return new string[1] { "su_neutral`snd_txtsus`* So if you could just\n  let us get home,^05 we\n  will." };
						}
						tired = true;
						return new string[2] { "* Susie tried to talk to\n  Napstablook.", "* Napstablook became TIRED." };
					}
					return new string[1] { hardmode ? "su_side`snd_txtsus`* And this kid...^10 I think\n  they're really freaked\n  out." : "su_side`snd_txtsus`* And Kris...^10 I think\n  they're really freaked\n  out." };
				}
				return new string[1] { "su_side`snd_txtsus`* The long story short\n  is that we're not\n  from here." };
			}
			return new string[2] { "* Susie tried to talk to\n  Napstablook.", "su_side`snd_txtsus`* Hey, there's something\n  that you need to\n  understand." };
		}
		return base.PerformAssistAct(i);
	}

	public override string GetRandomFlavorText()
	{
		if (hitAttempts >= 2 && hitLinesState == 0)
		{
			hitLinesState++;
			if (hardmode)
			{
				checkDesc = "* Damage the enemy with\n  RUDE magic.";
				return "su_side_sweat`snd_txtsus`* Umm...\n* I don't think this\n  enemy can get hurt.";
			}
			checkDesc = "* ENEMY WEAKNESS: RUDE Damage";
			return "su_side_sweat`snd_txtsus`* Kris...?\n* I don't think this\n  enemy can get hurt.";
		}
		if (hitAttempts >= 4 && hitLinesState == 1)
		{
			hitLinesState++;
			if (hardmode)
			{
				return "su_annoyed`snd_txtsus`* Dude.\n* This enemy is a ghost.\n* We can't hit it.";
			}
			return "su_annoyed`snd_txtsus`* Kris.\n* This enemy is a ghost.\n* We can't hit it.";
		}
		if (hitAttempts >= 6 && hitLinesState >= 2 && susieConvince == 0)
		{
			hitLinesState++;
			return "* You can use S-ACTION from\n  Susie's MAGIC menu to talk\n  to Napstablook.";
		}
		return base.GetRandomFlavorText();
	}

	public override int GetNextAttack()
	{
		if (hp <= 0 || susieConvince == 4)
		{
			killed = hp <= 0;
			return 13;
		}
		turnCount++;
		if (turnCount == 2)
		{
			return 12;
		}
		return base.GetNextAttack();
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (hp <= 0)
		{
			type = "RightWide";
			pos = new Vector2(180f, 57f);
			exp = 25;
			Object.FindObjectOfType<BattleManager>().StopMusic();
			text = new string[5] { "what the...?", "h-^10how did you...", "you used her to \nkill me.", "all just because \ni wanted to take \na nap...", "why......" };
		}
		else if (lastAct == 10)
		{
			if (susieConvince == 4)
			{
				type = "RightWide";
				pos = new Vector2(180f, 57f);
				text = new string[4]
				{
					"oh no, i'm really \nsorry.",
					"to be honest, the \nmonsters here aren't \nreally hostile",
					hardmode ? "they're mostly just \nreally sad." : "they mostly just \nspook easily.",
					"but if you need \nto get home, i'll \nget outta your way."
				};
			}
			else
			{
				string[] array = new string[3] { "what?", "really?", "uh oh" };
				text = new string[1] { array[susieConvince - 1] };
			}
		}
		else if (lastAct >= 0)
		{
			string[] array2 = new string[4] { "oh i'm \nREAL \nfunny.", "didn't \nyou want \nto beat \nme up?", "go \nahead, \ndo it.", "what's \nwrong?" };
			text = new string[1] { array2[lastAct] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		if (hp <= 0)
		{
			chatbox.gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
		}
		else
		{
			chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
		}
	}

	public int GetNapEndState()
	{
		if (finalHit)
		{
			return 2;
		}
		return 0;
	}
}
