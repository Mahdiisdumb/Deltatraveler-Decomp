using UnityEngine;

public class FallingChallenge1 : AttackBase
{
	private GameObject platforms;

	private GameObject boneOcean;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 400;
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
			platforms.transform.Find("BonesMoveRight").transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(2.33f, 0f), (float)(frames % 20) / 20f);
			platforms.transform.Find("BonesMoveLeft").transform.localPosition = Vector3.Lerp(Vector3.zero, new Vector3(-2.33f, 0f), (float)(frames % 20) / 20f);
			boneOcean.transform.position = Vector3.Lerp(new Vector3(-3f, -0.3f), new Vector3(-2.73f, -0.3f), (float)(frames % 5) / 5f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		platforms = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/FallingChallenge1Platforms"), base.transform);
		boneOcean = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/BoneLineSide"), base.transform);
		boneOcean.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
		bb.StartMovement(new Vector2(200f, 350f));
		Object.FindObjectOfType<SOUL>().SetPullForce(new Vector3(0f, 1f / 24f));
	}
}
