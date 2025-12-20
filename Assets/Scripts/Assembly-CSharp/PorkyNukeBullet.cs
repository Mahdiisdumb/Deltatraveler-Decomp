using UnityEngine;

public class PorkyNukeBullet : BulletBase
{
	private bool exploded;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
	}

	private void Update()
	{
		velocity += 0.25f;
		base.transform.position += new Vector3(0f, (0f - velocity) / 48f, 0f);
		if (base.transform.position.y <= -2.843f && !exploded)
		{
			Explode();
		}
	}

	public override void SOULHit()
	{
		base.SOULHit();
		if (!exploded)
		{
			Explode();
		}
	}

	public override void Parry()
	{
		exploded = true;
		Object.Destroy(base.gameObject);
	}

	private void Explode()
	{
		exploded = true;
		for (int i = 0; i < 12; i++)
		{
			int angle = i * 30;
			float distance = 2f;
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyFlameBullet"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<PorkyFlameBullet>().Activate(angle, distance, i < 2);
		}
		Object.Destroy(base.gameObject);
	}
}
