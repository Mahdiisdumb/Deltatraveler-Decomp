using UnityEngine;

public class TorielConfrontationCutscene : CutsceneBase
{
	private Animator toriel;

	private bool torielMusicPlay;

	private bool susieCoolPose;

	private bool itemSound;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			kris.transform.position = Vector3.Lerp(new Vector3(-0.77f, -2.92f), new Vector3(-0.77f, -1.97f), (float)frames / 20f);
			susie.transform.position = Vector3.Lerp(new Vector3(0.77f, -2.73f), new Vector3(0.77f, -1.78f), (float)frames / 20f);
			if (frames == 20)
			{
				kris.GetComponent<Animator>().SetBool("isMoving", false);
				susie.GetComponent<Animator>().SetBool("isMoving", false);
			}
			if (frames == 50)
			{
				StartText(new string[3] { "* ...", "* Uhmm...^15 hey.^15\n* Ms.^10, uh,^10 Dreemurr.", "* We're kinda wondering\n  ^05uhh,^05 why you're running\n  away from us." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side_sweat", "su_side_sweat", "su_smile_sweat" }, 0);
				frames = 0;
				state = 1;
			}
		}
		if (state == 1)
		{
			if (!txt)
			{
				state = 2;
				frames = 0;
				toriel.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = false;
				toriel.enabled = true;
				toriel.Play("StandAndTurn");
			}
			else if (txt.GetCurrentStringNum() >= 2)
			{
				frames++;
				toriel.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = frames >= 10 && frames <= 30;
				if (frames == 10)
				{
					toriel.enabled = true;
					toriel.Play("StandAndTurn");
				}
			}
		}
		if (state == 2 && (frames < 40 || !txt))
		{
			frames++;
			if (frames == 40)
			{
				StartText(new string[3] { "* My apologies.^10 *sniff*", "* I could not bear these\n  emotions,^10 and I did not\n  want to worry you two.", "* But of course, ^10*sniff*\n  ^10I failed at that." }, new string[3] { "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18]
				{
					1, 1, 1, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0
				}, new string[3] { "", "", "" }, 0);
			}
			if (frames == 41)
			{
				toriel.SetFloat("speed", 0.75f);
			}
			if (frames == 150)
			{
				frames = 0;
				state = 3;
				StartText(new string[4] { "* Oh...^20 uhh...", "* Umm,^10 sorry.", "* Please do not apologize.\n^10* You did nothing wrong.", "* But I... ^10*sniff*\n^10  I must ask you." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18]
				{
					0, 0, 1, 1, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0
				}, new string[4] { "su_concerned", "su_concerned", "tori_cry_side", "tori_cry_side" }, 0);
			}
		}
		if (state == 3)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					toriel.Play("WalkDownCrying");
				}
				if (frames <= 45)
				{
					toriel.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(-0.774f, -1.5f), (float)frames / 45f);
				}
				if (frames == 50)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_left_unhappy_0");
					kris.GetComponent<SpriteRenderer>().enabled = false;
					toriel.Play("GrabKris");
				}
				if (frames == 100)
				{
					gm.PlayMusic("music/mus_musicbox", 0.8f);
					StartText(new string[5] { "* My child,^15 who had been\n  taken from us far too\n  early.", "* My child,^15 who in spite\n  of your light-hearted\n  pranks...", "* Was a delight to our\n  people down here.", "* And inspired hope into\n  the lives of many.", "* My child,^15 is it truly\n  you?" }, new string[5] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18]
					{
						1, 1, 1, 1, 1, 0, 0, 0, 0, 0,
						0, 0, 0, 0, 0, 0, 0, 0
					}, new string[5] { "tori_cry", "tori_cry", "tori_cry", "tori_cry", "tori_cry" }, 0);
				}
				if (frames == 145)
				{
					toriel.SetFloat("speed", 0f);
					StartText(new string[4] { "* What???^10\n* Taken???", "* Ma'am,^10 I'm pretty sure\n  Kris is too weenie to\n  get themselves killed.", "* ...Kris?", "* Uhh,^10 yeah???\n^10* Your kid is Kris,^10\n  right?" }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txttor" }, new int[18], new string[5] { "su_worried", "su_smile_sweat", "tori_cry_weird", "su_concerned", "tori_cry" }, 0);
				}
				if (frames == 175)
				{
					toriel.Play("HugKris");
				}
				if (frames == 190)
				{
					StartText(new string[4] { "* O-^05Of course.\n^10* I apologize for my\n  emotional outburst.", "* Even if they survived,\n^10  they would not have\n  even lived this long.", "* It was unreasonable for\n  me to have thought that\n  \"Kris\" was them.", "* It's whatever...?" }, new string[5] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor" }, new int[18], new string[5] { "tori_worry", "tori_worry", "tori_worry", "su_smile_sweat", "tori_cry" }, 0);
					state = 4;
					frames = 0;
				}
			}
			else if (frames == 145 && txt.GetCurrentStringNum() == 3)
			{
				toriel.Play("GrabKrisShocked");
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				toriel.SetFloat("speed", 1f);
				gm.StopMusic(60f);
			}
			if (frames >= 90)
			{
				if (frames == 90)
				{
					susie.EnableAnimator();
					kris.GetComponent<SpriteRenderer>().enabled = true;
					toriel.Play("WalkDown");
				}
				toriel.transform.position = Vector3.Lerp(new Vector3(-0.774f, -1.5f), Vector3.zero, (float)(frames - 90) / 30f);
				if (frames == 120)
				{
					toriel.SetFloat("speed", 0f);
					toriel.Play("WalkDown", 0, 0f);
				}
			}
			if (frames == 150)
			{
				StartText(new string[30]
				{
					"* So you say that I am\n  the mother of this\n  child?", "* Uhh...^10 yeah?", "* And you know my\n  last name,^05 as well?", "* Yes??^10\n* You taught little kids\n  at the school.", "* Really?", "* Well,^10 unfortunately,\n  ^10I am not the same\n  Toriel.", "* Huh?", "* I am not a teacher,^10\n  nor am I the caretaker\n  of Kris.", "* Rather,^10 I am the\n  caretaker of the RUINS.", "* And formerly a fairly\n  regarded royal figure.",
					"* So how could this...?\n^15* ...No,^10 that...", "* ...", "* Huh?", "* Could it be that\n  you two...^10 are from\n  another world?", "* I guess that explains\n  what's going on.", "* Do you know how\n  we can get outta\n  here?", "* Possibly.", "* You could perhaps meet\n  with the UNDERGROUND's\n  <color=#FFFF00FF>ROYAL SCIENTIST</color>.", "* He is always doing...\n  ^15interesting experiments.", "* He may be able to help\n  you travel home.",
					"* And how do we find\n  this guy?", "* You will have to\n  travel all the way to\n  HOTLAND.", "* It is pretty far away,\n^10  but I'm sure that\n  you can make it.", "* After all,^10 you both are\n  traveling together,^10 are\n  you not?", "* Hell yeah,^05 we are!", "* Excellent!", "* I will prepare something\n  for you to take on\n  your travels.", "* It should not take\n  you very long to reach\n  the end of the RUINS.", "* Good luck,^10 you two.", "* Uhh,^08 thanks."
				}, new string[30]
				{
					"snd_txttor", "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor",
					"snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor",
					"snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus"
				}, new int[33], new string[30]
				{
					"tori_worry", "su_smile_sweat", "tori_weird", "su_annoyed", "tori_blush", "tori_worry", "su_surprised", "tori_worry", "tori_neutral", "tori_annoyed",
					"tori_worry", "tori_worry", "su_smile_sweat", "tori_worry", "su_inquisitive", "su_neutral", "tori_worry", "tori_neutral", "tori_worry", "tori_neutral",
					"su_surprised", "tori_worry", "tori_happy", "tori_neutral", "su_smile", "tori_happy", "tori_neutral", "tori_neutral", "tori_happy", "su_smile_side"
				}, 0);
				state = 5;
				frames = 0;
			}
		}
		if (state == 5)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					toriel.Play("WalkRight");
					toriel.SetFloat("speed", 1f);
					toriel.transform.Find("Exclaim").localPosition = new Vector3(0f, 1.33f);
				}
				if (frames > 30)
				{
					toriel.Play("WalkRight", 0, 0f);
					toriel.SetFloat("speed", 0f);
					toriel.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = frames >= 30 && frames < 50;
				}
				else
				{
					toriel.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(2f, 0f), (float)frames / 30f);
				}
				if (frames == 50)
				{
					toriel.GetComponent<SpriteRenderer>().flipX = true;
					StartText(new string[9] { "* Oh,^10 Kris!", "* You two could possibly\n  use my help on your\n  way.", "* I will give you a\n  <color=#FFFF00FF>CELL PHONE</color>.", "* ...You already have one?", "* Of course.\n^10* Then I can give you\n  <color=#FFFF00FF>my phone number</color>.", "* (You got Toriel's number.)", "* (You registered it under the\n  contact name \"Otherworldly\n  Mom\".)", "* If you have a need for\n  anything,^10 just call.", "* I hope to see you\n  two very soon." }, new string[9] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_text", "snd_text", "snd_txttor", "snd_txttor" }, new int[18], new string[9] { "tori_happy", "tori_worry", "tori_neutral", "tori_worry", "tori_neutral", "", "", "tori_neutral", "tori_neutral" }, 0);
					state = 6;
					frames = 0;
				}
			}
			else
			{
				if (txt.GetCurrentStringNum() == 8 && !torielMusicPlay)
				{
					torielMusicPlay = true;
					gm.PlayMusic("music/mus_toriel", 0.75f);
				}
				if (txt.GetCurrentStringNum() == 25 && !susieCoolPose)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_pose");
					susieCoolPose = true;
				}
				if (txt.GetCurrentStringNum() == 30 && susieCoolPose)
				{
					susie.EnableAnimator();
					susieCoolPose = false;
				}
			}
		}
		if (state == 6)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic(60f);
					toriel.GetComponent<SpriteRenderer>().flipX = false;
					toriel.Play("WalkRight");
					toriel.SetFloat("speed", 1f);
				}
				toriel.transform.position = Vector3.Lerp(new Vector3(2f, 0f), new Vector3(8f, 0f), (float)frames / 40f);
				if (frames == 60)
				{
					toriel.GetComponent<SpriteRenderer>().enabled = false;
					susie.ChangeDirection(Vector2.left);
					kris.ChangeDirection(Vector2.right);
					StartText(new string[4] { "* So,^10 we gotta meet with\n  this <color=#FFFF00FF>royal scientist</color>\n  guy to get home.", "* Hmm.", "* This should be pretty\n  fun.", "* Let's get going,^10 Kris." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_neutral", "su_side", "su_confident", "su_smile" }, 0);
				}
				if (frames == 61)
				{
					kris.SetSelfAnimControl(true);
					susie.SetSelfAnimControl(true);
					kris.ChangeDirection(Vector2.down);
					gm.SetFlag(1, "smirk");
					gm.SetCheckpoint(14, new Vector3(91.8f, 0.6f));
					EndCutscene();
					gm.PlayMusic("music/mus_ruins");
				}
			}
			else if (txt.GetCurrentStringNum() == 6 && !itemSound)
			{
				gm.SetFlag(8, 1);
				itemSound = true;
				PlaySFX("sounds/snd_item");
			}
		}
		if (state == 7 && !txt)
		{
			if (toriel.transform.position.x != 7.28f)
			{
				toriel.Play("WalkRight");
				toriel.SetFloat("speed", 1f);
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(7.28f, -0.36f), 1f / 6f);
			}
			else
			{
				toriel.Play("WalkRight", 0, 0f);
				toriel.SetFloat("speed", 0f);
				toriel.transform.position = new Vector3(26.51f, -0.81f);
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		if ((int)gm.GetFlag(108) == 1)
		{
			GameObject.Find("HardTorielCutsceneTrigger").transform.position = new Vector3(26.55f, -1.49f);
			toriel.transform.position = new Vector3(2.37f, -0.36f);
			toriel.enabled = true;
			state = 7;
			StartText(new string[2] { "* There is another puzzle\n  in this room.", "* I wonder if you can\n  solve it?" }, new string[2] { "snd_txttor", "snd_txttor" }, new int[18], new string[2] { "tori_worry", "tori_neutral" }, 0);
		}
		else
		{
			toriel.transform.position = Vector3.zero;
			gm.StopMusic(10f);
			kris.SetSelfAnimControl(false);
			susie.SetSelfAnimControl(false);
			kris.ChangeDirection(Vector2.up);
			susie.ChangeDirection(Vector2.up);
			kris.GetComponent<Animator>().SetBool("isMoving", true);
			susie.GetComponent<Animator>().SetBool("isMoving", true);
		}
	}
}
