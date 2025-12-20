using System;
using UnityEngine;

public class TorielHandBullet : BulletBase
{
	private float speed;

	[SerializeField]
	private bool isTop = true;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 3;
		destroyOnHit = false;
		if (isTop)
		{
			base.transform.position = new Vector3(-2.3f, -0.573f);
		}
		else
		{
			base.transform.position = new Vector3(2.3f, -2.72f);
		}
	}

	private void Update()
	{
		if (speed < 6f)
		{
			speed += 0.2f;
		}
		Vector3 position = base.transform.position;
		if (isTop)
		{
			position.x += speed / 48f;
			position.y = -0.573f - Mathf.Cos(MathF.PI * (Mathf.Abs(position.x) / 4.8f)) * 0.758f;
		}
		else
		{
			position.x -= speed / 48f;
			position.y = -2.72f + Mathf.Cos(MathF.PI * (Mathf.Abs(position.x) / 4.8f)) * 0.32f;
		}
		base.transform.position = position;
		if (Mathf.Abs(position.x) >= 3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void SOULHit()
	{
		if ((bool)Util.FindObjectOfType<TorielHandAttack>())
		{
			Util.FindObjectOfType<TorielHandAttack>().SetToDestroy();
		}
		if ((bool)Util.FindObjectOfType<TorielDualHandAttack>())
		{
			Util.FindObjectOfType<TorielDualHandAttack>().SetToDestroy();
		}
	}
}
