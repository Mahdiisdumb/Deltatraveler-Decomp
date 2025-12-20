using System;
using UnityEngine;

public class BatBoomerangBullet : BulletBase
{
	private float xLimitMulti;

	private float yVelocity;

	private bool right;

	private int decreaseAngle;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		destroyOnHit = false;
		right = UnityEngine.Random.Range(0, 2) == 0;
		yVelocity = UnityEngine.Random.Range(1f, 3f) / 48f;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			yVelocity *= -1f;
		}
		float x = (right ? 2.5f : (-2.5f));
		float num = 0f;
		num = ((!(yVelocity < 0f)) ? UnityEngine.Random.Range(-1.22f, -3.2f) : UnityEngine.Random.Range(0f, -1.22f));
		base.transform.position = new Vector3(x, num);
		xLimitMulti = UnityEngine.Random.Range(1.4f, 2.8f);
		sr.color = new Color(1f, 1f, 1f, 0f);
		decreaseAngle = ((!(yVelocity > 0f)) ? 1 : (-1));
		if (!right)
		{
			decreaseAngle *= -1;
		}
	}

	private void Update()
	{
		frames++;
		if (frames <= 5)
		{
			sr.color = new Color(1f, 1f, 1f, (float)frames / 5f);
		}
		else if (frames >= 55)
		{
			sr.color = new Color(1f, 1f, 1f, 1f - (float)(frames - 55) / 5f);
		}
		base.transform.eulerAngles = new Vector3(0f, 0f, frames * 12 * decreaseAngle);
		int num = ((!right) ? 1 : (-1));
		base.transform.position = new Vector3((2.5f - Mathf.Sin((float)(frames * 3) * ((float)Math.PI / 180f)) * xLimitMulti) * (float)num, base.transform.position.y + yVelocity);
		if (frames == 60)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
