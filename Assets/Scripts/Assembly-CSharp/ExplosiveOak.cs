using UnityEngine;

public class ExplosiveOak : EnemyBase
{
	private bool alone;

	private Vector3 basePos;

	private bool explode;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Explosive Oak";
		fileName = "oak";
		checkDesc = "* Very flammable and territorial.\n* Resistant to ICE attacks.";
		maxHp = 350;
		hp = maxHp;
		hpPos = new Vector2(2f, 122f);
		hpWidth = 101;
		atk = 10;
		def = 10;
		flavorTxt = new string[3] { "* Explosive Oak doesn't know why\n  it's here.", "* Smells like apples.", "* Explosive Oak looks really\n  clueless." };
		dyingTxt = new string[1] { "* Explosive Oak is starting\n  to heat up." };
		actNames = new string[2] { "Hug", "SN!Convince" };
		defaultChatSize = "RightSmall";
		exp = 14;
		gold = 6;
		attacks = new int[1] { 32 };
	}

	protected override void Start()
	{
		base.Start();
		basePos = GetEnemyObject().transform.position;
	}

	protected override void Update()
	{
		if (explode && !killed)
		{
			GetEnemyObject().transform.position = basePos + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) * 2f / 48f;
		}
		base.Update();
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		if ((bool)Object.FindObjectOfType<PKFreezeEffect>() || (bool)Object.FindObjectOfType<IceShock>())
		{
			rawDmg = 5f;
		}
		if ((bool)Object.FindObjectOfType<PKFireEffect>())
		{
			rawDmg = 100f;
		}
		base.Hit(partyMember, rawDmg, playSound);
		if (killed)
		{
			killed = false;
			explode = true;
		}
	}

	public override int CalculateDamage(int partyMember, float rawDmg, bool forceMagic = false)
	{
		if (partyMember == 2 && (bool)Object.FindObjectOfType<IceShock>())
		{
			return Mathf.FloorToInt((float)base.CalculateDamage(partyMember, rawDmg, forceMagic) * 0.166f);
		}
		return base.CalculateDamage(partyMember, rawDmg, forceMagic);
	}

	public override string[] PerformAct(int i)
	{
		if (GetActNames()[i] == "Hug")
		{
			tired = true;
			if (!((float)hp / (float)maxHp <= 0.2f))
			{
				return new string[1] { "* You hugged the tree.\n* The tree felt more relaxed.\n* It became TIRED." };
			}
			return new string[1] { "* You hugged the tree.\n* It felt very warm.\n* It became TIRED." };
		}
		if (GetActNames()[i] == "SN!Convince")
		{
			Spare();
			return new string[1] { "* Everyone somehow convinced the\n  tree to leave you alone.\n* Explosive Oak left the battle." };
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
			return new string[1] { "su_annoyed`snd_txtsus`* Uhh,^05 the hell do you\n  want me to do?" };
		case 2:
			return new string[1] { "no_happy`snd_txtnoe`* (Isn't it just a tree?)" };
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override bool CanSpare()
	{
		bool flag = true;
		EnemyBase[] array = Object.FindObjectsOfType<EnemyBase>();
		foreach (EnemyBase enemyBase in array)
		{
			if (enemyBase != this && !enemyBase.IsDone())
			{
				flag = false;
			}
		}
		if (flag)
		{
			return true;
		}
		return base.CanSpare();
	}

	public override void Chat()
	{
	}

	public bool IsGonnaExplode()
	{
		return explode;
	}

	public override int GetNextAttack()
	{
		if (explode)
		{
			return 32;
		}
		return base.GetNextAttack();
	}

	public void Explode()
	{
		aud.clip = Resources.Load<AudioClip>("sounds/snd_explosion_8bit");
		aud.Play();
		aud.volume = 1f;
		killed = true;
		explode = false;
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().enabled = false;
		for (int i = 0; i < 54; i++)
		{
			ExplosionFlameBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/ExplosionFlameBullet"), new Vector3(basePos.x, 1.24f), Quaternion.identity, Object.FindObjectOfType<AttackBase>().transform).GetComponent<ExplosionFlameBullet>();
			int num = i * 20;
			component.Activate(num, num / 360);
		}
	}

	public override void TurnToDust()
	{
	}
}
