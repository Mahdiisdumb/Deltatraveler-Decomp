using UnityEngine;

public class GreaterDogAttacks : AttackBase
{
	private int attack;

	private int spearFrames;

	private int maxSpearFrames;

	private bool spawnFromRight;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		attack = Random.Range(0, 2);
		maxFrames = 200;
		spawnFromRight = Random.Range(0, 2) == 0;
		attackAllTargets = attack == 1;
	}

	protected override void Update()
	{
		if (bb.IsPlaying() || !isStarted)
		{
			return;
		}
		base.Update();
		if (attack != 1)
		{
			return;
		}
		spearFrames++;
		if (spearFrames < maxSpearFrames)
		{
			return;
		}
		float num = Random.Range(-0.9f, 0.9f) - 1.33f;
		float num2 = Random.Range(1f / 3f, 0.5f);
		bool flag = Random.Range(0, 4) == 0;
		int startFrames = Random.Range(0, 40);
		int num3 = 0;
		if (Random.Range(0, 4) == 0)
		{
			num3 = Random.Range(1, 3);
			num2 = 0f;
			flag = false;
			num2 *= 1.5f;
		}
		for (int i = 0; i < 2; i++)
		{
			int num4 = ((i == 0) ? 1 : (-1));
			GreaterDogSpearBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/GreaterDogSpearBullet"), new Vector3(spawnFromRight ? 3.35f : (-3.35f), num - num2 * (float)num4), Quaternion.identity, base.transform).GetComponent<GreaterDogSpearBullet>();
			component.transform.localScale = new Vector3(1f, num4, 1f);
			component.ChangeType(num3);
			if (flag)
			{
				component.DoWave(num, startFrames);
			}
		}
		switch (num3)
		{
		case 1:
			maxSpearFrames = Random.Range(12, 20);
			break;
		case 0:
		case 2:
			maxSpearFrames = Random.Range(15, 25);
			break;
		}
		spearFrames = 0;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (attack == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/GreaterDogDogBullet"), base.transform);
			bb.StartMovement(new Vector2(235f, 140f));
		}
		else
		{
			bb.StartMovement(new Vector2(300f, 180f));
		}
	}
}
