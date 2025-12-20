using System.Collections.Generic;
using UnityEngine;

public class Flowey : EnemyBase
{
	private int bodyFrames;

	private int headDif = 1;

	private bool initialFlavor;

	private bool animateBody = true;

	private int curAttack = -1;

	private int[] orderedAttacks = new int[8] { 23, 24, 25, 26, 28, 24, 27, 29 };

	private Vector3 headOffsetEnd = Vector3.zero;

	private Vector3 headOffset = Vector3.zero;

	private int spareAttempts;

	private int spareAttemptsSpoken;

	private int spareAttemptsFinale;

	private int spareAttemptsSpokenFinale;

	private bool check;

	private bool doneChatting = true;

	private bool inFinale;

	private bool finaleKilled;

	private bool dodge;

	private bool hardmode;

	private bool krisFalling;

	private int krisFallingFrames;

	protected override void Awake()
	{
		base.Awake();
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		enemyName = "Flowey";
		fileName = "flowey";
		checkDesc = "* He believes this world is\n  KILL or BE KILLED.";
		if (hardmode && (int)Util.GameManager().GetFlag(13) == 3)
		{
			checkDesc = "* My best friend.";
		}
		maxHp = 900;
		if (!hardmode)
		{
			maxHp += 100;
		}
		hp = maxHp;
		hpPos = new Vector2(0f, 175f);
		atk = 30;
		def = 1;
		playerMultiplier = (hardmode ? 1.35f : 1.25f);
		flavorTxt = new string[6] { "* It smells like a vine\n  garden.", "* Flowey flashes a wicked\n  grin.", "* The vines are wriggling.", "* The earth is rumbling.", "* Flowey cackles to himself.", "* Smells like a rotting\n  bouquet." };
		dyingTxt = new string[1] { "* Flowey is withering." };
		chatter = new string[1] { "" };
		actNames = new string[1] { REDBUSTER_NAME };
		if (hardmode)
		{
			actNames = new string[1] { EnemyBase.MakeSpecialActString("KS", "Red Buster", "Deals RED Damage", 60) };
		}
		canSpareViaFight = false;
		renderSpareBar = false;
		emptyHPBarWhenZero = false;
		hpWidth = 200;
		attacks = new int[7] { 23, 24, 25, 26, 27, 28, 29 };
		if (hardmode)
		{
			attacks = new int[7] { 23, 24, 25, 26, 27, 49, 29 };
			orderedAttacks = new int[7] { 23, 24, 25, 26, 29, 27, 49 };
		}
		exp = 150;
		gold = 50;
		defaultChatPos = new Vector2(182f, 126f);
		defaultChatSize = "RightWide";
	}

	protected override void Update()
	{
		if (hp > 0)
		{
			Vector3 vector = Vector3.zero;
			headOffset = Vector3.Lerp(headOffset, headOffsetEnd, 0.5f);
			if (gotHit)
			{
				moveBody--;
				vector = new Vector3(Random.Range(0, 3) - 1, Random.Range(0, 3) - 1) * ((float)moveBody / 96f);
				if (moveBody == 0)
				{
					gotHit = false;
					SetFace("evil");
				}
			}
			if (animateBody)
			{
				bodyFrames++;
				float num = (float)bodyFrames / 30f;
				if (bodyFrames > 30)
				{
					num = (float)(60 - bodyFrames) / 30f;
				}
				num = num * num * (3f - 2f * num);
				GetPart("stem").transform.localScale = new Vector3(1f, Mathf.Lerp(1f, 0.9f, num), 1f);
				GetPart("head").transform.localPosition = Vector3.Lerp(new Vector3(-0.32f, 2.72f), new Vector3(-0.32f - 0.1f * (float)headDif, 2.45f), num) + vector + headOffset;
				if (bodyFrames == 60)
				{
					bodyFrames = 0;
					headDif *= -1;
				}
			}
		}
		else
		{
			if (gotHit)
			{
				frames++;
				int moveBody2 = moveBody;
				int num2 = (finaleKilled ? 6 : 4);
				if (frames % num2 == 0)
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
				if (frames == 60 && !finaleKilled)
				{
					gotHit = false;
				}
				if (finaleKilled)
				{
					int num3 = (frames - 90) / 6 % 4;
					if (frames > 90 && frames < 109)
					{
						SetFace("final_blow_" + num3);
					}
					if (frames == 130)
					{
						gotHit = false;
					}
				}
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
			}
			if (dodge)
			{
				GetPart("stem").transform.localScale = new Vector3(1f, Mathf.Lerp(GetPart("stem").transform.localScale.y, 0.3717188f, 0.25f), 1f);
				GetPart("head").transform.localPosition = Vector3.Lerp(GetPart("head").transform.localPosition, new Vector3(-0.78f, 0.95f), 0.25f);
			}
		}
		if ((bool)chatbox && !check && !finaleKilled && !doneChatting && spareAttemptsFinale < 10)
		{
			Dictionary<int, string> dictionary = new Dictionary<int, string>();
			if (spareAttemptsSpokenFinale == 1)
			{
				dictionary.Add(2, "sad_dying");
				dictionary.Add(3, "mad_dying");
				dictionary.Add(4, "side_dying");
				dictionary.Add(5, "sassy_dying");
			}
			else if (spareAttemptsSpokenFinale == 3)
			{
				dictionary.Add(1, "mad_dying");
			}
			else if (spareAttemptsSpokenFinale == 4)
			{
				dictionary.Add(1, "rage_dying");
			}
			else if (spareAttemptsSpokenFinale == 5)
			{
				dictionary.Add(1, "hanging");
			}
			else if (spareAttemptsSpokenFinale == 7)
			{
				dictionary.Add(1, "sad_dying");
				dictionary.Add(3, "reminice_dying");
			}
			else if (spareAttemptsSpokenFinale == 8)
			{
				dictionary.Add(1, "sad2_dying");
				dictionary.Add(3, "earnest_dying");
			}
			else if (spareAttemptsSpokenFinale == 9)
			{
				dictionary.Add(1, "reminice_dying");
				dictionary.Add(2, "sad2_dying");
				dictionary.Add(3, "hanging");
			}
			else if (inFinale && spareAttemptsSpokenFinale == 0)
			{
				if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) == 3)
				{
					if (hardmode)
					{
						dictionary.Add(1, "hanging");
						dictionary.Add(2, "neutral_dying");
						dictionary.Add(3, "side_dying");
						dictionary.Add(4, "evil_dying");
						dictionary.Add(5, "grin_dying");
						dictionary.Add(7, "sad_dying");
						dictionary.Add(10, "neutral_dying");
					}
					else
					{
						dictionary.Add(1, "hanging");
						dictionary.Add(2, "neutral_dying");
						dictionary.Add(3, "side_dying");
						dictionary.Add(4, "sassy_dying");
						dictionary.Add(5, "grin_dying");
						dictionary.Add(7, "earnest_dying");
						dictionary.Add(8, "evil_dying");
						dictionary.Add(10, "sad_dying");
						dictionary.Add(13, "neutral_dying");
					}
				}
				else
				{
					dictionary.Add(1, "hanging");
					dictionary.Add(2, "neutral_dying");
					dictionary.Add(3, "side_dying");
					dictionary.Add(5, "neutral_dying");
					dictionary.Add(6, "grin_dying");
					dictionary.Add(8, "sad_dying");
					dictionary.Add(10, "neutral_dying");
				}
			}
			else if (!inFinale)
			{
				if (spareAttemptsSpoken == 1)
				{
					dictionary.Add(1, "side");
					dictionary.Add(2, "laugh_0");
					dictionary.Add(3, "evil");
					dictionary.Add(4, "laugh_0");
				}
				else if (spareAttemptsSpoken == 2)
				{
					dictionary.Add(1, "sassy");
					dictionary.Add(2, "toriel");
					dictionary.Add(3, "laugh_0");
					if (chatbox.GetCurrentStringNum() == 1)
					{
						SetFace("sassy");
					}
					if (chatbox.GetCurrentStringNum() == 2)
					{
						SetFace("toriel");
					}
					if (chatbox.GetCurrentStringNum() == 3)
					{
						SetFace("laugh_0");
					}
				}
				else if (spareAttemptsSpoken == 3)
				{
					dictionary.Add(1, "mad");
					dictionary.Add(3, "laugh_0");
				}
			}
			if (dictionary.ContainsKey(chatbox.GetCurrentStringNum()))
			{
				SetFace(dictionary[chatbox.GetCurrentStringNum()]);
			}
		}
		else if (!doneChatting && !chatbox && !finaleKilled && spareAttemptsFinale < 10)
		{
			check = false;
			doneChatting = true;
			if (spareAttemptsFinale < 3)
			{
				if (inFinale)
				{
					SetFace("hanging");
				}
				else
				{
					SetFace("evil");
				}
			}
		}
		if (krisFalling)
		{
			krisFallingFrames++;
			GetPart("kris").transform.localPosition = new Vector3(2.9f, Mathf.Lerp(5.25f, 0.17f, (float)krisFallingFrames / 10f));
			if (krisFallingFrames == 10)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
			}
			int num4 = krisFallingFrames / 2 - 4;
			if (num4 > 3)
			{
				num4 = 3;
			}
			else if (num4 < 0)
			{
				num4 = 0;
			}
			if (!GetPart("kris").GetComponent<SpriteRenderer>().sprite.name.EndsWith(num4.ToString()))
			{
				GetPart("kris").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Flowey/spr_b_flowey_kris_fall_" + num4);
			}
		}
	}

	public void TriggerKrisFalling()
	{
		GetPart("kris").GetComponent<SpriteRenderer>().enabled = true;
		krisFalling = true;
	}

	public bool KrisDoneFalling()
	{
		if (krisFalling)
		{
			return krisFallingFrames > 30;
		}
		return false;
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Check")
		{
			check = true;
			return new string[1] { "* FLOWEY - LV 99\n" + checkDesc };
		}
		if (GetActNames()[i] == "KS!Red Buster;Deals RED Damage`60")
		{
			return new string[1] { "* Fuck you" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		return new string[1] { "* But she couldn't think\n  of anything to do." };
	}

	public override int GetPredictedHP()
	{
		if (inFinale)
		{
			return 1;
		}
		return base.GetPredictedHP();
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if (dodge)
		{
			rawDmg = 0f;
		}
		predictedDmg[partyMember] = 0f;
		int num = 0;
		int num2 = hp;
		if (rawDmg > 0f)
		{
			num = CalculateDamage(partyMember, rawDmg);
			if (num <= 0)
			{
				num = 1;
			}
			hp -= num;
			if (playSound)
			{
				aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
				aud.Play();
			}
			frames = 0;
			gotHit = true;
			if (hp <= 0)
			{
				GetPart("stem").transform.localScale = new Vector3(1f, 1f, 1f);
				GetPart("head").transform.localPosition = new Vector3(-0.34f, 2.72f);
				Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(0);
				Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
				Object.FindObjectOfType<BattleManager>().StopMusic();
				GetPart("vineLeft").GetComponent<Animator>().SetFloat("speed", 0f);
				GetPart("vineRight").GetComponent<Animator>().SetFloat("speed", 0f);
				SetFace("evil_hit_dying");
				if (inFinale)
				{
					finaleKilled = true;
					SetFace("final_blow_0");
				}
				hp = 0;
				moveBody = 10;
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
			}
			else
			{
				SetFace("evil_hit");
				moveBody = ((rawDmg > 30f) ? 30 : ((int)rawDmg));
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
		if ((!(rawDmg > 0f) || !(enemySOUL != null)) && (!finaleKilled || dodge))
		{
			string text = "EnemyHP" + obj.transform.parent.gameObject.name[5];
			if (!GameObject.Find(text))
			{
				EnemyHPBar component = Object.Instantiate(Resources.Load<GameObject>("battle/enemies/EnemyHP"), GameObject.Find("BattleCanvas").transform).GetComponent<EnemyHPBar>();
				component.gameObject.name = "EnemyHP" + obj.transform.parent.gameObject.name[5];
				component.transform.localScale = new Vector2(1f, 1f);
				component.transform.localPosition = hpPos;
				component.StartHP(num, num2, maxHp, partyMember, hpWidth, false, emptyHPBarWhenZero);
			}
			else
			{
				GameObject.Find(text).GetComponent<EnemyHPBar>().StartHP(num, num2, maxHp, partyMember, false, emptyHPBarWhenZero);
			}
		}
	}

	public void SetFace(string faceName)
	{
		if (!GetPart("head").GetComponent<SpriteRenderer>().sprite.name.EndsWith(faceName))
		{
			GetPart("head").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Flowey/spr_b_flowey_head_" + faceName);
		}
	}

	public override void Chat()
	{
		if (finaleKilled)
		{
			if (!hardmode)
			{
				Chat(new string[3] { "I knew it...!", "You're just like \nthem!", "Chara..." }, defaultChatSize, "snd_txtflw2", defaultChatPos, true, 2);
			}
			else if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) == 3)
			{
				Chat(new string[4]
				{
					(spareAttemptsFinale > 0) ? "Your sudden twist \nagainst me..." : "That lack of \nremorse...",
					"...^15 Frisk...",
					"You played your best \ntrick yet...!",
					"H^04a^04.^04.^04.^30 h^04a^04.^04.^04."
				}, defaultChatSize, "snd_txtflw2", defaultChatPos, true, 2);
			}
			else
			{
				Chat(new string[1] { "I knew you had \nit in you!" }, defaultChatSize, "snd_txtflw2", defaultChatPos, true, 2);
			}
			chatbox.gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
		}
		else if (spareAttemptsFinale > spareAttemptsSpokenFinale)
		{
			check = false;
			doneChatting = false;
			string[] array = (new string[10][]
			{
				new string[6] { "...", "... What...?", "You're seriously \ntrying THIS?", "SPARING me...?", "You can't possibly \nbe serious.", "Come on,^05 kill \nme already." },
				new string[1] { "Kill me." },
				new string[1] { "Kill me!" },
				new string[1] { "KILL ME!" },
				new string[1] { "..." },
				new string[1] { "...^15 ha...^10 Ha..." },
				new string[4] { "You know,^05 I...", "When I overheard \neverything.", "That in another \nworld,^05 SHE has \nchildren.", "I figured it all \nout." },
				new string[3]
				{
					"That maybe,^05 if I \ntook your SOUL...",
					hardmode ? "I could explore \nother worlds." : "I could explore \nother worlds in \nyour place.",
					"I could see THEM \nagain."
				},
				new string[3]
				{
					hardmode ? "... You remind me \na lot of them,^05 \nyou know." : "... You look so \nmuch like them,^05 \nyou know.",
					hardmode ? "That's how I figured..." : "That's how I figured,^10 \nyour brother...",
					"..."
				},
				new string[1] { "..." }
			})[spareAttemptsSpokenFinale];
			if (spareAttemptsSpokenFinale == 0 && spareAttempts > 0)
			{
				array[4] = "I already told you \nthat I won't accept \nyour mercy.";
			}
			if (spareAttemptsSpokenFinale == 0 && (int)Object.FindObjectOfType<GameManager>().GetFlag(13) == 3)
			{
				array[2] = "Are you like... \nACTUALLY braindead?";
				array[4] = "Didn't you murder \nall the monsters \nin the RUINS?";
			}
			spareAttemptsSpokenFinale++;
			Chat(array, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
		}
		else if (!inFinale && hp <= 0)
		{
			GetPart("vineLeft").GetComponent<Animator>().Play("Disappear");
			GetPart("vineRight").GetComponent<Animator>().Play("Disappear");
			doneChatting = false;
			actNames[1] = "";
			check = false;
			Object.FindObjectOfType<BattleManager>().ForceSoloKris();
			Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_f_wind", 1f, true);
			inFinale = true;
			if (Object.FindObjectOfType<GameManager>().GetHP(0) < 1)
			{
				Object.FindObjectOfType<GameManager>().SetHP(0, 1);
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_heal");
			}
			if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) == 3)
			{
				if (!hardmode)
				{
					Chat(new string[14]
					{
						"Hee hee hee...", "Well done,^05 Kris!", "You sure are no \npushover.", "All that power \nyou gained from \nmerciless slaughter...", "Through sheer will \nand DETERMINATION...", "And look where \nit's gotten you!", "Though...^10 it hasn't \nall been just you, \nhas it?", "After all,^05 it takes \nMAGIC to kill a \nghost.", "I was never any \nmatch against you \ntwo!", "...",
						"So...^05 Kris.", "It's time to finish \nwhat you started.", "Do it.", "Finish me off."
					}, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
					checkDesc = "^15* Your best friend.";
				}
				else
				{
					Chat(new string[10]
					{
						"Hee hee hee...",
						"Well done,^05 Frisk!",
						"While the IDIOT in \nmy grasp here \nnever fought back...",
						"I know that only \nYOU would have the \nguts to fight back!",
						(Util.GameManager().GetWeapon(0) == 27) ? "And given the stolen \nknife in your hands,^05\nyou were DEAD serious!" : "Hahaha...",
						"...",
						"So...^05 Frisk.",
						"It's time to finish \nwhat you started.",
						"Do it.",
						"Finish me off."
					}, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
				}
			}
			else if (!hardmode)
			{
				Chat(new string[11]
				{
					"Hee hee hee...", "Well done,^05 Kris!", "You're clearly not \nthe pushover I \nthought you were.", "I wouldn't have gone \ndown THAT easily if \nI fought anyone else.", "But you're different.", "Why,^05 through sheer \nwill and \nDETERMINATION...", "You were able to \nwither me down!", "So...^05 Kris.", "It's time to make \nyour final move.", "Do it.",
					"Finish me off."
				}, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
			}
			else
			{
				Chat(new string[11]
				{
					"Hee hee hee...", "Well done!", "You're clearly not \nthe pushover I \nthought you were.", "Especially not like \nthe IDIOT in my grasp, \nwho never fought back.", "But you're different.", "Why,^05 through sheer \nwill and \nDETERMINATION...", "You were able to \nwither me down!", "So...^05 human.", "It's time to make \nyour final move.", "Do it.",
					"Finish me off."
				}, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
			}
		}
		else if (inFinale)
		{
			if (spareAttemptsFinale > 0)
			{
				Chat(new string[1] { "..." }, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
			}
			else
			{
				Chat(new string[1] { "Come on,^10 finish me \noff." }, defaultChatSize, "snd_txtflw", defaultChatPos, true, 0);
			}
		}
		else if (spareAttempts > spareAttemptsSpoken)
		{
			check = false;
			doneChatting = false;
			string[] text = (new string[3][]
			{
				new string[4] { "You're trying to \nspare me?", "HAHAHAHA!!!\nThat's rich!", "This world is KILL \nor BE KILLED!", "I will NEVER accept \nyour mercy!" },
				new string[3] { "Do you honestly think \nI'll just let you \ngo like that?", "I'm not HER!", "You're a real idiot \nif you think I'm \nsparing you." },
				new string[3] { "Is this a joke?\nAre you braindead?", "I'm NEVER going to \naccept your MERCY!", "Now SHUT UP and DIE!" }
			})[spareAttemptsSpoken];
			spareAttemptsSpoken++;
			Chat(text, defaultChatSize, "snd_txtflw2", defaultChatPos, true, 0);
		}
		else if (check)
		{
			doneChatting = false;
			Chat(new string[1] { "Like I'm gonna let \nyou see MY stats!" }, defaultChatSize, "snd_txtflw2", defaultChatPos, true, 0);
		}
	}

	public override void AttemptedSpare()
	{
		if (spareAttempts < 3 && spareAttempts == spareAttemptsSpoken)
		{
			spareAttempts++;
		}
	}

	public override string GetRandomFlavorText()
	{
		if (!initialFlavor)
		{
			initialFlavor = true;
			return "* FLOWEY attacks!";
		}
		if (inFinale)
		{
			return "* ...";
		}
		return base.GetRandomFlavorText();
	}

	public override void Spare(bool sleepMist = false)
	{
		if (inFinale)
		{
			spareAttemptsFinale++;
		}
	}

	public override bool CanSpare()
	{
		return inFinale;
	}

	public override int GetNextAttack()
	{
		if (finaleKilled)
		{
			return 30;
		}
		if (spareAttemptsSpokenFinale == 10)
		{
			return 31;
		}
		if (hp <= 0)
		{
			return -1;
		}
		if (curAttack != orderedAttacks.Length - 1)
		{
			curAttack++;
			return orderedAttacks[curAttack];
		}
		return base.GetNextAttack();
	}

	public void SetHeadOffset(Vector3 headOffset)
	{
		headOffsetEnd = headOffset;
	}

	public void EnableDodge()
	{
		frames = 0;
		dodge = true;
	}
}
