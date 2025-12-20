using UnityEngine;

public class Glyde : EnemyBase
{
	private int dsiner;

	private float siner;

	private int response = -1;

	private bool strongerAttack;

	private bool forceKill;

	protected override void Awake()
	{
		base.Awake();
		enemyName = "Glyde";
		fileName = "glyde";
		hurtSound = "sounds/snd_hurtbeef";
		maxHp = 550;
		hp = maxHp;
		def = 0;
		playerMultiplier = 1.2f;
		hpWidth = 240;
		defaultChatPos = new Vector2(191f, 60f);
		defaultChatSize = "RightSmall";
		playerMultiplier = 0.5f;
		flavorTxt = new string[1] { "* Glyde is alive somehow." };
		actNames = new string[3] { "N!Applaud", "S!Boo", "SN!Nothing" };
		chatter = new string[4] { "How \ngreat \nI am.", "Look. \nWatch. \nObserve.", "Wow. \nCheck \nout my \npecs.", "Sorry...\nfor \nNOTHING \n*ollies*" };
		attacks = new int[1] { 89 };
	}

	protected override void Update()
	{
		base.Update();
		if (!gotHit && !killed)
		{
			dsiner++;
			siner += Mathf.Cos((float)dsiner / 24f) * 2f;
			float num = Mathf.Sin(siner / 6f);
			float num2 = Mathf.Sin(siner / 12f);
			float y = Mathf.Lerp(0.263f, -0.126f, (num2 + 1f) / 2f);
			GetPart("body").parent.localPosition = new Vector3(0f, y);
			GetPart("rightwing").localPosition = new Vector3(-0.29425f, 1.8947f + num / 24f);
			GetPart("rightwing").localScale = new Vector3(1f, 0.85f - num * 0.15f, 1f);
			GetPart("leftwing").localScale = new Vector3(0.975f - num * 0.025f, 0.9f - num * 0.1f, 1f);
			GetPart("antenna").localPosition = new Vector3(-0.142367f + num2 / 24f, 3.9575f);
			GetPart("antenna").localRotation = Quaternion.Euler(0f, 0f, 20f - num2 * 20f);
		}
	}

	public void ForceKill()
	{
		forceKill = true;
		Hit(3, 666f, true);
	}

	public override string[] PerformAct(int i)
	{
		if (i == 0)
		{
			return new string[2] { "* GLYDE - ATK HIGH DEF HIGH\n* Refuses to give more details\n  about its statistics.", "su_angry`snd_txtsus`* HEY,^05 THAT'S CHEATING!!!" };
		}
		if (GetActNames()[i] == "N!Applaud")
		{
			response = 0;
			AddActPoints(30);
			return new string[2] { "* You and Noelle clap for Glyde.\n* Glyde sucks up your praise\n  like a vacuum cleaner.", "no_confused_side`snd_txtnoe`* H...^05 how did he even\n  do that...?" };
		}
		if (GetActNames()[i] == "S!Boo")
		{
			response = 1;
			strongerAttack = true;
			return new string[3] { "* You and Susie boo Glyde...", "su_wtf`snd_txtsus`* BOO,^05 YOU SUCK ASS!!!", "* ... But haters only make\n  Glyde stronger.\n* Glyde ATTACK UP+DEFENSE DOWN." };
		}
		if (GetActNames()[i] == "SN!Nothing")
		{
			response = 2;
			return new string[2] { "* Everyone does nothing.\n* Nothing happened.", "su_inquisitive`snd_txtsus`* Great use of time." };
		}
		return base.PerformAct(i);
	}

	public override string[] PerformAssistAct(int i)
	{
		switch (i)
		{
		case 1:
			strongerAttack = true;
			AddActPoints(5);
			return new string[2] { "* Susie challenges Glyde's raw\n  strength.", "* Glyde takes this as a\n  challenge!\n* It's ATTACK increased!" };
		case 2:
		{
			GameManager gameManager = Util.GameManager();
			gameManager.SetHP(0, gameManager.GetHP(0) + 5, true);
			gameManager.SetHP(1, gameManager.GetHP(1) + 5, true);
			gameManager.SetHP(2, gameManager.GetHP(2) + 5, true);
			return new string[2] { "* Noelle creates frost rings\n  for Glyde to flip through.", "* Glyde does a flip so\n  incredible that it overheals\n  everyone!" };
		}
		default:
			return base.PerformAssistAct(i);
		}
	}

	public override void Chat(string[] text, string type, string sound, Vector2 pos, bool canSkip, int speed)
	{
		if (response == 0)
		{
			text = new string[1] { "OK!\nI rule!\nI admit \nit!" };
		}
		if (response == 1)
		{
			text = new string[1] { "Mmm, \nFresh \nSweet \nHaters" };
		}
		else if (response == 2)
		{
			text = new string[1] { "..." };
		}
		speed = 1;
		base.Chat(text, type, sound, pos, canSkip, speed);
		chatbox.GetComponent<SwirlingText>().StartSwirl("speechbubble");
	}

	public float GetGGValue()
	{
		return Mathf.Sin(siner / 12f);
	}

	public void TurnDark()
	{
		SpriteRenderer[] componentsInChildren = obj.transform.Find("parts").GetComponentsInChildren<SpriteRenderer>();
		foreach (SpriteRenderer spriteRenderer in componentsInChildren)
		{
			if (spriteRenderer.gameObject.name != "antenna")
			{
				spriteRenderer.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}
	}

	public bool StrongerAttack()
	{
		return strongerAttack;
	}

	public override bool IsKilled()
	{
		if (forceKill)
		{
			return false;
		}
		return base.IsKilled();
	}

	public override bool CanSpare()
	{
		return false;
	}
}
