using System;
using UnityEngine;

public class GreyDoorBGSquare : MonoBehaviour
{
	private int frames;

	private Color initColor = new Color(1f, 1f, 1f, 0.1f);

	private void LateUpdate()
	{
		frames++;
		float num = (float)frames / 60f;
		num = Mathf.Sin(num * (float)Math.PI * 0.5f);
		base.transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(640f, 480f), num);
		GetComponent<SpriteRenderer>().color = Color.Lerp(initColor, new Color(1f, 1f, 1f, 0f), num);
		GetComponent<SpriteRenderer>().sortingOrder = 480 - frames;
		if (frames == 60)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
