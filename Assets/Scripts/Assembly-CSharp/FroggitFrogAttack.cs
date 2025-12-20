using UnityEngine;

public class FroggitFrogAttack : AttackBase
{
	private GameObject frog;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(165f, 140f);
	}

	protected override void Update()
	{
		if (!frog && isStarted)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		frog = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/FrogBullet"), new Vector3(1.18f, -2.6f), Quaternion.identity, base.transform.parent);
		base.StartAttack();
	}
}
