using UnityEngine;

public class BattleButton : MonoBehaviour
{
	[SerializeField]
	private string type;

	private bool isSelected;

	private string suffix = "";

	private bool isSelectable = true;

	private Color color = new Color32(byte.MaxValue, 127, 39, byte.MaxValue);

	private Color selColor = new Color(1f, 1f, 0f);

	public static readonly Color[] BUTTON_COLORS = new Color[12]
	{
		new Color32(byte.MaxValue, 127, 39, byte.MaxValue),
		new Color32(0, 216, 140, byte.MaxValue),
		new Color32(byte.MaxValue, 0, 89, byte.MaxValue),
		new Color32(byte.MaxValue, 127, 39, byte.MaxValue),
		new Color32(206, 102, 33, byte.MaxValue),
		new Color32(82, 116, byte.MaxValue, byte.MaxValue),
		new Color32(byte.MaxValue, 51, 73, byte.MaxValue),
		new Color32(74, 175, 74, byte.MaxValue),
		new Color32(143, 146, 222, byte.MaxValue),
		Color.white,
		new Color32(84, 84, 124, byte.MaxValue),
		new Color32(165, 38, 38, byte.MaxValue)
	};

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(94) == 1)
		{
			suffix = "_ts";
			isSelected = true;
			color = new Color32(170, 114, 190, byte.MaxValue);
			selColor = new Color32(211, 142, 232, byte.MaxValue);
		}
		int num = (int)Util.GameManager().GetFlag(223);
		if (num > 0)
		{
			color = BUTTON_COLORS[num];
			selColor = Selection.SELECTION_COLORS[num];
			if (num == 9)
			{
				color = Selection.SELECTION_COLORS[num];
				selColor = BUTTON_COLORS[num];
			}
		}
		isSelected = false;
		UpdateSprite();
	}

	private void Start()
	{
		if (type == "item" && !Util.FindObjectOfType<UnoBattleManager>() && Util.GameManager().NumItemFreeSpace(equipment: true) + Util.GameManager().NumItemFreeSpace(equipment: false) == 16)
		{
			isSelectable = false;
		}
	}

	public void Select(bool boo)
	{
		if (boo && !isSelected)
		{
			if ((bool)Util.FindObjectOfType<UnoBattleManager>())
			{
				Util.FindObjectOfType<UnoBattleManager>().ButtonSFX();
			}
			else
			{
				Util.FindObjectOfType<BattleManager>().ButtonSFX();
			}
			isSelected = true;
		}
		else if (!boo && isSelected)
		{
			isSelected = false;
		}
		UpdateSprite();
	}

	public void ChangeButtonType(string type)
	{
		if (this.type != type)
		{
			this.type = type;
			UpdateSprite();
		}
	}

	public void ChangeButtonSuffix(string suffix)
	{
		if (this.suffix != suffix)
		{
			this.suffix = suffix;
			UpdateSprite();
		}
	}

	public string GetButtonType()
	{
		return type;
	}

	private void UpdateSprite()
	{
		if (isSelected)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/spr_" + type + "bt_1" + suffix);
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/spr_" + type + "bt_0" + suffix);
		}
		UpdateColor();
	}

	private void UpdateColor()
	{
		float num = (isSelectable ? 1f : 0.5f);
		if (isSelected)
		{
			GetComponent<SpriteRenderer>().color = new Color(selColor.r * num, selColor.g * num, selColor.b * num, GetComponent<SpriteRenderer>().color.a);
		}
		else
		{
			GetComponent<SpriteRenderer>().color = new Color(color.r * num, color.g * num, color.b * num, GetComponent<SpriteRenderer>().color.a);
		}
	}

	public void SetUnselectableColor()
	{
		isSelectable = false;
		UpdateColor();
	}

	public void SetSelectableColor()
	{
		isSelectable = true;
		UpdateColor();
	}
}
