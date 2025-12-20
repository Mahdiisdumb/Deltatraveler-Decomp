using UnityEngine;

public class SnowdrakeSpinBullet : BulletBase
{
	private float velocity;

	private Vector3 direction = Vector3.zero;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		if ((bool)Object.FindObjectOfType<Feraldrake>())
		{
			baseDmg = 8;
		}
		destroyOnHit = false;
	}

	private void Update()
	{
		base.transform.position += direction * velocity / 48f;
		frames++;
		if (frames % 2 == 0)
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, base.transform.rotation.eulerAngles.z - 90f);
		}
		if (Mathf.Abs(base.transform.position.x) > 2.5f || Mathf.Abs(base.transform.position.y) > 3.5f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Activate(float velocity, Vector2 direction)
	{
		this.velocity = velocity;
		this.direction = direction;
	}
}
