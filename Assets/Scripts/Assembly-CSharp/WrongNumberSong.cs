using UnityEngine;

public class WrongNumberSong : CutsceneBase
{
	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		gm.DisablePlayerMovement(false);
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				PlaySFX("sounds/snd_dial");
				gm.StopMusic(15f);
				StartText(new string[17]
				{
					"* (Ring... ring...)", "* Hello!^05\n* Can I speak to G...", "* ...^05\n* Wait a second.", "* Is this the wrong number?", "* Oh it's the wrong number!^05\n* The wrong number song!", "* We're very very sorry that\n  we got it wrong!", "* Oh it's the wrong number!^05\n* The wrong number song!", "* We're very very sorry that\n  we got it wrong!", "* (Click...)", "* What a fun song!",
					"* The hell is a\n  G?", "* And why do they\n  have Kris's number?", "* Maybe...^05 it's from this\n  world...?", "* Hmm...", "* Then why is Kris\n  able to call their\n  mom from here?", "* ...", "* Good point,^05 Susie."
				}, new string[17]
				{
					"snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtnoe",
					"snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe"
				}, new int[1], new string[17]
				{
					"", "", "", "", "", "", "", "", "", "no_tease_finger",
					"su_annoyed", "su_side", "no_happy", "su_side", "su_neutral", "no_thinking", "no_weird"
				});
				state = 1;
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(5))
				{
					gm.PlayMusic("music/mus_wrongnumbersong");
				}
				else if (AtLine(9))
				{
					gm.StopMusic();
				}
			}
			else
			{
				gm.PlayMusic("zoneMusic");
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		PlaySFX("sounds/snd_dial");
		gm.SetFlag(84, 11);
		if (gm.IsMenuOpen())
		{
			MainMenu mainMenu = Object.FindObjectOfType<MainMenu>();
			Object.Destroy(((object)mainMenu != null) ? mainMenu.gameObject : null);
			gm.DisablePlayerMovement(false);
		}
	}
}
