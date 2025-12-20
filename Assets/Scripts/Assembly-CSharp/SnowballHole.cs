using UnityEngine;

public class SnowballHole : InteractTextBox
{
	[SerializeField]
	private Sprite[] flags;

	private Transform flag;

	private bool flagRaise;

	private int flagFrames;

	private int goldAward;

	protected override void Awake()
	{
		base.Awake();
		flag = base.transform.GetChild(0);
		if ((int)Util.GameManager().GetFlag(191) == 0)
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public override void DoInteract()
	{
		if ((int)Util.GameManager().GetFlag(191) == 1)
		{
			if (!txt && enabled && flagRaise && !talkedToBefore)
			{
				Util.GameManager().AddGold(goldAward);
			}
			base.DoInteract();
		}
	}

	protected override void Update()
	{
		if (flagRaise && flag.localPosition.y < 0.648f)
		{
			flag.localPosition += new Vector3(0f, 1f / 24f);
			if (flag.localPosition.y > 0.648f)
			{
				flag.localPosition = new Vector3(flag.localPosition.x, 0.648f);
			}
		}
		if (GetComponent<BoxCollider2D>().enabled && (int)Util.GameManager().GetFlag(191) == 0)
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public void ResetFlag()
	{
		flagRaise = false;
		talkedToBefore = false;
		secondaryLines = false;
		lines = new string[1] { "* There's a hole here." };
		flag.localPosition = new Vector3(flag.localPosition.x, -0.65f);
	}

	public void RaiseFlag(int timer, float size)
	{
		SpriteRenderer component = flag.GetComponent<SpriteRenderer>();
		if (size < 0.6f)
		{
			component.sprite = flags[6];
			lines = new string[4] { "* <color=#00A2E8>LIGHT BLUE</color> - \"Ball\" is \"Small.\"^10\n* You waited,^05 still,^05 for\n  this opportunity...", "* ... then dethroned \"Ball\" with\n  a sharp attack.", "* ... For some reason,^05 this\n  seems to resonate with you.", "" };
			goldAward = 4;
		}
		else if (timer > 600)
		{
			component.sprite = flags[5];
			lines = new string[2] { "* <color=#D535D9>PURPLE</color> - Even when you felt\n  trapped,^05 you took notes and\n  achieved the end of \"Ball.\"", "" };
			goldAward = 2;
		}
		else if (timer > 450)
		{
			component.sprite = flags[4];
			lines = new string[2] { "* <color=#00C000>GREEN</color> - Your concern and care\n  for \"Ball\" led you to a\n  delicious victory.", "" };
			goldAward = 1;
		}
		else if (timer > 330)
		{
			component.sprite = flags[3];
			lines = new string[2] { "* <color=#003CFFFF>BLUE</color> - Hopping and twirling,^05\n  your original style\n  pulled you through.", "" };
			goldAward = 2;
		}
		else if (timer > 240)
		{
			component.sprite = flags[2];
			lines = new string[2] { "* <color=#FFFF00FF>YELLOW</color> - Your sure-fire\n  accuracy put an end to\n  the mayhem of \"Ball.\"", "" };
			goldAward = 3;
		}
		else if (timer > 180)
		{
			component.sprite = flags[1];
			lines = new string[2] { "* <color=#FCA600>ORANGE</color> - You are the kind of\n  person who rushes fists-first\n  through all obstacles.", "" };
			goldAward = 5;
		}
		else
		{
			component.sprite = flags[0];
			if ((int)Util.GameManager().GetFlag(192) != 1)
			{
				Util.GameManager().SetFlag(192, 1);
				lines = new string[3] { "* <color=#FCA600>Bravery.</color>^10 <color=#FFFF00FF>Justice.</color>^10\n* <color=#003CFFFF>Integrity.</color>^10 <color=#00C000>Kindness.</color>^10\n* <color=#D535D9>Perserverance.</color>^10 <color=#00A2E8>Patience.</color>", "* Using these,^05 you were\n  able to win at \"<color=#FF0000FF>Ball Game</color>.\"", "" };
				goldAward = 50;
			}
			else
			{
				lines = new string[3] { "* <color=#FF0000FF>RED</color> - Try as you might,^05\n  you continue to be yourself.", "* ... For some reason, this makes\n  you feel uneasy.", "" };
				goldAward = 10;
			}
		}
		flagRaise = true;
		secondaryLines = true;
		talkedToBefore = false;
		lines[lines.Length - 1] = "* (You are awarded " + goldAward + "G.)";
	}
}
