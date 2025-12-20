using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public static GameManager instance = null;

	private bool menuDisabled;

	private bool menuLocked;

	private GameObject menu;

	private bool menuIsOpen;

	private bool dev;

	private string playerName;

	private List<int> items;

	private int[] weapon;

	private int[] armor;

	private int[] hp;

	private int deaths;

	private int exp;

	private int gold;

	private readonly int[] lvs = new int[20]
	{
		0, 10, 30, 70, 120, 200, 300, 500, 800, 1200,
		1700, 2500, 3500, 5000, 7000, 10000, 15000, 25000, 50000, 99999
	};

	private int[] atBuffs = new int[3];

	private int[] dfBuffs = new int[3];

	private bool susieActive = true;

	private bool noelleActive = true;

	private int miniPartyMember = -1;

	private int zone;

	private int oldZone;

	private bool lastZoneForceLoad = true;

	private Vector2 spawnPos;

	private Vector2 spawnDir;

	private bool savePointSpawn;

	private bool newSceneFadeIn;

	private bool wrongWarp;

	private MusicPlayer mp;

	private AudioSource aud;

	private int healAudFrames;

	private string healAudSound = "sounds/snd_heal";

	private string nextOWSong;

	private bool trackTime;

	private int playTime;

	private int playTimeFrames;

	private PackManager packManager;

	private MiscellaneousStrings miscStrings;

	private bool forcedBattleEnd;

	private int ending = -1;

	private object[] flags;

	private object[] persFlags;

	private object[] sessionFlags;

	public SAVEFile save;

	private int fileID;

	public SAVEFile checkpointSave;

	private bool checkpointEnabled;

	private Vector3 checkpointPos = Vector3.zero;

	private int forceRespawnZone = -1;

	private int battleId;

	private int battleEndState;

	public Config config;

	private bool fullscreen;

	private static Options options = new Options();

	private static bool allowZoneChange = true;

	private UnoGameManager unoGm;

	private bool inSingleBattle;

	private List<int> invalidPreviousZones = new List<int> { 0, 1, 2, 77, 78 };

	public static bool autoLowGraphics = false;

	private int framesBeforeClipReset;

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (instance == null)
		{
			SetFramerate(30);
			instance = this;
			zone = SceneManager.GetActiveScene().buildIndex;
			GameObject gameObject = new GameObject("FadeCanvas", typeof(Canvas));
			gameObject.GetComponent<Canvas>().renderMode = RenderMode.ScreenSpaceOverlay;
			gameObject.GetComponent<Canvas>().renderMode = RenderMode.WorldSpace;
			gameObject.GetComponent<Canvas>().sortingOrder = 2000;
			gameObject.transform.position = Vector3.zero;
			gameObject.transform.localScale = new Vector3(1f / 48f, 1f / 48f, 1f);
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/FadeObj"), gameObject.transform).name = "FadeObj";
			UnityEngine.Object.DontDestroyOnLoad(gameObject);
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/QuitFunction"));
			obj.name = "QuitFunction";
			UnityEngine.Object.DontDestroyOnLoad(obj);
			SetDefaultValues();
			ConvertOldFile();
			save = new SAVEFile();
			Shader shader = Shader.Find("Custom/BackgroundShader");
			autoLowGraphics = (bool)shader && !shader.isSupported;
			if (PlayerPrefs.GetInt("fullscreen") == 1)
			{
				Resolution currentResolution = Screen.currentResolution;
				Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
				fullscreen = true;
			}
			else
			{
				int num = 1;
				Resolution currentResolution2 = Screen.currentResolution;
				if (PlayerPrefs.HasKey("WindowScale"))
				{
					num = PlayerPrefs.GetInt("WindowScale");
					if (num < 1 || num * 640 > currentResolution2.width || num * 480 > currentResolution2.height)
					{
						num = 1;
						PlayerPrefs.SetInt("WindowScale", 1);
					}
				}
				else
				{
					PlayerPrefs.SetInt("WindowScale", 1);
				}
				Screen.SetResolution(640 * num, 480 * num, false);
				fullscreen = false;
			}
			packManager = base.gameObject.AddComponent<PackManager>();
			config = new Config("config.ini");
			LoadConfigData();
			base.gameObject.AddComponent<UTInput>();
			miscStrings = base.gameObject.AddComponent<MiscellaneousStrings>();
		}
		else if (instance != this)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		menuIsOpen = false;
		menuDisabled = false;
		trackTime = false;
		battleId = 0;
		battleEndState = -1;
		newSceneFadeIn = false;
		spawnPos = Vector2.zero;
		spawnDir = Vector2.down;
		savePointSpawn = false;
		mp = base.gameObject.AddComponent<MusicPlayer>();
		aud = base.gameObject.AddComponent<AudioSource>();
		healAudFrames = 0;
		base.gameObject.AddComponent<ExceptionHandler>();
		dev = false;
		string[] commandLineArgs = Environment.GetCommandLineArgs();
		for (int i = 0; i < commandLineArgs.Length; i++)
		{
			if (commandLineArgs[i] == "-testmode")
			{
				dev = true;
			}
		}
	}

	private void Start()
	{
		if (UnityEngine.Object.FindObjectOfType<OverworldPlayer>() != null)
		{
			if (IsTestMode() && save.name == null)
			{
				fileID = 3;
				if (FileExists())
				{
					LoadFile();
				}
			}
			SetDefaultValues();
			PlayMusic(UnityEngine.Object.FindObjectOfType<CameraController>().GetZoneMusic(), UnityEngine.Object.FindObjectOfType<CameraController>().GetZoneMusicPitch());
			StartTime();
		}
		Font[] array = Resources.LoadAll<Font>("fonts");
		for (int i = 0; i < array.Length; i++)
		{
			array[i].material.mainTexture.filterMode = FilterMode.Point;
		}
	}

	private void Update()
	{
		if (UTInput.GetButtonDown("C") && (bool)UnityEngine.Object.FindObjectOfType<OverworldPlayer>() && !menuIsOpen && !menuDisabled && !menuLocked)
		{
			menu = new GameObject();
			menu.AddComponent<MainMenu>().CreateMainMenu();
		}
		if (healAudFrames > 0)
		{
			healAudFrames++;
		}
		if (healAudFrames > 12)
		{
			PlayGlobalSFX(healAudSound);
			healAudFrames = 0;
		}
		if (trackTime)
		{
			playTimeFrames++;
			if (playTimeFrames == 30)
			{
				playTime++;
				playTimeFrames = 0;
			}
		}
		if (dev)
		{
			if (Input.GetKeyDown(KeyCode.F12))
			{
				SpawnFromLastSave(false);
			}
			KeyCode[] keys = DebugTools.GetKeys();
			foreach (KeyCode keyCode in keys)
			{
				if (Input.GetKeyDown(keyCode))
				{
					if (!UnityEngine.Object.FindObjectOfType<OverworldPlayer>())
					{
						DebugTools.UseTool(keyCode);
						break;
					}
					if (keyCode == KeyCode.F6 || UnityEngine.Object.FindObjectOfType<OverworldPlayer>().CanMove() || (bool)UnityEngine.Object.FindObjectOfType<BattleManager>())
					{
						DebugTools.UseTool(keyCode);
						break;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			fullscreen = !fullscreen;
			if (fullscreen)
			{
				Resolution currentResolution = Screen.currentResolution;
				Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
				PlayerPrefs.SetInt("fullscreen", 1);
			}
			else
			{
				Screen.SetResolution(640 * PlayerPrefs.GetInt("WindowScale"), 480 * PlayerPrefs.GetInt("WindowScale"), false);
				PlayerPrefs.SetInt("fullscreen", 0);
			}
		}
		allowZoneChange = SceneManager.sceneCount == 1;
	}

	public void textboxtest()
	{
		menu = new GameObject();
		menu.AddComponent<TextBox>().CreateBox(new string[8] { "* Dum dee dum...", "* Oh?^10\n* Is someone there?", "* Just a moment!", "* I have almost finished watering\n  these flowers.", "* But when I finish,^15 <color=#ff0000ff>you're dead.</color>", "* Haha!^05\n* Just kidding!", "* I <color=#ffff00ff>fooled</color> you!!", "* Now screw off..." }, new string[8] { "snd_txtasg", "snd_txtasg", "snd_txtasg", "snd_txtasg", "snd_txtasg", "snd_text", "snd_text", "snd_text" }, new int[8] { 1, 1, 1, 1, 1, 0, 0, 0 });
		menuIsOpen = true;
	}

	public void DisableMenu()
	{
		menuDisabled = true;
	}

	public void EnableMenu()
	{
		menuDisabled = false;
	}

	public void LockMenu()
	{
		menuLocked = true;
	}

	public void UnlockMenu()
	{
		menuLocked = false;
	}

	public bool IsMenuDisabled()
	{
		return menuDisabled;
	}

	public void ClosedMenu()
	{
		menuIsOpen = false;
	}

	public bool IsMenuOpen()
	{
		return menuIsOpen;
	}

	public void DisablePlayerMovement(bool deactivatePartyMembers)
	{
		if (UnityEngine.Object.FindObjectOfType<OverworldPlayer>() != null)
		{
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetMovement(false);
		}
		if (deactivatePartyMembers)
		{
			OverworldPartyMember[] array = UnityEngine.Object.FindObjectsOfType<OverworldPartyMember>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Deactivate();
			}
		}
		menuIsOpen = true;
	}

	public void EnablePlayerMovement()
	{
		bool flag = true;
		if (UnityEngine.Object.FindObjectOfType<OverworldPlayer>() != null)
		{
			if (UnityEngine.Object.FindObjectOfType<OverworldPlayer>().CannotMoveBattleSpecial())
			{
				flag = false;
			}
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetMovement(true);
		}
		if ((bool)GameObject.Find("Susie") && susieActive && (bool)GameObject.Find("Susie").GetComponent<OverworldPartyMember>())
		{
			GameObject.Find("Susie").GetComponent<OverworldPartyMember>().Activate();
		}
		if ((bool)GameObject.Find("Noelle") && noelleActive && (bool)GameObject.Find("Noelle").GetComponent<OverworldPartyMember>())
		{
			GameObject.Find("Noelle").GetComponent<OverworldPartyMember>().Activate();
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<MoleFriend>())
		{
			UnityEngine.Object.FindObjectOfType<MoleFriend>().Activate();
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<CreepyLady>() && UnityEngine.Object.FindObjectOfType<CreepyLady>().IsFollowing())
		{
			UnityEngine.Object.FindObjectOfType<CreepyLady>().Activate();
		}
		if (flag)
		{
			ClosedMenu();
		}
	}

	public void SetPlayerName(string newPlayerName)
	{
		playerName = newPlayerName;
	}

	public string GetPlayerName()
	{
		return playerName;
	}

	public void TriggerWrongWarp()
	{
		wrongWarp = true;
	}

	public void ForceLoadArea(int sceneName)
	{
		lastZoneForceLoad = true;
		nextOWSong = "zoneMusic";
		zone = sceneName;
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		SceneManager.sceneLoaded += OnAreaLoaded;
	}

	public void LoadArea(int sceneName, bool fadeIn, Vector2 pos, Vector2 dir)
	{
		if ((bool)UnityEngine.Object.FindObjectOfType<OverworldPlayer>())
		{
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
		}
		if (sceneName != 97 && (int)GetSessionFlag(10) == 1)
		{
			SetSessionFlag(10, 0);
			UnlockMenu();
		}
		lastZoneForceLoad = false;
		nextOWSong = "zoneMusic";
		zone = sceneName;
		SceneManager.LoadScene(sceneName, LoadSceneMode.Single);
		spawnPos = pos;
		spawnDir = dir;
		newSceneFadeIn = fadeIn;
		SceneManager.sceneLoaded += OnAreaLoaded;
	}

	public void LoadBunnyCheck()
	{
		UnityEngine.Debug.Log("GET BUNNY'D");
		if ((bool)UnityEngine.Object.FindObjectOfType<Fade>())
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.FindObjectOfType<Fade>().gameObject);
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<QuitFunction>())
		{
			UnityEngine.Object.Destroy(UnityEngine.Object.FindObjectOfType<QuitFunction>().gameObject);
		}
		StopMusic();
		ForceLoadArea(78);
	}

	public void LoadArea(int sceneName, bool fadeIn, Vector2 pos, Vector2 dir, string music)
	{
		LoadArea(sceneName, fadeIn, pos, dir);
		nextOWSong = "music/" + music;
	}

	public void LoadArea(int sceneName, bool fadeIn, Vector2 pos, Vector2 dir, bool fromSavePoint)
	{
		LoadArea(sceneName, fadeIn, pos, dir);
		savePointSpawn = fromSavePoint;
	}

	private void OnAreaLoaded(Scene ascene, LoadSceneMode aMode)
	{
		SceneManager.sceneLoaded -= OnAreaLoaded;
		SceneManager.SetActiveScene(SceneManager.GetSceneByBuildIndex(zone));
		if (!UnityEngine.Object.FindObjectOfType<BattleManager>())
		{
			GameObject.Find("Canvas").GetComponent<Canvas>().pixelPerfect = true;
			EnableMenu();
			GameObject gameObject = GameObject.Find("FadeObj");
			if (newSceneFadeIn)
			{
				gameObject.GetComponent<Fade>().FadeIn(13);
			}
			if ((bool)GameObject.Find("Player") && !lastZoneForceLoad)
			{
				if (savePointSpawn && !checkpointEnabled)
				{
					spawnPos = UnityEngine.Object.FindObjectOfType<SAVEPoint>().GetSpawnPosition();
				}
				else if (savePointSpawn && checkpointEnabled)
				{
					if (checkpointPos == Vector3.zero)
					{
						spawnPos = GameObject.Find("Player").transform.position;
					}
					else
					{
						spawnPos = checkpointPos;
					}
					spawnDir = Vector2.down;
					UnlockMenu();
				}
				if (wrongWarp)
				{
					spawnPos = GameObject.Find("Player").transform.position;
					spawnDir = Vector2.down;
					wrongWarp = false;
				}
				if ((bool)GameObject.Find("Player").GetComponent<OverworldPlayer>())
				{
					GameObject.Find("Player").GetComponent<OverworldPlayer>().HandleSpawn(spawnPos, spawnDir);
				}
			}
			savePointSpawn = false;
			EnablePlayerMovement();
			PlayMusic(nextOWSong);
			if ((bool)GameObject.Find("Player"))
			{
				WeirdChecker.RoomModifications(this);
			}
			return;
		}
		throw new InvalidOperationException("A scene tried to load that shouldn't have: " + ascene.name);
	}

	public void StartBattle(int newBattleId, LoadSceneMode sceneMode = LoadSceneMode.Additive)
	{
		battleId = newBattleId;
		SceneManager.LoadScene(2, sceneMode);
		if (battleId == 75)
		{
			SceneManager.sceneLoaded += OnUnoBattleLoaded;
		}
		else
		{
			SceneManager.sceneLoaded += OnBattleLoaded;
		}
	}

	public void StartSingleBattle(int newBattleId)
	{
		inSingleBattle = true;
		StartBattle(newBattleId, LoadSceneMode.Single);
	}

	public void DisableSingleBattleMode()
	{
		inSingleBattle = false;
	}

	public void OnUnoBattleLoaded(Scene ascene, LoadSceneMode aMode)
	{
		SceneManager.sceneLoaded -= OnUnoBattleLoaded;
		SceneManager.SetActiveScene(ascene);
		GameObject obj = GameObject.Find("BattleFadeObj");
		GameObject obj2 = new GameObject("SOUL");
		obj2.AddComponent<SOUL>();
		obj2.GetComponent<SOUL>().CreateSOUL(new Color(1f, 0f, 0f), false, true);
		obj2.GetComponent<SpriteRenderer>().sortingOrder = 500;
		unoGm = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/UnoGameManager")).GetComponent<UnoGameManager>();
		unoGm.SetupPlayers();
		unoGm.StartGame(MusicChooser.musicID, false, true, true, false);
		obj.GetComponent<Fade>().FadeIn(5);
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/UnoBattleManager")).GetComponent<UnoBattleManager>().StartBattle(battleId);
	}

	public void OnBattleLoaded(Scene ascene, LoadSceneMode aMode)
	{
		SceneManager.sceneLoaded -= OnBattleLoaded;
		SceneManager.SetActiveScene(ascene);
		GameObject obj = GameObject.Find("BattleFadeObj");
		GameObject obj2 = new GameObject("SOUL");
		obj2.AddComponent<SOUL>();
		obj2.GetComponent<SOUL>().CreateSOUL(new Color(1f, 0f, 0f), false, true);
		obj2.GetComponent<SpriteRenderer>().sortingOrder = 500;
		obj.GetComponent<Fade>().FadeIn(5);
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/BattleManager")).GetComponent<BattleManager>().StartBattle(battleId);
	}

	public void EndBattle(int battleEndState, bool force = false)
	{
		forcedBattleEnd = force;
		ResetAllBuffs();
		if ((bool)UnityEngine.Object.FindObjectOfType<TouchPad>())
		{
			UnityEngine.Object.FindObjectOfType<TouchPad>().SetSoulColor(SOUL.GetSOULColorByID(GetFlagInt(312), true));
		}
		this.battleEndState = battleEndState;
		if (battleId == 75)
		{
			PlayMusic("zoneMusic");
		}
		if (inSingleBattle)
		{
			ForceLoadArea(6);
			inSingleBattle = false;
		}
		else
		{
			SceneManager.UnloadSceneAsync("Battle");
			SceneManager.sceneUnloaded += OnBattleUnloaded;
		}
	}

	public void OnBattleUnloaded(Scene ascene)
	{
		SceneManager.sceneUnloaded -= OnBattleUnloaded;
		SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		Collider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<Collider2D>();
		for (int i = 0; i < componentsInChildren2.Length; i++)
		{
			componentsInChildren2[i].enabled = true;
		}
		AudioSource[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
		for (int i = 0; i < componentsInChildren3.Length; i++)
		{
			componentsInChildren3[i].enabled = true;
		}
		TilemapRenderer[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
		foreach (TilemapRenderer tilemapRenderer in componentsInChildren4)
		{
			if (tilemapRenderer.GetComponent<Tilemap>().enabled)
			{
				tilemapRenderer.enabled = true;
			}
		}
		SpriteMask[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<SpriteMask>();
		for (int i = 0; i < componentsInChildren5.Length; i++)
		{
			componentsInChildren5[i].enabled = true;
		}
		UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetComponent<SpriteRenderer>().enabled = true;
		UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetCollision(true);
		OverworldPartyMember[] array = UnityEngine.Object.FindObjectsOfType<OverworldPartyMember>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].GetComponent<SpriteRenderer>().enabled = true;
		}
		ForceTogglePlayers(true);
		EnablePlayerMovement();
		ResumeMusic(12);
		if ((bool)UnityEngine.Object.FindObjectOfType<LostCoreMusic>())
		{
			UnityEngine.Object.FindObjectOfType<LostCoreMusic>().SetDanger(false);
		}
		UnityEngine.Object.FindObjectOfType<Fade>().FadeIn(12);
		if (!forcedBattleEnd)
		{
			EndBattleHandler.DoEndBattle(battleId, battleEndState);
		}
		else
		{
			forcedBattleEnd = false;
		}
		battleId = 0;
		battleEndState = -1;
	}

	public void ForceTogglePlayers(bool tog)
	{
		OverworldPlayer overworldPlayer = UnityEngine.Object.FindObjectOfType<OverworldPlayer>();
		GameObject gameObject = GameObject.Find("Susie");
		GameObject gameObject2 = GameObject.Find("Noelle");
		if ((bool)overworldPlayer)
		{
			overworldPlayer.GetComponent<OverworldPlayer>().enabled = tog;
			overworldPlayer.GetComponent<SpriteRenderer>().enabled = tog;
			gameObject.GetComponent<OverworldPartyMember>().enabled = tog;
			gameObject.GetComponent<SpriteRenderer>().enabled = tog;
			gameObject2.GetComponent<OverworldPartyMember>().enabled = tog;
			gameObject2.GetComponent<SpriteRenderer>().enabled = tog;
		}
	}

	public void Death(int specialText = -1)
	{
		deaths++;
		SetSessionFlag(7, specialText);
		if (!inSingleBattle && FileExists())
		{
			SaveFile(false);
		}
		inSingleBattle = false;
		SceneManager.LoadScene(3, LoadSceneMode.Single);
		spawnPos = Vector2.zero;
		if (UnityEngine.Object.FindObjectOfType<SOUL>() != null)
		{
			spawnPos = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position - GameObject.Find("BattleCamera").transform.position;
		}
		else if (UnityEngine.Object.FindObjectOfType<ActionSOUL>() != null)
		{
			if (UnityEngine.Object.FindObjectOfType<ActionSOUL>().transform.childCount > 0)
			{
				spawnPos = UnityEngine.Object.FindObjectOfType<ActionSOUL>().transform.GetChild(0).position - UnityEngine.Object.FindObjectOfType<CameraController>().transform.position;
			}
			else
			{
				spawnPos = UnityEngine.Object.FindObjectOfType<ActionSOUL>().transform.position - UnityEngine.Object.FindObjectOfType<CameraController>().transform.position;
			}
		}
		else if (UnityEngine.Object.FindObjectOfType<OverworldPlayer>() != null)
		{
			spawnPos = UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position - UnityEngine.Object.FindObjectOfType<CameraController>().transform.position;
		}
		SceneManager.sceneLoaded += OnDeathScreenLoaded;
	}

	public void OnDeathScreenLoaded(Scene ascene, LoadSceneMode aMode)
	{
		DisablePlayerMovement(true);
		aud.Stop();
		mp.Stop();
		SceneManager.sceneLoaded -= OnDeathScreenLoaded;
	}

	public Vector3 GetSpawnPos()
	{
		return spawnPos;
	}

	public int GetNumDeaths()
	{
		return deaths;
	}

	public List<int> GetItemList()
	{
		return items;
	}

	public List<int> GetBoxList()
	{
		List<int> list = new List<int>();
		if (GetFlagInt(156) == 1)
		{
			for (int i = 0; i < 10; i++)
			{
				int flagInt = GetFlagInt(157 + i);
				if (flagInt > -1)
				{
					list.Add(flagInt);
				}
			}
		}
		return list;
	}

	public int FirstFreeItemSpace()
	{
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i] == -1)
			{
				return i;
			}
		}
		return -1;
	}

	public int NumItemFreeSpace()
	{
		int num = 0;
		for (int i = 0; i < items.Count; i++)
		{
			if (items[i] == -1)
			{
				num++;
			}
		}
		return num;
	}

	public void AddItem(int id)
	{
		if (id > -1)
		{
			items[FirstFreeItemSpace()] = id;
		}
	}

	public void RemoveItem(int index)
	{
		if (GetItem(index) == 45)
		{
			SetFlag(312, 0);
			if ((bool)UnityEngine.Object.FindObjectOfType<SOUL>())
			{
				UnityEngine.Object.FindObjectOfType<SOUL>().AdjustSOULColor();
			}
			if ((bool)UnityEngine.Object.FindObjectOfType<TouchPad>())
			{
				UnityEngine.Object.FindObjectOfType<TouchPad>().SetSoulColor(Color.red);
			}
		}
		items.RemoveAt(index);
		items.Add(-1);
	}

	public void RemoveItemByID(int item)
	{
		for (int i = 0; i < 8; i++)
		{
			if (items[i] == item)
			{
				RemoveItem(i);
				break;
			}
		}
	}

	public int GetItem(int id)
	{
		return items[id];
	}

	public int GetWeapon(int partyMember)
	{
		if (partyMember > 2)
		{
			if (partyMember == 3)
			{
				return 20;
			}
			return 0;
		}
		return weapon[partyMember];
	}

	public int GetArmor(int partyMember)
	{
		if (partyMember > 2)
		{
			return 0;
		}
		return armor[partyMember];
	}

	public void ForceWeapon(int partyMember, int i)
	{
		weapon[partyMember] = i;
	}

	public void ForceArmor(int partyMember, int i)
	{
		armor[partyMember] = i;
	}

	public void ChangeWeapon(int partyMember, int index)
	{
		int id = weapon[partyMember];
		weapon[partyMember] = items[index];
		RemoveItem(index);
		AddItem(id);
	}

	public void ChangeArmor(int partyMember, int index)
	{
		int num = armor[partyMember];
		armor[partyMember] = items[index];
		RemoveItem(index);
		if (num == 4)
		{
			num = 7;
		}
		AddItem(num);
	}

	public void EatItem(int partyMember, int index)
	{
		int item = GetItem(index);
		if (item == 28)
		{
			if (partyMember == 1)
			{
				SetHP(1, GetMaxHP(1) + 10, true);
			}
			else
			{
				SetHP(partyMember, GetMaxHP(partyMember) - 1);
			}
		}
		else if (item == 39 && partyMember == 2)
		{
			if (GetHP(2) - GetMaxHP(2) < 5)
			{
				SetHP(2, GetMaxHP(2) + 5, true);
			}
		}
		else
		{
			int heal = Items.ItemValue(item, partyMember);
			Heal(partyMember, heal);
		}
		RemoveItem(index);
	}

	public void UseItem(int partyMember, int index)
	{
		if (Items.ItemType(GetItem(index)) == 0)
		{
			int item = GetItem(index);
			if (item == 7)
			{
				PlayGlobalSFX("sounds/snd_heal");
			}
			else
			{
				PlayGlobalSFX("sounds/snd_swallow");
				healAudFrames = 1;
				if (item == 22)
				{
					healAudSound = "sounds/snd_speedup";
					if (!UnityEngine.Object.FindObjectOfType<BattleManager>() && GetItem(index) == 22)
					{
						UnityEngine.Object.FindObjectOfType<OverworldPlayer>().SetSpeedMultiplier(1.5f);
					}
				}
				else
				{
					healAudSound = "sounds/snd_heal";
				}
			}
			EatItem(partyMember, index);
			if (item == 35)
			{
				AddItem(36);
			}
		}
		else if (Items.ItemType(GetItem(index)) == 1 && partyMember != 1 && (partyMember != 2 || GetItem(index) != 41))
		{
			PlayGlobalSFX("sounds/snd_item");
			aud.Play();
			ChangeWeapon(partyMember, index);
		}
		else if (Items.ItemType(GetItem(index)) == 2 && (partyMember != 1 || (GetItem(index) != 14 && GetItem(index) != 43)))
		{
			PlayGlobalSFX("sounds/snd_item");
			ChangeArmor(partyMember, index);
			if ((bool)UnityEngine.Object.FindObjectOfType<SOULGraze>())
			{
				UnityEngine.Object.FindObjectOfType<SOULGraze>().UpdateGrazeSize();
			}
		}
		else if (Items.ItemType(GetItem(index)) == 4)
		{
			PlayGlobalSFX("sounds/snd_swallow");
			healAudFrames = 1;
			healAudSound = "sounds/snd_heal";
			int heal = Items.ItemValue(GetItem(index));
			HealAll(heal, false);
			RemoveItem(index);
		}
		else if (GetItem(index) == 16)
		{
			PlayGlobalSFX("sounds/snd_egg");
		}
		else if (GetItem(index) == 24)
		{
			PlayGlobalSFX("sounds/snd_tearcard");
			RemoveItem(index);
		}
		else if (GetItem(index) == 45)
		{
			RemoveItem(index);
		}
	}

	public void PlayTimedHealSound()
	{
		healAudFrames = 1;
		healAudSound = "sounds/snd_heal";
	}

	public void PlayGlobalSFX(string clip)
	{
		aud.clip = Resources.Load<AudioClip>(clip);
		aud.Play();
	}

	public void PlayMusic(string music, float pitch, float volume)
	{
		if (music == "zoneMusic" && (bool)UnityEngine.Object.FindObjectOfType<CameraController>())
		{
			music = UnityEngine.Object.FindObjectOfType<CameraController>().GetZoneMusic();
			if (music.EndsWith("mus_mysteriousroom2"))
			{
				if ((int)GetFlag(209) != 0 && (int)GetFlag(229) == 0 && (int)GetFlag(230) == 0)
				{
					music = "music/mus_snowy";
					pitch = 1f;
				}
				else if ((int)GetFlag(205) == 0)
				{
					music = "";
				}
				else if ((int)GetFlag(208) == 1)
				{
					music = "music/mus_creepychase";
					pitch = 1f;
				}
			}
			if (music.EndsWith("mus_tone3") && (((int)GetFlag(60) == 1 && zone < 50) || ((int)GetFlag(180) == 1 && zone > 50)))
			{
				music = "music/mus_snowy";
			}
			if (music == "music/mus_home" && (int)GetFlag(108) == 1)
			{
				music = "music/mus_house1";
			}
			if (WeirdChecker.GetWeirdAreaProgress(this, music) == 1 && zone != 110)
			{
				music += "_alt";
				pitch = 1f;
			}
			else if (WeirdChecker.GetWeirdAreaProgress(this, music) == 2 && zone != 110)
			{
				music = music.Replace("_intro", "");
				music += "_empty";
				if ((int)GetFlag(108) == 1 || music.Contains("mus_cave") || music.Contains("mus_wintercaves"))
				{
					music = "music/mus_toomuch";
				}
			}
			pitch = UnityEngine.Object.FindObjectOfType<CameraController>().GetZoneMusicPitch();
			if ((int)GetFlag(87) >= 5 && music == "music/mus_happyhappy")
			{
				pitch = 0.3f;
			}
			if ((int)GetFlag(87) >= 5 && music == "music/mus_twoson_intro")
			{
				music = "music/mus_birdnoise";
			}
		}
		if (music.EndsWith("mus_snowy"))
		{
			pitch = ((zone >= 50 && zone < 110) ? 0.475f : (((int)GetFlag(13) >= 3) ? 0.6f : 0.95f));
		}
		bool intro = false;
		if (music.EndsWith("_intro"))
		{
			intro = true;
			music = music.Replace("_intro", "");
		}
		mp.SetVolume(volume);
		if ((mp.CurrentMusic() != music || !mp.IsPlaying()) && music != "" && music != "music/")
		{
			mp.ChangeMusic(music, intro, true);
			mp.GetSource().pitch = pitch;
		}
		else if (music == "")
		{
			mp.Stop();
		}
	}

	public string GetPlayingMusic()
	{
		return mp.CurrentMusic();
	}

	public void PlayMusic(string music, float pitch)
	{
		PlayMusic(music, pitch, 1f);
	}

	public void PlayMusic(string music)
	{
		PlayMusic(music, 1f);
	}

	public void StopSFX()
	{
		aud.Stop();
	}

	public void StopMusic()
	{
		if ((bool)mp)
		{
			mp.Stop();
		}
	}

	public void StopMusic(float fadeOutFrames)
	{
		if ((bool)mp)
		{
			if (fadeOutFrames <= 0f)
			{
				StopMusic();
			}
			else
			{
				mp.FadeOut(fadeOutFrames / 30f);
			}
		}
	}

	public void PauseMusic()
	{
		if ((bool)mp)
		{
			mp.Pause();
		}
	}

	public void ResumeMusic()
	{
		if ((bool)mp)
		{
			mp.Resume();
		}
	}

	public void ResumeMusic(int fadeInFrames)
	{
		if ((bool)mp && mp.IsPaused())
		{
			ResumeMusic();
			if (fadeInFrames > 0)
			{
				mp.FadeIn((float)fadeInFrames / 30f);
			}
		}
	}

	public MusicPlayer GetMusicPlayer()
	{
		return mp;
	}

	public int GetHP(int partyMember)
	{
		if (partyMember > 2)
		{
			return hp[0];
		}
		return hp[partyMember];
	}

	public int[] GetHPArray()
	{
		return hp;
	}

	public int GetCombinedHP()
	{
		int num = hp[0];
		if (SusieInParty())
		{
			num += hp[1];
		}
		if (NoelleInParty())
		{
			num += hp[2];
		}
		return num;
	}

	public int GetMaxHP(int partyMember)
	{
		return GetMaxHP(partyMember, exp);
	}

	public int GetMaxHP(int partyMember, int exp)
	{
		if (partyMember > 2)
		{
			return GetMiniMemberMaxHP();
		}
		float num = ((partyMember == 1) ? 1.5f : 1f);
		float num2 = ((partyMember == 1) ? 1.25f : 1f);
		int num3 = Mathf.RoundToInt(20f * num + (float)(4 * (GetLV(exp) - 1)) * num2);
		if (GetLV(exp) == 20)
		{
			num3 = ((partyMember == 1) ? 150 : 100);
		}
		if (partyMember == 0 && GetMiniPartyMember() > 0)
		{
			num3 += GetMiniMemberMaxHP();
		}
		return num3;
	}

	public int GetMiniMemberMaxHP()
	{
		if (miniPartyMember == 0)
		{
			return 0;
		}
		if (miniPartyMember == 3)
		{
			return 10;
		}
		return 20;
	}

	public int GetMiniMemberATK()
	{
		if (miniPartyMember == 3)
		{
			return 1;
		}
		return 15;
	}

	public int GetMiniMemberDEF()
	{
		if (miniPartyMember == 3)
		{
			return 0;
		}
		return 7;
	}

	public bool KrisInControl()
	{
		if (miniPartyMember > 0 && hp[0] - GetMiniMemberMaxHP() <= 0)
		{
			return false;
		}
		return true;
	}

	public int GetLV()
	{
		return GetLV(exp);
	}

	public int GetLV(int exp)
	{
		if (exp < 0)
		{
			return 1;
		}
		for (int i = 0; i < lvs.Length; i++)
		{
			if (exp < lvs[i])
			{
				return i;
			}
		}
		return lvs.Length;
	}

	public int GetLVExp()
	{
		return GetExpForLV(GetLV() + 1);
	}

	public int GetExpForLV(int lv)
	{
		if (lv > 0 && lv <= lvs.Length)
		{
			return lvs[lv - 1];
		}
		return lvs[lvs.Length - 1];
	}

	public void AddEXP(int exp)
	{
		this.exp += exp;
	}

	public void SetEXP(int exp)
	{
		this.exp = exp;
	}

	public int GetEXP()
	{
		return exp;
	}

	public int GetGold()
	{
		return gold;
	}

	public void AddGold(int gold)
	{
		this.gold += gold;
	}

	public void RemoveGold(int gold)
	{
		this.gold -= gold;
		if (this.gold < 0)
		{
			this.gold = 0;
		}
	}

	public void SetGold(int gold)
	{
		this.gold = gold;
	}

	public void Heal(int partyMember, int heal)
	{
		if (hp[partyMember] <= GetMaxHP(partyMember))
		{
			hp[partyMember] += heal;
			if (hp[partyMember] > GetMaxHP(partyMember))
			{
				hp[partyMember] = GetMaxHP(partyMember);
			}
		}
	}

	public void HealAll(int heal, bool includeOutOfParty = true)
	{
		Heal(0, heal);
		if (includeOutOfParty || (!includeOutOfParty && SusieInParty()))
		{
			Heal(1, heal);
		}
		if (includeOutOfParty || (!includeOutOfParty && NoelleInParty()))
		{
			Heal(2, heal);
		}
	}

	public void Damage(int partyMember, int dmg)
	{
		int num = hp[partyMember];
		hp[partyMember] -= dmg;
		if ((bool)UnityEngine.Object.FindObjectOfType<BattleManager>() && num > 0 && ((UnityEngine.Object.FindObjectOfType<BattleManager>().IsSeriousMode() && hp[partyMember] <= 0 && partyMember == 0 && num > 1) || (UnityEngine.Object.FindObjectOfType<BattleManager>().GetState() < 3 && hp[partyMember] <= 0)))
		{
			hp[partyMember] = 1;
		}
		if (hp[partyMember] <= 0)
		{
			hp[partyMember] = 0;
		}
		if (GetCombinedHP() == 0)
		{
			Death();
		}
	}

	public int[] HandleDamageCalculations(int hp, float damageMulti, bool applyDamageImmediately = true)
	{
		PartyPanels partyPanels = UnityEngine.Object.FindObjectOfType<PartyPanels>();
		SOUL sOUL = UnityEngine.Object.FindObjectOfType<SOUL>();
		KarmaHandler karmaHandler = UnityEngine.Object.FindObjectOfType<KarmaHandler>();
		int[] array = new int[3]
		{
			this.hp[0],
			this.hp[1],
			this.hp[2]
		};
		float num = hp;
		int num2 = -1;
		if ((bool)partyPanels)
		{
			AttackBase attackBase = UnityEngine.Object.FindObjectOfType<AttackBase>();
			if ((object)attackBase != null && !attackBase.AttackingAllTargets())
			{
				num2 = UnityEngine.Random.Range(0, partyPanels.NumTargettedMembers());
				if (partyPanels.NumTargettedMembers() == 2 && num2 == 1)
				{
					num2 = (SusieInParty() ? 1 : 2);
				}
				if (GetHP(num2) <= 0 || !partyPanels.GetTargettedMembers()[num2])
				{
					switch (num2)
					{
					case 2:
						num2 -= ((GetHP(1) > 0 && partyPanels.GetTargettedMembers()[1]) ? 1 : 2);
						break;
					case 1:
						num2 += ((GetHP(2) > 0 && partyPanels.GetTargettedMembers()[2]) ? 1 : (-1));
						break;
					case 0:
						num2 += ((GetHP(1) > 0 && partyPanels.GetTargettedMembers()[1]) ? 1 : 2);
						break;
					}
				}
			}
		}
		for (int i = 0; i < 3; i++)
		{
			if ((num2 != -1 && num2 != i) || ((bool)sOUL && sOUL.PapCharmWasHit(i)))
			{
				continue;
			}
			float num3 = num;
			float num4 = GetDEF(i);
			if (i == 0 && !KrisInControl())
			{
				num4 = GetMiniMemberDEF();
			}
			float num5 = num4 / 3f;
			num3 -= num5;
			float num6 = 1f + (float)(GetLV() / 2) / 10f;
			if ((bool)karmaHandler)
			{
				int num7 = Mathf.RoundToInt((num3 * num6 - num3) * 2f);
				karmaHandler.AddKarma(i, (num7 <= 1) ? 1 : num7);
			}
			else
			{
				num3 *= num6;
			}
			if ((bool)partyPanels && UnityEngine.Object.FindObjectOfType<BattleManager>().GetDefendingMembers()[i])
			{
				num3 *= 2f / 3f;
			}
			if ((bool)sOUL && sOUL.IsShieldActive())
			{
				num3 *= 2f / 3f;
			}
			if (IsEasyMode())
			{
				num3 *= 2f / 3f;
			}
			num3 *= damageMulti;
			if (num3 < 1f)
			{
				num3 = 1f;
			}
			if ((bool)partyPanels && num2 == -1)
			{
				if (partyPanels.NumTargettedMembers() == 2)
				{
					num3 *= 0.8f;
				}
				else if (partyPanels.NumTargettedMembers() == 3)
				{
					num3 *= 0.65f;
				}
			}
			if (((bool)partyPanels && partyPanels.GetTargettedMembers()[i]) || !partyPanels)
			{
				int num8 = Mathf.RoundToInt(num3);
				if (applyDamageImmediately)
				{
					Damage(i, num8);
				}
				array[i] -= num8;
			}
		}
		return array;
	}

	public void SetHP(int partyMember, int newHP, bool forceOverheal = false)
	{
		hp[partyMember] = newHP;
		if (hp[partyMember] > GetMaxHP(partyMember) && !forceOverheal)
		{
			hp[partyMember] = GetMaxHP(partyMember);
		}
		if (hp[partyMember] <= 0)
		{
			hp[partyMember] = 0;
		}
		if (GetCombinedHP() == 0)
		{
			Death();
		}
	}

	public int GetATK(int partyMember)
	{
		if (partyMember > 2)
		{
			return GetMiniMemberATK();
		}
		int num = Items.ItemValue(GetWeapon(partyMember), partyMember);
		return GetATKRaw(partyMember) + num;
	}

	public int GetATKRaw(int partyMember)
	{
		int num = (GetLV() - 1) * 2;
		if (partyMember == 1)
		{
			num += 2 + Mathf.FloorToInt((float)GetLV() / 4f);
		}
		if (partyMember == 2)
		{
			num = Mathf.RoundToInt((float)(num * 2) / 3f);
		}
		if (partyMember == 0 && (int)GetFlag(102) == 1)
		{
			num -= 6;
		}
		if (partyMember <= 2)
		{
			num += atBuffs[partyMember];
		}
		return num;
	}

	public int GetDEF(int partyMember)
	{
		return GetDEFRaw(partyMember) + Items.ItemValue(GetArmor(partyMember));
	}

	public int GetDEFRaw(int partyMember)
	{
		int num = Mathf.FloorToInt((float)GetLV() / 5f);
		if (partyMember == 0 && (int)GetFlag(102) == 1)
		{
			num -= 3;
		}
		if (partyMember <= 2)
		{
			num += dfBuffs[partyMember];
		}
		return num;
	}

	public float GetMagic(int partyMember)
	{
		return GetMagicRaw(partyMember) + (float)GetMagicEquipment(partyMember);
	}

	public int GetMagicEquipment(int partyMember)
	{
		return Items.GetItemMagic(GetWeapon(partyMember)) + Items.GetItemMagic(GetArmor(partyMember));
	}

	public float GetMagicRaw(int partyMember)
	{
		switch (partyMember)
		{
		case 1:
			return 1f + (float)GetLV() / 5f;
		case 2:
			return GetLV();
		default:
			return 0f;
		}
	}

	public void SetATKBuff(int partyMember, int buff)
	{
		atBuffs[partyMember] = buff;
	}

	public void SetDEFBuff(int partyMember, int buff)
	{
		dfBuffs[partyMember] = buff;
	}

	public void ResetAllBuffs()
	{
		for (int i = 0; i < atBuffs.Length; i++)
		{
			atBuffs[i] = 0;
		}
		for (int j = 0; j < dfBuffs.Length; j++)
		{
			dfBuffs[j] = 0;
		}
	}

	public void SetPartyMembers(bool susie, bool noelle)
	{
		susieActive = susie;
		noelleActive = noelle;
	}

	public bool SusieInParty()
	{
		return susieActive;
	}

	public bool NoelleInParty()
	{
		return noelleActive;
	}

	public void SetMiniPartyMember(int id)
	{
		miniPartyMember = id;
		SetFlag(86, id);
	}

	public int GetMiniPartyMember()
	{
		return miniPartyMember;
	}

	public void StopTime()
	{
		trackTime = false;
	}

	public void StartTime()
	{
		trackTime = true;
	}

	public int GetCurrentZone()
	{
		return zone;
	}

	public int GetFileZoneIndex()
	{
		return save.zone;
	}

	public void SetFlag(int i, object state)
	{
		UnityEngine.Debug.LogFormat("SetFlag({0}, {1})", i, state);
		if (i >= 0 && i <= flags.Length)
		{
			flags[i] = state;
		}
	}

	public object GetFlag(int i)
	{
		if (flags == null || i < 0 || i > flags.Length || flags[i] == null)
		{
			return 0;
		}
		return flags[i];
	}

	public int GetFlagInt(int i)
	{
		return (int)GetFlag(i);
	}

	public string GetFlagString(int i)
	{
		return GetFlag(i).ToString();
	}

	public double GetFlagDouble(int i)
	{
		return (double)GetFlag(i);
	}

	public void SetPersistentFlag(int i, object state)
	{
		if (i >= 0 && i <= persFlags.Length)
		{
			persFlags[i] = state;
			SaveFile(false);
		}
	}

	public object GetPersistentFlag(int i)
	{
		if (persFlags == null || i < 0 || i > persFlags.Length || persFlags[i] == null)
		{
			return 0;
		}
		return persFlags[i];
	}

	public int GetPersistentFlagInt(int i)
	{
		return (int)GetPersistentFlag(i);
	}

	public string GetPersistentFlagString(int i)
	{
		return GetPersistentFlag(i).ToString();
	}

	public double GetPersistentFlagDouble(int i)
	{
		return (double)GetPersistentFlag(i);
	}

	public void SetSessionFlag(int i, object state)
	{
		sessionFlags[i] = state;
	}

	public object GetSessionFlag(int i)
	{
		if (sessionFlags == null || sessionFlags[i] == null)
		{
			return 0;
		}
		return sessionFlags[i];
	}

	public int GetSessionFlagInt(int i)
	{
		return (int)GetSessionFlag(i);
	}

	public string GetSessionFlagString(int i)
	{
		return GetSessionFlag(i).ToString();
	}

	public double GetSessionFlagDouble(int i)
	{
		return (double)GetSessionFlag(i);
	}

	public object GetSaveFlag(int i)
	{
		if (save.flags == null || save.flags[i] == null)
		{
			return 0;
		}
		return save.flags[i];
	}

	public int GetSaveFlagInt(int i)
	{
		return (int)GetSaveFlag(i);
	}

	public string GetSaveFlagString(int i)
	{
		return GetSaveFlag(i).ToString();
	}

	public double GetSaveFlagDouble(int i)
	{
		return (double)GetSaveFlag(i);
	}

	public void SaveFile(bool savepoint)
	{
		SetFlag(177, Application.version);
		if (savepoint)
		{
			DeactivateCheckpoint();
			save.UpdateCharacterInfo(playerName, exp, items, weapon, armor, susieActive, noelleActive, playTime, zone, gold, "[???]", flags);
		}
		save.UpdatePersistentFlags(persFlags);
		save.UpdateDeathCount(deaths);
		string path = "SAVE" + fileID + ".sav";
		using (FileStream stream = File.Open(Path.Combine(Application.persistentDataPath, path), FileMode.OpenOrCreate))
		{
			SAVEFileIO.WriteFile(ref save, stream);
		}
	}

	public void SetCheckpoint(int respawnZone)
	{
		checkpointEnabled = true;
		checkpointSave = new SAVEFile();
		checkpointSave.UpdateCharacterInfo(playerName, exp, items, weapon, armor, susieActive, noelleActive, playTime, respawnZone, gold, "[???]", flags);
		checkpointSave.UpdatePersistentFlags(persFlags);
		checkpointSave.UpdateDeathCount(deaths);
		checkpointPos = Vector3.zero;
		forceRespawnZone = -1;
	}

	public void SetCheckpoint(int respawnZone, Vector3 checkpointPos)
	{
		SetCheckpoint(respawnZone);
		this.checkpointPos = checkpointPos;
	}

	public void SetCheckpoint()
	{
		SetCheckpoint(zone);
	}

	public void ModifyCheckpointLocation(int forceRespawnZone, Vector3 checkpointPos)
	{
		this.checkpointPos = checkpointPos;
		this.forceRespawnZone = forceRespawnZone;
	}

	public void DeactivateCheckpoint()
	{
		checkpointEnabled = false;
		checkpointPos = Vector3.zero;
	}

	public void SetFileID(int fileID)
	{
		this.fileID = fileID;
	}

	public void LoadFile(int fileID)
	{
		SetFileID(fileID);
		LoadFile();
	}

	public void LoadFile()
	{
		string path = "SAVE" + fileID + ".sav";
		using (FileStream fs = File.Open(Path.Combine(Application.persistentDataPath, path), FileMode.Open))
		{
			SAVEFileIO.ReadFile(ref save, fs);
		}
	}

	public void ConvertOldFile()
	{
		if (File.Exists(Path.Combine(Application.persistentDataPath, "SAVE.sav")))
		{
			File.Move(Path.Combine(Application.persistentDataPath, "SAVE.sav"), Path.Combine(Application.persistentDataPath, "SAVE0.sav"));
		}
	}

	public void SpawnFromLastSave(bool respawn)
	{
		ResetAllBuffs();
		if (!respawn)
		{
			sessionFlags = new object[100];
		}
		if (!FileExists() && !(checkpointEnabled && respawn))
		{
			return;
		}
		if (checkpointEnabled && respawn)
		{
			flags = (object[])checkpointSave.flags.Clone();
			exp = checkpointSave.exp;
			miniPartyMember = (int)GetFlag(86);
			hp = new int[3]
			{
				GetMaxHP(0),
				GetMaxHP(1),
				GetMaxHP(2)
			};
			playerName = checkpointSave.name;
			items = new List<int>(checkpointSave.items);
			weapon = (int[])checkpointSave.weapon.Clone();
			armor = (int[])checkpointSave.armor.Clone();
			susieActive = checkpointSave.susieActive;
			noelleActive = checkpointSave.noelleActive;
			playTime = checkpointSave.playTime;
			zone = checkpointSave.zone;
			gold = checkpointSave.gold;
			if (forceRespawnZone > -1)
			{
				zone = forceRespawnZone;
				forceRespawnZone = -1;
			}
			StartTime();
			LoadArea(zone, true, Vector2.zero, Vector2.down, true);
			return;
		}
		flags = (object[])save.flags.Clone();
		exp = save.exp;
		miniPartyMember = (int)GetFlag(86);
		hp = new int[3]
		{
			GetMaxHP(0),
			GetMaxHP(1),
			GetMaxHP(2)
		};
		playerName = save.name;
		items = new List<int>(save.items);
		weapon = (int[])save.weapon.Clone();
		armor = (int[])save.armor.Clone();
		susieActive = save.susieActive;
		noelleActive = save.noelleActive;
		playTime = save.playTime;
		zone = save.zone;
		gold = save.gold;
		StartTime();
		if (!respawn)
		{
			deaths = save.deaths;
			persFlags = (object[])save.persFlags.Clone();
		}
		if (DoBunnyCheck())
		{
			LoadBunnyCheck();
		}
		else
		{
			LoadArea(zone, respawn, Vector2.zero, Vector2.down, true);
		}
	}

	public void SetEnding(int ending)
	{
		this.ending = ending;
	}

	public int GetEnding()
	{
		return ending;
	}

	private bool DoBunnyCheck()
	{
		bool result = false;
		if (!MapInfo.IsValidMapSpawn(zone))
		{
			result = true;
		}
		if (GetFlagInt(12) == 0 && GetFlagInt(13) > 0)
		{
			result = true;
		}
		if (GetFlagInt(12) == 1 && GetFlagInt(13) != GetFlagInt(87) && GetFlagInt(87) > 0)
		{
			if (GetFlagInt(176) == 0)
			{
				WeirdChecker.AdvanceTo(this, GetFlagInt(13), false);
			}
			else
			{
				result = true;
			}
			UnityEngine.Debug.Log("oblit weirdness");
		}
		if (zone > 50)
		{
			if (zone < 63 || zone == 70 || zone == 71)
			{
				SetFlag(64, 1);
			}
			else if (zone >= 63 && zone <= 69)
			{
				SetFlag(64, 0);
			}
			else if (zone != 77 && zone != 78)
			{
				SetFlag(64, 2);
			}
			if (GetFlagInt(13) == 0)
			{
				SetFlag(12, 0);
			}
		}
		else
		{
			SetFlag(64, 0);
		}
		int num = GetFlagInt(64) + 1;
		UnityEngine.Debug.Log("Section: " + num);
		if ((GetFlagInt(108) == 1 || GetFlagInt(107) == 1) && (num > 1 || GetMiniPartyMember() > 0 || NoelleInParty()))
		{
			result = true;
			UnityEngine.Debug.Log("Hard Mode Section 2+?  No thanks!!!!!!!!");
		}
		if (num != 5 && GetFlagInt(94) == 1)
		{
			result = true;
			UnityEngine.Debug.Log("TS Mode isn't supported yet please stop.");
		}
		int num2 = 99999;
		if (num == 1)
		{
			num2 = 447;
		}
		else
		{
			MonoBehaviour.print("No defaults for this yet");
		}
		if (exp > num2)
		{
			exp = num2;
		}
		for (int i = 0; i < hp.Length; i++)
		{
			if (hp[i] > GetMaxHP(i))
			{
				hp[i] = GetMaxHP(i);
			}
		}
		int flagInt = GetFlagInt(86);
		UnityEngine.Debug.Log("Party Member: " + flagInt);
		if (flagInt == 1)
		{
			UnityEngine.Debug.Log(string.Concat("Oblit Progress: ", GetFlag(87), ", Seen Paula: ", GetFlag(103), ", Section: ", num));
			if (num != 2 || GetFlagInt(87) >= 5 || GetFlagInt(103) == 0)
			{
				result = true;
				UnityEngine.Debug.Log("Paula has escaped containment.");
			}
		}
		SetFlag(176, 1);
		return result;
	}

	public void SetDefaultValues()
	{
		playerName = "PLAYER";
		items = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
		weapon = new int[3] { 3, 6, 8 };
		armor = new int[3] { 4, 4, 9 };
		hp = new int[3] { 20, 30, 20 };
		deaths = 0;
		gold = 0;
		atBuffs = new int[3];
		dfBuffs = new int[3];
		miniPartyMember = 0;
		exp = 0;
		playTime = 0;
		playTimeFrames = 0;
		flags = new object[1000];
		persFlags = new object[1000];
		sessionFlags = new object[100];
		SetFlag(0, "neutral");
		SetFlag(1, "neutral");
		SetFlag(2, "neutral");
		SetFlag(12, 1);
		SetSessionFlag(11, 2);
		menuLocked = false;
		ending = -1;
		if (dev && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor))
		{
			SetFlag(168, 1);
			SetFlag(180, 1);
			SetFlag(181, 2);
			SetFlag(182, 1);
			SetFlag(190, 2);
			SetFlag(191, 1);
			SetFlag(203, 1);
			SetFlag(205, 2);
			SetFlag(204, 1);
			SetFlag(210, 1);
			SetFlag(206, 2);
			SetFlag(207, 2);
			SetFlag(212, 1);
			SetFlag(221, 1);
			SetFlag(222, 1);
			SetFlag(227, 1);
			SetFlag(228, 1);
			SetFlag(234, 1);
			SetFlag(236, 1);
			SetFlag(253, 2);
			SetFlag(245, 2);
			SetFlag(262, 1);
			SetFlag(318, 1);
			SetPartyMembers(true, true);
			HealAll(99);
			SetFlag(84, 11);
			SetFlag(293, 1);
			weapon = new int[3] { 13, 15, 8 };
			armor = new int[3] { 19, 19, 19 };
			items[0] = 24;
			items[1] = 44;
			items[2] = 45;
			items[3] = 23;
			items[4] = 23;
			items[5] = 23;
			items[6] = 23;
			items[7] = 16;
			gold = 150;
		}
	}

	public SAVEFile GetFile()
	{
		SAVEFile sAVEFile = new SAVEFile();
		sAVEFile.UpdateCharacterInfo(playerName, exp, items, weapon, armor, susieActive, noelleActive, playTime, zone, gold, "[???]", flags);
		sAVEFile.UpdatePersistentFlags(persFlags);
		sAVEFile.UpdateDeathCount(deaths);
		return sAVEFile;
	}

	public bool FileExists()
	{
		string path = "SAVE" + fileID + ".sav";
		return File.Exists(Path.Combine(Application.persistentDataPath, path));
	}

	public string GetFileName()
	{
		if (FileExists())
		{
			return save.name;
		}
		if ((int)GetFlag(108) == 1)
		{
			return "[EMPTY]";
		}
		return "Kris";
	}

	public string GetFormattedPlayTime()
	{
		if (FileExists())
		{
			string text = Mathf.FloorToInt((float)playTime / 3600f).ToString();
			string text2 = Mathf.FloorToInt((float)playTime % 3600f / 60f).ToString();
			string text3 = (save.playTime % 60).ToString();
			if (text3.Length == 1)
			{
				text3 = "0" + text3;
			}
			if (text2.Length == 1)
			{
				text2 = "0" + text2;
			}
			return text + ":" + text2 + ":" + text3;
		}
		return "--:--";
	}

	public string GetFormattedPlayTimeFromTime(int playTime)
	{
		string text = Mathf.FloorToInt((float)playTime / 3600f).ToString();
		string text2 = Mathf.FloorToInt((float)playTime % 3600f / 60f).ToString();
		string text3 = (playTime % 60).ToString();
		if (text3.Length == 1)
		{
			text3 = "0" + text3;
		}
		if (text2.Length == 1)
		{
			text2 = "0" + text2;
		}
		return text + ":" + text2 + ":" + text3;
	}

	public string GetFormattedUpdatedPlayTime()
	{
		string text = Mathf.FloorToInt((float)playTime / 3600f).ToString();
		string text2 = Mathf.FloorToInt((float)playTime % 3600f / 60f).ToString();
		string text3 = (playTime % 60).ToString();
		if (text3.Length == 1)
		{
			text3 = "0" + text3;
		}
		if (text2.Length == 1)
		{
			text2 = "0" + text2;
		}
		return text + ":" + text2 + ":" + text3;
	}

	public string GetFileZone()
	{
		if (FileExists())
		{
			return MapInfo.GetMapName(save.zone);
		}
		return "---";
	}

	public int GetFileLV()
	{
		if (FileExists())
		{
			return GetLV(save.exp);
		}
		return GetLV();
	}

	public int GetFileID()
	{
		return fileID;
	}

	public bool IsEasyMode()
	{
		if ((GetFlagInt(13) > 1 && GetFlagInt(127) == 1) || GetFlagInt(13) > 2)
		{
			return false;
		}
		return options.easyMode.value > 0;
	}

	public void CopyFile(int from, int to)
	{
		string path = "SAVE" + from + ".sav";
		string path2 = "SAVE" + to + ".sav";
		File.Copy(Path.Combine(Application.persistentDataPath, path), Path.Combine(Application.persistentDataPath, path2), true);
	}

	public void DeleteFile(int id)
	{
		string path = "SAVE" + id + ".sav";
		File.Delete(Path.Combine(Application.persistentDataPath, path));
	}

	public static void UpdateVolume(int volume)
	{
		AudioListener.volume = (float)volume / 100f;
	}

	public static void SetOptions(Options newOptions)
	{
		options = newOptions;
	}

	public static Options GetOptions()
	{
		return options;
	}

	public string GetVersion()
	{
		string[] array = Application.version.Split('.');
		return string.Format("{0}.{1}.{2}", array[0], array[1], array[2]);
	}

	public string GetVersionBuild()
	{
		return Application.version;
	}

	public string GetBuild()
	{
		return Application.version.Substring(5);
	}

	public bool IsTestMode()
	{
		return dev;
	}

	public static bool UsingRecordingSoftware()
	{
		try
		{
			List<string> list = new List<string> { "obs64", "obs32", "bdcam", "XSplit.Core" };
			Process[] processes = Process.GetProcesses();
			foreach (Process process in processes)
			{
				try
				{
					if (list.Contains(process.ProcessName))
					{
						return true;
					}
				}
				catch
				{
					UnityEngine.Debug.LogWarning("Process skipped");
				}
			}
		}
		catch
		{
			UnityEngine.Debug.LogWarning("Couldn't detect recording software");
		}
		return false;
	}

	public int GetEBTextColorID()
	{
		switch (GetPlayerName().ToUpper())
		{
		case "KRIS":
		case "FRISK":
			return 1;
		case "SUSIE":
		case "SUZY":
		case "SARAH":
		case "RYNO":
		case "VYLET":
			return 2;
		case "NOELLE":
		case "NOEL":
		case "CLOVER":
			return 3;
		case "DESS":
			return 4;
		case "SANS":
		case "NESS":
		case "SCOOT":
		case "TULIP":
			return 5;
		case "PAULA":
			return 6;
		case "RALSEI":
		case "CHARA":
			return 7;
		case "ISSUE":
			return 8;
		case "MADMEWMEW":
		case "SOPHIE":
			return 9;
		default:
			return 0;
		}
	}

	public void SetFramerate(int fps)
	{
		Application.targetFrameRate = fps;
	}

	public void LoadConfigData()
	{
		UpdateVolume(config.GetInt("General", "Volume", 60, true));
		packManager.SetPack(config.GetString("General", "LanguagePack", "", true));
		options.LoadFromConfig(ref config);
		config.Write();
	}
}
