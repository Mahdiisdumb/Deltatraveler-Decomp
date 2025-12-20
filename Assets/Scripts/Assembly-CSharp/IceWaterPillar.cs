using UnityEngine;

public class IceWaterPillar : MonoBehaviour
{
	private int frames;

	private float alpha = 1f;

	private void Update()
	{
		frames++;
		Vector3 localScale = base.transform.localScale;
		if (localScale.x > 2f)
		{
			localScale.x -= 0.4f;
		}
		localScale.y += 0.2f;
		if ((double)localScale.y > 0.8)
		{
			alpha -= 0.2f;
		}
		base.transform.localScale = localScale;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, alpha);
		if (frames > 30)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
