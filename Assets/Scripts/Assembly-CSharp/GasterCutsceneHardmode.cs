using System.Collections.Generic;
using UnityEngine;

public class GasterCutsceneHardmode : CutsceneBase
{
	private Animator gaster;

	private Animator dess;

	private int stopFrames;

	private int completionState;

	private bool recordingDeviceDetected;

	private int turnIndex = 1000;

	private Transform blasterCenter;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames >= 30)
			{
				gm.StopMusic(120f);
				if (frames == 30)
				{
					kris.SetSelfAnimControl(setAnimControl: false);
					kris.GetComponent<Animator>().SetBool("isMoving", value: true);
					kris.GetComponent<Animator>().SetFloat("speed", 0.5f);
				}
				if (kris.transform.position != new Vector3(0.42f, -1.7f))
				{
					kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(0.42f, -1.7f), 0.0625f);
				}
				else
				{
					cam.SetFollowPlayer(follow: false);
					kris.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
				if (!cam.FollowingPlayer())
				{
					if (cam.transform.position != new Vector3(0.41f, 0f, -10f))
					{
						cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(0.41f, 0f, -10f), 0.0625f);
					}
					else if (stopFrames == 0)
					{
						stopFrames = frames;
					}
					else if (frames - stopFrames >= 30)
					{
						frames = 0;
						state = 1;
						string text = "* IT APPEARS THAT YOU HAVE\n  FINALLY ARRIVED.";
						string text2 = "#v_gaster_arrival_1a";
						StartText(new string[4] { "/WDBRILLIANT^20\nIT HAS BEEN \nPLACATED", "* HMM...", text, "* ALLOW ME TO PROPERLY INTRODUCE\n  MYSELF." }, new string[4] { "snd_txtgas", "#v_gaster_arrival_0", text2, "#v_gaster_arrival_2" }, new int[4] { 1, 1, 1, 1 }, new string[0], 1);
					}
				}
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && gaster.enabled)
				{
					gaster.enabled = false;
					gaster.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_gaster_lookback");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gaster.enabled = true;
				}
				if (frames == 30)
				{
					gaster.SetFloat("dirX", 1f);
					gaster.SetFloat("dirY", 0f);
				}
				if (frames == 60)
				{
					gaster.SetFloat("dirX", 0f);
					gaster.SetFloat("dirY", -1f);
				}
				if (frames == 90)
				{
					state = 2;
					frames = 0;
					StartText(new string[1] { "* MY NAME^20 IS\n  <color=#FFFF00FF>DOCTOR W.D. GASTER</color>." }, new string[3] { "#v_gaster_reveal_0", "snd_txtgas", "snd_txtgas" }, new int[3] { 1, 1, 1 }, new string[1] { "gaster_neutral" }, 1);
				}
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.PlayMusic("music/mus_gaster");
			}
			if (frames >= 45)
			{
				if (frames == 45)
				{
					gaster.SetFloat("dirX", 1f);
					gaster.SetFloat("dirY", 0f);
					gaster.SetBool("isMoving", value: true);
				}
				if (gaster.transform.position != new Vector3(1.74f, 1.831f))
				{
					gaster.transform.position = Vector3.MoveTowards(gaster.transform.position, new Vector3(1.74f, 1.831f), 1f / 24f);
				}
				else
				{
					gaster.SetBool("isMoving", value: false);
				}
			}
			if (frames == 120)
			{
				StartText(new string[4] { "* I AM QUITE SURPRISED\n  WITH THE OUTCOME\n  OF THIS EXPERIMENT.", "* ESPECIALLY WITH HOW\n  DIFFICULT IT WAS\n  TO PROCURE.", "* MY POWER IS^15 NOT AS\n  USEFUL AS I WOULD\n  LIKE...", "* NONETHELESS,^15 THIS HAS\n  BEEN^15 A SUCCESSFUL\n  EXPERIMENT." }, new string[4] { "#v_gaster_first_rant_0", "#v_gaster_first_rant_1", "#v_gaster_first_rant_2", "#v_gaster_first_rant_3" }, new int[4] { 1, 1, 1, 1 }, new string[4] { "gaster_neutral", "gaster_closed", "gaster_sad", "gaster_neutral" }, 1);
			}
			if (frames == 121)
			{
				gaster.SetFloat("dirX", 0f);
				gaster.SetFloat("dirY", 1f);
			}
			if (frames == 180)
			{
				state = 3;
				frames = 0;
				List<string> list = new List<string> { "* BUT...", "* THIS IS NOT THE\n  PRIMARY EXPERIMENT THAT\n  I AM CONDUCTING.", "* YOU MUST LEAD THE\n  DELTA WARRIORS TO\n  THIS VERY LOCATION.", "* YOU MUST GET HERE\n  WITHOUT MY DIRECT\n  INTERFERENCE.", "* THEY MUST FACE THE\n  HARDSHIPS OF TRAVEL.", "* THE GREY DOORS ARE\n  YOUR ONLY PATH\n  TO PROGRESSION." };
				List<string> list2 = new List<string> { "gaster_closed", "gaster_closed", "gaster_neutral", "gaster_neutral", "gaster_closed", "gaster_closed" };
				List<string> list3 = new List<string> { "#v_gaster_part2_0", "#v_gaster_part2_1", "#v_gaster_part2_2", "#v_gaster_part2_3", "#v_gaster_part2_4", "#v_gaster_part2_5" };
				if (completionState == 0 && recordingDeviceDetected)
				{
					list.AddRange(new string[3] { "* ...", "* I CANNOT SHAKE THIS\n  FEELING THAT YOU\n  KNOW THIS ALREADY.", "* I DO HOPE THAT YOU\n  ARE NOT ATTEMPTING TO\n  GET A REACTION FROM ME." });
					list2.AddRange(new string[3] { "gaster_closed", "gaster_closed", "gaster_empty" });
					list3.AddRange(new string[3] { "", "#v_gaster_part2extA_0", "#v_gaster_part2extA_1" });
					turnIndex = 9;
				}
				else if (completionState == 1)
				{
					list.AddRange(new string[4] { "* BUT I IMAGINE\n  YOU ALREADY KNEW\n  THIS.", "* PERHAPS YOU WERE\n  PERPLEXED BY\n  THE QUEEN'S HOSTILITY.", "* ...", "* ONLY YOU KNOW THE\n  ANSWER TO THIS." });
					list2.AddRange(new string[4] { "gaster_neutral", "gaster_neutral", "gaster_closed", "gaster_closed" });
					list3.AddRange(new string[4] { "#v_gaster_part2extB_0", "#v_gaster_part2extB_1", "", "#v_gaster_part2extB_2" });
					if (recordingDeviceDetected)
					{
						list.AddRange(new string[4] { "* I DO HOPE THAT\n  THIS IS NOT JUST AN\n  ANSWER TO YOU.", "* AN ANSWER THAT YOU\n  COULD MAKE AVAILABLE\n  TO THE WORLD.", "* THAT WOULD BREACH\n  OUR PRIVACY,^15\n  OF COURSE.", "* YOU WOULD NOT BE\n  DOING SUCH A THING,^15\n  WOULD YOU?" });
						list2.AddRange(new string[4] { "gaster_closed", "gaster_closed", "gaster_closed", "gaster_empty" });
						list3.AddRange(new string[4] { "#v_gaster_part2extC_0", "#v_gaster_part2extC_1", "#v_gaster_part2extC_2", "#v_gaster_part2extC_3" });
						turnIndex = 14;
					}
				}
				else if (completionState == 2)
				{
					list.AddRange(new string[2] { "* BUT I IMAGINE\n  YOU HAVE ALREADY HEARD\n  THIS BEFORE.", "* ... NOT THAT I WOULD\n  KNOW,^20 OF COURSE." });
					list2.AddRange(new string[2] { "gaster_neutral", "gaster_closed" });
					list3.AddRange(new string[2] { "#v_gaster_part2extD_0", "#v_gaster_part2extD_1" });
					if (recordingDeviceDetected)
					{
						list.AddRange(new string[3] { "* IT WOULD BE TERRIBLE\n  FOR ME TO LIE TO\n  YOU KNOWINGLY.", "* JUST AS IT WOULD\n  TO BREACH OUR PRIVACY.", "* YOU WOULD NOT BE\n  DOING SUCH A THING,^15\n  WOULD YOU?" });
						list2.AddRange(new string[3] { "gaster_closed", "gaster_closed", "gaster_empty" });
						list3.AddRange(new string[3] { "#v_gaster_part2extE_0", "#v_gaster_part2extE_1", "#v_gaster_part2extC_3" });
						turnIndex = 11;
					}
				}
				StartText(list.ToArray(), list3.ToArray(), new int[1] { 1 }, list2.ToArray(), 1);
			}
		}
		if (state == 3)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == turnIndex && gaster.enabled)
				{
					gaster.enabled = false;
					gaster.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_gaster_down_empty_0");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gaster.enabled = true;
					gaster.SetFloat("dirX", -1f);
					gaster.SetFloat("dirY", 0f);
					gaster.SetBool("isMoving", value: true);
				}
				if (gaster.transform.position != new Vector3(-1f, 1.831f))
				{
					gaster.transform.position = Vector3.MoveTowards(gaster.transform.position, new Vector3(-1f, 1.831f), 1f / 24f);
				}
				else
				{
					gaster.SetBool("isMoving", value: false);
				}
				if (frames == 120)
				{
					state = 5;
					frames = 0;
					DoPathDialogue();
				}
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.DisableAnimator();
				kris.SetSprite("spr_fr_up_1");
			}
			if (frames < 30)
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(0.42f, 10f), 1f / 48f);
			}
			else if (frames == 30)
			{
				kris.EnableAnimator();
			}
			if (frames == 90)
			{
				gaster.SetFloat("dirX", 0f);
				gaster.SetFloat("dirY", 1f);
				state = 5;
				frames = 0;
				StartText(new string[4] { "* AH,^15 I DO BELIEVE\n  THEY ARE HEARING ME\n  AS WELL.", "* IT HAS BEEN^10 A\n  LONG TIME,^10 HAS IT\n  NOT?", "* ...", "/WDTHERE'S A SPECIAL \nPLACE IN HELL FOR \nYOU, CHARA" }, new string[4] { "#v_gaster_chara_0", "#v_gaster_chara_1", "", "snd_txtgas" }, new int[4] { 1, 1, 1, 1 }, new string[4] { "gaster_closed", "gaster_neutral", "gaster_closed", "gaster_empty" }, 1);
			}
		}
		if (state == 5 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gaster.SetFloat("dirX", 1f);
				gaster.SetFloat("dirY", 0f);
				gaster.SetBool("isMoving", value: true);
			}
			if (gaster.transform.position != new Vector3(0.376f, 1.831f))
			{
				gaster.transform.position = Vector3.MoveTowards(gaster.transform.position, new Vector3(0.376f, 1.831f), 1f / 24f);
			}
			else
			{
				gaster.SetBool("isMoving", value: false);
			}
			if (frames == 90)
			{
				state = 6;
				frames = 0;
				gaster.SetFloat("dirX", 0f);
				gaster.SetFloat("dirY", 1f);
				StartText(new string[5] { "* BUT REGARDLESS OF\n  YOUR PATH...", "* THERE IS BUT ONE\n  MORE THING THAT\n  WE NEED TO DO.", "* WE STILL NEED TO\n  CONCLUDE THIS\n  EXPERIMENT.", "* THEN,^15 ONCE WE ARE\n  DONE,^15 WE MAY GO OUR\n  SEPARATE WAYS.", "* SHALL WE GET\n  STARTED?" }, new string[5] { "#v_gaster_finish_path_rant_0", "#v_gaster_finish_path_rant_1", "#v_gaster_finish_path_rant_2", "#v_gaster_finish_path_rant_3", "#v_gaster_finish_path_rant_4" }, new int[5] { 1, 1, 1, 1, 1 }, new string[5] { "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_closed", "gaster_empty" }, 1);
			}
		}
		if (state == 6)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 5 && gaster.enabled)
				{
					gaster.enabled = false;
					gaster.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_gaster_down_empty_0");
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic();
					dess.SetFloat("dirX", -1f);
					dess.SetBool("isMoving", value: true);
				}
				dess.transform.position = new Vector3(Mathf.Lerp(8f, 5.28f, (float)frames / 30f), 0f);
				if (frames == 30)
				{
					dess.SetBool("isMoving", value: false);
				}
				if (frames == 15)
				{
					gaster.enabled = true;
					gaster.SetFloat("dirX", 1f);
					gaster.SetFloat("dirY", 0f);
					kris.ChangeDirection(Vector2.right);
				}
				if (frames == 45)
				{
					state = 7;
					frames = 0;
					StartText(new string[7] { "* Oh my GOD,^05 can\n  you shut up?\n* It's like 3AM.", "* !!!", "* WHAT THE HELL IS\n  THIS?!??!?!", "* WHY IS THERE LIKE???^10\n* A BABY KRIS IN\n  HERE?????", "* ARE YOU JUST THAT\n  DEPRAVED?!?", "* ...", "* LET US END THIS\n  QUICKLY." }, new string[7] { "snd_txtdes", "snd_txtdes", "snd_txtdes", "snd_txtdes", "snd_txtdes", "", "#v_gaster_end_0" }, new int[7] { 0, 0, 0, 0, 0, 1, 1 }, new string[7] { "dess_annoyed", "dess_surprise", "dess_murderous", "dess_murderous_thought", "dess_murderous_side", "gaster_closed", "gaster_closed" }, 1);
				}
			}
		}
		if (state != 7)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2)
			{
				dess.enabled = false;
				dess.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("player/dess/spr_de_surprise");
			}
			if (txt.GetCurrentStringNum() == 7)
			{
				gaster.enabled = false;
				gaster.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_gaster_down_empty_0");
				kris.ChangeDirection(Vector2.up);
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			gaster.enabled = true;
			gaster.Play("gaster_blast");
		}
		if (frames == 9)
		{
			PlaySFX("sounds/snd_grab");
		}
		if (frames == 15)
		{
			cam.SetFollowPlayer(follow: true);
			gm.EnablePlayerMovement();
			kris.SetSelfAnimControl(setAnimControl: true);
			Util.FindObjectOfType<ActionBulletHandler>().transform.position = Vector3.zero;
			for (int i = 0; i < 2; i++)
			{
				float num = ((i % 2 == 0) ? 1 : (-1));
				GasterBlasterOW component = Object.Instantiate(Resources.Load<GameObject>("overworld/bullets/GasterBlasterOW"), new Vector3(-12f * num, 0f), Quaternion.Euler(0f, 0f, -90f * num)).GetComponent<GasterBlasterOW>();
				component.transform.parent = blasterCenter;
				component.SetBaseDamage(7);
				component.Activate(5, 5, 90f * num, new Vector2(-4.48f * num, 0f), 999, 15);
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 7 && frames >= 15)
		{
			blasterCenter.transform.position = kris.transform.position;
		}
	}

	private void DoPathDialogue()
	{
		int num = (int)gm.GetFlag(125);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		if (completionState != 0 || recordingDeviceDetected)
		{
			list.Add("* NONETHELESS...");
			list2.Add("gaster_closed");
			list3.Add("#v_gaster_path_extstart_0");
		}
		list.AddRange(new string[3] { "* I WOULD LIKE TO\n  COMMENT ON AN ASPECT\n  OF THIS EXPERIMENT.", "* YOUR PATH.", "* THE CHOICES THAT YOU\n  MADE TO ARRIVE HERE." });
		list2.AddRange(new string[3] { "gaster_closed", "gaster_neutral", "gaster_neutral" });
		list3.AddRange(new string[3] { "#v_gaster_path_start_0", "#v_gaster_path_start_1", "#v_gaster_path_start_2" });
		bool flag = !WeirdChecker.HasKilled(gm) || (gm.GetEXP() == 150 && (int)gm.GetFlag(58) == 1);
		bool flag2 = gm.GetEXP() >= 150 && (int)gm.GetFlag(58) == 1;
		bool flag3 = (int)gm.GetFlag(13) == 3;
		if (flag2)
		{
			num++;
		}
		if (gm.GetFlagInt(127) == 1)
		{
			num++;
		}
		if (gm.GetFlagInt(6) == 1)
		{
			num--;
		}
		if (flag)
		{
			if (flag2)
			{
				list.Add("* FOR THE MOST PART...");
				list2.Add("gaster_closed");
				list3.Add("#v_gaster_path_pacifist_extstart_0");
			}
			list.AddRange(new string[3] { "* YOU HAVE DONE\n  EXCELLENTLY IN NOT\n  HURTING ANYONE.", "* THE MONSTERS THAT YOU\n  HAVE ENCOUNTERED...", "* YOU GAVE THEM YOUR\n  MERCY." });
			list2.AddRange(new string[3] { "gaster_neutral", "gaster_closed", "gaster_closed" });
			list3.AddRange(new string[3] { "#v_gaster_path_pacifist_0", "#v_gaster_path_pacifist_1", "#v_gaster_path_pacifist_2" });
		}
		else
		{
			string text = "#v_gaster_killcount_0`" + GetGasterLinesForNumbers(num) + "`#v_gaster_killcount_1";
			list.AddRange(new string[3]
			{
				"* NOW,^10 UNLIKE FOR MY\n  PRIMARY EXPERIMENT.",
				"* I HAVE TRACKED THE\n  NUMBER OF MONSTERS\n  THAT YOU HAVE SLAIN.",
				$"* YOU HAVE ELIMINATED\n  <color=#FF0000FF>{num} MONSTERS</color> ON YOUR\n  JOURNEY."
			});
			list2.AddRange(new string[3] { "gaster_neutral", "gaster_closed", "gaster_neutral" });
			list3.AddRange(new string[3] { "#v_gaster_path_neutraloblit_0", "#v_gaster_path_neutraloblit_1", text });
		}
		if (flag3)
		{
			state = 4;
			list.AddRange(new string[7] { "* IT IS QUITE\n  INTERESTING.", "* THE LENGTHS THAT\n  YOU WILL GO TO\n  GAIN STRENGTH.", "* MURDER.^10\n* MANIPULATION.^10\n* POSSESSION.", "* RIGHT NOW,^15 I CAN\n  FEEL ANOTHER PRESENCE\n  WITHIN YOU.", "* ONE THAT IS VENGEFUL\n  AND WILLING TO END\n  EVERYTHING.", "* AFTER ALL...", "* YOU HAVE MURDERED\n  THEIR BEST FRIEND." });
			list3.AddRange(new string[7] { "#v_gaster_path_fullgeno_0", "#v_gaster_path_fullgeno_1", "#v_gaster_path_fullgeno_2", "#v_gaster_path_fullgeno_3", "#v_gaster_path_fullgeno_4", "#v_gaster_path_fullgeno_5", "#v_gaster_path_fullgeno_6" });
			list2.AddRange(new string[7] { "gaster_closed", "gaster_closed", "gaster_empty", "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_empty" });
		}
		else
		{
			if (!flag)
			{
				if ((int)gm.GetFlag(127) == 1)
				{
					list.AddRange(new string[7] { "* THINK ABOUT THIS.", "* WHAT DID YOU FEEL\n  WHEN YOU SAW\n  <color=#FF0000FF>\"24 left\"</color>?", "* WAS IT PERHAPS FEAR?", "* WAS IT GRATIFICATION?", "* DID YOU PERHAPS\n  FORGET TO SLAY THE\n  REST OF THEM?", "* OR WAS THIS\n  INTENTIONAL?", "* AND YOU REMORSEFULLY\n  REGRETTED YOUR\n  ACTIONS?" });
					list2.AddRange(new string[7] { "gaster_closed", "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_closed", "gaster_neutral", "gaster_neutral" });
					list3.AddRange(new string[7] { "#v_gaster_path_killnapsta_0", "#v_gaster_path_killnapsta_1", "#v_gaster_path_killnapsta_2", "#v_gaster_path_killnapsta_3", "#v_gaster_path_killnapsta_4", "#v_gaster_path_killnapsta_5", "#v_gaster_path_killnapsta_6" });
				}
				else
				{
					list.AddRange(new string[5] { "* I MUST ASK YOU.", "* HAVE YOU CONSIDERED\n  THE LIVES OF THE\n  MONSTERS?", "* THEY HAVE FAMILIES\n  THAT THEY WILL NEVER\n  SEE AGAIN.", "* ... OR IS THIS NOT\n  SOMETHING THAT YOU\n  CARE FOR?", "* IS THIS JUST AN\n  EXPERIMENT TO YOU\n  AS WELL?" });
					list2.AddRange(new string[5] { "gaster_closed", "gaster_empty", "gaster_closed", "gaster_closed", "gaster_closed" });
					list3.AddRange(new string[5] { "#v_gaster_path_genoconsider_0", "#v_gaster_path_genoconsider_1", "#v_gaster_path_genoconsider_2", "#v_gaster_path_genoconsider_3", "#v_gaster_path_genoconsider_4" });
				}
				list.AddRange(new string[2] { "* ...", "* THIS IS SOMETHING\n  THAT YOU CAN ONLY\n  ANSWER TO YOURSELF." });
				list2.AddRange(new string[2] { "gaster_closed", "gaster_closed" });
				list3.AddRange(new string[2] { "", "#v_gaster_path_finishgeno_0" });
			}
			if (flag2)
			{
				list.AddRange(new string[9]
				{
					"* ... HOWEVER...",
					flag ? "* YOU MADE ONE EXCEPTION\n  AT THE LAST MINUTE." : "* THERE WAS ONE ENEMY\n  THAT I DID NOT COUNT\n  IN THE TALLY.",
					"* YOU DECIDED TO TAKE\n  THE LIFE OF THE\n  FLOWER.",
					"* THE ONE THAT ARRIVED\n  BEFORE THE OTHERS.",
					"* THE ONE WHO...",
					"* ... WELL...^15\n  I WILL SPARE YOU\n  THE DETAILS.",
					"* BUT I WILL SAY\n  THIS.",
					"* THERE IS A CAUSE\n  AND EFFECT FOR\n  EVERYTHING.",
					"* ... WHY WOULD A FLOWER\n  ACT IN SUCH A\n  WAY...?"
				});
				list2.AddRange(new string[9] { "gaster_neutral", "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_closed", "gaster_neutral", "gaster_neutral", "gaster_neutral", "gaster_closed" });
				list3.AddRange(new string[9]
				{
					"#v_gaster_path_killflowey_0",
					"#v_gaster_path_killflowey_1" + (flag ? "a" : "b"),
					"#v_gaster_path_killflowey_2",
					"#v_gaster_path_killflowey_3",
					"#v_gaster_path_killflowey_4",
					"#v_gaster_path_killflowey_5",
					"#v_gaster_path_killflowey_6",
					"#v_gaster_path_killflowey_7",
					"#v_gaster_path_killflowey_8"
				});
			}
			else
			{
				list.Add("* BUT STRANGELY...\n* DESPITE THE HYPOCRISY\n  IN DOING SO...");
				list2.Add("gaster_closed");
				list3.Add("#v_gaster_path_spareflowey_0");
				list.AddRange(new string[7] { "* YOU EVEN DECIDED TO\n  ALLOW THE FLOWER TO\n  GET AWAY.", "* EVEN THOUGH HE\n  CHALLENGED YOU TO\n  KILL HIM.", "* IT SEEMS RATHER\n  COUNTER-PRODUCTIVE\n  TO DO SO.", "* TO SPARE THE LIFE\n  OF A RUTHLESS\n  MURDERER.", "* AT LEAST,^15 THIS\n  BEHAVIOR GOES AGAINST\n  MY FINDINGS.", "* I DO WONDER...", "* ... IF YOU UNDERSTAND\n  THE CONTEXT OF THE\n  FLOWER." });
				list2.AddRange(new string[7] { "gaster_neutral", "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_closed", "gaster_neutral", "gaster_neutral" });
				list3.AddRange(new string[7] { "#v_gaster_path_spareflowey_1", "#v_gaster_path_spareflowey_2", "#v_gaster_path_spareflowey_3", "#v_gaster_path_spareflowey_4", "#v_gaster_path_spareflowey_5", "#v_gaster_path_spareflowey_6", "#v_gaster_path_spareflowey_7" });
			}
			if ((int)gm.GetFlag(63) == 1)
			{
				list.Add("* YOU ALSO FOUND IT.");
				list2.Add("gaster_neutral");
				list3.Add("#v_gaster_path_egg_0");
				if ((int)gm.GetFlag(126) == 1)
				{
					list.AddRange(new string[5] { "* AND YOU TRADED IT\n  FOR A CANDY.", "* OH,^15 HOW YOUR CURIOSITY\n  REWARDS YOU.", "* JUST AS MINE REWARDS\n  ME.", "* ...", "* AT LEAST,^15 MOST OF\n  THE TIME." });
					list2.AddRange(new string[5] { "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_sad", "gaster_closed" });
					list3.AddRange(new string[5] { "#v_gaster_path_egg_traded_0", "#v_gaster_path_egg_traded_1", "#v_gaster_path_egg_traded_2", "", "#v_gaster_path_egg_traded_3" });
				}
				else
				{
					list.AddRange(new string[5] { "* THOUGH...", "* I DO WONDER...", "* WOULD IT PERHAPS OFFER\n  A TRUE BENEFIT?", "* PERHAPS AN OFFER...", "* ... OF AN EGG." });
					list3.AddRange(new string[5] { "#v_gaster_path_egg_1", "#v_gaster_path_egg_2", "#v_gaster_path_egg_3", "#v_gaster_path_egg_4", "#v_gaster_path_egg_5" });
				}
			}
			int num2 = (int)gm.GetFlag(124);
			if (num2 != 0)
			{
				list.AddRange(new string[2] { "* AND IF I MAY ADD...", "* YOU DID NOT NEED\n  TO FIGHT THE ABANDONED\n  VESSEL." });
				list2.AddRange(new string[2] { "gaster_neutral", "gaster_neutral" });
				list3.AddRange(new string[2] { "#v_gaster_path_knight_0", "#v_gaster_path_knight_1" });
				if (num2 == 2)
				{
					list.AddRange(new string[4] { "* BUT I SUPPOSE YOU\n  DID SOMETHING GOOD\n  IN THE END.", "* I AM UNSURE OF IF\n  YOU CAN UNDERSTAND\n  ITS LANGUAGE...", "* BUT THE KINDNESS THAT\n  YOU GAVE IT...", "* CHANGED ITS MIND." });
					list2.AddRange(new string[4] { "gaster_closed", "gaster_neutral", "gaster_closed", "gaster_closed" });
					list3.AddRange(new string[4] { "#v_gaster_path_knight_spare_0", "#v_gaster_path_knight_spare_1", "#v_gaster_path_knight_spare_2", "#v_gaster_path_knight_spare_3" });
				}
				else
				{
					list.AddRange(new string[5] { "* IT IS ENOUGH THAT\n  ITS EXISTENCE WAS\n  DOOMED.", "* IT IS WORSE THAT\n  YOU DECIDED TO END\n  ITS POOR LIFE.", "* HOWEVER...", "* THAT MAY BE THE\n  BLESSING IT WAS\n  LOOKING FOR.", "* AS ITS OWN WILL\n  COULD NOT UNDO\n  ITS OWN LIFE." });
					list2.AddRange(new string[5] { "gaster_closed", "gaster_neutral", "gaster_closed", "gaster_closed", "gaster_closed" });
					list3.AddRange(new string[5] { "#v_gaster_path_knight_kill_0", "#v_gaster_path_knight_kill_1", "#v_gaster_path_knight_kill_2", "#v_gaster_path_knight_kill_3", "#v_gaster_path_knight_kill_4" });
				}
			}
		}
		StartText(list.ToArray(), list3.ToArray(), new int[1] { 1 }, list2.ToArray(), 1);
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		kris.ChangeDirection(Vector2.up);
		gaster = GameObject.Find("Gaster").GetComponent<Animator>();
		dess = GameObject.Find("Dess").GetComponent<Animator>();
		blasterCenter = new GameObject("BlasterCenter").transform;
		Object.Destroy(GameObject.Find("LoadingZone"));
		if (PersistentSAVE.GetInt("hardmode-completion", 0) == 1)
		{
			completionState = 2;
		}
		else if (PersistentSAVE.GetInt("completion", 0) >= 1)
		{
			completionState = 1;
		}
		PersistentSAVE.SetInt("hardmode-completion", 1);
		recordingDeviceDetected = GameManager.UsingRecordingSoftware();
		gm.SetEnding(0);
	}

	private string GetGasterLinesForNumbers(int num)
	{
		string[] array = new string[20]
		{
			"#v_gaster_killcount_zero", "#v_gaster_killcount_one", "#v_gaster_killcount_two", "#v_gaster_killcount_three", "#v_gaster_killcount_four", "#v_gaster_killcount_five", "#v_gaster_killcount_six", "#v_gaster_killcount_seven", "#v_gaster_killcount_eight", "#v_gaster_killcount_nine",
			"#v_gaster_killcount_ten", "#v_gaster_killcount_eleven", "#v_gaster_killcount_twelve", "#v_gaster_killcount_thirteen", "#v_gaster_killcount_fourteen", "#v_gaster_killcount_fifteen", "#v_gaster_killcount_sixteen", "#v_gaster_killcount_seventeen", "#v_gaster_killcount_eighteen", "#v_gaster_killcount_nineteen"
		};
		string[] array2 = new string[10] { "", "", "#v_gaster_killcount_twenty", "#v_gaster_killcount_thirty", "#v_gaster_killcount_fourty", "#v_gaster_killcount_fifty", "#v_gaster_killcount_sixty", "#v_gaster_killcount_seventy", "#v_gaster_killcount_eighty", "#v_gaster_killcount_ninety" };
		string text = "";
		try
		{
			if (num < 20)
			{
				text = array[num];
			}
			else if (num < 100)
			{
				text = array2[(int)Mathf.Floor(num / 10)] + ((num % 10 != 0) ? ("`" + array[num % 10]) : "");
			}
			if (text != "")
			{
				return text;
			}
			return "#v_gaster_killcount_many";
		}
		catch
		{
			return "#v_gaster_killcount_many";
		}
	}
}
