using UnityEngine;
using UnityEngine.UI;

public class PicnicBasket : InteractSelectionBase
{
	private bool banana;

	private const int BANANA_PRICE = 35;

	private const int EGG_PRICE = 20;

	private bool eggSoundPlayed;

	protected UIBackground shopBG;

	private bool disabled;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(116) != 0 || (int)Util.GameManager().GetFlag(87) >= 5)
		{
			disabled = true;
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/eb_objects/spr_hh_picnic_empty");
		}
	}

	private void LateUpdate()
	{
		if (!txt && (bool)shopBG)
		{
			Object.Destroy(shopBG.gameObject);
		}
		else if ((bool)txt && disabled && txt.GetCurrentStringNum() == 3 && !eggSoundPlayed)
		{
			eggSoundPlayed = true;
			Util.GameManager().PlayGlobalSFX("sounds/snd_egg");
		}
	}

	public override void DoInteract()
	{
		if (disabled)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			if (Util.GameManager().GetItemList().Contains(16))
			{
				Util.GameManager().RemoveItem(Util.GameManager().GetItemList().IndexOf(16));
				txt.CreateBox(new string[5] { "* (It appears someone has\n  taken all the food.)", "* (...)", "* (You put the Egg in the\n  empty egg basket.)", "* (Strangely,^05 you noticed a shiny\n  bat somehow hiding behind it.)", "* (You got the Aluminum Bat.)" });
				Util.GameManager().AddItem(31);
			}
			else
			{
				txt.CreateBox(new string[1] { "* (It appears someone has\n  taken all the food.)" });
			}
			Util.GameManager().DisablePlayerMovement(false);
			return;
		}
		if (!txt && enabled)
		{
			shopBG = new GameObject("ShopMenu").AddComponent<UIBackground>();
			shopBG.transform.parent = GameObject.Find("Canvas").transform;
			shopBG.CreateElement("space", new Vector2(189f, 2f), new Vector2(202f, 108f));
			Text component = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), shopBG.transform).GetComponent<Text>();
			component.gameObject.name = "SpaceInfo";
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			component.transform.localPosition = new Vector3(116f, -71f);
			component.text = "$ - " + Object.FindObjectOfType<GameManager>().GetGold() + "G\nSPACE - " + (8 - Object.FindObjectOfType<GameManager>().NumItemFreeSpace()) + "/8";
			component.lineSpacing = 1.3f;
		}
		base.DoInteract();
	}

	protected override void HandleTextExist()
	{
		if (selectID == 0)
		{
			base.HandleTextExist();
		}
		else if (selectID == 1 && txt.CanLoadSelection() && !selectActivated)
		{
			selectActivated = true;
			DeltaSelection component = Object.Instantiate(Resources.Load<GameObject>("ui/DeltaSelection"), Vector3.zero, Quaternion.identity, txt.GetUIBox().transform).GetComponent<DeltaSelection>();
			component.SetupChoice(Vector2.left, "Pay", leftOffset);
			component.SetupChoice(Vector2.right, "Don't Pay", new Vector2(-32f, 0f));
			component.SetupChoice(Vector2.down, "Cancel", downOffset);
			component.Activate(this, selectID, txt.gameObject);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectID = 0;
		selectActivated = false;
		switch (id)
		{
		case 0:
			if (index == Vector2.left || index == Vector2.right)
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				if (Util.GameManager().NumItemFreeSpace() == 0)
				{
					txt.CreateBox(new string[1] { "* (You're carrying too much.)" });
					break;
				}
				banana = index == Vector2.right;
				selectID = 1;
				txt.CreateBox(new string[2]
				{
					Items.ItemDescription(banana ? 29 : 30),
					string.Format("* (Costs {0}G.)\n* (Will you pay?)", banana ? 35 : 20)
				}, false);
				txt.EnableSelectionAtEnd();
			}
			else
			{
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			}
			break;
		case 1:
			if (index == Vector2.left || index == Vector2.right)
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				int id2 = (banana ? 29 : 30);
				int num = (banana ? 35 : 20);
				string text = string.Format("* You took the {0} without\n  paying.", banana ? "Banana" : "Boiled Egg");
				if (index == Vector2.left)
				{
					if (Util.GameManager().GetGold() == 0)
					{
						text = string.Format("* You didn't have any gold,^05\n  so you took the {0}\n  anyway.", banana ? "Banana" : "Boiled Egg");
					}
					else if (Util.GameManager().GetGold() < num)
					{
						text = string.Format("* You didn't have enough gold,^05\n  so you left {0}G and took\n  the {1} anyway.", Util.GameManager().GetGold(), banana ? "Banana" : "Boiled Egg");
						Util.GameManager().SetGold(0);
					}
					else
					{
						text = string.Format("* You bought the {0}.", banana ? "Banana" : "Boiled Egg");
						Util.GameManager().RemoveGold(num);
					}
				}
				Util.GameManager().AddItem(id2);
				shopBG.transform.Find("SpaceInfo").GetComponent<Text>().text = "$ - " + Util.GameManager().GetGold() + "G\nSPACE - " + (8 - Util.GameManager().NumItemFreeSpace()) + "/8";
				txt.CreateBox(new string[1] { text });
			}
			else
			{
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			}
			break;
		}
	}
}
