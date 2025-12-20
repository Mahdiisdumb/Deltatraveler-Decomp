using UnityEngine;

public class GasterTextEffect : MonoBehaviour
{
	private int frames;

	private void Update()
	{
		frames = (frames + 1) % 120;
		float t = (float)frames / 45f;
		if (frames > 60)
		{
			t = (float)(105 - frames) / 45f;
		}
		GetComponent<TextOutline>().effectColor = Color.Lerp(new Color(1f, 1f, 1f, 0.2f), new Color(1f, 1f, 1f, 0.4f), t);
	}
}
