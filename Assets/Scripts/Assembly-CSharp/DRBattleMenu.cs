using System;
using System.Collections.Generic;
using UnityEngine;

public class DRBattleMenu : MonoBehaviour
{
	public struct PartySelection
	{
		public ActionType action;

		public int target;

		public int selectionIndex;

		public Magic.ID spellId;

		public int miniActId;

		public bool miniIsDoingAction;

		public bool ignoreMainMember;

		public bool ignoreMiniMember;

		public bool itemUseEquipment;

		public bool lockedIn;

		public void Reset()
		{
			action = ActionType.Idle;
			target = 0;
			selectionIndex = 0;
			spellId = Magic.ID.None;
			miniActId = 0;
			miniIsDoingAction = false;
			ignoreMainMember = false;
			ignoreMiniMember = false;
			itemUseEquipment = false;
			lockedIn = false;
		}
	}

	public enum State
	{
		NoSelection = 0,
		MainMenu = 1,
		EnemyChoice = 2,
		PartyChoice = 3,
		ACT = 4,
		Magic = 5,
		ACTMagicChoice = 6,
		Items = 7,
		PartyActions = 8,
		Sparing = 9,
		Fighting = 10
	}

	public enum ActionType
	{
		Idle = 0,
		Fight = 1,
		ACT = 2,
		FollowACT = 3,
		Magic = 4,
		Item = 5,
		Spare = 6,
		Defend = 7
	}

	private GameManager gm;

	private State state;

	private int partyTurn;

	private PartySelection[] partySelections = new PartySelection[3];

	private bool[] defending = new bool[3];

	private TPBar tpBar;

	private bool holdAxisH;

	private bool holdAxisV;

	private int index;

	private int menuLimit;

	private bool inEquipmentMenu;

	private int[] actMagicSelectMenu = new int[0];

	private int[] lastMenuIndex = new int[3];

	private void Awake()
	{
		gm = Util.GameManager();
		PartySelection[] array = partySelections;
		foreach (PartySelection partySelection in array)
		{
			partySelection.Reset();
		}
	}

	private void Update()
	{
		if (UTInput.GetAxis("Horizontal") == 0f && holdAxisH)
		{
			holdAxisH = false;
		}
		if (UTInput.GetAxis("Vertical") == 0f && holdAxisV)
		{
			holdAxisV = false;
		}
		if (state == State.MainMenu)
		{
			ChangeIndexLinear(MoveHorizontalAxis());
			if (UTInput.GetButtonDown("X"))
			{
				if (partyTurn > 0)
				{
					GoToPreviousSelection();
				}
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
		else if (state == State.EnemyChoice)
		{
			ChangeIndexLinear(MoveVerticalAxis());
			if (UTInput.GetButtonDown("X"))
			{
				switch (partySelections[partyTurn].action)
				{
				case ActionType.Fight:
				case ActionType.ACT:
					EnterMainMenu();
					break;
				case ActionType.Magic:
					EnterMagicMenu(partySelections[partyTurn].selectionIndex);
					break;
				}
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
		else if (state == State.PartyChoice)
		{
			bool flag = (gm.PartySlotFilled(3) || gm.PartySlotFilled(4) || gm.PartySlotFilled(5)) && MoveHorizontalAxis() != 0;
			if (MoveVerticalAxis() != 0 || flag)
			{
				bool flag2 = index < 3;
				if (flag)
				{
					flag2 = !flag2;
				}
				int num = 0;
				for (int i = ((!flag2) ? 3 : 0); i < (flag2 ? 3 : 6); i++)
				{
					if (gm.PartySlotFilled(i))
					{
						num++;
					}
				}
				index = index % 3 + MoveVerticalAxis();
				if (index < 0)
				{
					index = num - 1;
				}
				else if (index >= num)
				{
					index = 0;
				}
				if (!flag2)
				{
					index += 3;
				}
			}
			if (UTInput.GetButtonDown("X"))
			{
				switch (partySelections[partyTurn].action)
				{
				case ActionType.Item:
					EnterItemMenu(partySelections[partyTurn].selectionIndex);
					break;
				case ActionType.Magic:
					EnterMagicMenu(partySelections[partyTurn].selectionIndex);
					break;
				}
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
		else if (state == State.ACT)
		{
			ChangeIndex2D(MoveHorizontalAxis(), MoveVerticalAxis());
			if (UTInput.GetButtonDown("X"))
			{
				EnterEnemyTargetChoice(partySelections[partyTurn].target);
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
		else if (state == State.Magic)
		{
			ChangeIndex2D(MoveHorizontalAxis(), MoveVerticalAxis());
			if (UTInput.GetButtonDown("X"))
			{
				EnterMainMenu();
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
		else if (state == State.ACTMagicChoice)
		{
			ChangeIndex2D(MoveHorizontalAxis(), MoveVerticalAxis());
			if (UTInput.GetButtonDown("X"))
			{
				EnterMainMenu();
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
		else if (state == State.Items)
		{
			ChangeIndex2D(MoveHorizontalAxis(), MoveVerticalAxis());
			if (UTInput.GetButtonDown("X"))
			{
				EnterMainMenu();
			}
			else if (UTInput.GetButtonDown("Z"))
			{
				Choice();
			}
		}
	}

	public void StartPlayerTurn()
	{
		partyTurn = 0;
		if (!IsTurnAvailable(0))
		{
			partyTurn = GetNextTurn();
		}
		EnterPartyMemberSelection();
	}

	private void AdvancePartyMemberTurn()
	{
		partyTurn = GetNextTurn();
		if (partyTurn == 3)
		{
			if (state < State.PartyActions)
			{
				Debug.Log("begin party actions");
				tpBar.UseTP();
			}
			else if (state == State.PartyActions)
			{
				Debug.Log("begin sparing");
			}
			else if (state == State.Sparing)
			{
				Debug.Log("begin fighting");
			}
		}
		else if (state < State.PartyActions)
		{
			EnterPartyMemberSelection();
		}
		else if (state == State.PartyActions)
		{
			Debug.Log("do spare for turnid " + partyTurn);
		}
	}

	private void GoToPreviousSelection()
	{
		int num = partyTurn - 1;
		while (num > -1 && !IsTurnAvailable(num))
		{
			num--;
		}
		if (num > -1 && num != partyTurn)
		{
			partyTurn = num;
			if (partySelections[partyTurn].action == ActionType.Defend)
			{
				tpBar.SetDefendingMember(partyTurn, tpToGain: false);
				defending[partyTurn] = false;
			}
			EnterPartyMemberSelection();
		}
	}

	private void EnterPartyMemberSelection()
	{
		EnterMainMenu();
	}

	private void EnterMainMenu()
	{
		state = State.MainMenu;
		index = lastMenuIndex[partyTurn];
		menuLimit = 5;
		partySelections[partyTurn].Reset();
		if (!IsMainMemberAlive(partyTurn))
		{
			partySelections[partyTurn].ignoreMainMember = true;
		}
	}

	private void EnterEnemyTargetChoice(int toIndex = 0)
	{
		state = State.EnemyChoice;
		index = toIndex;
	}

	private void EnterPartyTargetChoice(int toIndex = 0)
	{
		state = State.PartyChoice;
		index = toIndex;
	}

	private void EnterACTMenu(int toIndex = 0)
	{
		state = State.ACT;
		index = toIndex;
	}

	private void EnterMagicMenu(int toIndex = 0)
	{
		state = State.Magic;
		index = toIndex;
	}

	private void EnterACTMagicChoice(int toIndex = 0)
	{
		state = State.ACTMagicChoice;
		index = toIndex;
	}

	private void EnterItemMenu(int toIndex = 0, bool selectionChoice = false)
	{
		state = State.Items;
		index = toIndex;
		inEquipmentMenu = (selectionChoice ? partySelections[partyTurn].itemUseEquipment : (GetNumOfItems() > 0));
	}

	private void Choice()
	{
		if (state == State.MainMenu)
		{
			lastMenuIndex[partyTurn] = index;
			if (index == 0)
			{
				partySelections[partyTurn].action = ActionType.Fight;
				EnterEnemyTargetChoice();
			}
			else if (index == 1)
			{
				bool flag = (IsMainMemberAlive(partyTurn) && Magic.HasACTAbility(gm.GetPartyMember(partyTurn))) || (IsMiniMemberAlive(partyTurn) && Magic.HasACTAbility(gm.GetPartyMember(partyTurn + 3)));
				bool flag2 = (IsMainMemberAlive(partyTurn) && Magic.GetSpellListWithoutACT(partyTurn).Length != 0) || (IsMiniMemberAlive(partyTurn) && Magic.GetSpellListWithoutACT(partyTurn + 3).Length != 0);
				bool flag3 = IsMainMemberAlive(partyTurn) && Magic.GetSpellListWithoutACT(partyTurn).Length != 0 && IsMiniMemberAlive(partyTurn) && Magic.GetSpellListWithoutACT(partyTurn + 3).Length != 0;
				if (!flag2 && flag)
				{
					partySelections[partyTurn].action = ActionType.ACT;
					EnterEnemyTargetChoice();
					return;
				}
				if (!flag3 && !flag && flag2)
				{
					partySelections[partyTurn].ignoreMiniMember = Magic.GetSpellListWithoutACT(partyTurn).Length != 0;
					partySelections[partyTurn].ignoreMainMember = Magic.GetSpellListWithoutACT(partyTurn + 3).Length != 0;
					partySelections[partyTurn].action = ActionType.Magic;
					EnterMagicMenu();
					return;
				}
				if (!((flag && flag2) || flag3))
				{
					throw new Exception($"Turn ID {partyTurn} ({PartyMembers.GetMemberName(gm.GetPartyMember(partyTurn))} / {PartyMembers.GetMemberName(gm.GetPartyMember(partyTurn + 3))}) has no magic list");
				}
				int num = 0;
				if (flag)
				{
					num++;
				}
				if (flag3)
				{
					num += 2;
				}
				else if (flag2)
				{
					num++;
				}
				actMagicSelectMenu = new int[3];
				int num2 = 0;
				if (flag)
				{
					actMagicSelectMenu[num2] = -1;
					num2++;
				}
				if (flag3)
				{
					actMagicSelectMenu[num2] = gm.GetPartyMember(partyTurn);
					actMagicSelectMenu[++num2] = gm.GetPartyMember(partyTurn + 3);
				}
				else if (flag2)
				{
					if (IsMainMemberAlive(partyTurn) && Magic.GetSpellListWithoutACT(partyTurn).Length != 0)
					{
						actMagicSelectMenu[num2] = gm.GetPartyMember(partyTurn);
					}
					else if (IsMiniMemberAlive(partyTurn) && Magic.GetSpellListWithoutACT(partyTurn + 3).Length != 0)
					{
						actMagicSelectMenu[num2] = gm.GetPartyMember(partyTurn + 3);
					}
				}
				EnterACTMagicChoice();
			}
			else if (index == 2)
			{
				if (GetTotalNumOfItems() > 0)
				{
					partySelections[partyTurn].action = ActionType.Item;
					EnterItemMenu();
				}
			}
			else if (index == 3)
			{
				partySelections[partyTurn].action = ActionType.Spare;
				partySelections[partyTurn].lockedIn = true;
				AdvancePartyMemberTurn();
			}
			else if (index == 4)
			{
				partySelections[partyTurn].action = ActionType.Defend;
				partySelections[partyTurn].lockedIn = true;
				tpBar.SetDefendingMember(partyTurn, tpToGain: true);
				defending[partyTurn] = true;
				AdvancePartyMemberTurn();
			}
		}
		else if (state == State.EnemyChoice)
		{
			partySelections[partyTurn].lockedIn = true;
			AdvancePartyMemberTurn();
		}
		else if (state == State.PartyChoice)
		{
			partySelections[partyTurn].target = index;
			partySelections[partyTurn].lockedIn = true;
			AdvancePartyMemberTurn();
		}
		else if (state == State.ACT)
		{
			partySelections[partyTurn].selectionIndex = index;
			partySelections[partyTurn].lockedIn = true;
			AdvancePartyMemberTurn();
		}
		else if (state == State.Magic)
		{
			int partyMember = (partySelections[partyTurn].ignoreMainMember ? gm.GetPartyMember(partyTurn + 3) : gm.GetPartyMember(partyTurn));
			Magic.ID iD = Magic.GetSpellListWithoutACT(partyMember)[index];
			if (!Magic.CanCastSpell(iD, partyMember, tpBar.GetCalculatedTP()))
			{
				return;
			}
			partySelections[partyTurn].spellId = iD;
			partySelections[partyTurn].selectionIndex = index;
			if (!Magic.GetSpell(iD).TargetsEveryone())
			{
				if (Magic.GetSpell(iD).TargetsEnemies())
				{
					EnterEnemyTargetChoice();
				}
				else
				{
					EnterPartyMemberSelection();
				}
			}
			else
			{
				partySelections[partyTurn].lockedIn = true;
				AdvancePartyMemberTurn();
			}
		}
		else if (state == State.ACTMagicChoice)
		{
			int num3 = actMagicSelectMenu[index];
			if (num3 == -1)
			{
				partySelections[partyTurn].action = ActionType.ACT;
				EnterEnemyTargetChoice();
				return;
			}
			partySelections[partyTurn].ignoreMiniMember = num3 == gm.GetPartyMember(partyTurn);
			partySelections[partyTurn].ignoreMainMember = num3 == gm.GetPartyMember(partyTurn + 3);
			partySelections[partyTurn].action = ActionType.Magic;
			EnterMagicMenu();
		}
		else if (state == State.Items)
		{
			int num4 = ((!inEquipmentMenu) ? GetItemListPerTurn()[index] : GetEquipmentListPerTurn()[index]);
			partySelections[partyTurn].selectionIndex = index;
			if (!inEquipmentMenu && (Items.ItemType(num4) == 4 || num4 == 45))
			{
				partySelections[partyTurn].lockedIn = true;
				AdvancePartyMemberTurn();
			}
			else
			{
				EnterPartyTargetChoice();
			}
		}
	}

	private bool IsTurnAvailable(int turnId)
	{
		if (turnId < 0 || turnId > 2)
		{
			return false;
		}
		if (!IsMainMemberAlive(turnId))
		{
			return IsMiniMemberAlive(turnId);
		}
		return true;
	}

	private bool IsMainMemberAlive(int turnId)
	{
		if (gm.PartySlotFilled(turnId))
		{
			return gm.GetHP(gm.GetPartyMember(turnId)) > 0;
		}
		return false;
	}

	private bool IsMiniMemberAlive(int turnId)
	{
		if (gm.PartySlotFilled(turnId + 3))
		{
			return gm.GetHP(gm.GetPartyMember(turnId + 3)) > 0;
		}
		return false;
	}

	private int GetNextTurn()
	{
		int i;
		for (i = partyTurn + 1; i < 3; i++)
		{
			if (IsTurnAvailable(i))
			{
				return i;
			}
		}
		return i;
	}

	private List<int> GetItemListPerTurn()
	{
		List<int> list = new List<int>(gm.GetItemList());
		if (partyTurn > 0 && partySelections[0].action == ActionType.Item && !partySelections[0].itemUseEquipment && list[partySelections[0].selectionIndex] != 16)
		{
			list.RemoveAt(partySelections[0].selectionIndex);
		}
		if (partyTurn > 1 && partySelections[1].action == ActionType.Item && !partySelections[1].itemUseEquipment && list[partySelections[1].selectionIndex] != 16)
		{
			list.RemoveAt(partySelections[1].selectionIndex);
		}
		return list;
	}

	private List<int> GetEquipmentListPerTurn()
	{
		List<int> list = new List<int>(gm.GetEquipmentItemList());
		if (partyTurn > 0 && partySelections[0].action == ActionType.Item && partySelections[0].itemUseEquipment && list[partySelections[0].selectionIndex] != 16)
		{
			list.RemoveAt(partySelections[0].selectionIndex);
		}
		if (partyTurn > 1 && partySelections[1].action == ActionType.Item && partySelections[1].itemUseEquipment && list[partySelections[1].selectionIndex] != 16)
		{
			list.RemoveAt(partySelections[1].selectionIndex);
		}
		return list;
	}

	private int GetTotalNumOfItems()
	{
		return GetNumOfEquips() + GetNumOfItems();
	}

	private int GetNumOfItems()
	{
		List<int> itemListPerTurn = GetItemListPerTurn();
		itemListPerTurn.RemoveAll(isBlank);
		return itemListPerTurn.Count;
	}

	private int GetNumOfEquips()
	{
		List<int> equipmentListPerTurn = GetEquipmentListPerTurn();
		equipmentListPerTurn.RemoveAll(isBlank);
		return equipmentListPerTurn.Count;
	}

	private bool isBlank(int i)
	{
		return i == -1;
	}

	private void ChangeIndexLinear(int difference)
	{
		index += difference;
		if (index < 0)
		{
			index = menuLimit - 1;
		}
		else if (index >= menuLimit)
		{
			index = 0;
		}
	}

	private void ChangeIndex2D(int h, int v)
	{
		int num = index;
		if (h != 0)
		{
			int num2 = ((index % 2 == 0) ? 1 : (-1));
			index += num2;
			if (index >= menuLimit)
			{
				index = num;
			}
		}
		num = index;
		if (v != 0)
		{
			index += v * 2;
			if (index < 0 || index >= menuLimit)
			{
				index = num;
			}
		}
	}

	private int MoveHorizontalAxis()
	{
		if (UTInput.GetAxis("Horizontal") != 0f && !holdAxisH)
		{
			holdAxisH = true;
			return (int)UTInput.GetAxis("Horizontal");
		}
		return 0;
	}

	private int MoveVerticalAxis()
	{
		if (UTInput.GetAxis("Vertical") != 0f && !holdAxisV)
		{
			holdAxisV = true;
			return (int)UTInput.GetAxis("Vertical");
		}
		return 0;
	}
}
