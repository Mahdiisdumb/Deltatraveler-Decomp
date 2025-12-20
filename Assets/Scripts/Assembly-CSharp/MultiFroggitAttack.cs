using UnityEngine;

public class MultiFroggitAttack : AttackBase
{
	private int[] froggits;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 100;
		bbSize = new Vector2(165f, 140f);
		froggits = new int[2]
		{
			Random.Range(0, 2),
			Random.Range(0, 2)
		};
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || frames % 25 != 1)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (froggits[i] == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/FlyBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
			}
			else
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/MustardSeedBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
			}
		}
	}
}
