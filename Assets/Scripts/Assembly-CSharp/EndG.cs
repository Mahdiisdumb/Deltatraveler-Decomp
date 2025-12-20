using UnityEngine;

public class EndG : MonoBehaviour
{
	private int state;

	private int frames;

	private TextUT text;

	private string[] dialog;

	private int currentString;

	private int lastString;

	private bool firstString = true;

	private Transform flowey;

	private void Awake()
	{
		text = GameObject.Find("GText").GetComponent<TextUT>();
		flowey = GameObject.Find("Flowery").transform;
	}

	private void Start()
	{
		Util.GameManager().PlayMusic("music/mus_creepy_ambience", 0.4f);
	}

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			float num = Mathf.Lerp(0f, 0.2f, (float)(frames - 30) / 60f);
			SpriteRenderer[] componentsInChildren = flowey.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = new Color(num, num, num, 1f);
			}
			if (frames == 150)
			{
				dialog = new string[3] { "^N...", "Keep on the path.", "You must..." };
				lastString = dialog.Length - 1;
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if (lastString < 0)
			{
				return;
			}
			if (firstString)
			{
				text.StartText(dialog[currentString], new Vector3(65f, 54f), "snd_txtgnr", 1, "speechbubble");
				text.GetText().rectTransform.sizeDelta = new Vector2(528f, 150f);
				text.GetText().lineSpacing = 1.3f;
				text.GetText().color = new Color(0.5f, 0.25f, 0.25f);
				currentString++;
				firstString = false;
			}
			if (text.IsPlaying())
			{
				if (UTInput.GetButton("X") || UTInput.GetButton("C"))
				{
					text.SkipText();
				}
			}
			else
			{
				if (!UTInput.GetButtonDown("Z") && !UTInput.GetButton("C"))
				{
					return;
				}
				text.DestroyOldText();
				if (currentString <= lastString)
				{
					text.StartText(dialog[currentString], new Vector3(65f, 54f), "snd_txtgnr", 1, "speechbubble");
					text.GetText().rectTransform.sizeDelta = new Vector2(528f, 150f);
					text.GetText().lineSpacing = 1.3f;
					text.GetText().color = new Color(0.5f, 0.25f, 0.25f);
					currentString++;
					if (UTInput.GetButton("X") || UTInput.GetButton("C"))
					{
						text.SkipText();
					}
				}
				else
				{
					state = 2;
				}
			}
		}
		else if (state == 2)
		{
			frames++;
			float num2 = Mathf.Lerp(0.2f, 0f, (float)frames / 60f);
			SpriteRenderer[] componentsInChildren = flowey.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = new Color(num2, num2, num2, 1f);
			}
			if (frames == 30)
			{
				Util.GameManager().StopMusic(60f);
			}
			if (frames == 120)
			{
				Util.GameManager().ForceLoadArea(6);
			}
		}
	}
}
