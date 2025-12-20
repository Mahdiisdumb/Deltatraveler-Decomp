using System;
using UnityEngine;

public class PorkyFlameBullet : BulletBase
{
	private float distance;

	private int angle;

	private bool activated;

	private Vector3 basePos;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
		tpGrazeValue = 0.2f;
	}

	private void Update()
	{
		if (activated)
		{
			frames++;
		}
		sr.flipX = frames / 2 % 2 == 1;
		float num = (float)frames / 45f;
		if (num < 1f)
		{
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
		}
		else
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		base.transform.position = basePos + new Vector3(Mathf.Sin((float)angle * ((float)Math.PI / 180f)), Mathf.Cos((float)angle * ((float)Math.PI / 180f))) * Mathf.Lerp(0f, distance, num);
		sr.color = new Color(1f, 1f, 1f, (float)(45 - frames) / 10f);
	}

	public void Activate(int angle, float distance, bool playSound)
	{
		this.distance = distance;
		this.angle = angle;
		basePos = base.transform.position;
		if (playSound)
		{
			aud.Play();
		}
		activated = true;
	}
}
