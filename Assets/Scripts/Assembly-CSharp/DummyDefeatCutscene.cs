using UnityEngine;

public class DummyDefeatCutscene : CutsceneBase
{
	private int endState;

	private Animator toriel;

	private int FUCK;

	private int SHIT;

	private void Update()
	{
		if (state == 0 && !txt)
		{
			GameObject.Find("LoadingZoneLeave").GetComponent<BoxCollider2D>().enabled = true;
			GameObject.Find("SusieAnnoyed").GetComponent<InteractTextBox>().enabled = false;
			EndCutscene();
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				toriel.Play("WalkUp");
			}
			toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(-0.03f, 10f), 0.125f);
			toriel.GetComponent<SpriteRenderer>().enabled = toriel.transform.position.y < 2.84f;
			if (frames >= 5)
			{
				if (susie.transform.position.x != -1f)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-1f, -1.127f), 0.125f);
				}
				else
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					kris.ChangeDirection(Vector2.left);
					if ((int)gm.GetFlag(175) == 1)
					{
						StartText(new string[2] { "* ...", "* (...This kid is really\n  strange...)" }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_text" }, new int[18], new string[3] { "su_wideeye", "su_inquisitive", "" }, 0);
					}
					else if (endState == 1)
					{
						StartText(new string[3] { "* Yeesh...", "* That dummy just...\n^15  turned into dust.", "* That's kinda eerie,^10\n  not gonna lie." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side", "su_neutral", "su_side_sweat" }, 0);
					}
					else
					{
						StartText(new string[5] { "* Well, look at that.", "* ACTing is a thing\n  that works here.", "* You can also tell\n  me to ACT, too.", "* If Kris's mom lets\n  me actually fight WITH\n  you,^05 that is...", "* (You learned about S-ACTION.)" }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_txtsus" }, new int[18], new string[5] { "su_confident", "su_smile", "su_smile_side", "su_annoyed", "" }, 0);
					}
					state = 2;
				}
			}
		}
		if (state == 2 && !txt)
		{
			kris.ChangeDirection(Vector2.down);
			susie.SetSelfAnimControl(setAnimControl: true);
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = (int)par[0];
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		bool flag = (int)gm.GetFlag(108) == 1;
		Object.Destroy(GameObject.Find("susieinteract"));
		if (flag)
		{
			susie.SetSelfAnimControl(setAnimControl: false);
			gm.SetPartyMembers(susie: true, noelle: false);
		}
		if (endState == 1)
		{
			Object.Destroy(GameObject.Find("Dummy"));
		}
		if ((int)gm.GetFlag(175) == 1)
		{
			if (flag)
			{
				StartText(new string[3] { "* ^10.^10.^10.", "* ^10.^10.^10.", "* The next room awaits." }, new string[3] { "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[3] { "tori_weird", "tori_weird_side", "tori_happy" }, 0);
				state = 1;
			}
			else
			{
				StartText(new string[2] { "* ...", "* ...I guess that\n  works???" }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_wideeye", "su_inquisitive" }, 0);
			}
		}
		else if (endState == 1)
		{
			if (flag)
			{
				StartText(new string[4] { "* Why did you fight\n  the dummy,^05 my child?", "* We do not want to hurt\n  anybody,^10 do we...?", "* That is why I said\n  to talk to the dummy.", "* In any case,^05 come now." }, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[4] { "tori_weird", "tori_worry", "tori_worry", "tori_neutral" }, 0);
				state = 1;
			}
			else
			{
				StartText(new string[3] { "* Huh...", "* It just...\n^15  turned into dust.", "* That's kinda eerie,^10\n  not gonna lie." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side", "su_neutral", "su_side_sweat" }, 0);
			}
		}
		else if (endState == 2)
		{
			if (flag)
			{
				StartText(new string[2] { "* Ah,^05 very good!\n^10* You are very good.", "* Thank you for listening\n  to me,^05 my child." }, new string[2] { "snd_txttor", "snd_txttor" }, new int[18], new string[2] { "tori_happy", "tori_happy" }, 0);
				state = 1;
			}
			else
			{
				StartText(new string[2] { "* Aight, cool.", "* Good to know that\n  sucking ass at ACTing\n  still works here." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_smile", "su_smile_side" }, 0);
			}
		}
	}
}
