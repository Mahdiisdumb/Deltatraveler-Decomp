using UnityEngine;

public class NapstablookNopeAttack : AttackBase
{
	private Transform nope;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 140;
		bbSize = new Vector2(255f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted)
		{
			nope.transform.position = soulPos + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) / 48f;
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		nope = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/Nope"), soulPos, Quaternion.identity, base.transform).transform;
	}
}
