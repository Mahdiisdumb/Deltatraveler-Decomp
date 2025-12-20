using UnityEngine;
using UnityEngine.SceneManagement;

public class ActionPartyPanels : PartyPanelsBase
{
	private bool activated;

	private bool raised;

	private int activeFrames;

	private bool ts;

	private bool owMenuLowPosition;

	protected override void Awake()
	{
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			hd = true;
		}
		gm = Util.GameManager();
		for (int i = 0; i < panels.Length; i++)
		{
			InitializePanel(i, overworld: true);
		}
		SetXPositionsOW();
		ts = Util.GameManager().GetFlagInt(94) == 1;
		UpdateHP(Util.GameManager().GetHPArray());
	}

	private void Update()
	{
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].isActive)
			{
				int num = ((!raised) ? (-279) : (owMenuLowPosition ? (-219) : (-205)));
				if (hd)
				{
					num += 2;
				}
				if (i >= 3 && raised)
				{
					num += 30;
				}
				panels[i].statPanel.transform.localPosition = Vector3.Lerp(panels[i].statPanel.transform.localPosition, new Vector3(panels[i].xPos, num), 0.5f);
				if (hd)
				{
					base.transform.Find("Outlines").GetChild(i).localPosition = Vector3.Lerp(base.transform.Find("Outlines").GetChild(i).localPosition, new Vector3(panels[i].xPos, num + 279), 0.5f);
				}
			}
		}
		if (raised && !activated)
		{
			activeFrames++;
			if (activeFrames == 45)
			{
				Lower();
			}
		}
	}

	public override void UpdateHP(int[] hp)
	{
		int num = 20;
		int num2 = 42;
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].isActive)
			{
				int num3 = hp[i];
				_ = ref panels[i];
				int maxHP = gm.GetMaxHP(i);
				_ = i % 3;
				_ = i % 3;
				if (i < 3)
				{
					panels[i].statBorder.transform.Find("Stats").Find("HPBG").GetComponent<RectTransform>()
						.sizeDelta = new Vector2((maxHP >= 100) ? num : num2, 10f);
				}
				panels[i].hpText.text = num3.ToString((maxHP >= 100) ? "D3" : "D2") + "/" + maxHP.ToString((maxHP >= 100) ? "D3" : "D2");
				int num4 = Mathf.RoundToInt((float)num3 / (float)maxHP * panels[i].statBorder.transform.Find("Stats").Find("HPBG").GetComponent<RectTransform>()
					.sizeDelta.x);
				if (num4 < 1 && num3 > 0)
				{
					num4 = 1;
				}
				panels[i].hpBar.sizeDelta = new Vector2(num4, 10f);
				if ((float)num3 < (float)maxHP / 4f)
				{
					panels[i].hpText.color = new Color(1f, 1f, 0f);
				}
				else
				{
					panels[i].hpText.color = Color.white;
				}
				if (num3 <= 0)
				{
					panels[i].memberText.color = Color.grey;
					panels[i].statBorder.color = Color.grey;
					UpdateRoundedBorderColor(i);
					panels[i].hpText.color = Color.red;
				}
			}
		}
	}

	public void SetXPositionsOW()
	{
		int[] activePartySlots = gm.GetActivePartySlots();
		if (activePartySlots.Length > 1)
		{
			int num = ((NumOfActivePartyMembers() == 2) ? (-130) : (hd ? (-200) : (-196)));
			int num2 = ((NumOfActivePartyMembers() == 2) ? 260 : (hd ? 200 : 196));
			for (int i = 0; i < activePartySlots.Length; i++)
			{
				panels[activePartySlots[i]].xPos = num + num2 * i;
			}
		}
		else
		{
			panels[0].xPos = 0;
		}
		for (int j = 0; j < 3; j++)
		{
			panels[j + 3].xPos = panels[j].xPos;
		}
		for (int k = 0; k < panels.Length; k++)
		{
			if (panels[k].isActive)
			{
				panels[k].statPanel.transform.localPosition = new Vector3(panels[k].xPos, panels[k].statPanel.transform.localPosition.y);
				if ((bool)panels[k].memberSprite)
				{
					panels[k].memberSprite.transform.localPosition = new Vector3(panels[k].xPos, panels[k].memberSprite.transform.localPosition.y);
				}
			}
		}
	}

	public void SetActivated(bool activated)
	{
		this.activated = activated;
		activeFrames = 0;
	}

	public void Raise()
	{
		raised = true;
	}

	public void Lower()
	{
		raised = false;
		activeFrames = 0;
	}

	public void UseLowerPosition()
	{
		owMenuLowPosition = true;
	}
}
