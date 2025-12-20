using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BattleManager : TranslatableSelectableBehaviour
{
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

	protected BattleBG bg;

	protected ShakingText st;

	protected GameObject selObj;

	protected GameObject selObj2;

	protected bool doPage2;

	protected int selTarget;

	protected int actChoice;

	protected int battleId;

	protected bool startedBattle;

	protected EnemyBase[] enemies;

	protected bool isBoss;

	protected int curHP;

	protected TextUT boxText;

	protected RectTransform boxPortrait;

	protected string curFlavor;

	protected bool flavorPlayedOnce;

	protected string[] diag;

	protected int curDiag;

	protected int finalDiag;

	protected int state;

	protected AttackBase curAtk;

	protected PartyPanels partyPanels;

	protected int partySize;

	protected bool krisAndSusie;

	protected int miniPartyMember;

	protected int partyTurn;

	protected int[][] partySelections = new int[3][]
	{
		new int[3] { 0, -1, 0 },
		new int[3] { 0, -1, 0 },
		new int[3] { 0, -1, 0 }
	};

	protected bool susieDepressionRefuse;

	protected bool noelleDepressionRefuse;

	protected bool susieDeviousMisbehave;

	protected readonly string deviousString = "* Susie's acting devious...\n";

	protected int deviousChance = 10;

	protected int[] revivalTurns = new int[3];

	protected bool[] defending = new bool[3];

	protected bool selectingMagic;

	protected bool actMagicSelect;

	protected bool castingRedBuster;

	protected bool castingDualHeal;

	protected int firstAvail;

	protected int niceActIndex;

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

	public static readonly string[] PARTYMEMBER_NAMES = new string[5] { "Kris", "Susie", "Noelle", "Paula", "Chara" };

	protected DescriptionBox descriptionBox;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("act_check", new string[1] { "Check" });
		dictionary.Add("error_acts", new string[2] { "* But it didn't exist.\n* So this is an error.", "* But nothing happened.\n* So this is an error." });
		dictionary.Add("check_desc_base", new string[1] { "* {0} - ATK {1} DEF {2}\n{3}" });
		return dictionary;
	}

	protected virtual void Awake()
	{
		SetStrings(GetDefaultStrings(), GetType());
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
		gm = UnityEngine.Object.FindObjectOfType<GameManager>();
		cam = UnityEngine.Object.FindObjectOfType<BattleCamera>();
		mus = GetComponent<MusicPlayer>();
		aud = base.gameObject.AddComponent<AudioSource>();
		aud2 = base.gameObject.AddComponent<AudioSource>();
		bb = UnityEngine.Object.FindObjectOfType<BulletBoard>();
		fadeObj = GameObject.Find("BattleFadeObj").GetComponentInChildren<Fade>();
		bg = UnityEngine.Object.FindObjectOfType<BattleBG>();
		st = base.gameObject.AddComponent<ShakingText>();
		boxText = base.gameObject.AddComponent<TextUT>();
		boxText.SetParent(GameObject.Find("BattleCanvas").transform);
		soul = GameObject.Find("SOUL");
		soul.GetComponent<SOUL>().AdjustSOULColor();
		curHP = gm.GetCombinedHP();
		partyPanels = UnityEngine.Object.FindObjectOfType<PartyPanels>();
		ChangeHP();
		partySize = partyPanels.NumOfActivePartyMembers();
		krisAndSusie = gm.SusieInParty();
		partyTurn = 0;
		state = 0;
		actChoice = 0;
		selTarget = 0;
		buttonIndex = 0;
		SelectButton(buttonIndex);
		axisIsDown = false;
		descriptionBox = UnityEngine.Object.FindObjectOfType<DescriptionBox>();
		if (!gm.IsEasyMode())
		{
			didSoulSparkle = true;
		}
		isSOULOut = false;
	}

	public virtual void StartBattle(int id)
	{
		battleId = id;
		Initialize();
		enemies = EnemyGenerator.GetEnemies(battleId);
		object[] music = EnemyGenerator.GetMusic(battleId);
		PlayMusic(music[0].ToString(), float.Parse(music[1].ToString()));
		object[] battleBG = EnemyGenerator.GetBattleBG(battleId);
		bg.StartBG(int.Parse(battleBG[0].ToString()), float.Parse(battleBG[1].ToString()), float.Parse(battleBG[2].ToString()), (Color)battleBG[3], (bool)battleBG[4]);
		curFlavor = EnemyGenerator.GetApproachText(battleId);
		isBoss = battleId == 14 || battleId == 29 || battleId == 40 || battleId == 52 || battleId == 53 || battleId == 54 || battleId == 73;
		miniPartyMember = gm.GetMiniPartyMember();
		if (isBoss || (int)gm.GetFlag(13) >= 5 || ((int)gm.GetFlag(13) == 4 && (int)gm.GetFlag(87) == 4))
		{
			partyPanels.SetSprite(1, "spr_su_down_unhappy_0");
			partyPanels.SetSprite(2, "spr_no_down_unhappy_0");
		}
		if (gm.GetFlagInt(107) == 1)
		{
			if (gm.GetFlagInt(13) >= 2 && gm.GetFlagInt(127) == 1)
			{
				partyPanels.SetSprite(0, "g/spr_fr_down_0_g");
			}
		}
		else
		{
			if ((int)gm.GetFlag(87) >= 4)
			{
				partyPanels.SetSprite(2, "spr_no_down_unhappy_0");
			}
			if ((int)gm.GetFlag(102) == 1)
			{
				partyPanels.SetSprite(0, "injured/spr_kr_down_0_injured");
			}
			if ((int)gm.GetFlag(204) == 1)
			{
				partyPanels.SetSprite(0, "eye/spr_kr_down_0_eye");
			}
		}
		if (battleId == 1)
		{
			partyPanels.RaiseHeads(false, false, false);
			state = 5;
			soul.GetComponent<SpriteRenderer>().enabled = true;
			partyPanels.SetTargets(true, true, false);
			curAtk = AttackSpawner.GetAttack(3);
			bb.StartMovement(curAtk.GetBoardSize(), curAtk.GetBoardPos(), true);
			soul.transform.position = curAtk.GetSoulPos();
		}
		if (battleId == 14 || battleId == 40)
		{
			partyPanels.RaiseHeads(false, false, false);
			state = 5;
			soul.GetComponent<SpriteRenderer>().enabled = true;
			curAtk = AttackSpawner.GetAttack(22);
			soul.transform.position = curAtk.GetSoulPos();
			firstButton = true;
			SelectButton(-1);
		}
		if (battleId == 28)
		{
			partyPanels.RaiseHeads(false, false, false);
			state = 5;
			soul.GetComponent<SpriteRenderer>().enabled = false;
			curAtk = AttackSpawner.GetAttack(37);
			soul.transform.position = curAtk.GetSoulPos();
			firstButton = true;
			SelectButton(-1);
		}
		if (battleId == 29)
		{
			partyPanels.RaiseHeads(false, false, false);
			state = 5;
			soul.GetComponent<SpriteRenderer>().enabled = true;
			soul.GetComponent<SOUL>().CreateSOUL(Color.white, true, false);
			StopMusic();
			partyPanels.SetTargets(true, true, false);
			curAtk = AttackSpawner.GetAttack(40);
			bb.StartMovement(curAtk.GetBoardSize(), curAtk.GetBoardPos(), true);
			soul.transform.position = curAtk.GetSoulPos();
			firstButton = true;
			SelectButton(-1);
		}
		if (battleId == 73)
		{
			partyPanels.RaiseHeads(false, false, false);
			state = 5;
			soul.GetComponent<SpriteRenderer>().enabled = false;
			partyPanels.SetTargets(true, true, true);
			curAtk = AttackSpawner.GetAttack(108);
			soul.transform.position = curAtk.GetSoulPos();
			firstButton = true;
			SelectButton(-1);
			if (gm.GetEXP() == 0 && Util.GameManager().IsEasyMode())
			{
				soul.GetComponent<SOUL>().SetInvFrames(60, true);
				soul.GetComponent<SOUL>().SetDamageMultiplier(0.725f);
			}
		}
		if (state == 5)
		{
			SendBattleEvents(4);
		}
		startedBattle = true;
		UnityEngine.Object.FindObjectOfType<GameManager>().ForceTogglePlayers(false);
		DetermineDepressionReject();
		if (state == 0)
		{
			DoSOULSparkle();
		}
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
		int num = gm.GetHP(0);
		float num2 = gm.GetMaxHP(0);
		int num3 = 0;
		if (gm.GetHP(0) - gm.GetMiniMemberMaxHP() < 0)
		{
			num3 = 1;
		}
		if (gm.SusieInParty())
		{
			num += gm.GetHP(1);
			num2 += (float)gm.GetMaxHP(1);
			if (gm.GetHP(1) <= 0)
			{
				num3++;
			}
		}
		if (gm.NoelleInParty())
		{
			num += gm.GetHP(2);
			num2 += (float)gm.GetMaxHP(2);
			if (gm.GetHP(2) <= 0)
			{
				num3++;
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
			else if (num3 >= 3)
			{
				num4 = 75;
			}
			if (isBoss)
			{
				num4 = num4 * 2 / 3;
			}
			st.StartShake((int)((float)num / num2 * (float)num4));
		}
		if (state == 0 && gm.GetHP(partyTurn) == 0)
		{
			DecideMemberAction(0, -1, 0);
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
			if ((UTInput.GetButton("X") || UTInput.GetButton("C") || flavorPlayedOnce) && boxText.IsPlaying())
			{
				boxText.SkipText(false);
				flavorPlayedOnce = true;
			}
			soul.GetComponent<SOUL>().SetFrozen(true);
			soul.GetComponent<SpriteRenderer>().enabled = true;
			if (partyTurn == 0 && GameObject.Find("ACT").GetComponent<BattleButton>().GetButtonType() != "act")
			{
				GameObject.Find("ACT").GetComponent<BattleButton>().ChangeButtonType("act");
			}
			else if ((partyTurn == 1 || partyTurn == 2) && GameObject.Find("ACT").GetComponent<BattleButton>().GetButtonType() != "magic")
			{
				GameObject.Find("ACT").GetComponent<BattleButton>().ChangeButtonType("magic");
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
				bool flag = true;
				string[,] array = new string[4, 2];
				string[,] array2 = new string[3, 2];
				int i = 0;
				int num5 = 0;
				bool flag2 = false;
				bool flag3 = false;
				selObj = new GameObject("SelectTier1");
				selObj.layer = 5;
				selObj.AddComponent<RectTransform>();
				selObj.transform.SetParent(GameObject.Find("BattleCanvas").transform);
				selObj2 = new GameObject("SelectTier2");
				selObj2.layer = 5;
				selObj2.AddComponent<RectTransform>();
				selObj2.transform.SetParent(GameObject.Find("BattleCanvas").transform);
				firstAvail = -1;
				if (buttonIndex == 0)
				{
					array = GetEnemyListArray();
					DrawEnemyBars(selObj);
					flag3 = true;
					if (buttonIndex == 1 && gm.IsTestMode())
					{
						array[3, 0] = " ";
					}
					flag = false;
				}
				else if (buttonIndex == 1 && partyTurn == 0)
				{
					if (miniPartyMember != 1)
					{
						array = GetEnemyListArray();
						DrawEnemyBars(selObj);
						flag3 = true;
						if (buttonIndex == 1 && gm.IsTestMode())
						{
							array[3, 0] = " ";
						}
						flag = false;
					}
					else if (!gm.KrisInControl())
					{
						selectingMagic = true;
						array = GetPSISpells();
						flag = false;
					}
					else
					{
						actMagicSelect = true;
						array[0, 0] = "<color=#69FFFFFF>  ACT</color>";
						array[0, 1] = "<color=#FF6969FF>  PSI</color>";
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/KrisIcon"), selObj.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8f, 94f);
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/PaulaIcon"), selObj.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(248f, 94f);
						flag = false;
					}
				}
				else if (buttonIndex == 1 && partyTurn > 0)
				{
					selectingMagic = true;
					string text = ((partyTurn == 1) ? EnemyBase.SACTION_DEFAULT : EnemyBase.NACTION_DEFAULT);
					int num6 = 0;
					int num7 = 0;
					for (int j = 0; j < enemies.Length; j++)
					{
						if (!enemies[j].IsDone())
						{
							num6++;
							num7 = j;
						}
					}
					if (num6 == 1)
					{
						text = ((partyTurn == 1) ? enemies[num7].GetSActionName() : enemies[num7].GetNActionName());
					}
					array[0, 0] = ((partyTurn == 1) ? "<color=#FF69FFFF>* " : "<color=#FFFF69FF>* ") + text + "</color>";
					if (partyTurn == 1)
					{
						array[0, 1] = "* Rude Buster";
						array[1, 0] = "* UltimateHeal";
					}
					else if (partyTurn == 2)
					{
						array[0, 1] = "* Sleep Mist";
						array[1, 0] = "* HealPrayer";
						array[1, 1] = "* Ice Shock";
						if (Items.GetItemElement(gm.GetWeapon(2)) != 1)
						{
							array[0, 1] = "<color=#888888FF>* Sleep Mist</color>";
							array[1, 1] = "<color=#888888FF>* Ice Shock</color>";
						}
						else
						{
							for (; i < enemies.Length; i++)
							{
								if (enemies[i].IsTired() && !enemies[i].IsDone())
								{
									array[0, 1] = "<color=#00A2E8FF>* Sleep Mist</color>";
								}
							}
						}
					}
					flag = false;
				}
				else if (buttonIndex == 2)
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
					List<int> itemListPerTurn = GetItemListPerTurn();
					doPage2 = false;
					foreach (int item in itemListPerTurn)
					{
						if (item == -1)
						{
							continue;
						}
						flag = false;
						if (flag2)
						{
							doPage2 = true;
							array2[i, num5] = "* " + Items.ShortItemName(item, isBoss);
						}
						else
						{
							array[i, num5] = "* " + Items.ShortItemName(item, isBoss);
						}
						num5++;
						if (num5 == 2)
						{
							num5 = 0;
							i++;
							if (i == 2)
							{
								i = 0;
								flag2 = true;
							}
						}
					}
				}
				else if (buttonIndex == 3)
				{
					array[0, 0] = "* Spare";
					bool flag4 = false;
					for (int k = 0; k < enemies.Length; k++)
					{
						if (enemies[k].CanSpare() && !enemies[k].IsDone())
						{
							flag4 = true;
						}
					}
					if (flag4)
					{
						array[0, 0] = "<color=#ffff00ff>* Spare</color>";
					}
					array[1, 0] = "* Defend";
					if (gm.IsTestMode())
					{
						array[2, 0] = "* Flee (DEBUG)";
					}
					flag = false;
				}
				for (num5 = 0; num5 <= 1; num5++)
				{
					for (i = 0; i <= 2; i++)
					{
						if (array[i, num5] == null)
						{
							array[i, num5] = "";
						}
					}
				}
				boxText.SkipText(false);
				if (!flag)
				{
					flavorPlayedOnce = true;
					if (firstAvail == -1)
					{
						firstAvail = 0;
					}
					selObj.AddComponent<Selection>().CreateSelections(array, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", true, true, this, 0);
					selObj.transform.localScale = new Vector2(1f, 1f);
					selObj.GetComponent<Selection>().SetSelection(new Vector2(firstAvail, 0f), false);
					selObj2.AddComponent<Selection>().CreateSelections(array2, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", true, true, this, 1);
					selObj2.transform.localScale = new Vector2(1f, 1f);
					selObj2.GetComponent<Selection>().Disable();
					selObj2.SetActive(false);
					if (flag3)
					{
						HandleEnemyNameColor();
					}
					ResetText();
					state = 1;
				}
				else
				{
					UnityEngine.Object.Destroy(selObj);
				}
				aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
				aud.Play();
			}
			else if (UTInput.GetButtonDown("X") && partyTurn != 0)
			{
				int num8 = partyTurn;
				if (partySize == 2 && gm.GetHP(0) > 0)
				{
					partyTurn = 0;
				}
				else if ((gm.GetHP(1) == 0 || partySelections[1][1] == 4) && gm.GetHP(0) > 0)
				{
					partyTurn -= 2;
				}
				else if (((gm.GetHP(0) == 0 && gm.GetHP(1) > 0 && partyTurn == 2) || gm.GetHP(0) != 0) && partySize == 3)
				{
					partyTurn--;
				}
				if (num8 != partyTurn)
				{
					partyPanels.DeselectedAction(partyTurn);
					UnityEngine.Object.FindObjectOfType<TPBar>().SetSpecificTPUse(partyTurn, 0);
					int num9 = buttonIndex;
					buttonIndex = partySelections[partyTurn][1];
					if (buttonIndex == 6)
					{
						buttonIndex = 1;
					}
					if (num9 != buttonIndex)
					{
						firstButton = true;
					}
					SelectButton(buttonIndex);
					if (partySelections[partyTurn][1] == 3 && partySelections[partyTurn][2] == 1)
					{
						UnityEngine.Object.FindObjectOfType<TPBar>().SetDefendingMember(partyTurn, false);
						partyPanels.SetAsDefending(partyTurn, false);
						defending[partyTurn] = false;
					}
					if (partyTurn == 0)
					{
						if (partySelections[1][1] == 4)
						{
							partyPanels.DeselectedAction(1);
							partySelections[1][1] = -1;
						}
						if (partySelections[2][1] == 4)
						{
							partyPanels.DeselectedAction(2);
							partySelections[2][1] = -1;
						}
					}
				}
			}
		}
		if (state == 1)
		{
			if (buttonIndex == 2 && UTInput.GetAxisRaw("Horizontal") == 1f && selObj.GetComponent<Selection>().GetIndex()[1] == 1f && doPage2 && gm.GetItem(4 + 2 * (int)selObj.GetComponent<Selection>().GetIndex()[0]) != -1 && !selObj.GetComponent<Selection>().AxisDown())
			{
				Vector2 index = selObj.GetComponent<Selection>().GetIndex();
				if (GetItemListPerTurn().Count - 4 > 2)
				{
					index -= new Vector2(0f, 1f);
				}
				else
				{
					index -= new Vector2((index.x == 1f) ? 1 : 0, 1f);
				}
				selObj.GetComponent<Selection>().Disable();
				selObj.SetActive(false);
				selObj2.SetActive(true);
				selObj2.GetComponent<Selection>().Enable();
				selObj2.GetComponent<Selection>().SetSelection(index);
				selObj2.GetComponent<Selection>().SetAxisDown(true);
				gm.PlayGlobalSFX("sounds/snd_menumove");
				state = 2;
			}
			if (UTInput.GetButtonDown("X"))
			{
				UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(0);
				descriptionBox.Hide();
				UnityEngine.Object.Destroy(selObj);
				UnityEngine.Object.Destroy(selObj2);
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
				selObj2.SetActive(false);
				selObj.SetActive(true);
				selObj.GetComponent<Selection>().Enable();
				selObj.GetComponent<Selection>().SetSelection(selection);
				selObj.GetComponent<Selection>().SetAxisDown(true);
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
						UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(0);
					}
				}
				if (buttonIndex == 2)
				{
					UnityEngine.Object.Destroy(selObj);
					UnityEngine.Object.Destroy(selObj2);
					state = 0;
					SelectButton(buttonIndex);
					descriptionBox.Hide();
				}
				else
				{
					selObj2.SetActive(false);
					selObj.SetActive(true);
					state = 1;
				}
			}
		}
		if (state == 3)
		{
			if (!boxText.IsPlaying() && (bool)UnityEngine.Object.FindObjectOfType<SpecialACT>() && !UnityEngine.Object.FindObjectOfType<SpecialACT>().IsActivated())
			{
				UnityEngine.Object.FindObjectOfType<SpecialACT>().Activate();
			}
			if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && boxText.IsPlaying())
			{
				boxText.SkipText();
				if ((bool)UnityEngine.Object.FindObjectOfType<SpecialACT>())
				{
					UnityEngine.Object.FindObjectOfType<SpecialACT>().Activate();
				}
			}
			else if ((((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !boxText.IsPlaying()) || !boxText.GetGameObject()) && (!UnityEngine.Object.FindObjectOfType<SpecialACT>() || !UnityEngine.Object.FindObjectOfType<SpecialACT>().IsActivated()))
			{
				bool flag5 = false;
				if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && (bool)boxText.GetGameObject())
				{
					curDiag++;
					flag5 = true;
					if (!UnityEngine.Object.FindObjectOfType<SpecialACT>())
					{
						ResetText();
					}
				}
				bool flag6 = true;
				EnemyBase[] array3 = enemies;
				for (int m = 0; m < array3.Length; m++)
				{
					if (array3[m].IsShaking())
					{
						flag6 = false;
					}
				}
				if ((!boxText.Exists() || flag5) && !UnityEngine.Object.FindObjectOfType<SpecialAttackEffect>() && flag6)
				{
					if (curDiag > finalDiag)
					{
						if (boxText.Exists())
						{
							ResetText();
						}
						if (!UnityEngine.Object.FindObjectOfType<SpecialACT>())
						{
							if (niceActIndex < 3 || (niceActIndex == 3 && (fightingThisRound || sparingThisRound)))
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
						StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
						if (curDiag == 1 && castingRedBuster)
						{
							UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/RedBuster")).GetComponent<RedBusterEffect>().AssignEnemy(enemies[partySelections[0][0]]);
							castingRedBuster = false;
							if (miniPartyMember > 0)
							{
								fightingThisRound = true;
								partySelections[0][2] = 2;
								partySelections[0][1] = 0;
								partySelections[0][0] = 0;
							}
						}
						else if (curDiag == 1 && castingDualHeal)
						{
							castingDualHeal = false;
							int num10 = gm.GetMaxHP(2) / 2 + Mathf.FloorToInt(gm.GetMagicRaw(2) * 2f / 3f);
							if (Items.GetItemElement(gm.GetWeapon(2)) == 1)
							{
								int num11 = gm.GetMagicEquipment(2);
								if (Items.GetWeaponType(gm.GetWeapon(2)) == 4)
								{
									num11 = num11 * 2 / 3;
								}
								num10 += num11;
							}
							gm.HealAll(num10);
							gm.PlayTimedHealSound();
							aud2.clip = Resources.Load<AudioClip>("sounds/snd_spellcast");
							aud2.Play();
						}
					}
				}
			}
		}
		if (state == 7 && !target.GetComponentInChildren<FightTarget>().IsGoing() && !UnityEngine.Object.FindObjectOfType<SpecialAttackEffect>())
		{
			soul.GetComponent<SpriteRenderer>().enabled = true;
			AdvanceToEnemyTurn();
		}
		if (state == 4)
		{
			bool flag7 = false;
			EnemyBase[] array3 = enemies;
			for (int m = 0; m < array3.Length; m++)
			{
				if (array3[m].IsTalking())
				{
					flag7 = true;
				}
			}
			if (!bb.IsPlaying() && !flag7)
			{
				soul.GetComponent<SOUL>().SetFrozen(false);
				state = 5;
			}
		}
		if (state == 5 && !bb.IsPlaying())
		{
			if (curAtk == null)
			{
				soul.GetComponent<SOUL>().SetControllable(false);
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
			bool flag8 = false;
			for (int n = 0; n < 3; n++)
			{
				if (gm.GetHP(n) <= 0)
				{
					flag8 = true;
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
			if (flag8)
			{
				gm.PlayGlobalSFX("sounds/snd_heal");
			}
			ChangeHP();
			flavorPlayedOnce = false;
			defending = new bool[3];
			partyPanels.SetAsDefending(0, false);
			partyPanels.SetAsDefending(1, false);
			partyPanels.SetAsDefending(2, false);
			if (AllEnemiesDone())
			{
				bb.SetBGOrder(100);
				EndNormalFight(false, "");
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
				DoSOULSparkle();
			}
		}
		if (state == 10)
		{
			if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && boxText.IsPlaying())
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
				component.Select(true);
			}
			else
			{
				component.Select(false);
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
			for (int i = 0; i < 3; i++)
			{
				if (gm.GetHP(i) == 0 && revivalTurns[i] == 0)
				{
					revivalTurns[i] = 4;
				}
			}
			curHP = gm.GetCombinedHP();
			ChangeHP();
		}
		ChangeACTTPCost();
		if ((state == 1 || state == 2) && buttonIndex == 2 && (bool)selObj.transform.Find("PAGE1"))
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
				string battleDescription = Items.GetBattleDescription(GetItemListPerTurn()[num]);
				descriptionBox.SetDescription(battleDescription, "");
			}
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
	}

	private void ChangeACTTPCost()
	{
		if (state == 1 && buttonIndex == 1 && selectingMagic)
		{
			Dictionary<Vector2, string> dictionary = new Dictionary<Vector2, string>();
			string text = "`";
			if (partyTurn == 1)
			{
				dictionary = new Dictionary<Vector2, string>
				{
					{
						new Vector2(0f, 1f),
						"Deals RUDE Damage`50"
					},
					{
						new Vector2(1f, 0f),
						"The best healing`100"
					}
				};
			}
			else if (partyTurn == 2)
			{
				dictionary = new Dictionary<Vector2, string>
				{
					{
						new Vector2(0f, 1f),
						"Spares TIRED Enemies`32"
					},
					{
						new Vector2(1f, 0f),
						"Uses LIGHT to heal`32"
					},
					{
						new Vector2(1f, 1f),
						"Deals ICE Damage`24"
					}
				};
			}
			else if (partyTurn == 0)
			{
				dictionary = new Dictionary<Vector2, string>
				{
					{
						new Vector2(0f, 0f),
						"Heals 15 HP`24"
					},
					{
						new Vector2(0f, 1f),
						"Creates a LIGHT shield`50"
					},
					{
						new Vector2(1f, 0f),
						"Deals ICE Damage`24"
					},
					{
						new Vector2(1f, 1f),
						"Deals all FIRE Damage`36"
					}
				};
			}
			if (dictionary.ContainsKey(selObj.GetComponent<Selection>().GetIndex()))
			{
				text = dictionary[selObj.GetComponent<Selection>().GetIndex()];
			}
			string[] array = text.Split('`');
			if (array[1].Length != 0)
			{
				int tpPreview = int.Parse(array[1]);
				UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(tpPreview);
				array[1] += "% TP";
			}
			else
			{
				UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(0);
			}
			descriptionBox.SetDescription(array[0], array[1]);
		}
		if (state != 2 || buttonIndex != 1 || partyTurn != 0 || selectingMagic || selTarget <= -1 || selTarget >= enemies.Length)
		{
			return;
		}
		int num = (int)selObj2.GetComponent<Selection>().GetIndex()[1] + (int)selObj2.GetComponent<Selection>().GetIndex()[0] * 2;
		string text2 = enemies[selTarget].GetActNames()[num];
		if (text2 == null)
		{
			return;
		}
		if (text2.Contains(";"))
		{
			string[] array2 = text2.Substring(text2.IndexOf(";") + 1).Split('`');
			if (array2[1].Length != 0)
			{
				UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(int.Parse(array2[1]));
				array2[1] += "% TP";
			}
			else
			{
				UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(0);
			}
			descriptionBox.SetDescription(array2[0], array2[1]);
		}
		else
		{
			descriptionBox.Hide();
			UnityEngine.Object.FindObjectOfType<TPBar>().UpdateTPPreviewBar(0);
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
			DecideMemberAction(selTarget, 0, 0);
			aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
			aud.Play();
		}
		if (buttonIndex == 1)
		{
			ChangeACTTPCost();
			bool flag = true;
			if (partyTurn == 2)
			{
				int num = (int)index[0] * 2 + (int)index[1];
				if ((num == 1 || num == 3) && Items.GetItemElement(gm.GetWeapon(2)) != 1)
				{
					flag = false;
				}
			}
			if (id == 0 && partyTurn == 0 && actMagicSelect)
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
				string[,] array;
				if (index == Vector2.zero)
				{
					array = GetEnemyListArray();
					DrawEnemyBars(selObj);
					flag2 = true;
					if (buttonIndex == 1 && gm.IsTestMode())
					{
						array[3, 0] = " ";
					}
				}
				else
				{
					selectingMagic = true;
					array = GetPSISpells();
				}
				if (firstAvail == -1)
				{
					firstAvail = 0;
				}
				selObj.GetComponent<Selection>().CreateSelections(array, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", true, true, this, 0);
				selObj.transform.localScale = new Vector2(1f, 1f);
				selObj.GetComponent<Selection>().SetSelection(new Vector2(firstAvail, 0f), false);
				if (flag2)
				{
					HandleEnemyNameColor();
				}
				aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
				aud.Play();
			}
			else if (id == 0 && UnityEngine.Object.FindObjectOfType<TPBar>().ValidTPAmount() && flag)
			{
				selTarget = ((partyTurn == 0 && !selectingMagic) ? ((int)index[0]) : ((int)index[0] * 2 + (int)index[1]));
				int num2 = 0;
				int num3 = 0;
				bool flag3 = false;
				selObj.SetActive(false);
				selObj2.SetActive(true);
				selObj2.GetComponent<Selection>().Reset();
				int childCount2 = selObj2.transform.childCount;
				for (int j = 0; j < childCount2; j++)
				{
					UnityEngine.Object.DestroyImmediate(selObj2.transform.GetChild(0).gameObject);
				}
				string[,] array2 = new string[4, 2];
				firstAvail = -1;
				bool flag4 = false;
				if ((int)index[0] == 3)
				{
					array2[0, 0] = "* Godmode";
					array2[0, 1] = "* SwapSOULMode";
					array2[1, 0] = "* -1 HP Player";
					array2[1, 1] = "* +1 HP Player";
					array2[2, 0] = "* -25 DMG Enmy";
					array2[2, 1] = "* TestHUD";
					array2[3, 0] = "* +25 HP Enmy";
					array2[3, 1] = "* Max TP";
				}
				else if (partyTurn == 0 && !selectingMagic)
				{
					string[] actNames = enemies[(int)index[0]].GetActNames();
					for (int k = 0; k < actNames.Length; k++)
					{
						string text = actNames[k];
						array2[num2, num3] = "* " + text;
						if (text != null)
						{
							if (text.Contains(";"))
							{
								text = text.Substring(0, text.IndexOf(';'));
							}
							bool flag5 = text.StartsWith("S!") && gm.SusieInParty() && gm.GetHP(1) > 0;
							bool flag6 = text.StartsWith("N!") && gm.NoelleInParty() && gm.GetHP(2) > 0;
							bool flag7 = text.StartsWith("SN!") && partySize == 3 && gm.GetHP(1) > 0 && gm.GetHP(2) > 0;
							bool flag8 = text.StartsWith("KS!") && gm.SusieInParty() && gm.GetHP(1) > 0 && gm.KrisInControl() && gm.GetHP(0) > 0 && (int)gm.GetFlag(107) == 0;
							if (text.StartsWith("S!"))
							{
								if (flag5)
								{
									array2[num2, num3] = "  <color=#FF69FFFF>" + text.Replace("S!", "") + "</color>";
								}
								else
								{
									array2[num2, num3] = "  <color=#888888FF>" + text.Replace("S!", "") + "</color>";
								}
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SusieIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * num3, 94 + -32 * num2);
							}
							else if (text.StartsWith("N!"))
							{
								if (flag6)
								{
									array2[num2, num3] = "  <color=#FFFF69FF>" + text.Replace("N!", "") + "</color>";
								}
								else
								{
									array2[num2, num3] = "  <color=#888888FF>" + text.Replace("N!", "") + "</color>";
								}
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/NoelleIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * num3, 94 + -32 * num2);
							}
							else if (text.StartsWith("SN!"))
							{
								if (flag7)
								{
									array2[num2, num3] = "    " + text.Replace("SN!", "");
								}
								else
								{
									array2[num2, num3] = "    <color=#888888FF>" + text.Replace("SN!", "") + "</color>";
								}
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SusieIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * num3, 94 + -32 * num2);
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/NoelleIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(42 + 240 * num3, 94 + -32 * num2);
							}
							else if (text.StartsWith("KS!"))
							{
								if (flag8)
								{
									array2[num2, num3] = "    " + text.Replace("KS!", "");
								}
								else
								{
									array2[num2, num3] = "    <color=#888888FF>" + text.Replace("KS!", "") + "</color>";
								}
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/KrisIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(8 + 240 * num3, 94 + -32 * num2);
								UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SusieIcon"), selObj2.transform).transform.localPosition = new Vector3(-220f, -177f) + new Vector3(42 + 240 * num3, 94 + -32 * num2);
							}
							else
							{
								array2[num2, num3] = "* " + text;
							}
						}
						num3++;
						if (num3 == 2)
						{
							num3 = 0;
							num2++;
							if (num2 == 3)
							{
								break;
							}
						}
					}
				}
				else if (partyTurn > 0)
				{
					if (selTarget == 2)
					{
						array2 = GetMemberListArray();
						DrawMemberBars(selObj2);
					}
					else if (selTarget == 0 || (selTarget == 1 && partyTurn == 1) || (selTarget == 3 && partyTurn == 2))
					{
						array2 = GetEnemyListArray();
						DrawEnemyBars(selObj2);
						flag3 = true;
					}
					else if (selTarget == 1 && partyTurn == 2)
					{
						flag4 = true;
					}
					else
					{
						array2[0, 0] = "* Temp";
					}
				}
				else if (selTarget == 0)
				{
					array2 = GetMemberListArray();
					DrawMemberBars(selObj2);
				}
				else if (selTarget == 2)
				{
					array2 = GetEnemyListArray();
					DrawEnemyBars(selObj2);
					flag3 = true;
				}
				else
				{
					flag4 = true;
				}
				for (num3 = 0; num3 <= 1; num3++)
				{
					for (num2 = 0; num2 <= 2; num2++)
					{
						if (array2[num2, num3] == null || array2[num2, num3] == "* ")
						{
							array2[num2, num3] = "";
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
					if (partyTurn > 0)
					{
						DecideMemberAction(0, 1, selTarget);
					}
					else
					{
						DecideMemberAction(0, 6, selTarget);
					}
				}
				else
				{
					selObj2.GetComponent<Selection>().CreateSelections(array2, new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", true, true, this, origId);
					selObj2.transform.localScale = new Vector2(1f, 1f);
					selObj2.GetComponent<Selection>().SetSelection(new Vector2(firstAvail, 0f), false);
					state = 2;
					if (flag3)
					{
						HandleEnemyNameColor();
					}
				}
				aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
				aud.Play();
			}
			else if (id == 0 && (!UnityEngine.Object.FindObjectOfType<TPBar>().ValidTPAmount() || !flag))
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
					int num5 = (int)index[0] * 2 + (int)index[1];
					if (partyTurn == 0 && !selectingMagic)
					{
						string text2 = enemies[selTarget].GetActNames()[num5];
						bool num6 = !text2.StartsWith("S!") && !text2.StartsWith("N!") && !text2.StartsWith("SN!") && !text2.StartsWith("KS!");
						bool flag9 = text2.StartsWith("S!") && gm.SusieInParty() && gm.GetHP(1) > 0;
						bool flag10 = text2.StartsWith("N!") && gm.NoelleInParty() && gm.GetHP(2) > 0;
						bool flag11 = text2.StartsWith("SN!") && partySize == 3 && gm.GetHP(1) > 0 && gm.GetHP(2) > 0;
						bool flag12 = text2.StartsWith("KS!") && gm.SusieInParty() && gm.GetHP(1) > 0 && gm.KrisInControl() && gm.GetHP(0) > 0 && (int)gm.GetFlag(107) == 0;
						if ((num6 || flag9 || flag10 || flag11 || flag12) && UnityEngine.Object.FindObjectOfType<TPBar>().ValidTPAmount())
						{
							UnityEngine.Object.Destroy(selObj);
							UnityEngine.Object.Destroy(selObj2);
							DecideMemberAction(selTarget, 1, num5);
							if (text2.StartsWith("S!"))
							{
								DecideMemberAction(0, 4, 0);
							}
							if (text2.StartsWith("N!"))
							{
								if (partySize == 2 || gm.GetHP(1) == 0)
								{
									DecideMemberAction(0, 4, 0);
								}
								else
								{
									partySelections[2][1] = 4;
									partyPanels.SelectedAction(2);
								}
							}
							if (text2.StartsWith("SN!"))
							{
								DecideMemberAction(0, 4, 0);
								DecideMemberAction(0, 4, 0);
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
					}
					else
					{
						UnityEngine.Object.Destroy(selObj);
						UnityEngine.Object.Destroy(selObj2);
						if (partyTurn > 0)
						{
							DecideMemberAction(num5 / 2, 1, selTarget);
						}
						else
						{
							DecideMemberAction(num5 / 2, 6, selTarget);
						}
						aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
						aud.Play();
					}
					break;
				}
				case 2:
				{
					int num4 = (int)index[0] * 2 + (int)index[1];
					DebugTools.UseTool(DebugTools.GetKeys()[num4]);
					aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
					aud.Play();
					break;
				}
				}
			}
		}
		if (buttonIndex == 2)
		{
			bool flag13 = false;
			if (id != 2)
			{
				int num7 = (int)index[0] * 2 + (int)index[1];
				selObj.SetActive(true);
				selObj2.SetActive(false);
				state = 1;
				int num8 = num7 + 4 * id;
				partySelections[partyTurn][2] = num8;
				UnityEngine.Object.Destroy(selObj.transform.Find("PAGE1").gameObject);
				selObj.GetComponent<Selection>().Reset();
				if (partySize == 1 || Items.ItemType(gm.GetItem(num8)) == 4 || gm.GetItem(num8) == 45)
				{
					id = 2;
					flag13 = true;
				}
				else
				{
					selObj.GetComponent<Selection>().CreateSelections(GetMemberListArray(), new Vector2(-220f, -177f), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", true, true, this, 2);
					DrawMemberBars(selObj);
				}
			}
			if (id == 2)
			{
				UnityEngine.Object.Destroy(selObj);
				UnityEngine.Object.Destroy(selObj2);
				partySelections[partyTurn][0] = (int)index[0];
				if (!krisAndSusie && partySelections[partyTurn][0] == 1)
				{
					partySelections[partyTurn][0] = 2;
				}
				if (flag13)
				{
					partySelections[partyTurn][0] = 0;
				}
				DecideMemberAction(partySelections[partyTurn][0], 2, partySelections[partyTurn][2]);
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
				UnityEngine.Object.FindObjectOfType<TPBar>().SetDefendingMember(partyTurn, true);
				partyPanels.SetAsDefending(partyTurn, true);
				defending[partyTurn] = true;
			}
			if (index[0] == 2f)
			{
				gm.EndBattle(0, true);
				gm.EnablePlayerMovement();
			}
			DecideMemberAction(0, 3, (int)index[0]);
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
			if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(94) == 1)
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
		for (int i = 0; i < 3; i++)
		{
			if (i != 0 && (i != 1 || !gm.SusieInParty()) && (i != 2 || !gm.NoelleInParty()))
			{
				continue;
			}
			GameObject gameObject = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/enemies/FightEnemyHP"), selObj.transform);
			gameObject.name = "PartyMemberHP" + i;
			gameObject.transform.localPosition += new Vector3(80f, -32 * i - 36);
			gameObject.transform.Find("fg").GetComponent<RectTransform>().sizeDelta = new Vector2((float)gm.GetHP(i) / (float)gm.GetMaxHP(i) * 101f, 17f);
			gameObject.transform.Find("bg").GetComponent<RectTransform>().sizeDelta = new Vector2(101f, 17f);
			gameObject.transform.Find("Text").GetComponent<Text>().enabled = false;
			gameObject.transform.Find("TextShadow").GetComponent<Text>().enabled = false;
			if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(94) != 1)
			{
				continue;
			}
			Image[] componentsInChildren = gameObject.transform.Find("corners").GetComponentsInChildren<Image>();
			foreach (Image image in componentsInChildren)
			{
				image.enabled = true;
				if (image.gameObject.name.EndsWith("R"))
				{
					image.transform.localPosition = new Vector3(51f, image.transform.localPosition.y);
				}
			}
		}
	}

	protected string[,] GetMemberListArray()
	{
		string[,] array = new string[3, 2]
		{
			{ "* Kris", null },
			{ null, null },
			{ null, null }
		};
		if ((int)gm.GetFlag(108) == 1)
		{
			array[0, 0] = "* Frisk";
		}
		array[1, 0] = (gm.SusieInParty() ? "* Susie" : "");
		array[2, 0] = (gm.NoelleInParty() ? "* Noelle" : "");
		return array;
	}

	protected string[,] GetEnemyListArray()
	{
		string[,] array = new string[4, 2];
		for (int i = 0; i < enemies.Length; i++)
		{
			if (!enemies[i].IsDone())
			{
				if (firstAvail == -1)
				{
					firstAvail = i;
				}
				array[i, 0] = "* " + enemies[i].GetName();
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

	private string[,] GetPSISpells()
	{
		return new string[3, 2]
		{
			{ "* Lifeup", "* Shield" },
			{ "* PK Freeze", "* PK Fire" },
			{ null, null }
		};
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
				MonoBehaviour.print("Sending " + enemyBase.GetName() + " event for state " + state);
				switch (state)
				{
				case 4:
					enemyBase.EnemyTurnStart();
					break;
				case 6:
					enemyBase.EnemyTurnEnd();
					break;
				default:
					throw new InvalidOperationException("No event for state " + state);
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
			mus.ChangeMusic(flag ? music.Replace("_intro", "") : music, flag, true);
			mus.GetSource().pitch = pitch;
		}
		else if ((bool)UnityEngine.Object.FindObjectOfType<LostCoreMusic>())
		{
			UnityEngine.Object.FindObjectOfType<LostCoreMusic>().SetDanger(true);
		}
	}

	public void PlayMusic(string music, float pitch, bool hasIntro)
	{
		if (music != "" && music != mus.CurrentMusic())
		{
			mus.ChangeMusic(music, hasIntro, true);
			mus.GetSource().pitch = pitch;
		}
		else if ((bool)UnityEngine.Object.FindObjectOfType<LostCoreMusic>())
		{
			UnityEngine.Object.FindObjectOfType<LostCoreMusic>().SetDanger(true);
		}
	}

	public void StopMusic()
	{
		mus.Stop();
	}

	public void FadeEndBattle()
	{
		for (int i = 0; i < 3; i++)
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

	public virtual void DecideMemberAction(int target, int action, int extraData)
	{
		flavorPlayedOnce = true;
		partySelections[partyTurn] = new int[3] { target, action, extraData };
		if (action != -1)
		{
			partyPanels.SelectedAction(partyTurn);
		}
		switch (action)
		{
		case 1:
		case 6:
			descriptionBox.Hide();
			UnityEngine.Object.FindObjectOfType<TPBar>().ApplyPreviewTP(partyTurn);
			break;
		case 2:
			descriptionBox.Hide();
			break;
		}
		partyTurn++;
		if (partyTurn == 1 && gm.GetHP(1) == 0)
		{
			partySelections[1][1] = -1;
			partyTurn++;
		}
		if (partyTurn == 2 && gm.GetHP(2) == 0)
		{
			partySelections[2][1] = -1;
			partyTurn++;
		}
		if (partySelections[1][1] == 4 && partyTurn == 1)
		{
			partyTurn++;
		}
		if (partySelections[2][1] == 4 && partyTurn == 2)
		{
			partyTurn++;
		}
		MonoBehaviour.print(partySelections[2][1]);
		if (partyTurn >= 3 || (partySize == 2 && krisAndSusie && partyTurn >= 2) || partySize == 1)
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
			if (!gm.SusieInParty())
			{
				partySelections[1][1] = -1;
			}
			if (!gm.NoelleInParty())
			{
				partySelections[2][1] = -1;
			}
			niceActIndex = 0;
			partyPanels.RaiseHeads(false, false, false);
			state = 3;
			soul.GetComponent<SpriteRenderer>().enabled = false;
			soul.transform.position = new Vector3(500f, 500f);
			SelectButton(-1);
			fightingThisRound = false;
			UnityEngine.Object.FindObjectOfType<TPBar>().UseTP();
			AdvancePlayerTurn();
		}
		else
		{
			if (!krisAndSusie && partySize == 2 && partyTurn == 1)
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
		while (niceActIndex < 3)
		{
			bool flag4 = partySelections[niceActIndex][1] == 1 && (partySelections[niceActIndex][2] == 2 || partySelections[niceActIndex][2] == 0 || (partySelections[niceActIndex][2] == 1 && niceActIndex == 2));
			if (partySelections[niceActIndex][1] != -1 && partySelections[niceActIndex][1] < 2 && !flag4 && ((susieDepressionRefuse && niceActIndex == 1) || (noelleDepressionRefuse && niceActIndex == 2)))
			{
				flag3 = true;
				partySelections[niceActIndex][1] = -1;
				break;
			}
			if (partySelections[niceActIndex][1] != 0 && partySelections[niceActIndex][1] != 3 && partySelections[niceActIndex][1] != -1 && partySelections[niceActIndex][1] != 4)
			{
				break;
			}
			if (partySelections[niceActIndex][1] == 0)
			{
				if (niceActIndex != 0)
				{
					if (!enemies[partySelections[niceActIndex][0]].PartyMemberAcceptAttack(niceActIndex, 0))
					{
						flag2 = true;
						partySelections[niceActIndex][1] = -1;
						break;
					}
					fightingThisRound = true;
					if (niceActIndex == 1 && susieDeviousMisbehave)
					{
						break;
					}
				}
				else
				{
					if ((int)gm.GetFlag(102) == 1 && UnityEngine.Random.Range(0, 6) == 1)
					{
						flag = true;
						partySelections[niceActIndex][1] = -1;
						break;
					}
					fightingThisRound = true;
				}
			}
			if (partySelections[niceActIndex][1] == 3 && partySelections[niceActIndex][2] == 0)
			{
				sparingThisRound = true;
				sparers[niceActIndex] = true;
			}
			if (partySelections[niceActIndex][1] == 4)
			{
				partySelections[niceActIndex][1] = 5;
			}
			niceActIndex++;
			MonoBehaviour.print(niceActIndex);
			if (niceActIndex == 3)
			{
				break;
			}
		}
		string[] array = new string[3] { "* You", "* Susie", "* Noelle" };
		if (AllEnemiesDone())
		{
			EndNormalFight(false, "");
		}
		else if (!flag2 && susieDeviousMisbehave && niceActIndex == 1 && partySelections[niceActIndex][1] == 0)
		{
			partyPanels.RaiseHeads(false, true, false);
			diag = new string[1] { deviousString };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (flag3)
		{
			partyPanels.RaiseHeads(false, niceActIndex == 1, niceActIndex == 2);
			diag = new string[1] { array[niceActIndex] + " couldn't bring herself\n  to do anything." };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (flag)
		{
			partyPanels.RaiseHeads(true, false, false);
			string[] array2 = new string[4] { "* You felt light-headed and\n  couldn't draw your weapon.", "* You couldn't gather the\n  strength to fight.", "* You decided to listen to\n  the doctor and rested.", "* You collapsed to the ground\n  trying to draw your weapon." };
			diag = new string[1] { array2[UnityEngine.Random.Range(0, array2.Length)] };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (flag2)
		{
			partyPanels.RaiseHeads(false, niceActIndex == 1, niceActIndex == 2);
			diag = new string[1] { array[niceActIndex] + " refused to fight\n  " + enemies[partySelections[niceActIndex][0]].GetName() + "." };
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (niceActIndex == 3 && sparingThisRound)
		{
			sparingThisRound = false;
			partyPanels.RaiseHeads(partySelections[0][1] == 3 && partySelections[0][2] == 0, partySelections[1][1] == 3 && partySelections[1][2] == 0, partySelections[2][1] == 3 && partySelections[2][2] == 0);
			string text = "";
			bool flag5 = false;
			bool flag6 = false;
			if (sparers[0] && sparers[1] && sparers[2])
			{
				text = "* Everyone";
				sparers = new bool[3];
			}
			else
			{
				text = "*";
				if (sparers[0])
				{
					sparers[0] = false;
					text += " You";
					flag5 = true;
				}
				if (sparers[1])
				{
					sparers[1] = false;
					if (flag5)
					{
						flag6 = true;
						text += " and";
					}
					text += " Susie";
					flag5 = true;
				}
				if (sparers[2])
				{
					sparers[2] = false;
					if (flag5)
					{
						flag6 = true;
						text += " and";
					}
					text += " Noelle";
					flag5 = true;
				}
			}
			bool flag7 = false;
			int num = 0;
			for (int i = 0; i < enemies.Length; i++)
			{
				if (!enemies[i].IsDone())
				{
					num++;
				}
				if (enemies[i].CanSpare() && !enemies[i].IsDone())
				{
					enemies[i].Spare();
					if (flag7)
					{
						enemies[i].GetComponent<AudioSource>().Stop();
					}
					flag7 = true;
				}
				else if (!enemies[i].CanSpare() && !enemies[i].IsDone())
				{
					enemies[i].AttemptedSpare();
				}
			}
			string text2 = "* But none of the enemies'\n  names were <color=#FFFF00FF>YELLOW</color>...";
			if (num == 1)
			{
				text2 = "* But the enemy's name\n  wasn't <color=#FFFF00FF>YELLOW</color>...";
			}
			if (flag6)
			{
				if (flag7)
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
				if (!flag7)
				{
					string[] array3 = diag;
					int num2 = 0;
					array3[num2] = array3[num2] + "\n" + text2;
				}
			}
			curDiag = 0;
			finalDiag = diag.Length - 1;
			StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
		}
		else if (niceActIndex == 3 && fightingThisRound)
		{
			partyPanels.RaiseHeads(partySelections[0][1] == 0, partySelections[1][1] == 0, partySelections[2][1] == 0);
			target = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/FightTarget"));
			EnemyBase krisTarget = null;
			EnemyBase susieTarget = null;
			EnemyBase noelleTarget = null;
			if (partySelections[0][1] == 0)
			{
				krisTarget = enemies[partySelections[0][0]];
			}
			if (partySelections[1][1] == 0)
			{
				susieTarget = enemies[partySelections[1][0]];
			}
			if (partySelections[2][1] == 0)
			{
				noelleTarget = enemies[partySelections[2][0]];
			}
			target.GetComponent<FightTarget>().SetEnemies(krisTarget, susieTarget, noelleTarget);
			target.GetComponent<FightTarget>().SetAttackers(partySelections[0][1] == 0, partySelections[1][1] == 0, partySelections[2][1] == 0, partySize, partySelections[0][2]);
			state = 7;
		}
		else if (niceActIndex == 3 && !fightingThisRound)
		{
			AdvanceToEnemyTurn();
		}
		else
		{
			bool kris = niceActIndex == 0;
			bool susie = niceActIndex == 1 || (niceActIndex == 0 && partySelections[1][1] == 4);
			bool noelle = niceActIndex == 2 || (niceActIndex == 0 && partySelections[2][1] == 4);
			partyPanels.RaiseHeads(kris, susie, noelle);
			if (partySelections[niceActIndex][1] == 1)
			{
				if (niceActIndex == 0)
				{
					diag = enemies[partySelections[niceActIndex][0]].PerformAct(partySelections[niceActIndex][2]);
					if (diag[0] == "* Your SOUL shined its power\n  onto Susie!")
					{
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULShine"), new Vector3(partyPanels.transform.Find("KrisSprite").localPosition.x / 48f, -0.2f), Quaternion.identity);
						castingRedBuster = true;
						if ((int)gm.GetFlag(211) == 1)
						{
							partyPanels.SetSprite(0, "spr_kr_evil_look");
						}
					}
					else if (diag[0] == "* Your SOUL shined its power\n  onto Noelle!")
					{
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/SOULShine"), new Vector3(partyPanels.transform.Find("KrisSprite").localPosition.x / 48f, -0.2f), Quaternion.identity);
						castingDualHeal = true;
					}
				}
				else
				{
					int num3 = -1;
					if (susieDeviousMisbehave && niceActIndex == 1 && partySelections[niceActIndex][2] == 1)
					{
						num3 = UnityEngine.Random.Range(0, 5);
						if (num3 == 4)
						{
							partySelections[niceActIndex][2] = 2;
							UnityEngine.Object.FindObjectOfType<TPBar>().RemoveTP(100);
						}
						MonoBehaviour.print("SUSIE DEVIOUS RUDE BUSTER: " + num3);
					}
					if (partySelections[niceActIndex][2] == 0)
					{
						diag = enemies[partySelections[niceActIndex][0]].PerformAssistAct(niceActIndex);
					}
					else if (partySelections[niceActIndex][2] == 2)
					{
						string text3 = ((niceActIndex == 1) ? "ULTIMATE HEAL" : "HEAL PRAYER");
						int num4 = -1;
						if (susieDeviousMisbehave && niceActIndex == 1 && (num3 == 4 || UnityEngine.Random.Range(0, 1) == 0))
						{
							if (UnityEngine.Random.Range(0, 5) == 0)
							{
								if (num3 != 4)
								{
									MonoBehaviour.print("SUSIE DEVIOUS HEAL: random enemy");
								}
								for (int j = 0; j < enemies.Length; j++)
								{
									if (!enemies[j].IsDone())
									{
										num4 = j;
										break;
									}
								}
							}
							else
							{
								if (num3 != 4)
								{
									MonoBehaviour.print("SUSIE DEVIOUS HEAL: random party member");
								}
								partySelections[niceActIndex][0] = UnityEngine.Random.Range(0, partySize);
							}
						}
						string text4 = ((niceActIndex == partySelections[niceActIndex][0]) ? "herself." : (new string[3] { "you.", "Susie.", "Noelle." })[partySelections[niceActIndex][0]]);
						if (num4 >= 0)
						{
							text4 = enemies[num4].GetName() + ".";
						}
						int num5 = Mathf.FloorToInt(gm.GetMagicRaw(1));
						if (niceActIndex == 2)
						{
							num5 = gm.GetMaxHP(2) / 4 + Mathf.FloorToInt(gm.GetMagicRaw(2) / 2f);
							if (Items.GetItemElement(gm.GetWeapon(2)) == 1)
							{
								int num6 = gm.GetMagicEquipment(2);
								if (Items.GetWeaponType(gm.GetWeapon(2)) == 4)
								{
									num6 /= 2;
								}
								num5 += num6;
							}
						}
						else if (isBoss)
						{
							num5 += 3;
						}
						if (susieDeviousMisbehave)
						{
							string text5 = deviousString + array[niceActIndex] + " cast " + text3 + "\n  onto " + text4;
							if (num4 >= 0)
							{
								diag = new string[1] { text5 };
							}
							else
							{
								diag = new string[2]
								{
									text5,
									Items.GetRecoveryString(partySelections[niceActIndex][0], num5)
								};
							}
						}
						else
						{
							diag = new string[1] { array[niceActIndex] + " cast " + text3 + "\n  onto " + text4 + "\n" + Items.GetRecoveryString(partySelections[niceActIndex][0], num5) };
						}
						if (num4 >= 0)
						{
							enemies[num4].Hit(1, -num5, true);
						}
						else
						{
							gm.Heal(partySelections[niceActIndex][0], num5);
							gm.PlayTimedHealSound();
						}
						aud2.clip = Resources.Load<AudioClip>((niceActIndex == 1) ? "sounds/snd_spell_cure_slight_smaller" : "sounds/snd_spellcast");
						aud2.Play();
					}
					else if (partySelections[niceActIndex][2] == 1 && niceActIndex == 1)
					{
						if (!enemies[partySelections[niceActIndex][0]].PartyMemberAcceptAttack(niceActIndex, 1) || num3 == 1)
						{
							diag = new string[1] { "* Susie cast RUDE BUSTER...^10\n  onto the wall." };
						}
						else
						{
							RudeBusterEffect component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/RudeBuster")).GetComponent<RudeBusterEffect>();
							int num7 = partySelections[niceActIndex][0];
							if (enemies[num7].IsDone())
							{
								for (int k = 0; k < enemies.Length; k++)
								{
									if (!enemies[k].IsDone() && k != num7)
									{
										num7 = k;
										break;
									}
								}
							}
							component.AssignEnemy(enemies[num7]);
							if (num3 == 2 || num3 == 3)
							{
								component.SetDevious(num3 == 2);
							}
							diag = new string[1] { "* Susie cast RUDE BUSTER!" };
						}
						if (susieDeviousMisbehave)
						{
							diag[0] = deviousString + diag[0];
						}
					}
					else if (partySelections[niceActIndex][2] == 1 && niceActIndex == 2)
					{
						if (Items.GetItemElement(gm.GetWeapon(2)) == 1)
						{
							SleepMist component2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/SleepMist")).GetComponent<SleepMist>();
							diag = new string[1] { array[niceActIndex] + " cast SLEEP MIST!" };
							bool flag8 = false;
							int num8 = 0;
							for (int l = 0; l < enemies.Length; l++)
							{
								if (!enemies[l].IsDone())
								{
									num8++;
								}
								if (enemies[l].IsTired() && !enemies[l].IsDone())
								{
									enemies[l].Spare(true);
									if (flag8)
									{
										enemies[l].GetComponent<AudioSource>().Stop();
									}
									flag8 = true;
								}
								else if (!enemies[l].CanSpare() && !enemies[l].IsDone())
								{
									enemies[l].AttemptedSpare();
								}
							}
							string text6 = "* But none of the enemies\n  were <color=#00A2E8FF>TIRED</color>...";
							if (num8 == 1)
							{
								text6 = "* But the enemy wasn't\n  <color=#00A2E8FF>TIRED</color>...";
							}
							if (!flag8)
							{
								string[] array4 = diag;
								int num9 = 0;
								array4[num9] = array4[num9] + "\n" + text6;
							}
							else
							{
								component2.GetComponents<AudioSource>()[0].Play();
							}
						}
						else
						{
							diag = new string[1] { array[niceActIndex] + " tried SLEEP MIST,^05\n  but wasn't able to..." };
						}
					}
					else if (partySelections[niceActIndex][2] == 3 && niceActIndex == 2)
					{
						if (Items.GetItemElement(gm.GetWeapon(2)) == 1)
						{
							if (!enemies[partySelections[niceActIndex][0]].PartyMemberAcceptAttack(niceActIndex, 1))
							{
								diag = new string[1] { "* Noelle cast ICE SHOCK...^10\n  onto herself." };
								gm.PlayGlobalSFX("sounds/snd_hurt");
								gm.Damage(2, 5);
							}
							else
							{
								IceShock component3 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/IceShock")).GetComponent<IceShock>();
								int num10 = partySelections[niceActIndex][0];
								if (enemies[num10].IsDone())
								{
									for (int m = 0; m < enemies.Length; m++)
									{
										if (!enemies[m].IsDone())
										{
											num10 = m;
											break;
										}
									}
								}
								component3.AssignEnemy(enemies[num10]);
								diag = new string[1] { array[niceActIndex] + " cast ICE SHOCK!" };
							}
						}
						else
						{
							diag = new string[1] { array[niceActIndex] + " tried ICE SHOCK,^05\n  but wasn't able to..." };
						}
					}
					else
					{
						diag = new string[1] { "* not implemented yet lol" };
					}
					MonoBehaviour.print("Magic Index: " + partySelections[niceActIndex][2]);
				}
				curDiag = 0;
				finalDiag = diag.Length - 1;
				StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
			}
			else if (partySelections[niceActIndex][1] == 2)
			{
				int num11 = niceActIndex;
				int num12 = -1;
				if (susieDeviousMisbehave && Items.ItemType(gm.GetItem(partySelections[niceActIndex][2])) == 0 && niceActIndex == 1 && UnityEngine.Random.Range(0, 1) == 0)
				{
					if (UnityEngine.Random.Range(0, 5) == 0)
					{
						for (int n = 0; n < enemies.Length; n++)
						{
							if (!enemies[n].IsDone())
							{
								num12 = n;
								break;
							}
						}
					}
					else
					{
						partySelections[niceActIndex][0] = UnityEngine.Random.Range(0, partySize);
					}
				}
				int num13 = partySelections[niceActIndex][0];
				if (!gm.KrisInControl())
				{
					if (num11 == 0)
					{
						num11 = miniPartyMember + 2;
					}
					if (num13 == 0)
					{
						num13 = miniPartyMember + 2;
					}
				}
				if (num12 >= 0)
				{
					diag = new string[1] { deviousString + "* Susie gave the " + Items.ItemName(gm.GetItem(partySelections[niceActIndex][2])) + "\n  to " + enemies[num12].GetName() + "!" };
					enemies[num12].Hit(1, -Items.ItemValue(gm.GetItem(partySelections[niceActIndex][2])), true);
					gm.RemoveItem(partySelections[niceActIndex][2]);
				}
				else
				{
					bool flag9 = true;
					diag = Items.ItemUse(gm.GetItem(partySelections[niceActIndex][2]), num11, num13, isBoss).Split('}');
					if (gm.GetItem(partySelections[niceActIndex][2]) == 22 && soul.GetComponent<SOUL>().GetMaxSpeed() < 8f)
					{
						int num22 = 3;
						soul.GetComponent<SOUL>().IncrementSpeed();
						Vector3 position = partyPanels.GetStatPanel(num13).transform.localPosition / 48f;
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartWord("spdup", Color.white, position);
					}
					if (gm.GetItem(partySelections[niceActIndex][2]) == 24)
					{
						bool flag10 = false;
						if (gm.GetHP(1) == 0)
						{
							gm.Heal(1, gm.GetMaxHP(1));
							revivalTurns[1] = 0;
							partyPanels.UpdateHP(gm.GetHPArray());
							flag10 = true;
						}
						gm.SetATKBuff(1, 10);
						Vector3 position2 = partyPanels.GetStatPanel(1).transform.localPosition / 48f - new Vector3(0f, flag10 ? 0f : (-0.5f));
						UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/dr/DamageNumber"), new Vector3(10f, 0f), Quaternion.identity).GetComponent<DamageNumber>().StartWord("atup", Color.white, position2);
					}
					if (gm.GetItem(partySelections[niceActIndex][2]) == 45)
					{
						int num14 = 0;
						int num15 = 0;
						EnemyBase[] array5 = enemies;
						foreach (EnemyBase enemyBase in array5)
						{
							if ((bool)enemyBase && !enemyBase.IsDone())
							{
								num15++;
								if (enemyBase.CanBeSkipped())
								{
									num14++;
								}
							}
						}
						if (num14 == num15)
						{
							skipNextEnemyTurn = true;
							string[] array6 = new string[6] { "RED", "BLUE", "GREEN", "ORANGE", "CYAN", "YELLOW" };
							int num17 = niceActIndex;
							if (miniPartyMember > 0)
							{
								num17 = 2 + miniPartyMember;
							}
							string text7 = "* " + PARTYMEMBER_NAMES[num17] + " plays";
							if (num17 == 0)
							{
								text7 = "* You play";
							}
							diag = new string[1] { text7 + " a WILD REVERSE.\n* Turn order has been reversed!\n* The color changes to " + array6[soul.GetComponent<SOUL>().GetSOULMode()] + "!" };
						}
						else
						{
							flag9 = false;
							int num18 = niceActIndex;
							if (miniPartyMember > 0)
							{
								num18 = 2 + miniPartyMember;
							}
							string text8 = "* " + PARTYMEMBER_NAMES[num18] + " tries to play";
							if (num18 == 0)
							{
								text8 = "* You try to play";
							}
							string text9 = "the enemy\n  ";
							if (num15 > 1)
							{
								text9 = ((num14 == 0) ? "the enemies\n  " : "one of the\n  enemies ");
							}
							diag = new string[1] { text8 + " the\n  WILD REVERSE, but " + text9 + "cannot be skipped!" };
						}
					}
					if (flag9)
					{
						gm.UseItem(partySelections[niceActIndex][0], partySelections[niceActIndex][2]);
					}
				}
				curDiag = 0;
				finalDiag = diag.Length - 1;
				StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
			}
			else if (partySelections[niceActIndex][1] == 6 && niceActIndex == 0)
			{
				gm.SetFlag(105, 1);
				if (partySelections[niceActIndex][2] == 0)
				{
					int num19 = 15;
					diag = new string[1] { "* Paula tried LIFEUP...\n" + Items.GetRecoveryString(partySelections[niceActIndex][0], num19) };
					gm.Heal(partySelections[niceActIndex][0], num19);
					gm.PlayTimedHealSound();
					aud2.clip = Resources.Load<AudioClip>("sounds/snd_psi");
					aud2.Play();
				}
				else if (partySelections[niceActIndex][2] == 1)
				{
					diag = new string[1] { "* Paula tried SHIELD...\n* Your SOUL was protected by\n  a LIGHT shield for 15 hits!" };
					soul.GetComponent<SOUL>().ActivateLightShield();
					aud2.clip = Resources.Load<AudioClip>("sounds/snd_psi_shield");
					aud2.Play();
				}
				else if (partySelections[niceActIndex][2] == 2)
				{
					diag = new string[1] { "* Paula tried PK FREEZE..." };
					PKFreezeEffect component4 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/PKFreeze")).GetComponent<PKFreezeEffect>();
					int num20 = partySelections[niceActIndex][0];
					if (enemies[num20].IsDone())
					{
						for (int num21 = 0; num21 < enemies.Length; num21++)
						{
							if (!enemies[num21].IsDone())
							{
								num20 = num21;
								break;
							}
						}
					}
					component4.AssignEnemy(enemies[num20]);
					aud2.clip = Resources.Load<AudioClip>("sounds/snd_psi");
					aud2.Play();
					if (gm.KrisInControl())
					{
						fightingThisRound = true;
						partySelections[niceActIndex][2] = 1;
						partySelections[niceActIndex][1] = 0;
						partySelections[niceActIndex][0] = num20;
					}
				}
				else if (partySelections[niceActIndex][2] == 3)
				{
					diag = new string[1] { "* Paula tried PK FIRE..." };
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/PKFire")).GetComponent<PKFireEffect>();
					aud2.clip = Resources.Load<AudioClip>("sounds/snd_psi");
					aud2.Play();
					if (gm.KrisInControl())
					{
						fightingThisRound = true;
						partySelections[niceActIndex][2] = 1;
						partySelections[niceActIndex][1] = 0;
						partySelections[niceActIndex][0] = 0;
					}
				}
				curDiag = 0;
				finalDiag = diag.Length - 1;
				StartText(diag[curDiag], new Vector2(-4f, -134f), "snd_txtbtl");
			}
		}
		if (niceActIndex < 3)
		{
			niceActIndex++;
		}
	}

	private void DetermineDepressionReject()
	{
		if ((int)gm.GetFlag(172) == 2)
		{
			susieDepressionRefuse = UnityEngine.Random.Range(0, 5) == 0;
			noelleDepressionRefuse = UnityEngine.Random.Range(0, 8) == 0;
			partyPanels.SetSprite(1, "spr_su_down_depressed_" + (susieDepressionRefuse ? "reject" : "0"));
			partyPanels.SetSprite(2, "spr_no_down_depressed_" + (noelleDepressionRefuse ? "reject" : "0"));
		}
		else if ((int)gm.GetFlag(172) == 1)
		{
			noelleDepressionRefuse = UnityEngine.Random.Range(0, 8) == 0;
			partyPanels.SetSprite(2, "spr_no_down_depressed_" + (noelleDepressionRefuse ? "reject" : "0"));
		}
		if ((int)gm.GetFlag(257) == 1)
		{
			if (susieDeviousMisbehave)
			{
				partyPanels.SetSprite(1, "spr_su_down_unhappy_0");
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
	}

	public virtual void AdvanceToEnemyTurn()
	{
		if (boxText.Exists())
		{
			boxText.DestroyOldText();
		}
		partyPanels.RaiseHeads(false, false, false);
		state = 4;
		soul.GetComponent<SpriteRenderer>().enabled = true;
		if (diag == null || buttonIndex == 0 || buttonIndex == 3)
		{
			diag = new string[1] { "" };
			curDiag = 0;
		}
		if (AllEnemiesDone())
		{
			EndNormalFight(false, "");
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
			partyPanels.SetTargets(true, true, true);
			curAtk = AttackSpawner.GetAttack(-1);
		}
		bb.StartMovement(curAtk.GetBoardSize(), curAtk.GetBoardPos());
		soul.transform.position = curAtk.GetSoulPos();
	}

	public void SkipPartyMemberTurn(int partyMember)
	{
		partySelections[partyMember][1] = -1;
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

	public void StartText(string diag, Vector2 pos, string sound)
	{
		string[] array = diag.Split('`');
		if (boxText.Exists())
		{
			ResetText();
		}
		if (array.Length > 1 && !array[0].StartsWith("sounds/"))
		{
			boxPortrait = new GameObject("BoxPortrait", typeof(RectTransform), typeof(Image)).GetComponent<RectTransform>();
			boxPortrait.transform.SetParent(GameObject.Find("BattleCanvas").transform);
			Sprite sprite = Resources.Load<Sprite>("overworld/npcs/portraits/spr_" + array[0] + "_0");
			boxPortrait.sizeDelta = new Vector2(sprite.rect.width / 24f, sprite.rect.height / 24f);
			boxPortrait.GetComponent<Image>().sprite = sprite;
			boxPortrait.localPosition = new Vector2(-218f, 20f) + pos;
			pos += new Vector2(108f, 0f);
		}
		if (array.Length > 1 && array[array.Length - 2].StartsWith("snd_"))
		{
			sound = array[array.Length - 2];
		}
		boxText.StartText(array[array.Length - 1], pos, sound, 0, "DTM-Mono");
		if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && (state == 0 || state == 3 || state == 10))
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
		partyPanels.RaiseHeads(true, true, true);
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
		num2 += UnityEngine.Object.FindObjectOfType<TPBar>().GetCurrentTP() / 5;
		gm.SetFlag(125, num3);
		soul.GetComponent<SpriteRenderer>().enabled = false;
		StopMusic();
		for (int j = 0; j < 3; j++)
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
		StartText(text, new Vector2(-4f, -134f), "snd_txtbtl");
		state = 10;
	}

	public void ForceSoloKris(bool removeMiniPartyMember = false)
	{
		partySize = 1;
		partySelections[1] = new int[3] { 0, -1, 0 };
		partySelections[2] = new int[3] { 0, -1, 0 };
		if (removeMiniPartyMember && gm.GetMiniPartyMember() != 0)
		{
			miniPartyMember = 0;
			int hP = gm.GetHP(0);
			hP -= gm.GetMiniMemberMaxHP();
			gm.SetMiniPartyMember(0);
			if (hP < 0)
			{
				gm.SetHP(0, 1);
			}
			else
			{
				gm.SetHP(0, hP);
			}
			partyPanels.DisableMiniPartyMember();
		}
	}

	public void UpdatePartyMembers()
	{
		partyPanels.Reinitialize();
		partySize = partyPanels.NumOfActivePartyMembers();
		krisAndSusie = gm.SusieInParty();
		miniPartyMember = gm.GetMiniPartyMember();
		ChangeHP();
	}

	public void ActivateSeriousMode()
	{
		isBoss = true;
		partyPanels.SetSprite(1, "spr_su_down_unhappy_0");
		partyPanels.SetSprite(2, "spr_no_down_unhappy_0");
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

	private List<int> GetItemListPerTurn()
	{
		List<int> list = new List<int>(gm.GetItemList());
		if (partyTurn > 0 && partySelections[0][1] == 2 && list[partySelections[0][2]] != 16)
		{
			list.RemoveAt(partySelections[0][2]);
		}
		if (partyTurn > 1 && partySelections[1][1] == 2 && list[partySelections[1][2]] != 16)
		{
			list.RemoveAt(partySelections[1][2]);
		}
		return list;
	}
}
