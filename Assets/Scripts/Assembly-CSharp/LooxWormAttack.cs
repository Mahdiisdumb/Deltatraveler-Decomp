using UnityEngine;

public class LooxWormAttack : AttackBase
{
	private int spawnRate = 15;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 110;
		if (Object.FindObjectOfType<Loox>().IsNextAttackHard())
		{
			spawnRate = 6;
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
		if (isStarted && frames % spawnRate == 2)
		{
			int num = ((Random.Range(0, 2) != 1) ? 1 : (-1));
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/LooxWormBullet"), base.transform).GetComponent<LooxWormBullet>().Activate(new Vector3(1.7f * (float)num, Random.Range(-2.68f, -0.62f)), new Vector3(-5f / 96f * (float)num, Random.Range(-1.65f, 1.65f) / 48f), 0);
		}
	}
}
