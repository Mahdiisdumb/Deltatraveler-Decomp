using UnityEngine;

public class FallingChallengeAttack2 : AttackBase
{
	private GameObject platforms;

	private GameObject boneOcean;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 450;
		bbSize = new Vector2(200f, 140f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
	}

	private void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<SOUL>())
		{
			Object.FindObjectOfType<SOUL>().SetPullForce(Vector3.zero);
		}
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted)
		{
			platforms.transform.position += new Vector3(0f, 1f / 24f);
			platforms.transform.Find("BonesMoveRight").transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(2.33f, 0f), (float)(frames % 60) / 60f);
			platforms.transform.Find("BonesMoveLeft").transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(-2.33f, 0f), (float)(frames % 60) / 60f);
			boneOcean.transform.position = Vector3.Lerp(new Vector3(-3f, -0.3f), new Vector3(-2.73f, -0.3f), (float)(frames % 5) / 5f);
			float t = (float)(frames % 30) / 10f;
			if (frames % 60 >= 30)
			{
				t = 1f - (float)(frames % 60 - 30) / 10f;
			}
			platforms.transform.Find("MovieMovie").transform.localPosition = new Vector3(-0.51f, Mathf.Lerp(-6.6f, -7.54f, t));
			if (frames == 350)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster"), new Vector3(10f, 6f), Quaternion.identity).GetComponent<GasterBlaster>().Activate(2, 2, -90f, new Vector2(3.32f, 3.63f), 40);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		platforms = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/FallingChallenge2Platforms"), base.transform);
		boneOcean = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/BoneLineSide"), base.transform);
		boneOcean.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
		bb.StartMovement(new Vector2(200f, 350f));
		Object.FindObjectOfType<SOUL>().SetPullForce(new Vector3(0f, 1f / 24f));
	}
}
