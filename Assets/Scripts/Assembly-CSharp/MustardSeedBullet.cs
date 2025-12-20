using UnityEngine;

public class MustardSeedBullet : BulletBase
{
	private Vector3 velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 4;
	}

	private void Start()
	{
		velocity = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 24f) - base.transform.position;
	}

	private void Update()
	{
		base.transform.position += velocity;
	}
}
