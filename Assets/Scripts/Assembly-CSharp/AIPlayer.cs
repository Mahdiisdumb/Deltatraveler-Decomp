using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : UnoPlayer
{
	public static readonly bool FORCE_CHALLENGE;

	[SerializeField]
	private int aiDifficulty;

	private int chosenColor;

	public bool unoCallDelay;

	public int unoCallFrames;

	public int unoCallMaxWait = 30;

	private List<KeyValuePair<int, UnoCard>> cardMemory = new List<KeyValuePair<int, UnoCard>>();

	private int rPlayerId = -1;

	private List<UnoCard> cards = new List<UnoCard>();

	private bool stackingOverride;

	protected override void Awake()
	{
		base.Awake();
		SetReady(val: true);
	}

	protected override void Update()
	{
		if (unoCallDelay)
		{
			unoCallFrames++;
			if (unoCallFrames == unoCallMaxWait)
			{
				UnoGameManager.instance.SayUno(playerID);
				unoCallDelay = false;
				unoCallFrames = 0;
			}
		}
		base.Update();
	}

	public void ForceSayUno()
	{
		if (unoCallDelay)
		{
			UnoGameManager.instance.SayUno(playerID);
			unoCallDelay = false;
			unoCallFrames = 0;
		}
	}

	public override void SetAIDifficulty(int newDifficulty)
	{
		aiDifficulty = newDifficulty;
	}

	public int GetAIDifficulty()
	{
		return aiDifficulty;
	}

	public (AIAction, int) AITurn(bool stackingOverride = false)
	{
		UnoGame unoGame = UnoGameManager.instance.GetUnoGame();
		int num = -1;
		this.stackingOverride = stackingOverride;
		cards = unoGame.GetPlayerHand(unoPlayerID);
		chosenColor = -1;
		if (unoGame.GetTopCard() != null && unoGame.GetTopCard().GetCardType() == UnoCard.CardType.PlusFour && unoGame.GetCurrentDrawCardAmount() >= 4 && unoGame.GetDiscardPileSize() > 1)
		{
			bool flag = false;
			foreach (KeyValuePair<int, UnoCard> item in cardMemory)
			{
				if (item.Key == unoGame.GetPreviousPlayerTurn() && (item.Value.GetCardColor() == UnoCard.WHITE || item.Value.GetCardColor() == unoGame.GetTopCard().GetCardColor()))
				{
					flag = true;
					break;
				}
			}
			if (flag)
			{
				return (AIAction.ChallengePlusFour, unoGame.GetPreviousPlayerTurn());
			}
			int count = cards.Count;
			int count2 = unoGame.GetPlayerHand(unoGame.GetPreviousPlayerTurn()).Count;
			float num2 = 100f;
			if (aiDifficulty == 1)
			{
				num2 = count2;
				if (num2 < 2f)
				{
					num2 = 2f;
				}
			}
			else if (aiDifficulty == 2)
			{
				float num3 = (float)count / 6f;
				num2 = 20 - count2;
				if (num2 < 1f)
				{
					num2 = 2f;
				}
				num2 = num2 * num3 + 1.5f * num3;
			}
			else if (aiDifficulty == 3)
			{
				num2 = ((count2 > 4) ? Mathf.Lerp(20f, 5f, (float)(count2 - 4) / 8f) : Mathf.Lerp(1f, 10f, (float)count2 / 4f));
				float num4 = Mathf.Lerp(4f, 1f, (float)count / 5f);
				num2 *= num4;
				if (unoGame.GetTopCard().GetCardColor() == unoGame.GetSecondToLastPlayedCard().GetCardColor())
				{
					num2 = (num2 + 1f) * 2f;
				}
			}
			else
			{
				foreach (UnoCard item2 in UnoGameManager.instance.GetUnoGame().GetPlayerHand(UnoGameManager.instance.GetUnoGame().GetPreviousPlayerTurn()))
				{
					if (CardIsPlayableOnBeforeCard(item2))
					{
						return (AIAction.ChallengePlusFour, unoGame.GetPreviousPlayerTurn());
					}
				}
			}
			if (Mathf.RoundToInt(Random.Range(0f, num2)) == 1 || FORCE_CHALLENGE)
			{
				return (AIAction.ChallengePlusFour, unoGame.GetPreviousPlayerTurn());
			}
		}
		List<int> playableCards = GetPlayableCards();
		if (playableCards.Count > 0)
		{
			bool flag2 = true;
			if (aiDifficulty > 3)
			{
				UnoPlayer[] players = UnoGameManager.instance.GetPlayers();
				int num5 = -1;
				int nextPlayerTurn = UnoGameManager.instance.GetUnoGame().GetNextPlayerTurn(set: false);
				int count3 = UnoGameManager.instance.GetUnoGame().GetPlayerHand(nextPlayerTurn).Count;
				int previousPlayerTurn = UnoGameManager.instance.GetUnoGame().GetPreviousPlayerTurn();
				int count4 = UnoGameManager.instance.GetUnoGame().GetPlayerHand(previousPlayerTurn).Count;
				List<int> cardsOfType = GetCardsOfType(13);
				cardsOfType.AddRange(GetCardsOfType(14));
				for (int i = 0; i < players.Length; i++)
				{
					if (players[i].GetComponent<AIPlayer>() == null)
					{
						num5 = i;
					}
				}
				if (flag2 && unoGame.GetTopCard() != null && unoGame.GetCurrentDrawCardAmount() >= 2 && ((unoGame.GetTopCard().GetCardType() == UnoCard.CardType.PlusFour && playableCards.Count == 1) || unoGame.GetTopCard().GetCardType() == UnoCard.CardType.PlusTwo))
				{
					foreach (int item3 in cardsOfType)
					{
						if (CardIsPlayable(item3))
						{
							num = item3;
							flag2 = false;
							break;
						}
					}
				}
				if (flag2 && count3 < 5 && (count4 >= count3 || num5 == count3))
				{
					foreach (int item4 in GetCardsOfType(new int[3] { 11, 12, 13 }))
					{
						if (CardIsPlayable(item4))
						{
							num = item4;
							flag2 = false;
							break;
						}
					}
				}
				if (flag2 && count4 < 5 && (count4 < count3 || num5 == count4))
				{
					foreach (int item5 in GetCardsOfType(11))
					{
						if (CardIsPlayable(item5))
						{
							num = item5;
							flag2 = false;
							break;
						}
					}
				}
			}
			if (flag2)
			{
				bool flag3 = false;
				if (RollChance(30, 75, 100))
				{
					List<int> cardsOfColor = GetCardsOfColor(GetColorNeededMost());
					List<int> list = new List<int>();
					List<int> list2 = new List<int>();
					foreach (int item6 in cardsOfColor)
					{
						if (!CardIsPlayable(item6))
						{
							continue;
						}
						if (aiDifficulty == 4)
						{
							if (cards[item6].GetCardType() == UnoCard.CardType.Reverse || cards[item6].GetCardType() == UnoCard.CardType.Skip || cards[item6].GetCardType() == UnoCard.CardType.PlusTwo)
							{
								list.Add(item6);
								continue;
							}
							if (cards[item6].GetCardType() == UnoCard.CardType.PlusFour || cards[item6].GetCardType() == UnoCard.CardType.Wild)
							{
								list2.Add(item6);
								continue;
							}
							num = item6;
							break;
						}
						num = item6;
						break;
					}
					if (aiDifficulty == 4 && num == -1)
					{
						List<int> list3 = new List<int>();
						int num6 = -1;
						Debug.Log("no cards of most color?");
						if (cards.Count == list2.Count)
						{
							num = 0;
						}
						else
						{
							foreach (int item7 in playableCards)
							{
								if (!CardIsPlayable(item7))
								{
									continue;
								}
								if (cards[item7].GetCardType() == UnoCard.CardType.PlusFour)
								{
									num6 = item7;
									continue;
								}
								UnoCard.CardType cardType = cards[item7].GetCardType();
								if (cardType == UnoCard.CardType.PlusTwo || cardType == UnoCard.CardType.Skip || cardType == UnoCard.CardType.Skip || cardType == UnoCard.CardType.Reverse)
								{
									list3.Add(item7);
									continue;
								}
								num = item7;
								break;
							}
						}
						if (num6 != -1)
						{
							num = num6;
						}
						else if (list3.Count > 0)
						{
							Debug.Log("Superuse for NOTHING!!!!");
							num = list3[0];
						}
						else if (num == -1 && list2.Count > 0)
						{
							Debug.Log("is this cheating?");
							flag3 = true;
						}
					}
				}
				if (num == -1 && !flag3)
				{
					num = playableCards[Random.Range(0, playableCards.Count)];
				}
			}
		}
		if (((num >= 0) ? cards[num] : null) != null && unoGame.GetCurrentDrawCardAmount() <= 1)
		{
			return (AIAction.Play, num);
		}
		if (CardIsPlayable(unoGame.GetTopDrawCard()) && (Random.Range(0, 4) > 0 || aiDifficulty == 4) && unoGame.GetCurrentDrawCardAmount() <= 1)
		{
			return (AIAction.DrawAndPlay, -1);
		}
		return (AIAction.Draw, -1);
	}

	public void AddCardsToRemember(int playerId, List<UnoCard> cards)
	{
		rPlayerId = playerId;
		if (aiDifficulty == 1)
		{
			cardMemory.Clear();
		}
		else
		{
			cardMemory.RemoveAll(CardBelongsToPreviousTurnPlayer);
		}
		foreach (UnoCard card in cards)
		{
			cardMemory.Add(new KeyValuePair<int, UnoCard>(playerId, card));
		}
	}

	public void RemoveCardFromMemory(int playerId, UnoCard card)
	{
		KeyValuePair<int, UnoCard> item = new KeyValuePair<int, UnoCard>(playerId, card);
		if (cardMemory.Contains(item))
		{
			cardMemory.Remove(item);
		}
	}

	public bool CardBelongsToPreviousTurnPlayer(KeyValuePair<int, UnoCard> card)
	{
		return rPlayerId == card.Key;
	}

	public void ChooseWildColor(ref UnoCard playedCard)
	{
		if (!playedCard.IsWildCard())
		{
			return;
		}
		int num = chosenColor;
		if (num < 0)
		{
			num = Random.Range(0, 4);
			if (RollChance(10, 50, 80))
			{
				num = GetColorNeededMost();
			}
		}
		playedCard.SetWildColor(UnoCard.COLORS[num]);
		chosenColor = -1;
	}

	public void BeginUnoCountdown(bool forceUnfocus = false)
	{
		unoCallDelay = true;
		int num = 0;
		switch (aiDifficulty)
		{
		case 1:
			if (!forceUnfocus)
			{
				num = Random.Range(0, 3);
			}
			unoCallMaxWait = ((num == 0) ? 40 : 12);
			break;
		case 2:
			if (!forceUnfocus)
			{
				num = Random.Range(0, 8);
			}
			unoCallMaxWait = ((num == 0) ? 30 : 10);
			break;
		case 3:
			if (!forceUnfocus)
			{
				num = Random.Range(0, 20);
			}
			unoCallMaxWait = ((num == 0) ? 20 : Random.Range(5, 9));
			break;
		case 4:
			if (!forceUnfocus)
			{
				num = Random.Range(0, 2);
			}
			unoCallMaxWait = 2;
			if (UnoGameManager.instance.GetUnoGame().GetCurrentPlayerTurn() == playerID)
			{
				if (num != 0)
				{
					UnoGameManager.instance.SayUno(playerID);
					unoCallDelay = false;
				}
			}
			else
			{
				unoCallMaxWait = ((num == 0) ? 15 : Random.Range(3, 7));
			}
			break;
		}
		Debug.Log(num == 0);
	}

	public void EndUnoCountdown()
	{
		unoCallDelay = false;
		unoCallFrames = 0;
	}

	private bool RollChance(int susie, int noelle, int papyrus, int max = 100)
	{
		int num = 0;
		switch (aiDifficulty)
		{
		case 1:
			num = susie;
			break;
		case 2:
			num = noelle;
			break;
		case 3:
			num = papyrus;
			break;
		case 4:
			num = max;
			break;
		}
		return Random.Range(0, max) < num;
	}

	private int GetColorNeededMost()
	{
		int[] array = new int[4];
		for (int i = 0; i < cards.Count; i++)
		{
			UnoCard unoCard = cards[i];
			int num = -1;
			if (unoCard.GetCardColor() == UnoCard.RED)
			{
				num = 0;
			}
			if (unoCard.GetCardColor() == UnoCard.YELLOW)
			{
				num = 1;
			}
			if (unoCard.GetCardColor() == UnoCard.GREEN)
			{
				num = 2;
			}
			if (unoCard.GetCardColor() == UnoCard.BLUE)
			{
				num = 3;
			}
			if (num > -1)
			{
				array[num]++;
			}
		}
		int num2 = 0;
		int num3 = -1;
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j] > num2)
			{
				num2 = array[j];
				num3 = j;
			}
		}
		if (num3 == -1)
		{
			num3 = Random.Range(0, 4);
		}
		return num3;
	}

	private List<int> GetCardsOfColor(int color)
	{
		UnoCard.CardColor cardColor = UnoCard.WHITE;
		switch (color)
		{
		case 0:
			cardColor = UnoCard.RED;
			break;
		case 1:
			cardColor = UnoCard.YELLOW;
			break;
		case 2:
			cardColor = UnoCard.GREEN;
			break;
		case 3:
			cardColor = UnoCard.BLUE;
			break;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Count; i++)
		{
			if (cards[i].GetCardColor() == cardColor)
			{
				list.Add(i);
			}
		}
		return list;
	}

	private List<int> GetCardsOfColor(UnoCard.CardColor color)
	{
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Count; i++)
		{
			if (cards[i].GetCardColor() == color)
			{
				list.Add(i);
			}
		}
		return list;
	}

	private List<int> GetCardsOfType(int type)
	{
		UnoCard.CardType cardType = UnoCard.CardType.Zero;
		switch (type)
		{
		case 0:
			cardType = UnoCard.CardType.Zero;
			break;
		case 1:
			cardType = UnoCard.CardType.One;
			break;
		case 2:
			cardType = UnoCard.CardType.Two;
			break;
		case 3:
			cardType = UnoCard.CardType.Three;
			break;
		case 4:
			cardType = UnoCard.CardType.Four;
			break;
		case 5:
			cardType = UnoCard.CardType.Five;
			break;
		case 6:
			cardType = UnoCard.CardType.Six;
			break;
		case 7:
			cardType = UnoCard.CardType.Seven;
			break;
		case 8:
			cardType = UnoCard.CardType.Eight;
			break;
		case 9:
			cardType = UnoCard.CardType.Nine;
			break;
		case 10:
			cardType = UnoCard.CardType.Wild;
			break;
		case 11:
			cardType = UnoCard.CardType.Reverse;
			break;
		case 12:
			cardType = UnoCard.CardType.Skip;
			break;
		case 13:
			cardType = UnoCard.CardType.PlusTwo;
			break;
		case 14:
			cardType = UnoCard.CardType.PlusFour;
			break;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Count; i++)
		{
			if (cards[i].GetCardType() == cardType)
			{
				list.Add(i);
			}
		}
		return list;
	}

	private List<int> GetCardsOfType(int[] types)
	{
		List<int> list = new List<int>();
		list = new List<int>();
		for (int i = 0; i < types.Length; i++)
		{
			list.AddRange(GetCardsOfType(types[i]));
		}
		return list;
	}

	private List<int> GetPlayableCards()
	{
		UnoGameManager instance = UnoGameManager.instance;
		List<int> list = new List<int>();
		for (int i = 0; i < cards.Count; i++)
		{
			if (cards[i].CanBePlacedOn(instance.GetUnoGame().GetTopCard(), instance, stackingOverride))
			{
				list.Add(i);
			}
		}
		return list;
	}

	private bool CardIsPlayable(UnoCard card)
	{
		UnoGameManager instance = UnoGameManager.instance;
		return card?.CanBePlacedOn(instance.GetUnoGame().GetTopCard(), instance, stackingOverride) ?? false;
	}

	private bool CardIsPlayableOnBeforeCard(UnoCard card)
	{
		UnoGameManager instance = UnoGameManager.instance;
		return card?.CanBePlacedOn(instance.GetUnoGame().GetSecondToLastPlayedCard(), instance, stackingOverride) ?? false;
	}

	private bool CardIsPlayable(int card)
	{
		UnoGameManager instance = UnoGameManager.instance;
		if (card >= 0 && card < cards.Count)
		{
			return cards[card].CanBePlacedOn(instance.GetUnoGame().GetTopCard(), instance, stackingOverride);
		}
		return false;
	}

	public override bool IsHost()
	{
		return false;
	}
}
