using UnityEngine;

public class CinnamonOrBscotchCallCutscene : CutsceneBase
{
	private bool selecting;

	private string krisChoice;

	private string susieChoice;

	private void Update()
	{
		if (state == 0 && (bool)txt && txt.CanLoadSelection() && !selecting)
		{
			InitiateDeltaSelection();
			select.SetupChoice(Vector2.left, "Cinnamon", Vector3.zero);
			select.SetupChoice(Vector2.right, "Butterscotch", new Vector3(-90f, 0f));
			select.SetCenterOffset(new Vector2(-11f, 0f));
			select.Activate(this, 0, txt.gameObject);
			selecting = true;
		}
		if (state == 1 && !txt)
		{
			EndCutscene();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.right)
		{
			gm.SetFlag(18, 1);
			krisChoice = "Butterscotch";
			susieChoice = "cinnamon";
		}
		else
		{
			gm.SetFlag(18, 0);
		}
		StartText(Localizer.FormatArray(new string[8] { "* {0}?^10\n* I see.", "* And Susie?^10\n* What would you prefer?", "* ...Me?", "* (If she doesn't have\n  like chalk or snails,^10\n  then...)", "* I guess {1}.", "* Ah, I see!", "* {0} and\n  {1}.\n^10* Thank you very much!", "* Click..." }, krisChoice, susieChoice), new string[8] { "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_text" }, new int[18], new string[8] { "tori_happy", "tori_neutral", "su_surprised", "su_side_sweat", "su_smile", "tori_happy", "tori_happy", "" });
		state = 1;
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		krisChoice = "Cinnamon";
		susieChoice = "butterscotch";
		PlaySFX("sounds/snd_dial");
		string[] text = ((gm.GetFlagInt(108) != 1) ? new string[5] { "* Ring...", "* Hello.^10\n* This is TORIEL.", "* I suppose you are\n  already aware of my\n  affinity for pie,^10 yes?", "* Well,^10 I would like to\n  know your flavor\n  preferences.", "* Kris,^10 do you prefer\n  cinnamon or\n  butterscotch?" } : new string[4] { "* Ring...", "* Hello.^10\n* This is TORIEL.", "* For no reason in\n  particular...^05\n* Which do you prefer?", "* Cinnamon or\n  butterscotch?" });
		StartText(text, new string[5] { "snd_text", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[5]
		{
			"",
			"tori_neutral",
			((int)gm.GetFlag(108) == 1) ? "tori_neutral" : "tori_worry",
			"tori_neutral",
			"tori_neutral"
		});
		txt.EnableSelectionAtEnd();
		gm.SetFlag(17, 1);
	}
}
