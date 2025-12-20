using System;
using UnityEngine;

public class Paula : EnemyBase
{
	private bool startedFirstAttack;

	private bool usedFirstText;

	private bool inNessDeath;

	private bool heavyBreathing;

	private int stage;

	private int animFrames;

	private float x = 2.5f;

	private int redBusterCount = -1;

	private int[] orderedAttacks = new int[6] { 70, 71, 69, 72, 73, 74 };

	private int curAtk = -1;

	private BattleManager bm;

	private int menuAttackFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Paula";
		fileName = "paula";
		checkDesc = "";
		maxHp = 2500;
		hp = maxHp;
		hpPos = new Vector2(2f, 140f);
		hpWidth = 200;
		atk = 20;
		def = 4;
		displayedDef = 15;
		flavorTxt = new string[1] { "* ..." };
		dyingTxt = new string[1] { "* Paula is shaking." };
		actNames = new string[3] { "Talk", "Apologize", DUALHEAL_NAME };
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 250;
		gold = 20;
		attacks = new int[6] { 69, 70, 71, 72, 73, 74 };
		defaultChatPos = new Vector2(182f, 126f);
		defaultChatSize = "RightWide";
		bm = UnityEngine.Object.FindObjectOfType<BattleManager>();
	}

	protected override void Start()
	{
		base.Start();
		if (!UnityEngine.Object.FindObjectOfType<Ness>())
		{
			ActivatePhase2();
			x = 0f;
			ActivateHeavyBreathing();
			startedFirstAttack = true;
			usedFirstText = true;
		}
	}

	protected override void Update()
	{
		if (hp > 0)
		{
			base.Update();
			if (heavyBreathing)
			{
				if ((float)hp / (float)maxHp <= 0.4f && bm.GetState() < 3)
				{
					menuAttackFrames++;
					if (menuAttackFrames % 90 == 15)
					{
						float y = (new float[3] { -4.4f, -1.69f, -2.31f })[menuAttackFrames / 90 % 3];
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFireMainBullet"), new Vector3(0f, y), Quaternion.identity);
					}
				}
				else
				{
					menuAttackFrames = 0;
				}
			}
		}
		if (!gotHit && hp > 0)
		{
			animFrames++;
			float num = (Mathf.Cos((float)(animFrames * (heavyBreathing ? 4 : 3)) * ((float)Math.PI / 180f)) + 1f) / 2f;
			GetPart("body").transform.localPosition = new Vector3(0f, Mathf.Lerp(1.272f, 1.22f - (heavyBreathing ? 0.065f : 0f), 1f - num));
			GetPart("body").GetChild(0).transform.localPosition = new Vector3(0f, Mathf.Lerp(1.683f, 1.65f - (heavyBreathing ? 0.05f : 0f), 1f - num));
			if ((float)hp / (float)maxHp <= 0.1f)
			{
				float num2 = 0f;
				if (UnityEngine.Random.Range(0, 10) == 0)
				{
					num2 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
				}
				obj.transform.parent.position = new Vector3(num2 / 48f, 0f);
			}
			else
			{
				obj.transform.parent.position = new Vector3(Mathf.Lerp(obj.transform.parent.position.x, x, heavyBreathing ? 0.1f : 0.5f), 0f);
			}
		}
		else
		{
			if (!gotHit || hp > 0)
			{
				return;
			}
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
			if (frames == 90)
			{
				gotHit = false;
			}
			obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "SN!Debug")
		{
			stage = 2;
			satisfied = 100;
			GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_head_sad");
			return new string[1] { "* Magical redemption" };
		}
		if (GetActNames()[i] == "Talk")
		{
			if (UnityEngine.Object.FindObjectOfType<Ness>().ReduceDamage())
			{
				if (stage == 1)
				{
					satisfied += 50;
					stage++;
					GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_head_sad");
					return new string[1] { "* You talk Paula out of\n  fighting you." };
				}
				if (stage != 2)
				{
					return new string[1] { "* You tried to talk Paula out\n  of fighting you,^05 but she\n  doesn't seem to care." };
				}
				return new string[1] { "* Paula no longer wants to\n  talk to you." };
			}
			return new string[1] { "* You tried to talk to Paula,^05\n  but she isn't listening." };
		}
		if (GetActNames()[i] == "Apologize")
		{
			if (UnityEngine.Object.FindObjectOfType<Ness>().ReduceDamage())
			{
				if (stage == 0)
				{
					satisfied += 50;
					stage++;
					return new string[2] { "* You apologized to Paula for\n  betraying her.", "* She nods her head." };
				}
				return new string[1] { "* You have already apologized." };
			}
			return new string[1] { "* You tried to apologize to\n  Paula,^05 but she isn't\n  listening." };
		}
		if (GetActNames()[i] == REDBUSTER_NAME)
		{
			redBusterCount++;
			if (redBusterCount != 0)
			{
				if (redBusterCount == 1)
				{
					actNames = new string[4]
					{
						GetBMString("act_check", 0),
						"X-Slash;Physical Damage`40",
						"S!Red Buster;Deals RED Damage`60",
						"N!Dual Heal;Heals Everyone`50"
					};
					return new string[1] { "* You ordered Susie to cast\n  RED BUSTER.\n* Susie disobeyed orders!" };
				}
				Util.GameManager().Damage(0, 5);
				return new string[1] { "* You ordered Susie to cast\n  RED BUSTER.\n* Susie hits you!" };
			}
			return new string[2] { "* Your SOUL shined its power\n  onto Susie!", "* Susie cast RED BUSTER!" };
		}
		if (GetActNames()[i].StartsWith("X-Slash"))
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/CrossSlash")).GetComponent<CrossSlash>().AssignEnemy(this);
			return new string[1] { "* You used X-SLASH!" };
		}
		return base.PerformAct(i);
	}

	public override void Chat()
	{
		if (hp < maxHp && !heavyBreathing)
		{
			if ((bool)GameObject.Find("EnemyHP2"))
			{
				UnityEngine.Object.DestroyImmediate(GameObject.Find("EnemyHP2"));
			}
			UnityEngine.Object.FindObjectOfType<Ness>().CastHeal();
			Hit(0, -(maxHp - hp), true);
		}
	}

	public override bool IsShaking()
	{
		if ((bool)UnityEngine.Object.FindObjectOfType<Ness>() && UnityEngine.Object.FindObjectOfType<Ness>().IsShaking())
		{
			return true;
		}
		return base.IsShaking();
	}

	public override void SetPredictedDamage(int partyMember, float rawDmg)
	{
		if (partyMember == 0 && CanSpare())
		{
			UnityEngine.Object.FindObjectOfType<Ness>().ProtectPaula();
		}
		base.SetPredictedDamage(partyMember, rawDmg);
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (inNessDeath)
		{
			rawDmg = 0f;
		}
		if (partyMember == 0 && CanSpare() && rawDmg > 0f)
		{
			UnityEngine.Object.FindObjectOfType<Ness>().Hit(partyMember, rawDmg, playSound);
			return;
		}
		if (partyMember > 0 && CanSpare())
		{
			base.Hit(partyMember, 0f, playSound);
			return;
		}
		base.Hit(partyMember, rawDmg, playSound);
		if (heavyBreathing && hp > 0)
		{
			SeparateParts();
		}
		if (hp <= 0)
		{
			UnityEngine.Object.FindObjectOfType<BreathingBG>().SetColor(new Color(0f, 0f, 0f, 0f));
			UnityEngine.Object.FindObjectOfType<BattleManager>().StopMusic();
			UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
			UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
			UnityEngine.Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_kill_0");
			moveBody = 20;
			killed = false;
		}
	}

	public override int GetNextAttack()
	{
		if (hp <= 0)
		{
			return 76;
		}
		if (!startedFirstAttack)
		{
			startedFirstAttack = true;
			return 68;
		}
		if ((float)hp / (float)maxHp <= 0.4f)
		{
			return 75;
		}
		if (curAtk < orderedAttacks.Length - 1)
		{
			curAtk++;
			return orderedAttacks[curAtk];
		}
		return base.GetNextAttack();
	}

	public void ActivateHeavyBreathing()
	{
		inNessDeath = false;
		GetPart("body").GetChild(0).GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_head_phase2");
		GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_torso_pan");
		GetPart("legs").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Paula/spr_b_paula_leg_apart");
		frames = 0;
		heavyBreathing = true;
	}

	public void SetX(float x)
	{
		this.x = x;
	}

	public override string[] PerformAssistAct(int i)
	{
		return new string[1] { "* But she couldn't think\n  of anything to do." };
	}

	public override string GetRandomFlavorText()
	{
		if (!usedFirstText)
		{
			usedFirstText = true;
			return "* Paula attacks.";
		}
		if ((float)hp / (float)maxHp <= 0.1f)
		{
			return dyingTxt[0];
		}
		return flavorTxt[0];
	}

	public void ActivatePhase2()
	{
		inNessDeath = true;
		hpPos = new Vector2(2f, 140f);
		satisfied = 0;
		actNames = new string[3]
		{
			GetBMString("act_check", 0),
			REDBUSTER_NAME,
			DUALHEAL_NAME
		};
		Util.GameManager().SetFlag(172, 2);
		UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(1, "spr_su_down_depressed_0");
		UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(2, "spr_no_down_depressed_0");
		renderSpareBar = false;
	}

	public override bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		if (partyMember == 0 || !renderSpareBar)
		{
			return true;
		}
		return false;
	}
}
