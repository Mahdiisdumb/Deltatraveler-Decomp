using UnityEngine;

public class UndertalePapyrus : InteractTextBox
{
	private bool doEggSound;

	private bool playOminous;

	private TextBox tempTxt;

	protected override void Awake()
	{
		base.Awake();
		if (Util.GameManager().GetFlagInt(300) == 1)
		{
			SetNewLines();
		}
	}

	protected override void Update()
	{
		if ((bool)txt && txt.GetCurrentSound() != "snd_txtpap")
		{
			tempTxt = txt;
			txt = null;
		}
		else if ((bool)tempTxt && tempTxt.GetCurrentSound() == "snd_txtpap")
		{
			txt = tempTxt;
			tempTxt = null;
		}
		base.Update();
		if ((bool)txt && txt.GetCurrentStringNum() == 6 && doEggSound)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_item");
			doEggSound = false;
		}
		if (!txt && playOminous)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_creepyjingle");
			playOminous = false;
		}
	}

	public override void DoInteract()
	{
		if ((bool)txt)
		{
			return;
		}
		if (talkedToBefore)
		{
			if (Util.GameManager().GetFlagInt(286) == 1)
			{
				if (Util.GameManager().NumItemFreeSpace(equipment: true) == 0)
				{
					txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
					txt.CreateBox(new string[8] { "WHAT'S THIS...?", "AN EGG?", "FOR ME?????", "A PERFECT UNION OF \nPAPYRUS AND EGG!", "IN RETURN,^05 I SHALL \nGIVE A ONE-OF-A-KIND \nITEM!", "... YOU DO NOT HAVE \nANY EQUIPMENT SPACE?", "WELL DO COME BACK \nONCE YOU HAVE MADE \nROOM.", "I AM MORE IMPRESSED \nWITH HOW YOU KNEW \nIT WAS EQUIPMENT!" }, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: true, new string[8] { "pap_side", "pap_blush", "pap_blush", "pap_flush", "pap_neutral", "pap_side", "pap_laugh", "pap_neutral" });
					playOminous = Util.GameManager().GetFlagInt(293) == 0;
					Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
					return;
				}
				Util.GameManager().SetFlag(286, 0);
				Util.GameManager().SetFlag(300, 1);
				Util.GameManager().AddEquipment(42);
				doEggSound = true;
				SetNewLines();
				talkedToBefore = false;
			}
			else if (Util.GameManager().GetFlagInt(292) == 1 && Util.GameManager().GetFlagInt(299) == 0 && Util.GameManager().GetFlagInt(303) == 0)
			{
				Util.GameManager().SetFlag(299, 1);
				txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
				string text = "* (But it's his house...)";
				if (Util.GameManager().GetFlagInt(294) == 1)
				{
					text = "* (Pretty sure we were\n  already there.)";
				}
				if (Util.GameManager().GetFlagInt(293) == 1)
				{
					text = "* (Guess Kris doesn't\n  wanna tell the guy that\n  owns the damn place.)";
				}
				txt.CreateBox(new string[8] { "WHAT THE HECK!!!", "THIS IS THE KEY \nTO THE BASEMENT!", "SANS BORROWED IT A \nLONG TIME AGO AND \nNEVER RETURNED IT.", "MIND CHECKING THE \nBASEMENT FOR ME?", "IT IS THE ENTRANCE \nBEHIND MY HOUSE.", "THE HOUSE WITH THE \nSKELETON FLAG ON IT!", "NYEH HEH HEH!!!", text }, new string[8] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus" }, new int[1], giveBackControl: true, new string[8] { "pap_mad", "pap_mad", "pap_side", "pap_neutral", "pap_neutral", "pap_laugh", "pap_laugh", "su_inquisitive" });
				playOminous = Util.GameManager().GetFlagInt(293) == 0;
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
				return;
			}
		}
		base.DoInteract();
	}

	private void SetNewLines()
	{
		talkedToBefore = true;
		lines = new string[13]
		{
			"WHAT'S THIS...?", "AN EGG?", "FOR ME?????", "A PERFECT UNION OF \nPAPYRUS AND EGG!", "IN RETURN,^05 I SHALL \nGIVE A ONE-OF-A-KIND \nITEM!", "* (Before you could answer,^05\n  Papyrus swaps the Egg for the\n  Papyrus's Charm.)", "* The hell is this\n  dinky thing supposed\n  to do?", "AH,^05 IT IS BUT \nA MAGICAL ITEM.", "THE EQUIPPOR CAN \nNEGATE ONE HIT \nIN BATTLES!", "* Hmm...",
			"* Well,^05 we'll see if\n  it ends up being\n  useful.", "I AM SURE IT \nWILL.", "HAVE FUN WITH IT!!!"
		};
		sounds = new string[13]
		{
			"snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_text", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtsus",
			"snd_txtsus", "snd_txtpap", "snd_txtpap"
		};
		portraits = new string[13]
		{
			"pap_side", "pap_blush", "pap_blush", "pap_flush", "pap_neutral", "", "su_annoyed", "pap_laugh", "pap_neutral", "su_side",
			"su_smirk", "pap_neutral", "pap_neutral"
		};
		lines2 = new string[10] { "SAY...", "DO YOU THINK I'LL \nFIND A HUMAN TODAY?", "* Well,^05 I...", "* Umm,^05 the chances aren't\n  zero...?", "WHAT MAKES YOU \nTHINK THAT?", "* ...^10 The weather?", "I SUPPOSE IT IS \nQUITE MURKY TODAY.", "PERFECT WEATHER FOR \nA HUMAN TO ARRIVE!", "I WILL PREPARE \nHARDER THAN I \nUSUALLY DO TODAY!", "* Uhh,^05 have fun,^05 I\n  guess." };
		sounds2 = new string[10] { "snd_txtpap", "snd_txtpap", "snd_txtnoe", "snd_txtnoe", "snd_txtpap", "snd_txtnoe", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtsus" };
		portraits2 = new string[10] { "pap_neutral", "pap_side", "no_shocked", "no_confused_side", "pap_laugh", "no_weird", "pap_side", "pap_neutral", "pap_evil", "su_smirk_sweat" };
	}
}
