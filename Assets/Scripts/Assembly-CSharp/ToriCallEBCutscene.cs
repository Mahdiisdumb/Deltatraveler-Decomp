public class ToriCallEBCutscene : CutsceneBase
{
	public void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames >= 30)
			{
				state = 1;
				StartText(new string[12]
				{
					"* (Ring...)", "* Kris???^10 Hello???\n^10* Are you there??!", "* ... ^05Thank goodness.", "* Kris,^05 where on earth\n  are you and Susie???", "* I thought someone\n  had taken you two!", "* Did you not see what\n  happened to our car???", "* Kris,^05 please do not\n  leave like that\n  again.", "* Anyways,^05 please don't be\n  out for too long.", "* Be careful,^05 okay,^05 honey?", "* (Click...)",
					"* Wait,^05 Kris,^05 was that\n  your actual mom or the\n  one that cried?", "* (What...?)"
				}, new string[12]
				{
					"snd_text", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_text",
					"snd_txtsus", "snd_txtnoe"
				}, new int[23], new string[12]
				{
					"", "torid_blush", "torid_worry", "torid_weird", "torid_worry", "torid_mad", "torid_worry", "torid_wack", "torid_worry", "",
					"su_surprised", "no_confused"
				});
			}
		}
		if (state == 1 && !txt)
		{
			gm.PlayMusic("zoneMusic");
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetFlag(64, 1);
		PlaySFX("sounds/snd_dial");
		gm.StopMusic();
		gm.SetFlag(84, 1);
	}
}
