public class FlameBullet : BulletBase
{
	protected override void Awake()
	{
		base.Awake();
		baseDmg = 1;
		karmaImpact = 5;
	}

	protected virtual void Update()
	{
		frames++;
		if (frames % 6 == 3)
		{
			sr.flipX = true;
		}
		else if (frames % 6 == 0)
		{
			sr.flipX = false;
			frames = 0;
		}
	}
}
