using System;
using System.Collections.Generic;
using UnityEngine;

public class UnoGameManager : MonoBehaviour
{
	public enum Actions
	{
		Play = 0,
		Draw = 1,
		DrawAndPlay = 2,
		FailUno = 3,
		AcceptDare = 4,
		Forfeit = 5,
		ChallengeDare = 6,
		RefuseDare = 7,
		PointEnd = 8,
		ChallengePlus4Start = 9,
		ChallengePlus4End = 10,
		ChallengePlus4Act = 11
	}

	public struct EnemyCardInfo
	{
		public int playerID;

		public int cardCount;

		public UnoEnemy.WinCondition winCondition;

		public EnemyCardInfo(int playerID, int cardCount, UnoEnemy.WinCondition winCondition)
		{
			this.playerID = playerID;
			this.cardCount = cardCount;
			this.winCondition = winCondition;
		}
	}

	public static readonly int HARD_PLAYER_LIMIT = 4;

	public static readonly bool RANDOM_TURN_ORDER = true;

	public static readonly bool PLAYER_GOES_FIRST = true;

	public new Transform transform;

	private UnoPlayer[] players;

	private List<UnoPlayer> deletePlayersQueue;

	private UnoGame unoGame;

	private bool playing;

	private bool pointSystem;

	private bool stackableDraw;

	private bool challengableFour;

	private bool drawCard;

	private int musicID;

	private int bgColorID;

	private int bgType;

	private bool canDeletePlayer;

	private bool doingNextPlayerTurn;

	private bool stackingImminent;

	private bool challengeImminent;

	private bool challengeeSuccessful;

	private int challengee = -1;

	private int challenger = -1;

	private bool challengeDie;

	private bool canSayUno;

	private bool curPlayerSaidUno;

	private bool callerSaidUno;

	private int preemptiveSaidUno = -1;

	private bool clientIsTense;

	private int clientPlaceNo;

	private bool endGame;

	private UnoPlayer clientPlayer;

	public List<int> turnOrder;

	public static UnoGameManager instance { get; private set; }

	private void Awake()
	{
		instance = this;
		players = new UnoPlayer[HARD_PLAYER_LIMIT];
		deletePlayersQueue = new List<UnoPlayer>();
		transform = base.gameObject.transform;
		stackingImminent = false;
		canSayUno = false;
		curPlayerSaidUno = false;
		callerSaidUno = false;
		canDeletePlayer = false;
		doingNextPlayerTurn = false;
		playing = false;
		endGame = false;
		challengeImminent = false;
		clientPlaceNo = 0;
	}

	private void OnDestroy()
	{
		instance = null;
	}

	private void Update()
	{
		if (!canDeletePlayer || unoGame == null)
		{
			return;
		}
		while (deletePlayersQueue.Count > 0)
		{
			UnoPlayer unoPlayer = deletePlayersQueue[0];
			SendWinCondition(unoPlayer.GetPlayerID(), false, -2);
			int playerWinState = unoGame.GetPlayerWinState(unoPlayer.GetUnoPlayerID());
			int[] array = (int[])unoGame.GetAllWinningPlaces().Clone();
			unoGame.SetPlayerDisconnect(unoPlayer.GetUnoPlayerID());
			if (doingNextPlayerTurn && unoPlayer.GetUnoPlayerID() == unoGame.GetCurrentPlayerTurn() && unoGame.GetCurrentActivePlayerCount() > 1)
			{
				int nextPlayerTurn = unoGame.GetNextPlayerTurn(true);
				int playerID = players[nextPlayerTurn].GetPlayerID();
				string playerName = players[nextPlayerTurn].GetPlayerName();
				int playerID2 = players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerID();
				string playerName2 = players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerName();
				if (canSayUno)
				{
					canSayUno = false;
				}
				if (stackingImminent)
				{
					string playerName3 = players[nextPlayerTurn].GetPlayerName();
					ReceiveCardInfo("drawcount", unoGame.GetCurrentDrawCardAmount());
					DoNextTurn(playerID, "* Now the stack falls upon\n  " + playerName3 + "!", 1, playerID2, unoGame.GetTopDrawCard(), unoGame.GetDrawPileSize());
				}
				else
				{
					DoNextTurn(playerID, "* It's " + playerName + "'s turn.\n* " + playerName2 + " is next.", 0, playerID2, unoGame.GetTopDrawCard(), unoGame.GetDrawPileSize());
				}
			}
			else if (unoGame.GetCurrentActivePlayerCount() == 1)
			{
				DoTurn(-1, Actions.Forfeit, null, "* The last player disconnected!", -1);
			}
			if (playerWinState > -1)
			{
				int num = 0;
				for (int i = 0; i < unoGame.GetPlayerCount(); i++)
				{
					if (i != unoPlayer.GetUnoPlayerID() && unoGame.GetPlayerWinState(i) > playerWinState)
					{
						if (unoGame.GetPlayerWinState(i) > num)
						{
							num = unoGame.GetPlayerWinState(i);
						}
						unoGame.ForcePlayerWinState(i, unoGame.GetPlayerWinState(i) - 1);
						UpdateWinCondition(players[i], players[i].GetPlayerID(), unoGame.GetPlayerWinState(i) + 1);
					}
				}
				unoGame.SetCurrentWinPlace(num);
			}
			players[unoPlayer.GetPlayerID()] = null;
			deletePlayersQueue.RemoveAt(0);
		}
	}

	public void CreatePlayer(string name, int skin, int pronounsIndex, int aiDifficulty)
	{
		bool flag = aiDifficulty == -1;
		for (int i = 0; i < HARD_PLAYER_LIMIT; i++)
		{
			if (players[i] == null)
			{
				MonoBehaviour.print("New Player ID: " + i);
				players[i] = UnityEngine.Object.Instantiate(Resources.Load<GameObject>(flag ? "uno/Player" : "uno/AIPlayer")).GetComponent<UnoPlayer>();
				players[i].Initialize(i);
				players[i].UpdatePlayerInfo(name, skin, pronounsIndex);
				players[i].transform.parent = transform;
				players[i].SetUnoPlayerID(i);
				if (flag)
				{
					clientPlayer = players[i];
				}
				else
				{
					players[i].SetAIDifficulty(aiDifficulty);
				}
				break;
			}
		}
	}

	public void DeletePlayer(UnoPlayer player)
	{
		if (unoGame == null)
		{
			for (int i = 0; i < HARD_PLAYER_LIMIT; i++)
			{
				if (players[i] != null && players[i] == player)
				{
					players[i] = null;
					break;
				}
			}
			return;
		}
		UnoPlayer[] array = UnityEngine.Object.FindObjectsOfType<UnoPlayer>();
		foreach (UnoPlayer unoPlayer in array)
		{
			if (unoPlayer == player)
			{
				deletePlayersQueue.Add(unoPlayer);
				break;
			}
		}
	}

	public void StartGame(int music, bool apointSystem, bool astackableDraw, bool achallengableFour, bool adrawCard)
	{
		playing = true;
		stackingImminent = false;
		challengeImminent = false;
		canSayUno = false;
		curPlayerSaidUno = false;
		callerSaidUno = false;
		clientIsTense = false;
		clientPlaceNo = 0;
		endGame = false;
		musicID = music;
		pointSystem = apointSystem;
		stackableDraw = astackableDraw;
		challengableFour = achallengableFour;
		drawCard = adrawCard;
		MonoBehaviour.print("Point System: " + pointSystem.ToString() + "\nStackable Draw: " + stackableDraw.ToString() + "\nChallengable Four: " + challengableFour.ToString() + "\nDraw Cards: " + drawCard.ToString() + "\nMusicID: " + musicID + "\nBGColorID: " + bgColorID);
		clientIsTense = false;
	}

	public void BeginUnoGame(int playerCount)
	{
		unoGame = new UnoGame(playerCount, new bool[4] { pointSystem, stackableDraw, challengableFour, drawCard });
	}

	public void SubmitTurn(int playerID, Actions action, UnoCard card, string extra)
	{
		doingNextPlayerTurn = false;
		canDeletePlayer = false;
		bool drew = false;
		if (action == Actions.ChallengeDare && unoGame.GetPlayerWinState(unoGame.GetPreviousPlayerTurn()) != -1)
		{
			throw new NotSupportedException("Daring got removed (SubmitTurn)");
		}
		if (action == Actions.Draw || action == Actions.DrawAndPlay)
		{
			MonoBehaviour.print(unoGame.GetCurrentDrawCardAmount());
			UnoCard[] cards = unoGame.DrawCards(unoGame.GetCurrentPlayerTurn(), (unoGame.GetCurrentDrawCardAmount() <= 0) ? 1 : unoGame.GetCurrentDrawCardAmount());
			int index = unoGame.GetPlayerHand(unoGame.GetCurrentPlayerTurn()).Count - 1;
			UnoCard unoCard = unoGame.GetPlayerHand(unoGame.GetCurrentPlayerTurn())[index];
			if (action == Actions.DrawAndPlay)
			{
				action = Actions.Play;
				card = unoCard;
				drew = true;
			}
			else
			{
				string dialogue = "* " + players[unoGame.GetCurrentPlayerTurn()].GetPlayerName() + " draws a card.";
				if (unoGame.GetCurrentDrawCardAmount() > 1)
				{
					dialogue = "* " + players[unoGame.GetCurrentPlayerTurn()].GetPlayerName() + " draws " + unoGame.GetCurrentDrawCardAmount() + " cards!";
				}
				DoTurn(players[unoGame.GetCurrentPlayerTurn()].GetPlayerID(), action, null, dialogue, -1);
				ReceiveCardInfo(players[unoGame.GetCurrentPlayerTurn()], cards, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
				unoGame.ResetDrawCardCount();
				stackingImminent = false;
			}
		}
		if (action == Actions.Play)
		{
			UnoCard unoCard2 = unoGame.PlayCard(card, drew);
			bool flag = unoGame.GetPlayerHand(unoGame.GetCurrentPlayerTurn()).Count == 1;
			AIPlayer[] array = UnityEngine.Object.FindObjectsOfType<AIPlayer>();
			foreach (AIPlayer aIPlayer in array)
			{
				aIPlayer.RemoveCardFromMemory(unoGame.GetCurrentPlayerTurn(), card);
				if (flag)
				{
					aIPlayer.BeginUnoCountdown(true);
				}
			}
			if (unoCard2.IsWildCard())
			{
				unoGame.SetLastCardColor(unoCard2.GetCardColor());
			}
			string playerName = players[unoGame.GetCurrentPlayerTurn()].GetPlayerName();
			string playerName2 = players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerName();
			string text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".";
			if (unoCard2.GetCardType() == UnoCard.CardType.Skip)
			{
				text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".\n* " + playerName2 + "'s turn is skipped!";
			}
			else if (unoCard2.GetCardType() == UnoCard.CardType.Reverse)
			{
				string text2 = unoCard2.GetCardName();
				if (text2.StartsWith("YELLOW"))
				{
					text2 = "YELLOW\n  REVERSE";
				}
				text = "* " + playerName + " plays a " + text2 + ".\n* Turn order has been reversed!";
				UnityEngine.Object.FindObjectOfType<UnoPanels>().ReverseTurnOrder();
			}
			else if (unoCard2.GetCardType() == UnoCard.CardType.Wild)
			{
				text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".\n* The color changes to " + unoCard2.GetCardColorName() + "!";
			}
			else if (unoCard2.GetCardType() == UnoCard.CardType.PlusTwo)
			{
				text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".\n* " + playerName2 + " must draw 2 cards!";
				if (CanStackDrawCards())
				{
					text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".\n* " + playerName2 + " must draw " + unoGame.GetCurrentDrawCardAmount() + " cards...";
					stackingImminent = true;
				}
			}
			else if (unoCard2.GetCardType() == UnoCard.CardType.PlusFour)
			{
				text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".\n* The color changes to " + unoCard2.GetCardColorName() + "\n  and " + playerName2 + " must draw 4 cards!";
				if (CanStackDrawCards())
				{
					text = "* " + playerName + " plays a " + unoCard2.GetCardName() + ".\n* " + playerName2 + " must draw " + unoGame.GetCurrentDrawCardAmount() + " cards...";
					stackingImminent = true;
				}
			}
			if (unoGame.GetCurrentDrawCardAmount() > 0)
			{
				if (challengableFour && unoCard2.GetCardType() == UnoCard.CardType.PlusFour && unoGame.GetDiscardPileSize() > 1)
				{
					EnableChallenge(players[unoGame.GetNextUnskippedPlayerTurn()]);
				}
				else if (!stackingImminent)
				{
					UnoCard[] cards2 = unoGame.DrawCards(unoGame.GetNextUnskippedPlayerTurn(), unoGame.GetCurrentDrawCardAmount());
					ReceiveCardInfo(players[unoGame.GetNextUnskippedPlayerTurn()], cards2, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
					unoGame.ResetDrawCardCount();
					unoGame.ForceSkipNextTurn();
				}
			}
			int playerID2 = players[unoGame.GetCurrentPlayerTurn()].GetPlayerID();
			int playerID3 = players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerID();
			if (unoGame.GetPlayerHand(unoGame.GetCurrentPlayerTurn()).Count == 1)
			{
				canSayUno = true;
				curPlayerSaidUno = false;
				callerSaidUno = false;
				text = "\t" + text;
				if (extra == "true")
				{
					curPlayerSaidUno = true;
					SayUno(playerID2, 2);
				}
			}
			DoTurn(playerID2, action, card, text, playerID3);
		}
		switch (action)
		{
		case Actions.AcceptDare:
			throw new NotSupportedException("Daring got removed (SubmitTurn)");
		case Actions.Forfeit:
		{
			unoGame.SetPlayerWinState(unoGame.GetCurrentPlayerTurn(), false);
			int playerID4 = players[unoGame.GetCurrentPlayerTurn()].GetPlayerID();
			string playerName3 = players[unoGame.GetCurrentPlayerTurn()].GetPlayerName();
			ReceiveCardInfo(players[unoGame.GetCurrentPlayerTurn()], "removecards", unoGame.GetDrawPileSize());
			DoTurn(playerID4, action, null, "* " + playerName3 + " forfeits!", -1);
			UnityEngine.Object.FindObjectOfType<UnoPanels>().SetDone(unoGame.GetCurrentPlayerTurn());
			break;
		}
		}
		switch (action)
		{
		case Actions.ChallengeDare:
			throw new NotSupportedException("Daring got removed (SubmitTurn)");
		case Actions.RefuseDare:
			throw new NotSupportedException("Daring got removed (SubmitTurn)");
		case Actions.ChallengePlus4Start:
		case Actions.ChallengePlus4Act:
		{
			challengeImminent = true;
			int num = ((action == Actions.ChallengePlus4Start) ? players[unoGame.GetCurrentPlayerTurn()].GetPlayerID() : players[unoGame.GetPreviousPlayerTurn()].GetPlayerID());
			string text3 = ((action == Actions.ChallengePlus4Start) ? players[unoGame.GetCurrentPlayerTurn()].GetPlayerName() : players[unoGame.GetPreviousPlayerTurn()].GetPlayerName());
			int playerID5 = ((action == Actions.ChallengePlus4Start) ? players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerID() : players[unoGame.GetCurrentPlayerTurn()].GetPlayerID());
			string text4 = ((action == Actions.ChallengePlus4Start) ? players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerName() : players[unoGame.GetCurrentPlayerTurn()].GetPlayerName());
			challengee = playerID5;
			challenger = num;
			challengeDie = false;
			challengeeSuccessful = false;
			int num2 = 0;
			string text5 = "* Error";
			UnoCard secondToLastPlayedCard = unoGame.GetSecondToLastPlayedCard();
			foreach (UnoCard item in unoGame.GetPlayerHand(num))
			{
				if (item.GetCardColor() == secondToLastPlayedCard.GetCardColor() || item.GetCardColor() == UnoCard.WHITE)
				{
					num2++;
					challengeeSuccessful = true;
				}
			}
			unoGame.SetCurrentDrawCardAmount(unoGame.GetCurrentDrawCardAmount() + 2);
			text5 = "* " + text4 + " challenges " + text3 + "'s\n  WILD +4 CARD!\n* The last card was " + secondToLastPlayedCard.GetCardColorName() + ", so...";
			DoTurn(playerID5, action, null, text5, num);
			break;
		}
		}
		if (action == Actions.ChallengePlus4End)
		{
			string playerName4 = players[challenger].GetPlayerName();
			string playerName5 = players[challengee].GetPlayerName();
			UnoCard secondToLastPlayedCard2 = unoGame.GetSecondToLastPlayedCard();
			string text6 = "* Error";
			if (challengeeSuccessful)
			{
				text6 = "* " + playerName4 + " has at least one\n  " + secondToLastPlayedCard2.GetCardColorName() + " card!\n* " + playerName4 + " must draw " + unoGame.GetCurrentDrawCardAmount() + " cards!";
			}
			else
			{
				challengeDie = true;
				text6 = "* " + playerName4 + " has no " + secondToLastPlayedCard2.GetCardColorName() + " cards!\n* " + playerName5 + " must draw " + unoGame.GetCurrentDrawCardAmount() + " cards!";
			}
			DoTurn(challengee, action, null, text6, challenger);
		}
		if (action != Actions.RefuseDare)
		{
			SendEnemyCardInfo();
		}
		if (unoGame.GetPlayerHand(unoGame.GetCurrentPlayerTurn()).Count == 0)
		{
			if (!pointSystem || unoGame.GetPlayerWinState(unoGame.GetCurrentPlayerTurn()) <= -2)
			{
				int playerID6 = players[unoGame.GetCurrentPlayerTurn()].GetPlayerID();
				int num3 = unoGame.GetPlayerWinState(unoGame.GetCurrentPlayerTurn()) + 1;
				SendWinCondition(playerID6, num3 > 0, num3);
			}
			else if (pointSystem)
			{
				unoGame.ScorePlayer(unoGame.GetCurrentPlayerTurn());
				players[unoGame.GetCurrentPlayerTurn()].UpdatePoints(unoGame.GetPlayerScore(unoGame.GetCurrentPlayerTurn()));
			}
		}
	}

	public void DoTurn(int playerID, Actions action, UnoCard card, string dialogue, int targetPlayerID)
	{
		canDeletePlayer = true;
		switch (action)
		{
		case Actions.Play:
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().SetLastCard(card);
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().UpdateDrawPileCount(unoGame.GetCurrentDrawCardAmount());
			break;
		case Actions.Draw:
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().UpdateDrawCount(1);
			stackingImminent = false;
			break;
		case Actions.AcceptDare:
			throw new NotSupportedException("Daring got removed (accept dare)");
		case Actions.ChallengeDare:
			throw new NotSupportedException("Daring got removed (challenge dare)");
		case Actions.RefuseDare:
			throw new NotSupportedException("Daring got removed (dare ruling)");
		case Actions.PointEnd:
			endGame = true;
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().StopMusic();
			break;
		case Actions.ChallengePlus4Start:
		case Actions.ChallengePlus4Act:
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().StartHandScene(targetPlayerID);
			break;
		}
		if (dialogue.StartsWith("\t"))
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().ActivateUno();
			dialogue = dialogue.Substring(1);
		}
		if (dialogue.Contains("draws") && !callerSaidUno)
		{
			UnoPlayer[] array = players;
			foreach (UnoPlayer unoPlayer in array)
			{
				if (unoPlayer != null && unoPlayer != players[unoGame.GetNextUnskippedPlayerTurn()])
				{
					ReceiveCardInfo(unoPlayer, null, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
				}
			}
			if (CanStackDrawCards() && dialogue.Contains("must draw"))
			{
				stackingImminent = true;
			}
		}
		string[] dialogue2 = dialogue.Split(new char[1] { '`' }, 2);
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().StartFormattedText(dialogue2, true, playerID, targetPlayerID);
	}

	public void FinishTurn(int specialCondition)
	{
		if (!clientPlayer.IsHost())
		{
			return;
		}
		canDeletePlayer = false;
		if (specialCondition == 2)
		{
			throw new NotSupportedException("Daring got removed (FinishTurn)");
		}
		int currentPlayerTurn = unoGame.GetCurrentPlayerTurn();
		if (canSayUno && callerSaidUno)
		{
			UnoCard[] cards = unoGame.DrawCards(currentPlayerTurn, 4);
			UnoPlayer[] array = players;
			UnoPlayer[] array2 = players;
			foreach (UnoPlayer unoPlayer in array2)
			{
				if (unoPlayer != null)
				{
					bool flag = false;
					if (unoPlayer == array[unoGame.GetCurrentPlayerTurn()])
					{
						instance.ReceiveCardInfo(unoPlayer, cards, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
						flag = true;
					}
					if (!flag)
					{
						instance.ReceiveCardInfo(unoPlayer, null, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
					}
				}
			}
			SendEnemyCardInfo();
		}
		if (challengableFour && challengeImminent)
		{
			int currentDrawCardAmount = unoGame.GetCurrentDrawCardAmount();
			int num = (challengeeSuccessful ? challenger : challengee);
			UnoCard[] cards2 = unoGame.DrawCards(num, currentDrawCardAmount);
			instance.ReceiveCardInfo(players[num], cards2, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
			unoGame.ResetDrawCardCount();
			instance.ReceiveCardInfo("drawcount", unoGame.GetCurrentDrawCardAmount());
			if (num == unoGame.GetNextUnskippedPlayerTurn())
			{
				unoGame.ForceSkipNextTurn();
			}
			challengeImminent = false;
			challengeeSuccessful = false;
			stackingImminent = false;
			SendEnemyCardInfo();
		}
		if (unoGame.GetCurrentActivePlayerCount() > 1)
		{
			int num2 = unoGame.GetNextPlayerTurn(true);
			if (specialCondition == 0 && !canSayUno)
			{
				if (!pointSystem)
				{
					instance.ReceiveCardInfo(null, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
				}
				else
				{
					UnoPlayer[] array2 = players;
					foreach (UnoPlayer unoPlayer2 in array2)
					{
						if (unoPlayer2 != null && unoPlayer2 != players[currentPlayerTurn])
						{
							instance.ReceiveCardInfo(unoPlayer2, null, unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
						}
					}
				}
			}
			else if (specialCondition == 1)
			{
				num2 = 0;
				unoGame.SetNextPlayer(0);
				UnoPlayer[] array3 = players;
				UnoPlayer[] array2 = players;
				foreach (UnoPlayer unoPlayer3 in array2)
				{
					if (!(unoPlayer3 != null))
					{
						continue;
					}
					for (int j = 0; j < array3.Length; j++)
					{
						if (unoPlayer3 == array3[j])
						{
							instance.ReceiveCardInfo(unoPlayer3, unoGame.GetPlayerHand(j).ToArray(), unoGame.GetDrawPileSize(), unoGame.GetTopDrawCard());
						}
					}
				}
				SendEnemyCardInfo();
				UnityEngine.Object.FindObjectOfType<PartyPanels>().SetXOffset(0, -185);
			}
			int playerID = players[num2].GetPlayerID();
			string playerName = players[num2].GetPlayerName();
			int playerID2 = players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerID();
			string playerName2 = players[unoGame.GetNextUnskippedPlayerTurn()].GetPlayerName();
			if (canSayUno)
			{
				canSayUno = false;
			}
			if (stackingImminent && unoGame.GetCurrentDrawCardAmount() < 2)
			{
				stackingImminent = false;
			}
			if (stackingImminent)
			{
				string pronoun = players[num2].GetPronoun(0);
				instance.ReceiveCardInfo("drawcount", unoGame.GetCurrentDrawCardAmount());
				instance.DoNextTurn(playerID, "* Unless " + pronoun + " can stack on top\n  of it!", 1, playerID2, unoGame.GetTopDrawCard(), unoGame.GetDrawPileSize());
			}
			else
			{
				instance.DoNextTurn(playerID, "* It's " + playerName + "'s turn.\n* " + playerName2 + " is next.", 0, playerID2, unoGame.GetTopDrawCard(), unoGame.GetDrawPileSize());
			}
		}
		else if (unoGame.GetCurrentActivePlayerCount() <= 1)
		{
			UnoPlayer[] array4 = players;
			string text = "";
			string text2 = "kris";
			MonoBehaviour.print(unoGame.GetCurrentActivePlayerCount());
			if (unoGame.GetCurrentActivePlayerCount() == 1)
			{
				int lastActivePlayer = unoGame.GetLastActivePlayer();
				int playerID3 = players[lastActivePlayer].GetPlayerID();
				unoGame.SetPlayerWinState(unoGame.GetLastActivePlayer(), true);
				int num3 = unoGame.GetPlayerWinState(lastActivePlayer) + 1;
				instance.SendWinCondition(playerID3, num3 > 0, num3);
			}
			for (int k = 0; k < array4.Length; k++)
			{
				if (unoGame.GetPlayerWinState(k) < 0 || !(array4[k] != null))
				{
					text = ((unoGame.GetPlayerWinState(k) != -2 || !(array4[k] != null)) ? (text + "-2DISCONNECTED") : (text + (unoGame.GetPlayerWinState(k) + 1) + array4[k].GetPlayerName()));
				}
				else
				{
					text = text + (unoGame.GetPlayerWinState(k) + 1).ToString("D2") + array4[k].GetPlayerName();
					if (unoGame.GetPlayerWinState(k) == 0)
					{
						text2 = GetSkinFilename(array4[k].GetSkin());
					}
				}
				text += "`";
			}
			text += text2;
			instance.EndGame(text);
		}
		if (unoGame != null)
		{
			UnityEngine.Object.FindObjectOfType<UnoPanels>().UpdateTurn(unoGame.GetCurrentPlayerTurn());
		}
	}

	public void ReceiveCardInfo(UnoPlayer targetPlayer, string fullString, int drawSize)
	{
		if (!(targetPlayer == clientPlayer))
		{
			return;
		}
		if (fullString == "drawcount")
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().UpdateDrawCount(drawSize);
			return;
		}
		if (fullString == "removecards")
		{
			UnityEngine.Object.FindObjectOfType<CardHand>().RemoveAllCards();
			return;
		}
		throw new ArgumentException("Invalid argument '" + fullString + "' - must be either 'drawcount' or 'removecards'");
	}

	public void ReceiveCardInfo(UnoPlayer targetPlayer, UnoCard[] cards, int drawSize, UnoCard nextCard)
	{
		UnoBattleManager unoBattleManager = UnityEngine.Object.FindObjectOfType<UnoBattleManager>();
		if (targetPlayer == clientPlayer)
		{
			if (cards != null)
			{
				unoBattleManager.AddCards(cards);
			}
			unoBattleManager.UpdateDrawPileCount(unoGame.GetCurrentDrawCardAmount());
		}
		unoBattleManager.UpdateNextDrawCard(nextCard);
	}

	public void ReceiveCardInfo(UnoCard[] cards, int drawSize, UnoCard nextCard)
	{
		UnoPlayer[] array = players;
		foreach (UnoPlayer targetPlayer in array)
		{
			ReceiveCardInfo(targetPlayer, cards, drawSize, nextCard);
		}
	}

	public void ReceiveCardInfo(string fullString, int drawSize)
	{
		UnoPlayer[] array = players;
		foreach (UnoPlayer targetPlayer in array)
		{
			ReceiveCardInfo(targetPlayer, fullString, drawSize);
		}
	}

	public void DoNextTurn(int playerID, string flavorText, int specialCondition, int nextPlayerID, UnoCard nextCard, int drawSize)
	{
		canDeletePlayer = true;
		doingNextPlayerTurn = true;
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().DeactivateUno();
		switch (specialCondition)
		{
		case 1:
			stackingImminent = true;
			break;
		case 2:
			throw new NotSupportedException("Daring got removed (DoNextTurn)");
		default:
			stackingImminent = false;
			break;
		}
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().StartFormattedText(new string[1] { flavorText }, false, playerID, nextPlayerID);
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().UpdateDrawPileCount(unoGame.GetCurrentDrawCardAmount());
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().UpdateNextDrawCard(nextCard);
		AIPlayer aIPlayer;
		if ((object)(aIPlayer = players[playerID] as AIPlayer) != null)
		{
			ValueTuple<AIAction, int> valueTuple = aIPlayer.AITurn();
			AIAction aIAction = valueTuple.Item1;
			int item = valueTuple.Item2;
			if (unoGame.GetTopCard() != null)
			{
				if ((aIAction == AIAction.Play && !unoGame.GetTopCard().CanBePlacedOn(unoGame.GetPlayerHand(playerID)[item])) || (aIAction == AIAction.DrawAndPlay && !unoGame.GetTopCard().CanBePlacedOn(unoGame.GetTopDrawCard())))
				{
					Debug.LogError(string.Format("AI Player {0} tried playing {1} on {2} (stack size = {3}); forcing draw", aIPlayer.GetPlayerName(), unoGame.GetPlayerHand(playerID)[item].GetCardName(), unoGame.GetTopCard().GetCardName(), unoGame.GetCurrentDrawCardAmount()));
					aIAction = AIAction.Draw;
				}
				else if (aIAction == AIAction.DrawAndPlay && stackingImminent)
				{
					Debug.LogError(string.Format("AI Player {0} tried DrawAndPlay during stack, which is not allowed; forcing draw.", aIPlayer.GetPlayerName()));
					aIAction = AIAction.Draw;
				}
				if (aIAction == AIAction.ChallengePlusFour)
				{
					SubmitTurn(aIPlayer.GetPlayerID(), Actions.ChallengePlus4Act, null, "");
					return;
				}
			}
			if (aIAction < AIAction.Play)
			{
				throw new InvalidOperationException("AI player " + playerID + " (" + aIPlayer.GetPlayerName() + ") returned AIAction.Invalid");
			}
			List<UnoCard> playerHand = unoGame.GetPlayerHand(playerID);
			UnoCard playedCard = ((item >= 0 && item < playerHand.Count) ? playerHand[item] : null);
			if (aIAction == AIAction.DrawAndPlay)
			{
				playedCard = unoGame.GetTopDrawCard();
			}
			if (aIAction == AIAction.Play || aIAction == AIAction.DrawAndPlay)
			{
				if (playedCard == null)
				{
					throw new InvalidOperationException("Cannot play non-existing card " + item);
				}
				if (playedCard.IsWildCard())
				{
					aIPlayer.ChooseWildColor(ref playedCard);
				}
			}
			if (unoGame.GetTopCard() != null)
			{
				string text = "Top: " + unoGame.GetTopCard().GetCardName();
				if (unoGame.GetTopCard().IsWildCard())
				{
					text = text + " [" + unoGame.GetTopCard().GetCardColorName() + "]";
				}
				Debug.Log(text);
			}
			Debug.Log(string.Concat(aIPlayer.GetPlayerName(), ": [", aIAction, (playedCard == null) ? "" : (" " + playedCard.GetCardName()), "]"));
			SubmitTurn(playerID, (Actions)aIAction, playedCard, "");
			if (playerHand.Count == 1)
			{
				aIPlayer.BeginUnoCountdown();
			}
		}
		if (!clientPlayer.IsSpeaking())
		{
			bool flag = clientPlayer.GetPlayerID() == unoGame.GetCurrentPlayerTurn();
			UnityEngine.Object.FindObjectOfType<PartyPanels>().RaiseHeads(flag, false, false);
			UnityEngine.Object.FindObjectOfType<PartyPanels>().SetRaisedPanel((!flag) ? (-1) : 0);
		}
	}

	public void SayUno(int callerID, int condition = 0)
	{
		if (!canSayUno)
		{
			return;
		}
		int playerID = players[unoGame.GetCurrentPlayerTurn()].GetPlayerID();
		if (!curPlayerSaidUno && !callerSaidUno)
		{
			string playerName = players[unoGame.GetCurrentPlayerTurn()].GetPlayerName();
			string playerName2 = players[callerID].GetPlayerName();
			if (callerID != playerID)
			{
				DoTurn(playerID, Actions.FailUno, null, "* " + playerName2 + " called UNO first!\n* " + playerName + " must draw 4 cards!", callerID);
				callerSaidUno = true;
				condition = 1;
			}
			else
			{
				curPlayerSaidUno = true;
				condition = 2;
			}
		}
		players[callerID].SayUno(condition);
	}

	public void SendWinCondition(int playerID, bool win, int place)
	{
		UnityEngine.Object.FindObjectOfType<UnoPanels>().SetDone(playerID);
		UnoEnemy[] array = UnityEngine.Object.FindObjectsOfType<UnoEnemy>();
		foreach (UnoEnemy unoEnemy in array)
		{
			if (unoEnemy.GetPlayer().GetPlayerID() == playerID)
			{
				if ((place > -2 && !pointSystem) || (place == -1 && pointSystem))
				{
					unoEnemy.Spare(win);
				}
				else if (place <= -2)
				{
					unoEnemy.TurnToDust();
				}
				break;
			}
		}
		if (clientPlayer.GetPlayerID() == playerID)
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().SetWinCondition(place, false);
			clientPlaceNo = place;
			if (clientPlaceNo <= -1)
			{
				UnityEngine.Object.FindObjectOfType<UnoBattleManager>().GetPlayerSOUL().UnoDamage(0f);
				UnityEngine.Object.FindObjectOfType<PartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
			}
		}
		if (ClientHasFinished() || place <= 0)
		{
			return;
		}
		float num = 1f;
		float num2 = 1f;
		array = UnityEngine.Object.FindObjectsOfType<UnoEnemy>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].AcceptingNewChanges())
			{
				num += 1f;
			}
			num2 += 1f;
		}
		if (num <= 1f)
		{
			endGame = true;
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().StopMusic();
		}
		num -= 1f;
		num2 -= 1f;
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().GetPlayerSOUL().UnoDamage(num / num2);
	}

	public void EndGame(string names)
	{
		playing = false;
		string[] array = names.Split('`');
		List<KeyValuePair<int, string>> list = new List<KeyValuePair<int, string>>();
		for (int i = 0; i < array.Length - 1; i++)
		{
			list.Add(new KeyValuePair<int, string>(int.Parse(array[i].Substring(0, 2)), array[i].Substring(2)));
		}
		UnityEngine.Object.FindObjectOfType<UnoBattleManager>().FadeEndBattle();
		unoGame = null;
	}

	public void UpdateEnemyCards(List<EnemyCardInfo> fullInfo)
	{
		int num = ((!ClientHasFinished()) ? 1 : 0);
		int num2 = 0;
		foreach (EnemyCardInfo item in fullInfo)
		{
			UnoEnemy[] array = UnityEngine.Object.FindObjectsOfType<UnoEnemy>();
			foreach (UnoEnemy unoEnemy in array)
			{
				if (unoEnemy.GetPlayer().GetPlayerID() != item.playerID)
				{
					continue;
				}
				int cardCount = unoEnemy.GetCardCount();
				unoEnemy.SetCardCount(item.cardCount);
				unoEnemy.SetTenseFace(item.winCondition);
				if (cardCount - unoEnemy.GetCardCount() <= -2)
				{
					if (!unoEnemy.HasBegun())
					{
						unoEnemy.StartEnemyChanges();
					}
					else if (cardCount - unoEnemy.GetCardCount() != -7)
					{
						unoEnemy.Hit(0, 0f, true);
					}
				}
				if (unoEnemy.AcceptingNewChanges() && !unoEnemy.IsKilled())
				{
					num++;
				}
			}
			if (item.playerID == clientPlayer.GetPlayerID() && UnityEngine.Object.FindObjectOfType<UnoBattleManager>().GetState() != 9 && item.winCondition != UnoEnemy.WinCondition.Neutral && GetUnoMusic(musicID).Length > 1)
			{
				num2 = 1;
			}
		}
		if (num > 1 && !endGame)
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().PlayMusic(GetUnoMusic(musicID)[num2].Key, GetUnoMusic(musicID)[num2].Value);
		}
		else
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().StopMusic();
		}
	}

	public void EnableChallenge(UnoPlayer targetPlayer)
	{
		if (targetPlayer == clientPlayer)
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().EnableChallenge();
		}
	}

	public void UpdateWinCondition(UnoPlayer targetPlayer, int playerID, int place)
	{
		if (clientPlayer.GetPlayerID() == playerID)
		{
			UnityEngine.Object.FindObjectOfType<UnoBattleManager>().SetWinCondition(place, true);
			clientPlaceNo = place;
		}
	}

	public void SendEnemyCardInfo()
	{
		List<EnemyCardInfo> list = new List<EnemyCardInfo>();
		for (int i = 0; i < players.Length; i++)
		{
			if (!players[i])
			{
				continue;
			}
			UnoEnemy.WinCondition winCondition = UnoEnemy.WinCondition.Neutral;
			if (MusicChooser.musicID == MusicChooser.FRANKNESS_ID && players[i].GetSkin() != 4)
			{
				winCondition = UnoEnemy.WinCondition.Nervous;
			}
			if (challengeImminent)
			{
				winCondition = ((i != challengee || challengeDie) ? UnoEnemy.WinCondition.Losing : UnoEnemy.WinCondition.Winning);
			}
			else if (unoGame.GetPlayerScore(i) >= 400 && pointSystem)
			{
				winCondition = UnoEnemy.WinCondition.Winning;
			}
			else if (unoGame.GetPlayerHand(i).Count == 1)
			{
				winCondition = UnoEnemy.WinCondition.Winning;
			}
			else if (unoGame.GetPlayerHand(i).Count >= 12)
			{
				winCondition = UnoEnemy.WinCondition.Losing;
			}
			else if (i == unoGame.GetNextUnskippedPlayerTurn() && stackingImminent && unoGame.GetCurrentDrawCardAmount() < 6)
			{
				winCondition = UnoEnemy.WinCondition.Losing;
			}
			else if (stackingImminent && unoGame.GetCurrentDrawCardAmount() >= 6)
			{
				winCondition = UnoEnemy.WinCondition.Losing;
			}
			else if (unoGame.GetCurrentActivePlayerCount() == 2)
			{
				int num = -1;
				int i2 = -1;
				if (i == unoGame.GetCurrentPlayerTurn())
				{
					num = unoGame.GetCurrentPlayerTurn();
					i2 = unoGame.GetNextUnskippedPlayerTurn();
				}
				else if (i == unoGame.GetNextUnskippedPlayerTurn())
				{
					num = unoGame.GetNextUnskippedPlayerTurn();
					i2 = unoGame.GetCurrentPlayerTurn();
				}
				if (num != -1)
				{
					if (unoGame.GetPlayerHand(num).Count < unoGame.GetPlayerHand(i2).Count && ((unoGame.GetPlayerHand(num).Count <= 3 && unoGame.GetPlayerCount() == 2) || unoGame.GetPlayerCount() > 2))
					{
						winCondition = UnoEnemy.WinCondition.Winning;
					}
					else if (unoGame.GetPlayerHand(num).Count >= unoGame.GetPlayerHand(i2).Count && ((unoGame.GetPlayerHand(i2).Count <= 3 && unoGame.GetPlayerCount() == 2) || unoGame.GetPlayerCount() > 2))
					{
						winCondition = UnoEnemy.WinCondition.Losing;
					}
				}
				else
				{
					winCondition = UnoEnemy.WinCondition.Losing;
				}
			}
			list.Add(new EnemyCardInfo(players[i].GetPlayerID(), unoGame.GetPlayerHand(i).Count, winCondition));
		}
		instance.UpdateEnemyCards(list);
	}

	public UnoPlayer[] GetPlayers()
	{
		return players;
	}

	public static bool IsPlayersReady()
	{
		UnoPlayer[] array = UnityEngine.Object.FindObjectsOfType<UnoPlayer>();
		foreach (UnoPlayer unoPlayer in array)
		{
			if (unoPlayer != null && unoPlayer.GetPlayerID() != 0 && !unoPlayer.IsReady())
			{
				return false;
			}
		}
		return true;
	}

	public bool IsPlaying()
	{
		return playing;
	}

	public bool PointSystemEnabled()
	{
		return pointSystem;
	}

	public bool CanStackDrawCards()
	{
		return stackableDraw;
	}

	public bool CanChallengePlusFourCards()
	{
		return challengableFour;
	}

	public bool CurrentlyStackingDrawCards()
	{
		return stackingImminent;
	}

	public int GetMusicID()
	{
		return musicID;
	}

	public int GetBGColorID()
	{
		return bgColorID;
	}

	public int GetBGType()
	{
		return bgType;
	}

	public bool ClientHasFinished()
	{
		return clientPlaceNo != 0;
	}

	public UnoPlayer GetClientPlayer()
	{
		return clientPlayer;
	}

	public UnoGame GetUnoGame()
	{
		return unoGame;
	}

	public void DebugChoice(int playerID, int choice)
	{
		UnoPlayer unoPlayer = players[playerID];
		switch (choice)
		{
		case 0:
		{
			FreeCardHand component = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("uno/FreeCardHand")).GetComponent<FreeCardHand>();
			{
				foreach (UnoCard item in unoGame.GetPlayerHand(unoPlayer.GetPlayerID()))
				{
					component.AddCard(item.GetCardType(), item.GetCardColor());
				}
				break;
			}
		}
		case 1:
		case 2:
			unoGame.ModifyCards(playerID, (choice != 1) ? 1 : (-1));
			SendEnemyCardInfo();
			break;
		case 3:
		case 4:
		{
			unoGame.SetPlayerWinState(playerID, choice == 3);
			int num = ((choice == 3) ? unoGame.GetPlayerWinState(playerID) : (-1));
			SendWinCondition(playerID, num >= 0, num);
			Debug.Log(num);
			break;
		}
		}
	}

	public void SetupPlayers()
	{
		bool flag = MusicChooser.musicID == MusicChooser.FRANKNESS_ID;
		turnOrder = new List<int>();
		if (!flag)
		{
			if (PLAYER_GOES_FIRST)
			{
				turnOrder.Add(0);
			}
			if (RANDOM_TURN_ORDER)
			{
				while (turnOrder.Count < 4)
				{
					int item = UnityEngine.Random.Range(0, 4);
					if (!turnOrder.Contains(item))
					{
						turnOrder.Add(item);
					}
				}
			}
		}
		if (turnOrder.Count == 1)
		{
			turnOrder.AddRange(new int[3] { 2, 1, 3 });
		}
		else if (turnOrder.Count == 0)
		{
			turnOrder.AddRange(new int[4] { 0, 2, 1, 3 });
		}
		string[] array = new string[4] { "Kris", "Susie", "Noelle", "Papyrus" };
		int[] array2 = new int[4]
		{
			0,
			1,
			2,
			flag ? 4 : 3
		};
		int[] array3 = new int[4] { 2, 0, 0, 1 };
		int[] obj = new int[4] { -1, 1, 2, 0 };
		obj[3] = (flag ? 4 : 3);
		int[] array4 = obj;
		for (int i = 0; i < 4; i++)
		{
			int num = turnOrder[i];
			CreatePlayer(array[num], array2[num], array3[num], array4[num]);
		}
	}

	public List<int> GetTurnOrder()
	{
		return turnOrder;
	}

	public static string[] GetSkinArray()
	{
		return new List<string> { "kris", "susie", "noelle", "papyrus", "papyrus_hard" }.ToArray();
	}

	public static string GetSkinFilename(int i)
	{
		if (i >= GetSkinArray().Length)
		{
			return "kris";
		}
		return GetSkinArray()[i];
	}

	public static string GetSkinName(string filename)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "papyrus", "Papyrus" },
			{ "kris", "Kris" },
			{ "susie", "Susie" },
			{ "noelle", "Noelle" },
			{ "papyrus_hard", "Papyrus" }
		};
		if (dictionary.ContainsKey(filename))
		{
			return dictionary[filename];
		}
		return "Unnamed";
	}

	public static string GetSkinTextSound(string filename)
	{
		Dictionary<string, string> dictionary = new Dictionary<string, string>
		{
			{ "papyrus", "snd_txtpap" },
			{ "susie", "snd_txtsus" },
			{ "kris", "snd_txtkrs" },
			{ "noelle", "snd_txtnoe" },
			{ "papyrus_hard", "snd_txtpap" }
		};
		if (dictionary.ContainsKey(filename))
		{
			return dictionary[filename];
		}
		return "snd_text";
	}

	public static string GetSkinUNODialogue(string filename, int condition)
	{
		if (Localizer.HasText("uno_" + filename + "_" + condition))
		{
			return "uno_" + filename + "_" + condition;
		}
		return "uno_default";
	}

	public Color32 GetSOULColor(int i)
	{
		return (new Color32[9]
		{
			Color.red,
			new Color32(66, 252, byte.MaxValue, byte.MaxValue),
			new Color32(252, 166, 0, byte.MaxValue),
			new Color32(0, 60, byte.MaxValue, byte.MaxValue),
			new Color32(213, 53, 217, byte.MaxValue),
			new Color32(0, 192, 0, byte.MaxValue),
			new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue),
			new Color32(byte.MaxValue, 174, 201, byte.MaxValue),
			Color.white
		})[i];
	}

	public static Color[] GetBGColors(bool dark = false)
	{
		if (!dark)
		{
			return new Color[6]
			{
				new Color32(34, 177, 76, byte.MaxValue),
				new Color32(66, 0, 66, byte.MaxValue),
				new Color32(159, 0, 0, byte.MaxValue),
				new Color32(0, 107, 183, byte.MaxValue),
				new Color32(83, 83, 83, byte.MaxValue),
				new Color(0f, 0f, 0f, 0f)
			};
		}
		return new Color[6]
		{
			new Color32(0, 192, 0, 24),
			new Color32(213, 53, 217, 24),
			new Color32(byte.MaxValue, 0, 0, 24),
			new Color32(0, 60, byte.MaxValue, 24),
			new Color32(192, 192, 192, 24),
			new Color(0f, 0f, 0f, 0f)
		};
	}

	public static Color GetBGColor(int i)
	{
		Color[] bGColors = GetBGColors();
		if (i < bGColors.Length)
		{
			return bGColors[i];
		}
		return bGColors[0];
	}

	public static KeyValuePair<string, float>[][] GetUnoMusicArray()
	{
		return new KeyValuePair<string, float>[21][]
		{
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_date", 1f),
				new KeyValuePair<string, float>("music/mus_date_fight", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_battle1", 1f),
				new KeyValuePair<string, float>("music/mus_battle2", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_muscle", 1f),
				new KeyValuePair<string, float>("music/mus_sansfight", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_undyneboss", 1f),
				new KeyValuePair<string, float>("music/mus_justice", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_spider", 1f),
				new KeyValuePair<string, float>("music/mus_mewmew", 0.95f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_battledelta", 1f),
				new KeyValuePair<string, float>("music/mus_checkers", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_mansion_entrance", 1f),
				new KeyValuePair<string, float>("music/mus_mansion", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_berdly_chase", 1f),
				new KeyValuePair<string, float>("music/mus_queen_boss", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_spamton_battle", 1f),
				new KeyValuePair<string, float>("music/mus_spamton_neo_mix_ex_wip", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_battle", 1f),
				new KeyValuePair<string, float>("music/mus_battle_hard", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_battle_eb", 1f),
				new KeyValuePair<string, float>("music/mus_unsettling_battle", 1f)
			},
			new KeyValuePair<string, float>[1]
			{
				new KeyValuePair<string, float>("music/mus_floweyboss", 1f)
			},
			new KeyValuePair<string, float>[1]
			{
				new KeyValuePair<string, float>("music/mus_pokeyboss_intro", 1f)
			},
			new KeyValuePair<string, float>[1]
			{
				new KeyValuePair<string, float>("music/mus_vsufsans_intro", 1f)
			},
			new KeyValuePair<string, float>[1]
			{
				new KeyValuePair<string, float>("music/mus_frankness_intro", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_sandstorm_approaching", 1f),
				new KeyValuePair<string, float>("music/mus_deal_em_out", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_decibat", 1f),
				new KeyValuePair<string, float>("music/mus_dalv_intro", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_protocol", 1f),
				new KeyValuePair<string, float>("music/mus_apprehension", 1f)
			},
			new KeyValuePair<string, float>[1]
			{
				new KeyValuePair<string, float>("music/mus_showdown", 1f)
			},
			new KeyValuePair<string, float>[2]
			{
				new KeyValuePair<string, float>("music/mus_guns_blazing", 1f),
				new KeyValuePair<string, float>("music/mus_end_of_the_line", 1f)
			},
			new KeyValuePair<string, float>[1]
			{
				new KeyValuePair<string, float>("music/mus_some_point_of_no_return", 1f)
			}
		};
	}

	public static KeyValuePair<string, float>[] GetUnoMusic(int i)
	{
		KeyValuePair<string, float>[][] unoMusicArray = GetUnoMusicArray();
		if (i < unoMusicArray.Length)
		{
			return unoMusicArray[i];
		}
		return unoMusicArray[0];
	}
}
