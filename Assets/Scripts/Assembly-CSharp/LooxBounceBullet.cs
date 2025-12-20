using UnityEngine;

public class LooxBounceBullet : BulletBase
{
	private float velocity = 1f / 64f;

	private Vector3 trajectory = new Vector3(1f, 1f);

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
		trajectory = new Vector3((Random.Range(0, 2) != 1) ? 1 : (-1), (Random.Range(0, 2) != 1) ? 1 : (-1));
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0f);
	}

	private void Update()
	{
		if (frames < 10)
		{
			frames++;
			GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)frames / 10f);
		}
		base.transform.position += trajectory * velocity;
		velocity += 0.0010416667f;
		if (base.transform.position.x >= 1.7f)
		{
			trajectory = new Vector3(-1f, trajectory.y);
		}
		if (base.transform.position.x <= -1.7f)
		{
			trajectory = new Vector3(1f, trajectory.y);
		}
		if (base.transform.position.y >= -0.4f)
		{
			trajectory = new Vector3(trajectory.x, -1f);
		}
		if (base.transform.position.y <= -2.922f)
		{
			trajectory = new Vector3(trajectory.x, 1f);
		}
	}
}
