using UnityEngine;

public class Card : MonoBehaviour
{
	private UnoCard card;

	private SpriteRenderer fg;

	private SpriteRenderer bg;

	private int frames;

	private bool front;

	private bool playingAnim;

	private int colorFrames;

	[SerializeField]
	private bool doFront = true;

	[SerializeField]
	private int changeType;

	[SerializeField]
	private int changeColor;

	private void Awake()
	{
		fg = base.transform.Find("fg").GetComponent<SpriteRenderer>();
		bg = base.transform.Find("bg").GetComponent<SpriteRenderer>();
		frames = 0;
		front = doFront;
		playingAnim = false;
		colorFrames = 0;
		ChangeTargetCard(new UnoCard(UnoCard.CardType.Zero, UnoCard.RED));
	}

	private void Update()
	{
		if (!playingAnim)
		{
			return;
		}
		frames++;
		if (frames >= 10)
		{
			base.transform.localScale = new Vector3(1f, 1f, 1f);
			playingAnim = false;
			frames = 0;
		}
		if (frames > 5)
		{
			base.transform.localScale = Vector3.Lerp(new Vector3(0f, 1f, 1f), new Vector3(1f, 1f, 1f), (float)(frames - 5) / 5f);
			return;
		}
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 1f, 1f), (float)frames / 5f);
		if (frames == 5)
		{
			UpdateVisuals();
		}
	}

	public void ChangeTargetCard(UnoCard card)
	{
		this.card = card;
		UpdateVisuals();
	}

	public void UpdateVisuals()
	{
		if (front)
		{
			fg.enabled = true;
			fg.sprite = Resources.Load<Sprite>("uno/spr_card_fg_" + (int)card.GetCardType());
			fg.color = (card.IsWildCard() ? Color.white : card.GetCardColor().color);
			bg.sprite = Resources.Load<Sprite>("uno/spr_card_bg");
			bg.color = card.GetCardColor().color;
		}
		else
		{
			fg.enabled = false;
			bg.sprite = Resources.Load<Sprite>("uno/spr_card_back");
			bg.color = Color.white;
		}
	}

	public void SetSortingOrder(int i)
	{
		fg.sortingOrder = i + 1;
		bg.sortingOrder = i;
	}

	public void FlipCard()
	{
		FlipCard(!front);
	}

	public void FlipCard(bool forceFront)
	{
		front = forceFront;
		playingAnim = true;
	}

	public void ForceDown()
	{
		front = false;
		UpdateVisuals();
	}

	public UnoCard GetCardData()
	{
		return card;
	}

	public bool IsPlayingAnim()
	{
		return playingAnim;
	}
}
