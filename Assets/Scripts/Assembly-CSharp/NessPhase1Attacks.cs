using System.Collections.Generic;
using UnityEngine;

public class NessPhase1Attacks : AttackBase
{
	private int nessAttack;

	private int paulaAttack;

	private List<PKFreezeBlock>[] blocks = new List<PKFreezeBlock>[6];

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 230;
		bbSize = new Vector2(165f, 140f);
		nessAttack = Random.Range(0, 3);
		paulaAttack = Random.Range(0, 4);
		if (paulaAttack == 3)
		{
			bbSize = new Vector3(181f, 140f);
		}
		else if (paulaAttack == 1)
		{
			for (int i = 0; i < 6; i++)
			{
				blocks[i] = new List<PKFreezeBlock>();
			}
		}
	}

	private void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<PaulaMeleeTarget>())
		{
			Object.FindObjectOfType<PaulaMeleeTarget>().SetToDestroy();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (nessAttack == 0 && frames % ((paulaAttack == 3) ? 15 : 20) == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/BaseballBatBullet"), base.transform);
		}
		else if (nessAttack == 1 && frames % ((paulaAttack == 3) ? 24 : 30) == 1)
		{
			for (int i = 0; i < 2; i++)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/RockinBullet"), base.transform).GetComponent<BulletBase>();
			}
		}
		else if (nessAttack == 2 && frames % ((paulaAttack == 3) ? 25 : 45) == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/LightningBullet"), new Vector3(Random.Range(-1.151f, 1.151f), -3.018f), Quaternion.identity, base.transform).transform.GetChild(1).GetComponent<AudioSource>().Play();
		}
		if (paulaAttack == 0 && frames % 60 == 15)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFireMainBullet"), Object.FindObjectOfType<SOUL>().transform.position, Quaternion.identity, base.transform);
		}
		else
		{
			if (paulaAttack != 1)
			{
				return;
			}
			for (int j = 0; j < 6; j++)
			{
				for (int k = 0; k < blocks[j].Count; k++)
				{
					if (blocks[j][k] == null)
					{
						blocks[j].RemoveAt(k);
						k--;
					}
				}
			}
			if (frames % ((nessAttack == 2) ? 20 : 30) != 1)
			{
				return;
			}
			List<int> list = new List<int>();
			for (int l = 0; l < 6; l++)
			{
				if (blocks[l].Count >= 4)
				{
					continue;
				}
				if (blocks[l].Count > 0)
				{
					if ((bool)blocks[l][blocks[l].Count - 1])
					{
						if (!blocks[l][blocks[l].Count - 1].IsForming())
						{
							list.Add(l);
						}
					}
					else
					{
						list.Add(l);
					}
				}
				else
				{
					list.Add(l);
				}
			}
			int num = list[Random.Range(0, list.Count)];
			float num2 = -1.31f + 0.52f * (float)num;
			if (num >= 3)
			{
				num2 += 0.02f;
			}
			PKFreezeBlock component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFreezeBlock"), new Vector3(num2, 0.13f), Quaternion.identity, base.transform).GetComponent<PKFreezeBlock>();
			if (blocks[num].Count > 0)
			{
				blocks[num][blocks[num].Count - 1].SetTopBlock(component);
				component.SetBottomBlock(blocks[num][blocks[num].Count - 1], blocks[num].Count);
			}
			blocks[num].Add(component);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (paulaAttack == 3)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PSIMagnetBullet"), base.transform).GetComponent<PSIMagnetBullet>().ToggleStrongMode();
			bb.StartMovement(new Vector3(181f, 180f));
		}
		else if (paulaAttack == 2)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaTarget")).GetComponent<PaulaMeleeTarget>().Activate(3);
		}
	}
}
