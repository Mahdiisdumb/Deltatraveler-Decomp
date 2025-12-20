using System;
using UnityEngine;

public class ToriOutisdeHomeCutscene : CutsceneBase
{
	private Animator toriel;

	private int torielPosIndex;

	private int edge;

	private bool selecting;

	private bool musicStart;

	private bool hardmode;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			cam.transform.position = Vector3.Lerp(Vector3.zero, cam.GetClampedPos(), (float)frames / 150f);
			if (frames == 60)
			{
				StartText(new string[1] { hardmode ? "* ...if I could just convince\n  them to not..." : "* ...if this magic truly did\n  work,^15 then I..." }, new string[3] { "snd_txttor", "snd_txtsus", "snd_txtsus" }, new int[18]
				{
					1, 0, 0, 0, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0
				}, new string[3] { "", "su_side_sweat", "su_smile_sweat" }, 0);
			}
			kris.transform.position = Vector3.Lerp(new Vector3(0f, -17.625f), new Vector3(0f, -14.9f), (float)(frames - 130) / 45f);
			susie.transform.position = Vector3.Lerp(new Vector3(0f, -18.51f), new Vector3(0f, -16.01f), (float)(frames - 130) / 45f);
			if (frames >= 175)
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
				if (!txt)
				{
					if ((int)gm.GetFlag(13) < 3)
					{
						state = 1;
						string[] array = new string[7] { "* Umm,^10 hi,^05 Ms. Dreemurr.", "* Oh!\n^10* Hello Kris,^10 Susie!", "* It is wonderful that\n  you were able to make\n  it here unharmed.", "* Hell yeah,^05 we did!", "* My home is just\n  up ahead.", "* Please follow me\n  inside.", "* I have a surprise\n  for you." };
						string[] array2 = new string[7] { "su_side_sweat", "tori_blush", "tori_happy", "su_happy", "tori_neutral", "tori_neutral", "tori_neutral" };
						if (hardmode)
						{
							array[1] = "* Oh!\n^10* Hello,^05 you two!";
							array[2] = "* Susie,^05 I knew that\n  I could trust you\n  with the human child.";
							array[3] = "* Heh,^05 why wouldn't you?";
						}
						else if ((int)gm.GetFlag(13) == 2)
						{
							array[4] = "* ...Kris?\n* You don't look very\n  well,^10 dear.";
							array2[4] = "tori_worry";
						}
						if (gm.GetHP(0) != gm.GetMaxHP(0) || gm.GetHP(1) != gm.GetMaxHP(1))
						{
							array[3] = (hardmode ? "* (If only she knew.)" : "* (Umm,^05 no???)");
							array2[3] = "su_smirk_sweat";
						}
						StartText(array, new string[7] { "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], array2, 0);
					}
					else if (hardmode)
					{
						StartText(new string[9] { "* Umm,^10 hi,^05 Ms. Dreemurr.", "* Oh!\n^10* Hello,^05 you two!", "* ... Huh?\n* My child,^05 you don't look\n  too well.", "* Is everything alright,^05\n  dear?", "* ...", "* ...", "* Maybe,^05 uhhh,^05 they need\n  to rest.", "* Oh!^10\n* Then why don't you\n  come inside?", "* I have a surprise\n  for you." }, new string[9] { "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18], new string[9] { "su_side_sweat", "tori_blush", "tori_worry", "tori_worry", "tori_worry", "tori_weird", "su_smile_sweat", "tori_blush", "tori_neutral" }, 0);
						state = 1;
						edge++;
					}
					else
					{
						StartText(new string[4] { "* Umm,^10 hi,^05 Ms. Dreemurr.", "* Oh!\n^10* Hello Kris,^10 Susie!", "* Oh, Kris!^10\n* You look miserable.", "* What's wrong,^10 dear?" }, new string[4] { "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "su_side_sweat", "tori_blush", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
						txt.EnableSelectionAtEnd();
						state = 3;
					}
				}
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && !musicStart)
				{
					if ((int)gm.GetFlag(13) != 3)
					{
						gm.PlayMusic("music/mus_toriel", 0.75f);
					}
					musicStart = true;
					toriel.Play("WalkDown");
				}
			}
			else if (torielPosIndex == 0)
			{
				toriel.SetFloat("speed", 1f);
				toriel.Play("WalkRight");
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(2.94f, -13.22f), 1f / 6f);
				if (toriel.transform.position == new Vector3(2.94f, -13.22f))
				{
					torielPosIndex = 1;
				}
			}
			else if (torielPosIndex == 1)
			{
				toriel.Play("WalkUp");
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(2.94f, -6f), 1f / 6f);
				if (toriel.transform.position == new Vector3(2.94f, -6f))
				{
					gm.StopMusic(60f);
					toriel.GetComponent<SpriteRenderer>().enabled = false;
					state = 2;
					if (hardmode && edge > 0)
					{
						StartText(new string[2] { "* Now that I think\n  about it,^05 you could\n  use some shut-eye.", "* Yeah,^05 you might just\n  be going crazy from\n  this place..." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_neutral", "su_inquisitive" }, 0);
					}
					else if (edge == 1)
					{
						StartText(new string[3] { "* Not gonna lie,^05 Kris.", "* You need to rest.", "* She probably has like...\n  ^10a bedroom made for us." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side", "su_neutral", "su_smile_sweat" }, 0);
					}
					else if (edge == 2)
					{
						StartText(new string[3] { "* You okay,^05 Kris?", "* You were shaking.", "* Why don't you go\n  to sleep when we\n  go inside?" }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_side_sweat", "su_side", "su_smirk" }, 0);
					}
					else
					{
						StartText(new string[3] { "* A surprise...?", "* I bet she made us\n  a pie or something.", "* It isn't a surprise if\n  she does this stuff all\n  the time." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_surprised", "su_confident", "su_teeth_eyes" }, 0);
					}
				}
			}
		}
		if (state == 2 && !txt)
		{
			gm.PlayMusic("zoneMusic");
			kris.GetComponent<BoxCollider2D>().enabled = true;
			kris.SetSelfAnimControl(setAnimControl: true);
			susie.SetSelfAnimControl(setAnimControl: true);
			cam.SetFollowPlayer(follow: true);
			EndCutscene();
		}
		if (state == 3 && (bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2)
			{
				toriel.Play("WalkDown");
			}
			if (txt.CanLoadSelection() && !selecting)
			{
				InitiateDeltaSelection();
				select.SetupChoice(Vector2.left, "I need rest", Vector3.zero);
				select.SetupChoice(Vector2.right, "I feel great", new Vector3(-90f, 0f));
				select.SetCenterOffset(new Vector2(0f, 0f));
				select.Activate(this, 0, txt.gameObject);
				selecting = true;
			}
		}
		if (state == 4 && (bool)txt && txt.CanLoadSelection() && !selecting)
		{
			InitiateDeltaSelection();
			select.SetupChoice(Vector2.left, "I don't know", Vector3.zero);
			select.SetupChoice(Vector2.right, "I'm getting\nstronger", new Vector3(-90f, 0f));
			select.SetCenterOffset(new Vector2(0f, 0f));
			select.Activate(this, 0, txt.gameObject);
			selecting = true;
		}
		if (state == 5 && (bool)txt && txt.CanLoadSelection() && !selecting)
		{
			InitiateDeltaSelection();
			select.SetupChoice(Vector2.left, "I'm very tired", Vector3.zero);
			select.SetupChoice(Vector2.right, "We killed\nthem all", new Vector3(-80f, 0f));
			select.SetCenterOffset(new Vector2(0f, 0f));
			select.Activate(this, 0, txt.gameObject);
			selecting = true;
		}
		if (state == 6)
		{
			if (!txt)
			{
				frames++;
				if (frames <= 3)
				{
					int num = ((frames % 2 == 0) ? 1 : (-1));
					int num2 = 3 - frames;
					kris.transform.position = new Vector3(0f, -14.9f) + new Vector3((float)(num2 * num) / 24f, 0f);
				}
				if (frames >= 10)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.GetComponent<Animator>().SetFloat("speed", 2f);
					susie.transform.position = Vector3.Lerp(new Vector3(0f, -16.01f), new Vector3(1.43f, -14.82f), (float)(frames - 10) / 20f);
				}
				if (frames == 30)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					susie.ChangeDirection(Vector2.left);
					StartText(new string[2] { "* Kris,^05 what the hell\n  are you muttering?", "* I don't think she" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "su_annoyed", "su_annoyed", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				}
			}
			else if (txt.GetCurrentStringNum() == 2 && !txt.IsPlaying())
			{
				state = 7;
				frames = 0;
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				UnityEngine.Object.Destroy(txt.gameObject);
			}
		}
		if (state == 7)
		{
			frames++;
			if (frames == 30)
			{
				toriel.Play("Shocked");
			}
			if (frames >= 38 && frames <= 41)
			{
				int num3 = ((frames % 2 == 0) ? 1 : (-1));
				int num4 = 41 - frames;
				kris.transform.position = new Vector3(0f, -14.9f) + new Vector3((float)(num4 * num3) / 24f, 0f);
			}
			if (frames >= 48 && frames <= 51)
			{
				int num5 = ((frames % 2 == 0) ? 1 : (-1));
				int num6 = 51 - frames;
				kris.transform.position = new Vector3(0f, -14.9f) + new Vector3((float)(num6 * num5) / 24f, 0f);
			}
			if (frames >= 58 && frames <= 61)
			{
				int num7 = ((frames % 2 == 0) ? 1 : (-1));
				int num8 = 61 - frames;
				kris.transform.position = new Vector3(0f, -14.9f) + new Vector3((float)(num8 * num7) / 24f, 0f);
			}
			if (frames >= 64 && frames <= 67)
			{
				int num9 = ((frames % 2 == 0) ? 1 : (-1));
				int num10 = 67 - frames;
				kris.transform.position = new Vector3(0f, -14.9f) + new Vector3((float)(num10 * num9) / 24f, 0f);
				susie.transform.position = new Vector3(1.43f, -14.82f) + new Vector3((float)(num10 * num9) / 24f, 0f);
			}
			if (frames == 38 || frames == 48 || frames == 58)
			{
				PlaySFX("sounds/snd_bump");
			}
			if (frames == 64)
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), new Vector3(-0.031f, -15f), Quaternion.identity);
				PlaySFX("sounds/snd_grab");
				susie.DisableAnimator();
				susie.SetSprite("spr_su_surprise_right");
				susie.GetComponent<SpriteRenderer>().flipX = true;
			}
			if (frames == 90)
			{
				kris.GetComponent<Animator>().Play("SoulThrow_Left");
			}
			if (frames == 92)
			{
				PlaySFX("sounds/snd_heavyswing");
			}
			if (frames == 94)
			{
				GameObject.Find("FakeSOUL").transform.position = kris.transform.position;
				GameObject.Find("FakeSOUL").GetComponent<Animator>().Play("Yeet", 0, 0f);
			}
			if (frames == 96)
			{
				PlaySFX("sounds/snd_hurt");
			}
			if (frames == 96)
			{
				cam.transform.position += new Vector3(0.1f, 0.1f);
			}
			if (frames == 98)
			{
				cam.transform.position -= new Vector3(0.15f, 0.15f);
			}
			if (frames == 100)
			{
				cam.transform.position += new Vector3(0.05f, 0.05f);
			}
			if (frames >= 110)
			{
				float num11 = frames - 110;
				if (num11 == 0f)
				{
					string text = ((gm.GetWeapon(0) == 3) ? "Pencil" : "Knife");
					kris.GetComponent<Animator>().Play("Lunge" + text);
				}
				if (num11 >= 6f)
				{
					if (num11 == 6f)
					{
						PlaySFX("sounds/snd_weaponpull");
					}
					kris.transform.position = new Vector3(Mathf.Lerp(0f, -2.3f, (num11 - 6f) / 15f), Mathf.Sin(Mathf.Lerp(0f, 170f, (num11 - 6f) / 15f) * (MathF.PI / 180f)) - 14.9f);
				}
				if (num11 == 22f)
				{
					cam.transform.position += new Vector3(0.1f, 0.1f);
					gm.PlayGlobalSFX("sounds/snd_hurt");
					gm.Death(0);
				}
			}
		}
		if (state != 8)
		{
			return;
		}
		if (!txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.Death();
			}
		}
		else if (txt.GetCurrentStringNum() == 3)
		{
			kris.ChangeDirection(Vector2.down);
			susie.DisableAnimator();
			susie.SetSprite("spr_su_shrug");
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selecting = false;
		if (state == 3)
		{
			if (index == Vector2.right)
			{
				edge = 1;
				gm.StopMusic();
				StartText(new string[1] { "* Huh?^10\n* What do you mean\n  by that?" }, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "tori_worry", "tori_neutral", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				state = 4;
				txt.EnableSelectionAtEnd();
			}
			else
			{
				StartText(new string[2] { "* Oh!^10\n* Then why don't you\n  come inside?", "* I have a surprise\n  for you." }, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "tori_blush", "tori_neutral", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				state = 1;
			}
		}
		else if (state == 4)
		{
			if (index == Vector2.right)
			{
				edge = 2;
				gm.PlayMusic("music/mus_prebattle1", 0.25f);
				StartText(new string[2]
				{
					"* What are you talking\n  about...?",
					hardmode ? "* (I swear,^05 if you're\n  gonna say what I think\n  you're gonna say...)" : "* (Kris,^05 now's not the\n  time to be freaking\n  out.)"
				}, new string[4] { "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "tori_weird", "su_side_sweat", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				state = 5;
				txt.EnableSelectionAtEnd();
			}
			else
			{
				StartText(new string[2]
				{
					hardmode ? "* My child,^10 you might\n  just be tired." : "* Kris,^10 you might just\n  be tired.",
					"* Come with me\n^10* I have a surprise\n  for you."
				}, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "tori_worry", "tori_neutral", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				state = 1;
			}
		}
		else
		{
			if (state != 5)
			{
				return;
			}
			gm.StopMusic();
			if (index == Vector2.right)
			{
				if (hardmode)
				{
					toriel.Play("Shocked");
					state = 8;
					frames = 0;
					StartText(new string[4] { "* ...", "* Y-^05you mean you...", "* Well,^05 well,^05 well.\n^05* Look who decided to\n  reveal themselves.", "* I warned you,^05 you <color=#FF0000FF>FREAK</color>." }, new string[4] { "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "tori_shocked", "tori_shocked", "su_depressed_smile", "su_teeth" }, 0);
				}
				else
				{
					frames = 0;
					state = 6;
					kris.GetComponent<Animator>().Play("RemoveSoul_WalkUp");
					kris.GetComponent<Animator>().SetFloat("speed", 0f);
					PlaySFX("sounds/snd_bump");
				}
			}
			else
			{
				StartText(new string[2] { "* ...", "* Why don't you two\n  follow me?" }, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[7] { "tori_worry", "tori_worry", "tori_worry", "tori_worry", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				state = 1;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(51) == 1)
		{
			EndCutscene();
			return;
		}
		kris.GetComponent<BoxCollider2D>().enabled = false;
		base.StartCutscene(par);
		hardmode = (int)gm.GetFlag(108) == 1;
		gm.StopMusic(30f);
		gm.SetFlag(51, 1);
		kris.SetSelfAnimControl(setAnimControl: false);
		kris.ChangeDirection(Vector2.up);
		kris.GetComponent<Animator>().SetBool("isMoving", value: true);
		susie.SetSelfAnimControl(setAnimControl: false);
		susie.ChangeDirection(Vector2.up);
		susie.GetComponent<Animator>().SetBool("isMoving", value: true);
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		toriel.transform.position = new Vector3(0f, -13.22f);
		toriel.Play("WalkUp");
		cam.SetFollowPlayer(follow: false);
		cam.transform.position = Vector3.zero;
	}
}
