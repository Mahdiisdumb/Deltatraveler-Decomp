using UnityEngine;

public class MobileSprout : EnemyBase
{
	private int bodyFrames;

	private Sprite[] bodySprites;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Mobile Sprout";
		fileName = "sprout";
		checkDesc = "* A small, wandering plant\n  in dire need of water.";
		maxHp = 220;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 10;
		def = 8;
		flavorTxt = new string[4] { "* Mobile Sprout is wandering the\n  battlefield.", "* Mobile Sprout doesn't seem\n  to show any signs of\n  awareness.", "* It smells like weeds.", "* Mobile Sprout looks rather\n  dry." };
		dyingTxt = new string[1] { "* Mobile Sprout is slowing\n  down." };
		satisfyTxt = new string[1] { "* Mobile Sprout looks hydrated." };
		actNames = new string[2] { "Water", "SN!WaterX" };
		defaultChatSize = "RightSmall";
		exp = 12;
		gold = 6;
		tired = true;
		attacks = new int[1] { 32 };
		bodySprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/enemies/Mobile Sprout/spr_b_sprout_0"),
			Resources.Load<Sprite>("battle/enemies/Mobile Sprout/spr_b_sprout_1")
		};
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Water")
		{
			if (satisfied < 100)
			{
				AddActPoints(50);
			}
			return new string[1] { "* You threw some river water\n  onto Mobile Sprout." };
		}
		if (GetActNames()[i] == "SN!WaterX")
		{
			if (satisfied < 100)
			{
				AddActPoints(100);
			}
			return new string[1] { "* Everyone threw river water\n  onto Mobile Sprout." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		if (spared)
		{
			return base.PerformAssistAct(i);
		}
		if (satisfied < 100)
		{
			AddActPoints(25);
		}
		switch (i)
		{
		case 1:
			return new string[1] { "* Susie spit on Mobile Spout." };
		case 2:
			return new string[1] { "* Noelle threw a little\n  bit of river water onto\n  Mobile Sprout." };
		default:
			return base.PerformAssistAct(i);
		}
	}

	protected override void Update()
	{
		if (!gotHit)
		{
			bodyFrames++;
			if (bodyFrames >= 20)
			{
				bodyFrames = 0;
			}
			GetPart("body").transform.GetComponent<SpriteRenderer>().sprite = bodySprites[bodyFrames / 10 % 2];
		}
		base.Update();
	}

	public override void Chat()
	{
	}

	public override void TurnToDust()
	{
		aud.clip = Resources.Load<AudioClip>("sounds/snd_dust");
		aud.Play();
		CombineParts();
		obj.transform.Find("mainbody").GetComponent<ParticleDuplicator>().Activate(true, Vector2.zero);
	}
}
