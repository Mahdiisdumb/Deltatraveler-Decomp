using UnityEngine;

public class PicnicBasket : InteractSelectionBase
{
	private bool banana;

	private const int BANANA_PRICE = 35;

	private const int EGG_PRICE = 20;

	private bool eggSoundPlayed;

	protected MiniShopUI shopBG;

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
		else if ((bool)txt && disabled && txt.GetCurrentStringNum() == 3 && !eggSoundPlayed && Util.GameManager().GetFlagInt(286) == 0)
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
			if (Util.GameManager().GetFlagInt(286) == 1)
			{
				if (Util.GameManager().NumItemFreeSpace(equipment: true) == 0)
				{
					txt.CreateBox(new string[4] { "* (It appears someone has\n  taken all the food.)", "* (...)", "* (You wanted to place the Egg\n  that you're carrying here...)", "* (But you felt that you should\n  make room in your EQUIPMENT\n  first.)" });
				}
				else
				{
					Util.GameManager().SetFlag(286, 0);
					txt.CreateBox(new string[5] { "* (It appears someone has\n  taken all the food.)", "* (...)", "* (You put the Egg in the\n  empty egg basket.)", "* (Strangely,^05 you noticed a shiny\n  bat somehow hiding behind it.)", "* (You got the Aluminum Bat.)" });
					Util.GameManager().AddEquipment(31);
				}
			}
			else
			{
				txt.CreateBox(new string[1] { "* (It appears someone has\n  taken all the food.)" });
			}
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		else
		{
			if (!txt && enabled)
			{
				shopBG = Object.Instantiate(Resources.Load<GameObject>("ui/MiniShopUI"), GameObject.Find("Canvas").transform).GetComponent<MiniShopUI>();
			}
			base.DoInteract();
		}
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
				if (Util.GameManager().NumItemFreeSpace(equipment: false) == 0)
				{
					txt.CreateBox(new string[1] { "* (You don't have enough space\n  in your ITEMs.)" });
					break;
				}
				banana = index == Vector2.right;
				selectID = 1;
				txt.CreateBox(new string[2]
				{
					Items.ItemDescription(banana ? 29 : 30),
					$"* (Costs {(banana ? 35 : 20)}G.)\n* (Will you pay?)"
				}, giveBackControl: false);
				txt.EnableSelectionAtEnd();
			}
			else
			{
				Util.GameManager().EnablePlayerMovement();
			}
			break;
		case 1:
			if (index == Vector2.left || index == Vector2.right)
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				int item = (banana ? 29 : 30);
				int num = (banana ? 35 : 20);
				string text = string.Format("* You took the {0} without\n  paying.\n* It was added to your ITEMs.", banana ? "Banana" : "Boiled Egg");
				string text2 = "* Perhaps you can guess which\n  inventory it was added to.";
				bool flag = false;
				if (index == Vector2.left)
				{
					if (Util.GameManager().GetGold() == 0)
					{
						text = string.Format("* You didn't have any gold,^05\n  so you took the {0}\n  anyway.", banana ? "Banana" : "Boiled Egg");
						flag = true;
					}
					else if (Util.GameManager().GetGold() < num)
					{
						text = string.Format("* You didn't have enough gold,^05\n  so you left {0}G and took\n  the {1} anyway.", Util.GameManager().GetGold(), banana ? "Banana" : "Boiled Egg");
						Util.GameManager().SetGold(0);
						flag = true;
					}
					else
					{
						text = string.Format("* You bought the {0}.\n* It was added to your ITEMs.", banana ? "Banana" : "Boiled Egg");
						Util.GameManager().RemoveGold(num);
					}
				}
				Util.GameManager().AddItem(item);
				shopBG.UpdateText();
				if (flag)
				{
					txt.CreateBox(new string[2] { text, text2 });
				}
				else
				{
					txt.CreateBox(new string[1] { text });
				}
			}
			else
			{
				Util.GameManager().EnablePlayerMovement();
			}
			break;
		}
	}
}
