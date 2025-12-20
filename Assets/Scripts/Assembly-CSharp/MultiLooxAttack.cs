using UnityEngine;

public class MultiLooxAttack : AttackBase
{
	private int[] loox = new int[2];

	private int[] looxSpawnRate = new int[2];

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 110;
		for (int i = 0; i < 2; i++)
		{
			loox[i] = Random.Range(0, 2);
			looxSpawnRate[i] = ((loox[i] == 1) ? 22 : 20);
		}
		Loox[] array = Object.FindObjectsOfType<Loox>();
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].IsNextAttackHard())
			{
				looxSpawnRate[0] = 8;
			}
		}
		attackAllTargets = false;
	}

	public void OnDestroy()
	{
		Loox[] array = Object.FindObjectsOfType<Loox>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].ResetAttackDifficulty();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		for (int i = 0; i < 2; i++)
		{
			if (frames % looxSpawnRate[i] == 2)
			{
				if (loox[i] == 0)
				{
					int num = ((Random.Range(0, 2) != 1) ? 1 : (-1));
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/LooxWormBullet"), base.transform).GetComponent<LooxWormBullet>().Activate(new Vector3(1.7f * (float)num, Random.Range(-2.68f, -0.62f)), new Vector3(-5f / 96f * (float)num, Random.Range(-1.65f, 1.65f) / 48f), 0);
				}
				else if (Object.FindObjectsOfType<LooxBounceBullet>().Length < 5)
				{
					int num2 = ((Random.Range(0, 2) != 1) ? 1 : (-1));
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/LooxBounceBullet"), new Vector3(1.7f * (float)num2, Random.Range(-2.68f, -0.62f)), Quaternion.identity, base.transform);
				}
			}
		}
	}
}
