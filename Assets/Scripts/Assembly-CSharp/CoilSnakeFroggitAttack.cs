using UnityEngine;

public class CoilSnakeFroggitAttack : AttackBase
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
		if (isStarted && frames % 12 == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegCarrotBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegCarrotBullet>();
		}
	}
}
