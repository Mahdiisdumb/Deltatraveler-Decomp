using System.Collections.Generic;
using UnityEngine;

public class UpsideDownAlternateAttack : AttackBase
{
	private GameObject bonePrefab;

	private List<Transform> leftBones = new List<Transform>();

	private List<Transform> rightBones = new List<Transform>();

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 300;
		bbSize = new Vector2(375f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames == 1)
		{
			Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.up, 12f);
		}
		if (frames == 25)
		{
			Object.FindObjectOfType<Sans>().ResetBreatheAnimation();
		}
		if (frames % 25 == 1 && frames <= 201)
		{
			Transform transform = Object.Instantiate(bonePrefab, base.transform).transform;
			transform.GetComponent<BoneBullet>().ChangeHeight(2f);
			transform.position = new Vector3(4f, -0.52f);
			rightBones.Add(transform);
			Transform transform2 = Object.Instantiate(bonePrefab, base.transform).transform;
			transform2.GetComponent<BoneBullet>().ChangeHeight(17f);
			transform2.position = new Vector3(-4f, -1.88f);
			leftBones.Add(transform2);
		}
		foreach (Transform leftBone in leftBones)
		{
			leftBone.position += new Vector3(1f / 12f, 0f);
		}
		foreach (Transform rightBone in rightBones)
		{
			rightBone.position -= new Vector3(1f / 12f, 0f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bonePrefab = Resources.Load<GameObject>("battle/attacks/bullets/sans/Bone");
	}
}
