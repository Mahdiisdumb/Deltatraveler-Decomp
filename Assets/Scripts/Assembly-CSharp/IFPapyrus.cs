using System.Collections.Generic;
using UnityEngine;

public class IFPapyrus : InteractSelectionBase
{
	private TextBox tempTxt;

	private List<int> soundAt = new List<int>();

	private bool startingGame;

	private bool choosingMusic;

	private bool disabled;

	private void Awake()
	{
		disabled = Util.GameManager().GetFlagInt(87) >= 7;
		if (disabled)
		{
			ModifyContents(new string[13]
			{
				"OH,^05 HELLO THERE!", "ARE YOU PERHAPS \nDIMENSION TRAVELLING \nLIKE I AM?", "* Okay,^05 the hell is\n  this about?", "WELL,^10 UMM...", "I AM A BIT BORED,^05 \nAND WOULD LIKE TO \nPLAY UNO.", "* Sorry,^05 not in the\n  mood.", "OH...", "IS...^10 IS THERE \nANYTHING I CAN \nDO TO HELP?", "* It's... too complicated.", "OKAY,^05 OKAY.^05\nSORRY.",
				"I HOPE THAT WHATEVER \nIS TROUBLING YOU \nIS RESOLVED SOON!", "* Yeah,^05 we know.^10\n* Thanks.", "* Thank you."
			}, new string[13]
			{
				"snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap", "snd_txtsus", "snd_txtpap",
				"snd_txtpap", "snd_txtsus", "snd_txtnoe"
			}, new int[1], new string[13]
			{
				"ifpap_neutral", "ifpap_confused", "su_annoyed", "ifpap_worry", "ifpap_side", "su_side", "ifpap_side", "ifpap_confused", "su_depressed", "ifpap_worry",
				"ifpap_neutral", "su_neutral", "no_depressed"
			});
		}
	}

	protected override void Update()
	{
		if ((bool)txt && txt.GetCurrentSound() != "snd_txtpap" && !txt.CanLoadSelection())
		{
			tempTxt = txt;
			txt = null;
		}
		else if ((bool)tempTxt && (tempTxt.GetCurrentSound() == "snd_txtpap" || tempTxt.CanLoadSelection()))
		{
			txt = tempTxt;
			tempTxt = null;
		}
		if ((bool)tempTxt && soundAt.Count > 0 && tempTxt.GetCurrentStringNum() == soundAt[0])
		{
			soundAt.RemoveAt(0);
			Util.GameManager().PlayGlobalSFX("sounds/snd_item");
		}
		if (!txt && choosingMusic && !startingGame)
		{
			Util.GameManager().StopMusic(15f);
			Object.Instantiate(Resources.Load<GameObject>("uno/MusicChooser"), GameObject.Find("Canvas").transform);
			startingGame = true;
		}
		else if (startingGame && choosingMusic && !Util.FindObjectOfType<MusicChooser>())
		{
			txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
			if (MusicChooser.musicID == MusicChooser.FRANKNESS_ID)
			{
				txt.CreateBox(new string[3] { "SO YOU ARE \nCHOOSING VIOLENCE,^05 \nEH?", "THEN I SHAN'T \nRESTRAIN MYSELF!", "NYEH HEH HEH!^05\nGET READY!" }, new string[3] { "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], giveBackControl: false, new string[3] { "ifpap_side", "ifpap_evil", "ifpap_laugh" });
				txt.AddRemark(new Remark(3, "su_inquisitive", "(Oh boy...)", "br"));
			}
			else
			{
				txt.CreateBox(new string[1] { "NYEH HEH HEH!^05\nLET US BEGIN!" }, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: false, new string[1] { "ifpap_neutral" });
			}
			choosingMusic = false;
		}
		else if (!txt && !tempTxt && startingGame && !Util.FindObjectOfType<MusicChooser>())
		{
			startingGame = false;
			Util.GameManager().SetPartyMembers(susie: false, noelle: false);
			Util.OverworldPlayer().InitiateBattle(75);
		}
		base.Update();
	}

	public override void DoInteract()
	{
		if (Util.GameManager().GetFlagInt(307) == 1)
		{
			lines = new string[1] { disabled ? "I HOPE YOU THREE \nGET BETTER SOON." : "WOULD YOU THREE \nLIKE TO PLAY UNO?" };
			if (disabled)
			{
				portraits = new string[1] { "ifpap_worry" };
			}
		}
		else
		{
			Util.GameManager().SetFlag(307, 1);
		}
		if (disabled)
		{
			if (!txt && enabled)
			{
				CreateTextBox(lines, sounds, speed, giveBackControl: true, portraits, remarks);
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			}
		}
		else
		{
			base.DoInteract();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
		if (index == Vector2.left)
		{
			txt.CreateBox(new string[2] { "WONDERFUL!", "WANT TO CHANGE THE \nMUSIC BEFORE\nSTARTING?" }, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: false, new string[2] { "ifpap_neutral", "ifpap_laugh" });
			choosingMusic = true;
		}
		else if (index == Vector2.right)
		{
			txt.CreateBox(new string[1] { "OKAY!^05\nDO COME BACK IF \nYOU DO." }, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: true, new string[1] { "ifpap_neutral" });
		}
		else if (index == Vector2.up)
		{
			txt.CreateBox(new string[45]
			{
				"YOU NEED A RUNDOWN \nOF THE RULES?", "OKAY!", "UNO IS A CARD \nGAME.", "THE FIRST PERSON \nTO GET RID OF \nTHEIR CARDS WINS.", "YOU START OUT WITH \n7 CARDS.", "THE FIRST PERSON TO \nPLAY GETS TO PUT \nDOWN THE FIRST CARD.", "EACH CARD HAS A \nSPECIFIC COLOR AND \nA SPECIFIC TYPE.", "YOU CAN ONLY PUT \nDOWN A CARD THAT \nIS THE SAME", "COLOR OR TYPE OF \nTHE LAST CARD \nPLAYED.", "THERE ARE FOUR \nDISTINCT COLORS:",
				"<color=#FF0000FF>RED</color>, <color=#FFFF00FF>YELLOW</color>, <color=#00C000FF>GREEN</color>, \nAND <color=#003CFFFF>BLUE</color>.", "BUT THERE IS ONE \nEXCEPTION TO COLOR:^05\nWILD CARDS.", "THESE CARDS CAN BE \nPLAYED REGARDLESS \nOF COLOR OR TYPE.", "AND ALLOW YOU TO \nPICK THE COLOR.", "BUT WILD CARDS ARE \nA TYPE OF CARD.", "OTHER TYPES OF \nCARDS ARE NUMBER \nCARDS,^05 FROM 0 TO 9.", "SKIP CARDS LET YOU \nSKIP THE TURN OF \nTHE NEXT PERSON.", "REVERSE CARDS \nREVERSE TURN ORDER.", "BUT IF USED WHEN \n<color=#FFFF00FF>2 PLAYERS ARE LEFT</color>,^05 \nTHEY ACT AS SKIP.", "THERE ARE WILD \nCARDS,^05 AS ALREADY \nSTATED.",
				"+2 CARDS FORCE THE \nNEXT PLAYER TO \nPICK UP 2 CARDS.", "AS WELL AS SKIP \nTHEIR TURN.", "HOWEVER,^05 THAT PLAYER \nCAN <color=#FFFF00FF>COUNTER WITH \nANOTHER ADD CARD</color>.", "WHICH INCREASES THE \nNUMBER OF CARDS TO \nPICK UP...", "GOING TO THE NEXT \nPERSON AFTER THAT.", "THIS ALSO APPLIES \nTO WILD +4 CARDS.", "ALLOWING YOU TO \nCHANGE THE COLOR \nAND ADD 4 CARDS", "TO THE NEXT PERSON.", "+2 AND +4 STACKING \nARE INTER-COMPATIBLE.", "HOWEVER,^05 YOU CAN \n<color=#FFFF00FF>ONLY PLAY +4 IF YOU \nCAN'T PLAY ANYTHING</color>.",
				"IF YOU DO,^05 THE \nPLAYER THAT'S FORCED \nTO PICK UP", "CAN CHALLENGE YOUR \nDECISION.", "<color=#FFFF00FF>YOU MUST SHOW \nEVERYONE YOUR ENTIRE \nHAND.</color>", "IF THEIR CHALLENGE \nLOSES,^05 THEY MUST \nPICK UP <color=#FFFF00FF>2 EXTRA</color>.", "BUT IF IT WINS, \n<color=#FFFF00FF>YOU PICK UP THE \nCARDS INSTEAD</color>.", "THAT INCLUDES \nANYTHING THAT'S \nIN THE STACK.", "THAT WAS A LOT \nOF WORDS,^05 BUT I \nHOPE THIS HELPS.", "WAIT,^05 I THINK \nI'M FORGETTING \nSOMETHING...", "OH RIGHT!!!^05\nCALLING UNO!", "YOU MUST CALL \"UNO\" \nWHEN PLAYING YOUR \nLAST CARD.",
				"I BELIEVE YOU \nDO A KIND OF \n^C MANEUVER...", "YOU CAN PREPARE \nONE BEFORE PLAYING \nTHE 2nd LAST CARD.", "BUT IF YOU FAIL \nTO CALL BEFORE \nSOMEONE ELSE...", "YOU MUST PICK UP \n<color=#FFFF00FF>4 MORE CARDS</color>!", "THERE WE ARE!^05\nALL OF THE RULES!"
			}, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: true, new string[45]
			{
				"ifpap_side", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral",
				"ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral",
				"ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral",
				"ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_side", "ifpap_wacky", "ifpap_neutral",
				"ifpap_side", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral"
			});
		}
		else
		{
			if (!(index == Vector2.down))
			{
				return;
			}
			if (Util.GameManager().GetFlagInt(292) == 1 && Util.GameManager().GetFlagInt(309) == 1 && Util.GameManager().GetFlagInt(311) == 1)
			{
				txt.CreateBox(new string[2] { "WOW,^05 YOU HAVE WON \nALL OF MY PRIZES!", "I HOPE THAT YOU \nARE ENJOYING THEM." }, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: true, new string[2] { "ifpap_wacky", "ifpap_neutral" });
				return;
			}
			bool flag = Util.GameManager().GetFlagInt(308) == 1 && Util.GameManager().GetFlagInt(309) == 0;
			bool flag2 = Util.GameManager().GetFlagInt(310) == 1 && Util.GameManager().GetFlagInt(311) == 0;
			if (flag2 || flag)
			{
				bool flag3 = flag2 && flag && Util.GameManager().NumItemFreeSpace(equipment: false) == 1;
				bool num = Util.GameManager().NumItemFreeSpace(equipment: false) == 0;
				string arg = ((flag && flag2) ? "PRIZES" : "PRIZE");
				if (num)
				{
					txt.CreateBox(new string[3]
					{
						$"YOU DIDN'T EVEN \nMAKE SPACE FOR \nTHE {arg}?",
						"THAT IS QUITE...^05 \nFRISK-LIKE OF YOU.",
						"COME BACK WHEN \nYOU'VE ACTUALLY \nMADE ROOM."
					}, new string[1] { "snd_txtpap" }, new int[1], giveBackControl: true, new string[3] { "ifpap_confused", "ifpap_side", "ifpap_neutral" });
				}
				else if (flag3)
				{
					Util.GameManager().SetFlag(311, 1);
					Util.GameManager().AddItem(45);
					txt.CreateBox(new string[5] { "YOU ONLY HAVE \nROOM FOR ONE?", "THAT IS QUITE...^05 \nFRISK-LIKE OF YOU.", "WELL,^05 HERE'S ONE \nOF THEM!", "* (You got the WILD REVERSE\n  CARD.)^05\n* (It was added to your ITEMs.)", "COME BACK WHEN \nYOU'VE ACTUALLY MADE \nROOM FOR THE OTHER." }, new string[5] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_text", "snd_txtpap" }, new int[1], giveBackControl: true, new string[5] { "ifpap_confused", "ifpap_side", "ifpap_neutral", "", "ifpap_neutral" });
					soundAt.Add(4);
				}
				else
				{
					int num2 = 2;
					List<string> list = new List<string> { $"ALRIGHT,^05 HERE'S THE \n{arg} THAT YOU \nHAVE EARNED!" };
					if (flag2)
					{
						Util.GameManager().SetFlag(311, 1);
						soundAt.Add(num2++);
						Util.GameManager().AddItem(45);
						list.Add("* (You got the WILD REVERSE\n  CARD.)^05\n* (It was added to your ITEMs.)");
					}
					if (flag)
					{
						Util.GameManager().SetFlag(309, 1);
						soundAt.Add(num2++);
						Util.GameManager().AddItem(44);
						list.Add("* (You got the Spaghetti.)^05\n* (It was added to your ITEMs.)");
					}
					txt.CreateBox(list.ToArray(), new string[2] { "snd_txtpap", "snd_text" }, new int[1], giveBackControl: true, new string[2] { "ifpap_neutral", "" });
				}
			}
			else
			{
				List<string> list2 = new List<string> { "SO I HAVE SOME \nPRIZES BASED ON\nYOUR PLACEMENT.", "THERE IS ONE FOR \n3rd,^05 ONE FOR 2nd,^05\nAND ONE FOR 1st.", "WINNING HIGHER WILL \nALSO WIN THE LOWER \nONES AS WELL.", "ONE OF THESE IS \nA KEY ITEM,^05 SO \nIT TAKES NO SPACE.", "HOWEVER, ALL OF \nTHEM ARE IN LIMITED \nSUPPLY!", "HOW WELL DO YOU \nTHINK YOU'LL DO?^05\nNYEH HEH HEH!" };
				List<string> list3 = new List<string> { "ifpap_side", "ifpap_neutral", "ifpap_neutral", "ifpap_neutral", "ifpap_laugh", "ifpap_laugh" };
				if (Util.GameManager().GetFlagInt(310) == 1)
				{
					string arg2 = ((Util.GameManager().GetFlagInt(308) == 1) ? "ONE" : "TWO");
					list2.AddRange(new string[2]
					{
						$"I STILL HAVE {arg2} \nOF THE THREE \nPRIZES.",
						"SEE IF YOU CAN \nSCORE HIGHER THAN \nYOU HAVE BEEN!"
					});
					list3.AddRange(new string[2] { "ifpap_side", "ifpap_laugh" });
				}
				txt.CreateBox(list2.ToArray(), new string[1] { "snd_txtpap" }, new int[1], giveBackControl: true, list3.ToArray());
			}
		}
	}
}
