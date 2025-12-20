using UnityEngine;
using UnityEngine.UI;

public class MoleToss : SpecialACT
{
	private int frames;

	private float score;

	private int scoreLimit;

	private Text timerText;

	private Text timerShadow;

	private Image scoreBar;

	private RoughMole mole;

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		score -= 0.375f;
		if (score < 0f)
		{
			score = 0f;
		}
		if (UTInput.GetButtonDown("Z"))
		{
			score += 6.5f;
		}
		scoreBar.rectTransform.sizeDelta = new Vector2(score * 2f, 20f);
		if (Mathf.Abs((float)scoreLimit - score) <= 20f)
		{
			scoreBar.color = new Color(Mathf.Abs((float)scoreLimit - score) / 20f, 1f, 0f);
		}
		else if (Mathf.Abs((float)scoreLimit - score) <= 40f)
		{
			scoreBar.color = new Color(1f, (40f - Mathf.Abs((float)scoreLimit - score)) / 20f, 0f);
		}
		frames++;
		string text = ((float)(150 - frames) / 30f + Random.Range(0f, 0.05f)).ToString("F5");
		timerText.text = text;
		timerShadow.text = text;
		if (frames >= 150)
		{
			float num = Mathf.Abs((float)scoreLimit - score) - 5f;
			if (num < 0f)
			{
				num = 0f;
			}
			int num2 = Mathf.CeilToInt(100f - num * 2f);
			if (num2 < 1)
			{
				num2 = 1;
			}
			mole.AddActPoints(num2);
			mole.PlayTossAnim(num2);
			string text2 = "* Tossed successfully" + ((num2 <= 20) ? "...?\n" : "!\n");
			text2 = ((num2 <= 20) ? (text2 + ((score > (float)scoreLimit) ? "* The throw was way too\n  strong." : "* It wasn't impressed at\n  all.")) : ((num2 <= 50) ? (text2 + ((score > (float)scoreLimit) ? "* The throw was somewhat\n  too strong." : "* The throw was somewhat\n  too weak.")) : ((num2 <= 80) ? (text2 + ((score > (float)scoreLimit) ? "* The throw was a bit\n  too strong." : "* The throw was a bit\n  too weak.")) : ((num2 >= 100) ? (text2 + "* The throw was perfect!") : (text2 + "* The throw was almost\n  perfect!")))));
			Object.FindObjectOfType<BattleManager>().StartText(text2, new Vector2(-4f, -134f), "snd_txtbtl");
			Object.Destroy(base.gameObject);
		}
	}

	public void SetMole(RoughMole mole)
	{
		this.mole = mole;
	}

	public override void Activate()
	{
		scoreLimit = Random.Range(65, 90);
		bool joystickIsActive = UTInput.joystickIsActive;
		string text = (joystickIsActive ? "MASH     " : string.Format("MASH [{0}]", UTInput.GetKeyName("Confirm")));
		Text[] componentsInChildren = GetComponentsInChildren<Text>();
		foreach (Text text2 in componentsInChildren)
		{
			if (text2.gameObject.name == "Timer")
			{
				timerText = text2;
			}
			else if (text2.gameObject.name == "Timer-Shadow")
			{
				timerShadow = text2;
			}
			else if (text2.gameObject.name.StartsWith("Text"))
			{
				text2.text = text;
			}
			text2.enabled = true;
		}
		Image[] componentsInChildren2 = GetComponentsInChildren<Image>();
		foreach (Image image in componentsInChildren2)
		{
			if (image.gameObject.name == "FG")
			{
				scoreBar = image;
			}
			else if (image.gameObject.name == "Tick")
			{
				image.transform.localPosition = new Vector3(scoreLimit * 2 - 100, 0f);
			}
			image.enabled = true;
			if (!(image.gameObject.name == "Confirm"))
			{
				continue;
			}
			if (!joystickIsActive)
			{
				image.enabled = false;
				continue;
			}
			for (int j = 0; j < ButtonPrompts.validButtons.Length; j++)
			{
				if (UTInput.GetKeyOrButtonReplacement("Confirm") == ButtonPrompts.GetButtonChar(ButtonPrompts.validButtons[j]))
				{
					image.sprite = Resources.Load<Sprite>("ui/buttons/" + ButtonPrompts.GetButtonGraphic(ButtonPrompts.validButtons[j]));
					break;
				}
			}
		}
		base.Activate();
	}
}
