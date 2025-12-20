using UnityEngine;
using UnityEngine.UI;

public class SwirlingText : MonoBehaviour
{
	private int angleBase;

	private bool isPlaying;

	private string focusFont = "DTM-Mono";

	private void Awake()
	{
		isPlaying = false;
	}

	private void LateUpdate()
	{
		if (!isPlaying)
		{
			return;
		}
		Text[] componentsInChildren = GetComponentsInChildren<Text>();
		foreach (Text text in componentsInChildren)
		{
			if (text.font.name == focusFont)
			{
				text.GetComponent<LetterSpacing>().ForceSwirl(true);
			}
		}
	}

	public void StartSwirl(string focusFont)
	{
		isPlaying = true;
		Text[] componentsInChildren = GetComponentsInChildren<Text>();
		foreach (Text text in componentsInChildren)
		{
			if (text.font.name == focusFont)
			{
				text.GetComponent<LetterSpacing>().ForceSwirl(true);
			}
		}
		this.focusFont = focusFont;
	}

	public void Stop()
	{
		Text[] componentsInChildren = GetComponentsInChildren<Text>();
		foreach (Text text in componentsInChildren)
		{
			if (text.font.name == focusFont)
			{
				text.GetComponent<LetterSpacing>().ForceSwirl(false);
			}
		}
		isPlaying = false;
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}
}
