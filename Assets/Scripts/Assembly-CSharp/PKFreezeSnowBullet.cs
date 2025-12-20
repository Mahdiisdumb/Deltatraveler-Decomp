using System;
using UnityEngine;

public class PKFreezeSnowBullet : BulletBase
{
	private Vector3 direction;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		direction = ((UnityEngine.Random.Range(0, 2) == 1) ? Vector2.left : Vector2.right);
	}

	private void Update()
	{
		velocity += 0.1f;
		if (velocity > 1f)
		{
			velocity += 0.2f;
		}
		base.transform.position += direction * velocity / 96f;
	}

	public void SetDirection(int degree)
	{
		direction = new Vector3(Mathf.Sin((float)degree * ((float)Math.PI / 180f)), Mathf.Cos((float)degree * ((float)Math.PI / 180f)));
	}
}
