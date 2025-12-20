using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UnoBattleManager : BattleManager
{
	private static readonly int UI_OFFSET = 36;

	private static readonly float OBJ_OFFSET = (float)UI_OFFSET / 48f;

	private readonly Vector2 textPos = new Vector2(-4f, -98 - UI_OFFSET);

	private int selEnemy;

	private static UnoPlayer[] players;

	private int[] unoIDToPlayerID;

	private CardHand cardHand;

	private int cardSelIndex;

	private Card selectedCard;

	private Card lastCard;

	private Card secondToLastCard;

	private string dareText;

	private Card[] newCards;

	private bool actionText;

	private UnoPlayer clientPlayer;

	private int specialCond;

	private int drawCount;

	private UnoCard nextCard;

	private Card previewCard;

	private int selectedColor;

	private bool curPlayerTurn;

	private int unoFrames;

	private bool startTextSeen;

	private int delayFrames;

	private int place;

	private int lastPlace;

	private int curPoints;

	private int earnedEXP;

	private string firstPlaceSkin;

	private bool saidUno;

	private bool canChallenge;

	private bool preemptiveSayUno;

	private bool handScene;

	private int handId = -1;

	private Transform stackText;

	private bool stackSize;

	private UnoPanels unoPanels;

	private int debugSelectedPlayer = -1;

	private int fakeHp;

	private int fakeMaxHp;

	protected override void Awake()
	{
		base.Awake();
		UnityEngine.Object.Destroy(GameObject.Find("TP"));
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/UnoObjects"));
		cardHand = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/CardHand")).GetComponent<CardHand>();
		cardHand.transform.position = new Vector3(0f, -0.5f);
		cardHand.SetSpread(3f);
		cardHand.SetMaxOverlap(6f);
		cardHand.SetXOffset(2f);
		UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/UnoCanvasObjects")).transform.SetParent(GameObject.Find("BattleCanvas").transform, worldPositionStays: false);
		BattleButton[] array = Util.FindObjectsOfType<BattleButton>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ChangeButtonSuffix("_uno");
		}
		lastCard = GameObject.Find("TopCard").GetComponent<Card>();
		lastCard.gameObject.SetActive(value: false);
		secondToLastCard = GameObject.Find("BottomCard").GetComponent<Card>();
		secondToLastCard.gameObject.SetActive(value: false);
		frames = 30;
		saidUno = true;
		canChallenge = false;
		curPlayerTurn = false;
		unoFrames = 0;
		curPoints = 0;
		delayFrames = 0;
		startTextSeen = false;
		stackText = GameObject.Find("Remaining").transform;
		unoPanels = Util.FindObjectOfType<UnoPanels>();
	}

	public override void StartBattle(int bgColorID)
	{
		Initialize();
		firstButton = true;
		SelectButton(-1);
		soul.GetComponent<SOUL>().ChangeSOULMode(Util.GameManager().GetFlagInt(312));
		soul.GetComponent<SpriteRenderer>().flipY = false;
		partyPanels.Reinitialize();
		bg = EnemyGenerator.GetBattleBG(75);
		if ((bool)Util.FindObjectOfType<SansBG>())
		{
			Util.FindObjectOfType<SansBG>().FadeIn(moveBones: false);
		}
		if ((int)gm.GetFlag(204) == 1)
		{
			partyPanels.SetSprite(0, "eye/spr_kr_down_0_eye");
		}
		soul.GetComponent<SOUL>().SetFrozen(boo: true);
		drawCount = 1;
		selectedColor = 0;
		preemptiveSayUno = false;
		clientPlayer = UnoGameManager.instance.GetClientPlayer();
		EnemyBase[] array = new UnoEnemy[Util.FindObjectsOfType<UnoPlayer>().Length - 1];
		enemies = array;
		float x = 0f;
		float num = 0f;
		switch (enemies.Length)
		{
		case 2:
			x = -2f;
			num = 4f;
			break;
		case 3:
			x = -3f;
			num = 3f;
			break;
		case 4:
			x = -4f;
			num = 2.6666667f;
			break;
		case 5:
			x = -4f;
			num = 2f;
			break;
		case 6:
			x = -5f;
			num = 2f;
			break;
		case 7:
			x = -6f;
			num = 2f;
			break;
		}
		int num2 = 0;
		UnoPlayer[] array2 = Util.FindObjectsOfType<UnoPlayer>();
		foreach (UnoPlayer unoPlayer in array2)
		{
			MonoBehaviour.print(unoPlayer.GetPlayerName());
			if (unoPlayer != clientPlayer)
			{
				UnoEnemy component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/unoenemies/" + UnoGameManager.GetSkinFilename(unoPlayer.GetSkin())), new Vector3(x, -0.1f), Quaternion.identity).GetComponent<UnoEnemy>();
				component.SetUpPlayerInfo(unoPlayer);
				enemies[num2] = component;
				num2++;
			}
		}
		if (clientPlayer.IsHost())
		{
			new List<UnoPlayer>();
			unoPanels.SetPanels(UnoGameManager.instance.GetTurnOrder());
			int num3 = 0;
			int num4 = 0;
			int num5 = 0;
			while (num3 < 2)
			{
				if (num4 != clientPlayer.GetUnoPlayerID() && num3 == 1)
				{
					array = enemies;
					for (int i = 0; i < array.Length; i++)
					{
						UnoEnemy unoEnemy = (UnoEnemy)array[i];
						if (num4 == unoEnemy.GetPlayer().GetUnoPlayerID())
						{
							unoEnemy.transform.position += new Vector3(num * (float)num5, 0f);
							unoEnemy.SetNewMainPosition();
							num5++;
							break;
						}
					}
				}
				else if (num4 == clientPlayer.GetUnoPlayerID())
				{
					num3++;
					if (num3 == 2)
					{
						break;
					}
				}
				num4 = (num4 + 1) % 4;
			}
			players = UnoGameManager.instance.GetPlayers();
			UnoGameManager.instance.BeginUnoGame(players.Length);
		}
		if (UnoGameManager.instance.PointSystemEnabled())
		{
			GameObject.Find("Points").GetComponent<Text>().enabled = true;
		}
		fakeHp = gm.GetMaxHP(0);
		fakeMaxHp = fakeHp;
		UpdateFakeHP(1f);
		startedBattle = true;
		StartFormattedText(new string[1] { "           ^18UNO  ^20START!" }, actionText: true, -1, -1);
	}

	protected override void Update()
	{
		if (!startedBattle)
		{
			return;
		}
		bool flag = false;
		if (!fadeObj.IsPlaying())
		{
			soul.GetComponent<SpriteRenderer>().sortingOrder = 299;
		}
		stackText.localPosition = new Vector3(-115f, Mathf.Lerp(stackText.localPosition.y, stackSize ? 216 : 253, 0.5f));
		if (fakeHp == fakeMaxHp)
		{
			st.Stop();
		}
		else if (fakeHp > 0)
		{
			st.StartShake(Mathf.FloorToInt((float)fakeHp / (float)gm.GetMaxHP(0) * 40f));
		}
		if (clientPlayer.GetPoints() > curPoints)
		{
			curPoints = clientPlayer.GetPoints();
			gm.PlayGlobalSFX("sounds/snd_heal");
			GameObject.Find("Points").GetComponent<Text>().text = curPoints + " points";
		}
		if (state == 0)
		{
			if (!boxText.Exists())
			{
				StartText(curFlavor, textPos, "snd_txtbtl");
			}
			if ((UTInput.GetButton("X") || flag) && boxText.IsPlaying())
			{
				boxText.SkipText();
			}
			soul.GetComponent<SOUL>().SetFrozen(boo: true);
			soul.GetComponent<SpriteRenderer>().enabled = true;
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
			}
			else if (Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal")) == 0 && axisIsDown)
			{
				axisIsDown = false;
			}
			string text = "";
			string text2 = "";
			if (soul.transform.parent != null)
			{
				text2 = soul.transform.parent.gameObject.name;
			}
			text = (new string[4] { "FIGHT", "ACT", "ITEM", "MERCY" })[buttonIndex];
			soul.transform.SetParent(GameObject.Find(text).transform);
			soul.transform.localPosition = new Vector2(-0.82f, -0.022f);
			if (text != text2)
			{
				GameObject.Find(text).GetComponent<BattleButton>().Select(boo: true);
				if (GameObject.Find(text2) != null && GameObject.Find(text2).GetComponent<BattleButton>() != null)
				{
					GameObject.Find(text2).GetComponent<BattleButton>().Select(boo: false);
				}
			}
			if (UTInput.GetButtonDown("Z"))
			{
				bool flag2 = true;
				string[,] array = new string[4, 2];
				_ = new string[3, 2];
				int num = 0;
				int num2 = 0;
				selObj = new GameObject("SelectTier1");
				selObj.layer = 5;
				selObj.AddComponent<RectTransform>();
				selObj.transform.SetParent(GameObject.Find("BattleCanvas").transform);
				bool flag3 = CantDrawCards();
				if (buttonIndex == 0)
				{
					if (cardHand.GetNumCards() > 0)
					{
						cardHand.HideCards();
						state = 1;
						boxText.DestroyOldText();
						soul.GetComponent<SpriteRenderer>().enabled = false;
					}
				}
				else if (buttonIndex == 1)
				{
					array[0, 0] = "* Check";
					if (UnoGameManager.instance.CurrentlyStackingDrawCards())
					{
						array[1, 0] = "* Draw " + drawCount + " Card";
						if (drawCount != 1)
						{
							array[1, 0] += "s";
						}
						UnoGame unoGame = UnoGameManager.instance.GetUnoGame();
						if (lastCard.GetCardData().GetCardType() == UnoCard.CardType.PlusFour && unoGame.GetDiscardPileSize() > 1 && unoGame.GetPlayerWinState(unoGame.GetPreviousPlayerTurn()) == -1)
						{
							array[2, 0] = "* Challenge +4";
						}
					}
					if (gm.IsTestMode())
					{
						array[3, 0] = " ";
					}
					flag2 = false;
				}
				else if (buttonIndex == 2 && !flag3)
				{
					array[0, 0] = "* Draw " + drawCount + " Card";
					if (drawCount != 1)
					{
						array[0, 0] += "s";
					}
					flag2 = false;
				}
				else if (buttonIndex == 3)
				{
					array[0, 0] = "* Forfeit";
					if (gm.IsTestMode())
					{
						array[0, 1] = "* Force 1st";
						array[1, 0] = "* Force 2nd";
						array[1, 1] = "* Force 3rd";
					}
					flag2 = false;
				}
				for (num2 = 0; num2 <= 1; num2++)
				{
					for (num = 0; num <= 2; num++)
					{
						if (array[num, num2] == null)
						{
							array[num, num2] = "";
						}
					}
				}
				if (buttonIndex == 2 && flag3)
				{
					gm.PlayGlobalSFX("sounds/snd_cantselect");
					boxText.DestroyOldText();
					if (UnityEngine.Random.Range(0, 10) == 0)
					{
						diag = new string[1] { "su_pissed`snd_txtsus`* KRIS!!!^10 STOP HOGGING\n  THE CARDS,^05 DAMN IT!!!" };
					}
					else
					{
						diag = new string[1] { "* You felt that you shouldn't\n  draw another card.^10\n* You have cards you can play." };
					}
					curDiag = 0;
					state = 8;
				}
				else
				{
					boxText.SkipText();
					if (!flag2)
					{
						selObj.AddComponent<Selection>().CreateSelections(array, new Vector2(-220f, -141 - UI_OFFSET), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 0);
						selObj.transform.localScale = new Vector2(1f, 1f);
						boxText.DestroyOldText();
						state = 2;
					}
					else
					{
						UnityEngine.Object.Destroy(selObj);
					}
					aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
					aud.Play();
				}
			}
		}
		if (state == 1)
		{
			if (!cardHand.CardsAreOnBoard())
			{
				cardHand.ForceCardsToBoard();
				cardHand.HighlightCard(0);
				cardSelIndex = 0;
				soul.transform.SetParent(null);
				soul.GetComponent<SpriteRenderer>().enabled = true;
				soul.transform.position = new Vector3(cardHand.GetHighlightedCard().transform.position.x, -0.53f);
				GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), GameObject.Find("BattleCanvas").transform);
				obj.transform.localPosition = new Vector3(-319f, -222 - UI_OFFSET);
				obj.transform.localScale = new Vector3(1f, 1f, 1f);
				obj.name = "CardName";
				obj.GetComponent<Text>().text = cardHand.GetHighlightedCard().GetCardData().GetCardName();
				obj.GetComponent<Text>().alignment = TextAnchor.UpperCenter;
			}
			else
			{
				if (Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal")) != 0 && !axisIsDown)
				{
					cardSelIndex += Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal"));
					if (cardSelIndex > cardHand.GetNumCards() - 1)
					{
						cardSelIndex = 0;
					}
					else if (cardSelIndex < 0)
					{
						cardSelIndex = cardHand.GetNumCards() - 1;
					}
					cardHand.HighlightCard(cardSelIndex);
					GameObject.Find("CardName").GetComponent<Text>().text = cardHand.GetHighlightedCard().GetCardData().GetCardName();
					axisIsDown = true;
					aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
					aud.Play();
				}
				else if (Mathf.RoundToInt(UTInput.GetAxisRaw("Horizontal")) == 0 && axisIsDown)
				{
					axisIsDown = false;
				}
				soul.transform.position = new Vector3(cardHand.GetHighlightedCard().transform.position.x, -0.53f);
				if (UTInput.GetButtonDown("X"))
				{
					cardHand.HighlightCard(-1);
					cardSelIndex = 0;
					state = 0;
					cardHand.ShowCards();
					cardHand.RemoveCardsFromBoard();
					UnityEngine.Object.Destroy(GameObject.Find("CardName"));
				}
				else if (UTInput.GetButtonDown("Z"))
				{
					bool flag4 = false;
					if (lastCard.isActiveAndEnabled)
					{
						if (cardHand.GetHighlightedCard().GetCardData().CanBePlacedOn(lastCard.GetCardData()))
						{
							flag4 = true;
						}
					}
					else
					{
						flag4 = true;
					}
					if (flag4)
					{
						state = 3;
						GameObject.Find("FIGHT").GetComponent<BattleButton>().Select(boo: false);
						selectedCard = cardHand.GetHighlightedCard();
						selectedCard.FlipCard();
						selectedCard.transform.SetParent(null);
						cardHand.RemoveCard(selectedCard);
						cardHand.HighlightCard(-1);
						cardHand.ShowCards();
						cardHand.RemoveCardsFromBoard();
						aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
						aud.Play();
						UnityEngine.Object.Destroy(GameObject.Find("CardName"));
						soul.GetComponent<SpriteRenderer>().enabled = false;
					}
					else
					{
						aud.clip = Resources.Load<AudioClip>("sounds/snd_cantselect");
						aud.Play();
					}
				}
			}
		}
		if (state == 2 && UTInput.GetButtonDown("X") && buttonIndex != 0)
		{
			UnityEngine.Object.Destroy(selObj);
			state = 0;
			if ((bool)GameObject.Find("ForfeitPrompt"))
			{
				UnityEngine.Object.Destroy(GameObject.Find("ForfeitPrompt"));
			}
		}
		if (state == 3)
		{
			soul.transform.SetParent(null);
			soul.transform.position = new Vector2(-0.055f, -1.63f);
			soul.GetComponent<SpriteRenderer>().enabled = false;
			firstButton = true;
			if ((buttonIndex == 0 || buttonIndex == 2) && !selectedCard.IsPlayingAnim())
			{
				selectedCard.transform.position += new Vector3(0f, 0.5f);
				if (selectedCard.transform.position.y >= 6f)
				{
					if (selectedCard.GetCardData().IsWildCard() && !GameObject.Find("SelectNewColor"))
					{
						GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), GameObject.Find("BattleCanvas").transform);
						obj2.transform.localPosition = new Vector3(-96f, -206f);
						obj2.transform.localScale = new Vector3(1f, 1f, 1f);
						obj2.name = "SelectNewColor";
						obj2.GetComponent<Text>().text = "Select New Color";
						obj2.GetComponent<Text>().color = new Color(1f, 1f, 1f, 0.25f);
						selObj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/DeltaSelection"), GameObject.Find("BattleCanvas").transform);
						selObj.GetComponent<DeltaSelection>().SetupChoice(Vector2.up, "Red", Vector3.zero);
						selObj.GetComponent<DeltaSelection>().SetupChoice(Vector2.down, "Yellow", Vector3.zero);
						selObj.GetComponent<DeltaSelection>().SetupChoice(Vector2.left, "Green", Vector3.zero);
						selObj.GetComponent<DeltaSelection>().SetupChoice(Vector2.right, "Blue", Vector3.zero);
						selObj.GetComponent<DeltaSelection>().Activate(this, 1, null);
						selObj.transform.localScale = new Vector2(1f, 1f);
						selObj.transform.localPosition = new Vector3(0f, -114f);
						state = 7;
					}
					else if (buttonIndex == 0)
					{
						SubmitSelectedCard("");
					}
					else if (buttonIndex == 2)
					{
						SubmitDrawnCard("", play: true);
					}
				}
			}
			if (buttonIndex == 3)
			{
				soul.GetComponent<SpriteRenderer>().enabled = true;
			}
		}
		if ((((state >= 0 || state <= 3) && cardHand.GetNumCards() == 2) || (state == 13 && cardHand.GetNumCards() == 1)) && curPlayerTurn && !preemptiveSayUno && UTInput.GetButtonDown("C"))
		{
			TogglePreemptiveUnoCall();
		}
		if (state == 4)
		{
			Text component = GameObject.Find("DareText").GetComponent<Text>();
			bool flag5 = false;
			string inputString = Input.inputString;
			for (int i = 0; i < inputString.Length; i++)
			{
				char c = inputString[i];
				switch (c)
				{
				case '\b':
					if (dareText.Length != 0)
					{
						dareText = dareText.Substring(0, dareText.Length - 1);
						flag5 = true;
					}
					break;
				case '\n':
				case '\r':
					if (buttonIndex == 0)
					{
						SubmitSelectedCard(component.text);
					}
					else if (buttonIndex == 2)
					{
						SubmitDrawnCard(component.text, play: true);
					}
					dareText = "";
					UnityEngine.Object.Destroy(component.gameObject);
					UnityEngine.Object.Destroy(GameObject.Find("SelectNewColor"));
					aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
					aud.Play();
					break;
				default:
					dareText += c;
					flag5 = true;
					break;
				}
			}
			if (flag5)
			{
				string text3 = component.text;
				component.text = "* \"" + dareText + "\"";
				if (component.text.Length > 33)
				{
					string[] array2 = component.text.Split(' ');
					string text4 = "";
					string text5 = "\n  ";
					bool flag6 = false;
					bool flag7 = false;
					for (int j = 0; j < array2.Length; j++)
					{
						MonoBehaviour.print(array2[j]);
						string text6 = " ";
						if (j == array2.Length - 1)
						{
							text6 = "";
						}
						if ((text4 + array2[j] + text6).Length < 33 && !flag6)
						{
							text4 = text4 + array2[j] + text6;
							continue;
						}
						flag6 = true;
						if ((text5 + array2[j] + text6).Length < 33)
						{
							text5 = text5 + array2[j] + text6;
							continue;
						}
						flag7 = true;
						dareText = dareText.Substring(0, dareText.Length - 1);
						component.text = text3;
						aud.clip = Resources.Load<AudioClip>("sounds/snd_cantselect");
						aud.Play();
						break;
					}
					if (!flag7)
					{
						component.text = text4 + text5;
					}
				}
			}
		}
		if (state == 5)
		{
			soul.GetComponent<SpriteRenderer>().enabled = false;
			if (!boxText.Exists() && curDiag < diag.Length)
			{
				StartText(diag[curDiag], textPos, "snd_txtbtl");
			}
			else if (!boxText.Exists() && curDiag == diag.Length)
			{
				clientPlayer.SetReady(val: true);
			}
			if (clientPlayer.IsHost() && !boxText.Exists())
			{
				if (place != 0)
				{
					fadeObj.FadeOut(11);
					state = 12;
				}
				else if (UnoGameManager.IsPlayersReady())
				{
					if (handScene)
					{
						handScene = false;
						state = 15;
						FreeCardHand component2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/FreeCardHand")).GetComponent<FreeCardHand>();
						foreach (UnoCard item in UnoGameManager.instance.GetUnoGame().GetPlayerHand(handId))
						{
							component2.AddCard(item.GetCardType(), item.GetCardColor());
						}
						for (int k = 1; k <= 3; k++)
						{
							List<UnoCard> list = new List<UnoCard>();
							if (k == 3)
							{
								list = UnoGameManager.instance.GetUnoGame().GetPlayerHand(handId);
							}
							else
							{
								int index = UnityEngine.Random.Range(0, 4);
								foreach (UnoCard item2 in UnoGameManager.instance.GetUnoGame().GetPlayerHand(handId))
								{
									if (item2.GetCardColor() == UnoCard.COLORS[index] || (k == 2 && item2.IsWildCard()))
									{
										list.Add(item2);
									}
								}
							}
							AIPlayer[] array3 = Util.FindObjectsOfType<AIPlayer>();
							foreach (AIPlayer aIPlayer in array3)
							{
								if (aIPlayer.GetPlayerID() != handId && aIPlayer.GetAIDifficulty() == k)
								{
									aIPlayer.AddCardsToRemember(handId, list);
								}
							}
						}
					}
					else
					{
						state = 7;
						UnoGameManager.instance.FinishTurn(specialCond);
					}
				}
			}
			if (boxText.Exists())
			{
				if ((UTInput.GetButton("X") || flag) && boxText.IsPlaying())
				{
					boxText.SkipText();
				}
				else if (!boxText.IsPlaying())
				{
					if (!startTextSeen)
					{
						delayFrames++;
					}
					if (UTInput.GetButtonDown("Z") || flag || (!startTextSeen && delayFrames >= 40))
					{
						if (!saidUno && UnoGameManager.instance.GetUnoGame().GetCurrentPlayerTurn() == clientPlayer.GetPlayerID())
						{
							return;
						}
						AIPlayer[] array3 = Util.FindObjectsOfType<AIPlayer>();
						foreach (AIPlayer aIPlayer2 in array3)
						{
							if (!saidUno && aIPlayer2.GetUnoPlayerID() == UnoGameManager.instance.GetUnoGame().GetCurrentPlayerTurn())
							{
								aIPlayer2.ForceSayUno();
							}
							else
							{
								aIPlayer2.EndUnoCountdown();
							}
						}
						curDiag++;
						boxText.DestroyOldText();
						if (!startTextSeen)
						{
							startTextSeen = true;
							delayFrames = 0;
						}
						if (curDiag == diag.Length && newCards == null)
						{
							DisableChallenge();
							clientPlayer.SetReady(val: true);
						}
					}
				}
			}
		}
		if (state == 6)
		{
			if (!boxText.Exists() && curDiag < diag.Length)
			{
				StartText(diag[curDiag], textPos, "snd_txtbtl");
			}
			if (boxText.Exists() && (UTInput.GetButton("X") || flag) && boxText.IsPlaying())
			{
				boxText.SkipText();
			}
		}
		if (state == 8)
		{
			if (!boxText.Exists() && curDiag < diag.Length)
			{
				StartText(diag[curDiag], textPos, "snd_txtbtl");
			}
			else if (!boxText.Exists())
			{
				if ((bool)boxPortrait)
				{
					UnityEngine.Object.Destroy(boxPortrait.gameObject);
				}
				state = 0;
			}
			if (boxText.Exists())
			{
				if ((UTInput.GetButton("X") || flag) && boxText.IsPlaying())
				{
					boxText.SkipText();
				}
				else if (!boxText.IsPlaying() && (UTInput.GetButtonDown("Z") || flag))
				{
					boxText.DestroyOldText();
					curDiag++;
				}
			}
		}
		if (state == 9)
		{
			frames++;
			if (frames == 30)
			{
				clientPlayer.SetReady(val: true);
				aud.clip = Resources.Load<AudioClip>("sounds/snd_drumroll_loop");
				aud.loop = true;
				aud.Play();
			}
			if (frames == 40)
			{
				GameObject.Find("8place").GetComponent<Text>().enabled = true;
			}
			if (frames == 50 && lastPlace >= 5)
			{
				GameObject.Find("567place").GetComponent<Text>().enabled = true;
			}
			if (frames == 60 && lastPlace >= 2)
			{
				GameObject.Find("234place").GetComponent<Text>().enabled = true;
			}
			if (frames == maxFrames - 15)
			{
				aud.clip = Resources.Load<AudioClip>("sounds/snd_cymbal");
				aud.loop = false;
				aud.Play();
				GameObject.Find("1place").GetComponent<Text>().enabled = true;
				GameObject.Find("1placeshadow").GetComponent<Text>().enabled = true;
				UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/unoenemies/" + firstPlaceSkin), new Vector3(-3.35f, -0.1f), Quaternion.identity);
			}
			if (frames == maxFrames)
			{
				state = 10;
				string text7 = "";
				if (place >= 1)
				{
					string text8 = "TH";
					string[] array4 = new string[3] { "ST", "ND", "RD" };
					if (place <= 3)
					{
						text8 = array4[place - 1];
					}
					text7 = text7 + "* YOU WON " + place + text8 + " PLACE!\n";
				}
				else
				{
					text7 += "* Better luck next time!\n";
				}
				text7 += "* You earned 0 XP and 0 gold.";
				StartText(text7, textPos, "snd_txtbtl");
			}
		}
		if (state == 10)
		{
			if ((UTInput.GetButton("X") || flag) && boxText.IsPlaying())
			{
				boxText.SkipText();
			}
			else if ((UTInput.GetButtonDown("Z") || flag) && !boxText.IsPlaying())
			{
				EndUnoGame();
			}
		}
		if (state == 11)
		{
			fadeObj.FadeOut(11);
			state = 12;
		}
		if (state == 12 && !fadeObj.IsPlaying())
		{
			EndUnoGame();
		}
		if (state == 14)
		{
			if (delayFrames == 45)
			{
				state = 5;
				delayFrames = 0;
			}
			else
			{
				delayFrames++;
			}
		}
		if (state == 15 && !Util.FindObjectOfType<FreeCardHand>())
		{
			MonoBehaviour.print("SENT!!!!!!");
			state = 7;
			UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.ChallengePlus4End, null, "");
		}
		if (newCards != null && state < 9)
		{
			bool flag8 = true;
			Card[] array5 = newCards;
			for (int i = 0; i < array5.Length; i++)
			{
				if (array5[i].IsPlayingAnim())
				{
					flag8 = false;
				}
			}
			if (flag8)
			{
				array5 = newCards;
				foreach (Card card in array5)
				{
					cardHand.AddCard(card);
				}
				newCards = null;
			}
		}
		Vector3 vector = new Vector3(69f, 420f);
		if ((bool)selObj && (bool)selObj.GetComponent<Selection>() && selObj.GetComponent<Selection>().IsEnabled() && (bool)selObj.GetComponent<Selection>().GetSOUL() && selObj.activeInHierarchy)
		{
			vector = selObj.GetComponent<Selection>().GetSOUL().transform.localPosition / 48f;
		}
		if ((bool)selObj2 && (bool)selObj2.GetComponent<Selection>() && selObj2.GetComponent<Selection>().IsEnabled() && (bool)selObj2.GetComponent<Selection>().GetSOUL() && selObj2.activeInHierarchy)
		{
			vector = selObj2.GetComponent<Selection>().GetSOUL().transform.localPosition / 48f;
		}
		if (vector != new Vector3(69f, 420f))
		{
			soul.transform.position = vector;
		}
		if (frames < 30 && state < 9)
		{
			frames++;
			float num3 = (float)frames / 30f;
			num3 = Mathf.Sin(num3 * MathF.PI * 0.5f);
			lastCard.transform.position = Vector3.Lerp(new Vector3(5.78f, 6f), new Vector3(5.78f, 3.8f), num3);
		}
		if (UTInput.GetButtonDown("C") && !saidUno)
		{
			UnoGameManager.instance.SayUno(clientPlayer.GetPlayerID());
			DeactivateUno();
			unoFrames = 0;
		}
		else if (unoFrames < 8 && !saidUno)
		{
			unoFrames++;
		}
		else if (UTInput.GetButtonDown("C") && canChallenge)
		{
			DisableChallenge();
			UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.ChallengePlus4Start, null, "");
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (id != 6)
		{
			int childCount = selObj.transform.childCount;
			for (int i = 0; i < childCount; i++)
			{
				UnityEngine.Object.DestroyImmediate(selObj.transform.GetChild(0).gameObject);
			}
		}
		if (id == 0)
		{
			if (buttonIndex == 1)
			{
				if ((int)index[0] == 3)
				{
					UnoPlayer[] array = UnoGameManager.instance.GetPlayers();
					string[,] array2 = new string[4, 2];
					for (int j = 0; j < array.Length; j++)
					{
						array2[j / 2, j % 2] = "* " + array[j].GetPlayerName();
					}
					CreateNewSelections(array2, 5);
				}
				else if (index == Vector2.zero)
				{
					CreateEnemySelections(4);
				}
				else if (UnoGameManager.instance.CurrentlyStackingDrawCards() && (int)index[0] == 1)
				{
					soul.transform.SetParent(null);
					soul.transform.position = new Vector2(-0.055f, -1.63f);
					soul.GetComponent<SpriteRenderer>().enabled = false;
					GameObject.Find("ACT").GetComponent<BattleButton>().Select(boo: false);
					firstButton = true;
					UnityEngine.Object.Destroy(selObj);
					UnityEngine.Object.Destroy(selObj);
					SubmitDrawnCard("", play: false);
				}
				else if (UnoGameManager.instance.CurrentlyStackingDrawCards() && (int)index[0] == 2)
				{
					soul.transform.SetParent(null);
					soul.transform.position = new Vector2(-0.055f, -1.63f);
					soul.GetComponent<SpriteRenderer>().enabled = false;
					GameObject.Find("ACT").GetComponent<BattleButton>().Select(boo: false);
					firstButton = true;
					UnityEngine.Object.Destroy(selObj);
					state = 7;
					UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.ChallengePlus4Act, null, "");
				}
			}
			if (buttonIndex == 2)
			{
				GameObject.Find("ITEM").GetComponent<BattleButton>().Select(boo: false);
				UnoCard card = null;
				if (lastCard.isActiveAndEnabled)
				{
					card = lastCard.GetCardData();
				}
				if (drawCount == 1 && nextCard.CanBePlacedOn(card))
				{
					selObj.GetComponent<Selection>().Reset();
					GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), GameObject.Find("BattleCanvas").transform);
					obj.transform.localPosition = new Vector3(-181f, -138 - UI_OFFSET);
					obj.transform.localScale = new Vector3(1f, 1f, 1f);
					obj.name = "PlayThisCard";
					obj.GetComponent<Text>().text = "You can play this card!\nWould you like to?";
					selObj.transform.localScale = new Vector2(48f, 48f);
					selObj.GetComponent<Selection>().CreateSelections(new string[1, 2] { { "* Play", "* Keep" } }, new Vector2(-220f, -205 - UI_OFFSET), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 2);
					selObj.transform.localScale = new Vector2(1f, 1f);
					previewCard = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/Card")).GetComponent<Card>();
					previewCard.transform.position = new Vector3(3.94f, -1.63f - OBJ_OFFSET);
					previewCard.SetSortingOrder(300);
					previewCard.ChangeTargetCard(nextCard);
					state = 13;
				}
				else
				{
					UnityEngine.Object.Destroy(selObj);
					SubmitDrawnCard("", play: false);
				}
			}
			if (buttonIndex == 3)
			{
				if (index != Vector2.zero)
				{
					SetWinCondition(Mathf.RoundToInt(index[0] * 2f + index[1]), updated: false);
					EndUnoGame();
					return;
				}
				selObj.GetComponent<Selection>().Reset();
				GameObject obj2 = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), GameObject.Find("BattleCanvas").transform);
				obj2.transform.localPosition = new Vector3(-181f, -138 - UI_OFFSET);
				obj2.transform.localScale = new Vector3(1f, 1f, 1f);
				obj2.name = "ForfeitPrompt";
				obj2.GetComponent<Text>().text = "Really Forfeit?";
				selObj.transform.localScale = new Vector2(48f, 48f);
				selObj.GetComponent<Selection>().CreateSelections(new string[2, 1]
				{
					{ "* Keep going" },
					{ "* Give up" }
				}, new Vector2(-220f, -173 - UI_OFFSET), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, 3);
				selObj.transform.localScale = new Vector2(1f, 1f);
			}
		}
		if (id == 1)
		{
			UnityEngine.Object.Destroy(selObj);
			if (index == Vector2.up)
			{
				selectedCard.GetCardData().SetWildColor(UnoCard.RED);
				selectedColor = 0;
			}
			else if (index == Vector2.down)
			{
				selectedCard.GetCardData().SetWildColor(UnoCard.YELLOW);
				selectedColor = 1;
			}
			else if (index == Vector2.left)
			{
				selectedCard.GetCardData().SetWildColor(UnoCard.GREEN);
				selectedColor = 2;
			}
			else if (index == Vector2.right)
			{
				selectedCard.GetCardData().SetWildColor(UnoCard.BLUE);
				selectedColor = 3;
			}
			UnityEngine.Object.Destroy(GameObject.Find("SelectNewColor"));
			if (buttonIndex == 0)
			{
				SubmitSelectedCard("");
			}
			else if (buttonIndex == 2)
			{
				SubmitDrawnCard("", play: true);
			}
		}
		if (id == 2)
		{
			UnityEngine.Object.Destroy(selObj);
			UnityEngine.Object.Destroy(GameObject.Find("PlayThisCard"));
			if ((int)index[1] == 0)
			{
				selectedCard = previewCard;
				previewCard = null;
				selectedCard.FlipCard();
				state = 3;
			}
			else
			{
				SubmitDrawnCard("", play: false);
			}
		}
		if (id == 3)
		{
			UnityEngine.Object.Destroy(selObj);
			UnityEngine.Object.Destroy(GameObject.Find("ForfeitPrompt"));
			if ((int)index[0] == 0)
			{
				state = 0;
			}
			else
			{
				soul.transform.SetParent(null);
				soul.transform.position = new Vector2(-0.055f, -1.63f);
				soul.GetComponent<SpriteRenderer>().enabled = false;
				GameObject.Find("MERCY").GetComponent<BattleButton>().Select(boo: false);
				firstButton = true;
				UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.Forfeit, null, "");
			}
		}
		if (id == 4)
		{
			UnityEngine.Object.Destroy(selObj);
			soul.transform.SetParent(null);
			soul.transform.position = new Vector2(-0.055f, -1.63f);
			soul.GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("ACT").GetComponent<BattleButton>().Select(boo: false);
			firstButton = true;
			int num = (int)index[0];
			if (enemies[num] != null)
			{
				if (!enemies[num].IsKilled())
				{
					UnoPlayer player = enemies[num].GetPlayer();
					int cardCount = enemies[num].GetCardCount();
					if (boxText.Exists())
					{
						boxText.DestroyOldText();
					}
					int partyMember = ((UnoGameManager.GetSkinFilename(player.GetSkin()) == "susie") ? 1 : 2);
					string text = Localizer.GetText("uno_check_" + UnoGameManager.GetSkinFilename(player.GetSkin()), PartyMembers.GetATK(partyMember).ToString(), PartyMembers.GetDEF(partyMember).ToString());
					diag = new string[2] { text, "" };
					string text2 = ((cardCount == 0) ? player.GetPronoun(0) : player.GetPronoun(7));
					text2 = ((text2.Length <= 1) ? text2.ToUpper() : (text2.Substring(0, 1).ToUpper() + text2.Substring(1)));
					if (cardCount > 1)
					{
						ref string reference = ref diag[1];
						reference = reference + "* " + text2 + " " + cardCount + " cards left.";
						if (cardCount >= 30)
						{
							diag[1] += "\n* Fear";
						}
						else if (cardCount >= 20)
						{
							ref string reference2 = ref diag[1];
							reference2 = reference2 + "\n* WILL " + player.GetPronoun(0).ToUpper() + " SURVIVE?";
						}
						else if (cardCount >= 12)
						{
							diag[1] += "\n* How tense!";
						}
						else if (cardCount <= 3)
						{
							string pronoun = player.GetPronoun(4);
							pronoun = pronoun.Substring(0, 1).ToUpper() + pronoun.Substring(1);
							ref string reference3 = ref diag[1];
							reference3 = reference3 + "\n* " + pronoun + " close.";
						}
					}
					else
					{
						switch (cardCount)
						{
						case 1:
						{
							ref string reference = ref diag[1];
							reference = reference + "* " + text2 + " " + cardCount + " card left!!!";
							break;
						}
						case 0:
						{
							string text3 = (player.PronounIsPlural() ? "have" : "has");
							ref string reference = ref diag[1];
							reference = reference + "* " + text2 + " no longer " + text3 + " cards.";
							if (((UnoEnemy)enemies[num]).Forfeit())
							{
								ref string reference4 = ref diag[1];
								reference4 = reference4 + "^15\n* Because " + player.GetPronoun(0) + " forfeited.";
							}
							break;
						}
						}
					}
					curDiag = 0;
					state = 8;
				}
				else
				{
					state = 0;
				}
			}
			else
			{
				state = 0;
			}
		}
		if (id == 5)
		{
			debugSelectedPlayer = (int)index[0] * 2 + (int)index[1];
			CreateNewSelections(new string[4, 2]
			{
				{ "* Show Hand", "* -1 Card" },
				{ "* +1 Card", "* Win" },
				{ "* Forfeit", null },
				{ null, null }
			}, 6);
			Debug.Log(debugSelectedPlayer);
		}
		if (id == 6)
		{
			int choice = (int)index[0] * 2 + (int)index[1];
			UnoGameManager.instance.DebugChoice(debugSelectedPlayer, choice);
		}
		aud.clip = Resources.Load<AudioClip>("sounds/snd_select");
		aud.Play();
	}

	private void CreateNewSelections(string[,] sels, int id)
	{
		selObj.transform.localScale = new Vector2(48f, 48f);
		selObj.GetComponent<Selection>().CreateSelections(sels, new Vector2(-220f, -141 - UI_OFFSET), new Vector2(240f, -32f), new Vector2(-28f, 95f), "DTM-Mono", useSoul: true, makeSound: true, this, id);
		selObj.transform.localScale = new Vector2(1f, 1f);
	}

	private void CreateEnemySelections(int id)
	{
		string[,] array = new string[4, 2];
		for (int i = 0; i < enemies.Length; i++)
		{
			array[i, 0] = "* " + enemies[i].GetPlayer().GetPlayerName();
			Text component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/CardsLeft"), selObj.transform).GetComponent<Text>();
			component.text = component.text.Replace("X", enemies[i].GetCardCount().ToString());
			if (enemies[i].GetCardCount() == 0)
			{
				component.color = new Color(1f, 1f, 1f, 0.25f);
			}
			else if (enemies[i].GetCardCount() <= 3)
			{
				component.color = new Color(1f, 1f, 0f, 1f);
			}
			else if (enemies[i].GetCardCount() > 15)
			{
				component.color = new Color(1f, 0f, 0f, 1f);
			}
			component.transform.localPosition = new Vector3(15f, -99 - 32 * i);
		}
		CreateNewSelections(array, id);
	}

	public void StartFormattedText(string[] dialogue, bool actionText, int playerId, int targetPlayerId)
	{
		if (boxText.Exists())
		{
			boxText.DestroyOldText();
		}
		if (preemptiveSayUno)
		{
			TogglePreemptiveUnoCall();
		}
		clientPlayer.SetReady(val: false);
		diag = dialogue;
		this.actionText = actionText;
		curDiag = 0;
		if (actionText)
		{
			state = 5;
			if (playerId == clientPlayer.GetPlayerID())
			{
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " plays", "* You play");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " dares", "* You dare");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " draws", "* You draw");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " rejects the dare\n  and draws 10 cards!", "* You reject the dare\n  and draw 10 cards!");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " must", "* You must");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " forfeits", "* You forfeit");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " calls", "* You call");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " reached", "* You reached");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " already won, though...", "* Luckily, you already won!");
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " challenges", "* You challenge");
				if (diag.Length > 1)
				{
					diag[1] = diag[1].Replace("* " + clientPlayer.GetPlayerName() + " must", "* You must");
				}
				if (diag[0].Contains("accepts the dare!"))
				{
					diag[0] = "* You accept the dare!\n* Be sure to actually do it,\n  mmkay?";
				}
			}
			else if (targetPlayerId == clientPlayer.GetPlayerID())
			{
				diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " has", "* You have");
				if (diag[0].Contains("called UNO first!"))
				{
					diag[0] = diag[0].Replace("* " + clientPlayer.GetPlayerName() + " called", "* You called");
				}
				else
				{
					string text = diag[0].Substring(10);
					text = text.Replace("* " + clientPlayer.GetPlayerName() + "'s", "* Your");
					text = text.Replace("* " + clientPlayer.GetPlayerName(), "* You");
					text = text.Replace(" " + clientPlayer.GetPlayerName() + " ", " you ");
					text = text.Replace(" " + clientPlayer.GetPlayerName() + "'s\n", " your\n");
					diag[0] = diag[0].Substring(0, 10) + text;
					if (diag.Length > 1)
					{
						diag[1] = diag[1].Replace("* " + clientPlayer.GetPlayerName() + " was", "* You were");
						diag[1] = diag[1].Replace("* " + clientPlayer.GetPlayerName() + " must", "* You must");
					}
				}
			}
			if (diag[0].Contains("UNO  ^"))
			{
				specialCond = 1;
			}
			else if (diag[0].Contains("reached 500 points!"))
			{
				specialCond = 3;
			}
			else
			{
				specialCond = 0;
			}
		}
		else
		{
			curPlayerTurn = false;
			state = 6;
			if (targetPlayerId == clientPlayer.GetPlayerID())
			{
				diag[0] = diag[0].Replace(clientPlayer.GetPlayerName() + " is next.", "You're next!");
			}
			if (playerId == clientPlayer.GetPlayerID())
			{
				curFlavor = diag[0];
				curFlavor = curFlavor.Replace("* " + clientPlayer.GetPlayerName() + " must", "* You must");
				curFlavor = curFlavor.Replace(clientPlayer.GetPlayerName() + "'s", "your");
				curFlavor = curFlavor.Replace(" " + clientPlayer.GetPronoun(0) + " ", " you ");
				gm.PlayGlobalSFX("sounds/snd_item");
				curPlayerTurn = true;
				state = 0;
			}
		}
		if (diag[0].Contains("called UNO first!"))
		{
			state = 14;
		}
	}

	public void SetLastCard(UnoCard card)
	{
		if (!lastCard.isActiveAndEnabled)
		{
			lastCard.gameObject.SetActive(value: true);
		}
		else if (!secondToLastCard.isActiveAndEnabled)
		{
			secondToLastCard.gameObject.SetActive(value: true);
		}
		frames = 0;
		if (secondToLastCard.isActiveAndEnabled)
		{
			secondToLastCard.ChangeTargetCard(lastCard.GetCardData());
		}
		lastCard.ChangeTargetCard(card);
		GameObject.Find("LastCard").GetComponent<Text>().text = card.GetCardName();
	}

	public Card GetLastCard()
	{
		return lastCard;
	}

	public void AddCards(UnoCard[] cards)
	{
		if (newCards != null)
		{
			Card[] array = newCards;
			foreach (Card card in array)
			{
				cardHand.AddCard(card);
			}
			newCards = null;
		}
		newCards = new Card[cards.Length];
		for (int j = 0; j < cards.Length; j++)
		{
			Card component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/Card")).GetComponent<Card>();
			component.transform.position = GameObject.Find("DrawDeck").transform.position;
			component.ChangeTargetCard(cards[j]);
			component.ForceDown();
			component.FlipCard();
			newCards[j] = component;
		}
	}

	public void UpdateDrawPileCount(int i)
	{
		stackSize = i > 1;
		if (stackSize)
		{
			GameObject.Find("Remaining").GetComponent<Text>().text = "Stack Size: " + i;
		}
	}

	private void SubmitSelectedCard(string dareText)
	{
		state = 7;
		string extra = (preemptiveSayUno ? "true" : "");
		UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.Play, selectedCard.GetCardData(), extra);
		cardSelIndex = 0;
		UnityEngine.Object.Destroy(selectedCard.gameObject);
	}

	public void SubmitDrawnCard(string dareText, bool play)
	{
		state = 7;
		if (play)
		{
			string extra = (preemptiveSayUno ? "true" : "");
			UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.DrawAndPlay, null, extra);
			UnityEngine.Object.Destroy(selectedCard.gameObject);
		}
		else
		{
			soul.transform.SetParent(null);
			soul.transform.position = new Vector2(-0.055f, -1.63f);
			soul.GetComponent<SpriteRenderer>().enabled = false;
			firstButton = true;
			UnoGameManager.instance.SubmitTurn(clientPlayer.GetPlayerID(), UnoGameManager.Actions.Draw, null, "");
		}
		if (previewCard != null)
		{
			UnityEngine.Object.Destroy(previewCard.gameObject);
		}
	}

	public void UpdateDrawCount(int drawCount)
	{
		this.drawCount = (int)Mathf.Clamp(drawCount, 1f, float.PositiveInfinity);
	}

	public void UpdateNextDrawCard(UnoCard nextCard)
	{
		MonoBehaviour.print("update next card");
		this.nextCard = nextCard;
	}

	public void ActivateUno()
	{
		if (!preemptiveSayUno)
		{
			saidUno = false;
			unoFrames = 0;
			bool joystickIsActive = UTInput.joystickIsActive;
			Transform transform = GameObject.Find("SayUno").transform;
			transform.GetComponent<Text>().enabled = true;
			transform.GetComponent<Text>().text = string.Format("Press {0} to say UNO!", joystickIsActive ? "     " : string.Format("[{0}]", UTInput.GetKeyName("Menu")));
			if (joystickIsActive)
			{
				transform.GetComponentInChildren<Image>().enabled = true;
				ButtonPrompts.UpdateImageWithGraphic("Menu", transform.GetComponentInChildren<Image>());
			}
		}
	}

	public void DeactivateUno()
	{
		saidUno = true;
		Transform obj = GameObject.Find("SayUno").transform;
		obj.GetComponent<Text>().enabled = false;
		obj.GetComponentInChildren<Image>().enabled = false;
	}

	public void SetWinCondition(int place, bool updated)
	{
		MonoBehaviour.print("PLACE!!" + place);
		this.place = place;
		if (!GameObject.Find("WinStatus") || UnoGameManager.instance.PointSystemEnabled())
		{
			return;
		}
		string text = "th";
		string[] array = new string[3] { "st", "nd", "rd" };
		if (place >= 1)
		{
			if (!updated && place < 4)
			{
				gm.PlayGlobalSFX("sounds/snd_won");
			}
			if (place <= 3)
			{
				text = array[place - 1];
			}
		}
		GameObject.Find("WinStatus").GetComponent<Text>().text = place + text + " place";
		if (place == -1)
		{
			GameObject.Find("WinStatus").GetComponent<Text>().text = "Forfeited";
		}
		StopMusic();
	}

	public void EndUnoGame()
	{
		int num = PersistentSAVE.GetInt("uno-personal-best", 0);
		if (place > 0 && (num == 0 || place < num))
		{
			PersistentSAVE.SetInt("uno-personal-best", place);
			if (UnoGameManager.IsPapyrusHardMode() && PersistentSAVE.GetInt("uno-papyrus-hardmode-win", 0) == 0 && place == 1)
			{
				PersistentSAVE.SetInt("uno-papyrus-hardmode-win", 1);
			}
		}
		gm.EndBattle(place);
	}

	public void EnableChallenge()
	{
		UnoGame unoGame = UnoGameManager.instance.GetUnoGame();
		if (unoGame.GetPlayerWinState(unoGame.GetCurrentPlayerTurn()) == -1)
		{
			canChallenge = true;
		}
		if (canChallenge)
		{
			bool joystickIsActive = UTInput.joystickIsActive;
			Transform transform = GameObject.Find("Challenge").transform;
			transform.GetComponent<Text>().enabled = true;
			transform.GetComponent<Text>().text = string.Format("Press {0} to challenge +4 Card", joystickIsActive ? "       " : string.Format("[{0}]", UTInput.GetKeyName("Menu")));
			if (joystickIsActive)
			{
				transform.GetComponentInChildren<Image>().enabled = true;
				ButtonPrompts.UpdateImageWithGraphic("Menu", transform.GetComponentInChildren<Image>());
			}
		}
	}

	public void DisableChallenge()
	{
		canChallenge = false;
		Transform obj = GameObject.Find("Challenge").transform;
		obj.GetComponent<Text>().enabled = false;
		obj.GetComponentInChildren<Image>().enabled = false;
	}

	public void StartHandScene(int handId)
	{
		handScene = true;
		this.handId = handId;
	}

	public void TogglePreemptiveUnoCall()
	{
		preemptiveSayUno = !preemptiveSayUno;
		if (preemptiveSayUno)
		{
			gm.PlayGlobalSFX("sounds/snd_equip");
			GameObject.Find("UnoCall").GetComponent<Text>().enabled = true;
		}
		else
		{
			GameObject.Find("UnoCall").GetComponent<Text>().enabled = false;
		}
	}

	public void UpdateFakeHP(float percentage)
	{
		fakeHp = Mathf.RoundToInt((float)fakeMaxHp * percentage);
		if (fakeHp < 1)
		{
			fakeHp = 1;
		}
		partyPanels.UnoTick(fakeHp);
		partyPanels.UpdateHP(new int[6] { fakeHp, 30, 20, 0, 0, 0 });
	}

	public static UnoPlayer[] GetUnoPlayers()
	{
		return players;
	}

	public CardHand GetCardHand()
	{
		return cardHand;
	}

	public SOUL GetPlayerSOUL()
	{
		return soul.GetComponent<SOUL>();
	}

	protected override void LateUpdate()
	{
	}

	public override void DoSOULSparkle()
	{
	}

	private bool CantDrawCards()
	{
		if (lastCard.isActiveAndEnabled)
		{
			bool flag = false;
			foreach (Card card in cardHand.GetCards())
			{
				if (card.GetCardData().CanBePlacedOn(lastCard.GetCardData()))
				{
					flag = true;
					break;
				}
			}
			if (flag && cardHand.GetNumCards() >= 30)
			{
				return !UnoGameManager.instance.CurrentlyStackingDrawCards();
			}
			return false;
		}
		return false;
	}
}
