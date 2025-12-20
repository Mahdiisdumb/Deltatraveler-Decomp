using System;
using UnityEngine;

public class GyftrotBreath : MonoBehaviour
{
	private float startY;

	private float finalY;

	private bool reverseRotate;

	private int frames;

	private void Start()
	{
		startY = base.transform.position.y;
		finalY = startY - 0.618f;
		reverseRotate = UnityEngine.Random.Range(0, 2) == 0;
		base.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
	}

	private void Update()
	{
		frames++;
		float num = (float)frames / 20f;
		if (num > 1f)
		{
			num = 1f;
		}
		base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(startY, finalY, Mathf.Sin(num * (float)Math.PI * 0.5f)));
		int num2 = ((!reverseRotate) ? 1 : (-1));
		base.transform.rotation = Quaternion.Euler(0f, 0f, base.transform.rotation.eulerAngles.z + (float)num2);
		base.transform.localScale = Vector3.Lerp(new Vector3(0.15f, 0.15f), new Vector3(1.3f, 1.3f), (float)frames / 40f);
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0.5f, 0f, (float)(frames - 20) / 20f));
		if (frames == 40)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
