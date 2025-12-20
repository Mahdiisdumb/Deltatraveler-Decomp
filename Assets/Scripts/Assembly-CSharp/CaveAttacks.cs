using UnityEngine;

public class CaveAttacks : AttackBase
{
	private int moleCount;

	private bool doWhackAMole;

	private int batCount;

	private int bearCount;

	private int totalCount;

	private bool moleIsRough;

	private bool moleAttackPhase;

	private WhackAMoleSpot[] moleSpots = new WhackAMoleSpot[9];

	private int[] nextMoleSpots = new int[3];

	private int[] batAttacks = new int[3];

	private int[] batSlowFactor = new int[3];

	private int batSwoopAttacks;

	private bool batSwoopFromLeft = true;

	private int bearAttack;

	private bool startHoneyAnger;

	protected override void Awake()
	{
		base.Awake();
		RoughMole[] array = Object.FindObjectsOfType<RoughMole>();
		foreach (RoughMole roughMole in array)
		{
			if (!roughMole.IsDone())
			{
				if (!moleIsRough)
				{
					moleIsRough = roughMole.PlayingRough();
				}
				moleCount++;
				totalCount++;
			}
		}
		MrBatty[] array2 = Object.FindObjectsOfType<MrBatty>();
		foreach (MrBatty mrBatty in array2)
		{
			if (!mrBatty.IsDone() && !mrBatty.IsDisabled())
			{
				batAttacks[batCount] = Random.Range(0, 2);
				if (batSwoopAttacks == 2)
				{
					batAttacks[batCount] = 0;
				}
				else if (batAttacks[batCount] == 1)
				{
					batSwoopAttacks++;
				}
				batSlowFactor[batCount] = mrBatty.GetSatisfactionLevel() / 25;
				batCount++;
				totalCount++;
			}
		}
		if ((bool)Object.FindObjectOfType<MightyBear>() && !Object.FindObjectOfType<MightyBear>().IsDone())
		{
			bearAttack = Random.Range(0, 2);
			bearCount++;
			totalCount++;
			if (bearAttack == 0 || totalCount > 1)
			{
				Object.FindObjectOfType<MightyBear>().SetToBulletBoard(true);
			}
			startHoneyAnger = Object.FindObjectOfType<MightyBear>().AreBeesPissed();
		}
		if (moleCount == totalCount)
		{
			doWhackAMole = Random.Range(0, 2) == 0;
		}
		if (doWhackAMole && totalCount != 0)
		{
			maxFrames = 200;
			bbSize = new Vector2(104f, 140f);
		}
		else
		{
			maxFrames = 150;
			bbSize = new Vector2(165f, 140f);
		}
		if (totalCount == 0)
		{
			maxFrames = 0;
		}
		attackAllTargets = false;
	}

	private void OnDestroy()
	{
		MrBatty[] array = Object.FindObjectsOfType<MrBatty>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].Reenable();
		}
		if ((bool)Object.FindObjectOfType<MightyBear>() && !Object.FindObjectOfType<MightyBear>().IsDone())
		{
			Object.FindObjectOfType<MightyBear>().SetToBulletBoard(false);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (doWhackAMole)
		{
			int num = 12 + 4 * moleCount;
			if (frames % num != num - 1)
			{
				return;
			}
			if (moleAttackPhase)
			{
				for (int i = 0; i < moleCount; i++)
				{
					moleSpots[nextMoleSpots[i]].Unlight();
					moleSpots[nextMoleSpots[i]].ActivateMole(i == 0);
				}
				moleAttackPhase = false;
				return;
			}
			for (int j = 0; j < moleCount; j++)
			{
				if (j == 0)
				{
					nextMoleSpots[j] = Random.Range(0, 9);
				}
				else
				{
					nextMoleSpots[j] = Random.Range(0, 9);
					bool flag = Random.Range(0, 2) == 1;
					if (nextMoleSpots[0] == nextMoleSpots[j])
					{
						nextMoleSpots[j] += ((!flag) ? 1 : (-1));
						if (nextMoleSpots[j] < 0)
						{
							nextMoleSpots[j] = 8;
						}
						else if (nextMoleSpots[j] > 8)
						{
							nextMoleSpots[j] = 0;
						}
					}
					if (j == 2)
					{
						if (nextMoleSpots[1] == nextMoleSpots[j])
						{
							nextMoleSpots[j] += ((!flag) ? 1 : (-1));
						}
						if (nextMoleSpots[j] < 0)
						{
							nextMoleSpots[j] = 8;
						}
						else if (nextMoleSpots[j] > 8)
						{
							nextMoleSpots[j] = 0;
						}
					}
				}
				moleSpots[nextMoleSpots[j]].LightUp(j == 0);
			}
			moleAttackPhase = true;
			return;
		}
		for (int k = 0; k < moleCount; k++)
		{
			int num2 = 5 + 5 * totalCount;
			if (k == 0 && moleIsRough)
			{
				num2 /= 2;
			}
			if (frames % num2 == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/MoleDigBullet"), base.transform);
			}
		}
		for (int l = 0; l < batCount; l++)
		{
			if (batAttacks[l] == 0)
			{
				int num3 = 7 + 3 * totalCount + 2 * batSlowFactor[l] + 2 * (batCount - 1);
				if (frames % num3 == 1)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/BatBoomerangBullet"), base.transform);
				}
				continue;
			}
			int num4 = 4 + 5 * totalCount + 2 * batSlowFactor[l] + 2 * (batCount - 1);
			if (totalCount == 3)
			{
				num4 += 2;
			}
			if (frames % num4 == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/BatSwoopBullet"), base.transform).GetComponent<BatSwoopBullet>().Activate(batSwoopFromLeft);
				batSwoopFromLeft = !batSwoopFromLeft;
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (doWhackAMole)
		{
			bb.StartMovement(new Vector2(104f, 104f));
			for (int i = 0; i < 9; i++)
			{
				WhackAMoleSpot component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/WhackAMoleSpot"), base.transform).GetComponent<WhackAMoleSpot>();
				Vector3 position = new Vector3
				{
					x = -0.667f + (float)(i % 3) * 0.6585f,
					y = -1.388f - (float)(i / 3) * 0.65f
				};
				component.transform.position = position;
				moleSpots[i] = component;
			}
		}
		else
		{
			if (bearCount <= 0)
			{
				return;
			}
			if (totalCount > 1 || bearAttack == 0)
			{
				for (int j = 0; j < 2; j++)
				{
					Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/BearClawBullet"), base.transform).GetComponent<BearClawBullet>().Activate(j == 1, bearAttack == 1, 10 - (totalCount - 1) * 4);
				}
			}
			else
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/HoneySetup"), base.transform);
			}
		}
	}
}
