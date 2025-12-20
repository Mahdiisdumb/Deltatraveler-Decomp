using UnityEngine;

public class AstigVegeAttacks : AttackBase
{
	private int totalCount;

	private int astigCount;

	private int parsCount;

	private int[] astigBulletType = new int[2];

	private bool[] astigAttackHard = new bool[2];

	private int[] parsBulletType = new int[2];

	private int snakesSpawned;

	private bool spawnGreenSnake;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 150;
		bbSize = new Vector2(165f, 140f);
		Astigmatism[] array = Object.FindObjectsOfType<Astigmatism>();
		foreach (Astigmatism astigmatism in array)
		{
			if (!astigmatism.IsDone())
			{
				astigBulletType[astigCount] = Random.Range(0, 2);
				astigAttackHard[astigCount] = astigmatism.IsNextAttackHard();
				if (astigAttackHard[astigCount] && astigmatism.GetSatisfactionLevel() < 100)
				{
					astigmatism.AddActPoints(100);
				}
				astigmatism.ResetAttackDifficulty();
				astigCount++;
				totalCount++;
			}
		}
		Parsnik[] array2 = Object.FindObjectsOfType<Parsnik>();
		foreach (Parsnik parsnik in array2)
		{
			if (!parsnik.IsDone())
			{
				parsBulletType[parsCount] = Random.Range(0, 2);
				parsCount++;
				totalCount++;
				if (parsnik.ExpectingGreensEaten())
				{
					spawnGreenSnake = true;
				}
			}
		}
		attackAllTargets = false;
	}

	private void OnDestroy()
	{
		Parsnik[] array = Object.FindObjectsOfType<Parsnik>();
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
		for (int i = 0; i < astigCount; i++)
		{
			int num = ((!astigAttackHard[i]) ? 1 : 2);
			if (astigBulletType[i] == 0 && frames % ((2 + 15 * totalCount) / num) == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/AstigSpinBullet"), new Vector3(4.16f, Random.Range(-2.2f, -0.75f)), Quaternion.identity, base.transform).GetComponent<AstigSpinBullet>().GenerateChildren();
			}
			else if (astigBulletType[i] == 1 && frames % ((6 + 12 * totalCount) / num) == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/AstigBounceBullet"), base.transform);
			}
		}
		for (int j = 0; j < parsCount; j++)
		{
			BulletBase bulletBase = null;
			if (parsBulletType[j] == 0 && frames % (4 * totalCount) == 1)
			{
				bulletBase = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegCarrotBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegCarrotBullet>();
			}
			else if (parsBulletType[j] == 1 && frames % (6 + 6 * totalCount) == 1)
			{
				bulletBase = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegBounceBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegBounceBullet>();
			}
			if ((bool)bulletBase && spawnGreenSnake)
			{
				int num2 = ((parsBulletType[j] == 0) ? 4 : 4);
				snakesSpawned++;
				if (snakesSpawned == num2)
				{
					bulletBase.ChangeType(3);
					spawnGreenSnake = false;
				}
			}
		}
	}
}
