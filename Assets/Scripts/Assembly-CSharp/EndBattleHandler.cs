using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleHandler : Object
{
	private static Dictionary<int, int> IDToFlag = new Dictionary<int, int>
	{
		{ 0, 3 },
		{ 2, 6 },
		{ 8, 127 },
		{ 14, 58 },
		{ 23, 89 },
		{ 24, 97 },
		{ 26, 106 },
		{ 27, 109 },
		{ 28, 116 },
		{ 40, 58 },
		{ 41, 124 },
		{ 51, 150 },
		{ 52, 154 },
		{ 53, 173 },
		{ 54, 173 },
		{ 57, 185 },
		{ 59, 270 },
		{ 62, 205 },
		{ 65, 209 },
		{ 66, 241 },
		{ 68, 253 },
		{ 70, 270 },
		{ 72, 245 },
		{ 73, 281 }
	};

	private static Dictionary<int, int> IDToCutsceneID = new Dictionary<int, int>
	{
		{ 0, 0 },
		{ 1, 3 },
		{ 2, 6 },
		{ 8, 12 },
		{ 14, 21 },
		{ 23, 32 },
		{ 24, 34 },
		{ 26, 37 },
		{ 27, 52 },
		{ 28, 40 },
		{ 29, 44 },
		{ 40, 46 },
		{ 51, 49 },
		{ 52, 53 },
		{ 53, 56 },
		{ 54, 56 },
		{ 57, 62 },
		{ 59, 90 },
		{ 62, 70 },
		{ 65, 80 },
		{ 66, 86 },
		{ 68, 87 },
		{ 70, 90 },
		{ 72, 93 },
		{ 73, 97 },
		{ 75, 101 }
	};

	public static void DoEndBattle(int battleId, int endState)
	{
		int num = (int)Object.FindObjectOfType<GameManager>().GetFlag(13);
		Object.FindObjectOfType<InteractionTrigger>().GetComponent<BoxCollider2D>().enabled = true;
		for (int i = 0; i < 3; i++)
		{
			if (Object.FindObjectOfType<GameManager>().GetHP(i) <= 0)
			{
				Object.FindObjectOfType<GameManager>().SetHP(i, 5);
			}
		}
		if (IDToFlag.ContainsKey(battleId) && IDToFlag[battleId] > -1)
		{
			Object.FindObjectOfType<GameManager>().SetFlag(IDToFlag[battleId], endState);
		}
		if (IDToCutsceneID.ContainsKey(battleId))
		{
			CutsceneHandler.GetCutscene(IDToCutsceneID[battleId]).StartCutscene(endState);
		}
		if (!IDToCutsceneID.ContainsKey(battleId) && !IDToFlag.ContainsKey(battleId))
		{
			int num2 = -1;
			OverworldEnemyBase[] array = Object.FindObjectsOfType<OverworldEnemyBase>();
			foreach (OverworldEnemyBase overworldEnemyBase in array)
			{
				if (overworldEnemyBase.GetBattleID() != battleId || !overworldEnemyBase.IsDisabled() || overworldEnemyBase.IsHandled())
				{
					continue;
				}
				num2 = overworldEnemyBase.GetDefeatFlagID();
				Object.FindObjectOfType<GameManager>().SetFlag(overworldEnemyBase.GetDefeatFlagID(), endState);
				Object.FindObjectOfType<GameManager>().SetFlag(overworldEnemyBase.GetCounterFlagID(), overworldEnemyBase.GetCounter() + 1);
				if (endState != 1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(12) == 1)
				{
					Object.FindObjectOfType<GameManager>().SetFlag(12, 0);
					if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 1)
					{
						WeirdChecker.Abort(Object.FindObjectOfType<GameManager>());
						GameObject.Find("Susie").GetComponent<OverworldPartyMember>().UseHappySprites();
					}
				}
				if (overworldEnemyBase.GetCounter() == overworldEnemyBase.GetKillExhaustCount() && (int)Object.FindObjectOfType<GameManager>().GetFlag(12) == 1)
				{
					if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) == 0)
					{
						WeirdChecker.AdvanceTo(Object.FindObjectOfType<GameManager>(), 1, false);
					}
					Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_ominous");
					if (SceneManager.GetActiveScene().buildIndex == 20 && (bool)GameObject.Find("RockThrown(Clone)"))
					{
						GameObject.Find("RockThrown(Clone)").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* ...", "* You two are twisted,^10\n  y'know that?", "* (What...?)" }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[3], new string[3] { "", "", "su_side_sweat" });
					}
					if ((bool)overworldEnemyBase.GetComponent<OverworldBloodEnemyBase>() && (int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 4)
					{
						overworldEnemyBase.GetComponent<OverworldBloodEnemyBase>().CreateDeadEnemy();
					}
					if (SceneManager.GetActiveScene().buildIndex < 50 && (int)Util.GameManager().GetFlag(120) == 0 && WeirdChecker.GetExhaustedEncounterCount(Util.GameManager(), WeirdChecker.ruinsCombos) >= 3)
					{
						Object.FindObjectOfType<GameManager>().SetFlag(120, 1);
						TextBox component = new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>();
						List<string> list = new List<string> { "* ... Kris?", "* Not that I mind\n  or anything, but like...", "* Okay,^05 I do mind.\n^10* What the hell are\n  we doing.", "* Why are we like...\n^05  hunting down enemies?", "* Is this place freaking\n  you out or something?", "* Honestly,^05 I think you\n  should just ignore them.", "* They aren't a good\n  use of time,^05 y'know." };
						if ((int)Util.GameManager().GetFlag(108) == 1)
						{
							list[0] = "* ... uh...^05 hey.";
							list.Add("* ... What the hell\n  is that look\n  supposed to mean?");
							list.Add("* Were you even paying\n  attention to me?");
						}
						component.CreateBox(list.ToArray(), new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], 1, true, new string[9] { "su_neutral", "su_side", "su_annoyed", "su_annoyed", "su_dejected", "su_side", "su_smirk", "su_annoyed", "su_annoyed" });
						Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
					}
					if (SceneManager.GetActiveScene().buildIndex == 53 && (int)Object.FindObjectOfType<GameManager>().GetFlag(90) == 0)
					{
						Object.FindObjectOfType<GameManager>().SetFlag(90, 1);
						new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[7] { "* H-^05hey,^05 is there a reason\n  why we keep defeating\n  the same enemies...?", "* I'm just assuming that\n  Kris knows what's going\n  on up ahead.", "* Not that I think it's\n  a good idea,^05 but an\n  excuse is an excuse.", "* Kris...^10\n* Are you sure this is\n  necessary?", "* M-maybe we can give them\n  a hand next time...", "* O-or do literally\n  anything else...", "* This might not even\n  be a good use of\n  time." }, new string[7] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], 1, true, new string[7] { "no_thinking", "su_annoyed", "su_dejected", "no_confused", "no_happy", "no_thinking", "no_thinking" });
						Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
					}
				}
				if (overworldEnemyBase.GetCounter() == overworldEnemyBase.GetKillExhaustCount() && (int)Object.FindObjectOfType<GameManager>().GetFlag(95) == 1)
				{
					Object.FindObjectOfType<GameManager>().SetFlag(95, 0);
					Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_ominous_cancel");
					TextBox component2 = new GameObject("DamnBroYouCool", typeof(TextBox)).GetComponent<TextBox>();
					if ((int)Util.GameManager().GetFlag(108) == 1)
					{
						component2.CreateBox(new string[1] { "* Felt as though things won't\n  escalate." });
					}
					else
					{
						component2.CreateBox(new string[3] { "* (You thought about the path\n  that you're on.)", "* (You realized that <color=#FFFF00FF>things won't\n  escalate, as you left\n  Napstablook alone</color>.)", "* (You let out a sigh of\n  relief.)" });
					}
					Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
				}
				if (battleId == 56 && (int)Util.GameManager().GetFlag(180) == 0)
				{
					Util.GameManager().SetFlag(180, 1);
					Util.GameManager().PlayMusic("zoneMusic");
					Util.GameManager().SetCheckpoint(76);
					if (endState == 1)
					{
						CutsceneHandler.GetCutscene(58).StartCutscene(endState);
					}
					else
					{
						Object.FindObjectOfType<SectionTitleCard>().Activate();
						Object.FindObjectOfType<OverworldPlayer>().SetSelfAnimControl(true);
						OverworldPartyMember[] array2 = Object.FindObjectsOfType<OverworldPartyMember>();
						for (int k = 0; k < array2.Length; k++)
						{
							array2[k].SetSelfAnimControl(true);
						}
						Object.FindObjectOfType<CameraController>().SetFollowPlayer(true);
						if ((int)Util.GameManager().GetFlag(12) == 0)
						{
							GameObject.Find("Susie").GetComponent<OverworldPartyMember>().UseHappySprites();
						}
						if ((int)Util.GameManager().GetFlag(87) == 0)
						{
							GameObject.Find("Noelle").GetComponent<OverworldPartyMember>().UseHappySprites();
						}
					}
					Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(-1.79f, -1.61f), Quaternion.identity);
				}
				overworldEnemyBase.ActivateHandled();
				if (overworldEnemyBase.CanInstantlyRespawn() && endState == 2)
				{
					overworldEnemyBase.InstantSpareRespawn();
				}
				break;
			}
			if (num2 != -1)
			{
				array = Object.FindObjectsOfType<OverworldEnemyBase>();
				foreach (OverworldEnemyBase overworldEnemyBase2 in array)
				{
					if (overworldEnemyBase2.GetDefeatFlagID() == num2 && !overworldEnemyBase2.IsHandled())
					{
						overworldEnemyBase2.ActivateHandled();
						if ((bool)overworldEnemyBase2.GetComponent<OverworldBloodEnemyBase>() && (int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 4 && endState == 1)
						{
							overworldEnemyBase2.GetComponent<OverworldBloodEnemyBase>().CreateDeadEnemy();
						}
						if (overworldEnemyBase2.CanInstantlyRespawn() && endState == 2)
						{
							overworldEnemyBase2.InstantSpareRespawn();
						}
					}
					else if (!overworldEnemyBase2.IsHandled())
					{
						overworldEnemyBase2.Reactivate();
					}
				}
			}
		}
		if (SceneManager.GetActiveScene().buildIndex < 30 && num == 2 && WeirdChecker.GetWeirdAreaProgress(Object.FindObjectOfType<GameManager>(), "mus_ruins") == 2)
		{
			TextBox component3 = new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>();
			if ((int)Util.GameManager().GetFlag(108) == 1)
			{
				component3.CreateBox(new string[3] { "* (...)", "* (You stood above the dust in\n  the midst of deafening\n  silence.)", "* (You felt nothing.)" });
			}
			else
			{
				component3.CreateBox(new string[3] { "* (...)", "* (You can hear the wind blowing\n  amid the silence.)", "* (You feel the power in\n  your hands.)" });
			}
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			Object.FindObjectOfType<GameManager>().PlayMusic("zoneMusic");
		}
		if (SceneManager.GetActiveScene().buildIndex == 53 && num == 3 && WeirdChecker.GetWeirdAreaProgress(Object.FindObjectOfType<GameManager>(), "mus_pr_valley") == 2)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[4] { "* (...)", "* (The emptiness of the running\n  river fills you with dread.)", "* (This power...)", "* (You felt strong enough to\n  draw blood.)" });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			Object.FindObjectOfType<GameManager>().PlayMusic("zoneMusic");
		}
		if ((SceneManager.GetActiveScene().buildIndex == 70 || SceneManager.GetActiveScene().buildIndex == 57) && num == 5 && WeirdChecker.GetWeirdAreaProgress(Object.FindObjectOfType<GameManager>(), "mus_cave") == 2)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[4] { "* (...)", "* (You didn't feel anything.)", "* (It's all too numbing.)", "* (You hope deep in your heart\n  that it doesn't get any\n  worse.)" });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			Object.FindObjectOfType<GameManager>().PlayMusic("zoneMusic");
		}
		if (SceneManager.GetActiveScene().buildIndex >= 72 && SceneManager.GetActiveScene().buildIndex <= 86 && num == 8 && WeirdChecker.GetWeirdAreaProgress(Object.FindObjectOfType<GameManager>(), "mus_snowy") == 1)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[2] { "* (...)", "* (The cycle continues.)" });
			if ((bool)Object.FindObjectOfType<Snowball>())
			{
				Object.Destroy(Object.FindObjectOfType<Snowball>().gameObject);
			}
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			Object.FindObjectOfType<GameManager>().PlayMusic("zoneMusic");
		}
		if (SceneManager.GetActiveScene().buildIndex >= 87 && SceneManager.GetActiveScene().buildIndex <= 103 && num == 9 && WeirdChecker.GetWeirdAreaProgress(Object.FindObjectOfType<GameManager>(), "mus_snowy") == 2 && !IDToCutsceneID.ContainsKey(battleId))
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[2]
			{
				"* (...)",
				GetSnowdinSecondHalfString()
			});
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			Object.FindObjectOfType<GameManager>().PlayMusic("zoneMusic");
		}
	}

	public static string GetSnowdinSecondHalfString()
	{
		if (WeirdChecker.GetWeirdAreaProgress(Object.FindObjectOfType<GameManager>(), "mus_snowy") == 2)
		{
			return "* (It draws silent once again.)";
		}
		return "";
	}

	public static object GetFlagFromId(int battleId)
	{
		if (!IDToFlag.ContainsKey(battleId))
		{
			return 0;
		}
		if (IDToFlag[battleId] == -1)
		{
			return 0;
		}
		return Object.FindObjectOfType<GameManager>().GetFlag(IDToFlag[battleId]);
	}
}
