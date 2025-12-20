using System.Collections.Generic;
using UnityEngine;

public class FightTargetBarKatana : FightTargetBar
{
	private FightTargetBarMini[] miniBars;

	private int barIndex;

	private int startFrames;

	private bool forcingFrames;

	private bool missRegister;

	private float[] successRate = new float[2];

	private int crits;

	private int farSpaces;

	private List<EnemyBase> extraHits = new List<EnemyBase>();

	private void Awake()
	{
		miniBars = new FightTargetBarMini[2];
		for (int i = 0; i < 2; i++)
		{
			miniBars[i] = base.transform.GetChild(i).GetComponent<FightTargetBarMini>();
		}
	}

	protected override void Update()
	{
		bool activated2 = activated;
		if (!ending || endFrames >= 15)
		{
			return;
		}
		endFrames++;
		if (enemy.GetPredictedHP() <= 0 && endFrames <= 12)
		{
			int num = partyMember;
			if (num > 2)
			{
				num = 0;
			}
			enemy = Object.FindObjectOfType<FightTarget>().GetEnemies()[num];
		}
		if (endFrames == 12 && enemyAlive && !miss)
		{
			MonoBehaviour.print("SUCCESS QUAD: " + GetSuccessRate());
			enemy.Hit(partyMember, GetSuccessRate(), true);
		}
		if (endFrames < 13)
		{
			return;
		}
		for (int i = 0; i < extraHits.Count; i++)
		{
			if (endFrames == 13 + i && extraHits[i] != enemy && extraHits[i].GetPredictedHP() > 0)
			{
				extraHits[i].Hit(partyMember, 10f, true);
			}
		}
	}

	public override void Activate()
	{
		disabled = false;
		activated = true;
		if (!forcingFrames)
		{
			startFrames = Random.Range(1, 4) * 4;
			ActivateMiniBars();
		}
	}

	public override void Activate(int forceFrames)
	{
		forcingFrames = true;
		Activate();
		startFrames = forceFrames;
		ActivateMiniBars();
	}

	public override void AssignValues(EnemyBase enemy, int partyMember)
	{
		base.AssignValues(enemy, partyMember);
		FightTargetBarMini[] array = miniBars;
		for (int i = 0; i < array.Length; i++)
		{
			array[i].AssignValues(enemy, partyMember);
		}
	}

	private void ActivateMiniBars()
	{
		for (int i = 0; i < 2; i++)
		{
			miniBars[i].Activate(startFrames);
			miniBars[i].SetMultiplier(0.75f);
			startFrames -= 12;
		}
	}

	public override bool PushZ(bool enemyAlive)
	{
		miniBars[barIndex].PushZ(enemyAlive);
		successRate[barIndex] = miniBars[barIndex].GetSuccessRate();
		if (successRate[barIndex] == 20f)
		{
			crits++;
		}
		barIndex++;
		base.enemyAlive = enemyAlive;
		if (barIndex == 2 && !missRegister)
		{
			activated = false;
			ending = true;
			if (enemyAlive)
			{
				enemy.SetPredictedDamage(partyMember, GetSuccessRate());
			}
			if (GetSuccessRate() >= 20f)
			{
				Object.FindObjectOfType<TPBar>().AddTP(5);
				GetComponent<AudioSource>().Play();
				EnemyBase[] array = Object.FindObjectsOfType<EnemyBase>();
				foreach (EnemyBase enemyBase in array)
				{
					if (enemyBase != enemy && !enemyBase.IsDone() && enemyBase.GetPredictedHP() > 0)
					{
						extraHits.Add(enemyBase);
					}
				}
			}
		}
		return barIndex == 2;
	}

	public void RegisterMiss()
	{
		missRegister = true;
		PushZ(enemyAlive);
		successRate[barIndex - 1] = 0f;
		missRegister = false;
		if (barIndex == 2)
		{
			activated = false;
			ending = true;
			if ((bool)enemy && IsACompleteMiss())
			{
				enemy.Hit(partyMember, 0f, false);
				miss = true;
			}
			else if ((bool)enemy)
			{
				int num = ((partyMember > 2) ? 1 : partyMember);
				Object.FindObjectOfType<FightTarget>().PlayHitAnimation(enemy, partyMember, GetSuccessRate(), num);
			}
		}
	}

	public override int GetCurFrames()
	{
		if (barIndex < 2)
		{
			return miniBars[barIndex].GetCurFrames();
		}
		return 0;
	}

	public override int GetLastFrames()
	{
		return miniBars[1].GetCurFrames();
	}

	public override float GetSuccessRate()
	{
		if (barIndex == 2)
		{
			float num = 0f;
			for (int i = 0; i < 2; i++)
			{
				num = ((miniBars[i].GetSuccessRate() != 20f) ? (num + miniBars[i].GetSuccessRate() * 0.8f) : (num + 20f));
			}
			return num / 2f;
		}
		return base.GetSuccessRate();
	}

	public override bool CanPushZ()
	{
		if (barIndex == 2)
		{
			return false;
		}
		return miniBars[barIndex].CanPushZ();
	}

	public bool MissedOneBar()
	{
		int num = 0;
		for (int i = 0; i < 2; i++)
		{
			if (!miniBars[i].IsMiss())
			{
				num++;
			}
		}
		return num == 1;
	}

	public bool IsACompleteMiss()
	{
		for (int i = 0; i < 2; i++)
		{
			if (!miniBars[i].IsMiss())
			{
				return false;
			}
		}
		return true;
	}
}
