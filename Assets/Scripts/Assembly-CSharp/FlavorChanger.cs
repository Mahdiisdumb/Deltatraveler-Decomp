using UnityEngine;
using UnityEngine.UI;

public class FlavorChanger : UIComponent
{
	private int oldChoice;

	private int index;

	private int limit = 12;

	private bool holdingAxis;

	private Transform list;

	private Transform preview;

	private void Awake()
	{
		list = base.transform.Find("List");
		preview = base.transform.Find("Preview");
		index = (int)Util.GameManager().GetFlag(223);
		oldChoice = index;
		if ((int)Util.GameManager().GetFlag(13) < 7 && !Util.GameManager().IsTestMode())
		{
			Object.Destroy(list.Find("11").gameObject);
			limit = 11;
			if (index == 11)
			{
				index = 0;
			}
		}
		if (UTInput.GetAxis("Vertical") != 0f)
		{
			holdingAxis = true;
		}
		list.Find("SOUL").GetComponent<Image>().color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
		UpdateUI();
	}

	private void Update()
	{
		if (UTInput.GetAxis("Vertical") != 0f && !holdingAxis)
		{
			index -= (int)UTInput.GetAxis("Vertical");
			if (index >= limit)
			{
				index = 0;
			}
			else if (index < 0)
			{
				index = limit - 1;
			}
			UpdateUI();
			holdingAxis = true;
		}
		else if (UTInput.GetAxis("Vertical") == 0f && holdingAxis)
		{
			holdingAxis = false;
		}
		if (UTInput.GetButtonDown("Z"))
		{
			Util.GameManager().SetFlag(223, index);
			TextBox component = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			string[] array = new string[2] { "* What a nice color choice!", "* You have a wonderful rest\n  of your day!" };
			if (oldChoice == index)
			{
				array = new string[2]
				{
					"* No change?^05\n* That's perfectly fine.",
					(index == 11) ? "* Have a wonderful rest of your\n  day,^05 I suppose..." : "* You have a wonderful rest\n  of your day!"
				};
			}
			else if (index > 0 && Util.GameManager().GetEBTextColorID() == index)
			{
				array = new string[3]
				{
					array[0],
					"* It feels right at home\n  with your name.",
					array[1]
				};
			}
			else if (index == 11)
			{
				array = new string[3] { "* What an...^05 interesting color\n  choice.", "* I could've sworn I got rid\n  of that one a long time\n  ago...", "* Well,^05 have a wonderful rest\n  of your day,^05 I suppose..." };
			}
			component.CreateBox(array, true);
			Object.FindObjectOfType<FlavorChangerNPC>().SetTalkable(component);
			Object.Destroy(base.gameObject);
		}
	}

	private void UpdateUI()
	{
		list.Find("SOUL").transform.localPosition = new Vector3(-123f, 160 - 30 * index);
		list.GetComponent<Image>().color = UIBackground.borderColors[index];
		preview.GetComponent<Image>().color = UIBackground.borderColors[index];
		preview.Find("SelText").GetComponent<Text>().color = Selection.selectionColors[index];
		preview.Find("TestButton").GetComponent<Image>().color = BattleButton.buttonColors[index];
		preview.Find("TestButtonSel").GetComponent<Image>().color = Selection.selectionColors[index];
	}
}
