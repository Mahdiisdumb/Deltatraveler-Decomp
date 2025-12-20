using UnityEngine;

public class OverworldZ : MonoBehaviour
{
	private int frames;

	private Vector3 velocity = Vector3.zero;

	private float a = 0.2f;

	private void Awake()
	{
	}

	private void Update()
	{
		frames++;
		velocity = (velocity.magnitude - 0.01f) * velocity.normalized;
		base.transform.position += velocity / 24f + new Vector3(Mathf.Sin((float)frames / 8f) * 0.5f / 24f, 0f);
		if (frames < 40 && a < 1f)
		{
			a += 0.05f;
		}
		else if (frames > 50)
		{
			a -= 0.05f;
		}
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, a);
		if (a <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void SetVelocity(float x)
	{
		velocity = new Vector3(x, 1f);
	}
}
