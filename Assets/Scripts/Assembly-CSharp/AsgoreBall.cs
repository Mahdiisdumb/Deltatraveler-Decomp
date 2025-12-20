using UnityEngine;

public class AsgoreBall : MonoBehaviour
{
	private float yMove = 1f / 192f;

	private float increaseVelocity;

	private float xMove;

	private void Awake()
	{
		float num = Random.Range(0.25f, 2f);
		increaseVelocity = Random.Range(0.00020833332f, 0.0026041667f);
		xMove = Random.Range(-1f / 24f, 1f / 24f);
		base.transform.localScale = new Vector3(num, num, 1f);
		Color color = Object.FindObjectOfType<BreathingBG>().GetColor();
		color.r += 0.5f;
		color.g += 0.5f;
		color.b += 0.5f;
		color.a = 1f;
		GetComponent<SpriteRenderer>().color = color;
	}

	private void Update()
	{
		Color32 color = GetComponent<SpriteRenderer>().color;
		color.a -= 3;
		GetComponent<SpriteRenderer>().color = color;
		yMove += increaseVelocity;
		base.transform.position += new Vector3(xMove, yMove);
		if (color.a <= 0)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
