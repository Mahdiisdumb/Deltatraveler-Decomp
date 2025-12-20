using UnityEngine;
using UnityEngine.UI;

public class TextRemark : MonoBehaviour
{
	private Text text;

	private Portrait portrait;

	private bool playing;

	private int frames;

	private Vector3 posStart;

	private Remark remark;

	private void Awake()
	{
		text = GetComponentInChildren<Text>();
		playing = false;
		frames = 0;
	}

	private void Update()
	{
		if (playing)
		{
			frames++;
			if (frames == remark.GetFrames())
			{
				playing = false;
			}
			float num = (float)frames / (float)remark.GetFrames();
			MonoBehaviour.print(num);
			Color color = new Color(1f, 1f, 1f, num);
			text.color = color;
			if ((bool)portrait)
			{
				portrait.SetColor(color);
			}
			Vector3 b = posStart + new Vector3(remark.GetDir().x, remark.GetDir().y) * remark.GetSpeed() * remark.GetFrames();
			base.transform.localPosition = Vector3.Lerp(posStart, b, num);
		}
	}

	public void StartRemark(Vector3 position, Remark remk)
	{
		remark = remk;
		position += new Vector3(remk.pos.x, 0f - remk.pos.y);
		text.text = Util.Unescape(remk.text);
		if ((bool)portrait)
		{
			Object.Destroy(portrait.gameObject);
		}
		if (remk.portrait != "")
		{
			portrait = Portrait.CreatePortrait(remk.portrait);
			portrait.transform.SetParent(base.transform, worldPositionStays: false);
			portrait.transform.localScale = Vector3.one / 2f;
			portrait.GetComponent<Image>().rectTransform.pivot = new Vector2(0f, 1f);
			portrait.SetImage(remk.portrait);
			portrait.SetColor(new Color(1f, 1f, 1f, 0f));
			if (remk.portrait.Contains("sans"))
			{
				text.font = Resources.Load<Font>("fonts/sans");
				if (text.fontSize > 20)
				{
					text.fontSize = 32;
				}
				else
				{
					text.fontSize = 16;
				}
			}
			if (remk.portrait.Contains("pap"))
			{
				text.font = Resources.Load<Font>("fonts/papyrus");
				if (text.fontSize > 20)
				{
					text.fontSize = 32;
				}
				else
				{
					text.fontSize = 16;
				}
			}
		}
		base.transform.localPosition += position;
		posStart = base.transform.localPosition;
		playing = true;
	}

	public bool CanAdvance()
	{
		return frames >= 4;
	}

	public void Skip()
	{
		playing = false;
		Color color = new Color(1f, 1f, 1f, 1f);
		text.color = color;
		if ((bool)portrait)
		{
			portrait.SetColor(color);
		}
		base.transform.localPosition = posStart + new Vector3(remark.GetDir().x, remark.GetDir().y) * remark.GetSpeed() * remark.GetFrames();
	}
}
