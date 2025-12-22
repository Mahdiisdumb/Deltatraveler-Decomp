using System;
using UnityEngine;

public class TorielChair : Interactable
{
	private bool torielInChair;

	private int torielDialogue;

	private bool talkedToBefore;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(53) == 1 && (int)Util.GameManager().GetFlag(56) == 0)
		{
			torielInChair = true;
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/toriel_home/spr_chairiel_withtoriel");
		}
		torielDialogue = (int)Util.GameManager().GetFlag(55);
	}

	public override void DoInteract()
	{
		if ((bool)txt)
		{
			return;
		}
		TextBox component = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
		if (torielInChair)
		{
			if (torielDialogue == 0)
			{
				component.CreateBox(new string[6]
				{
					"* My,^10 look who has\n  gotten up!",
					"* You look very well\n  rested.",
					"* So,^10 do you two\n  plan on going soon?",
					((int)Util.GameManager().GetFlag(108) == 1) ? "* No idea." : "* I dunno.^10\n* I'm mostly just\n  following Kris.",
					"* Very well then.",
					"* I hope you come\n  talk to me before\n  you go."
				}, new string[6] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[9], new string[6] { "tori_happy", "tori_happy", "tori_neutral", "su_side", "tori_worry", "tori_worry" });
			}
			else if (torielDialogue == 1)
			{
				if ((int)Util.GameManager().GetFlag(108) == 1)
				{
					component.CreateBox(new string[6] { "* Speaking of going\n  soon...", "* I have an errand\n  of my own to tend\n  to.", "* You two landed on\n  the bed of flowers,^10\n  did you not?", "* Yeah...?", "* I will need to\n  go check on them\n  very soon.", "* Flowers are quite\n  fragile,^05 you know." }, new string[9] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[14], new string[6] { "tori_neutral", "tori_neutral", "tori_worry", "su_smirk_sweat", "tori_worry", "tori_wack" });
				}
				else
				{
					component.CreateBox(new string[9] { "* Speaking of going\n  soon...", "* I have an errand\n  of my own to tend\n  to.", "* You two landed on\n  the bed of flowers,^10\n  did you not?", "* Yeah...?", "* I will need to\n  go check on them\n  very soon.", "* ...Checking it before\n  you leave...?", "* No,^10 do not worry\n  about that.", "* It is just inspection\n  and watering.", "* I would not want to\n  pain you with more\n  traveling." }, new string[9] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[14], new string[9] { "tori_neutral", "tori_neutral", "tori_worry", "su_smirk_sweat", "tori_worry", "tori_blush", "tori_worry", "tori_wack", "tori_worry" });
				}
			}
			else if (torielDialogue == 2)
			{
				if ((int)Util.GameManager().GetFlag(108) == 1)
				{
					component.CreateBox(new string[1] { "* My child,^05 I hope that\n  you are making good\n  decisions." }, new string[9] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[14], new string[6] { "tori_worry", "tori_neutral", "tori_worry", "su_smirk_sweat", "tori_worry", "tori_wack" });
				}
				else
				{
					component.CreateBox(new string[2] { "* Kris,^10 your mother is\n  worried about you.", "* You should probably\n  start on your way\n  soon." }, new string[2] { "snd_txttor", "snd_txttor" }, new int[14], new string[2] { "tori_worry", "tori_worry" });
				}
			}
			if (torielDialogue < 2)
			{
				torielDialogue++;
				Util.GameManager().SetFlag(55, torielDialogue);
			}
		}
		else
		{
			if ((int)Util.GameManager().GetFlag(108) == 1)
			{
				component.CreateBox(new string[1] { "* (Seems like the right size\n  for Toriel.)" });
			}
			else if (talkedToBefore)
			{
				component.CreateBox(new string[1] { "* (You refuse to believe that\n  this is not Chairiel.)" });
			}
			else
			{
				component.CreateBox(new string[2] { "* (It's Toriel's most\n  comfortable sitting chair.)", "* (It's so cozy,^10 that it's hard\n  to believe that this isn't\n  this world's Chairiel.)" });
			}
			talkedToBefore = true;
		}
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
	}

	public override int GetEventData()
	{
		throw new NotImplementedException();
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		throw new NotImplementedException();
	}
}
