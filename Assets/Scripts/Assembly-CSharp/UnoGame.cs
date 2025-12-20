using System;
using System.Collections.Generic;
using UnityEngine;

public class UnoGame
{
	private List<UnoCard>[] players;

	private List<UnoCard> drawPile;

	private List<UnoCard> discardPile;

	private int[] points;

	private int[] winningPlaces;

	private int prevPlayerTurn;

	private int turn;

	private bool reverse;

	private bool challengeImminent;

	private bool skipNextTurn;

	private int currentDrawCount;

	private int currentWinPlace;

	private bool pointSystem;

	private bool stackDrawCards;

	private bool challengePlusFour;

	public UnoGame(UnoGame game)
	{
		players = new List<UnoCard>[game.players.Length];
		for (int i = 0; i < players.Length; i++)
		{
			players[i] = SafeCopy(game.players[i]);
		}
		drawPile = SafeCopy(game.drawPile);
		discardPile = SafeCopy(game.discardPile);
		points = (int[])game.points.Clone();
		winningPlaces = (int[])game.winningPlaces.Clone();
		prevPlayerTurn = game.prevPlayerTurn;
		turn = game.turn;
		reverse = game.reverse;
		challengeImminent = game.challengeImminent;
		skipNextTurn = game.skipNextTurn;
		currentDrawCount = game.currentDrawCount;
		currentWinPlace = game.currentWinPlace;
		pointSystem = game.pointSystem;
		stackDrawCards = game.stackDrawCards;
		challengePlusFour = game.challengePlusFour;
	}

	public UnoGame(int playerCount, bool[] rules)
	{
		players = new List<UnoCard>[playerCount];
		drawPile = new List<UnoCard>();
		discardPile = new List<UnoCard>();
		points = new int[playerCount];
		winningPlaces = new int[playerCount];
		UnoCard.CardColor[] array = new UnoCard.CardColor[4]
		{
			UnoCard.RED,
			UnoCard.YELLOW,
			UnoCard.GREEN,
			UnoCard.BLUE
		};
		foreach (UnoCard.CardColor color in array)
		{
			discardPile.Add(new UnoCard(UnoCard.CardType.Zero, color));
			for (int j = 1; j < 10; j++)
			{
				discardPile.Add(new UnoCard((UnoCard.CardType)j, color));
				discardPile.Add(new UnoCard((UnoCard.CardType)j, color));
			}
			discardPile.Add(new UnoCard(UnoCard.CardType.Skip, color));
			discardPile.Add(new UnoCard(UnoCard.CardType.Skip, color));
			discardPile.Add(new UnoCard(UnoCard.CardType.Reverse, color));
			discardPile.Add(new UnoCard(UnoCard.CardType.Reverse, color));
			discardPile.Add(new UnoCard(UnoCard.CardType.PlusTwo, color));
			discardPile.Add(new UnoCard(UnoCard.CardType.PlusTwo, color));
		}
		for (int k = 0; k < 4; k++)
		{
			discardPile.Add(new UnoCard(UnoCard.CardType.Wild, UnoCard.WHITE));
			discardPile.Add(new UnoCard(UnoCard.CardType.PlusFour, UnoCard.WHITE));
		}
		pointSystem = rules[0];
		stackDrawCards = rules[1];
		challengePlusFour = rules[2];
		_ = rules[3];
		ShuffleCards();
		for (int l = 0; l < playerCount; l++)
		{
			players[l] = new List<UnoCard>();
			DrawCards(l, 7);
			points[l] = 0;
			winningPlaces[l] = -1;
		}
		prevPlayerTurn = 0;
		turn = 0;
		reverse = false;
		challengeImminent = false;
		currentDrawCount = 0;
		skipNextTurn = false;
		currentWinPlace = 0;
	}

	private static List<UnoCard> SafeCopy(List<UnoCard> cards)
	{
		List<UnoCard> list = new List<UnoCard>();
		foreach (UnoCard card in cards)
		{
			if (card.IsWildCard())
			{
				UnoCard unoCard = new UnoCard(card.GetCardType(), null);
				unoCard.SetWildColor(card.GetCardColor());
				list.Add(unoCard);
			}
			else
			{
				list.Add(card);
			}
		}
		return list;
	}

	private void ShuffleCards()
	{
		while (discardPile.Count > 0)
		{
			int index = Mathf.RoundToInt(UnityEngine.Random.Range(0, discardPile.Count - 1));
			if (discardPile[index].IsWildCard())
			{
				discardPile[index].SetWildColor(UnoCard.WHITE);
			}
			drawPile.Add(discardPile[index]);
			discardPile.RemoveAt(index);
		}
	}

	public UnoCard[] DrawCards(int player, int amount)
	{
		if (drawPile.Count <= amount)
		{
			ShuffleCards();
		}
		if (drawPile.Count <= amount)
		{
			amount = drawPile.Count;
		}
		UnoCard[] array = new UnoCard[amount];
		for (int i = 0; i < amount; i++)
		{
			players[player].Add(drawPile[0]);
			array[i] = drawPile[0];
			drawPile.RemoveAt(0);
		}
		return array;
	}

	public UnoCard PlayCard(UnoCard card, bool drew = false)
	{
		UnoCard topCard = GetTopCard();
		if (!drew && !players[turn].Contains(card))
		{
			string text = "Player " + turn + " cannot play card '" + card.GetCardName() + "' ";
			int num = -1;
			for (int i = 0; i < players.Length; i++)
			{
				if (players[i].Contains(card))
				{
					num = i;
					break;
				}
			}
			text = ((num >= 0) ? (text + "as Player " + num + " currently holds it") : (text + "as no player is currently holding it"));
			throw new InvalidOperationException(text);
		}
		if (currentDrawCount > 0 && card.type != UnoCard.CardType.PlusTwo && card.type != UnoCard.CardType.PlusFour)
		{
			throw new InvalidOperationException("Cannot place " + card.GetCardName() + " (draw count is " + currentDrawCount + ")");
		}
		if (!card.CanBePlacedOn(topCard))
		{
			string text2 = topCard?.GetCardName();
			if (text2 == null)
			{
				text2 = "empty stack???";
			}
			else if (topCard.IsWildCard())
			{
				text2 = text2 + " [" + topCard.GetCardColor()?.ToString() + "]";
			}
			throw new InvalidOperationException("Card " + card.GetCardName() + " cannot be placed on " + text2);
		}
		discardPile.Add(card);
		players[turn].Remove(card);
		if (card.GetCardType() == UnoCard.CardType.Reverse)
		{
			reverse = !reverse;
			Debug.Log("Reverse: " + reverse);
			Debug.Log(GetCurrentActivePlayerCount());
			if (GetCurrentActivePlayerCount() == 2)
			{
				skipNextTurn = true;
			}
		}
		else if (card.GetCardType() == UnoCard.CardType.Skip)
		{
			skipNextTurn = true;
		}
		else if (card.GetCardType() == UnoCard.CardType.PlusFour)
		{
			currentDrawCount += 4;
		}
		else if (card.GetCardType() == UnoCard.CardType.PlusTwo)
		{
			currentDrawCount += 2;
		}
		if (players[turn].Count == 0 && !pointSystem)
		{
			winningPlaces[turn] = currentWinPlace;
			currentWinPlace++;
		}
		return card;
	}

	public void ModifyCards(int player, int amt)
	{
		if (amt > 0)
		{
			for (int i = 0; i < amt; i++)
			{
				UnoCard topDrawCard = GetTopDrawCard();
				players[turn].Add(topDrawCard);
				discardPile.Remove(topDrawCard);
			}
		}
		else if (amt < 0)
		{
			for (int j = 0; j < -amt; j++)
			{
				UnoCard item = players[player][players[player].Count - 1];
				discardPile.Add(item);
				players[turn].Remove(item);
			}
		}
	}

	public int GetNextPlayerTurn(bool set)
	{
		int num = turn;
		bool flag = skipNextTurn;
		while (true)
		{
			num = ((!reverse) ? ((num + 1) % players.Length) : ((num - 1) % players.Length));
			if (num < 0)
			{
				num = players.Length + num;
			}
			if (winningPlaces[num] == -1)
			{
				if (!flag)
				{
					break;
				}
				flag = false;
			}
		}
		if (set)
		{
			if (winningPlaces[turn] > -3)
			{
				prevPlayerTurn = turn;
			}
			turn = num;
			skipNextTurn = false;
		}
		return num;
	}

	public int GetNextUnskippedPlayerTurn()
	{
		int num = turn;
		do
		{
			num = ((!reverse) ? ((num + 1) % players.Length) : ((num - 1) % players.Length));
			if (num < 0)
			{
				num = players.Length + num;
			}
		}
		while (winningPlaces[num] != -1);
		return num;
	}

	public int GetPreviousPlayerTurn()
	{
		return prevPlayerTurn;
	}

	public void ForceSkipNextTurn()
	{
		Debug.Log("Force skip next turn");
		skipNextTurn = true;
	}

	public void ResetDrawCardCount()
	{
		Debug.Log("Reset draw count " + currentDrawCount);
		currentDrawCount = 0;
	}

	public void SetPlayerWinState(int player, bool won)
	{
		if (won)
		{
			winningPlaces[player] = currentWinPlace;
			currentWinPlace++;
		}
		else
		{
			winningPlaces[player] = -2;
		}
		while (players[player].Count > 0)
		{
			discardPile.Add(players[player][0]);
			players[player].RemoveAt(0);
		}
	}

	public void ForcePlayerWinState(int player, int state)
	{
		winningPlaces[player] = state;
	}

	public void SetCurrentWinPlace(int currentWinPlace)
	{
		this.currentWinPlace = currentWinPlace;
	}

	public void SetPlayerDisconnect(int player)
	{
		winningPlaces[player] = -3;
		while (players[player].Count > 0)
		{
			discardPile.Add(players[player][0]);
			players[player].RemoveAt(0);
		}
	}

	public void ScorePlayer(int player)
	{
		for (int i = 0; i < players.Length; i++)
		{
			if (i == player)
			{
				continue;
			}
			foreach (UnoCard item in players[i])
			{
				points[player] += item.GetPoints();
			}
		}
	}

	public void SetLastCardColor(UnoCard.CardColor color)
	{
		discardPile[discardPile.Count - 1].SetWildColor(color);
	}

	public void UpdatePlayerHands(List<UnoCard>[] players)
	{
		this.players = players;
	}

	public void UpdateDrawPile(List<UnoCard> drawPile)
	{
		this.drawPile = drawPile;
	}

	public void UpdateDiscardPile(List<UnoCard> discardPile)
	{
		this.discardPile = discardPile;
	}

	public void SetNextPlayer(int i)
	{
		turn = i;
	}

	public int GetCurrentPlayerTurn()
	{
		return turn;
	}

	public List<UnoCard> GetPlayerHand(int i)
	{
		return players[i];
	}

	public int GetDrawPileSize()
	{
		return drawPile.Count;
	}

	public int GetDiscardPileSize()
	{
		return discardPile.Count;
	}

	public UnoCard GetSecondToLastPlayedCard()
	{
		return discardPile[discardPile.Count - 2];
	}

	public UnoCard GetTopDrawCard()
	{
		return drawPile[0];
	}

	public List<UnoCard> GetDrawPile()
	{
		return drawPile;
	}

	public bool IsReversed()
	{
		return reverse;
	}

	public bool CanChallengeFourCard()
	{
		return challengeImminent;
	}

	public bool CanStackDrawCards()
	{
		return stackDrawCards;
	}

	public int GetCurrentActivePlayerCount()
	{
		int num = 0;
		int[] array = winningPlaces;
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i] == -1)
			{
				num++;
			}
		}
		return num;
	}

	public int GetLastActivePlayer()
	{
		if (GetCurrentActivePlayerCount() == 1)
		{
			for (int i = 0; i < winningPlaces.Length; i++)
			{
				if (winningPlaces[i] == -1)
				{
					return i;
				}
			}
		}
		return -1;
	}

	public int GetPlayerWinState(int player)
	{
		return winningPlaces[player];
	}

	public int[] GetAllWinningPlaces()
	{
		return winningPlaces;
	}

	public int GetPlayerCount()
	{
		return players.Length;
	}

	public void SetCurrentDrawCardAmount(int currentDrawCount)
	{
		this.currentDrawCount = currentDrawCount;
	}

	public int GetCurrentDrawCardAmount()
	{
		return currentDrawCount;
	}

	public int GetPlayerScore(int player)
	{
		return points[player];
	}

	public UnoCard GetTopCard()
	{
		if (discardPile.Count <= 0)
		{
			return null;
		}
		return discardPile[discardPile.Count - 1];
	}

	public UnoGame Clone()
	{
		return new UnoGame(this);
	}
}
