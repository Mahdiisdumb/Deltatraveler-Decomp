using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : SelectableBehaviour
{
	public struct PartySelection
	{
		public int target;

		public ActionType action;

		public int extraData;

		public bool isEquipment;

		public bool miniMagic;

		public bool mainNoFight;

		public bool magicEnemyTarget;

		public int miniActID;

		public void Reset()
		{
			target = 0;
			action = ActionType.Idle;
			extraData = 0;
			isEquipment = false;
			miniMagic = false;
			mainNoFight = false;
			magicEnemyTarget = false;
			miniActID = 0;
		}

		public bool IsDefend()
		{
			if (action == ActionType.Mercy)
			{
				return extraData == 1;
			}
			return false;
		}

		public bool IsSparing()
		{
			if (action == ActionType.Mercy)
			{
				return extraData == 0;
			}
			return false;
		}
	}

	public enum ActionType
	{
		Idle = -1,
		Fight = 0,
		Act = 1,
		Item = 2,
		Mercy = 3,
		FollowACT = 4,
		HasFollowedACT = 5,
		Magic = 6
	}

	protected GameManager gm;

	protected BattleCamera cam;

	protected MusicPlayer mus;

	protected AudioSource aud;

	protected AudioSource aud2;

	protected GameObject soul;

	protected GameObject target;

	protected BulletBoard bb;

	protected Fade fadeObj;

	protected bool doneIntroFade;

	protected GameObject bg;

	protected ShakingText st;

	protected GameObject selObj;

	protected GameObject selObj2;

	protected GameObject tabSwitcher;

	protected bool doPage2;

	protected int selTarget;

	protected int actChoice;

	protected int battleId;

	protected bool startedBattle;

	protected EnemyBase[] enemies;

	protected bool isBoss;

	protected int curHP;

	protected TextUT boxText;

	protected Portrait boxPortrait;

	protected string curFlavor;

	protected bool flavorPlayedOnce;

	protected bool allowSkip = true;

	protected string[] diag;

	protected int curDiag;

	protected int finalDiag;

	protected int state;

	protected AttackBase curAtk;

	protected PartyPanels partyPanels;

	protected int partySize;

	protected bool twoPartySecondSlot;

	protected int partyTurn;

	protected PartySelection[] partySelections = new PartySelection[3];

	protected bool susieDepressionRefuse;

	protected bool noelleDepressionRefuse;

	protected bool susieDeviousMisbehave;

	public static readonly string DEVIOUS_STRING = "* Susie's acting devious...\n";

	protected int deviousChance = 10;

	protected int[] revivalTurns = new int[6];

	protected bool[] defending = new bool[3];

	protected TPBar tpBar;

	protected Magic.ID[] spellList;

	protected bool selectingMagic;

	protected bool actMagicSelect;

	protected int[] actMagicSelectMenu = new int[0];

	protected bool castingRedBuster;

	protected bool castingDualHeal;

	protected int dualHealUses;

	protected List<EnemyBase.MiniACT> miniACTs = new List<EnemyBase.MiniACT>();

	protected List<int> miniACTIds = new List<int>();

	protected bool inMiniACTEnemyMenu;

	protected int miniACTId = -1;

	protected int firstAvail;

	protected int actionTurn;

	protected bool sparingThisRound;

	protected bool[] sparers = new bool[3];

	protected bool fightingThisRound;

	protected bool firstButton;

	protected int buttonIndex;

	protected bool axisIsDown;

	protected bool isSOULOut;

	protected int endState;

	protected int curDT;

	protected int frames;

	protected int maxFrames;

	protected bool didSoulSparkle;

	private bool skipNextEnemyTurn;

	protected bool isSelEquipment;

	private int itemCount;

	protected DescriptionBox descriptionBox;

	protected virtual void Awake()
	{
		endState = 0;
		startedBattle = false;
		firstButton = true;
	}

	protected virtual void Start()
	{
	}

	protected void Initialize()
	{
		UnityEngine.Object.Destroy(GameObject.Find("OWSoul(Clone)"));
		gm = Util.GameManager();
		cam = Util.FindObjectOfType<BattleCamera>();
		mus = GetComponent<MusicPlayer>();
		aud = base.gameObject.AddComponent<AudioSource>();
		aud2 = base.gameObject.AddComponent<AudioSource>();
		bb = Util.FindObjectOfType<BulletBoard>();
		fadeObj = GameObject.Find("BattleFadeObj").GetComponentInChildren<Fade>();
		st = base.gameObject.AddComponent<ShakingText>();
		tpBar = Util.FindObjectOfType<TPBar>();
		boxText = base.gameObject.AddComponent<TextUT>();
		boxText.SetParent(GameObject.Find("BattleCanvas").transform);
		soul = GameObject.Find("SOUL");
		soul.GetComponent<SOUL>().AdjustSOULColor();
		curHP = gm.GetCombinedHP();
		partyPanels = Util.FindObjectOfType<PartyPanels>();
		ChangeHP();
		partySize = partyPanels.NumOfActivePartyMembers();
		twoPartySecondSlot = gm.PartySlotFilled(1);
		partyTurn = 0;
		state = 0;
		actChoice = 0;
		selTarget = 0;
		buttonIndex = 0;
		SelectButton(buttonIndex);
		axisIsDown = false;
		descriptionBox = Util.FindObjectOfType<DescriptionBox>();
		if (!gm.IsEasyMode())
		{
			didSoulSparkle = true;
		}
		isSOULOut = false;
		for (int i = 0; i < 3; i++)
		{
			partySelections[i].Reset();
		}
	}

	public virtual void StartBattle(int id)
	{
		battleId = id;
		Initialize();
		enemies = EnemyGenerator.GetEnemies(battleId);
		PlayMusic(EnemyGenerator.GetMusic(battleId), EnemyGenerator.GetMusicPitch(battleId));
		bg = EnemyGenerator.GetBattleBG(battleId);
		curFlavor = EnemyGenerator.GetApproachText(battleId);
		isBoss = EnemyGenerator.IsBossEncounter(battleId);
		partyPanels.SetInitialSprites(isBoss);
		int introAttack = EnemyGenerator.GetIntroAttack(battleId);
		if (introAttack > -1)
		{
			partyPanels.RaiseHeads(kris: false, susie: false, noelle: false);
			partyPanels.SetTargets(kris: true, susie: true, noelle: true);
			state = 5;
			curAtk = AttackSpawner.GetAttack(introAttack);
			bb.StartMovement(curAtk.GetBoardSize(), curAtk.GetBoardPos(), instant: true);
			soul.GetComponent<SpriteRenderer>().enabled = true;
			soul.transform.position = curAtk.GetSoulPos();
			firstButton = true;
			SelectButton(-1);
		}
		if (state == 5)
		{
			SendBattleEvents(4);
		}
		startedBattle = true;
		Util.GameManager().ForceTogglePlayers(tog: false);
		DetermineDepressionReject();
		if (state == 0)
		{
			DoSOULSparkle();
		}
		itemCount = GetTotalNumOfItems();
	}

	protected virtual void Update()
	{
		if (!startedBattle)
		{
			return;
		}
		if (!fadeObj.IsPlaying() && !doneIntroFade)
		{
			soul.GetComponent<SpriteRenderer>().sortingOrder = 199;
			doneIntroFade = true;
		}
		int num = 0;
		float num2 = 0f;
		int num3 = 0;
		for (int i = 0; i < 6; i++)
		{
			if (gm.PartySlotFilled(i))
			{
				num += gm.GetHP(i);
				num2 += (float)gm.GetMaxHP(i);
				if (gm.GetHP(i) <= 0)
				{
					num3++;
				}
			}
		}
		if (num > 0)
		{
			int num4 = 250;
			if (num3 == 1)
			{
				num4 = 175;
			}
			else if (num3 == 2)
			{
				num4 = 100;
			}
			else if (num3 == 3)
			{
				num4 = 75;
			}
			else if (num3 >= 4)
			{
				num4 = 50;
			}
			if (isBoss)
			{
				num4 = num4 * 2 / 3;
			}
			st.StartShake((int)((float)num / num2 * (float)num4));
		}
		if (state == 0 && !IsSlotAlive(partyTurn))
		{
			DecideMemberAction(0, ActionType.Idle, 0);
		}
		else if (state == 0)
		{
			selectingMagic = false;
			actMagicSelect = false;
			partyPanels.RaiseHeads(partyTurn == 0, partyTurn == 1, partyTurn == 2);
			partyPanels.SetRaisedPanel(partyTurn);
			if (!boxText.Exists())
			{
				StartText(curFlavor, new Vector2(-4f, -134f), "snd_txtbtl");
			}
			if (allowSkip && (UTInput.GetButton("X") || UTInput.GetButton("C") || flavorPlayedOnce) && boxText.IsPlaying())
			{
				boxText.SkipText(sound: false);
				flavorPlayedOnce = true;
			}
			soul.GetComponent<SOUL>().SetFrozen(boo: true);
			soul.GetComponent<SpriteRenderer>().enabled = true;
			int partyMember = gm.GetPartyMember(partyTurn);
			int partyMember2 = gm.GetPartyMember(partyTurn + 3);
			BattleButton component = GameObject.Find("ACT").GetComponent<BattleButton>();
			bool flag = gm.GetHP(partyTurn) > 0;
			bool flag2 = gm.PartySlotFilled(partyTurn + 3) && gm.GetHP(partyTurn + 3) > 0;
			bool flag3 = (flag && Magic.HasACTAbility(partyMember)) || (flag2 && Magic.HasACTAbility(partyMember2));
			if (component.GetButtonType() != "act" && flag3)
			{
				component.ChangeButtonType("act");
			}
			else if (component.GetButtonType() != "magic" && !flag3 && partyMember2 != 3)
			{
				component.ChangeButtonType("magic");
			}
			else if (component.GetButtonType() != "psi" && !flag3 && partyMember2 == 3)
			{
				component.ChangeButtonType("psi");
			}
			if (Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal")) != 0 && !axisIsDown)
			{
				buttonIndex += Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal"));
				if (buttonIndex > 3)
				{
					buttonIndex = 0;
				}
				else if (buttonIndex < 0)
				{
					buttonIndex = 3;
				}
				axisIsDown = true;
				buttonIndex = Mathf.Abs(buttonIndex % 4);
				SelectButton(buttonIndex);
			}
			else if (Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal")) == 0 && axisIsDown)
			{
				axisIsDown = false;
			}
			if (UTInput.GetButtonDown("Z"))
			{
				bool ignore = true;
				string[,] selTxt = new string[4, 2];
				string[,] selTxt2 = new string[3, 2];
				int i2 = 0;
				int j = 0;
				bool doNum = false;
				bool enemyList = false;
				CreateSelectionObjects();
				firstAvail = -1;
				if (buttonIndex == 0)
				{
					selTxt = GetEnemyListArray();
					DrawEnemyBars(selObj);
					enemyList = true;
					if (buttonIndex == 1 && gm.IsTestMode())
					{
						selTxt[3, 0] = " ";
					}
					ignore = false;
				}
				else if (buttonIndex == 1)
				{
					int num5 = 0;
					bool flag4 = Magic.GetSpellListWithoutACT(partyMember).Length != 0 && flag;
					if (flag4)
					{
						num5++;
					}
					bool flag5 = Magic.GetSpellListWithoutACT(partyMember2).Length != 0 && flag2;
					if (flag5)
					{
						num5++;
					}
					if (num5 == 0)
					{
						selTxt = GetEnemyListArray();
						DrawEnemyBars(selObj);
						enemyList = true;
						if (buttonIndex == 1 && gm.IsTestMode())
						{
							selTxt[3, 0] = " ";
						}
						ignore = false;
					}
					else if (num5 == 1 && !flag3)
					{
						partySelections[partyTurn].miniMagic = flag2;
						selectingMagic = true;
						selTxt = GetSpellList();
						ignore = false;
					}
					else
					{
						bool flag6 = Magic.HasACTAbility(partyMember);
						bool flag7 = Magic.HasACTAbility(partyMember2);
						string arg = "FFF";
						int id = -1;
						if (flag6 && !flag7)
						{
							id = partyMember;
							arg = PartyMembers.GetMemberNeonColorMenu(partyMember);
						}
						else if (!flag6 && flag7)
						{
							id = partyMember2;
							arg = PartyMembers.GetMemberNeonColorMenu(partyMember2);
						}
						bool flag8 = num5 == 2;
						int num6 = 0;
						Vector3[] array = new Vector3[3]
						{
							new Vector3(8f, 94f),
							new Vector3(248f, 94f),
							new Vector3(8f, 62f)
						};
						actMagicSelect = true;
						actMagicSelectMenu = new int[(flag8 && flag3) ? 3 : 2];
						if (flag3)
						{
							selTxt[num6 / 2, num6 % 2] = ((flag6 && flag7) ? "    ACT" : $"<color=#{arg}>  ACT</color>");
							actMagicSelectMenu[num6] = -1;
							if (flag6 != flag7)
							{
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/" + PartyMembers.GetMemberName(id) + "Icon"), selObj.transform).transform.localPosition = new Vector3(-220f, -177f) + array[num6];
							}
							else
							{
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/" + PartyMembers.GetMemberName(partyMember) + "Icon"), selObj.transform).transform.localPosition = new Vector3(-220f, -177f) + array[num6];
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/" + PartyMembers.GetMemberName(partyMember2) + "Icon"), selObj.transform).transform.localPosition = new Vector3(-186f, -177f) + array[num6];
							}
							num6++;
						}
						if (flag4 && flag)
						{
							selTxt[num6 / 2, num6 % 2] = $"<color=#{PartyMembers.GetMemberNeonColorMenu(partyMember)}>  {Magic.GetNameOfMagicMenu(partyMember)}</color>";
							actMagicSelectMenu[num6] = partyMember;
							UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/" + PartyMembers.GetMemberName(partyMember) + "Icon"), selObj.transform).transform.localPosition = new Vector3(-220f, -177f) + array[num6];
							num6++;
						}
						if (flag5 && flag2)
						{
							selTxt[num6 / 2, num6 % 2] = $"<color=#{PartyMembers.GetMemberNeonColorMenu(partyMember2)}>  {Magic.GetNameOfMagicMenu(partyMember2)}</color>";
							actMagicSelectMenu[num6] = partyMember2;
							UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/" + PartyMembers.GetMemberName(partyMember2) + "Icon"), selObj.transform).transform.localPosition = new Vector3(-220f, -177f) + array[num6];
						}
						ignore = false;
					}
				}
				else if (buttonIndex == 2)
				{
					InstantiateItems(ref ignore, ref doNum, ref selTxt, ref selTxt2, ref i2, ref j);
					if (ignore)
					{
						isSelEquipment = !isSelEquipment;
						UnityEngine.Object.Destroy(selObj);
						UnityEngine.Object.Destroy(selObj2);
						UnityEngine.Object.Destroy(tabSwitcher);
						CreateSelectionObjects();
						InstantiateItems(ref ignore, ref doNum, ref selTxt, ref selTxt2, ref i2, ref j);
					}
				}
				else if (buttonIndex == 3)
				{
					selTxt[0, 0] = "* Spare";
					bool flag9 = false;
					for (int k = 0; k < enemies.Length; k++)
					{
						if (enemies[k].CanSpare() && !enemies[k].IsDone())
						{
							flag9 = true;
						}
					}
					if (flag9)
					{
						selTxt[0, 0] = "<color=#ffff00ff>* Spare</color>";
					}
					selTxt[1, 0] = "* Defend";
					if (gm.IsTestMode())
					{
						selTxt[2, 0] = "* Flee (DEBUG)";
					}
					ignore = false;
				}
				for (j = 0; j <= 1; j++)
				{
					for (i2 = 0; i2 <= 2; i2++)
					{
						if (selTxt[i2, j] == null)
						{
							selTxt[i2, j] = "";
						}
					}
				}
				boxText.SkipText(sound: false);
				if (!ignore)
				{
					CreateSelectionsItems(ref flavorPlayedOnce, ref selTxt, ref selTxt2, ref enemyList);
				}
				else
				{
					UnityEngine.Object.Destroy(selObj);
				}
				aud.clip = Resources.Load<AudioClip>(ignore ? "sounds/snd_cantselect" : "sounds/snd_select");
				aud.Play();
			}
			else if (UTInput.GetButtonDown("X") && partyTurn != 0)
			{
				int num7 = partyTurn;
				if (partySize == 2 && IsSlotAlive(0))
				{
					partyTurn = 0;
				}
				else if ((!IsSlotAlive(1) || partySelections[1].action == ActionType.FollowACT) && IsSlotAlive(0))
				{
					partyTurn -= 2;
				}
				else if (((!IsSlotAlive(0) && IsSlotAlive(1) && partyTurn == 2) || IsSlotAlive(0)) && partySize == 3)
				{
					partyTurn--;
				}
				if (num7 != partyTurn)
				{
					partySelections[num7].Reset();
					partyPanels.DeselectedAction(partyTurn);
					tpBar.SetSpecificTPUse(partyTurn, 0);
					int num8 = buttonIndex;
					buttonIndex = (int)partySelections[partyTurn].action;
					if (buttonIndex == 6)
					{
						buttonIndex = 1;
					}
					if (num8 != buttonIndex)
					{
						firstButton = true;
					}
					SelectButton(buttonIndex);
					if (partySelections[partyTurn].IsDefend())
					{
						tpBar.SetDefendingMember(partyTurn, tpToGain: false);
						partyPanels.SetAsDefending(partyTurn, defend: false);
						defending[partyTurn] = false;
					}
					partySelections[partyTurn].Reset();
					if (partyTurn == 0)
					{
						if (partySelections[1].action == ActionType.FollowACT)
						{
							partyPanels.DeselectedAction(1);
							partySelections[1].Reset();
						}
						if (partySelections[2].action == ActionType.FollowACT)
						{
							partyPanels.DeselectedAction(2);
							partySelections[2].Reset();
						}
					}
				}
			}
		}
		if (state == 1)
		{
			if (buttonIndex == 2 && selObj.GetComponent<Selection>().GetID() != 2 && UTInput.GetAxisRaw("Horizontal") == 1f && selObj.GetComponent<Selection>().GetIndex()[1] == 1f && doPage2 && CanMoveToNextPage() && !selObj.GetComponent<Selection>().AxisDown())
			{
				Vector2 index = selObj.GetComponent<Selection>().GetIndex();
				if ((isSelEquipment ? GetNumOfEquips() : GetNumOfItems()) - 4 > 2)
				{
					index -= new Vector2(0f, 1f);
				}
				else
				{
					index -= new Vector2((index.x == 1f) ? 1 : 0, 1f);
				}
				selObj.GetComponent<Selection>().Disable();
				selObj.SetActive(value: false);
				selObj2.SetActive(value: true);
				selObj2.GetComponent<Selection>().Enable();
				selObj2.GetComponent<Selection>().SetSelection(index);
				selObj2.GetComponent<Selection>().SetAxisDown(boo: true);
				gm.PlayGlobalSFX("sounds/snd_menumove");
				state = 2;
			}
			if (UTInput.GetButtonDown("X"))
			{
				tpBar.UpdateTPPreviewBar(0);
				descriptionBox.Hide();
				UnityEngine.Object.Destroy(selObj);
				UnityEngine.Object.Destroy(selObj2);
				UnityEngine.Object.Destroy(tabSwitcher);
				isSelEquipment = false;
				state = 0;
				SelectButton(buttonIndex);
			}
		}
		if (state == 2)
		{
			if (buttonIndex == 2 && UTInput.GetAxisRaw("Horizontal") == -1f && selObj2.GetComponent<Selection>().GetIndex()[1] == 0f && !selObj2.GetComponent<Selection>().AxisDown())
			{
				Vector2 selection = selObj2.GetComponent<Selection>().GetIndex() + new Vector2(0f, 1f);
				selObj2.GetComponent<Selection>().Disable();
				selObj2.SetActive(value: false);
				selObj.SetActive(value: true);
				selObj.GetComponent<Selection>().Enable();
				selObj.GetComponent<Selection>().SetSelection(selection);
				selObj.GetComponent<Selection>().SetAxisDown(boo: true);
				gm.PlayGlobalSFX("sounds/snd_menumove");
				state = 1;
			}
			if (UTInput.GetButtonDown("X"))
			{
				if (buttonIndex == 1)
				{
					for (int l = 0; l < 3; l++)
					{
						if ((bool)GameObject.Find("PartyMemberHP" + l))
						{
							UnityEngine.Object.Destroy(GameObject.Find("PartyMemberHP" + l));
						}
					}
					if (partyTurn == 0)
					{
						descriptionBox.Hide();
						tpBar.UpdateTPPreviewBar(0);
					}
				}
				if (buttonIndex == 2)
				{
					UnityEngine.Object.Destroy(selObj);
					UnityEngine.Object.Destroy(selObj2);
					UnityEngine.Object.Destroy(tabSwitcher);
					state = 0;
					SelectButton(buttonIndex);
					descriptionBox.Hide();
				}
				else
				{
					selObj2.SetActive(value: false);
					selObj.SetActive(value: true);
					state = 1;
				}
			}
		}
		if (buttonIndex == 2 && (state == 1 || state == 2) && UTInput.GetButtonDown("C"))
		{
			bool ignore2 = true;
			string[,] selTxt3 = new string[4, 2];
			string[,] selTxt4 = new string[3, 2];
			int i3 = 0;
			int j2 = 0;
			bool doNum2 = false;
			bool enemyList2 = false;
			isSelEquipment = !isSelEquipment;
			ItemFill(ref ignore2, ref doNum2, ref selTxt3, ref selTxt4, ref i3, ref j2);
			if (ignore2)
			{
				isSelEquipment = !isSelEquipment;
				aud.clip = Resources.Load<AudioClip>("sounds/snd_cantselect");
				aud.Play();
			}
			else
			{
				UnityEngine.Object.Destroy(selObj);
				UnityEngine.Object.Destroy(selObj2);
				UnityEngine.Object.Destroy(tabSwitcher);
				CreateSelectionObjects();
				CreateSelectionsItems(ref flavorPlayedOnce, ref selTxt3, ref selTxt4, ref enemyList2);
				InstantiateItems(ref ignore2, ref doNum2, ref selTxt3, ref selTxt4, ref i3, ref j2);
				aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
				aud.Play();
			}
			descriptionBox.SetDescription(GetDescriptionOfItemFromSelection(), "");
		}
		if (state == 3)
		{
			if (!boxText.IsPlaying() && (bool)Util.FindObjectOfType<SpecialACT>() && !Util.FindObjectOfType<SpecialACT>().IsActivated())
			{
				Util.FindObjectOfType<SpecialACT>().Activate();
			}
			if (allowSkip && (UTInput.GetButton("X") || UTInput.GetButton("C")) && boxText.IsPlaying())
			{
				boxText.SkipText();
				if ((bool)Util.FindObjectOfType<SpecialACT>())
				{
					Util.FindObjectOfType<SpecialACT>().Activate();
				}
			}
			else if ((((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !boxText.IsPlaying()) || !boxText.GetGameObject()) && (!Util.FindObjectOfType<SpecialACT>() || !Util.FindObjectOfType<SpecialACT>().IsActivated()))
			{
				bool flag10 = false;
				if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && (bool)boxText.GetGameObject())
				{
					curDiag++;
					flag10 = true;
					if (!Util.FindObjectOfType<SpecialACT>())
					{
						ResetText();
					}
				}
				bool flag11 = true;
				EnemyBase[] array2 = enemies;
				for (int m = 0; m < array2.Length; m++)
				{
					if (array2[m].IsShaking())
					{
						flag11 = false;
					}
				}
				if ((!boxText.Exists() || flag10) && !Util.FindObjectOfType<SpecialAttackEffect>() && flag11)
				{
					if (curDiag > finalDiag)
					{
						if (boxText.Exists())
						{
							ResetText();
						}
						if (!Util.FindObjectOfType<SpecialACT>())
						{
							if (actionTurn < 3 || (actionTurn == 3 && (fightingThisRound || sparingThisRound)))
							{
								AdvancePlayerTurn();
							}
							else
							{
								AdvanceToEnemyTurn();
							}
						}
					}
					else
					{
						if (curDiag == 1 && castingDualHeal && dualHealUses >= 1 && dualHealUses <= 6)
						{
							if (dualHealUses == 1)
							{
								diag[curDiag] += "\n* The power of the spell began\n  to weaken...";
							}
							else if (dualHealUses < 6)
							{
								diag[curDiag] += "\n* The power of the spell\n  continues to weaken...";
							}
							else
							{
								diag[curDiag] += "\n* The power of the spell\n  has fully weakened!";
							}
						}
						StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
						if (curDiag == 1 && castingRedBuster)
						{
							UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/RedBuster")).GetComponent<RedBusterEffect>().AssignEnemy(enemies[partySelections[0].target]);
							castingRedBuster = false;
							if (gm.PartySlotFilled(3) && gm.GetHP(3) > 0)
							{
								fightingThisRound = true;
								partySelections[0].mainNoFight = true;
								partySelections[0].action = ActionType.Fight;
								partySelections[0].target = 0;
							}
						}
						else if (curDiag == 1 && castingDualHeal)
						{
							castingDualHeal = false;
							int num9 = PartyMembers.GetMaxHP(0) / 2 + Mathf.FloorToInt(PartyMembers.GetMagicRaw(2) * 2f / 3f);
							if (Items.GetItemElement(PartyMembers.GetWeapon(2)) == 1)
							{
								int num10 = PartyMembers.GetMagicEquipment(2);
								if (Items.GetWeaponType(PartyMembers.GetWeapon(2)) == 4)
								{
									num10 = num10 * 2 / 3;
								}
								num9 += num10;
							}
							if (!gm.IsEasyMode())
							{
								float b = ((gm.GetFlagInt(211) == 1 || gm.GetFlagInt(172) > 0) ? 0.6f : 0.8f);
								num9 = Mathf.RoundToInt((float)num9 * Mathf.Lerp(1f, b, (float)dualHealUses / 6f));
								dualHealUses++;
							}
							gm.HealAll(num9);
							gm.PlayTimedHealSound();
							aud2.clip = Resources.Load<AudioClip>("sounds/snd_spellcast");
							aud2.Play();
						}
					}
				}
			}
		}
		if (state == 7 && !target.GetComponentInChildren<FightTarget>().IsGoing() && !Util.FindObjectOfType<SpecialAttackEffect>())
		{
			soul.GetComponent<SpriteRenderer>().enabled = true;
			AdvanceToEnemyTurn();
		}
		if (state == 4)
		{
			bool flag12 = false;
			EnemyBase[] array2 = enemies;
			for (int m = 0; m < array2.Length; m++)
			{
				if (array2[m].IsTalking())
				{
					flag12 = true;
				}
			}
			if (!bb.IsPlaying() && !flag12)
			{
				soul.GetComponent<SOUL>().SetFrozen(boo: false);
				state = 5;
			}
		}
		if (state == 5 && !bb.IsPlaying())
		{
			if (curAtk == null)
			{
				soul.GetComponent<SOUL>().SetControllable(boo: false);
				soul.GetComponent<SpriteRenderer>().enabled = false;
				partyPanels.DeactivateTargets();
				bb.ResetSize();
				state = 6;
				SendBattleEvents();
			}
			else if (!curAtk.HasStarted())
			{
				curAtk.StartAttack();
			}
		}
		if (state == 6 && !bb.IsPlaying())
		{
			bool flag13 = false;
			for (int n = 0; n < 6; n++)
			{
				if (gm.GetHP(n) <= 0 && gm.PartySlotFilled(n))
				{
					flag13 = true;
					revivalTurns[n]--;
					if (revivalTurns[n] == 0)
					{
						gm.SetHP(n, gm.GetMaxHP(n) / 4);
					}
				}
				else
				{
					revivalTurns[n] = 0;
				}
			}
			if (flag13)
			{
				gm.PlayGlobalSFX("sounds/snd_heal");
			}
			ChangeHP();
			flavorPlayedOnce = false;
			defending = new bool[3];
			partyPanels.SetAsDefending(0, defend: false);
			partyPanels.SetAsDefending(1, defend: false);
			partyPanels.SetAsDefending(2, defend: false);
			if (AllEnemiesDone())
			{
				bb.SetBGOrder(100);
				EndNormalFight(customMessage: false, "");
			}
			else
			{
				ChangeFlavorText();
				bb.SetBGOrder(100);
				partyTurn = 0;
				state = 0;
				SelectButton(buttonIndex);
				soul.GetComponent<SOUL>().SetGravityDirection(Vector2.down);
				DetermineDepressionReject();
				for (int num11 = 0; num11 < 3; num11++)
				{
					partySelections[num11].Reset();
				}
				DoSOULSparkle();
			}
		}
		if (state == 10)
		{
			if (allowSkip && (UTInput.GetButton("X") || UTInput.GetButton("C")) && boxText.IsPlaying())
			{
				boxText.SkipText();
			}
			else if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !boxText.IsPlaying())
			{
				gm.EndBattle(endState);
			}
		}
		if (state == 11)
		{
			fadeObj.FadeOut(11);
			state = 12;
		}
		if (state == 12 && !fadeObj.IsPlaying())
		{
			gm.EndBattle(endState);
		}
	}

	protected void SelectButton(int buttonIndex)
	{
		string[] array = new string[4] { "FIGHT", "ACT", "ITEM", "MERCY" };
		for (int i = 0; i < 4; i++)
		{
			BattleButton component = GameObject.Find(array[i]).GetComponent<BattleButton>();
			if (buttonIndex == i)
			{
				soul.transform.SetParent(component.transform);
				soul.transform.localPosition = new Vector2(-0.82f, -0.022f);
				soul.transform.SetParent(null);
				component.Select(boo: true);
			}
			else
			{
				component.Select(boo: false);
			}
		}
	}

	protected virtual void LateUpdate()
	{
		if (!startedBattle)
		{
			return;
		}
		if (gm.GetCombinedHP() != curHP)
		{
			for (int i = 0; i < 6; i++)
			{
				if (gm.GetHP(i) == 0 && revivalTurns[i] == 0 && gm.PartySlotFilled(i))
				{
					revivalTurns[i] = 4;
					if (i > 2 || gm.PartySlotFilled(i + 3))
					{
						revivalTurns[i] = 6;
					}
				}
			}
			curHP = gm.GetCombinedHP();
			ChangeHP();
		}
		ChangeACTTPCost();
		if ((state == 1 || state == 2) && buttonIndex == 2 && (bool)selObj.transform.Find("PAGE1"))
		{
			descriptionBox.SetDescription(GetDescriptionOfItemFromSelection(), "");
		}
		Vector3 vector = new Vector3(69f, 420f);
		if ((bool)selObj && (bool)selObj.GetComponent<Selection>() && selObj.GetComponent<Selection>().IsEnabled() && selObj.activeInHierarchy)
		{
			vector = selObj.GetComponent<Selection>().GetSOUL().transform.localPosition / 48f;
		}
		if ((bool)selObj2 && (bool)selObj2.GetComponent<Selection>() && selObj2.GetComponent<Selection>().IsEnabled() && selObj2.activeInHierarchy)
		{
			vector = selObj2.GetComponent<Selection>().GetSOUL().transform.localPosition / 48f;
		}
		if (vector != new Vector3(69f, 420f))
		{
			soul.transform.position = vector;
		}
		if (doneIntroFade)
		{
			if (state == 1 || state == 2)
			{
				soul.GetComponent<SpriteRenderer>().sortingOrder = 401;
			}
			else if (state == 3 || state == 0)
			{
				soul.GetComponent<SpriteRenderer>().sortingOrder = 199;
			}
		}
		int totalNumOfItems = GetTotalNumOfItems();
		if (totalNumOfItems == 0 && itemCount > 0)
		{
			GameObject.Find("ITEM").GetComponent<BattleButton>().SetUnselectableColor();
		}
		else if (totalNumOfItems > itemCount)
		{
			GameObject.Find("ITEM").GetComponent<BattleButton>().SetSelectableColor();
		}
		itemCount = totalNumOfItems;
	}

	private void ChangeACTTPCost()
	{
		if (state == 1 && buttonIndex == 1 && selectingMagic)
		{
			Vector2 index = selObj.GetComponent<Selection>().GetIndex();
			int num = (int)index.y + (int)index.x * 2;
			Magic.Spell spell = Magic.GetSpell(spellList[num]);
			string description = spell.GetShortDescription();
			int tPCost = spell.GetTPCost();
			if (spellList[num] == Magic.ID.MiniACT)
			{
				description = miniACTs[num].GetDescription();
				tPCost = miniACTs[num].GetTPCost();
			}
			string tpCost = tPCost + "% TP";
			if (tPCost == 0)
			{
				tpCost = "";
			}
			tpBar.UpdateTPPreviewBar(tPCost);
			descriptionBox.SetDescription(description, tpCost);
		}
		if (state != 2 || buttonIndex != 1 || partyTurn != 0 || selectingMagic || selTarget <= -1 || selTarget >= enemies.Length)
		{
			return;
		}
		int num2 = (int)selObj2.GetComponent<Selection>().GetIndex()[1] + (int)selObj2.GetComponent<Selection>().GetIndex()[0] * 2;
		string text = enemies[selTarget].GetActNames()[num2];
		if (text == null)
		{
			return;
		}
		if (text.Contains(";"))
		{
			string[] array = text.Substring(text.IndexOf(";") + 1).Split('`');
			if (array[1].Length != 0)
			{
				tpBar.UpdateTPPreviewBar(int.Parse(array[1]));
				array[1] += "% TP";
			}
			else
			{
				tpBar.UpdateTPPreviewBar(0);
			}
			descriptionBox.SetDescription(array[0], array[1]);
		}
		else
		{
			descriptionBox.Hide();
			tpBar.UpdateTPPreviewBar(0);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		actChoice = 0;
		if (buttonIndex == 0)
		{
			selTarget = (int)index[0];
			UnityEngine.Object.Destroy(selObj);
			UnityEngine.Object.Destroy(selObj2);
			DecideMemberAction(selTarget, ActionType.Fight, 0);
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
		}
		if (buttonIndex == 1)
		{
			ChangeACTTPCost();
			bool flag = true;
			if (selectingMagic)
			{
				int num = (int)index[0] * 2 + (int)index[1];
				int partyMember = gm.GetPartyMember(partySelections[partyTurn].miniMagic ? (partyTurn + 3) : partyTurn);
				if (spellList[num] == Magic.ID.MiniACT && tpBar.GetCalculatedTP() < miniACTs[num].GetTPCost())
				{
					flag = false;
				}
				else if (!Magic.CanCastSpell(spellList[num], partyMember, tpBar.GetCalculatedTP()))
				{
					flag = false;
				}
			}
			if (id == 0 && actMagicSelect)
			{
				firstAvail = -1;
				selObj.GetComponent<Selection>().Reset();
				actMagicSelect = false;
				bool flag2 = false;
				int childCount = selObj.transform.childCount;
				for (int i = 0; i < childCount; i++)
				{
					UnityEngine.Object.DestroyImmediate(selObj.transform.GetChild(0).gameObject);
				}
				int num2 = (int)index[0] * 2 + (int)index[1];
				string[,] enemyListArray;
				if (actMagicSelectMenu[num2] == -1)
				{
					enemyListArray = GetEnemyListArray();
					DrawEnemyBars(selObj);
					flag2 = true;
					if (buttonIndex == 1 && gm.IsTestMode())
					{
						enemyListArray[3, 0] = " ";
					}
				}
				else
				{
					partySelections[partyTurn].miniMagic = gm.GetPartyMember(partyTurn + 3) == actMagicSelectMenu[num2];
					selectingMagic = true;
					enemyListArray = GetSpellList();
				}
				if (firstAvail == -1)
				{
					firstAvail = 0;
				}
				selObj.GetComponent<Selection>().CreateSelections(enemyListArray, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 0);
				selObj.transform.localScale = new Vector2(1f, 1f);
				selObj.GetComponent<Selection>().SetSelection(new Vector2(firstAvail, 0f), playSound: false);
				if (flag2)
				{
					HandleEnemyNameColor();
				}
				aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
				aud.Play();
			}
			else if (id == 0 && flag)
			{
				selTarget = ((!selectingMagic) ? ((int)index[0]) : ((int)index[0] * 2 + (int)index[1]));
				int num3 = 0;
				int num4 = 0;
				bool flag3 = false;
				selObj.SetActive(value: false);
				selObj2.SetActive(value: true);
				selObj2.GetComponent<Selection>().Reset();
				int childCount2 = selObj2.transform.childCount;
				for (int j = 0; j < childCount2; j++)
				{
					UnityEngine.Object.DestroyImmediate(selObj2.transform.GetChild(0).gameObject);
				}
				string[,] array = new string[4, 2];
				firstAvail = -1;
				bool flag4 = false;
				if ((int)index[0] == 3)
				{
					array[0, 0] = "* Godmode";
					array[0, 1] = "* SwapSOULMode";
					array[1, 0] = "* -1 HP Player";
					array[1, 1] = "* +1 HP Player";
					array[2, 0] = "* -25 DMG Enmy";
					array[2, 1] = "* TestHUD";
					array[3, 0] = "* +25 HP Enmy";
					array[3, 1] = "* Max TP";
				}
				else if (!selectingMagic)
				{
					string[] actNames = enemies[(int)index[0]].GetActNames();
					foreach (string actName in actNames)
					{
						array[num3, num4] = DetermineACTMenuName(actName, num3, num4);
						num4++;
						if (num4 == 2)
						{
							num4 = 0;
							num3++;
							if (num3 == 3)
							{
								break;
							}
						}
					}
				}
				else
				{
					gm.GetPartyMember(partySelections[partyTurn].miniMagic ? (partyTurn + 3) : partyTurn);
					Magic.Spell spell = Magic.GetSpell(spellList[selTarget]);
					if (spell.TargetsEveryone())
					{
						flag4 = true;
					}
					else if (spell.TargetsEnemies())
					{
						if (spellList[selTarget] == Magic.ID.MiniACT)
						{
							miniACTId = miniACTIds[selTarget];
							partySelections[partyTurn].miniActID = miniACTId;
						}
						array = GetEnemyListArray(spellList[selTarget] == Magic.ID.MiniACT);
						DrawEnemyBars(selObj2);
						flag3 = true;
						partySelections[partyTurn].magicEnemyTarget = true;
					}
					else
					{
						array = GetMemberListArray();
						DrawMemberBars(selObj2);
						partySelections[partyTurn].magicEnemyTarget = false;
					}
				}
				for (num4 = 0; num4 <= 1; num4++)
				{
					for (num3 = 0; num3 <= 2; num3++)
					{
						if (array[num3, num4] == null || array[num3, num4] == "* ")
						{
							array[num3, num4] = "";
						}
					}
				}
				int origId = 1;
				if ((int)index[0] == 3)
				{
					origId = 2;
				}
				if (firstAvail == -1)
				{
					firstAvail = 0;
				}
				if (flag4)
				{
					UnityEngine.Object.Destroy(selObj);
					UnityEngine.Object.Destroy(selObj2);
					DecideMemberAction(0, ActionType.Magic, (int)spellList[selTarget]);
				}
				else
				{
					selObj2.GetComponent<Selection>().CreateSelections(array, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, origId);
					selObj2.transform.localScale = new Vector2(1f, 1f);
					selObj2.GetComponent<Selection>().SetSelection(new Vector2(firstAvail, 0f), playSound: false);
					state = 2;
					if (flag3)
					{
						HandleEnemyNameColor();
					}
				}
				aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
				aud.Play();
			}
			else if (id == 0 && (!tpBar.ValidTPAmount() || !flag))
			{
				selObj.GetComponent<Selection>().GetSelectionTexts()[(int)index[0], (int)index[1]].GetComponent<AudioSource>().Stop();
				aud.clip = Resources.Load<AudioClip>("sounds/snd_cantselect");
				aud.Play();
			}
			else
			{
				switch (id)
				{
				case 1:
				{
					if (!selectingMagic)
					{
						int num6 = (int)index[0] * 2 + (int)index[1];
						string text = enemies[selTarget].GetActNames()[num6];
						if (IsValidACT(text) && tpBar.ValidTPAmount())
						{
							UnityEngine.Object.Destroy(selObj);
							UnityEngine.Object.Destroy(selObj2);
							DecideMemberAction(selTarget, ActionType.Act, num6);
							if (text.StartsWith("S!"))
							{
								DecideMemberAction(0, ActionType.FollowACT, 0);
							}
							if (text.StartsWith("N!"))
							{
								if (partySize == 2 || gm.GetHP(1) == 0)
								{
									DecideMemberAction(0, ActionType.FollowACT, 0);
								}
								else
								{
									partySelections[2].action = ActionType.FollowACT;
									partyPanels.SelectedAction(2);
								}
							}
							if (text.StartsWith("SN!"))
							{
								DecideMemberAction(0, ActionType.FollowACT, 0);
								DecideMemberAction(0, ActionType.FollowACT, 0);
							}
							aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
							aud.Play();
						}
						else
						{
							selObj2.GetComponent<Selection>().GetSelectionTexts()[(int)index[0], (int)index[1]].GetComponent<AudioSource>().Stop();
							aud.clip = Resources.Load<AudioClip>("sounds/snd_cantselect");
							aud.Play();
						}
						break;
					}
					int num7 = (int)index[0];
					int partyMember2 = gm.GetPartyMember(partySelections[partyTurn].miniMagic ? (partyTurn + 3) : partyTurn);
					if (spellList[selTarget] == Magic.ID.MiniACT && !enemies[num7].HasMiniACT(partyMember2, miniACTId))
					{
						selObj2.GetComponent<Selection>().GetSelectionTexts()[(int)index[0], (int)index[1]].GetComponent<AudioSource>().Stop();
						aud.clip = Resources.Load<AudioClip>("sounds/snd_cantselect");
						aud.Play();
						break;
					}
					UnityEngine.Object.Destroy(selObj);
					UnityEngine.Object.Destroy(selObj2);
					if (!partySelections[partyTurn].magicEnemyTarget)
					{
						if (!twoPartySecondSlot && num7 == 1)
						{
							num7 = 2;
						}
						num7 += (int)index[1] * 3;
					}
					DecideMemberAction(num7, ActionType.Magic, (int)spellList[selTarget]);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
					aud.Play();
					break;
				}
				case 2:
				{
					int num5 = (int)index[0] * 2 + (int)index[1];
					DebugTools.UseTool(DebugTools.GetKeys()[num5]);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
					aud.Play();
					break;
				}
				}
			}
		}
		if (buttonIndex == 2)
		{
			bool flag5 = false;
			if (id != 2)
			{
				int num8 = (int)index[0] * 2 + (int)index[1];
				selObj.SetActive(value: true);
				selObj2.SetActive(value: false);
				state = 1;
				int extraData = num8 + 4 * id;
				partySelections[partyTurn].isEquipment = isSelEquipment;
				partySelections[partyTurn].extraData = extraData;
				if ((bool)selObj.transform.Find("PAGE1"))
				{
					UnityEngine.Object.Destroy(selObj.transform.Find("PAGE1").gameObject);
				}
				UnityEngine.Object.Destroy(tabSwitcher);
				selObj.GetComponent<Selection>().Reset();
				if (partySize == 1 || (!partySelections[partyTurn].isEquipment && (Items.ItemType(GetItemListPerTurn()[num8]) == 4 || GetItemListPerTurn()[num8] == 45)))
				{
					id = 2;
					flag5 = true;
				}
				else
				{
					selObj.GetComponent<Selection>().CreateSelections(GetMemberListArray(), new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 2);
					DrawMemberBars(selObj);
				}
			}
			if (id == 2)
			{
				UnityEngine.Object.Destroy(selObj);
				UnityEngine.Object.Destroy(selObj2);
				isSelEquipment = false;
				int num9 = (int)index[0];
				if (!twoPartySecondSlot && num9 == 1)
				{
					num9 = 2;
				}
				num9 = ((!flag5) ? (num9 + (int)index[1] * 3) : 0);
				DecideMemberAction(num9, ActionType.Item, partySelections[partyTurn].extraData);
			}
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
		}
		if (buttonIndex == 3)
		{
			UnityEngine.Object.Destroy(selObj);
			UnityEngine.Object.Destroy(selObj2);
			if (index[0] == 1f)
			{
				tpBar.SetDefendingMember(partyTurn, tpToGain: true);
				partyPanels.SetAsDefending(partyTurn, defend: true);
				defending[partyTurn] = true;
			}
			if (index[0] == 2f)
			{
				gm.EndBattle(2, !UTInput.GetButton("C"));
				gm.EnablePlayerMovement();
			}
			DecideMemberAction(0, ActionType.Mercy, (int)index[0]);
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
		}
	}

	protected virtual void DrawEnemyBars(GameObject selObj)
	{
		for (int i = 0; i < enemies.Length; i++)
		{
			if (enemies[i].IsDone())
			{
				continue;
			}
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/HPMercyLabel"), selObj.transform);
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/FightEnemyHP"), selObj.transform);
			gameObject.name = "CoolGamerHP" + i;
			gameObject.transform.localPosition += new Vector3(220f, -32 * i - 36);
			int num = Mathf.CeilToInt((float)enemies[i].GetHP() / (float)enemies[i].GetMaxHP() * 100f);
			if (num > 100)
			{
				num = 100;
			}
			else if (num < 1)
			{
				num = 1;
			}
			float f = (float)num * 0.75f;
			gameObject.transform.Find("fg").GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.CeilToInt(f), 17f);
			gameObject.transform.Find("Text").GetComponent<Text>().text = num + "%";
			gameObject.transform.Find("TextShadow").GetComponent<Text>().text = num + "%";
			GameObject gameObject2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/FightEnemyHP"), selObj.transform);
			gameObject2.name = "CoolGamerMercy" + i;
			gameObject2.transform.localPosition += new Vector3(310f, -32 * i - 36);
			if (enemies[i].RenderSpareBar())
			{
				int num2 = enemies[i].GetSatisfactionLevel();
				if (num2 > 100)
				{
					num2 = 100;
				}
				else if (num2 < 0)
				{
					num2 = 0;
				}
				float f2 = (float)num2 * 0.75f;
				gameObject2.transform.Find("fg").GetComponent<RectTransform>().sizeDelta = new Vector2(Mathf.Ceil(f2), 17f);
				gameObject2.transform.Find("fg").GetComponent<Image>().color = new Color(1f, 1f, 0f);
				gameObject2.transform.Find("bg").GetComponent<Image>().color = new Color32(byte.MaxValue, 94, 27, byte.MaxValue);
				gameObject2.transform.Find("Text").GetComponent<Text>().text = num2 + "%";
				gameObject2.transform.Find("Text").GetComponent<Text>().color = new Color32(142, 12, 0, byte.MaxValue);
				gameObject2.transform.Find("TextShadow").GetComponent<Text>().text = num2 + "%";
			}
			else
			{
				gameObject2.transform.Find("nomercy").GetComponent<Image>().enabled = true;
				gameObject2.transform.Find("fg").GetComponent<Image>().color = new Color32(byte.MaxValue, 94, 27, byte.MaxValue);
				gameObject2.transform.Find("bg").GetComponent<Image>().color = new Color32(byte.MaxValue, 94, 27, byte.MaxValue);
				gameObject2.transform.Find("Text").GetComponent<Text>().enabled = false;
				gameObject2.transform.Find("TextShadow").GetComponent<Text>().enabled = false;
			}
			if ((int)Util.GameManager().GetFlag(94) == 1)
			{
				Image[] componentsInChildren = gameObject.transform.Find("corners").GetComponentsInChildren<Image>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].enabled = true;
				}
				componentsInChildren = gameObject2.transform.Find("corners").GetComponentsInChildren<Image>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].enabled = true;
				}
			}
			if (enemies[i].GetType() == typeof(Sans) && !enemies[i].IsTired())
			{
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), selObj.transform);
				obj.transform.localPosition = new Vector3(-116f, -177f);
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.GetComponent<Text>().text = ((Sans)enemies[i]).GetDistractedText();
				obj.GetComponent<Text>().color = new Color32(96, 96, 96, byte.MaxValue);
				obj.GetComponent<Text>().font = Resources.Load<Font>("fonts/DTM-Mono");
			}
			else if (enemies[i].GetType() == typeof(GreaterDog) && ((GreaterDog)enemies[i]).IsDistracted())
			{
				GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), selObj.transform);
				obj2.transform.localPosition = new Vector3(-188f, -209f);
				obj2.transform.localScale = new Vector3(1f, 1f, 1f);
				obj2.GetComponent<Text>().text = "(Distracted)";
				obj2.GetComponent<Text>().color = new Color32(96, 96, 96, byte.MaxValue);
				obj2.GetComponent<Text>().font = Resources.Load<Font>("fonts/DTM-Mono");
			}
			else if (enemies[i].GetType() == typeof(Porky) && ((Porky)enemies[i]).IsVulnerable())
			{
				GameObject obj3 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), selObj.transform);
				obj3.transform.localPosition = new Vector3(-188f, -209f);
				obj3.transform.localScale = new Vector3(1f, 1f, 1f);
				obj3.GetComponent<Text>().text = "(Vulnerable)";
				obj3.GetComponent<Text>().color = new Color32(96, 96, 96, byte.MaxValue);
				obj3.GetComponent<Text>().font = Resources.Load<Font>("fonts/DTM-Mono");
			}
		}
	}

	protected virtual void DrawMemberBars(GameObject selObj)
	{
		int num = ((gm.PartySlotFilled(3) || gm.PartySlotFilled(4) || gm.PartySlotFilled(5)) ? (-32) : 0);
		for (int i = 0; i < 6; i++)
		{
			int num2 = i / 3;
			if (!gm.PartySlotFilled(i))
			{
				continue;
			}
			int num3 = 101;
			if ((i < 3 && gm.PartySlotFilled(i + 3)) || i >= 3)
			{
				num3 = 48;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/FightEnemyHP"), selObj.transform);
			gameObject.name = "PartyMemberHP" + i;
			gameObject.transform.localPosition += new Vector3(80 + num + 240 * num2, -32 * (i % 3) - 36);
			gameObject.transform.Find("fg").GetComponent<RectTransform>().sizeDelta = new Vector2((float)gm.GetHP(i) / (float)gm.GetMaxHP(i) * (float)num3, 17f);
			gameObject.transform.Find("bg").GetComponent<RectTransform>().sizeDelta = new Vector2(num3, 17f);
			gameObject.transform.Find("Text").GetComponent<Text>().enabled = false;
			gameObject.transform.Find("TextShadow").GetComponent<Text>().enabled = false;
			if ((int)Util.GameManager().GetFlag(94) != 1)
			{
				continue;
			}
			Image[] componentsInChildren = gameObject.transform.Find("corners").GetComponentsInChildren<Image>();
			foreach (Image image in componentsInChildren)
			{
				image.enabled = true;
				if (image.gameObject.name.EndsWith("R"))
				{
					image.transform.localPosition = new Vector3(num3 - 50, image.transform.localPosition.y);
				}
			}
		}
	}

	protected string[,] GetMemberListArray()
	{
		string[,] array = new string[3, 2];
		for (int i = 0; i < 6; i++)
		{
			if (gm.PartySlotFilled(i))
			{
				array[i % 3, i / 3] = "* " + PartyMembers.GetMemberName(gm.GetPartyMember(i));
			}
			else
			{
				array[i % 3, i / 3] = "";
			}
		}
		return array;
	}

	protected string[,] GetEnemyListArray(bool greyNamesOnMiniACT = false)
	{
		string[,] array = new string[4, 2];
		int partyMember = gm.GetPartyMember(partySelections[partyTurn].miniMagic ? (partyTurn + 3) : partyTurn);
		for (int i = 0; i < enemies.Length; i++)
		{
			if (!enemies[i].IsDone())
			{
				if (firstAvail == -1)
				{
					firstAvail = i;
				}
				if (greyNamesOnMiniACT && !enemies[i].HasMiniACT(partyMember, miniACTId))
				{
					array[i, 0] = "<color=#888888FF>* " + enemies[i].GetName() + "</color>";
				}
				else
				{
					array[i, 0] = "* " + enemies[i].GetName();
				}
			}
		}
		return array;
	}

	protected void HandleEnemyNameColor()
	{
		Selection selection = (((bool)selObj && selObj.activeInHierarchy) ? selObj.GetComponent<Selection>() : selObj2.GetComponent<Selection>());
		Color color = new Color32(0, 162, 232, byte.MaxValue);
		Color color2 = new Color(1f, 1f, 0f);
		for (int i = 0; i < enemies.Length; i++)
		{
			if (!enemies[i].IsDone())
			{
				bool num = enemies[i].IsTired() && enemies[i].CanSpare();
				Text component = selection.GetSelectionTexts()[i, 0].GetComponent<Text>();
				if (num)
				{
					UnityEngine.UI.Gradient gradient = component.gameObject.AddComponent<UnityEngine.UI.Gradient>();
					gradient.GradientType = UnityEngine.UI.Gradient.Type.Horizontal;
					gradient.EffectGradient = new UnityEngine.Gradient
					{
						colorKeys = new GradientColorKey[2]
						{
							new GradientColorKey(color2, 0.2f),
							new GradientColorKey(color, 1f)
						}
					};
				}
				else if (enemies[i].CanSpare())
				{
					selection.GetSelectionTexts()[i, 0].GetComponent<Text>().color = color2;
				}
				else if (enemies[i].IsTired())
				{
					selection.GetSelectionTexts()[i, 0].GetComponent<Text>().color = color;
				}
				if (enemies[i].CanSpare())
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SpareIcon"), selection.transform).transform.localPosition = new Vector3(-192 + (enemies[i].GetName().Length + 2) * 16, -82 + -32 * i);
				}
				if (enemies[i].IsTired())
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/TiredIcon"), selection.transform).transform.localPosition = new Vector3(-172 + (enemies[i].GetName().Length + 2) * 16, -82 + -32 * i);
				}
			}
		}
	}

	private string[,] GetSpellList()
	{
		string[,] array = new string[3, 2];
		spellList = new Magic.ID[6];
		miniACTIds = new List<int>();
		miniACTs = new List<EnemyBase.MiniACT>();
		List<bool> list = new List<bool>();
		inMiniACTEnemyMenu = false;
		int partyMember = gm.GetPartyMember(partySelections[partyTurn].miniMagic ? (partyTurn + 3) : partyTurn);
		int num = 0;
		for (int i = 0; i < 3; i++)
		{
			for (int j = 0; j < 2; j++)
			{
				if (num >= Magic.GetSpellListWithoutACT(partyMember).Length)
				{
					continue;
				}
				Magic.ID iD = Magic.GetSpellListWithoutACT(partyMember)[num];
				if (iD == Magic.ID.MiniACT)
				{
					int num2 = 0;
					List<EnemyBase.MiniACT> list2 = new List<EnemyBase.MiniACT>();
					for (int k = 0; k < enemies.Length; k++)
					{
						if (!enemies[k].IsDone())
						{
							num2++;
							list2.AddRange(enemies[k].GetMiniACTs(partyMember));
						}
					}
					for (int l = 0; l < list2.Count; l++)
					{
						int iD2 = list2[l].GetID();
						if (!miniACTIds.Contains(iD2))
						{
							miniACTIds.Add(iD2);
							miniACTs.Add(list2[l]);
							list.Add(item: false);
							continue;
						}
						int index = miniACTIds.IndexOf(iD2);
						list[index] = true;
						if (iD2 == 0)
						{
							switch (partyMember)
							{
							case 1:
								miniACTs[index] = EnemyBase.SACTION_DEFAULT;
								break;
							case 2:
								miniACTs[index] = EnemyBase.NACTION_DEFAULT;
								break;
							case 5:
								miniACTs[index] = EnemyBase.CACTION_DEFAULT;
								break;
							default:
								miniACTs[index] = EnemyBase.SACTION_DEFAULT;
								miniACTs[index].SetName("Mini-ACT");
								break;
							}
						}
						else
						{
							miniACTs[index] = new EnemyBase.MiniACT("Conflict", iD2, $"Mini-ACT conflict id {iD2}", 101);
						}
					}
					for (int m = 0; m < miniACTs.Count; m++)
					{
						array[i, j] = $"<color=#{PartyMembers.GetMemberNeonColorMenu(partyMember)}>* {miniACTs[m].GetName()}</color>";
						if (m < miniACTs.Count - 1)
						{
							spellList[j + i * 2] = iD;
							j++;
							if (j > 1)
							{
								j = 0;
								i++;
							}
						}
					}
				}
				else if (!Magic.CanCastSpell(iD, partyMember, 100))
				{
					array[i, j] = "<color=#888888FF>* " + Magic.GetSpell(iD).GetName() + "</color>";
				}
				else if (iD == Magic.ID.SleepMist)
				{
					bool flag = false;
					for (int n = 0; n < enemies.Length; n++)
					{
						if (enemies[n].IsTired() && !enemies[n].IsDone())
						{
							flag = true;
							break;
						}
					}
					if (flag)
					{
						array[i, j] = "<color=#00A2E8FF>* " + Magic.GetSpell(iD).GetName() + "</color>";
					}
					else
					{
						array[i, j] = "* " + Magic.GetSpell(iD).GetName();
					}
				}
				else
				{
					array[i, j] = "* " + Magic.GetSpell(iD).GetName();
				}
				spellList[j + i * 2] = iD;
				num++;
			}
		}
		return array;
	}

	public virtual void SendBattleEvents(int? state = null)
	{
		if (!state.HasValue)
		{
			state = this.state;
		}
		EnemyBase[] array = enemies;
		foreach (EnemyBase enemyBase in array)
		{
			if (!enemyBase.IsDone())
			{
				string text = enemyBase.GetName();
				int? num = state;
				MonoBehaviour.print("Sending " + text + " event for state " + num);
				switch (state)
				{
				case 4:
					enemyBase.EnemyTurnStart();
					break;
				case 6:
					enemyBase.EnemyTurnEnd();
					break;
				default:
					num = state;
					throw new InvalidOperationException("No event for state " + num);
				}
			}
		}
	}

	public void ChangeFlavorText()
	{
		int i;
		for (i = 0; i < enemies.Length && enemies[i].IsDone(); i++)
		{
		}
		curFlavor = enemies[i].GetRandomFlavorText();
	}

	public void ChangeHP()
	{
		partyPanels.UpdateHP(gm.GetHPArray());
	}

	public void ButtonSFX()
	{
		if (!firstButton)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
		}
		firstButton = false;
	}

	public void StartSOULDecision()
	{
		mus.Stop();
		isSOULOut = true;
	}

	public int GetBattleID()
	{
		return battleId;
	}

	public EnemyBase[] GetEnemies()
	{
		return enemies;
	}

	public void PlayMusic(string music, float pitch)
	{
		if (music != "" && music.Replace("_intro", "") != mus.CurrentMusic())
		{
			bool flag = music.EndsWith("_intro");
			mus.ChangeMusic(flag ? music.Replace("_intro", "") : music, flag, playImmediately: true);
			mus.GetSource().pitch = pitch;
		}
		else if ((bool)Util.FindObjectOfType<LostCoreMusic>())
		{
			Util.FindObjectOfType<LostCoreMusic>().SetDanger(danger: true);
		}
	}

	public void PlayMusic(string music, float pitch, bool hasIntro)
	{
		if (music != "" && music != mus.CurrentMusic())
		{
			mus.ChangeMusic(music, hasIntro, playImmediately: true);
			mus.GetSource().pitch = pitch;
		}
		else if ((bool)Util.FindObjectOfType<LostCoreMusic>())
		{
			Util.FindObjectOfType<LostCoreMusic>().SetDanger(danger: true);
		}
	}

	public void StopMusic()
	{
		mus.Stop();
	}

	public void FadeEndBattle()
	{
		for (int i = 0; i < 6; i++)
		{
			if (gm.GetHP(i) < 1)
			{
				gm.SetHP(i, gm.GetMaxHP(i) / 4);
			}
		}
		partyPanels.UpdateHP(gm.GetHPArray());
		fadeObj.FadeOut(11);
		state = 12;
	}

	public void FadeEndBattle(int endState)
	{
		this.endState = endState;
		FadeEndBattle();
	}

	public Fade GetBattleFade()
	{
		return fadeObj;
	}

	public virtual void DecideMemberAction(int target, ActionType action, int extraData)
	{
		flavorPlayedOnce = true;
		partySelections[partyTurn].target = target;
		partySelections[partyTurn].action = action;
		partySelections[partyTurn].extraData = extraData;
		if (action != ActionType.Idle)
		{
			partyPanels.SelectedAction(partyTurn);
		}
		switch (action)
		{
		case ActionType.Act:
		case ActionType.Magic:
			descriptionBox.Hide();
			tpBar.ApplyPreviewTP(partyTurn);
			break;
		case ActionType.Item:
			descriptionBox.Hide();
			break;
		}
		partyTurn++;
		if (partyTurn == 1 && gm.GetHP(1) == 0 && (!gm.PartySlotFilled(4) || gm.GetHP(4) == 0))
		{
			partySelections[1].action = ActionType.Idle;
			partyTurn++;
		}
		if (partyTurn == 2 && gm.GetHP(2) == 0 && (!gm.PartySlotFilled(5) || gm.GetHP(5) == 0))
		{
			partySelections[2].action = ActionType.Idle;
			partyTurn++;
		}
		if (partySelections[1].action == ActionType.FollowACT && partyTurn == 1)
		{
			partyTurn++;
		}
		if (partySelections[2].action == ActionType.FollowACT && partyTurn == 2)
		{
			partyTurn++;
		}
		MonoBehaviour.print(partySelections[2].action.ToString());
		if (partyTurn >= 3 || (partySize == 2 && twoPartySecondSlot && partyTurn >= 2) || partySize == 1)
		{
			partyPanels.SetRaisedPanel(-1);
			MonoBehaviour.print("BEGIN ROUND EXECUTION");
			GameObject.Find("ACT").GetComponent<BattleButton>().ChangeButtonType("act");
			soul.transform.SetParent(null);
			soul.transform.position = new Vector2(-0.055f, -1.63f);
			firstButton = true;
			for (int i = 0; i < 3; i++)
			{
				partyPanels.DeselectedAction(i);
			}
			if (!gm.PartySlotFilled(1) && !gm.PartySlotFilled(4))
			{
				partySelections[1].action = ActionType.Idle;
			}
			if (!gm.PartySlotFilled(2) && !gm.PartySlotFilled(5))
			{
				partySelections[2].action = ActionType.Idle;
			}
			actionTurn = 0;
			partyPanels.RaiseHeads(kris: false, susie: false, noelle: false);
			state = 3;
			soul.GetComponent<SpriteRenderer>().enabled = false;
			soul.transform.position = new Vector3(500f, 500f);
			SelectButton(-1);
			fightingThisRound = false;
			tpBar.UseTP();
			AdvancePlayerTurn();
		}
		else
		{
			if (!twoPartySecondSlot && partySize == 2 && partyTurn == 1)
			{
				partyTurn = 2;
			}
			state = 0;
			SelectButton(buttonIndex);
		}
	}

	public virtual void AdvancePlayerTurn()
	{
		bool flag = false;
		bool flag2 = false;
		bool flag3 = false;
		while (actionTurn < 3)
		{
			bool flag4 = partySelections[actionTurn].action == ActionType.Magic && !Magic.GetSpell(partySelections[actionTurn].extraData).IsAttackMagic();
			if (partySelections[actionTurn].action != ActionType.Idle && partySelections[actionTurn].action < ActionType.Item && !flag4 && ((susieDepressionRefuse && actionTurn == 1) || (noelleDepressionRefuse && actionTurn == 2)))
			{
				flag3 = true;
				partySelections[actionTurn].action = ActionType.Idle;
				break;
			}
			if (partySelections[actionTurn].action != ActionType.Fight && partySelections[actionTurn].action != ActionType.Mercy && partySelections[actionTurn].action != ActionType.Idle && partySelections[actionTurn].action != ActionType.FollowACT)
			{
				break;
			}
			if (partySelections[actionTurn].action == ActionType.Fight)
			{
				if (actionTurn != 0)
				{
					if (!enemies[partySelections[actionTurn].target].PartyMemberAcceptAttack(gm.GetPartyMember(actionTurn), 0))
					{
						flag2 = true;
						partySelections[actionTurn].action = ActionType.Idle;
						break;
					}
					fightingThisRound = true;
					if (actionTurn == 1 && susieDeviousMisbehave)
					{
						break;
					}
				}
				else
				{
					if (gm.GetPartyMember(0) == 0 && (int)gm.GetFlag(102) == 1 && UnityEngine.Random.Range(0, 6) == 1)
					{
						flag = true;
						partySelections[actionTurn].action = ActionType.Idle;
						break;
					}
					fightingThisRound = true;
				}
			}
			if (partySelections[actionTurn].IsSparing())
			{
				sparingThisRound = true;
				sparers[actionTurn] = true;
			}
			if (partySelections[actionTurn].action == ActionType.FollowACT)
			{
				partySelections[actionTurn].action = ActionType.HasFollowedACT;
			}
			actionTurn++;
			MonoBehaviour.print(actionTurn);
			if (actionTurn == 3)
			{
				break;
			}
		}
		string[] array = new string[3] { "* You", "* Susie", "* Noelle" };
		if (AllEnemiesDone())
		{
			EndNormalFight(customMessage: false, "");
		}
		else if (!flag2 && susieDeviousMisbehave && actionTurn == 1 && partySelections[actionTurn].action == ActionType.Fight)
		{
			partyPanels.RaiseHeads(kris: false, susie: true, noelle: false);
			diag = new string[1] { DEVIOUS_STRING };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (flag3)
		{
			partyPanels.RaiseHeads(kris: false, actionTurn == 1, actionTurn == 2);
			diag = new string[1] { array[actionTurn] + " couldn't bring herself\n  to do anything." };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (flag)
		{
			partyPanels.RaiseHeads(kris: true, susie: false, noelle: false);
			string[] array2 = new string[4] { "* You felt light-headed and\n  couldn't draw your weapon.", "* You couldn't gather the\n  strength to fight.", "* You decided to listen to\n  the doctor and rested.", "* You collapsed to the ground\n  trying to draw your weapon." };
			diag = new string[1] { array2[UnityEngine.Random.Range(0, array2.Length)] };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (flag2)
		{
			partyPanels.RaiseHeads(kris: false, actionTurn == 1, actionTurn == 2);
			diag = new string[1] { array[actionTurn] + " refused to fight\n  " + enemies[partySelections[actionTurn].target].GetName() + "." };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (actionTurn == 3 && sparingThisRound)
		{
			sparingThisRound = false;
			partyPanels.RaiseHeads(partySelections[0].IsSparing(), partySelections[1].IsSparing(), partySelections[2].IsSparing());
			string text = "";
			bool flag5 = false;
			if (sparers[0] && sparers[1] && sparers[2])
			{
				text = "* Everyone";
			}
			else
			{
				int num = -1;
				for (int i = 0; i < sparers.Length; i++)
				{
					if (sparers[i])
					{
						int num2 = ((gm.GetHP(i) == 0 && gm.PartySlotFilled(i + 3)) ? gm.GetPartyMember(i + 3) : gm.GetPartyMember(i));
						text = "* " + PartyMembers.GetResponsibilityString(num, num2);
						if (num > -1)
						{
							flag5 = true;
						}
						num = num2;
					}
				}
			}
			sparers = new bool[3];
			bool flag6 = false;
			int num3 = 0;
			for (int j = 0; j < enemies.Length; j++)
			{
				if (!enemies[j].IsDone())
				{
					num3++;
				}
				if (enemies[j].CanSpare() && !enemies[j].IsDone())
				{
					enemies[j].Spare();
					if (flag6)
					{
						enemies[j].GetComponent<AudioSource>().Stop();
					}
					flag6 = true;
				}
				else if (!enemies[j].CanSpare() && !enemies[j].IsDone())
				{
					enemies[j].AttemptedSpare();
				}
			}
			string text2 = "* But none of the enemies'\n  names were <color=#FFFF00FF>YELLOW</color>...";
			if (num3 == 1)
			{
				text2 = "* But the enemy's name\n  wasn't <color=#FFFF00FF>YELLOW</color>...";
			}
			if (flag5)
			{
				if (flag6)
				{
					diag = new string[1] { text + " spared\n  the enemies!" };
				}
				else
				{
					diag = new string[2]
					{
						text + " spared\n  the enemies!",
						text2
					};
				}
			}
			else
			{
				diag = new string[1] { text + " spared the enemies!" };
				if (!flag6)
				{
					ref string reference = ref diag[0];
					reference = reference + "\n" + text2;
				}
			}
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (actionTurn == 3 && fightingThisRound)
		{
			partyPanels.RaiseHeads(partySelections[0].action == ActionType.Fight, partySelections[1].action == ActionType.Fight, partySelections[2].action == ActionType.Fight);
			target = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/FightTarget"));
			EnemyBase krisTarget = null;
			EnemyBase susieTarget = null;
			EnemyBase noelleTarget = null;
			if (partySelections[0].action == ActionType.Fight)
			{
				krisTarget = enemies[partySelections[0].target];
			}
			if (partySelections[1].action == ActionType.Fight)
			{
				susieTarget = enemies[partySelections[1].target];
			}
			if (partySelections[2].action == ActionType.Fight)
			{
				noelleTarget = enemies[partySelections[2].target];
			}
			target.GetComponent<FightTarget>().SetEnemies(krisTarget, susieTarget, noelleTarget);
			bool kris = gm.PartySlotFilled(0) && gm.GetHP(0) > 0 && partySelections[0].action == ActionType.Fight && !partySelections[0].mainNoFight;
			bool susie = gm.PartySlotFilled(1) && gm.GetHP(1) > 0 && partySelections[1].action == ActionType.Fight && !partySelections[1].mainNoFight;
			bool noelle = gm.PartySlotFilled(2) && gm.GetHP(2) > 0 && partySelections[2].action == ActionType.Fight && !partySelections[2].mainNoFight;
			bool mini = gm.PartySlotFilled(3) && gm.GetHP(3) > 0 && partySelections[0].action == ActionType.Fight && !partySelections[0].miniMagic;
			bool mini2 = gm.PartySlotFilled(4) && gm.GetHP(4) > 0 && partySelections[1].action == ActionType.Fight && !partySelections[1].miniMagic;
			bool mini3 = gm.PartySlotFilled(5) && gm.GetHP(5) > 0 && partySelections[2].action == ActionType.Fight && !partySelections[2].miniMagic;
			target.GetComponent<FightTarget>().SetAttackers(kris, susie, noelle, partySize, mini, mini2, mini3);
			state = 7;
		}
		else if (actionTurn == 3 && !fightingThisRound)
		{
			AdvanceToEnemyTurn();
		}
		else
		{
			bool kris2 = actionTurn == 0;
			bool susie2 = actionTurn == 1 || (actionTurn == 0 && partySelections[1].action == ActionType.FollowACT);
			bool noelle2 = actionTurn == 2 || (actionTurn == 0 && partySelections[2].action == ActionType.FollowACT);
			partyPanels.RaiseHeads(kris2, susie2, noelle2);
			if (partySelections[actionTurn].action == ActionType.Act)
			{
				diag = enemies[partySelections[actionTurn].target].PerformAct(partySelections[actionTurn].extraData);
				if (diag[0] == "* Your SOUL shined its power\n  onto Susie!")
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULShine"), new Vector3(partyPanels.transform.Find("Party0Sprite").localPosition.x / 48f, -0.2f), Quaternion.identity);
					castingRedBuster = true;
					if ((int)gm.GetFlag(211) == 1)
					{
						partyPanels.SetSprite(0, "spr_kr_evil_look");
					}
				}
				else if (diag[0] == "* Your SOUL shined its power\n  onto Noelle!")
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULShine"), new Vector3(partyPanels.transform.Find("Party0Sprite").localPosition.x / 48f, -0.2f), Quaternion.identity);
					castingDualHeal = true;
				}
				curDiag = 0;
				finalDiag = diag.Length - 1;
				StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
			}
			else if (partySelections[actionTurn].action == ActionType.Magic)
			{
				int num4 = -1;
				if (susieDeviousMisbehave && actionTurn == 1 && partySelections[actionTurn].extraData == 2)
				{
					num4 = UnityEngine.Random.Range(0, 5);
					if (num4 == 4)
					{
						partySelections[actionTurn].extraData = 3;
						tpBar.RemoveTP(100);
					}
					MonoBehaviour.print("SUSIE DEVIOUS RUDE BUSTER: " + num4);
				}
				int partyMember = gm.GetPartyMember(partySelections[actionTurn].miniMagic ? (actionTurn + 3) : actionTurn);
				int partyMember2 = partySelections[actionTurn].target;
				if (!partySelections[actionTurn].magicEnemyTarget)
				{
					partyMember2 = gm.GetPartyMember(partyMember2);
				}
				if (partyMember == 3)
				{
					gm.SetFlag(105, 1);
				}
				diag = Magic.UseMagic((Magic.ID)partySelections[actionTurn].extraData, enemies, partyMember, partyMember2, num4, partySelections[actionTurn].miniActID);
				MonoBehaviour.print("Magic Spell Index: " + partySelections[actionTurn].extraData);
				curDiag = 0;
				finalDiag = diag.Length - 1;
				StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
			}
			else if (partySelections[actionTurn].action == ActionType.Item)
			{
				int from_slot = ((gm.GetHP(actionTurn) == 0 && gm.PartySlotFilled(actionTurn + 3)) ? (actionTurn + 3) : actionTurn);
				int num5 = -1;
				bool flag7 = false;
				if (partySelections[actionTurn].isEquipment)
				{
					if (susieDeviousMisbehave && Items.ItemType(gm.GetEquipment(partySelections[actionTurn].extraData)) != 3 && actionTurn == 1 && UnityEngine.Random.Range(0, 5) == 0)
					{
						diag = new string[1] { DEVIOUS_STRING + "* Susie threw away the\n  " + Items.ItemName(gm.GetEquipment(partySelections[actionTurn].extraData)) + "!" };
						gm.RemoveEquipment(partySelections[actionTurn].extraData);
						flag7 = true;
					}
				}
				else if (susieDeviousMisbehave && Items.ItemType(gm.GetItem(partySelections[actionTurn].extraData)) == 0 && actionTurn == 1 && UnityEngine.Random.Range(0, 1) == 0)
				{
					if (UnityEngine.Random.Range(0, 5) == 0)
					{
						for (int k = 0; k < enemies.Length; k++)
						{
							if (!enemies[k].IsDone())
							{
								num5 = k;
								break;
							}
						}
					}
					else
					{
						partySelections[actionTurn].target = UnityEngine.Random.Range(0, partySize);
					}
				}
				int num6 = partySelections[actionTurn].target;
				if (num5 >= 0)
				{
					diag = new string[1] { DEVIOUS_STRING + "* Susie gave the " + Items.ItemName(gm.GetItem(partySelections[actionTurn].extraData)) + "\n  to " + enemies[num5].GetName() + "!" };
					enemies[num5].Hit(1, -Items.ItemValue(gm.GetItem(partySelections[actionTurn].extraData)), playSound: true);
					gm.RemoveItem(partySelections[actionTurn].extraData);
				}
				else if (!flag7)
				{
					bool flag8 = true;
					int extraData = partySelections[actionTurn].extraData;
					bool isEquipment = partySelections[actionTurn].isEquipment;
					int num7 = -1;
					Debug.Log(partySelections[actionTurn].extraData);
					Debug.Log("EQUIP " + partySelections[actionTurn].isEquipment);
					MonoBehaviour.print(num6);
					num7 = ((!isEquipment) ? gm.GetItem(extraData) : gm.GetEquipment(extraData));
					diag = Items.ItemUse(num7, from_slot, num6, isBoss).Split('}');
					if (num7 == 22 && soul.GetComponent<SOUL>().GetMaxSpeed() < 8f)
					{
						soul.GetComponent<SOUL>().IncrementSpeed();
						Vector3 position = partyPanels.GetStatPanel(num6).transform.localPosition / 48f;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartWord("spdup", Color.white, position);
					}
					if (num7 == 24)
					{
						if (gm.SusieInParty())
						{
							bool flag9 = false;
							if (PartyMembers.GetHP(1) == 0)
							{
								PartyMembers.Heal(1, PartyMembers.GetMaxHP(1));
								revivalTurns[1] = 0;
								partyPanels.UpdateHP(gm.GetHPArray());
								flag9 = true;
							}
							gm.SetATKBuff(1, 10);
							Vector3 position2 = partyPanels.GetStatPanel(1).transform.localPosition / 48f - new Vector3(0f, flag9 ? 0f : (-0.5f));
							UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartWord("atup", Color.white, position2);
						}
						else
						{
							flag8 = false;
						}
					}
					if (num7 == 45)
					{
						int num8 = 0;
						int num9 = 0;
						EnemyBase[] array3 = enemies;
						foreach (EnemyBase enemyBase in array3)
						{
							if ((bool)enemyBase && !enemyBase.IsDone())
							{
								num9++;
								if (enemyBase.CanBeSkipped())
								{
									num8++;
								}
							}
						}
						if (num8 == num9)
						{
							skipNextEnemyTurn = true;
							diag = Items.ItemUse(-21, from_slot, num6, isBoss).Split('}');
						}
						else
						{
							flag8 = false;
							diag = Items.ItemUse(-22, from_slot, num6, isBoss).Split('}');
							if (num9 > 1)
							{
								diag[0] = diag[0].Replace("the enemy\n  ", (num8 == 0) ? "the enemies\n  " : "one of the\n  enemies ");
							}
						}
					}
					if (flag8)
					{
						gm.UseItem(partySelections[actionTurn].target, extraData, isEquipment);
					}
					else
					{
						gm.MoveItemToBack(extraData);
					}
				}
				curDiag = 0;
				finalDiag = diag.Length - 1;
				StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
			}
		}
		if (actionTurn < 3)
		{
			actionTurn++;
		}
	}

	private void DetermineDepressionReject()
	{
		if ((int)gm.GetFlag(172) == 2 && gm.SusieInParty())
		{
			susieDepressionRefuse = UnityEngine.Random.Range(0, 5) == 0;
			partyPanels.SetSprite(1, susieDepressionRefuse ? "spr_su_down_depressed_reject" : "depressed/spr_su_down_0_depressed");
		}
		if ((int)gm.GetFlag(172) >= 1 && gm.NoelleInParty())
		{
			noelleDepressionRefuse = UnityEngine.Random.Range(0, 8) == 0;
			partyPanels.SetSprite(2, noelleDepressionRefuse ? "spr_no_down_depressed_reject" : "depressed/spr_no_down_0_depressed");
		}
		if ((int)gm.GetFlag(257) != 1 || !gm.SusieInParty())
		{
			return;
		}
		if (gm.SusieInParty() && gm.GetHP(1) > 0)
		{
			if (susieDeviousMisbehave)
			{
				partyPanels.SetSprite(1, "unhappy/spr_su_down_0_unhappy");
			}
			susieDeviousMisbehave = UnityEngine.Random.Range(0, deviousChance) == 0;
			if (susieDeviousMisbehave)
			{
				partyPanels.SetSprite(1, "spr_su_down_devious");
				deviousChance = 10;
			}
			else if (deviousChance > 4)
			{
				deviousChance -= 2;
			}
		}
		else
		{
			susieDeviousMisbehave = false;
		}
	}

	public virtual void AdvanceToEnemyTurn()
	{
		if (boxText.Exists())
		{
			boxText.DestroyOldText();
		}
		partyPanels.RaiseHeads(kris: false, susie: false, noelle: false);
		state = 4;
		soul.GetComponent<SpriteRenderer>().sortingOrder = 199;
		soul.GetComponent<SpriteRenderer>().enabled = true;
		if (diag == null || buttonIndex == 0 || buttonIndex == 3)
		{
			diag = new string[1] { "" };
			curDiag = 0;
		}
		if (AllEnemiesDone())
		{
			EndNormalFight(customMessage: false, "");
			return;
		}
		SendBattleEvents();
		int num = -1;
		for (int i = 0; i < enemies.Length; i++)
		{
			if (!enemies[i].IsDone())
			{
				enemies[i].Chat();
				if (num == -1)
				{
					num = i;
				}
			}
		}
		if (num != -1 && !skipNextEnemyTurn)
		{
			bool[] targets = enemies[num].GetTargets();
			partyPanels.SetTargets(targets[0], targets[1], targets[2]);
			curAtk = AttackSpawner.GetAttack(enemies[num].GetNextAttack());
		}
		else
		{
			skipNextEnemyTurn = false;
			partyPanels.SetTargets(kris: true, susie: true, noelle: true);
			curAtk = AttackSpawner.GetAttack(-1);
		}
		bb.StartMovement(curAtk.GetBoardSize(), curAtk.GetBoardPos());
		soul.transform.position = curAtk.GetSoulPos();
	}

	public void SkipPartyMemberTurn(int partyMember)
	{
		partySelections[partyMember].action = ActionType.Idle;
	}

	public void ForceNoSpare()
	{
		sparers = new bool[3];
		sparingThisRound = false;
	}

	public void ForceNoFight()
	{
		fightingThisRound = false;
	}

	public void StartText(string diag, Vector2 pos, string sound, bool allowSkip = true)
	{
		this.allowSkip = allowSkip;
		string[] array = diag.Split('`');
		if (boxText.Exists())
		{
			ResetText();
		}
		if (array.Length > 1 && !array[0].StartsWith("sounds/"))
		{
			boxPortrait = Portrait.CreatePortrait(array[0]);
			boxPortrait.transform.SetParent(GameObject.Find("BattleCanvas").transform);
			boxPortrait.transform.localPosition = new Vector2(-218f, 20f) + pos;
			boxPortrait.transform.localScale = Vector3.one;
			boxPortrait.Play();
			pos += new Vector2(108f, 0f);
		}
		if (array.Length > 1 && array[^2].StartsWith("snd_"))
		{
			sound = array[^2];
		}
		boxText.StartText(array[^1], pos, sound, 0, "DTM-Mono");
		if (allowSkip && (UTInput.GetButton("X") || UTInput.GetButton("C")) && (state == 0 || state == 3 || state == 10))
		{
			boxText.SkipText(state != 0);
		}
		boxText.GetText().lineSpacing = 1.025f;
	}

	public void ResetText()
	{
		if ((bool)boxPortrait)
		{
			UnityEngine.Object.Destroy(boxPortrait.gameObject);
		}
		boxText.DestroyOldText();
	}

	public TextUT GetBattleText()
	{
		return boxText;
	}

	private bool AllEnemiesDone()
	{
		bool result = true;
		EnemyBase[] array = enemies;
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsDone())
			{
				result = false;
			}
		}
		return result;
	}

	public void EndNormalFight(bool customMessage, string message)
	{
		int num = 0;
		int num2 = 0;
		int num3 = (int)gm.GetFlag(125);
		partyPanels.RaiseHeads(kris: true, susie: true, noelle: true);
		bool flag = false;
		endState = 2;
		EnemyBase[] array = enemies;
		foreach (EnemyBase enemyBase in array)
		{
			if (enemyBase.IsKilled())
			{
				num3++;
				endState = 1;
			}
			if (enemyBase.IsDone())
			{
				num += enemyBase.GetFinalEXP();
			}
			if (enemyBase.IsSpared())
			{
				flag = true;
			}
			num2 += enemyBase.GetGold();
		}
		if (gm.GetEXP() + num > 99999)
		{
			num = 99999 - gm.GetEXP();
		}
		if (flag && endState == 1)
		{
			endState = 3;
		}
		num2 += tpBar.GetCurrentTP() / 5;
		gm.SetFlag(125, num3);
		soul.GetComponent<SpriteRenderer>().enabled = false;
		StopMusic();
		for (int j = 0; j < 6; j++)
		{
			if (gm.GetHP(j) < 1)
			{
				gm.SetHP(j, gm.GetMaxHP(j) / 4);
			}
		}
		string text = "* YOU WON!\n* You earned " + num + " XP and " + num2 + " gold.";
		int lV = gm.GetLV();
		gm.AddEXP(num);
		gm.AddGold(num2);
		if (gm.GetLV() > lV)
		{
			gm.PlayGlobalSFX("sounds/snd_levelup");
			text += "\n* Your LOVE increased.";
		}
		partyPanels.UpdateHP(gm.GetHPArray());
		if (customMessage)
		{
			text = message;
		}
		StartText(text, new Vector2(-4f, -134f), "snd_txtbtl", allowSkip: false);
		state = 10;
	}

	public void ForceSoloKris(bool removeMiniPartyMember = false)
	{
		partySize = 1;
		partySelections[1].Reset();
		partySelections[2].Reset();
		if (removeMiniPartyMember && gm.PartySlotFilled(3))
		{
			gm.SetPartyMember(3, -1);
			partyPanels.DisableMiniPartyMember();
		}
	}

	public void UpdatePartyMembers()
	{
		partyPanels.Reinitialize();
		partySize = partyPanels.NumOfActivePartyMembers();
		twoPartySecondSlot = gm.SusieInParty();
		ChangeHP();
	}

	public void ActivateSeriousMode()
	{
		isBoss = true;
		partyPanels.SetSprite(1, "unhappy/spr_su_down_0_unhappy");
		partyPanels.SetSprite(2, "unhappy/spr_no_down_0_unhappy");
	}

	public void JerryFightReorganize()
	{
		enemies = new EnemyBase[1] { enemies[1] };
	}

	public virtual void DoSOULSparkle()
	{
		if (!didSoulSparkle)
		{
			didSoulSparkle = true;
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/EyeFlashSparkle"), soul.transform.position, Quaternion.identity);
		}
	}

	public void MiniPartyMemberSpellToMainFight(int enemyID = 0)
	{
		fightingThisRound = true;
		partySelections[actionTurn].miniMagic = true;
		partySelections[actionTurn].action = ActionType.Fight;
		partySelections[actionTurn].target = enemyID;
	}

	public bool[] GetDefendingMembers()
	{
		return defending;
	}

	public int[] GetRevivalTurns()
	{
		return revivalTurns;
	}

	public bool IsSeriousMode()
	{
		return isBoss;
	}

	public int GetState()
	{
		return state;
	}

	public int GetCurrentStringNum()
	{
		return curDiag;
	}

	public bool IsSusieDevious()
	{
		return susieDeviousMisbehave;
	}

	public int GetPartySize()
	{
		return partySize;
	}

	public void PlaySound2(string path)
	{
		aud2.clip = Resources.Load<AudioClip>(path);
		aud2.Play();
	}

	private List<int> GetItemListPerTurn()
	{
		List<int> list = new List<int>(gm.GetItemList());
		if (partyTurn > 0 && partySelections[0].action == ActionType.Item && !partySelections[0].isEquipment && list[partySelections[0].extraData] != 16)
		{
			list.RemoveAt(partySelections[0].extraData);
		}
		if (partyTurn > 1 && partySelections[1].action == ActionType.Item && !partySelections[1].isEquipment && list[partySelections[1].extraData] != 16)
		{
			list.RemoveAt(partySelections[1].extraData);
		}
		return list;
	}

	private List<int> GetEquipmentListPerTurn()
	{
		List<int> list = new List<int>(gm.GetEquipmentItemList());
		if (partyTurn > 0 && partySelections[0].action == ActionType.Item && partySelections[0].isEquipment && list[partySelections[0].extraData] != 16)
		{
			list.RemoveAt(partySelections[0].extraData);
		}
		if (partyTurn > 1 && partySelections[1].action == ActionType.Item && partySelections[1].isEquipment && list[partySelections[1].extraData] != 16)
		{
			list.RemoveAt(partySelections[1].extraData);
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

	private bool CanMoveToNextPage()
	{
		List<int> obj = (isSelEquipment ? GetEquipmentListPerTurn() : GetItemListPerTurn());
		obj.RemoveAll(isBlank);
		return obj.Count > 4;
	}

	private bool isBlank(int i)
	{
		return i == -1;
	}

	private void InstantiateItems(ref bool ignore, ref bool doNum2, ref string[,] selTxt, ref string[,] selTxt2, ref int i, ref int j)
	{
		GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/TextBase"), selObj.transform);
		obj.name = "PAGE1";
		obj.transform.localPosition = new Vector2(330f, -198f);
		obj.transform.localScale = new Vector3(1f, 1f, 1f);
		obj.GetComponent<Text>().text = "PAGE 1";
		GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/TextBase"), selObj2.transform);
		obj2.name = "PAGE2";
		obj2.transform.localPosition = new Vector2(330f, -198f);
		obj2.transform.localScale = new Vector3(1f, 1f, 1f);
		obj2.GetComponent<Text>().text = "PAGE 2";
		GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/TextButtonBase"), tabSwitcher.transform);
		gameObject.name = "TABSWITCH";
		gameObject.transform.localPosition = new Vector2(45f, -210f);
		gameObject.transform.localScale = new Vector3(1f, 1f, 1f);
		bool joystickIsActive = UTInput.joystickIsActive;
		Debug.Log(ColorUtility.ToHtmlStringRGB(Selection.SELECTION_COLORS[gm.GetFlagInt(223)]));
		string arg = ((GetNumOfItems() == 0) ? "888" : ((!isSelEquipment) ? ColorUtility.ToHtmlStringRGB(Selection.SELECTION_COLORS[gm.GetFlagInt(223)]) : "FFF"));
		string text = string.Concat(str1: string.Format(" <color=#{0}>item</color>  |  <color=#{1}>equip</color>", arg, (GetNumOfEquips() == 0) ? "888" : (isSelEquipment ? ColorUtility.ToHtmlStringRGB(Selection.SELECTION_COLORS[gm.GetFlagInt(223)]) : "FFF")), str0: joystickIsActive ? "        " : string.Format("<color=#FFF>[{0}]</color> ", UTInput.GetKeyName("Menu")));
		gameObject.GetComponent<Text>().font = Resources.Load<Font>("fonts/battlehud");
		gameObject.GetComponent<Text>().fontSize = 16;
		gameObject.GetComponent<Text>().text = text;
		gameObject.GetComponent<Text>().color = Color.grey;
		Image component = gameObject.transform.Find("Confirm").GetComponent<Image>();
		component.transform.localPosition = new Vector3(-252f, 57f);
		if (!joystickIsActive)
		{
			component.enabled = false;
		}
		else
		{
			component.enabled = true;
			ButtonPrompts.UpdateImageWithGraphic("Menu", component);
		}
		ItemFill(ref ignore, ref doNum2, ref selTxt, ref selTxt2, ref i, ref j);
	}

	private void ItemFill(ref bool ignore, ref bool doNum2, ref string[,] selTxt, ref string[,] selTxt2, ref int i, ref int j)
	{
		List<int> obj = (isSelEquipment ? GetEquipmentListPerTurn() : GetItemListPerTurn());
		doPage2 = false;
		foreach (int item in obj)
		{
			if (item == -1)
			{
				continue;
			}
			ignore = false;
			if (doNum2)
			{
				doPage2 = true;
				selTxt2[i, j] = "* " + Items.ShortItemName(item, isBoss);
			}
			else
			{
				selTxt[i, j] = "* " + Items.ShortItemName(item, isBoss);
			}
			j++;
			if (j == 2)
			{
				j = 0;
				i++;
				if (i == 2)
				{
					i = 0;
					doNum2 = true;
				}
			}
		}
	}

	private void CreateSelectionsItems(ref bool flavorPlayedOnce, ref string[,] selTxt, ref string[,] selTxt2, ref bool enemyList)
	{
		flavorPlayedOnce = true;
		if (firstAvail == -1)
		{
			firstAvail = 0;
		}
		selObj.AddComponent<Selection>().CreateSelections(selTxt, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 0);
		selObj.transform.localScale = new Vector2(1f, 1f);
		selObj.GetComponent<Selection>().SetSelection(new Vector2(firstAvail, 0f), playSound: false);
		selObj2.AddComponent<Selection>().CreateSelections(selTxt2, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 1);
		selObj2.transform.localScale = new Vector2(1f, 1f);
		selObj2.GetComponent<Selection>().Disable();
		selObj2.SetActive(value: false);
		tabSwitcher.transform.localScale = new Vector2(1f, 1f);
		if (enemyList)
		{
			HandleEnemyNameColor();
		}
		ResetText();
		state = 1;
	}

	private string GetDescriptionOfItemFromSelection()
	{
		int num = -1;
		if (state == 1)
		{
			num = (int)selObj.GetComponent<Selection>().GetIndex()[1] + (int)selObj.GetComponent<Selection>().GetIndex()[0] * 2;
		}
		else if (state == 2)
		{
			num = (int)selObj2.GetComponent<Selection>().GetIndex()[1] + (int)selObj2.GetComponent<Selection>().GetIndex()[0] * 2 + 4;
		}
		if (num > -1)
		{
			return Items.GetBattleDescription(isSelEquipment ? GetEquipmentListPerTurn()[num] : GetItemListPerTurn()[num]);
		}
		Debug.Log("Error in Battle Manager: Failed to get description of Item");
		return "";
	}

	private void CreateSelectionObjects()
	{
		if ((bool)selObj)
		{
			UnityEngine.Object.Destroy(selObj);
		}
		if ((bool)selObj2)
		{
			UnityEngine.Object.Destroy(selObj2);
		}
		if ((bool)tabSwitcher)
		{
			UnityEngine.Object.Destroy(tabSwitcher);
		}
		selObj = new GameObject("SelectTier1");
		selObj.layer = 5;
		selObj.AddComponent<RectTransform>();
		selObj.transform.SetParent(GameObject.Find("BattleCanvas").transform);
		selObj2 = new GameObject("SelectTier2");
		selObj2.layer = 5;
		selObj2.AddComponent<RectTransform>();
		selObj2.transform.SetParent(GameObject.Find("BattleCanvas").transform);
		tabSwitcher = new GameObject("ItemSwitcher");
		tabSwitcher.layer = 5;
		tabSwitcher.AddComponent<RectTransform>();
		tabSwitcher.transform.SetParent(GameObject.Find("BattleCanvas").transform);
	}

	private string DetermineACTMenuName(string actName, int i, int j)
	{
		if (actName != null)
		{
			if (actName.Contains(";"))
			{
				actName = actName.Substring(0, actName.IndexOf(';'));
			}
			bool flag = gm.SusieInParty() && PartyMembers.GetHP(1) > 0;
			bool flag2 = gm.NoelleInParty() && PartyMembers.GetHP(2) > 0;
			bool flag3 = gm.GetPartyMember(0) == 0 && gm.GetHP(0) > 0;
			if (actName.StartsWith("S!"))
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SusieIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * j, 94 + -32 * i);
				if (flag)
				{
					return "  <color=#FF69FFFF>" + actName.Replace("S!", "") + "</color>";
				}
				return "  <color=#888888FF>" + actName.Replace("S!", "") + "</color>";
			}
			if (actName.StartsWith("N!"))
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/NoelleIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * j, 94 + -32 * i);
				if (flag2)
				{
					return "  <color=#FFFF69FF>" + actName.Replace("N!", "") + "</color>";
				}
				return "  <color=#888888FF>" + actName.Replace("N!", "") + "</color>";
			}
			if (actName.StartsWith("SN!"))
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SusieIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * j, 94 + -32 * i);
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/NoelleIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(42 + 240 * j, 94 + -32 * i);
				if (flag && flag2)
				{
					return "    " + actName.Replace("SN!", "");
				}
				return "    <color=#888888FF>" + actName.Replace("SN!", "") + "</color>";
			}
			if (actName.StartsWith("KS!"))
			{
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/KrisIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * j, 94 + -32 * i);
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SusieIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(42 + 240 * j, 94 + -32 * i);
				if (flag3 && flag)
				{
					return "    " + actName.Replace("KS!", "");
				}
				return "    <color=#888888FF>" + actName.Replace("KS!", "") + "</color>";
			}
			return "* " + actName;
		}
		return "";
	}

	private bool IsValidACT(string actName)
	{
		if (actName.Contains(";"))
		{
			actName = actName.Substring(0, actName.IndexOf(';'));
		}
		bool flag = gm.SusieInParty() && PartyMembers.GetHP(1) > 0;
		bool flag2 = gm.NoelleInParty() && PartyMembers.GetHP(2) > 0;
		bool flag3 = gm.GetPartyMember(0) == 0 && gm.GetHP(0) > 0;
		if (!actName.Contains("!"))
		{
			return true;
		}
		if (actName.StartsWith("S!") && flag)
		{
			return true;
		}
		if (actName.StartsWith("N!") && flag2)
		{
			return true;
		}
		if (actName.StartsWith("SN!") && flag && flag2)
		{
			return true;
		}
		if (actName.StartsWith("KS!") && flag3 && flag)
		{
			return true;
		}
		return false;
	}

	private bool IsSlotAlive(int i)
	{
		if (gm.GetHP(i) > 0)
		{
			return true;
		}
		if (i < 3 && gm.PartySlotFilled(i + 3) && gm.GetHP(i + 3) > 0)
		{
			return true;
		}
		return false;
	}

	public bool AttackIsActive()
	{
		if (state == 5 && curAtk != null)
		{
			return curAtk.HasStarted();
		}
		return false;
	}
}
