using UnityEngine;

public class FloweyBiteAttackHarder : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbSize = new Vector2(300f, 101f);
		bbPos = Vector2.zero;
		soulPos = bbPos;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyHeadBullet"), new Vector3(10f, 0f), Quaternion.identity, base.transform).GetComponent<FloweyHeadBullet>().SetToHard();
	}
}
