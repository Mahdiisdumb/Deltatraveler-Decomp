using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class GasterSection3Cutscene : CutsceneBase
{
	private int variance = 3;

	private int pupilFrames;

	private bool creepygastershit;

	private Sprite[] gasterEyes;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			Tilemap[] array = Object.FindObjectsOfType<Tilemap>();
			foreach (Tilemap obj in array)
			{
				float t = 1f;
				if ((bool)txt)
				{
					t = (float)(txt.GetCurrentStringNum() - 5) / 35f;
				}
				obj.color = Color.Lerp(b: Color.Lerp(new Color(0.5f, 0.5f, 0.5f), Color.black, t), a: obj.color, t: 0.05f);
			}
			if (!txt)
			{
				frames++;
				if (frames == 60)
				{
					array = Object.FindObjectsOfType<Tilemap>();
					for (int i = 0; i < array.Length; i++)
					{
						array[i].color = Color.black;
					}
					string text = ((kris.transform.position.x - GameObject.Find("Gaster").transform.position.x < 0f) ? "_left" : "");
					SetSprite(GameObject.Find("Gaster").GetComponent<SpriteRenderer>(), "overworld/npcs/spr_gaster_lookback" + text);
					GameObject.Find("windsound").GetComponent<AudioSource>().Stop();
					gm.PlayMusic("music/mus_him", (variance == 0) ? 1f : 0.6f, 0f);
					if (variance == 0)
					{
						StartText(new string[8] { "* ...", "* THAT IS...", "* UNTIL I FOUND IT.", "* THE DARK FOUNTAIN.", "* FROM UNDER THE EARTH.", "* THERE IS A DARK MAGIC THAT CAN\n  ENVELOP ITS SURROUNDINGS AND\n  BECOME A NEW WORLD.", "* THAT YOU KNOW ALL TOO WELL,^10\n  KRIS.", "* ... SO THEN." }, new string[8] { "", "#v_gaster_s3_end_1", "#v_gaster_s3_end_2", "#v_gaster_s3_end_3", "#v_gaster_s3_end_4", "#v_gaster_s3_end_5", "#v_gaster_s3_end_6", "#v_gaster_s3_end_7" }, new int[1] { 1 }, new string[0]);
					}
					else
					{
						List<string> list = new List<string> { "* IT IS TORTURE,^10 KRIS.", "* THE REGRET I FEEL DAILY\n  HAUNTS ME.", "* HOW EVERYTHING COULD HAVE\n  TURNED HAD I NOT JUMPED.", "* MAYBE MY NAME WOULD LIVE ON IF\n  I HADN'T \"GONE MISSING.\"", "* MAYBE YOU WOULD NOT BE\n  CONDUCTING YOUR OWN EXPERIMENT.", "* WHEREIN YOU OBLITERATE\n  EVERYTHING IN THESE WORLDS." };
						List<string> list2 = new List<string> { "#v_gaster_s3_end_o_0", "#v_gaster_s3_end_o_1", "#v_gaster_s3_end_o_2", "#v_gaster_s3_end_o_3", "#v_gaster_s3_end_o_4", "#v_gaster_s3_end_o_5" };
						if (variance == 1)
						{
							list.Add("* ...");
							list2.Add("");
						}
						else
						{
							list.Add("* YOU BARELY EVEN CONSIDERED\n  A MORE TAME ROUTE.");
							list2.Add("#v_gaster_s3_end_o_6");
							if (variance == 3)
							{
								list.Add("* AND ALL THE WHILE FILMING\n  YOUR FINDINGS.");
								list2.Add("#v_gaster_s3_end_o_6a");
							}
							list.Add("* HOW SADISTIC...");
							list2.Add("#v_gaster_s3_end_o_7");
						}
						StartText(list.ToArray(), list2.ToArray(), new int[1] { 1 }, new string[0]);
					}
					txt.MakeUnskippable();
					frames = 0;
					state = 1;
				}
			}
		}
		if (state == 1)
		{
			float num = 1f;
			if ((bool)txt)
			{
				num = (float)(txt.GetCurrentStringNum() - 1) / 6f;
			}
			num *= num;
			gm.GetMusicPlayer().SetVolume(Mathf.Lerp(gm.GetMusicPlayer().GetVolume(), num * 0.5f, 0.01f));
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic();
					GameObject.Find("darkness").GetComponent<SpriteRenderer>().enabled = true;
				}
				if (frames == 45)
				{
					gm.SetPersistentFlag(1, 1);
					GameObject.Find("darkness").GetComponent<SpriteRenderer>().enabled = false;
					GameObject.Find("gasterlookin").GetComponent<SpriteRenderer>().enabled = true;
				}
				if (frames >= 90 && frames <= 108)
				{
					GameObject.Find("eye").GetComponent<SpriteRenderer>().sprite = gasterEyes[frames - 90];
					GameObject.Find("eye").GetComponent<SpriteMask>().sprite = gasterEyes[frames - 90];
					if (frames == 90)
					{
						GameObject.Find("eye").GetComponent<SpriteRenderer>().enabled = true;
						GameObject.Find("pupils").GetComponent<SpriteRenderer>().enabled = true;
						GameObject.Find("pinpointL").GetComponent<SpriteRenderer>().enabled = true;
						GameObject.Find("pinpointR").GetComponent<SpriteRenderer>().enabled = true;
					}
					num = (float)(frames - 94) / 14f;
					num = num * num * num * (num * (6f * num - 15f) + 10f);
					GameObject.Find("pupils").transform.localPosition = new Vector3(0f, Mathf.Lerp(0.213f, 0f, num));
					if (frames == 108)
					{
						creepygastershit = true;
					}
				}
				if (frames == 150)
				{
					string text2 = "";
					int num2 = 15 - gm.GetPlayerName().Length / 2;
					for (int j = 0; j < num2; j++)
					{
						text2 += " ";
					}
					List<string> list3 = new List<string>();
					if (variance == 0)
					{
						list3.Add("\b       SHALL YOU HELP ME \n\b       CREATE A NEW WORLD,^20 \n\b" + text2 + "^N?");
					}
					else if (variance == 1)
					{
						list3.Add("\b      YOU AND I AREN'T SO \n\b      DIFFERENT,^20 ^N.");
					}
					else
					{
						list3.Add("\b  PERHAPS WE ARE MUCH THE SAME,^20 \n\b" + text2 + "^N.");
					}
					if (variance == 3)
					{
						list3.Add("\b  MAY YOUR FOLLOWERS GUIDE YOU \n\b          TO OBLIVION.");
					}
					StartText(list3.ToArray(), new string[1] { "" }, new int[1] { 1 }, new string[0]);
					Object.Destroy(GameObject.Find("menuBorder"));
					Object.Destroy(GameObject.Find("menuBox"));
					txt.MakeUnskippable();
					frames = 0;
					state = 2;
				}
			}
		}
		if (creepygastershit)
		{
			if (pupilFrames < 40)
			{
				Transform[] componentsInChildren = GameObject.Find("pupils").GetComponentsInChildren<Transform>();
				foreach (Transform transform in componentsInChildren)
				{
					if (transform.gameObject.name != "pupils")
					{
						transform.localScale = Vector3.Lerp(transform.localScale, Vector3.zero, 0.1f);
					}
				}
			}
			pupilFrames++;
			if (pupilFrames % 40 == 0)
			{
				GameObject.Find("pupils").transform.localPosition = new Vector3((float)Random.Range(-1, 2) / 48f, (float)Random.Range(-1, 2) / 48f);
			}
		}
		if (state == 2 && !txt)
		{
			if ((int)gm.GetSessionFlag(2) == 1)
			{
				gm.SetSessionFlag(2, 0);
				gm.SetPartyMembers(true, true);
			}
			gm.LoadArea(81, false, kris.transform.position, kris.GetDirection());
			state = 3;
			creepygastershit = false;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetFlag(183, 1);
		if ((int)gm.GetFlag(12) == 0)
		{
			variance = 0;
		}
		else if (PlayerPrefs.GetInt("CompletionState", 0) < 3)
		{
			variance = (GameManager.UsingRecordingSoftware() ? 3 : 2);
		}
		else
		{
			variance = 1;
		}
		gasterEyes = new Sprite[19];
		for (int i = 0; i < 19; i++)
		{
			gasterEyes[i] = Resources.Load<Sprite>("vfx/spr_gaster_eyes_" + i);
		}
		StartText(new string[40]
		{
			"* BACK IN MY YOUTH,^10 BEFORE I WAS\n  ASSIGNED ROYAL SCIENTIST...", "* I WAS BUT THE ROYAL MAGE.", "* IT WAS A FAMILY LINEAGE\n  FOR EONS.", "* WHEN I DISCOVERED HOW TO\n  CONVERT HOTLAND'S MAGMA INTO\n  ELECTRICITY...", "* IT WAS POSSIBLE TO POWER\n  ABANDONED HUMAN APPLIANCES\n  FOUND IN WATERFALL.", "* FROM THEN ON,^10 THE FAMILY\n  POSITION BECAME THAT OF\n  SCIENCE.", "* BUT THE LINEAGE ENDED WITH ME.", "* IT WAS WHEN THE DREEMURR\n  CHILDREN DIED.", "* FROM ASRIEL'S DUST LAID A\n  RED HUMAN SOUL.", "* FROM ASRIEL LEAVING WITH IT,^10\n  I FIGURED THE BARRIER UTILIZED\n  SOUL POWER.",
			"* I UNOFFICIALLY INVESTIGATED\n  THE HUMAN SOUL AND ITS\n  BEHAVIOR.", "* PERHAPS I COULD DISCOVER\n  A WAY TO BREAK THE BARRIER...?", "* HOWEVER,^10 AS MORE HUMAN SOULS\n  CAME INTO THE KING'S\n  POSSESSION...", "* I NOTICED SOMETHING PECULIAR.", "* THE RED SOUL WAS COMPLETELY\n  IMPERVIOUS TO DAMAGE.", "* UNLIKE THE OTHER HUMAN\n  SOULS...", "* WHICH WOULD SHOW SIGNS OF\n  DAMAGE UPON TAKING ITS\n  INTERNAL SUBSTANCE.", "* AS THE SOUL POWER EXPERIMENT\n  CAME TO AN INCONCLUSIVE\n  CLOSE...", "* I HAD TO KNOW HOW RESISTANT TO\n  CHANGE THE RED SOUL WAS.", "* MY TIME OF \"FALLING\" WAS\n  COMING CLOSE.",
			"* WITH WHAT LITTLE MORTAL LIFE I\n  HAD,^10 I RESIGNED FROM MY\n  POSITION...", "* TOOK THE RED SOUL WITH ME\n  TO THE CORE...", "* AND CLUTCHING IT IN MY GRASP...", "* LEAPED INTO THE OZONE BELOW.", "* WITHIN MERE MOMENTS,^10 I FELT\n  MYSELF BEING SCATTERED AND\n  SHATTERED.", "* AND YET MY GRASP ON THE SOUL\n  HAD NOT LOOSENED.", "* IT WAS ONLY WHEN MY MIND BEGAN\n  TO RIP APART DID I DECIDE TO\n  RELEASE.", "* AND HERE I WAS.", "* SURROUNDED BY AND SURROUNDING\n  EVERYTHING AND NOTHING.", "* YET...^10 SOMEHOW...",
			"* I FELT THE RED SOUL NEAR ME.", "* IT IS ALWAYS NEAR.", "* DESPITE BEING ABLE TO\n  RECONSTRUCT A NEW RESIDENCE\n  HERE...", "* DESPITE BEING ABLE TO REFORM MY\n  MIND INTO ONE ESSENCE...", "* I COULD NEVER ESCAPE.", "* I COULD ONLY LOOK IN FROM THE\n  OUTSIDE.", "* ONLY THROUGH THE VIEWPORT\n  OF THAT VERY SOUL.", "* WHEREVER IN TIME AND SPACE IT\n  RESIDES.", "* I ONLY DESIRE TO BECOME\n  WHOLE AGAIN.", "* AND FOR A LONG TIME,^10 I FELT\n  THAT I WOULD REMAIN HERE,^10\n  STUCK IN ETERNAL PURGATORY."
		}, new string[40]
		{
			"#v_gaster_s3_initial_0", "#v_gaster_s3_initial_1", "#v_gaster_s3_initial_2", "#v_gaster_s3_initial_3", "#v_gaster_s3_initial_4", "#v_gaster_s3_initial_5", "#v_gaster_s3_initial_6", "#v_gaster_s3_initial_7", "#v_gaster_s3_initial_8", "#v_gaster_s3_initial_9",
			"#v_gaster_s3_initial_10", "#v_gaster_s3_initial_11", "#v_gaster_s3_initial_12", "#v_gaster_s3_initial_13", "#v_gaster_s3_initial_14", "#v_gaster_s3_initial_15", "#v_gaster_s3_initial_16", "#v_gaster_s3_initial_17", "#v_gaster_s3_initial_18", "#v_gaster_s3_initial_19",
			"#v_gaster_s3_initial_20", "#v_gaster_s3_initial_21", "#v_gaster_s3_initial_22", "#v_gaster_s3_initial_23", "#v_gaster_s3_initial_24", "#v_gaster_s3_initial_25", "#v_gaster_s3_initial_26", "#v_gaster_s3_initial_27", "#v_gaster_s3_initial_28", "#v_gaster_s3_initial_29",
			"#v_gaster_s3_initial_30", "#v_gaster_s3_initial_31", "#v_gaster_s3_initial_32", "#v_gaster_s3_initial_33", "#v_gaster_s3_initial_34", "#v_gaster_s3_initial_35", "#v_gaster_s3_initial_36", "#v_gaster_s3_initial_37", "#v_gaster_s3_initial_38", "#v_gaster_s3_initial_39"
		}, new int[1] { 1 }, new string[0]);
		if (PlayerPrefs.GetInt("GasterSection3", 0) == 0)
		{
			PlayerPrefs.SetInt("GasterSection3", 1);
			txt.MakeUnskippable();
		}
	}
}
