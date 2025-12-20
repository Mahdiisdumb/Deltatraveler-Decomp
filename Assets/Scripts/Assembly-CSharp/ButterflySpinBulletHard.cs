using System;
using UnityEngine;

public class ButterflySpinBulletHard : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private float angle;

	private bool pulsate;

	private float speed;

	private bool healing;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
	}

	private void Update()
	{
		frames++;
		float num = 1.3f;
		if (pulsate)
		{
			num = Mathf.Lerp(1.3f, 1.05f, (0f - Mathf.Cos((float)(frames * 9) * ((float)Math.PI / 180f)) + 1f) / 2f);
		}
		angle += speed;
		base.transform.localPosition = new Vector3(Mathf.Sin(angle * ((float)Math.PI / 180f)), 0f - Mathf.Cos(angle * ((float)Math.PI / 180f))) * num;
		base.transform.eulerAngles = new Vector3(0f, 0f, angle);
		sr.sprite = sprites[frames / 5 % 2];
	}

	public void Activate(float angle, bool pulsate, float speed, bool isHealing)
	{
		this.angle = angle;
		this.pulsate = pulsate;
		this.speed = speed;
		base.transform.localPosition = new Vector3(Mathf.Sin(angle * ((float)Math.PI / 180f)), 0f - Mathf.Cos(angle * ((float)Math.PI / 180f))) * 1.3f;
		base.transform.eulerAngles = new Vector3(0f, 0f, angle);
		healing = isHealing;
		if (isHealing)
		{
			ChangeType(3);
			baseDmg = 1;
			destroyOnHit = true;
		}
	}

	public override void SOULHit()
	{
		if (healing && UnityEngine.Object.FindObjectOfType<Whimsalot>().GetSatisfactionLevel() < 100)
		{
			UnityEngine.Object.FindObjectOfType<Whimsalot>().AddActPoints(50);
		}
		base.SOULHit();
	}
}
