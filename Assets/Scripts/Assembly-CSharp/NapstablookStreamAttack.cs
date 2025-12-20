using UnityEngine;

public class NapstablookStreamAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(165f, 140f);
		maxFrames = 170;
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 10 == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/NapstablookStreamBullet"), new Vector3(-0.08f, 1.38f), Quaternion.identity, base.transform).GetComponent<NapstablookStreamBullet>().SetToLeft();
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/NapstablookStreamBullet"), new Vector3(0.55f, 1.17f), Quaternion.identity, base.transform);
		}
	}
}
