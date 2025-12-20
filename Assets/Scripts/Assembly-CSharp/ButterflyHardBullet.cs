using System;
using UnityEngine;

public class ButterflyHardBullet : BulletBase
{
	private int speedFrames;

	private float speedVariation = 1f;

	private int flapCompare = 2;

	private int sprite;

	[SerializeField]
	private Sprite[] sprites;

	private float angle;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
		angle = UnityEngine.Random.Range(-12f, 12f);
		base.transform.eulerAngles = new Vector3(0f, 0f, 90f + angle);
	}

	private void Update()
	{
		speedFrames++;
		speedVariation = Mathf.Lerp(1f, 0f, (float)speedFrames / 30f);
		if (speedFrames <= 30)
		{
			if (speedFrames == 15)
			{
				flapCompare = 3;
			}
			else if (speedFrames == 20)
			{
				flapCompare = 4;
			}
			else if (speedFrames == 30)
			{
				flapCompare = 6;
			}
		}
		if (speedFrames % flapCompare == 0)
		{
			sprite = (sprite + 1) % 2;
			sr.sprite = sprites[sprite];
		}
		float num = Mathf.Lerp(1.5f, 5f, speedVariation);
		base.transform.position += new Vector3(0f - Mathf.Sin(angle * ((float)Math.PI / 180f)), Mathf.Cos(angle * ((float)Math.PI / 180f))) * num / 48f;
		if (base.transform.position.y > 0.14f)
		{
			sr.color = new Color(1f, 1f, 1f, (0.86f - base.transform.position.y) / 0.72f);
			if (sr.color.a <= 0f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void SetSpeedFrames(int speedFrames)
	{
		this.speedFrames = speedFrames;
	}
}
