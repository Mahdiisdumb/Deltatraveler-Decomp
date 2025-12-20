using UnityEngine;

public class JerrySpinSword : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames % 4];
	}
}
