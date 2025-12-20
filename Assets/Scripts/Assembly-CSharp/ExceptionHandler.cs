using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;

public static class ExceptionHandler
{
	private static string logFolder = "";

	public static string cond;

	public static string stack;

	public static void Init()
	{
		logFolder = Path.Combine(Directory.GetParent(Application.dataPath).ToString(), "logs/");
		Reset();
	}

	public static void Reset()
	{
		cond = null;
		stack = null;
		Application.logMessageReceived += HandleException;
	}

	private static void HandleException(string condition, string stackTrace, LogType type)
	{
		if (type != LogType.Exception)
		{
			return;
		}
		if (logFolder != "")
		{
			Scene activeScene = SceneManager.GetActiveScene();
			string text = DateTime.Now.ToString().Replace('/', '-').Replace(' ', '-')
				.Replace(':', '-');
			string text2 = "DELTATRAVELER Exception Log\n\n--- EXCEPTION ---\n" + condition + "\n\nStack Trace: \n" + stackTrace + "\n--- MISC ---\nLast Scene: " + activeScene.name + " (" + activeScene.buildIndex + ")\n" + GetExtraDebugInfo();
			Debug.Log(text2);
			try
			{
				string path = logFolder + "crash-" + text + ".txt";
				if (!Directory.Exists(logFolder))
				{
					Directory.CreateDirectory(logFolder);
				}
				if (File.Exists(path))
				{
					Debug.Log("Crash log " + text + " already exists");
					return;
				}
				File.WriteAllText(path, text2);
			}
			catch (Exception message)
			{
				Debug.Log("Failed to save exception log... :(");
				Debug.Log(message);
			}
		}
		cond = condition;
		stack = stackTrace;
		SceneManager.LoadScene(1, LoadSceneMode.Single);
		Application.logMessageReceived -= HandleException;
	}

	public static string GetExtraDebugInfo()
	{
		try
		{
			Scene activeScene = SceneManager.GetActiveScene();
			string text = "";
			if (activeScene.buildIndex == 2)
			{
				BattleManager battleManager = Util.FindObjectOfType<BattleManager>();
				if ((bool)battleManager)
				{
					List<string> list = new List<string>();
					EnemyBase[] enemies = battleManager.GetEnemies();
					foreach (EnemyBase enemyBase in enemies)
					{
						list.Add(enemyBase.GetName());
					}
					text = text + "Battle ID: " + battleManager.GetBattleID() + " (" + string.Join(", ", list.ToArray()) + ")";
				}
			}
			CutsceneStart[] array = Util.FindObjectsOfType<CutsceneStart>();
			foreach (CutsceneStart cutsceneStart in array)
			{
				if ((bool)cutsceneStart && (bool)cutsceneStart.GetCutscene() && cutsceneStart.GetCutscene().IsPlaying())
				{
					text = text + "Active Cutscene: " + CutsceneHandler.GetCutscene(cutsceneStart.GetCutsceneID()).ToString() + "(" + cutsceneStart.GetCutsceneID() + ")";
				}
			}
			return text;
		}
		catch (Exception)
		{
			return "";
		}
	}
}
