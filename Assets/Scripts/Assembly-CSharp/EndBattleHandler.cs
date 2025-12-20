using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndBattleHandler : Object
{
	public static void DoEndBattle(int battleId, int endState)
	{
		int num = (int)Util.GameManager().GetFlag(13);
		Util.FindObjectOfType<InteractionTrigger>().GetComponent<BoxCollider2D>().enabled = true;
		for (int i = 0; i < 3; i++)
		{
			if (Util.GameManager().GetHP(i) <= 0)
			{
				Util.GameManager().SetHP(i, 5);
			}
		}
		int stateFlag = EnemyGenerator.GetStateFlag(battleId);
		if (stateFlag > -1)
		{
			Util.GameManager().SetFlag(stateFlag, endState);
		}
		int endCutscene = EnemyGenerator.GetEndCutscene(battleId);
		if (endCutscene > -1)
		{
			CutsceneHandler.GetCutscene(endCutscene).StartCutscene(endState);
		}
		if (stateFlag == -1 && endCutscene == -1)
		{
			int num2 = -1;
			OverworldEnemyBase[] array = Util.FindObjectsOfType<OverworldEnemyBase>();
			foreach (OverworldEnemyBase overworldEnemyBase in array)
			{
				if (overworldEnemyBase.GetBattleID() != battleId || !overworldEnemyBase.IsDisabled() || overworldEnemyBase.IsHandled())
				{
					continue;
				}
				num2 = overworldEnemyBase.GetDefeatFlagID();
				Util.GameManager().SetFlag(overworldEnemyBase.GetDefeatFlagID(), endState);
				Util.GameManager().SetFlag(overworldEnemyBase.GetCounterFlagID(), overworldEnemyBase.GetCounter() + 1);
				if (endState != 1 && (int)Util.GameManager().GetFlag(12) == 1)
				{
					Util.GameManager().SetFlag(12, 0);
					if ((int)Util.GameManager().GetFlag(13) >= 1)
					{
						WeirdChecker.Abort(Util.GameManager());
						Util.OverworldPlayer().GetPartyMemberByID(1).UseHappySprites();
					}
				}
				if (overworldEnemyBase.GetCounter() == overworldEnemyBase.GetKillExhaustCount() && (int)Util.GameManager().GetFlag(12) == 1)
				{
					if ((int)Util.GameManager().GetFlag(13) == 0)
					{
						WeirdChecker.AdvanceTo(Util.GameManager(), 1, sound: false);
					}
					Util.GameManager().PlayGlobalSFX("sounds/snd_ominous");
					if (SceneManager.GetActiveScene().buildIndex == 20 && (bool)GameObject.Find("RockThrown(Clone)"))
					{
						GameObject.Find("RockThrown(Clone)").GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* ...", "* You two are twisted,^10\n  y'know that?", "* (What...?)" }, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[3], new string[3] { "", "", "su_side_sweat" });
					}
					if ((bool)overworldEnemyBase.GetComponent<OverworldBloodEnemyBase>() && (int)Util.GameManager().GetFlag(13) >= 4)
					{
						overworldEnemyBase.GetComponent<OverworldBloodEnemyBase>().CreateDeadEnemy();
					}
					if (SceneManager.GetActiveScene().buildIndex < 50 && (int)Util.GameManager().GetFlag(120) == 0 && WeirdChecker.GetExhaustedEncounterCount(Util.GameManager(), WeirdChecker.ruinsCombos) >= 3)
					{
						Util.GameManager().SetFlag(120, 1);
						TextBox component = new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>();
						List<string> list = new List<string> { "* ... Kris?", "* Not that I mind\n  or anything, but like...", "* Okay,^05 I do mind.\n^10* What the hell are\n  we doing.", "* Why are we like...\n^05  hunting down enemies?", "* Is this place freaking\n  you out or something?", "* Honestly,^05 I think you\n  should just ignore them.", "* They aren't a good\n  use of time,^05 y'know." };
						if ((int)Util.GameManager().GetFlag(108) == 1)
						{
							list[0] = "* ... uh...^05 hey.";
							list.Add("* ... What the hell\n  is that look\n  supposed to mean?");
							list.Add("* Were you even paying\n  attention to me?");
						}
						component.CreateBox(list.ToArray(), new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], 1, giveBackControl: true, new string[9] { "su_neutral", "su_side", "su_annoyed", "su_annoyed", "su_dejected", "su_side", "su_smirk", "su_annoyed", "su_annoyed" });
						Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
					}
					if (SceneManager.GetActiveScene().buildIndex == 53 && (int)Util.GameManager().GetFlag(90) == 0)
					{
						Util.GameManager().SetFlag(90, 1);
						new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[7] { "* H-^05hey,^05 is there a reason\n  why we keep defeating\n  the same enemies...?", "* I'm just assuming that\n  Kris knows what's going\n  on up ahead.", "* Not that I think it's\n  a good idea,^05 but an\n  excuse is an excuse.", "* Kris...^10\n* Are you sure this is\n  necessary?", "* M-maybe we can give them\n  a hand next time...", "* O-or do literally\n  anything else...", "* This might not even\n  be a good use of\n  time." }, new string[7] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[18], 1, giveBackControl: true, new string[7] { "no_thinking", "su_annoyed", "su_dejected", "no_confused", "no_happy", "no_thinking", "no_thinking" });
						Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
					}
				}
				if (overworldEnemyBase.GetCounter() == overworldEnemyBase.GetKillExhaustCount() && (int)Util.GameManager().GetFlag(95) == 1)
				{
					Util.GameManager().SetFlag(95, 0);
					Util.GameManager().PlayGlobalSFX("sounds/snd_ominous_cancel");
					TextBox component2 = new GameObject("DamnBroYouCool", typeof(TextBox)).GetComponent<TextBox>();
					if ((int)Util.GameManager().GetFlag(108) == 1)
					{
						component2.CreateBox(new string[1] { "* Felt as though things won't\n  escalate." });
					}
					else
					{
						component2.CreateBox(new string[3] { "* (You thought about the path\n  that you're on.)", "* (You realized that <color=#FFFF00FF>things won't\n  escalate, as you left\n  Napstablook alone</color>.)", "* (You let out a sigh of\n  relief.)" });
					}
					Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
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
						Util.FindObjectOfType<SectionTitleCard>().Activate();
						Util.OverworldPlayer().SetSelfAnimControl(setAnimControl: true);
						OverworldPartyMember[] array2 = Util.FindObjectsOfType<OverworldPartyMember>();
						for (int k = 0; k < array2.Length; k++)
						{
							array2[k].SetSelfAnimControl(setAnimControl: true);
						}
						Util.FindObjectOfType<CameraController>().SetFollowPlayer(follow: true);
						if ((int)Util.GameManager().GetFlag(12) == 0)
						{
							Util.OverworldPlayer().GetPartyMemberByID(1).UseHappySprites();
						}
						if ((int)Util.GameManager().GetFlag(87) == 0)
						{
							Util.OverworldPlayer().GetPartyMemberByID(2).UseHappySprites();
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
				array = Util.FindObjectsOfType<OverworldEnemyBase>();
				foreach (OverworldEnemyBase overworldEnemyBase2 in array)
				{
					if (overworldEnemyBase2.GetDefeatFlagID() == num2 && !overworldEnemyBase2.IsHandled())
					{
						overworldEnemyBase2.ActivateHandled();
						if ((bool)overworldEnemyBase2.GetComponent<OverworldBloodEnemyBase>() && (int)Util.GameManager().GetFlag(13) >= 4 && endState == 1)
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
		if (SceneManager.GetActiveScene().buildIndex < 30 && num == 2 && WeirdChecker.GetWeirdAreaProgress(Util.GameManager(), "mus_ruins") == 2)
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
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			Util.GameManager().PlayMusic("zoneMusic");
		}
		if (SceneManager.GetActiveScene().buildIndex == 53 && num == 3 && WeirdChecker.GetWeirdAreaProgress(Util.GameManager(), "mus_pr_valley") == 2)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[4] { "* (...)", "* (The emptiness of the running\n  river fills you with dread.)", "* (This power...)", "* (You felt strong enough to\n  draw blood.)" });
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			Util.GameManager().PlayMusic("zoneMusic");
		}
		if ((SceneManager.GetActiveScene().buildIndex == 70 || SceneManager.GetActiveScene().buildIndex == 57) && num == 5 && WeirdChecker.GetWeirdAreaProgress(Util.GameManager(), "mus_cave") == 2)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[4] { "* (...)", "* (You didn't feel anything.)", "* (It's all too numbing.)", "* (You hope deep in your heart\n  that it doesn't get any\n  worse.)" });
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			Util.GameManager().PlayMusic("zoneMusic");
		}
		if (SceneManager.GetActiveScene().buildIndex >= 72 && SceneManager.GetActiveScene().buildIndex <= 86 && num == 8 && WeirdChecker.GetWeirdAreaProgress(Util.GameManager(), "mus_snowy") == 1)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[2] { "* (...)", "* (The cycle continues.)" });
			if ((bool)Util.FindObjectOfType<Snowball>())
			{
				Object.Destroy(Util.FindObjectOfType<Snowball>().gameObject);
			}
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			Util.GameManager().PlayMusic("zoneMusic");
		}
		if (SceneManager.GetActiveScene().buildIndex >= 87 && SceneManager.GetActiveScene().buildIndex <= 103 && num == 9 && WeirdChecker.GetWeirdAreaProgress(Util.GameManager(), "mus_snowy") == 2 && endCutscene == -1)
		{
			new GameObject("DamnBroYouSuck", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[2]
			{
				"* (...)",
				GetSnowdinSecondHalfString()
			});
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			Util.GameManager().PlayMusic("zoneMusic");
		}
	}

	public static string GetSnowdinSecondHalfString()
	{
		if (WeirdChecker.GetWeirdAreaProgress(Util.GameManager(), "mus_snowy") == 2)
		{
			return "* (It draws silent once again.)";
		}
		return "";
	}

	public static object GetFlagFromId(int battleId)
	{
		return Util.GameManager().GetFlag(EnemyGenerator.GetStateFlag(battleId));
	}
}
