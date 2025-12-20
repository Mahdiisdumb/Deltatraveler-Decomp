using UnityEngine;

public class BigMoleAttack : AttackBase
{
	private int spawnRate = 12;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 180;
		bbSize = new Vector2(165f, 140f);
		spawnRate = 12 - Object.FindObjectOfType<MondoMole>().GetDifficultyLevel();
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % spawnRate == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/MoleDigBullet"), base.transform).transform.localScale = new Vector3(2f, 2f, 1f);
		}
	}
}
