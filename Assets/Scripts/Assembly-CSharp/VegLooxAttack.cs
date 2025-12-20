using UnityEngine;

public class VegLooxAttack : AttackBase
{
	private int veggieFucker;

	private int vegSpawnRate = 10;

	private int loox;

	private int looxSpawnRate = 20;

	private bool spawnAGreenie;

	private int veggieSpawns;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 110;
		veggieFucker = Random.Range(0, 2);
		vegSpawnRate = ((veggieFucker == 1) ? 30 : 10);
		if (Object.FindObjectOfType<Vegetoid>().ExpectingGreensEaten())
		{
			spawnAGreenie = true;
		}
		loox = Random.Range(0, 2);
		looxSpawnRate = ((loox == 1) ? 22 : 20);
		if (Object.FindObjectOfType<Loox>().IsNextAttackHard())
		{
			looxSpawnRate = 8;
		}
		attackAllTargets = false;
	}

	public void OnDestroy()
	{
		Vegetoid[] array = Object.FindObjectsOfType<Vegetoid>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DisableEatingGreens();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames % vegSpawnRate == 1)
		{
			if (veggieFucker == 0)
			{
				VegCarrotBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegCarrotBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegCarrotBullet>();
				veggieSpawns++;
				if (veggieSpawns == 4 && spawnAGreenie)
				{
					component.ChangeType("GreenBullet");
				}
			}
			else
			{
				VegBounceBullet component2 = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegBounceBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegBounceBullet>();
				veggieSpawns++;
				if (veggieSpawns == 2 && spawnAGreenie)
				{
					component2.ChangeType("GreenBullet");
				}
			}
		}
		if (frames % looxSpawnRate == 2)
		{
			if (loox == 0)
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
