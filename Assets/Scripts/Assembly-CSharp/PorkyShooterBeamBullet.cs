using UnityEngine;

public class PorkyShooterBeamBullet : BulletBase
{
	private Vector3 tradjectory;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		destroyOnHit = true;
		tpGrazeValue = 0.5f;
	}

	private void Start()
	{
		base.transform.up = Object.FindObjectOfType<SOUL>().transform.position - base.transform.position;
		tradjectory = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 0.125f) - base.transform.position;
	}

	private void Update()
	{
		base.transform.position += tradjectory;
		if ((Mathf.Abs(base.transform.position.x) >= 7f || Mathf.Abs(base.transform.position.y) >= 5f) && !aud.isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void Parry()
	{
		base.Parry();
		Object.Destroy(base.gameObject);
	}
}
