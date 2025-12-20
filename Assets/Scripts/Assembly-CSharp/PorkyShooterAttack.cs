using UnityEngine;

public class PorkyShooterAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 180;
		bbSize = new Vector2(165f, 140f);
	}

	public override void StartAttack()
	{
		base.StartAttack();
		int num = 8;
		for (int i = 0; i < num; i++)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyShooterBullet"), base.transform).GetComponent<PorkyShooterBullet>().Activate(i, num);
		}
	}
}
