using UnityEngine;

public class PaintBullet : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
	}

	private void LateUpdate()
	{
		base.transform.localScale += new Vector3(0f, 4f, 0f);
	}

	public override void SOULHit()
	{
		if ((bool)Object.FindObjectOfType<PaintbrushAttack>())
		{
			Object.FindObjectOfType<PaintbrushAttack>().GetHit();
		}
		if ((bool)Object.FindObjectOfType<CarPaintbrushAttack>())
		{
			Object.FindObjectOfType<CarPaintbrushAttack>().GetHit();
		}
	}
}
