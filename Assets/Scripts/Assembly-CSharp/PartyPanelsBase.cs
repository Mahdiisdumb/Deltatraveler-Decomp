using UnityEngine;
using UnityEngine.UI;

public abstract class PartyPanelsBase : UIComponent
{
	protected struct Panel
	{
		public GameObject statPanel;

		public Image statBorder;

		public Image[] roundBorders;

		public RectTransform hpBar;

		public Text hpText;

		public Text memberText;

		public Image memberSprite;

		public bool isActive;

		public int xPos;

		public int hp;

		public int revivalTurn;

		public bool raiseHead;

		public bool target;

		public bool defending;

		public bool ignoreChanges;
	}

	protected GameManager gm;

	protected bool hd;

	protected Panel[] panels = new Panel[6];

	protected virtual void Awake()
	{
		gm = Util.GameManager();
		for (int i = 0; i < panels.Length; i++)
		{
			InitializePanel(i);
		}
		SetXPositions();
	}

	protected virtual void InitializePanel(int i, bool overworld = false)
	{
		int partyMember = gm.GetPartyMember(i);
		panels[i].isActive = partyMember > -1;
		panels[i].hp = gm.GetHP(i);
		panels[i].statPanel = base.transform.Find("Party" + i + "Stats").gameObject;
		panels[i].statBorder = panels[i].statPanel.GetComponent<Image>();
		panels[i].statBorder.enabled = gm.GetFlagInt(94) == 0;
		panels[i].statBorder.color = GetDefaultColor(i);
		panels[i].hpBar = panels[i].statBorder.transform.Find("Stats").Find("HPFG").GetComponent<RectTransform>();
		panels[i].hpText = panels[i].statBorder.transform.Find("Stats").Find("HPTEXT").GetComponent<Text>();
		panels[i].memberText = panels[i].statBorder.transform.Find("Stats").Find("name").GetComponent<Text>();
		panels[i].memberText.text = PartyMembers.GetMemberName(partyMember).ToLower();
		if (i < 3)
		{
			float num = 0f;
			if (overworld)
			{
				num = (panels[i].memberText.text.Length - 4) / 2 * -2;
			}
			float num2 = (overworld ? (-50f) : (-52.5f));
			float num3 = ((panels[i].memberText.text.Length < 6) ? (-56) : (-52));
			if (overworld)
			{
				num3 = -56 + (panels[i].memberText.text.Length - 4) * 5;
			}
			float num4 = panels[i].memberText.text.Length * 6;
			float y = ((!overworld) ? 1 : 0);
			panels[i].hpBar.localPosition = new Vector3(num2 + num4 + num, y);
			panels[i].statBorder.transform.Find("Stats").Find("HPBG").transform.localPosition = panels[i].hpBar.localPosition;
			float num5 = 1f + num4;
			if (overworld)
			{
				num5 = num4 + 4f - (float)((panels[i].memberText.text.Length - 4) * 4);
			}
			if (num5 > 34f)
			{
				num5 = 34f;
			}
			float y2 = (overworld ? (-7) : (-4));
			panels[i].hpText.transform.localPosition = new Vector3(num5 + num, y2);
			panels[i].memberText.transform.localPosition = new Vector3(num3 + num, y2);
		}
		panels[i].roundBorders = new Image[6];
		int num6 = 0;
		Image[] componentsInChildren = panels[i].statBorder.transform.Find("roundedges").GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren)
		{
			image.enabled = gm.GetFlagInt(94) == 1;
			panels[i].roundBorders[num6] = image;
			num6++;
		}
		componentsInChildren = panels[i].statBorder.transform.Find("roundcorners").GetComponentsInChildren<Image>();
		foreach (Image image2 in componentsInChildren)
		{
			image2.enabled = gm.GetFlagInt(94) == 1;
			panels[i].roundBorders[num6] = image2;
			num6++;
		}
		componentsInChildren = panels[i].statBorder.transform.Find("Stats").GetComponentsInChildren<Image>();
		foreach (Image image3 in componentsInChildren)
		{
			if (gm.GetFlagInt(94) == 1)
			{
				if (!image3.enabled)
				{
					image3.enabled = true;
				}
				if (image3.gameObject.name == "HPFG")
				{
					image3.color = new Color(0f, 1f, 0f);
				}
			}
		}
		panels[i].statPanel.SetActive(panels[i].isActive);
		if (overworld)
		{
			panels[i].statPanel.transform.localPosition = new Vector3(panels[i].statPanel.transform.localPosition.x, -279f);
		}
	}

	public virtual void UpdateHP(int[] hp)
	{
	}

	public void SetXPositions()
	{
		int[] activePartySlots = gm.GetActivePartySlots();
		if (activePartySlots.Length > 1)
		{
			int num = ((NumOfActivePartyMembers() == 2) ? (-130) : (-190));
			int num2 = ((NumOfActivePartyMembers() == 2) ? 260 : 190);
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

	public void SetXOffset(int i, int x)
	{
		panels[i].xPos = x;
	}

	protected void UpdateRoundedBorderColor(int i)
	{
		Image[] roundBorders = panels[i].roundBorders;
		for (int j = 0; j < roundBorders.Length; j++)
		{
			roundBorders[j].color = panels[i].statBorder.color;
		}
	}

	public int NumOfActivePartyMembers()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (panels[i].isActive)
			{
				num++;
			}
		}
		return num;
	}

	public void Reinitialize()
	{
		Awake();
	}

	public Color GetDefaultColor(int i)
	{
		return PartyMembers.GetMemberNeonColor(gm.GetPartyMember(i));
	}

	public Transform GetStatPanel(int i)
	{
		return panels[i].statPanel.transform;
	}
}
