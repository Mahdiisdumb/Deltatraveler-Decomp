using System;
using UnityEngine;

public class ButterflySpinBullet : BulletBase
{
	private float angleOffset;

	private Vector3 center = Vector3.zero;

	[SerializeField]
	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 4;
	}

	private void Update()
	{
		frames++;
		float num = (float)(frames * 3) + angleOffset;
		float num2 = 1f + Mathf.Sin((float)(270 + frames * 3) * ((float)Math.PI / 180f)) * 0.125f;
		base.transform.position = center + new Vector3(Mathf.Sin(num * ((float)Math.PI / 180f)), 0f - Mathf.Cos(num * ((float)Math.PI / 180f))) * num2;
		base.transform.rotation = Quaternion.Euler(0f, 0f, num);
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 4 % 2];
	}

	public void SetValues(float angleOffset, Vector3 center)
	{
		this.center = center;
		this.angleOffset = angleOffset;
	}
}
