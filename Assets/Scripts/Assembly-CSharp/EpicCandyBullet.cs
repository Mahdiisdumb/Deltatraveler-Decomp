using UnityEngine;

public class EpicCandyBullet : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 3;
		ChangeType(3);
	}

	public override void PreSOULHit()
	{
		base.PreSOULHit();
		Object.FindObjectOfType<TPBar>().AddTP(16);
	}
}
