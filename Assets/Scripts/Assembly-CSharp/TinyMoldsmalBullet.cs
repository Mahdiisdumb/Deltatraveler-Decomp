using System;
using UnityEngine;

public class TinyMoldsmalBullet : BulletBase
{
	private Vector3 direction;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
	}

	private void Update()
	{
		base.transform.position += direction * 0.028125001f;
	}

	public void SetDirection(int degree)
	{
		direction = new Vector3(Mathf.Sin((float)degree * ((float)Math.PI / 180f)), Mathf.Cos((float)degree * ((float)Math.PI / 180f)));
	}
}
