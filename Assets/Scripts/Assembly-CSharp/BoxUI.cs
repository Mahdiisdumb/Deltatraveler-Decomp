using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : UIComponent
{
	private static readonly int BOX_SIZE = 30;

	private static readonly int INVENTORY_SIZE = 8;

	private static readonly int BOX_UI_SIZE = 10;

	private GameManager gm;

	private int index;

	private bool inventorySide = true;

	private int scrollOffset;

	private bool holdAxis;

	private bool equipmentTab;

	private bool selectingTab = true;

	private List<int> boxItems = new List<int>();

	private void Awake()
	{
		gm = Util.GameManager();
		if ((int)gm.GetFlag(156) == 0)
		{
			gm.SetFlag(156, 1);
			boxItems.Add(32);
			SaveItems();
		}
		else
		{
			boxItems = gm.GetBoxList();
		}
		int num = (int)gm.GetFlag(223);
		if (num > 0)
		{
			base.transform.Find("Border").GetComponent<Image>().color = UIBackground.borderColors[num];
			base.transform.Find("Separator").GetComponent<Image>().color = UIBackground.borderColors[num];
			base.transform.Find("InvLines").GetComponent<Image>().color = BattleButton.BUTTON_COLORS[num];
		}
		UpdateText();
		if (UTInput.joystickIsActive)
		{
			base.transform.Find("Labels").Find("Exit").GetComponent<Text>()
				.text = "Press      to Finish";
			base.transform.Find("Labels").Find("Cancel").GetComponent<Image>()
				.enabled = true;
			ButtonPrompts.UpdateImageWithGraphic("Cancel", base.transform.Find("Labels").Find("Cancel").GetComponent<Image>());
		}
		else
		{
			base.transform.Find("Labels").Find("Exit").GetComponent<Text>()
				.text = string.Format("Press [{0}] to Finish", UTInput.GetKeyName("Cancel"));
		}
	}

	private void Update()
	{
		if (selectingTab)
		{
			if (UTInput.GetAxis("Horizontal") != 0f && !holdAxis)
			{
				equipmentTab = !equipmentTab;
				holdAxis = true;
				UpdateText();
			}
			else if (UTInput.GetAxis("Vertical") != 0f && !holdAxis)
			{
				index = ((UTInput.GetAxis("Vertical") > 0f) ? (INVENTORY_SIZE - 1) : 0);
				holdAxis = true;
				selectingTab = false;
			}
			else if (UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f && holdAxis)
			{
				holdAxis = false;
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				index = 0;
				selectingTab = false;
			}
		}
		else
		{
			int num = (inventorySide ? INVENTORY_SIZE : BOX_SIZE);
			if (UTInput.GetAxis("Horizontal") != 0f && !holdAxis)
			{
				inventorySide = !inventorySide;
				if (inventorySide)
				{
					if (index >= INVENTORY_SIZE)
					{
						index = INVENTORY_SIZE - 1;
					}
					else if (index == 0)
					{
						index = 0;
					}
					else
					{
						index--;
					}
				}
				else
				{
					index++;
				}
				holdAxis = true;
			}
			else if (UTInput.GetAxis("Vertical") != 0f && !holdAxis)
			{
				index -= (int)UTInput.GetAxis("Vertical");
				if (inventorySide)
				{
					if (index >= num || index < 0)
					{
						selectingTab = true;
					}
				}
				else
				{
					int num2 = scrollOffset;
					if (index < 0)
					{
						index = 0;
						scrollOffset--;
						if (scrollOffset < 0)
						{
							scrollOffset = BOX_SIZE - BOX_UI_SIZE;
							index = BOX_UI_SIZE - 1;
							MonoBehaviour.print("TESTING MAX CALCULATION " + GetBoxIndex());
						}
					}
					else if (index >= BOX_UI_SIZE)
					{
						index = BOX_UI_SIZE - 1;
						scrollOffset++;
						if (scrollOffset >= BOX_SIZE - BOX_UI_SIZE)
						{
							scrollOffset = 0;
							index = 0;
						}
					}
					if (scrollOffset != num2)
					{
						UpdateText();
					}
				}
				holdAxis = true;
			}
			else if (UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f && holdAxis)
			{
				holdAxis = false;
			}
			if (UTInput.GetButtonDown("Z"))
			{
				if (inventorySide && boxItems.Count < BOX_SIZE)
				{
					int num3 = (equipmentTab ? gm.GetEquipment(index) : gm.GetItem(index));
					if (num3 > -1)
					{
						boxItems.Add(num3);
						if (equipmentTab)
						{
							gm.RemoveEquipment(index);
						}
						else
						{
							gm.RemoveItem(index);
						}
						UpdateText();
					}
				}
				else if (!inventorySide && GetBoxIndex() < boxItems.Count)
				{
					int item = boxItems[GetBoxIndex()];
					equipmentTab = Items.IsEquipment(item);
					if (gm.NumItemFreeSpace(equipmentTab) > 0)
					{
						gm.AddAmbiguousItem(item);
						boxItems.RemoveAt(GetBoxIndex());
					}
					UpdateText();
				}
			}
		}
		if (selectingTab)
		{
			base.transform.Find("SOUL").localPosition = base.transform.Find("Tabs").GetChild(equipmentTab ? 1 : 0).localPosition - new Vector3(19f, -14f);
		}
		else
		{
			base.transform.Find("SOUL").localPosition = base.transform.Find(inventorySide ? "InvText" : "BoxText").GetChild(index).localPosition - new Vector3(19f, -14f);
		}
		if (UTInput.GetButtonDown("X"))
		{
			SaveItems();
			gm.EnablePlayerMovement();
			Object.Destroy(base.gameObject);
		}
	}

	private void UpdateText()
	{
		base.transform.Find("Tabs").GetChild(0).GetComponent<Text>()
			.color = (equipmentTab ? Color.white : Selection.SELECTION_COLORS[gm.GetFlagInt(223)]);
		base.transform.Find("Tabs").GetChild(1).GetComponent<Text>()
			.color = ((!equipmentTab) ? Color.white : Selection.SELECTION_COLORS[gm.GetFlagInt(223)]);
		base.transform.Find("Arrows").Find("Up").GetComponent<Image>()
			.enabled = scrollOffset > 0;
		base.transform.Find("Arrows").Find("Down").GetComponent<Image>()
			.enabled = scrollOffset < BOX_SIZE - BOX_UI_SIZE - 1;
		for (int i = 0; i < BOX_UI_SIZE; i++)
		{
			if (i < 8)
			{
				int num = (equipmentTab ? gm.GetEquipment(i) : gm.GetItem(i));
				if (num > -1)
				{
					base.transform.Find("InvText").GetChild(i).GetComponent<Text>()
						.text = Items.ItemName(num);
					base.transform.Find("InvText").GetChild(i).GetComponent<Text>()
						.enabled = true;
					base.transform.Find("InvLineCovers").GetChild(i).GetComponent<Image>()
						.enabled = true;
				}
				else
				{
					base.transform.Find("InvText").GetChild(i).GetComponent<Text>()
						.enabled = false;
					base.transform.Find("InvLineCovers").GetChild(i).GetComponent<Image>()
						.enabled = false;
				}
			}
			int num2 = i + scrollOffset;
			if (num2 < boxItems.Count)
			{
				base.transform.Find("BoxText").GetChild(i).GetComponent<Text>()
					.text = Items.ItemName(boxItems[num2]);
				base.transform.Find("BoxText").GetChild(i).GetComponent<Text>()
					.enabled = true;
				base.transform.Find("BoxLineCovers").GetChild(i).GetComponent<Image>()
					.enabled = true;
			}
			else
			{
				base.transform.Find("BoxText").GetChild(i).GetComponent<Text>()
					.enabled = false;
				base.transform.Find("BoxLineCovers").GetChild(i).GetComponent<Image>()
					.enabled = false;
			}
		}
		base.transform.Find("SOUL").GetComponent<Image>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
	}

	public void SaveItems()
	{
		gm.SetBoxList(boxItems);
	}

	private int GetBoxIndex()
	{
		return index + scrollOffset;
	}
}
