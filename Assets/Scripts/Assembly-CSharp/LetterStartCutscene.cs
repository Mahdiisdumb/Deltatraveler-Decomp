using UnityEngine;

public class LetterStartCutscene : CutsceneBase
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
		base.StartCutscene(par);
		gm.PlayMusic("music/mus_papyrus", 0.85f);
		gm.SetFlag(84, 8);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		GameObject.Find("LoadingZone").GetComponent<LoadingZone>().SetForceActivationTrigger(forceActivationTrigger: true);
		StartText(new string[4] { "HUMAN!!!", "I HOPE YOU'RE READY \nFOR THIS INCREDIBLE \nCHALLENGE!!!", "CREATED BY MY \nINCREDIBLY TALENTED \nBROTHER.", "IT IS SURELY \nSOMETHING THAT YOU \nCANNOT PASS UP!!" }, new string[1] { "snd_txtpap" }, new int[1], new string[4] { "ufpap_evil", "ufpap_laugh", "ufpap_side", "ufpap_laugh" });
	}
}
