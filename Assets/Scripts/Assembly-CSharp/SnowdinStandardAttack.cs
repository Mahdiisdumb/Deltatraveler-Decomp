using UnityEngine;

public class SnowdinStandardAttack : AttackBase
{
	private int enemyCount;

	private int snowdrakeCount;

	private int[] snowdrakeAttacks = new int[3];

	private int icecapCount;

	private int[] icecapAttacks = new int[3];

	private bool[] icecapPissed = new bool[3];

	private bool createSineWave;

	private int sineWaveID;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 200;
		attackAllTargets = false;
		Snowdrake[] array = Object.FindObjectsOfType<Snowdrake>();
		for (int i = 0; i < array.Length; i++)
		{
			if (!array[i].IsDone())
			{
				snowdrakeAttacks[snowdrakeCount] = Random.Range(0, 2);
				enemyCount++;
				snowdrakeCount++;
			}
		}
		IceCap[] array2 = Object.FindObjectsOfType<IceCap>();
		foreach (IceCap iceCap in array2)
		{
			if (!iceCap.IsDone() && !iceCap.IsIce())
			{
				if (!createSineWave)
				{
					icecapAttacks[icecapCount] = Random.Range(0, 2);
					createSineWave = icecapAttacks[icecapCount] == 0;
					sineWaveID = icecapCount;
				}
				else
				{
					icecapAttacks[icecapCount] = 1;
				}
				icecapPissed[icecapCount] = iceCap.IsPissed();
				enemyCount++;
				icecapCount++;
			}
		}
		if (icecapCount == 1 && snowdrakeCount == 0)
		{
			maxFrames = (createSineWave ? 100 : 150);
			bbSize = new Vector2(165f, 140f);
		}
		else if (icecapCount == 0 && snowdrakeCount == 0)
		{
			maxFrames = 0;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (snowdrakeCount > 0)
		{
			for (int i = 0; i < snowdrakeCount; i++)
			{
				bool flag = false;
				if (snowdrakeAttacks[i] == 0)
				{
					flag = frames % (70 + 30 * (enemyCount - 1)) == 1;
				}
				else if (snowdrakeAttacks[i] == 1)
				{
					flag = frames % (7 + 4 * (enemyCount - 1)) == 1;
				}
				if (flag)
				{
					Vector3 position = Vector3.zero;
					float num = 0f;
					bool flag2 = Random.Range(0, 2) == 0;
					bool flag3 = Random.Range(0, 2) == 0;
					if (flag2)
					{
						num = (flag3 ? 180 : 0);
						position = new Vector3(flag3 ? (-2.2f) : 2.2f, Random.Range(-3.2f, 0.9f));
					}
					else
					{
						num = (flag3 ? (-90) : 90);
						position = new Vector3(Random.Range(-2f, 2f), flag3 ? (-3.4f) : 1.1f);
					}
					if (snowdrakeAttacks[i] == 0)
					{
						SnowdrakeHomingBlade component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SnowdrakeHomingBlade"), base.transform).GetComponent<SnowdrakeHomingBlade>();
						component.transform.position = position;
						component.ActivateHead(num, (enemyCount == 1) ? 7 : (8 - enemyCount * 2));
					}
					else if (snowdrakeAttacks[i] == 1)
					{
						Vector2 zero = Vector2.zero;
						zero = ((!flag2) ? (flag3 ? Vector2.up : Vector2.down) : (flag3 ? Vector2.right : Vector2.left));
						SnowdrakeSpinBullet component2 = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/SnowdrakeSpinBullet"), base.transform).GetComponent<SnowdrakeSpinBullet>();
						component2.transform.position = position;
						component2.Activate(Random.Range(2f, 3f), zero);
					}
				}
			}
		}
		if (icecapCount <= 0)
		{
			return;
		}
		for (int j = 0; j < icecapCount; j++)
		{
			if (frames % ((36 + 15 * (enemyCount - 1)) / ((!icecapPissed[j]) ? 1 : 2)) == 1 && icecapAttacks[j] == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/IceCapPipe"), new Vector3(Random.Range(-1.61f, 1.61f), -3.52f), Quaternion.identity, base.transform);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (snowdrakeCount > 0 || icecapCount > 2)
		{
			bb.StartMovement(new Vector2(185f, 190f));
		}
		if (createSineWave)
		{
			IceCapSineWave component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/IceCapSineWave"), base.transform).GetComponent<IceCapSineWave>();
			if (icecapPissed[sineWaveID])
			{
				component.Activate(Random.Range(-2.1f, -1.5f), 130 + (enemyCount - 1) * 15, 6f, 70 - (enemyCount - 1) * 20, 8);
			}
			else
			{
				component.Activate(Random.Range(-2.1f, -1.5f), 140 + (enemyCount - 1) * 20, 6f, 60 - (enemyCount - 1) * 20, 10);
			}
		}
	}
}
