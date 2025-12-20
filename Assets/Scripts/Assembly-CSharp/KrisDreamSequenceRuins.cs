using System.Collections.Generic;
using UnityEngine;

public class KrisDreamSequenceRuins : CutsceneBase
{
	private int genoID;

	private Animator krisDream;

	private Transform road;

	private bool aborted;

	private bool coolSound;

	private bool noSpace;

	private bool hardmode;

	private bool putDownPie;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeOut(45);
				gm.StopMusic(45f);
			}
			if (frames == 120)
			{
				if (hardmode)
				{
					List<string> list = new List<string> { "\b    PERHAPS YOU ARE WONDERING\n\b     WHAT I AM DISCUSSING...", "\b       THIS \"EXPERIMENT.\"", "\b   IT IS QUITE DIFFERENT FROM\n\b  WHAT I HAD ORIGINALLY PLANNED.", "\b   ONCE YOU OVERCOME YOUR FINAL\n\b   CHALLENGE,^10 I SHALL BRING YOU\n\b             HERE.", "\b      I SHALL SEE YOU SOON." };
					List<string> list2 = new List<string> { "\b      HOW VERY INTERESTING.", "\b   THIS EXPERIMENT HAS THUS FAR\n\b         BEEN A SUCCESS." };
					if (genoID == 2)
					{
						list2 = new List<string> { "\b          HOW STRANGE.", "\b   YOU ENTERED THIS EXPERIMENT\n\b    JUST TO INFLICT MORE PAIN\n\b           ON OTHERS.", "\b  NOT ONLY THAT, YOU HAVE BEEN\n\b      REVIVING THE SPIRIT\n\b         OF THE DEAD.", "\b       WE HAVE OURSELVES\n\b        TWO INDIVIDUALS\n\b       OF THE SAME NAME.", "\b    NOT COUNTING YOUR NAME,\n\b          OF COURSE.", "REGARDLESS, THE EXPERIMENT HAS\n\b  THUS FAR BEEN A SUCCESS." };
					}
					else if (genoID == 1)
					{
						list2 = new List<string> { "\b         HOW PECULIAR.", "\b     YOU HAVE GONE FORWARD\n\b   NOT HEEDING THE WARNINGS\n\b        OF YOUR PARTNER.", "\b    I AM NOT REFERRING TO\n\b      SUSIE, OF COURSE.", "\b    I AM REFERRING TO THE\n\b   ONE COUNTING THE LIVING.", "\b NOW THE ENEMIES YOU HAVE NOT\n\b  FOUGHT HAVE HIDDEN AWAY.", "REGARDLESS, THE EXPERIMENT HAS\n\b  THUS FAR BEEN A SUCCESS." };
					}
					List<string> list3 = new List<string>(list.Count + list2.Count);
					list3.AddRange(list2);
					list3.AddRange(list);
					StartText(list3.ToArray(), new string[20]
					{
						"", "", "", "", "", "", "", "", "", "",
						"", "", "", "", "", "", "", "", "", ""
					}, new int[22]
					{
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2
					}, new string[0], 0);
					txt.EnableGasterText();
					GameObject.Find("TextBox").transform.localPosition = new Vector3(0f, -152f);
					Object.Destroy(GameObject.Find("menuBorder"));
					Object.Destroy(GameObject.Find("menuBox"));
					gm.PlayMusic("music/AUDIO_DRONE");
					fade.FadeIn(1);
					GameObject.Find("screenblock").transform.position = Vector3.zero;
					frames = 0;
					state = 7;
				}
				else
				{
					GameObject.Find("screenblock").transform.position = Vector3.zero;
					krisDream.transform.position = new Vector3(-1.43f, -0.92f);
					krisDream.Play("DreamStumble");
					road.position = new Vector3(0.12f, -1.1f);
					frames = 0;
					state = 1;
					fade.FadeIn(30);
				}
			}
		}
		if (state == 1)
		{
			frames++;
			if (frames % 60 == 31)
			{
				int num = 2;
				if (frames % 120 == 31)
				{
					num = 1;
				}
				PlaySFX("sounds/snd_step" + num);
			}
			if (frames >= 90)
			{
				road.position = Vector3.MoveTowards(road.position, new Vector3(-100f, -1.1f), 1f / 48f);
				if (frames <= 180)
				{
					float num2 = (float)(frames - 90) / 90f;
					for (int i = 0; i < 2; i++)
					{
						road.GetChild(i).localPosition = new Vector3(Mathf.Lerp((i != 0) ? 1 : (-1), 0f, num2), 0f);
						road.GetChild(i).GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, num2 * num2);
						krisDream.transform.GetChild(i).localPosition = new Vector3(Mathf.Lerp((i != 0) ? 1 : (-1), 0f, num2), 0f);
						krisDream.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, num2 * num2);
						krisDream.transform.GetChild(i).GetComponent<SpriteRenderer>().sprite = krisDream.GetComponent<SpriteRenderer>().sprite;
						if (frames == 180)
						{
							krisDream.GetComponent<SpriteRenderer>().color = Color.white;
							road.GetComponent<SpriteRenderer>().color = Color.white;
							krisDream.transform.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
							road.GetChild(i).GetComponent<SpriteRenderer>().enabled = false;
						}
					}
				}
			}
			if (frames == 310)
			{
				krisDream.SetFloat("speed", 0f);
				state = 2;
				frames = 0;
			}
		}
		if (state == 2)
		{
			frames++;
			if (frames >= 45 && frames <= 48)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				int num4 = 48 - frames;
				krisDream.transform.position = new Vector3(-1.43f, -0.92f) + new Vector3((float)(num4 * num3) / 24f, 0f);
			}
			if (frames == 45)
			{
				PlaySFX("sounds/snd_step2");
				krisDream.SetFloat("speed", 1f);
				if (genoID == 2)
				{
					krisDream.Play("DreamGround_G");
				}
				else
				{
					krisDream.Play("DreamGround");
				}
				road.GetComponent<AudioSource>().Play();
			}
			road.GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 0.5f, (float)(frames - 45) / 30f);
			if (frames == 150)
			{
				if (genoID == 2)
				{
					StartText(new string[19]
					{
						"\b       YOU HAVE BEEN BUSY.", "\b  MERCILESSLY GAINING STRENGTH.", "\b     HOW VERY INTERESTING...", "\b        YOU ARE AWARE OF \n\b        THE CIRCUMSTANCES\n\b        ARE YOU NOT?", "\b      WHETHER YOU CONTINUE\n\b      DOWN THIS...", "\b         OR SHOW MERCY...", "\b       IT DOES NOT MATTER.", "\b          EITHER WAY...", "\b             KRIS.", "\b       YOU ARE QUITE LOST,\n         AREN'T YOU...?",
						"\b  I HAVE A PROPOSITION FOR YOU.", "\b             YOU.^10\n\b             KRIS.^10\n\b             SUSIE.", "\b         COME FIND ME.", "TRAVERSE THE GREY DOORS UNTIL YOU \nREACH THE GOLDEN CORRIDOR.", "\b   DEFEAT THE ONE THAT CARRIES \n\b        YOUR OWN ESSENCE.", "\b AFTERWARDS,^20 I CAN CLEANSE YOU\n\b          OF YOUR SIN.", "\b      I CAN SET YOU FREE.", "\b             KRIS.", "\b     I SHALL SEE YOU SOON."
					}, new string[20]
					{
						"", "", "", "", "", "", "", "", "", "",
						"", "", "", "", "", "", "", "", "", ""
					}, new int[22]
					{
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2
					}, new string[0], 0);
				}
				else if (genoID == 1)
				{
					StartText(new string[16]
					{
						"\b         HOW REMARKABLE.", "\b   IT SEEMS YOU HAVE REALIZED \n\b   THE FUTILITY OF YOUR ACTIONS.", "\b       YOU CANNOT CHANGE\n\b       THE END CONDITION\n\b       THROUGH SHEER WILL.", "\b  THE ENEMIES THAT YOU MISSED\n\b       HAVE HIDDEN AWAY.", "\b       YOU NO LONGER HAVE\n\b           TO WORRY,\n\b             KRIS.", "\b        BUT EVEN STILL...", "\b       YOU ARE QUITE LOST,\n         AREN'T YOU...?", "\b  I HAVE A PROPOSITION FOR YOU.", "\b             YOU.^10\n\b             KRIS.^10\n\b             SUSIE.", "\b         COME FIND ME.",
						"TRAVERSE THE GREY DOORS UNTIL YOU \nREACH THE GOLDEN CORRIDOR.", "\b   DEFEAT THE ONE THAT CARRIES \n\b        YOUR OWN ESSENCE.", "\b         MEET WITH ME.", "\b      I CAN SET YOU FREE.", "\b             KRIS.", "\b     I SHALL SEE YOU SOON."
					}, new string[18]
					{
						"", "", "", "", "", "", "", "", "", "",
						"", "", "", "", "", "", "", ""
					}, new int[20]
					{
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2
					}, new string[0], 0);
				}
				else
				{
					StartText(new string[11]
					{
						"\b             KRIS.", "\b       YOU ARE QUITE LOST,\n         AREN'T YOU...?", "\b  I HAVE A PROPOSITION FOR YOU.", "\b             YOU.^10\n\b             KRIS.^10\n\b             SUSIE.", "\b         COME FIND ME.", "TRAVERSE THE GREY DOORS UNTIL YOU \nREACH THE GOLDEN CORRIDOR.", "\b   DEFEAT THE ONE THAT CARRIES \n\b        YOUR OWN ESSENCE.", "\b         MEET WITH ME.", "\b      I CAN SET YOU FREE.", "\b             KRIS.",
						"\b     I SHALL SEE YOU SOON."
					}, new string[18]
					{
						"", "", "", "", "", "", "", "", "", "",
						"", "", "", "", "", "", "", ""
					}, new int[20]
					{
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2
					}, new string[0], 0);
				}
				txt.EnableGasterText();
				state = 3;
				frames = 0;
				Object.Destroy(GameObject.Find("menuBorder"));
				Object.Destroy(GameObject.Find("menuBox"));
			}
		}
		if (state == 3)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					road.GetComponent<AudioSource>().Stop();
					if (genoID == 2)
					{
						krisDream.Play("DreamGive");
						PlaySFX("sounds/snd_darkness", 0.8f);
					}
					else
					{
						krisDream.Play("DreamGround", 0, 1.25f);
						krisDream.SetFloat("speed", -0.5f);
						PlaySFX("sounds/snd_darkness");
					}
				}
				if (frames == 60)
				{
					fade.FadeOut(60);
				}
				if (frames == 120)
				{
					krisDream.transform.position = new Vector3(0f, 50f);
					road.position = new Vector3(0f, 50f);
					fade.FadeIn(1);
				}
				if (frames == 240)
				{
					StartText(new string[1] { "* KRIS!!!^10\n* WAKE UP!!!" }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "" }, 1);
					state = 4;
					frames = 0;
				}
			}
			else if (genoID == 1 && txt.GetCurrentStringNum() == 5 && !aborted)
			{
				aborted = true;
				WeirdChecker.Abort(gm);
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames == 5)
			{
				GameObject.Find("SusieBed").transform.position = new Vector3(3.14f, 1.42f);
				GameObject.Find("SusieBedCutscene").transform.position = new Vector3(6.14f, 1.42f);
				GameObject.Find("KrisBed").transform.position = new Vector3(-2.29f, 1.42f);
				GameObject.Find("KrisBedCutscene").transform.position = new Vector3(-7.06f, 1.42f);
				GameObject.Find("screenblock").transform.position = new Vector3(0f, 50f);
				if (hardmode)
				{
					kris.ChangeDirection(Vector2.right);
				}
				else
				{
					kris.DisableAnimator();
					kris.SetSprite("spr_kr_sleep");
				}
				kris.transform.position = new Vector3(-2.83f, 2.63f);
				susie.SetSprite("spr_su_pissed");
				susie.transform.position = new Vector3(-0.89f, 2.03f);
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 25)
			{
				if (hardmode)
				{
					if (genoID == 1)
					{
						StartText(new string[5] { "* Oh my GOD,^05 you've been\n  sleeping for like TWO\n  HOURS!!", "* ... You look different.", "* Did you have a\n  good dream or\n  something?", "* Anyways,^05 I've got a pie\n  slice for ya.^05\n* Straight from Toriel.", "* ... What's with that\n  look?^05\n* Just take it." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_wtf", "su_side", "su_smirk", "su_confident", "su_annoyed" }, 1);
					}
					else
					{
						StartText(new string[5] { "* Oh my GOD,^05 you've been\n  sleeping for like TWO\n  HOURS!!", "* The pie apparently\n  got COLD.", "* So I ate most of\n  it.", "* But...^10\n* I did leave a slice\n  for you.", "* ... What's with that\n  look?^05\n* Just take it." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_wtf", "su_angry", "su_teeth_eyes", "su_confident", "su_annoyed" }, 1);
					}
				}
				else if (genoID == 2)
				{
					StartText(new string[5] { "* Kris,^10 you've been\n  sleeping for TWO HOURS!!", "* ... You look kinda\n  off,^05 Kris.", "* You're sweating.", "* ...", "* Here,^05 why don't you\n  take this pie.\n^10* Lil' taste of home." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_wtf", "su_side", "su_side_sweat", "su_dejected", "su_smirk_sweat" }, 1);
				}
				else if (genoID == 1)
				{
					StartText(new string[5] { "* Kris,^10 you've been\n  sleeping for TWO HOURS!!", "* ... You look alot\n  better.", "* I guess nevermind about\n  me being mad or\n  whatever.", "* But hey.^10\n* I left you a slice\n  of pie.", "* Which got COLD,^05 BY\n  THE WAY." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_wtf", "su_side", "su_smirk", "su_confident", "su_angry" }, 1);
				}
				else
				{
					StartText(new string[4] { "* Kris,^10 you've been\n  sleeping for TWO HOURS!!", "* The pie apparently\n  got COLD.", "* So I ate most of\n  it.", "* But...^10\n* I did leave a slice\n  for you." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_wtf", "su_angry", "su_teeth_eyes", "su_confident" }, 1);
				}
				state = 5;
			}
		}
		if (state == 5)
		{
			if (!txt)
			{
				if (!hardmode)
				{
					kris.SetSprite("spr_kr_sit_injured");
					kris.transform.position = new Vector3(-2.83f, 2.92f);
				}
				if (gm.NumItemFreeSpace() > 0)
				{
					PlaySFX("sounds/snd_item");
					gm.AddItem(hardmode ? 28 : 5);
					StartText(new string[3]
					{
						hardmode ? "* (You got the Snail Pie...)" : "* (You got the Butterscotch Pie.)",
						"* Now let's get outta\n  here.",
						"* I'm tired of this\n  place."
					}, new string[4] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_text" }, new int[18], new string[4] { "", "su_annoyed", "su_annoyed", "su_confident" }, 1);
				}
				else
				{
					noSpace = true;
					if (genoID == 2 && !hardmode)
					{
						StartText(new string[4] { "* You don't have space\n  for it?", "* Then just eat it\n  right now.\n^10* Jeez.", "* (You ate the pie.)^10\n* (It tasted like home.)^10\n* (You felt a little better.)", "* Now let's go home." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_text", "snd_txtsus" }, new int[18], new string[4] { "su_surprised", "su_annoyed", "", "su_smile" }, 1);
					}
					else if (!hardmode)
					{
						putDownPie = true;
						gm.SetFlag(316, 1);
						StartText(new string[4] { "* You don't have space\n  for it?", "* Then uhh...^05 I guess\n  I'll put it down here.", "* You can pick it up\n  when you make room.", "* Now let's get going." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_surprised", "su_side", "su_smirk_sweat", "su_neutral" }, 1);
					}
					else
					{
						StartText(new string[4] { "* You don't have space\n  for it?", "* THEN I GUESS I'LL\n  EAT IT THEN!!!", "* (Susie scarfed down the pie.)", "* Now let's get outta\n  here!!!" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_text", "snd_txtsus" }, new int[18], new string[4] { "su_surprised", "su_wtf", "", "su_angry" }, 1);
					}
				}
				state = 6;
				frames = 0;
			}
			else if (txt.GetCurrentStringNum() == 2)
			{
				susie.EnableAnimator();
				susie.GetComponent<Animator>().Play("idle");
				susie.ChangeDirection(Vector2.left);
			}
		}
		if (state == 6)
		{
			if ((bool)txt)
			{
				if (genoID == 2 && noSpace && txt.GetCurrentStringNum() == 3 && !coolSound && !hardmode)
				{
					gm.SetHP(0, 99);
					coolSound = true;
					PlaySFX("sounds/snd_swallow");
				}
				if (putDownPie)
				{
					if (AtLine(2))
					{
						Transform transform = GameObject.Find("Pie").transform;
						transform.position = new Vector3(0f, transform.position.y);
						ChangeDirection(susie, Vector2.right);
					}
					else if (AtLine(3))
					{
						ChangeDirection(susie, Vector2.left);
					}
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_wing");
					gm.SetPartyMembers(true, false);
					kris.EnableAnimator();
					kris.ChangeDirection(Vector2.right);
				}
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(-1.13f, 2.32f), 1f / 12f);
				if (kris.transform.position == new Vector3(-1.13f, 2.32f))
				{
					kris.ChangeDirection(Vector2.down);
					susie.SetSelfAnimControl(true);
					gm.PlayMusic(hardmode ? "music/mus_house1" : "music/mus_home");
					EndCutscene();
				}
			}
		}
		if (state != 7)
		{
			return;
		}
		if (!txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic(30f);
			}
			if (frames == 60)
			{
				StartText(new string[1] { "* HEY!!!^10\n* WAKE UP!!!" }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "" }, 1);
				state = 4;
				frames = 0;
			}
		}
		else if (genoID == 1 && txt.GetCurrentStringNum() == 5 && !aborted)
		{
			aborted = true;
			WeirdChecker.Abort(gm);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		gm.SetFlag(53, 1);
		StartText(new string[1] { "* (You decided to climb into\n  bed.)" }, new string[1] { "snd_text" }, new int[18], new string[1] { "" });
		if ((int)gm.GetFlag(13) == 3)
		{
			genoID = 2;
		}
		else if ((int)gm.GetFlag(13) > 0)
		{
			genoID = 1;
		}
		gm.SetHP(0, 99);
		gm.SetHP(1, 99);
		hardmode = (int)gm.GetFlag(108) == 1;
		krisDream = GameObject.Find("KrisDream").GetComponent<Animator>();
		road = GameObject.Find("Road").transform;
		base.StartCutscene(par);
	}
}
