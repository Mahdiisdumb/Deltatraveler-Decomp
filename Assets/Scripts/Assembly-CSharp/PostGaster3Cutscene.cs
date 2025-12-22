using UnityEngine;

public class PostGaster3Cutscene : CutsceneBase
{
	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 40)
			{
				gm.DisablePlayerMovement(deactivatePartyMembers: true);
				ChangeDirection(kris, Vector2.down);
				if (Util.GameManager().GetFlagInt(172) == 0)
				{
					StartText(new string[11]
					{
						"* Kris...?", "* What the hell were\n  you doing?", "* You were kind of...\n  wandering.", "* You weren't gonna...", "* ...^10 Gonna...^05 what?", "* ...", "* ...Nevermind.", "* Maybe they just...^05\n  needed a minute.", "* These woods are kinda\n  hypnotic in a way.", "* Okay,^05 this is getting\n  weird.",
						"* Let's just go."
					}, new string[11]
					{
						"snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus",
						"snd_txtsus"
					}, new int[1], new string[11]
					{
						"su_side_sweat", "su_annoyed", "no_thinking", "no_shocked", "su_inquisitive", "no_sad", "no_depressed", "no_relief", "no_weird", "su_annoyed",
						"su_annoyed"
					});
				}
				else
				{
					StartText(new string[21]
					{
						"* Kris...?", "* What the hell were\n  you doing?", "* Kris.", "* If you were serious\n  about not having\n  control...", "* You...", "* ...^05 I...\n^10* I mean...", "* ...^10 nevermind.", "* The hell were you\n  gonna say?", "* I...^05 shouldn't say.", "* Everything is just...^05\n  getting to me.",
						"* ...", "* Noelle,^05 I get it.", "* But we're also in\n  this together.", "* Doing something rash\n  is gonna ruin our\n  chances of getting home.", "* ...", "* I...^10 probably need\n  some rest.", "* I feel like I'm\n  going crazy.", "* I'm sure we can\n  find someplace ahead\n  we can sleep.", "* But for now,^05 we\n  aren't really safe.", "* So,^05 uhh...",
						"* I guess let's find\n  that place."
					}, new string[21]
					{
						"snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe",
						"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus",
						"snd_txtsus"
					}, new int[1], new string[21]
					{
						"su_side_sweat", "su_annoyed", "no_depressedx", "no_depressedx", "no_depressedx_smile", "no_afraid", "no_depressedx", "su_inquisitive", "no_depressed_side", "no_depressedx",
						"su_depressed", "su_sad", "su_sad_side", "su_dejected", "no_depressedx", "no_depressedx", "no_depressedx", "su_worriedsmile", "su_dejected", "su_side",
						"su_smirk"
					});
				}
				state = 1;
			}
		}
		if (state == 1 && !txt)
		{
			RestorePlayerControl();
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(183) == 0)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		kris.DisableStepSounds();
		gm.SetFlag(184, 1);
		gm.StopMusic();
		if (gm.SusieInParty())
		{
			base.StartCutscene(par);
			susie.transform.position = new Vector3(-1.35f, -0.55f);
			susie.ChangeDirection(Vector2.up);
			noelle.transform.position = new Vector3(0.43f, -0.58f);
			noelle.ChangeDirection(Vector2.up);
			susie.UseUnhappySprites();
			noelle.UseUnhappySprites();
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
