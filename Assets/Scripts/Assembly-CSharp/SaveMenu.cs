using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;
using UnityEngine.UI;

public class SaveMenu : UIComponent
{
	private int state;

	private int saveSlot = -1;

	private int overwriteSaveSlot;

	private SAVEFile[] saves;

	private bool[] saveSlotsTaken = new bool[4];

	private Transform mainbox;

	private Transform savefiles;

	private Transform overwrite;

	private Transform soul;

	private int index;

	private bool holdingAxis;

	private bool tsMode;

	private bool confirmQuit;

	private bool returnControl = true;

	private GameManager gm;

	private int tsExit;

	private Color borderColor = Color.white;

	private Color selectionColor = new Color(1f, 1f, 0f);

	private void Awake()
	{
		gm = Util.GameManager();
		borderColor = UIBackground.borderColors[(int)gm.GetFlag(223)];
		selectionColor = Selection.selectionColors[(int)gm.GetFlag(223)];
		Image[] componentsInChildren = GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			if (image.color == Color.white && image.gameObject.name != "TimeIcon")
			{
				image.color = borderColor;
			}
		}
		Text[] componentsInChildren2 = GetComponentsInChildren<Text>();
		foreach (Text text in componentsInChildren2)
		{
			if (text.color == new Color(1f, 1f, 0f))
			{
				text.color = selectionColor;
			}
		}
		tsMode = (int)gm.GetFlag(94) == 1;
		saveSlot = gm.GetFileID();
		mainbox = (tsMode ? base.transform.Find("MainBoxTS") : base.transform.Find("MainBox"));
		savefiles = base.transform.Find("SaveFiles");
		overwrite = base.transform.Find("Overwrite");
		soul = base.transform.Find("SOUL");
		soul.GetComponent<Image>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
		UpdateAllText();
		if (!tsMode)
		{
			return;
		}
		mainbox.localPosition = Vector3.zero;
		base.transform.Find("MainBox").localPosition = new Vector3(1000f, 0f);
		soul.localPosition = new Vector3(19f, 43f);
		for (int j = 0; j < 5; j++)
		{
			if (j != 3 && (bool)savefiles.GetChild(j).Find("TSBox"))
			{
				savefiles.GetChild(j).GetComponent<Image>().enabled = false;
				componentsInChildren = savefiles.GetChild(j).Find("TSBox").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = true;
				}
				componentsInChildren = savefiles.GetChild(j).Find("TSCorners").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = true;
				}
			}
		}
		overwrite.Find("Outline").GetComponent<Image>().enabled = false;
		componentsInChildren = overwrite.Find("TSBox").GetComponentsInChildren<Image>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		componentsInChildren = overwrite.Find("TSCorners").GetComponentsInChildren<Image>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
	}

	private void Update()
	{
		if (gm.IsTestMode() && Input.GetKey(KeyCode.F1))
		{
			gm.PlayGlobalSFX("sounds/snd_save");
			gm.SetFileID(3);
			gm.SaveFile(true);
			Object.Destroy(base.gameObject);
			return;
		}
		if (state <= 1)
		{
			string formattedUpdatedPlayTime = gm.GetFormattedUpdatedPlayTime();
			mainbox.Find("Time").GetComponent<Text>().text = formattedUpdatedPlayTime;
			savefiles.Find("CurBox").Find("Time").GetComponent<Text>()
				.text = formattedUpdatedPlayTime;
			if (tsMode)
			{
				mainbox.Find("RoomBox").transform.localPosition = Vector3.Lerp(mainbox.Find("RoomBox").transform.localPosition, new Vector3(0f, 40f), 0.2f);
			}
		}
		if (holdingAxis && UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f)
		{
			holdingAxis = false;
		}
		if (state == 0)
		{
			if (tsMode)
			{
				if (UTInput.GetAxis("Vertical") != 0f && !holdingAxis)
				{
					if (confirmQuit)
					{
						holdingAxis = true;
						confirmQuit = false;
						mainbox.Find(index.ToString()).GetComponent<Text>().text = "Quit Game";
						mainbox.Find(index.ToString()).GetComponent<Text>().color = Color.white;
					}
					else
					{
						index = (index - (int)UTInput.GetAxis("Vertical")) % 4;
						if (index < 0)
						{
							index = 3;
						}
						holdingAxis = true;
					}
				}
				soul.localPosition = new Vector3(19f, 43 - 30 * index);
			}
			else
			{
				if (UTInput.GetAxis("Horizontal") != 0f && !holdingAxis)
				{
					index = (index + (int)UTInput.GetAxis("Horizontal")) % 2;
					if (index < 0)
					{
						index = 1;
					}
					holdingAxis = true;
				}
				soul.localPosition = mainbox.Find(index.ToString()).localPosition + new Vector3(-19f, 96f);
			}
			if (UTInput.GetButtonDown("X") || UTInput.GetButtonDown("C") || (UTInput.GetButtonDown("Z") && index == 1))
			{
				if (confirmQuit)
				{
					confirmQuit = false;
					mainbox.Find(index.ToString()).GetComponent<Text>().text = "Quit Game";
					mainbox.Find(index.ToString()).GetComponent<Text>().color = Color.white;
				}
				else
				{
					Object.Destroy(base.gameObject);
				}
			}
			else if (index == 0 && UTInput.GetButtonDown("Z"))
			{
				gm.PlayGlobalSFX("sounds/snd_select");
				mainbox.transform.localPosition = new Vector3(1000f, 0f);
				savefiles.transform.localPosition = Vector3.zero;
				base.transform.Find("Background").GetComponent<Image>().enabled = true;
				index = ((saveSlot != 3) ? saveSlot : 0);
				state = 1;
				UpdateSaveFileColors();
			}
			else if (index >= 2 && UTInput.GetButtonDown("Z") && tsMode)
			{
				if (index == 3 && !confirmQuit)
				{
					confirmQuit = true;
					mainbox.Find(index.ToString()).GetComponent<Text>().text = "Really Quit?";
					mainbox.Find(index.ToString()).GetComponent<Text>().color = new Color32(byte.MaxValue, 76, 76, byte.MaxValue);
					return;
				}
				gm.StopMusic(15f);
				gm.PlayGlobalSFX("sounds/snd_select");
				mainbox.transform.localPosition = new Vector3(1000f, 0f);
				soul.transform.localPosition = new Vector3(1000f, 0f);
				state = 4;
				Object.FindObjectOfType<Fade>().FadeOut(15);
				tsExit = ((index == 3) ? 1 : 0);
			}
		}
		else if (state == 1)
		{
			if (UTInput.GetAxis("Vertical") != 0f && !holdingAxis)
			{
				index = (index - (int)UTInput.GetAxis("Vertical")) % 4;
				if (index < 0)
				{
					index = 3;
				}
				holdingAxis = true;
				UpdateSaveFileColors();
			}
			if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && index == 3))
			{
				gm.PlayGlobalSFX("sounds/snd_select");
				mainbox.transform.localPosition = Vector3.zero;
				savefiles.transform.localPosition = new Vector3(1000f, 0f);
				base.transform.Find("Background").GetComponent<Image>().enabled = false;
				index = 0;
				state = 0;
				if (tsMode)
				{
					soul.localPosition = new Vector3(19f, 43f);
				}
				else
				{
					soul.localPosition = mainbox.Find(index.ToString()).localPosition + new Vector3(-19f, 96f);
				}
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				overwriteSaveSlot = index;
				if (index == saveSlot || !saveSlotsTaken[index])
				{
					SaveFile();
					return;
				}
				index = 0;
				state = 2;
				LoadOverwrite();
				soul.localPosition = overwrite.Find(index.ToString()).localPosition + new Vector3(-20f, 16f);
			}
		}
		else if (state == 2)
		{
			if (UTInput.GetAxis("Horizontal") != 0f && !holdingAxis)
			{
				index = (index + (int)UTInput.GetAxis("Horizontal")) % 2;
				if (index < 0)
				{
					index = 1;
				}
				holdingAxis = true;
				overwrite.Find("0").GetComponent<Text>().color = ((index == 0) ? selectionColor : Color.white);
				overwrite.Find("1").GetComponent<Text>().color = ((index == 1) ? selectionColor : Color.white);
			}
			soul.localPosition = overwrite.Find(index.ToString()).localPosition + new Vector3(-20f, 16f);
			if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && index == 1))
			{
				overwrite.localPosition = new Vector3(1000f, 0f);
				state = 1;
				index = overwriteSaveSlot;
				Image[] componentsInChildren = savefiles.GetComponentsInChildren<Image>();
				foreach (Image image in componentsInChildren)
				{
					if (image.gameObject.name != "Background" && image.color != Color.black)
					{
						image.color = borderColor;
					}
				}
				UpdateSaveFileColors();
			}
			else
			{
				if (index != 0 || !UTInput.GetButtonDown("Z"))
				{
					return;
				}
				overwrite.localPosition = new Vector3(1000f, 0f);
				Image[] componentsInChildren = savefiles.GetComponentsInChildren<Image>();
				foreach (Image image2 in componentsInChildren)
				{
					if (image2.gameObject.name != "Background" && image2.color != Color.black)
					{
						image2.color = borderColor;
					}
				}
				UpdateSaveFileColors();
				SaveFile();
			}
		}
		else if (state == 3 && (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X")))
		{
			Object.Destroy(base.gameObject);
		}
		else if (state == 4 && !Object.FindObjectOfType<Fade>().IsPlaying())
		{
			if (tsExit == 1)
			{
				Application.Quit(0);
			}
			else
			{
				gm.ForceLoadArea(6);
			}
		}
	}

	private void LoadOverwrite()
	{
		overwrite.transform.localPosition = Vector3.zero;
		overwrite.Find("Confirm").GetComponent<Text>().text = "Overwrite Slot " + (overwriteSaveSlot + 1) + "?";
		overwrite.Find("Name").GetComponent<Text>().text = gm.GetPlayerName();
		overwrite.Find("Room").GetComponent<Text>().text = MapInfo.GetMapName(gm.GetCurrentZone());
		overwrite.Find("LV").GetComponent<Text>().text = "LV " + gm.GetLV();
		overwrite.Find("Time").GetComponent<Text>().text = gm.GetFormattedUpdatedPlayTime();
		overwrite.Find("NameOld").GetComponent<Text>().text = saves[overwriteSaveSlot].name;
		overwrite.Find("RoomOld").GetComponent<Text>().text = MapInfo.GetMapName(saves[overwriteSaveSlot].zone);
		overwrite.Find("LVOld").GetComponent<Text>().text = "LV " + gm.GetLV(saves[overwriteSaveSlot].exp);
		overwrite.Find("TimeOld").GetComponent<Text>().text = gm.GetFormattedPlayTimeFromTime(saves[overwriteSaveSlot].playTime);
		overwrite.Find("0").GetComponent<Text>().color = selectionColor;
		overwrite.Find("1").GetComponent<Text>().color = Color.white;
		Image[] componentsInChildren = savefiles.GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			if (image.gameObject.name != "Background" && image.color != Color.black)
			{
				image.color = new Color(borderColor.r * 0.2f, borderColor.g * 0.2f, borderColor.b * 0.2f);
			}
		}
		Text[] componentsInChildren2 = savefiles.GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].color = new Color32(51, 51, 51, byte.MaxValue);
		}
	}

	private void SaveFile()
	{
		gm.PlayGlobalSFX("sounds/snd_save");
		gm.SetFileID(overwriteSaveSlot);
		gm.SaveFile(true);
		Text[] componentsInChildren = savefiles.GetComponentsInChildren<Text>();
		foreach (Text text in componentsInChildren)
		{
			if (text.transform.parent.gameObject.name == "CurBox" || text.transform.parent.gameObject.name == "Save" + overwriteSaveSlot || text.transform.parent.parent.gameObject.name == "CurBox" || text.transform.parent.parent.gameObject.name == "Save" + overwriteSaveSlot)
			{
				text.color = selectionColor;
			}
			else
			{
				text.color = new Color32(51, 51, 51, byte.MaxValue);
			}
		}
		componentsInChildren = savefiles.Find("Save" + overwriteSaveSlot).GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		savefiles.Find("Save" + overwriteSaveSlot).Find("CenterText").GetComponent<Text>()
			.enabled = true;
		savefiles.Find("Save" + overwriteSaveSlot).Find("CenterText").GetComponent<Text>()
			.text = "File Saved";
		if (tsMode)
		{
			savefiles.GetChild(3).GetComponent<Image>().enabled = false;
			Image[] componentsInChildren2 = savefiles.GetChild(3).Find("TSBox").GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = true;
			}
			componentsInChildren2 = savefiles.GetChild(3).Find("TSCorners").GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = true;
			}
		}
		Object.Destroy(savefiles.Find("Save3").gameObject);
		soul.GetComponent<Image>().enabled = false;
		state = 3;
	}

	private void UpdateSaveFileColors()
	{
		Text[] componentsInChildren;
		for (int i = 0; i < 4; i++)
		{
			componentsInChildren = savefiles.Find("Save" + i).GetComponentsInChildren<Text>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].color = ((i == index) ? selectionColor : Color.white);
			}
		}
		componentsInChildren = savefiles.Find("CurBox").GetComponentsInChildren<Text>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].color = Color.white;
		}
		soul.localPosition = ((index < 4 && saveSlotsTaken[index]) ? new Vector3(-220f, 17f + savefiles.Find("Save" + index).localPosition.y) : new Vector3(-77f, savefiles.Find("Save" + index).localPosition.y));
	}

	private void UpdateAllText()
	{
		mainbox.Find("Name").GetComponent<Text>().text = gm.GetFileName();
		mainbox.Find("Time").GetComponent<Text>().text = gm.GetFormattedUpdatedPlayTime();
		if (tsMode)
		{
			mainbox.Find("RoomBox").Find("Room").GetComponent<Text>()
				.text = MapInfo.GetMapName(gm.GetCurrentZone());
			mainbox.Find("LV").GetComponent<Text>().text = "LV " + gm.GetLV() + "\nHP " + gm.GetMaxHP(0);
			mainbox.Find("EXP").GetComponent<Text>().text = "EXP " + gm.GetEXP() + "\nG " + gm.GetGold();
		}
		else
		{
			mainbox.Find("Room").GetComponent<Text>().text = MapInfo.GetMapName(gm.GetCurrentZone());
			mainbox.Find("LV").GetComponent<Text>().text = "LV " + gm.GetLV();
		}
		savefiles.Find("CurBox").Find("Name").GetComponent<Text>()
			.text = gm.GetPlayerName();
		savefiles.Find("CurBox").Find("Room").GetComponent<Text>()
			.text = MapInfo.GetMapName(gm.GetCurrentZone());
		savefiles.Find("CurBox").Find("LV").GetComponent<Text>()
			.text = "LV " + gm.GetLV();
		savefiles.Find("CurBox").Find("Time").GetComponent<Text>()
			.text = gm.GetFormattedUpdatedPlayTime();
		saves = new SAVEFile[3];
		for (int i = 0; i < 3; i++)
		{
			string path = Path.Combine(Application.persistentDataPath, "SAVE" + i + ".sav");
			new BinaryFormatter();
			if (File.Exists(path))
			{
				try
				{
					using (FileStream fs = File.Open(path, FileMode.Open))
					{
						SAVEFileIO.ReadFile(ref saves[i], fs);
						saveSlotsTaken[i] = true;
						savefiles.Find("Save" + i).Find("Name").GetComponent<Text>()
							.text = saves[i].name;
						savefiles.Find("Save" + i).Find("Time").GetComponent<Text>()
							.text = gm.GetFormattedPlayTimeFromTime(saves[i].playTime);
						savefiles.Find("Save" + i).Find("Room").GetComponent<Text>()
							.text = MapInfo.GetMapName(saves[i].zone);
						savefiles.Find("Save" + i).Find("LV").GetComponent<Text>()
							.text = "LV " + gm.GetLV(saves[i].exp);
					}
				}
				catch
				{
					Text[] componentsInChildren = savefiles.Find("Save" + i).GetComponentsInChildren<Text>();
					for (int j = 0; j < componentsInChildren.Length; j++)
					{
						componentsInChildren[j].enabled = false;
					}
					savefiles.Find("Save" + i).Find("CenterText").GetComponent<Text>()
						.enabled = true;
				}
			}
			else
			{
				Text[] componentsInChildren = savefiles.Find("Save" + i).GetComponentsInChildren<Text>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].enabled = false;
				}
				savefiles.Find("Save" + i).Find("CenterText").GetComponent<Text>()
					.enabled = true;
			}
		}
	}

	public override void CancelControlReturn()
	{
		returnControl = false;
	}
}
