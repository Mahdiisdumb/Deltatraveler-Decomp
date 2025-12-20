using System;
using UnityEngine;

public class GauntletSpearBullet : ActionBulletBase
{
	private Vector3 direction = new Vector3(0f - Mathf.Sin((float)Math.PI / 6f), 0f - Mathf.Cos((float)Math.PI / 6f));

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 7;
	}

	protected override void Update()
	{
		base.Update();
		frames++;
		if (frames <= 10)
		{
			base.transform.position += direction * 0.16f;
		}
		else if (frames >= 20)
		{
			if (frames == 20)
			{
				PlaySFX("sounds/snd_spearrise");
			}
			velocity += 1f;
			base.transform.position += direction * (velocity / 48f);
		}
		if (Mathf.Abs(base.transform.localPosition.y) >= 8f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetDirection()
	{
		direction.x *= base.transform.localScale.x;
		direction.y *= base.transform.localScale.y;
	}
}
