using System;
using UnityEngine;

public class FloweyBossIntro : CutsceneBase
{
	private Animator floweyVine;

	private Animator flowey;

	private bool grabbedKris;

	private int grabbedKrisFrame;

	private float grabbingKrisY;

	private float krisFallY;

	private float velocity;

	private int floweyFaceState;

	private int routeState;

	private bool quickStart;

	private bool hardmode;

	private bool animatingVineHardmode;

	private Vector3 vinePos = Vector3.zero;

	private int vineFrames;

	private Vector3 initSusiePos;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			float num = 0f;
			float y = grabbingKrisY;
			if (grabbedKris)
			{
				float num2 = frames - grabbedKrisFrame;
				float num3 = num2 - 65f;
				y = Mathf.Lerp(grabbingKrisY, 1.26f, num2 / 15f) + Mathf.Sin(num2 * 2f * (MathF.PI / 180f)) / 2f;
				if (num2 < 5f)
				{
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(susie.transform.position.x, -100f), 1f / 12f);
				}
				if (num2 == 25f || num2 == 45f || num2 == 65f)
				{
					PlaySFX("sounds/snd_bump");
				}
				if (num2 >= 45f && num2 <= 48f)
				{
					int num4 = ((frames % 2 == 0) ? 1 : (-1));
					num = (48f - num2) * (float)num4 / 24f;
				}
				if (num2 >= 65f && num2 <= 68f)
				{
					int num5 = ((frames % 2 == 0) ? 1 : (-1));
					num = (68f - num2) * (float)num5 / 24f;
				}
				if (num2 >= 25f && num2 <= 28f)
				{
					int num6 = ((frames % 2 == 0) ? 1 : (-1));
					num = (28f - num2) * (float)num6 / 24f;
				}
				if (num3 == 0f)
				{
					floweyVine.Play("StealSOUL", 0, 0f);
				}
				if (num3 == 14f)
				{
					GameObject.Find("White").GetComponent<SpriteRenderer>().enabled = true;
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULRemoveEffect"), new Vector3(-2.234f, floweyVine.transform.position.y + 0.209878f), Quaternion.identity);
				}
				if (num3 >= 14f && num3 <= 17f)
				{
					int num7 = ((frames % 2 == 0) ? 1 : (-1));
					num = (17f - num3) * (float)num7 / 24f;
				}
				if (num3 >= 36f && num3 <= 39f)
				{
					int num8 = ((frames % 2 == 0) ? 1 : (-1));
					num = (39f - num3) * (float)num8 / 24f;
				}
				if (num3 >= 46f && num3 <= 49f)
				{
					int num9 = ((frames % 2 == 0) ? 1 : (-1));
					num = (49f - num3) * (float)num9 / 24f;
				}
				if (num3 >= 56f && num3 <= 59f)
				{
					int num10 = ((frames % 2 == 0) ? 1 : (-1));
					num = (59f - num3) * (float)num10 / 24f;
				}
				if (num3 == 64f)
				{
					UnityEngine.Object.Destroy(GameObject.Find("White"));
					PlaySFX("sounds/snd_damage");
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SusieSlashDR"), new Vector3(0f, floweyVine.transform.position.y), Quaternion.identity);
					floweyVine.enabled = false;
					floweyVine.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_flowey_vine_kris_9");
					floweyVine.GetComponent<SpriteRenderer>().sortingOrder = -100;
					floweyVine.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
					susie.transform.position = new Vector3(1.11f, -1.59f);
					susie.SetSprite("spr_su_save_kris");
					state = 1;
					frames = 0;
					krisFallY = floweyVine.transform.position.y + 0.2491478f;
					kris.transform.position = new Vector3(-2.5f, krisFallY);
					kris.DisableAnimator();
					kris.GetComponent<SpriteRenderer>().enabled = true;
					kris.SetSprite("spr_kr_vinefall");
				}
			}
			if (state != 1)
			{
				floweyVine.transform.position = new Vector3(Mathf.Lerp(6.74f, -5.75f, (float)frames / 10f) + num, y);
			}
			if (floweyVine.transform.position.x <= kris.transform.position.x && !grabbedKris)
			{
				gm.PlayGlobalSFX("sounds/snd_damage");
				kris.GetComponent<SpriteRenderer>().enabled = false;
				floweyVine.enabled = true;
				floweyVine.Play("GrabKris", 0, 0f);
				grabbedKris = true;
				grabbedKrisFrame = frames;
				floweyVine.GetComponent<SpriteRenderer>().sortingOrder = 100;
				susie.DisableAnimator();
				susie.SetSprite("spr_su_surprise_up");
			}
		}
		if (state == 1)
		{
			frames++;
			float num11 = (float)frames / 15f;
			if (frames == 1)
			{
				cam.transform.position += new Vector3(0.15f, 0.15f);
			}
			if (frames == 3)
			{
				cam.transform.position -= new Vector3(0.25f, 0.25f);
			}
			if (frames == 5)
			{
				cam.transform.position += new Vector3(0.1f, 0.1f);
			}
			if (frames <= 15)
			{
				kris.transform.position = Vector3.Lerp(new Vector3(-2.5f, krisFallY), new Vector3(-2.5f, -1.8f), num11 * num11);
			}
			if (frames == 15)
			{
				kris.SetSprite("spr_kr_ko");
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 30 && quickStart)
			{
				state = 2;
				frames = 0;
			}
			if (frames == 45)
			{
				gm.PlayMusic("music/mus_gallery");
				StartText(new string[1] { "* Back off." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_determined" }, 0);
				state = 2;
				frames = 0;
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				flowey.enabled = true;
				kris.SetSprite("spr_kr_sit");
				PlaySFX("sounds/snd_wing");
			}
			if (frames <= 3)
			{
				int num12 = ((frames % 2 == 0) ? 1 : (-1));
				float num13 = 3 - frames;
				kris.transform.position = new Vector3(-2.5f, -1.8f) + new Vector3(num13 * (float)num12 / 24f, 0f);
			}
			if (floweyVine.transform.position.x < 8.34f)
			{
				velocity += 1f / 64f;
				floweyVine.transform.position = Vector3.MoveTowards(floweyVine.transform.position, new Vector3(8.34f, 1.16f), velocity);
			}
			if (frames == 45 && quickStart)
			{
				state = 3;
				frames = 0;
				flowey.enabled = false;
			}
			if (frames == 50)
			{
				if (routeState == 1)
				{
					StartText(new string[13]
					{
						"* Hee hee hee...", "* And let you leave?", "* Run away to your\n  \"other world\"?", "* Back to the comfort\n  of your loving mommy?", "* Boy,^05 just hearing\n  there's even MORE\n  worlds out there...", "* IMAGINE the fun I'd\n  have!", "* Shut up!", "* Just get the hell\n  away from us.", "* Or WHAT?", "* You'll kill me?",
						"* You wouldn't kill your\n  best friend,^05 would\n  you,^05 Kris?", "* Like all the ones\n  that you DID kill.\n^10* Right,^05 murderer?", "* Arright,^05 you've gone too\n  damn far."
					}, new string[13]
					{
						"snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtflw",
						"snd_txtflw", "snd_txtflw", "snd_txtsus"
					}, new int[16], new string[13]
					{
						"flowey_neutral", "flowey_side", "flowey_sassy", "flowey_tori", "flowey_neutral", "flowey_grin", "su_annoyed_sweat", "su_annoyed_sweat", "flowey_side", "flowey_sassy",
						"flowey_asriel", "flowey_asriel_fuckedup", "su_annoyed"
					}, 0);
				}
				else if (routeState == 2)
				{
					StartText(new string[14]
					{
						"* Hee hee hee...", "* And let you leave?", "* Run away to your\n  \"other world\"?", "* Back to the comfort\n  of your loving mommy?", "* Boy,^05 just hearing\n  there's even MORE\n  worlds out there...", "* IMAGINE the fun I'd\n  have!", "* Shut up!", "* Just get the hell\n  away from us.", "* Or WHAT?", "* You'll kill me?",
						"* You wouldn't kill your\n  best friend,^05 would\n  you,^05 Kris?", "* Like all the ones\n  that you ruthlessly\n  murdered?", "* You're empty inside,^05\n  just like me!", "* Arright,^05 you've gone too\n  damn far."
					}, new string[14]
					{
						"snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtflw",
						"snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus"
					}, new int[16], new string[14]
					{
						"flowey_neutral", "flowey_side", "flowey_sassy", "flowey_tori", "flowey_neutral", "flowey_grin", "su_annoyed_sweat", "su_annoyed_sweat", "flowey_side", "flowey_sassy",
						"flowey_asriel", "flowey_asriel_fuckedup", "flowey_asriel_fuckedup", "su_annoyed"
					}, 0);
				}
				else
				{
					StartText(new string[13]
					{
						"* Hee hee hee...", "* And let you leave?", "* Run away to your\n  \"other world\"?", "* Back to the comfort\n  of your loving mommy?", "* Boy,^05 just hearing\n  there's even MORE\n  worlds out there...", "* IMAGINE the fun I'd\n  have!", "* Shut up!", "* Just get the hell\n  away from us.", "* Or WHAT?", "* You'll kill me?",
						"* You wouldn't kill your\n  best friend,^05 would\n  you,^05 Kris?", "* You wouldn't let him\n  get away from you,\n^05  WOULD YOU KRIS?", "* Arright,^05 you've gone too\n  damn far."
					}, new string[13]
					{
						"snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtflw",
						"snd_txtflw", "snd_txtflw", "snd_txtsus"
					}, new int[16], new string[13]
					{
						"flowey_neutral", "flowey_side", "flowey_sassy", "flowey_tori", "flowey_neutral", "flowey_grin", "su_annoyed_sweat", "su_annoyed_sweat", "flowey_side", "flowey_sassy",
						"flowey_asriel", "flowey_asriel_fuckedup", "su_annoyed"
					}, 0);
				}
				state = 3;
				frames = 0;
				flowey.enabled = false;
			}
		}
		if (state == 3)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && floweyFaceState < 1)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_side");
				}
				if (txt.GetCurrentStringNum() == 3 && floweyFaceState < 2)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sassy");
				}
				if (txt.GetCurrentStringNum() == 4 && floweyFaceState < 3)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_tori_face");
				}
				if (txt.GetCurrentStringNum() == 5 && floweyFaceState < 4)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_rise_7");
				}
				if (txt.GetCurrentStringNum() == 6 && floweyFaceState < 5)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_laugh_0");
				}
				if (txt.GetCurrentStringNum() == 7 && floweyFaceState == 5)
				{
					floweyFaceState = 0;
				}
				if (txt.GetCurrentStringNum() == 9 && floweyFaceState < 1)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_side");
				}
				if (txt.GetCurrentStringNum() == 10 && floweyFaceState < 2)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sassy");
				}
				if (txt.GetCurrentStringNum() >= 11)
				{
					if (UnityEngine.Random.Range(0, (txt.GetCurrentStringNum() == 11) ? 12 : 6) == 0)
					{
						int num14 = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
						kris.transform.position = new Vector3(-2.5f + (float)UnityEngine.Random.Range(1, 3) / 48f * (float)num14, -1.8f);
					}
					else
					{
						kris.transform.position = new Vector3(-2.5f, -1.8f);
					}
				}
				if (txt.GetCurrentStringNum() == 11 && floweyFaceState < 3)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_asriel_face");
				}
				if (txt.GetCurrentStringNum() == 12 && floweyFaceState < 4)
				{
					floweyFaceState++;
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_asriel_face_fuckedup");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_rise_7");
					kris.EnableAnimator();
					string text = ((gm.GetWeapon(0) == 3) ? "Pencil" : "Knife");
					kris.GetComponent<Animator>().Play("Brandish" + text);
					susie.EnableAnimator();
					susie.SetSelfAnimControl(setAnimControl: false);
					susie.GetComponent<Animator>().Play("PreparePencil");
					PlaySFX("sounds/snd_weaponpull");
				}
				if (frames == 25 && quickStart)
				{
					kris.InitiateBattle(14);
					EndCutscene(enablePlayerMovement: false);
				}
				if (frames == 30)
				{
					if (routeState == 2)
					{
						StartText(new string[5] { "* You're gonna be\n  compost after I'm\n  done with you.", "* Oh^05 really?", "* Why,^05 this can't be\n  happening!", "* This is all just\n  a bad dream...", "* ...And you'll NEVER\n  wake up!" }, new string[5] { "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw2" }, new int[6] { 0, 0, 0, 0, 2, 0 }, new string[5] { "su_teeth_determined", "flowey_sad_0", "flowey_sad_1", "flowey_sad_2", "flowey_evil" }, 0);
					}
					else
					{
						StartText(new string[5] { "* You're going to eat\n  those words,^05 plant.", "* Aww,^05 little old me?", "* That's just silly!", "* Why would I do that...", "* ...When I can have your\n  SOULs instead?" }, new string[5] { "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw2" }, new int[6] { 0, 0, 0, 0, 2, 0 }, new string[5] { "su_teeth_determined", "flowey_side", "flowey_sassy", "flowey_side", "flowey_evil" }, 0);
					}
					state = 4;
					frames = 0;
					floweyFaceState = 0;
				}
			}
		}
		if (state == 4)
		{
			if ((bool)txt)
			{
				if (routeState == 2)
				{
					if (txt.GetCurrentStringNum() == 2 && floweyFaceState < 1)
					{
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sad_0");
					}
					if (txt.GetCurrentStringNum() == 3 && floweyFaceState < 2)
					{
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sad_1");
					}
					if (txt.GetCurrentStringNum() == 4 && floweyFaceState < 3)
					{
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sad_2");
					}
					if (txt.GetCurrentStringNum() == 5 && floweyFaceState < 4)
					{
						gm.StopMusic();
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_evil");
						txt.gameObject.AddComponent<ShakingText>().StartShake(0);
					}
				}
				else
				{
					if (txt.GetCurrentStringNum() == 2 && floweyFaceState < 1)
					{
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_side");
					}
					if (txt.GetCurrentStringNum() == 3 && floweyFaceState < 2)
					{
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sassy");
					}
					if (txt.GetCurrentStringNum() == 4 && floweyFaceState < 3)
					{
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_side");
					}
					if (txt.GetCurrentStringNum() == 5 && floweyFaceState < 4)
					{
						gm.StopMusic();
						floweyFaceState++;
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_evil");
						txt.gameObject.AddComponent<ShakingText>().StartShake(0);
					}
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					flowey.enabled = true;
					flowey.Play("Laugh");
					PlaySFX("sounds/snd_floweylaugh");
				}
				if (frames == 140)
				{
					kris.InitiateBattle(hardmode ? 40 : 14);
					EndCutscene(enablePlayerMovement: false);
				}
			}
		}
		if (state == 5)
		{
			if ((bool)txt)
			{
				string text2 = "";
				if (routeState == 2)
				{
					if (txt.GetCurrentStringNum() == 4 || txt.GetCurrentStringNum() == 10)
					{
						text2 = "sassy";
					}
					else if (txt.GetCurrentStringNum() == 8)
					{
						text2 = "laugh_0";
					}
					else if (txt.GetCurrentStringNum() == 5 || txt.GetCurrentStringNum() == 11)
					{
						text2 = "side";
					}
					else if (txt.GetCurrentStringNum() == 6 || txt.GetCurrentStringNum() == 9 || txt.GetCurrentStringNum() == 12)
					{
						text2 = "evil";
					}
					else if (txt.GetCurrentStringNum() == 7)
					{
						text2 = "sad_0";
					}
				}
				else if (txt.GetCurrentStringNum() == 1 || txt.GetCurrentStringNum() == 7)
				{
					text2 = "sassy";
				}
				else if (txt.GetCurrentStringNum() == 5 || txt.GetCurrentStringNum() == 8)
				{
					text2 = "rise_7";
				}
				else if (txt.GetCurrentStringNum() == 6)
				{
					text2 = "side";
				}
				else if (txt.GetCurrentStringNum() == 9)
				{
					text2 = "evil";
				}
				if (text2 != "" && !flowey.GetComponent<SpriteRenderer>().sprite.name.EndsWith(text2))
				{
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_" + text2);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					animatingVineHardmode = true;
					PlaySFX("sounds/snd_spearrise");
					if (quickStart)
					{
						flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sassy");
					}
				}
				if (frames == 10)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_surprise_up");
				}
				if (frames >= 10 && frames <= 13)
				{
					int num15 = ((frames % 2 == 0) ? 1 : (-1));
					float num16 = 13 - frames;
					susie.transform.position = initSusiePos + new Vector3(num16 * (float)num15 / 24f, 0f);
				}
				if (frames == 15 && !quickStart)
				{
					gm.PlayMusic("music/mus_gallery");
					PlaySFX("sounds/snd_floweylaugh");
					flowey.enabled = true;
					flowey.Play("Laugh");
				}
				float num17 = (float)frames / 15f;
				num17 = ((!(num17 > 1f)) ? Mathf.Sin(num17 * MathF.PI * 0.5f) : 1f);
				vinePos = new Vector3(Mathf.Lerp(4.24f, -3.51f, num17), 1.84f);
				if (frames == 45)
				{
					if (!quickStart)
					{
						StartText(new string[1] { "* K-KRIS!" }, new string[1] { "snd_txtsus" }, new int[20], new string[1] { "su_shocked" }, 1);
						state = 6;
						frames = 0;
					}
					else
					{
						kris.InitiateBattle(hardmode ? 40 : 14);
						EndCutscene(enablePlayerMovement: false);
					}
				}
			}
		}
		if (state == 6 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				susie.EnableAnimator();
				susie.SetSelfAnimControl(setAnimControl: false);
				susie.ChangeDirection(Vector2.up);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.GetComponent<Animator>().SetFloat("speed", 2f);
				kris.ChangeDirection(Vector2.right);
				kris.SetSelfAnimControl(setAnimControl: false);
				kris.GetComponent<Animator>().SetBool("isMoving", value: true);
			}
			bool flag = false;
			if (kris.transform.position != new Vector3(-0.875f, -1.315f))
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(-0.875f, -1.315f), 1f / 12f);
			}
			else if (!flag)
			{
				flag = true;
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (frames <= 7)
			{
				susie.transform.position = Vector3.Lerp(initSusiePos, new Vector3(1.051f, -1.326f), (float)frames / 7f);
			}
			else if (frames == 8)
			{
				susie.transform.position = new Vector3(1.53f, -0.89f);
				susie.GetComponent<Animator>().Play("DR Attack Up", 0, 0f);
				gm.PlayGlobalSFX("sounds/snd_attack");
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SusieSlashDR"), new Vector3(2.75f, 1.97f), Quaternion.identity);
				flowey.enabled = false;
				flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_sassy");
			}
			float num18 = (float)(frames - 7) / 10f;
			num18 = ((num18 > 1f) ? 1f : ((!(num18 < 0f)) ? Mathf.Sin(num18 * MathF.PI * 0.5f) : 0f));
			vinePos = new Vector3(-3.51f, Mathf.Lerp(1.84f, 4f, num18));
			if (frames == 40)
			{
				susie.transform.position = new Vector3(1.051f, -1.326f);
				susie.DisableAnimator();
				susie.SetSprite("spr_su_kneel");
				gm.PlayGlobalSFX("sounds/snd_noise");
			}
			if (frames >= 60 && flag)
			{
				flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_side");
				kris.ChangeDirection(Vector2.up);
				StartText(new string[7]
				{
					(routeState == 2) ? "* Hahaha...\n^05* Are you bad at\n  LISTENING or something?" : "* Well,^05 looks like\n  someone's a bit upset!",
					"* Let them go.",
					"* Let them go?",
					"* And let my opportunity\n  to rejoice in another\n  world slip away?",
					"* You're an IDIOT if you\n  think I'll let you\n  get away!",
					"* So you wanna do this\n  the hard way, huh.",
					"* Well then..."
				}, new string[7] { "snd_txtflw", "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus" }, new int[20], new string[7] { "flowey_side", "su_angry_hero", "flowey_sassy", "flowey_evil", "flowey_grin", "su_depressed", "su_depressed_smile" }, 0);
				state = 7;
			}
		}
		if (state == 7)
		{
			if ((bool)txt)
			{
				string text3 = "";
				if (txt.GetCurrentStringNum() == 3)
				{
					text3 = "sassy";
				}
				else if (txt.GetCurrentStringNum() == 2)
				{
					susie.SetSprite("spr_su_save_kris");
				}
				else if (txt.GetCurrentStringNum() == 5)
				{
					text3 = "laugh_0";
				}
				else if (txt.GetCurrentStringNum() == 4)
				{
					text3 = "evil";
				}
				if (text3 != "" && !flowey.GetComponent<SpriteRenderer>().sprite.name.EndsWith(text3))
				{
					flowey.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_" + text3);
				}
			}
			else
			{
				state = 3;
				frames = 0;
			}
		}
		if (animatingVineHardmode)
		{
			vineFrames++;
			floweyVine.transform.position = vinePos + new Vector3(0f, Mathf.Sin((float)(vineFrames * 2) * (MathF.PI / 180f)) / 2f);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		cam.SetFollowPlayer(follow: false);
		hardmode = (int)gm.GetFlag(108) == 1;
		gm.SetCheckpoint(45);
		gm.SetFlag(57, 1);
		routeState = (WeirdChecker.HasKilled(gm) ? 1 : 0);
		if ((int)gm.GetFlag(13) == 3)
		{
			routeState = 2;
		}
		if ((int)gm.GetSessionFlag(0) == 1 || gm.IsTestMode())
		{
			quickStart = true;
		}
		else
		{
			gm.SetSessionFlag(0, 1);
		}
		floweyVine = GameObject.Find("FloweyVine").GetComponent<Animator>();
		flowey = GameObject.Find("BigFlower").GetComponent<Animator>();
		if (hardmode)
		{
			initSusiePos = susie.transform.position;
			floweyVine.enabled = false;
			floweyVine.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_flowey_vine_kris_5_hardmode");
			if (!quickStart)
			{
				if (routeState == 2)
				{
					StartText(new string[12]
					{
						"* Hahaha...", "* What the hell are\n  you laughing about?", "* Also,^05 weren't you like...^05\n  smaller?", "* I'll get to you.^05\n* Lemme talk with the\n  \"human\" first.", "* Speaking of,^05 you're not\n  really human,^05 are you?", "* No.^05 You're empty inside.^05\n* Just like me.^05\n* In fact...", "* You're Frisk,^05 right?", "* ... Ha! Just kidding!", "* I know exactly what's\n  going on here.", "* All of this?^05\n* You wanted me to think\n  that you were THEM.",
						"* If you REALLY were\n  trying to trick me...", "* You should've made sure\n  they weren't already\n  HERE!"
					}, new string[12]
					{
						"snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw",
						"snd_txtflw", "snd_txtflw"
					}, new int[20], new string[12]
					{
						"flowey_neutral", "su_annoyed", "su_smirk_sweat", "flowey_sassy", "flowey_side", "flowey_evil", "flowey_sad_0", "flowey_grin", "flowey_evil", "flowey_sassy",
						"flowey_side", "flowey_evil"
					}, 0);
				}
				else
				{
					StartText(new string[9] { "* Clever.^05\n* Verrrryyy clever.", "* You think you're really\n  smart,^05 don't you?", "* Uhh,^05 what the hell\n  are you talking about?", "* Also,^05 weren't you like...^05\n  smaller?", "* Hehehe...", "* So we just want to\n  jump into it?", "* Alright,^05 I'll play along.", "* If you're one to spare\n  the life of a single\n  person...", "* Then tell me what\n  you're gonna do NOW?" }, new string[9] { "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtflw" }, new int[20], new string[9] { "flowey_sassy", "flowey_sassy", "su_annoyed", "su_smirk_sweat", "flowey_neutral", "flowey_side", "flowey_sassy", "flowey_neutral", "flowey_evil" }, 0);
				}
			}
			state = 5;
		}
		else
		{
			PlaySFX("sounds/snd_spearrise");
			grabbingKrisY = kris.transform.position.y;
		}
		if (gm.IsTestMode())
		{
			EndCutscene(enablePlayerMovement: false);
			kris.InitiateBattle(hardmode ? 40 : 14);
		}
	}
}
