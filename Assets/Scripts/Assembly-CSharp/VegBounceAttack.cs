using UnityEngine;

public class VegBounceAttack : AttackBase
{
	private int veggieSpawns;

	private bool spawnAGreenie;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 110;
		bbSize = new Vector2(165f, 140f);
		Vegetoid[] array = Object.FindObjectsOfType<Vegetoid>();
		for (int i = 0; i < array.Length; i++)
		{
			if (array[i].ExpectingGreensEaten())
			{
				spawnAGreenie = true;
			}
		}
		attackAllTargets = false;
	}

	public void OnDestroy()
	{
		Vegetoid[] array = Object.FindObjectsOfType<Vegetoid>();
		for (int i = 0; i < array.Length; i++)
		{
			array[i].DisableEatingGreens();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted && frames % 18 == 1)
		{
			veggieSpawns++;
			VegBounceBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/VegBounceBullet"), new Vector3(Random.Range(-1.5f, 1.5f), -0.43f), Quaternion.identity, base.transform).GetComponent<VegBounceBullet>();
			if (veggieSpawns == 3 && spawnAGreenie)
			{
				component.ChangeType("GreenBullet");
			}
		}
	}
}
