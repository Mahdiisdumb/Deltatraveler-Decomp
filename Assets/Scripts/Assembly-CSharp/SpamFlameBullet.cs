using System;
using UnityEngine;

public class SpamFlameBullet : FlameBullet
{
	private bool falling = true;

	private float gravity;

	private float angle;

	private float speed = 0.25f;

	private int multi = 1;

	protected override void Awake()
	{
		base.Awake();
		multi = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
		tpGrazeValue = 0.025f;
		tpGrazeValueReuse = 0f;
	}

	protected override void Update()
	{
		base.Update();
		angle += 12f;
		float num = (Mathf.Cos(angle * ((float)Math.PI / 180f)) - 1f) * (float)multi / 2f;
		if (falling)
		{
			speed += gravity;
			base.transform.position = new Vector3(num * 1.64f, base.transform.position.y - speed / 48f);
			if (base.transform.position.y <= -2.867f)
			{
				GetComponent<AudioSource>().Play();
				falling = false;
			}
		}
		else
		{
			base.transform.position = new Vector3(num * 1.64f, base.transform.position.y + 0.0020833334f);
		}
	}

	public void SetGravity(float gravity)
	{
		this.gravity = gravity;
	}
}
