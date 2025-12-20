using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OverworldMenu : MonoBehaviour
{
	protected enum State
	{
		MainMenu = 0,
		ItemCategory = 1,
		ItemList = 2,
		ItemAction = 3,
		Stats = 4,
		StatsMagic = 5,
		Cell = 6,
		Debug = 7,
		TargetMenu = 8,
		ReadingTextBox = 9
	}

	private readonly int PANEL_IDLE_TIME = 15;

	protected GameManager gm;

	protected bool isAlone;

	protected TextBox txt;

	protected bool ts;

	[SerializeField]
	protected bool frosted;

	protected Transform soul;

	protected State state;

	protected ActionPartyPanels panels;

	protected int idleFrames = 14;

	protected bool returnPlayerControl = true;

	protected GameObject gameObjectToSpawn;

	protected bool useUpTextbox;

	protected Transform mainMenu;

	protected Transform itemMenu;

	protected Transform statsMenu;

	protected Transform cellMenu;

	protected Transform magicMenu;

	protected Transform targetMenu;

	protected Transform debugMenu;

	protected Transform storage;

	protected Transform itemStats;

	protected Transform itemStatsDiff;

	protected bool debugEnabled;

	protected int itemCategory;

	protected int[] itemCount = new int[3];

	protected int totalItemCount;

	protected int[] keyItems = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };

	protected int itemIndex;

	protected List<int> partySlots = new List<int>();

	protected int index;

	protected int menuLimit;

	protected bool holdAxis;

	protected bool vertAxis = true;

	protected bool canMove = true;

	protected int[] callerIDs = new int[6] { -1, -1, -1, -1, -1, -1 };

	protected int callers;

	protected bool hasCell = true;

	protected int partySlot;

	protected Magic.ID[] spellIDs;

	protected string targetType = "";

	protected bool targetMenuInitialized;

	protected bool hasMiniRow;

	protected bool onMiniRow;

	protected int mainRowSize;

	protected int miniRowSize;

	protected bool t_holdVertAxis;

	protected AudioSource cursor;

	protected AudioSource sel;

	protected AudioSource error;

	private Sprite[] prevIcons = new Sprite[8];

	private int flavor;

	private void Awake()
	{
		gm = Util.GameManager();
		debugEnabled = Util.GameManager().IsTestMode();
		if (UTInput.GetAxis("Vertical") != 0f)
		{
			holdAxis = true;
		}
		ts = Util.GameManager().GetFlagInt(94) == 1;
		cursor = GetComponents<AudioSource>()[0];
		sel = GetComponents<AudioSource>()[1];
		error = GetComponents<AudioSource>()[2];
		soul = base.transform.Find("SOUL");
		soul.GetComponent<Image>().color = SOUL.GetSOULColorByID(gm.GetFlagInt(312));
		flavor = gm.GetFlagInt(223);
		CreateEveryMenu();
	}

	private void Update()
	{
		if (canMove)
		{
			int num = (vertAxis ? (-(int)UTInput.GetAxis("Vertical")) : ((int)UTInput.GetAxis("Horizontal")));
			if (!holdAxis && num != 0)
			{
				if (state == State.ItemCategory)
				{
					int num2 = itemCategory;
					do
					{
						itemCategory += num;
						if (itemCategory >= 3)
						{
							itemCategory = 0;
						}
						else if (itemCategory < 0)
						{
							itemCategory = 2;
						}
						if (frosted)
						{
							storage.Find("0").GetComponent<Text>().text = itemCount[itemCategory] + "/8";
						}
					}
					while (itemCount[itemCategory] <= 0 && itemCategory != num2);
					if (itemCategory == num2)
					{
						error.Play();
					}
					else
					{
						index = itemCategory;
						cursor.Play();
						GenerateItemList();
					}
				}
				else
				{
					Transform transform = null;
					if (frosted)
					{
						if (state == State.MainMenu)
						{
							transform = mainMenu.Find("MenuIcons");
						}
						else if (state == State.ItemList)
						{
							transform = itemMenu.Find("ItemIcons");
						}
					}
					if ((bool)transform)
					{
						transform.GetChild(index).GetComponent<Image>().enabled = true;
					}
					index += num;
					if (index >= menuLimit)
					{
						index = 0;
					}
					else if (index < 0)
					{
						index = menuLimit - 1;
					}
					if ((bool)transform)
					{
						transform.GetChild(index).GetComponent<Image>().enabled = false;
					}
					if (frosted && (state == State.ItemList || state == State.TargetMenu))
					{
						UpdateFrostedItemStats();
					}
					if (menuLimit > 1)
					{
						cursor.Play();
						if (state == State.Stats)
						{
							GeneratePartyMemberStats(partySlots[index]);
						}
						else if (state == State.StatsMagic)
						{
							GenerateMagicTextBox();
						}
					}
				}
				holdAxis = true;
			}
			else if (holdAxis && num == 0)
			{
				holdAxis = false;
			}
		}
		if (state == State.MainMenu)
		{
			idleFrames++;
			if (idleFrames == PANEL_IDLE_TIME && !panels && gm.NumActivePartyMembers(includeMinis: true) > 1)
			{
				CreatePartyPanels();
			}
			if (UTInput.GetButtonDown("Z"))
			{
				idleFrames = 0;
				if (index == 0)
				{
					if (totalItemCount == 0)
					{
						error.Play();
					}
					else
					{
						sel.Play();
						EnterItemMenu();
					}
				}
				else if (index == 1)
				{
					sel.Play();
					EnterStatsMenu();
				}
				else if (index == 2)
				{
					if (hasCell)
					{
						sel.Play();
						EnterCellMenu();
					}
					else
					{
						error.Play();
					}
				}
				else if (index == 3)
				{
					sel.Play();
					debugMenu.gameObject.SetActive(value: true);
					index = 0;
					menuLimit = 7;
					state = State.Debug;
				}
			}
			else if (UTInput.GetButtonDown("X") || UTInput.GetButtonDown("C"))
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else if (state == State.ItemCategory)
		{
			if (frosted && UTInput.GetButtonDown("C"))
			{
				gm.PlayGlobalSFX("sounds/snd_pombark");
			}
			if (UTInput.GetButtonDown("Z") || UTInput.GetAxis("Vertical") < 0f)
			{
				if (UTInput.GetButtonDown("Z"))
				{
					sel.Play();
				}
				else
				{
					cursor.Play();
				}
				if (frosted)
				{
					itemMenu.Find("Category").GetChild(index).GetComponent<Text>()
						.color = Selection.SELECTION_COLORS[flavor];
				}
				vertAxis = true;
				state = State.ItemList;
				index = 0;
				menuLimit = itemCount[itemCategory];
				if (frosted)
				{
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.enabled = false;
					UpdateFrostedItemStats();
				}
				if (UTInput.GetAxis("Vertical") < 0f)
				{
					holdAxis = true;
				}
			}
			else if (UTInput.GetButtonDown("X"))
			{
				if (frosted)
				{
					itemStats.gameObject.SetActive(value: false);
					itemStatsDiff.gameObject.SetActive(value: false);
				}
				itemMenu.gameObject.SetActive(value: false);
				EnterMainMenu();
			}
		}
		else if (state == State.ItemList)
		{
			if (frosted && UTInput.GetButtonDown("C"))
			{
				gm.PlayGlobalSFX("sounds/snd_pombark");
			}
			if (UTInput.GetButtonDown("Z"))
			{
				sel.Play();
				if (frosted)
				{
					prevIcons[index] = itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.sprite;
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_yes");
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.color = Selection.SELECTION_COLORS[flavor];
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.enabled = true;
					itemMenu.Find("List").GetChild(index).GetComponent<Text>()
						.color = Selection.SELECTION_COLORS[flavor];
				}
				if (ts || frosted)
				{
					for (int i = 0; i < 3; i++)
					{
						itemMenu.Find("Action").GetChild(i).GetComponent<Text>()
							.color = Color.white;
					}
				}
				itemIndex = index;
				state = State.ItemAction;
				vertAxis = false;
				index = 0;
				menuLimit = 3;
			}
			else if (UTInput.GetButtonDown("X"))
			{
				if (frosted)
				{
					itemStats.gameObject.SetActive(value: false);
					itemStatsDiff.gameObject.SetActive(value: false);
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.enabled = true;
					itemMenu.Find("Category").GetChild(itemCategory).GetComponent<Text>()
						.color = Color.white;
				}
				vertAxis = false;
				state = State.ItemCategory;
				index = itemCategory;
				menuLimit = 3;
			}
		}
		else if (state == State.ItemAction)
		{
			if (frosted && UTInput.GetButtonDown("C"))
			{
				gm.PlayGlobalSFX("sounds/snd_pombark");
			}
			if (UTInput.GetButtonDown("Z"))
			{
				if (gm.GetSessionFlagInt(6) == 1)
				{
					txt = EnterTextBox();
					txt.CreateBox(new string[1] { "* (You can't muster the\n  motivation to look through\n  your pockets.)" });
				}
				else if (index == 0)
				{
					if (ShouldSkipTargetMenu())
					{
						if (itemCategory == 2)
						{
							UseKeyItem();
						}
						else
						{
							UseItem();
						}
					}
					else
					{
						sel.Play();
						string itemName = GetItemName(GetItem(itemIndex));
						EnterTargetMenu((itemCategory == 1) ? ("Equip " + itemName + " on") : ("Use " + itemName + " on"));
						if (itemCategory == 0)
						{
							CreatePartyPanels();
						}
					}
				}
				else if (index == 1)
				{
					ShowItemInfo();
				}
				else if (index == 2)
				{
					DropItem();
				}
			}
			else if (UTInput.GetButtonDown("X"))
			{
				vertAxis = true;
				state = State.ItemList;
				index = itemIndex;
				menuLimit = itemCount[itemCategory];
				if (frosted)
				{
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.sprite = prevIcons[index];
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.enabled = false;
					itemMenu.Find("List").GetChild(index).GetComponent<Text>()
						.color = Color.white;
					itemMenu.Find("ItemIcons").GetChild(index).GetComponent<Image>()
						.color = Color.white;
				}
				if (ts || frosted)
				{
					for (int j = 0; j < 3; j++)
					{
						itemMenu.Find("Action").GetChild(j).GetComponent<Text>()
							.color = new Color(0.5f, 0.5f, 0.5f, 1f);
					}
				}
			}
		}
		else if (state == State.Stats)
		{
			if (UTInput.GetButtonDown("Z"))
			{
				sel.Play();
				partySlot = partySlots[index];
				EnterMagicMenu();
			}
			else if (UTInput.GetButtonDown("X"))
			{
				statsMenu.gameObject.SetActive(value: false);
				EnterMainMenu();
			}
		}
		else if (state == State.StatsMagic)
		{
			if (UTInput.GetButtonDown("Z"))
			{
				error.Play();
			}
			else if (UTInput.GetButtonDown("X"))
			{
				magicMenu.gameObject.SetActive(value: false);
				EnterStatsMenu();
				index = partySlots.IndexOf(partySlot);
				GeneratePartyMemberStats(partySlot);
			}
		}
		else if (state == State.Cell)
		{
			if (UTInput.GetButtonDown("Z"))
			{
				if (gm.GetSessionFlagInt(6) == 1)
				{
					txt = EnterTextBox();
					txt.CreateBox(new string[1] { "* (You couldn't bring yourself\n  to dial.)" });
				}
				else if (MapInfo.GetCurrentWorld() == World.LOSTCORE)
				{
					txt = EnterTextBox();
					txt.CreateBox(new string[1] { "* (The phone won't turn on.)" });
				}
				else
				{
					txt = EnterTextBox();
					if (frosted)
					{
						txt.SetFrostedOffset(2);
					}
					Calls.CallCharacter((Calls.ID)callerIDs[index], txt, (!useUpTextbox) ? 1 : 0);
					if (frosted)
					{
						txt.GetUIBox().GetComponent<FrostedBox>().SetContactName((callerIDs[index] == 0) ? "Mom" : Calls.GetCallerName((Calls.ID)callerIDs[index]));
					}
				}
			}
			else if (UTInput.GetButtonDown("X"))
			{
				cellMenu.gameObject.SetActive(value: false);
				EnterMainMenu();
			}
		}
		else if (state == State.TargetMenu)
		{
			if (hasMiniRow)
			{
				if (UTInput.GetAxis("Vertical") != 0f && !t_holdVertAxis)
				{
					SwitchToRow(!onMiniRow);
					t_holdVertAxis = true;
					cursor.Play();
				}
				else if (UTInput.GetAxis("Vertical") == 0f && t_holdVertAxis)
				{
					t_holdVertAxis = false;
				}
			}
			if (UTInput.GetButtonDown("Z"))
			{
				partySlot = partySlots[index];
				if (onMiniRow)
				{
					partySlot += 3;
				}
				UseItem();
			}
			else if (UTInput.GetButtonDown("X"))
			{
				targetMenu.gameObject.SetActive(value: false);
				state = State.ItemAction;
				vertAxis = false;
				index = 0;
				menuLimit = 3;
				if (frosted)
				{
					UpdateFrostedItemStats();
				}
			}
		}
		else if (state == State.Debug)
		{
			if (UTInput.GetButtonDown("Z"))
			{
				DebugTools.UseTool(DebugTools.GetKeys()[index]);
				UnityEngine.Object.Destroy(base.gameObject);
			}
			else if (UTInput.GetButtonDown("X"))
			{
				debugMenu.gameObject.SetActive(value: false);
				EnterMainMenu();
			}
		}
		PositionSOUL();
	}

	private void LateUpdate()
	{
		if (state != State.TargetMenu && (bool)panels && idleFrames < PANEL_IDLE_TIME)
		{
			UnityEngine.Object.Destroy(panels.gameObject);
		}
	}

	private void EnterMainMenu()
	{
		if (state == State.ItemCategory)
		{
			index = 0;
		}
		else if (state == State.Stats)
		{
			index = 1;
		}
		else if (state == State.Cell)
		{
			index = 2;
		}
		else if (state == State.Debug)
		{
			index = 3;
		}
		if (frosted)
		{
			string text = (new string[4] { "item", "stat", "cell", "" })[index];
			if (text != "")
			{
				mainMenu.Find("MenuIcons").GetChild(index).GetComponent<Image>()
					.enabled = false;
				mainMenu.Find("MenuIcons").GetChild(index).GetComponent<Image>()
					.sprite = Resources.Load<Sprite>("ui/frostedicons/spr_" + text);
				mainMenu.Find("MenuIcons").GetChild(index).GetComponent<Image>()
					.color = Color.white;
				mainMenu.GetChild(index + 1).GetComponent<Text>().color = Color.white;
			}
			storage.gameObject.SetActive(value: false);
			itemStats.gameObject.SetActive(value: false);
			itemStatsDiff.gameObject.SetActive(value: false);
		}
		state = State.MainMenu;
		vertAxis = true;
		menuLimit = 3;
		if (debugEnabled)
		{
			menuLimit = 4;
		}
		else if (!hasCell)
		{
			menuLimit = 2;
		}
	}

	private void EnterItemMenu()
	{
		state = State.ItemCategory;
		vertAxis = false;
		GenerateItemList();
		itemCategory = 0;
		if (itemCount[0] == 0)
		{
			itemCategory = 1;
			if (itemCount[1] == 0)
			{
				itemCategory = 2;
			}
		}
		GenerateItemList();
		if (frosted)
		{
			mainMenu.Find("MenuIcons").Find("Item").GetComponent<Image>()
				.enabled = true;
			mainMenu.Find("MenuIcons").Find("Item").GetComponent<Image>()
				.sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_yes");
			mainMenu.Find("MenuIcons").Find("Item").GetComponent<Image>()
				.color = Selection.SELECTION_COLORS[flavor];
			storage.gameObject.SetActive(value: true);
			storage.Find("0").GetComponent<Text>().text = itemCount[itemCategory] + "/8";
		}
		index = itemCategory;
		menuLimit = 3;
		itemMenu.gameObject.SetActive(value: true);
		if (frosted)
		{
			bool joystickIsActive = UTInput.joystickIsActive;
			string text = (joystickIsActive ? "C" : UTInput.GetKeyName("Menu"));
			itemMenu.Find("Sort").GetComponent<Text>().text = text + " to";
			itemMenu.Find("Sort").Find("Dog").localPosition = new Vector3(29 + 5 * text.Length, 45f);
			Image component = itemMenu.Find("Sort").Find("Menu").GetComponent<Image>();
			if (!joystickIsActive)
			{
				component.enabled = false;
				return;
			}
			component.enabled = true;
			ButtonPrompts.UpdateImageWithGraphic("Menu", component, 2f, ButtonPrompts.ButtonType.Small);
		}
	}

	private void EnterStatsMenu()
	{
		state = State.Stats;
		if (frosted)
		{
			mainMenu.Find("MenuIcons").Find("Stat").GetComponent<Image>()
				.enabled = true;
			mainMenu.Find("MenuIcons").Find("Stat").GetComponent<Image>()
				.sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_yes");
			mainMenu.Find("MenuIcons").Find("Stat").GetComponent<Image>()
				.color = Selection.SELECTION_COLORS[flavor];
			mainMenu.Find("1").GetComponent<Text>().color = Selection.SELECTION_COLORS[flavor];
		}
		vertAxis = false;
		index = 0;
		menuLimit = partySlots.Count;
		GeneratePartyMemberStats(0);
		statsMenu.gameObject.SetActive(value: true);
	}

	private void EnterCellMenu()
	{
		state = State.Cell;
		if (frosted)
		{
			mainMenu.Find("MenuIcons").Find("Cell").GetComponent<Image>()
				.enabled = true;
			mainMenu.Find("MenuIcons").Find("Cell").GetComponent<Image>()
				.sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_yes");
			mainMenu.Find("MenuIcons").Find("Cell").GetComponent<Image>()
				.color = Selection.SELECTION_COLORS[flavor];
			mainMenu.Find("2").GetComponent<Text>().color = Selection.SELECTION_COLORS[flavor];
		}
		vertAxis = true;
		index = 0;
		menuLimit = callers;
		cellMenu.gameObject.SetActive(value: true);
	}

	private void PositionSOUL()
	{
		if (state == State.MainMenu)
		{
			float y = mainMenu.localPosition.y + mainMenu.Find(index.ToString()).localPosition.y;
			soul.localPosition = new Vector3(-255f, y);
		}
		else if (state == State.ItemCategory)
		{
			float x = itemMenu.localPosition.x + itemMenu.Find("Category").Find(index.ToString()).localPosition.x - 15f;
			soul.localPosition = new Vector3(x, 147f);
			if (frosted)
			{
				soul.localPosition += new Vector3(0f, 22f);
			}
		}
		else if (state == State.ItemList)
		{
			float y2 = itemMenu.localPosition.y + itemMenu.Find("List").Find(index.ToString()).localPosition.y;
			soul.localPosition = new Vector3(-103f, y2);
			if (frosted)
			{
				soul.localPosition += new Vector3(-4f, 15f);
			}
		}
		else if (state == State.ItemAction)
		{
			float x2 = itemMenu.localPosition.x + itemMenu.Find("Action").Find(index.ToString()).localPosition.x - 15f;
			soul.localPosition = new Vector3(x2, -161f);
			if (frosted)
			{
				soul.localPosition += new Vector3(0f, 24f);
			}
		}
		else if (state == State.Cell)
		{
			float y3 = cellMenu.localPosition.y + cellMenu.Find(index.ToString()).localPosition.y;
			soul.localPosition = new Vector3(-103f, y3);
			if (frosted)
			{
				soul.localPosition += new Vector3(12f, 0f);
			}
		}
		else if (state == State.StatsMagic)
		{
			float y4 = magicMenu.GetChild(0).localPosition.y + magicMenu.GetChild(0).Find(index.ToString()).localPosition.y;
			soul.localPosition = new Vector3(-103f, y4);
			if (frosted)
			{
				soul.localPosition += new Vector3(-10f, 0f);
			}
		}
		else if (state == State.TargetMenu)
		{
			int num = ((menuLimit == 1) ? 1 : index);
			float x3 = targetMenu.localPosition.x + (onMiniRow ? targetMenu.Find("MiniRow").Find(targetType).Find(num.ToString())
				.localPosition.x : targetMenu.Find(targetType).Find(num.ToString()).localPosition.x) - 24f;
			if (hasMiniRow)
			{
				soul.localPosition = new Vector3(x3, -115 + (onMiniRow ? (-14) : 18));
			}
			else
			{
				soul.localPosition = new Vector3(x3, -115f);
			}
			if (frosted)
			{
				soul.localPosition += new Vector3(0f, -8f);
			}
		}
		else if (state == State.Debug)
		{
			float y5 = debugMenu.localPosition.y + debugMenu.Find(index.ToString()).localPosition.y;
			soul.localPosition = new Vector3(-103f, y5);
		}
		else
		{
			soul.localPosition = new Vector3(1000f, 1000f);
		}
	}

	private void CreateEveryMenu()
	{
		int num = 0;
		useUpTextbox = Util.OverworldPlayer().transform.position[1] - Util.FindObjectOfType<CameraController>().transform.position[1] < -0.9f;
		Image[] componentsInChildren = GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			if (!(image.sprite == null) || !(image.gameObject.name != "Button") || image.gameObject.name.Contains("Frosted"))
			{
				continue;
			}
			UIBackground uIBackground = new GameObject(image.gameObject.name + "GenBG").AddComponent<UIBackground>();
			uIBackground.transform.SetParent(base.transform.parent);
			uIBackground.CreateElement("menu", image.transform.localPosition, image.rectTransform.sizeDelta);
			uIBackground.transform.parent = image.transform;
			uIBackground.transform.SetAsFirstSibling();
			num++;
			if (useUpTextbox)
			{
				float y = image.transform.localPosition.y;
				switch (image.gameObject.name)
				{
				case "PlayerInfo":
					y = (frosted ? (-157) : (-137));
					break;
				case "MagicMenu":
					y = (frosted ? (-78) : (-60));
					break;
				case "Textbox":
					y = (frosted ? 144 : 154);
					break;
				case "MainMenu":
					if (frosted)
					{
						y = -18f;
					}
					break;
				}
				image.transform.localPosition = new Vector3(image.transform.localPosition.x, y);
			}
			switch (image.gameObject.name)
			{
			case "MainMenu":
				mainMenu = image.transform;
				break;
			case "Item":
				itemMenu = image.transform;
				break;
			case "Stats":
				statsMenu = image.transform;
				break;
			case "Cell":
				cellMenu = image.transform;
				break;
			case "MagicMenu":
				magicMenu = image.transform.parent;
				break;
			case "TargetMenu":
				targetMenu = image.transform;
				break;
			case "Debug":
				debugMenu = image.transform;
				break;
			case "F_Storage":
				storage = image.transform;
				break;
			case "F_ItemStats":
				itemStats = image.transform;
				break;
			case "F_ItemStatsDif":
				itemStatsDiff = image.transform;
				break;
			}
		}
		GeneratePlayerInfo();
		GenerateItemList();
		GenerateCallerList();
		if (!hasCell)
		{
			mainMenu.Find("2").GetComponent<Text>().text = "";
		}
		partySlots = new List<int>(gm.GetActivePartySlots(includeMinis: true));
		if (partySlots.Count == 1)
		{
			statsMenu.Find("ScrollBar").gameObject.SetActive(value: false);
		}
		else if (!frosted)
		{
			statsMenu.Find("ScrollBar").Find("SOUL").GetComponent<Image>()
				.color = SOUL.GetSOULColorByID(gm.GetFlagInt(312));
		}
		if (ts)
		{
			for (num = 0; num < 3; num++)
			{
				itemMenu.Find("Action").GetChild(num).GetComponent<Text>()
					.color = new Color(0.5f, 0.5f, 0.5f, 1f);
			}
		}
		itemMenu.gameObject.SetActive(value: false);
		statsMenu.gameObject.SetActive(value: false);
		cellMenu.gameObject.SetActive(value: false);
		magicMenu.gameObject.SetActive(value: false);
		targetMenu.gameObject.SetActive(value: false);
		debugMenu.gameObject.SetActive(value: false);
		if (frosted)
		{
			storage.gameObject.SetActive(value: false);
			itemStats.gameObject.SetActive(value: false);
			itemStatsDiff.gameObject.SetActive(value: false);
		}
		EnterMainMenu();
		PositionSOUL();
	}

	private void GeneratePlayerInfo()
	{
		base.transform.Find("PlayerInfo").Find("Name").GetComponent<Text>()
			.text = PartyMembers.GetMemberName(gm.GetPartyMember(0));
		string format = (frosted ? "lv  {0}\nhp  {1}/{2}\ng" : "lv  {0}\nhp  {1}/{2}\ng   {3}");
		base.transform.Find("PlayerInfo").Find("Stats").GetComponent<Text>()
			.text = string.Format(format, gm.GetLV(), gm.GetHP(0), gm.GetMaxHP(0), gm.GetGold());
		if (frosted)
		{
			base.transform.Find("PlayerInfo").Find("StatsG").GetComponent<Text>()
				.text = gm.GetGold().ToString();
		}
		base.transform.Find("PlayerInfo").Find("LOVE").GetComponent<Text>()
			.text = "";
	}

	private void GenerateItemList()
	{
		itemCount[0] = 0;
		itemCount[1] = 0;
		itemCount[2] = 0;
		keyItems = new int[8] { -1, -1, -1, -1, -1, -1, -1, -1 };
		List<KeyItems.ID> listOfKeyItems = KeyItems.GetListOfKeyItems();
		for (int i = 0; i < listOfKeyItems.Count; i++)
		{
			if (i >= 8)
			{
				Debug.LogError($"KeyItems: Attempted to add {KeyItems.GetName(listOfKeyItems[i])} after maxing out key item space");
			}
			else
			{
				keyItems[i] = (int)listOfKeyItems[i];
			}
		}
		for (int j = 0; j < 8; j++)
		{
			if (gm.GetItem(j) > -1)
			{
				itemCount[0]++;
			}
			if (gm.GetEquipment(j) > -1)
			{
				itemCount[1]++;
			}
			if (keyItems[j] > -1)
			{
				itemCount[2]++;
			}
		}
		for (int k = 0; k < 8; k++)
		{
			SetItem(k);
		}
		totalItemCount = itemCount[0] + itemCount[1] + itemCount[2];
		Color color = Color.white;
		if (frosted && (state == State.ItemCategory || state == State.ItemList))
		{
			color = Selection.SELECTION_COLORS[flavor];
		}
		mainMenu.Find("0").GetComponent<Text>().color = ((totalItemCount > 0) ? color : new Color(0.5f, 0.5f, 0.5f, 1f));
		for (int l = 0; l < 3; l++)
		{
			itemMenu.Find("Category").GetChild(l).GetComponent<Text>()
				.color = ((itemCount[l] > 0) ? Color.white : new Color(0.5f, 0.5f, 0.5f, 1f));
		}
	}

	private void GenerateCallerList()
	{
		Tuple<int[], int> callerList = Calls.GetCallerList();
		callerIDs = callerList.Item1;
		callers = callerList.Item2;
		hasCell = callers > 0;
		if (frosted)
		{
			string[] array = new string[2] { "ui/frostedicons/spr_contact_toriel_dr", "ui/frostedicons/spr_contact_toriel" };
			for (int i = 0; i < 3; i++)
			{
				if (callerIDs[i] != -1)
				{
					cellMenu.Find(i.ToString()).GetComponent<Image>().enabled = true;
					cellMenu.Find(i.ToString()).GetComponent<Image>().sprite = Resources.Load<Sprite>(array[callerIDs[i]]);
					cellMenu.Find(i.ToString()).GetComponentInChildren<Text>().text = Calls.GetCallerName((Calls.ID)callerIDs[i]);
				}
				else
				{
					cellMenu.Find(i.ToString()).GetComponent<Image>().enabled = false;
					cellMenu.Find(i.ToString()).GetComponentInChildren<Text>().text = "";
				}
			}
			return;
		}
		for (int j = 0; j < 6; j++)
		{
			if (callerIDs[j] != -1)
			{
				cellMenu.Find(j.ToString()).GetComponent<Text>().text = Calls.GetCallerName((Calls.ID)callerIDs[j]);
			}
			else
			{
				cellMenu.Find(j.ToString()).GetComponent<Text>().text = (ts ? "<color=#404040>------------------</color>" : "");
			}
		}
	}

	private void EnterMagicMenu()
	{
		partySlot = partySlots[index];
		spellIDs = Magic.GetSpellList(gm.GetPartyMember(partySlot));
		vertAxis = true;
		state = State.StatsMagic;
		index = 0;
		menuLimit = spellIDs.Length;
		statsMenu.gameObject.SetActive(value: false);
		magicMenu.gameObject.SetActive(value: true);
		GenerateMagicMenu();
	}

	private void EnterTargetMenu(string message)
	{
		state = State.TargetMenu;
		vertAxis = false;
		index = 0;
		targetMenu.gameObject.SetActive(value: true);
		targetMenu.Find("Header").GetComponent<Text>().text = message;
		if (!targetMenuInitialized)
		{
			targetMenuInitialized = true;
			hasMiniRow = gm.PartySlotFilled(3) || gm.PartySlotFilled(4) || gm.PartySlotFilled(5);
			if (hasMiniRow)
			{
				targetMenu.Find("Header").localPosition += new Vector3(0f, 12f);
				targetMenu.Find("Menu2").localPosition += new Vector3(0f, 18f);
				targetMenu.Find("Menu3").localPosition += new Vector3(0f, 18f);
			}
			else
			{
				targetMenu.Find("MiniRow").gameObject.SetActive(value: false);
			}
			targetMenu.Find("Menu2").gameObject.SetActive(value: false);
			targetMenu.Find("Menu3").gameObject.SetActive(value: false);
			targetMenu.Find("MiniRow").Find("Menu2").gameObject.SetActive(value: false);
			targetMenu.Find("MiniRow").Find("Menu3").gameObject.SetActive(value: false);
			mainRowSize = 0;
			miniRowSize = 0;
			for (int i = 0; i < 6; i++)
			{
				if (gm.PartySlotFilled(i))
				{
					if (i < 3)
					{
						mainRowSize++;
					}
					else
					{
						miniRowSize++;
					}
				}
			}
			string n = "Menu" + ((mainRowSize % 2 == 0) ? 2 : 3);
			targetMenu.Find(n).gameObject.SetActive(value: true);
			if (mainRowSize == 1)
			{
				targetMenu.Find(n).GetChild(0).GetComponent<Text>()
					.text = "";
				targetMenu.Find(n).GetChild(1).GetComponent<Text>()
					.text = PartyMembers.GetMemberName(gm.GetPartyMember(0));
				targetMenu.Find(n).GetChild(2).GetComponent<Text>()
					.text = "";
			}
			else
			{
				for (int j = 0; j < mainRowSize; j++)
				{
					targetMenu.Find(n).GetChild(j).GetComponent<Text>()
						.text = PartyMembers.GetMemberName(gm.GetPartyMember(j));
				}
			}
			string n2 = "Menu" + ((miniRowSize % 2 == 0) ? 2 : 3);
			targetMenu.Find("MiniRow").Find(n2).gameObject.SetActive(value: true);
			if (miniRowSize == 1)
			{
				targetMenu.Find("MiniRow").Find(n2).GetChild(0)
					.GetComponent<Text>()
					.text = "";
				targetMenu.Find("MiniRow").Find(n2).GetChild(1)
					.GetComponent<Text>()
					.text = PartyMembers.GetMemberName(gm.GetPartyMember(3));
				targetMenu.Find("MiniRow").Find(n2).GetChild(2)
					.GetComponent<Text>()
					.text = "";
			}
			else
			{
				for (int k = 0; k < miniRowSize; k++)
				{
					targetMenu.Find("MiniRow").Find(n2).GetChild(k)
						.GetComponent<Text>()
						.text = PartyMembers.GetMemberName(gm.GetPartyMember(k + 3));
				}
			}
		}
		if (frosted)
		{
			UpdateFrostedItemStats();
		}
		SwitchToRow(toMiniRow: false);
	}

	private void SwitchToRow(bool toMiniRow)
	{
		if (toMiniRow != onMiniRow)
		{
			int num = (toMiniRow ? miniRowSize : mainRowSize);
			if (num == 3 && menuLimit == 1)
			{
				index = 1;
			}
			else if (index >= num)
			{
				index = num - 1;
			}
		}
		onMiniRow = toMiniRow;
		menuLimit = (onMiniRow ? miniRowSize : mainRowSize);
		targetType = "Menu" + ((menuLimit % 2 == 0) ? 2 : 3);
	}

	private TextBox EnterTextBox()
	{
		canMove = false;
		state = State.ReadingTextBox;
		itemMenu.gameObject.SetActive(value: false);
		cellMenu.gameObject.SetActive(value: false);
		magicMenu.gameObject.SetActive(value: false);
		targetMenu.gameObject.SetActive(value: false);
		if (frosted)
		{
			storage.gameObject.SetActive(value: false);
			itemStats.gameObject.SetActive(value: false);
			itemStatsDiff.gameObject.SetActive(value: false);
		}
		return base.gameObject.AddComponent<TextBox>();
	}

	private void UseItem()
	{
		if (itemCategory == 0)
		{
			if (gm.GetItem(itemIndex) == 24)
			{
				gameObjectToSpawn = Resources.Load<GameObject>("ui/Postcard");
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
			if (gm.GetItem(itemIndex) == 45)
			{
				gameObjectToSpawn = Resources.Load<GameObject>("ui/WildCardOverworld");
				returnPlayerControl = false;
				UnityEngine.Object.Destroy(base.gameObject);
				return;
			}
		}
		txt = EnterTextBox();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<int> list3 = new List<int>();
		List<string> list4 = new List<string>();
		int num = ((itemCategory == 0) ? gm.GetItem(itemIndex) : gm.GetEquipment(itemIndex));
		string[] array = Items.ItemUse(num, partySlot, partySlot, serious: false).Split('}');
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('`');
			if (array2.Length > 1)
			{
				list4.Add(array2[0]);
				if (array2[^2].StartsWith("snd"))
				{
					list2.Add(array2[^2]);
				}
				else
				{
					list2.Add("snd_text");
				}
			}
			else
			{
				list4.Add("");
				list2.Add("snd_text");
			}
			list.Add(array2[^1]);
			list3.Add(0);
		}
		gm.UseItem(partySlot, itemIndex, itemCategory == 1);
		txt.CreateBox(list.ToArray(), list2.ToArray(), list3.ToArray(), (!useUpTextbox) ? 1 : 0, giveBackControl: false, list4.ToArray());
		txt.AddRemarks(Items.GetUseRemarks(num, gm.GetPartyMember(partySlot)));
	}

	private void UseKeyItem()
	{
		int num = keyItems[itemIndex];
		txt = EnterTextBox();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<int> list3 = new List<int>();
		List<string> list4 = new List<string>();
		string[] array = KeyItems.UseItem((KeyItems.ID)num, txt);
		if (num == 4 && (bool)Util.FindObjectOfType<CaveSeal>() && txt == Util.FindObjectOfType<CaveSeal>().GetTextBox())
		{
			returnPlayerControl = false;
		}
		for (int i = 0; i < array.Length; i++)
		{
			string[] array2 = array[i].Split('`');
			if (array2.Length > 1)
			{
				list4.Add(array2[0]);
				if (array2[^2].StartsWith("snd"))
				{
					list2.Add(array2[^2]);
				}
				else
				{
					list2.Add("snd_text");
				}
			}
			else
			{
				list4.Add("");
				list2.Add("snd_text");
			}
			list.Add(array2[^1]);
			list3.Add(0);
		}
		txt.CreateBox(list.ToArray(), list2.ToArray(), list3.ToArray(), (!useUpTextbox) ? 1 : 0, giveBackControl: false, list4.ToArray());
	}

	private void ShowItemInfo()
	{
		txt = EnterTextBox();
		List<string> list = new List<string>();
		new List<string>();
		new List<int>();
		new List<string>();
		string[] array = new string[1] { "* What are you looking at?" };
		int num = ((itemCategory == 0) ? gm.GetItem(itemIndex) : gm.GetEquipment(itemIndex));
		if (itemCategory == 2)
		{
			num = keyItems[itemIndex];
			array = KeyItems.GetDescription((KeyItems.ID)keyItems[itemIndex]).Split('}');
		}
		else
		{
			array = Items.ItemDescription(num).Split('}');
		}
		string[] array2 = array;
		foreach (string item in array2)
		{
			list.Add(item);
		}
		txt.CreateBox(list.ToArray(), "snd_text", 0, (!useUpTextbox) ? 1 : 0, giveBackControl: false);
		if (frosted)
		{
			txt.GetUIBox().GetComponent<FrostedBox>().ActivateItemIcon(num, itemCategory);
			txt.GetUIBox().GetComponent<FrostedBox>().SetName("INFO", force: true);
		}
	}

	private void DropItem()
	{
		txt = EnterTextBox();
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<int> list3 = new List<int>();
		List<string> list4 = new List<string>();
		if (itemCategory == 2)
		{
			string[] array = KeyItems.GetDropText((KeyItems.ID)keyItems[itemIndex]).Split('}');
			for (int i = 0; i < array.Length; i++)
			{
				string[] array2 = array[i].Split('`');
				if (array2.Length > 1)
				{
					list4.Add(array2[0]);
					if (array2[^2].StartsWith("snd"))
					{
						list2.Add(array2[^2]);
					}
					else
					{
						list2.Add("snd_text");
					}
				}
				else
				{
					list4.Add("");
					list2.Add("snd_text");
				}
				list.Add(array2[^1]);
				list3.Add(0);
			}
		}
		else
		{
			int i2 = ((itemCategory == 0) ? gm.GetItem(itemIndex) : gm.GetEquipment(itemIndex));
			list.Add(Items.ItemDrop(i2));
			if (itemCategory == 1)
			{
				gm.RemoveEquipment(itemIndex);
			}
			else
			{
				gm.RemoveItem(itemIndex);
			}
			list4.Add("");
			list2.Add("snd_text");
			list3.Add(0);
		}
		txt.CreateBox(list.ToArray(), list2.ToArray(), list3.ToArray(), (!useUpTextbox) ? 1 : 0, giveBackControl: false, list4.ToArray());
	}

	private void GeneratePartyMemberStats(int slot)
	{
		MonoBehaviour.print(slot);
		int partyMember = gm.GetPartyMember(slot);
		MonoBehaviour.print(partyMember);
		string arg = Items.ItemName(PartyMembers.GetWeapon(partyMember));
		string arg2 = Items.ItemName(PartyMembers.GetArmor(partyMember));
		string text = "GOLD: " + gm.GetGold();
		switch (partyMember)
		{
		case 3:
			text = "MONEY: 5";
			break;
		case 5:
			text = "GOLD: 1";
			break;
		}
		int num = gm.GetLV();
		switch (partyMember)
		{
		case 3:
			num = 3;
			break;
		case 4:
			num = 1;
			break;
		case 5:
			num = 1;
			break;
		default:
			if (slot > 0 && partyMember == 6)
			{
				num = 1;
			}
			break;
		}
		int hP = PartyMembers.GetHP(partyMember);
		int maxHP = PartyMembers.GetMaxHP(partyMember);
		string text2 = PartyMembers.GetATKRaw(partyMember).ToString();
		string text3 = PartyMembers.GetDEFRaw(partyMember).ToString();
		string text4 = Mathf.FloorToInt(PartyMembers.GetMagicRaw(partyMember)).ToString();
		if (partyMember == 0 && gm.GetFlagInt(102) == 1)
		{
			text2 = "<color=#FF8080FF>" + text2 + "</color>";
			text3 = "<color=#FF8080FF>" + text3 + "</color>";
		}
		int num2 = PartyMembers.GetATK(partyMember) - PartyMembers.GetATKRaw(partyMember);
		int num3 = PartyMembers.GetDEF(partyMember) - PartyMembers.GetDEFRaw(partyMember);
		int magicEquipment = PartyMembers.GetMagicEquipment(partyMember);
		string memberName = PartyMembers.GetMemberName(partyMember);
		statsMenu.Find("Name").GetComponent<Text>().text = $"\"{memberName}\"";
		if (frosted)
		{
			float num4 = (float)(memberName.Length * 8) + 16f;
			statsMenu.Find("ScrollBar").Find("Frosted_ArrowLeft").localPosition = new Vector3(0f - num4, 1f);
			statsMenu.Find("ScrollBar").Find("Frosted_ArrowRight").localPosition = new Vector3(num4, 1f);
		}
		statsMenu.Find("CharStats").GetComponent<Text>().text = string.Format(frosted ? "LV  {0}\nHP" : "LV  {0}\nHP  {1} / {2}\n\nAT  {3} ({4})\nDF  {5} ({6})\nMG  {7} ({8})", num, hP, maxHP, text2, num2, text3, num3, text4, magicEquipment);
		statsMenu.Find("EquipStats").GetComponent<Text>().text = string.Format(frosted ? "{0}\n{1}" : "WEAPON: {0}\nARMOR: {1}\n{2}", arg, arg2, text);
		if (frosted)
		{
			float num5 = (float)hP / (float)maxHP;
			statsMenu.Find("Frosted_HPFG").GetComponent<Image>().rectTransform.sizeDelta = new Vector2(100f * num5, 18f);
			statsMenu.Find("Frosted_HP").GetComponent<Text>().text = $"{hP}/{maxHP}";
		}
		string text5 = "EXP: {0}\nNEXT: {1}";
		switch (partyMember)
		{
		case 3:
			text5 = "EXP: N/A\nNEXT: N/A";
			break;
		case 4:
			text5 = "EXP: 0\nNEXT: 10";
			break;
		case 5:
			text5 = "EXP: 0\nNEXT: 10";
			break;
		default:
			if (slot > 0 && partyMember == 6)
			{
				text5 = "EXP: ???\nNEXT: ???";
			}
			break;
		}
		if (frosted)
		{
			text5 = text5 + "\n" + text;
			text5 = text5.Replace(" ", "  ");
			text5 = text5.Replace("EXP:  ", "EXP:   ");
		}
		statsMenu.Find("ExpStats").GetComponent<Text>().text = string.Format(text5, gm.GetEXP(), gm.GetLVExp() - gm.GetEXP());
		if (frosted)
		{
			statsMenu.Find("Frosted_BattleStats").GetComponent<Text>().text = $"ATK:  {text2} <color=#FFFFFF7F>({num2})</color>\nDEF:  {text3} <color=#FFFFFF7F>({num3})</color>\nMAG:  {text4} <color=#FFFFFF7F>({magicEquipment})</color>";
		}
		statsMenu.Find("Portrait").GetComponent<Portrait>().SetImage(PartyMembers.GetMemberStatPortrait(partyMember));
		bool joystickIsActive = UTInput.joystickIsActive;
		if (joystickIsActive)
		{
			statsMenu.Find("Abilities").GetComponent<Text>().text = "press    to view abilities";
		}
		else
		{
			statsMenu.Find("Abilities").GetComponent<Text>().text = string.Format("press [{0}] to view abilities", UTInput.GetKeyName("Confirm"));
		}
		Image component = statsMenu.Find("Button").GetComponent<Image>();
		if (!joystickIsActive)
		{
			component.enabled = false;
		}
		else
		{
			component.enabled = true;
			ButtonPrompts.UpdateImageWithGraphic("Confirm", component, 2f, ButtonPrompts.ButtonType.Small);
		}
		string text6 = "";
		if (partyMember == 0 && gm.GetFlagInt(102) == 1)
		{
			text6 = "<color=#FF8080FF>Concussed</color>";
		}
		else if (partyMember == 0 && gm.GetFlagInt(211) == 1)
		{
			text6 = "<color=#FF8080FF>Deceitful</color>";
		}
		else if (partyMember == 1 && gm.GetFlagInt(257) == 1)
		{
			text6 = "<color=#FF8080FF>Devious</color>";
		}
		statsMenu.Find("Condition").GetComponent<Text>().text = text6;
	}

	private void GenerateMagicMenu()
	{
		int partyMember = gm.GetPartyMember(partySlot);
		for (int i = 0; i < 6; i++)
		{
			if (i >= spellIDs.Length)
			{
				magicMenu.Find("MagicMenu").Find(i.ToString()).GetComponent<Text>()
					.text = "";
				magicMenu.Find("MagicMenu").Find("Cost" + i).GetComponent<Text>()
					.text = "";
				if (frosted)
				{
					magicMenu.Find("MagicMenu").Find("MagicIcons").GetChild(i)
						.GetComponent<Image>()
						.enabled = false;
				}
				continue;
			}
			Magic.Spell spell = Magic.GetSpell((int)spellIDs[i]);
			magicMenu.Find("MagicMenu").Find(i.ToString()).GetComponent<Text>()
				.text = spell.GetName();
			if (spellIDs[i] == Magic.ID.MiniACT)
			{
				switch (partyMember)
				{
				case 1:
					magicMenu.Find("MagicMenu").Find(i.ToString()).GetComponent<Text>()
						.text = "S-Action";
					break;
				case 2:
					magicMenu.Find("MagicMenu").Find(i.ToString()).GetComponent<Text>()
						.text = "N-Action";
					break;
				case 5:
					magicMenu.Find("MagicMenu").Find(i.ToString()).GetComponent<Text>()
						.text = "C-Action";
					break;
				}
			}
			magicMenu.Find("MagicMenu").Find("Cost" + i).GetComponent<Text>()
				.text = spell.GetTPCost() + "% TP";
			if (frosted)
			{
				magicMenu.Find("MagicMenu").Find("MagicIcons").GetChild(i)
					.GetComponent<Image>()
					.enabled = true;
			}
		}
		GenerateMagicTextBox();
	}

	private void GenerateMagicTextBox()
	{
		int partyMember = gm.GetPartyMember(partySlot);
		string text = Magic.GetSpell((int)spellIDs[index]).GetLongDescription();
		if (spellIDs[index] == Magic.ID.MiniACT)
		{
			switch (partyMember)
			{
			case 1:
				text = string.Format(text, "Susie", "her");
				break;
			case 2:
				text = string.Format(text, "Noelle", "her");
				break;
			case 5:
				text = string.Format(text, "Sans", "his");
				break;
			}
		}
		magicMenu.Find("Textbox").Find("Name").GetComponent<Text>()
			.text = text;
	}

	private bool ShouldSkipTargetMenu()
	{
		if (itemCategory == 0)
		{
			if (gm.NumActivePartyMembers(includeMinis: true) > 1)
			{
				if (gm.GetItem(itemIndex) == 16 || gm.GetItem(itemIndex) == 24 || gm.GetItem(itemIndex) == 45 || Items.ItemType(gm.GetItem(itemIndex)) == 4)
				{
					return true;
				}
				return false;
			}
			return true;
		}
		if (itemCategory == 1)
		{
			return gm.NumActivePartyMembers(includeMinis: true) <= 1;
		}
		_ = itemCategory;
		_ = 2;
		return true;
	}

	private void CreatePartyPanels()
	{
		if ((bool)panels)
		{
			UnityEngine.Object.Destroy(panels.gameObject);
		}
		if (frosted)
		{
			panels = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/ActionPartyPanelsHD"), GameObject.Find("Canvas").transform).GetComponent<ActionPartyPanels>();
		}
		else
		{
			panels = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/ActionPartyPanels"), GameObject.Find("Canvas").transform).GetComponent<ActionPartyPanels>();
		}
		panels.UpdateHP(gm.GetHPArray());
		panels.SetActivated(activated: true);
		panels.Raise();
		if (useUpTextbox && state == State.MainMenu)
		{
			panels.UseLowerPosition();
		}
	}

	private void SetItem(int i)
	{
		int item = GetItem(i);
		if (item > -1)
		{
			string itemName = GetItemName(item);
			if (frosted)
			{
				Sprite sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item" + ((itemCategory == 2) ? "_key_" : "_") + item);
				if (!sprite)
				{
					sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_0");
				}
				itemMenu.Find("ItemIcons");
				itemMenu.Find("ItemIcons").GetChild(i).GetComponent<Image>()
					.sprite = sprite;
			}
			itemMenu.Find("List").GetChild(i).GetComponent<Text>()
				.text = itemName;
		}
		else
		{
			if (frosted)
			{
				itemMenu.Find("ItemIcons").GetChild(i).GetComponent<Image>()
					.sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_no");
			}
			itemMenu.Find("List").GetChild(i).GetComponent<Text>()
				.text = ((ts || frosted) ? "<color=#404040>------------------</color>" : "");
		}
	}

	private int GetItem(int i)
	{
		if (itemCategory == 0)
		{
			return gm.GetItem(i);
		}
		if (itemCategory == 1)
		{
			return gm.GetEquipment(i);
		}
		if (itemCategory == 2)
		{
			return keyItems[i];
		}
		return -1;
	}

	private string GetItemName(int i)
	{
		if (itemCategory == 0 || itemCategory == 1)
		{
			return Items.ItemName(i);
		}
		if (itemCategory == 2)
		{
			return KeyItems.GetName((KeyItems.ID)i);
		}
		return "";
	}

	private void UpdateFrostedItemStats()
	{
		itemStats.gameObject.SetActive(value: false);
		itemStatsDiff.gameObject.SetActive(value: false);
		if (itemCategory == 2)
		{
			return;
		}
		bool flag = itemCategory != 0 && state == State.TargetMenu;
		Transform transform = (flag ? itemStatsDiff : itemStats);
		transform.Find("Icon").GetComponent<Image>().enabled = true;
		int item = GetItem((state == State.ItemList) ? index : itemIndex);
		int num = ((state == State.TargetMenu) ? (onMiniRow ? (index + 3) : index) : 0);
		int num2 = Items.ItemValue(item, num);
		string text = "   " + num2.ToString("D2");
		Sprite sprite = null;
		Color color = Color.white;
		if (Items.ItemType(item) == 0 || Items.ItemType(item) == 4)
		{
			if (item == 28 && num != 1)
			{
				text = "   ??";
			}
			else if (num2 > 99 || (item == 39 && num == 2))
			{
				text = "MAX";
				transform.Find("Icon").GetComponent<Image>().enabled = false;
			}
			sprite = Resources.Load<Sprite>("ui/frostedicons/spr_hp_green");
			color = new Color32(0, byte.MaxValue, 0, byte.MaxValue);
		}
		else if (Items.ItemType(item) == 1)
		{
			sprite = Resources.Load<Sprite>("ui/frostedicons/spr_stat_at");
			color = new Color32(byte.MaxValue, 85, 0, byte.MaxValue);
		}
		else if (Items.ItemType(item) == 2)
		{
			sprite = Resources.Load<Sprite>("ui/frostedicons/spr_stat_df");
			color = new Color32(0, 170, byte.MaxValue, byte.MaxValue);
		}
		if (!sprite)
		{
			return;
		}
		transform.gameObject.SetActive(value: true);
		transform.Find("Icon").GetComponent<Image>().sprite = sprite;
		transform.Find("Text").GetComponent<Text>().color = color;
		transform.Find("Text").GetComponent<Text>().text = text;
		if (flag)
		{
			int i = ((Items.ItemType(item) == 2) ? PartyMembers.GetArmor(num) : PartyMembers.GetWeapon(num));
			int num3 = Items.ItemValue(item, num) - Items.ItemValue(i, num);
			Sprite sprite2;
			Color color2;
			if (num3 > 0)
			{
				sprite2 = Resources.Load<Sprite>("ui/frostedicons/spr_stat_diff_up");
				color2 = new Color(0f, 1f, 0f, 1f);
			}
			else if (num3 < 0)
			{
				sprite2 = Resources.Load<Sprite>("ui/frostedicons/spr_stat_diff_down");
				color2 = new Color(1f, 0f, 0f, 1f);
			}
			else
			{
				sprite2 = Resources.Load<Sprite>("ui/frostedicons/spr_stat_diff_equal");
				color2 = new Color(1f, 1f, 0f, 1f);
			}
			transform.Find("DifIcon").GetComponent<Image>().sprite = sprite2;
			transform.Find("DifText").GetComponent<Text>().text = Mathf.Abs(num3).ToString("D2");
			transform.Find("DifText").GetComponent<Text>().color = color2;
		}
	}

	public void CancelControlReturn()
	{
		returnPlayerControl = false;
	}

	public void OnDestroy()
	{
		if ((bool)panels)
		{
			UnityEngine.Object.Destroy(panels.gameObject);
		}
		if (returnPlayerControl)
		{
			gm.EnablePlayerMovement();
			if ((bool)Util.OverworldPlayer())
			{
				Util.OverworldPlayer().SetCollision(onoff: true);
			}
			gm.ClosedMenu();
		}
		if ((bool)gameObjectToSpawn)
		{
			UnityEngine.Object.Instantiate(gameObjectToSpawn, GameObject.Find("Canvas").transform, worldPositionStays: false);
		}
	}
}
