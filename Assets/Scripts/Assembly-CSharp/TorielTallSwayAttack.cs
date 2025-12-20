using System.Collections.Generic;
using UnityEngine;

public class TorielTallSwayAttack : TorielAttackBase
{
	private GameObject flamePrefab;

	private List<Transform> leftFlames = new List<Transform>();

	private List<Transform> rightFlames = new List<Transform>();

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
		if (frames % 4 == 0)
		{
			Transform transform = Object.Instantiate(flamePrefab, new Vector3(0f, 0.8f), Quaternion.identity, base.transform).transform;
			transform.GetComponent<FlameBullet>().SetBaseDamage(Object.FindObjectOfType<PartyPanels>().GetTargettedMembers()[0] ? 2 : 5);
			if (Random.Range(0, 2) == 1)
			{
				leftFlames.Add(transform);
			}
			else
			{
				rightFlames.Add(transform);
			}
		}
		foreach (Transform leftFlame in leftFlames)
		{
			if (leftFlame != null)
			{
				leftFlame.position += new Vector3(momentum, -1f / 12f);
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
				rightFlame.position += new Vector3(0f - momentum, -1f / 12f);
				if (rightFlame.position.y <= -2.867f)
				{
					rightFlame.position = new Vector3(rightFlame.position.x, -2.867f);
				}
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(185f, 190f));
		flamePrefab = Resources.Load<GameObject>("battle/attacks/bullets/FlameBullet");
	}
}
