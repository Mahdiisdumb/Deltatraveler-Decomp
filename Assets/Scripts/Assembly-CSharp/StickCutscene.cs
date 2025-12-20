using UnityEngine;

public class StickCutscene : CutsceneBase
{
	private Vector3 camInitPos;

	private bool susieGrabbedStick;

	private Vector3 noelleNewPos;

	private Animator sans;

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
				cam.SetFollowPlayer(false);
				camInitPos = cam.transform.position;
				noelleNewPos = kris.transform.position - new Vector3(1.25f, 0f) + noelle.GetPositionOffset();
			}
			cam.transform.position = Vector3.Lerp(camInitPos, new Vector3(17f, 0f), (float)frames / 25f);
			if (noelle.transform.position != noelleNewPos)
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", true);
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, noelleNewPos, 1f / 12f);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
			}
			if (susie.transform.position != new Vector3(16.27f, -2.17f))
			{
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetFloat("speed", 3f);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(16.27f, -2.17f), 5f / 24f);
			}
			else if (!susieGrabbedStick)
			{
				susieGrabbedStick = true;
				PlaySFX("sounds/snd_grab");
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.DisableAnimator();
				susie.SetSprite("spr_su_grab_stick");
				Object.Destroy(GameObject.Find("Stick"));
			}
			if (susieGrabbedStick && noelle.transform.position == noelleNewPos && frames >= 45)
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				StartText(new string[7]
				{
					"* HOLY SHIT IT'S A\n  REALLY COOL STICK",
					"* Screw this pencil.",
					"* Susie equipped the\n  Heavy Branch.",
					Items.ItemDrop(6),
					"* Susie,^05 you're...^10 really\n  excited about that\n  tree branch.",
					"* I mean...^10 y'know...^10\n  it isn't a pencil.",
					"* Yeah,^05 okay,^05 Susie."
				}, new string[7] { "snd_txtsus", "snd_txtsus", "snd_text", "snd_text", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[7], new string[7] { "su_excited", "su_smile", "", "", "no_confused", "su_smirk_sweat", "no_playful" }, 0);
				state = 1;
				frames = 0;
				susieGrabbedStick = false;
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3 && !susieGrabbedStick)
				{
					susieGrabbedStick = true;
					gm.ForceWeapon(1, 15);
					PlaySFX("sounds/snd_item");
				}
				if (txt.GetCurrentStringNum() == 5)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_blush");
				}
				if (txt.GetCurrentStringNum() == 6)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_embarrassed_0");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					susie.SetSprite("spr_su_right_unhappy_0");
					gm.PlayMusic("music/mus_snowwalk");
					sans.SetFloat("speed", 0.5f);
					sans.Play("WalkLeft");
					sans.GetComponent<SpriteRenderer>().color = Color.black;
				}
				sans.transform.position = Vector3.Lerp(new Vector3(25f, -2.52f), new Vector3(20f, -2.52f), (float)frames / 135f);
				if (frames == 60)
				{
					noelle.SetSprite("spr_no_right_unhappy_0");
				}
				if (frames == 135)
				{
					sans.SetFloat("speed", 0f);
					susie.SetSprite("spr_su_threaten_stick");
					gm.StopMusic();
					PlaySFX("sounds/snd_weaponpull");
				}
				if (frames > 135 && frames < 140)
				{
					susie.transform.position += new Vector3(1f / 12f, 0f);
				}
				if (frames == 150)
				{
					StartText(new string[40]
					{
						"* Take another step\n  forward and your\n  head is gone.", "*\twhoa,^05 sorry,^05 didn't\n\tmean to scare ya.", "* ... Great,^05 it's\n  this guy.", "*\tyeah,^05 the woods are\n\tpretty dark.", "*\tspeaking of dark,^05 how\n\tare ya doing?", "* Aren't you the guy\n  that runs the\n  convenience store?", "*\t...i don't run a\n\tconvenience store.", "*\ti'm sans.^10\n*\tsans the skeleton.", "*\taren't you that hoofed\n\tgirl from back in\n\ttown?", "*\twhat are you doing\n\tout here?",
						"* What do you mean\n  by that?", "* Are you from our\n  world or something?", "*\t...", "*\tthat tells me everything\n\ti need to know.", "* The hell does that\n  mean?", "*\ti mean,^05 seeing someone\n\tfrom town palling around\n\twith a human...", "*\tthen that same someone\n\tnot recognizing me...", "*\tit's a bit strange\n\tto say the least.", "* Wait,^05 you're not saying\n  what I think you're\n  saying...?", "*\tuhh,^05 let's not think\n\tabout that right now.",
						"*\tyou're trying to make\n\tyour way home,^05 right?", "* Yeah, we're tryna find\n  a scientist dude in\n  Hotland.", "*\tscientist in hotland?", "*\tthe only person that\n\tfits that description is\n\talphys.", "*\tshe might be able to\n\thelp.", "* (THE HELL DOES HE\n  MEAN ALPHYS????)", "* And how do we get\n  there?", "*\tyou're gonna have to\n\tget through the forest.", "*\tthen go through the\n\tmarsh until you reach\n\ta laboratory.", "*\tthat's where you'll be\n\table to meet alphys.",
						"* Sounds pretty easy.", "*\tnot so fast,^05 kid.", "*\tyou've got my brother\n\tto worry about.", "* Is he dangerous?", "*\tnah,^05 but he might\n\ttry to capture that\n\thuman.", "*\the's currently out setting\n\tup his puzzles.", "*\tyou might run into\n\thim on the way.", "*\tso if you encounter him,\n\t^10just play along.", "* Okay...", "*\talright,^05 see ya up ahead."
					}, new string[40]
					{
						"snd_txtsus", "snd_txtsans", "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtnoe", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans",
						"snd_txtnoe", "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtnoe", "snd_txtsans",
						"snd_txtsans", "snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsus", "snd_txtnoe", "snd_txtsans", "snd_txtsans", "snd_txtsans",
						"snd_txtsus", "snd_txtsans", "snd_txtsans", "snd_txtnoe", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsus", "snd_txtsans"
					}, new int[40], new string[40]
					{
						"su_teeth", "sans_neutral", "su_annoyed", "sans_side", "sans_wink", "no_curious", "sans_closed", "sans_wink", "sans_side", "sans_neutral",
						"no_confused", "su_smirk_sweat", "sans_closed", "sans_closed", "su_inquisitive", "sans_rolleye", "sans_neutral", "sans_closed", "no_shocked", "sans_side",
						"sans_neutral", "su_annoyed", "sans_closed", "sans_neutral", "sans_side", "su_wtf", "no_curious", "sans_side", "sans_neutral", "sans_neutral",
						"su_smirk", "sans_neutral", "sans_wink", "no_confused", "sans_side", "sans_rolleye", "sans_neutral", "sans_wink", "su_inquisitive", "sans_wink"
					}, 0);
					state = 2;
					frames = 0;
					susieGrabbedStick = false;
				}
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					sans.Play("WalkLeft", 0, 0f);
					sans.SetFloat("speed", 0f);
					sans.GetComponent<SpriteRenderer>().color = Color.white;
				}
				if (txt.GetCurrentStringNum() == 3 && !susieGrabbedStick)
				{
					susieGrabbedStick = true;
					susie.SetSprite("spr_su_right_unhappy_0");
					PlaySFX("sounds/snd_smallswing");
					gm.PlayMusic("music/mus_muscle");
				}
				if (txt.GetCurrentStringNum() == 13)
				{
					sans.Play("WalkUp");
				}
				if (txt.GetCurrentStringNum() == 16)
				{
					sans.Play("WalkLeft");
				}
				if (txt.GetCurrentStringNum() == 31)
				{
					susie.EnableAnimator();
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic(30f);
					sans.SetFloat("speed", 1f);
					sans.Play("WalkRight", 0, 0f);
				}
				sans.transform.position = Vector3.Lerp(new Vector3(20f, -2.52f), new Vector3(25f, -2.52f), (float)frames / 40f);
				cam.transform.position = Vector3.Lerp(new Vector3(17f, 0f), cam.GetClampedPos(), (float)(frames - 20) / 30f);
				if (frames == 50)
				{
					susie.ChangeDirection(Vector2.left);
					state = 3;
					StartText(new string[8] { "* That was really stupid.", "* ALPHYS is the royal\n  scientist???", "* Is there...^10 another\n  me in this world...?", "* I guess?", "* Does that also mean\n  that...^10 there's also...", "* ...", "* ...Nevermind.^10\n* Let's go.", "* ...?" }, new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[8], new string[8] { "su_annoyed", "su_pissed", "no_confused", "su_inquisitive", "no_sad", "no_depressed", "no_happy", "su_neutral" }, 0);
				}
			}
		}
		if (state != 3)
		{
			return;
		}
		if (!txt)
		{
			cam.SetFollowPlayer(true);
			susie.SetSelfAnimControl(true);
			noelle.SetSelfAnimControl(true);
			kris.ChangeDirection(Vector2.down);
			gm.PlayMusic("music/mus_snowy");
			Object.Destroy(sans.gameObject);
			Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(3f, -2.69f), Quaternion.identity);
			EndCutscene();
		}
		else
		{
			if (txt.GetCurrentStringNum() == 5)
			{
				noelle.EnableAnimator();
				noelle.ChangeDirection(Vector2.up);
			}
			if (txt.GetCurrentStringNum() == 7)
			{
				noelle.ChangeDirection(Vector2.right);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetFlag(60, 1);
		StartText(new string[1] { "* WAIT GUYS HOLD ON" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_surprised" }, 0);
		kris.ChangeDirection(Vector2.right);
		susie.ChangeDirection(Vector2.right);
		noelle.ChangeDirection(Vector2.right);
		susie.SetSelfAnimControl(false);
		susie.GetComponent<Animator>().SetBool("isMoving", false);
		noelle.SetSelfAnimControl(false);
		noelle.GetComponent<Animator>().SetBool("isMoving", false);
		sans = GameObject.Find("Sans").GetComponent<Animator>();
	}
}
