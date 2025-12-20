using UnityEngine;

public class CornerWaterWake : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private float wide;

	private void Awake()
	{
		wide = 0f - (base.transform.position.y - 3f);
		frames = Random.Range(0, 20);
	}

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 5 % 4];
		if (base.transform.position.x < 12.797f + wide)
		{
			base.transform.position += new Vector3(0.0625f, 0f);
			return;
		}
		base.transform.position += new Vector3(0f, 0.0625f);
		if (base.transform.position.y > 5.105f)
		{
			base.transform.position = new Vector3(-6.781f, 0f - wide + 3f);
		}
	}
}
