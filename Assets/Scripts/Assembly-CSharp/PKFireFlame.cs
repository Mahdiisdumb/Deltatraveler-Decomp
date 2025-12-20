using UnityEngine;

public class PKFireFlame : FlameBullet
{
	private bool activated;

	private Vector3 tradjectory;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		tpGrazeValue = (Object.FindObjectOfType<AttackBase>() ? 0.3f : 0f);
		tpGrazeValueReuse = 0f;
		tpBuildRate = 0f;
	}

	protected override void Update()
	{
		base.Update();
		if (activated)
		{
			base.transform.position += tradjectory * velocity / 48f;
			float num = velocity / 12f;
			if (num < 0.05f)
			{
				num = 0.05f;
			}
			velocity += num;
			if (Mathf.Abs(base.transform.position.x) > 8f || Mathf.Abs(base.transform.position.y) > 7f)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void Activate(float initialVelocity, Vector3 tradjectory)
	{
		velocity = initialVelocity;
		this.tradjectory = tradjectory;
		activated = true;
	}
}
