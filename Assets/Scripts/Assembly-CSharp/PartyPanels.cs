using UnityEngine;
using UnityEngine.UI;

public class PartyPanels : MonoBehaviour
{
	private GameManager gm;

	private bool[] isActive = new bool[4];

	private GameObject[] statPanels = new GameObject[4];

	private Image[] statBorders = new Image[4];

	private Image[][] roundBorders = new Image[4][];

	private RectTransform[] hpBars = new RectTransform[4];

	private Text[] hpText = new Text[4];

	private Text[] memberText = new Text[4];

	private Image[] memberSprite = new Image[4];

	private int[] xPos = new int[4];

	private bool[] raiseHead = new bool[3] { true, false, false };

	private bool defense;

	private int[] hp = new int[3] { 20, 15, 5 };

	private int[] revivalTurns = new int[3];

	private bool[] targets = new bool[3];

	private bool hpCalibrated;

	private bool[] defending = new bool[4];

	private bool miniPartyMember;

	private bool miniPartyMemberDisabled;

	public static readonly Color[] defaultColors = new Color[7]
	{
		new Color(0f, 1f, 1f),
		new Color(1f, 0f, 1f),
		new Color(1f, 1f, 0f),
		Color.red,
		Color.green,
		Color.blue,
		new Color(0f, 1f, 1f)
	};

	private bool manualManipulation;

	private bool friskMode;

	private bool ignoreNextHPModification;

	private int raisedPanel = -1;

	private bool usingKarma;

	private KarmaHandler karmaHandler;

	private void Awake()
	{
		gm = Object.FindObjectOfType<GameManager>();
		miniPartyMember = gm.GetMiniPartyMember() > 0;
		string[] array = new string[4] { "Kris", "Susie", "Noelle", "Mini" };
		if ((int)gm.GetFlag(107) == 1)
		{
			friskMode = true;
		}
		for (int i = 0; i < 4; i++)
		{
			if (i < 3)
			{
				hp[i] = gm.GetHP(i);
				if (i == 0 && miniPartyMember)
				{
					hp[i] = gm.GetHP(i) - gm.GetMiniMemberMaxHP();
				}
				isActive[i] = i == 0 || (i == 1 && gm.SusieInParty()) || (i == 2 && gm.NoelleInParty());
			}
			else
			{
				isActive[i] = miniPartyMember;
			}
			statPanels[i] = base.transform.Find(array[i] + "Stats").gameObject;
			statBorders[i] = statPanels[i].GetComponent<Image>();
			statBorders[i].enabled = (int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 0;
			hpBars[i] = statBorders[i].transform.Find("Stats").Find("HPFG").GetComponent<RectTransform>();
			hpText[i] = statBorders[i].transform.Find("Stats").Find("HPTEXT").GetComponent<Text>();
			memberText[i] = statBorders[i].transform.Find("Stats").Find("name").GetComponent<Text>();
			memberSprite[i] = base.transform.Find(array[i] + "Sprite").GetComponent<Image>();
			memberSprite[i].enabled = false;
			if (friskMode && i == 0)
			{
				if ((int)gm.GetFlag(108) == 1)
				{
					statBorders[i].color = UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
				}
				memberText[i].text = "Frisk";
				SetSprite(0, "spr_fr_down_0");
				for (int j = 0; j < statBorders[i].transform.Find("Stats").childCount; j++)
				{
					Transform child = statBorders[i].transform.Find("Stats").GetChild(j);
					if (child.name != "name")
					{
						child.transform.localPosition += new Vector3(6f, 0f);
					}
				}
			}
			roundBorders[i] = new Image[6];
			int num = 0;
			Image[] componentsInChildren = statBorders[i].transform.Find("roundedges").GetComponentsInChildren<Image>();
			foreach (Image image in componentsInChildren)
			{
				image.enabled = (int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 1;
				roundBorders[i][num] = image;
				num++;
			}
			componentsInChildren = statBorders[i].transform.Find("roundcorners").GetComponentsInChildren<Image>();
			foreach (Image image2 in componentsInChildren)
			{
				image2.enabled = (int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 1;
				roundBorders[i][num] = image2;
				num++;
			}
			componentsInChildren = statBorders[i].transform.Find("Stats").GetComponentsInChildren<Image>();
			foreach (Image image3 in componentsInChildren)
			{
				if ((int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 1)
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
			if (i == 3 && isActive[i])
			{
				string[] array2 = new string[4] { "Paula", "Chara", "Sans", "Frisk" };
				string[] array3 = new string[4] { "spr_paula_down_0", "spr_ch_down_0", "spr_sans_down_0", "spr_fr_down_0" };
				statBorders[i].transform.Find("Stats").Find("name").GetComponent<Text>()
					.text = array2[gm.GetMiniPartyMember() - 1];
				statBorders[i].color = defaultColors[gm.GetMiniPartyMember() + 2];
				UpdateRoundedBorderColor(i);
				SetSprite(3, array3[gm.GetMiniPartyMember() - 1]);
			}
			statPanels[i].SetActive(isActive[i]);
		}
		SetXPositions();
	}

	private void Update()
	{
		if (manualManipulation)
		{
			return;
		}
		for (int i = 0; i < 4; i++)
		{
			if (!isActive[i])
			{
				continue;
			}
			int num = 0;
			if (i == 3)
			{
				num = (defense ? (-30) : 32);
			}
			int num2 = ((i != 3) ? i : 0);
			int num3 = (defense ? (-159) : (-35)) + num;
			if (raisedPanel == num2 && !defense)
			{
				num3 += 8;
			}
			int num4 = (defense ? (-4) : 4);
			Transform obj = statPanels[i].transform.Find("Stats");
			statPanels[i].transform.localPosition = Vector3.Lerp(statPanels[i].transform.localPosition, new Vector3(xPos[i], num3), 0.5f);
			obj.localPosition = Vector3.Lerp(obj.localPosition, new Vector3(0f, num4), 0.5f);
			if (i == 3)
			{
				num = 20;
			}
			bool flag = true;
			if (i == 0 && miniPartyMember && !miniPartyMemberDisabled)
			{
				if (hp[i] - gm.GetMiniMemberMaxHP() <= 0)
				{
					flag = false;
				}
			}
			else if (i == 3 && miniPartyMemberDisabled)
			{
				flag = false;
			}
			int num5 = ((raiseHead[num2] && !defense && flag) ? (Mathf.CeilToInt(15f + 2.5f * (float)i) * 2 + num) : ((int)statPanels[i].transform.localPosition.y + 25));
			if (raisedPanel == num2 && raiseHead[num2] && flag)
			{
				num5 += 8;
			}
			memberSprite[i].transform.localPosition = Vector3.Lerp(memberSprite[i].transform.localPosition, new Vector3(xPos[i], num5), 0.5f);
			memberSprite[i].enabled = memberSprite[i].transform.localPosition.y > -10f;
		}
	}

	public void SetXPositions()
	{
		int num = NumOfActivePartyMembers();
		int num2 = (gm.SusieInParty() ? 1 : 2);
		if (num > 1)
		{
			int num3 = ((NumOfActivePartyMembers() == 2) ? (-130) : (-190));
			int num4 = ((NumOfActivePartyMembers() == 2) ? 260 : 190);
			for (int i = 0; i < num; i++)
			{
				if (i != 0 && num == 2)
				{
					xPos[num2] = num3 + num4 * i;
				}
				else
				{
					xPos[i] = num3 + num4 * i;
				}
			}
		}
		else
		{
			xPos[0] = 0;
		}
		if (miniPartyMember)
		{
			xPos[3] = xPos[0];
		}
		for (int j = 0; j < 4; j++)
		{
			if (isActive[j])
			{
				statPanels[j].transform.localPosition = new Vector3(xPos[j], statPanels[j].transform.localPosition.y);
				memberSprite[j].transform.localPosition = new Vector3(xPos[j], memberSprite[j].transform.localPosition.y);
			}
		}
	}

	private void UpdateRoundedBorderColor(int i)
	{
		Image[] array = roundBorders[i];
		for (int j = 0; j < array.Length; j++)
		{
			array[j].color = statBorders[i].color;
		}
	}

	public void KarmaTick(int i)
	{
		if (hpCalibrated && isActive[i] && hp[i] > 0)
		{
			hp[i]--;
		}
	}

	public void UnoTick(int hp)
	{
		this.hp[0] = hp;
	}

	public void UpdateHP(int[] hp)
	{
		int[] array = Object.FindObjectOfType<BattleManager>().GetRevivalTurns();
		if (ignoreNextHPModification)
		{
			ignoreNextHPModification = false;
		}
		else
		{
			for (int i = 0; i < 4; i++)
			{
				if (!isActive[i])
				{
					continue;
				}
				int num = 0;
				int num2 = 0;
				int num3 = 0;
				int num4 = i;
				int num5 = i;
				if (i < 3)
				{
					num = hp[i];
					num2 = this.hp[i];
					num3 = gm.GetMaxHP(i);
					if (i == 0 && miniPartyMember && !miniPartyMemberDisabled)
					{
						num -= gm.GetMiniMemberMaxHP();
						num2 -= gm.GetMiniMemberMaxHP();
						if (num < 0)
						{
							num = 0;
						}
						if (num2 < 0)
						{
							num2 = 0;
						}
						num3 -= gm.GetMiniMemberMaxHP();
						num4 = -1;
					}
					statBorders[i].transform.Find("Stats").Find("HPBG").GetComponent<RectTransform>()
						.sizeDelta = new Vector2((num3 >= 100) ? 25 : 45, 10f);
				}
				else
				{
					if (miniPartyMemberDisabled)
					{
						continue;
					}
					num = hp[0];
					if (num > gm.GetMiniMemberMaxHP())
					{
						num = gm.GetMiniMemberMaxHP();
					}
					num2 = (miniPartyMemberDisabled ? num : this.hp[0]);
					if (num2 > gm.GetMiniMemberMaxHP())
					{
						num2 = gm.GetMiniMemberMaxHP();
					}
					num3 = gm.GetMiniMemberMaxHP();
					num4 = 0;
					num5 = 0;
				}
				hpText[i].text = num.ToString((num3 >= 100) ? "D3" : "D2") + "/" + num3.ToString((num3 >= 100) ? "D3" : "D2");
				int num6 = Mathf.RoundToInt((float)num / (float)num3 * statBorders[i].transform.Find("Stats").Find("HPBG").GetComponent<RectTransform>()
					.sizeDelta.x);
				if (num6 < 1 && num > 0)
				{
					num6 = 1;
				}
				hpBars[i].sizeDelta = new Vector2(num6, 10f);
				if ((bool)karmaHandler)
				{
					karmaHandler.ReadjustKarma(i);
				}
				if (defending[num5])
				{
					hpText[i].color = new Color(0f, 1f, 1f);
				}
				else if ((bool)karmaHandler && karmaHandler.GetKarma(i) > 0)
				{
					hpText[i].color = new Color(1f, 0f, 1f);
				}
				else if ((float)num < (float)num3 / 4f)
				{
					hpText[i].color = new Color(1f, 1f, 0f);
				}
				else
				{
					hpText[i].color = Color.white;
				}
				if (num <= 0)
				{
					if (num4 != -1 && array[num4] > 0)
					{
						hpText[i].text = "-" + array[num4] + "/" + num3.ToString("D2");
					}
					memberText[i].color = Color.grey;
					statBorders[i].color = Color.grey;
					UpdateRoundedBorderColor(i);
					hpText[i].color = Color.red;
				}
				else if (memberText[i].color == Color.grey && !defense)
				{
					memberText[i].color = Color.white;
					statBorders[i].color = GetDefaultColor(i);
					UpdateRoundedBorderColor(i);
				}
				if (!hpCalibrated)
				{
					continue;
				}
				Vector3 position = statPanels[i].transform.localPosition / 48f - new Vector3(0f, defense ? 0.4f : (-0.5f));
				if (num > num2)
				{
					DamageNumber component = Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>();
					if (num == num3)
					{
						component.StartWord("max", new Color(0f, 1f, 0f), position);
					}
					else if (num2 <= 0)
					{
						component.StartWord("up", new Color(0f, 1f, 0f), position);
					}
					else
					{
						component.StartNumber(num - num2, new Color(0f, 1f, 0f), position);
					}
				}
				else if (num < num2)
				{
					DamageNumber component2 = Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>();
					if (num <= 0)
					{
						component2.StartWord("down", new Color(1f, 0f, 0f), position);
					}
					else
					{
						component2.StartNumber(num2 - num, GetDefaultColor(i) + Color.white / 2f, position);
					}
				}
				if (num4 != -1 && (array[num4] < revivalTurns[num4] || (array[num4] == 3 && revivalTurns[num4] == 0)) && num == 0)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartNumber(1, new Color(0f, 1f, 0f), position);
				}
			}
		}
		this.hp = (int[])hp.Clone();
		revivalTurns = (int[])array.Clone();
		hpCalibrated = true;
		if (!LivingMembersBeingTargetted() && defense)
		{
			TargetLivingMembers();
		}
	}

	public void SetAsDefending(int i, bool defend)
	{
		if (!isActive[i])
		{
			return;
		}
		defending[i] = defend;
		int num = hp[i];
		int maxHP = gm.GetMaxHP(i);
		if (i == 0 && miniPartyMember && !miniPartyMemberDisabled)
		{
			num -= gm.GetMiniMemberMaxHP();
			if (num < 0)
			{
				num = 0;
			}
			maxHP -= gm.GetMiniMemberMaxHP();
		}
		if (num > 0)
		{
			if (defend)
			{
				hpText[i].color = new Color(0f, 1f, 1f);
			}
			else if ((bool)karmaHandler && karmaHandler.GetKarma(i) > 0)
			{
				hpText[i].color = new Color(1f, 0f, 1f);
			}
			else if ((float)hp[i] < (float)gm.GetMaxHP(i) / 4f)
			{
				hpText[i].color = new Color(1f, 1f, 0f);
			}
			else
			{
				hpText[i].color = Color.white;
			}
		}
		if (i == 0 && miniPartyMember && !miniPartyMemberDisabled && num + gm.GetMiniMemberMaxHP() > 0)
		{
			defending[3] = defend;
			if (defend)
			{
				hpText[3].color = new Color(0f, 1f, 1f);
			}
			else if ((float)hp[0] < (float)gm.GetMiniMemberMaxHP() / 4f)
			{
				hpText[3].color = new Color(1f, 1f, 0f);
			}
			else
			{
				hpText[3].color = Color.white;
			}
		}
	}

	public Transform GetStatPanel(int i)
	{
		return statPanels[i].transform;
	}

	public Color GetDefaultColor(int i)
	{
		if (i == 0 && friskMode && (int)gm.GetFlag(108) == 1)
		{
			return UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
		}
		return defaultColors[i];
	}

	public void TargetLivingMembers()
	{
		if (!defense)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			if (hp[i] > 0 && !targets[i] && isActive[i])
			{
				targets[i] = true;
			}
		}
		if (LivingMembersBeingTargetted())
		{
			SetTargets(targets[0], targets[1], targets[2], defense);
		}
	}

	public void SetTargets(bool kris, bool susie, bool noelle, bool activateDefense = true)
	{
		defense = activateDefense;
		targets = new bool[3] { kris, susie, noelle };
		for (int i = 0; i < 4; i++)
		{
			if (!isActive[i])
			{
				continue;
			}
			int num = 0;
			int num2 = i;
			if (i < 3)
			{
				num = hp[i];
				if (i == 0 && miniPartyMember && !miniPartyMemberDisabled)
				{
					num -= gm.GetMiniMemberMaxHP();
				}
			}
			else
			{
				num = hp[0];
				num2 = 0;
			}
			if (num > 0)
			{
				if (targets[num2])
				{
					memberText[i].color = Color.white;
					statBorders[i].color = GetDefaultColor(i);
					UpdateRoundedBorderColor(i);
				}
				else
				{
					Color color = GetDefaultColor(i) * 0.3f + new Color(0.2f, 0.2f, 0.2f);
					color.a = 1f;
					memberText[i].color = new Color(0.5f, 0.5f, 0.5f);
					statBorders[i].color = color;
					UpdateRoundedBorderColor(i);
				}
			}
		}
		if (!LivingMembersBeingTargetted())
		{
			TargetLivingMembers();
		}
	}

	public void DeactivateTargets()
	{
		defense = false;
		for (int i = 0; i < 4; i++)
		{
			if (!isActive[i])
			{
				continue;
			}
			int num = 0;
			if (i < 3)
			{
				num = hp[i];
				if (i == 0 && miniPartyMember && !miniPartyMemberDisabled)
				{
					num -= gm.GetMiniMemberMaxHP();
				}
			}
			else
			{
				num = hp[0];
			}
			if (num > 0)
			{
				memberText[i].color = Color.white;
				statBorders[i].color = GetDefaultColor(i);
				UpdateRoundedBorderColor(i);
			}
		}
	}

	public void RaiseHeads(bool kris, bool susie, bool noelle)
	{
		raiseHead = new bool[3] { kris, susie, noelle };
	}

	public void SelectedAction(int partyMember)
	{
		int num = hp[partyMember];
		if (partyMember == 0 && miniPartyMember && !miniPartyMemberDisabled)
		{
			num -= gm.GetMiniMemberMaxHP();
			memberText[3].color = new Color(1f, 1f, 0f);
		}
		if (isActive[partyMember] && num > 0)
		{
			memberText[partyMember].color = new Color(1f, 1f, 0f);
		}
	}

	public void DeselectedAction(int partyMember)
	{
		int num = hp[partyMember];
		if (partyMember == 0 && miniPartyMember && !miniPartyMemberDisabled)
		{
			num -= gm.GetMiniMemberMaxHP();
			if (hp[partyMember] > 0)
			{
				memberText[3].color = Color.white;
			}
		}
		if (isActive[partyMember] && num > 0)
		{
			memberText[partyMember].color = Color.white;
		}
	}

	public void ActivateManualManipulation()
	{
		manualManipulation = true;
	}

	public void DeactivateManualManipulation()
	{
		manualManipulation = false;
	}

	public void SetRaisedPanel(int raisedPanel)
	{
		this.raisedPanel = raisedPanel;
	}

	public void DisableMiniPartyMember()
	{
		miniPartyMemberDisabled = true;
		ignoreNextHPModification = true;
	}

	public void IgnoreNextHPModification()
	{
		ignoreNextHPModification = true;
	}

	public void SetSprite(int i, string spriteName)
	{
		if (i > 3)
		{
			return;
		}
		string text = "";
		switch (i)
		{
		case 0:
			text = (friskMode ? "player/Frisk/" : "player/Kris/");
			break;
		case 1:
			text = "player/Susie/";
			break;
		case 2:
			text = "player/Noelle/";
			break;
		default:
			if (miniPartyMember)
			{
				switch (gm.GetMiniPartyMember())
				{
				case 1:
					text = "overworld/npcs/";
					break;
				case 2:
					text = "player/Chara/";
					break;
				case 3:
					text = "player/Sans/";
					break;
				default:
					text = "player/Frisk/";
					break;
				}
			}
			break;
		}
		Sprite sprite = Resources.Load<Sprite>(text + spriteName);
		if (sprite != null)
		{
			memberSprite[i].sprite = sprite;
			memberSprite[i].rectTransform.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height) * 2f;
		}
	}

	public void UseKarma(KarmaHandler karmaHandler)
	{
		this.karmaHandler = karmaHandler;
	}

	public int NumOfActivePartyMembers()
	{
		int num = 0;
		bool[] array = isActive;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i])
			{
				num++;
			}
		}
		if (miniPartyMember)
		{
			num--;
		}
		return num;
	}

	public bool LivingMembersBeingTargetted()
	{
		for (int i = 0; i < 3; i++)
		{
			if (hp[i] > 0 && targets[i] && isActive[i])
			{
				return true;
			}
		}
		return false;
	}

	public int NumTargettedMembers()
	{
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			if (isActive[i] && hp[i] > 0 && targets[i])
			{
				num++;
			}
		}
		return num;
	}

	public bool[] GetTargettedMembers()
	{
		return targets;
	}

	public bool IsDefending(int partyMember)
	{
		return defending[partyMember];
	}

	public void Reinitialize()
	{
		Awake();
	}

	public void SetXOffset(int i, int x)
	{
		xPos[i] = x;
	}
}
