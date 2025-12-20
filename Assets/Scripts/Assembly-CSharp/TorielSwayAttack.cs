using System.Collections.Generic;
using UnityEngine;

public class TorielSwayAttack : TorielAttackBase
{
	private GameObject flamePrefab;

	private List<Transform> leftFlames = new List<Transform>();

	private List<Transform> rightFlames = new List<Transform>();

	private List<Transform> sideFlames = new List<Transform>();

	private List<float> moveRates = new List<float>();

	private float momentum;

	private bool momentumDecrease;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 160;
		bbSize = new Vector2(185f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || talking)
		{
			return;
		}
		int num = ((!momentumDecrease) ? 1 : (-1));
		momentum += 0.005f * (float)num;
		if ((momentum > 0.09f && !momentumDecrease) || (momentum < -0.09f && momentumDecrease))
		{
			momentumDecrease = !momentumDecrease;
		}
		if (frames % 5 == 0)
		{
			for (int i = 0; i < 3; i++)
			{
				Transform transform = Object.Instantiate(flamePrefab, new Vector3(0f, -0.3f), Quaternion.identity, base.transform).transform;
				transform.GetComponent<FlameBullet>().SetBaseDamage(Object.FindObjectOfType<PartyPanels>().GetTargettedMembers()[0] ? 2 : 5);
				if (i == 0)
				{
					if (Random.Range(0, 2) == 1)
					{
						leftFlames.Add(transform);
					}
					else
					{
						rightFlames.Add(transform);
					}
				}
				else
				{
					transform.transform.position = new Vector3((i == 1) ? 1.64f : (-1.64f), -2.15f);
					sideFlames.Add(transform);
					moveRates.Add(Random.Range(0.5f, 4f) / 48f * (float)((Random.Range(0, 2) != 0) ? 1 : (-1)));
				}
			}
		}
		foreach (Transform leftFlame in leftFlames)
		{
			if (leftFlame != null)
			{
				leftFlame.position += new Vector3(momentum, -0.0625f);
				if (leftFlame.position.y <= -2.867f)
				{
					leftFlame.position = new Vector3(leftFlame.position.x, -2.867f);
				}
			}
		}
		foreach (Transform rightFlame in rightFlames)
		{
			if (rightFlame != null)
			{
				rightFlame.position += new Vector3(0f - momentum, -0.0625f);
				if (rightFlame.position.y <= -2.867f)
				{
					rightFlame.position = new Vector3(rightFlame.position.x, -2.867f);
				}
			}
		}
		for (int j = 0; j < sideFlames.Count; j++)
		{
			if (sideFlames[j] != null)
			{
				sideFlames[j].position += new Vector3(0f, moveRates[j]);
				if (sideFlames[j].position.y <= -3.2f || sideFlames[j].position.y >= -0.2f)
				{
					GameObject obj = sideFlames[j].gameObject;
					sideFlames.RemoveAt(j);
					moveRates.RemoveAt(j);
					Object.Destroy(obj);
					j--;
				}
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		flamePrefab = Resources.Load<GameObject>("battle/attacks/bullets/FlameBullet");
	}
}
