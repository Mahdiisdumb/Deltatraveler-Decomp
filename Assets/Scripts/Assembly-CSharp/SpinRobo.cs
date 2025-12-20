using UnityEngine;

public class SpinRobo : EnemyBase
{
	private bool spinTime;

	private Vector3 basePos;

	private bool isDying;

	private int dyingFrames;

	private int bodyFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Spinning Robo";
		fileName = "robo";
		checkDesc = "* Protocol: MUST SPIN\n* Can't handle standing still.";
		maxHp = 300;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 15;
		def = 15;
		flavorTxt = new string[4] { "* Spinning Robo is rolling\n  around.", "* Spinning Robo can't stop\n  moving.", "* Smells like gasoline.", "* Spinning Robo desperately needs\n  some spinning action." };
		dyingTxt = new string[1] { "* Spinning Robo is smoking." };
		satisfyTxt = new string[1] { "* Spinning Robo is all spun\n  out." };
		chatter = new string[3] { "SPIN \nWITH \nME", "WHERE \nARE THE \nSPINNERS", "PROTOCOL:\nMUST \nSPIN" };
		actNames = new string[2] { "Spin", "S!Hold Down" };
		hurtSound = "sounds/snd_mtt_hit";
		defaultChatSize = "RightSmall";
		exp = 16;
		gold = 10;
		attacks = new int[1] { 32 };
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 103, 117f);
		basePos = GetEnemyObject().transform.position;
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit && !isDying)
		{
			bodyFrames++;
			if (bodyFrames >= 40)
			{
				bodyFrames = 0;
			}
			float num = (float)bodyFrames / 20f;
			if (bodyFrames > 20)
			{
				num = 2f - num;
			}
			GetPart("arms").transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0.155f), num);
		}
		else
		{
			if (!isDying)
			{
				return;
			}
			dyingFrames++;
			float num2 = ((dyingFrames >= 15) ? 1 : 2);
			GetEnemyObject().transform.position = basePos + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) * num2 / 48f;
			if (dyingFrames % 2 == 0)
			{
				SpriteRenderer component = Object.Instantiate(Resources.Load<GameObject>("vfx/EnemySmoke"), new Vector3(Random.Range(-0.725f, 0.725f), Random.Range(-0.224f, 2.934f)) + basePos, Quaternion.identity).GetComponent<SpriteRenderer>();
				component.sortingOrder = Random.Range(23, 27);
				if (component.sortingOrder == 25)
				{
					component.sortingOrder++;
				}
			}
			if (dyingFrames == 30)
			{
				Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyExplosion")).transform.position += new Vector3(basePos.x, 0f);
				Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeIn(10, Color.white);
				Object.FindObjectOfType<BattleCamera>().GiantBlastShake();
				obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = false;
				isDying = false;
			}
		}
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Spin")
		{
			spinTime = true;
			return new string[1] { "* You offered to spin with\n  Spinning Robo.\n* Spinning Robo got excited." };
		}
		if (GetActNames()[i] == "S!Hold Down")
		{
			tired = true;
			return new string[2] { "* You and Susie held down\n  Spinning Robo.", "* Spinning Robo became TIRED\n  for the rest of the turn." };
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
			if (satisfied < 100)
			{
				AddActPoints(50);
			}
			return new string[1] { "* Susie spun around until\n  she got dizzy." };
		case 2:
			if (satisfied < 100)
			{
				AddActPoints(50);
			}
			return new string[1] { "* Noelle spun around like\n  an ice skater." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		tired = false;
		if (spinTime)
		{
			text = new string[1] { "LET US \nSPIN, \nBABY" };
		}
		base.Chat(text, type, "snd_txtmtt", pos, canSkip, 0);
	}

	public override void TurnToDust()
	{
		CombineParts();
		isDying = true;
	}

	public override int GetNextAttack()
	{
		if (spinTime)
		{
			return 33;
		}
		return base.GetNextAttack();
	}

	public override bool[] GetTargets()
	{
		if (spinTime && Object.FindObjectOfType<GameManager>().GetHP(0) > 0)
		{
			return new bool[3] { true, false, false };
		}
		return base.GetTargets();
	}

	public void Unspin()
	{
		if (satisfied < 100)
		{
			AddActPoints(100);
		}
		spinTime = false;
	}
}
