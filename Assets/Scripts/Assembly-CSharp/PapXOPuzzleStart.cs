using UnityEngine;

public class PapXOPuzzleStart : CutsceneBase
{
	private InteractPapyrusTextbox papyrus;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				papyrus.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				StartText(new string[7] { "OH,^05 HELLO THERE.", "* YOU!!!", "* Why the hell weren't\n  you keeping an eye\n  on Sans???!", "WHAT ARE YOU \nTALKING ABOUT?", "* WE GOT AMBUSHED\n  BY HIM,^05 DUMBASS!!!", "WHAT???^05\nTHAT'S RIDICULOUS!!!", "HE'S BEEN DILIGENTLY \nLISTENING TO ME TALK,^05\nRIGHT TO THE SID--" }, new string[7] { "snd_txtpap", "snd_txtsus", "snd_txtsus", "snd_txtpap", "snd_txtsus", "snd_txtpap", "snd_txtpap" }, new int[1], new string[7] { "ufpap_neutral", "su_wtf", "su_pissed", "ufpap_side", "su_angry", "ufpap_mad", "ufpap_neutral" });
				papyrus.SetTalkable(txt);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(2) || AtLine(5))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(3))
				{
					SetSprite(susie, "spr_su_throw_ready");
				}
				else if (AtLine(6))
				{
					susie.EnableAnimator();
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(papyrus, Vector2.up);
			}
			if (frames == 60)
			{
				ChangeDirection(papyrus, Vector2.left);
				StartText(new string[20]
				{
					"I...^05 THOUGHT HE WAS \nBEING UNUSUALLY \nQUIET.",
					"* YOU THINK?!?!?!?",
					"WHERE COULD HE...?",
					"OH,^05 HE'S OVER THERE \nAT THE NEXT PUZZLE.",
					"SO HE DID GET \nAWAY BEFORE I COULD \nGET OVER THERE!!!",
					"APOLOGIES FOR THAT.",
					"I DO KNOW THAT \nHE IS AT THE \nPUZZLE AHEAD.",
					"I CAN EVEN SEE \nHIM FROM HERE!",
					"SO YOU ALL ARE \nSAFE FROM HIM FOR \nNOW.",
					"BUT,^05 AH...",
					"THERE IS ANOTHER \nPUZZLE HERE FOR \nYOU TO SOLVE.",
					"I WAS GOING TO \nMAKE IT INTO THE \nSHAPE OF MY FACE...",
					"BUT SANS WANTED IT \nTO BE MORE \nINTIMIDATING.",
					"SO I MADE IT INTO \nHIS FACE INSTEAD!!",
					"BUT I AM UNSURE \nOF THE SOLUTION.",
					"FEEL FREE TO STAND \nBY WHILE I FIGURE \nIT OUT.",
					"YOU TOTALLY SHOULD \nNOT PUSH ANY COOL \nRED BUTTONS...",
					"THAT ARE JUST IDLY \nLAYING ABOUT.",
					"NYEH HEH HEH...",
					(Util.GameManager().GetFlagInt(211) == 1) ? "* (You kinda want to push the\n  button...)" : "* (... Don't push the\n  red button,^05 Kris.)"
				}, new string[20]
				{
					"snd_txtpap",
					"snd_txtsus",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					"snd_txtpap",
					(Util.GameManager().GetFlagInt(211) == 1) ? "snd_text" : "snd_txtnoe"
				}, new int[1], new string[20]
				{
					"ufpap_worry",
					"su_wtf",
					"ufpap_side",
					"ufpap_side",
					"ufpap_mad",
					"ufpap_neutral",
					"ufpap_side",
					"ufpap_neutral",
					"ufpap_neutral",
					"ufpap_laugh",
					"ufpap_laugh",
					"ufpap_side",
					"ufpap_side",
					"ufpap_neutral",
					"ufpap_side",
					"ufpap_neutral",
					"ufpap_laugh",
					"ufpap_side",
					"ufpap_laugh",
					(Util.GameManager().GetFlagInt(211) == 1) ? "" : "no_shocked"
				});
				papyrus.SetTalkable(txt);
				state = 2;
			}
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(susie, "spr_su_wtf");
				}
				else if (AtLine(4))
				{
					ChangeDirection(papyrus, Vector2.right);
				}
				else if (AtLine(5))
				{
					susie.EnableAnimator();
					ChangeDirection(papyrus, Vector2.up);
				}
				else if (AtLine(6) || AtLine(9) || AtLine(16))
				{
					ChangeDirection(papyrus, Vector2.left);
				}
				else if (AtLine(8) || AtLine(14))
				{
					ChangeDirection(papyrus, Vector2.right);
				}
				else if (AtLine(10))
				{
					papyrus.SetTalkable(null);
					PlayAnimation(papyrus, "Pose");
				}
				else if (AtLine(12))
				{
					papyrus.SetTalkable(txt);
					PlayAnimation(papyrus, "idle");
					ChangeDirection(papyrus, Vector2.up);
				}
				else if (AtLine(15) || AtLine(17))
				{
					ChangeDirection(papyrus, Vector2.up);
				}
			}
			else
			{
				papyrus.Stare();
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		papyrus = Object.FindObjectOfType<InteractPapyrusTextbox>();
		ChangeDirection(papyrus, Vector2.left);
		papyrus.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		SetSprite(susie, "spr_su_surprise_right");
		PlaySFX("sounds/snd_encounter");
		gm.SetFlag(236, 1);
	}
}
