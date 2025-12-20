using UnityEngine;

public class ButterflySpinAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 95;
		bbSize = new Vector2(165f, 140f);
		attackAllTargets = false;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		float num = 10f;
		for (int i = 0; (float)i < num; i++)
		{
			ButterflySpinBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/ButterflySpinBullet"), base.transform).GetComponent<ButterflySpinBullet>();
			component.transform.position = new Vector3(0f, 1000f);
			component.SetValues(360f / num * (float)i, soulPos);
		}
	}
}
