using UnityEngine;
using UnityEngine.UI;

public class TouchSensitivityButton : MonoBehaviour
{
	[SerializeField]
	private bool add = true;

	private bool holdingDown;

	private int holdingFrames;

	private void Update()
	{
		if (holdingDown)
		{
			holdingFrames++;
			if (holdingFrames >= 20 && holdingFrames % 3 == 0)
			{
				UpdateSensitivity();
				Util.GameManager().PlayGlobalSFX("sounds/snd_menumove");
			}
		}
	}

	private void UpdateSensitivity()
	{
		Util.FindObjectOfType<TouchPad>().ChangeSensitivity(add ? 1 : (-1));
		Util.FindObjectOfType<TitleScreen>().UpdateSensitivityText();
	}

	public void OnPointerDown()
	{
		Util.GameManager().PlayGlobalSFX("sounds/snd_select");
		GetComponent<Image>().color = new Color(1f, 1f, 0f);
		UpdateSensitivity();
		holdingDown = true;
		holdingFrames = 0;
	}

	public void OnPointerUp()
	{
		GetComponent<Image>().color = new Color(1f, 1f, 1f);
		holdingDown = false;
		holdingFrames = 0;
	}
}
