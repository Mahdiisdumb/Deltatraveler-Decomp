using UnityEngine;
using UnityEngine.UI;

public class Section3DreamSequencePt3 : CutsceneBase
{
	private GonerCreator gonerCreator;

	private Animator goner;

	private Animator bed;

	private int sceneVariant;

	private int endingFrames;

	private bool hoveringOverOption;

	private int option;

	private int toPath;

	private Vector3[] gonerPath = new Vector3[4]
	{
		new Vector3(4.6f, 2.65f),
		new Vector3(4.6f, -1.4f),
		new Vector3(2.49f, -1.4f),
		new Vector3(2.49f, -3.18f)
	};

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (frames < 300)
			{
				frames++;
			}
			GameObject.Find("GasterBG").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.75f, (float)frames / 300f));
			if (!txt)
			{
				gm.PlayMusic("music/AUDIO_ANOTHERHIM", 0.7f, 0.75f);
				state = 5;
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames == 30)
			{
				gonerCreator.StartMode(0, resetFrames: true);
			}
			if (frames < 30)
			{
				return;
			}
			if (gonerCreator.GetMode() == 0)
			{
				float num = (float)(frames - 30) / 10f;
				if (num > 1f)
				{
					num = 1f;
				}
				GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, num * 0.6f);
				GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), (float)(frames - 30) / 10f);
				return;
			}
			if ((bool)txt)
			{
				Object.Destroy(txt);
			}
			endingFrames++;
			GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (1f - (float)endingFrames / 10f) * 0.6f);
			GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), 1f - (float)endingFrames / 10f);
			if (endingFrames == 15)
			{
				state = 2;
				frames = 0;
				endingFrames = 0;
				StartText(new string[1] { "\b          WHICH TORSO\n\b          DO YOU PREFER?" }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
				txt.Disable();
				txt.MakeUnskippable();
				txt.EnableGasterText();
				Object.Destroy(GameObject.Find("menuBorder"));
				Object.Destroy(GameObject.Find("menuBox"));
			}
		}
		else if (state == 2)
		{
			frames++;
			if (frames == 30)
			{
				gonerCreator.StartMode(1, resetFrames: true);
			}
			if (frames < 30)
			{
				return;
			}
			if (gonerCreator.GetMode() == 1)
			{
				float num2 = (float)(frames - 30) / 10f;
				if (num2 > 1f)
				{
					num2 = 1f;
				}
				GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, num2 * 0.6f);
				GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), (float)(frames - 30) / 10f);
				return;
			}
			if ((bool)txt)
			{
				Object.Destroy(txt);
			}
			endingFrames++;
			GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (1f - (float)endingFrames / 10f) * 0.6f);
			GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), 1f - (float)endingFrames / 10f);
			if (endingFrames == 15)
			{
				state = 3;
				frames = 0;
				endingFrames = 0;
				StartText(new string[1] { "\b          WHICH LEGS\n\b          DO YOU PREFER?" }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
				txt.Disable();
				txt.MakeUnskippable();
				txt.EnableGasterText();
				Object.Destroy(GameObject.Find("menuBorder"));
				Object.Destroy(GameObject.Find("menuBox"));
			}
		}
		else if (state == 3)
		{
			frames++;
			if (frames == 30)
			{
				gonerCreator.StartMode(2, resetFrames: true);
			}
			if (frames < 30)
			{
				return;
			}
			if (gonerCreator.GetMode() == 2)
			{
				float num3 = (float)(frames - 30) / 10f;
				if (num3 > 1f)
				{
					num3 = 1f;
				}
				GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, num3 * 0.6f);
				GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), (float)(frames - 30) / 10f);
				return;
			}
			if ((bool)txt)
			{
				Object.Destroy(txt);
			}
			endingFrames++;
			GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (1f - (float)endingFrames / 10f) * 0.6f);
			GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), 1f - (float)endingFrames / 10f);
			if (endingFrames == 15)
			{
				state = 4;
				frames = 0;
				endingFrames = 0;
				gonerCreator.transform.position = new Vector3(0f, 0.5f);
				gonerCreator.StartMode(3, resetFrames: true);
				GameObject.Find("BlurrySoul").transform.position = new Vector3(0f, -2.46f);
				StartText(new string[2] { "\b              THIS^15\n\b           IS YOUR BODY.", "\b        DO YOU ACCEPT IT?" }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
				txt.MakeUnskippable();
				txt.EnableGasterText();
				Object.Destroy(GameObject.Find("menuBorder"));
				Object.Destroy(GameObject.Find("menuBox"));
			}
		}
		else if (state == 4)
		{
			if ((bool)txt && txt.GetCurrentStringNum() == 2)
			{
				txt.Disable();
				if (gonerCreator.GetMode() == 3)
				{
					frames++;
					if (UTInput.GetAxis("Horizontal") > 0f)
					{
						option = 1;
						hoveringOverOption = true;
					}
					else if (UTInput.GetAxis("Horizontal") < 0f)
					{
						option = 0;
						hoveringOverOption = true;
					}
					Text[] componentsInChildren = GameObject.Find("Choice").GetComponentsInChildren<Text>();
					foreach (Text text in componentsInChildren)
					{
						text.color = new Color(1f, 1f, (!(option.ToString() == text.gameObject.name) || !hoveringOverOption) ? 1 : 0, (float)frames / 10f);
					}
					float num4 = (float)frames / 10f;
					if (num4 > 1f)
					{
						num4 = 1f;
					}
					GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, num4 * 0.6f);
					GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), (float)frames / 10f);
					if (UTInput.GetButtonDown("Z") && hoveringOverOption)
					{
						Object.Destroy(txt);
						if (option == 1)
						{
							gonerCreator.Hide();
						}
						endingFrames = ((frames < 10) ? (10 - frames) : 0);
					}
				}
			}
			else if (!txt)
			{
				endingFrames++;
				Text[] componentsInChildren = GameObject.Find("Choice").GetComponentsInChildren<Text>();
				foreach (Text text2 in componentsInChildren)
				{
					text2.color = new Color(1f, 1f, (!(option.ToString() == text2.gameObject.name) || !hoveringOverOption) ? 1 : 0, 1f - (float)endingFrames / 10f);
				}
				GameObject.Find("BlurrySoul").GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 0f, (1f - (float)endingFrames / 10f) * 0.6f);
				GameObject.Find("BedSoul").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.red, new Color32(66, 252, byte.MaxValue, byte.MaxValue), 1f - (float)endingFrames / 10f);
				if (endingFrames == 15)
				{
					hoveringOverOption = false;
					if (option == 1)
					{
						StartText(new string[1] { "\b      THEN LET US BEGIN\n\b            ANEW." }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
						txt.MakeUnskippable();
						txt.EnableGasterText();
						Object.Destroy(GameObject.Find("menuBorder"));
						Object.Destroy(GameObject.Find("menuBox"));
						frames = 0;
						state = 5;
					}
					else
					{
						StartText(new string[3] { "\b          EXCELLENT.", "\b       TRULY^15 EXCELLENT.", "\b      NOW,^15 LET US SHAPE\n\b      ITS MIND^15 AS YOUR OWN." }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
						txt.MakeUnskippable();
						txt.EnableGasterText();
						Object.Destroy(GameObject.Find("menuBorder"));
						Object.Destroy(GameObject.Find("menuBox"));
						gonerCreator.SetValues();
						state = 6;
						frames = 0;
					}
				}
			}
			float b = 0f;
			if (hoveringOverOption)
			{
				b = ((option == 0) ? (-2.11f) : 2.11f);
			}
			GameObject.Find("BlurrySoul").transform.position = new Vector3(Mathf.Lerp(GameObject.Find("BlurrySoul").transform.position.x, b, 0.4f), -2.46f);
		}
		else if (state == 5 && !txt)
		{
			frames = 0;
			endingFrames = 0;
			state = 1;
			GameObject.Find("GasterBG").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 0.75f);
			GameObject.Find("BlurrySoul").transform.position = new Vector3(0f, 0.7f);
			gonerCreator.transform.position = new Vector3(0f, -0.96f);
			StartText(new string[1] { "\b          WHICH HEAD\n\b          DO YOU PREFER?" }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
			txt.Disable();
			txt.MakeUnskippable();
			txt.EnableGasterText();
			Object.Destroy(GameObject.Find("menuBorder"));
			Object.Destroy(GameObject.Find("menuBox"));
		}
		else if (state == 6)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic();
					Object.Destroy(gonerCreator.gameObject);
					goner.transform.position = new Vector3(0f, -0.3f);
					goner.transform.localScale = new Vector3(2f, 2f, 1f);
					StartText(new string[1] { (sceneVariant == 2) ? "* Hey,^05 don't be so down,^05 dude." : "* Yeesh,^05 dude...^05\n* That sounds like hell." }, new string[1] { "snd_txtsus" }, new int[1], new string[0], 1);
				}
				else if (frames == 2)
				{
					StartText(new string[2] { "\b          IT APPEARS\n\b        WE HAVE RUN OUT\n\b           OF TIME.", "\b    WE SHALL CONTINUE THIS\n\b^15    VERY,^15 VERY SOON." }, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
					txt.MakeUnskippable();
					txt.EnableGasterText();
					Object.Destroy(GameObject.Find("menuBorder"));
					Object.Destroy(GameObject.Find("menuBox"));
					state = 7;
					frames = 0;
				}
			}
		}
		else if (state == 7)
		{
			if ((bool)txt)
			{
				frames++;
				GameObject.Find("GasterBG").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, Mathf.Lerp(0.75f, 0f, (float)frames / 150f));
				goner.transform.position = Vector3.Lerp(new Vector3(0f, -0.3f), new Vector3(-2.75f, 1.34f), (float)frames / 150f);
				goner.transform.localScale = Vector3.Lerp(new Vector3(2f, 2f, 1f), new Vector3(1f, 1f, 1f), (float)frames / 150f);
				return;
			}
			if (!hoveringOverOption)
			{
				hoveringOverOption = true;
				frames = 0;
				ChangeDirection(goner, Vector2.left);
				SetMoveAnim(goner, isMoving: true);
				goner.GetComponent<SpriteRenderer>().sortingOrder = -60;
				kris.transform.position = new Vector3(-8.42f, -5.16f);
				susie.transform.position = new Vector3(-9.53f, -4.93f);
				noelle.transform.position = new Vector3(-7.3f, -4.93f);
				ChangeDirection(kris, Vector2.right);
				ChangeDirection(susie, Vector2.right);
				ChangeDirection(noelle, Vector2.right);
				SetMoveAnim(kris, isMoving: true);
				SetMoveAnim(susie, isMoving: true);
				SetMoveAnim(noelle, isMoving: true);
				susie.UseHappySprites();
				if (sceneVariant == 2)
				{
					noelle.UseUnhappySprites();
				}
				else
				{
					noelle.UseHappySprites();
				}
			}
			if (MoveTo(goner, new Vector3(-4.05f, 1.34f), 3f))
			{
				return;
			}
			MoveTo(noelle, new Vector3(-2.23f, -4.93f), 4f);
			MoveTo(susie, new Vector3(-4.34f, -4.93f), 4f);
			if (MoveTo(kris, new Vector3(-3.32f, -5.16f), 4f))
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(noelle, Vector2.left);
				SetMoveAnim(kris, isMoving: false);
				SetMoveAnim(susie, isMoving: false);
				SetMoveAnim(noelle, isMoving: false);
			}
			if (frames == 5)
			{
				if (sceneVariant == 2)
				{
					StartText(new string[22]
					{
						"* So what if some\n  weirdo's pulling strings\n  or whatever.", "* We're strong!", "* We've endured this\n  for this long,^05 right?", "* I'd say we give\n  them pain back\n  however we can.", "* Just like you said,^05\n  right?", "* Kris,^05 are you sure\n  about this?", "* It might really affect\n  our chances of getting\n  home.", "* I'm sure.", "* I don't want to go\n  back home like this.", "* Then it's a done deal.",
						"* We'll keep an eye\n  out on your behavior.", "* And if you do any\n  funny business,^05 we'll\n  do what we can.", "* We want to help,^05\n  Kris.", "* I'm sorry we can't\n  just make this go\n  away.", "* We can only hope\n  that it stops doing\n  what it's doing.", "* Thanks,^05 guys.", "* No prob,^05 Kris!", "* ... Oh,^05 you should\n  prolly get that thing\n  back in you.", "* Before you,^05 well...", "* Drop dead.",
						"* We'll see you downstairs,^05\n  Kris!", "* Oh,^05 and Kris..."
					}, new string[22]
					{
						"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtkrs", "snd_txtkrs", "snd_txtsus",
						"snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtkrs", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus",
						"snd_txtnoe", "snd_txtsus"
					}, new int[1], new string[22]
					{
						"su_annoyed", "su_smile", "su_smile_side", "su_teeth", "su_smile_side", "no_depressed_look", "no_depressed_side", "kr_dejected", "kr_dejected", "su_neutral",
						"su_side", "su_teeth", "no_relief", "no_depressed", "no_depressed_side", "kr_relieved", "su_smile", "su_smirk_sweat", "su_side_sweat", "su_inquisitive",
						"no_happy", "su_neutral"
					});
				}
				else
				{
					StartText(new string[10]
					{
						"* Kris,^05 you really\n  should've told us\n  sooner!",
						"* We could've helped out!",
						"* Or...^05 tried to do\n  something,^05 at least.",
						(sceneVariant == 1) ? "* Kris,^05 I think because\n  it stopped..." : "* Kris,^05 don't bother\n  worrying about...",
						(sceneVariant == 1) ? "* Things won't escalate\n  any further." : "* I dunno,^05 standing still\n  I guess?",
						(sceneVariant == 1) ? "* So,^05 I'd say don't\n  worry about this\n  anymore." : "* We're not gonna think\n  it's weird that you're,^05\n  uhh...",
						(sceneVariant == 1) ? "* Wasn't even your fault\n  if you couldn't do\n  anything about it." : "* A little robotic.",
						(sceneVariant == 1) ? "* We don't hate you,^05\n  so you can tell your\n  dream to EAT IT." : "* ...Well,^05 it's a LITTLE\n  weird,^05 but you get it.",
						"* We'll get through this\n  together,^05 Kris!",
						"* I hope we can at least\n  try to keep it under\n  control."
					}, new string[10] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe" }, new int[1], new string[10]
					{
						"no_happy",
						"no_neutral",
						"no_thinking",
						(sceneVariant == 1) ? "su_neutral" : "su_smile",
						(sceneVariant == 1) ? "su_smile_side" : "su_smile_sweat",
						"su_smirk",
						"su_smirk_sweat",
						(sceneVariant == 1) ? "su_teeth_eyes" : "su_inquisitive",
						"no_happy",
						"no_happy"
					});
				}
				state = ((sceneVariant == 2) ? 12 : 8);
				frames = 0;
			}
		}
		else if (state == 8)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					ChangeDirection(kris, Vector2.left);
				}
				else if (AtLine(8) && sceneVariant == 0)
				{
					PlayAnimation(susie, "Embarrassed");
				}
				else if (AtLine(9))
				{
					PlayAnimation(susie, "idle");
					ChangeDirection(kris, Vector2.right);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				ChangeDirection(kris, Vector2.up);
				SetSprite(kris, "depressed/spr_kr_up_0_depressed");
			}
			if (frames == 75)
			{
				kris.EnableAnimator();
				state = 9;
				frames = 0;
				bool flag = sceneVariant == 0 || (int)gm.GetFlag(154) != 0;
				StartText(new string[9]
				{
					"* ...^05 Thanks,^05 guys.",
					"* Don't mention it,^05 dude.",
					"* Anyway,^05 we should prolly\n  get moving.",
					flag ? "* All that talk about\n  \"destabilization\" from\n  earlier is bugging me." : "* I dunno about you,^05\n  but jumping between a\n  bunch of worlds...",
					flag ? "* We need to find\n  Alphys before that gets\n  worse." : "* Feels like something\n  that could really get\n  outta hand soon.",
					"* Let's get going.",
					"* Well,^05 after you grab\n  your...^05 thing.",
					"* Yeah...\n^05* See you two downstairs.",
					"* Alright,^05 Kris!\n* See you in a bit!"
				}, new string[9] { "snd_txtkrs", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtkrs", "snd_txtnoe" }, new int[1], new string[9] { "kr_relieved", "su_smile", "su_smile_side", "su_side_sweat", "su_neutral", "su_smirk", "su_smirk_sweat", "kr_unimpressed", "no_happy" });
			}
		}
		else if (state == 9)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					ChangeDirection(kris, Vector2.left);
				}
				else if (AtLine(8))
				{
					ChangeDirection(kris, Vector2.up);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlaySFX("sounds/snd_dooropen");
				SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_1");
				ChangeDirection(susie, Vector2.left);
				ChangeDirection(noelle, Vector2.down);
				SetMoveAnim(susie, isMoving: true);
				SetMoveAnim(noelle, isMoving: true);
			}
			if (noelle.transform.position.y > -5.84f)
			{
				MoveTo(noelle, new Vector3(noelle.transform.position.x, -5.84f), 4f);
			}
			else if (MoveTo(noelle, new Vector3(-7.57f, -5.84f), 4f))
			{
				ChangeDirection(noelle, Vector2.left);
			}
			MoveTo(susie, new Vector3(-10f, -4.93f), 4f);
			if (frames >= 30 && frames <= 120)
			{
				if (!MoveTo(kris, new Vector3(-3.32f, -2.46f), 4f))
				{
					SetMoveAnim(kris, isMoving: false);
				}
				else
				{
					SetMoveAnim(kris, isMoving: true);
				}
				if (frames == 60)
				{
					PlaySFX("sounds/snd_doorclose");
					SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_0");
				}
			}
			if (frames == 120)
			{
				PlayAnimation(kris, "Collapse");
				PlaySFX("sounds/snd_noise");
			}
			if (frames >= 210 && frames <= 390)
			{
				if (frames == 210)
				{
					kris.GetComponent<SpriteRenderer>().flipX = true;
					PlayAnimation(kris, "StumbleWalkUp");
				}
				kris.transform.position = Vector3.Lerp(new Vector3(-3.32f, -2.46f), new Vector3(-2.85f, 1.41f), (float)(frames - 210) / 180f);
				if (frames % 30 == 15)
				{
					int num5 = 2;
					if (frames % 60 == 15)
					{
						num5 = 1;
					}
					PlaySFX("sounds/snd_step" + num5);
				}
				if (frames == 390)
				{
					PlayAnimation(kris, "StumbleWalkUp", 0f);
				}
			}
			if (frames == 435)
			{
				kris.GetComponent<SpriteRenderer>().enabled = false;
				kris.GetComponent<SpriteRenderer>().flipX = false;
				PlaySFX("sounds/snd_wing");
				SetSprite(bed, "overworld/snow_objects/spr_bunny_singlebed_nocover_putsoulin_8");
				Object.Destroy(GameObject.Find("BedSoul"));
			}
			if (frames == 465)
			{
				bed.enabled = true;
				bed.SetFloat("speed", -1f);
				bed.Play("SoulUnderBed", 0, 0.99f);
			}
			if (frames == 520)
			{
				PlayAnimation(bed, "InsertSoul");
				PlaySFX("sounds/snd_wing");
			}
			if (frames == 612)
			{
				PlaySFX("sounds/snd_grab");
				gm.PlayGlobalSFX("sounds/snd_hurt");
				Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), new Vector3(-3.53f, 1.62f), Quaternion.identity);
				cam.SetFollowPlayer(follow: true);
				cam.StartHitShake();
			}
			if (frames == 655)
			{
				PlaySFX("sounds/snd_bump");
			}
			if (frames == 700)
			{
				PlaySFX("sounds/snd_wing");
				SetSprite(bed, "overworld/snow_objects/spr_bunny_singlebed_nocover");
				kris.GetComponent<SpriteRenderer>().enabled = true;
				PlayAnimation(kris, "idle");
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				GameObject.Find("DoorInteract").GetComponent<BoxCollider2D>().enabled = true;
				gm.SetFlag(210, 1);
				if (sceneVariant != 2)
				{
					gm.EnablePlayerMovement();
				}
				else
				{
					gm.SetFlag(211, 1);
					gm.SetFlag(0, "smug");
				}
				state = 10;
				frames = 0;
			}
		}
		else if (state == 10)
		{
			frames++;
			if (frames == 30 && sceneVariant == 2)
			{
				new GameObject("Lol").AddComponent<TextBox>().CreateBox(new string[1] { "* You can now access new\n  ACTs in battle!" }, giveBackControl: true);
			}
			if (frames == 900)
			{
				frames = 0;
				state = 11;
				SetSprite(GameObject.Find("NoelleBedSide").transform, "overworld/snow_objects/spr_bnuy_home_1_bed");
				GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>().enabled = true;
				goner.transform.position = new Vector3(-10f, 0f);
				goner.GetComponent<SpriteRenderer>().sortingOrder = 0;
				goner.gameObject.AddComponent<AutoSort>();
				SetMoveAnim(goner, isMoving: false);
				ChangeDirection(goner, Vector2.down);
			}
		}
		else if (state == 11)
		{
			if (frames <= 120)
			{
				frames++;
				if (frames <= 30)
				{
					goner.transform.position = new Vector3(2.37f, Mathf.Lerp(1.8f, 2.65f, (float)frames / 30f));
				}
				if (frames == 45)
				{
					PlayAnimation(goner, "CreepySmile");
				}
				if (frames == 120)
				{
					PlayAnimation(goner, "walk");
					ChangeDirection(goner, Vector2.right);
					SetMoveAnim(goner, isMoving: true);
				}
				return;
			}
			if (toPath < 4)
			{
				if (!MoveTo(goner, gonerPath[toPath], 4f))
				{
					toPath++;
					if (toPath == 1 || toPath == 3)
					{
						ChangeDirection(goner, Vector2.down);
					}
					else if (toPath == 2)
					{
						ChangeDirection(goner, Vector2.left);
					}
					else
					{
						SetMoveAnim(goner, isMoving: false);
					}
				}
				return;
			}
			frames++;
			if (frames == 150)
			{
				PlaySFX("sounds/snd_dooropen");
				SetSprite(GameObject.Find("Doors").transform, "overworld/snow_objects/spr_bnuy_doors_2");
			}
			if (frames >= 180 && frames <= 300)
			{
				if (frames == 180)
				{
					SetMoveAnim(goner, isMoving: true);
				}
				if (goner.transform.position.y > -4.85f)
				{
					MoveTo(goner, new Vector3(2.49f, -4.85f), 4f);
				}
				else
				{
					ChangeDirection(goner, Vector2.right);
					MoveTo(goner, new Vector3(7.27f, -4.85f), 4f);
				}
				if (frames == 300)
				{
					PlaySFX("sounds/snd_glassbreak");
				}
			}
		}
		else if (state == 12)
		{
			if ((bool)txt)
			{
				if (AtLine(1))
				{
					ChangeDirection(kris, Vector2.left);
				}
				else if (AtLine(6))
				{
					ChangeDirection(kris, Vector2.right);
				}
				else if (AtLine(7))
				{
					ChangeDirection(noelle, Vector2.up);
				}
				else if (AtLine(8))
				{
					ChangeDirection(noelle, Vector2.left);
				}
				else if (AtLine(10))
				{
					ChangeDirection(kris, Vector2.left);
					SetSprite(susie, "spr_su_shrug");
				}
				else if (AtLine(11))
				{
					susie.EnableAnimator();
				}
				else if (AtLine(13))
				{
					ChangeDirection(kris, Vector2.right);
				}
				else if (AtLine(16))
				{
					ChangeDirection(kris, Vector2.up);
					noelle.UseHappySprites();
				}
				else if (AtLine(18))
				{
					susie.UseUnhappySprites();
				}
				else if (AtLine(19))
				{
					PlayAnimation(susie, "Embarrassed");
				}
				else if (AtLine(21))
				{
					PlayAnimation(susie, "idle");
				}
			}
			else if (!MoveTo(susie, new Vector3(-4.04f, -4.93f), 4f))
			{
				frames++;
				if (frames == 1)
				{
					SetSprite(kris, "spr_kr_hugsusie_6");
					kris.transform.position += new Vector3(-0.38f, -0.166f);
					susie.GetComponent<SpriteRenderer>().enabled = false;
				}
				if (frames == 20)
				{
					SetSprite(kris, "spr_kr_hugsusie_4");
					PlaySFX("sounds/snd_swallow");
				}
				if (frames == 50)
				{
					StartText(new string[3] { "* Don't make me cry\n  again.", "* This stays between the\n  three of us.", "* Yeah,^05 I got it." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtkrs" }, new int[1], new string[3] { "su_smile", "su_smile_side", "kr_relieved_side" });
					state = 13;
					frames = 0;
				}
			}
			else
			{
				SetMoveAnim(susie, isMoving: true);
			}
		}
		else if (state == 13 && !txt)
		{
			frames++;
			if (frames == 10)
			{
				SetSprite(kris, "spr_kr_hugsusie_5");
			}
			else if (frames == 30)
			{
				SetSprite(kris, "spr_kr_hugsusie_6");
				PlaySFX("sounds/snd_wing");
			}
			else if (frames == 45)
			{
				kris.transform.position -= new Vector3(-0.38f, -0.166f);
				susie.GetComponent<SpriteRenderer>().enabled = true;
				kris.EnableAnimator();
				state = 9;
				frames = 0;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		GameObject.Find("BedSoul").transform.position = new Vector3(-4.124f, 1.433f);
		gonerCreator = GameObject.Find("GonerCreatorMain").AddComponent<GonerCreator>();
		goner = GameObject.Find("Goner").GetComponent<Animator>();
		bed = GameObject.Find("Bed").GetComponent<Animator>();
		int num = 12 - gm.GetPlayerName().Length / 2;
		string text = "\b";
		for (int i = 0; i < num; i++)
		{
			text += " ";
		}
		if ((int)gm.GetFlag(87) >= 4 && WeirdChecker.HasCommittedBloodshed(gm))
		{
			sceneVariant = (((int)gm.GetFlag(12) == 0) ? 1 : 2);
		}
		gm.SetFlag(1, "smile");
		gm.SetFlag(2, (sceneVariant == 0) ? "happy" : "relief");
		StartText(new string[8]
		{
			"\b          EXCELLENT.",
			"\b       TRULY^15 EXCELLENT.",
			"\b      WE FINALLY\n^15\b      HAVE TIME\n^15\b      FOR A HEART TO HEART.",
			text + "SO,^15 ^N.",
			"\b   I AM SURE IT IS EXHAUSTING^15\n\b        TO BE TETHERED^15\n\b           TO KRIS.",
			"\b     WHY DON'T I OFFER YOU^15\n\b        AN OPPORTUNITY^15\n\b          TO ESCAPE?",
			"\b       I SHALL ALLOW YOU^15\n\b           TO CRAFT^15\n\b           A VESSEL.",
			"\b            LET US^15\n\b            BEGIN."
		}, new string[1] { "" }, new int[1] { 2 }, new string[0], 0);
		txt.MakeUnskippable();
		txt.EnableGasterText();
		Object.Destroy(GameObject.Find("menuBorder"));
		Object.Destroy(GameObject.Find("menuBox"));
	}
}
