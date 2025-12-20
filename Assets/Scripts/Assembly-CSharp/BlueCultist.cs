using System;
using UnityEngine;

public class BlueCultist : EnemyBase
{
	private bool geno;

	private bool playingDefeatAnim;

	private int defeatFrames;

	private bool painted;

	private int animFrames;

	private bool lookingForAvoid;

	private bool lookingForHit;

	private bool worried;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Blue Cultist";
		fileName = "cultist";
		checkDesc = "* True believer in Happy-happism.\n* Wants to paint the world blue.";
		maxHp = 470;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 150;
		atk = 20;
		def = 10;
		flavorTxt = new string[4] { "* The cultist's paintbrush sways\n  to and fro.", "* Smells like paint.", "* Blue Cultist stares right\n  through you.", "* Looks like the cultist\n  wants to paint you." };
		dyingTxt = new string[1] { "* Blue Cultist is losing\n  their grip." };
		satisfyTxt = new string[1] { "* Blue Cultist is satisfied\n  with your dedication to\n  blue." };
		chatter = new string[5] { "Blue,\nblue.", "Join \nus!", "Happy-\nhappism \nis the \nanswer.", "I shall \npaint \nyou \nblue", "Your \nSOUL is \ntoo red" };
		actNames = new string[3] { "Paint", "N!Join", "S!Refuse" };
		defaultChatSize = "RightSmall";
		exp = 1;
		gold = 12;
		geno = (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) >= 4;
		attacks = new int[2] { 35, 36 };
	}

	protected override void Start()
	{
		Util.GameManager().SetFlag(129, 1);
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
				if (defeatFrames == 15)
				{
					base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_1");
					aud.clip = Resources.Load<AudioClip>("sounds/snd_noise");
					aud.Play();
				}
				if (defeatFrames < 15)
				{
					base.obj.transform.Find("mainbody").localPosition = new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 24f;
				}
				else if (defeatFrames < 40)
				{
					base.obj.transform.Find("mainbody").localPosition = new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) / 48f;
				}
				else if (defeatFrames == 40)
				{
					base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_2");
					base.obj.transform.Find("mainbody").localPosition = Vector3.zero;
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), base.obj.transform.Find("mainbody").position + new Vector3(0f, 0.2f), Quaternion.identity);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_noise");
					aud.Play();
				}
			}
		}
		else if (!gotHit)
		{
			animFrames++;
			float t = (Mathf.Cos((float)(animFrames * 12) * ((float)Math.PI / 180f)) + 1f) / 2f;
			GetPart("body").transform.localPosition = new Vector3(-0.209f, Mathf.Lerp(0.922f, 1.041f, t));
			GetPart("body").transform.Find("leftarm").eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-8.102f, 0f, t));
			GetPart("body").transform.Find("rightarm").eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-8.677f, 0f, t));
			GetPart("legs").transform.localScale = new Vector3(1f, Mathf.Lerp(0.9f, 1f, t), 1f);
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (!UnityEngine.Object.FindObjectOfType<IceShock>())
		{
			Util.GameManager().SetFlag(129, 0);
		}
		if (hp > 0 || !(rawDmg > 0f))
		{
			return;
		}
		if (geno)
		{
			BlueCultist[] array = UnityEngine.Object.FindObjectsOfType<BlueCultist>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].SetToWorry();
			}
			exp = 50;
			gold = 16;
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_die_0");
		}
		else
		{
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_run");
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Paint")
		{
			bool flag = Util.GameManager().GetMiniPartyMember() == 1;
			if (satisfied < 100)
			{
				AddActPoints(flag ? 75 : 25);
			}
			if (!painted)
			{
				string text = "";
				if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(102) == 1)
				{
					text = "_injured";
				}
				UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_paintedblue" + text);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().SetSprite(3, "spr_paula_paintedblue");
				painted = true;
				return new string[1] { flag ? "* You and Paula painted your\n  whole bodies blue." : "* You painted your whole body\n  blue." };
			}
			return new string[1] { flag ? "* You and Paula reapplied the\n  paint." : "* You reapplied the paint." };
		}
		if (GetActNames()[i] == "S!Refuse")
		{
			lookingForAvoid = true;
			return new string[2] { "su_annoyed`snd_txtsus`* Keep your cult shit\n  away from us.", "* (Susie encouraged you to <color=#FFFF00FF>avoid\n  getting hit</color> this turn!)" };
		}
		if (GetActNames()[i] == "N!Join")
		{
			lookingForHit = true;
			return new string[2] { "no_happy`snd_txtnoe`* Will you leave us\n  alone if you paint\n  us blue?", "* (Noelle encouraged you to \n  <color=#FFFF00FF>get hit</color> this turn!)" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			AddActPoints(25);
			return new string[1] { "* Susie ate some blue chalk.\n* The cultist seemed pleased." };
		case 2:
			AddActPoints(25);
			return new string[1] { "* Noelle performed some ice\n  magic.\n* The cultist seemed impressed." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
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

	public void ResetLookVariables()
	{
		lookingForHit = false;
		lookingForAvoid = false;
	}

	public bool LookingForHit()
	{
		return lookingForHit;
	}

	public bool LookingForAvoid()
	{
		return lookingForAvoid;
	}

	public void RewardLooking()
	{
		if (satisfied < 100)
		{
			AddActPoints(100);
		}
	}

	public override bool CanSpare()
	{
		if (!base.CanSpare())
		{
			return worried;
		}
		return true;
	}

	public override void EnemyTurnEnd()
	{
		ResetLookVariables();
	}

	public void SetToWorry()
	{
		worried = true;
		GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Blue Cultist/spr_b_cultist_torso_worry");
		chatter = new string[4] { "W-what \nhave \nyou...?!", "K...\nkillers!", "I will \navenge \nyou, my \nfriend...", "E-enough \nof \nthis!!!" };
	}

	public bool IsWorried()
	{
		return worried;
	}

	public override bool[] GetTargets()
	{
		if (lookingForAvoid)
		{
			return new bool[3] { true, true, false };
		}
		if (lookingForHit)
		{
			return new bool[3] { true, false, true };
		}
		return base.GetTargets();
	}
}
