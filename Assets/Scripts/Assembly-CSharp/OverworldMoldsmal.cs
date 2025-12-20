using UnityEngine;

public class OverworldMoldsmal : OverworldEnemyBase
{
	protected override void Awake()
	{
		base.Awake();
		speed = 0f;
		if (GetCounter() >= 1)
		{
			int num = 0;
			if (base.gameObject.name != "Moldsmal")
			{
				num = int.Parse(base.gameObject.name.Substring(0, 1));
			}
			base.transform.position = new Vector3(5.46f + 1.44f * (float)num, 1.36f);
		}
		if ((int)Util.GameManager().GetFlag(108) == 1)
		{
			anim.Play("Moldessa");
		}
		sortingOrderOffset = -2;
	}
}
