using UnityEngine;

public class TestRandomPelletAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector3(300f, 250f);
		maxFrames = 90;
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 3 == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/TestFloweyPellet"), base.transform);
		}
	}
}
