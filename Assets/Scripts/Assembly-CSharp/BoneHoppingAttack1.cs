using UnityEngine;

public class BoneHoppingAttack1 : AttackBase
{
	private GameObject[] leftBones;

	private GameObject[] rightBones;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 285;
		bbSize = new Vector2(250f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || frames >= maxFrames)
		{
			return;
		}
		for (int i = 0; i < 6; i++)
		{
			int num = frames - i * 20;
			if (num == 130)
			{
				leftBones[i].transform.rotation = Quaternion.Euler(0f, 0f, 180f);
				rightBones[i].transform.rotation = Quaternion.Euler(0f, 0f, 180f);
			}
			if (num < 130)
			{
				leftBones[i].transform.position = Vector3.Lerp(new Vector3(-3f, 0f), new Vector3(3f, 0f), (float)num / 50f);
				rightBones[i].transform.position = Vector3.Lerp(new Vector3(3f, 0f), new Vector3(-3f, 0f), (float)num / 50f);
			}
			else
			{
				leftBones[i].transform.position = Vector3.Lerp(new Vector3(-3f, -3.35f), new Vector3(3f, -3.35f), (float)(num - 140) / 50f);
				rightBones[i].transform.position = Vector3.Lerp(new Vector3(3f, -3.35f), new Vector3(-3f, -3.35f), (float)(num - 140) / 50f);
			}
		}
		if (frames == 126)
		{
			Object.FindObjectOfType<SansGravityManager>().Slam(Vector2.up);
		}
		if (frames == 156)
		{
			Object.FindObjectOfType<Sans>().ResetBreatheAnimation();
		}
	}

	public void OnDestroy()
	{
		for (int i = 0; i < 6; i++)
		{
			if (leftBones != null && rightBones != null)
			{
				if ((bool)leftBones[i])
				{
					Object.Destroy(leftBones[i]);
				}
				if ((bool)rightBones[i])
				{
					Object.Destroy(rightBones[i]);
				}
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		GameObject original = Resources.Load<GameObject>("battle/attacks/bullets/sans/BoneHopGroup");
		leftBones = new GameObject[10];
		rightBones = new GameObject[10];
		for (int i = 0; i < 6; i++)
		{
			leftBones[i] = Object.Instantiate(original, new Vector3(-3f, 0f), Quaternion.identity);
			rightBones[i] = Object.Instantiate(original, new Vector3(3f, 0f), Quaternion.identity);
			BoneBullet[] componentsInChildren = leftBones[i].GetComponentsInChildren<BoneBullet>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].SetTPGrazeValue(0.5f);
			}
			componentsInChildren = rightBones[i].GetComponentsInChildren<BoneBullet>();
			for (int j = 0; j < componentsInChildren.Length; j++)
			{
				componentsInChildren[j].SetTPGrazeValue(0.5f);
			}
		}
	}
}
