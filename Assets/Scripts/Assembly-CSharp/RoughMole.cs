using System;
using UnityEngine;

public class RoughMole : EnemyBase
{
	private bool geno;

	private bool playingDefeatAnim;

	private int defeatFrames;

	private int animFrames;

	private bool tossAnim;

	private int tossFrames;

	private int power;

	private bool playRough;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Rough Mole";
		fileName = "mole";
		checkDesc = "* This bored mole likes playing\n  rough.";
		maxHp = 600;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 150;
		atk = 20;
		def = 0;
		displayedDef = 5;
		flavorTxt = new string[4] { "* Rough Mole is clawing the\n  ground with its claws.", "* Rough Mole is getting\n  bored.", "* Smells like dirt.", "* Rough Mole is running all\n  over the place." };
		dyingTxt = new string[1] { "* Rough Mole is starting to\n  back away." };
		satisfyTxt = new string[1] { "* Rough Mole has played enough." };
		actNames = new string[2] { "Play Rough", "S!Toss" };
		canBeSkipped = true;
		defaultChatSize = "RightSmall";
		exp = 1;
		gold = 12;
		geno = (int)Util.GameManager().GetFlag(13) >= 5;
		attacks = new int[1] { 53 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 109, 121f);
	}

	protected override void Update()
	{
		base.Update();
		if (playingDefeatAnim)
		{
			defeatFrames++;
			if (!geno && defeatFrames == 14)
			{
				Transform obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyRunEffect")).transform;
				obj.position = base.obj.transform.Find("mainbody").position;
				SpriteRenderer[] componentsInChildren = obj.GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].sprite = base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite;
				}
				base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = false;
			}
			else if (geno)
			{
				if (defeatFrames < 15)
				{
					base.obj.transform.Find("mainbody").localPosition = new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 48f;
				}
				else if (defeatFrames < 40)
				{
					base.obj.transform.Find("mainbody").localPosition = new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 24f;
				}
				else if (defeatFrames == 40)
				{
					base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_1");
					base.obj.transform.Find("mainbody").localPosition = Vector3.zero;
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), base.obj.transform.Find("mainbody").position + new Vector3(0f, 0.2f), Quaternion.identity);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_noise");
					aud.Play();
				}
			}
		}
		else if (tossAnim)
		{
			tossFrames++;
			if (tossFrames == 1)
			{
				CombineParts();
				base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Rough Mole/spr_b_mole_normal");
			}
			base.obj.transform.Find("mainbody").localPosition = new Vector3(0f, Mathf.Lerp(1.234f, Mathf.Lerp(2f, 5.3f, (float)power / 100f), Mathf.Sin((float)tossFrames * 4.5f * (MathF.PI / 180f))));
			base.obj.transform.Find("mainbody").eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(0f, -360f, (float)tossFrames / 40f));
			if (tossFrames == 40)
			{
				base.obj.transform.Find("mainbody").localPosition = Vector3.zero;
				tossAnim = false;
				SeparateParts();
			}
		}
		else if (!gotHit && (bool)base.obj)
		{
			animFrames++;
			float num = Mathf.Sin((float)(animFrames * 12) * (MathF.PI / 180f));
			float num2 = Mathf.Sin((float)(animFrames * 6) * (MathF.PI / 180f));
			GetPart("body").localScale = new Vector3(1f + num * 0.05f, 1f - num * 0.05f);
			GetPart("arms").localPosition = new Vector3(num * -0.15f, 0f - Mathf.Abs(num2 * 0.15f));
		}
	}

	public override void TurnToDust()
	{
		playingDefeatAnim = true;
		if (!geno)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_defeatrun");
			aud.Play();
		}
		CombineParts();
	}

	public void PlayTossAnim(int power)
	{
		this.power = power;
		animFrames = 0;
		tossAnim = true;
		tossFrames = 0;
		Util.GameManager().PlayGlobalSFX((power <= 20) ? "sounds/snd_awkward" : "sounds/snd_jump");
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Play Rough")
		{
			if (satisfied < 100)
			{
				AddActPoints(50);
			}
			playRough = true;
			return new string[1] { "* You encouraged the mole to\n  play with you.\n* Its attacks became extreme!" };
		}
		if (GetActNames()[i] == "S!Toss")
		{
			if (satisfied >= 100)
			{
				Spare();
				return new string[1] { "* You and Susie tossed the mole\n  out of the battle!" };
			}
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/acts/MoleToss"), GameObject.Find("BattleCanvas").transform, worldPositionStays: false).GetComponent<MoleToss>().SetMole(this);
			return new string[1] { "* Determine the power to toss\n  the mole!" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct_Old(int i)
	{
		switch (i)
		{
		case 1:
			AddActPoints(35);
			return new string[1] { "* Susie roughhoused with the\n  mole." };
		case 2:
			AddActPoints(5 + satisfied / 5);
			return new string[1] { "* Noelle gave the mole a\n  big hug." };
		default:
			return base.PerformAssistAct_Old(i);
		}
	}

	public bool PlayingRough()
	{
		if (playRough)
		{
			playRough = false;
			return true;
		}
		return false;
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
	}

	public override void Spare(bool sleepMist = false)
	{
		tossAnim = false;
		obj.transform.Find("mainbody").localPosition = Vector3.zero;
		obj.transform.Find("mainbody").localEulerAngles = Vector3.zero;
		base.Spare(sleepMist);
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		tossAnim = false;
		obj.transform.Find("mainbody").localPosition = Vector3.zero;
		obj.transform.Find("mainbody").localEulerAngles = Vector3.zero;
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0 && rawDmg > 0f && geno)
		{
			exp = 50;
			gold = 16;
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_0");
		}
	}
}
