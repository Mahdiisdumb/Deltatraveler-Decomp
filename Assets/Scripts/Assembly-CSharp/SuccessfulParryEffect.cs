using System;
using UnityEngine;

public class SuccessfulParryEffect : MonoBehaviour
{
	private int frames;

	private void Update()
	{
		frames++;
		float num = (float)frames / 15f;
		float a = Mathf.Lerp(0.75f, 0f, num);
		GetComponent<SpriteRenderer>().color = new Color(GetComponent<SpriteRenderer>().color.r, GetComponent<SpriteRenderer>().color.g, GetComponent<SpriteRenderer>().color.b, a);
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f), new Vector3(4f, 4f), Mathf.Sin(num * (float)Math.PI * 0.5f));
		if (frames >= 25 && (!GetComponent<AudioSource>() || !GetComponent<AudioSource>().isPlaying))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
