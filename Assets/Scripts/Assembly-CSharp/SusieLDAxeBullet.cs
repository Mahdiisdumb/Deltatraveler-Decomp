using System;
using UnityEngine;

public class SusieLDAxeBullet : BulletBase
{
	private float xLimitMulti;

	private float yVelocity;

	private bool right;

	private float endVelocity;

	private int decreaseAngle = 1;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = true;
		right = UnityEngine.Random.Range(0, 2) == 0;
		yVelocity = UnityEngine.Random.Range(1f, 3f) / 48f;
		if (UnityEngine.Random.Range(0, 2) == 0)
		{
			yVelocity *= -1f;
		}
		float num = (right ? 4f : (-4f));
		float num2 = 0f;
		num2 = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y - yVelocity * 30f;
		base.transform.position = new Vector3(num, num2);
		xLimitMulti = Mathf.Abs(UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x + num);
		sr.color = new Color(1f, 1f, 1f, 0f);
		if (right)
		{
			decreaseAngle = -1;
			sr.flipX = true;
		}
	}

	private void Update()
	{
		frames++;
		if (frames <= 5)
		{
			sr.color = new Color(1f, 1f, 1f, (float)frames / 5f);
		}
		int num = frames / 3 * 45;
		base.transform.eulerAngles = new Vector3(0f, 0f, num * decreaseAngle);
		int num2 = ((!right) ? 1 : (-1));
		if (frames <= 60)
		{
			float x = base.transform.position.x;
			base.transform.position = new Vector3((4f - Mathf.Sin((float)(frames * 3) * ((float)Math.PI / 180f)) * xLimitMulti) * (float)num2, base.transform.position.y + yVelocity);
			endVelocity = base.transform.position.x - x;
			return;
		}
		endVelocity += Mathf.Sign(endVelocity) / 48f;
		base.transform.position += new Vector3(endVelocity, yVelocity);
		if (Mathf.Abs(base.transform.position.x) > 7f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
