using UnityEngine;

public class FreeCardHand : CardHand
{
	private int frames;

	protected override void Update()
	{
		AlignCardsOnBoard();
		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].SetSortingOrder(520 + i * 2);
			cards[i].gameObject.name = "Card" + i;
		}
		frames++;
		if (frames < 60)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0f, 3.72f), 0.5f);
		}
		else
		{
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(0f, 7f), 0.5f);
		}
		if (base.transform.position.y > 6f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	protected override void AlignCardsOnBoard()
	{
		for (int i = 0; i < cards.Count; i++)
		{
			cards[i].transform.localPosition = new Vector3(CalculateXValue(i, center: true), 0f);
		}
	}
}
