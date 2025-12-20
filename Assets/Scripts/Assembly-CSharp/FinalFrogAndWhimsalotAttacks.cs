using UnityEngine;

public class FinalFrogAndWhimsalotAttacks : AttackBase
{
	private int totalCount;

	private int frogCount;

	private int[] frogAttackOffset = new int[2];

	private bool frogBulletAttack;

	private int whimsunBulletType = -1;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 150;
		bbSize = new Vector2(165f, 140f);
		bool flag = false;
		FinalFroggit[] array = Object.FindObjectsOfType<FinalFroggit>();
		foreach (FinalFroggit finalFroggit in array)
		{
			if (!finalFroggit.IsDone())
			{
				frogAttackOffset[frogCount] = finalFroggit.GetAttackOffset();
				frogCount++;
				totalCount++;
				if (frogCount == 1)
				{
					flag = finalFroggit.GetAttackType() == 0;
				}
			}
		}
		if ((bool)Object.FindObjectOfType<Whimsalot>() && !Object.FindObjectOfType<Whimsalot>().IsDone())
		{
			totalCount++;
			if (Object.FindObjectOfType<Whimsalot>().IsPraying())
			{
				whimsunBulletType = 2;
			}
			else
			{
				whimsunBulletType = ((Random.Range(0, 2) == 1 && totalCount > 1) ? 1 : 0);
			}
		}
		frogBulletAttack = totalCount == 1 && frogCount == 1 && flag;
		if (whimsunBulletType == 2 && totalCount == 1)
		{
			maxFrames = 55;
		}
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames % (2 + 8 * totalCount) == 1 && !frogBulletAttack)
		{
			for (int i = 0; i < frogCount; i++)
			{
				BulletBase component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/FlyBulletHard"), new Vector3(Random.Range(-1.47f, 1.47f), -0.2f), Quaternion.identity, base.transform).GetComponent<BulletBase>();
				int num = component.GetBaseDamage() + frogAttackOffset[i];
				if (num <= 0)
				{
					num = 1;
				}
				component.SetBaseDamage(num);
			}
		}
		if (whimsunBulletType != 0 || frames % ((totalCount == 1) ? 22 : 40) != 1)
		{
			return;
		}
		for (int j = 0; j < 3; j++)
		{
			ButterflyHardBullet component2 = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/ButterflyHardBullet"), new Vector3(Object.FindObjectOfType<SOUL>().transform.position.x + (1.25f - 1.25f * (float)j), -3.45f), Quaternion.identity, base.transform).GetComponent<ButterflyHardBullet>();
			int baseDamage = component2.GetBaseDamage() + Object.FindObjectOfType<Whimsalot>().GetAttackOffset();
			component2.SetBaseDamage(baseDamage);
			if (totalCount > 1)
			{
				component2.SetSpeedFrames(15);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (frogBulletAttack)
		{
			BulletBase component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/FrogBulletHard"), new Vector3(0f, -2.6f), Quaternion.identity, base.transform).GetComponent<BulletBase>();
			int num = component.GetBaseDamage() + frogAttackOffset[0];
			if (num <= 0)
			{
				num = 1;
			}
			component.SetBaseDamage(num);
		}
		if (whimsunBulletType > 0)
		{
			ButterflySpinBulletOrigin butterflySpinBulletOrigin = new GameObject("Spin").AddComponent<ButterflySpinBulletOrigin>();
			butterflySpinBulletOrigin.transform.parent = base.transform;
			butterflySpinBulletOrigin.Begin(whimsunBulletType == 1, 360f / (float)((totalCount > 1) ? 90 : 69), whimsunBulletType == 2);
		}
	}
}
