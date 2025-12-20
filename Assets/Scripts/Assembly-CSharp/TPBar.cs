using UnityEngine;
using UnityEngine.UI;

public class TPBar : MonoBehaviour
{
	private int tp;

	private int[] tpToUse = new int[3];

	private bool[] tpToGain = new bool[3];

	private int tpPreview;

	private RectTransform tpBar;

	private RectTransform useBar;

	private Text tpText;

	private Text tpTextShadow;

	private bool disabled;

	private void Awake()
	{
		tpBar = base.transform.Find("TPFG").GetComponent<RectTransform>();
		useBar = tpBar.Find("TPUSE").GetComponent<RectTransform>();
		tpText = base.transform.Find("TPTEXT").GetComponent<Text>();
		tpTextShadow = base.transform.Find("TPTEXT-Shadow").GetComponent<Text>();
		tpBar.sizeDelta = new Vector2(20f, 0f);
		tpText.text = "0%";
		tpTextShadow.text = "0%";
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 1)
		{
			Image[] componentsInChildren = base.transform.Find("roundcorners").GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
		}
	}

	private void Update()
	{
		if (!disabled)
		{
			int calculatedTP = GetCalculatedTP();
			tpBar.sizeDelta = Vector2.Lerp(tpBar.sizeDelta, new Vector2(20f, (float)calculatedTP * 1.5f), 0.5f);
			if (calculatedTP == 100 && tpText.text != "MAX")
			{
				tpBar.GetComponent<Image>().color = new Color(1f, 1f, 0f);
				tpText.text = "MAX";
				tpTextShadow.text = "MAX";
				tpText.color = new Color(1f, 1f, 0f);
			}
			else if (calculatedTP < 100 && tpText.text != calculatedTP + "%")
			{
				tpBar.GetComponent<Image>().color = new Color32(byte.MaxValue, 160, 64, byte.MaxValue);
				tpText.text = calculatedTP + "%";
				tpTextShadow.text = calculatedTP + "%";
				tpText.color = Color.white;
			}
		}
	}

	public void Disable()
	{
		disabled = true;
	}

	public void UpdateTPPreviewBar(int tpPreview)
	{
		int calculatedTP = GetCalculatedTP();
		this.tpPreview = tpPreview;
		useBar.sizeDelta = new Vector2(20f, (float)((calculatedTP > tpPreview) ? tpPreview : calculatedTP) * 1.5f);
	}

	public void ApplyPreviewTP(int partyMember)
	{
		tpToUse[partyMember] = tpPreview;
		UpdateTPPreviewBar(0);
	}

	public void SetSpecificTPUse(int partyMember, int tpToUse)
	{
		this.tpToUse[partyMember] = tpToUse;
	}

	public void SetDefendingMember(int partyMember, bool tpToGain)
	{
		this.tpToGain[partyMember] = tpToGain;
	}

	public void UseTP()
	{
		for (int i = 0; i < 3; i++)
		{
			int num = 16;
			if (i == 0 && Object.FindObjectOfType<GameManager>().GetMiniPartyMember() > 0 && Object.FindObjectOfType<GameManager>().KrisInControl())
			{
				num = 24;
			}
			if (tpToGain[i])
			{
				AddTP(num);
			}
			tpToGain[i] = false;
			tp -= tpToUse[i];
			tpToUse[i] = 0;
		}
	}

	public void AddTP(int tp)
	{
		this.tp += tp;
		if (this.tp > 100)
		{
			this.tp = 100;
		}
	}

	public void RemoveTP(int tp)
	{
		this.tp -= tp;
		if (this.tp < 0)
		{
			this.tp = 0;
		}
	}

	public bool ValidTPAmount()
	{
		if (tpPreview <= GetCalculatedTP())
		{
			return true;
		}
		return false;
	}

	public int GetCalculatedTP()
	{
		int num = tp;
		for (int i = 0; i < 3; i++)
		{
			if (tpToGain[i])
			{
				int num2 = 16;
				if (i == 0 && Object.FindObjectOfType<GameManager>().GetMiniPartyMember() > 0 && Object.FindObjectOfType<GameManager>().KrisInControl())
				{
					num2 = 24;
				}
				num += num2;
				if (num > 100)
				{
					num = 100;
				}
			}
			else
			{
				num -= tpToUse[i];
			}
		}
		return num;
	}

	public int GetCurrentTP()
	{
		return tp;
	}
}
