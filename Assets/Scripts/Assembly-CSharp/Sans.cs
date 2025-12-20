using System;
using UnityEngine;

public class Sans : EnemyBase
{
	private readonly bool SKIP_INTRO;

	private int distractedLv;

	private int distractTurns;

	private bool xdistractOnce;

	private bool wasFullyDistracted;

	private bool ignoreDistract;

	private bool resetFaceAfterHit;

	private int talked;

	private bool dodging;

	private int dodgeFrames;

	private bool fireBurning = true;

	private int fireFrames;

	private Sprite[] fireSprites = new Sprite[4];

	private bool doingBreatheAnimation;

	private int breatheFrames;

	private bool introText;

	private bool doingChatFaces;

	private int defaultFaceType;

	private int defaultFaceFrames;

	private int lowHpRanting = -1;

	private int curAtk;

	private int[] mainAttacks = new int[7] { 109, 110, 111, 112, 113, 114, 126 };

	private int[] attackLoop = new int[8] { 115, 109, 128, 116, 117, 118, 119, 125 };

	private bool frozen;

	private bool wasFrozen;

	private bool disableFaceControl;

	private bool inFinale;

	private bool wasSleepMisted;

	private bool wasIceShocked;

	private bool deathCore;

	private int deathCoreLevel;

	private bool pacifist;

	private bool karmaVer;

	private int pTalked;

	private int pTalkedProgress;

	private int bumpFrames;

	private bool bump;

	private MenuBone menuBone;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Sans";
		fileName = "sans";
		checkDesc = "* A once simple skeleton\n  gone mad.";
		maxHp = Mathf.RoundToInt(Mathf.Lerp(1450f, 1950f, (float)(Util.GameManager().GetLV() - 1) / 12f));
		hp = maxHp;
		hpPos = new Vector2(2f, 140f);
		hpWidth = 200;
		minHp = 1;
		atk = 40;
		def = 1;
		flavorTxt = new string[5] { "* Sans gazes into your SOUL.", "* Sans flashes a demented\n  smile.", "* Sans chuckles under his\n  breath.", "* It smells of burning charcoal.", "* The bones swirl around you." };
		satisfyTxt = flavorTxt;
		dyingTxt = new string[4] { "* Sans's face dementedly\n  contorts.", "* Sans is acting erratic.", "* Sans looks like he's about\n  to collapse.", "* The burning smell seems to\n  be waning." };
		actNames = new string[5] { "Talk", "Distract", "SN!XDistract", REDBUSTER_NAME, DUALHEAL_NAME };
		susieMiniACTs[0].SetName("Distract");
		noelleMiniACTs[0].SetName("Distract");
		canSpareViaFight = false;
		renderSpareBar = false;
		defaultChatSize = "RightSmall";
		exp = 450;
		gold = 350;
		attacks = new int[1] { -1 };
		defaultChatPos = new Vector2(180f, 84f);
		defaultChatSize = "RightWide";
		for (int i = 0; i < 4; i++)
		{
			fireSprites[i] = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_hoodflame_" + i);
		}
		base.gameObject.AddComponent<SansGravityManager>();
		MonoBehaviour.print("LV " + Util.GameManager().GetLV());
		playerMultiplier = Mathf.Lerp(1f, 0.8f, (float)(Util.GameManager().GetLV() - 1) / 12f);
		if (Util.GameManager().GetLV() >= 7)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/KarmaHandler"), Util.FindObjectOfType<PartyPanels>().transform);
			karmaVer = true;
		}
		else if (Util.GameManager().GetEXP() == 0)
		{
			pacifist = true;
		}
		menuBone = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/MenuBone")).GetComponent<MenuBone>();
	}

	protected override void Start()
	{
		base.Start();
		if (SKIP_INTRO)
		{
			UnityEngine.Object.Destroy(Util.FindObjectOfType<SansIntroAttack>().gameObject);
			SetFace("empty_down");
			ResetBreatheAnimation();
			Util.FindObjectOfType<PartyPanels>().transform.position = Vector3.zero;
			Util.FindObjectOfType<SansBG>().FadeIn();
			Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_vsufsans", 1f, hasIntro: true);
			Distract(4);
			StartFinale();
			hp = Mathf.RoundToInt((float)maxHp * 0.1f);
			Util.FindObjectOfType<SansBG>().FadeOut();
			Util.FindObjectOfType<BattleManager>().StopMusic();
			curAtk = 2;
		}
	}

	protected override void Update()
	{
		if (frozen)
		{
			return;
		}
		if (!inFinale || deathCore || pTalked >= 4)
		{
			base.Update();
		}
		else if (gotHit)
		{
			frames++;
			int num = 4;
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
			if (frames >= 60)
			{
				gotHit = false;
			}
			obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
		}
		if (!inFinale && (float)hp / (float)maxHp <= 0.5f && Util.FindObjectOfType<BattleManager>().GetState() < 3)
		{
			float t = ((float)hp / (float)maxHp - 0.25f) / 0.25f;
			menuBone.Activate(Mathf.Lerp(8f, 2f, t) / 48f);
		}
		else
		{
			menuBone.Deactivate();
		}
		if (!gotHit && deathCore && deathCoreLevel == 5)
		{
			deathCoreLevel = 6;
			TurnToDust();
			killed = true;
		}
		if (!gotHit && resetFaceAfterHit)
		{
			ResetToDefaultFace();
			resetFaceAfterHit = false;
		}
		if (!gotHit && !doingChatFaces && defaultFaceType > 0 && !disableFaceControl)
		{
			defaultFaceFrames++;
			if (defaultFaceType == 1)
			{
				int num2 = defaultFaceFrames % 50 / 2;
				SetFace((num2 == 5 || num2 == 10) ? "empty_twitch" : "empty_down");
			}
			else if (defaultFaceType == 2)
			{
				int num3 = defaultFaceFrames % 50 / 2;
				SetFace((num3 == 5 || num3 == 10) ? "losingit_twitch" : "losingit");
			}
		}
		if (distractedLv == 0 && !dodging && !gotHit && !inFinale)
		{
			bool flag = false;
			PlayerAttackAnimation[] array = Util.FindObjectsOfType<PlayerAttackAnimation>();
			if (array != null && array.Length != 0)
			{
				PlayerAttackAnimation[] array2 = array;
				for (int i = 0; i < array2.Length; i++)
				{
					if (array2[i].IsPlaying())
					{
						flag = true;
						break;
					}
				}
			}
			if (flag || ((bool)Util.FindObjectOfType<SpecialAttackEffect>() && Util.FindObjectOfType<SpecialAttackEffect>().IsPlaying()))
			{
				dodging = true;
				dodgeFrames = 0;
			}
		}
		if (dodging)
		{
			dodgeFrames++;
			float num4 = (float)dodgeFrames / 15f;
			if (dodgeFrames > 45)
			{
				num4 = 1f - (float)(dodgeFrames - 45) / 15f;
			}
			if (num4 < 1f)
			{
				num4 = Mathf.Sin(num4 * MathF.PI * 0.5f);
			}
			obj.transform.Find("parts").localPosition = new Vector3(Mathf.Lerp(0f, -2f, num4), 0f);
			playerMultiplier = ((dodgeFrames <= 5 || dodgeFrames >= 55) ? 1 : 0);
			if (dodgeFrames >= 60)
			{
				dodging = false;
			}
		}
		if (fireBurning)
		{
			fireFrames++;
			GetPart("body").Find("flame").GetComponent<SpriteRenderer>().sprite = fireSprites[fireFrames / 5 % 4];
		}
		if (doingBreatheAnimation)
		{
			breatheFrames++;
			float num5 = (0f - Mathf.Cos((float)breatheFrames * 3.75f * (MathF.PI / 180f)) + 1f) / 2f;
			if (inFinale)
			{
				num5 = (0f - Mathf.Cos((float)(breatheFrames * 2) * (MathF.PI / 180f)) + 1f) / 2f;
			}
			GetPart("body").localPosition = new Vector3(inFinale ? 0f : (-1f / 12f), (inFinale ? 1.25f : 1.5f) - num5 * 2f / 24f);
			GetPart("body").Find("head").localPosition = new Vector3(0f, (inFinale ? 0.75f : 0.875f) - num5 * 2f / 24f);
		}
		if (bump)
		{
			bumpFrames++;
			base.transform.position = new Vector3((float)((4 - bumpFrames) * ((bumpFrames % 2 != 0) ? 1 : (-1))) / 48f, 0f);
			if (bumpFrames >= 4)
			{
				bump = false;
				bumpFrames = 0;
			}
		}
	}

	private void LateUpdate()
	{
		if (!doingChatFaces)
		{
			return;
		}
		if ((bool)GetTextBubble())
		{
			if (lowHpRanting == 0)
			{
				if (GetTextBubble().GetCurrentStringNum() == 1)
				{
					SetFace("closed_down");
				}
			}
			else if (lowHpRanting == 1)
			{
				if (GetTextBubble().GetCurrentStringNum() == 1)
				{
					SetFace("closed_down");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 2)
				{
					SetFace("glare");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 3)
				{
					SetFace("empty_down");
				}
			}
			else if (lowHpRanting == 2)
			{
				if (GetTextBubble().GetCurrentStringNum() == 1)
				{
					SetFace("closed_down");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 3)
				{
					SetFace("losingit");
				}
			}
			else if (lowHpRanting == 3)
			{
				if (GetTextBubble().GetCurrentStringNum() == 1)
				{
					SetFace("closed_down");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 2)
				{
					SetFace("losingit");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 3)
				{
					SetFace("insane");
				}
			}
			else if (lowHpRanting == 4)
			{
				if (GetTextBubble().GetCurrentStringNum() == 1)
				{
					SetFace("losingit");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 2)
				{
					SetFace("goinginsane_twitch");
				}
			}
			if (curAtk == 4)
			{
				if (GetTextBubble().GetCurrentStringNum() == 1)
				{
					SetFace("closed_down");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 3)
				{
					SetFace("glare");
				}
				else if (GetTextBubble().GetCurrentStringNum() == 4)
				{
					SetFace("empty_down");
				}
			}
		}
		else
		{
			ResetToDefaultFace();
			doingChatFaces = false;
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i].EndsWith("Talk"))
		{
			if (tired)
			{
				if (pacifist)
				{
					actNames[1] = "SN!Talk";
					Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: true, susie: false, noelle: false);
					pTalked++;
					if (pTalked != 1)
					{
						if (pTalked == 2)
						{
							actNames[2] = "";
							actNames[3] = "";
							actNames[4] = "";
							actNames[5] = "";
							return new string[1] { "* You question why Sans hates\n  you so much." };
						}
						if (pTalked != 3)
						{
							return new string[1] { "* You ask if he truly killed her." };
						}
						return new string[1] { "* You ask what actually happened\n  after he attacked this world's\n  Susie." };
					}
					return new string[1] { (talked >= 3) ? "* You ask again why Sans is\n  doing all of this." : "* You ask why Sans is doing\n  all of this." };
				}
				return new string[1] { (Util.GameManager().GetFlagInt(87) >= 10) ? "* It isn't worth it." : "* But you couldn't think of\n  anything to say." };
			}
			if (lowHpRanting <= -1)
			{
				talked++;
				if (talked != 1)
				{
					if (talked != 2)
					{
						if (talked != 3)
						{
							if (talked != 4)
							{
								if (talked != 5)
								{
									return new string[1] { pacifist ? "* Seems like talking won't\n  do any good for now." : "* Seems like talking won't\n  do any good." };
								}
								return new string[2] { "* You try to think of something\n  else to say,^05 but...", "* As hard as you try,^05 you\n  can't seem to be able to\n  de-escalate the situation." };
							}
							return new string[2] { "ufsans_empty`snd_txtsans`* quit talking to me.", "ufsans_sadistic`snd_txtsans`* just lie down and make\n  this easy for me." };
						}
						return new string[2] { "ufsans_side`snd_txtsans`* why the hell would i\n  tell you why i'm doing\n  this?", "ufsans_closed`snd_txtsans`* i'd be giving you an\n  opportunity to make things\n  worse." };
					}
					return new string[2] { "ufsans_side`snd_txtsans`* oh,^05 I'M being\n  unreasonable?", "ufsans_empty`snd_txtsans`* the only reasonable thing\n  i can think of doing is\n  pulverizing you." };
				}
				return new string[4]
				{
					"* You try to talk to Sans.",
					"ufsans_closed`snd_txtsans`* you wanna talk?",
					"ufsans_side`snd_txtsans`* really now.",
					(Util.GameManager().GetFlagInt(87) >= 5) ? "ufsans_sadistic`snd_txtsans`* you're really gonna go back\n  on that mass murder thing\n  with a mass murderer?" : "ufsans_empty`snd_txtsans`* what makes you think i'd\n  be willing to talk to\n  the likes of you?"
				};
			}
			return new string[1] { (talked >= 5) ? "* If Sans was unwilling to talk\n  before,^05 now would be a worse\n  time." : "* It seems like Sans is too\n  aggressive to even respond." };
		}
		if (GetActNames()[i] == "Distract")
		{
			if (!tired)
			{
				string text = (new string[3] { "* You call Sans a smiley\n  trashbag.", "* You try to get behind Sans\n  for another backstab.", "* You tell Sans that Papyrus\n  is nearby!" })[UnityEngine.Random.Range(0, 3)];
				string text2 = Distract();
				return new string[2] { text, text2 };
			}
			return new string[1] { "* Looks like Sans is too\n  distracted to be distracted." };
		}
		if (GetActNames()[i] == "SN!XDistract")
		{
			if (!tired)
			{
				string text3 = (xdistractOnce ? "* Everyone continues being\n  distracting!" : "* You, Susie, and Noelle distract\n  Sans by being very obnoxious!");
				xdistractOnce = true;
				string text4 = Distract(pacifist ? 4 : 3);
				return new string[2] { text3, text4 };
			}
			return new string[1] { "* Looks like Sans is too\n  distracted to be distracted." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		if (!tired)
		{
			string text = "* K-ACTION DISTRACT????";
			switch (i)
			{
			case 1:
				text = (new string[3] { "* Susie throws the Heavy Branch!\n* Then picks it back up.", "* Susie mocks Sans for failing\n  to kill her.", "* Susie quickly breaks down a\n  nearby tree!" })[UnityEngine.Random.Range(0, 3)];
				break;
			case 2:
				text = (new string[2] { "* Noelle swirls icy wind around\n  Sans.\n* Sans seems nervous.", "* Noelle quickly builds a snow\n  sculpture and destroys it\n  loudly!" })[UnityEngine.Random.Range(0, 2)];
				break;
			}
			string text2 = Distract();
			return new string[2] { text, text2 };
		}
		return new string[1] { "* Looks like Sans is too\n  distracted to be distracted." };
	}

	public string Distract(int times = 1)
	{
		for (int i = 0; i < times; i++)
		{
			if (distractedLv == 0)
			{
				SetSweat(0);
				distractedLv = 1;
				if (defaultFaceType == 0)
				{
					SetFace("distracted");
				}
				continue;
			}
			wasFullyDistracted = true;
			if (distractedLv == 1)
			{
				distractTurns = 1;
			}
			distractedLv = 2;
			if (distractTurns < ((Util.GameManager().GetLV() >= 7) ? 2 : 3))
			{
				distractTurns++;
			}
			ignoreDistract = true;
			SetSweat(distractTurns - 1);
		}
		if (distractedLv == 1)
		{
			return "* Sans can barely concentrate.";
		}
		if (distractedLv == 2)
		{
			return "* Sans can't seem to concentrate!\n* Lasts " + distractTurns + ((distractTurns == 1) ? " turn." : " turns.");
		}
		return "* Nothing happened.";
	}

	public override void EnemyTurnEnd()
	{
		base.EnemyTurnEnd();
		if (ignoreDistract)
		{
			ignoreDistract = false;
		}
		else if (distractedLv == 2 && !inFinale)
		{
			distractTurns--;
			SetSweat(distractTurns - 1);
			if (distractTurns == 0)
			{
				distractedLv = 0;
				ResetToDefaultFace();
				PlaySFX("sounds/snd_sans_refocus");
			}
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (deathCore)
		{
			if (deathCoreLevel >= 5)
			{
				return;
			}
			base.Hit(partyMember, rawDmg, playSound);
			if ((bool)Util.FindObjectOfType<EnemyHPBar>())
			{
				UnityEngine.Object.Destroy(Util.FindObjectOfType<EnemyHPBar>().gameObject);
			}
			if (deathCoreLevel < 5)
			{
				base.CombineParts();
				string text = "";
				if (GameManager.GetOptions().contentSetting.value == 1)
				{
					text = "_r";
				}
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_death_" + deathCoreLevel + text);
			}
			deathCoreLevel++;
			return;
		}
		if (inFinale)
		{
			if (hp <= 0)
			{
				return;
			}
			if ((bool)Util.FindObjectOfType<IceShock>())
			{
				playerMultiplier = 666f;
			}
			else
			{
				playerMultiplier = 420f;
			}
		}
		if (dodging && (playerMultiplier == 1f || ((bool)Util.FindObjectOfType<KSliceAnimation>() && Util.FindObjectOfType<KSliceAnimation>().DoingBigSwing())))
		{
			obj.transform.Find("parts").localPosition = Vector3.zero;
			dodging = false;
			playerMultiplier = 1f;
		}
		else if (playerMultiplier == 0f)
		{
			rawDmg = 0f;
		}
		base.Hit(partyMember, rawDmg, playSound);
		if (gotHit)
		{
			if (defaultFaceType < 2)
			{
				SetFace("dmg");
			}
			if ((float)hp / (float)maxHp <= 0.5f)
			{
				defaultFaceType = 1;
			}
			if ((float)hp / (float)maxHp <= 0.25f)
			{
				defaultFaceType = 2;
			}
			resetFaceAfterHit = true;
		}
		if (!inFinale && killed)
		{
			killed = false;
			hp = 1;
		}
		if (inFinale && killed)
		{
			killed = false;
			StopBreatheAnimation();
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
			Util.FindObjectOfType<BattleManager>().ForceNoSpare();
			Util.FindObjectOfType<BattleManager>().ForceNoFight();
			if ((bool)Util.FindObjectOfType<IceShock>())
			{
				wasIceShocked = true;
				SetSweat(-1);
				SetFace("shocked");
				GetPart("body").Find("head").localPosition = new Vector3(0f, 0.75f);
			}
			else
			{
				SetFace("shocked_cracked");
				GetPart("body").Find("head").localPosition = new Vector3(0f, 0.875f);
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso_sliced");
			}
			GetPart("body").localPosition = new Vector3(0f, 1.25f);
			resetFaceAfterHit = false;
		}
		if (distractedLv == 1 && gotHit && !inFinale)
		{
			SetSweat(-1);
			distractedLv = 0;
		}
	}

	public override int GetNextAttack()
	{
		if (pTalked >= 4)
		{
			return 129;
		}
		if (hp <= 0)
		{
			if (!wasIceShocked)
			{
				return 122;
			}
			return 123;
		}
		if (wasSleepMisted)
		{
			if (Util.GameManager().GetFlagInt(87) < 10)
			{
				return 121;
			}
			return 124;
		}
		if (tired)
		{
			return -1;
		}
		if ((float)hp / (float)maxHp <= 0.1f)
		{
			return 120;
		}
		curAtk++;
		int num = attackLoop[Mathf.Abs(curAtk - mainAttacks.Length - 1) % attackLoop.Length];
		if (curAtk <= mainAttacks.Length)
		{
			num = mainAttacks[curAtk - 1];
		}
		if (karmaVer)
		{
			switch (num)
			{
			case 126:
				num = 125;
				break;
			case 125:
				num = 127;
				break;
			}
		}
		return num;
	}

	public void SetFace(string suffix)
	{
		GetPart("body").Find("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_head_" + suffix);
	}

	public void ResetToDefaultFace()
	{
		if (defaultFaceType == 0)
		{
			SetFace((distractedLv == 0) ? "empty_down" : "distracted");
		}
		else if (defaultFaceType == 1)
		{
			SetFace("empty_down");
		}
		else if (defaultFaceType == 2)
		{
			SetFace("losingit");
		}
	}

	public void SetSweat(int i)
	{
		GetPart("body").Find("head").GetChild(0).GetComponent<SpriteRenderer>()
			.sprite = ((i == -1) ? null : Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_sweat_" + i));
	}

	public override string GetRandomFlavorText()
	{
		if (!introText)
		{
			introText = true;
			return "* You feel like you're going to\n  have a bad time.";
		}
		if (pTalked > 0)
		{
			return "* ...";
		}
		if (tired)
		{
			return "* Sans is too tired to fight.";
		}
		if ((float)hp / (float)maxHp <= 0.15f)
		{
			return "* Sans looks like he's about\n  to lose it...!";
		}
		if (wasFullyDistracted && distractedLv == 0)
		{
			wasFullyDistracted = false;
			return "* Sans regains his focus.";
		}
		return base.GetRandomFlavorText();
	}

	public void SetFreeze(bool frozen)
	{
		this.frozen = frozen;
		if (frozen)
		{
			wasFrozen = true;
		}
	}

	public void StartFinale()
	{
		ResetBreatheAnimation();
		GetPart("body").localPosition = new Vector3(0f, 1.25f);
		GetPart("body").Find("head").localPosition = new Vector3(0f, 0.75f);
		GetPart("legs").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_legs_sit");
		fireBurning = false;
		inFinale = true;
		minHp = int.MinValue;
		disableFaceControl = true;
		tired = true;
		PlaySFX("sounds/snd_noise");
		GetPart("body").Find("flame").GetComponent<SpriteRenderer>().sprite = null;
		SetFace("tired");
	}

	public override void Chat()
	{
		if (wasSleepMisted || hp <= 0)
		{
			return;
		}
		if (tired)
		{
			if (pTalked > pTalkedProgress)
			{
				pTalkedProgress = pTalked;
				if (pTalked == 1)
				{
					Chat(new string[4] { "*huff*^15 really...?", "you really think...^10 \ni'll just leave \nmyself open like \nthat?", "giving you another \nchance to screw me \nover?", "i'm not talking." }, "snd_txtsans", canSkip: true, 0);
				}
				else if (pTalked == 2)
				{
					Chat(new string[5] { "why...^15 why the hell \ndo YOU care?", "you really wanna \nget chummy with \nme?^10\nespecially NOW?", "after i tried killing \nyou multiple times \nand admitted to \nkilling an orphan?", "and especially after \nall that i've been \nthrough.", "you're nuts." }, "snd_txtsans", canSkip: true, 0);
				}
				else if (pTalked == 3)
				{
					Chat(new string[6] { "...", "w-^10well...", "...", "i...^15 already said.", "she's one with \nthe wind.", "b-^10but i ran away \nbefore i could see \nher die." }, "snd_txtsans", canSkip: true, 0);
				}
			}
			else
			{
				Chat(new string[1] { (UnityEngine.Random.Range(0, 2) == 0) ? "..." : "*huff*...^15 *puff*..." }, "snd_txtsans", canSkip: true, 0);
			}
		}
		else if ((float)hp / (float)maxHp <= 0.1f)
		{
			Chat(new string[3] { "you...^10 you really \nnever give up,^05 do \nyou?", "i'm on my last legs \nhere,^05 kris.", "don't say i didn't \nwarn you." }, "snd_txtsans", canSkip: true, 0);
		}
		else if ((float)hp / (float)maxHp <= 0.5f)
		{
			doingChatFaces = true;
			lowHpRanting++;
			if (lowHpRanting == 0)
			{
				Chat(new string[3] { "i've been waiting \nfor this moment for \na long time.", "to finally take \nrevenge on humanity,^05 \nand rid this world \nof its problems.", "forever." }, "snd_txtsans", canSkip: true, 0);
			}
			else if (lowHpRanting == 1)
			{
				Chat(new string[3] { "but yet...^10\nout of all potential \ntimelines...", "it had to be against \nyou three?", "what an insulting \nfate." }, "snd_txtsans", canSkip: true, 0);
			}
			else if (lowHpRanting == 2)
			{
				Chat(new string[4] { "and the worst part?", "the look on your \nface.^10\nhow confused you \nseem.", "you're COMPLETELY \noblivious to what \nyou did to me.", "it's like the \nuniverse wants to \nsee me lose it." }, "snd_txtsans", canSkip: true, 0);
			}
			else if (lowHpRanting == 3)
			{
				Chat(new string[3] { "and,^05 y'know....", "if you keep \nresisting...", "then i WILL lose it." }, "snd_txtsans", canSkip: true, 0);
			}
			else if (lowHpRanting == 4)
			{
				Chat(new string[2] { "just make things \neasier for both of \nus...", "and let me rip that \nsoul out of your \ncold,^05 dead body." }, "snd_txtsans", canSkip: true, 0);
			}
		}
		else if (curAtk == 2)
		{
			Chat(new string[3] { "you and your damn \n\"determination\".", "all living things \nlose the will to live \nat some point.", "for you, that point \nis NOW." }, "snd_txtsans", canSkip: true, 0);
		}
		else if (curAtk == 3)
		{
			doingChatFaces = true;
			if (wasFrozen)
			{
				Chat(new string[4] { "mhmm...", "all of you \nlooked shocked,^05 and \nyet you barely \ndodged it.", "you didn't know \nhow to avoid the \npitfall,^05 did you?", "what the hell \nkind of power \ndo you have?" }, "snd_txtsans", canSkip: true, 0);
			}
			else
			{
				Chat(new string[4] { "of course,^05 of \ncourse.", "you knew exactly \nhow to avoid such \nan unavoidable \npitfall.", "almost like you're \nplaying god or \nsomething.", "all you humans \ndeserve to die at \nthe hands of \nmonsterkind." }, "snd_txtsans", canSkip: true, 0);
			}
		}
	}

	public override void Chat(string[] text, string sound, bool canSkip, int speed)
	{
		base.Chat(text, sound, canSkip, speed);
		if ((bool)chatbox)
		{
			chatbox.gameObject.AddComponent<ShakingText>();
		}
	}

	public override void CombineParts()
	{
	}

	public override void SeparateParts()
	{
	}

	public override bool IsShaking()
	{
		bool flag = false;
		if (dodging && (bool)Util.FindObjectOfType<FightTarget>())
		{
			flag = dodging && Util.FindObjectOfType<FightTarget>().IsGoing();
		}
		return base.IsShaking() || flag;
	}

	public override void Spare(bool sleepMist = false)
	{
		if (inFinale && sleepMist)
		{
			wasSleepMisted = true;
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
			Util.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
			Util.FindObjectOfType<BattleManager>().ForceNoSpare();
			Util.FindObjectOfType<BattleManager>().ForceNoFight();
			SetFace("shocked");
		}
	}

	public void ResetBreatheAnimation()
	{
		GetComponent<SansGravityManager>().StopPlaying();
		breatheFrames = 0;
		doingBreatheAnimation = true;
		GetPart("body").localPosition = new Vector3(-1f / 12f, 1.5f);
		GetPart("body").Find("head").localPosition = new Vector3(0f, 0.875f);
		GetPart("body").Find("flame").localPosition = new Vector3(1f / 24f, 7f / 24f);
	}

	public void StartDeathCore()
	{
		deathCore = true;
	}

	public void FreezeDeath()
	{
		base.CombineParts();
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_freeze");
		killed = true;
	}

	public void Bump()
	{
		PlaySFX("sounds/snd_swallow");
		bump = true;
		bumpFrames = 0;
	}

	public override void Unhostile()
	{
		hostile = true;
		bump = false;
		base.transform.position = Vector3.zero;
		base.Unhostile();
	}

	public void ForceCombineParts()
	{
		base.CombineParts();
	}

	public void StopBreatheAnimation()
	{
		doingBreatheAnimation = false;
	}

	public void DisableFaceControl()
	{
		disableFaceControl = true;
	}

	public string GetDistractedText()
	{
		if (distractedLv == 0)
		{
			return "";
		}
		if (distractedLv == 1)
		{
			return "(Unfocused)";
		}
		return "(Distracted)";
	}

	public override int GetMaxHP()
	{
		return base.GetMaxHP() + 480;
	}
}
