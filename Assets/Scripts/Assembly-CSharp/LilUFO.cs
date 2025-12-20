using System;
using UnityEngine;

public class LilUFO : EnemyBase
{
	private int bodyFrames;

	private bool checkingForSlow;

	private Vector3 basePos;

	private bool isDying;

	private int dyingFrames;

	private bool laserRod;

	private bool canMove = true;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Li'l UFO";
		fileName = "ufo";
		checkDesc = "* This tiny spacecraft is\n  anxious about finally landing.";
		maxHp = 280;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 12;
		def = 9;
		flavorTxt = new string[4] { "* Li'l UFO is shifting all\n  over the place.", "* Otherworldly whirrs fill the\n  sky.^10\n* ...Or just where Li'l UFO is.", "* Smells metallic.", "* Li'l UFO seems a bit stressed\n  over the battle." };
		dyingTxt = new string[1] { "* Li'l UFO is smoking." };
		satisfyTxt = new string[1] { "* Li'l UFO feels relaxed." };
		chatter = new string[4] { "I'm \njust a \nli'l \nUFO", "Can I \nland \nsome\nwhere?", "The \nsky is \ntoo \nbusy", "Hello \nearth-\nlings" };
		actNames = new string[2] { "Encourage", "N!Construct;`32" };
		defaultChatSize = "RightSmall";
		exp = 15;
		gold = 9;
		attacks = new int[1] { 32 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 132, 119f);
		basePos = GetEnemyObject().transform.position;
		if (base.transform.position.x >= 3.5f)
		{
			defaultChatPos.x = 265f;
		}
	}

	public override string[] PerformAct(int i)
	{
		checkingForSlow = false;
		if (GetActNames()[i] == "Encourage")
		{
			checkingForSlow = true;
			return new string[2] { "* You told Li'l UFO that you'll\n  move slowly during the next\n  attack.", "* Li'l UFO then shot you with\n  a movement-slowing beam...!" };
		}
		if (GetActNames()[i] == "N!Construct;`32")
		{
			if (satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[1] { "* You and Noelle constructed an\n  landing pad made of ice.\n* Li'l UFO seems satisfied." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (spared)
		{
			return base.PerformAssistAct(i);
		}
		switch (i)
		{
		case 1:
			tired = true;
			return new string[1] { "* Susie chased Li'l UFO around\n  the battlefield.\n* Li'l UFO became TIRED." };
		case 2:
			if (satisfied < 100)
			{
				AddActPoints(50);
			}
			return new string[1] { "* Noelle smiled at Li'l UFO.\n* It felt less anxious." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (checkingForSlow)
		{
			text = new string[1] { "Please \nmove \nslowly" };
		}
		else if (satisfied >= 100)
		{
			text = new string[1] { "I can \nfinally \nland" };
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	protected override void Update()
	{
		if (!gotHit && !isDying)
		{
			if (canMove)
			{
				bodyFrames++;
				if (bodyFrames >= 60)
				{
					bodyFrames = 0;
				}
			}
			GetPart("body").transform.parent.localPosition = new Vector3(Mathf.Sin((float)(6 * bodyFrames) * ((float)Math.PI / 180f)) * 0.4f, Mathf.Sin((float)(12 * bodyFrames) * ((float)Math.PI / 180f)) * 0.4f);
			if (laserRod)
			{
				GetPart("rod").transform.localPosition = Vector3.Lerp(GetPart("rod").transform.localPosition, Vector3.zero, 0.5f);
			}
			else
			{
				GetPart("rod").transform.localPosition = Vector3.Lerp(GetPart("rod").transform.localPosition, new Vector3(0f, 0.8f), 0.5f);
			}
		}
		else if (isDying)
		{
			dyingFrames++;
			float x = basePos.x + Mathf.Sin((float)(dyingFrames * 18) * ((float)Math.PI / 180f)) / 4f;
			float num = (float)dyingFrames / 40f;
			float y = Mathf.Lerp(basePos.y, 0.49f, num * num);
			GetEnemyObject().transform.position = new Vector3(x, y);
			GetEnemyObject().transform.eulerAngles += new Vector3(0f, 0f, 0.5f);
			if (dyingFrames % 2 == 0)
			{
				SpriteRenderer component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemySmoke"), new Vector3(UnityEngine.Random.Range(-0.5f, 0.5f), UnityEngine.Random.Range(-0.5f, 0.5f)) + GetEnemyObject().transform.position, Quaternion.identity).GetComponent<SpriteRenderer>();
				component.sortingOrder = UnityEngine.Random.Range(23, 27);
				if (component.sortingOrder == 25)
				{
					component.sortingOrder++;
				}
			}
			if (dyingFrames == 40)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyExplosion")).transform.position += new Vector3(x, 0f);
				UnityEngine.Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeIn(10, Color.white);
				UnityEngine.Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = false;
				isDying = false;
			}
		}
		base.Update();
	}

	public override void EnemyTurnEnd()
	{
		UncheckForSlow();
	}

	public void ExposeLaser()
	{
		laserRod = true;
		aud.clip = Resources.Load<AudioClip>("sounds/snd_spearappear");
		aud.Play();
	}

	public void UnexposeLaser()
	{
		laserRod = false;
	}

	public bool LaserExposed()
	{
		return laserRod;
	}

	public void SetMovement(bool canMove)
	{
		this.canMove = canMove;
		if (canMove)
		{
			GetPart("rod").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Li'l UFO/spr_b_ufo_laser_rod");
		}
		else
		{
			GetPart("rod").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Li'l UFO/spr_b_ufo_laser_rod_shoot");
		}
	}

	public override void TurnToDust()
	{
		CombineParts();
		isDying = true;
	}

	public void UncheckForSlow()
	{
		checkingForSlow = false;
	}

	public bool IsCheckingForSlow()
	{
		return checkingForSlow;
	}
}
