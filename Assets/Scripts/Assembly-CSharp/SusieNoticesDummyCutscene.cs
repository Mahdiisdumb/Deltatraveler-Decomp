using UnityEngine;

public class SusieNoticesDummyCutscene : CutsceneBase
{
	private Animator toriel;

	private void Update()
	{
		if (state == 0 && !txt)
		{
			EndCutscene();
		}
		if (state == 1 && !txt)
		{
			toriel.Play("WalkRight");
			toriel.SetFloat("speed", 1f);
			if (toriel.transform.position != new Vector3(-0.03f, 2.15f))
			{
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(-0.03f, 2.15f), 0.125f);
			}
			else
			{
				toriel.Play("WalkDown", 0, 0f);
				toriel.SetFloat("speed", 0f);
				state = 2;
				StartText(new string[5] { "* ... And lady,^10 do NOT\n  enter the fight with\n  the human.", "* Come on, REALLY???", "* Literally all I\n  want to do is--", "* Fight the human?\n^10* Do not touch them.", "* (I know way more\n  about fighting than\n  she does...)" }, new string[5] { "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txtsus" }, new int[18], new string[5] { "tori_mad", "su_pissed", "su_pissed", "tori_weird", "su_dejected" }, 0);
			}
		}
		if (state == 2 && !txt)
		{
			kris.ChangeDirection(Vector2.down);
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		if ((int)gm.GetFlag(108) == 1)
		{
			toriel = GameObject.Find("Toriel").GetComponent<Animator>();
			toriel.transform.position = new Vector3(-3.45f, -0.33f);
			susie.transform.position = new Vector3(-6.148f, -1.127f);
			GameObject.Find("susieinteract").transform.position = new Vector3(-6.229f, -1.65f);
			gm.SetPartyMembers(susie: false, noelle: false);
			StartText(new string[13]
			{
				"* As a human living in\n  the UNDERGROUND,^10\n  monsters may attack you.", "* As you likely already\n  know...", "* You will need to be\n  prepared for this\n  situation.", "* However,^10 worry not!^10\n* The process is simple.", "* When you encounter a\n  monster,^10 you will enter\n  a FIGHT.", "* While you are in a\n  FIGHT,^10 strike up a\n  friendly conversation.", "* Or you could fight\n  back,^05 if they're--", "* Child,^05 do not listen\n  to her!", "* Practice talking to\n  the dummy.", "* Or beat it up???",
				"* It's literally a\n  training dummy.", "* Violence is not the\n  answer,^05 my child.", "* (I literally didn't\n  say that???)"
			}, new string[13]
			{
				"snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txtsus",
				"snd_txtsus", "snd_txttor", "snd_txtsus"
			}, new int[18], new string[13]
			{
				"tori_neutral", "tori_annoyed", "tori_neutral", "tori_happy", "tori_neutral", "tori_neutral", "su_side_sweat", "tori_wack", "tori_neutral", "su_inquisitive",
				"su_annoyed", "tori_wack", "su_side_sweat"
			}, 0);
			state = 1;
			return;
		}
		GameObject.Find("LoadingZoneLeave").GetComponent<BoxCollider2D>().enabled = false;
		if ((int)gm.GetFlag(7) == 0)
		{
			StartText(new string[2] { "* Kris, look!^10\n* A training dummy!", "* We should go beat\n  it up." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_surprised", "su_smile" }, 0);
			gm.SetFlag(7, 1);
		}
		else if ((int)gm.GetFlag(7) == 1)
		{
			StartText(new string[3] { "* Uhh, Kris?", "* What's with the\n  hesitation?", "* We could use a warm up." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_smile_sweat", "su_smile_sweat", "su_smile_sweat" }, 0);
			gm.SetFlag(7, 2);
		}
		else if ((int)gm.GetFlag(7) == 2)
		{
			StartText(new string[1] { "* ..." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_inquisitive", "su_smile_sweat", "su_smile_sweat" }, 0);
			gm.SetFlag(7, 3);
		}
		else if ((int)gm.GetFlag(7) == 3)
		{
			EndCutscene();
		}
		if ((int)gm.GetFlag(7) == 3)
		{
			GameObject.Find("SusieAnnoyed").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* Kris, ^05stop fucking\n  around." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_annoyed" });
		}
		GameObject.Find("SusieAnnoyed").GetComponent<InteractTextBox>().enabled = true;
	}
}
