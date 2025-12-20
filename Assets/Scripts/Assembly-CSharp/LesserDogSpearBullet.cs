using System;
using UnityEngine;

public class LesserDogSpearBullet : BulletBase
{
	private bool moveToLeft;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
		moveToLeft = base.transform.position.x > 0f;
		int num = UnityEngine.Random.Range(0, 3);
		ChangeType(num);
		if (num == 0)
		{
			frames = UnityEngine.Random.Range(0, 40);
		}
	}

	private void Update()
	{
		float num = 0.0625f;
		if (moveToLeft)
		{
			num *= -1f;
		}
		if (base.gameObject.tag == "Bullet")
		{
			frames++;
			base.transform.position = new Vector3(base.transform.position.x + num, Mathf.Lerp(-2.9f, -2.074f, (Mathf.Cos((float)(frames * 9) * ((float)Math.PI / 180f)) + 1f) / 2f));
		}
		else
		{
			base.transform.position += new Vector3(num, 0f);
		}
		if (Mathf.Abs(base.transform.position.x) > 2.8f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
