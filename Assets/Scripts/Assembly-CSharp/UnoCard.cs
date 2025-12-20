using System.Collections.Generic;
using UnityEngine;

public class UnoCard
{
	public enum CardType
	{
		Zero = 0,
		One = 1,
		Two = 2,
		Three = 3,
		Four = 4,
		Five = 5,
		Six = 6,
		Seven = 7,
		Eight = 8,
		Nine = 9,
		Wild = 10,
		Reverse = 11,
		Skip = 12,
		PlusTwo = 13,
		PlusFour = 14
	}

	public class CardColor
	{
		public string name { get; private set; }

		public Color color { get; private set; }

		internal CardColor(string name, Color color)
		{
			this.color = color;
			this.name = name;
		}

		public override string ToString()
		{
			return name;
		}
	}

	public static readonly CardColor RED = new CardColor("RED", Color.red);

	public static readonly CardColor YELLOW = new CardColor("YELLOW", new Color32(byte.MaxValue, byte.MaxValue, 0, byte.MaxValue));

	public static readonly CardColor GREEN = new CardColor("GREEN", new Color32(0, 192, 0, byte.MaxValue));

	public static readonly CardColor BLUE = new CardColor("BLUE", new Color32(0, 162, 232, byte.MaxValue));

	public static readonly CardColor WHITE = new CardColor("WHITE", Color.white);

	public static readonly List<CardColor> COLORS = new List<CardColor> { RED, YELLOW, GREEN, BLUE, WHITE };

	public CardType type { get; private set; }

	public CardColor color { get; private set; }

	public UnoCard(CardType type, CardColor color)
	{
		this.type = type;
		if (IsWildCard())
		{
			this.color = WHITE;
		}
		else
		{
			this.color = color;
		}
	}

	public void SetWildColor(CardColor color)
	{
		if (IsWildCard())
		{
			this.color = color;
		}
	}

	public bool CanBePlacedOn(UnoCard card, UnoGameManager manager = null, bool stackingOverride = false)
	{
		if (manager == null)
		{
			manager = UnoGameManager.instance;
		}
		if (card == null)
		{
			return true;
		}
		if (stackingOverride || manager.CurrentlyStackingDrawCards())
		{
			return IsStackable(card, manager, stackingOverride);
		}
		if (IsWildCard())
		{
			return true;
		}
		if (type == card.GetCardType())
		{
			return true;
		}
		if (IsStackable(card, manager))
		{
			return true;
		}
		if (color == card.GetCardColor())
		{
			return true;
		}
		if (card.IsWildCard())
		{
			return card.GetCardColor() == WHITE;
		}
		return false;
	}

	public bool IsStackable(UnoCard card, UnoGameManager manager = null, bool stackingOverride = false)
	{
		if (manager == null)
		{
			manager = UnoGameManager.instance;
		}
		if ((stackingOverride || manager.CanStackDrawCards()) && (type == CardType.PlusTwo || type == CardType.PlusFour))
		{
			if (card.type != CardType.PlusTwo)
			{
				return card.type == CardType.PlusFour;
			}
			return true;
		}
		return false;
	}

	public CardType GetCardType()
	{
		return type;
	}

	public CardColor GetCardColor()
	{
		return color;
	}

	public int GetPoints()
	{
		if (type >= CardType.Zero && type <= CardType.Nine)
		{
			return (int)type;
		}
		if (IsWildCard())
		{
			return 50;
		}
		if (type >= CardType.Reverse && type <= CardType.PlusTwo)
		{
			return 20;
		}
		return 0;
	}

	public bool IsWildCard()
	{
		if (type != CardType.Wild)
		{
			return type == CardType.PlusFour;
		}
		return true;
	}

	public string GetCardName()
	{
		if (IsWildCard())
		{
			if (type == CardType.Wild)
			{
				return "WILD CARD";
			}
			if (type == CardType.PlusFour)
			{
				return "WILD +4 CARD";
			}
		}
		string text = color.ToString() + " ";
		if (type == CardType.Reverse)
		{
			return text + "REVERSE";
		}
		if (type == CardType.Skip)
		{
			return text + "SKIP";
		}
		if (type == CardType.PlusTwo)
		{
			return text + "+2";
		}
		return text + (int)type;
	}

	public string GetCardColorName()
	{
		return color.ToString();
	}

	public override string ToString()
	{
		string text = GetCardName();
		if (IsWildCard())
		{
			text = text + " [" + GetCardColorName() + "]";
		}
		return text;
	}
}
