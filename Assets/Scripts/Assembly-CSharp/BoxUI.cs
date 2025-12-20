using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BoxUI : UIComponent
{
	private static readonly int BOX_SIZE = 10;

	private static readonly int INVENTORY_SIZE = 8;

	private GameManager gm;

	private int index;

	private bool inventorySide = true;

	private List<int> boxItems = new List<int>();

	private bool holdAxis;

	private void Awake()
	{
		gm = Util.GameManager();
		if ((int)gm.GetFlag(156) == 0)
		{
			gm.SetFlag(156, 1);
			gm.SetFlag(157, 32);
			for (int i = 1; i < BOX_SIZE; i++)
			{
				gm.SetFlag(157 + i, -1);
			}
			boxItems.Add(32);
		}
		else
		{
			for (int j = 0; j < BOX_SIZE; j++)
			{
				int num = (int)gm.GetFlag(157 + j);
				if (num > -1)
				{
					boxItems.Add(num);
				}
			}
		}
		int num2 = (int)gm.GetFlag(223);
		if (num2 > 0)
		{
			base.transform.Find("Border").GetComponent<Image>().color = UIBackground.borderColors[num2];
			base.transform.Find("Separator").GetComponent<Image>().color = UIBackground.borderColors[num2];
			base.transform.Find("InvLines").GetComponent<Image>().color = BattleButton.buttonColors[num2];
		}
		UpdateText();
		if (UTInput.joystickIsActive)
		{
			base.transform.Find("Labels").Find("Exit").GetComponent<Text>()
				.text = "Press      to Finish";
			base.transform.Find("Labels").Find("Cancel").GetComponent<Image>()
				.enabled = true;
			for (int k = 0; k < ButtonPrompts.validButtons.Length; k++)
			{
				if (UTInput.GetKeyOrButtonReplacement("Cancel") == ButtonPrompts.GetButtonChar(ButtonPrompts.validButtons[k]))
				{
					base.transform.Find("Labels").Find("Cancel").GetComponent<Image>()
						.sprite = Resources.Load<Sprite>("ui/buttons/" + ButtonPrompts.GetButtonGraphic(ButtonPrompts.validButtons[k]));
					break;
				}
			}
		}
		else
		{
			base.transform.Find("Labels").Find("Exit").GetComponent<Text>()
				.text = string.Format("Press [{0}] to Finish", UTInput.GetKeyName("Cancel"));
		}
	}

	private void Update()
	{
		int num = (inventorySide ? INVENTORY_SIZE : BOX_SIZE);
		if (UTInput.GetAxis("Horizontal") != 0f && !holdAxis)
		{
			inventorySide = !inventorySide;
			if (inventorySide && index >= INVENTORY_SIZE)
			{
				index = INVENTORY_SIZE - 1;
			}
			holdAxis = true;
		}
		else if (UTInput.GetAxis("Vertical") != 0f && !holdAxis)
		{
			index -= (int)UTInput.GetAxis("Vertical");
			if (index >= num)
			{
				index = 0;
			}
			else if (index < 0)
			{
				index = num - 1;
			}
			holdAxis = true;
		}
		else if (UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f && holdAxis)
		{
			holdAxis = false;
		}
		base.transform.Find("SOUL").localPosition = base.transform.Find(inventorySide ? "InvText" : "BoxText").GetChild(index).localPosition - new Vector3(19f, -14f);
		if (UTInput.GetButtonDown("Z"))
		{
			if (inventorySide && gm.GetItem(index) > -1 && boxItems.Count < BOX_SIZE)
			{
				boxItems.Add(gm.GetItem(index));
				gm.RemoveItem(index);
				UpdateText();
			}
			else if (!inventorySide && index < boxItems.Count && gm.NumItemFreeSpace() > 0)
			{
				int id = boxItems[index];
				gm.AddItem(id);
				boxItems.RemoveAt(index);
				UpdateText();
			}
		}
		if (UTInput.GetButtonDown("X"))
		{
			WriteBoxItems();
			gm.EnablePlayerMovement();
			Object.Destroy(base.gameObject);
		}
	}

	public void WriteBoxItems()
	{
		for (int i = 0; i < BOX_SIZE; i++)
		{
			if (i < boxItems.Count)
			{
				gm.SetFlag(157 + i, boxItems[i]);
			}
			else
			{
				gm.SetFlag(157 + i, -1);
			}
		}
	}

	private void UpdateText()
	{
		for (int i = 0; i < BOX_SIZE; i++)
		{
			if (i < 8)
			{
				if (gm.GetItem(i) > -1)
				{
					base.transform.Find("InvText").GetChild(i).GetComponent<Text>()
						.text = Items.ItemName(gm.GetItem(i));
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
			if (i < boxItems.Count)
			{
				base.transform.Find("BoxText").GetChild(i).GetComponent<Text>()
					.text = Items.ItemName(boxItems[i]);
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
}
