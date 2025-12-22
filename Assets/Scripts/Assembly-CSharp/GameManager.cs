using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Tilemaps;

public class GameManager : MonoBehaviour
{
	public static readonly int FULL_COMPLETION = 3;

	public static readonly int FULL_MURDER_LEVEL = 10;

	public static GameManager instance = null;

	private bool menuDisabled;

	private bool menuLocked;

	private GameObject menu;

	private bool menuIsOpen;

	private bool dev;

	private string playerName;

	private int[] party = new int[6] { 0, 1, -1, -1, -1, -1 };

	private List<int> items;

	private List<int> equipItems;

	private List<int> boxItems;

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

	private bool forcedBattleEnd;

	private int ending = -1;

	private MonitorInfo monitorInfo;

	private bool monitorInfoEnabled;

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

	private static Options options = new Options();

	private UnoGameManager unoGm;

	private bool inSingleBattle;

	private bool onPrimaryDisplay;

	public static bool autoLowGraphics = false;

	private int curFps = 30;

	private int refreshRate = 60;

	private List<DisplayInfo> displayInfo = new List<DisplayInfo>();

	private void Awake()
	{
		UnityEngine.Object.DontDestroyOnLoad(base.gameObject);
		if (!instance)
		{
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
			PersistentSAVE.Load();
			SetDefaultValues();
			ConvertOldFile();
			save = new SAVEFile();
			config = new Config("config.ini");
			LoadConfigData();
			UpdateWindow();
			base.gameObject.AddComponent<UTInput>();
			Screen.GetDisplayLayout(displayInfo);
			refreshRate = Mathf.RoundToInt((float)displayInfo[0].refreshRate.value);
			SetFramerate(curFps);
			ExceptionHandler.Init();
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
		if (Util.OverworldPlayer() != null)
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
			PlayMusic(Util.FindObjectOfType<CameraController>().GetZoneMusic(), Util.FindObjectOfType<CameraController>().GetZoneMusicPitch());
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
		Screen.GetDisplayLayout(displayInfo);
		if (displayInfo[0].refreshRate.value != Screen.mainWindowDisplayInfo.refreshRate.value)
		{
			onPrimaryDisplay = false;
			QualitySettings.vSyncCount = 0;
		}
		else
		{
			int num = Mathf.RoundToInt((float)displayInfo[0].refreshRate.value);
			if (refreshRate != num || !onPrimaryDisplay)
			{
				onPrimaryDisplay = true;
				refreshRate = num;
				SetFramerate(curFps);
			}
		}
		if (monitorInfoEnabled && !monitorInfo)
		{
			monitorInfo = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/debug/MonitorInfoCanvas")).GetComponentInChildren<MonitorInfo>();
			UnityEngine.Object.DontDestroyOnLoad(monitorInfo.transform.parent.gameObject);
		}
		else if (!monitorInfoEnabled && (bool)monitorInfo)
		{
			UnityEngine.Object.Destroy(monitorInfo.gameObject);
		}
		if (UTInput.GetButtonDown("C") && (bool)Util.OverworldPlayer() && !menuIsOpen && !menuDisabled && !menuLocked)
		{
			if (SceneManager.GetActiveScene().buildIndex == 123)
			{
				menu = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/OverworldMenuHD"), GameObject.Find("Canvas").transform);
			}
			else
			{
				menu = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/OverworldMenu"), GameObject.Find("Canvas").transform);
			}
			DisablePlayerMovement(deactivatePartyMembers: false);
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
				SpawnFromLastSave(respawn: false);
			}
			KeyCode[] keys = DebugTools.GetKeys();
			foreach (KeyCode keyCode in keys)
			{
				if (Input.GetKeyDown(keyCode))
				{
					if (!Util.OverworldPlayer())
					{
						DebugTools.UseTool(keyCode);
						break;
					}
					if (keyCode == KeyCode.F6 || Util.OverworldPlayer().CanMove() || (bool)Util.FindObjectOfType<BattleManager>())
					{
						DebugTools.UseTool(keyCode);
						break;
					}
				}
			}
		}
		if (Input.GetKeyDown(KeyCode.F4))
		{
			SetFullscreen(!GetFullscreen());
			UpdateWindow();
		}
	}

	private void OnDestroy()
	{
		if (instance == this)
		{
			if ((bool)UnityEngine.Object.FindAnyObjectByType<Fade>())
			{
				UnityEngine.Object.Destroy(UnityEngine.Object.FindAnyObjectByType<Fade>().gameObject);
			}
			if ((bool)GameObject.Find("FadeCanvas"))
			{
				UnityEngine.Object.Destroy(GameObject.Find("FadeCanvas"));
			}
			if ((bool)UnityEngine.Object.FindAnyObjectByType<QuitFunction>())
			{
				UnityEngine.Object.Destroy(UnityEngine.Object.FindAnyObjectByType<QuitFunction>().gameObject);
			}
		}
	}

	public void Disable()
	{
		StopMusic();
		GetComponent<UTInput>().enabled = false;
		if ((bool)UnityEngine.Object.FindAnyObjectByType<Fade>())
		{
			UnityEngine.Object.FindAnyObjectByType<Fade>().FadeIn(0);
			UnityEngine.Object.FindAnyObjectByType<Fade>().enabled = false;
		}
		if ((bool)UnityEngine.Object.FindAnyObjectByType<QuitFunction>())
		{
			UnityEngine.Object.FindAnyObjectByType<QuitFunction>().enabled = false;
		}
		base.enabled = false;
	}

	public void Enable()
	{
		GetComponent<UTInput>().enabled = true;
		if ((bool)UnityEngine.Object.FindAnyObjectByType<Fade>())
		{
			UnityEngine.Object.FindAnyObjectByType<Fade>().enabled = true;
		}
		if ((bool)UnityEngine.Object.FindAnyObjectByType<QuitFunction>())
		{
			UnityEngine.Object.FindAnyObjectByType<QuitFunction>().enabled = true;
		}
		base.enabled = true;
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
		if (Util.OverworldPlayer() != null)
		{
			Util.OverworldPlayer().SetMovement(newMove: false, !deactivatePartyMembers);
		}
		menuIsOpen = true;
	}

	public void EnablePlayerMovement()
	{
		bool flag = true;
		if (Util.OverworldPlayer() != null)
		{
			if (Util.OverworldPlayer().CannotMoveBattleSpecial())
			{
				flag = false;
			}
			Util.OverworldPlayer().SetMovement(newMove: true);
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
		if ((bool)Util.OverworldPlayer())
		{
			Util.OverworldPlayer().SetCollision(onoff: true);
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
		SceneManager.LoadScene(78, LoadSceneMode.Single);
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
		if (!Util.FindObjectOfType<BattleManager>())
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
					spawnPos = Util.FindObjectOfType<SAVEPoint>().GetSpawnPosition();
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
		obj2.GetComponent<SOUL>().CreateSOUL(new Color(1f, 0f, 0f), monster: false, player: true);
		obj2.GetComponent<SpriteRenderer>().sortingOrder = 500;
		unoGm = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/UnoGameManager")).GetComponent<UnoGameManager>();
		unoGm.SetupPlayers();
		unoGm.StartGame(MusicChooser.musicID, apointSystem: false, astackableDraw: true, achallengableFour: true, adrawCard: false);
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
		obj2.GetComponent<SOUL>().CreateSOUL(new Color(1f, 0f, 0f), monster: false, player: true);
		obj2.GetComponent<SpriteRenderer>().sortingOrder = 500;
		obj.GetComponent<Fade>().FadeIn(5);
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/BattleManager")).GetComponent<BattleManager>().StartBattle(battleId);
	}

	public void EndBattle(int battleEndState, bool force = false)
	{
		forcedBattleEnd = force;
		ResetAllBuffs();
		if ((bool)Util.FindObjectOfType<TouchPad>())
		{
			Util.FindObjectOfType<TouchPad>().SetSoulColor(SOUL.GetSOULColorByID(GetFlagInt(312), forceNormal: true));
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
			if (((Behaviour)(object)tilemapRenderer.GetComponent<Tilemap>()).enabled)
			{
				tilemapRenderer.enabled = true;
			}
		}
		SpriteMask[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<SpriteMask>();
		for (int i = 0; i < componentsInChildren5.Length; i++)
		{
			componentsInChildren5[i].enabled = true;
		}
		Util.OverworldPlayer().GetComponent<SpriteRenderer>().enabled = true;
		Util.OverworldPlayer().SetCollision(onoff: true);
		OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ShowSprite();
		}
		ForceTogglePlayers(tog: true);
		EnablePlayerMovement();
		ResumeMusic(12);
		if ((bool)Util.FindObjectOfType<LostCoreMusic>())
		{
			Util.FindObjectOfType<LostCoreMusic>().SetDanger(danger: false);
		}
		Util.FindObjectOfType<Fade>().FadeIn(12);
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
		OverworldPlayer overworldPlayer = Util.OverworldPlayer();
		if ((bool)overworldPlayer)
		{
			overworldPlayer.GetComponent<OverworldPlayer>().enabled = tog;
			overworldPlayer.GetComponent<SpriteRenderer>().enabled = tog;
			OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
			foreach (OverworldPartyMember obj in array)
			{
				obj.GetComponent<OverworldPartyMember>().enabled = tog;
				obj.GetComponent<SpriteRenderer>().enabled = tog;
			}
		}
	}

	public void Death(int specialText = -1)
	{
		deaths++;
		SetSessionFlag(7, specialText);
		if (!inSingleBattle && FileExists())
		{
			SaveFile(savepoint: false);
		}
		inSingleBattle = false;
		SceneManager.LoadScene(3, LoadSceneMode.Single);
		spawnPos = Vector2.zero;
		if (Util.FindObjectOfType<SOUL>() != null)
		{
			spawnPos = Util.FindObjectOfType<SOUL>().transform.position - GameObject.Find("BattleCamera").transform.position;
		}
		else if (Util.FindObjectOfType<ActionSOUL>() != null)
		{
			if (Util.FindObjectOfType<ActionSOUL>().transform.childCount > 0)
			{
				spawnPos = Util.FindObjectOfType<ActionSOUL>().transform.GetChild(0).position - Util.FindObjectOfType<CameraController>().transform.position;
			}
			else
			{
				spawnPos = Util.FindObjectOfType<ActionSOUL>().transform.position - Util.FindObjectOfType<CameraController>().transform.position;
			}
		}
		else if (Util.OverworldPlayer() != null)
		{
			spawnPos = Util.OverworldPlayer().transform.position - Util.FindObjectOfType<CameraController>().transform.position;
		}
		SceneManager.sceneLoaded += OnDeathScreenLoaded;
	}

	public void OnDeathScreenLoaded(Scene ascene, LoadSceneMode aMode)
	{
		DisablePlayerMovement(deactivatePartyMembers: true);
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

	public List<int> GetEquipmentItemList()
	{
		return equipItems;
	}

	public List<int> GetBoxList()
	{
		return boxItems;
	}

	public void SetBoxList(List<int> boxItems)
	{
		this.boxItems = boxItems;
	}

	public int FirstFreeItemSpace(bool equipment)
	{
		int[] array = (equipment ? equipItems.ToArray() : items.ToArray());
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == -1)
			{
				return i;
			}
		}
		return -1;
	}

	public int FirstFreeItemSpace(int item)
	{
		return FirstFreeItemSpace(Items.IsEquipment(item));
	}

	public int NumItemFreeSpace(bool equipment)
	{
		int[] array = (equipment ? equipItems.ToArray() : items.ToArray());
		int num = 0;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == -1)
			{
				num++;
			}
		}
		return num;
	}

	public int NumItemFreeSpace(int item)
	{
		return NumItemFreeSpace(Items.IsEquipment(item));
	}

	public void AddItem(int item)
	{
		if (item == 16)
		{
			SetFlag(286, 1);
		}
		else if (item > -1)
		{
			items[FirstFreeItemSpace(equipment: false)] = item;
		}
	}

	public void RemoveItem(int index)
	{
		if (GetItem(index) == 45)
		{
			SetFlag(312, 0);
			if ((bool)Util.FindObjectOfType<SOUL>())
			{
				Util.FindObjectOfType<SOUL>().AdjustSOULColor();
			}
			if ((bool)Util.FindObjectOfType<TouchPad>())
			{
				Util.FindObjectOfType<TouchPad>().SetSoulColor(Color.red);
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

	public void MoveItemToBack(int index)
	{
		int item = items[index];
		items.RemoveAt(index);
		items.Add(item);
	}

	public int GetItem(int index)
	{
		return items[index];
	}

	public void AddEquipment(int item)
	{
		if (item > -1)
		{
			equipItems[FirstFreeItemSpace(equipment: true)] = item;
		}
	}

	public void RemoveEquipment(int index)
	{
		equipItems.RemoveAt(index);
		equipItems.Add(-1);
	}

	public void RemoveEquipmentByID(int item)
	{
		for (int i = 0; i < 8; i++)
		{
			if (equipItems[i] == item)
			{
				RemoveEquipment(i);
				break;
			}
		}
	}

	public int GetEquipment(int index)
	{
		return equipItems[index];
	}

	public void AddAmbiguousItem(int item)
	{
		if (Items.IsEquipment(item))
		{
			AddEquipment(item);
		}
		else
		{
			AddItem(item);
		}
	}

	public int GetWeapon(int slot)
	{
		return PartyMembers.GetWeapon(party[slot]);
	}

	public int GetArmor(int slot)
	{
		return PartyMembers.GetArmor(party[slot]);
	}

	public void ForceWeapon(int slot, int i)
	{
		PartyMembers.SetWeapon(party[slot], i);
	}

	public void ForceArmor(int slot, int i)
	{
		PartyMembers.SetArmor(party[slot], i);
	}

	public void ChangeWeapon(int slot, int index)
	{
		int weapon = PartyMembers.GetWeapon(party[slot]);
		PartyMembers.SetWeapon(party[slot], equipItems[index]);
		RemoveEquipment(index);
		AddAmbiguousItem(weapon);
	}

	public void ChangeArmor(int slot, int index)
	{
		int num = PartyMembers.GetArmor(party[slot]);
		PartyMembers.SetArmor(party[slot], equipItems[index]);
		RemoveEquipment(index);
		if (num == 4)
		{
			num = 7;
		}
		if (num == 7 && NumItemFreeSpace(equipment: false) == 0)
		{
			AddEquipment(7);
		}
		else
		{
			AddAmbiguousItem(num);
		}
	}

	public void EatItem(int slot, int index, bool useEquipmentInventory = false)
	{
		int num = GetItem(index);
		if (useEquipmentInventory)
		{
			num = GetEquipment(index);
		}
		if (num == 17 && party[slot] == 0)
		{
			int num2 = Items.ItemValue(num, slot);
			int num3 = GetHP(slot) + num2;
			int num4 = GetMaxHP(slot) + 16;
			if (num3 < num4)
			{
				SetHP(slot, num3, forceOverheal: true);
			}
			else
			{
				SetHP(slot, num4, forceOverheal: true);
			}
		}
		else if (num == 28)
		{
			if (party[slot] == 1)
			{
				SetHP(slot, GetMaxHP(slot) + 10, forceOverheal: true);
			}
			else if (GetHP(slot) < GetMaxHP(slot) - 1)
			{
				SetHP(slot, GetMaxHP(slot) - 1);
			}
		}
		else if (num == 39 && party[slot] == 2)
		{
			if (GetHP(slot) - GetMaxHP(slot) < 5)
			{
				SetHP(slot, GetMaxHP(slot) + 5, forceOverheal: true);
			}
		}
		else
		{
			int num5 = Items.ItemValue(num, slot);
			if (num5 > 0)
			{
				Heal(slot, num5);
			}
			else if (num5 < 0)
			{
				Damage(slot, -num5);
			}
		}
		if (useEquipmentInventory)
		{
			RemoveEquipment(index);
		}
		else
		{
			RemoveItem(index);
		}
	}

	public void UseItem(int slot, int index, bool equipment)
	{
		if (equipment)
		{
			if (!Items.CanEquipItem((PartyMembers.ID)party[slot], GetEquipment(index)))
			{
				return;
			}
			if (Items.ItemType(GetEquipment(index)) == 1)
			{
				PlayGlobalSFX("sounds/snd_item");
				aud.Play();
				ChangeWeapon(slot, index);
			}
			else if (Items.ItemType(GetEquipment(index)) == 2)
			{
				PlayGlobalSFX("sounds/snd_item");
				ChangeArmor(slot, index);
				if ((bool)Util.FindObjectOfType<SOULGraze>())
				{
					Util.FindObjectOfType<SOULGraze>().UpdateGrazeSize();
				}
			}
			else if (GetEquipment(index) == 7)
			{
				PlayGlobalSFX("sounds/snd_heal");
				EatItem(slot, index, useEquipmentInventory: true);
			}
		}
		else if (Items.ItemType(GetItem(index)) == 0)
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
					if (!Util.FindObjectOfType<BattleManager>() && GetItem(index) == 22)
					{
						Util.OverworldPlayer().SetSpeedMultiplier(1.5f);
					}
				}
				else
				{
					healAudSound = "sounds/snd_heal";
				}
			}
			EatItem(slot, index);
			if (item == 35)
			{
				AddItem(36);
			}
		}
		else if (Items.ItemType(GetItem(index)) == 4)
		{
			PlayGlobalSFX("sounds/snd_swallow");
			healAudFrames = 1;
			healAudSound = "sounds/snd_heal";
			int num = Items.ItemValue(GetItem(index));
			if (num > 0)
			{
				HealAll(num, includeOutOfParty: false);
			}
			else if (num < 0)
			{
				Damage(0, -num);
				Damage(1, -num);
				Damage(2, -num);
				Damage(3, -num);
				Damage(4, -num);
				Damage(5, -num);
			}
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
		if (music == "zoneMusic" && (bool)Util.FindObjectOfType<CameraController>())
		{
			music = Util.FindObjectOfType<CameraController>().GetZoneMusic();
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
			pitch = Util.FindObjectOfType<CameraController>().GetZoneMusicPitch();
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
		if (music.EndsWith("mus_muscle") && playerName == "SHAYY" && (zone != 115 || GetFlagInt(291) == 0))
		{
			music = "music/mus_muscle_improved";
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
			mp.ChangeMusic(music, intro, playImmediately: true);
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

	public int GetHP(int slot)
	{
		return PartyMembers.GetHP(party[slot]);
	}

	public int[] GetHPArray()
	{
		return new int[6]
		{
			PartyMembers.GetHP(party[0]),
			PartyMembers.GetHP(party[1]),
			PartyMembers.GetHP(party[2]),
			PartyMembers.GetHP(party[3]),
			PartyMembers.GetHP(party[4]),
			PartyMembers.GetHP(party[5])
		};
	}

	public int GetCombinedHP()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			num += GetHP(i);
		}
		return num;
	}

	public int GetCombinedHPNoOverheal()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			num = ((GetHP(i) <= GetMaxHP(i)) ? (num + GetHP(i)) : (num + GetMaxHP(i)));
		}
		return num;
	}

	public int GetCombinedMaxHP()
	{
		int num = 0;
		for (int i = 0; i < 6; i++)
		{
			num += GetMaxHP(i);
		}
		return num;
	}

	public int GetMaxHP(int slot)
	{
		return GetMaxHP(slot, exp);
	}

	public int GetMaxHP(int slot, int exp)
	{
		return PartyMembers.GetMaxHP(party[slot], exp);
	}

	public int GetMiniMemberMaxHP()
	{
		return PartyMembers.GetMaxHP(party[3], exp);
	}

	public bool KrisInControl()
	{
		if (PartyMembers.GetHP(0) > 0)
		{
			return party[0] != 0;
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

	public void Heal(int slot, int heal)
	{
		PartyMembers.Heal(party[slot], heal);
	}

	public void HealAll(int heal, bool includeOutOfParty = true)
	{
		PartyMembers.HealAll(heal, includeOutOfParty);
	}

	public void Damage(int slot, int dmg)
	{
		PartyMembers.Damage(party[slot], dmg);
	}

	public void DetermineDeath()
	{
		if (GetCombinedHP() == 0)
		{
			Death();
		}
	}

	public int[] HandleDamageCalculations(int hp, float damageMulti, bool applyDamageImmediately = true, bool[] forceAttackMinis = null)
	{
		PartyPanels partyPanels = Util.FindObjectOfType<PartyPanels>();
		SOUL sOUL = Util.FindObjectOfType<SOUL>();
		KarmaHandler karmaHandler = Util.FindObjectOfType<KarmaHandler>();
		int[] array = new int[6]
		{
			PartyMembers.GetHP(party[0]),
			PartyMembers.GetHP(party[1]),
			PartyMembers.GetHP(party[2]),
			PartyMembers.GetHP(party[3]),
			PartyMembers.GetHP(party[4]),
			PartyMembers.GetHP(party[5])
		};
		float num = hp;
		int num2 = -1;
		if ((bool)partyPanels)
		{
			AttackBase attackBase = Util.FindObjectOfType<AttackBase>();
			if ((object)attackBase != null && !attackBase.AttackingAllTargets())
			{
				bool flag = false;
				bool flag2 = false;
				List<PartyMembers.ID> list = new List<PartyMembers.ID>
				{
					PartyMembers.ID.Kris,
					PartyMembers.ID.Frisk,
					PartyMembers.ID.Chara,
					PartyMembers.ID.Paula
				};
				do
				{
					if (flag2)
					{
						flag = true;
					}
					num2 = UnityEngine.Random.Range(0, partyPanels.NumTargettedMembers());
					if (partyPanels.NumTargettedMembers() == 2 && num2 == 1)
					{
						num2 = (PartySlotFilled(1) ? 1 : 2);
					}
					if ((GetHP(num2) <= 0 && GetHP(num2 + 3) <= 0) || !partyPanels.GetTargettedMembers()[num2])
					{
						switch (num2)
						{
						case 2:
							num2 -= (((GetHP(1) > 0 || GetHP(4) > 0) && partyPanels.GetTargettedMembers()[1]) ? 1 : 2);
							break;
						case 1:
							num2 += (((GetHP(2) > 0 || GetHP(5) > 0) && partyPanels.GetTargettedMembers()[2]) ? 1 : (-1));
							break;
						case 0:
							num2 += (((GetHP(1) > 0 || GetHP(4) > 0) && partyPanels.GetTargettedMembers()[1]) ? 1 : 2);
							break;
						}
					}
					int num3 = ((GetHP(num2) > 0) ? num2 : (num2 + 3));
					PartyMembers.ID partyMember = (PartyMembers.ID)GetPartyMember(num3);
					if ((float)GetHP(num3) / (float)GetMaxHP(num3) <= 0.25f && !flag2 && list.Contains(partyMember))
					{
						flag2 = true;
					}
				}
				while (!flag && flag2);
			}
		}
		bool[] array2 = new bool[3];
		for (int i = 0; i < 3; i++)
		{
			if (num2 == -1 || num2 == i)
			{
				int num4 = ((GetHP(i) > 0) ? i : (i + 3));
				if (array2[i])
				{
					num4 = i + 3;
				}
				if ((bool)sOUL && sOUL.PapCharmWasHit(num4))
				{
					continue;
				}
				float num5 = num;
				float num6 = (float)GetDEF(num4) / 3f;
				num5 -= num6;
				float num7 = 1f + (float)(GetLV() / 2) / 10f;
				if ((bool)karmaHandler)
				{
					int num8 = Mathf.RoundToInt((num5 * num7 - num5) * 2f);
					karmaHandler.AddKarma(i, (num8 <= 1) ? 1 : num8);
				}
				else
				{
					num5 *= num7;
				}
				if ((bool)partyPanels && Util.FindObjectOfType<BattleManager>().GetDefendingMembers()[i])
				{
					num5 *= 2f / 3f;
				}
				if ((bool)sOUL && sOUL.IsShieldActive())
				{
					num5 *= 2f / 3f;
				}
				if (IsEasyMode())
				{
					num5 *= 2f / 3f;
				}
				num5 *= damageMulti;
				if (num5 < 1f)
				{
					num5 = 1f;
				}
				if ((bool)partyPanels && num2 == -1)
				{
					if (partyPanels.NumTargettedMembers() == 2)
					{
						num5 *= 0.8f;
					}
					else if (partyPanels.NumTargettedMembers() == 3)
					{
						num5 *= 0.65f;
					}
				}
				if (((bool)partyPanels && partyPanels.GetTargettedMembers()[i]) || !partyPanels)
				{
					int num9 = Mathf.RoundToInt(num5);
					if (applyDamageImmediately)
					{
						Damage(num4, num9);
					}
					array[num4] -= num9;
				}
			}
			if (forceAttackMinis != null && forceAttackMinis[i] && !array2[i])
			{
				array2[i] = true;
				i--;
			}
		}
		return array;
	}

	public void SetHP(int slot, int newHP, bool forceOverheal = false)
	{
		PartyMembers.SetHP(party[slot], newHP, forceOverheal);
	}

	public int GetATK(int slot)
	{
		return PartyMembers.GetATK(party[slot]);
	}

	public int GetATKRaw(int slot)
	{
		return PartyMembers.GetATKRaw(party[slot]);
	}

	public int GetDEF(int slot)
	{
		return PartyMembers.GetDEF(party[slot]);
	}

	public int GetDEFRaw(int slot)
	{
		return PartyMembers.GetDEFRaw(party[slot]);
	}

	public float GetMagic(int slot)
	{
		return PartyMembers.GetMagic(party[slot]);
	}

	public int GetMagicEquipment(int slot)
	{
		return PartyMembers.GetMagicEquipment(party[slot]);
	}

	public float GetMagicRaw(int slot)
	{
		return PartyMembers.GetMagicRaw(party[slot]);
	}

	public void SetATKBuff(int slot, int buff)
	{
		atBuffs[slot] = buff;
	}

	public void SetDEFBuff(int slot, int buff)
	{
		dfBuffs[slot] = buff;
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

	public int GetPartyMember(int slotID)
	{
		int num = party[slotID];
		if (!PartyMembers.IsMemberAllowedInSlot(num, slotID))
		{
			UnityEngine.Debug.LogError(PartyMembers.GetMemberName(num) + " is not allowed to occupy slot " + slotID + "... fix this.");
		}
		return num;
	}

	public void SetPartyMember(int slotID, int member)
	{
		if (!PartyMembers.IsMemberAllowedInSlot(member, slotID))
		{
			UnityEngine.Debug.LogError(PartyMembers.GetMemberName(member) + " is not allowed to occupy slot " + slotID + "... fix this.");
		}
		party[slotID] = member;
		if ((bool)Util.OverworldPlayer())
		{
			Util.OverworldPlayer().ResetPartyMemberList();
		}
	}

	public void SetPartyMembers(bool susie, bool noelle)
	{
		party[1] = -1;
		party[2] = -1;
		if (susie && noelle)
		{
			party[1] = 1;
			party[2] = 2;
		}
		else if (susie)
		{
			party[1] = 1;
		}
		else if (noelle)
		{
			party[1] = 2;
		}
		if ((bool)Util.OverworldPlayer())
		{
			Util.OverworldPlayer().ResetPartyMemberList();
		}
	}

	public void SetPartyMembers(int[] party)
	{
		this.party = party;
		if ((bool)Util.OverworldPlayer())
		{
			Util.OverworldPlayer().ResetPartyMemberList();
		}
	}

	public int[] GetParty()
	{
		return party;
	}

	public bool PartySlotFilled(int slot)
	{
		return party[slot] != -1;
	}

	public bool SusieInParty()
	{
		return party[1] == 1;
	}

	public bool NoelleInParty()
	{
		if (party[1] != 2)
		{
			return party[2] == 2;
		}
		return true;
	}

	public int NumActivePartyMembers(bool includeMinis = false)
	{
		int num = 0;
		for (int i = 0; i < (includeMinis ? 6 : 3); i++)
		{
			if (party[i] > -1)
			{
				num++;
			}
		}
		return num;
	}

	public int[] GetActivePartySlots(bool includeMinis = false)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < (includeMinis ? 6 : 3); i++)
		{
			if (party[i] > -1)
			{
				list.Add(i);
			}
		}
		return list.ToArray();
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
			SaveFile(savepoint: false);
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
			save.UpdateCharacterInfo(playerName, exp, items, equipItems, boxItems, party, PartyMembers.GetAllHP(), PartyMembers.GetAllWeapon(), PartyMembers.GetAllArmor(), playTime, zone, gold, flags);
		}
		save.UpdatePersistentFlags(persFlags);
		save.UpdateDeathCount(deaths);
		string path = "SAVE" + fileID + ".sav";
		using FileStream stream = File.Open(Path.Combine(Application.persistentDataPath, path), FileMode.OpenOrCreate);
		SAVEFileIO.WriteFile(ref save, stream);
	}

	public void SetCheckpoint(int respawnZone)
	{
		checkpointEnabled = true;
		checkpointSave = new SAVEFile();
		checkpointSave.UpdateCharacterInfo(playerName, exp, items, equipItems, boxItems, party, PartyMembers.GetAllHP(), PartyMembers.GetAllWeapon(), PartyMembers.GetAllArmor(), playTime, respawnZone, gold, flags);
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
		using FileStream fs = File.Open(Path.Combine(Application.persistentDataPath, path), FileMode.Open);
		SAVEFileIO.ReadFile(ref save, fs);
	}

	public void ConvertOldFile()
	{
		if (File.Exists(Path.Combine(Application.persistentDataPath, "SAVE.sav")))
		{
			File.Move(Path.Combine(Application.persistentDataPath, "SAVE.sav"), Path.Combine(Application.persistentDataPath, "SAVE0.sav"));
		}
	}

	public void NewGame(string playerName)
	{
		SetPlayerName(playerName);
		if (GetPlayerName() == "FRISK")
		{
			SetPartyMember(0, 6);
			SetFlag(108, 1);
			ForceWeapon(0, 25);
		}
		SetFlag(223, options.startingFlavor.value);
		SetPartyMembers(susie: true, noelle: false);
	}

	public void SpawnFromLastSave(bool respawn)
	{
		ResetAllBuffs();
		if (!respawn)
		{
			SetDefaultValues(ignoreDebug: true);
			sessionFlags = new object[100];
		}
		if (!FileExists() && !(checkpointEnabled && respawn))
		{
			return;
		}
		if (checkpointEnabled && respawn)
		{
			flags = (object[])checkpointSave.flags.Clone();
			playerName = checkpointSave.name;
			exp = checkpointSave.exp;
			items = new List<int>(checkpointSave.items);
			equipItems = new List<int>(checkpointSave.equipItems);
			boxItems = new List<int>(checkpointSave.boxItems);
			party = (int[])checkpointSave.party.Clone();
			PartyMembers.SetAllHP((int[])checkpointSave.hp.Clone(), maxPartyMembers: true);
			PartyMembers.SetAllWeapon((int[])checkpointSave.weapon.Clone());
			PartyMembers.SetAllArmor((int[])checkpointSave.armor.Clone());
			playTime = checkpointSave.playTime;
			zone = checkpointSave.zone;
			gold = checkpointSave.gold;
			if (forceRespawnZone > -1)
			{
				zone = forceRespawnZone;
				forceRespawnZone = -1;
			}
			StartTime();
			LoadArea(zone, fadeIn: true, Vector2.zero, Vector2.down, fromSavePoint: true);
			return;
		}
		flags = (object[])save.flags.Clone();
		playerName = save.name;
		exp = save.exp;
		items = new List<int>(save.items);
		equipItems = new List<int>(save.equipItems);
		boxItems = new List<int>(save.boxItems);
		party = (int[])save.party.Clone();
		PartyMembers.SetAllHP((int[])save.hp.Clone());
		PartyMembers.SetAllWeapon((int[])save.weapon.Clone());
		PartyMembers.SetAllArmor((int[])save.armor.Clone());
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
			LoadArea(zone, respawn, Vector2.zero, Vector2.down, fromSavePoint: true);
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
				WeirdChecker.AdvanceTo(this, GetFlagInt(13), sound: false);
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
		if ((GetFlagInt(108) == 1 || party[0] == 6) && (num > 1 || Util.GameManager().PartySlotFilled(3) || NoelleInParty()))
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
		int num3 = party[3];
		UnityEngine.Debug.Log("Party Member: " + PartyMembers.GetMemberName(num3));
		if (num3 == 3)
		{
			UnityEngine.Debug.Log("Oblit Progress: " + GetFlag(87)?.ToString() + ", Seen Paula: " + GetFlag(103)?.ToString() + ", Section: " + num);
			if (num != 2 || GetFlagInt(87) >= 5 || GetFlagInt(103) == 0)
			{
				result = true;
				UnityEngine.Debug.Log("Paula has escaped containment.");
			}
		}
		SetFlag(176, 1);
		return result;
	}

	public void SetDefaultValues(bool ignoreDebug = false)
	{
		playerName = "PLAYER";
		exp = 0;
		items = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
		equipItems = new List<int> { -1, -1, -1, -1, -1, -1, -1, -1 };
		boxItems = new List<int>();
		party = new int[6] { 0, 1, -1, -1, -1, -1 };
		deaths = 0;
		gold = 0;
		atBuffs = new int[3];
		dfBuffs = new int[3];
		playTime = 0;
		playTimeFrames = 0;
		flags = new object[1000];
		persFlags = new object[1000];
		sessionFlags = new object[100];
		PartyMembers.SetDefaultValues();
		SetFlag(12, 1);
		SetFlag(322, 2);
		SetFlag(325, 2);
		SetSessionFlag(11, 2);
		menuLocked = false;
		ending = -1;
		if (dev && (Application.platform == RuntimePlatform.WindowsEditor || Application.platform == RuntimePlatform.OSXEditor) && !ignoreDebug)
		{
			SetFlag(204, 1);
			SetFlag(223, 5);
			SetPartyMembers(susie: true, noelle: true);
			HealAll(99);
			SetFlag(84, 11);
			SetFlag(293, 1);
			PartyMembers.SetWeapon(0, 13);
			PartyMembers.SetWeapon(1, 15);
			PartyMembers.SetArmor(0, 19);
			PartyMembers.SetArmor(1, 19);
			PartyMembers.SetArmor(2, 19);
			AddAmbiguousItem(24);
			AddAmbiguousItem(44);
			AddAmbiguousItem(45);
			AddAmbiguousItem(23);
			AddAmbiguousItem(23);
			AddAmbiguousItem(23);
			AddAmbiguousItem(13);
			AddAmbiguousItem(8);
			AddAmbiguousItem(19);
			AddAmbiguousItem(42);
			AddAmbiguousItem(28);
			AddAmbiguousItem(14);
			SetFlag(286, 1);
			SetFlag(116, 2);
			SetFlag(332, 1);
			gold = 150;
		}
	}

	public SAVEFile GetFile()
	{
		SAVEFile sAVEFile = new SAVEFile();
		sAVEFile.UpdateCharacterInfo(playerName, exp, items, equipItems, boxItems, party, PartyMembers.GetAllHP(), PartyMembers.GetAllWeapon(), PartyMembers.GetAllArmor(), playTime, zone, gold, flags);
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
			string text = Mathf.FloorToInt((float)save.playTime / 3600f).ToString();
			string text2 = Mathf.FloorToInt((float)save.playTime % 3600f / 60f).ToString();
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
		if (GetFlagInt(108) == 1 || (GetFlagInt(13) > 1 && GetFlagInt(127) == 1) || GetFlagInt(13) > 2)
		{
			return false;
		}
		return options.easyMode.value > 0;
	}

	public void CopyFile(int from, int to)
	{
		string path = "SAVE" + from + ".sav";
		string path2 = "SAVE" + to + ".sav";
		File.Copy(Path.Combine(Application.persistentDataPath, path), Path.Combine(Application.persistentDataPath, path2), overwrite: true);
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
		return $"{array[0]}.{array[1]}.{array[2]}";
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
		case "SARAH":
		case "RYNO":
		case "VYLET":
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
		case "SHAYY":
			return 10;
		default:
			return 0;
		}
	}

	public void SetFramerate()
	{
		SetFramerate(curFps);
	}

	public void SetFramerate(int fps)
	{
		curFps = fps;
		if (options.vSync.value == 1 && refreshRate % 60 == 0 && refreshRate > 0 && refreshRate <= 120)
		{
			QualitySettings.vSyncCount = Mathf.FloorToInt(refreshRate / fps);
		}
		else
		{
			QualitySettings.vSyncCount = 0;
		}
		Application.targetFrameRate = fps;
	}

	public int GetFramerate()
	{
		return curFps;
	}

	public List<DisplayInfo> GetDisplayInfo()
	{
		return displayInfo;
	}

	public void LoadConfigData()
	{
		UpdateVolume(config.GetInt("General", "Volume", 60, writeIfNotExist: true));
		options.LoadFromConfig(ref config);
		if (PersistentSAVE.HasKey("window-scale"))
		{
			SetWindowScale(PersistentSAVE.GetInt("window-scale", 0));
			PersistentSAVE.RemoveKey("window-scale");
		}
		if (PersistentSAVE.HasKey("fullscreen"))
		{
			SetFullscreen(PersistentSAVE.GetInt("fullscreen", 0) == 1);
			PersistentSAVE.RemoveKey("fullscreen");
		}
		SetMonitorInfoEnabled(config.GetInt("Debug", "MonitorInfo", 0, writeIfNotExist: true) == 1);
		config.Write();
	}

	public void SetFullscreen(bool fullscreen)
	{
		config.SetInt("Window", "Fullscreen", fullscreen ? 1 : 0);
		config.Write();
	}

	public bool GetFullscreen()
	{
		return config.GetInt("Window", "Fullscreen", 0) == 1;
	}

	public void SetWindowScale(int windowScale)
	{
		config.SetInt("Window", "WindowScale", windowScale);
		config.Write();
	}

	public int GetWindowScale()
	{
		return config.GetInt("Window", "WindowScale", 1);
	}

	public void UpdateWindow()
	{
		Resolution currentResolution = Screen.currentResolution;
		int num = GetWindowScale();
		if (num < 1 || num * 640 > currentResolution.width || num * 480 > currentResolution.height)
		{
			num = 1;
			SetWindowScale(num);
		}
		if (GetFullscreen())
		{
			Screen.SetResolution(currentResolution.width, currentResolution.height, FullScreenMode.FullScreenWindow);
		}
		else
		{
			Screen.SetResolution(640 * num, 480 * num, fullscreen: false);
		}
	}

	public int GetRefreshRate()
	{
		return refreshRate;
	}

	public void SetMonitorInfoEnabled(bool enabled)
	{
		monitorInfoEnabled = enabled;
	}
}
