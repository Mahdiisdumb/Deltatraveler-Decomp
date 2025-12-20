using System;
using UnityEngine;

public class SimplePelletAttack : AttackBase
{
	private bool hardmode;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 200;
		bbSize = new Vector2(165f, 140f);
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % (hardmode ? 4 : 5) == 1)
		{
			float f = (float)UnityEngine.Random.Range(-90, 90) * ((float)Math.PI / 180f);
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/flowey/FloweyPelletStandard"), new Vector3(Mathf.Sin(f) * 2f, Mathf.Cos(f) + 3f), Quaternion.identity, base.transform);
		}
	}
}
