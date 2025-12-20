using UnityEngine;

public class Petal : MonoBehaviour
{
	private Color32 colorA = new Color32(160, 236, byte.MaxValue, byte.MaxValue);

	private Color32 colorB = new Color32(160, 236, byte.MaxValue, 0);

	private Vector3 momentum;

	private void Awake()
	{
		momentum = new Vector3(Random.Range(0.09f, 0.135f), 0f - Random.Range(0.05f, 0.075f));
		base.transform.rotation = Quaternion.Euler(0f, 0f, Random.Range(0, 360));
	}

	private void Update()
	{
		base.transform.position += momentum;
		GetComponent<SpriteRenderer>().color = Color32.Lerp(colorB, colorA, (base.transform.position.y + 1f) / 2.65f);
		if (base.transform.position.x > 6.86f || base.transform.position.y < -1f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SetColor(Color32 color)
	{
		colorA = color;
		colorB = new Color32(color.r, color.g, color.b, 0);
	}
}
