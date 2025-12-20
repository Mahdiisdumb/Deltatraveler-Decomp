using System;
using UnityEngine;

public class PorkyFinalBeam : MonoBehaviour
{
	private int frames;

	private Transform[] balls = new Transform[2];

	private void Awake()
	{
		for (int i = 0; i < 2; i++)
		{
			balls[i] = base.transform.Find("ball" + i);
		}
	}

	private void LateUpdate()
	{
		base.transform.position = UnityEngine.Object.FindObjectOfType<Porky>().GetPart("mech").position - new Vector3(0f, 2.122f);
		frames++;
		float num = Mathf.Cos((float)(frames * 24) * ((float)Math.PI / 180f));
		for (int i = 0; i < 2; i++)
		{
			balls[i].transform.localScale = Vector3.Lerp(Vector3.zero, new Vector3(11f + num, 11f + num), (float)frames / 60f);
		}
	}
}
