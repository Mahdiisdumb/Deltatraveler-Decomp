using System;
using UnityEngine;

public class ExplosionFlameBullet : BulletBase
{
	private bool activated;

	private int speed;

	private int angle;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = true;
	}

	private void Update()
	{
		if (activated)
		{
			GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
			base.transform.position += new Vector3(Mathf.Cos((float)angle * ((float)Math.PI / 180f)), Mathf.Sin((float)angle * ((float)Math.PI / 180f))) * ((float)speed / 48f);
		}
	}

	public void Activate(int angle, int layer)
	{
		activated = true;
		this.angle = angle;
		speed = 8 - 2 * layer;
		if (layer % 2 == 1)
		{
			this.angle -= 10;
		}
	}
}
