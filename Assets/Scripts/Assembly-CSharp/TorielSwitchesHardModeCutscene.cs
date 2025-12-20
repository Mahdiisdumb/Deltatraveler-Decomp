using UnityEngine;

public class TorielSwitchesHardModeCutscene : CutsceneBase
{
	private Animator toriel;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			if (toriel.transform.position != new Vector3(10f, -0.78f))
			{
				toriel.Play("WalkRight");
				toriel.SetFloat("speed", 1f);
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(10f, -0.78f), 0.125f);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_boomf");
				}
				if (frames == 30)
				{
					toriel.GetComponent<SpriteRenderer>().enabled = false;
					StartText(new string[2] { "* I didn't know that\n  she could be this\n  mean...", "* Honestly,^05 it's kinda\n  making me feel\n  uncomfortable." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_concerned", "su_dejected" }, 0);
					state = 1;
				}
			}
		}
		if (state == 1 && !txt)
		{
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(108) == 1)
		{
			gm.SetFlag(119, 1);
			toriel = GameObject.Find("Toriel").GetComponent<Animator>();
			toriel.transform.position = new Vector3(0.51f, -0.78f);
			StartText(new string[9] { "* To make progress here,^10\n  you will need to trigger\n  several switches.", "* Do not worry,^10 I have--", "* Uhh,^05 can you do one\n  of those for us?", "* Silence.\n^10* I am teaching this\n  child on the--", "* Well I'm not supposed\n  to be here!", "* It wouldn't hurt to\n  speed things up,^05\n  would it?", "* ...", "* Fine,^05 but I shall only\n  do it for this room.", "* Please learn to be\n  more patient." }, new string[9] { "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[9] { "tori_neutral", "tori_neutral", "su_smile_sweat", "tori_mad", "su_pissed", "su_annoyed", "tori_annoyed", "tori_mad", "tori_annoyed" }, 0);
			base.StartCutscene(par);
		}
		else
		{
			EndCutscene();
		}
	}
}
