using System;
using UnityEngine;

public class EnemyBase : MonoBehaviour
{
	public struct MiniACT
	{
		private string name;

		private string description;

		private int id;

		private int tpCost;

		public MiniACT(string name, int id, string description = "", int tpCost = 0)
		{
			this.name = name;
			this.description = description;
			this.id = id;
			this.tpCost = tpCost;
		}

		public void SetName(string name)
		{
			this.name = name;
		}

		public string GetName()
		{
			return name;
		}

		public string GetDescription()
		{
			return description;
		}

		public int GetID()
		{
			return id;
		}

		public int GetTPCost()
		{
			return tpCost;
		}
	}

	public static readonly string SACTION_DEFAULT_NAME = "S-Action";

	public static readonly string NACTION_DEFAULT_NAME = "N-Action";

	public static readonly string CACTION_DEFAULT_NAME = "C-Action";

	public static readonly MiniACT SACTION_DEFAULT = new MiniACT(SACTION_DEFAULT_NAME, 0);

	public static readonly MiniACT NACTION_DEFAULT = new MiniACT(NACTION_DEFAULT_NAME, 0);

	public static readonly MiniACT CACTION_DEFAULT = new MiniACT(CACTION_DEFAULT_NAME, 0);

	protected static readonly string CHECK_NAME = "Check";

	protected string enemyName;

	[SerializeField]
	protected string fileName;

	protected string checkDesc;

	protected string[] actNames;

	protected bool hasSoul;

	protected GameObject obj;

	protected int frames;

	protected bool gotHit;

	protected int hpWidth;

	protected int moveBody;

	protected Vector3 mainPos;

	protected float xDif;

	protected bool revealSOUL;

	protected Vector3 oldSoulPos;

	protected Vector3 newSoulPos;

	protected SOUL enemySOUL;

	protected AudioSource aud;

	protected int minHp;

	protected int hp;

	protected int maxHp;

	protected Vector2 hpPos;

	protected int atk;

	protected int def;

	protected int displayedDef;

	protected bool emptyHPBarWhenZero = true;

	protected float playerMultiplier = 1f;

	protected float[] predictedDmg = new float[7];

	protected int[] buffs;

	protected int satisfied;

	protected bool tired;

	protected bool spared;

	protected bool killed;

	protected bool canSpareViaFight = true;

	protected int exp = 10;

	protected int gold = 10;

	protected bool renderSpareBar = true;

	protected bool canBeSkipped;

	protected string[] flavorTxt;

	protected string[] dyingTxt;

	protected string[] satisfyTxt;

	protected string[] chatter;

	protected TextBubble chatbox;

	protected string defaultChatSize = "RightWide";

	protected Vector2 defaultChatPos = Vector2.zero;

	protected int susieActVal = 50;

	protected int noelleActVal = 50;

	protected int forcedAttack;

	protected int[] attacks;

	protected string hurtSpriteName = "_dmg";

	protected bool useCustomDamageAnimation;

	protected bool hostile;

	protected int hpToUnhostile;

	protected readonly string REDBUSTER_NAME = MakeSpecialActString("S", "Red Buster", "Deals RED Damage", 60);

	protected readonly string DUALHEAL_NAME = MakeSpecialActString("N", "Dual Heal", "Heals Everyone", 50);

	protected string hurtSound = "";

	private bool playHurtSound;

	private int hurtSoundFrames;

	protected MiniACT[] susieMiniACTs = new MiniACT[1] { SACTION_DEFAULT };

	protected MiniACT[] noelleMiniACTs = new MiniACT[1] { NACTION_DEFAULT };

	protected MiniACT[] sansMiniACTs = new MiniACT[1] { CACTION_DEFAULT };

	protected virtual void Awake()
	{
		enemyName = "Enemy";
		checkDesc = "";
		actNames = new string[1] { CHECK_NAME };
		flavorTxt = new string[1] { "* Error errors." };
		satisfyTxt = new string[1] { "* Error is satisfied." };
		dyingTxt = new string[1] { "* Error is dying." };
		chatter = new string[1] { "I am error" };
		fileName = "kris";
		checkDesc = "";
		maxHp = 50;
		hp = maxHp;
		hpPos = new Vector2(0f, 80f);
		minHp = int.MinValue;
		atk = 1;
		def = 1;
		attacks = new int[1];
		hasSoul = true;
		gotHit = false;
		revealSOUL = false;
		forcedAttack = -1;
		hpWidth = 102;
		buffs = new int[2];
		aud = base.gameObject.AddComponent<AudioSource>();
		xDif = 0f;
	}

	protected virtual void Start()
	{
		if (displayedDef == 0 && displayedDef != def)
		{
			displayedDef = def;
		}
		obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/" + enemyName.Replace(".", "") + "/main"), base.transform);
		obj.name = "EnemyObj";
		obj.transform.Find("mainbody").gameObject.AddComponent<ParticleDuplicator>();
		mainPos = obj.transform.localPosition;
		xDif = base.transform.position.x - obj.transform.localPosition.x;
		hpPos.x += Mathf.Round(xDif * 48f);
		string[] array = actNames;
		actNames = new string[6];
		int num = 0;
		if (array[0] != CHECK_NAME)
		{
			actNames[0] = CHECK_NAME;
			num++;
		}
		string[] array2 = array;
		foreach (string text in array2)
		{
			actNames[num] = text;
			num++;
			if (num > 5)
			{
				break;
			}
		}
	}

	protected virtual void Update()
	{
		if (gotHit)
		{
			frames++;
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
				else
				{
					if (hp <= 0)
					{
						TurnToDust();
					}
					else
					{
						SeparateParts();
					}
					gotHit = false;
				}
			}
			if (!useCustomDamageAnimation)
			{
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
			}
		}
		if (!playHurtSound)
		{
			return;
		}
		hurtSoundFrames++;
		if (hurtSoundFrames == 10)
		{
			if (hurtSound.StartsWith("sounds/snd"))
			{
				PlaySFX(hurtSound);
			}
			playHurtSound = false;
			hurtSoundFrames = 0;
		}
	}

	public virtual void Hit(int partyMember, float rawDmg, bool playSound)
	{
		predictedDmg[partyMember] = 0f;
		int num = 0;
		int num2 = hp;
		if (rawDmg > 0f)
		{
			num = CalculateDamage(partyMember, rawDmg);
			if (num <= 0 && playerMultiplier > 0f)
			{
				num = 1;
			}
			hp -= num;
			if (hp <= minHp)
			{
				hp = minHp;
			}
			if (playSound)
			{
				PlaySFX("sounds/snd_damage");
			}
			hurtSoundFrames = 0;
			playHurtSound = true;
			frames = 0;
			gotHit = true;
			string text = hurtSpriteName;
			if (hp <= 0)
			{
				killed = true;
				hp = 0;
			}
			moveBody = -10;
			if (!useCustomDamageAnimation)
			{
				CombineParts();
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + text);
			}
			if (hostile && hp <= hpToUnhostile)
			{
				Unhostile();
			}
		}
		else
		{
			num = (int)rawDmg;
			hp -= num;
			if (hp > num2 && num != 0)
			{
				PlaySFX("sounds/snd_heal");
				if (hp > maxHp)
				{
					hp = maxHp;
				}
			}
		}
		if (!(rawDmg > 0f) || !enemySOUL)
		{
			string text2 = "EnemyHP" + obj.transform.parent.gameObject.name[5];
			if (!GameObject.Find(text2))
			{
				EnemyHPBar component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/EnemyHP"), GameObject.Find("BattleCanvas").transform).GetComponent<EnemyHPBar>();
				component.gameObject.name = "EnemyHP" + obj.transform.parent.gameObject.name[5];
				component.transform.localScale = new Vector2(1f, 1f);
				component.transform.localPosition = hpPos;
				component.StartHP(num, num2, GetMaxHP(), partyMember, hpWidth, mercy: false, emptyHPBarWhenZero);
			}
			else
			{
				GameObject.Find(text2).GetComponent<EnemyHPBar>().StartHP(num, num2, GetMaxHP(), partyMember, mercy: false, emptyHPBarWhenZero);
			}
		}
	}

	public virtual int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		GameManager gameManager = Util.GameManager();
		if (PartyMembers.GetWeapon(partyMember) == -1)
		{
			rawDmg *= 0.75f;
		}
		else if (Items.GetWeaponType(PartyMembers.GetWeapon(partyMember)) == 1 && gameManager.GetLV() < 9)
		{
			rawDmg = ((gameManager.GetLV() >= 6) ? (rawDmg * 0.9f) : ((gameManager.GetLV() < 3) ? (rawDmg * 0.8f) : (rawDmg * 0.85f)));
		}
		float num = (float)(8 + PartyMembers.GetATK(partyMember)) * rawDmg / 8f - (float)def;
		float magic = PartyMembers.GetMagic(partyMember);
		if (forceMagic || (bool)Util.FindObjectOfType<SpecialAttackEffect>())
		{
			if (partyMember == 0 && (forceMagic || (bool)Util.FindObjectOfType<CrossSlash>()))
			{
				num = (float)(8 + PartyMembers.GetATKRaw(partyMember) + 20) * rawDmg / 8f - (float)def;
			}
			else if ((forceMagic || (bool)Util.FindObjectOfType<RudeBusterEffect>()) && partyMember == 1)
			{
				num = (float)(6 + PartyMembers.GetATK(partyMember)) / 2f * 9f + magic * 5f - (float)def;
				if (rawDmg == 40f)
				{
					num += 20f;
				}
				if (PartyMembers.GetWeapon(partyMember) == -1)
				{
					num *= 0.6f;
				}
			}
			else if ((forceMagic || (bool)Util.FindObjectOfType<RedBusterEffect>()) && partyMember == 1)
			{
				num = 90f + (float)(6 + PartyMembers.GetATK(partyMember)) / 2f * 13f + magic * 6f - (float)(def * 2);
				if (rawDmg == 70f)
				{
					num += 40f;
				}
			}
			else if ((forceMagic || (bool)Util.FindObjectOfType<IceShock>()) && partyMember == 2)
			{
				num = 10f + magic * 8f + (float)UnityEngine.Random.Range(-5, 5);
			}
			else if ((forceMagic || (bool)Util.FindObjectOfType<PKFireEffect>() || (bool)Util.FindObjectOfType<PKFreezeEffect>()) && partyMember == 3)
			{
				num = (magic + (float)PartyMembers.GetATK(partyMember)) * rawDmg / 5f;
			}
		}
		float num2 = 1f;
		if (PartyMembers.GetArmor(partyMember) == 33)
		{
			int num3 = PartyMembers.GetMaxHP(partyMember) / 2;
			num2 = ((PartyMembers.GetHP(partyMember) < num3) ? Mathf.Lerp(1.5f, 1f, (float)PartyMembers.GetHP(partyMember) / (float)num3) : Mathf.Lerp(1f, 0.8f, ((float)PartyMembers.GetHP(partyMember) - (float)num3) / (float)num3));
		}
		if (gameManager.IsEasyMode())
		{
			num *= 1.2f;
		}
		return Mathf.RoundToInt(num * playerMultiplier * num2);
	}

	public virtual void Unhostile()
	{
		if (!hostile)
		{
			return;
		}
		PlaySFX("sounds/snd_dust");
		SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			spriteRenderer.color = new Color(1f, 1f, 1f, spriteRenderer.color.a);
		}
		for (int j = 0; j < 10; j++)
		{
			float f = UnityEngine.Random.Range(0f, MathF.PI * 2f);
			Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
			SpareDust component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SpareDust"), base.transform, worldPositionStays: false).GetComponent<SpareDust>();
			component.GetComponent<SpriteRenderer>().color = new Color(1f, 0.5f, 0.5f);
			component.transform.position = GetEnemyObject().transform.Find("atkpos").position + vector * 0.25f;
			component.StartMovement(vector);
		}
		if (!gotHit)
		{
			frames = 0;
			gotHit = true;
			string text = hurtSpriteName;
			moveBody = -10;
			if (!useCustomDamageAnimation)
			{
				CombineParts();
				obj.transform.localPosition = mainPos + new Vector3((float)moveBody / 24f, 0f);
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + text);
			}
		}
		hostile = false;
	}

	public virtual int GetPredictedHP()
	{
		if (IsDone())
		{
			return 0;
		}
		int num = hp;
		for (int i = 0; i < 5; i++)
		{
			if (predictedDmg[i] > 0f)
			{
				int num2 = CalculateDamage(i, predictedDmg[i]);
				if (num2 <= 0)
				{
					num2 = 1;
				}
				num -= num2;
			}
		}
		return num;
	}

	public virtual void SetPredictedDamage(int partyMember, float rawDmg)
	{
		predictedDmg[partyMember] = rawDmg;
	}

	public virtual void CombineParts()
	{
		SpriteRenderer[] componentsInChildren = obj.transform.Find("parts").GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = true;
	}

	public virtual void SeparateParts()
	{
		SpriteRenderer[] componentsInChildren = obj.transform.Find("parts").GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = false;
	}

	public virtual string GetName()
	{
		return enemyName;
	}

	public string[] GetActNames()
	{
		return actNames;
	}

	public virtual string[] PerformAct(int i)
	{
		if (i != 0)
		{
			if (!(GetActNames()[i] == REDBUSTER_NAME))
			{
				if (!(GetActNames()[i] == DUALHEAL_NAME))
				{
					if (!(GetActNames()[i] == "") && GetActNames()[i] != null)
					{
						return new string[1] { "* But nothing happened.\n* So this is an error." };
					}
					return new string[1] { "* But it didn't exist.\n* So this is an error." };
				}
				return new string[2] { "* Your SOUL shined its power\n  onto Noelle!", "* Noelle cast DUAL HEAL!" };
			}
			return new string[2] { "* Your SOUL shined its power\n  onto Susie!", "* Susie cast RED BUSTER!" };
		}
		return new string[1] { $"* {enemyName.ToUpper()} - ATK {atk + GetBuff(0)} DEF {displayedDef + GetBuff(1)}\n{checkDesc}" };
	}

	public virtual string[] PerformAssistAct(int partyMember, int i)
	{
		if (i == 0)
		{
			return PerformAssistAct_Old(partyMember);
		}
		return new string[1] { $"* Mini-ACT {i} from {PartyMembers.GetMemberName(partyMember)}" };
	}

	public virtual string[] PerformAssistAct_Old(int partyMember)
	{
		switch (partyMember)
		{
		case 1:
			if (!spared)
			{
				return new string[1] { "* Susie used S-ACTION!\n* " + enemyName + " liked that!" };
			}
			return new string[1] { "* Susie tried to ACT,\n  but " + enemyName + " was\n  already spared!" };
		case 2:
			if (!spared)
			{
				return new string[1] { "* Noelle used N-ACTION!\n* " + enemyName + " liked that!" };
			}
			return new string[1] { "* Noelle tried to ACT,\n  but " + enemyName + " was\n  already spared!" };
		default:
			return new string[1] { $"* Mini-ACT {0} from {PartyMembers.GetMemberName(partyMember)}" };
		}
	}

	public void AddActPoints(int points)
	{
		satisfied += points;
		float num = 0.8f;
		if (points < 99)
		{
			num = 1f;
		}
		else if (num <= 50f)
		{
			num = 1.2f;
		}
		else if (num <= 25f)
		{
			num = 1.4f;
		}
		if (points <= 0)
		{
			PlaySFX("sounds/snd_awkward");
		}
		else
		{
			PlaySFX("sounds/snd_mercyadd", 1f, num);
		}
		string text = "EnemyMercy" + obj.transform.parent.gameObject.name[5];
		if (!GameObject.Find(text))
		{
			EnemyHPBar component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/EnemyHP"), GameObject.Find("BattleCanvas").transform).GetComponent<EnemyHPBar>();
			component.gameObject.name = text;
			component.transform.localScale = new Vector2(1f, 1f);
			component.transform.localPosition = hpPos;
			component.StartHP(points, satisfied - points, 100, -1, hpWidth, mercy: true);
		}
		else
		{
			GameObject.Find(text).GetComponent<EnemyHPBar>().StartHP(points, satisfied - points, 100, -1, hpWidth, mercy: true);
		}
	}

	public virtual string GetRandomFlavorText()
	{
		if ((float)hp / (float)maxHp <= 0.2f)
		{
			int num = UnityEngine.Random.Range(0, dyingTxt.Length);
			return dyingTxt[num];
		}
		if (satisfied >= 100)
		{
			int num = UnityEngine.Random.Range(0, satisfyTxt.Length);
			return satisfyTxt[num];
		}
		if (flavorTxt.Length != 0)
		{
			int num = UnityEngine.Random.Range(0, flavorTxt.Length);
			return flavorTxt[num];
		}
		return "* But there was no\n  flavor text.\n* Error!!";
	}

	public virtual bool IsShaking()
	{
		return gotHit;
	}

	public virtual string GetChatter()
	{
		return chatter[UnityEngine.Random.Range(0, chatter.Length)];
	}

	public virtual void PlaySFX(string sound, float volume = 1f, float pitch = 1f)
	{
		aud.clip = Resources.Load<AudioClip>(sound);
		aud.Play();
		aud.volume = volume;
		aud.pitch = pitch;
	}

	public virtual void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (text.Length != 1 || !(text[0] == ""))
		{
			chatbox = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/bubble/Speech" + type), GameObject.Find("BattleCanvas").transform).GetComponent<TextBubble>();
			chatbox.transform.localScale = new Vector2(1f, 1f);
			chatbox.transform.localPosition = pos;
			chatbox.CreateBubble(text, 0, sound, speed, canSkip);
		}
	}

	public virtual void Chat(string[] text, string sound, bool canSkip, int speed)
	{
		Chat(text, defaultChatSize, sound, defaultChatPos, canSkip, speed);
	}

	public virtual void Chat()
	{
		Chat(new string[1] { GetChatter() }, defaultChatSize, "snd_text", defaultChatPos, canSkip: true, 0);
	}

	public GameObject GetEnemyObject()
	{
		return obj;
	}

	public virtual bool IsTalking()
	{
		return chatbox != null;
	}

	public void SetHP(int hp)
	{
		this.hp = hp;
	}

	public int GetHP()
	{
		return hp;
	}

	public virtual int GetMaxHP()
	{
		return maxHp;
	}

	public virtual int GetNextAttack()
	{
		if (hp <= 0)
		{
			return -1;
		}
		if (forcedAttack > -1)
		{
			return forcedAttack;
		}
		return attacks[UnityEngine.Random.Range(0, attacks.Length)];
	}

	public virtual bool[] GetTargets()
	{
		return new bool[3]
		{
			true,
			Util.GameManager().SusieInParty(),
			Util.GameManager().NoelleInParty()
		};
	}

	public TextBubble GetTextBubble()
	{
		return chatbox;
	}

	public Transform GetPart(string obje)
	{
		return obj.transform.Find("parts").Find(obje);
	}

	public virtual void TurnToDust()
	{
		PlaySFX("sounds/snd_dust");
		CombineParts();
		obj.transform.Find("mainbody").GetComponent<ParticleDuplicator>().Activate();
		killed = true;
	}

	public virtual void EnemyTurnStart()
	{
	}

	public virtual void EnemyTurnEnd()
	{
	}

	public virtual void AttemptedSpare()
	{
	}

	public virtual void Spare(bool sleepMist = false)
	{
		if (!spared)
		{
			CombineParts();
			SpriteRenderer component = obj.transform.Find("mainbody").GetComponent<SpriteRenderer>();
			component.color = new Color(component.color.r, component.color.g, component.color.b, 0.5f);
			component.sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + hurtSpriteName);
			for (int i = 0; i < 10; i++)
			{
				float f = UnityEngine.Random.Range(0f, MathF.PI * 2f);
				Vector3 vector = new Vector3(Mathf.Cos(f), Mathf.Sin(f));
				SpareDust component2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SpareDust"), base.transform, worldPositionStays: false).GetComponent<SpareDust>();
				component2.transform.position = GetEnemyObject().transform.Find("atkpos").position + vector * 0.25f;
				component2.StartMovement(vector);
			}
			spared = true;
			PlaySFX("sounds/snd_dust");
		}
	}

	protected void SetBuff(int i, int amount)
	{
		buffs[i] = amount;
	}

	public int GetBuff(int i)
	{
		return buffs[i];
	}

	public virtual bool IsDone()
	{
		if (!spared)
		{
			return killed;
		}
		return true;
	}

	public virtual bool IsKilled()
	{
		return killed;
	}

	public virtual bool IsSpared()
	{
		return spared;
	}

	public virtual bool CanSpare()
	{
		if (satisfied < 100)
		{
			if ((float)hp / (float)maxHp <= 0.2f)
			{
				return canSpareViaFight;
			}
			return false;
		}
		return true;
	}

	public virtual bool IsTired()
	{
		return tired;
	}

	public int GetSatisfactionLevel()
	{
		return satisfied;
	}

	public int GetFinalEXP()
	{
		if (spared)
		{
			return 0;
		}
		return exp;
	}

	public int GetGold()
	{
		return gold;
	}

	public bool RenderSpareBar()
	{
		return renderSpareBar;
	}

	public virtual bool PartyMemberAcceptAttack(int partyMember, int attackType)
	{
		return true;
	}

	protected static string MakeSpecialActString(string partyMembers, string name, string description = "", int tpCost = -1)
	{
		if (description != "")
		{
			if (tpCost != -1)
			{
				return partyMembers + "!" + name + ";" + description + "`" + tpCost;
			}
			return partyMembers + "!" + name + ";" + description;
		}
		return partyMembers + "!" + name;
	}

	public MiniACT[] GetMiniACTs(int partyMember)
	{
		return partyMember switch
		{
			1 => susieMiniACTs, 
			2 => noelleMiniACTs, 
			5 => sansMiniACTs, 
			_ => new MiniACT[1]
			{
				new MiniACT("Mini-ACT", 0, "You shouldn't see this...")
			}, 
		};
	}

	public bool HasMiniACT(int partyMember, int i)
	{
		MiniACT[] miniACTs = GetMiniACTs(partyMember);
		foreach (MiniACT miniACT in miniACTs)
		{
			if (miniACT.GetID() == i)
			{
				return true;
			}
		}
		return false;
	}

	public virtual int GetCardCount()
	{
		return -1;
	}

	public virtual UnoPlayer GetPlayer()
	{
		return null;
	}

	public bool CanBeSkipped()
	{
		return canBeSkipped;
	}
}
