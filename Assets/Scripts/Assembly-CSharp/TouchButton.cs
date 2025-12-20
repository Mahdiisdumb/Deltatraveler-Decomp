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
		Util.GameManager().PlayGlobalSFX("sounds/snd_menumove");
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
		UTInput.SetValue(input, value: true, pos, isDiag, goLeft);
		if ((bool)Util.FindObjectOfType<TitleScreen>() && Util.FindObjectOfType<TitleScreen>().RebindingKey() && (input == "X" || input == "C"))
		{
			Util.FindObjectOfType<TitleScreen>().CancelRebind();
		}
	}

	public void OnPointerUp()
	{
		if (input != "quit")
		{
			UTInput.SetValue(input, value: false, pos, isDiag, goLeft);
		}
	}
}
