public class IceCapSinePillar : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 11;
		destroyOnHit = false;
		tpGrazeValue = 0.25f;
		tpGrazeValueReuse = 0.1f;
	}
}
