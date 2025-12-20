using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MonitorInfo : MonoBehaviour
{
	private Text text;

	private void Awake()
	{
		text = GetComponent<Text>();
	}

	private void LateUpdate()
	{
		text.text = string.Format("Platform: {0}\nvSync Setting: {1}\nTarget FPS: {2}\nvSync Count (Calculated): {3}\nMonitor Information:\n", Application.platform.ToString(), (GameManager.GetOptions().vSync.value == 1) ? "ON" : "OFF", Util.GameManager().GetFramerate(), QualitySettings.vSyncCount);
		List<DisplayInfo> list = new List<DisplayInfo>(Util.GameManager().GetDisplayInfo());
		list.Add(Screen.mainWindowDisplayInfo);
		for (int i = 0; i < list.Count; i++)
		{
			string format = "- MONITOR {0} Refresh Rate: {1} ({2} rounded)\n";
			if (i == list.Count - 1)
			{
				format = "- \"MAIN MONITOR\" Refresh Rate: {1} ({2} rounded)\n";
			}
			text.text += string.Format(format, i, list[i].refreshRate.value, Mathf.RoundToInt((float)list[i].refreshRate.value));
		}
	}
}
