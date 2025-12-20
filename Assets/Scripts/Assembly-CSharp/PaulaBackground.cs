using UnityEngine;

public class PaulaBackground : MonoBehaviour
{
	private int frames;

	private int fadeFrames;

	private void Update()
	{
		if (fadeFrames >= 60)
		{
			return;
		}
		if (fadeFrames == 30)
		{
			GetComponentInChildren<BreathingBG>().SetColor(new Color(1f, 0f, 0f, 8f / 85f));
			GetComponentInChildren<BreathingBG>().StartGeneratingBalls();
		}
		fadeFrames++;
		if (GameManager.GetOptions().lowGraphics.value != 1)
		{
			base.transform.Find("BG").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)fadeFrames / 60f);
			return;
		}
		Color color = new Color(1f, 0f, 0f);
		BattleBGPiece[] array = Util.FindObjectsOfType<BattleBGPiece>();
		foreach (BattleBGPiece obj in array)
		{
			_ = obj.GetComponent<SpriteRenderer>().color;
			obj.GetComponent<SpriteRenderer>().color = new Color(color.r, color.g, color.b, (float)fadeFrames / 60f);
		}
	}
}
