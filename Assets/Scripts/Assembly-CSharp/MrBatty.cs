using System;
using UnityEngine;

public class MrBatty : EnemyBase
{
	private bool geno;

	private bool playingDefeatAnim;

	private int defeatFrames;

	private bool disabled;

	private int animFrames;

	private bool assured;

	private Vector3 basePos = Vector3.zero;

	private int lastAct = -1;

	private bool chasedOnce;

	private bool noelleAct;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Mr.Batty";
		fileName = "batty";
		checkDesc = "* He's looking for any\n  reason to slow down.";
		maxHp = 400;
		hp = maxHp;
		hpPos = new Vector2(2f, 142f);
		hpWidth = 150;
		atk = 16;
		def = 5;
		displayedDef = 8;
		flavorTxt = new string[4] { "* Mr. Batty is flying all\n  over the place.", "* Smells like mothballs.", "* Mr. Batty's wings flap very\n  loudly.", "* Mr. Batty looks kinda goofy\n  floating there." };
		dyingTxt = new string[1] { "* Mr. Batty is struggling to\n  stay afloat." };
		satisfyTxt = new string[1] { "* Mr. Batty is learning to\n  take things slow." };
		actNames = new string[2] { "Assure", "Approach" };
		hurtSound = "sounds/snd_ehurt1";
		chatter = new string[4] { "* flap \nflap*", "Hey hey", "Squeak", "Du nu nu \nnu nu nu \nnu nu" };
		defaultChatSize = "RightSmall";
		exp = 1;
		gold = 10;
		geno = (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) >= 5;
		attacks = new int[1] { 53 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 120f);
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i - 1;
		if (GetActNames()[i] == "Assure")
		{
			if (satisfied < 100)
			{
				bool flag = Util.GameManager().GetMiniPartyMember() == 1;
				AddActPoints(flag ? 50 : 25);
				if (!assured)
				{
					if (flag)
					{
						assured = true;
						return new string[1] { "* You assured Mr. Batty that\n  everything will be okay.\n* Mr. Batty's attacks slow!" };
					}
					assured = true;
					return new string[2] { "* You assured Mr. Batty that\n  everything will be okay.", "* Mr. Batty doesn't seem to\n  believe you entirely,^05 but\n  it's attacks slow slightly." };
				}
				return new string[1] { "* You kept reassuring Mr. Batty." };
			}
			return new string[1] { "* Mr. Batty is already\n  calmed down." };
		}
		if (GetActNames()[i] == "Approach")
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_eb_ailment");
			aud.Play();
			disabled = true;
			return new string[1] { "* You slowly approached\n  Mr. Batty.\n* Mr. Batty's body solidified!" };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			AddActPoints(10);
			if (!chasedOnce)
			{
				chasedOnce = true;
				return new string[1] { "* Susie chased Mr. Batty.\n* Mr. Batty became slightly\n  more tired." };
			}
			tired = true;
			return new string[1] { "* Susie chased Mr. Batty.\n* Mr. Batty became truly\n  exhausted!" };
		case 2:
			if (noelleAct)
			{
				AddActPoints(20);
				return new string[1] { "* Noelle continued to pet\n  Mr. Batty." };
			}
			AddActPoints(50);
			noelleAct = true;
			return new string[3] { "* Noelle reached her hand out\n  for Mr. Batty to land on.", "* Mr. Batty landed on Noelle's\n  hand.", "* Noelle proceeded to comfort\n  and pet Mr. Batty." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (lastAct > -1)
		{
			string[] array = new string[2] { "I'm \nlistening", "Whoa \nwhoa \nwhoa \nwhoa" };
			if (satisfied >= 100)
			{
				array[0] = "Thank \nyou...";
			}
			text = new string[1] { array[lastAct] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
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
				defeatFrames++;
				if (defeatFrames <= 40)
				{
					float x = basePos.x + Mathf.Sin((float)(defeatFrames * 18) * ((float)Math.PI / 180f)) / 8f;
					float num = (float)defeatFrames / 40f;
					float y = Mathf.Lerp(basePos.y, -0.37f, num * num);
					GetEnemyObject().transform.position = new Vector3(x, y);
				}
				if (defeatFrames == 40)
				{
					base.obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_kill_1");
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), base.obj.transform.Find("mainbody").position + new Vector3(0f, 0.4f), Quaternion.identity);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_noise");
					aud.Play();
				}
			}
		}
		else if (!gotHit && (bool)base.obj && !disabled)
		{
			animFrames++;
			float t = (0f - (Mathf.Cos((float)(animFrames * 10) * ((float)Math.PI / 180f)) - 1f)) / 2f;
			Vector3 localScale = Vector3.Lerp(new Vector3(1f, 1f), new Vector3(0.4125f, 1.3625f), t);
			float y2 = Mathf.Sin((float)(animFrames * 5) * ((float)Math.PI / 180f)) * 0.5f;
			GetPart("fullbody").localPosition = new Vector3(0f, y2);
			GetPart("fullbody").Find("left").localScale = localScale;
			GetPart("fullbody").Find("right").localScale = localScale;
		}
	}

	public bool IsDisabled()
	{
		return disabled;
	}

	public void Reenable()
	{
		disabled = false;
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0 && rawDmg > 0f && geno)
		{
			exp = 50;
			gold = 13;
			obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_kill_0");
		}
	}

	public override void TurnToDust()
	{
		if (!playingDefeatAnim)
		{
			basePos = obj.transform.position;
		}
		playingDefeatAnim = true;
		if (!geno)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_defeatrun");
			aud.Play();
		}
		CombineParts();
	}
}
