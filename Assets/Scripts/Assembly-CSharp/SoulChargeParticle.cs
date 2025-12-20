using System;
using UnityEngine;

public class SoulChargeParticle : MonoBehaviour
{
	private float angle;

	private int frames;

	private SpriteRenderer sr;

	private void Awake()
	{
		angle = UnityEngine.Random.Range(0f, 360f);
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		frames++;
		sr.color = new Color(sr.color.r, sr.color.g, sr.color.b, (float)frames / 5f);
		float num = (float)frames / 20f;
		base.transform.localPosition = Vector3.Lerp(new Vector3(Mathf.Cos(angle * ((float)Math.PI / 180f)), Mathf.Sin(angle * ((float)Math.PI / 180f))) * 0.5f, Vector3.zero, num * num);
		base.transform.localScale = Vector3.Lerp(new Vector3(2f, 2f, 1f), new Vector3(1f, 1f, 1f), num * num);
		if (frames == 20)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
