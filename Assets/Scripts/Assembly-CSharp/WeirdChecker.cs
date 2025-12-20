using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public static class WeirdChecker
{
	public static readonly KeyValuePair<int, int>[] ruinsCombos = new KeyValuePair<int, int>[11]
	{
		new KeyValuePair<int, int>(10, 11),
		new KeyValuePair<int, int>(22, 23),
		new KeyValuePair<int, int>(24, 25),
		new KeyValuePair<int, int>(26, 27),
		new KeyValuePair<int, int>(28, 29),
		new KeyValuePair<int, int>(31, 32),
		new KeyValuePair<int, int>(35, 36),
		new KeyValuePair<int, int>(37, 38),
		new KeyValuePair<int, int>(43, 44),
		new KeyValuePair<int, int>(45, 46),
		new KeyValuePair<int, int>(47, 48)
	};

	public static readonly KeyValuePair<int, int>[] prvCombos = new KeyValuePair<int, int>[8]
	{
		new KeyValuePair<int, int>(68, 69),
		new KeyValuePair<int, int>(70, 71),
		new KeyValuePair<int, int>(72, 73),
		new KeyValuePair<int, int>(74, 75),
		new KeyValuePair<int, int>(76, 77),
		new KeyValuePair<int, int>(78, 79),
		new KeyValuePair<int, int>(80, 81),
		new KeyValuePair<int, int>(82, 83)
	};

	public static readonly KeyValuePair<int, int>[] caveCombos = new KeyValuePair<int, int>[10]
	{
		new KeyValuePair<int, int>(100, 101),
		new KeyValuePair<int, int>(131, 132),
		new KeyValuePair<int, int>(133, 134),
		new KeyValuePair<int, int>(135, 136),
		new KeyValuePair<int, int>(137, 138),
		new KeyValuePair<int, int>(139, 140),
		new KeyValuePair<int, int>(141, 142),
		new KeyValuePair<int, int>(143, 144),
		new KeyValuePair<int, int>(145, 146),
		new KeyValuePair<int, int>(147, 148)
	};

	public static readonly KeyValuePair<int, int>[] snowdinFirstHalfCombos = new KeyValuePair<int, int>[5]
	{
		new KeyValuePair<int, int>(181, 182),
		new KeyValuePair<int, int>(187, 188),
		new KeyValuePair<int, int>(190, 191),
		new KeyValuePair<int, int>(195, 196),
		new KeyValuePair<int, int>(201, 202)
	};

	public static readonly KeyValuePair<int, int>[] snowdinSecondHalfCombos = new KeyValuePair<int, int>[5]
	{
		new KeyValuePair<int, int>(206, 221),
		new KeyValuePair<int, int>(207, 222),
		new KeyValuePair<int, int>(242, 243),
		new KeyValuePair<int, int>(263, 264),
		new KeyValuePair<int, int>(277, 278)
	};

	private static Dictionary<int, int> enemyCounts = new Dictionary<int, int>
	{
		{ 11, 1 },
		{ 23, 2 },
		{ 25, 1 },
		{ 27, 2 },
		{ 29, 3 },
		{ 32, 1 },
		{ 36, 1 },
		{ 38, 1 },
		{ 44, 2 },
		{ 46, 2 },
		{ 48, 2 }
	};

	private static int[] specialEncounters = new int[19]
	{
		127, 58, 89, 98, 106, 109, 116, 124, 150, 154,
		173, 185, 205, 209, 241, 253, 245, 270, 281
	};

	private static Dictionary<int, int> exhaustCounts = new Dictionary<int, int>
	{
		{ 11, 3 },
		{ 23, 3 },
		{ 25, 3 },
		{ 27, 3 },
		{ 29, 3 },
		{ 32, 3 },
		{ 36, 2 },
		{ 38, 2 },
		{ 44, 3 },
		{ 46, 2 },
		{ 48, 3 },
		{ 69, 3 },
		{ 71, 3 },
		{ 73, 3 },
		{ 75, 3 },
		{ 77, 3 },
		{ 79, 3 },
		{ 81, 3 },
		{ 83, 3 },
		{ 101, 1 },
		{ 132, 1 },
		{ 134, 1 },
		{ 136, 1 },
		{ 138, 1 },
		{ 140, 1 },
		{ 142, 1 },
		{ 144, 1 },
		{ 146, 1 },
		{ 148, 1 },
		{ 182, 3 },
		{ 188, 3 },
		{ 191, 1 },
		{ 196, 3 },
		{ 202, 1 },
		{ 221, 1 },
		{ 222, 1 },
		{ 243, 3 },
		{ 264, 1 },
		{ 278, 1 }
	};

	public static int GetWeirdAreaProgress(GameManager gm, string music)
	{
		if (((int)gm.GetFlag(12) == 0 || (int)gm.GetFlag(13) == 0) && (int)gm.GetFlag(87) == 0)
		{
			return 0;
		}
		bool flag = (int)gm.GetFlag(12) == 1;
		if (music.Contains("mus_ruins"))
		{
			int exhaustedEncounterCount = GetExhaustedEncounterCount(gm, ruinsCombos);
			if (exhaustedEncounterCount >= 11)
			{
				if (gm.GetFlagInt(108) == 0)
				{
					gm.SetFlag(0, "g_1");
				}
				if (flag && (int)gm.GetFlag(13) < 3)
				{
					AdvanceTo(gm, 3, false);
				}
				return 2;
			}
			if (exhaustedEncounterCount >= 5)
			{
				if (gm.GetFlagInt(108) == 0)
				{
					gm.SetFlag(0, "g_start");
				}
				if (flag)
				{
					AdvanceTo(gm, 2, false);
				}
				return 1;
			}
		}
		if (music.Contains("mus_pr_valley") && GetExhaustedEncounterCount(gm, prvCombos) >= 8)
		{
			if ((int)gm.GetFlag(13) < 4 && flag)
			{
				AdvanceTo(gm, 4, false);
			}
			return 2;
		}
		if ((music.Contains("mus_cave") || music.Contains("mus_wintercaves")) && GetExhaustedEncounterCount(gm, caveCombos) >= 10)
		{
			if ((int)gm.GetFlag(13) < 6 && flag)
			{
				AdvanceTo(gm, 6, false);
			}
			return 2;
		}
		if (music.Contains("mus_snowy"))
		{
			int num = GetExhaustedEncounterCount(gm, snowdinFirstHalfCombos);
			if ((int)gm.GetFlag(185) == 1)
			{
				num++;
			}
			if (num >= 6)
			{
				num += GetExhaustedEncounterCount(gm, snowdinSecondHalfCombos);
				if ((int)gm.GetFlag(205) == 1)
				{
					num++;
				}
				if ((int)gm.GetFlag(209) == 1)
				{
					num++;
				}
				if ((int)gm.GetFlag(241) == 1)
				{
					num++;
				}
				if ((int)gm.GetFlag(253) == 1)
				{
					num++;
				}
				if ((int)gm.GetFlag(245) == 1)
				{
					num++;
				}
				if (num >= 16)
				{
					if ((int)gm.GetFlag(13) < 10 && flag)
					{
						AdvanceTo(gm, 10, false);
					}
					return 2;
				}
				if ((int)gm.GetFlag(13) < 9 && flag)
				{
					AdvanceTo(gm, 9, false);
				}
				return 1;
			}
		}
		return 0;
	}

	public static int GetExhaustedEncounterCount(GameManager gm, KeyValuePair<int, int>[] areaCombos)
	{
		int num = 0;
		for (int i = 0; i < areaCombos.Length; i++)
		{
			KeyValuePair<int, int> keyValuePair = areaCombos[i];
			if ((int)gm.GetFlag(keyValuePair.Key) == 1 && (int)gm.GetFlag(keyValuePair.Value) >= exhaustCounts[keyValuePair.Value])
			{
				num++;
			}
		}
		return num;
	}

	public static void Abort(GameManager gm)
	{
		gm.SetFlag(12, 0);
		gm.SetFlag(13, 0);
		if ((int)gm.GetFlag(204) == 1)
		{
			gm.SetFlag(0, "eye");
		}
		else if ((int)gm.GetFlag(102) == 0)
		{
			gm.SetFlag(0, "neutral");
		}
		if ((int)gm.GetFlag(223) == 11)
		{
			gm.SetFlag(223, 0);
		}
		if ((int)gm.GetFlag(211) == 1)
		{
			gm.SetFlag(211, 0);
		}
		if ((int)gm.GetFlag(257) == 1)
		{
			gm.SetFlag(257, 0);
			gm.SetFlag(1, "neutral");
		}
		gm.PlayGlobalSFX("sounds/snd_ominous_cancel");
	}

	public static void AdvanceTo(GameManager gm, int level, bool sound = true)
	{
		gm.SetFlag(13, level);
		gm.SetFlag(87, level);
		if (sound)
		{
			gm.PlayGlobalSFX("sounds/snd_ominous");
		}
	}

	public static void RoomModifications(GameManager gm)
	{
		if ((int)gm.GetFlag(13) >= 2 && (bool)GameObject.Find("RalseiSmokinAFatOne"))
		{
			Object.Destroy(GameObject.Find("RalseiSmokinAFatOne"));
		}
		if (HasCommittedBloodshed(gm))
		{
			if ((int)gm.GetFlag(12) == 1)
			{
				GameObject.Find("Susie").GetComponent<OverworldPartyMember>().UseUnhappySprites();
			}
			GameObject.Find("Noelle").GetComponent<OverworldPartyMember>().UseUnhappySprites();
		}
		if (gm.GetFlagInt(108) == 1 && gm.GetFlagInt(13) >= 2 && gm.GetFlagInt(127) == 1)
		{
			GameObject.Find("Susie").GetComponent<OverworldPartyMember>().UseUnhappySprites();
		}
		if (SceneManager.GetActiveScene().buildIndex < 50)
		{
			RuinsRoomMods(gm);
		}
		else if (SceneManager.GetActiveScene().buildIndex <= 71)
		{
			EBRoomMods(gm);
		}
		else
		{
			SnowdinRoomMods(gm);
		}
	}

	public static void DebugKill(GameManager gm, int untilID, bool aborted)
	{
		if (!gm.IsTestMode())
		{
			return;
		}
		int num = 0;
		if (untilID >= 2)
		{
			if (untilID == 3)
			{
				gm.SetFlag(127, 1);
				num++;
			}
			int num2 = 0;
			int num3 = ((untilID == 2) ? 5 : 11);
			KeyValuePair<int, int>[] array = ruinsCombos;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<int, int> keyValuePair = array[i];
				num += enemyCounts[keyValuePair.Value] * exhaustCounts[keyValuePair.Value];
				gm.SetFlag(keyValuePair.Key, 1);
				gm.SetFlag(keyValuePair.Value, exhaustCounts[keyValuePair.Value]);
				num2++;
				if (num2 == num3)
				{
					if (num2 >= 11)
					{
						gm.SetFlag(0, "g_1");
					}
					else if (num2 >= 5)
					{
						gm.SetFlag(0, "g_start");
					}
					break;
				}
			}
			gm.SetFlag(125, num);
		}
		if (untilID >= 4)
		{
			KeyValuePair<int, int>[] array = prvCombos;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<int, int> keyValuePair2 = array[i];
				gm.SetFlag(keyValuePair2.Key, 1);
				gm.SetFlag(keyValuePair2.Value, exhaustCounts[keyValuePair2.Value]);
			}
		}
		if (untilID >= 5)
		{
			gm.SetFlag(97, 1);
			gm.SetFlag(89, 1);
		}
		if (untilID >= 6)
		{
			gm.SetFlag(106, 1);
			gm.SetFlag(109, 1);
			gm.SetFlag(116, 1);
			gm.SetFlag(150, 1);
			KeyValuePair<int, int>[] array = caveCombos;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<int, int> keyValuePair3 = array[i];
				gm.SetFlag(keyValuePair3.Key, 1);
				gm.SetFlag(keyValuePair3.Value, exhaustCounts[keyValuePair3.Value]);
			}
		}
		if (untilID >= 7)
		{
			gm.SetFlag(172, 2);
			gm.SetFlag(173, 1);
		}
		if (untilID >= 8)
		{
			gm.SetFlag(172, 1);
			gm.SetFlag(181, 1);
			gm.SetFlag(182, 1);
		}
		if (untilID >= 9)
		{
			gm.SetFlag(185, 1);
			KeyValuePair<int, int>[] array = snowdinFirstHalfCombos;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<int, int> keyValuePair4 = array[i];
				gm.SetFlag(keyValuePair4.Key, 1);
				gm.SetFlag(keyValuePair4.Value, exhaustCounts[keyValuePair4.Value]);
			}
		}
		if (untilID >= 10)
		{
			gm.SetFlag(172, 0);
			gm.SetFlag(211, 1);
			gm.SetFlag(205, 1);
			gm.SetFlag(209, 1);
			gm.SetFlag(241, 1);
			gm.SetFlag(253, 1);
			gm.SetFlag(245, 1);
			KeyValuePair<int, int>[] array = snowdinSecondHalfCombos;
			for (int i = 0; i < array.Length; i++)
			{
				KeyValuePair<int, int> keyValuePair5 = array[i];
				gm.SetFlag(keyValuePair5.Key, 1);
				gm.SetFlag(keyValuePair5.Value, exhaustCounts[keyValuePair5.Value]);
			}
		}
		if (!aborted)
		{
			gm.SetFlag(13, untilID);
		}
		else
		{
			gm.SetFlag(12, 0);
			gm.SetFlag(13, 0);
		}
		gm.SetFlag(87, untilID);
	}

	public static bool HasKilled(GameManager gm)
	{
		int[] array = specialEncounters;
		foreach (int i2 in array)
		{
			if ((int)gm.GetFlag(i2) == 1 || (int)gm.GetFlag(i2) == 3)
			{
				return true;
			}
		}
		if (!HasKilledInGroup(ruinsCombos, gm) && !HasKilledInGroup(prvCombos, gm) && !HasKilledInGroup(caveCombos, gm) && !HasKilledInGroup(snowdinFirstHalfCombos, gm))
		{
			return HasKilledInGroup(snowdinSecondHalfCombos, gm);
		}
		return true;
	}

	public static bool HasKilledInGroup(KeyValuePair<int, int>[] group, GameManager gm)
	{
		for (int i = 0; i < group.Length; i++)
		{
			KeyValuePair<int, int> keyValuePair = group[i];
			if ((int)gm.GetFlag(keyValuePair.Key) == 1 || (int)gm.GetFlag(keyValuePair.Key) == 3 || (int)gm.GetFlag(keyValuePair.Value) > 1)
			{
				return true;
			}
		}
		return false;
	}

	public static bool HasCommittedBloodshed(GameManager gm)
	{
		if ((int)gm.GetFlag(87) >= 4)
		{
			return (int)gm.GetFlag(89) == 1;
		}
		return false;
	}

	public static int GetRemainingHardModeEnemies(GameManager gm)
	{
		int num = 0;
		if ((int)gm.GetFlag(30) == 0)
		{
			num++;
		}
		KeyValuePair<int, int>[] array = ruinsCombos;
		for (int i = 0; i < array.Length; i++)
		{
			KeyValuePair<int, int> keyValuePair = array[i];
			int num2 = exhaustCounts[keyValuePair.Value] - (int)gm.GetFlag(keyValuePair.Value);
			num += enemyCounts[keyValuePair.Value] * num2;
		}
		return num;
	}

	public static bool HasSparedEveryEncounterInUnderfell(GameManager gm)
	{
		KeyValuePair<int, int>[][] array = new KeyValuePair<int, int>[2][] { snowdinFirstHalfCombos, snowdinSecondHalfCombos };
		foreach (KeyValuePair<int, int>[] array2 in array)
		{
			foreach (KeyValuePair<int, int> keyValuePair in array2)
			{
				if ((int)gm.GetFlag(keyValuePair.Key) != 2)
				{
					return false;
				}
			}
		}
		int[] array3 = new int[5] { 185, 205, 209, 253, 245 };
		foreach (int i2 in array3)
		{
			if ((int)gm.GetFlag(i2) != 2)
			{
				return false;
			}
		}
		return true;
	}

	private static void RuinsRoomMods(GameManager gm)
	{
		if ((int)gm.GetFlag(87) >= 2)
		{
			if (SceneManager.GetActiveScene().buildIndex == 24)
			{
				Object.Destroy(GameObject.Find("Froggit"));
				Object.Destroy(GameObject.Find("WallSign"));
			}
			else if (SceneManager.GetActiveScene().buildIndex == 32)
			{
				Object.Destroy(GameObject.Find("Froggit"));
			}
			else if (SceneManager.GetActiveScene().buildIndex == 33)
			{
				Object.FindObjectOfType<InteractItemPickup>().ModifyPurchaseContents(new string[5] { "* You got the Toy Knife.", "* Oh shoot,^05 Kris!\n^05* A knife!", "* That's uhh...^05 really\n  you...", "* What do you mean\n  I'M acting weird???", "* Maybe if you loosened\n  up a bit,^05 I wouldn't\n  be so tense." }, new string[5] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[6], new string[5] { "", "su_surprised", "su_smirk_sweat", "su_angry", "su_annoyed" });
			}
		}
		if ((int)gm.GetFlag(87) >= 3)
		{
			if ((int)gm.GetFlag(13) >= 3 && SceneManager.GetActiveScene().buildIndex == 7)
			{
				GameObject.Find("Frisk").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* (You felt your heart sink.)" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			}
			if (SceneManager.GetActiveScene().buildIndex == 35)
			{
				GameObject.Find("Mirror").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (It's you and Susie.)", "* (You couldn't look yourself in\n  the eyes.)" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[2] { "", "" });
			}
			if (SceneManager.GetActiveScene().buildIndex == 40)
			{
				GameObject.Find("Cupboard").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (No knives.)", "* (You felt a relief settle.)" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 15 && HasKilled(gm))
		{
			Object.Destroy(GameObject.Find("Froggit"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 12 && gm.GetFlagInt(58) == 0)
		{
			Object.Destroy(GameObject.Find("MadMewMew"));
		}
		if ((int)gm.GetFlag(108) == 1)
		{
			if (SceneManager.GetActiveScene().buildIndex == 7)
			{
				GameObject.Find("Flowers").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (Golden flowers.)^05\n* (They must have\n  broken your fall.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 9 && (int)gm.GetFlag(13) < 2)
			{
				Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[2] { "* (The shadow of the ruins\n  looms above,^05 filling you with\n  determination.)", "* (HP fully restored.)" });
			}
			else if (SceneManager.GetActiveScene().buildIndex == 11)
			{
				Object.Destroy(GameObject.Find("PleaseGodReadThis"));
				Object.Destroy(GameObject.Find("PleaseGodReadThis (1)"));
				GameObject.Find("WallSwitch (2)").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* This switch doesn't even\n  work." }, new string[1] { "snd_text" }, new int[1], new string[0]);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 15)
			{
				if ((int)gm.GetFlag(13) < 2)
				{
					Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[2] { "* (Playfully crinkling through\n  the leaves fills you with\n  determination.)", "* (HP fully restored.)" });
				}
				if ((bool)GameObject.Find("Froggit"))
				{
					GameObject.Find("Froggit").GetComponent<InteractTextBox>().ModifyContents(new string[6] { "* Ribbit,^10 ribbit.\n^10* (Excuse me,^10 human.)", "* (I have some advice\n  for you about battling\n  monsters.)", "* (The monsters in this world\n  are very dangerous and\n  aggressive.)", "* (In this place, monsters won't\n  become sparable by reducing\n  their HP.)", "* (They will stand their ground\n  unless you resolve the\n  conflict through ACTing.)", "* (I hope this proves as useful.)\n^10* Ribbit." }, new string[6] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text" }, new int[6], new string[0]);
					GameObject.Find("Froggit").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[3] { "* Wait,^05 if the people\n  here are aggressive,^05\n  then why'd you help us?", "* Ribbit.\n^10* (I'm unsure,^05 but it might\n  be because of my low LV.)", "* Alright,^05 nerd,^05 whatever\n  you say." }, new string[3] { "snd_txtsus", "snd_text", "snd_txtsus" }, new int[6], new string[3] { "su_side", "", "su_confident" });
				}
			}
			else if (SceneManager.GetActiveScene().buildIndex == 21 && (int)gm.GetFlag(13) < 2)
			{
				Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[2] { "* (Knowing the mouse might one\n  day leave its hole and\n  get the cheese...)", "* (It fills you with\n  determination.)" });
			}
			else if (SceneManager.GetActiveScene().buildIndex == 24 && (int)gm.GetFlag(13) < 2)
			{
				GameObject.Find("Froggit").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[4] { "* Where are the other\n  two?", "* Ribbit.\n* (You probably fought them\n  already.)", "* And...^05 I guess you\n  won't?", "* Ribbit...\n* (I'm too weak to fight.)" }, new string[4] { "snd_txtsus", "snd_text", "snd_txtsus", "snd_text" }, new int[6], new string[4] { "su_side", "", "su_inquisitive", "" });
			}
			else if (SceneManager.GetActiveScene().buildIndex == 25)
			{
				Object.FindObjectOfType<InteractItemPickup>().ModifyPurchaseContents(new string[4] { "* You got the Faded Ribbon.", "* Hey,^05 I think that's\n  like...^10 armor?", "* I know someone that\n  would say like... you\n^10  wear it with ^C.", "* Hell if I'm wearing\n  THAT,^05 though." }, new string[4] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[6], new string[4] { "", "su_smirk_sweat", "su_smile_sweat", "su_annoyed" });
			}
			else if (SceneManager.GetActiveScene().buildIndex == 32)
			{
				if ((bool)GameObject.Find("Froggit"))
				{
					GameObject.Find("Froggit").GetComponent<InteractTextBox>().ModifyContents(new string[6] { "* Ribbit,^10 ribbit.\n^10* (Ooh,^10 are you Susie,^05 who's\n  looking over the human?)", "* HOW DO YOU KNOW\n  WHO WE ARE???", "* Ribbit,^10 ribbit.\n^10* (<color=#0000FFFF>TORIEL</color> has been talking about\n  you two a lot.)", "* (She apparently has a surprise\n  for you.)", "* Huh?", "* (Don't keep her waiting now.)\n* Ribbit." }, new string[6] { "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_txtsus", "snd_text" }, new int[6], new string[6] { "", "su_angry", "", "", "su_surprised", "" });
				}
			}
			else if (SceneManager.GetActiveScene().buildIndex == 33)
			{
				if ((int)gm.GetFlag(13) < 2)
				{
					Object.FindObjectOfType<InteractItemPickup>().ModifyPurchaseContents(new string[3] { "* You got the Toy Knife.", "* Oh shoot!\n^05* A knife!", "* Wait, is that a\n  PLASTIC knife?^05\n* That's kinda lame." }, new string[4] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[6], new string[3] { "", "su_surprised", "su_smile_sweat" });
				}
				else
				{
					Object.FindObjectOfType<InteractItemPickup>().ModifyPurchaseContents(new string[5] { "* You got the Toy Knife.", "* ...", "* Hey,^05 isn't that made\n  of plastic?", "* Seems kinda useless,^05\n  huh.", "* (... Can this kid stop\n  looking at me like\n  that?)" }, new string[5] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[6], new string[5] { "", "su_side_sweat", "su_smile_sweat", "su_smile", "su_side_sweat" });
				}
			}
			else if (SceneManager.GetActiveScene().buildIndex == 37)
			{
				if ((int)gm.GetFlag(13) < 2)
				{
					Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[1] { "* (Seeing such a cute,^05 tiny\n  house in the RUINS gives\n  you determination.)" });
				}
				Object.FindObjectOfType<Moss>().ModifyContents(new string[4] { "* You found the <color=#00FF00FF>[MOSS]</color>!", "* What the hell is\n  with that look?", "* You know what?^05\n* Move over.", "* Susie eats the moss.^10\n* Alone." }, new string[4] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_text" }, new int[4], new string[4] { "", "su_side", "su_annoyed", "" });
			}
			else if (SceneManager.GetActiveScene().buildIndex == 34)
			{
				GameObject.Find("Plant").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* Inside is an old calendar\n  from the beginning of\n  201X." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Shelf").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* These books are worn...\n^05* They must have been read\n  many times." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("LoadingZone (3)").GetComponent<LoadingZone>().ModifyContents("* We should at least\n  check out the surprise\n  before leaving.", "snd_txtsus", "su_annoyed");
			}
			else if (SceneManager.GetActiveScene().buildIndex == 35)
			{
				string[] array = new string[2] { "WaterSausage", "WaterSausage (1)" };
				foreach (string name in array)
				{
					string text = (((int)gm.GetFlag(121) == 1) ? "* Oh!\n^10* It is a \"water sausage.\"" : "* You have seen this type\n  of plant before but\n  do not know its name.");
					GameObject.Find(name).GetComponent<InteractTextBox>().ModifyContents(new string[1] { text }, new string[1] { "snd_text" }, new int[1], new string[0]);
					GameObject.Find(name).GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { text }, new string[1] { "snd_text" }, new int[1], new string[0]);
				}
				Object.Destroy(GameObject.Find("RedFlower").GetComponent<InteractTextBox>());
				Object.Destroy(GameObject.Find("YellowFlower").GetComponent<InteractTextBox>());
				GameObject.Find("RedFlower (1)").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* Inside the drawer are\n  flower seeds and some\n  broken crayons." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Mirror").GetComponent<InteractTextBox>().ModifyContents(new string[1] { ((int)gm.GetFlag(13) >= 3) ? "* It's me,^05 Frisk." : "* It's you and Susie!" }, new string[1] { "snd_text" }, new int[1], new string[0]);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 36)
			{
				GameObject.Find("Diary").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* It seems to be TORIEL's diary,^05\n  but the pages are blank." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Chairiel").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (Toriel's small chair.)\n* (Its name is Chairiel.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Chairiel").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* (Toriel's small chair.)\n* (Its name is Chairiel.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Bucket").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* Hell yeah, snails!", "* ...", "* Hey,^05 mind your\n  business!" }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[4], new string[4] { "su_excited", "su_smile_sweat", "su_angry", "" });
				GameObject.Find("Bucket").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* Just a regular old bucket^30\n\n  of snails." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Bed").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (Definitely bigger than\n  a twin-sized bed.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Books").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* It's an encyclopedia of\n  subterranean plants. You\n  open to the middle...", "* \"Typha\" - A group of wetland\n  flowering plants with\n  brown,^10 oblong seedpods.", "* Known more commonly as\n \"water sausages.\"" }, new string[3] { "snd_text", "snd_text", "snd_text" }, new int[3], new string[0]);
				GameObject.Find("Books").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[3] { "* It's an encyclopedia of\n  subterranean plants. You\n  open to the middle...", "* \"Typha\" - A group of wetland\n  flowering plants with\n  brown,^10 oblong seedpods.", "* Known more commonly as\n \"water sausages.\"" }, new string[3] { "snd_text", "snd_text", "snd_text" }, new int[3], new string[0]);
				GameObject.Find("SockDrawer").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* You peek inside...^05\n* Scandalous!", "* It's TORIEL's sock drawer." }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[0]);
				GameObject.Find("SockDrawer").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[2] { "* You peek inside...^05\n* Scandalous!", "* It's TORIEL's sock drawer." }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[0]);
				GameObject.Find("Cactus").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* Ah,^05 the cactus.^05\n* Truly the most tsundere\n  of plants." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Cactus").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* I hate that this\n  cactus is reminding me\n  of someone." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_inquisitive" });
			}
			else if (SceneManager.GetActiveScene().buildIndex == 38)
			{
				if ((int)gm.GetFlag(13) > 2)
				{
					GameObject.Find("KrisBed").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* ..." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				}
				else
				{
					GameObject.Find("KrisBed").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (Perhaps it's a bit too big\n  for you.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
				}
				GameObject.Find("LoadingZone").GetComponent<LoadingZone>().ModifyContents("* C'mon.", "snd_txtsus", "su_annoyed");
			}
			else if (SceneManager.GetActiveScene().buildIndex == 39)
			{
				GameObject.Find("Fire").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* The fire isn't burning hot...\n^05* Just pleasantly warm.\n^05* You could put your hand inside." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Tools").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* The ends of the tools have\n  been filed down to\n  make them safer." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Bookshelf").GetComponent<InteractTextBox>().ModifyContents(new string[6] { "* It's a history book.\n^05* Here's a random page...", "* Trapped behind the barrier\n  and fearful of further\n  human attacks,^05 we retreated.", "* Far,^05 far into the earth\n  we walked,^05 until we reached\n  the cavern's end.", "* This was our new home,^05\n  which we named...", "* \"Home.\"", "* As great as our king is,^05\n  he is pretty lousy at\n  names." }, new string[6] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text" }, new int[6], new string[0]);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 40)
			{
				GameObject.Find("Stove").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* The stovetop is very clean.\n^05* Toriel must use fire\n  magic instead." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Cupboard").GetComponent<InteractTextBox>().ModifyContents(new string[1] { ((int)gm.GetFlag(13) >= 3) ? "<color=#FF0000FF>* Where are the knives.</color>" : "* Inside the cupboard are\n  cookie cutters for\n  gingerbread monsters." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Sink").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* There is some white fur\n  stuck in the drain." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Fridge").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* For some reason,^05 there\n  is a brand-name chocolate\n  bar in the fridge." }, new string[1] { "snd_text" }, new int[1], new string[0]);
				GameObject.Find("Fridge").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* For some reason,^05 there\n  is a brand-name chocolate\n  bar in the fridge." }, new string[1] { "snd_text" }, new int[1], new string[0]);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 46 && (int)gm.GetFlag(57) == 0)
			{
				GameObject.Find("BigFlower").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/flowey_scene/spr_bigflowey_rise_7");
			}
		}
		if ((int)gm.GetFlag(53) == 1)
		{
			if (SceneManager.GetActiveScene().buildIndex == 7 && (int)gm.GetFlag(108) == 0)
			{
				GameObject.Find("Frisk").transform.position = new Vector3(-0.292f, -0.603f);
				Object.Destroy(GameObject.Find("Flowers"));
			}
			else if (SceneManager.GetActiveScene().buildIndex == 8 && (int)gm.GetFlag(122) == 0 && (int)gm.GetFlag(108) == 1)
			{
				GameObject.Find("Knife").transform.position = new Vector3(-0.05f, -0.76f);
			}
			else if (SceneManager.GetActiveScene().buildIndex == 35 && (int)gm.GetFlag(56) == 1)
			{
				gm.PlayMusic("music/mus_toriel", 0.4f, 0.1f);
				if ((int)gm.GetFlag(13) >= 3)
				{
					GameObject.Find("AsrielRoom").GetComponent<InteractTextBox>().ModifyContents(new string[1] { ((int)gm.GetFlag(108) == 1) ? "* Not worth talking to." : "* (You don't want to think\n  about it.)" }, new string[1] { "snd_text" }, new int[2], new string[2] { "", "" });
				}
				else
				{
					GameObject.Find("AsrielRoom").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (You could hear quiet sobbing\n  on the other side.)" }, new string[1] { "snd_text" }, new int[2], new string[2] { "", "" });
				}
			}
			else if (SceneManager.GetActiveScene().buildIndex == 36)
			{
				if ((int)gm.GetFlag(108) == 1)
				{
					GameObject.Find("Diary").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* It's TORIEL's diary.^05\n* There's something written\n  here...", "* \"Why did the skeleton want\n  a friend?\"", "* \"Because she was feeling\n  BONELY...\"" }, new string[4] { "snd_text", "snd_text", "snd_text", "snd_text" }, new int[4], new string[4] { "", "", "", "" });
				}
				else
				{
					GameObject.Find("Diary").GetComponent<InteractTextBox>().ModifyContents(new string[4] { "* (There is something written\n  in Toriel's diary.)", "* \"Why did the skeleton want\n  a friend?\"", "* \"Because she was feeling\n  BONELY...\"", "* (You remembered something\n  about a skeleton that\n  needs friends.)" }, new string[4] { "snd_text", "snd_text", "snd_text", "snd_text" }, new int[4], new string[4] { "", "", "", "" });
				}
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 38 && gm.GetFlagInt(316) == 1)
		{
			GameObject gameObject = GameObject.Find("Pie");
			if ((bool)gameObject)
			{
				gameObject.transform.position = new Vector3(0f, gameObject.transform.position.y);
			}
		}
		if ((int)gm.GetFlag(58) == 1 && SceneManager.GetActiveScene().buildIndex == 46)
		{
			GameObject.Find("DeadFlowey").transform.position = new Vector3(0f, 0.172f);
		}
		if ((int)gm.GetFlag(60) == 1 && SceneManager.GetActiveScene().buildIndex == 48)
		{
			Object.Destroy(GameObject.Find("Stick"));
		}
	}

	private static void EBRoomMods(GameManager gm)
	{
		if (SceneManager.GetActiveScene().buildIndex >= 51 && ((int)gm.GetFlag(13) >= 4 || (int)gm.GetFlag(87) >= 4) && (bool)GameObject.Find("HintMan"))
		{
			Object.Destroy(GameObject.Find("HintMan"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 54 && (int)gm.GetFlag(87) >= 5)
		{
			if ((bool)GameObject.Find("BikeGuy"))
			{
				Object.Destroy(GameObject.Find("BikeGuy"));
			}
			GameObject.Find("Bunny").transform.position = new Vector3(0.23f, -4.91f);
			GameObject.Find("Bunny").GetComponent<BoxCollider2D>().size = new Vector2(0.8f, 5f);
			GameObject.Find("Bunny").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (A small, white bunny is\n  protecting the town.)", "* (It appears to be working.)" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[0]);
			GameObject.Find("Bunny").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* (She is doing a great job.)" }, new string[1] { "snd_text" }, new int[1], new string[0]);
		}
		if (SceneManager.GetActiveScene().buildIndex == 53)
		{
			if (GameManager.GetOptions().contentSetting.value == 1)
			{
				SpriteRenderer[] componentsInChildren = GameObject.Find("DeadCultists").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill_age_tw");
				}
			}
			if ((int)gm.GetFlag(87) >= 5 && (int)gm.GetFlag(106) == 1)
			{
				GameObject.Find("DeadCultists").transform.position = Vector3.zero;
			}
			if ((int)gm.GetFlag(116) != 0 && (int)gm.GetFlag(87) < 5)
			{
				GameObject.Find("FixedBridge").GetComponent<Tilemap>().enabled = true;
				GameObject.Find("FixedBridge").GetComponent<TilemapRenderer>().enabled = true;
				Object.Destroy(GameObject.Find("bridgeblock"));
				Object.Destroy(GameObject.Find("BlueMario"));
				GameObject.Find("Sign").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (Cross bridge to Happy-Happy\n  Village)", "* FINALLY!!!" }, new string[2] { "snd_text", "snd_txtsus" }, new int[1], new string[2] { "", "su_angry" });
			}
			if (gm.GetFlagInt(150) == 0)
			{
				Object.Destroy(GameObject.Find("MadMewMew"));
			}
			if (HasCommittedBloodshed(gm) && (int)gm.GetFlag(87) == 4)
			{
				Object.FindObjectOfType<SansShopBase>().ModifyContents(new string[8] { "*\tif i may ask...", "*\twhat was that yelling that\n\ti heard?", "*\t...", "*\teh,^05 forget it.", "*\tyou don't need to tell\n\tme.", "*\tit doesn't really matter\n\tin the end.", "*\tjust...", "*\ti hope you're happy\n\twith the path that\n\tyou're taking." }, new string[8] { "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans" }, new int[8], new string[8] { "sans_closed", "sans_empty", "sans_closed", "sans_side", "sans_neutral", "sans_neutral", "sans_closed", "sans_closed" });
			}
			else if ((int)gm.GetFlag(87) >= 5)
			{
				Object.FindObjectOfType<SansShopBase>().ModifyContents(new string[6] { "*\tdid uhh...^05 something\n\thappen?", "*\ti just saw a bunch\n\tof people run out of\n\tthis cave in a hurry.", "*\tand you...^05 you look awful.", "*\t...", "*\teh,^05 don't worry about\n\ttelling me anything.", "*\tyou've got other things\n\tto worry about." }, new string[8] { "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans", "snd_txtsans" }, new int[8], new string[6] { "sans_side", "sans_closed", "sans_neutral", "sans_neutral", "sans_side", "sans_closed" });
				Object.Destroy(GameObject.Find("MushroomLady"));
				Object.Destroy(GameObject.Find("BlueMario"));
			}
			else if (gm.GetMiniPartyMember() == 1)
			{
				Object.FindObjectOfType<SansShopBase>().ModifyContents(new string[9] { "*\they, is that another\n\thuman that you're lugging\n\taround?", "*\tis that the girl that the\n\tpeople in twoson were\n\tworried about?", "* Well,^05 I'd hope that\n  they're worried...", "*\ti see.", "*\tso that makes you\n\tpaula.", "* ...Yeah.", "*\tcool.", "*\twell i hope you get\n\thome safe soon.", "*\tsame to the rest\n\tof you." }, new string[9] { "snd_txtsans", "snd_txtsans", "snd_txtpau", "snd_txtsans", "snd_txtsans", "snd_txtpau", "snd_txtsans", "snd_txtsans", "snd_txtsans" }, new int[9], new string[9] { "sans_side", "sans_neutral", "pau_dejected", "sans_closed", "sans_neutral", "pau_annoyed", "sans_side", "sans_wink", "sans_closed" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 55 && (((int)gm.GetFlag(13) >= 4 && (int)gm.GetFlag(12) == 1) || (int)gm.GetFlag(12) == 0))
		{
			Object.Destroy(GameObject.Find("OblitNotif"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 56)
		{
			if ((int)gm.GetFlag(87) >= 5)
			{
				string[] array = new string[11]
				{
					"CreepyLady", "NPC-Hippie", "KidnapGuy", "NPC-TMI", "NPC-Divorce", "NPC-SaxPlayer", "NPC-WeirdoPeace", "NPC-Lady", "Cow", "NPC-Healer",
					"NPC-HeadLady"
				};
				for (int i = 0; i < array.Length; i++)
				{
					Object.Destroy(GameObject.Find(array[i]));
				}
				GameObject.Find("Farm").GetComponent<InteractKnockKnockDoor>().ModifyContents(new string[2] { "* (Knock knock knock)", "* (There's no response...)" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[2]);
				GameObject.Find("Saturn").GetComponent<InteractKnockKnockDoor>().ModifyContents(new string[2] { "* (Knock knock knock)", "* (There's no response...)" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[2]);
				GameObject.Find("Saturn").GetComponent<InteractKnockKnockDoor>().ModifySecondaryContents(new string[2] { "* (Knock knock knock)", "* (There's no response...)" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[2]);
				if ((int)gm.GetFlag(12) == 1)
				{
					Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[1] { "* The silence is deafening." });
				}
				else
				{
					Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[2] { "* (You can feel the guilt\n  weighing down on you as you\n  stand in this empty town.)", "* (Despite this,^05 you are filled\n  with a certain power.)" });
				}
				GameObject.Find("Recruit").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* (There's a sign on the door.)", "* \"The recruitment center will\n  open back up at 1:00 PM!\"", "* Something's telling me\n  they won't get another\n  recruit ever again." }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[4], new string[3] { "", "", "su_side_sweat" });
			}
			else if ((int)gm.GetFlag(116) != 0)
			{
				GameObject.Find("NPC-WeirdoPeace").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* We need to return the town\n  to its original colors.", "* I'll be busy.", "* Uhh... good luck,^05\n  I guess." }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[4], new string[3] { "", "", "su_side" });
				GameObject.Find("NPC-WeirdoPeace").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[3] { "* You're not gonna help us?", "* We're pretty busy\n  ourselves,^05 actually.", "* Oh..." }, new string[3] { "snd_text", "snd_txtnoe", "snd_text" }, new int[4], new string[3] { "", "no_happy", "" });
				GameObject.Find("NPC-SaxPlayer").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* It was hard on the eyes\n  to have everything blue.", "* THEN WHY DID YOU\n  LET EVERYTHING BE\n  BLUE?!?!?", "* You don't think about these\n  things when you're ingrained\n  in them." }, new string[3] { "snd_text", "snd_txtsus", "snd_text" }, new int[4], new string[3] { "", "su_angry", "" });
				GameObject.Find("NPC-HeadLady").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* I woke up from the dream.", "* ... That's how you'd\n  describe it?" }, new string[3] { "snd_text", "snd_txtnoe", "snd_text" }, new int[4], new string[3] { "", "no_thinking", "" });
				GameObject.Find("NPC-Hippie").GetComponent<InteractTextBox>().ModifyContents(new string[4] { "* We might've been listening\n  to evil messages rather\n  than good ones.", "* So...", "* I was right.", "* ...^05 Yeah,^05 whatever man." }, new string[4] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_text" }, new int[4], new string[4] { "", "su_side", "su_smile", "" });
				GameObject.Find("NPC-Hippie").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* I still need some time,^05 man." }, new string[1] { "snd_text" }, new int[4], new string[1] { "" });
				Object.FindObjectOfType<KidnapGuy>().ModifySecondaryContents(new string[6] { "* You really think I was\n  a bad boy?", "* YEAH????", "* YOU LITERALLY\n  KIDNAPPED ME!!!", "* S-sometimes you give into\n  peer pressure!!", "* I'll give into putting\n  this stick through your\n  chest...", "* ... If you don't\n  SHUT UP." }, new string[6] { "snd_text", "snd_txtpau", "snd_txtpau", "snd_text", "snd_txtsus", "snd_txtsus" }, new int[6], new string[6] { "", "pau_mad_sweat", "pau_mad_sweat", "", "su_depressed", "su_teeth" });
				GameObject.Find("Cow").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cow");
				GameObject.Find("Cow").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (The blue cow thing wasn't\n  such a great idea, huh.)" }, new string[1] { "snd_text" }, new int[4], new string[1] { "" });
				GameObject.Find("NPC-Divorce").GetComponent<InteractTextBox>().ModifyContents(new string[4] { "* My wife has run away for\n  a second time.", "* Maybe the issue isn't\n  your religion...", "* Hey,^05 that's a good thing!\n^10* I'm such a lucky man!", "* ..." }, new string[4] { "snd_text", "snd_txtnoe", "snd_text", "snd_txtnoe" }, new int[4], new string[4] { "", "no_happy", "", "no_confused" });
				GameObject.Find("NPC-TMI").GetComponent<InteractTextBox>().ModifyContents(new string[6] { "* I apologize.", "* ... For?", "* Giving you useless information.", "* It wasn't useless.", "* It...^05 insinuated that\n  Paula was in\n  Carpainter's hands.", "* Wait,^05 he was about to\n  sacrifice a little girl??!?!" }, new string[6] { "snd_text", "snd_txtsus", "snd_text", "snd_txtnoe", "snd_txtnoe", "snd_text" }, new int[6], new string[6] { "", "su_side", "", "no_neutral", "no_happy", "" });
				GameObject.Find("NPC-TMI").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* I don't know how to\n  process this information." }, new string[1] { "snd_text" }, new int[4], new string[1] { "" });
				GameObject.Find("Sign (2)").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (All of our products are\n  blessed.)\n         --Blue-Blue Drugstore", "* You'd think they'd at\n  least change the damn\n  sign." }, new string[2] { "snd_text", "snd_txtsus" }, new int[4], new string[2] { "", "su_annoyed" });
				GameObject.Find("Recruit").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (The door is locked.)", "* Looks like they'll need\n  to find another use\n  for this house." }, new string[2] { "snd_text", "snd_txtsus" }, new int[4], new string[2] { "", "su_smile" });
			}
			else if (gm.GetMiniPartyMember() == 1)
			{
				Object.FindObjectOfType<KidnapGuy>().ModifySecondaryContents(new string[4] { "* Recognize her...?", "* P-^05please leave me alone...", "* That's what I thought,^05\n  freak.", "* (...!)" }, new string[4] { "snd_txtsus", "snd_text", "snd_txtsus", "snd_txtnoe" }, new int[4], new string[4] { "su_depressed", "", "su_teeth", "no_surprised_happy" });
			}
			if ((int)gm.GetFlag(87) < 5 && (int)gm.GetFlag(117) == 1)
			{
				Object.FindObjectOfType<SAVEPoint>().ModifyPhrases(new string[2] { "* (This town's future is starting\n  to look brighter and brighter.)", "* (You're filled with the power\n  of reform.)" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 59)
		{
			if ((int)gm.GetFlag(87) >= 6)
			{
				Object.Destroy(GameObject.Find("DoctorAmigoBalls"));
				Object.Destroy(GameObject.Find("Nurse"));
			}
			else if ((int)gm.GetFlag(150) != 0)
			{
				GameObject.Find("DoctorAmigoBalls").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* Amoogoos", "* Soosy amoogoss" }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
			}
			else if ((int)gm.GetFlag(87) == 5)
			{
				GameObject.Find("DoctorAmigoBalls").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* Do take care of yourself,^05 Kris.", "* After all,^05 you wouldn't want to\n  end up back in here again." }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 61)
		{
			if ((int)gm.GetFlag(87) >= 5)
			{
				Object.Destroy(GameObject.Find("BlueMario"));
				Object.Destroy(GameObject.Find("BlueMarioDesk"));
			}
			else if ((int)gm.GetFlag(116) != 0)
			{
				GameObject.Find("BlueMario").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* So you're the people that...", "* You three went after Mr.\n  Carpainter and...", "* You're awesome!" }, new string[1] { "snd_text" }, new int[4], new string[1] { "" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 65 && gm.GetFlagInt(87) == 3)
		{
			GameObject.Find("GonerInteract").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* Useless bodies." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
		}
	}

	private static void SnowdinRoomMods(GameManager gm)
	{
		if (SceneManager.GetActiveScene().buildIndex == 73 && (int)gm.GetFlag(172) > 0)
		{
			if (gm.NoelleInParty())
			{
				GameObject.Find("Sentry").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (It's some kind of sentry\n  station.)", "* ..." }, new string[2] { "snd_text", "snd_txtnoe" }, new int[4], new string[2] { "", "no_depressedx" });
			}
			else
			{
				GameObject.Find("Sentry").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (It's some kind of sentry\n  station.)" }, new string[2] { "snd_text", "snd_txtnoe" }, new int[4], new string[2] { "", "no_depressedx" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 75 && gm.GetFlagInt(245) == 0)
		{
			Object.Destroy(GameObject.Find("Smurf"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 79)
		{
			if (!gm.SusieInParty())
			{
				GameObject.Find("Sign").GetComponent<InteractTextBox>().ShrinkLines(2);
			}
			if (!gm.NoelleInParty() || (int)gm.GetFlag(172) > 0)
			{
				GameObject.Find("SmokeTreats").GetComponent<InteractTextBox>().ShrinkLines(1);
				GameObject.Find("TreatPile").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (You considered smoking one...)", "* (... But you realized it\n  wouldn't make you feel any\n  better.)" }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
				GameObject.Find("TreatPile").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 84)
		{
			if (!gm.SusieInParty())
			{
				GameObject.Find("Dogamy").GetComponent<InteractTextBox>().ShrinkLines(1);
				GameObject.Find("Sign").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
			if (!gm.NoelleInParty() || (int)gm.GetFlag(172) > 0)
			{
				GameObject.Find("Dogaressa").GetComponent<InteractTextBox>().ShrinkLines(1);
				GameObject.Find("Sign").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 86)
		{
			if (!gm.SusieInParty())
			{
				GameObject.Find("Spaghetti").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (It's a plate of frozen\n  spaghetti.)", "* (It's so cold,^05 it's stuck\n  to the table.)" }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
				GameObject.Find("Spaghetti").GetComponent<InteractTextBox>().DisableSecondaryLines();
			}
			else if ((int)gm.GetFlag(172) > 0)
			{
				GameObject.Find("Spaghetti").GetComponent<InteractTextBox>().ShrinkSecondaryLines(1);
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 90 && gm.GetFlagInt(245) == 0)
		{
			Object.Destroy(GameObject.Find("MadMewMew"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 95 && !gm.NoelleInParty())
		{
			GameObject.Find("Sign").GetComponent<InteractTextBox>().ShrinkLines(2);
		}
		if (SceneManager.GetActiveScene().buildIndex == 96 && ((int)gm.GetFlag(270) != 2 || (int)gm.GetFlag(275) == 1))
		{
			Object.Destroy(GameObject.Find("Jerry"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 97 && (int)gm.GetFlag(12) == 1)
		{
			Object.Destroy(GameObject.Find("RalseiSmokinAFatOne"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 99)
		{
			if (gm.GetFlagInt(270) == 0)
			{
				Object.Destroy(GameObject.Find("Creatyre"));
			}
			else if (gm.GetFlagInt(270) == 1)
			{
				GameObject.Find("Creatyre").GetComponent<InteractTextBox>().EnableSecondaryLines();
				GameObject.Find("Creatyre").GetComponent<InteractTextBox>().ForceTalkedToBefore();
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 105 && (int)gm.GetFlag(245) != 0)
		{
			if ((bool)GameObject.Find("GreaterDog"))
			{
				Object.Destroy(GameObject.Find("GreaterDog"));
			}
			if ((bool)GameObject.Find("QC"))
			{
				Object.Destroy(GameObject.Find("QC"));
			}
			if ((int)gm.GetFlag(245) == 2)
			{
				GameObject.Find("DogArmor").transform.position += new Vector3(0f, -10f);
				GameObject.Find("Doghouse").GetComponent<InteractTextBox>().ModifyContents(new string[2] { "* (It's a tidy little dog\n  house with a dog sleeping\n  inside.)", "* (You wish you could pet\n  the dog,^05 but you didn't want\n  to disturb its nap.)" }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
			}
			else
			{
				GameObject.Find("Doghouse").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (It's an abandoned dog house.)" }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 106 && (((int)gm.GetFlag(13) >= 10 && (int)gm.GetFlag(12) == 1) || (int)gm.GetFlag(12) == 0))
		{
			Object.Destroy(GameObject.Find("OblitNotif"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 108 && (int)gm.GetFlag(275) == 0)
		{
			Object.Destroy(GameObject.Find("JerrySword"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 112 && gm.GetFlagInt(288) == 1)
		{
			GameObject.Find("Bunny").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* Wow...", "* I got to meet a human!", "* ... what's a human, exactly?" }, new string[2] { "snd_text", "snd_text" }, new int[4], new string[2] { "", "" });
		}
		if (SceneManager.GetActiveScene().buildIndex == 115)
		{
			if (gm.GetFlagInt(185) == 1)
			{
				GameObject.Find("Doggo").GetComponent<InteractTextBox>().ModifyContents(new string[6] { "* ...", "* Why do the three of you\n  smell familiar?????", "* I've never even met you\n  before??!", "* A-^05-a^05-and why am I shaking?!?!?", "* Bring a jacket next\n  time,^05 buddy.", "* B-^05but it isn't cold...!" }, new string[6] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_text" }, new int[4], new string[6] { "", "", "", "", "su_side", "" });
				GameObject.Find("Doggo").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[5] { "* (Does...^05 does he know?)", "* (Nah,^05 he would've tried\n  killing us by now.)", "* What are you guys doing?^05\n* I can still smell you.", "* Just thinkin'.", "* Oh,^05 my bad,^05 carry on." }, new string[5] { "snd_txtnoe", "snd_txtsus", "snd_text", "snd_txtsus", "snd_text" }, new int[4], new string[5] { "no_depressed_side", "su_smirk_sweat", "", "su_dejected", "" });
			}
			if (gm.GetFlagInt(241) == 1)
			{
				GameObject.Find("Dogaressa").GetComponent<InteractTextBox>().EnableSecondaryLines();
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 113 && gm.GetFlagInt(87) == 10 && gm.GetFlagInt(281) == 1 && (gm.GetFlagInt(270) == 0 || gm.GetFlagInt(270) == 1))
		{
			GameObject.Find("Charles").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[3] { "* I've gotta say,^05 though.^05\n* There are more moles out today\n  than I thought there were here.", "* There's one that warned me\n  about a dangerous atmosphere\n  around here.", "* But it seems fine around here?^05\n* No idea what that was\n  about." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			if (gm.GetFlagInt(305) == 1)
			{
				GameObject.Find("Charles").GetComponent<InteractTextBox>().ModifySecondaryContents(new string[7] { "* I've gotta say,^05 though.^05\n* There are more moles out today\n  than I thought there were here.", "* There's one that warned me\n  about a dangerous atmosphere\n  around here.", "* And I,^05 umm...^05\n* I'm starting to get what\n  they were saying.", "* I feel really anxious for no\n  reason...", "* Does it have anything to\n  do with the readings...?", "* ... Readings?", "* ... Oop,^05 you wouldn't know\n  anything about that.^05\n* My bad!" }, new string[7] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtnoe", "snd_text" }, new int[1], new string[7] { "", "", "", "", "", "no_thinking", "" });
			}
		}
		if (SceneManager.GetActiveScene().buildIndex == 111 && gm.GetFlagInt(87) == 10 && gm.GetFlagInt(281) == 1 && (gm.GetFlagInt(270) == 0 || gm.GetFlagInt(270) == 1))
		{
			Object.Destroy(GameObject.Find("SemiHypercube"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 114 && gm.GetFlagInt(87) == 10 && gm.GetFlagInt(281) == 1 && (gm.GetFlagInt(270) == 0 || gm.GetFlagInt(270) == 1))
		{
			Object.Destroy(GameObject.Find("Goku"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 116 && gm.GetFlagInt(87) == 10 && gm.GetFlagInt(281) == 1 && (gm.GetFlagInt(270) == 0 || gm.GetFlagInt(270) == 1))
		{
			Object.Destroy(GameObject.Find("Goku"));
		}
		if (SceneManager.GetActiveScene().buildIndex == 127)
		{
			Util.GameManager().StopMusic(60f);
			Object.FindObjectOfType<LoadingZone>().SetForceActivationTrigger(true);
		}
	}
}
