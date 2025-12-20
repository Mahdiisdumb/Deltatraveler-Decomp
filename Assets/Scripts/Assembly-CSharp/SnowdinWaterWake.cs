using UnityEngine;

public class SnowdinWaterWake : MonoBehaviour
{
	[SerializeField]
	private float xStart;

	[SerializeField]
	private float xEnd;

	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private void Awake()
	{
		frames = Random.Range(0, 20);
	}

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 5 % 4];
		base.transform.position += new Vector3(5f / 96f, 0f);
		if (base.transform.position.x > xEnd)
		{
			base.transform.position = new Vector3(xStart, base.transform.position.y);
		}
	}
}
