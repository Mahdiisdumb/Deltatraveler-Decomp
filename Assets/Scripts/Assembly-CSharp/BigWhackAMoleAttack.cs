using System.Collections.Generic;
using UnityEngine;

public class BigWhackAMoleAttack : AttackBase
{
	private WhackAMoleSpot[] moleSpots = new WhackAMoleSpot[9];

	private List<int> orderedSpots = new List<int>();

	private int whackAMoleSpots = 3;

	private int currentSpot;

	private bool execute;

	private int spawnRate;

	protected override void Awake()
	{
		base.Awake();
		whackAMoleSpots = Util.FindObjectOfType<MondoMole>().GetMaxWhackAMoleSpots();
		bbSize = new Vector2(104f, 140f);
		maxFrames = 20;
		spawnRate = 20 - Mathf.RoundToInt((float)Util.FindObjectOfType<MondoMole>().GetDifficultyLevel() * 1.5f) - (whackAMoleSpots - 2) / 2;
		for (int i = 0; i < whackAMoleSpots; i++)
		{
			orderedSpots.Add(Random.Range(0, 9));
			maxFrames += spawnRate * 2;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || frames < 20 || (frames - 20) % spawnRate != 0 || currentSpot >= whackAMoleSpots)
		{
			return;
		}
		if (!execute)
		{
			if (currentSpot - 1 >= 0)
			{
				moleSpots[orderedSpots[currentSpot - 1]].Unlight();
			}
			moleSpots[orderedSpots[currentSpot]].LightUp(sound: true);
			currentSpot++;
			if (currentSpot >= whackAMoleSpots)
			{
				currentSpot = 0;
				execute = true;
			}
		}
		else
		{
			if (currentSpot == 0)
			{
				moleSpots[orderedSpots[whackAMoleSpots - 1]].Unlight();
			}
			moleSpots[orderedSpots[currentSpot]].ActivateMole(sound: true, spawnRate <= 12);
			currentSpot++;
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
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
}
