using UnityEngine;

public class DogiAttack : AttackBase
{
	private int type;

	private bool alone;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(185f, 140f);
		maxFrames = 190;
		type = Random.Range(0, 2);
		if (Object.FindObjectOfType<Dogamy>().IsKilled())
		{
			type = 1;
			alone = true;
		}
		else if (Object.FindObjectOfType<Dogaressa>().IsKilled())
		{
			type = 0;
			alone = true;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted || frames != 5)
		{
			return;
		}
		if (type == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DogHeartSpawner"), base.transform).GetComponent<DogHeartSpawner>().SetAsSpawner();
			if (!alone)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DogHeartSpawner"), base.transform).transform.position = new Vector3(3.16f, -1.78f);
			}
		}
		else
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DogiAxe"), new Vector3(-3f, -2.264f), Quaternion.identity, base.transform);
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DogiAxe"), new Vector3(3f, -2.264f), Quaternion.identity, base.transform).transform.localScale = new Vector3(-1f, 1f, 1f);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		if (type == 0)
		{
			bb.StartMovement(new Vector2(365f, 140f));
		}
		else if (type == 1)
		{
			bb.StartMovement(new Vector2(235f, 100f));
		}
	}
}
