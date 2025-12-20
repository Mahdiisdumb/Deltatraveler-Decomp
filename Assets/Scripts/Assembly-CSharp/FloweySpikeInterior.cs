public class FloweySpikeInterior : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		destroyOnHit = false;
		if ((bool)Util.FindObjectOfType<Jerry>())
		{
			baseDmg = Util.FindObjectOfType<Jerry>().GetDamageValue();
		}
		tpGrazeValue = 0f;
		tpGrazeValueReuse = 0.25f;
	}
}
