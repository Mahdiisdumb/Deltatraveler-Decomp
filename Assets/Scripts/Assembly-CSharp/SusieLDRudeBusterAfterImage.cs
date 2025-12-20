using UnityEngine;

public class SusieLDRudeBusterAfterImage : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames = 8;

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 2 % sprites.Length];
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, GetComponent<SpriteRenderer>().color.a - 0.08f);
		base.transform.localScale -= new Vector3(0f, 0.1f);
		if (GetComponent<SpriteRenderer>().color.a <= 0f || base.transform.localScale.y <= 0.1f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Activate(float alpha)
	{
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 2 % sprites.Length];
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha - 0.2f);
	}
}
