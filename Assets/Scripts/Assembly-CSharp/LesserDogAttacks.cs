using UnityEngine;

public class LesserDogAttacks : AttackBase
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
		maxFrames = ((attack == 1) ? 180 : 160);
		spawnFromRight = Random.Range(0, 2) == 0;
		attackAllTargets = false;
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
		if (spearFrames >= maxSpearFrames)
		{
			switch (Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/LesserDogSpearBullet"), new Vector3(spawnFromRight ? 2.64f : (-2.64f), -2.074f), Quaternion.identity, base.transform).GetComponent<LesserDogSpearBullet>().GetBulletType())
			{
			case 1:
				maxSpearFrames = Random.Range(15, 25);
				break;
			case 0:
			case 2:
				maxSpearFrames = Random.Range(20, 30);
				break;
			}
			spearFrames = 0;
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(235f, 100f));
		if (attack == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/LesserDogDogBullet"), base.transform);
		}
	}
}
