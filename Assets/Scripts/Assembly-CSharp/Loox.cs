using UnityEngine;

public class Loox : EnemyBase
{
	private bool saidDontPickOnMe;

	private bool krisAct;

	private bool susieAct;

	private int lastAct = -1;

	private bool nextAttackHard;

	private Sprite[] sprites;

	private int bodyFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Loox";
		fileName = "loox";
		checkDesc = "* Don't pick on him.\n* Family name: Eyewalker";
		maxHp = 90;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 6;
		def = 7;
		flavorTxt = new string[4] { "* Loox is gazing at you.", "* Loox is staring right\n  through you.", "* Loox gnashes its teeth.", "* Smells like eyedrops." };
		dyingTxt = new string[1] { "* Loox is watering." };
		satisfyTxt = flavorTxt;
		chatter = new string[5] { "I've got \nmy eye \non you.", "Don't \npoint \nthat \nat me.", "Quit \nstaring \nat me.", "What an \neyesore.", "How \nabout a \nstaring \ncontest?" };
		actNames = new string[2] { "Pick On", "Don't Pick On" };
		defaultChatSize = "RightSmall";
		exp = 3;
		gold = 2;
		attacks = new int[2] { 15, 16 };
		sprites = new Sprite[3]
		{
			Resources.Load<Sprite>("battle/enemies/Loox/spr_b_loox_0"),
			Resources.Load<Sprite>("battle/enemies/Loox/spr_b_loox_1"),
			Resources.Load<Sprite>("battle/enemies/Loox/spr_b_loox_2")
		};
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit)
		{
			bodyFrames++;
			int num = 0;
			if (bodyFrames == 22 || bodyFrames == 23 || bodyFrames == 28 || bodyFrames == 29)
			{
				num = 1;
			}
			if (bodyFrames >= 24 && bodyFrames <= 27)
			{
				num = 2;
			}
			if (bodyFrames == 30)
			{
				bodyFrames = 0;
			}
			GetPart("body").GetComponent<SpriteRenderer>().sprite = sprites[num];
		}
	}

	public override string[] PerformAct(int i)
	{
		lastAct = i - 1;
		if (GetActNames()[i] == "Pick On")
		{
			nextAttackHard = true;
			if (exp < 11)
			{
				exp += 2;
			}
			return new string[1] { "* You picked on Loox." };
		}
		if (GetActNames()[i] == "Don't Pick On")
		{
			if (!krisAct)
			{
				saidDontPickOnMe = true;
				krisAct = true;
				AddActPoints(50);
			}
			return new string[1] { "* You stepped away from\n  Loox." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (spared)
		{
			return base.PerformAssistAct(i);
		}
		if (i == 1)
		{
			lastAct = 2;
			if (!susieAct)
			{
				saidDontPickOnMe = true;
				susieAct = true;
				AddActPoints(50);
			}
			return new string[2] { "* Susie picked on Loox.", "su_teeth`snd_txtsus`* I'm gonna throw you\n  against the wall like\n  a dodgeball." };
		}
		return base.PerformAssistAct(i);
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (!saidDontPickOnMe)
		{
			saidDontPickOnMe = true;
			text = new string[1] { "Please \ndon't \npick on \nme." };
		}
		else if (lastAct > -1)
		{
			string[] array = new string[3] { "You rude \nlittle \nsnipe!", "Finally \nsomeone \ngets it.", "I won't \npick on \nyou." };
			if (!krisAct)
			{
				array[2] = "The \nhuman \nwon't \npick on \nme.";
			}
			if (!susieAct)
			{
				array[1] = "But what \nabout \nthe \ngirl?";
			}
			text = new string[1] { array[lastAct] };
			lastAct = -1;
		}
		base.Chat(text, type, sound, pos, canSkip, 1);
		chatbox.gameObject.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public override int GetNextAttack()
	{
		if (Object.FindObjectsOfType<Loox>().Length == 2 && !Object.FindObjectsOfType<Loox>()[0].IsDone() && !Object.FindObjectsOfType<Loox>()[1].IsDone())
		{
			return 19;
		}
		if ((bool)Object.FindObjectOfType<Vegetoid>() && !Object.FindObjectOfType<Vegetoid>().IsDone())
		{
			return 21;
		}
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
}
