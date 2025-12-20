using UnityEngine;

public class PlatformChallenge1 : AttackBase
{
	private GameObject platforms;

	private GameObject floorBones;

	private GameObject[] flameBullets;

	private GameObject randomBone;

	private bool frozen;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 335;
		bbSize = new Vector2(375f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
	}

	protected override void Update()
	{
		if (frozen)
		{
			return;
		}
		base.Update();
		if (!isStarted)
		{
			return;
		}
		platforms.transform.position -= new Vector3(1f / 12f, 0f);
		floorBones.transform.position -= new Vector3(0.0625f, 0f);
		if (frames <= 250)
		{
			randomBone.transform.position = Vector3.Lerp(new Vector3(5f, -0.45f), new Vector3(-5f, -0.45f), (float)(frames - 135) / 66f);
		}
		else
		{
			randomBone.transform.position = Vector3.Lerp(new Vector3(-5f, -0.45f), new Vector3(5f, -0.45f), (float)(frames - 260) / 66f);
		}
		if (frames <= 135)
		{
			return;
		}
		for (int i = 0; i < flameBullets.Length; i++)
		{
			if ((bool)flameBullets[i])
			{
				int num = (frames + i * 30) % 60;
				float num2 = (float)num / 30f;
				if (num > 30)
				{
					num2 = (float)(60 - num) / 30f;
				}
				num2 = num2 * num2 * (3f - 2f * num2);
				float y = Mathf.Lerp(-0.59f, -2.07f, num2);
				int num3 = frames - 25 * i;
				flameBullets[i].transform.position = Vector3.Lerp(new Vector3(5f, y), new Vector3(-5f, y), (float)(num3 - 135) / 100f);
			}
		}
	}

	public void SetFreeze(bool frozen)
	{
		this.frozen = frozen;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		platforms = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/PlatformChallenge1Platforms"), base.transform);
		platforms.transform.position = new Vector3(8.35f, 0f);
		floorBones = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/BoneOcean"), base.transform);
		floorBones.transform.position = new Vector3(9.79f, 0f);
		GameObject original = Resources.Load<GameObject>("battle/attacks/bullets/FlameBullet");
		flameBullets = new GameObject[2]
		{
			Object.Instantiate(original, base.transform),
			Object.Instantiate(original, base.transform)
		};
		GameObject[] array = flameBullets;
		foreach (GameObject obj in array)
		{
			obj.transform.position = new Vector3(5f, 0f);
			obj.GetComponent<FlameBullet>().SetBaseDamage(8);
		}
		randomBone = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/Bone"), base.transform);
		randomBone.GetComponent<BoneBullet>().ChangeHeight(5f);
		randomBone.transform.position = new Vector3(5f, -0.45f);
	}
}
