using UnityEngine;

public class ButterflyFallAttack : AttackBase
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
		if (isStarted && frames % 9 == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/ButterflyFallBullet"), base.transform).transform.position = new Vector3(Object.FindObjectOfType<SOUL>().transform.position.x - 1f, -2.74f);
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/ButterflyFallBullet"), base.transform).transform.position = new Vector3(Object.FindObjectOfType<SOUL>().transform.position.x + 1f, -2.74f);
		}
	}
}
