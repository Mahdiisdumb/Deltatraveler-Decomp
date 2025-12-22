using UnityEngine;

public class SansShopBase : InteractSelectionBase
{
	[SerializeField]
	private GameObject shopObject;

	private bool preparingToLoadShop;

	private void Awake()
	{
		left = "Shop";
		up = "Talk";
		right = "Nothing";
		rightOffset = new Vector2(-20f, 0f);
		down = "";
	}

	private void Start()
	{
		if (Util.GameManager().GetCurrentZone() == 80)
		{
			if ((int)Util.GameManager().GetFlag(186) == 1)
			{
				SetSecondaryLines();
			}
			else if (!Util.GameManager().SusieInParty())
			{
				sounds = new string[1] { "snd_txtsans" };
				lines[0] = "* say,^05 where'd the other\n  two go?";
				lines[1] = "* didja do something to\n  upset 'em?";
				lines[2] = "* (...^10 not surprising.)";
				lines[3] = "* well,^05 since you're alone\n  you should prolly know\n  a few things about battle.";
				portraits[0] = "sans_side";
				portraits[1] = "sans_neutral";
				portraits[2] = "sans_closed";
				lines[9] = "* huh?";
				lines[10] = "* you saw orange bullets\n  last fight?";
				portraits[9] = "sans_neutral";
				portraits[10] = "sans_neutral";
			}
			else if ((int)Util.GameManager().GetFlag(251) == 1)
			{
				lines[9] = "* Yeah,^05 we were told.";
				portraits[9] = "su_annoyed";
			}
		}
	}

	protected override void Update()
	{
		base.Update();
		if (preparingToLoadShop && !Util.FindObjectOfType<Fade>().IsPlaying())
		{
			preparingToLoadShop = false;
			Object.Instantiate(shopObject, GameObject.Find("Canvas").transform);
		}
	}

	public override void DoInteract()
	{
		if (!txt && enabled)
		{
			string[] stuffToSay = new string[1] { "* what's up?" };
			string[] sound = new string[1] { "snd_txtsans" };
			int[] array = new int[1];
			string[] portraitNames = new string[1] { "sans_wink" };
			if (Util.GameManager().GetCurrentZone() > 71)
			{
				Util.GameManager().SetFlag(67, 1);
			}
			if ((int)Util.GameManager().GetFlag(67) == 0)
			{
				stuffToSay = new string[7] { "* heya.", "* WHAT THE HELL ARE YOU\n  DOING HERE?!?!", "* oh,^05 sorry about not \n  knockin' on the door\n  before entering.", "* besides, you could\n  probably use my help.", "* And how are you\n  going to help us,^05\n  exactly...?", "* by selling you useful\n  stuff that i don't need,^05\n  of course.", "* whaddya want?" };
				sound = new string[7] { "snd_txtsans", "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtnoe", "snd_txtsans", "snd_txtsans" };
				array = new int[7];
				portraitNames = new string[7] { "sans_neutral", "su_wtf", "sans_wink", "sans_side", "no_confused", "sans_wink", "sans_neutral" };
				Util.GameManager().SetFlag(67, 1);
			}
			txt = new GameObject("InteractTextBoxSansShop", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(stuffToSay, sound, array, giveBackControl: false, portraitNames);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			txt.EnableSelectionAtEnd();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			preparingToLoadShop = true;
			Util.FindObjectOfType<Fade>().FadeOut(7);
		}
		else if (index == Vector2.up)
		{
			txt = new GameObject("InteractTextBoxSansShop", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(lines, sounds, speed, giveBackControl: true, portraits);
			SetSecondaryLines();
		}
		else if (index == Vector2.right)
		{
			txt = new GameObject("InteractTextBoxSansShop", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* see ya." }, new string[1] { "snd_txtsans" }, new int[1], giveBackControl: true, new string[1] { "sans_wink" });
		}
		selectActivated = false;
	}

	private void SetSecondaryLines()
	{
		if (Util.GameManager().GetCurrentZone() == 80)
		{
			lines = new string[1] { "* remember...^10\n* blue stop signs.^10\n* orange go signs." };
			sounds = new string[1] { "snd_txtsans" };
			portraits = new string[1] { "sans_neutral" };
			Util.GameManager().SetFlag(186, 1);
		}
	}
}
