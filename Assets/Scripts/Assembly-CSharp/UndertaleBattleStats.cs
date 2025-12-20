using UnityEngine;
using UnityEngine.UI;

public class UndertaleBattleStats : MonoBehaviour
{
	private Text hpText;

	private Text nameText;

	private Image hpBG;

	private Image hpFG;

	private void Awake()
	{
		hpText = base.transform.Find("HPTEXT").GetComponent<Text>();
		nameText = base.transform.Find("NameText").GetComponent<Text>();
		hpBG = base.transform.Find("HPBG").GetComponent<Image>();
		hpFG = base.transform.Find("HPFG").GetComponent<Image>();
		UpdateInfo();
		Update();
	}

	private void Update()
	{
		hpFG.rectTransform.sizeDelta = new Vector3(Mathf.RoundToInt(1.25f * (float)Util.GameManager().GetHP(0)), 21f);
		hpText.text = string.Format("{0} / {1}", Util.GameManager().GetHP(0).ToString("D2"), Util.GameManager().GetMaxHP(0).ToString("D2"));
	}

	public void UpdateInfo()
	{
		float num = Mathf.RoundToInt(1.25f * (float)Util.GameManager().GetMaxHP(0));
		nameText.text = $"{PartyMembers.GetMemberName(Util.GameManager().GetPartyMember(0))}   lv {Util.GameManager().GetLV()}";
		hpBG.rectTransform.sizeDelta = new Vector3(num, 21f);
		hpText.transform.localPosition = new Vector3(94f + num, -175f);
	}
}
