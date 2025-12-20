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
				cam.SetFollowPlayer(follow: false);
				camInitPos = cam.transform.position;
				noelleNewPos = kris.transform.position - new Vector3(1.25f, 0f) + noelle.GetPositionOffset();
			}
			cam.transform.position = Vector3.Lerp(camInitPos, new Vector3(17f, 0f), (float)frames / 25f);
			if (noelle.transform.position != noelleNewPos)
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, noelleNewPos, 1f / 12f);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (susie.transform.position != new Vector3(16.27f, -2.17f))
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.GetComponent<Animator>().SetFloat("speed", 3f);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(16.27f, -2.17f), 5f / 24f);
			}
			else if (!susieGrabbedStick)
			{
				susieGrabbedStick = true;
				PlaySFX("sounds/snd_grab");
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.DisableAnimator();
				susie.SetSprite("spr_su_grab_stick");
				Object.Destroy(GameObject.Find("Stick"));
			}
			if (susieGrabbedStick && noelle.transform.position == noelleNewPos && frames >= 45)
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
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
					susie.SetSprite("unhappy/spr_su_right_0_unhappy");
					gm.PlayMusic("music/mus_snowwalk");
					sans.SetFloat("speed", 0.5f);
					sans.Play("WalkLeft");
					sans.GetComponent<SpriteRenderer>().color = Color.black;
				}
				sans.transform.position = Vector3.Lerp(new Vector3(25f, -2.52f), new Vector3(20f, -2.52f), (float)frames / 135f);
				if (frames == 60)
				{
					noelle.SetSprite("unhappy/spr_no_right_0_unhappy");
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
						"* Take another step\n  forward and your\n  head is gone.", "* whoa,^05 sorry,^05 didn't\n  mean to scare ya.", "* ... Great,^05 it's\n  this guy.", "* yeah,^05 the woods are\n  pretty dark.", "* speaking of dark,^05 how\n  are ya doing?", "* Aren't you the guy\n  that runs the\n  convenience store?", "* ...i don't run a\n  convenience store.", "* i'm sans.^10\n* sans the skeleton.", "* aren't you that hoofed\n  girl from back in\n  town?", "* what are you doing\n  out here?",
						"* What do you mean\n  by that?", "* Are you from our\n  world or something?", "* ...", "* that tells me everything\n  i need to know.", "* The hell does that\n  mean?", "* i mean,^05 seeing someone\n  from town palling around\n  with a human...", "* then that same someone\n  not recognizing me...", "* it's a bit strange\n  to say the least.", "* Wait,^05 you're not saying\n  what I think you're\n  saying...?", "* uhh,^05 let's not think\n  about that right now.",
						"* you're trying to make\n  your way home,^05 right?", "* Yeah, we're tryna find\n  a scientist dude in\n  Hotland.", "* scientist in hotland?", "* the only person that\n  fits that description is\n  alphys.", "* she might be able to\n  help.", "* (THE HELL DOES HE\n  MEAN ALPHYS????)", "* And how do we get\n  there?", "* you're gonna have to\n  get through the forest.", "* then go through the\n  marsh until you reach\n  a laboratory.", "* that's where you'll be\n  able to meet alphys.",
						"* Sounds pretty easy.", "* not so fast,^05 kid.", "* you've got my brother\n  to worry about.", "* Is he dangerous?", "* nah,^05 but he might\n  try to capture that\n  human.", "* he's currently out\n  setting up his puzzles.", "* you might run into\n  him on the way.", "* so if you encounter him,\n  ^10just play along.", "* Okay...", "* alright,^05 see ya up ahead."
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
					susie.SetSprite("unhappy/spr_su_right_0_unhappy");
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
			cam.SetFollowPlayer(follow: true);
			susie.SetSelfAnimControl(setAnimControl: true);
			noelle.SetSelfAnimControl(setAnimControl: true);
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
		MonoBehaviour.print("bo");
		gm.SetFlag(60, 1);
		StartText(new string[1] { "* WAIT GUYS HOLD ON" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_surprised" }, 0);
		kris.ChangeDirection(Vector2.right);
		susie.ChangeDirection(Vector2.right);
		noelle.ChangeDirection(Vector2.right);
		susie.SetSelfAnimControl(setAnimControl: false);
		susie.GetComponent<Animator>().SetBool("isMoving", value: false);
		noelle.SetSelfAnimControl(setAnimControl: false);
		noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
		sans = GameObject.Find("Sans").GetComponent<Animator>();
	}
}
