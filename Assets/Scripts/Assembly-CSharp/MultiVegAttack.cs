using UnityEngine;

public class MultiVegAttack : AttackBase
{
	private int[] veggieFucker = new int[2];

	private int[] vegSpawnRate = new int[2];

	private bool spawnAGreenie;

	private int veggieSpawns;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 110;
		for (int i = 0; i < 2; i++)
		{
			veggieFucker[i] = Random.Range(0, 2);
			vegSpawnRate[i] = ((veggieFucker[i] == 1) ? 30 : 10);
		}
		Vegetoid[] array = Object.FindObjectsOfType<Vegetoid>();
		for (int j = 0; j < array.Length; j++)
		{
			if (array[j].ExpectingGreensEaten())
			{
				spawnAGreenie = true;
			}
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
		for (int i = 0; i < 2; i++)
		{
			if (frames % vegSpawnRate[i] != 1)
			{
				continue;
			}
			if (veggieFucker[i] == 0)
			{
				VegCarrotBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegCarrotBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegCarrotBullet>();
				if (spawnAGreenie && i == 0)
				{
					veggieSpawns++;
					if (veggieSpawns == 4)
					{
						component.ChangeType("GreenBullet");
					}
				}
				continue;
			}
			VegBounceBullet component2 = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegBounceBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegBounceBullet>();
			if (spawnAGreenie && i == 0)
			{
				veggieSpawns++;
				if (veggieSpawns == 2 && spawnAGreenie)
				{
					component2.ChangeType("GreenBullet");
				}
			}
		}
	}
}
