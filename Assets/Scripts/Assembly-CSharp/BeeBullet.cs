using UnityEngine;

public class BeeBullet : BulletBase
{
	private bool kill;

	private float speed = 4f;

	private Vector3 toPos = Vector3.zero;

	private Vector3 tradjectory = Vector3.zero;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 8;
		if (Object.FindObjectOfType<MightyBear>().IsLectured())
		{
			baseDmg = 7;
		}
		destroyOnHit = false;
		base.transform.position = GetRandomPosition();
		SetNewPosition();
	}

	private void Update()
	{
		if (!kill)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, toPos, speed / 48f);
			if (base.transform.position == toPos)
			{
				SetNewPosition();
			}
		}
		else
		{
			base.transform.position += tradjectory * speed;
		}
	}

	private void SetNewPosition()
	{
		if (!kill)
		{
			toPos = GetRandomPosition();
			base.transform.up = toPos - base.transform.position;
		}
	}

	private Vector3 GetRandomPosition()
	{
		return new Vector3(Random.Range(-0.72f, 0.72f), Random.Range(-2.5f, -1.96f));
	}

	public void PissOff()
	{
		speed = (Object.FindObjectOfType<MightyBear>().IsLectured() ? 7 : 8);
	}

	public void KillPlayer()
	{
		kill = true;
		tradjectory = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 48f) - base.transform.position;
		base.transform.up = -tradjectory;
	}
}
