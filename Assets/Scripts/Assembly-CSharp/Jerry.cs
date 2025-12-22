using System.Collections.Generic;
using UnityEngine;

public class Jerry : EnemyBase
{
	private bool playedIntro;

	private bool finaleMode;

	private bool backgroundChange;

	private int backgroundChangeFrames;

	private bool freezeBandana;

	private Sprite[] bandanaSprites = new Sprite[3];

	private int bandanaFrames;

	private int damageDecreaseDiag;

	private int damageDecreaseDiagMatch;

	private int[] mainAttacks = new int[10] { 91, 93, 92, 94, 95, 96, 97, 98, 99, 100 };

	private int[] attackLoop = new int[4] { 101, 102, 103, 104 };

	private int curAtk;

	private float lvNerf = 1f;

	private bool ditchedLastTurn;

	private int ditchCount = -1;

	private bool showGuidance;

	private int guidanceMessage;

	private readonly string[] GUIDANCE_MESSAGES = new string[7] { "* Noelle is confused about\n  Jerry's motivations.", "* A thought stirs within your\n  mind.", "* You and Susie feel like voicing\n  your disapproval.", "* Jerry's logic still perplexes\n  both of you.", "* Noelle feels strongly about\n  something...", "* Susie looks annoyed with\n  Jerry's logic.", "* Seems like Jerry's conflicted." };

	private int[] actPath = new int[11]
	{
		0, 4, 1, 3, 0, 0, 1, 4, 3, 2,
		500
	};

	private int actPosition;

	private int respondedToPosition;

	private int recognizeDamageNerf;

	private int recognizeDamageNerfResponded;

	private int useChatFace = -1;

	private string[][] chatFaces = new string[9][]
	{
		new string[7] { "eyeshut_smug", "smirk", "smirk", "eyeshut_smirk", "smirk_full", "eyeshut", null },
		new string[2] { "rage", "rage" },
		new string[2] { "rage", null },
		new string[4] { null, null, "eyeshut", "eyeshut" },
		new string[2] { "shock", null },
		new string[2] { "eyeshut", null },
		new string[1] { "thinking" },
		new string[2] { "thinking", "eyeshut" },
		new string[3] { "curious", "curious", "eyeshut_smug" }
	};

	private int lastCheatingAmount;

	private int rageAmount;

	private bool rage;

	private int rageFrames;

	private bool ditchedlol;

	private int ditchFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Jerry";
		fileName = "jerry";
		checkDesc = "* Everyone knew Jerry, but\n  no one that knew is alive.";
		maxHp = 2000;
		hp = maxHp;
		atk = 30;
		displayedDef = 30;
		def = 5;
		exp = 1;
		gold = 0;
		susieMiniACTs[0].SetName("Criticize");
		noelleMiniACTs[0].SetName("Inquire");
		hpWidth = 202;
		defaultChatPos = new Vector2(191f, 60f);
		canSpareViaFight = false;
		playerMultiplier = 1.5f;
		lvNerf = (float)(27 - Util.GameManager().GetLV()) / 26f;
		chatter = new string[1] { "" };
		flavorTxt = new string[5] { "* Jerry readies itself.", "* Smells like....^02.^02.^02.^10 Jerry.", "* Jerry.", "* Jerry stands unwavering.^90\n* It definitely needs to use\n  the bathroom right now.", "* Jerry's blade seems to\n  be glaring into you." };
		dyingTxt = new string[1] { "* Jerry is wounded." };
		satisfyTxt = new string[1] { "* Jerry cannot bring itself\n  to swing its sword." };
		actNames = new string[4] { "SN!Ditch", "S!Challenge", "Reflect", "SN!Praise" };
		for (int i = 0; i < 3; i++)
		{
			bandanaSprites[i] = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_headband_end_" + i);
		}
		hurtSpriteName = "_dying_3";
		hostile = true;
		attacks = new int[1] { 90 };
	}

	protected override void Start()
	{
		base.Start();
		xDif = base.transform.position.x - obj.transform.localPosition.x;
		hpPos.x -= Mathf.Round(xDif * 48f);
		new GameObject("PetalGenerator").AddComponent<PetalGenerator>();
		if (!Util.FindObjectOfType<Glyde>())
		{
			playedIntro = true;
			Util.FindObjectOfType<PetalGenerator>().Activate();
			Util.FindObjectOfType<BattleManager>().ActivateSeriousMode();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!freezeBandana)
		{
			bandanaFrames++;
		}
		GetPart("body").Find("headband").GetComponent<SpriteRenderer>().sprite = bandanaSprites[bandanaFrames / 3 % 3];
		if (playedIntro && !backgroundChange)
		{
			backgroundChangeFrames++;
			float t = (float)backgroundChangeFrames / 90f;
			BattleBGPiece[] array = Util.FindObjectsOfType<BattleBGPiece>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(0.1333f, 0.694f, 0.298f), new Color32(17, 87, 145, byte.MaxValue), t);
			}
			if (backgroundChangeFrames >= 90)
			{
				backgroundChange = true;
			}
		}
		if (ditchedlol)
		{
			ditchFrames++;
			SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in componentsInChildren)
			{
				float num = Mathf.Lerp(1f, 2f / 51f, (float)ditchFrames / 30f);
				if (spriteRenderer.gameObject.name == "face")
				{
					spriteRenderer.color = new Color(num, num, num);
				}
				else
				{
					spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, Mathf.Lerp(1f, 2f / 51f, (float)ditchFrames / 30f));
				}
			}
		}
		int bigShotCheating = Util.FindObjectOfType<SOUL>().GetBigShotCheating();
		if (bigShotCheating > 1 && bigShotCheating > lastCheatingAmount)
		{
			rageAmount++;
			if (rageAmount == 6)
			{
				atk = 50;
				rage = true;
				PlaySFX("sounds/snd_carhonk");
			}
		}
		lastCheatingAmount = bigShotCheating;
		if (!rage || satisfied >= 100 || killed)
		{
			return;
		}
		rageFrames++;
		if (rageFrames % 10 == 0)
		{
			int num2 = Random.Range(-60, 60);
			Vector3 position = new Vector3(0f - Mathf.Cos(num2), Mathf.Sin(num2)) * Random.Range(0.5f, 1f) + new Vector3(0f, 1.27f);
			SpriteRenderer component = Object.Instantiate(Resources.Load<GameObject>("vfx/EnemySmoke"), position, Quaternion.identity).GetComponent<SpriteRenderer>();
			component.sortingOrder = Random.Range(30, 32);
			if (component.sortingOrder == 30)
			{
				component.sortingOrder = 19;
			}
		}
	}

	private void LateUpdate()
	{
		if (useChatFace <= -1)
		{
			return;
		}
		if (!GetTextBubble())
		{
			useChatFace = -1;
			if (!finaleMode && !CanSpare())
			{
				SetFace(null);
			}
		}
		else if (GetTextBubble().GetCurrentStringNum() > 0)
		{
			SetFace(chatFaces[useChatFace][GetTextBubble().GetCurrentStringNum() - 1]);
		}
	}

	public override string[] PerformAct(int i)
	{
		if (i == 0)
		{
			return new string[3]
			{
				base.PerformAct(i)[0],
				"* Or so it claims.",
				"* Its WIND-infused katana is\n  its only means of attack."
			};
		}
		if (GetActNames()[i].StartsWith("SN!SoulFinisher"))
		{
			Util.FindObjectOfType<BattleManager>().StopMusic();
			Object.Instantiate(Resources.Load<GameObject>("battle/acts/JerryFinisher"));
			return new string[8]
			{
				"* You called on your friends\n  to help defeat Jerry!",
				"* Susie lends you the power\n  of RUDE magic!",
				((int)Util.GameManager().GetFlag(87) >= 5) ? "su_side`snd_txtsus`* Is it bad that\n  I'm fine with you\n  killing him?" : "su_annoyed`snd_txtsus`* Dude,^05 this guy is\n  really getting on my\n  nerves.",
				((int)Util.GameManager().GetFlag(87) >= 5) ? "su_smile_sweat`snd_txtsus`* Whatever.^05\n* Get this guy off\n  our backs." : "su_smile`snd_txtsus`* Show him who's boss.",
				"* Noelle offers her ICE magic\n  for extra reinforcement!",
				"no_depressed_side`snd_txtnoe`* Kris...",
				"no_depressed`snd_txtnoe`* Be careful.",
				"* Your SOUL is glowing..."
			};
		}
		if (GetActNames()[i].StartsWith("SN!Ditch"))
		{
			ditchCount++;
			ditchedLastTurn = true;
			if (CanSpare())
			{
				Util.GameManager().SetFlag(271, 1);
				spared = true;
				ditchedlol = true;
				return new string[1] { "* You and the other\n  monsters ditch Jerry\n  when it looks away!" };
			}
			return new string[1] { "* You all tried to ditch\n  Jerry,^05 but it appeared\n  everywhere you looked!" };
		}
		if (GetActNames()[i].StartsWith("S!Challenge"))
		{
			if (actPath[actPosition] == 0)
			{
				actPosition++;
				if (respondedToPosition == 0)
				{
					SetGuidanceMessage(0);
					AddActPoints(1);
					return new string[3] { "* You and Susie express your\n  flabbergastment with what\n  just happened.", "su_pissed`snd_txtsus`* What the hell are\n  you doing,^05 dude?????", "su_annoyed`snd_txtsus`* This wasn't your fight\n  to fight." };
				}
				if (respondedToPosition == 4)
				{
					SetGuidanceMessage(3);
					AddActPoints(5);
					return new string[5] { "* You and Susie bash Jerry.", "su_angry`snd_txtsus`* DON'T GET IT???", "su_pissed`snd_txtsus`* Do you know who\n  you're talking to,^05\n  loser???", "su_annoyed`snd_txtsus`* The hell even is\n  your goal?", "su_annoyed`snd_txtsus`* Cuz being called\n  the \"big bully\" is\n  NOT fun." };
				}
				if (respondedToPosition == 5)
				{
					SetGuidanceMessage(1);
					AddActPoints(10);
					return new string[4] { "* You and Susie press Jerry\n  on this point.", "su_confident`snd_txtsus`* Says the one trying\n  to steal enemies.", "su_smile`snd_txtsus`* Is this what it's\n  all about?^05\n* Being a loser?", "su_smile_sweat`snd_txtsus`* Cuz that's...^05 uhh...^05\n* Pretty pathetic." };
				}
			}
			return new string[2] { "* You and Susie tried\n  thinking of something to\n  berate Jerry with...", "* But nothing came to mind." };
		}
		if (GetActNames()[i].StartsWith("Reflect"))
		{
			if (actPath[actPosition] == 1)
			{
				actPosition++;
				if (respondedToPosition == 2)
				{
					SetGuidanceMessage(5);
					AddActPoints(3);
					return new string[2] { "* You wonder why it\n  matters if Jerry is\n  respected.", "* Jerry seems willing to\n  further elaborate." };
				}
				if (respondedToPosition == 6)
				{
					SetGuidanceMessage(4);
					AddActPoints(10);
					return new string[2] { "* You reflect on when Susie\n  saved you from King's wrath.", "* Jerry looks confused." };
				}
			}
			return new string[2] { "* You tried reflecting upon\n  Jerry's actions...", "* But nothing seemed to resonate." };
		}
		if (GetActNames()[i].StartsWith("SN!Praise"))
		{
			if (actPath[actPosition] == 2)
			{
				actPosition++;
				AddActPoints(100);
				SetFace("thinking");
				Util.FindObjectOfType<BattleManager>().StopMusic();
				Util.FindObjectOfType<PetalGenerator>().SetRate(20);
				Unhostile();
				playerMultiplier = 0f;
				displayedDef = 99;
				return new string[2] { "* You all encouraged Jerry\n  to take a new direction\n  with its life.", "* Jerry seems fully convinced." };
			}
			if (!CanSpare())
			{
				return new string[2] { "* You all pretended to\n  praise Jerry's abilities.", "* Jerry saw them as insincere\n  and nothing happened." };
			}
			return new string[1] { "* Jerry's already been convinced." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		switch (i)
		{
		case 1:
			if (actPath[actPosition] == 3 && respondedToPosition == actPosition)
			{
				actPosition++;
				if (respondedToPosition == 3)
				{
					SetGuidanceMessage(2);
					AddActPoints(4);
					SetFace("shock");
					return new string[4] { "* Susie criticized Jerry's\n  mindset.", "su_annoyed`snd_txtsus`* That's so stupid.", "su_side`snd_txtsus`* No one's gonna like\n  you if you're just\n  a big,^05 mean brute.", "* Jerry became ENRAGED!\n^05* Its ATTACK increased!" };
				}
				if (respondedToPosition == 8)
				{
					SetGuidanceMessage(6);
					AddActPoints(15);
					return new string[3] { "su_annoyed`snd_txtsus`* Screw the status quo.", "su_smile`snd_txtsus`* Making a good change\n  on the world actually\n  rocks really hard.", "su_smile_side`snd_txtsus`* Honestly,^05 it'd prolly\n  get you a lot more\n  respect than not." };
				}
			}
			if (!CanSpare())
			{
				return new string[1] { "* Susie calls Jerry stupid.\n* Jerry scoffs at her." };
			}
			return new string[1] { "* Susie almost calls Jerry\n  stupid again,^05 but changes\n  her mind." };
		case 2:
			if (actPath[actPosition] == 4 && (respondedToPosition == 0 || respondedToPosition == actPosition))
			{
				actPosition++;
				if (respondedToPosition == 0)
				{
					SetGuidanceMessage(1);
					AddActPoints(2);
					return new string[3] { "* Noelle inquired about Jerry's\n  motivations.", "no_curious`snd_txtnoe`* This is incredibly\n  extreme,^05 especially for,^05\n  you know...", "no_thinking`snd_txtnoe`* ... Someone that just\n  appeared and did\n  a couple flips...?" };
				}
				if (respondedToPosition == 1)
				{
					SetGuidanceMessage(1);
					AddActPoints(2);
					return new string[4] { "* Noelle inquired about Jerry's\n  motivations.", "no_curious`snd_txtnoe`* So what if he\n  didn't do anything\n  important?", "no_happy`snd_txtnoe`* Shouldn't it be a good\n  thing that you have\n  something like that...", "no_weird`snd_txtnoe`* ... In a place like\n  this?" };
				}
				if (respondedToPosition == 7)
				{
					SetGuidanceMessage(5);
					AddActPoints(10);
					return new string[4] { "* Noelle inquired about Jerry's\n  mindset.", "no_curious`snd_txtnoe`* Everything happens for\n  a reason,^05 right?", "no_thinking`snd_txtnoe`* Why should you keep\n  being another killer\n  among many?", "no_curious`snd_txtnoe`* Won't that just make\n  more killers?" };
				}
			}
			return new string[1] { "* Noelle tried asking about\n  Jerry,^05 but Jerry ignored her." };
		default:
			return base.PerformAssistAct_Old(i);
		}
	}

	public void SetFace(string faceName)
	{
		if (faceName == null)
		{
			GetPart("body").Find("face").GetComponent<SpriteRenderer>().enabled = false;
			return;
		}
		GetPart("body").Find("face").GetComponent<SpriteRenderer>().enabled = true;
		GetPart("body").Find("face").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_face_" + faceName);
	}

	public override void PlaySFX(string sound, float volume = 1f, float pitch = 1f)
	{
		if (!aud.clip || !aud.clip.name.EndsWith("carhonk") || !aud.isPlaying)
		{
			base.PlaySFX(sound, volume, pitch);
		}
	}

	public void SetPose(int i)
	{
		switch (i)
		{
		case 0:
			GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Jerry/spr_b_jerry_stance");
			GetPart("body").GetComponent<SpriteRenderer>().flipX = false;
			GetPart("body").GetComponent<SpriteRenderer>().color = GetPart("body").Find("headband").GetComponent<SpriteRenderer>().color;
			GetPart("body").Find("headband").localPosition = new Vector3(1.2083334f, 1.25f);
			GetPart("body").Find("headband").GetComponent<SpriteRenderer>().sortingOrder = 23;
			GetPart("body").Find("headband").GetComponent<SpriteRenderer>().enabled = true;
			GetPart("sword").localPosition = new Vector3(-0.968f, 1.936f);
			GetPart("sword").eulerAngles = new Vector3(0f, 0f, -60f);
			GetPart("sword").GetComponent<SpriteRenderer>().flipX = false;
			GetPart("sword").GetComponent<SpriteRenderer>().enabled = true;
			break;
		case 1:
			GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("battle/enemies/Jerry/spr_b_jerry_spin_attack")[0];
			GetPart("body").GetComponent<SpriteRenderer>().flipX = false;
			GetPart("body").GetComponent<SpriteRenderer>().color = GetPart("body").Find("headband").GetComponent<SpriteRenderer>().color;
			GetPart("body").Find("headband").localPosition = new Vector3(1.371f, 1.273f);
			GetPart("body").Find("headband").GetComponent<SpriteRenderer>().sortingOrder = 23;
			GetPart("body").Find("headband").GetComponent<SpriteRenderer>().enabled = true;
			GetPart("sword").localPosition = new Vector3(0f, 2.389f);
			GetPart("sword").eulerAngles = new Vector3(0f, 0f, -72.5f);
			GetPart("sword").GetComponent<SpriteRenderer>().flipX = false;
			GetPart("sword").GetComponent<SpriteRenderer>().enabled = true;
			break;
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (ditchedLastTurn)
		{
			ditchedLastTurn = false;
			text = (new string[5][]
			{
				new string[4] { "Trying to escape,^05 \nhuh?", "Not the first time \nthis happened to me.", "I won't let myself \nget ditched again...", "Now fight me,^05 \ncowards!" },
				new string[2] { "My blade is faster \nthan your feet!", "Don't be foolish!" },
				new string[2] { "Just give up \nalready!", "You're wasting your \nlife trying to \nescape!" },
				new string[3] { "What's this point \nyou're trying to \nprove???", "I'm immune to being \nditched!", "Knock it off!" },
				new string[1] { "..." }
			})[(ditchCount <= 4) ? ditchCount : 4];
		}
		if (!finaleMode)
		{
			if (actPosition > respondedToPosition)
			{
				if (respondedToPosition <= 1)
				{
					List<string> list = ((respondedToPosition == 0) ? new List<string> { "Shut up!!!^05\nYou know NOTHING!!!", "That stupid,^05 ego-\nridden asshole \ncontributed NOTHING to \nsociety!", "And yet he's \nconsidered one of the \nremaining highlights \nof modern Snowdin???", "Get outta here with \nthat GARBAGE." } : new List<string> { "No!!!" });
					if (respondedToPosition == 0 && actPosition == 2)
					{
						list.Add("And yes,^05 this is \nVERY necessary!!!");
					}
					if (actPosition == 2)
					{
						list.AddRange(new string[5] { "Why should a useless \npiece of shit get \nrespected by the \nsane people here...", "When he has done \nNOTHING to help us \nescape this prison!!", "Like,^05 I dunno...", "Like I'VE BEEN TRYING \nTO DO?", "Like right now???" });
					}
					text = list.ToArray();
				}
				else if (actPosition == 3)
				{
					text = new string[7] { "And besides...", "I don't know about \nyou,^05 but I'm kinda \na tough guy...?", "I've got a LOT \nof EXP and a HIGH \nLV,^05 so people should \nbe respecting me.", "You know...", "So they don't die.", "...^10 But they don't.", "No one does." };
				}
				else if (actPosition == 4)
				{
					text = new string[2] { "SHUT UP!!!^05\nSHUT YOUR FUCKING \nMOUTH,^05 ASSHOLE!!!", "YOU DON'T GET IT!!!" };
				}
				else if (actPosition == 5)
				{
					text = new string[2] { "Don't call me a \nloser,^05 loser!!!", "I'm fucking tired \nof hearing that!" };
				}
				else if (actPosition == 6)
				{
					text = new string[4] { "Well what am I \nsupposed to do???^05\nKeep being left \nbehind?", "It's either catch-up \nor become part of \nthe dust.", "Or as that one \nskeleton keeps \nsaying...", "\"Kill or be killed.\"" };
				}
				else if (actPosition == 7)
				{
					text = new string[2] { "What...?", "Do you know where \nthat kind of naivety \ncan get you here???" };
				}
				else if (actPosition == 8)
				{
					text = new string[2] { "But...", "But it's always been \nthis way." };
				}
				else if (actPosition == 9)
				{
					text = new string[1] { "I...^05 I don't know." };
				}
				else if (actPosition == 10)
				{
					text = new string[2] { "Well,^05 uhh...", "Gimme a moment..." };
				}
				if (actPosition >= 3)
				{
					useChatFace = actPosition - 3;
				}
				respondedToPosition = actPosition;
			}
			else if (recognizeDamageNerf > recognizeDamageNerfResponded)
			{
				if (recognizeDamageNerf == 1)
				{
					text = new string[1] { "I think you're just \nbad at your job,^05\nheh." };
				}
				else if (recognizeDamageNerf == 2)
				{
					text = new string[1] { "Why don't you just \ntry harder,^05 loser?" };
				}
				else if (recognizeDamageNerf == 3)
				{
					text = new string[6] { "Heheheheh...", "Y'know,^05 I was letting \nyou hurt me on \nPURPOSE!", "I have the natural \nability to harden \nmy body in response \nto trauma!", "Basically...", "You can't hurt me,^05 \nhuman!!!", "Give up and surrender \nyour SOUL!!!" };
				}
				else if (recognizeDamageNerf == 4)
				{
					text = new string[2] { "Not even your \nstrongest attack can \nkill me now!", "Just die already!" };
				}
			}
		}
		recognizeDamageNerfResponded = recognizeDamageNerf;
		pos = defaultChatPos;
		if (finaleMode && speed != 1 && type != "RightSmall")
		{
			text = new string[3] { "Oh,^05 you're approaching \nme?", "You're approaching me \nwith no fear in your \neyes?", "Well then..." };
			useChatFace = 8;
		}
		else if (finaleMode && speed == 1)
		{
			speed = 0;
		}
		base.Chat(text, type, sound, pos, canSkip, speed);
		if (type == "RightSmall")
		{
			chatbox.gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
		}
	}

	public override int GetNextAttack()
	{
		if (finaleMode)
		{
			return 82;
		}
		if (CanSpare())
		{
			return -1;
		}
		curAtk++;
		if (curAtk <= mainAttacks.Length)
		{
			return mainAttacks[curAtk - 1];
		}
		return attackLoop[(curAtk - mainAttacks.Length - 1) % attackLoop.Length];
	}

	public void StartFinale()
	{
		hp = 400;
		finaleMode = true;
	}

	public override void CombineParts()
	{
	}

	public override void SeparateParts()
	{
	}

	public void FinaleDeath()
	{
		hp = 0;
		killed = true;
		gold = 30;
		TurnToDust();
	}

	public int GetDamageValue()
	{
		int num = 9;
		if (hp <= 0)
		{
			return 1;
		}
		if (rage)
		{
			num += 5;
		}
		if (finaleMode)
		{
			return Mathf.RoundToInt((float)num * Mathf.Lerp(1f, 2f / 3f, lvNerf * lvNerf));
		}
		if (actPosition == 4)
		{
			num = Mathf.RoundToInt((float)num * 1.1f);
		}
		return num;
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (CanSpare() && rawDmg > 0f)
		{
			Util.GameManager().SetFlag(269, 1);
			rawDmg = 0f;
		}
		else if (hp <= 400)
		{
			if (recognizeDamageNerf < 3)
			{
				recognizeDamageNerf = 3;
			}
			else if (recognizeDamageNerfResponded >= 3)
			{
				recognizeDamageNerf = recognizeDamageNerfResponded + 1;
			}
			actNames = new string[2]
			{
				GetActNames()[0],
				EnemyBase.MakeSpecialActString("SN", "SoulFinisher", "The last resort", 100)
			};
			renderSpareBar = false;
			playerMultiplier = 0f;
			displayedDef = 99;
		}
		else
		{
			float num = (float)hp / (float)maxHp;
			playerMultiplier = Mathf.Lerp(0.4f, 3f, num * num) * lvNerf;
			MonoBehaviour.print(playerMultiplier);
			if (playerMultiplier / lvNerf <= 1f && damageDecreaseDiag < 1)
			{
				damageDecreaseDiag = 1;
			}
			else if (playerMultiplier / lvNerf <= 0.75f && damageDecreaseDiag < 2)
			{
				damageDecreaseDiag = 2;
			}
			displayedDef = (int)Mathf.Lerp(30f, 60f, num);
		}
		base.Hit(partyMember, rawDmg, playSound);
	}

	public override string GetRandomFlavorText()
	{
		if (!playedIntro)
		{
			Util.FindObjectOfType<PetalGenerator>().Activate();
			Util.FindObjectOfType<BattleManager>().ActivateSeriousMode();
			Util.FindObjectOfType<BattleManager>().JerryFightReorganize();
			playedIntro = true;
			return "* Jerry suddenly attacks!";
		}
		if (recognizeDamageNerf >= 3)
		{
			return "* Jerry has you in check.";
		}
		if (showGuidance)
		{
			showGuidance = false;
			return GUIDANCE_MESSAGES[guidanceMessage];
		}
		if (actPosition == 9)
		{
			return "* Jerry's having second thoughts.";
		}
		if (curAtk == 3)
		{
			if (UTInput.GetKeyOrButtonReplacement("Confirm").Length < 5)
			{
				return "* Press ^Z to shoot when yellow!\n* Hold and release ^Z to fire\n  a BIG SHOT!";
			}
			return "* Press ^Z to shoot when\n  yellow! Hold and release\n  ^Z to fire a BIG SHOT!";
		}
		if (curAtk == 6)
		{
			return "* It has been 0 days since\n  last incident.";
		}
		if (damageDecreaseDiag > damageDecreaseDiagMatch)
		{
			damageDecreaseDiagMatch++;
			recognizeDamageNerf = damageDecreaseDiagMatch;
			return (new string[2] { "su_side`snd_txtsus`* Is it just me,^05 or\n  are our attacks not\n  hitting as hard...?", "su_inquisitive`snd_txtsus`* Okay,^05 something's up.^05\n* This isn't normal." })[damageDecreaseDiagMatch - 1];
		}
		return base.GetRandomFlavorText();
	}

	private void SetGuidanceMessage(int guidanceMessage)
	{
		showGuidance = true;
		this.guidanceMessage = guidanceMessage;
	}

	public override bool IsDone()
	{
		if (!Util.FindObjectOfType<Glyde>() || Util.FindObjectOfType<Glyde>().IsDone())
		{
			return base.IsDone();
		}
		return true;
	}

	public override void Unhostile()
	{
		gotHit = true;
		Color color = GetPart("sword").GetComponent<SpriteRenderer>().color;
		base.Unhostile();
		GetPart("sword").GetComponent<SpriteRenderer>().color = color;
		gotHit = false;
	}

	public override void Spare(bool sleepMist = false)
	{
		base.Spare(sleepMist);
		SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.gameObject.name == "face")
			{
				spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f);
			}
			else
			{
				spriteRenderer.color = new Color(spriteRenderer.color.r, spriteRenderer.color.g, spriteRenderer.color.b, 0.5f);
			}
		}
		freezeBandana = true;
	}

	public override bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		if (CanSpare())
		{
			return false;
		}
		return true;
	}

	public bool InFinale()
	{
		return finaleMode;
	}

	public void ReduceHPFinale()
	{
		hp--;
	}
}
