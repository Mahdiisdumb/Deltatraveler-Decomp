using UnityEngine;

public class DoggoAttack : AttackBase
{
	private GameObject prefab;

	private bool scan;

	private int spawnRate = 20;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(235f, 140f);
		int progress = Object.FindObjectOfType<Doggo>().GetProgress();
		maxFrames = ((progress == 0) ? 100 : 140);
		switch (progress)
		{
		case 0:
			scan = true;
			break;
		case 1:
			spawnRate = 15;
			break;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!scan && isStarted && frames % spawnRate == 0)
		{
			CreateBullet();
		}
	}

	private void CreateBullet()
	{
		bool flag = !scan && Random.Range(0, 2) == 0;
		Object.Instantiate(prefab, new Vector3(flag ? (-2.55f) : 2.55f, 0f), Quaternion.identity, base.transform);
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(235f, 100f));
		prefab = Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DoggoBullet");
		CreateBullet();
	}
}
