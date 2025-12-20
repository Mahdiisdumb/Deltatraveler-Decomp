using UnityEngine;

public class SnowSculptureBreak : MonoBehaviour
{
	private int frames;

	private float xVelocity;

	private void Awake()
	{
		xVelocity = Random.Range(-1f / 12f, 1f / 12f);
	}

	private void Update()
	{
		base.transform.position += new Vector3(xVelocity, -1f / 12f);
		frames++;
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 1f), (float)frames / 10f);
		if (frames == 10)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
