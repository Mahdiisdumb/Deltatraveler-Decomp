using System.Collections.Generic;
using UnityEngine;

public class MoleClawAttack : AttackBase
{
	private bool grabbed;

	private int grabFrames;

	private int times;

	private int maxTimes;

	private int target = -1;

	private bool krisWasAlive;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 180;
		bbSize = new Vector2(104f, 140f);
		List<int> list = new List<int>();
		for (int i = 0; i < 6; i++)
		{
			if (i >= 3 && !list.Contains(i - 3) && Util.GameManager().PartySlotFilled(i) && Util.GameManager().GetHP(i) > 0)
			{
				list.Add(i);
			}
			else if (i < 3 && Util.GameManager().GetHP(i) > 0)
			{
				list.Add(i);
			}
		}
		target = list[Random.Range(0, list.Count)];
		Util.FindObjectOfType<PartyPanels>().SetTargets(target == 0, target == 1, target == 2);
		if (Util.GameManager().GetHP(target) == 0)
		{
			target += 3;
		}
		int maxHP = Util.GameManager().GetMaxHP(target);
		maxTimes = maxHP / (Util.FindObjectOfType<MondoMole>().IsLectured() ? 6 : 5);
		Util.FindObjectOfType<MondoMole>().SetToBulletBoard(toBulletBoard: true);
	}

	public void OnDestroy()
	{
		Util.FindObjectOfType<SOUL>().SetInvFrames(30);
		if ((bool)Util.FindObjectOfType<MondoMole>())
		{
			Util.FindObjectOfType<MondoMole>().SetToBulletBoard(toBulletBoard: false);
		}
	}

	protected override void Update()
	{
		if (!grabbed)
		{
			base.Update();
			return;
		}
		grabFrames++;
		if (grabFrames >= 15)
		{
			int num = 10;
			if (maxTimes * num > 60)
			{
				num = 60 / maxTimes;
			}
			if (grabFrames % num == 0)
			{
				times++;
				Util.FindObjectOfType<BattleCamera>().HurtShake();
				Util.FindObjectOfType<SOUL>().Damage(1);
			}
			int num2 = Util.GameManager().GetHP(target);
			if (krisWasAlive)
			{
				num2 -= 20;
			}
			if (times >= maxTimes || num2 <= 0)
			{
				Util.FindObjectOfType<SOUL>().SetInvFrames(30);
				Util.FindObjectOfType<SOUL>().transform.parent = null;
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void Grab()
	{
		grabbed = true;
		Util.FindObjectOfType<SOUL>().SetInvFrames(0);
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bool hugMode = Random.Range(0, 2) == 1;
		for (int i = 0; i < 2; i++)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/MoleClawBullet"), base.transform).GetComponent<MoleClawBullet>().Activate(i == 1, hugMode, Util.FindObjectOfType<MondoMole>().GetDifficultyLevel());
		}
	}
}
