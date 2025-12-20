using System.Collections.Generic;
using UnityEngine;

public class EarthboundFirstCutscenePt2 : CutsceneBase
{
	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("part_0", new string[10] { "* W-^05what just happened???", "* Everything went all\n  white...!", "* ...^05 but I don't\n  feel weird anymore.", "* Me...^10 neither...", "* Kris,^05 I think the\n  spell worked!", "* But how the hell\n  do we get back to\n  that forest...?", "* Maybe we find another\n  grey door?", "* Might as well.", "* Not like we'll really\n  get anywhere just\n  standing in this cave.", "* C'mon,^05 let's go." });
		return dictionary;
	}

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 60)
			{
				noelle.SetSprite("spr_no_surprise");
				kris.EnableAnimator();
				kris.ChangeDirection(Vector2.right);
				StartText(GetStringArray("part_0"), new string[10] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[11], new string[10] { "no_scared", "su_surprised", "su_inquisitive", "no_silent", "no_surprised_happy", "su_side", "no_curious", "su_annoyed", "su_smirk_sweat", "su_neutral" }, 0);
				state = 1;
			}
		}
		if (state == 1)
		{
			if (!txt)
			{
				kris.ChangeDirection(Vector2.down);
				gm.PlayMusic("music/mus_cave");
				gm.SetCheckpoint(51);
				Object.FindObjectOfType<SectionTitleCard>().Activate();
				EndCutscene();
			}
			else if (txt.GetCurrentStringNum() == 3)
			{
				susie.EnableAnimator();
				susie.ChangeDirection(Vector2.right);
				kris.ChangeDirection(Vector2.left);
			}
			else if (txt.GetCurrentStringNum() == 5)
			{
				noelle.EnableAnimator();
				noelle.ChangeDirection(Vector2.left);
				kris.ChangeDirection(Vector2.right);
			}
			else if (txt.GetCurrentStringNum() == 8)
			{
				kris.ChangeDirection(Vector2.left);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		gm.StopMusic();
		gm.SetFlag(65, 1);
		base.StartCutscene(par);
		kris.DisableAnimator();
		susie.DisableAnimator();
		noelle.DisableAnimator();
		kris.SetSprite("spr_kr_point");
		susie.SetSprite("spr_su_surprise_right");
		noelle.SetSprite("spr_no_cast_left");
		gm.SetFramerate(30);
		fade.FadeIn(30, Color.white);
	}
}
