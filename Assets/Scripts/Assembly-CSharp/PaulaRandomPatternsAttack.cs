using System.Collections.Generic;
using UnityEngine;

public class PaulaRandomPatternsAttack : AttackBase
{
	private int[] patterns = new int[2];

	private bool ice;

	private bool starstorm;

	private bool spawnMelee;

	private List<PKFreezeBlock>[] blocks = new List<PKFreezeBlock>[6];

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 300;
		bbSize = new Vector2(165f, 140f);
		bool flag = false;
		for (int i = 0; i < 2; i++)
		{
			patterns[i] = Random.Range(0, 6);
			if (!flag)
			{
				flag = true;
			}
			else if (patterns[0] == patterns[1])
			{
				patterns[1] += ((Random.Range(0, 2) != 0) ? 1 : (-1));
				if (patterns[1] > 5)
				{
					patterns[1] = 0;
				}
				else if (patterns[1] < 0)
				{
					patterns[1] = 5;
				}
			}
			if (patterns[i] == 1)
			{
				ice = true;
			}
			if (patterns[i] == 5 || patterns[i] == 4)
			{
				starstorm = true;
			}
			if (patterns[i] == 2)
			{
				spawnMelee = true;
			}
		}
		if (starstorm && !ice)
		{
			bbSize = new Vector2(320f, 140f);
		}
		if (ice)
		{
			for (int j = 0; j < 6; j++)
			{
				blocks[j] = new List<PKFreezeBlock>();
			}
		}
	}

	private void OnDestroy()
	{
		if ((bool)Util.FindObjectOfType<PaulaMeleeTarget>())
		{
			Util.FindObjectOfType<PaulaMeleeTarget>().SetToDestroy();
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
			if (patterns[i] == 0 && frames % (spawnMelee ? 50 : 35) == 1)
			{
				Vector3 position = new Vector3(Random.Range(-1.32f, 1.32f) * ((bbSize.x == 320f) ? 2.2f : 1f), Random.Range(-2.83f, ice ? 0.48f : (-0.48f)));
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFireMainBullet"), position, Quaternion.Euler(0f, 0f, Random.Range(-60, 60)), base.transform).GetComponent<PKFireMainBullet>().MisalignFire();
			}
			else if (patterns[i] == 1 && frames % (spawnMelee ? 26 : 18) == 1)
			{
				List<int> list = new List<int>();
				for (int j = 0; j < 6; j++)
				{
					if (blocks[j].Count >= 9)
					{
						continue;
					}
					if (blocks[j].Count > 0)
					{
						if ((bool)blocks[j][blocks[j].Count - 1])
						{
							if (!blocks[j][blocks[j].Count - 1].IsForming())
							{
								list.Add(j);
							}
						}
						else
						{
							list.Add(j);
						}
					}
					else
					{
						list.Add(j);
					}
				}
				int num = list[Random.Range(0, list.Count)];
				float num2 = -1.31f + 0.52f * (float)num;
				if (num >= 3)
				{
					num2 += 0.02f;
				}
				PKFreezeBlock component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFreezeBlock"), new Vector3(num2, 2.25f), Quaternion.identity, base.transform).GetComponent<PKFreezeBlock>();
				if (blocks[num].Count > 0)
				{
					blocks[num][blocks[num].Count - 1].SetTopBlock(component);
					component.SetBottomBlock(blocks[num][blocks[num].Count - 1], blocks[num].Count);
				}
				blocks[num].Add(component);
			}
			else if (patterns[i] == 3 && frames % (spawnMelee ? 30 : 20) == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/FireballPanBullet"), base.transform);
			}
			else if (patterns[i] == 4 && frames % ((spawnMelee || ice) ? 40 : 30) == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(Random.Range(-1.151f, 1.151f) * ((bbSize.x == 320f) ? 2.2f : 1f), -3.018f), Quaternion.identity, base.transform).transform.GetChild(1).GetComponent<AudioSource>().Play();
			}
			else if (patterns[i] == 5 && frames % (spawnMelee ? 32 : 16) == 1 && frames <= 240)
			{
				BigStar component2 = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/BigStar"), base.transform).GetComponent<BigStar>();
				if (frames % 64 == 1)
				{
					component2.GetComponents<AudioSource>()[0].Play();
				}
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (ice)
		{
			bb.StartMovement(new Vector3(165f, 240f));
		}
		if (spawnMelee)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaTarget")).GetComponent<PaulaMeleeTarget>().Activate(3, hard: true);
		}
	}

	public bool UsingIce()
	{
		return ice;
	}
}
