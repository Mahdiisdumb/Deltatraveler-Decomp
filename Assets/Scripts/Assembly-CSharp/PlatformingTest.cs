using UnityEngine;

public class PlatformingTest : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(350f, 140f);
	}

	protected override void Update()
	{
		if (isStarted && UTInput.GetButtonDown("C"))
		{
			Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
			Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector3(350f, 250f));
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/PlatformingTestPlatforms"), base.transform);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
	}
}
