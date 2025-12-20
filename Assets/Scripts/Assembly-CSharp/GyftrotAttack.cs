using UnityEngine;

public class GyftrotAttack : AttackBase
{
	private int type;

	private int rage;

	private int presentType;

	private bool presentsActivated;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		rage = Object.FindObjectOfType<Gyftrot>().GetRage();
		type = Random.Range(0, 2);
		type = 1;
		maxFrames = 120;
		if (type == 0)
		{
			maxFrames += rage * 3;
		}
		if (type == 1)
		{
			int num = 6 - rage;
			if (num < 1)
			{
				num = 1;
			}
			presentType = ((Random.Range(0, num) == 0) ? 1 : 0);
		}
		attackAllTargets = false;
	}

	protected override void Update()
	{
		if (bb.IsPlaying() || !isStarted)
		{
			return;
		}
		base.Update();
		if (type == 0)
		{
			int num = ((rage >= 4) ? 4 : 5);
			int baseDamage = 7 + rage;
			if (frames % num == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/GyftrotPelletBullet"), new Vector3(Random.Range(-1.5f, 1.5f), 0.9f), Quaternion.identity, base.transform).GetComponent<GyftrotPelletBullet>().SetBaseDamage(baseDamage);
			}
		}
		else if (type == 1 && !presentsActivated)
		{
			presentsActivated = true;
			GyftrotPresentBullet[] array = Object.FindObjectsOfType<GyftrotPresentBullet>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].Activate();
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(185f, 190f));
		if (type != 1)
		{
			return;
		}
		int baseDamage = 7 + rage;
		int num = Random.Range(0, 3);
		int num2 = -1;
		if (rage >= 2 && rage < 4)
		{
			num2 = Random.Range(0, 3);
			if (num == num2)
			{
				num2 = (num + 1) % 3;
			}
		}
		else if (rage >= 4)
		{
			num2 = num;
			num = -1;
		}
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			int num3 = 0;
			int num4 = 0;
			if (num == i)
			{
				num3 = 1;
			}
			else if (num2 == i)
			{
				num3 = 2;
			}
			if (rage == 0)
			{
				num4 = num3;
			}
			else if (rage <= 3)
			{
				if (num == (i + 1) % 3)
				{
					num4 = 1;
				}
				else if (num2 == (i + 1) % 3)
				{
					num4 = 2;
				}
			}
			else if (!flag)
			{
				num4 = ((Random.Range(0, 3) == 0) ? 1 : 0);
				if (num4 == 1)
				{
					flag = true;
				}
				else if (i == 2)
				{
					num4 = 1;
				}
			}
			int num5 = -90 + 90 * i;
			GyftrotPresentBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/GyftrotPresentBullet"), new Vector3(Mathf.Lerp(-1.405f, 1.405f, (float)i / 2f), -2.396f), Quaternion.identity, base.transform).GetComponent<GyftrotPresentBullet>();
			component.SetBaseDamage(baseDamage);
			component.SetParameters(num3, num5, presentType == 1, num4);
		}
	}
}
