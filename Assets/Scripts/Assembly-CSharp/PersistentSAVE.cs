using System.Collections.Generic;
using System.IO;
using Microsoft.Win32;
using UnityEngine;

public class PersistentSAVE
{
	public static readonly string DEFAULT_CATEGORY = "DELTATRAVELER";

	public static readonly string TITLE_CATEGORY = "Title";

	public static readonly string GAME_CATEGORY = "Game";

	public static readonly string MINIGAME_CATEGORY = "Mini-Game";

	public static readonly string ENDING_CATEGORY = "Ending";

	public static readonly string NOTIF_CATEGORY = "Notifications";

	private static readonly string CONFIG_NAME = "deltatraveler.ini";

	private static readonly Dictionary<string, string> CATEGORIES = new Dictionary<string, string>
	{
		{ "kris-eye-title", TITLE_CATEGORY },
		{ "uno-personal-best", MINIGAME_CATEGORY },
		{ "completion", GAME_CATEGORY },
		{ "jerry", GAME_CATEGORY },
		{ "last-flowey-section", GAME_CATEGORY },
		{ "gaster-s3", GAME_CATEGORY },
		{ "flowey-iteration", GAME_CATEGORY },
		{ "hardmode-completion", ENDING_CATEGORY },
		{ "content-warning", GAME_CATEGORY },
		{ "flowey-killed-last-time", GAME_CATEGORY },
		{ "low-graphics-warning", GAME_CATEGORY },
		{ "mario-unlocked", MINIGAME_CATEGORY },
		{ "mario-score", MINIGAME_CATEGORY },
		{ "new-title", TITLE_CATEGORY },
		{ "hardmode-reminder", GAME_CATEGORY },
		{ "new-input", GAME_CATEGORY },
		{ "fullscreen", GAME_CATEGORY },
		{ "window-scale", GAME_CATEGORY },
		{ "shayy-cool-s3", GAME_CATEGORY },
		{ "mario-phase", MINIGAME_CATEGORY },
		{ "last-saved-pm-0", TITLE_CATEGORY },
		{ "last-saved-pm-1", TITLE_CATEGORY },
		{ "last-saved-pm-2", TITLE_CATEGORY },
		{ "last-saved-pm-3", TITLE_CATEGORY },
		{ "last-saved-pm-4", TITLE_CATEGORY },
		{ "last-saved-pm-5", TITLE_CATEGORY },
		{ "uno-papyrus-hardmode-win", MINIGAME_CATEGORY },
		{ "flavor-notification", NOTIF_CATEGORY },
		{ "uno-notification", NOTIF_CATEGORY },
		{ "mario-notification", NOTIF_CATEGORY },
		{ "arena-notification", NOTIF_CATEGORY },
		{ "new-extra", GAME_CATEGORY },
		{ "seen-credits", ENDING_CATEGORY },
		{ "seen-hardmode-credits", ENDING_CATEGORY }
	};

	private static Config config;

	public static void Load()
	{
		config = new Config(CONFIG_NAME);
		if (GetInt("converted", -1) == -1)
		{
			string[] oldConfigNames = new string[20]
			{
				"KrisEye", "UnoPersonalBest", "CompletionState", "JerryDefeated", "LastFloweySection", "GasterSection3", "FloweyIteration", "HardmodeCompletion", "ContentWarningV2", "FloweyKilledLastTime",
				"AutoLowGraphicsWarning", "MBUnlocked", "MBScore", "NewTitleScreen", "HardModeReminder", "NewInput", "fullscreen", "WindowScale", "ShayyCoolS3", "MBPhase"
			};
			string[] newConfigNames = new string[20]
			{
				"kris-eye-title", "uno-personal-best", "completion", "jerry", "last-flowey-section", "gaster-s3", "flowey-iteration", "hardmode-completion", "content-warning", "flowey-killed-last-time",
				"low-graphics-warning", "mario-unlocked", "mario-score", "new-title", "hardmode-reminder", "new-input", "fullscreen", "window-scale", "shayy-cool-s3", "mario-phase"
			};
			bool converted = false;
			ConvertOldFiles(oldConfigNames, newConfigNames, ref converted);
			if (converted)
			{
				SetInt("converted", 1);
			}
			else
			{
				SetInt("converted", 0);
			}
		}
	}

	private static void ConvertOldFiles(string[] oldConfigNames, string[] newConfigNames, ref bool converted)
	{
		string value = "This file is here to tell the game to NOT copy over files from the old SAVE directory in case you choose to delete \"deltatraveler.ini\", since I know some people would want to do a manual true reset.\n\nIf you care about your files in the new VyletBunni DELTATRAVELER directory, then do not delete, rename, or move this file without deleting this folder.  If you lose your new files this way, then it is only your fault.";
		string text = Path.Combine(Application.persistentDataPath, "../../RynoGG/DELTATRAVELER");
		if (!Directory.Exists(text) || File.Exists(Path.Combine(text, "copy_lock.txt")))
		{
			return;
		}
		Debug.Log("RynoGG spotted");
		string[] array = new string[5] { "config.ini", "SAVE0.sav", "SAVE1.sav", "SAVE2.sav", "SAVE3.sav" };
		foreach (string path in array)
		{
			if (File.Exists(Path.Combine(text, path)))
			{
				File.Copy(Path.Combine(text, path), Path.Combine(Application.persistentDataPath, path), overwrite: true);
			}
		}
		List<string> list = new List<string>(oldConfigNames);
		RegistryKey registryKey = Registry.CurrentUser.OpenSubKey("Software").OpenSubKey("RynoGG").OpenSubKey("DELTATRAVELER");
		array = registryKey.GetValueNames();
		foreach (string text2 in array)
		{
			string text3 = text2.Split('_')[0];
			Debug.Log(text3);
			if (list.Contains(text3))
			{
				int num = list.IndexOf(text3);
				SetInt(newConfigNames[num], (int)registryKey.GetValue(text2));
			}
		}
		SetInt("last-saved-pm-0", 0);
		SetInt("last-saved-pm-1", 1);
		SetInt("last-saved-pm-2", 2);
		StreamWriter streamWriter = File.CreateText(Path.Combine(text, "copy_lock.txt"));
		streamWriter.Write(value);
		streamWriter.Close();
		converted = true;
	}

	public static void SetInt(string key, int value, string category = "DELTATRAVELER")
	{
		if (category == DEFAULT_CATEGORY && CATEGORIES.ContainsKey(key))
		{
			category = CATEGORIES[key];
		}
		config.SetInt(category, key, value);
		config.Write();
	}

	public static void SetString(string key, string value, string category = "DELTATRAVELER")
	{
		if (category == DEFAULT_CATEGORY && CATEGORIES.ContainsKey(key))
		{
			category = CATEGORIES[key];
		}
		config.SetString(category, key, value);
		config.Write();
	}

	public static int GetInt(string key, int defaultValue, string category = "DELTATRAVELER")
	{
		if (category == DEFAULT_CATEGORY && CATEGORIES.ContainsKey(key))
		{
			category = CATEGORIES[key];
		}
		return config.GetInt(category, key, defaultValue);
	}

	public static string GetString(string key, string defaultValue, string category = "DELTATRAVELER")
	{
		if (category == DEFAULT_CATEGORY && CATEGORIES.ContainsKey(key))
		{
			category = CATEGORIES[key];
		}
		return config.GetString(category, key, defaultValue);
	}

	public static bool HasKey(string key, string category = "DELTATRAVELER")
	{
		if (category == DEFAULT_CATEGORY && CATEGORIES.ContainsKey(key))
		{
			category = CATEGORIES[key];
		}
		return config.HasKey(category, key);
	}

	public static void RemoveKey(string key, string category = "DELTATRAVELER")
	{
		if (category == DEFAULT_CATEGORY && CATEGORIES.ContainsKey(key))
		{
			category = CATEGORIES[key];
		}
		config.RemoveKey(category, key);
		config.Write();
	}
}
