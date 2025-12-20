using UnityEngine;
using UnityEngine.UI;

public class ShopUIBase : MonoBehaviour
{
	[SerializeField]
	protected string textSound = "snd_text";

	[SerializeField]
	protected string fontName = "DTM-Mono";

	[SerializeField]
	protected string greetMessage = "* Hello,^05 traveller.\n* How can I help you?";

	[SerializeField]
	protected string idleMessage = "* Take your time.";

	[SerializeField]
	protected string exitMessage = "* Thank you!\n^05* Come again!";

	[SerializeField]
	protected string buyGreet = "What would \nyou like \nto buy?";

	[SerializeField]
	protected string buySuccess = "Thank you \nso much!";

	[SerializeField]
	protected string buyReject = "Changed your \nmind?";

	[SerializeField]
	protected string buyNoMoney = "You don't \nhave enough \nGOLD.";

	[SerializeField]
	protected string buyNoSpace = "You don't \nhave enough \nspace.";

	[SerializeField]
	protected int item1ID;

	[SerializeField]
	protected int item1price = 10;

	[SerializeField]
	protected string item1ShortDesc = "A cool item.";

	[SerializeField]
	protected int item2ID = 1;

	[SerializeField]
	protected int item2price = 20;

	[SerializeField]
	protected string item2ShortDesc = "A cool knife";

	[SerializeField]
	protected int item3ID = 2;

	[SerializeField]
	protected int item3price = 35;

	[SerializeField]
	protected string item3ShortDesc = "A cool self \ninsert";

	[SerializeField]
	protected int item4ID;

	[SerializeField]
	protected int item4price = 10;

	[SerializeField]
	protected string item4ShortDesc = "Description";

	[SerializeField]
	protected string[] sellPrompt = new string[1] { "* Sorry,^05 we don't buy things\n  here." };

	[SerializeField]
	protected string talkGreet = "What would \nyou like \nto talk \nabout?";

	[SerializeField]
	protected string topic1 = "Talk1";

	[SerializeField]
	protected string[] topic1lines = new string[2] { "* This is Talk1.", "* Woo..." };

	[SerializeField]
	protected string topic2 = "Talk2";

	[SerializeField]
	protected string[] topic2lines = new string[2] { "* This is Talk2.", "* Woo..." };

	[SerializeField]
	protected string topic3 = "Talk3";

	[SerializeField]
	protected string[] topic3lines = new string[2] { "* This is Talk3.", "* Woo..." };

	[SerializeField]
	protected string topic4 = "Talk4";

	[SerializeField]
	protected string[] topic4lines = new string[2] { "* This is Talk4.", "* Woo..." };

	[SerializeField]
	protected string sellGreet = "What would \nyou like \nto sell?";

	[SerializeField]
	protected string sellSuccess = "Thanks for \nthat!";

	[SerializeField]
	protected string sellReject = "Changed your \nmind?";

	[SerializeField]
	protected string sellDeny = "We are \nnot accepting \nthat.";

	protected int[] itemIDs;

	protected int[] itemPrices;

	protected string[] itemDescriptions;

	protected string[] talkNames;

	protected int state;

	protected bool greeted;

	protected AudioSource aud;

	protected TextUT text;

	protected string[] diag;

	protected int curString;

	protected int endToState;

	protected int index;

	protected int buyIndex;

	protected bool selecting = true;

	protected bool holdingAxis;

	protected bool holdingAxisH;

	protected Transform detailsPanel;

	protected int detailsScrollFrames;

	protected Sprite[] detailsIcons;

	protected Transform soul;

	protected bool sellMenuEnabled;

	protected bool willBuyItems;

	protected bool confirmSell;

	protected int flavor;

	protected bool sellingEquipment;

	protected virtual void Awake()
	{
		text = GetComponent<TextUT>();
		aud = GetComponent<AudioSource>();
		soul = base.transform.Find("SOUL");
		soul.GetComponent<Image>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
		detailsPanel = base.transform.Find("InfoPanel");
		detailsIcons = Resources.LoadAll<Sprite>("ui/shop/spr_shop_stat_icons");
		flavor = (int)Util.GameManager().GetFlag(223);
		if (flavor != 0)
		{
			Image[] componentsInChildren = GetComponentsInChildren<Image>();
			foreach (Image image in componentsInChildren)
			{
				if (image.sprite == null && image.color == Color.white)
				{
					image.color = UIBackground.borderColors[flavor];
				}
			}
		}
		DetermineWillBuy(justSold: false);
	}

	private void Start()
	{
		Util.FindObjectOfType<Fade>().FadeIn(13);
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
		OrganizeArrays();
		UpdateStatText();
		ToMainMenu();
	}

	protected virtual void Update()
	{
		if (selecting)
		{
			if (state == 6)
			{
				int num = Mathf.RoundToInt(UTInput.GetAxis("Vertical"));
				int num2 = Mathf.RoundToInt(UTInput.GetAxis("Horizontal"));
				if (num != 0 && !holdingAxis)
				{
					holdingAxis = true;
					if (index == 8)
					{
						if (num < 0 && GetItem(0) > -1)
						{
							index = 0;
						}
						else if (num > 0)
						{
							int num3 = 6;
							while (num3 >= 0 && GetItem(num3) == -1)
							{
								num3 -= 2;
							}
							index = ((num3 >= 0) ? num3 : 8);
						}
					}
					else
					{
						index += -num * 2;
						if (index < 0)
						{
							index = 8;
						}
						if (index == 9)
						{
							index = 8;
						}
						if (index > 8)
						{
							index = 0;
						}
						else if (index < 8 && index % 2 == 1 && GetItem(index) == -1)
						{
							index--;
						}
						if (index < 8 && GetItem(index) == -1)
						{
							index = 8;
						}
					}
					UpdateDetailsPanel();
				}
				else if (num == 0 && holdingAxis)
				{
					holdingAxis = false;
				}
				if (num2 != 0 && !holdingAxisH)
				{
					holdingAxisH = true;
					if (index < 8)
					{
						if (index % 2 == 0 && GetItem(index + 1) > -1)
						{
							index++;
						}
						else if (index % 2 == 1)
						{
							index--;
						}
						UpdateDetailsPanel();
					}
				}
				else if (num2 == 0 && holdingAxisH)
				{
					holdingAxisH = false;
				}
			}
			else if (UTInput.GetAxis("Vertical") != 0f && !holdingAxis)
			{
				holdingAxis = true;
				int num4 = 5;
				if (state == 0)
				{
					num4 = 4;
				}
				if (state == 2)
				{
					num4 = 2;
				}
				index = (index - (int)UTInput.GetAxis("Vertical")) % num4;
				if (index < 0)
				{
					index = num4 - 1;
				}
				if (state == 1)
				{
					UpdateDetailsPanel();
					UpdateStatText(index >= 4 || Items.IsEquipment(itemIDs[index]));
				}
				MonoBehaviour.print(index);
			}
			else if (UTInput.GetAxis("Vertical") == 0f && holdingAxis)
			{
				holdingAxis = false;
			}
		}
		bool flag = false;
		if (state == 1 || state == 2 || state == 6)
		{
			int num5 = ((state == 6 || (state == 2 && confirmSell)) ? 5 : 20);
			int num6 = ((state == 6) ? 8 : 4);
			if (index < num6 && detailsScrollFrames < num5)
			{
				detailsScrollFrames++;
			}
			else if (index == num6 && detailsScrollFrames > 0)
			{
				detailsScrollFrames -= 4;
				if (index == num6 && detailsScrollFrames < 0)
				{
					detailsScrollFrames = 0;
				}
			}
		}
		if ((bool)text.GetGameObject() && text.IsPlaying() && (UTInput.GetButton("X") || UTInput.GetButton("C")))
		{
			flag = true;
			text.SkipText();
		}
		if (state == 0)
		{
			if (UTInput.GetButtonDown("Z"))
			{
				if (index == 0)
				{
					ToBuyMenu();
				}
				else if (index == 1)
				{
					if (sellMenuEnabled)
					{
						ToSellMenu();
					}
					else
					{
						endToState = 0;
						StartFullTalk(sellPrompt);
					}
				}
				else if (index == 2)
				{
					ToTalkMenu();
				}
				else if (index == 3)
				{
					endToState = 5;
					StartFullTalk(new string[1] { exitMessage });
				}
			}
		}
		else if (state == 1)
		{
			if ((UTInput.GetButtonDown("X") && !flag) || (UTInput.GetButtonDown("Z") && index == 4))
			{
				index = 0;
				ToMainMenu();
				detailsScrollFrames = 0;
				UpdateStatText();
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				if (text.IsPlaying())
				{
					text.SkipText();
				}
				text.DestroyOldText();
				buyIndex = index;
				index = 0;
				Text[] componentsInChildren = base.transform.Find("ConfirmMenu").GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = true;
				}
				base.transform.Find("ConfirmMenu").Find("Message").GetComponent<Text>()
					.text = $"Buy it for\n{itemPrices[buyIndex]}G ?";
				state = 2;
				confirmSell = false;
			}
		}
		else if (state == 2)
		{
			if ((UTInput.GetButtonDown("X") && !flag) || UTInput.GetButtonDown("Z"))
			{
				Text[] componentsInChildren = base.transform.Find("ConfirmMenu").GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				bool flag2 = index == 1 || UTInput.GetButtonDown("X");
				if (flag2)
				{
					StartText(confirmSell ? sellReject : buyReject);
				}
				else if (confirmSell && !willBuyItems)
				{
					StartText(GetSellDenyText(-1));
				}
				else if (!confirmSell && Util.GameManager().GetGold() < itemPrices[buyIndex])
				{
					StartText(buyNoMoney);
				}
				else if (!confirmSell && Util.GameManager().NumItemFreeSpace(Items.IsEquipment(itemIDs[buyIndex])) == 0)
				{
					StartText(buyNoSpace);
				}
				else if (confirmSell)
				{
					int item = GetItem(buyIndex);
					Util.GameManager().AddGold(Items.GetSellPrice(item));
					if (sellingEquipment)
					{
						Util.GameManager().RemoveEquipment(buyIndex);
					}
					else
					{
						Util.GameManager().RemoveItem(buyIndex);
					}
					soul.GetComponent<Image>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
					aud.Play();
					StartText(sellSuccess);
					UpdateStatText();
					UpdateSellOptions();
					DetermineWillBuy(justSold: true);
				}
				else
				{
					Util.GameManager().RemoveGold(itemPrices[buyIndex]);
					Util.GameManager().AddAmbiguousItem(itemIDs[buyIndex]);
					aud.Play();
					StartText(buySuccess);
					UpdateStatText();
				}
				index = buyIndex;
				state = ((!confirmSell) ? 1 : 6);
				if (state == 1)
				{
					UpdateStatText(Items.IsEquipment(itemIDs[index]));
				}
				if (state == 6 && !flag2 && GetItem(index) == -1)
				{
					index--;
					if (index < 0)
					{
						index = 8;
					}
				}
			}
		}
		else if (state == 3)
		{
			if ((UTInput.GetButtonDown("X") && !flag) || (UTInput.GetButtonDown("Z") && index == 4))
			{
				index = 2;
				ToMainMenu();
				detailsScrollFrames = 0;
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				buyIndex = index;
				endToState = 3;
				switch (index)
				{
				case 0:
					StartFullTalk(topic1lines);
					break;
				case 1:
					StartFullTalk(topic2lines);
					break;
				case 2:
					StartFullTalk(topic3lines);
					break;
				case 3:
					StartFullTalk(topic4lines);
					break;
				}
			}
		}
		else if (state == 4)
		{
			if ((bool)text.GetGameObject() && !(text.IsPlaying() || flag) && (UTInput.GetButtonDown("Z") || UTInput.GetButton("C")))
			{
				curString++;
				if (curString < diag.Length)
				{
					StartText(diag[curString]);
				}
				else
				{
					text.DestroyOldText();
					if (endToState == 0)
					{
						ToMainMenu();
						index = 1;
					}
					else if (endToState == 3)
					{
						ToTalkMenu();
						index = buyIndex;
					}
					else if (endToState == 5)
					{
						Util.FindObjectOfType<Fade>().FadeOut(12);
						state = 5;
					}
					else if (endToState == 6)
					{
						ToSellMenu();
					}
				}
			}
		}
		else if (state == 5)
		{
			if (!Util.FindObjectOfType<Fade>().IsPlaying())
			{
				HandleExit(enableMovement: true);
			}
		}
		else if (state == 6)
		{
			if ((UTInput.GetButtonDown("X") && !flag) || (UTInput.GetButtonDown("Z") && index == 8))
			{
				base.transform.Find("SellMenu").Find("Tab").GetChild(0)
					.GetComponent<Image>()
					.enabled = false;
				index = 0;
				ToMainMenu();
				detailsScrollFrames = 0;
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				if (text.IsPlaying())
				{
					text.SkipText();
				}
				text.DestroyOldText();
				int item2 = GetItem(index);
				if (Items.GetSellPrice(item2) > 0)
				{
					buyIndex = index;
					index = 0;
					Text[] componentsInChildren = base.transform.Find("ConfirmMenu").GetComponentsInChildren<Text>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].enabled = true;
					}
					base.transform.Find("ConfirmMenu").Find("Message").GetComponent<Text>()
						.text = $"Sell the\n{Items.ShortItemName(item2)}?";
					confirmSell = true;
					state = 2;
				}
				else
				{
					StartText(GetSellDenyText(item2));
				}
			}
			else if (UTInput.GetButtonDown("C"))
			{
				sellingEquipment = !sellingEquipment;
				UpdateSellOptions();
				if (index < 8 && GetItem(index) == -1)
				{
					int num7 = 7 - Util.GameManager().NumItemFreeSpace(sellingEquipment);
					index = num7;
					if (index == -1)
					{
						index = 8;
					}
				}
				UpdateDetailsPanel();
			}
		}
		string n = "SelectionMenu";
		if (state == 0)
		{
			n = "MainMenu";
		}
		if (state == 2)
		{
			n = "ConfirmMenu";
		}
		if (state == 6)
		{
			n = "SellMenu";
		}
		soul.localPosition = base.transform.Find(n).GetChild(index).localPosition - new Vector3(21f, 0f);
		soul.GetComponent<Image>().enabled = state < 4 || state == 6;
		detailsPanel.localPosition = new Vector2(0f, Mathf.Lerp(-220f, 0f, (float)detailsScrollFrames / 20f));
	}

	protected void ToMainMenu()
	{
		ToggleOtherObjects(enabled: true);
		state = 0;
		Text[] componentsInChildren = base.transform.Find("SelectionMenu").GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		componentsInChildren = base.transform.Find("SellMenu").GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		componentsInChildren = base.transform.Find("MainMenu").GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = true;
		}
		if (greeted)
		{
			StartText(idleMessage);
		}
		else
		{
			StartText(greetMessage);
		}
		greeted = true;
	}

	protected void ToBuyMenu()
	{
		state = 1;
		index = 0;
		for (int i = 0; i < 4; i++)
		{
			base.transform.Find("SelectionMenu").GetChild(i).GetComponent<Text>()
				.enabled = true;
			base.transform.Find("SelectionMenu").GetChild(i).GetComponent<Text>()
				.text = itemPrices[i] + "G - " + Items.ItemName(itemIDs[i]);
		}
		base.transform.Find("SelectionMenu").GetChild(4).GetComponent<Text>()
			.enabled = true;
		StartText(buyGreet);
		Text[] componentsInChildren = base.transform.Find("MainMenu").GetComponentsInChildren<Text>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].enabled = false;
		}
		UpdateStatText(Items.IsEquipment(itemIDs[index]));
		UpdateDetailsPanel();
	}

	protected virtual void ToSellMenu()
	{
		state = 6;
		index = ((GetItem(index) <= -1) ? 8 : 0);
		for (int i = 0; i < 10; i++)
		{
			base.transform.Find("SellMenu").GetChild(i).GetComponent<Text>()
				.enabled = true;
		}
		StartText(sellGreet);
		Text[] componentsInChildren = base.transform.Find("MainMenu").GetComponentsInChildren<Text>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].enabled = false;
		}
		UpdateSellOptions();
		UpdateDetailsPanel();
	}

	protected void ToTalkMenu()
	{
		ToggleOtherObjects(enabled: true);
		state = 3;
		index = 0;
		for (int i = 0; i < 4; i++)
		{
			base.transform.Find("SelectionMenu").GetChild(i).GetComponent<Text>()
				.enabled = true;
			base.transform.Find("SelectionMenu").GetChild(i).GetComponent<Text>()
				.text = talkNames[i];
		}
		base.transform.Find("SelectionMenu").GetChild(4).GetComponent<Text>()
			.enabled = true;
		StartText(talkGreet);
		Text[] componentsInChildren = base.transform.Find("MainMenu").GetComponentsInChildren<Text>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].enabled = false;
		}
	}

	protected virtual void StartFullTalk(string[] diag)
	{
		Text[] componentsInChildren = base.transform.Find("SelectionMenu").GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		componentsInChildren = base.transform.Find("MainMenu").GetComponentsInChildren<Text>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].enabled = false;
		}
		ToggleOtherObjects(enabled: false);
		this.diag = diag;
		curString = 0;
		state = 4;
		StartText(diag[0]);
	}

	protected virtual void StartText(string txt)
	{
		if (text.IsPlaying())
		{
			text.SkipText();
		}
		text.DestroyOldText();
		Vector2 thePos = new Vector2(407f, -141f);
		if (state == 0)
		{
			thePos.x = -11f;
		}
		if (state == 4)
		{
			thePos.x = 9f;
		}
		if (fontName == "sans")
		{
			thePos.y -= 4f;
		}
		text.StartText(txt, thePos, textSound, 0, fontName);
		text.GetGameObject().GetComponent<Text>().lineSpacing = ((fontName == "sans") ? 0.9f : 1.15f);
		text.GetGameObject().GetComponent<Text>().rectTransform.sizeDelta = new Vector2(538f, 240f);
	}

	protected void OrganizeArrays()
	{
		itemIDs = new int[4] { item1ID, item2ID, item3ID, item4ID };
		itemPrices = new int[4] { item1price, item2price, item3price, item4price };
		itemDescriptions = new string[4] { item1ShortDesc, item2ShortDesc, item3ShortDesc, item4ShortDesc };
		talkNames = new string[4] { topic1, topic2, topic3, topic4 };
	}

	protected void UpdateSellOptions()
	{
		for (int i = 0; i < 8; i++)
		{
			string text = "";
			int item = GetItem(i);
			if (item > -1)
			{
				text = Items.ShortItemName(item);
			}
			base.transform.Find("SellMenu").GetChild(i).GetComponent<Text>()
				.text = text;
		}
		bool joystickIsActive = UTInput.joystickIsActive;
		string text2 = "";
		if (!joystickIsActive)
		{
			text2 = string.Format("[{0}] ", UTInput.GetKeyName("Menu"));
		}
		string arg = (sellingEquipment ? "FFF" : ColorUtility.ToHtmlStringRGB(Selection.SELECTION_COLORS[Util.GameManager().GetFlagInt(223)]));
		string arg2 = ((!sellingEquipment) ? "FFF" : ColorUtility.ToHtmlStringRGB(Selection.SELECTION_COLORS[Util.GameManager().GetFlagInt(223)]));
		base.transform.Find("SellMenu").Find("Tab").GetComponent<Text>()
			.text = text2 + $"<color=#{arg}>Item</color> <color=#888>|</color> <color=#{arg2}>Equip</color>";
		Image component = base.transform.Find("SellMenu").Find("Tab").GetChild(0)
			.GetComponent<Image>();
		if (!joystickIsActive)
		{
			component.enabled = false;
			return;
		}
		component.enabled = true;
		ButtonPrompts.UpdateImageWithGraphic("Menu", component, 2f, ButtonPrompts.ButtonType.Small);
	}

	protected void UpdateStatText(bool isEquipment = false)
	{
		if (state == 1 && index < 4)
		{
			base.transform.Find("Gold").GetComponent<Text>().text = Util.GameManager().GetGold() + "G";
			base.transform.Find("Space").GetComponent<Text>().text = 8 - Util.GameManager().NumItemFreeSpace(isEquipment) + "/8";
		}
		else
		{
			base.transform.Find("Gold").GetComponent<Text>().text = "";
			base.transform.Find("Space").GetComponent<Text>().text = Util.GameManager().GetGold() + "G";
		}
	}

	protected void ToggleOtherObjects(bool enabled)
	{
		base.transform.Find("Gold").GetComponent<Text>().enabled = enabled;
		base.transform.Find("Space").GetComponent<Text>().enabled = enabled;
		base.transform.Find("Separator").GetComponent<Image>().enabled = enabled;
	}

	protected virtual void HandleExit(bool enableMovement)
	{
		Util.FindObjectOfType<Fade>().FadeIn(13);
		if (enableMovement)
		{
			Util.GameManager().EnablePlayerMovement();
		}
		Object.Destroy(base.gameObject);
	}

	protected void UpdateDetailsPanel()
	{
		if (state == 1)
		{
			if (index == 4)
			{
				Text[] componentsInChildren = detailsPanel.GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				Image[] componentsInChildren2 = detailsPanel.Find("Stats").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
				return;
			}
			detailsPanel.Find("Type").GetComponent<Text>().enabled = true;
			detailsPanel.Find("Description").GetComponent<Text>().enabled = true;
			int num = Items.ItemType(itemIDs[index]);
			string text = (new string[4] { "HEALS {0} HP", "WEAPON {0} AT", "ARMOR {0} DF", "ITEM" })[num];
			if (text.Contains("{0}"))
			{
				text = string.Format(text, Items.ItemValue(itemIDs[index]));
			}
			detailsPanel.Find("Type").GetComponent<Text>().text = text;
			detailsPanel.Find("Description").GetComponent<Text>().text = Util.Unescape(itemDescriptions[index]);
			if (num == 1 || num == 2)
			{
				Text[] componentsInChildren = detailsPanel.Find("Stats").GetComponentsInChildren<Text>();
				foreach (Text obj in componentsInChildren)
				{
					obj.color = Color.white;
					obj.text = "0";
				}
				string[] array = new string[3] { "Kris", "Susie", "Noelle" };
				int[] array2 = new int[3];
				int[] array3 = new int[3];
				_ = 1;
				if (num == 1)
				{
					detailsPanel.Find("Stats").Find("SusieIcon").GetComponent<Image>()
						.sprite = Resources.Load<Sprite>("battle/spr_su_icon_grey");
				}
				else
				{
					detailsPanel.Find("Stats").Find("SusieIcon").GetComponent<Image>()
						.sprite = Resources.Load<Sprite>("battle/spr_su_icon");
				}
				for (int j = 0; j < 3; j++)
				{
					bool flag = j == 0 || (j == 1 && Util.GameManager().SusieInParty()) || (j == 2 && Util.GameManager().NoelleInParty());
					detailsPanel.Find("Stats").Find(array[j] + "Icon").GetComponent<Image>()
						.enabled = flag;
					detailsPanel.Find("Stats").Find(array[j] + "StatIcons").GetComponent<Image>()
						.enabled = flag;
					detailsPanel.Find("Stats").Find(array[j] + "ATK").GetComponent<Text>()
						.enabled = flag;
					detailsPanel.Find("Stats").Find(array[j] + "DEF").GetComponent<Text>()
						.enabled = flag;
					if (j == 0 || (j == 1 && Util.GameManager().SusieInParty()) || (j == 2 && Util.GameManager().NoelleInParty()))
					{
						string text2 = "0";
						string text3 = "0";
						Color[] array4 = new Color[2]
						{
							Color.white,
							Color.white
						};
						detailsPanel.Find("Stats").Find(array[j] + "StatIcons").GetComponent<Image>()
							.sprite = detailsIcons[num - 1];
						array2[j] = Items.ItemValue(itemIDs[index], j) + Util.GameManager().GetATKRaw(j) - Util.GameManager().GetATK(j);
						if (num == 2)
						{
							array2[j] = Items.ItemValue(itemIDs[index], j) + Util.GameManager().GetDEFRaw(j) - Util.GameManager().GetDEF(j);
						}
						array3[j] = Items.GetItemMagic(itemIDs[index]) - Items.GetItemMagic((num == 1) ? Util.GameManager().GetWeapon(j) : Util.GameManager().GetArmor(j));
						if (array2[j] > 0)
						{
							text2 = "+" + array2[j];
							array4[0] = new Color(1f, 1f, 0f);
						}
						else if (array2[j] < 0)
						{
							text2 = array2[j].ToString();
							array4[0] = new Color(0f, 1f, 1f);
						}
						if (array3[j] > 0)
						{
							text3 = "+" + array3[j];
							array4[1] = new Color(1f, 1f, 0f);
						}
						else if (array3[j] < 0)
						{
							text3 = array3[j].ToString();
							array4[1] = new Color(0f, 1f, 1f);
						}
						detailsPanel.Find("Stats").Find(array[j] + "ATK").GetComponent<Text>()
							.text = text2;
						detailsPanel.Find("Stats").Find(array[j] + "ATK").GetComponent<Text>()
							.color = array4[0];
						detailsPanel.Find("Stats").Find(array[j] + "DEF").GetComponent<Text>()
							.text = text3;
						detailsPanel.Find("Stats").Find(array[j] + "DEF").GetComponent<Text>()
							.color = array4[1];
					}
				}
			}
			else
			{
				Text[] componentsInChildren = detailsPanel.Find("Stats").GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				Image[] componentsInChildren2 = detailsPanel.Find("Stats").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
			}
		}
		else
		{
			if (state != 6)
			{
				return;
			}
			if (index == 8)
			{
				Text[] componentsInChildren = detailsPanel.GetComponentsInChildren<Text>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				Image[] componentsInChildren2 = detailsPanel.Find("Stats").GetComponentsInChildren<Image>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
			}
			else
			{
				detailsPanel.Find("Type").GetComponent<Text>().enabled = true;
				int sellPrice = Items.GetSellPrice(GetItem(index));
				if (sellPrice > 0)
				{
					detailsPanel.Find("Type").GetComponent<Text>().text = "Worth " + sellPrice + "G";
				}
				else
				{
					detailsPanel.Find("Type").GetComponent<Text>().text = "Not buying";
				}
			}
		}
	}

	protected virtual string GetSellDenyText(int itemID)
	{
		return sellDeny;
	}

	protected virtual void DetermineWillBuy(bool justSold)
	{
	}

	protected int GetItem(int index)
	{
		if (!sellingEquipment)
		{
			return Util.GameManager().GetItem(index);
		}
		return Util.GameManager().GetEquipment(index);
	}
}
