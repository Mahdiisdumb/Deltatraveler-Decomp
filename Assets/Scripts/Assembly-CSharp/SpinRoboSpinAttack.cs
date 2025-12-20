using UnityEngine;

public class SpinRoboSpinAttack : ShieldAttackBase
{
	protected override void Awake()
	{
		base.Awake();
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(2);
		int num = ((Random.Range(0, 2) == 0) ? 1 : (-1));
		int num2 = ((Random.Range(0, 2) == 0) ? 1 : (-1));
		spawnRate = 4;
		bulletInfo = new object[40][]
		{
			new object[4]
			{
				0,
				10,
				Vector3.down * num2,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.left * num,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.up * num2,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.right * num,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.down * num2,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.left * num,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.up * num2,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.right * num,
				Vector3.zero
			},
			null,
			null,
			new object[4]
			{
				0,
				10,
				Vector3.down * num2,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.left * num,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.up * num2,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.right * num,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.down * num2,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.left * num,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.up * num2,
				Vector3.zero
			},
			null,
			new object[4]
			{
				0,
				10,
				Vector3.right * num,
				Vector3.zero
			},
			null
		};
	}

	protected override void OnDestroy()
	{
		base.OnDestroy();
		if ((bool)Object.FindObjectOfType<SpinRobo>())
		{
			Object.FindObjectOfType<SpinRobo>().Unspin();
			Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if ((bool)Object.FindObjectOfType<ExplosiveOak>() && Object.FindObjectOfType<ExplosiveOak>().IsGonnaExplode())
		{
			Object.FindObjectOfType<ExplosiveOak>().Explode();
		}
	}
}
