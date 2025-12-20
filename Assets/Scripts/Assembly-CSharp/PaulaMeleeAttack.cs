using UnityEngine;

public class PaulaMeleeAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 365;
		bbSize = new Vector2(165f, 140f);
	}

	private void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<PaulaMeleeTarget>())
		{
			Object.FindObjectOfType<PaulaMeleeTarget>().SetToDestroy();
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PaulaTarget")).GetComponent<PaulaMeleeTarget>().Activate(4, true);
	}
}
