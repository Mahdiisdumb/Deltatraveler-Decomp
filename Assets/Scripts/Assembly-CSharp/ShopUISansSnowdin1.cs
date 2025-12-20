using UnityEngine;
using UnityEngine.UI;

public class ShopUISansSnowdin1 : ShopUISansBase
{
	protected override void Awake()
	{
		base.Awake();
		if ((int)Util.GameManager().GetFlag(87) >= 8)
		{
			item1price *= 4;
			item2price *= 2;
			item3price += 10;
			item4price += 10;
		}
		if (Util.GameManager().GetCurrentZone() == 98)
		{
			base.transform.Find("Background").GetComponent<Image>().sprite = Resources.Load<Sprite>("ui/shop/sans/spr_tundrashop_bg_1");
			Object.Instantiate(Resources.Load<GameObject>("ui/shop/sans/WallHoleShop"), base.transform.Find("Background")).transform.localPosition = Vector3.zero;
		}
		if (Util.GameManager().GetCurrentZone() == 107)
		{
			item1price /= 2;
			item2price /= 4;
			item3price /= 2;
			item4price /= 2;
		}
	}
}
