using System.Collections.Generic;
using UnityEngine;

public class TallBlasterPlatforms : AttackBase
{
	private Transform[] platforms = new Transform[2];

	private bool[] platformGoingRight = new bool[2] { true, false };

	private int[] blasterCounts = new int[3];

	private int lastBlaster;

	private int spawnRate = 30;

	private GameObject blasterPrefab;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 285;
		bbSize = new Vector2(185f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(1);
		blasterPrefab = Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster");
		spawnRate = (Util.FindObjectOfType<KarmaHandler>() ? 30 : 35);
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
			if ((platforms[i].position.x < -1.33f && !platformGoingRight[i]) || (platforms[i].position.x > 1.33f && platformGoingRight[i]))
			{
				platformGoingRight[i] = !platformGoingRight[i];
			}
			platforms[i].position += new Vector3((float)(platformGoingRight[i] ? 2 : (-2)) / 48f, 0f);
		}
		if (frames % spawnRate == 1 && maxFrames - frames >= 30)
		{
			int num = ((frames % (spawnRate * 2) / 2 != 0) ? 1 : (-1));
			int heightIndex = GetHeightIndex();
			float[] array = new float[3] { -2.06f, 0.6f, 2.82f };
			Object.Instantiate(blasterPrefab, new Vector3(9.1f * (float)num, array[heightIndex]), Quaternion.Euler(0f, 0f, 90 * num)).GetComponent<GasterBlaster>().Activate(3, 3, -90 * num, new Vector2(4.47f * (float)num, array[heightIndex]), 15, inSpearAttack: false, 6);
		}
	}

	private int GetHeightIndex()
	{
		int num = -1;
		for (int i = 0; i < 3; i++)
		{
			if (num == -1 || blasterCounts[i] < num)
			{
				num = blasterCounts[i];
			}
		}
		List<int> list = new List<int>();
		for (int j = 0; j < 3; j++)
		{
			if (blasterCounts[j] < num + 2)
			{
				list.Add(j);
			}
		}
		int num2 = Random.Range(0, list.Count);
		if (list[num2] == lastBlaster)
		{
			num2 += ((Random.Range(0, 2) != 0) ? 1 : (-1));
			if (num2 >= list.Count)
			{
				num2 = 0;
			}
			else if (num2 <= 0)
			{
				num2 = list.Count - 1;
			}
		}
		int num3 = list[num2];
		lastBlaster = blasterCounts[num3];
		blasterCounts[num3]++;
		return num3;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Transform transform = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/TallBlasterPlatforms"), base.transform).transform;
		bb.StartMovement(new Vector2(185f, 300f));
		for (int i = 0; i < 2; i++)
		{
			platforms[i] = transform.GetChild(i);
		}
	}
}
