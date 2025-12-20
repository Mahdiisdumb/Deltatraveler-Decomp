using UnityEngine;
using UnityEngine.UI;

public class AutofireUI : MonoBehaviour
{
	private SOUL soul;

	private bool usingController;

	private bool autofireEnabled;

	private bool activated;

	private int flavor;

	private float a;

	private void Awake()
	{
		flavor = (int)Util.GameManager().GetFlag(223);
		autofireEnabled = (int)Util.GameManager().GetFlag(333) == 1;
		AdjustButtonLayout();
		SetEnabledColor();
		SetAlphaColor();
	}

	private void Start()
	{
		soul = Object.FindAnyObjectByType<SOUL>();
	}

	private void Update()
	{
		activated = Object.FindAnyObjectByType<BattleManager>().AttackIsActive() && soul.GetSOULMode() == 5;
		if (activated)
		{
			if (a < 1f)
			{
				a += 0.2f;
			}
			if (UTInput.GetButtonDown("C"))
			{
				autofireEnabled = !autofireEnabled;
				Util.GameManager().SetFlag(333, autofireEnabled ? 1 : 0);
				if (autofireEnabled)
				{
					Object.FindAnyObjectByType<SOUL>().EnableAutofire();
				}
				else
				{
					Object.FindAnyObjectByType<SOUL>().DisableAutofire();
				}
				SetEnabledColor();
			}
		}
		else if (a > 0f)
		{
			a -= 0.2f;
		}
		if (UTInput.joystickIsActive != usingController)
		{
			AdjustButtonLayout();
		}
		SetAlphaColor();
	}

	private void AdjustButtonLayout()
	{
		usingController = UTInput.joystickIsActive;
		if (usingController)
		{
			base.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(130f, 40f);
			base.transform.Find("Key").GetComponent<Text>().enabled = false;
			base.transform.Find("Button").GetComponent<Image>().enabled = true;
			base.transform.Find("Button").GetComponent<Image>().sprite = ButtonPrompts.GetButtonGraphic(UTInput.GetButtonName("Menu"), ButtonPrompts.ButtonType.Small);
		}
		else
		{
			base.transform.Find("Key").GetComponent<Text>().enabled = true;
			base.transform.Find("Button").GetComponent<Image>().enabled = false;
			string text = string.Format("[{0}]", UTInput.GetKeyName("Menu"));
			base.transform.Find("Key").GetComponent<Text>().text = text;
			base.transform.GetComponent<RectTransform>().sizeDelta = new Vector2(106 + text.Length * 10, 40f);
		}
		base.transform.Find("Black").GetComponent<RectTransform>().sizeDelta = base.transform.GetComponent<RectTransform>().sizeDelta - new Vector2(10f, 10f);
	}

	private void SetEnabledColor()
	{
		if (autofireEnabled)
		{
			base.transform.Find("Text").GetComponent<Text>().color = new Color(1f, 1f, 0f);
			base.transform.GetComponent<Image>().color = UIBackground.borderColors[flavor];
		}
		else
		{
			base.transform.Find("Text").GetComponent<Text>().color = new Color(0.5f, 0.5f, 0.5f);
			base.transform.GetComponent<Image>().color = UIBackground.borderColors[flavor] * 0.5f;
		}
	}

	private void SetAlphaColor()
	{
		Graphic[] componentsInChildren = GetComponentsInChildren<Graphic>();
		foreach (Graphic graphic in componentsInChildren)
		{
			graphic.color = new Color(graphic.color.r, graphic.color.g, graphic.color.b, a);
		}
	}
}
