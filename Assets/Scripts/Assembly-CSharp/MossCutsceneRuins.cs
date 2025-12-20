using UnityEngine;

public class MossCutsceneRuins : CutsceneBase
{
	private bool resumedMusic;

	private bool eat;

	private void Update()
	{
		if (!isPlaying || state != 0)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 1 && !GetComponent<AudioSource>().isPlaying && !resumedMusic)
			{
				resumedMusic = true;
				gm.ResumeMusic(15);
			}
			if (txt.GetCurrentStringNum() == 2 && !resumedMusic)
			{
				GetComponent<AudioSource>().Stop();
				resumedMusic = true;
				gm.ResumeMusic(15);
			}
			if (txt.GetCurrentStringNum() == 4 && !eat)
			{
				eat = true;
				gm.PlayGlobalSFX("sounds/snd_swallow");
			}
		}
		else
		{
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.PauseMusic();
		gm.SetFlag(50, 1);
		PlaySFX("sounds/snd_moss_fanfare");
		StartText(new string[4] { "* You found the <color=#00FF00FF>[MOSS]</color>!", "* Hell yeah, Kris!!!", "* ...Shall we?", "* You and Susie eat the moss." }, new string[4] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_text" }, new int[4], new string[4] { "", "su_excited", "su_happy", "" });
	}
}
