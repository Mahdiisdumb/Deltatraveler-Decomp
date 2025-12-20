using UnityEngine;

public class PaulaFireAttack : AttackBase
{
	private float misalignAngle = 10f;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 280;
		bbSize = new Vector2(165f, 140f);
		misalignAngle += (float)(Object.FindObjectOfType<Paula>().GetMaxHP() - Object.FindObjectOfType<Paula>().GetHP()) / 25f;
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 20 == 1)
		{
			Vector3 position = Object.FindObjectOfType<SOUL>().transform.position;
			if (frames % 40 == 21)
			{
				position = new Vector3(Random.Range(-1.32f, 1.32f), Random.Range(-2.83f, -0.48f));
			}
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFireMainBullet"), position, Quaternion.Euler(0f, 0f, Random.Range(0f - misalignAngle, misalignAngle)), base.transform).GetComponent<PKFireMainBullet>().MisalignFire();
		}
	}
}
