using UnityEngine;

public class TwoHeliAttack : AttackBase
{
	private GameObject heliPrefab;

	private bool doRight;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 210;
		bbSize = new Vector2(185f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 30 == 1)
		{
			PorkyHelicopterBullet component = Object.Instantiate(heliPrefab, base.transform).GetComponent<PorkyHelicopterBullet>();
			if (doRight)
			{
				component.Reverse();
			}
			doRight = !doRight;
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		heliPrefab = Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyHelicopterBullet");
	}
}
