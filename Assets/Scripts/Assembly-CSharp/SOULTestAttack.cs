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
		if (UTInput.GetButtonDown("C"))
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(SOUL.SoulMode.Shoot);
		Object.FindObjectOfType<SOUL>().EnableYDash();
	}
}
