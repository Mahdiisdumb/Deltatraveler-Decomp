using System;
using UnityEngine;

public class StarBullet : BulletBase
{
	private float velocity = 3f;

	private Vector3 tradjectory;

	private bool activated;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 11;
		destroyOnHit = false;
	}

	private void Update()
	{
		if (activated)
		{
			base.transform.position += tradjectory * velocity / 48f;
			if (velocity < 6f)
			{
				velocity += 0.25f;
			}
			base.transform.eulerAngles += new Vector3(0f, 0f, 5f);
			if (Mathf.Abs(base.transform.position.x) > 8f || Mathf.Abs(base.transform.position.y) > 7f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void Activate(float angle)
	{
		tradjectory = new Vector3(Mathf.Sin(angle * ((float)Math.PI / 180f)), Mathf.Cos(angle * ((float)Math.PI / 180f)));
		activated = true;
	}
}
