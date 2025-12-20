using System;
using UnityEngine;

public class Porky : EnemyBase
{
	private int animFrames;

	private bool leftLeg = true;

	private int[] legStates = new int[4];

	private bool eyeIsOpen;

	private int eyeFrames = 9;

	private int moveFrameMulti = 1;

	private bool toppleEyeAnim;

	private int toppleEyeFrames;

	private Sprite[] eyeSprites = new Sprite[4];

	private Sprite eyeHurt;

	private bool digIntoGround;

	private Vector3[] positions = new Vector3[5];

	private Vector3[] stretches = new Vector3[5];

	private readonly string[] PART_NAMES = new string[5] { "mech", "backleft", "frontright", "backright", "frontleft" };

	private bool statedFirstPhrase;

	private int startPhrase;

	private int curAttack = -1;

	private int[] orderedAttacks = new int[12]
	{
		58, 59, 60, 58, 61, 59, 64, 58, 65, 66,
		59, 58
	};

	private int randomAttackNo;

	private bool parryDetected;

	private bool parryPhraseStated;

	private int parryHint = -1;

	private bool destroyed;

	private bool inFinale;

	private bool finaleKilled;

	private bool decommission;

	private int decommissionFrames;

	private bool sparedInFinale;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Porky";
		fileName = "porky";
		checkDesc = "* Dedicated troublemaker turned\n  loyal servant of evil.";
		maxHp = 1600;
		hp = maxHp;
		hpPos = new Vector2(2f, 69f);
		hpWidth = 150;
		atk = 50;
		def = -10;
		displayedDef = 50;
		flavorTxt = new string[4] { "* Mechanical whirrs fill the\n  air.", "* Smells like metal and gas.", "* Porky flashes a mischievous\n  grin.", "* Porky's mech stomps idly." };
		dyingTxt = new string[1] { "* Porky's mech is sparking." };
		actNames = new string[4] { "Topple", "SN!XTopple", REDBUSTER_NAME, DUALHEAL_NAME };
		sActionName = "Topple";
		nActionName = "Topple";
		playerMultiplier = Mathf.Lerp(1.1f, 1f, (float)(Util.GameManager().GetLV() - 1) / 6f);
		useCustomDamageAnimation = true;
		canSpareViaFight = false;
		renderSpareBar = false;
		emptyHPBarWhenZero = false;
		defaultChatSize = "RightSmall";
		exp = 250;
		gold = 60;
		for (int i = 0; i < 4; i++)
		{
			eyeSprites[i] = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_mech_body_" + i);
		}
		eyeHurt = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_mech_body_dmg");
		for (int j = 0; j < 5; j++)
		{
			positions[j] = Vector3.zero;
		}
		attacks = new int[6] { 59, 60, 61, 64, 65, 66 };
		defaultChatPos = new Vector2(182f, 126f);
		defaultChatSize = "RightWide";
	}

	protected override void Update()
	{
		if (!destroyed)
		{
			base.Update();
			IdleAnimation();
			for (int i = 0; i < 5; i++)
			{
				GetPart(PART_NAMES[i]).transform.localPosition = positions[i];
				if (i > 0)
				{
					GetPart(PART_NAMES[i]).transform.localScale = stretches[i];
					Color b = Color.white;
					if (legStates[i - 1] > 0)
					{
						b = Color.Lerp(new Color(0.65f, 0.65f, 0.65f), Color.red, (float)(legStates[i - 1] - 1) / 3f);
					}
					GetPart(PART_NAMES[i]).GetComponent<SpriteRenderer>().color = Color.Lerp(GetPart(PART_NAMES[i]).GetComponent<SpriteRenderer>().color, b, 0.2f);
				}
				else
				{
					GetPart(PART_NAMES[i]).transform.localPosition += new Vector3(0f, (float)moveBody / 96f * (float)moveFrameMulti);
				}
			}
			if ((eyeIsOpen && gotHit) || toppleEyeAnim)
			{
				GetPart("mech").GetComponent<SpriteRenderer>().sprite = eyeHurt;
				if (toppleEyeAnim)
				{
					toppleEyeFrames++;
					if (toppleEyeFrames == 12)
					{
						toppleEyeAnim = false;
					}
				}
			}
			else if (eyeFrames / 3 > 0 && !eyeIsOpen)
			{
				eyeFrames--;
				GetPart("mech").GetComponent<SpriteRenderer>().sprite = eyeSprites[eyeFrames / 3];
			}
			else if (eyeFrames / 3 < 3 && eyeIsOpen)
			{
				eyeFrames++;
				GetPart("mech").GetComponent<SpriteRenderer>().sprite = eyeSprites[eyeFrames / 3];
			}
			else
			{
				GetPart("mech").GetComponent<SpriteRenderer>().sprite = eyeSprites[eyeFrames / 3];
			}
			return;
		}
		if (gotHit && !finaleKilled)
		{
			frames++;
			if (frames <= 60)
			{
				int num = (finaleKilled ? 6 : 4);
				if (frames % num == 0)
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
				if (frames == 60)
				{
					GetPart("mech").Find("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_head_nohp");
					Util.GameManager().PlayGlobalSFX("sounds/snd_bomb");
					moveBody = 10;
					for (int j = 0; j < 5; j++)
					{
						float num2 = ((j >= 3) ? 1 : (-1));
						if (j == 0)
						{
							GetPart(PART_NAMES[j]).transform.localPosition = new Vector3(0f, 2.252f);
							continue;
						}
						GetPart(PART_NAMES[j]).transform.localPosition = new Vector3(((j == 1 || j == 3) ? 3.329f : (-1.936f)) * num2, -1f / 24f);
						GetPart(PART_NAMES[j]).transform.localScale = new Vector3(1f, 1f, 1f);
					}
				}
			}
			else
			{
				if (frames % 2 == 0)
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
				for (int k = 0; k < 5; k++)
				{
					if (k == 0)
					{
						GetPart(PART_NAMES[k]).transform.localPosition = new Vector3(0f, Mathf.Lerp(2.252f, 1.44f, (float)(frames - 80) / 5f));
					}
					else
					{
						GetPart(PART_NAMES[k]).transform.localScale = new Vector3(1f, Mathf.Lerp(1f, (k % 2 == 1) ? 0.6875f : 0.739f, (float)(frames - 80) / 5f), 1f);
					}
				}
				if (frames == 85)
				{
					Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
					UnityEngine.Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
					decommission = true;
					GetPart("mech").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_mech_body_nohp");
				}
				if (frames == 110)
				{
					frames = 0;
					gotHit = false;
				}
			}
			obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
		}
		else if (gotHit && finaleKilled)
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
			obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
			if (frames == 60)
			{
				gotHit = false;
				aud.clip = Resources.Load<AudioClip>("sounds/snd_f_destroyed3");
				aud.Play();
				aud.loop = true;
			}
		}
		else if (finaleKilled)
		{
			obj.transform.position = mainPos + new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 48f;
		}
		if (!decommission)
		{
			return;
		}
		decommissionFrames++;
		if (decommissionFrames % (finaleKilled ? 4 : 10) == 1)
		{
			int num3 = UnityEngine.Random.Range(-60, 60);
			Vector3 position = new Vector3(0f - Mathf.Cos(num3), Mathf.Sin(num3)) * UnityEngine.Random.Range(1.13f, 2.31f) + new Vector3(0f, 1.27f);
			SpriteRenderer component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemySmoke"), position, Quaternion.identity).GetComponent<SpriteRenderer>();
			component.sortingOrder = UnityEngine.Random.Range(30, 32);
			if (component.sortingOrder == 30)
			{
				component.sortingOrder = 19;
			}
		}
	}

	private void IdleAnimation()
	{
		animFrames++;
		int num = (leftLeg ? 1 : 3);
		int num2 = ((!leftLeg) ? 1 : 3);
		float num3 = ((!leftLeg) ? 1 : (-1));
		if (animFrames < 25)
		{
			positions[0] = new Vector3(0f, Mathf.Lerp(2.252f, 2.44f, (float)animFrames / 25f));
			for (int i = 0; i < 2; i++)
			{
				positions[i + num] = new Vector3(((i == 0) ? 3.329f : (-1.936f)) * num3, Mathf.Lerp(-1f / 24f, 0.3f, (float)animFrames / 25f));
				positions[i + num2] = new Vector3(((i == 0) ? 3.329f : (-1.936f)) * (0f - num3), -1f / 24f);
				stretches[i + num] = new Vector3(1f, 1f, 1f);
				stretches[i + num2] = new Vector3(1f, Mathf.Lerp(1f, 1.025f, (float)animFrames / 25f), 1f);
			}
		}
		else if (animFrames <= 30)
		{
			int num4 = animFrames - 25;
			positions[0] = new Vector3(0f, Mathf.Lerp(2.44f, 2.252f, (float)num4 / 5f));
			for (int j = 0; j < 2; j++)
			{
				positions[j + num] = new Vector3(((j == 0) ? 3.329f : (-1.936f)) * num3, Mathf.Lerp(0.3f, -1f / 24f, (float)num4 / 5f));
				positions[j + num2] = new Vector3(((j == 0) ? 3.329f : (-1.936f)) * (0f - num3), -1f / 24f);
				stretches[j + num] = new Vector3(1f, 1f, 1f);
				stretches[j + num2] = new Vector3(1f, Mathf.Lerp(1.025f, 1f, (float)num4 / 5f), 1f);
			}
		}
		else if (animFrames <= 40)
		{
			float t = Mathf.Sin((float)((animFrames - 30) * 18) * ((float)Math.PI / 180f));
			positions[0] = new Vector3(0f, Mathf.Lerp(2.252f, 2.14f, t));
			for (int k = 0; k < 2; k++)
			{
				stretches[k + num] = new Vector3(1f, Mathf.Lerp(1f, 0.95f, t), 1f);
				stretches[k + num2] = new Vector3(1f, Mathf.Lerp(1f, 0.95f, t), 1f);
			}
			if (animFrames == 40)
			{
				animFrames = 0;
				leftLeg = !leftLeg;
			}
		}
	}

	public void Explode()
	{
		aud.Stop();
		CombineParts();
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyExplosion")).transform.position += new Vector3(mainPos.x, 0f);
		UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeIn(10, Color.white);
		UnityEngine.Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = false;
		decommission = false;
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/Porky/PorkyFling"));
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Check")
		{
			if (inFinale)
			{
				return base.PerformAct(i);
			}
			if (!eyeIsOpen)
			{
				return new string[3]
				{
					base.PerformAct(i)[0],
					"* The mech's closed eye gives\n  it much more defense.",
					"* Topple its legs to startle\n  it and open its eye!"
				};
			}
			return new string[2]
			{
				base.PerformAct(i)[0],
				"* The mech's eye is currently\n  open, greatly weakening its\n  defenses!"
			};
		}
		if (GetActNames()[i] == "Topple")
		{
			bool flag = eyeIsOpen;
			int num = ((Util.GameManager().GetMiniPartyMember() != 1) ? 1 : 2);
			if (ToppleALeg(num))
			{
				toppleEyeAnim = true;
				toppleEyeFrames = 0;
				return new string[1] { string.Format("* You {0}", (num == 2) ? "and Paula moved\n  to topple the mech legs!" : "moved to topple\n  Porky's mech legs!") + ((flag != eyeIsOpen) ? "\n* The mech is now vulnerable!" : ((num == 2) ? "\n* Got two!" : "\n* Got one!")) };
			}
			return new string[1] { "* But the mech's legs were\n  already completely disabled." };
		}
		if (GetActNames()[i].EndsWith("XTopple"))
		{
			bool flag2 = eyeIsOpen;
			int count = ((Util.GameManager().GetMiniPartyMember() == 1) ? 4 : 3);
			if (ToppleALeg(count))
			{
				toppleEyeAnim = true;
				toppleEyeFrames = 0;
				return new string[1] { "* Everyone moved to topple\n  Porky's mech legs!" + ((flag2 != eyeIsOpen) ? "\n* The mech is now vulnerable!" : "") };
			}
			return new string[1] { "* But the mech's legs were\n  already completely disabled." };
		}
		return base.PerformAct(i);
	}

	public override void Chat()
	{
		if (!inFinale)
		{
			if (hp <= 0)
			{
				if (UnityEngine.Object.FindObjectOfType<GameManager>().GetHP(0) - Util.GameManager().GetMiniMemberMaxHP() < 1)
				{
					UnityEngine.Object.FindObjectOfType<GameManager>().SetHP(0, 1 + Util.GameManager().GetMiniMemberMaxHP());
					UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_heal");
				}
				UnityEngine.Object.FindObjectOfType<BattleManager>().ForceSoloKris(true);
				inFinale = true;
				UnityEngine.Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
				GetPart("mech").Find("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_head_worry");
				actNames = new string[1] { GetBMString("act_check", 0) };
				Chat(new string[7] { "Well, well, well.\nLooks like you bested \nme.", "But I'm not done \njust yet.", "As you can see,^05 \nmy pathetic little \nmech is still alive.", "One more hit,^05 and \nit explodes and \ndies.", "You'll be rewarded \nwith EXP if you \nstrike it.", "But if you let \nit live,^05 then I \nget to run away \nwith no punishment!", "What'll you do,^05 you \nself-righteous moron?" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
			}
			else if (parryDetected && !parryPhraseStated)
			{
				parryPhraseStated = true;
				Chat(new string[3] { "W-^05what did you...\n^05I thought you...", "D-^05Doesn't matter!\n^05I'll still crush \nyou!", "(Oh god why doesn't \nthis thing have a \nNORMAL ray???)" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
			}
			else if (!statedFirstPhrase)
			{
				if (startPhrase == 1)
				{
					Chat(new string[4] { "HAHAHAHA!!!^05\nYou IDIOTS!", "As long as my \nmachine's eye is \nclosed,^05 you can't \nhurt it!", "And you know what?", "I'll make it even \nharder for you to \ndo so!" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
				}
				else if (startPhrase == 2)
				{
					Chat(new string[3] { "You PESTS!", "You knew what to \ndo all along,^05 didn't \nyou???", "I'll make you PAY \nfor that!" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
				}
				else
				{
					Chat(new string[5] { "I bet you're so \nstumped as to what \nto do...", "Well,^05 you can GIVE \nUP!", "As long as my \nmachine's eye is \nclosed,^05 you can't \nhurt it!", "And you know what?", "I'll make it even \nharder for you to \ndo so!" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
				}
			}
		}
		else if (finaleKilled)
		{
			Chat(new string[8] { "HAHAHAHA!!!\n^05I can't believe you \nactually killed it \nto spite me!", "Did you honestly \nthink it WANTED to \nhurt you???", "ABSOLUTELY NOT!", "And you know what \nthe best part is?", "IT WON'T DO \nANYTHING TO ME!!!", "Constantly time-\ntraveling has made \nme invincible!", "I'll just be able \nto get away \nanyway!!!", "SO LONG,^05 LOSERS!" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
		}
		else if (sparedInFinale)
		{
			Chat(new string[1] { "HA!^05\nYou've fallen into \nmy trap!" }, defaultChatSize, "snd_txtpor", defaultChatPos, true, 0);
		}
	}

	private bool ToppleALeg(int count, bool freeze = false)
	{
		bool flag = false;
		for (int i = 0; i < count; i++)
		{
			int num = 0;
			for (int j = 1; j < 4; j++)
			{
				if (legStates[j] < legStates[num])
				{
					num = j;
				}
			}
			if (legStates[num] < 3)
			{
				legStates[num] = 3;
				flag = true;
			}
		}
		if (flag)
		{
			startPhrase = 2;
			aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
			aud.Play();
		}
		DetermineEyeOpen();
		return flag;
	}

	public void ToppleSpecificLeg(int leg)
	{
		legStates[leg] = 4;
		toppleEyeAnim = true;
		toppleEyeFrames = 0;
		aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
		aud.Play();
		DetermineEyeOpen();
	}

	public int[] GetLegStates()
	{
		return legStates;
	}

	public Color GetLegColor(int leg)
	{
		return GetPart(PART_NAMES[leg + 1]).GetComponent<SpriteRenderer>().color;
	}

	private void DetermineEyeOpen()
	{
		eyeIsOpen = true;
		for (int i = 0; i < 4; i++)
		{
			if (legStates[i] == 0)
			{
				eyeIsOpen = false;
			}
		}
	}

	public override string[] PerformAssistAct(int i)
	{
		bool flag = eyeIsOpen;
		switch (i)
		{
		case 1:
			if (ToppleALeg(1))
			{
				toppleEyeAnim = true;
				toppleEyeFrames = 0;
				return new string[1] { "* Susie moved to topple\n  Porky's mech legs!" + ((flag != eyeIsOpen) ? "\n* The mech is now vulnerable!" : "\n* Got one!") };
			}
			return new string[1] { "* But the mech's legs were\n  already completely disabled." };
		case 2:
			if (ToppleALeg(1))
			{
				toppleEyeAnim = true;
				toppleEyeFrames = 0;
				return new string[1] { "* Noelle moved to topple\n  Porky's mech legs!" + ((flag != eyeIsOpen) ? "\n* The mech is now vulnerable!" : "\n* Got one!") };
			}
			return new string[1] { "* But the mech's legs were\n  already completely disabled." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override string GetRandomFlavorText()
	{
		MonoBehaviour.print("Get Random Flavor Text");
		if (inFinale)
		{
			return "* ...";
		}
		for (int i = 0; i < 4; i++)
		{
			if (legStates[i] > 0)
			{
				legStates[i]--;
				if (legStates[i] == 0)
				{
					eyeIsOpen = false;
				}
			}
		}
		if (!parryDetected)
		{
			parryHint++;
			if (parryHint == 0)
			{
				return "su_inquisitive`snd_txtsus`* (Parrying... by hitting\n  ^Z before getting\n  hit?)";
			}
			if (parryHint == 1)
			{
				return "no_playful`snd_txtnoe`* Kris,^05 you have SOUL\n  powers,^05 right?\n* Try to parry!";
			}
			if (parryHint == 2)
			{
				return "su_annoyed`snd_txtsus`* Dude,^05 try doing that\n  ^Z thing right before\n  you get hit.";
			}
		}
		else if (!parryPhraseStated)
		{
			return "su_surprised`snd_txtsus`* Hey,^05 that made us\n  invincible for a\n  bit...";
		}
		return base.GetRandomFlavorText();
	}

	public override void Spare(bool sleepMist = false)
	{
		sparedInFinale = true;
	}

	public override int GetNextAttack()
	{
		if (inFinale)
		{
			if (finaleKilled)
			{
				return 63;
			}
			if (sparedInFinale)
			{
				return 62;
			}
			return -1;
		}
		if (!statedFirstPhrase)
		{
			statedFirstPhrase = true;
			return 57;
		}
		if (curAttack != orderedAttacks.Length - 1)
		{
			curAttack++;
			return orderedAttacks[curAttack];
		}
		randomAttackNo++;
		if (randomAttackNo % 4 == 0)
		{
			return 58;
		}
		return base.GetNextAttack();
	}

	public void DetectParry()
	{
		if (!parryDetected)
		{
			parryDetected = true;
			GetPart("mech").Find("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_head_worry");
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (hp > 0 && !inFinale)
		{
			base.Hit(partyMember, rawDmg, playSound);
			killed = false;
			if (hp <= 0)
			{
				destroyed = true;
				GetPart("mech").GetComponent<SpriteRenderer>().sprite = eyeHurt;
				UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
				UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
				UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
				UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
				hp = 0;
			}
			if (startPhrase != 2)
			{
				startPhrase = 1;
			}
			if ((partyMember == 1 && (bool)UnityEngine.Object.FindObjectOfType<SpecialAttackEffect>()) || (partyMember == 3 && !UnityEngine.Object.FindObjectOfType<SpecialAttackEffect>() && rawDmg >= 35f))
			{
				moveFrameMulti = 2;
			}
			else
			{
				moveFrameMulti = 1;
			}
		}
		else if (rawDmg > 0f && inFinale)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
			aud.Play();
			moveBody = 16;
			gotHit = true;
			finaleKilled = true;
		}
	}

	public override int GetPredictedHP()
	{
		if (inFinale)
		{
			return 1;
		}
		return base.GetPredictedHP();
	}

	public override bool CanSpare()
	{
		return inFinale;
	}

	public override void TurnToDust()
	{
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		int num = base.CalculateDamage(partyMember, rawDmg, forceMagic);
		if (!eyeIsOpen)
		{
			num /= 10;
		}
		return num;
	}

	public bool IsVulnerable()
	{
		return eyeIsOpen;
	}
}
