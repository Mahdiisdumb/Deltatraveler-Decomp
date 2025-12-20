using System;
using UnityEngine;

public class IceCapIcicleBullet : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		tpGrazeValue = 0.5f;
		sr = GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		if (sr.flipY)
		{
			frames++;
			if (frames == 50)
			{
				sr.flipY = false;
			}
		}
		else
		{
			frames--;
			if (frames == 0)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		float num = (float)frames / 50f;
		num = Mathf.Sin(num * (float)Math.PI * 0.5f);
		base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(-3.44f, -0.15f, num));
	}
}
