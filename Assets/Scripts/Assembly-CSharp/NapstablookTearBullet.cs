using System;
using UnityEngine;

public class NapstablookTearBullet : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		base.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(-50, 50));
	}

	private void Update()
	{
		base.transform.rotation = Quaternion.Lerp(base.transform.rotation, Quaternion.Euler(0f, 0f, 0f), 0.075f);
		float z = base.transform.rotation.eulerAngles.z;
		base.transform.position += new Vector3(Mathf.Sin(z * ((float)Math.PI / 180f)), 0f - Mathf.Cos(z * ((float)Math.PI / 180f))) * (5f / 48f);
	}
}
