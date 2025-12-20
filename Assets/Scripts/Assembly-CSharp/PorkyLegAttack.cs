using UnityEngine;

public class PorkyLegAttack : AttackBase
{
	private GameObject legPrefab;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 180;
		bbSize = new Vector2(165f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 45 == 1)
		{
			for (int i = 0; i < 4; i++)
			{
				Object.Instantiate(legPrefab, base.transform).GetComponent<PorkyLegBullet>().Activate(i, i == 0);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		legPrefab = Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyLegBullet");
	}
}
