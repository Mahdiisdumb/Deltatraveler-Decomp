using UnityEngine;

public class PapXOPuzzleENd : CutsceneBase
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
			if (frames == 10)
			{
				RevokePlayerControl();
				susie.UseUnhappySprites();
				noelle.UseUnhappySprites();
				ChangeDirection(kris, papyrus.transform.position - kris.transform.position);
				ChangeDirection(susie, papyrus.transform.position - susie.transform.position);
				ChangeDirection(noelle, papyrus.transform.position - noelle.transform.position);
				StartText(new string[5] { "SO YOU HAVE \nTRIUMPHED OVER THE \nPUZZLE...", "AND IT DID NOT \nEXPLODE^05, EITHER!^05\nIMPRESSIVE!", "BUT THE NEXT PUZZLE \nSHALL NOT BE SO \nSAFE!!!", "I HOPE YOU'RE SKILLED \nAT FOLLOWING SETS \nOF RULES!", "HEH HEH HEH HEH \nNYEH!!!" }, new string[5] { "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap", "snd_txtpap" }, new int[1], new string[5] { "ufpap_side", "ufpap_neutral", "ufpap_evil", "ufpap_side", "ufpap_evil" });
				papyrus.SetTalkable(txt);
				state = 1;
			}
		}
		else if (state == 1 && !txt)
		{
			if (!MoveTo(papyrus, new Vector3(17.2f, 1.54f), 8f))
			{
				Object.Destroy(papyrus.gameObject);
				state = 2;
				if (Util.GameManager().GetFlagInt(87) < 7)
				{
					StartText(new string[8] { "* Why can't WE climb\n  over those rocks,^05 Kris?", "* That'd make this puzzle\n  a lot easier.", "* Easy for you,^05 I\n  think.", "* Kris is pretty short.", "* Oh,^05 right.", "* Wait,^05 what if we\n  stretched them to make\n  them taller?", "* ...^05 Actually,^05 nah that\n  wouldn't work with\n  humans.", "* Let's go." }, new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[8] { "su_annoyed", "su_smirk_sweat", "no_playful", "no_neutral", "su_side", "su_surprised", "su_inquisitive", "su_neutral" });
					ChangeDirection(kris, susie.transform.position - kris.transform.position);
					ChangeDirection(susie, kris.transform.position - susie.transform.position);
					ChangeDirection(noelle, susie.transform.position - noelle.transform.position);
				}
			}
			else
			{
				SetMoveAnim(papyrus, true);
				ChangeDirection(papyrus, Vector2.right);
				ChangeDirection(kris, papyrus.transform.position - kris.transform.position);
				ChangeDirection(susie, papyrus.transform.position - susie.transform.position);
				ChangeDirection(noelle, papyrus.transform.position - noelle.transform.position);
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
				if (AtLine(3))
				{
					noelle.UseHappySprites();
					ChangeDirection(susie, noelle.transform.position - susie.transform.position);
				}
				else if (AtLine(8))
				{
					ChangeDirection(susie, kris.transform.position - susie.transform.position);
				}
			}
			else
			{
				RestorePlayerControl();
				ChangeDirection(kris, Vector2.down);
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(-5.61f, -0.24f), Quaternion.identity);
				EndCutscene();
				state = 3;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		papyrus = Object.FindObjectOfType<InteractPapyrusTextbox>();
		papyrus.StopStare();
	}
}
