using UnityEngine;

public class NapstaDefeatCutscene : CutsceneBase
{
	private bool selecting;

	private int endState;

	private bool hardmode;

	private void Update()
	{
		if (state == 0)
		{
			if ((bool)txt)
			{
				if (endState == 1 && txt.CanLoadSelection() && !selecting)
				{
					selecting = true;
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "We need to\nget stronger", Vector3.zero);
					select.SetupChoice(Vector2.right, "...", new Vector3(38f, 0f));
					select.Activate(this, 0, txt.gameObject);
				}
			}
			else if (endState != 1 || (int)gm.GetFlag(12) == 0)
			{
				WeirdChecker.Abort(Object.FindObjectOfType<GameManager>());
				gm.SetCheckpoint(22);
				EndCutscene();
			}
		}
		if (state != 1)
		{
			return;
		}
		if (!txt)
		{
			gm.PlayGlobalSFX("sounds/snd_ominous");
			gm.SetFlag(1, "side_sweat");
			gm.SetCheckpoint(22);
			EndCutscene();
		}
		else if (hardmode)
		{
			if (txt.GetCurrentStringNum() == 5)
			{
				kris.ChangeDirection(Vector2.up);
			}
			if (txt.GetCurrentStringNum() == 7)
			{
				susie.ChangeDirection(Vector2.left);
			}
			if (txt.GetCurrentStringNum() == 8)
			{
				susie.ChangeDirection(Vector2.up);
				kris.ChangeDirection(Vector2.left);
			}
			if (txt.GetCurrentStringNum() == 11)
			{
				susie.ChangeDirection(Vector2.right);
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			StartText(new string[4] { "* ...What?", "* We're just gonna be\n  going back to our\n  world after this.", "* We REALLY don't need\n  to do this.", "* Just...^05 try to cool\n  it,^10 alright?" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[15], new string[4] { "su_surprised", "su_smile_sweat", "su_annoyed", "su_dejected" });
		}
		else if (index == Vector2.right)
		{
			StartText(new string[3] { "* ...", "* Look, just try to\n  cool it,^05 alright?", "* This feels really\n  messed up." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_side_sweat", "su_annoyed", "su_dejected", "su_teeth_eyes", "su_smile_side" });
		}
		state = 1;
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		Object.Destroy(GameObject.Find("Napstablook"));
		hardmode = (int)gm.GetFlag(108) == 1;
		if (endState == 2)
		{
			gm.PlayMusic("mus_ruins");
			if (hardmode)
			{
				StartText(new string[5] { "* ...You okay?", "* ...^05 You look really\n  relieved about something.", "* Well whatever it is,^05\n  just know that if\n  anyone messes with you.", "* I'll mess them up\n  right back.", "* So,^05 don't worry about\n  them anymore,^05 okay?" }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_side", "su_neutral", "su_confident", "su_teeth_eyes", "su_smile_side" });
			}
			else
			{
				StartText(new string[5] { "* ...You okay Kris?", "* Look,^05 you got me\n  with you.", "* If anyone tries to\n  mess with you...", "* I'll mess them up\n  right back.", "* So,^05 don't worry about\n  them anymore,^05 okay?" }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_side", "su_neutral", "su_confident", "su_teeth_eyes", "su_smile_side" });
			}
		}
		else if ((int)gm.GetFlag(12) == 1)
		{
			gm.StopMusic();
			susie.UseUnhappySprites();
			if (hardmode)
			{
				gm.SetFlag(0, "g_start");
				StartText(new string[11]
				{
					"* ...", "* Why...", "* Why did you...^10\n  make me do that?", "* They weren't even\n  really doing anything.", "* ...^05 What the hell is\n  with that expression???", "* This isn't funny!", "* ...", "* I know that I\n  have the role of\n  protecting you,^05 but...", "* If you do something\n  screwed up like this\n  again...", "* I'll make sure the\n  light in your eyes\n  fades.",
					"* So quit pushing me."
				}, new string[11]
				{
					"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus",
					"snd_txtsus"
				}, new int[18], new string[11]
				{
					"su_concerned", "su_side_sweat", "su_side_sweat", "su_neutral", "su_pissed", "su_pissed", "su_dejected", "su_depressed", "su_depressed", "su_teeth",
					"su_annoyed_sweat"
				});
				state = 1;
			}
			else
			{
				StartText(new string[4] { "* ...", "* K-^05Kris...?", "* Why did you...^10\n  make me do that?", "* They weren't even\n  really doing anything." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_concerned", "su_side_sweat", "su_side_sweat", "su_neutral", "su_smile_side" });
				txt.EnableSelectionAtEnd();
			}
		}
		else
		{
			gm.PlayMusic("mus_ruins");
			StartText(new string[5] { "* Heh,^05 we showed that\n  dumb ghost who's boss.", "* ...", "* (What even was that?)", "* (Kris looked so...\n  ^10worried.)", "* (I guess they snapped\n  out of whatever was\n  happening.)" }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_teeth_eyes", "su_smile_sweat", "su_side_sweat", "su_dejected", "su_side" });
		}
	}
}
