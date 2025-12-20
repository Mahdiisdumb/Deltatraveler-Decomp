using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ExtrasMenu : MonoBehaviour
{
	private enum State
	{
		Opening = 0,
		TabSelect = 1,
		OptionSelect = 2,
		FadingOut = 3,
		ConfirmName = 4,
		EndingsPage = 5,
		None = 6
	}

	private enum Tab
	{
		Visuals = 0,
		Minigames = 1,
		ExtraModes = 2
	}

	private enum Season
	{
		Winter = 0,
		Spring = 1,
		Summer = 2,
		Fall = 3
	}

	private enum EndingStatus
	{
		Locked = 0,
		HalfComplete = 1,
		Completed = 2
	}

	private static readonly string[] ENDINGS = new string[7] { "???", "???", "???", "???", "Gaster Blasted", "???", "???" };

	private static readonly string[] ENDING_DESCRIPTIONS = new string[7] { "Beated it", "Description goes here", "Description goes here", "Evil mode", "An experiment made\nin and for boredom", "awaaga", "..." };

	private static readonly string[] ENDING_IMAGES = new string[7] { "ui/title/endings/spr_ending_main", "ui/title/endings/spr_ending_unknown", "ui/title/endings/spr_ending_unknown", "ui/title/endings/spr_ending_unknown", "ui/title/endings/spr_ending_hardmode", "ui/title/endings/spr_ending_unknown", "ui/title/endings/spr_ending_unknown" };

	private static readonly Vector3 SOUL_OFFSET = new Vector3(-16f, 16f);

	private GameManager gm;

	private Fade fade;

	private AudioSource music;

	private AudioSource moveSound;

	private AudioSource selectSound;

	private AudioSource backSound;

	private AudioSource errorSound;

	private string musicToPlay = "";

	private Image curtainLeft;

	private Image curtainRight;

	private Image soul;

	private Transform mainScreen;

	private Transform nameScreen;

	private Transform endingsScreen;

	private Text flavorText;

	private Transform flavorPreview;

	private Transform marioBrosScore;

	private Animator charAnimator;

	private SpriteRenderer charSprite;

	private EndNameEvent endNameEvent;

	private TextUT nameText;

	private bool holdHoriz;

	private bool holdVert;

	private Options localOptions;

	private bool newTitle;

	private bool trainingModeUnlocked;

	private bool flavorUnlocked;

	private bool unoUnlocked;

	private bool marioBrosUnlocked;

	private string modeName = "";

	private bool nameConfirmed;

	private EndingStatus[] endingsStatus = new EndingStatus[ENDINGS.Length];

	private Season season;

	private State state;

	private int tab;

	private int option;

	private int optionLimit;

	private int transFrames;

	private int swirlingFrames;

	private int newScene;

	private int ending;

	private static int comboProgress = 0;

	private static int[] correctMBCombo = new int[8] { 0, 1, 0, 0, 1, 2, 2, 3 };

	private void Awake()
	{
		gm = Util.GameManager();
		fade = Util.FindObjectOfType<Fade>();
		fade.FadeIn(0);
		AudioSource[] components = GetComponents<AudioSource>();
		music = components[0];
		moveSound = components[1];
		selectSound = components[2];
		backSound = components[3];
		errorSound = components[4];
		curtainLeft = base.transform.Find("CurtainLeft").GetComponent<Image>();
		curtainRight = base.transform.Find("CurtainRight").GetComponent<Image>();
		soul = base.transform.Find("SOUL").GetComponent<Image>();
		mainScreen = base.transform.Find("MainScreen");
		nameScreen = base.transform.Find("NameScreen");
		endingsScreen = base.transform.Find("EndingsScreen");
		flavorText = mainScreen.Find("FlavorText").GetComponent<Text>();
		flavorPreview = mainScreen.Find("FlavorPreview");
		marioBrosScore = mainScreen.Find("MBInfo");
		charAnimator = GameObject.Find("Character").GetComponent<Animator>();
		charSprite = GameObject.Find("Character").GetComponent<SpriteRenderer>();
		endNameEvent = Util.FindObjectOfType<EndNameEvent>();
		endNameEvent.SetTextObject(nameScreen.Find("Name").GetComponent<RectTransform>());
		nameText = base.gameObject.AddComponent<TextUT>();
		nameText.EnableGasterEffect();
		newTitle = true;
		bool flag = gm.IsTestMode();
		trainingModeUnlocked = PersistentSAVE.GetInt("completion", 0) == GameManager.FULL_COMPLETION || flag;
		flavorUnlocked = PersistentSAVE.GetInt("completion", 0) >= 2 || flag;
		unoUnlocked = PersistentSAVE.GetInt("completion", 0) >= 3 || flag;
		marioBrosUnlocked = PersistentSAVE.GetInt("mario-unlocked", 0) == 1 || flag;
		int month = DateTime.Now.Month;
		if (month >= 12 && month <= 2)
		{
			season = Season.Winter;
		}
		if (month >= 3 && month <= 5)
		{
			season = Season.Spring;
		}
		if (month >= 6 && month <= 8)
		{
			season = Season.Summer;
		}
		if (month >= 9 && month <= 11)
		{
			season = Season.Fall;
		}
		localOptions = GameManager.GetOptions();
		DetermineEndingStatus();
		if (!newTitle)
		{
			charAnimator.enabled = false;
			charSprite.enabled = false;
			GameObject.Find("CharPlatform").GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("Deltarune").GetComponent<SpriteRenderer>().enabled = false;
			flavorText.enabled = false;
			state = State.TabSelect;
			UpdateOptions(select: false);
		}
		else
		{
			curtainLeft.enabled = true;
			curtainRight.enabled = true;
			state = State.Opening;
			music.Play();
			SetFlavorCharacter();
			SetSeason();
			UpdateOptions(select: false, init: true);
		}
		PersistentSAVE.SetInt("new-extra", 0);
	}

	private void Update()
	{
		swirlingFrames++;
		float f = (float)(-swirlingFrames) / 8f;
		flavorText.transform.localPosition = new Vector2(-42f, -175f) + new Vector2(Mathf.Sin(f), Mathf.Cos(f)) * 3f;
		switch (state)
		{
		case State.Opening:
		{
			transFrames++;
			Vector2 sizeDelta = Vector2.Lerp(new Vector2(640f, 480f), new Vector2(0f, 480f), (float)transFrames / 30f);
			curtainLeft.rectTransform.sizeDelta = sizeDelta;
			curtainRight.rectTransform.sizeDelta = sizeDelta;
			if (transFrames == 30)
			{
				state = State.TabSelect;
				curtainLeft.enabled = false;
				curtainRight.enabled = false;
				mainScreen.Find("GoBack").gameObject.SetActive(value: true);
				UpdateOptions(select: false);
				gm.PlayMusic(musicToPlay);
			}
			break;
		}
		case State.TabSelect:
			if (holdHoriz && UTInput.GetAxis("Horizontal") == 0f)
			{
				holdHoriz = false;
			}
			else if (!holdHoriz && UTInput.GetAxis("Horizontal") != 0f)
			{
				holdHoriz = true;
				if (UTInput.GetAxis("Horizontal") > 0f)
				{
					tab++;
					if (tab > 2)
					{
						tab = 0;
					}
				}
				if (UTInput.GetAxis("Horizontal") < 0f)
				{
					tab--;
					if (tab < 0)
					{
						tab = 2;
					}
				}
				moveSound.Play();
				UpdateOptions(select: false);
			}
			if (UTInput.GetButtonDown("Z"))
			{
				selectSound.Play();
				state = State.OptionSelect;
				UpdateOptions(select: true);
			}
			if (UTInput.GetButtonDown("X"))
			{
				selectSound.Play();
				UpdateOptions(select: false, init: true);
				state = State.FadingOut;
				newScene = 6;
				fade.FadeOut(15);
				gm.StopMusic();
			}
			soul.transform.localPosition = Vector3.Lerp(soul.transform.localPosition, mainScreen.Find("Tab" + tab).localPosition + SOUL_OFFSET, 0.5f);
			break;
		case State.OptionSelect:
			if (holdVert && UTInput.GetAxis("Vertical") == 0f)
			{
				holdVert = false;
			}
			else if (!holdVert && UTInput.GetAxis("Vertical") != 0f)
			{
				holdVert = true;
				if (UTInput.GetAxis("Vertical") > 0f)
				{
					option--;
					if (option < 0)
					{
						option = optionLimit;
					}
				}
				if (UTInput.GetAxis("Vertical") < 0f)
				{
					option++;
					if (option > optionLimit)
					{
						option = 0;
					}
				}
				moveSound.Play();
				UpdateOptions(select: true);
			}
			if (UTInput.GetButtonDown("Z"))
			{
				if (SelectOption())
				{
					selectSound.Play();
				}
				else
				{
					errorSound.Play();
				}
				UpdateOptions(select: true);
			}
			if (UTInput.GetButtonDown("X"))
			{
				option = 0;
				backSound.Play();
				state = State.TabSelect;
				UpdateOptions(select: false);
			}
			if (tab == 1 && option == 1 && !marioBrosUnlocked && trainingModeUnlocked)
			{
				if (UTInput.GetButtonDown("Z"))
				{
					HandleMarioBrosCode(2);
				}
				if (UTInput.GetButtonDown("C"))
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
			soul.transform.localPosition = Vector3.Lerp(soul.transform.localPosition, mainScreen.Find("Option" + option).localPosition + SOUL_OFFSET, 0.5f);
			break;
		case State.EndingsPage:
			if (holdVert && UTInput.GetAxis("Vertical") == 0f)
			{
				holdVert = false;
			}
			else if (!holdVert && UTInput.GetAxis("Vertical") != 0f)
			{
				holdVert = true;
				if (UTInput.GetAxis("Vertical") > 0f)
				{
					ending--;
					if (ending < 0)
					{
						ending = ENDINGS.Length - 1;
					}
					UpdateEndingsPage();
				}
				if (UTInput.GetAxis("Vertical") < 0f)
				{
					ending++;
					if (ending > ENDINGS.Length - 1)
					{
						ending = 0;
					}
					UpdateEndingsPage();
				}
				moveSound.Play();
			}
			if (UTInput.GetButtonDown("X"))
			{
				backSound.Play();
				state = State.OptionSelect;
				EndingsState(entering: false);
			}
			soul.transform.localPosition = Vector3.Lerp(soul.transform.localPosition, endingsScreen.Find("Ending" + ending).localPosition + SOUL_OFFSET, 0.5f);
			break;
		case State.FadingOut:
			if ((bool)fade && !fade.IsPlaying())
			{
				if (newScene < 0)
				{
					gm.StartTime();
					gm.LoadArea(7, fadeIn: true, new Vector3(0.16f, -0.08f), Vector2.down);
					state = State.None;
					fade.FadeIn(8);
				}
				else if (newScene == 116)
				{
					gm.LoadArea(116, fadeIn: true, new Vector2(0f, -3.745f), Vector2.up);
				}
				else
				{
					SceneManager.LoadScene(newScene);
				}
			}
			break;
		case State.ConfirmName:
			if (holdHoriz && UTInput.GetAxis("Horizontal") == 0f)
			{
				holdHoriz = false;
			}
			else if (!holdHoriz && UTInput.GetAxis("Horizontal") != 0f)
			{
				holdHoriz = true;
				if (UTInput.GetAxis("Horizontal") != 0f)
				{
					nameConfirmed = !nameConfirmed;
					for (int i = 0; i < 2; i++)
					{
						nameScreen.Find(i.ToString()).GetComponent<Text>().color = ((i == ((!nameConfirmed) ? 1 : 0)) ? new Color(1f, 1f, 0f) : Color.white);
					}
				}
			}
			if (UTInput.GetButtonDown("Z"))
			{
				if (nameConfirmed)
				{
					gm.SetFileID(3);
					newScene = -1;
					state = State.FadingOut;
					nameText.DestroyOldText();
					nameScreen.Find("0").GetComponent<Text>().enabled = false;
					nameScreen.Find("1").GetComponent<Text>().enabled = false;
					soul.enabled = false;
					gm.NewGame(modeName);
					music.Stop();
					fade.UTFadeOut();
				}
				else
				{
					NameState(entering: false);
				}
			}
			if (UTInput.GetButtonDown("X"))
			{
				if ((bool)nameText && nameText.IsPlaying())
				{
					nameText.SkipText();
				}
				else
				{
					NameState(entering: false);
				}
			}
			soul.transform.localPosition = Vector3.Lerp(soul.transform.localPosition, nameScreen.Find(nameConfirmed ? "0" : "1").localPosition + SOUL_OFFSET, 0.5f);
			break;
		}
	}

	private void UpdateOptions(bool select, bool init = false)
	{
		mainScreen.Find("GoBack").gameObject.SetActive(!init);
		if (!init)
		{
			bool joystickIsActive = UTInput.joystickIsActive;
			Text component = mainScreen.Find("GoBack").GetComponent<Text>();
			if (!joystickIsActive)
			{
				component.text = string.Format("[press {0} or shift to go back]", UTInput.GetKeyName("X").ToLower());
			}
			else
			{
				component.text = "[press    to go back]";
				ButtonPrompts.UpdateImageWithGraphic("Cancel", component.transform.Find("Cancel").GetComponent<Image>(), 2f, ButtonPrompts.ButtonType.Small);
			}
			component.transform.Find("Cancel").GetComponent<Image>().enabled = joystickIsActive;
		}
		for (int i = 0; i < 3; i++)
		{
			if (select)
			{
				mainScreen.Find("Tab" + i).GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.5f);
			}
			else
			{
				mainScreen.Find("Tab" + i).GetComponent<Text>().color = ((!init && tab == i) ? new Color(1f, 1f, 0f) : Color.white);
			}
		}
		for (int j = 0; j < 5; j++)
		{
			if (select)
			{
				mainScreen.Find("Option" + j).GetComponent<Text>().color = ((option == j) ? new Color(1f, 1f, 0f) : Color.white);
			}
			else
			{
				mainScreen.Find("Option" + j).GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.5f);
			}
			mainScreen.Find("Option" + j).GetComponent<Text>().text = "";
		}
		if (select)
		{
			localOptions.SaveToConfig(ref gm.config);
			gm.config.Write();
		}
		soul.enabled = !init;
		if (flavorUnlocked)
		{
			flavorPreview.gameObject.SetActive(tab == 0 && option == 1);
			int value = localOptions.startingFlavor.value;
			flavorPreview.GetComponent<Image>().color = UIBackground.borderColors[value];
			flavorPreview.Find("SelText").GetComponent<Text>().color = Selection.SELECTION_COLORS[value];
			flavorPreview.Find("TestButton").GetComponent<Image>().color = BattleButton.BUTTON_COLORS[value];
			flavorPreview.Find("TestButtonSel").GetComponent<Image>().color = Selection.SELECTION_COLORS[value];
			if (value == 9)
			{
				flavorPreview.Find("TestButton").GetComponent<Image>().color = Selection.SELECTION_COLORS[value];
				flavorPreview.Find("TestButtonSel").GetComponent<Image>().color = BattleButton.BUTTON_COLORS[value];
			}
		}
		if (marioBrosUnlocked)
		{
			marioBrosScore.gameObject.SetActive(tab == 1 && option == 1);
			marioBrosScore.Find("Score").GetComponent<Text>().text = PersistentSAVE.GetInt("mario-score", 20000).ToString();
			marioBrosScore.Find("Phase").GetComponent<Text>().text = PersistentSAVE.GetInt("mario-phase", 3).ToString();
		}
		Image[] componentsInChildren = mainScreen.GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			if (image.gameObject.name.StartsWith("CompletionStar"))
			{
				image.enabled = false;
			}
		}
		switch (tab)
		{
		case 0:
			OptionsTabVisuals(select);
			break;
		case 1:
			OptionsTabMinigames(select);
			break;
		case 2:
			OptionsTabExtraModes(select);
			break;
		}
		for (int l = 0; l < 5; l++)
		{
			Text component2 = mainScreen.Find("Option" + l).GetComponent<Text>();
			mainScreen.Find("CompletionStar" + l).localPosition = new Vector3(-250f + component2.preferredWidth + 16f, 68 - 40 * l);
		}
	}

	private bool SelectOption()
	{
		switch (tab)
		{
		case 0:
			switch (option)
			{
			case 0:
				localOptions.runAnimations.Increase();
				GameManager.SetOptions(localOptions);
				return true;
			case 1:
				if (flavorUnlocked)
				{
					localOptions.startingFlavor.Increase();
					GameManager.SetOptions(localOptions);
					return true;
				}
				break;
			}
			break;
		case 1:
			switch (option)
			{
			case 0:
				if (unoUnlocked)
				{
					gm.StopMusic();
					gm.SetDefaultValues();
					gm.SetFlag(204, 1);
					gm.SetFlag(223, localOptions.startingFlavor.value);
					gm.SetFlag(292, 1);
					for (int i = 0; i < 5; i++)
					{
						gm.SetFlag(307 + i, 1);
					}
					gm.SetSessionFlag(17, 1);
					gm.SetPartyMembers(susie: true, noelle: true);
					gm.LockMenu();
					newScene = 116;
					fade.FadeOut(15, Color.black);
					state = State.FadingOut;
					return true;
				}
				break;
			case 1:
				if (marioBrosUnlocked)
				{
					gm.StopMusic();
					newScene = 103;
					music.Stop();
					fade.FadeOut(20, Color.white);
					state = State.FadingOut;
					return true;
				}
				break;
			}
			break;
		case 2:
			switch (option)
			{
			case 0:
				ending = 0;
				EndingsState(entering: true);
				UpdateEndingsPage();
				return true;
			case 1:
				modeName = "FRISK";
				nameConfirmed = false;
				NameState(entering: true);
				nameText.StartText("\b      THIS NAME SHALL COMMENCE\n\b     AN INTERESTING EXPERIMENT.", new Vector2(0f, 111f), "", 2);
				return true;
			}
			break;
		}
		return false;
	}

	private void OptionsTabVisuals(bool select)
	{
		if (select)
		{
			optionLimit = 1;
		}
		mainScreen.Find("Option0").GetComponent<Text>().text = "Run Animations: " + (new string[2] { "OFF", "ON" })[localOptions.runAnimations.value];
		mainScreen.Find("Option1").GetComponent<Text>().text = (flavorUnlocked ? ("Starting Flavor: " + Options.FLAVORS[localOptions.startingFlavor.value]) : "---");
	}

	private void OptionsTabMinigames(bool select)
	{
		if (select)
		{
			optionLimit = 1;
		}
		mainScreen.Find("Option0").GetComponent<Text>().text = (unoUnlocked ? "UNOTRAVELER" : "---");
		mainScreen.Find("Option1").GetComponent<Text>().text = (marioBrosUnlocked ? "Mario Bros." : "---");
		mainScreen.Find("CompletionStar0").GetComponent<Image>().enabled = PersistentSAVE.GetInt("uno-papyrus-hardmode-win", 0) == 1;
		mainScreen.Find("CompletionStar1").GetComponent<Image>().enabled = PersistentSAVE.GetInt("mario-score", 0) >= 100000;
	}

	private void OptionsTabExtraModes(bool select)
	{
		if (select)
		{
			optionLimit = 1;
		}
		mainScreen.Find("Option0").GetComponent<Text>().text = "Endings...";
		mainScreen.Find("Option1").GetComponent<Text>().text = "Hard Mode";
		mainScreen.Find("CompletionStar1").GetComponent<Image>().enabled = PersistentSAVE.GetInt("hardmode-completion", 0) == 1;
	}

	private void EndingsState(bool entering)
	{
		mainScreen.position = new Vector2(entering ? (-640) : 0, 0f);
		endingsScreen.position = new Vector2((!entering) ? (-640) : 0, 0f);
		state = (entering ? State.EndingsPage : State.OptionSelect);
		if (newTitle)
		{
			charSprite.enabled = !entering;
			charAnimator.enabled = !entering;
			GameObject.Find("CharPlatform").GetComponent<SpriteRenderer>().enabled = !entering;
		}
		if (!entering)
		{
			return;
		}
		bool joystickIsActive = UTInput.joystickIsActive;
		Text component = endingsScreen.Find("GoBack").GetComponent<Text>();
		if (!joystickIsActive)
		{
			component.text = string.Format("[press {0} or shift to go back]", UTInput.GetKeyName("X").ToLower());
		}
		else
		{
			component.text = "[press    to go back]";
			ButtonPrompts.UpdateImageWithGraphic("Cancel", component.transform.Find("Cancel").GetComponent<Image>(), 2f, ButtonPrompts.ButtonType.Small);
		}
		component.transform.Find("Cancel").GetComponent<Image>().enabled = joystickIsActive;
		for (int i = 0; i < endingsStatus.Length; i++)
		{
			Text component2 = endingsScreen.Find("Ending" + i).GetComponent<Text>();
			switch (endingsStatus[i])
			{
			case EndingStatus.Locked:
				component2.text = "--------------------";
				component2.color = new Color(1f, 1f, 1f, 0.5f);
				break;
			case EndingStatus.HalfComplete:
				component2.text = ENDINGS[i];
				component2.color = Color.white;
				break;
			case EndingStatus.Completed:
				component2.text = ENDINGS[i];
				component2.color = new Color(1f, 1f, 0f);
				break;
			}
			Image component3 = endingsScreen.Find("CompletionStar" + i).GetComponent<Image>();
			component3.enabled = endingsStatus[i] == EndingStatus.Completed;
			component3.transform.localPosition = new Vector3(-250f + component2.preferredWidth + 16f, component3.transform.localPosition.y);
		}
	}

	private void UpdateEndingsPage()
	{
		Image component = endingsScreen.Find("EndingImage").GetComponent<Image>();
		Text component2 = endingsScreen.Find("EndingVariants").GetComponent<Text>();
		Text component3 = endingsScreen.Find("EndingDescription").GetComponent<Text>();
		switch (endingsStatus[ending])
		{
		case EndingStatus.Locked:
			component.sprite = Resources.Load<Sprite>("ui/title/endings/spr_ending_locked");
			component2.text = "variants: ?";
			component3.text = "You haven't seen\nthis ending yet";
			break;
		case EndingStatus.HalfComplete:
		case EndingStatus.Completed:
			component3.text = ENDING_DESCRIPTIONS[ending];
			component2.text = "variants: 1";
			component.sprite = Resources.Load<Sprite>(ENDING_IMAGES[ending]);
			break;
		}
	}

	private void DetermineEndingStatus()
	{
		if (PersistentSAVE.GetInt("hardmode-completion", 0) == 1)
		{
			endingsStatus[4] = EndingStatus.Completed;
		}
	}

	private void NameState(bool entering)
	{
		nameText.DestroyOldText();
		if (modeName != "")
		{
			RectTransform component = nameScreen.Find("Name").Find("Text").GetComponent<RectTransform>();
			component.localPosition = new Vector2(-modeName.Length * 7, 123f);
			component.sizeDelta = new Vector2(modeName.Length * 16, 32f);
			component.GetComponent<Text>().text = modeName;
		}
		if (newTitle)
		{
			charSprite.enabled = !entering;
			charAnimator.enabled = !entering;
			GameObject.Find("CharPlatform").GetComponent<SpriteRenderer>().enabled = !entering;
		}
		mainScreen.position = new Vector2(entering ? (-640) : 0, 0f);
		nameScreen.position = new Vector2((!entering) ? (-640) : 0, 0f);
		state = (entering ? State.ConfirmName : State.OptionSelect);
		endNameEvent.GetComponentInChildren<SpriteRenderer>().enabled = entering;
		if (entering)
		{
			endNameEvent.StartNameShake();
		}
		else
		{
			endNameEvent.StopNameShake();
		}
	}

	private void HandleMarioBrosCode(int input)
	{
		if (input == correctMBCombo[comboProgress])
		{
			comboProgress++;
			if (comboProgress == correctMBCombo.Length)
			{
				marioBrosUnlocked = true;
				UpdateOptions(select: true);
				gm.PlayGlobalSFX("mariobros/sounds/snd_coin");
				PersistentSAVE.SetInt("mario-unlocked", 1);
				UnityEngine.Object.FindFirstObjectByType<Notifications>().CheckForNewNotifications(playSound: false);
			}
		}
		else
		{
			comboProgress = 0;
		}
	}

	private void SetFlavorCharacter()
	{
		List<string> list = new List<string> { "lancer", "ralsei" };
		List<string> list2 = new List<string> { "Something Lancer says", "             doobie" };
		List<string> list3 = new List<string> { "music/mus_thrash_machine", "music/mus_acid_tunnel" };
		int num = PersistentSAVE.GetInt("completion", 0);
		if (num >= 1)
		{
			list.Add("flowey");
			list2.Add("Don't you have anything\nbetter to do?");
			list3.Add("music/mus_options_fall");
		}
		if (num >= 2)
		{
			list.Add("eb");
			list2.Add("     This game stinks!");
			list3.Add("music/mus_yourname");
		}
		int index = UnityEngine.Random.Range(0, list.Count);
		flavorText.text = list2[index];
		charAnimator.Play(list[index]);
		if (!string.IsNullOrEmpty(list3[index]))
		{
			Resources.Load<AudioClip>(list3[index]);
			musicToPlay = list3[index];
		}
	}

	private void SetSeason()
	{
	}
}
