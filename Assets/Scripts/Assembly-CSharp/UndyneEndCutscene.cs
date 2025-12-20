using System;
using UnityEngine;

public class UndyneEndCutscene : CutsceneBase
{
	private bool selecting;

	private bool altText;

	private bool knowAboutDestab = true;

	private Animator undyne;

	private int krisLookFrames;

	private float velocity;

	private SpriteRenderer greyDoor;

	private SpriteRenderer krisBW;

	private int ki;

	private TextZelda text;

	private bool oblit;

	private int advanceFrames;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (cam.transform.position.x <= 15.82f)
			{
				cam.transform.position += new Vector3(1f / 12f, 0f);
				return;
			}
			frames++;
			if (frames == 15)
			{
				if (oblit)
				{
					StartText(new string[14]
					{
						"* Heya Kris.", "* ... Yeah,^05 it's another\n  god damn door.", "* No,^05 we are NOT going\n  inside of it.", "* Not risking making yet\n  another blood bath in\n  ANOTHER world.", "* So we're just gonna\n  need to find another\n  way forward.", "* Actually,^05 I think I\n  found something.", "* Yeah?^10\n* What?", "* There's a weird\n  message on this\n  telescope.", "* I think there's a\n  hidden door at the\n  end of the hall.", "* Huh.^10\n* Convenient.",
						"* Nice job,^05 Noelle.", "* ...!", "* T-^05thanks,^10 Susie...!", "* Arright,^05 let's get\n  moving."
					}, new string[14]
					{
						"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus",
						"snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus"
					}, new int[1], new string[14]
					{
						"su_neutral", "su_annoyed", "su_annoyed", "su_disappointed", "su_side", "no_confused", "su_surprised", "no_nobitches", "no_confused_side", "su_side",
						"su_smile", "no_surprised_happy", "no_blush", "su_smile_side"
					}, 0);
				}
				else
				{
					StartText(new string[5] { "* Heya Kris.", "* ... Yeah,^05 it's another\n  god damn door.", "* I really don't wanna\n  go in another one\n  of these things.", "* ... But I dunno if\n  we're gonna be safe\n  in here.", "* Whaddya think?" }, new string[1] { "snd_txtsus" }, new int[1], new string[5] { "su_neutral", "su_annoyed", "su_side", "su_smirk_sweat", "su_neutral" }, 0);
				}
				if (!oblit)
				{
					txt.EnableSelectionAtEnd();
				}
				state = ((!oblit) ? 1 : 2);
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt && txt.CanLoadSelection() && !selecting)
			{
				InitiateDeltaSelection();
				select.SetupChoice(Vector2.left, "Go through\ndoor", Vector3.zero);
				select.SetupChoice(Vector2.right, "Find another\nway", new Vector3(-90f, 0f));
				select.Activate(this, 0, txt.gameObject);
				selecting = true;
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				if (oblit)
				{
					if (AtLine(3))
					{
						SetSprite(susie, "spr_su_point_right_unhappy", true);
					}
					else if (AtLine(4))
					{
						ChangeDirection(susie, Vector2.left);
						susie.EnableAnimator();
						susie.GetComponent<SpriteRenderer>().flipX = false;
					}
					else if (AtLine(6))
					{
						ChangeDirection(kris, Vector2.down);
						ChangeDirection(susie, Vector2.down);
						ChangeDirection(noelle, Vector2.up);
						noelle.EnableAnimator();
					}
					else if (AtLine(8))
					{
						SetSprite(noelle, "spr_no_telescope");
					}
					else if (AtLine(9))
					{
						noelle.EnableAnimator();
					}
					else if (AtLine(11))
					{
						susie.UseHappySprites();
					}
					else if (AtLine(12))
					{
						SetSprite(noelle, "spr_no_blush");
					}
					else if (AtLine(13))
					{
						noelle.EnableAnimator();
					}
					else if (AtLine(14))
					{
						ChangeDirection(kris, Vector2.right);
						ChangeDirection(susie, Vector2.left);
					}
				}
				else if (altText)
				{
					if (AtLine(2))
					{
						susie.EnableAnimator();
						susie.UseUnhappySprites();
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(3))
					{
						ChangeDirection(kris, Vector2.down);
						ChangeDirection(susie, Vector2.down);
						ChangeDirection(noelle, Vector2.up);
						noelle.EnableAnimator();
					}
					else if (AtLine(5))
					{
						SetSprite(noelle, "spr_no_telescope");
					}
					else if (AtLine(6))
					{
						noelle.EnableAnimator();
					}
					else if (AtLine(7))
					{
						ChangeDirection(kris, Vector2.right);
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(8))
					{
						SetSprite(susie, "spr_su_shrug");
					}
					else if (AtLine(9))
					{
						susie.UseHappySprites();
						susie.EnableAnimator();
						ChangeDirection(susie, Vector2.right);
					}
					else if (AtLine(10))
					{
						PlayAnimation(susie, "Embarrassed");
					}
					else if (AtLine(11))
					{
						PlayAnimation(susie, "idle");
						ChangeDirection(susie, Vector2.left);
					}
				}
				else if (AtLine(2))
				{
					SetSprite(susie, "spr_su_shrug");
				}
				else if (AtLine(3))
				{
					PlayAnimation(susie, "Embarrassed");
				}
				else if (AtLine(4))
				{
					PlayAnimation(susie, "idle");
					susie.UseUnhappySprites();
					ChangeDirection(susie, Vector2.left);
				}
				else if (AtLine(5))
				{
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(susie, Vector2.down);
					ChangeDirection(noelle, Vector2.up);
					noelle.EnableAnimator();
				}
				else if (AtLine(7))
				{
					SetSprite(noelle, "spr_no_telescope");
				}
				else if (AtLine(8))
				{
					noelle.EnableAnimator();
				}
				else if (AtLine(9))
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(10))
				{
					SetSprite(susie, "spr_su_shrug");
				}
				else if (AtLine(12))
				{
					susie.UseHappySprites();
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.left);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlaySFX("sounds/snd_escaped");
				SetMoveAnim(kris, true);
				SetMoveAnim(susie, true);
				SetMoveAnim(noelle, true);
				ChangeDirection(kris, Vector2.up);
			}
			if (frames < 55)
			{
				if (frames == 40)
				{
					fade.FadeOut(20);
				}
				if (susie.transform.position.x > 15.22f)
				{
					susie.transform.position += new Vector3(-1f / 12f, 0f);
				}
				else
				{
					ChangeDirection(susie, Vector2.up);
					susie.transform.position += new Vector3(0f, 1f / 12f);
				}
				if (kris.transform.position.y < 2.62f)
				{
					kris.transform.position += new Vector3(0f, 1f / 12f);
				}
				else
				{
					krisLookFrames++;
					if (krisLookFrames == 1)
					{
						SetMoveAnim(kris, false);
					}
					if (krisLookFrames == 10)
					{
						SetSprite(kris, "spr_kr_surprise_upright");
					}
				}
				noelle.transform.position += new Vector3(0f, 1f / 12f);
				float num = (float)(frames - 45) / 10f;
				if (num > 0f)
				{
					num *= num;
				}
				undyne.transform.position = new Vector3(14.61f, Mathf.Lerp(10.87f, 4.79f, num));
			}
			else
			{
				if (frames == 55)
				{
					fade.FadeIn(0);
					undyne.transform.position = new Vector3(14.61f, 4.79f);
					SetSprite(kris, "spr_kr_surprise_upright");
					SetSprite(noelle, "spr_no_surprise_up");
					SetSprite(susie, "spr_su_surprise_up", true);
					SetSprite(undyne, "overworld/npcs/waterfall/spr_undyne_a_jump_0");
					PlaySFX("sounds/snd_crash");
					cam.SetFollowPlayer(true);
					cam.StartHitShake();
				}
				if (frames == 75)
				{
					undyne.enabled = true;
					SetMoveAnim(undyne, true);
					SetMoveAnim(kris, true, 1.5f);
					SetMoveAnim(susie, true, 1.5f);
					SetMoveAnim(noelle, true, 1.5f);
					kris.EnableAnimator();
					susie.EnableAnimator();
					noelle.EnableAnimator();
					susie.UseUnhappySprites();
					noelle.UseUnhappySprites();
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
				if (frames > 75)
				{
					if (kris.transform.position.y > -1.5f)
					{
						kris.transform.position -= new Vector3(0f, 1f / 6f);
					}
					else if (kris.transform.position.x < 18.83f)
					{
						ChangeDirection(kris, Vector2.left);
						kris.transform.position += new Vector3(1f / 6f, 0f);
					}
					else
					{
						SetMoveAnim(kris, false);
					}
					if (susie.transform.position.y > -0.36f)
					{
						susie.transform.position -= new Vector3(0f, 1f / 6f);
					}
					else if (susie.transform.position.x < 18.83f)
					{
						ChangeDirection(susie, Vector2.left);
						susie.transform.position += new Vector3(1f / 6f, 0f);
					}
					else
					{
						SetMoveAnim(susie, false);
					}
					if (noelle.transform.position.y > -2.34f)
					{
						noelle.transform.position -= new Vector3(0f, 1f / 6f);
					}
					else if (noelle.transform.position.x < 18.83f)
					{
						ChangeDirection(noelle, Vector2.left);
						noelle.transform.position += new Vector3(1f / 6f, 0f);
					}
					else
					{
						SetMoveAnim(noelle, false);
					}
					if (undyne.transform.position.y > -1.24f)
					{
						undyne.transform.position -= new Vector3(0f, 1f / 6f);
					}
					else if (undyne.transform.position.x < 16.47f)
					{
						undyne.transform.position += new Vector3(1f / 24f, 0f);
						ChangeDirection(undyne, Vector2.right);
					}
					else
					{
						SetMoveAnim(undyne, false);
					}
				}
			}
			if (frames == 180)
			{
				StartText(new string[2] { "* D-^05damn it...!^05\n* We're cornered!", "* You two,^05 step away\n  from the human!" }, new string[2] { "snd_txtsus", "snd_txtund" }, new int[1], new string[2] { "su_shocked", "und_helm" }, 0);
				state = 3;
				frames = 0;
			}
		}
		else if (state == 3 && !txt)
		{
			if (susie.transform.position.x < 20.51f)
			{
				susie.transform.position += new Vector3(1f / 24f, 0f);
				noelle.transform.position += new Vector3(1f / 24f, 0f);
			}
			else
			{
				SetMoveAnim(susie, false);
				SetMoveAnim(noelle, false);
			}
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(susie, true, 0.5f);
				SetMoveAnim(noelle, true, 0.5f);
			}
			if (frames == 75)
			{
				bool flag = Util.GameManager().GetFlagInt(87) >= 7;
				StartText(new string[3]
				{
					"* Human.^05\n* Either surrender your\n  SOUL now...",
					"* Or I'll rip it\n  out of your chest!",
					flag ? "* Hey,^05 I wouldn't do\n  that if I were--" : "* Heh,^05 as if you\n  can take on the\n  thr--"
				}, new string[3] { "snd_txtund", "snd_txtund", "snd_txtsus" }, new int[1], new string[3]
				{
					"und_helm",
					"und_helm",
					flag ? "su_concerned" : "su_confident"
				}, 0);
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)txt)
			{
				if (AtLine(3) && !oblit)
				{
					SetSprite(susie, "spr_su_shrug");
				}
				if (AtLineRepeat(3))
				{
					advanceFrames++;
					if (advanceFrames == 10)
					{
						txt.ForceAdvanceCurrentLine();
					}
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				PlayAnimation(kris, "RemoveSoulUndyne");
			}
			if (frames == 18)
			{
				gm.PlayGlobalSFX("sounds/snd_grab");
				PlaySFX("sounds/snd_hurt");
				susie.EnableAnimator();
				SetSprite(noelle, "spr_no_surprise_left");
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
				cam.StartHitShake();
			}
			if (frames >= 18 && frames <= 28)
			{
				SetMoveAnim(undyne, true);
				undyne.transform.position -= new Vector3(1f / 24f, 0f);
			}
			else
			{
				SetMoveAnim(undyne, false);
			}
			if (frames == 43)
			{
				SetSprite(susie, "spr_su_surprise_right", true);
				PlaySFX("sounds/snd_grab");
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), kris.transform.position, Quaternion.identity);
				cam.StartHitShake();
			}
			if (frames == 80)
			{
				StartText(new string[8] { "* L...^10 like this?", "* ...", "* I...^10 umm...", "* I guess???", "* Quick question,^05 then.", "* If I were to jump\n  through this door\n  into another world...", "* A world which might\n  not even have a\n  way to come back...", "* Would you still\n  chase me?" }, new string[8] { "snd_txtkrs", "snd_txtund", "snd_txtund", "snd_txtund", "snd_txtkrs", "snd_txtkrs", "snd_txtkrs", "snd_txtkrs" }, new int[2] { 1, 0 }, new string[8] { "kr_susgrin", "und_helm_weird_forward", "und_helm_weird", "und_helm_confused", "kr_smirk_half", "kr_side", "kr_side", "kr_smug" }, 0);
				state = 5;
				frames = 0;
				txt.gameObject.AddComponent<ShakingText>().StartShake(5);
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					txt.gameObject.GetComponent<ShakingText>().Stop();
				}
				else if (AtLine(5))
				{
					SetSprite(kris, "spr_kr_taunt_undyne_0");
					txt.gameObject.GetComponent<ShakingText>().StartShake(100);
				}
				else if (AtLine(7))
				{
					txt.gameObject.GetComponent<ShakingText>().Stop();
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				susie.EnableAnimator();
				susie.GetComponent<SpriteRenderer>().flipX = false;
				SetSprite(noelle, "spr_no_think_right_panic", true);
			}
			if (frames == 45)
			{
				SetSprite(kris, "spr_kr_taunt_undyne_1");
				StartText(new string[6]
				{
					"* Well?",
					oblit ? "* (KRIS,^05 WHAT THE HELL\n  DID I JUST SAY!?)" : "* (KRIS,^05 WHAT THE HELL\n  ARE YOU DOING???)",
					"* Well,^05 if I can't\n  get back here with\n  the SOUL...",
					"* There wouldn't be\n  a point.",
					"* Thought so.",
					"* See ya."
				}, new string[6] { "snd_txtkrs", "snd_txtsus", "snd_txtund", "snd_txtund", "snd_txtkrs", "snd_txtkrs" }, new int[1], new string[6] { "kr_annoyed", "su_wtf", "und_helm_weird", "und_helm_confused", "kr_annoyed", "kr_smug" }, 0);
				state = 6;
				frames = 0;
			}
		}
		else if (state == 6)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					SetSprite(susie, "spr_su_wtf", true);
				}
				else if (AtLine(3))
				{
					ChangeDirection(undyne, Vector2.up);
				}
				else if (AtLine(6))
				{
					ChangeDirection(undyne, Vector2.right);
					SetSprite(noelle, "spr_no_surprise_left");
					SetSprite(susie, "spr_su_surprise_right", true);
					SetSprite(kris, "spr_kr_taunt_undyne_0");
				}
			}
			else
			{
				kris.EnableAnimator();
				PlayAnimation(kris, "idle");
				ChangeDirection(kris, Vector2.up);
				state = 51;
				SetSprite(noelle, "spr_no_panic_right", true);
				GameObject.Find("Canvas").GetComponent<Canvas>().sortingOrder = 200;
				StartText(new string[1] { oblit ? "* Kris,^05 stop!!!" : "* Kris,^05 wait!!!^05\n* Don't jump without\n  us!" }, new string[1] { "snd_txtnoe" }, new int[1], new string[1] { "no_scared" }, 0);
				txt.Disable();
			}
		}
		else if (state == 51)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.sprite = Resources.Load<Sprite>("overworld/spr_grey_door_1");
				PlaySFX("sounds/snd_elecdoor_shutheavy");
				cam.SetFollowPlayer(false);
				frames = 45;
			}
			if (frames >= 45 && frames <= 65)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 0f);
				kris.GetComponent<Animator>().Play("RunUp", 0, 0f);
				kris.transform.position = Vector3.Lerp(new Vector3(18.83f, -1.5f), new Vector3(18.83f, -2.3600001f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
			}
			else if (frames >= 65 && frames <= 75)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				kris.transform.position = Vector3.Lerp(new Vector3(18.83f, -1.5f), new Vector3(18.83f, -0.22f), (float)(frames - 65) / 10f);
			}
			if (frames == 75)
			{
				kris.GetComponent<Animator>().Play("Fall", 0, 0f);
				state = 52;
				frames = 0;
			}
		}
		else if (state == 52)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.GetComponent<AudioSource>().Play();
			}
			if (frames <= 15)
			{
				float num2 = (float)frames / 15f;
				num2 = Mathf.Sin(num2 * (float)Math.PI * 0.5f);
				kris.transform.position = Vector3.Lerp(new Vector3(18.83f, -0.22f), new Vector3(18.83f, 0.45000002f), num2);
			}
			if (frames < 50)
			{
				greyDoor.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f) * Mathf.Lerp(0f, 20f, (float)frames / 40f);
				greyDoor.GetComponent<AudioSource>().pitch = Mathf.Lerp(0.8f, 1.15f, (float)frames / 10f);
			}
			else if (frames > 50)
			{
				if (frames == 51)
				{
					velocity = 0f;
					greyDoor.GetComponent<AudioSource>().pitch = 0.8f;
					greyDoor.GetComponent<AudioSource>().volume = 0f;
					greyDoor.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_dtrans_drone");
					greyDoor.GetComponent<AudioSource>().loop = true;
					greyDoor.GetComponent<AudioSource>().Play();
					GameObject.Find("Canvas").GetComponent<Canvas>().sortingOrder = 1000;
					if ((bool)txt)
					{
						UnityEngine.Object.Destroy(txt.gameObject);
					}
				}
				if (frames < 120)
				{
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, (float)(frames - 50) / 60f);
				}
				else
				{
					if (frames == 120)
					{
						PlaySFX("sounds/snd_dtrans_lw");
						fade.FadeOut(60, Color.white);
					}
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, (float)(frames - 120) / 30f);
				}
				if (velocity < 0.5f)
				{
					velocity += 0.01f;
				}
				kris.transform.position += new Vector3(-1f / 96f, -1f / 32f) * velocity;
			}
			if (frames >= 40 && frames % 15 == 1)
			{
				SpriteRenderer component = new GameObject("GreyDoorBGSquare", typeof(SpriteRenderer), typeof(GreyDoorBGSquare)).GetComponent<SpriteRenderer>();
				component.sprite = Resources.Load<Sprite>("spr_pixel");
				component.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y);
			}
			if (frames == 180)
			{
				state = 53;
				frames = 0;
			}
		}
		else if (state == 53)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeIn(60, Color.white);
				GameObject.Find("Black").transform.position = Vector3.zero;
			}
			if (frames == 120)
			{
				PlayerPrefs.SetInt("CompletionState", 3);
				gm.SetFramerate(60);
				text = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/zelda/TextZelda"), GameObject.Find("Canvas").transform).GetComponent<TextZelda>();
				text.transform.localScale *= 2f;
				text.StartTextBox(new string[1] { "To be continued..." }, false);
				state = 54;
				frames = 0;
			}
		}
		else if (state == 54 && !text)
		{
			frames++;
			if (frames == 1)
			{
				gm.SetFramerate(30);
				PlaySFX("sounds/zelda/snd_lowhp");
			}
			if (frames == 60)
			{
				gm.ForceLoadArea(128);
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 52)
		{
			kris.GetComponent<SpriteRenderer>().sortingOrder = 500;
			Color color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)(frames - 50) / 45f);
			int num = int.Parse(kris.GetComponent<SpriteRenderer>().sprite.name.Substring(kris.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			if (ki != num)
			{
				ki = num;
				krisBW.sprite = Resources.Load<Sprite>("player/Kris/spr_kr_fall_bw_" + ki);
			}
			krisBW.transform.position = kris.transform.position;
			krisBW.color = color;
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			altText = true;
			StartText(new string[11]
			{
				"* ...",
				"* Well,^05 if you think\n  we should,^05 then...",
				"* Wait,^05 I think I\n  found something...!",
				"* What?",
				"* There's a weird\n  message on this\n  telescope.",
				"* I think there's a\n  hidden door at the\n  end of the hall.",
				"* ...",
				"* Y'know what,^05 let's just\n  stay here.",
				"* It'd prolly be better\n  than maybe making things\n  worse.",
				knowAboutDestab ? "* Y'know,^05 universe\n  destabilization and\n  all." : "* Don't wanna risk jumping\n  back into the crazy\n  dimension again.",
				"* Let's get going."
			}, new string[11]
			{
				"snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus",
				"snd_txtsus"
			}, new int[1], new string[11]
			{
				"su_disappointed", "su_annoyed", "no_confused", "su_surprised", "no_nobitches", "no_confused_side", "su_surprised", "su_confident", "su_smile", "su_smile_sweat",
				"su_smile"
			}, 0);
		}
		else
		{
			StartText(new string[13]
			{
				"* ...",
				"* Yeah,^05 it'd prolly be\n  better if we DIDN'T\n  make things worse.",
				knowAboutDestab ? "* Y'know,^05 universe\n  destabilization and\n  all." : "* Don't wanna risk jumping\n  back into the crazy\n  dimension again.",
				"* But then what the\n  hell do we do?",
				"* I think I found\n  something...",
				"* What?",
				"* There's a weird\n  message on this\n  telescope.",
				"* I think there's a\n  hidden door at the\n  end of the hall.",
				"* ...",
				"* Noelle solving a puzzle\n  instead of Kris?^05\n* Shocker.",
				"* Hey!!!",
				"* Well,^05 we found another\n  way out.",
				"* Let's get going."
			}, new string[13]
			{
				"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus",
				"snd_txtnoe", "snd_txtsus", "snd_txtsus"
			}, new int[1], new string[13]
			{
				"su_side", "su_annoyed", "su_smile_sweat", "su_inquisitive", "no_thinking", "su_surprised", "no_nobitches", "no_confused_side", "su_surprised", "su_confident",
				"no_playful", "su_smile_side", "su_smile"
			}, 0);
		}
		state = 2;
		frames = 0;
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		cam.SetFollowPlayer(false);
		ChangeDirection(kris, Vector2.right);
		SetMoveAnim(kris, false);
		knowAboutDestab = gm.GetFlagInt(154) != 0;
		oblit = Util.GameManager().GetFlagInt(13) >= 10;
		gm.UnlockMenu();
		undyne = GameObject.Find("Undyne").GetComponent<Animator>();
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
		krisBW = GameObject.Find("KrisBW").GetComponent<SpriteRenderer>();
	}
}
