using UnityEngine;
using UnityEngine.UI;

public class TouchButton : MonoBehaviour
{
	private Button button;

	[SerializeField]
	private string input;

	[SerializeField]
	private bool pos = true;

	[SerializeField]
	private bool isDiag;

	[SerializeField]
	private bool goLeft;

	private void Start()
	{
		button = GetComponent<Button>();
	}

	private void Update()
	{
	}

	public void OnPointerEnter()
	{
		Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_menumove");
		GetComponent<Image>().color = new Color(1f, 1f, 0f);
	}

	public void OnPointerExit()
	{
		GetComponent<Image>().color = new Color(1f, 1f, 1f);
	}

	public void OnPointerDown()
	{
		if (input == "quit")
		{
			Application.Quit();
			return;
		}
		UTInput.SetValue(input, true, pos, isDiag, goLeft);
		if ((bool)Object.FindObjectOfType<TitleScreen>() && Object.FindObjectOfType<TitleScreen>().RebindingKey() && (input == "X" || input == "C"))
		{
			Object.FindObjectOfType<TitleScreen>().CancelRebind();
		}
	}

	public void OnPointerUp()
	{
		if (input != "quit")
		{
			UTInput.SetValue(input, false, pos, isDiag, goLeft);
		}
	}
}
