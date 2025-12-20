using UnityEngine;

public class DoggoBullet : BulletBase
{
	private bool scan;

	private bool reverse;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 8;
		destroyOnHit = false;
		reverse = base.transform.position.x < 0f;
		if (Object.FindObjectOfType<Doggo>().GetProgress() == 0)
		{
			scan = true;
			ChangeType(1);
		}
		else
		{
			velocity = ((Object.FindObjectOfType<Doggo>().GetProgress() == 1) ? 8 : 4);
			ChangeType((Random.Range(0, 5) != 0) ? 1 : 2);
		}
	}

	private void Update()
	{
		SOUL sOUL = Object.FindObjectOfType<SOUL>();
		float y = sOUL.transform.position.y - 0.304f;
		if (scan)
		{
			if (Mathf.Abs(base.transform.position.x - sOUL.transform.position.x) <= 0.25f)
			{
				velocity = 2f;
			}
			else
			{
				velocity += 0.15f;
			}
		}
		base.transform.position = new Vector3(base.transform.position.x + (float)(reverse ? 1 : (-1)) * velocity / 48f, y);
		if (Mathf.Abs(base.transform.position.x) > 3.5f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void SOULHit()
	{
		base.SOULHit();
		Object.FindObjectOfType<Doggo>().HitLastAttack();
	}
}
