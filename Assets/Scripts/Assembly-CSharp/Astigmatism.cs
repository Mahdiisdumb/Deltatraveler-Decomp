using System;
using UnityEngine;

public class Astigmatism : EnemyBase
{
	private bool wantsPickedOn;

	private bool saidDontPickOnMe;

	private int lastAct = -1;

	private Sprite[] sprites;

	private int bodyFrames;

	private readonly float rotateMulti = 9.473684f;

	private bool respondToDefy;

	private bool nextAttackHard;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Astigmatism";
		fileName = "astigmatism";
		checkDesc = "* This relentless bully ALWAYS\n  gets its way.";
		maxHp = 120;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 6;
		def = 7;
		flavorTxt = new string[5] { "* Astigmatism gazes through\n  your soul.", "* Astigmatism gives you a\n  hypnotizing glare.^15\n* ... Susie seems captivated.", "* Astigmatism clicks its teeth.", "* Smells like glasses.", "* Astigmatism has gone\n  bloodshot." };
		dyingTxt = new string[1] { "* Astigmatism is watering." };
		satisfyTxt = new string[1] { "* Astigmatism doesn't care\n  about fighting anymore." };
		chatter = new string[3] { "You've \nseen \nenough.", "Take \nyour \nlast \nlook.", "You've \npushed \nyour \nluck." };
		actNames = new string[3] { "Pick On", "Don't Pick\n  On", "Challenge" };
		hurtSound = "sounds/snd_hurtgirl";
		canSpareViaFight = false;
		defaultChatSize = "RightSmall";
		exp = 3;
		gold = 5;
		attacks = new int[1] { 48 };
		sprites = new Sprite[12];
		for (int i = 0; i < 12; i++)
		{
			sprites[i] = Resources.Load<Sprite>("battle/enemies/Astigmatism/spr_b_astigmatism_anim2_" + i);
		}
		wantsPickedOn = UnityEngine.Random.Range(0, 2) == 0;
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		bodyFrames = (bodyFrames + 1) % 406;
		float t = (float)(bodyFrames - 198) / 30f;
		if (bodyFrames < 145)
		{
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
		else if (bodyFrames < 163)
		{
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[(bodyFrames - 145) / 3 + 1];
		}
		else if (bodyFrames >= 189 && bodyFrames < 376)
		{
			int num = (bodyFrames - 189) / 3 + 6;
			if (num > 11)
			{
				num = 11;
			}
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[num];
		}
		else if (bodyFrames >= 376)
		{
			int num2 = 10 - (bodyFrames - 376) / 3;
			if (num2 < 0)
			{
				num2 = 0;
			}
			t = (float)(406 - bodyFrames) / 30f;
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[num2];
		}
		float num3 = Mathf.Lerp(0f, 0.08f, t);
		float num4 = Mathf.Lerp(0f, 0.05f, t);
		GetPart("leftarm").transform.localPosition = new Vector3(-0.687f, 0.707f) + new Vector3(Mathf.Cos((float)bodyFrames * rotateMulti * ((float)Math.PI / 180f)), Mathf.Sin((float)bodyFrames * rotateMulti * ((float)Math.PI / 180f))) * num3;
		GetPart("rightarm").transform.localPosition = new Vector3(0.687f, 0.707f) - new Vector3(Mathf.Cos((float)bodyFrames * rotateMulti * ((float)Math.PI / 180f)), Mathf.Sin((float)bodyFrames * rotateMulti * ((float)Math.PI / 180f))) * num3;
		GetPart("leftleg").transform.localPosition = new Vector3(-0.228f, 0.208f) + new Vector3(0f - Mathf.Sin(((float)bodyFrames * rotateMulti + 75f) * ((float)Math.PI / 180f)), 0f - Mathf.Cos(((float)bodyFrames * rotateMulti + 75f) * ((float)Math.PI / 180f))) * num4;
		GetPart("rightleg").transform.localPosition = new Vector3(0.228f, 0.208f) - new Vector3(0f - Mathf.Sin(((float)bodyFrames * rotateMulti + 75f) * ((float)Math.PI / 180f)), 0f - Mathf.Cos(((float)bodyFrames * rotateMulti + 75f) * ((float)Math.PI / 180f))) * num4;
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i - 1;
		if (GetActNames()[i] == "Check")
		{
			return new string[1] { "* ASTIGMATISM - 32 ATK 26 DEF\n" + checkDesc };
		}
		if (GetActNames()[i] == "Pick On")
		{
			if (wantsPickedOn && satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[1] { "* You pick on Astigmatism." };
		}
		if (GetActNames()[i] == "Don't Pick\n  On")
		{
			if (!wantsPickedOn && satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[1] { "* You don't pick on Astigmatism." };
		}
		if (GetActNames()[i] == "Challenge")
		{
			EnemyBase[] array = UnityEngine.Object.FindObjectsOfType<EnemyBase>();
			foreach (EnemyBase enemyBase in array)
			{
				if (!enemyBase.IsDone() && enemyBase != this)
				{
					if (enemyBase.GetSatisfactionLevel() < 100)
					{
						enemyBase.AddActPoints(100);
					}
					if ((bool)enemyBase.GetComponent<Astigmatism>())
					{
						enemyBase.GetComponent<Astigmatism>().RespondToDefy();
					}
				}
			}
			return new string[2] { "* You stand up to Astigmatism.", "su_wideeye`snd_txtsus`* ..." };
		}
		return base.PerformAct(i);
	}

	public void RespondToDefy()
	{
		respondToDefy = true;
	}

	public override string[] PerformAssistAct(int i)
	{
		if (spared)
		{
			return base.PerformAssistAct(i);
		}
		if (i == 1)
		{
			lastAct = 3;
			nextAttackHard = true;
			return new string[2] { "* Susie picked on Astigmatism.", "su_teeth`snd_txtsus`* I'm gonna throw you\n  against the wall like\n  a dodgeball." };
		}
		return base.PerformAssistAct(i);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (!saidDontPickOnMe && satisfied < 100)
		{
			saidDontPickOnMe = true;
			text = new string[1] { wantsPickedOn ? "...\nPick on \nme." : "...\nDon't \npick on \nme." };
		}
		else if (respondToDefy)
		{
			text = new string[1] { "That \ntakes \nguts." };
			respondToDefy = false;
		}
		else if (lastAct > -1)
		{
			string[] array = new string[4]
			{
				wantsPickedOn ? "That's \nright!" : "How \ndare \nyou.",
				wantsPickedOn ? "What are \nyou so \nafraid \nof?" : "Finally, \nsomeone \ngets it.",
				"You \ndare \ndefy me?",
				"Silence \nyour \ntone, \nlady."
			};
			text = new string[1] { array[lastAct] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override int GetNextAttack()
	{
		return base.GetNextAttack();
	}

	public bool IsNextAttackHard()
	{
		return nextAttackHard;
	}

	public void ResetAttackDifficulty()
	{
		nextAttackHard = false;
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
