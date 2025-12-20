using UnityEngine;

public class PaulaStarstormAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 250;
		bbSize = new Vector2(320f, 140f);
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames <= 180 && frames % 9 == 1)
		{
			BigStar component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/BigStar"), base.transform).GetComponent<BigStar>();
			if (frames % 63 == 1)
			{
				component.GetComponents<AudioSource>()[0].Play();
			}
		}
	}
}
