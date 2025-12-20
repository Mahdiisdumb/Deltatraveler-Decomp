using UnityEngine;

public class FroggitWhimsunAttack : AttackBase
{
	private int froggit;

	private int whimsun = -1;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 100;
		bbSize = new Vector2(165f, 140f);
		froggit = Random.Range(0, 2);
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames % 25 == 1)
		{
			if (froggit == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/FlyBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
			}
			else
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/MustardSeedBullet"), new Vector3(Random.Range(-1.47f, 1.47f), -0.3f), Quaternion.identity, base.transform);
			}
		}
		if (frames % 9 == 1 && whimsun == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/ButterflyFallBullet"), base.transform).transform.position = new Vector3(Object.FindObjectOfType<SOUL>().transform.position.x - 1f, -2.74f);
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/ButterflyFallBullet"), base.transform).transform.position = new Vector3(Object.FindObjectOfType<SOUL>().transform.position.x + 1f, -2.74f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		whimsun = Random.Range(0, 2);
		if (whimsun == 0)
		{
			float num = 10f;
			for (int i = 0; (float)i < num; i++)
			{
				ButterflySpinBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/ButterflySpinBullet"), base.transform).GetComponent<ButterflySpinBullet>();
				component.transform.position = new Vector3(0f, 1000f);
				component.SetValues(360f / num * (float)i, soulPos);
			}
		}
	}
}
