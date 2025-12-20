using UnityEngine;
using UnityEngine.UI;

public class FrostedBox : MonoBehaviour
{
	private static readonly string[] SOUL_SPRITES = new string[2] { "overworld/spr_soul_ow", "ui/frostedicons/spr_soul_ow_small" };

	private TextBox textBox;

	private Transform objects;

	private Image nameBox;

	private Text nameText;

	private Image continuePrompt;

	private int continueFrames;

	private Image itemIcon;

	private Image cellIcons;

	private Text contactText;

	private bool forceName;

	private void Update()
	{
		if (!textBox)
		{
			return;
		}
		if ((bool)textBox.GetTextUT())
		{
			objects.SetAsLastSibling();
			continuePrompt.enabled = !textBox.GetTextUT().IsPlaying() && !textBox.CanLoadSelection();
			continuePrompt.sprite = Resources.Load<Sprite>(SOUL_SPRITES[continueFrames / 10 % 2]);
			if (continuePrompt.enabled)
			{
				continueFrames++;
			}
			else
			{
				continueFrames = 0;
			}
		}
		else
		{
			continuePrompt.enabled = false;
		}
		if (!forceName)
		{
			if ((bool)textBox.GetPortrait())
			{
				string portraitName = GetPortraitName(textBox.GetPortrait().GetCurrentPortrait());
				SetName(portraitName);
			}
			else
			{
				SetName("");
			}
		}
		if (itemIcon.enabled && textBox.GetCurrentStringNum() > 1)
		{
			itemIcon.enabled = false;
		}
	}

	public void Create(TextBox textBox)
	{
		this.textBox = textBox;
		objects = Object.Instantiate(Resources.Load<GameObject>("ui/FrostedBoxObjects")).transform;
		objects.transform.SetParent(base.transform, worldPositionStays: true);
		objects.transform.localPosition = textBox.GetTextPos();
		objects.transform.localScale = Vector3.one;
		nameBox = objects.Find("Name").GetComponent<Image>();
		nameText = nameBox.GetComponentInChildren<Text>();
		continuePrompt = objects.Find("Continue").GetComponent<Image>();
		continuePrompt.color = SOUL.GetSOULColorByID(Util.GameManager().GetFlagInt(312));
		if (textBox.GetFrostedOffset() == 2)
		{
			continuePrompt.transform.localPosition += new Vector3(0f, 6f);
		}
		itemIcon = objects.Find("ItemIcon").GetComponent<Image>();
		cellIcons = objects.Find("CellIcons").GetComponent<Image>();
		contactText = objects.Find("ContactText").GetComponent<Text>();
	}

	public void ActivateItemIcon(int itemInd, int itemCategory)
	{
		itemIcon.enabled = true;
		Sprite sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item" + ((itemCategory == 2) ? "_key_" : "_") + itemInd);
		if (!sprite)
		{
			sprite = Resources.Load<Sprite>("ui/frostedicons/spr_item_0");
		}
		itemIcon.GetComponent<Image>().sprite = sprite;
	}

	public void ActivateCellIcons()
	{
		cellIcons.enabled = true;
	}

	public void SetName(string name, bool force = false)
	{
		forceName = force;
		if (!string.IsNullOrEmpty(name))
		{
			nameBox.enabled = true;
			nameBox.rectTransform.sizeDelta = new Vector3(6 + name.Length * 16, nameBox.rectTransform.sizeDelta.y);
			nameText.text = name;
		}
		else
		{
			nameBox.enabled = false;
			nameText.text = "";
		}
	}

	public void SetContactName(string name)
	{
		forceName = true;
		nameBox.enabled = false;
		contactText.text = name;
	}

	private string GetPortraitName(string spriteName)
	{
		Debug.Log(spriteName);
		if (spriteName.StartsWith("suhd_"))
		{
			return "SUSIE";
		}
		if (spriteName.StartsWith("nohd_"))
		{
			return "NOELLE";
		}
		if (spriteName.StartsWith("toridhd_"))
		{
			return "TORIEL";
		}
		return "???";
	}
}
