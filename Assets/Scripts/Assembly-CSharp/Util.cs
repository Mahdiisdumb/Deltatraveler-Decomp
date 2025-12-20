using System;
using System.Text.RegularExpressions;
using UnityEngine;

public static class Util
{
	private static OverworldPlayer player;

	public static GameManager GameManager()
	{
		if ((bool)global::GameManager.instance)
		{
			return global::GameManager.instance;
		}
		return UnityEngine.Object.FindFirstObjectByType<GameManager>();
	}

	public static OverworldPlayer OverworldPlayer()
	{
		if ((bool)player)
		{
			return player;
		}
		player = UnityEngine.Object.FindFirstObjectByType<OverworldPlayer>();
		return player;
	}

	public static T FindObjectOfType<T>() where T : UnityEngine.Object
	{
		return UnityEngine.Object.FindFirstObjectByType<T>();
	}

	public static T[] FindObjectsOfType<T>() where T : UnityEngine.Object
	{
		return UnityEngine.Object.FindObjectsByType<T>(FindObjectsSortMode.None);
	}

	public static string Unescape(string str)
	{
		try
		{
			string text = "";
			for (int i = 0; i < str.Length; i++)
			{
				text += ((i > 0 && str[i - 1] == '\\') ? char.ToLower(str[i]) : str[i]);
			}
			return Regex.Unescape(text);
		}
		catch (ArgumentException ex)
		{
			Debug.LogWarning(ex);
			return (ex is ArgumentNullException) ? "* [NULL_STRING]" : "* [INVALID_ESCAPE]";
		}
		catch (IndexOutOfRangeException message)
		{
			Debug.LogWarning(message);
			return "* [INVALID_STRING]";
		}
	}
}
