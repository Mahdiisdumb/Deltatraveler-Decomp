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
		if ((bool)Util.FindObjectOfType<PaintbrushAttack>())
		{
			Util.FindObjectOfType<PaintbrushAttack>().GetHit();
		}
		if ((bool)Util.FindObjectOfType<CarPaintbrushAttack>())
		{
			Util.FindObjectOfType<CarPaintbrushAttack>().GetHit();
		}
	}
}
