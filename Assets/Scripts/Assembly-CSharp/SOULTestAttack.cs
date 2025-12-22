using UnityEngine;

public class SOULTestAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 3000;
	}

	protected override void Update()
	{
		base.Update();
		if (Input.GetKeyDown(KeyCode.V))
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(SOUL.SoulMode.Shoot);
		Util.FindObjectOfType<SOUL>().EnableYDash();
	}
}
