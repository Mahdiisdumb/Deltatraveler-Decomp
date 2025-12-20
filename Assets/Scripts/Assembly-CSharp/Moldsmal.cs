using UnityEngine;

public class Moldsmal : EnemyBase
{
	private bool susieBiteAttempt;

	private bool down = true;

	private int bounceFrames;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Moldsmal";
		fileName = "moldsmal";
		checkDesc = "^10* Stereotypical: Curvaceously\n  attractive, but no brains...";
		maxHp = 100;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 6;
		def = 0;
		flavorTxt = new string[4] { "* Moldsmal burbles quietly.", "* Moldsmal waits pensively.", "* Moldsmal is ruminating.", "* The aroma of lime gelatin\n  wafts through." };
		dyingTxt = new string[1] { "* Moldsmal has started to spoil." };
		satisfyTxt = flavorTxt;
		satisfied = 100;
		chatter = new string[4] { "Burble \nburb...", "Squorch\n...", "*Slime \nsounds*", "*Sexy \nwiggle*" };
		actNames = new string[2] { "Imitate", "Flirt" };
		defaultChatSize = "RightSmall";
		exp = 4;
		gold = 0;
		attacks = new int[1] { 10 };
	}

	protected override void Update()
	{
		base.Update();
		if (gotHit)
		{
			return;
		}
		if (down)
		{
			bounceFrames++;
			if (bounceFrames == 23)
			{
				down = false;
			}
		}
		else
		{
			bounceFrames--;
			if (bounceFrames == 0)
			{
				down = true;
			}
		}
		GetPart("body").transform.localScale = Vector3.Lerp(new Vector3(1f, 1.15f), new Vector3(1f, 0.85f), (float)bounceFrames / 23f);
	}

	protected override void Start()
	{
		base.Start();
		defaultChatPos = new Vector2(Mathf.RoundToInt(GetEnemyObject().transform.position.x * 48f) + 100, 51f);
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		base.Hit(partyMember, rawDmg, playSound);
		if (hp <= 0)
		{
			gold = 3;
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
		if (GetActNames()[i] == "Imitate")
		{
			return new string[1] { "* You lie immobile with Moldsmal.^05\n* You feel like you understand\n  the world a little better." };
		}
		if (GetActNames()[i] == "Flirt")
		{
			if (susieBiteAttempt)
			{
				gold = 5;
				return new string[2] { "* You wiggle your hips.^05\n* Moldsmal wiggles back\n  affectionately!", "su_inquisitive`snd_txtsus`* What the hell are\n  you doing?" };
			}
			gold = 1;
			return new string[1] { "* You wiggle your hips.^05\n* Moldsmal wiggles back.^05\n* What a meaningful conversation!" };
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
			if (!susieBiteAttempt)
			{
				susieBiteAttempt = true;
				gold = 3;
				return new string[2] { "* Susie tried to get a bite\n  out of Moldsmal.^05\n* Moldsmal became AFFECTIONATE!", "su_wtf`snd_txtsus`* Hey, get away from\n  me!" };
			}
			return new string[1] { "* Moldsmal is still\n  AFFECTIONATE.\n^05* Cannot consume." };
		}
		return base.PerformAssistAct(i);
	}
}
