using UnityEngine;

public class FroggitFlyAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 100;
		bbSize = new Vector2(165f, 140f);
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 20 == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/FlyBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
		}
	}
}
