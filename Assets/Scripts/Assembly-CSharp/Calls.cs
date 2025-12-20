using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Calls : MonoBehaviour
{
	public enum ID
	{
		TorielDR = 0,
		TorielUT = 1
	}

	private static string[] callerNames = new string[2] { "Call Home", "Otherworldly Mom" };

	public static string GetCallerName(ID caller)
	{
		if (Util.GameManager().GetFlagInt(108) == 1 && caller == ID.TorielUT)
		{
			return "Call Toriel";
		}
		return callerNames[(int)caller];
	}

	public static void CallCharacter(ID caller, TextBox txt = null, int txtPos = 0)
	{
		switch (caller)
		{
		case ID.TorielDR:
			CallToriel(txt, txtPos);
			break;
		case ID.TorielUT:
			CallUTToriel(txt, txtPos);
			break;
		}
	}

	public static Tuple<int[], int> GetCallerList()
	{
		GameManager gameManager = Util.GameManager();
		int[] array = new int[6] { -1, -1, -1, -1, -1, -1 };
		int num = 0;
		if (gameManager.GetFlagInt(108) == 0)
		{
			array[num] = 0;
			num++;
		}
		if ((int)gameManager.GetFlag(8) == 1)
		{
			array[num] = 1;
			num++;
		}
		return new Tuple<int[], int>(array, num);
	}

	private static void CallToriel(TextBox txt, int txtPos)
	{
		GameManager gameManager = Util.GameManager();
		gameManager.PlayGlobalSFX("sounds/snd_dial");
		List<string> list = new List<string>();
		int num = (int)gameManager.GetFlag(84);
		string arg = num.ToString();
		bool flag = false;
		if ((int)gameManager.GetFlag(200) == 0 && num >= 5 && num != 6)
		{
			gameManager.SetFlag(200, 1);
			arg = "5";
			if (num == 5)
			{
				gameManager.SetFlag(84, 7);
			}
			else
			{
				flag = true;
			}
		}
		list.Add("* Dialing...");
		for (int i = 0; Localizer.HasText($"phone_home_{arg}_{i}"); i++)
		{
			list.Add(Localizer.GetText($"phone_home_{arg}_{i}"));
		}
		if (flag)
		{
			list[7] = "torid_worry`snd_txttor`* ...^10 Please stay safe.^05\n* Call me back soon,^05\n  too...";
		}
		if (list.Count == 1)
		{
			list.Add("* ...");
			list.Add("* No one picked up.");
		}
		string[] array = new string[list.Count];
		string[] array2 = new string[list.Count];
		string[] array3 = new string[list.Count];
		int[] array4 = new int[list.Count];
		for (int i = 0; i < list.Count; i++)
		{
			string[] array5 = list[i].Split('`');
			if (array5.Length > 1)
			{
				array2[i] = array5[0];
				if (array5[^2].StartsWith("snd"))
				{
					array3[i] = array5[^2];
				}
				else
				{
					array3[i] = "snd_text";
				}
			}
			else
			{
				array2[i] = "";
				array3[i] = "snd_text";
			}
			array[i] = array5[^1];
			array4[i] = 0;
		}
		txt.CreateBox(array, array3, array4, txtPos, giveBackControl: true, array2);
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			txt.GetUIBox().GetComponent<FrostedBox>().ActivateCellIcons();
		}
	}

	private static void CallUTToriel(TextBox txt, int txtPos)
	{
		bool num = Util.GameManager().GetFlagInt(108) == 1;
		Util.GameManager().PlayGlobalSFX("sounds/snd_dial");
		List<string> list = new List<string>();
		int num2 = SceneManager.GetActiveScene().buildIndex;
		int num3 = 0;
		list.Add("* Dialing...");
		if (num)
		{
			if (Util.OverworldPlayer().cellphoneCall && (Localizer.HasText($"phone_toriel_{num2}_1_0") || Localizer.HasText($"phone_toriel_{num2}_1_0_h")))
			{
				num3 = 1;
			}
			list.Add("* Dialing...");
			bool flag = false;
			if (!flag && Localizer.HasText($"phone_toriel_{num2}_{num3}_0_h"))
			{
				flag = true;
			}
			string text = (flag ? "_h" : "");
			int num4 = 0;
			while (Localizer.HasText($"phone_toriel_{num2}_{num3}_{num4}{text}") && (int)Util.GameManager().GetFlag(53) == 0)
			{
				list.Add(Localizer.GetText($"phone_toriel_{num2}_{num3}_{num4}{text}"));
				num4++;
			}
		}
		else
		{
			if ((int)Util.GameManager().GetFlag(13) >= 2 && num2 < 21)
			{
				num2 = 0;
			}
			if (Util.OverworldPlayer().cellphoneCall && Localizer.HasText($"phone_toriel_{num2}_1_0"))
			{
				num3 = 1;
			}
			for (int i = 0; Localizer.HasText($"phone_toriel_{num2}_{num3}_{i}"); i++)
			{
				if ((int)Util.GameManager().GetFlag(53) != 0)
				{
					break;
				}
				list.Add(Localizer.GetText($"phone_toriel_{num2}_{num3}_{i}"));
			}
		}
		if (list.Count == 1)
		{
			list.Add("* ...");
			list.Add("* No one picked up.");
		}
		string[] array = new string[list.Count];
		string[] array2 = new string[list.Count];
		string[] array3 = new string[list.Count];
		int[] array4 = new int[list.Count];
		for (int j = 0; j < list.Count; j++)
		{
			string[] array5 = list[j].Split('`');
			if (array5.Length > 1)
			{
				array2[j] = array5[0];
				if (array5[^2].StartsWith("snd"))
				{
					array3[j] = array5[^2];
				}
				else
				{
					array3[j] = "snd_text";
				}
			}
			else
			{
				array2[j] = "";
				array3[j] = "snd_text";
			}
			array[j] = array5[^1];
			array4[j] = 0;
		}
		txt.CreateBox(array, array3, array4, txtPos, giveBackControl: true, array2);
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			txt.GetUIBox().GetComponent<FrostedBox>().ActivateCellIcons();
		}
		Util.OverworldPlayer().cellphoneCall = true;
	}
}
