using System;
using UnityEngine;

public class EnemyBlood : MonoBehaviour
{
	private int frames;

	private void Awake()
	{
		if (GameManager.GetOptions().contentSetting.value >= 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		if (frames < 180)
		{
			frames++;
			float num = (float)frames / 180f;
			base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(6f, 3.5f, 1f), Mathf.Sin(num * (float)Math.PI * 0.5f));
		}
	}
}
