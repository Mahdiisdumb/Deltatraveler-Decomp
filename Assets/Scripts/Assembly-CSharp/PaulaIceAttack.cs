using System.Collections.Generic;
using UnityEngine;

public class PaulaIceAttack : AttackBase
{
	private List<PKFreezeBlock>[] blocks = new List<PKFreezeBlock>[6];

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 350;
		bbSize = new Vector2(165f, 140f);
		for (int i = 0; i < 6; i++)
		{
			blocks[i] = new List<PKFreezeBlock>();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || frames % 12 != 1)
		{
			return;
		}
		List<int> list = new List<int>();
		for (int i = 0; i < 6; i++)
		{
			if (blocks[i].Count >= 9)
			{
				continue;
			}
			if (blocks[i].Count > 0)
			{
				if ((bool)blocks[i][blocks[i].Count - 1])
				{
					if (!blocks[i][blocks[i].Count - 1].IsForming())
					{
						list.Add(i);
					}
				}
				else
				{
					list.Add(i);
				}
			}
			else
			{
				list.Add(i);
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

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector3(165f, 240f));
	}
}
