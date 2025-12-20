using UnityEngine;

public class FightTargetBarQuad : FightTargetBar
{
	private FightTargetBarMini[] miniBars;

	private int barIndex;

	private int startFrames;

	private bool forcingFrames;

	private bool missRegister;

	private float[] successRate = new float[4];

	private int crits;

	private int farSpaces;

	private void Awake()
	{
		miniBars = new FightTargetBarMini[4];
		for (int i = 0; i < 4; i++)
		{
			miniBars[i] = base.transform.GetChild(i).GetComponent<FightTargetBarMini>();
		}
	}

	protected override void Update()
	{
		_ = activated;
		if (!ending || endFrames >= 12)
		{
			return;
		}
		endFrames++;
		if (enemy.GetPredictedHP() <= 0)
		{
			int num = partyMember;
			if (num > 2)
			{
				num = 0;
			}
			enemy = Util.FindObjectOfType<FightTarget>().GetEnemies()[num];
		}
		if (endFrames == 12 && enemyAlive && !miss)
		{
			MonoBehaviour.print("SUCCESS QUAD: " + GetSuccessRate());
			enemy.Hit(partyMember, GetSuccessRate(), playSound: true);
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
		for (int i = 0; i < 4; i++)
		{
			miniBars[i].Activate(startFrames);
			int num = Random.Range(3, 6) * 2;
			if (num == 8 && farSpaces < 2)
			{
				farSpaces++;
			}
			else
			{
				num = 6;
			}
			if (Util.GameManager().GetWeapon(partyMember) == 32)
			{
				num = 4;
			}
			startFrames -= num;
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
		if (barIndex == 4 && !missRegister)
		{
			activated = false;
			ending = true;
			if (enemyAlive)
			{
				enemy.SetPredictedDamage(partyMember, GetSuccessRate());
			}
			if (GetSuccessRate() >= 20f)
			{
				Util.FindObjectOfType<TPBar>().AddTP(5);
				GetComponent<AudioSource>().Play();
			}
		}
		return barIndex == 4;
	}

	public void RegisterMiss()
	{
		missRegister = true;
		PushZ(enemyAlive);
		successRate[barIndex - 1] = 0f;
		missRegister = false;
		if (barIndex == 4)
		{
			activated = false;
			ending = true;
			if ((bool)enemy && IsACompleteMiss())
			{
				enemy.Hit(partyMember, 0f, playSound: false);
				miss = true;
			}
			else if ((bool)enemy)
			{
				int num = ((partyMember > 2) ? 3 : partyMember);
				Util.FindObjectOfType<FightTarget>().PlayHitAnimation(enemy, partyMember, GetSuccessRate(), num);
			}
		}
	}

	public override int GetCurFrames()
	{
		if (barIndex < 4)
		{
			return miniBars[barIndex].GetCurFrames();
		}
		return 0;
	}

	public override int GetLastFrames()
	{
		return miniBars[3].GetCurFrames();
	}

	public override float GetSuccessRate()
	{
		if (barIndex == 4)
		{
			float num = 0f;
			for (int i = 0; i < 4; i++)
			{
				num += miniBars[i].GetSuccessRate();
			}
			num /= 4f;
			if (crits < 2 || Util.GameManager().GetWeapon(partyMember) == 32)
			{
				num *= (float)(crits + 16) / 16f;
			}
			else if (crits < 4)
			{
				num *= (float)(crits + 8) / 8f;
			}
			else if (crits == 4)
			{
				num = 35f;
			}
			return num;
		}
		return base.GetSuccessRate();
	}

	public override bool CanPushZ()
	{
		if (barIndex == 4)
		{
			return false;
		}
		return miniBars[barIndex].CanPushZ();
	}

	public bool IsACompleteMiss()
	{
		for (int i = 0; i < 4; i++)
		{
			if (!miniBars[i].IsMiss())
			{
				return false;
			}
		}
		return true;
	}
}
