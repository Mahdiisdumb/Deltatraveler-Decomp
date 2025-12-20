using UnityEngine;

public class PorkyGasAttack : AttackBase
{
	private GameObject gasPrefab;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 250;
		bbSize = new Vector2(165f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 45 == 1 && frames < 225)
		{
			Object.Instantiate(gasPrefab, base.transform);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		gasPrefab = Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyGasBullet");
	}
}
