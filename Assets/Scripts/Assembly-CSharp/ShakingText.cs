using UnityEngine;
using UnityEngine.UI;

public class ShakingText : MonoBehaviour
{
	private int frequency;

	private bool isPlaying;

	private string focusFont = "DTM-Mono";

	private void Awake()
	{
		frequency = 1;
		isPlaying = false;
	}

	private void LateUpdate()
	{
		if (!isPlaying)
		{
			return;
		}
		Text[] array = Util.FindObjectsOfType<Text>();
		foreach (Text text in array)
		{
			if (text.font.name == focusFont)
			{
				text.GetComponent<LetterSpacing>().ForceShake(forceShake: true, frequency);
			}
		}
	}

	public void StartShake(int freq, string focusFont = "DTM-Mono", bool half = false)
	{
		frequency = freq;
		isPlaying = true;
		Text[] array = Util.FindObjectsOfType<Text>();
		foreach (Text text in array)
		{
			if (text.font.name == focusFont)
			{
				text.GetComponent<LetterSpacing>().ForceShake(forceShake: true, frequency);
			}
		}
		this.focusFont = focusFont;
	}

	public void Stop()
	{
		Text[] array = Util.FindObjectsOfType<Text>();
		foreach (Text text in array)
		{
			if (text.font.name == focusFont)
			{
				text.GetComponent<LetterSpacing>().ForceShake(forceShake: false);
			}
		}
		isPlaying = false;
	}

	public bool IsPlaying()
	{
		return isPlaying;
	}
}
