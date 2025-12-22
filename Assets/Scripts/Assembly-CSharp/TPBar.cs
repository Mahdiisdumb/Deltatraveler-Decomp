using UnityEngine;
using UnityEngine.UI;

public class TPBar : MonoBehaviour
{
	protected int tp;

	protected int[] tpToUse = new int[3];

	protected bool[] tpToGain = new bool[3];

	protected int tpPreview;

	protected RectTransform tpBar;

	protected RectTransform useBar;

	protected Text tpText;

	protected bool disabled;

	protected virtual void Awake()
	{
		tpBar = base.transform.Find("TPFG").GetComponent<RectTransform>();
		useBar = tpBar.Find("TPUSE").GetComponent<RectTransform>();
		tpText = base.transform.Find("TPTEXT").GetComponent<Text>();
		tpBar.sizeDelta = new Vector2(20f, 0f);
		tpText.text = "0%";
		if ((int)Util.GameManager().GetFlag(94) == 1)
		{
			Image[] componentsInChildren = base.transform.Find("roundcorners").GetComponentsInChildren<Image>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
		}
	}

	protected virtual void Update()
	{
		if (!disabled)
		{
			int calculatedTP = GetCalculatedTP();
			tpBar.sizeDelta = Vector2.Lerp(tpBar.sizeDelta, new Vector2(20f, (float)calculatedTP * 1.5f), 0.5f);
			if (calculatedTP == 100 && tpText.text != "MAX")
			{
				tpBar.GetComponent<Image>().color = new Color(1f, 1f, 0f);
				tpText.text = "MAX";
				tpText.color = new Color(1f, 1f, 0f);
			}
			else if (calculatedTP < 100 && tpText.text != calculatedTP + "%")
			{
				tpBar.GetComponent<Image>().color = new Color32(byte.MaxValue, 160, 64, byte.MaxValue);
				tpText.text = calculatedTP + "%";
				tpText.color = Color.white;
			}
		}
	}

	public void Disable()
	{
		disabled = true;
	}

	public virtual void UpdateTPPreviewBar(int tpPreview)
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
			if (IsSuperDefend(i))
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
				if (IsSuperDefend(i))
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

	private bool IsSuperDefend(int i)
	{
		if (Util.GameManager().GetHP(i) > 0 && Util.GameManager().PartySlotFilled(i + 3))
		{
			return Util.GameManager().GetHP(i + 3) > 0;
		}
		return false;
	}
}
