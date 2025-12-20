using UnityEngine;

public class PorkyHeliShooterAttack : AttackBase
{
	private GameObject heliPrefab;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 210;
		bbSize = new Vector2(185f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 60 == 1)
		{
			Object.Instantiate(heliPrefab, base.transform);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		heliPrefab = Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyHelicopterBullet");
		int num = 5;
		for (int i = 0; i < num; i++)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyShooterBullet"), base.transform).GetComponent<PorkyShooterBullet>().Activate(i, num);
		}
	}
}
