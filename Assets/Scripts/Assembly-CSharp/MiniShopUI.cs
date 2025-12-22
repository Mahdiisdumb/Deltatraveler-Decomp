using UnityEngine;
using UnityEngine.UI;

public class MiniShopUI : MonoBehaviour
{
	[SerializeField]
	private Sprite itemIcon;

	[SerializeField]
	private Sprite weaponIcon;

	[SerializeField]
	private Sprite armorIcon;

	private GameManager gm;

	private bool isEquip;

	private void Awake()
	{
		gm = Util.GameManager();
		if (Util.OverworldPlayer().transform.position.x - Util.FindObjectOfType<CameraController>().transform.position.x > 0f)
		{
			base.transform.localPosition = new Vector3(-187f, 2f);
		}
		UIBackground uIBackground = new GameObject("MiniShopGenBG").AddComponent<UIBackground>();
		uIBackground.transform.SetParent(base.transform.parent);
		uIBackground.CreateElement("menu", base.transform.localPosition, GetComponent<RectTransform>().sizeDelta);
		uIBackground.transform.parent = base.transform;
		uIBackground.transform.SetAsFirstSibling();
		UpdateText();
	}

	public void UpdateText()
	{
		int num = 8 - gm.NumItemFreeSpace(isEquip);
		int gold = gm.GetGold();
		base.transform.Find("Money").GetComponent<Text>().text = $"$ - {gold}G";
		base.transform.Find("Space").GetComponent<Text>().text = $"SPACE - {num}/8";
	}

	public void SetInventoryType(bool isEquip, bool isWeapon = false)
	{
		this.isEquip = isEquip;
		if (!isEquip)
		{
			base.transform.Find("Icon").GetComponent<Image>().sprite = itemIcon;
		}
		else if (isWeapon)
		{
			base.transform.Find("Icon").GetComponent<Image>().sprite = weaponIcon;
		}
		else
		{
			base.transform.Find("Icon").GetComponent<Image>().sprite = armorIcon;
		}
		UpdateText();
	}

	public void SetInventoryType(int item)
	{
		SetInventoryType(Items.IsEquipment(item), Items.ItemType(item) == 1);
	}
}
