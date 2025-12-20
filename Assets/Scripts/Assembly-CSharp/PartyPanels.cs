using UnityEngine;
using UnityEngine.UI;

public class PartyPanels : PartyPanelsBase
{
	private readonly float[] HEAD_OFFSETS = new float[3] { 0f, 2.5f, 5f };

	private bool defense;

	private bool hpCalibrated;

	private bool manualManipulation;

	private bool ignoreNextHPModification;

	private int raisedPanel = -1;

	private KarmaHandler karmaHandler;

	protected override void InitializePanel(int i, bool overworld = false)
	{
		base.InitializePanel(i);
		panels[i].memberSprite = base.transform.Find("Party" + i + "Sprite").GetComponent<Image>();
		panels[i].memberSprite.enabled = false;
	}

	private void Update()
	{
		if (manualManipulation)
		{
			return;
		}
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].isActive)
			{
				int num = 0;
				if (i > 2)
				{
					num = (defense ? (-30) : 32);
				}
				int num2 = i % 3;
				int num3 = (defense ? (-159) : (-35)) + num;
				if (raisedPanel == num2 && !defense)
				{
					num3 += 8;
				}
				int num4 = (defense ? (-4) : 4);
				Transform obj = panels[i].statPanel.transform.Find("Stats");
				panels[i].statPanel.transform.localPosition = Vector3.Lerp(panels[i].statPanel.transform.localPosition, new Vector3(panels[i].xPos, num3), 0.5f);
				obj.localPosition = Vector3.Lerp(obj.localPosition, new Vector3(0f, num4), 0.5f);
				if (i > 2)
				{
					num = 36;
				}
				int partyMember = gm.GetPartyMember(i);
				float num5 = ((partyMember < HEAD_OFFSETS.Length && partyMember > -1) ? HEAD_OFFSETS[partyMember] : 0f);
				bool flag = panels[i].hp > 0;
				int num6 = ((panels[num2].raiseHead && !defense && flag) ? (Mathf.CeilToInt(15f + num5) * 2 + num) : ((int)panels[i].statPanel.transform.localPosition.y + 25));
				if (raisedPanel == num2 && panels[num2].raiseHead && flag)
				{
					num6 += 8;
				}
				panels[i].memberSprite.transform.localPosition = Vector3.Lerp(panels[i].memberSprite.transform.localPosition, new Vector3(panels[i].xPos, num6), 0.5f);
				panels[i].memberSprite.enabled = panels[i].memberSprite.transform.localPosition.y > -10f;
			}
		}
	}

	public override void UpdateHP(int[] hp)
	{
		int[] revivalTurns = Util.FindObjectOfType<BattleManager>().GetRevivalTurns();
		if (ignoreNextHPModification)
		{
			ignoreNextHPModification = false;
		}
		else
		{
			for (int i = 0; i < panels.Length; i++)
			{
				if (!panels[i].isActive || panels[i].ignoreChanges)
				{
					continue;
				}
				int num = hp[i];
				int hp2 = panels[i].hp;
				int maxHP = gm.GetMaxHP(i);
				int num2 = i % 3;
				if (i < 3)
				{
					panels[i].statBorder.transform.Find("Stats").Find("HPBG").GetComponent<RectTransform>()
						.sizeDelta = new Vector2((maxHP >= 100) ? 25 : 45, 10f);
				}
				panels[i].hpText.text = num.ToString((maxHP >= 100) ? "D3" : "D2") + "/" + maxHP.ToString((maxHP >= 100) ? "D3" : "D2");
				int num3 = Mathf.RoundToInt((float)num / (float)maxHP * panels[i].statBorder.transform.Find("Stats").Find("HPBG").GetComponent<RectTransform>()
					.sizeDelta.x);
				if (num3 < 1 && num > 0)
				{
					num3 = 1;
				}
				panels[i].hpBar.sizeDelta = new Vector2(num3, 10f);
				if ((bool)karmaHandler)
				{
					karmaHandler.ReadjustKarma(i);
				}
				if (panels[num2].defending)
				{
					panels[i].hpText.color = new Color(0f, 1f, 1f);
				}
				else if ((bool)karmaHandler && karmaHandler.GetKarma(i) > 0)
				{
					panels[i].hpText.color = new Color(1f, 0f, 1f);
				}
				else if ((float)num < (float)maxHP / 4f)
				{
					panels[i].hpText.color = new Color(1f, 1f, 0f);
				}
				else
				{
					panels[i].hpText.color = Color.white;
				}
				if (num <= 0)
				{
					if (revivalTurns[i] > 0)
					{
						panels[i].hpText.text = "-" + revivalTurns[i] + "/" + maxHP.ToString("D2");
					}
					panels[i].memberText.color = Color.grey;
					panels[i].statBorder.color = Color.grey;
					UpdateRoundedBorderColor(i);
					panels[i].hpText.color = Color.red;
				}
				else if (panels[i].memberText.color == Color.grey && !defense)
				{
					panels[i].memberText.color = Color.white;
					panels[i].statBorder.color = GetDefaultColor(i);
					UpdateRoundedBorderColor(i);
				}
				if (!hpCalibrated)
				{
					continue;
				}
				Vector3 position = panels[i].statPanel.transform.localPosition / 48f - new Vector3(0f, defense ? 0.4f : (-0.5f));
				if (num > hp2)
				{
					DamageNumber component = Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>();
					if (num == maxHP)
					{
						component.StartWord("max", new Color(0f, 1f, 0f), position);
					}
					else if (hp2 <= 0)
					{
						component.StartWord("up", new Color(0f, 1f, 0f), position);
					}
					else
					{
						component.StartNumber(num - hp2, new Color(0f, 1f, 0f), position);
					}
				}
				else if (num < hp2)
				{
					DamageNumber component2 = Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>();
					if (num <= 0)
					{
						component2.StartWord("down", new Color(1f, 0f, 0f), position);
					}
					else
					{
						component2.StartNumber(hp2 - num, GetDefaultColor(i) + Color.white / 2f, position);
					}
				}
				if ((revivalTurns[i] < panels[i].revivalTurn || (revivalTurns[i] == 3 && panels[i].revivalTurn == 0)) && num == 0)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartNumber(1, new Color(0f, 1f, 0f), position);
				}
			}
		}
		for (int j = 0; j < panels.Length; j++)
		{
			panels[j].hp = hp[j];
			panels[j].revivalTurn = revivalTurns[j];
		}
		hpCalibrated = true;
		if (!LivingMembersBeingTargetted() && defense)
		{
			TargetLivingMembers();
		}
	}

	public void SetAsDefending(int i, bool defend)
	{
		if (!panels[i].isActive)
		{
			return;
		}
		for (int j = 0; j < 2; j++)
		{
			int num = i + j * 3;
			panels[num].defending = defend;
			int hp = panels[num].hp;
			gm.GetMaxHP(num);
			if (hp > 0)
			{
				if (defend)
				{
					panels[num].hpText.color = new Color(0f, 1f, 1f);
				}
				else if (j == 0 && (bool)karmaHandler && karmaHandler.GetKarma(num) > 0)
				{
					panels[num].hpText.color = new Color(1f, 0f, 1f);
				}
				else if ((float)panels[num].hp < (float)gm.GetMaxHP(num) / 4f)
				{
					panels[num].hpText.color = new Color(1f, 1f, 0f);
				}
				else
				{
					panels[num].hpText.color = Color.white;
				}
			}
		}
	}

	public void TargetLivingMembers()
	{
		if (!defense)
		{
			return;
		}
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				int num = i + j * 3;
				if (panels[num].hp > 0 && !panels[num].target && panels[num].isActive)
				{
					panels[num].target = true;
				}
			}
		}
		if (LivingMembersBeingTargetted())
		{
			SetTargets(panels[0].target, panels[1].target, panels[2].target, defense);
		}
	}

	public void SetTargets(bool kris, bool susie, bool noelle, bool activateDefense = true)
	{
		defense = activateDefense;
		bool[] array = new bool[3] { kris, susie, noelle };
		for (int i = 0; i < panels.Length; i++)
		{
			if (!panels[i].isActive)
			{
				continue;
			}
			int hp = panels[i].hp;
			int num = i % 3;
			panels[i].target = array[num];
			if (hp > 0)
			{
				if (array[num])
				{
					panels[i].memberText.color = Color.white;
					panels[i].statBorder.color = GetDefaultColor(i);
					UpdateRoundedBorderColor(i);
				}
				else
				{
					Color color = GetDefaultColor(i) * 0.3f + new Color(0.2f, 0.2f, 0.2f);
					color.a = 1f;
					panels[i].memberText.color = new Color(0.5f, 0.5f, 0.5f);
					panels[i].statBorder.color = color;
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
		for (int i = 0; i < panels.Length; i++)
		{
			if (panels[i].isActive && panels[i].hp > 0)
			{
				panels[i].memberText.color = Color.white;
				panels[i].statBorder.color = GetDefaultColor(i);
				UpdateRoundedBorderColor(i);
			}
		}
	}

	public void RaiseHeads(bool kris, bool susie, bool noelle)
	{
		bool[] array = new bool[3] { kris, susie, noelle };
		for (int i = 0; i < panels.Length; i++)
		{
			panels[i].raiseHead = array[i % 3];
		}
	}

	public void SelectedAction(int partyTurn)
	{
		int hp = panels[partyTurn].hp;
		if (panels[partyTurn].isActive && hp > 0)
		{
			panels[partyTurn].memberText.color = new Color(1f, 1f, 0f);
		}
		int hp2 = panels[partyTurn + 3].hp;
		if (panels[partyTurn + 3].isActive && hp2 > 0)
		{
			panels[partyTurn + 3].memberText.color = new Color(1f, 1f, 0f);
		}
	}

	public void DeselectedAction(int partyTurn)
	{
		int hp = panels[partyTurn].hp;
		if (panels[partyTurn].isActive && hp > 0)
		{
			panels[partyTurn].memberText.color = Color.white;
		}
		int hp2 = panels[partyTurn + 3].hp;
		if (panels[partyTurn + 3].isActive && hp2 > 0)
		{
			panels[partyTurn + 3].memberText.color = Color.white;
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
		panels[3].ignoreChanges = true;
	}

	public void IgnoreNextHPModification()
	{
		ignoreNextHPModification = true;
	}

	public void SetSprite(int i, string spriteName)
	{
		string memberSpritePath = PartyMembers.GetMemberSpritePath(gm.GetPartyMember(i));
		Sprite sprite = Resources.Load<Sprite>(memberSpritePath + spriteName);
		Debug.Log(memberSpritePath + spriteName);
		if (sprite != null)
		{
			panels[i].memberSprite.sprite = sprite;
			panels[i].memberSprite.rectTransform.sizeDelta = new Vector2(sprite.texture.width, sprite.texture.height) * 2f;
		}
	}

	public void UseKarma(KarmaHandler karmaHandler)
	{
		this.karmaHandler = karmaHandler;
	}

	public bool LivingMembersBeingTargetted()
	{
		for (int i = 0; i < 3; i++)
		{
			if (panels[i].hp > 0 && panels[i].target && panels[i].isActive)
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
			if (panels[i].isActive && panels[i].hp > 0 && panels[i].target)
			{
				num++;
			}
		}
		return num;
	}

	public bool[] GetTargettedMembers()
	{
		return new bool[3]
		{
			panels[0].target,
			panels[1].target,
			panels[2].target
		};
	}

	public bool IsDefending(int partyMember)
	{
		return panels[partyMember].defending;
	}

	public void SetInitialSprites(bool serious)
	{
		for (int i = 0; i < panels.Length; i++)
		{
			SetSprite(i, PartyMembers.GetMemberPanelSprite(gm.GetPartyMember(i), serious));
		}
	}

	public void KarmaTick(int i)
	{
		if (hpCalibrated && panels[i].isActive && panels[i].hp > 0)
		{
			panels[i].hp--;
		}
	}

	public void UnoTick(int hp)
	{
		panels[0].hp = hp;
	}
}
