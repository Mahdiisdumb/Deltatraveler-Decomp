using System.Collections.Generic;
using UnityEngine;

public class CardHand : MonoBehaviour
{
	[SerializeField]
	protected float maxCardOverlap = 8f;

	[SerializeField]
	protected float cardSpread = 5f;

	protected List<Card> cards;

	protected bool show;

	protected bool onBoard;

	protected bool isPlaying;

	protected int highlight;

	protected float xOffset;

	protected virtual void Awake()
	{
		cards = new List<Card>();
		show = true;
		onBoard = false;
		highlight = -1;
		xOffset = 0f;
	}

	protected virtual void Update()
	{
		if (onBoard)
		{
			AlignCardsOnBoard();
		}
		else
		{
			AlignCardsInHand(0.25f, show);
		}
		for (int i = 0; i < cards.Count; i++)
		{
			int num = 0;
			if (highlight == i)
			{
				num = 500;
			}
			else if (highlight >= 0)
			{
				num = 300;
			}
			cards[i].SetSortingOrder(20 + i * 2 + num);
			cards[i].gameObject.name = "Card" + i;
		}
	}

	protected void AlignCardsInHand(float lerp, bool doShow)
	{
		for (int i = 0; i < cards.Count; i++)
		{
			float num = (doShow ? (-0.72f) : (-1.62f));
			cards[i].transform.position = Vector3.Lerp(cards[i].transform.position, new Vector3(CalculateXValue(i, center: false), num + base.transform.position.y), 0.25f);
		}
	}

	protected virtual void AlignCardsOnBoard()
	{
		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].transform.position = new Vector3(CalculateXValue(i, center: true), (highlight == i) ? (-1.7f) : (-2.2f));
		}
	}

	protected float CalculateXValue(int i, bool center)
	{
		float num = (center ? 0f : xOffset);
		float num2 = cards.Count - 1;
		if (num2 <= 0f)
		{
			num2 = 1f;
		}
		float num3 = (onBoard ? (cardSpread + cardSpread / 4f) : cardSpread);
		if ((float)cards.Count > maxCardOverlap)
		{
			return num + Mathf.Lerp(0f - num3, num3, (float)i / num2);
		}
		return num + Mathf.Lerp((0f - num3) / maxCardOverlap * (float)(cards.Count - 1), num3 / maxCardOverlap * (float)(cards.Count - 1), (float)i / num2);
	}

	public void SetXOffset(float xOff)
	{
		xOffset = xOff;
	}

	public void AddCard(Card card)
	{
		cards.Add(card);
		card.transform.SetParent(base.transform);
	}

	public void AddCard(UnoCard.CardType type, UnoCard.CardColor color)
	{
		Card component = Object.Instantiate(Resources.Load<GameObject>("uno/Card")).GetComponent<Card>();
		component.transform.position = GameObject.Find("DrawDeck").transform.position;
		component.ChangeTargetCard(new UnoCard(type, color));
		AddCard(component);
	}

	public void ForceCardsToBoard()
	{
		onBoard = true;
		show = false;
		AlignCardsOnBoard();
	}

	public void RemoveCardsFromBoard()
	{
		onBoard = false;
		show = true;
		AlignCardsInHand(1f, doShow: true);
	}

	public bool CardsAreOnBoard()
	{
		return onBoard;
	}

	public void ShowCards()
	{
		show = true;
	}

	public void HideCards()
	{
		show = false;
	}

	public void RemoveAllCards()
	{
		while (cards.Count > 0)
		{
			cards.RemoveAt(0);
		}
	}

	public void HighlightCard(int highlight)
	{
		this.highlight = highlight;
	}

	public Card GetHighlightedCard()
	{
		return cards[highlight];
	}

	public int GetNumCards()
	{
		return cards.Count;
	}

	public void RemoveCard(Card card)
	{
		cards.Remove(card);
	}

	public void SetSpread(float spread)
	{
		cardSpread = spread;
	}

	public void SetMaxOverlap(float overlap)
	{
		maxCardOverlap = overlap;
	}

	public List<Card> GetCards()
	{
		return cards;
	}
}
