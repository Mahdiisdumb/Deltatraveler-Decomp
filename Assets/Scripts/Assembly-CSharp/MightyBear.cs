using UnityEngine;

public class MightyBear : EnemyBase
{
	private bool geno;

	private bool playingDefeatAnim;

	private int defeatFrames;

	private int animFrames;

	private Vector3 basePos = Vector3.zero;

	private bool toBulletBoard;

	private bool beesArePissed;

	private bool lectured;

	private int lastAct = -1;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Mighty Bear";
		fileName = "bear";
		checkDesc = "* It doesn't seem to know the\n  concept of personal space.";
		maxHp = 800;
		hp = maxHp;
		hpPos = new Vector2(2f, 142f);
		hpWidth = 150;
		atk = 25;
		def = 10;
		displayedDef = 11;
		flavorTxt = new string[4] { "* Mighty Bear is wandering\n  around you.", "* Smells like trees and grass.^15\n* Somehow.", "* Mighty Bear lets out a mighty\n  yawn.", "* Mighty Bear looks like it's\n  about to fall over." };
		dyingTxt = new string[1] { "* Mighty Bear is wobbling." };
		satisfyTxt = new string[1] { "* Mighty Bear is keeping its\n  distance." };
		actNames = new string[3] { "SN!Hug", "Lecture", "Shoo" };
		defaultChatSize = "RightSmall";
		exp = 2;
		gold = 10;
		geno = (int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 5;
		attacks = new int[1] { 53 };
	}

	protected override void Start()
	{
		base.Start();
		basePos = obj.transform.position;
	}

	protected override void Update()
	{
		base.Update();
		if (playingDefeatAnim)
		{
			defeatFrames++;
			if (!geno && defeatFrames == 14)
			{
				Transform obj = Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyRunEffect")).transform;
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
				if (defeatFrames >= 20 && defeatFrames <= 23)
				{
					int num = ((defeatFrames % 2 == 0) ? 1 : (-1));
					int num2 = 23 - defeatFrames;
					base.obj.transform.position = basePos + new Vector3((float)(num2 * num) / 24f, 0f);
				}
				else if (defeatFrames == 35)
				{
					Object.FindObjectOfType<BattleCamera>().BlastShake();
					base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_1");
					base.obj.transform.Find("mainbody").localPosition = Vector3.zero;
					Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), base.obj.transform.Find("mainbody").position + new Vector3(0f, 0.2f), Quaternion.identity);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_crash");
					aud.Play();
				}
			}
		}
		else if (!gotHit && (bool)base.obj)
		{
			animFrames++;
			if (animFrames == 30 || animFrames == 45)
			{
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_body_1");
			}
			else if (animFrames == 32 || animFrames == 47)
			{
				GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_body_0");
			}
			if (animFrames == 50)
			{
				animFrames = 0;
			}
			base.obj.transform.position = Vector3.Lerp(base.obj.transform.position, toBulletBoard ? new Vector3(0f, -4.05f) : basePos, 0.5f);
		}
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i - 1;
		if (GetActNames()[i] == "SN!Hug")
		{
			if (lectured)
			{
				if (satisfied < 100)
				{
					AddActPoints(100);
				}
				return new string[1] { "* Everyone hugged Mighty Bear.\n* It now fully understands\n  boundaries." };
			}
			return new string[2] { "* You tried to convince Susie\n  and Noelle to hug the bear...", "* But it seems to have a\n  very intrusive presence." };
		}
		if (GetActNames()[i] == "Lecture")
		{
			if (!lectured)
			{
				lectured = true;
				atk -= 5;
				def -= 5;
				return new string[2] { "* You lectured Mighty Bear on\n  suddenly jumping at you.", "* It seems to feel bad.\n* It's ATTACK and DEFENSE\n  drop!" };
			}
			return new string[1] { "* You lectured Mighty Bear again,\n^05  but nothing seems to happen." };
		}
		if (GetActNames()[i] == "Shoo")
		{
			int num = (lectured ? 50 : 15);
			if (Util.GameManager().GetMiniPartyMember() == 1)
			{
				num += 15;
			}
			if (satisfied < 100)
			{
				AddActPoints(num);
			}
			else if (lectured)
			{
				Spare();
				return new string[1] { "* You tried to get Mighty Bear\n  to go away.\n* Mighty Bear left the battle." };
			}
			if (!lectured)
			{
				return new string[2] { "* You tried to get Mighty Bear\n  to go away.", "* It doesn't seem to fully\n  understand what you mean." };
			}
			return new string[2] { "* You tried to get Mighty Bear\n  to go away.", "* It seems to be grasping\n  at what you're trying to\n  say." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			AddActPoints(25);
			return new string[2] { "* Susie roared at Mighty Bear.\n* It seemed entertained by it.", "su_angry`snd_txtsus`* Quit staring at me,^05\n  Winnie the Pooh lookin'\n  ass!" };
		case 2:
			tired = true;
			return new string[4] { "no_scared`snd_txtnoe`* M^05-me???", "no_surprised_happy`snd_txtnoe`* Uhh...^05 anything but\n  a GIANT BEAR.", "no_happy`snd_txtnoe`* And ESPECIALLY not\n  by myself.", "* Mighty Bear saw the lack\n  of action and became tired." };
		default:
			return base.PerformAssistAct(i);
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

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0 && rawDmg > 0f && geno)
		{
			exp = 80;
			gold = 15;
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_0");
		}
	}

	public void SetToBulletBoard(bool toBulletBoard)
	{
		this.toBulletBoard = toBulletBoard;
	}

	public void PissOffBees()
	{
		beesArePissed = true;
	}

	public bool AreBeesPissed()
	{
		return beesArePissed;
	}

	public bool IsLectured()
	{
		return lectured;
	}
}
