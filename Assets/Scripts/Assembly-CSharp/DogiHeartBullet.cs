using System;
using UnityEngine;

public class DogiHeartBullet : BulletBase
{
	private float degree;

	private Vector3 center;

	private bool sad = true;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 8;
		destroyOnHit = false;
		center = base.transform.position;
	}

	private void Update()
	{
		if (sad)
		{
			frames++;
			float num = (float)frames / 30f;
			if (num <= 1f)
			{
				base.transform.position = new Vector3(base.transform.position.x + 1f / 24f, Mathf.Lerp(-1.78f, -3f, num * num));
			}
		}
		else
		{
			frames++;
			center.x += 1f / 24f;
			float num2 = Mathf.Lerp(0f, 1.15f, (float)frames / 22f);
			float num3 = 1.6363636f * (float)frames + degree;
			float x = Mathf.Cos(num3 * ((float)Math.PI / 180f));
			float y = Mathf.Sin(num3 * ((float)Math.PI / 180f));
			base.transform.position = new Vector3(x, y) * num2 + center;
		}
		if (center.x >= 5.08f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetDegree(float degree)
	{
		sad = false;
		this.degree = degree;
	}
}
