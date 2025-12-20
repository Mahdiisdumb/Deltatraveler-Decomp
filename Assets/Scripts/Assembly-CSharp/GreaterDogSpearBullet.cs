using System;
using UnityEngine;

public class GreaterDogSpearBullet : BulletBase
{
	private bool moveToLeft;

	private bool doWave;

	private float yFrom;

	private float yTo;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		destroyOnHit = false;
		moveToLeft = base.transform.position.x > 0f;
	}

	private void Update()
	{
		float num = 1f / 12f;
		if (moveToLeft)
		{
			num *= -1f;
		}
		if (doWave)
		{
			frames++;
			base.transform.position = new Vector3(base.transform.position.x + num, Mathf.Lerp(yFrom, yTo, (Mathf.Cos((float)(frames * 12) * ((float)Math.PI / 180f)) + 1f) / 2f));
		}
		else
		{
			base.transform.position += new Vector3(num, 0f);
		}
		if (Mathf.Abs(base.transform.position.x) > 3.5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void DoWave(float centerY, int startFrames)
	{
		doWave = true;
		yFrom = base.transform.position.y;
		yTo = centerY;
		frames = startFrames;
	}
}
