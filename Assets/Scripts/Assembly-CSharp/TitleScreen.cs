using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : SelectableBehaviour
{
	public enum State
	{
		None = 0,
		Title = 1,
		NewGame = 2,
		ContinueGame = 3,
		Options = 4,
		VolumeSlider = 5,
		NameSelect = 6,
		LoadNewGame = 7,
		CorruptSave = 8,
		DeleteSaveOld = 9,
		FileSelect = 10,
		Controls = 11,
		NameConfirm = 12,
		Extras = 13,
		LoadNewDimension = 14
	}

	public enum SaveState
	{
		Select = 0,
		CopyFrom = 1,
		CopyTo = 2,
		CopyOverwriteConfirm = 3,
		DeleteSelect = 4,
		DeleteConfirm = 5,
		DeleteDoubleConfirm = 6
	}

	private State state = State.Title;

	private int frames;

	private Fade fade;

	private GameManager gm;

	private AudioSource mus;

	private AudioSource menuSfx;

	private AudioSource selSfx;

	private AudioSource backSfx;

	private Image logo;

	private Transform characters;

	private Transform gamerules;

	private Transform saveinfo;

	private Transform letters;

	private Transform soul;

	private Transform options;

	private Transform saveCharacters;

	private Transform deleteSave;

	private Transform saveFiles;

	private Transform controls;

	private Transform kris;

	private Transform susie;

	private Transform noelle;

	private Transform mini;

	private Transform door;

	private SpriteRenderer savePlatform;

	private Transform[] optionsTabs;

	private int optionsTab;

	private TextUT deleteText;

	private bool selecting;

	private bool selVertical = true;

	private int index;

	private int oldNameIndex;

	private int indexY;

	private int menuLimit;

	private float soulMoveRate = 0.2f;

	private int correction;

	private bool correctionNotice;

	private bool disappointment;

	private Selection selection;

	private int startPhase;

	private bool holdingAxis;

	private bool holdingAxisY;

	private bool usingNewTitle;

	private int windowScale;

	private int volume;

	private int volumeFrames;

	private Options localOptions;

	private bool mobile;

	private SAVEFile[] saves = new SAVEFile[3];

	private int saveIndex;

	private int copiedSaveIndex = -1;

	private int savePages;

	private SaveState saveState;

	private int saveHeaderResetFrames = 120;

	private string saveHeaderText = "";

	private int controlsType;

	private bool rebinding;

	private FileStatus[] fileStatuses;

	private SOUL corruptSoul;

	private int loadToScene = -1;

	private static string[] controlNames = new string[7] { "Down", "Right", "Up", "Left", "Confirm", "Cancel", "Menu" };

	private void Awake()
	{
		gm = Util.GameManager();
		gm.SetDefaultValues();
		mus = GetComponents<AudioSource>()[0];
		menuSfx = GetComponents<AudioSource>()[1];
		selSfx = GetComponents<AudioSource>()[2];
		backSfx = GetComponents<AudioSource>()[3];
		logo = base.transform.Find("Logo").GetComponent<Image>();
		characters = base.transform.Find("Characters");
		gamerules = base.transform.Find("GameRules");
		saveinfo = base.transform.Find("SaveInfo");
		letters = base.transform.Find("Letters");
		options = base.transform.Find("Options");
		soul = base.transform.Find("SOUL");
		saveCharacters = base.transform.Find("SaveCharacters");
		deleteSave = base.transform.Find("DeleteSave");
		saveFiles = base.transform.Find("SaveFiles");
		controls = base.transform.Find("Controls");
		optionsTabs = new Transform[4]
		{
			options.Find("MainTab"),
			null,
			options.Find("VisualsTab"),
			options.Find("MobileButtonsTab")
		};
		selection = base.gameObject.AddComponent<Selection>();
		deleteText = base.gameObject.AddComponent<TextUT>();
		deleteText.EnableGasterEffect();
		fileStatuses = new FileStatus[3];
		gm.SetFramerate(30);
		UTInput.SetPriority(b: true);
		Util.GameManager().SetSessionFlag(20, 0);
		if (UnityEngine.Random.Range(0, 1200) == 0 || (DateTime.Now.Day == 1 && DateTime.Now.Month == 4))
		{
			logo.sprite = Resources.Load<Sprite>("ui/title/anagrams/spr_logo_anagram_" + UnityEngine.Random.Range(0, 3));
		}
		UpdateAllText();
	}

	private void Start()
	{
		gm.DisableSingleBattleMode();
		fade = Util.FindObjectOfType<Fade>();
		fade.transform.parent.position = Vector3.zero;
		base.transform.Find("Copyright").GetComponent<Text>().text = base.transform.Find("Copyright").GetComponent<Text>().text.Replace("VER", gm.GetVersion());
		volume = gm.config.GetInt("General", "Volume", 100);
		localOptions = new Options();
		localOptions.LoadFromConfig(ref gm.config);
		UpdateSettingsText();
		if (PersistentSAVE.GetInt("new-title", 0) == 1 && PersistentSAVE.GetInt("completion", 0) == 0)
		{
			PersistentSAVE.SetInt("completion", 1);
		}
		if (PersistentSAVE.GetInt("completion", 0) >= 1)
		{
			mus.clip = Resources.Load<AudioClip>("music/mus_castletown");
			kris = GameObject.Find("KrisBG").transform;
			susie = GameObject.Find("SusieBG").transform;
			noelle = GameObject.Find("NoelleBG").transform;
			mini = GameObject.Find("MiniBG").transform;
			Transform[] array = new Transform[6] { kris, susie, noelle, mini, null, null };
			for (int i = 0; i < array.Length; i++)
			{
				int num = PersistentSAVE.GetInt("last-saved-pm-" + i, -1);
				if ((bool)array[i] && num > -1)
				{
					array[i].GetComponent<SpriteRenderer>().enabled = true;
				}
				if (i == 0)
				{
					if (num == 0 && PersistentSAVE.GetInt("kris-eye-title", 0) == 1)
					{
						kris.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/title/spr_kris_title");
					}
					else if (num == 6)
					{
						kris.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/title/spr_frisk_title");
					}
				}
				if (i == 1 && num == 2)
				{
					array[1].GetComponent<SpriteRenderer>().enabled = false;
					array[2].GetComponent<SpriteRenderer>().enabled = true;
				}
				if (i == 3 && num == 3)
				{
					mini.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/title/spr_paula_title");
				}
			}
			door = GameObject.Find("DoorBG").transform;
			door.GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("Deltarune").GetComponent<SpriteRenderer>().enabled = true;
			characters.localPosition = new Vector3(640f, 0f);
			logo.transform.localPosition = new Vector3(0f, 167f);
			UnityEngine.Object.Destroy(logo.transform.GetChild(0).gameObject);
			base.transform.Find("Unresponsive").localPosition = new Vector3(-313f, 75f);
			soul.GetComponent<Image>().enabled = true;
			soul.localScale *= 2f;
			savePlatform = GameObject.Find("SavePlatform").GetComponent<SpriteRenderer>();
			usingNewTitle = true;
		}
		fade.FadeIn((!usingNewTitle) ? 1 : 15);
		if (mobile)
		{
			base.transform.Find("Unresponsive").GetComponent<Text>().text = "[press z]";
		}
		else
		{
			base.transform.Find("Unresponsive").GetComponent<Text>().text = base.transform.Find("Unresponsive").GetComponent<Text>().text.Replace("z", UTInput.GetKeyName("Z"));
		}
		if (mobile)
		{
			Util.FindObjectOfType<MobileUI>().EnableButtons(dPadEnabled: true, z: true, x: true, c: true, instant: false);
			optionsTabs[3].Find("Color").GetComponent<Text>().text = Util.FindObjectOfType<MobileUI>().GetCurrentColorName();
			optionsTabs[3].Find("DPADSkin").GetComponent<Text>().text = Util.FindObjectOfType<MobileUI>().GetCurrentPadSkin();
			optionsTabs[3].Find("ButtonSkin").GetComponent<Text>().text = Util.FindObjectOfType<MobileUI>().GetCurrentButtonSkin();
			optionsTabs[3].Find("TouchPadSensitivityValue").GetComponent<Text>().text = PlayerPrefs.GetInt("DPADSensitivity", 10).ToString();
		}
		Util.FindObjectOfType<EndNameEvent>().SetTextObject(base.transform.Find("Letters").Find("Name").GetComponent<RectTransform>());
		gm.DeactivateCheckpoint();
		mus.Play();
	}

	private void Update()
	{
		if (state == State.NameSelect || state == State.NameConfirm)
		{
			soul.localScale = Vector3.Lerp(soul.localScale, new Vector3(2f, 2f, 1f), 0.4f);
			soul.GetComponent<Image>().color = Color.Lerp(soul.GetComponent<Image>().color, new Color(1f, 0f, 0f, 0.6f), 0.2f);
		}
		else if (state > State.Title)
		{
			soul.localScale = Vector3.Lerp(soul.localScale, new Vector3(1f, 1f, 1f), 0.4f);
			soul.GetComponent<Image>().color = Color.Lerp(soul.GetComponent<Image>().color, new Color(1f, 0f, 0f, 1f), 0.2f);
		}
		if (selecting)
		{
			if (GetAxisRaw() != 0f && !holdingAxis)
			{
				if (state == State.NameSelect)
				{
					letters.GetChild(index + indexY * 10).GetComponent<Text>().color = Color.white;
				}
				else if (state == State.NameConfirm)
				{
					letters.GetChild(index + 29).GetComponent<Text>().color = Color.white;
				}
				if (state == State.Controls)
				{
					string text = index.ToString();
					controls.Find(text).GetComponent<Text>().color = Color.white;
					if (index < 7 && !mobile)
					{
						controls.Find(text + "-Text").GetComponent<Text>().color = Color.white;
					}
				}
				soulMoveRate = 0.5f;
				holdingAxis = true;
				index = (index - (int)GetAxisRaw()) % menuLimit;
				if (index < 0)
				{
					index = menuLimit - 1;
				}
				if (index == 8 && indexY == 2)
				{
					index = 0;
				}
				else if (index == 9 && indexY == 2)
				{
					index = 7;
				}
				if (state == State.NameSelect)
				{
					letters.GetChild(index + indexY * 10).GetComponent<Text>().color = new Color(1f, 1f, 0f);
				}
				else if (state == State.NameConfirm)
				{
					letters.GetChild(index + 29).GetComponent<Text>().color = new Color(1f, 1f, 0f);
				}
				else
				{
					menuSfx.Play();
				}
				if (state == State.Controls)
				{
					string text2 = index.ToString();
					controls.Find(text2).GetComponent<Text>().color = new Color(0f, 1f, 1f);
					if (index < 7 && !mobile)
					{
						controls.Find(text2 + "-Text").GetComponent<Text>().color = new Color(0f, 1f, 1f);
					}
				}
			}
			else if (GetAxisRaw() == 0f && holdingAxis)
			{
				holdingAxis = false;
			}
		}
		if (state == State.Title)
		{
			if (usingNewTitle)
			{
				kris.position = Vector3.Lerp(kris.position, Vector3.zero, 0.2f);
				susie.position = Vector3.Lerp(susie.position, Vector3.zero, 0.2f);
				noelle.position = Vector3.Lerp(noelle.position, Vector3.zero, 0.2f);
				mini.position = Vector3.Lerp(mini.position, Vector3.zero, 0.2f);
				door.position = Vector3.Lerp(door.position, Vector3.zero, 0.2f);
				frames++;
				soul.transform.localPosition = new Vector3(0f, -192f + Mathf.Sin((float)(frames * 2) * (MathF.PI / 180f)) * 6f);
			}
			UpdateUnresponsive();
			if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("C"))
			{
				if (usingNewTitle)
				{
					kris.GetComponent<SpriteRenderer>().enabled = false;
					susie.GetComponent<SpriteRenderer>().enabled = false;
					noelle.GetComponent<SpriteRenderer>().enabled = false;
					mini.GetComponent<SpriteRenderer>().enabled = false;
					door.GetComponent<SpriteRenderer>().enabled = false;
				}
				soul.GetComponent<Image>().enabled = true;
				Vector3 position = characters.position;
				LoadSAVEFiles();
				if (!usingNewTitle)
				{
					soul.position += characters.position - position;
				}
				frames = 0;
				selSfx.Play();
				base.transform.Find("Unresponsive").gameObject.SetActive(value: false);
			}
		}
		else if (state == State.NewGame)
		{
			soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(-170f, gamerules.GetChild(index).localPosition.y + 16f), 0.4f);
			if ((UTInput.GetButtonDown("Z") && index == 1) || UTInput.GetButtonDown("X"))
			{
				if (UTInput.GetButtonDown("X"))
				{
					backSfx.Play();
				}
				else
				{
					selSfx.Play();
				}
				LoadSAVEFiles();
			}
			else if (UTInput.GetButtonDown("Z") && index == 0)
			{
				selSfx.Play();
				base.transform.Find("Copyright").GetComponent<Text>().enabled = false;
				soul.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/spr_blurry_soul");
				deleteText.StartText("\b          YOUR OWN NAME.", new Vector2(0f, 111f), "", 2);
				state = State.NameSelect;
				indexY = 0;
				selVertical = false;
				selecting = true;
				menuLimit = 10;
				letters.position = Vector3.zero;
				gamerules.position = new Vector3(640f, 0f);
				characters.position = Vector3.zero;
				characters.Find("Susie").localPosition = new Vector3(640f, 0f);
				characters.Find("Kris").localPosition = new Vector3(640f, 0f);
				Util.FindObjectOfType<EndNameEvent>().transform.Find("Kris").position = new Vector3(0f, -2.2f);
			}
		}
		else if (state == State.ContinueGame)
		{
			soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(saveinfo.GetChild(index).localPosition.x - 20f, saveinfo.GetChild(index).localPosition.y + 17f), 0.4f);
			if ((UTInput.GetButtonDown("Z") && index == 1) || UTInput.GetButtonDown("X"))
			{
				if (UTInput.GetButtonDown("X"))
				{
					backSfx.Play();
				}
				else
				{
					selSfx.Play();
				}
				LoadSAVEFiles();
			}
			else if (UTInput.GetButtonDown("Z") && index == 0)
			{
				selSfx.Play();
				gm.SpawnFromLastSave(respawn: false);
			}
		}
		else if (state == State.Options)
		{
			soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(optionsTabs[optionsTab].GetChild(index).localPosition.x - 36f, optionsTabs[optionsTab].GetChild(index).localPosition.y + 16f), 0.4f);
			if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && index == menuLimit - 1))
			{
				if (optionsTab > 0)
				{
					if (optionsTab == 2)
					{
						index = 6;
					}
					else if (optionsTab == 3)
					{
						index = 1;
					}
					else
					{
						index = 0;
					}
					optionsTabs[optionsTab].gameObject.SetActive(value: false);
					optionsTab = 0;
					optionsTabs[optionsTab].gameObject.SetActive(value: true);
					menuLimit = 8;
				}
				else
				{
					index = 2;
					menuLimit = 3;
					LoadSAVEFiles();
				}
				SaveSettings();
				LoadGMSettings();
				if (UTInput.GetButtonDown("X"))
				{
					backSfx.Play();
				}
				else
				{
					selSfx.Play();
				}
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				if (optionsTab == 0)
				{
					if (index == 0)
					{
						selecting = false;
						selSfx.Play();
						optionsTabs[optionsTab].Find("0").GetComponent<Text>().color = new Color(1f, 1f, 0f);
						optionsTabs[optionsTab].Find("Volume").GetComponent<Text>().color = new Color(1f, 1f, 0f);
						state = State.VolumeSlider;
					}
					else if (index == 1)
					{
						if (!mobile)
						{
							selSfx.Play();
							gm.SetWindowScale(gm.GetWindowScale() + 1);
							gm.UpdateWindow();
							UpdateSettingsText();
						}
						else
						{
							optionsTabs[0].gameObject.SetActive(value: false);
							optionsTabs[3].gameObject.SetActive(value: true);
							menuLimit = 4;
							index = 0;
							optionsTab = 3;
							selSfx.Play();
						}
					}
					else if (index == 2)
					{
						selSfx.Play();
						state = State.Controls;
						controlsType = index;
						controls.localPosition = Vector3.zero;
						string text3 = "0";
						controls.Find(text3).GetComponent<Text>().color = new Color(0f, 1f, 1f);
						if (!mobile)
						{
							controls.Find(text3 + "-Text").GetComponent<Text>().color = new Color(0f, 1f, 1f);
						}
						controls.Find("8").GetComponent<Text>().color = Color.white;
						index = 0;
						menuLimit = 9;
						UpdateControlText();
					}
					else if (index == 3)
					{
						selSfx.Play();
						localOptions.contentSetting.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					else if (index == 4)
					{
						selSfx.Play();
						localOptions.autoRun.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					else if (index == 5)
					{
						selSfx.Play();
						localOptions.easyMode.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					else if (index == 6)
					{
						optionsTabs[0].gameObject.SetActive(value: false);
						optionsTabs[2].gameObject.SetActive(value: true);
						menuLimit = 5;
						index = 0;
						optionsTab = 2;
						selSfx.Play();
					}
				}
				else if (optionsTab == 2)
				{
					if (index == 0)
					{
						selSfx.Play();
						localOptions.autoButton.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					if (index == 1)
					{
						selSfx.Play();
						localOptions.buttonStyle.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					if (index == 2)
					{
						selSfx.Play();
						localOptions.vSync.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
						gm.SetFramerate();
					}
					if (index == 3)
					{
						selSfx.Play();
						localOptions.monitorInfo.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
						gm.SetFramerate();
						gm.SetMonitorInfoEnabled(localOptions.monitorInfo.value == 1);
					}
				}
				else if (optionsTab == 3)
				{
					if (index == 0)
					{
						selSfx.Play();
						Util.FindObjectOfType<MobileUI>().CycleButtonColors();
						optionsTabs[3].Find("Color").GetComponent<Text>().text = Util.FindObjectOfType<MobileUI>().GetCurrentColorName();
					}
					else if (index == 1)
					{
						selSfx.Play();
						Util.FindObjectOfType<MobileUI>().CyclePadSkin();
						optionsTabs[3].Find("DPADSkin").GetComponent<Text>().text = Util.FindObjectOfType<MobileUI>().GetCurrentPadSkin();
					}
					else if (index == 2)
					{
						selSfx.Play();
						Util.FindObjectOfType<MobileUI>().CycleButtonSkin();
						optionsTabs[3].Find("ButtonSkin").GetComponent<Text>().text = Util.FindObjectOfType<MobileUI>().GetCurrentButtonSkin();
					}
				}
			}
			if (optionsTab == 2)
			{
				characters.Find("Kris").GetComponent<Image>().enabled = index != 2;
				characters.Find("Susie").GetComponent<Image>().enabled = index != 2;
				optionsTabs[2].Find("vSyncText").GetComponent<Text>().enabled = index == 2;
			}
			else
			{
				characters.Find("Kris").GetComponent<Image>().enabled = true;
				characters.Find("Susie").GetComponent<Image>().enabled = true;
			}
		}
		else if (state == State.VolumeSlider)
		{
			soul.localPosition = new Vector3(optionsTabs[optionsTab].GetChild(index).localPosition.x - 20f, optionsTabs[optionsTab].GetChild(index).localPosition.y + 16f);
			if (UTInput.GetAxis("Horizontal") == 0f)
			{
				volumeFrames = 0;
			}
			else
			{
				volume += (int)UTInput.GetAxis("Horizontal") * 2;
				if (volume > 100)
				{
					volume = 100;
				}
				else if (volume < 0)
				{
					volume = 0;
				}
				GameManager.UpdateVolume(volume);
				UpdateSettingsText();
				if (volumeFrames == 0)
				{
					optionsTabs[optionsTab].Find("Volume").GetComponent<AudioSource>().Play();
				}
				volumeFrames = (volumeFrames + 1) % 3;
			}
			if (UTInput.GetButtonDown("Z") || UTInput.GetButtonDown("X"))
			{
				selecting = true;
				SaveSettings();
				if (UTInput.GetButtonDown("X"))
				{
					backSfx.Play();
				}
				else
				{
					selSfx.Play();
				}
				optionsTabs[optionsTab].Find("0").GetComponent<Text>().color = Color.white;
				optionsTabs[optionsTab].Find("Volume").GetComponent<Text>().color = Color.white;
				state = State.Options;
			}
		}
		else if (state == State.NameSelect)
		{
			if (UTInput.GetAxisRaw("Vertical") != 0f && !holdingAxisY)
			{
				letters.GetChild(index + indexY * 10).GetComponent<Text>().color = Color.white;
				int num = indexY;
				holdingAxisY = true;
				indexY = (indexY - (int)UTInput.GetAxisRaw("Vertical")) % 3;
				if (indexY < 0)
				{
					indexY = 2;
				}
				if (indexY == 2)
				{
					if (index >= 8)
					{
						index = 7;
					}
					else if (index >= 6)
					{
						index = 6;
					}
				}
				if (num == 2 && index == 7)
				{
					index = 8;
				}
				letters.GetChild(index + indexY * 10).GetComponent<Text>().color = new Color(1f, 1f, 0f);
			}
			else if (UTInput.GetAxisRaw("Vertical") == 0f && holdingAxisY)
			{
				holdingAxisY = false;
			}
			soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(letters.GetChild(index + indexY * 10).localPosition.x + 15f, letters.GetChild(indexY * 10).localPosition.y + 16f), 0.4f);
			if (UTInput.GetButtonDown("Z") && index + indexY * 10 < 26)
			{
				if (letters.Find("Name").Find("Text").GetComponent<Text>()
					.text.Length < 12)
				{
					letters.Find("Name").Find("Text").GetComponent<Text>()
						.text += letters.GetChild(index + indexY * 10).GetComponent<Text>().text;
				}
				if (letters.Find("Name").Find("Text").GetComponent<Text>()
					.text == "GASTER")
				{
					SceneManager.LoadScene(0);
				}
			}
			else if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && index + indexY * 10 == 26))
			{
				if (letters.Find("Name").Find("Text").GetComponent<Text>()
					.text.Length > 0)
				{
					letters.Find("Name").Find("Text").GetComponent<Text>()
						.text = letters.Find("Name").Find("Text").GetComponent<Text>()
						.text.Substring(0, letters.Find("Name").Find("Text").GetComponent<Text>()
						.text.Length - 1);
				}
			}
			else if (UTInput.GetButtonDown("Z") && index + indexY * 10 == 27 && letters.Find("Name").Find("Text").GetComponent<Text>()
				.text.Length > 0)
			{
				for (int i = 0; i < 28; i++)
				{
					letters.GetChild(i).GetComponent<Text>().enabled = false;
				}
				letters.GetChild(29).GetComponent<Text>().enabled = true;
				letters.GetChild(29).GetComponent<Text>().color = new Color(1f, 1f, 0f);
				letters.GetChild(30).GetComponent<Text>().enabled = true;
				letters.GetChild(30).GetComponent<Text>().color = Color.white;
				Util.FindObjectOfType<EndNameEvent>().StartNameShake();
				deleteText.DestroyOldText();
				string text4 = letters.Find("Name").Find("Text").GetComponent<Text>()
					.text;
				List<string> list = new List<string>
				{
					"SUSIE", "NOELLE", "SANS", "TORIEL", "NESS", "PAULA", "CHARA", "FLOWEY", "PRUNSEL", "MARIO",
					"LUIGI", "NOEL", "SUZY", "PAPYRUS", "KAPPY", "KOFFIN", "AGAHNIM", "GANON", "PORKY", "POKEY",
					"BERDLY", "STARLOW"
				};
				if (correction >= 1 && !disappointment && text4 == "AAAAAAAAAAAA")
				{
					disappointment = true;
					deleteText.StartText("\b       IS THIS HOW YOU INTEND \n\b       TO SPEND PRECIOUS TIME?", new Vector2(0f, 111f), "", 2);
				}
				else if (list.Contains(text4))
				{
					deleteText.StartText("\b    AN INTERESTING COINCIDENCE.", new Vector2(0f, 111f), "", 2);
				}
				else
				{
					switch (text4)
					{
					case "KRIS":
						deleteText.StartText("\b     THEY CANNOT HEAR YOU HERE.", new Vector2(0f, 111f), "", 2);
						break;
					case "FRISK":
						deleteText.StartText("\b      THIS NAME SHALL COMMENCE\n\b     AN INTERESTING EXPERIMENT.", new Vector2(0f, 111f), "", 2);
						break;
					case "DESS":
						deleteText.StartText("\b               ...", new Vector2(0f, 111f), "", 2);
						break;
					case "CLOVER":
						deleteText.StartText("\b     INTERESTING... YOU INVOKE\n\b       THE NAME OF JUSTICE.", new Vector2(0f, 111f), "", 2);
						break;
					case "CEROBA":
						deleteText.StartText("\b     WHAT A DISTINGUISHED NAME.", new Vector2(0f, 111f), "", 2);
						break;
					default:
						deleteText.StartText("\b        THIS IS YOUR NAME.", new Vector2(0f, 111f), "", 2);
						break;
					}
				}
				state = State.NameConfirm;
				menuLimit = 2;
				oldNameIndex = index;
				index = 0;
			}
			letters.Find("Name").Find("Text").localPosition = new Vector2(-letters.Find("Name").Find("Text").GetComponent<Text>()
				.text.Length * 7, 123f);
			letters.Find("Name").Find("Text").GetComponent<RectTransform>()
				.sizeDelta = new Vector2(letters.Find("Name").Find("Text").GetComponent<Text>()
				.text.Length * 16, 32f);
		}
		else if (state == State.NameConfirm)
		{
			soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(letters.GetChild(index + 29).localPosition.x + 15f, letters.GetChild(29).localPosition.y + 16f), 0.4f);
			if (UTInput.GetButtonDown("Z"))
			{
				if (index == 0)
				{
					deleteText.DestroyOldText();
					gm.NewGame(letters.Find("Name").Find("Text").GetComponent<Text>()
						.text);
						Util.FindObjectOfType<EndNameEvent>().StartScene(gm.GetPlayerName());
						soul.GetComponent<Image>().enabled = false;
						for (int j = 0; j < letters.childCount; j++)
						{
							if ((bool)letters.GetChild(j).GetComponent<Text>())
							{
								letters.GetChild(j).GetComponent<Text>().enabled = false;
							}
						}
						state = State.LoadNewGame;
						mus.Stop();
						selecting = false;
						Util.FindObjectOfType<Fade>().UTFadeOut();
					}
					else
					{
						index = oldNameIndex;
						state = State.NameSelect;
						menuLimit = 10;
						UnityEngine.Object.FindObjectOfType<EndNameEvent>().StopNameShake();
						correction++;
						deleteText.DestroyOldText();
						if (correction >= 10 && !correctionNotice)
						{
							correctionNotice = true;
							deleteText.StartText("\b        WHAT AN INTERESTING \n\b             BEHAVIOR.", new Vector2(0f, 149f), "", 2);
						}
						else
						{
							deleteText.StartText("\b          YOUR OWN NAME.", new Vector2(0f, 111f), "", 2);
						}
						for (int k = 0; k < 28; k++)
						{
							letters.GetChild(k).GetComponent<Text>().enabled = true;
						}
						letters.GetChild(29).GetComponent<Text>().enabled = false;
						letters.GetChild(30).GetComponent<Text>().enabled = false;
					}
				}
			}
			else if (state == State.LoadNewGame)
			{
				if (!Util.FindObjectOfType<Fade>().IsPlaying())
				{
					gm.StartTime();
					gm.LoadArea(7, fadeIn: true, new Vector3(0.16f, -0.08f), Vector2.down);
					state = State.None;
				}
			}
			else if (state == State.CorruptSave)
			{
				soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(deleteSave.GetChild(index).localPosition.x - 20f, deleteSave.GetChild(index).localPosition.y + 16f), 0.4f);
				soul.GetComponent<Image>().enabled = true;
				deleteSave.GetChild(0).GetComponent<Text>().enabled = true;
				deleteSave.GetChild(1).GetComponent<Text>().enabled = true;
				if ((!deleteText.IsPlaying() && UTInput.GetButtonDown("X")) || (UTInput.GetButtonDown("Z") && index == 1))
				{
					deleteText.DestroyOldText();
					if (UTInput.GetButtonDown("X"))
					{
						backSfx.Play();
					}
					else
					{
						selSfx.Play();
					}
					LoadSAVEFiles();
					index = 0;
				}
				else if (UTInput.GetButtonDown("Z") && index == 0)
				{
					deleteText.DestroyOldText();
					deleteSave.localPosition = new Vector3(640f, 0f);
					gm.DeleteFile(saveIndex);
					fileStatuses[saveIndex] = FileStatus.Empty;
					selSfx.Play();
					LoadSAVEFiles();
					gm.PlayGlobalSFX("sounds/snd_appearance");
					corruptSoul = new GameObject("SOULDie", typeof(SOUL)).GetComponent<SOUL>();
					corruptSoul.transform.position = saveFiles.Find("file" + saveIndex).Find("soulpos").transform.position;
					corruptSoul.CreateSOUL(Color.red, monster: false, player: false);
					corruptSoul.Break();
					frames = 0;
				}
				else if (UTInput.GetButtonDown("X") || UTInput.GetButtonDown("C"))
				{
					deleteText.SkipText();
				}
			}
			else if (state == State.DeleteSaveOld)
			{
				frames++;
				if (frames == 19)
				{
					Util.FindObjectOfType<SOUL>().Break();
				}
				if (frames == 120)
				{
					MonoBehaviour.print("FUCKY!!!");
					Application.Quit();
				}
			}
			else if (state == State.FileSelect)
			{
				if (saveState != SaveState.CopyOverwriteConfirm && saveState < SaveState.DeleteConfirm)
				{
					if (UTInput.GetAxis("Vertical") != 0f && !holdingAxisY)
					{
						holdingAxisY = true;
						saveIndex = (saveIndex - (int)UTInput.GetAxis("Vertical")) % savePages;
						if (saveIndex < 0)
						{
							saveIndex = savePages - 1;
						}
						MonoBehaviour.print(saveIndex);
						menuSfx.Play();
						if (saveIndex == 3)
						{
							selecting = true;
							selVertical = false;
							menuLimit = ((saveState != SaveState.Select) ? 1 : 3);
						}
						else
						{
							selecting = false;
						}
					}
					else if (UTInput.GetAxis("Vertical") == 0f && holdingAxisY)
					{
						holdingAxisY = false;
					}
					if (saveIndex < 3)
					{
						soul.localPosition = Vector3.Lerp(soul.localPosition, saveFiles.GetChild(saveIndex + 1).Find("soulpos").transform.position * 48f, 0.4f);
					}
					else if (saveIndex == 4)
					{
						soul.localPosition = Vector3.Lerp(soul.localPosition, saveFiles.Find("Extras").transform.localPosition + new Vector3(-20f, 16f), 0.4f);
					}
					else
					{
						if (index < 0 || index > 2)
						{
							index = 0;
						}
						soul.localPosition = Vector3.Lerp(soul.localPosition, saveFiles.Find(index.ToString()).transform.localPosition + new Vector3(-20f, 16f), 0.4f);
					}
				}
				else
				{
					soul.localPosition = Vector3.Lerp(soul.localPosition, saveFiles.GetChild(saveIndex + 1).Find("selection").Find("soulpos-s")
						.transform.position * 48f + new Vector3(index * 180, 0f), 0.4f);
				}
				if (saveState == SaveState.Select)
				{
					if (UTInput.GetButtonDown("Z"))
					{
						if (saveIndex < 3)
						{
							if (fileStatuses[saveIndex] == FileStatus.Newer)
							{
								saveHeaderResetFrames = 0;
								saveFiles.GetChild(0).GetComponent<Text>().text = "It can't be loaded.";
								Util.GameManager().PlayGlobalSFX("sounds/snd_cantselect");
							}
							else
							{
								selSfx.Play();
								if (fileStatuses[saveIndex] < FileStatus.Empty)
								{
									LoadDeleteOption();
								}
								else
								{
									savePages = 5;
									LoadDefaultMenu();
								}
							}
						}
						else if (saveIndex == 3)
						{
							selSfx.Play();
							savePages = 4;
							if (index == 0)
							{
								index = 0;
								saveIndex = 0;
								saveFiles.Find("0").GetComponent<Text>().text = "Cancel";
								saveFiles.Find("1").GetComponent<Text>().enabled = false;
								saveFiles.Find("2").GetComponent<Text>().enabled = false;
								saveFiles.Find("Extras").GetComponent<Text>().enabled = false;
								saveHeaderText = "Choose a file to copy.";
								saveState = SaveState.CopyFrom;
							}
							if (index == 1)
							{
								index = 0;
								saveIndex = 0;
								saveFiles.Find("0").GetComponent<Text>().text = "Cancel";
								saveFiles.Find("1").GetComponent<Text>().enabled = false;
								saveFiles.Find("2").GetComponent<Text>().enabled = false;
								saveFiles.Find("Extras").GetComponent<Text>().enabled = false;
								saveHeaderText = "Choose a file to erase.";
								saveState = SaveState.DeleteSelect;
							}
							if (index == 2)
							{
								LoadOptions();
							}
						}
						else if (saveIndex == 4)
						{
							fade.FadeOut(15);
							state = State.Extras;
							selSfx.Play();
							mus.Stop();
							frames = 0;
						}
					}
					if (saveHeaderResetFrames < 90)
					{
						saveHeaderResetFrames++;
					}
					else
					{
						saveFiles.GetChild(0).GetComponent<Text>().text = saveHeaderText;
					}
				}
				else if (saveState == SaveState.CopyFrom)
				{
					if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && saveIndex == 3))
					{
						if (UTInput.GetButtonDown("X"))
						{
							backSfx.Play();
						}
						else
						{
							selSfx.Play();
						}
						index = 0;
						ResetSaveState();
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						if (saves[saveIndex] == null)
						{
							backSfx.Play();
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = "It can't be copied.";
						}
						else
						{
							selSfx.Play();
							copiedSaveIndex = saveIndex;
							saveIndex = 0;
							saveState = SaveState.CopyTo;
							saveHeaderText = "Choose a file to copy to.";
						}
					}
				}
				else if (saveState == SaveState.CopyTo)
				{
					if (UTInput.GetButtonDown("X"))
					{
						backSfx.Play();
						saveIndex = copiedSaveIndex;
						copiedSaveIndex = -1;
						saveState = SaveState.CopyFrom;
						saveHeaderText = "Choose a file to copy.";
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						if (saveIndex == copiedSaveIndex)
						{
							backSfx.Play();
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = "You can't copy there.";
						}
						else if (saveIndex == 3)
						{
							selSfx.Play();
							index = 0;
							ResetSaveState();
						}
						else if (saves[saveIndex] != null)
						{
							selSfx.Play();
							saveHeaderText = "The file will be overwritten.";
							saveState = SaveState.CopyOverwriteConfirm;
							saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
								.text = "Copy over this file?";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = "Yes";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = "No";
							selecting = true;
							menuLimit = 2;
						}
						else
						{
							gm.PlayGlobalSFX("sounds/snd_appearance");
							gm.CopyFile(copiedSaveIndex, saveIndex);
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = "Copy complete.";
							ResetSaveState();
							SetSAVEStrings();
						}
					}
				}
				else if (saveState == SaveState.CopyOverwriteConfirm)
				{
					if (UTInput.GetButtonDown("X"))
					{
						saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
							.GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
							.GetComponent<Text>()
							.text = "";
						backSfx.Play();
						saveIndex = copiedSaveIndex;
						copiedSaveIndex = -1;
						saveState = SaveState.CopyFrom;
						saveHeaderText = "Choose a file to copy.";
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
							.GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
							.GetComponent<Text>()
							.text = "";
						if (index == 0)
						{
							gm.PlayGlobalSFX("sounds/snd_appearance");
							gm.CopyFile(copiedSaveIndex, saveIndex);
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = "Copy complete.";
							ResetSaveState();
							SetSAVEStrings();
						}
						else if (index == 1)
						{
							selSfx.Play();
							index = 0;
							ResetSaveState();
						}
					}
				}
				else if (saveState == SaveState.DeleteSelect)
				{
					if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && saveIndex == 3))
					{
						if (UTInput.GetButtonDown("X"))
						{
							backSfx.Play();
						}
						else
						{
							selSfx.Play();
						}
						index = 0;
						ResetSaveState();
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						if (saves[saveIndex] == null)
						{
							backSfx.Play();
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = "There's nothing to erase.";
						}
						else
						{
							selSfx.Play();
							saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
								.text = "Erase this file?";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = "Yes";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = "No";
							saveState = SaveState.DeleteConfirm;
							selecting = true;
							menuLimit = 2;
						}
					}
				}
				else if (saveState == SaveState.DeleteConfirm)
				{
					if (UTInput.GetButtonDown("X"))
					{
						saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
							.GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
							.GetComponent<Text>()
							.text = "";
						backSfx.Play();
						saveState = SaveState.DeleteSelect;
						saveHeaderText = "Choose a file to erase.";
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						if (index == 0)
						{
							selSfx.Play();
							saveState = SaveState.DeleteDoubleConfirm;
							saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
								.text = "Really erase it?";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = "Yes!";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = "No!";
						}
						else if (index == 1)
						{
							selSfx.Play();
							saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
								.enabled = true;
							saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
								.enabled = true;
							saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
								.enabled = true;
							saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
								.text = "";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = "";
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = "";
							index = 1;
							ResetSaveState();
						}
					}
				}
				else if (saveState == SaveState.DeleteDoubleConfirm)
				{
					if (UTInput.GetButtonDown("X"))
					{
						saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
							.GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
							.GetComponent<Text>()
							.text = "";
						backSfx.Play();
						saveState = SaveState.DeleteSelect;
						saveHeaderText = "Choose a file to erase.";
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
							.enabled = true;
						saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
							.GetComponent<Text>()
							.text = "";
						saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
							.GetComponent<Text>()
							.text = "";
						if (index == 0)
						{
							SOUL component = new GameObject("SOULDie", typeof(SOUL)).GetComponent<SOUL>();
							component.transform.position = soul.localPosition / 48f;
							component.CreateSOUL(Color.red, monster: false, player: false);
							component.Break();
							gm.PlayGlobalSFX("sounds/snd_appearance");
							gm.DeleteFile(saveIndex);
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = "Erase complete.";
							index = 1;
							ResetSaveState();
							SetSAVEStrings();
						}
						else if (index == 1)
						{
							selSfx.Play();
							index = 1;
							ResetSaveState();
						}
					}
				}
				for (int l = 0; l < 3; l++)
				{
					Color color = new Color(0.5f, 0.5f, 0.5f);
					if (saveIndex == l)
					{
						color = ((saveState == SaveState.DeleteDoubleConfirm) ? Color.red : Color.white);
					}
					if (copiedSaveIndex == l)
					{
						color = new Color(1f, 1f, 0.5f);
					}
					if (fileStatuses[l] < FileStatus.Empty)
					{
						color = ((saveIndex != l) ? new Color(0.5f, 0f, 0f) : Color.red);
					}
					Text[] componentsInChildren = saveFiles.GetChild(l + 1).GetComponentsInChildren<Text>();
					foreach (Text text5 in componentsInChildren)
					{
						if (text5.gameObject.name != "cont" && text5.gameObject.name != "back")
						{
							text5.color = color;
						}
					}
					Image[] componentsInChildren2 = saveFiles.GetChild(l + 1).GetComponentsInChildren<Image>();
					foreach (Image image in componentsInChildren2)
					{
						if (image.gameObject.name != "fg")
						{
							image.color = color;
						}
					}
				}
				if (saveHeaderResetFrames < 90)
				{
					saveHeaderResetFrames++;
				}
				else
				{
					saveFiles.GetChild(0).GetComponent<Text>().text = saveHeaderText;
				}
			}
			else if (state == State.Controls)
			{
				soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(controls.GetChild(index + 3).localPosition.x - 26f, controls.GetChild(index + 3).localPosition.y + 16f), 0.4f);
				if (!rebinding)
				{
					if (UTInput.GetButtonDown("Z"))
					{
						selSfx.Play();
						if (index == 7)
						{
							UTInput.ResetKeys();
							UpdateControlText();
							gm.config.Read();
						}
						else if (index == 8)
						{
							controls.localPosition = new Vector3(640f, 0f);
							state = State.Options;
							menuLimit = 8;
							index = 2;
						}
						else
						{
							string text6 = index.ToString();
							controls.Find(text6).GetComponent<Text>().color = Color.red;
							if (!mobile)
							{
								controls.Find(text6 + "-Text").GetComponent<Text>().color = Color.red;
							}
							selecting = false;
							rebinding = true;
						}
					}
				}
				else if (Input.GetKeyDown(KeyCode.LeftShift) || Input.GetKeyDown(KeyCode.RightShift) || Input.GetKeyDown(KeyCode.LeftControl) || Input.GetKeyDown(KeyCode.LeftControl))
				{
					CancelRebind();
				}
				else if (Input.GetKeyDown(KeyCode.Return))
				{
					selSfx.Play();
				}
				else if (UTInput.joystickIsActive)
				{
					GamepadButton[] array = (GamepadButton[])Enum.GetValues(typeof(GamepadButton));
					foreach (GamepadButton gamepadButton in array)
					{
						if (Gamepad.current[gamepadButton].wasPressedThisFrame)
						{
							holdingAxis = true;
							selSfx.Play();
							UTInput.BindButton(controlNames[index], gamepadButton);
							UpdateControlText();
							selecting = true;
							rebinding = false;
							controls.Find(index.ToString()).GetComponent<Text>().color = new Color(0f, 1f, 1f);
							if (!mobile)
							{
								controls.Find(index + "-Text").GetComponent<Text>().color = new Color(0f, 1f, 1f);
							}
							gm.config.Read();
						}
					}
				}
				else if (Keyboard.current != null)
				{
					foreach (Key validKeyInput in UTInput.GetValidKeyInputs())
					{
						if (Keyboard.current[validKeyInput].wasPressedThisFrame)
						{
							holdingAxis = true;
							selSfx.Play();
							UTInput.BindKey(controlNames[index], validKeyInput);
							UpdateControlText();
							selecting = true;
							rebinding = false;
							controls.Find(index.ToString()).GetComponent<Text>().color = new Color(0f, 1f, 1f);
							if (!mobile)
							{
								controls.Find(index + "-Text").GetComponent<Text>().color = new Color(0f, 1f, 1f);
							}
							gm.config.Read();
						}
					}
				}
			}
			else if (state == State.Extras)
			{
				if (!UnityEngine.Object.FindObjectOfType<Fade>().IsPlaying())
				{
					SceneManager.LoadScene(132);
				}
			}
			else if (state == State.LoadNewDimension && !Util.FindObjectOfType<Fade>().IsPlaying())
			{
				frames++;
				if (frames >= 10)
				{
					if (loadToScene == -1)
					{
						gm.SpawnFromLastSave(respawn: false);
					}
					else if (loadToScene == 103)
					{
						SceneManager.LoadScene(103);
					}
					state = State.None;
				}
			}
			if ((bool)corruptSoul)
			{
				corruptSoul.ChangeSOULMode(UnityEngine.Random.Range(0, 25));
			}
		}

		private void ResetSaveState()
		{
			menuLimit = 3;
			selVertical = false;
			selecting = true;
			saveIndex = 3;
			saveState = SaveState.Select;
			copiedSaveIndex = -1;
			saveFiles.Find("0").GetComponent<Text>().text = "Copy";
			saveFiles.Find("1").GetComponent<Text>().enabled = true;
			saveFiles.Find("2").GetComponent<Text>().enabled = true;
			saveFiles.Find("Extras").GetComponent<Text>().enabled = true;
			saveHeaderText = "Please select a file.";
			savePages = 5;
		}

		private float GetAxisRaw()
		{
			if (selVertical)
			{
				return UTInput.GetAxisRaw("Vertical");
			}
			return 0f - UTInput.GetAxisRaw("Horizontal");
		}

		public override void MakeDecision(Vector2 index, int id)
		{
			selection.Disable();
			mus.Stop();
			startPhase = (int)(index.y + index.x * 2f);
			if (startPhase != 0)
			{
				fade.FadeOut(20, Color.white);
			}
			else
			{
				fade.FadeOut(20, Color.black);
			}
			state = State.VolumeSlider;
		}

		public void LoadDefaultMenu()
		{
			gm.SetFileID(saveIndex);
			if ((bool)logo)
			{
				UnityEngine.Object.Destroy(logo.gameObject);
			}
			options.localPosition = new Vector3(1640f, 0f);
			saveFiles.localPosition = new Vector3(640f, 0f);
			base.transform.Find("Copyright").GetComponent<Text>().enabled = true;
			index = 0;
			menuLimit = 2;
			selecting = true;
			if (fileStatuses[saveIndex] > FileStatus.Empty)
			{
				gm.LoadFile();
				saveinfo.Find("Name").GetComponent<Text>().text = gm.GetFileName();
				saveinfo.Find("LV").GetComponent<Text>().text = "LV " + gm.GetFileLV();
				saveinfo.Find("Time").GetComponent<Text>().text = gm.GetFormattedPlayTime();
				saveinfo.Find("Location").GetComponent<Text>().text = gm.GetFileZone();
				characters.localPosition = new Vector3(0f, -37f);
				selVertical = false;
				state = State.ContinueGame;
				saveinfo.localPosition = Vector3.zero;
				saveCharacters.localPosition = Vector3.zero;
				characters.Find("Kris").GetComponent<Image>().enabled = gm.save.party[0] == 0;
				if ((int)gm.GetSaveFlag(102) == 1)
				{
					characters.Find("Kris").GetComponent<Image>().sprite = Resources.Load<Sprite>("player/Kris/injured/spr_kr_down_0_injured");
				}
				else if ((int)gm.GetSaveFlag(204) == 1)
				{
					characters.Find("Kris").GetComponent<Image>().sprite = Resources.Load<Sprite>("player/Kris/eye/spr_kr_down_0_eye");
				}
				characters.Find("Susie").GetComponent<Image>().enabled = gm.save.party[1] == 1 || gm.save.party[2] == 1;
				saveCharacters.Find("Toriel").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(8) == 1 && (int)gm.GetSaveFlag(56) == 0;
				saveCharacters.Find("Noelle").GetComponent<Image>().enabled = gm.save.party[1] == 2 || gm.save.party[2] == 2;
				saveCharacters.Find("Sans").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(60) == 1;
				saveCharacters.Find("Mom").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(84) > 0 && ((int)gm.GetSaveFlag(154) == 0 || (int)gm.GetSaveFlag(87) >= 5);
				saveCharacters.Find("Ralsei").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(33) == 1 || (int)gm.GetSaveFlag(66) == 1;
				saveCharacters.Find("Paula").GetComponent<Image>().enabled = gm.save.party[3] == 3;
				saveCharacters.Find("Frisk").GetComponent<Image>().enabled = gm.save.party[0] == 6;
				saveCharacters.Find("Ness").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(154) != 0 && (int)gm.GetSaveFlag(87) >= 5;
				saveCharacters.Find("TorielS2").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(154) != 0 && (int)gm.GetSaveFlag(87) < 5;
				saveCharacters.Find("Papyrus").GetComponent<Image>().enabled = gm.GetSaveFlagInt(281) == 2;
				if (usingNewTitle)
				{
					string mapSavePlatform = MapInfo.GetMapSavePlatform(gm.GetFileZoneIndex());
					if (mapSavePlatform != "")
					{
						savePlatform.sprite = Resources.Load<Sprite>("ui/title/spr_save_platform_" + mapSavePlatform);
						savePlatform.enabled = true;
					}
					else
					{
						savePlatform.enabled = false;
					}
				}
				return;
			}
			gm.SetDefaultValues();
			characters.localPosition = new Vector3(117f, -96f);
			selVertical = true;
			state = State.NewGame;
			gamerules.localPosition = Vector3.zero;
			if (UTInput.joystickIsActive)
			{
				string text = "     - Confirm\n     - Cancel\n     - Menu (In-game)\n[F4] - Fullscreen\n[Hold ESC] - Quit\nWhen HP is 0, you lose.";
				gamerules.Find("Rules").GetComponent<Text>().text = text;
				string[] array = new string[3] { "Z", "X", "C" };
				for (int i = 0; i < 3; i++)
				{
					ButtonPrompts.UpdateImageWithGraphic(array[i], gamerules.Find(array[i]).GetComponent<Image>());
					gamerules.Find(array[i]).GetComponent<Image>().enabled = true;
				}
				if (mobile)
				{
					gamerules.Find("Rules").GetComponent<Text>().text = gamerules.Find("Rules").GetComponent<Text>().text.Replace("\n[F4] - Fullscreen\n[Hold ESC] - Quit", "");
				}
				return;
			}
			string format = "[{0} or ENTER] - Confirm\n[{1} or SHIFT] - Cancel\n[{2} or CTRL] - Menu (In-game)\n[F4] - Fullscreen\n[Hold ESC] - Quit\nWhen HP is 0, you lose.";
			gamerules.Find("Rules").GetComponent<Text>().text = string.Format(format, UTInput.GetKeyName("Confirm"), UTInput.GetKeyName("Cancel"), UTInput.GetKeyName("Menu"));
			gamerules.Find("Z").GetComponent<Image>().enabled = false;
			gamerules.Find("X").GetComponent<Image>().enabled = false;
			gamerules.Find("C").GetComponent<Image>().enabled = false;
			if (mobile)
			{
				gamerules.Find("Rules").GetComponent<Text>().text = gamerules.Find("Rules").GetComponent<Text>().text.Replace("\n[F4] - Fullscreen\n[Hold ESC] - Quit", "");
				if (UTInput.touchpadIsActive)
				{
					gamerules.Find("Rules").GetComponent<Text>().text = gamerules.Find("Rules").GetComponent<Text>().text.Replace(" or ENTER", "").Replace(" or SHIFT", "").Replace(" or CTRL", "");
				}
			}
		}

		public void LoadOptions()
		{
			index = 0;
			base.transform.Find("Copyright").GetComponent<Text>().enabled = false;
			if (usingNewTitle)
			{
				savePlatform.enabled = false;
			}
			deleteSave.localPosition = new Vector3(640f, 0f);
			gamerules.localPosition = new Vector3(640f, 0f);
			saveinfo.localPosition = new Vector3(640f, 0f);
			characters.localPosition = (mobile ? new Vector3(90f, -146f) : new Vector3(90f, -120f));
			saveCharacters.localPosition = new Vector3(640f, 0f);
			options.localPosition = Vector3.zero;
			saveFiles.localPosition = new Vector3(640f, 0f);
			selVertical = true;
			soulMoveRate = 0.25f;
			state = State.Options;
			menuLimit = 8;
		}

		public void LoadDeleteOption()
		{
			deleteText.StartText("\b    THIS DATA IS CORRUPT\n\b OR UNREACHABLE. DO YOU WISH\n\b TO TERMINATE ITS CONNECTION?", new Vector2(16f, 35f), "", 2);
			characters.localPosition = new Vector3(640f, 0f);
			options.localPosition = new Vector3(1640f, 0f);
			saveFiles.localPosition = new Vector3(640f, 0f);
			deleteSave.localPosition = Vector3.zero;
			deleteSave.GetChild(0).GetComponent<Text>().enabled = false;
			deleteSave.GetChild(1).GetComponent<Text>().enabled = false;
			soul.GetComponent<Image>().enabled = false;
			selVertical = false;
			state = State.CorruptSave;
			selecting = true;
			menuLimit = 2;
			index = 0;
		}

		public void LoadSAVEFiles()
		{
			if ((bool)logo)
			{
				UnityEngine.Object.Destroy(logo.gameObject);
			}
			base.transform.Find("Copyright").GetComponent<Text>().enabled = true;
			deleteSave.localPosition = new Vector3(640f, 0f);
			gamerules.localPosition = new Vector3(640f, 0f);
			saveinfo.localPosition = new Vector3(640f, 0f);
			characters.localPosition = new Vector3(640f, 0f);
			saveCharacters.localPosition = new Vector3(640f, 0f);
			options.localPosition = new Vector3(1640f, 0f);
			saveFiles.localPosition = Vector3.zero;
			if (usingNewTitle)
			{
				savePlatform.enabled = false;
			}
			SetSAVEStrings();
			saveHeaderResetFrames = 90;
			Text[] componentsInChildren = saveFiles.GetChild(saveIndex + 1).GetComponentsInChildren<Text>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = Color.white;
			}
			Image[] componentsInChildren2 = saveFiles.GetChild(saveIndex + 1).GetComponentsInChildren<Image>();
			foreach (Image image in componentsInChildren2)
			{
				if (image.gameObject.name != "fg")
				{
					image.color = Color.white;
				}
			}
			characters.Find("Kris").GetComponent<Image>().sprite = Resources.Load<Sprite>("player/Kris/spr_kr_down_0");
			characters.Find("Kris").GetComponent<Image>().enabled = true;
			characters.Find("Susie").GetComponent<Image>().enabled = true;
			saveFiles.Find("Extras").GetComponent<Text>().color = ((PersistentSAVE.GetInt("new-extra", 0) == 1) ? new Color(1f, 1f, 0f) : Color.white);
			selVertical = false;
			savePages = 5;
			state = State.FileSelect;
		}

		public void SetSAVEStrings()
		{
			saves = new SAVEFile[3];
			for (int i = 0; i < 3; i++)
			{
				string path = Path.Combine(Application.persistentDataPath, "SAVE" + i + ".sav");
				if (File.Exists(path))
				{
					try
					{
						using FileStream fs = File.Open(path, FileMode.Open);
						fileStatuses[i] = SAVEFileIO.ReadFile(ref saves[i], fs);
					}
					catch (EndOfStreamException ex)
					{
						Debug.Log("File was unable to be read\n" + ex);
						fileStatuses[i] = FileStatus.Corrupted;
					}
					catch (Exception ex2)
					{
						Debug.Log("Failed to read file " + i + "... maybe it just doesn't exist?\n" + ex2);
						fileStatuses[i] = FileStatus.Empty;
					}
				}
				else
				{
					fileStatuses[i] = FileStatus.Empty;
				}
				Debug.Log("File " + i + " status: " + fileStatuses[i]);
				Transform transform = saveFiles.Find("file" + i);
				switch (fileStatuses[i])
				{
				case FileStatus.Empty:
					transform.Find("name").GetComponent<Text>().text = "[EMPTY]";
					transform.Find("time").GetComponent<Text>().text = "––:––";
					transform.Find("location").GetComponent<Text>().text = "------------";
					break;
				case FileStatus.Corrupted:
				case FileStatus.Newer:
					transform.Find("name").GetComponent<Text>().text = ((fileStatuses[i] == FileStatus.Corrupted) ? "[CORRUPTED]" : "[INCOMPATIBLE]");
					transform.Find("time").GetComponent<Text>().text = "––:––";
					transform.Find("location").GetComponent<Text>().text = "------------";
					break;
				case FileStatus.OK:
				case FileStatus.Older:
					transform.Find("name").GetComponent<Text>().text = saves[i].name;
					transform.Find("time").GetComponent<Text>().text = gm.GetFormattedPlayTimeFromTime(saves[i].playTime);
					transform.Find("location").GetComponent<Text>().text = MapInfo.GetMapName(saves[i].zone);
					break;
				default:
					transform.Find("name").GetComponent<Text>().text = "[???]";
					transform.Find("time").GetComponent<Text>().text = "––:––";
					transform.Find("location").GetComponent<Text>().text = "------------";
					break;
				}
			}
		}

		public void SaveSettings()
		{
			gm.config.SetInt("General", "Volume", volume);
			localOptions.SaveToConfig(ref gm.config);
			gm.config.Write();
		}

		public void UpdateSettingsText()
		{
			optionsTabs[0].Find("Volume").GetComponent<Text>().text = volume + "%";
			optionsTabs[0].Find("Scale").GetComponent<Text>().text = "x" + gm.GetWindowScale();
			optionsTabs[0].Find("Content").GetComponent<Text>().text = ((localOptions.contentSetting.value == 1) ? "Reduced Blood" : "Normal");
			optionsTabs[0].Find("AutoRun").GetComponent<Text>().text = ((localOptions.autoRun.value == 1) ? "ON" : "OFF");
			optionsTabs[0].Find("EasyMode").GetComponent<Text>().text = ((localOptions.easyMode.value == 1) ? "ON" : "OFF");
			optionsTabs[2].Find("AutoButton").GetComponent<Text>().text = ((localOptions.autoButton.value == 1) ? "ON" : "OFF");
			optionsTabs[2].Find("Buttons").GetComponent<Text>().text = (new string[5] { "XBOX", "PS4", "NINTENDO", "PS5", "PS3-" })[localOptions.buttonStyle.value];
			optionsTabs[2].Find("vSync").GetComponent<Text>().text = ((localOptions.vSync.value == 1) ? "ON" : "OFF");
			optionsTabs[2].Find("Debugger").GetComponent<Text>().text = ((localOptions.monitorInfo.value == 1) ? "ON" : "OFF");
		}

		private void UpdateControlText()
		{
			for (int i = 0; i < 7; i++)
			{
				controls.Find(i + "-Text").GetComponent<Text>().text = UTInput.GetKeyName(controlNames[i]);
				ButtonPrompts.UpdateImageWithGraphic(controlNames[i], controls.Find(i + "-Button").GetComponent<Image>());
			}
		}

		public void UpdateSensitivityText()
		{
			if (mobile)
			{
				optionsTabs[3].Find("TouchPadSensitivityValue").GetComponent<Text>().text = PlayerPrefs.GetInt("DPADSensitivity", 10).ToString();
			}
		}

		public void UpdateAllText()
		{
			saveHeaderText = "Please select a file.";
			for (int i = 0; i < 3; i++)
			{
				saveFiles.Find("file" + i).Find("name").GetComponent<Text>()
					.text = "[EMPTY]";
			}
			UpdateUnresponsive();
			string format = "[{0} or ENTER] - Confirm\n[{1} or SHIFT] - Cancel\n[{2} or CTRL] - Menu (In-game)\n[F4] - Fullscreen\n[Hold ESC] - Quit\nWhen HP is 0, you lose.";
			gamerules.Find("Rules").GetComponent<Text>().text = string.Format(format, UTInput.GetKeyName("Confirm"), UTInput.GetKeyName("Cancel"), UTInput.GetKeyName("Menu"));
		}

		public void LoadGMSettings()
		{
			Util.GameManager().LoadConfigData();
			localOptions = GameManager.GetOptions();
		}

		public bool RebindingKey()
		{
			if (state == State.Controls)
			{
				return rebinding;
			}
			return false;
		}

		public void CancelRebind()
		{
			string n = index.ToString();
			controls.Find(n).GetComponent<Text>().color = new Color(0f, 1f, 1f);
			selecting = true;
			rebinding = false;
		}

		private void UpdateUnresponsive()
		{
			bool joystickIsActive = UTInput.joystickIsActive;
			Text component = base.transform.Find("Unresponsive").GetComponent<Text>();
			if (joystickIsActive)
			{
				component.text = "[press    or    ]";
				ButtonPrompts.UpdateImageWithGraphic("Confirm", component.transform.Find("Confirm").GetComponent<Image>(), 2f, ButtonPrompts.ButtonType.Small);
				ButtonPrompts.UpdateImageWithGraphic("Menu", component.transform.Find("Menu").GetComponent<Image>(), 2f, ButtonPrompts.ButtonType.Small);
			}
			else
			{
				component.text = string.Format("[press {0} or enter]", UTInput.GetKeyName("Confirm").ToLower());
			}
			component.transform.Find("Confirm").GetComponent<Image>().enabled = joystickIsActive;
			component.transform.Find("Menu").GetComponent<Image>().enabled = joystickIsActive;
		}
	}
