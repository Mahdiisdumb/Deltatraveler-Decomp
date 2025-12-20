using UnityEngine;

public class WaterIce : MonoBehaviour
{
	[SerializeField]
	private float xStart;

	[SerializeField]
	private float xEnd;

	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 10 % 2];
		base.transform.position += new Vector3(0.0625f, 0f);
		if (base.transform.position.x > xEnd)
		{
			base.transform.position = new Vector3(xStart, base.transform.position.y);
		}
	}
}
