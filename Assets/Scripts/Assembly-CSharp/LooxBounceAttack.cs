using UnityEngine;

public class LooxBounceAttack : AttackBase
{
	private int spawnRate = 15;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 110;
		if (Object.FindObjectOfType<Loox>().IsNextAttackHard())
		{
			spawnRate = 5;
		}
		attackAllTargets = false;
	}

	public void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<Loox>())
		{
			Object.FindObjectOfType<Loox>().ResetAttackDifficulty();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % spawnRate == 1 && Object.FindObjectsOfType<LooxBounceBullet>().Length < 5)
		{
			int num = ((Random.Range(0, 2) != 1) ? 1 : (-1));
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/LooxBounceBullet"), new Vector3(1.7f * (float)num, Random.Range(-2.68f, -0.62f)), Quaternion.identity, base.transform);
		}
	}
}
