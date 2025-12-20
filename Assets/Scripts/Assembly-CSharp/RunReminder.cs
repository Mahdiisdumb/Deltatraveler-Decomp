public class RunReminder : CutsceneBase
{
	private void Update()
	{
		if (isPlaying && state == 0 && !txt)
		{
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		gm.SetFlag(9, 1);
		if (UTInput.GetButton("X"))
		{
			EndCutscene();
			return;
		}
		base.StartCutscene(par);
		if (GameManager.GetOptions().autoRun.value == 1)
		{
			StartText(new string[1] { "* (You felt that you can hold\n  ^X to walk slower.)" }, new string[1] { "snd_text" }, new int[18], new string[1] { "" });
		}
		else if ((int)gm.GetFlag(108) == 1)
		{
			StartText(new string[3] { "* Hey,^05 I get that\n  you're like...^05 small.", "* But you can run a\n  LITTLE faster than this,^05\n  right?", "* If I'm remembering^05\n  right, you do something\n  with ^X." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side", "su_annoyed", "su_smile_sweat" });
		}
		else
		{
			StartText(new string[3] { "* Oh my god, Kris, are\n  you seriously gonna walk\n  this slow the whole way?", "* You do remember that\n  you can run,^10 right?", "* You do something with,^10\n  like,^10 ^X.\n^10* Or whatever Ralsei said." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_annoyed", "su_annoyed", "su_smile_sweat" });
		}
	}
}
