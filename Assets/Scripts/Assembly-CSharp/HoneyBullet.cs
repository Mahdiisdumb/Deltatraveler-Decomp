using UnityEngine;

public class HoneyBullet : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
		destroyOnHit = false;
		ChangeType(3);
	}

	public override void SOULHit()
	{
		base.SOULHit();
		sr.color = new Color(0.5f, 0.5f, 0.5f, 1f);
		GetComponent<BoxCollider2D>().enabled = false;
		if ((bool)Util.FindObjectOfType<BeeController>())
		{
			Util.FindObjectOfType<BeeController>().StartFiringBees();
		}
		if ((bool)Util.FindObjectOfType<MightyBear>())
		{
			Util.FindObjectOfType<MightyBear>().PissOffBees();
		}
	}
}
