using UnityEngine;

public class PorkyHelicopterAttack : AttackBase
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
		if (isStarted && frames % 40 == 1)
		{
			Object.Instantiate(heliPrefab, base.transform);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		heliPrefab = Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyHelicopterBullet");
	}
}
