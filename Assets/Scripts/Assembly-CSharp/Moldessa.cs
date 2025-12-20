using System;
using UnityEngine;

public class Moldessa : EnemyBase
{
	private Sprite[] faceSprites = new Sprite[10];

	private bool susieBiteAttempt;

	private bool down = true;

	private int bounceFrames;

	private bool faceSpin = true;

	private int faceFrames;

	private int switchCount;

	private bool sleptOnce;

	private bool pieceRegrow;

	private int pieceFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Moldessa";
		fileName = "moldessa";
		checkDesc = "* Can't decide on a face.\n* Can't see friend from foe.";
		maxHp = 155;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 6;
		def = 2;
		flavorTxt = new string[4] { "* Moldessa's face rearranges\n  itself.", "* Moldessa tries to make its\n  face look like a movie-\n  star's,^05 but it shifts apart.", "* Moldessa hides behind its ears.^15\n* ... arms?", "* Smells like twelve-week-old\n  gummy bears." };
		dyingTxt = new string[1] { "* Moldessa's face falls apart." };
		satisfyTxt = new string[1] { "* Moldessa's face looks... happy?" };
		chatter = new string[4] { "(slime \nsounds)", "Shh...", "Hsh...", "Krr..." };
		actNames = new string[3] { "Switch", "Fix", "Lie Down" };
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 4;
		gold = 3;
		attacks = new int[1] { 10 };
		for (int i = 0; i < 10; i++)
		{
			string text = i.ToString();
			if (i == 9)
			{
				text = "unused";
			}
			faceSprites[i] = Resources.Load<Sprite>("battle/enemies/Moldessa/spr_b_moldessa_face_" + text);
		}
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
		SwitchAct(owo: false);
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		if (faceSpin)
		{
			faceFrames++;
			GetPart("hair").transform.Find("face").transform.eulerAngles = new Vector3(0f, 0f, (float)faceFrames * 3.6f);
			for (int i = 0; i < 3; i++)
			{
				GetPart("hair").transform.Find("face").GetChild(i).localEulerAngles = new Vector3(0f, 0f, (float)(-faceFrames) * 0.6f);
			}
		}
		if (pieceRegrow)
		{
			pieceFrames++;
			GetPart("hair").transform.Find("face").GetChild(0).localScale = Vector3.Lerp(Vector3.zero, new Vector3(1f, 1f), (float)(pieceFrames - 45) / 600f);
		}
		bounceFrames++;
		float t = (0f - Mathf.Cos((float)(bounceFrames * 8) * (MathF.PI / 180f)) + 1f) / 2f;
		GetPart("hair").transform.localPosition = new Vector3(0f, Mathf.Lerp(2.042f, 2.121f, t));
		GetPart("body").transform.localScale = Vector3.Lerp(new Vector3(1f, 1f), new Vector3(0.9375f, 1.0625f), t);
		GetPart("left").transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, -15f, t));
		GetPart("left").transform.localPosition = new Vector3(GetPart("left").transform.localPosition.x, Mathf.Lerp(2.446f, 2.413f, t));
		GetPart("right").transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, 15f, t));
		GetPart("right").transform.localPosition = new Vector3(GetPart("right").transform.localPosition.x, Mathf.Lerp(2.446f, 2.413f, t));
	}

	private void SwitchAct(bool owo)
	{
		for (int i = 0; i < 3; i++)
		{
			if (owo)
			{
				if (i == 2)
				{
					GetPart("hair").transform.Find("face").GetChild(i).GetComponent<SpriteRenderer>()
						.sprite = faceSprites[9];
				}
				else
				{
					GetPart("hair").transform.Find("face").GetChild(i).GetComponent<SpriteRenderer>()
						.sprite = faceSprites[6];
				}
			}
			else
			{
				GetPart("hair").transform.Find("face").GetChild(i).GetComponent<SpriteRenderer>()
					.sprite = faceSprites[UnityEngine.Random.Range(0, 9)];
			}
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		speed = 1;
		base.Chat(text, type, sound, pos, canSkip, speed);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == EnemyBase.CHECK_NAME)
		{
			return new string[1] { "* MOLDESSA - 27 ATK 23 DEF\n" + checkDesc };
		}
		if (GetActNames()[i] == "Switch")
		{
			SwitchAct(UnityEngine.Random.Range(0, 20) == 0);
			if (switchCount < 4)
			{
				switchCount++;
				gold += 2;
			}
			return new string[1] { "* You encourage Moldessa to try\n  a new look.\n* Its face shifts." };
		}
		if (GetActNames()[i] == "Fix")
		{
			if (faceSpin)
			{
				faceSpin = false;
				if (satisfied < 100)
				{
					AddActPoints(100);
				}
				return new string[1] { "* You adjust Moldessa's face.\n* It seems to be happy with its\n  new look." };
			}
			faceSpin = true;
			return new string[1] { "* You adjust Moldessa's face." };
		}
		if (GetActNames()[i] == "Lie Down")
		{
			if (!sleptOnce)
			{
				Util.GameManager().Heal(0, 2);
				sleptOnce = true;
				EnemyBase[] array = Util.FindObjectsOfType<EnemyBase>();
				foreach (EnemyBase enemyBase in array)
				{
					if (enemyBase != this && enemyBase.GetSatisfactionLevel() < 100)
					{
						enemyBase.AddActPoints(100);
					}
				}
				return new string[3] { "* You lie down and rest.\n* Moldessa tucks you in with\n  a blanket of <color=#00FF00FF>[MOSS]</color>.", "* While you were sleeping,^05 the\n  other monsters get bored.", "su_wtf`snd_txtsus`* WHAT THE HELL ARE\n  YOU DOING???\n* GET UP!" };
			}
			return new string[1] { "* You lie down and rest.\n* Moldessa tucks you in with\n  a blanket of <color=#00FF00FF>[MOSS]</color>." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		if (spared)
		{
			return base.PerformAssistAct_Old(i);
		}
		if (i == 1)
		{
			if (!susieBiteAttempt)
			{
				susieBiteAttempt = true;
				Util.GameManager().Heal(1, 2);
				Util.GameManager().PlayGlobalSFX("sounds/snd_swallow");
				Util.GameManager().PlayTimedHealSound();
				pieceRegrow = true;
				GetPart("hair").transform.Find("face").GetChild(0).localScale = Vector3.zero;
				return new string[1] { "* Susie managed to get a\n  bite out of Moldessa.\n* She recovered 2 HP!" };
			}
			return new string[1] { "* Moldessa only allows one bite." };
		}
		return base.PerformAssistAct_Old(i);
	}

	public override bool IsTired()
	{
		if (!tired)
		{
			return (float)hp / (float)maxHp <= 0.2f;
		}
		return true;
	}
}
