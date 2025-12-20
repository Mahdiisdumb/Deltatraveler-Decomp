using System;
using UnityEngine;

public class GauntletCannonBall : ActionBulletBase
{
	private readonly float SPEED = 1f / 3f;

	private Vector3 direction = Vector3.zero;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 7;
	}

	protected override void Update()
	{
		base.Update();
		GetComponent<Rigidbody2D>().MovePosition(base.transform.position + direction * SPEED);
		if (base.transform.localPosition.y > 6f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetDirection(float angle)
	{
		direction = new Vector3(0f - Mathf.Sin(angle * ((float)Math.PI / 180f)), Mathf.Cos(angle * ((float)Math.PI / 180f)));
	}
}
