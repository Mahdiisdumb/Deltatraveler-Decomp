using UnityEngine;

public class SansZ : MonoBehaviour
{
	private int frames;

	private float yVelocity = 1f;

	private void Update()
	{
		yVelocity += 0.04f;
		if (base.transform.localScale.x < 1f)
		{
			base.transform.localScale += new Vector3(0.04f, 0.04f);
		}
		frames++;
		base.transform.position += new Vector3(Mathf.Sin((float)frames / 4f) + 2f, Mathf.Cos((float)frames / 4f) + yVelocity) / 48f;
		if (frames > 60)
		{
			GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 7f - (float)frames / 10f);
			if (frames == 70)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}
}
