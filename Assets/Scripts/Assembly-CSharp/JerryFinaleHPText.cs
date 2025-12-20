using UnityEngine;

public class JerryFinaleHPText : EnemyHPText
{
	protected override void Update()
	{
		base.Update();
		if (frames == 30)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
