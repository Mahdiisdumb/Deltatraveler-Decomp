using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Experimental.Input;
using UnityEngine.Experimental.Input.LowLevel;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TitleScreen : TranslatableSelectableBehaviour
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
		LanguagePacks = 12,
		NameConfirm = 13,
		Extras = 14,
		LoadNewDimension = 15
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

	private static readonly int FULL_COMPLETION = 3;

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

	private Transform langPackMenu;

	private Transform extras;

	private Transform kris;

	private Transform susie;

	private Transform noelle;

	private Transform door;

	private SpriteRenderer savePlatform;

	private Transform[] optionsTabs;

	private int optionsTab;

	private Transform flavorPreview;

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

	private bool trainingModeUnlocked;

	private bool unoUnlocked;

	private bool instantLoadHardMode;

	private bool flavorUnlocked;

	private bool marioBrosUnlocked;

	private string langPack = "";

	private LanguagePack[] packList;

	private int packIndex;

	private int packPage;

	private int packPageCount;

	private int lastPage;

	private int lastAxisX;

	private int controlsType;

	private bool rebinding;

	private FileStatus[] fileStatuses;

	private SOUL corruptSoul;

	private int loadToScene = -1;

	private static string[] controlNames = new string[7] { "Down", "Right", "Up", "Left", "Confirm", "Cancel", "Menu" };

	private bool holdHoriz;

	private static int comboProgress = 0;

	private static int[] correctMBCombo = new int[8] { 0, 1, 0, 0, 1, 2, 2, 3 };

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("title", new string[2] { "[press {0} or enter]", "Kris and Susie go places that\nthey shouldn't be at" });
		dictionary.Add("save_files", new string[15]
		{
			"Please select a file.", "Choose a file to copy.", "Choose a file to copy to.", "It can't be copied.", "You can't copy there.", "Copy complete.", "The file will be overwritten.", "Choose a file to erase.", "There's nothing to erase.", "Erase complete.",
			"Copy", "Erase", "Options", "Cancel", "It can't be loaded."
		});
		dictionary.Add("pack_menu", new string[3] { "--- Language Packs ---", "[press {0} to go back]", "<   PAGE {0}   >" });
		dictionary.Add("instructions", new string[5] { "--- Instruction ---", "[{0} or ENTER] - Confirm\n[{1} or SHIFT] - Cancel\n[{2} or CTRL] - Menu (In-game)\n[F4] - Fullscreen\n[Hold ESC] - Quit\nWhen HP is 0, you lose.", "Begin Game", "Back", "     - Confirm\n     - Cancel\n     - Menu (In-game)\n[F4] - Fullscreen\n[Hold ESC] - Quit\nWhen HP is 0, you lose." });
		dictionary.Add("options", new string[21]
		{
			"--- Options ---", "Master Volume", "Controls", "Window Scale", "Language Packs...", "Go Back", "Content Setting", "Low Graphics", "Visuals...", "Touchpad Settings...",
			"Touch Color Scheme", "Touchpad Skin", "Buttons Skin", "Touchpad Resistance", "lower value = more sensitive", "Starting Flavor", "Auto-Run", "Gamepad Button Style", "Run Animations", "Automatic Button Style",
			"Easy Mode"
		});
		dictionary.Add("content_settings", new string[2] { "Normal", "Reduced Blood" });
		dictionary.Add("low_graphics", new string[2] { "OFF", "ON" });
		dictionary.Add("button_styles", new string[3] { "XBOX", "PS4", "SWITCH" });
		dictionary.Add("flavors", new string[11]
		{
			"Vanilla", "Mint", "Strawberry", "Banana", "ButtsPie", "Blueberry", "Cinnamon", "Moss", "Nerds", "Cotton Candy",
			"Eggplant"
		});
		dictionary.Add("controls", new string[12]
		{
			"Function", "Key", "DOWN", "RIGHT", "UP", "LEFT", "CONFIRM", "CANCEL", "MENU", "Reset to default",
			"Finish", "Gamepad"
		});
		dictionary.Add("name_selection", new string[15]
		{
			"BACK", "END", "Yes", "No", "\b          YOUR OWN NAME.", "\b        WHAT AN INTERESTING \n\b             BEHAVIOR.", "\b    AN INTERESTING COINCIDENCE.", "\b     THEY CANNOT HEAR YOU HERE.", "\b      THIS NAME SHALL COMMENCE\n\b     AN INTERESTING EXPERIMENT.", "\b               ...",
			"\b        THIS IS YOUR NAME.", "\b       IS THIS HOW YOU INTEND \n\b       TO SPEND PRECIOUS TIME?", "\b         A NAME HAS ALREADY \n\b         BEEN CHOSEN.", "\b     INTERESTING... YOU INVOKE\n\b       THE NAME OF JUSTICE.", "\b     WHAT A DISTINGUISHED NAME."
		});
		dictionary.Add("file_options", new string[14]
		{
			"Erase this file?", "Really erase it?", "Copy over this file?", "Yes", "No", "Yes!", "No!", "[EMPTY]", "[CORRUPTED]", "\b    THIS DATA IS CORRUPT\n\b OR UNREACHABLE. DO YOU WISH\n\b TO TERMINATE ITS CONNECTION?",
			"--- WARNING ---", "ERASE", "DO NOT", "[INCOMPATIBLE]"
		});
		dictionary.Add("extras", new string[6] { "--- Extras ---", "Hard Mode", "Training Mode", "UNOTRAVELER", "Mario Bros.", "Go Back" });
		dictionary.Add("easy_mode", new string[2] { "OFF", "ON" });
		return dictionary;
	}

	private void Awake()
	{
		gm = UnityEngine.Object.FindObjectOfType<GameManager>();
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
		langPackMenu = base.transform.Find("LanguageSelector");
		extras = base.transform.Find("Extras");
		optionsTabs = new Transform[4]
		{
			options.Find("MainTab"),
			null,
			options.Find("VisualsTab"),
			options.Find("MobileButtonsTab")
		};
		flavorPreview = optionsTabs[2].Find("FlavorPreview");
		selection = base.gameObject.AddComponent<Selection>();
		deleteText = base.gameObject.AddComponent<TextUT>();
		deleteText.EnableGasterEffect();
		fileStatuses = new FileStatus[3];
		gm.SetFramerate(30);
		UTInput.SetPriority(true);
		if (UnityEngine.Random.Range(0, 1200) == 0 || (DateTime.Now.Day == 1 && DateTime.Now.Month == 4))
		{
			logo.sprite = Resources.Load<Sprite>("ui/title/anagrams/spr_logo_anagram_" + UnityEngine.Random.Range(0, 3));
		}
		UpdateAllText();
	}

	private void Start()
	{
		gm.DisableSingleBattleMode();
		fade = UnityEngine.Object.FindObjectOfType<Fade>();
		fade.transform.parent.position = Vector3.zero;
		base.transform.Find("Copyright").GetComponent<Text>().text = base.transform.Find("Copyright").GetComponent<Text>().text.Replace("VER", gm.GetVersion());
		volume = gm.config.GetInt("General", "Volume", 100);
		langPack = gm.config.GetString("General", "LanguagePack", "");
		windowScale = PlayerPrefs.GetInt("WindowScale", 1);
		localOptions = new Options();
		localOptions.LoadFromConfig(ref gm.config);
		flavorUnlocked = PlayerPrefs.GetInt("CompletionState", 0) >= 2;
		if (!flavorUnlocked)
		{
			UnityEngine.Object.Destroy(optionsTabs[2].Find("4").gameObject);
			UnityEngine.Object.Destroy(optionsTabs[2].Find("Flavor").gameObject);
			optionsTabs[2].Find("5").gameObject.name = "4";
		}
		UpdateSettingsText();
		if (PlayerPrefs.GetInt("NewTitleScreen", 0) == 1 && PlayerPrefs.GetInt("CompletionState", 0) == 0)
		{
			PlayerPrefs.SetInt("CompletionState", 1);
		}
		trainingModeUnlocked = PlayerPrefs.GetInt("CompletionState", 0) == FULL_COMPLETION || gm.IsTestMode();
		unoUnlocked = PlayerPrefs.GetInt("CompletionState", 0) >= 3 || gm.IsTestMode();
		marioBrosUnlocked = PlayerPrefs.GetInt("MBUnlocked", 0) == 1 || gm.IsTestMode();
		if (trainingModeUnlocked)
		{
			extras.Find("1").GetComponent<Text>().text = GetString("extras", 2);
			extras.Find("1").GetComponent<Text>().color = Color.white;
		}
		if (unoUnlocked)
		{
			extras.Find("2").GetComponent<Text>().text = GetString("extras", 3);
			extras.Find("2").GetComponent<Text>().color = Color.white;
		}
		if (marioBrosUnlocked)
		{
			extras.Find("3").GetComponent<Text>().text = GetString("extras", 4);
			extras.Find("3").GetComponent<Text>().color = Color.white;
		}
		if (PlayerPrefs.GetInt("CompletionState", 0) >= 1)
		{
			mus.clip = Resources.Load<AudioClip>("music/mus_castletown");
			kris = GameObject.Find("KrisBG").transform;
			kris.GetComponent<SpriteRenderer>().enabled = true;
			if (PlayerPrefs.GetInt("KrisEye", 0) == 1)
			{
				kris.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("ui/title/spr_kris_title");
			}
			susie = GameObject.Find("SusieBG").transform;
			susie.GetComponent<SpriteRenderer>().enabled = true;
			noelle = GameObject.Find("NoelleBG").transform;
			noelle.GetComponent<SpriteRenderer>().enabled = true;
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
			UnityEngine.Object.FindObjectOfType<MobileUI>().EnableButtons(true, true, true, true, false);
			optionsTabs[3].Find("Color").GetComponent<Text>().text = UnityEngine.Object.FindObjectOfType<MobileUI>().GetCurrentColorName();
			optionsTabs[3].Find("DPADSkin").GetComponent<Text>().text = UnityEngine.Object.FindObjectOfType<MobileUI>().GetCurrentPadSkin();
			optionsTabs[3].Find("ButtonSkin").GetComponent<Text>().text = UnityEngine.Object.FindObjectOfType<MobileUI>().GetCurrentButtonSkin();
			optionsTabs[3].Find("TouchPadSensitivityValue").GetComponent<Text>().text = PlayerPrefs.GetInt("DPADSensitivity", 10).ToString();
		}
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
				else if (state == State.Options && flavorUnlocked && optionsTab == 2)
				{
					flavorPreview.gameObject.SetActive(index == 4);
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
				door.position = Vector3.Lerp(door.position, Vector3.zero, 0.2f);
				frames++;
				soul.transform.localPosition = new Vector3(0f, -192f + Mathf.Sin((float)(frames * 2) * ((float)Math.PI / 180f)) * 6f);
			}
			if (UTInput.GetButtonDown("Z"))
			{
				if (usingNewTitle)
				{
					kris.GetComponent<SpriteRenderer>().enabled = false;
					susie.GetComponent<SpriteRenderer>().enabled = false;
					noelle.GetComponent<SpriteRenderer>().enabled = false;
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
				base.transform.Find("Unresponsive").GetComponent<Text>().enabled = false;
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
				UnityEngine.Object.FindObjectOfType<EndNameEvent>().transform.Find("Kris").position = new Vector3(0f, -2.2f);
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
				gm.SpawnFromLastSave(false);
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
						flavorPreview.gameObject.SetActive(false);
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
					optionsTabs[optionsTab].gameObject.SetActive(false);
					optionsTab = 0;
					optionsTabs[optionsTab].gameObject.SetActive(true);
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
							windowScale = PlayerPrefs.GetInt("WindowScale") + 1;
							Resolution currentResolution = Screen.currentResolution;
							if (windowScale * 640 > currentResolution.width || windowScale * 480 > currentResolution.height)
							{
								windowScale = 1;
							}
							if (PlayerPrefs.GetInt("fullscreen") != 1)
							{
								Screen.SetResolution(640 * windowScale, 480 * windowScale, false);
							}
							PlayerPrefs.SetInt("WindowScale", windowScale);
							UpdateSettingsText();
						}
						else
						{
							optionsTabs[0].gameObject.SetActive(false);
							optionsTabs[3].gameObject.SetActive(true);
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
						optionsTabs[0].gameObject.SetActive(false);
						optionsTabs[2].gameObject.SetActive(true);
						menuLimit = (flavorUnlocked ? 6 : 5);
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
						localOptions.lowGraphics.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					if (index == 1)
					{
						selSfx.Play();
						localOptions.autoButton.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					if (index == 2)
					{
						selSfx.Play();
						localOptions.buttonStyle.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					if (index == 3)
					{
						selSfx.Play();
						localOptions.runAnimations.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
					if (index == 4 && flavorUnlocked)
					{
						selSfx.Play();
						localOptions.startingFlavor.Increase();
						GameManager.SetOptions(localOptions);
						UpdateSettingsText();
					}
				}
				else if (optionsTab == 3)
				{
					if (index == 0)
					{
						selSfx.Play();
						UnityEngine.Object.FindObjectOfType<MobileUI>().CycleButtonColors();
						optionsTabs[3].Find("Color").GetComponent<Text>().text = UnityEngine.Object.FindObjectOfType<MobileUI>().GetCurrentColorName();
					}
					else if (index == 1)
					{
						selSfx.Play();
						UnityEngine.Object.FindObjectOfType<MobileUI>().CyclePadSkin();
						optionsTabs[3].Find("DPADSkin").GetComponent<Text>().text = UnityEngine.Object.FindObjectOfType<MobileUI>().GetCurrentPadSkin();
					}
					else if (index == 2)
					{
						selSfx.Play();
						UnityEngine.Object.FindObjectOfType<MobileUI>().CycleButtonSkin();
						optionsTabs[3].Find("ButtonSkin").GetComponent<Text>().text = UnityEngine.Object.FindObjectOfType<MobileUI>().GetCurrentButtonSkin();
					}
				}
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
				UnityEngine.Object.FindObjectOfType<EndNameEvent>().StartNameShake();
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
					deleteText.StartText(GetString("name_selection", 11), new Vector2(0f, 111f), "", 2);
				}
				else if (list.Contains(text4))
				{
					deleteText.StartText(GetString("name_selection", 6), new Vector2(0f, 111f), "", 2);
				}
				else
				{
					switch (text4)
					{
					case "KRIS":
						deleteText.StartText(GetString("name_selection", 7), new Vector2(0f, 111f), "", 2);
						break;
					case "FRISK":
						deleteText.StartText(GetString("name_selection", 8), new Vector2(0f, 111f), "", 2);
						break;
					case "DESS":
						deleteText.StartText(GetString("name_selection", 9), new Vector2(0f, 111f), "", 2);
						break;
					case "CLOVER":
						deleteText.StartText(GetString("name_selection", 13), new Vector2(0f, 111f), "", 2);
						break;
					case "CEROBA":
						deleteText.StartText(GetString("name_selection", 14), new Vector2(0f, 111f), "", 2);
						break;
					default:
						deleteText.StartText(GetString("name_selection", 10), new Vector2(0f, 111f), "", 2);
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
					if (instantLoadHardMode)
					{
						gm.SetFileID(3);
					}
					deleteText.DestroyOldText();
					gm.SetPlayerName(letters.Find("Name").Find("Text").GetComponent<Text>()
						.text);
						if (gm.GetPlayerName() == "FRISK")
						{
							gm.SetFlag(107, 1);
							gm.SetFlag(108, 1);
							gm.ForceWeapon(0, 25);
						}
						gm.SetFlag(223, localOptions.startingFlavor.value);
						UnityEngine.Object.FindObjectOfType<EndNameEvent>().StartScene(gm.GetPlayerName());
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
						gm.SetPartyMembers(true, false);
						selecting = false;
						UnityEngine.Object.FindObjectOfType<Fade>().UTFadeOut();
					}
					else if (instantLoadHardMode)
					{
						UnityEngine.Object.FindObjectOfType<EndNameEvent>().StopNameShake();
						deleteText.DestroyOldText();
						LoadExtras();
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
							deleteText.StartText(GetString("name_selection", 5), new Vector2(0f, 149f), "", 2);
						}
						else
						{
							deleteText.StartText(GetString("name_selection", 4), new Vector2(0f, 111f), "", 2);
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
				if (!UnityEngine.Object.FindObjectOfType<Fade>().IsPlaying())
				{
					gm.StartTime();
					gm.LoadArea(7, true, new Vector3(0.16f, -0.08f), Vector2.down);
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
					corruptSoul.CreateSOUL(Color.red, false, false);
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
					UnityEngine.Object.FindObjectOfType<SOUL>().Break();
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
								saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 14);
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
								saveFiles.Find("0").GetComponent<Text>().text = GetString("save_files", 13);
								saveFiles.Find("1").GetComponent<Text>().enabled = false;
								saveFiles.Find("2").GetComponent<Text>().enabled = false;
								saveFiles.Find("Extras").GetComponent<Text>().enabled = false;
								saveHeaderText = GetString("save_files", 1);
								saveState = SaveState.CopyFrom;
							}
							if (index == 1)
							{
								index = 0;
								saveIndex = 0;
								saveFiles.Find("0").GetComponent<Text>().text = GetString("save_files", 13);
								saveFiles.Find("1").GetComponent<Text>().enabled = false;
								saveFiles.Find("2").GetComponent<Text>().enabled = false;
								saveFiles.Find("Extras").GetComponent<Text>().enabled = false;
								saveHeaderText = GetString("save_files", 7);
								saveState = SaveState.DeleteSelect;
							}
							if (index == 2)
							{
								LoadOptions();
							}
						}
						else if (saveIndex == 4)
						{
							LoadExtras();
							selSfx.Play();
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
							saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 3);
						}
						else
						{
							selSfx.Play();
							copiedSaveIndex = saveIndex;
							saveIndex = 0;
							saveState = SaveState.CopyTo;
							saveHeaderText = GetString("save_files", 2);
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
						saveHeaderText = GetString("save_files", 1);
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						if (saveIndex == copiedSaveIndex)
						{
							backSfx.Play();
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 4);
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
							saveHeaderText = GetString("save_files", 6);
							saveState = SaveState.CopyOverwriteConfirm;
							saveFiles.GetChild(saveIndex + 1).Find("name").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("time").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("location").GetComponent<Text>()
								.enabled = false;
							saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
								.text = GetString("file_options", 2);
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = GetString("file_options", 3);
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = GetString("file_options", 4);
							selecting = true;
							menuLimit = 2;
						}
						else
						{
							gm.PlayGlobalSFX("sounds/snd_appearance");
							gm.CopyFile(copiedSaveIndex, saveIndex);
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 5);
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
						saveHeaderText = GetString("save_files", 1);
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
							saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 5);
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
							saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 8);
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
								.text = GetString("file_options", 0);
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = GetString("file_options", 3);
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = GetString("file_options", 4);
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
						saveHeaderText = GetString("save_files", 7);
					}
					else if (UTInput.GetButtonDown("Z"))
					{
						if (index == 0)
						{
							selSfx.Play();
							saveState = SaveState.DeleteDoubleConfirm;
							saveFiles.GetChild(saveIndex + 1).Find("erasetext").GetComponent<Text>()
								.text = GetString("file_options", 1);
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(0)
								.GetComponent<Text>()
								.text = GetString("file_options", 5);
							saveFiles.GetChild(saveIndex + 1).Find("selection").GetChild(1)
								.GetComponent<Text>()
								.text = GetString("file_options", 6);
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
						saveHeaderText = GetString("save_files", 7);
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
							component.CreateSOUL(Color.red, false, false);
							component.Break();
							gm.PlayGlobalSFX("sounds/snd_appearance");
							gm.DeleteFile(saveIndex);
							saveHeaderResetFrames = 0;
							saveFiles.GetChild(0).GetComponent<Text>().text = GetString("save_files", 9);
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
			else if (state == State.LanguagePacks)
			{
				int num2 = (int)UTInput.GetAxisRaw("Horizontal");
				if (lastAxisX != num2 && num2 != 0)
				{
					packPage += num2;
					if (packPage < 0)
					{
						packPage = packPageCount - 1;
					}
					if (packPage > packPageCount - 1)
					{
						packPage = 0;
					}
					menuSfx.Play();
					UpdatePackList(false);
				}
				lastAxisX = num2;
				if (UTInput.GetButtonDown("X"))
				{
					backSfx.Play();
					LoadOptions();
				}
				packIndex = index + packPage * 3;
				if (UTInput.GetButtonDown("Z"))
				{
					selSfx.Play();
					Util.PackManager().SetPack(packList[packIndex].GetFileNameWithoutExtension());
					UpdatePackList(false);
					UpdateAllText();
				}
				langPack = Util.PackManager().GetPackName();
				soul.localPosition = Vector3.Lerp(soul.localPosition, langPackMenu.Find("pack" + index).Find("soulpos").transform.position * 48f, 0.4f);
			}
			else if (state == State.Extras)
			{
				soul.localPosition = Vector3.Lerp(soul.localPosition, new Vector3(extras.GetChild(index).localPosition.x - 36f, extras.GetChild(index).localPosition.y + 16f), 0.4f);
				if (UTInput.GetButtonDown("X") || (UTInput.GetButtonDown("Z") && index == menuLimit - 1))
				{
					LoadSAVEFiles();
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
					if (index == 0)
					{
						selSfx.Play();
						LoadHardModeName();
					}
					else if (index == 1 || index == 2)
					{
						if (index == 1 && trainingModeUnlocked)
						{
							gm.SetDefaultValues();
							gm.SetPartyMembers(true, true);
							gm.SetFlag(223, localOptions.startingFlavor.value);
							gm.StartSingleBattle(55);
						}
						if (index == 2 && unoUnlocked)
						{
							gm.SetDefaultValues();
							gm.SetFlag(204, 1);
							gm.SetFlag(223, localOptions.startingFlavor.value);
							gm.SetFlag(292, 1);
							for (int n = 0; n < 5; n++)
							{
								gm.SetFlag(307 + n, 1);
							}
							gm.SetSessionFlag(17, 1);
							gm.SetPartyMembers(true, true);
							gm.LockMenu();
							gm.LoadArea(116, true, new Vector2(0f, -3.745f), Vector2.up);
						}
					}
					else if (index == 3 && marioBrosUnlocked)
					{
						loadToScene = 103;
						frames = 0;
						selSfx.Play();
						mus.Stop();
						fade.FadeOut(20, Color.white);
						state = State.LoadNewDimension;
					}
					else
					{
						Util.GameManager().PlayGlobalSFX("sounds/snd_cantselect");
					}
				}
				if (index == 3 && !marioBrosUnlocked && trainingModeUnlocked)
				{
					if (UTInput.GetButtonDown("Z"))
					{
						HandleMarioBrosCode(2);
					}
					else if (UTInput.GetButtonDown("C"))
					{
						HandleMarioBrosCode(3);
					}
					if (holdHoriz && UTInput.GetAxis("Horizontal") == 0f)
					{
						holdHoriz = false;
					}
					else if (!holdHoriz && UTInput.GetAxis("Horizontal") != 0f)
					{
						holdHoriz = true;
						if (UTInput.GetAxis("Horizontal") > 0f)
						{
							HandleMarioBrosCode(1);
						}
						if (UTInput.GetAxis("Horizontal") < 0f)
						{
							HandleMarioBrosCode(0);
						}
					}
				}
				if (marioBrosUnlocked)
				{
					if (index == 3 && !extras.Find("MBInfo").GetComponentInChildren<Text>().enabled)
					{
						Text[] componentsInChildren = extras.Find("MBInfo").GetComponentsInChildren<Text>();
						for (int m = 0; m < componentsInChildren.Length; m++)
						{
							componentsInChildren[m].enabled = true;
						}
					}
					else if (index != 3 && extras.Find("MBInfo").GetComponentInChildren<Text>().enabled)
					{
						Text[] componentsInChildren = extras.Find("MBInfo").GetComponentsInChildren<Text>();
						for (int m = 0; m < componentsInChildren.Length; m++)
						{
							componentsInChildren[m].enabled = false;
						}
					}
				}
			}
			else if (state == State.LoadNewDimension && !UnityEngine.Object.FindObjectOfType<Fade>().IsPlaying())
			{
				frames++;
				if (frames >= 10)
				{
					if (loadToScene == -1)
					{
						gm.SpawnFromLastSave(false);
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
			saveFiles.Find("0").GetComponent<Text>().text = GetString("save_files", 10);
			saveFiles.Find("1").GetComponent<Text>().enabled = true;
			saveFiles.Find("2").GetComponent<Text>().enabled = true;
			saveFiles.Find("Extras").GetComponent<Text>().enabled = true;
			saveHeaderText = GetString("save_files", 0);
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
				characters.Find("Kris").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(107) == 0;
				if ((int)gm.GetSaveFlag(102) == 1)
				{
					characters.Find("Kris").GetComponent<Image>().sprite = Resources.Load<Sprite>("player/Kris/injured/spr_kr_down_0_injured");
				}
				else if ((int)gm.GetSaveFlag(204) == 1)
				{
					characters.Find("Kris").GetComponent<Image>().sprite = Resources.Load<Sprite>("player/Kris/eye/spr_kr_down_0_eye");
				}
				characters.Find("Susie").GetComponent<Image>().enabled = gm.save.susieActive;
				saveCharacters.Find("Toriel").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(8) == 1 && (int)gm.GetSaveFlag(56) == 0;
				saveCharacters.Find("Noelle").GetComponent<Image>().enabled = gm.save.noelleActive;
				saveCharacters.Find("Sans").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(60) == 1;
				saveCharacters.Find("Mom").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(84) > 0 && ((int)gm.GetSaveFlag(154) == 0 || (int)gm.GetSaveFlag(87) >= 5);
				saveCharacters.Find("Ralsei").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(33) == 1 || (int)gm.GetSaveFlag(66) == 1;
				saveCharacters.Find("Paula").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(86) == 1;
				saveCharacters.Find("Frisk").GetComponent<Image>().enabled = (int)gm.GetSaveFlag(107) == 1;
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
				gamerules.Find("Rules").GetComponent<Text>().text = GetString("instructions", 4);
				string[] array = new string[3] { "Z", "X", "C" };
				string[] array2 = new string[3] { "Confirm", "Cancel", "Menu" };
				for (int i = 0; i < 3; i++)
				{
					for (int j = 0; j < ButtonPrompts.validButtons.Length; j++)
					{
						if (UTInput.GetKeyOrButtonReplacement(array2[i]) == ButtonPrompts.GetButtonChar(ButtonPrompts.validButtons[j]))
						{
							gamerules.Find(array[i]).GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/buttons/" + ButtonPrompts.GetButtonGraphic(ButtonPrompts.validButtons[j]));
							gamerules.Find(array[i]).GetComponent<Image>().enabled = true;
							break;
						}
					}
				}
				if (mobile)
				{
					gamerules.Find("Rules").GetComponent<Text>().text = gamerules.Find("Rules").GetComponent<Text>().text.Replace("\n[F4] - Fullscreen\n[Hold ESC] - Quit", "");
				}
				return;
			}
			gamerules.Find("Rules").GetComponent<Text>().text = string.Format(GetString("instructions", 1), UTInput.GetKeyName("Confirm"), UTInput.GetKeyName("Cancel"), UTInput.GetKeyName("Menu"));
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
			langPackMenu.localPosition = new Vector3(640f, 0f);
			selVertical = true;
			soulMoveRate = 0.25f;
			state = State.Options;
			menuLimit = 8;
		}

		public void LoadDeleteOption()
		{
			deleteText.StartText(GetString("file_options", 9), new Vector2(16f, 35f), "", 2);
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

		public void LoadLanguagePackOptions()
		{
			packIndex = 0;
			characters.localPosition = new Vector3(640f, 0f);
			options.localPosition = new Vector3(640f, 0f);
			langPackMenu.localPosition = Vector2.zero;
			state = State.LanguagePacks;
			packList = Util.PackManager().GetPacks(true);
			packPage = 0;
			packPageCount = (int)Mathf.Ceil((float)packList.Length / 3f);
			UpdatePackList(true);
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
			extras.localPosition = new Vector3(640f, 0f);
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
			selVertical = false;
			savePages = 5;
			state = State.FileSelect;
		}

		public void LoadExtras()
		{
			base.transform.Find("Copyright").GetComponent<Text>().enabled = false;
			if (instantLoadHardMode)
			{
				backSfx.Play();
				for (int i = 0; i < 28; i++)
				{
					letters.GetChild(i).GetComponent<Text>().enabled = true;
				}
				letters.Find("Name").Find("Text").GetComponent<Text>()
					.text = "";
				letters.GetChild(29).GetComponent<Text>().enabled = false;
				letters.GetChild(29).GetComponent<Text>().color = Color.white;
				letters.GetChild(30).GetComponent<Text>().enabled = false;
				letters.GetChild(30).GetComponent<Text>().color = Color.white;
				letters.localPosition = new Vector3(640f, 0f);
				soul.GetComponent<Image>().sprite = Resources.Load<Sprite>("battle/spr_soul");
				instantLoadHardMode = false;
				UnityEngine.Object.FindObjectOfType<EndNameEvent>().transform.Find("Kris").position = new Vector3(0f, -6000f);
			}
			index = 0;
			gamerules.localPosition = new Vector3(640f, 0f);
			saveFiles.localPosition = new Vector3(640f, 0f);
			saveinfo.localPosition = new Vector3(640f, 0f);
			characters.localPosition = new Vector3(640f, 0f);
			extras.localPosition = Vector3.zero;
			selecting = true;
			selVertical = true;
			soulMoveRate = 0.25f;
			state = State.Extras;
			menuLimit = 8;
		}

		public void LoadHardModeName()
		{
			instantLoadHardMode = true;
			letters.localPosition = Vector3.zero;
			for (int i = 0; i < 28; i++)
			{
				letters.GetChild(i).GetComponent<Text>().enabled = false;
			}
			letters.GetChild(29).GetComponent<Text>().enabled = true;
			letters.GetChild(29).GetComponent<Text>().color = new Color(1f, 1f, 0f);
			letters.GetChild(30).GetComponent<Text>().enabled = true;
			letters.GetChild(30).GetComponent<Text>().color = Color.white;
			UnityEngine.Object.FindObjectOfType<EndNameEvent>().StartNameShake();
			UnityEngine.Object.FindObjectOfType<EndNameEvent>().transform.Find("Kris").position = new Vector3(0f, -2.2f);
			extras.localPosition = new Vector3(640f, 0f);
			soul.GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/spr_blurry_soul");
			letters.Find("Name").Find("Text").GetComponent<Text>()
				.text = "FRISK";
			letters.Find("Name").Find("Text").GetComponent<Text>()
				.enabled = true;
			letters.Find("Name").Find("Text").localPosition = new Vector2(-letters.Find("Name").Find("Text").GetComponent<Text>()
				.text.Length * 7, 123f);
			letters.Find("Name").Find("Text").GetComponent<RectTransform>()
				.sizeDelta = new Vector2(letters.Find("Name").Find("Text").GetComponent<Text>()
				.text.Length * 16, 32f);
			deleteText.StartText(GetString("name_selection", 8), new Vector2(0f, 111f), "", 2);
			state = State.NameConfirm;
			selVertical = false;
			menuLimit = 2;
			index = 0;
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
						using (FileStream fs = File.Open(path, FileMode.Open))
						{
							fileStatuses[i] = SAVEFileIO.ReadFile(ref saves[i], fs);
						}
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
					transform.Find("name").GetComponent<Text>().text = GetString("file_options", 7);
					transform.Find("time").GetComponent<Text>().text = "––:––";
					transform.Find("location").GetComponent<Text>().text = "------------";
					break;
				case FileStatus.Corrupted:
				case FileStatus.Newer:
					transform.Find("name").GetComponent<Text>().text = GetString("file_options", (fileStatuses[i] == FileStatus.Corrupted) ? 8 : 13);
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
			gm.config.SetString("General", "LanguagePack", langPack);
			localOptions.SaveToConfig(ref gm.config);
			gm.config.Write();
		}

		public void UpdateSettingsText()
		{
			optionsTabs[0].Find("Volume").GetComponent<Text>().text = volume + "%";
			optionsTabs[0].Find("Scale").GetComponent<Text>().text = "x" + windowScale;
			optionsTabs[0].Find("Content").GetComponent<Text>().text = GetString("content_settings", localOptions.contentSetting.value);
			optionsTabs[0].Find("AutoRun").GetComponent<Text>().text = GetString("low_graphics", localOptions.autoRun.value);
			optionsTabs[0].Find("EasyMode").GetComponent<Text>().text = GetString("easy_mode", localOptions.easyMode.value);
			optionsTabs[2].Find("Graphics").GetComponent<Text>().text = GetString("low_graphics", localOptions.lowGraphics.value);
			optionsTabs[2].Find("AutoButton").GetComponent<Text>().text = GetString("low_graphics", localOptions.autoButton.value);
			optionsTabs[2].Find("Buttons").GetComponent<Text>().text = GetString("button_styles", localOptions.buttonStyle.value);
			optionsTabs[2].Find("RunAnimations").GetComponent<Text>().text = GetString("low_graphics", localOptions.runAnimations.value);
			if (flavorUnlocked)
			{
				int value = localOptions.startingFlavor.value;
				optionsTabs[2].Find("Flavor").GetComponent<Text>().text = GetString("flavors", value);
				flavorPreview.GetComponent<Image>().color = UIBackground.borderColors[value];
				flavorPreview.Find("SelText").GetComponent<Text>().color = Selection.selectionColors[value];
				flavorPreview.Find("TestButton").GetComponent<Image>().color = BattleButton.buttonColors[value];
				flavorPreview.Find("TestButtonSel").GetComponent<Image>().color = Selection.selectionColors[value];
			}
		}

		private void UpdateControlText()
		{
			for (int i = 0; i < 7; i++)
			{
				string text = i.ToString();
				controls.Find(text + "-Text").GetComponent<Text>().text = UTInput.GetKeyName(controlNames[i]);
			}
			for (int j = 0; j < 7; j++)
			{
				string text2 = j.ToString();
				Sprite sprite = Resources.Load<Sprite>("ui/buttons/" + ButtonPrompts.GetButtonGraphic(UTInput.GetButtonName(controlNames[j])));
				controls.Find(text2 + "-Button").GetComponent<Image>().sprite = sprite;
				controls.Find(text2 + "-Button").GetComponent<Image>().rectTransform.localScale = new Vector3(1f, 1f, 1f);
				controls.Find(text2 + "-Button").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(sprite.textureRect.width, sprite.textureRect.height) * 2f;
			}
		}

		public void UpdateSensitivityText()
		{
			if (mobile)
			{
				optionsTabs[3].Find("TouchPadSensitivityValue").GetComponent<Text>().text = PlayerPrefs.GetInt("DPADSensitivity", 10).ToString();
			}
		}

		public void UpdatePackList(bool force)
		{
			if (lastPage != packPage || force)
			{
				index = 0;
				menuLimit = 0;
			}
			langPackMenu.Find("PageNumber").GetComponent<Text>().text = string.Format(GetString("pack_menu", 2), (packPage + 1).ToString());
			for (int i = 0; i < 3; i++)
			{
				int num = packPage * 3 + i;
				if (num < packList.Length)
				{
					Transform transform = langPackMenu.Find("pack" + i);
					transform.gameObject.SetActive(true);
					transform.Find("Name").GetComponent<Text>().text = packList[num].GetPackInfo().language;
					transform.Find("Description").GetComponent<Text>().text = packList[num].GetPackInfo().description;
					if (lastPage != packPage || force)
					{
						menuLimit++;
					}
					if (packList[num].GetFileNameWithoutExtension() == Util.PackManager().GetPackName())
					{
						transform.Find("Name").GetComponent<Text>().color = new Color(0f, 1f, 0f, 1f);
						transform.Find("Description").GetComponent<Text>().color = new Color(0f, 1f, 0f, 0.75f);
					}
					else
					{
						transform.Find("Name").GetComponent<Text>().color = Color.white;
						transform.Find("Description").GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.75f);
					}
				}
				else
				{
					langPackMenu.Find("pack" + i).gameObject.SetActive(false);
				}
			}
		}

		public void UpdateAllText()
		{
			SetStrings(GetDefaultStrings(), GetType());
			saveHeaderText = GetString("save_files", 0);
			saveFiles.Find("Header").GetComponent<Text>().text = GetString("save_files", 0);
			saveFiles.Find("0").GetComponent<Text>().text = GetString("save_files", 10);
			saveFiles.Find("1").GetComponent<Text>().text = GetString("save_files", 11);
			saveFiles.Find("2").GetComponent<Text>().text = GetString("save_files", 12);
			for (int i = 0; i < 3; i++)
			{
				saveFiles.Find("file" + i).Find("name").GetComponent<Text>()
					.text = GetString("file_options", 7);
			}
			if ((bool)logo && (bool)logo.gameObject.transform.Find("Subtitle"))
			{
				logo.gameObject.transform.Find("Subtitle").GetComponent<Text>().text = GetString("title", 1);
			}
			base.gameObject.transform.Find("Unresponsive").GetComponent<Text>().text = string.Format(GetString("title", 0), UTInput.GetKeyName("Confirm").ToLower());
			langPackMenu.Find("PackHeader").GetComponent<Text>().text = GetString("pack_menu", 0);
			langPackMenu.Find("Unresponsive").GetComponent<Text>().text = string.Format(GetString("pack_menu", 1), UTInput.GetKeyName("Cancel").ToLower());
			gamerules.Find("Header").GetComponent<Text>().text = GetString("instructions", 0);
			gamerules.Find("Rules").GetComponent<Text>().text = string.Format(GetString("instructions", 1), UTInput.GetKeyName("Confirm"), UTInput.GetKeyName("Cancel"), UTInput.GetKeyName("Menu"));
			gamerules.Find("0").GetComponent<Text>().text = GetString("instructions", 2);
			gamerules.Find("1").GetComponent<Text>().text = GetString("instructions", 3);
			options.Find("OptionsHeader").GetComponent<Text>().text = GetString("options", 0);
			optionsTabs[0].Find("0").GetComponent<Text>().text = GetString("options", 1);
			optionsTabs[0].Find("1").GetComponent<Text>().text = GetString("options", (!mobile) ? 3 : 9);
			optionsTabs[0].Find("2").GetComponent<Text>().text = GetString("options", 2);
			if (mobile)
			{
				optionsTabs[0].Find("2").GetComponent<Text>().text = "Keyboard / Controller";
			}
			optionsTabs[0].Find("3").GetComponent<Text>().text = GetString("options", 6);
			optionsTabs[0].Find("4").GetComponent<Text>().text = GetString("options", 16);
			optionsTabs[0].Find("5").GetComponent<Text>().text = GetString("options", 20);
			optionsTabs[0].Find("6").GetComponent<Text>().text = GetString("options", 8);
			optionsTabs[0].Find("7").GetComponent<Text>().text = GetString("options", 5);
			optionsTabs[2].Find("0").GetComponent<Text>().text = GetString("options", 7);
			optionsTabs[2].Find("1").GetComponent<Text>().text = GetString("options", 19);
			optionsTabs[2].Find("2").GetComponent<Text>().text = GetString("options", 17);
			optionsTabs[2].Find("3").GetComponent<Text>().text = GetString("options", 18);
			optionsTabs[2].Find("4").GetComponent<Text>().text = GetString("options", 15);
			optionsTabs[2].Find("5").GetComponent<Text>().text = GetString("options", 5);
			optionsTabs[3].Find("0").GetComponent<Text>().text = GetString("options", 10);
			optionsTabs[3].Find("1").GetComponent<Text>().text = GetString("options", 11);
			optionsTabs[3].Find("2").GetComponent<Text>().text = GetString("options", 12);
			optionsTabs[3].Find("3").GetComponent<Text>().text = GetString("options", 5);
			letters.Find("BACK").GetComponent<Text>().text = GetString("name_selection", 0);
			letters.Find("END").GetComponent<Text>().text = GetString("name_selection", 1);
			letters.Find("0").GetComponent<Text>().text = GetString("name_selection", 2);
			letters.Find("1").GetComponent<Text>().text = GetString("name_selection", 3);
			if (!mobile)
			{
				controls.Find("Header-Key").GetComponent<Text>().text = GetString("controls", 1);
			}
			controls.Find("Header-Function").GetComponent<Text>().text = GetString("controls", 0);
			controls.Find("Header-Gamepad").GetComponent<Text>().text = GetString("controls", 11);
			controls.Find("0").GetComponent<Text>().text = GetString("controls", 2);
			controls.Find("1").GetComponent<Text>().text = GetString("controls", 3);
			controls.Find("2").GetComponent<Text>().text = GetString("controls", 4);
			controls.Find("3").GetComponent<Text>().text = GetString("controls", 5);
			controls.Find("4").GetComponent<Text>().text = GetString("controls", 6);
			controls.Find("5").GetComponent<Text>().text = GetString("controls", 7);
			controls.Find("6").GetComponent<Text>().text = GetString("controls", 8);
			controls.Find("7").GetComponent<Text>().text = GetString("controls", 9);
			controls.Find("8").GetComponent<Text>().text = GetString("controls", 10);
			extras.Find("ExtrasHeader").GetComponent<Text>().text = GetString("extras", 0);
			extras.Find("0").GetComponent<Text>().text = GetString("extras", 1);
			extras.Find("7").GetComponent<Text>().text = GetString("extras", 5);
			extras.Find("MBInfo").Find("Score").GetComponent<Text>()
				.text = PlayerPrefs.GetInt("MBScore", 20000).ToString();
			extras.Find("MBInfo").Find("Phase").GetComponent<Text>()
				.text = PlayerPrefs.GetInt("MBPhase", 3).ToString();
			deleteSave.Find("0").GetComponent<Text>().text = GetString("file_options", 11);
			deleteSave.Find("1").GetComponent<Text>().text = GetString("file_options", 12);
			deleteSave.Find("Header").GetComponent<Text>().text = GetString("file_options", 10);
		}

		public void LoadGMSettings()
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().LoadConfigData();
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
			string text = index.ToString();
			controls.Find(text).GetComponent<Text>().color = new Color(0f, 1f, 1f);
			if (!mobile)
			{
				controls.Find(text + "-Text").GetComponent<Text>().color = new Color(0f, 1f, 1f);
			}
			selecting = true;
			rebinding = false;
		}

		private void HandleMarioBrosCode(int input)
		{
			if (input == correctMBCombo[comboProgress])
			{
				comboProgress++;
				if (comboProgress == correctMBCombo.Length)
				{
					marioBrosUnlocked = true;
					extras.Find(index.ToString()).GetComponent<Text>().text = GetString("extras", 4);
					extras.Find(index.ToString()).GetComponent<Text>().color = Color.white;
					gm.PlayGlobalSFX("mariobros/sounds/snd_coin");
					PlayerPrefs.SetInt("MBUnlocked", 1);
				}
			}
			else
			{
				comboProgress = 0;
			}
		}
	}
