using UnityEngine;

public class PaulaFireballAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 300;
		bbSize = new Vector2(165f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 12 == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/FireballPanBullet"), base.transform);
		}
	}
}
