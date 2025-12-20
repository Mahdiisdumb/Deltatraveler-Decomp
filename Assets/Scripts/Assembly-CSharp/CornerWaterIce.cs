using UnityEngine;

public class CornerWaterIce : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private float wide;

	private void Awake()
	{
		wide = 0f - (base.transform.position.y - 3f);
	}

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 10 % 2];
		if (base.transform.position.x < 13.74f)
		{
			base.transform.position += new Vector3(0.0625f, 0f);
			return;
		}
		base.transform.position += new Vector3(0f, 0.0625f);
		if (base.transform.position.y > 5.5f)
		{
			base.transform.position = new Vector3(-7.5f, 0f - wide + 3f);
		}
	}
}
