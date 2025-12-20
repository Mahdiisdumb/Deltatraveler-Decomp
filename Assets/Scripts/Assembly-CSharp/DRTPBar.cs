using UnityEngine;
using UnityEngine.UI;

public class DRTPBar : TPBar
{
	private readonly float MAX_HEIGHT = 188f;

	private readonly Color YELLOW_COLOR = new Color32(byte.MaxValue, 208, 32, byte.MaxValue);

	private float sine;

	private int displayTP;

	private float sizedTP;

	private Image fg;

	private Image bg;

	private Image use;

	private Image red;

	private Image top;

	private Transform number;

	private Text numberText;

	private Transform max;

	private bool topRising;

	private bool redFalling;

	protected override void Awake()
	{
		fg = base.transform.Find("Bar").Find("FG").GetComponent<Image>();
		bg = base.transform.Find("Bar").Find("BG").GetComponent<Image>();
		use = base.transform.Find("Bar").Find("Use").GetComponent<Image>();
		red = fg.transform.Find("Red").GetComponent<Image>();
		top = base.transform.Find("Bar").Find("Top").GetComponent<Image>();
		number = base.transform.Find("Numbers");
		numberText = number.Find("Number").GetComponent<Text>();
		max = base.transform.Find("MAX");
	}

	protected override void Update()
	{
		if (displayTP < GetCalculatedTP())
		{
			if (!topRising)
			{
				topRising = true;
				top.rectTransform.pivot = new Vector2(0.5f, 1f);
			}
			if (redFalling)
			{
				redFalling = false;
				red.rectTransform.sizeDelta = Vector2.zero;
			}
			displayTP += 8;
			if (displayTP > GetCalculatedTP())
			{
				displayTP = GetCalculatedTP();
			}
			numberText.text = displayTP.ToString();
			if (displayTP == 100 && fg.color != YELLOW_COLOR)
			{
				fg.color = YELLOW_COLOR;
				max.gameObject.SetActive(value: true);
				number.gameObject.SetActive(value: false);
			}
			float y = top.transform.localPosition.y;
			SetFGSize(Mathf.RoundToInt(MAX_HEIGHT * ((float)displayTP / 100f)));
			SetTopPosition();
			top.rectTransform.sizeDelta += new Vector2(0f, top.transform.localPosition.y - y);
			bg.rectTransform.sizeDelta = new Vector2(26f, MAX_HEIGHT - fg.rectTransform.sizeDelta.y);
		}
		else if (displayTP > GetCalculatedTP())
		{
			if (topRising)
			{
				top.rectTransform.pivot = new Vector2(0.5f, 0.5f);
				top.rectTransform.sizeDelta = new Vector2(26f, 2f);
				SetTopPosition();
				topRising = false;
			}
			redFalling = true;
			displayTP -= 8;
			if (displayTP < GetCalculatedTP())
			{
				displayTP = GetCalculatedTP();
			}
			numberText.text = displayTP.ToString();
			if (fg.color == YELLOW_COLOR)
			{
				fg.color = new Color32(byte.MaxValue, 160, 64, byte.MaxValue);
				max.gameObject.SetActive(value: false);
				number.gameObject.SetActive(value: true);
			}
			float num = Mathf.RoundToInt(MAX_HEIGHT * ((float)displayTP / 100f));
			red.rectTransform.sizeDelta = new Vector2(26f, GetFGSize() - num);
		}
		if (topRising)
		{
			sizedTP = (GetFGSize() - top.rectTransform.sizeDelta.y) / MAX_HEIGHT * 100f;
			float num2 = Mathf.Abs((float)displayTP - sizedTP);
			float num3 = 2f;
			if (num2 > 4f)
			{
				num3 += 2f;
			}
			if (num2 > 10f)
			{
				num3 += 3f;
			}
			if (num2 > 20f)
			{
				num3 += 4f;
			}
			if (num2 > 40f)
			{
				num3 += 5f;
			}
			num3 *= 0.7f;
			if (top.rectTransform.sizeDelta.y - num3 <= 2f || num2 < 1.2f)
			{
				top.rectTransform.pivot = new Vector2(0.5f, 0.5f);
				top.rectTransform.sizeDelta = new Vector2(26f, 2f);
				SetTopPosition();
				topRising = false;
			}
			else
			{
				top.rectTransform.sizeDelta -= new Vector2(0f, num3);
			}
		}
		if (redFalling)
		{
			sizedTP = GetFGSize() / MAX_HEIGHT * 100f;
			float num4 = Mathf.Abs((float)displayTP - sizedTP);
			float num5 = 2f;
			if (num4 > 4f)
			{
				num5 += 2f;
			}
			if (num4 > 10f)
			{
				num5 += 3f;
			}
			if (num4 > 20f)
			{
				num5 += 4f;
			}
			if (num4 > 40f)
			{
				num5 += 5f;
			}
			num5 *= 0.7f;
			if (red.rectTransform.sizeDelta.y - num5 <= 0f || num4 < 1.2f)
			{
				redFalling = false;
				red.rectTransform.sizeDelta = Vector2.zero;
				SetFGSize(Mathf.RoundToInt(MAX_HEIGHT * ((float)displayTP / 100f)));
			}
			else
			{
				SetFGSize(GetFGSize() - num5);
				red.rectTransform.sizeDelta -= new Vector2(0f, num5);
			}
			SetTopPosition();
			bg.rectTransform.sizeDelta = new Vector2(26f, MAX_HEIGHT - fg.rectTransform.sizeDelta.y);
		}
	}

	private void LateUpdate()
	{
		if (use.enabled)
		{
			sine += 1f;
			Color color = new Color(0.25f, 0.25f, 0.25f, 0.7f);
			if (GetCalculatedTP() >= tpPreview)
			{
				color = Color.Lerp(Color.white, fg.color, Mathf.Abs(Mathf.Sin(sine / 8f) * 0.5f) + 0.2f);
				color.a = 0.7f;
			}
			use.color = color;
			SetFGSize(Mathf.RoundToInt(MAX_HEIGHT * ((float)displayTP / 100f)));
		}
	}

	public override void UpdateTPPreviewBar(int tpPreview)
	{
		base.tpPreview = tpPreview;
		if (tpPreview > 0)
		{
			use.enabled = true;
			return;
		}
		use.enabled = false;
		SetFGSize(Mathf.RoundToInt(MAX_HEIGHT * ((float)displayTP / 100f)));
	}

	private void SetFGSize(float height)
	{
		if (use.enabled)
		{
			use.rectTransform.sizeDelta = new Vector2(26f, MAX_HEIGHT * ((float)tpPreview / 100f));
			fg.rectTransform.sizeDelta = new Vector2(26f, height - use.rectTransform.sizeDelta.y);
		}
		else
		{
			fg.rectTransform.sizeDelta = new Vector2(26f, height);
		}
	}

	private float GetFGSize()
	{
		if (use.enabled)
		{
			return fg.rectTransform.sizeDelta.y + use.rectTransform.sizeDelta.y;
		}
		return fg.rectTransform.sizeDelta.y;
	}

	private void SetTopPosition()
	{
		top.transform.localPosition = new Vector2(0f, fg.rectTransform.sizeDelta.y + use.rectTransform.sizeDelta.y - MAX_HEIGHT / 2f - 1f);
		use.rectTransform.localPosition = top.transform.localPosition;
		if (use.enabled)
		{
			red.rectTransform.anchoredPosition = new Vector2(0f, red.rectTransform.sizeDelta.y);
		}
		else
		{
			red.rectTransform.anchoredPosition = Vector2.zero;
		}
	}
}
