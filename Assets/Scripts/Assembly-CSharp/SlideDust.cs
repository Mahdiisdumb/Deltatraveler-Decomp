using UnityEngine;

public class SlideDust : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private void Update()
	{
		int num = frames / 2;
		if (num >= sprites.Length)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = sprites[num];
			base.transform.position += new Vector3(0f, 0.0625f);
		}
		frames++;
	}
}
