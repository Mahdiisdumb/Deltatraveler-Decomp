using UnityEngine;

public class BouncingFlameBullet : FlameBullet
{
	private bool moving;

	private Vector3 velocity = Vector3.zero;

	private float speed = 1f;

	private bool speedGoingUp = true;

	protected override void Update()
	{
		base.Update();
		if (moving)
		{
			if (speedGoingUp)
			{
				speed += 0.1f;
			}
			else if (speed > 1f)
			{
				speed -= 0.1f;
			}
			else
			{
				speed = 1f;
			}
			if (speed >= 6f)
			{
				speedGoingUp = false;
			}
			base.transform.position += velocity * speed;
			if (base.transform.position.x >= 1.7f && velocity.x > 0f)
			{
				velocity = new Vector3(0f - velocity.x, velocity.y);
			}
			else if (base.transform.position.x <= -1.7f && velocity.x < 0f)
			{
				velocity = new Vector3(0f - velocity.x, velocity.y);
			}
			if (base.transform.position.y >= -0.4f && velocity.y > 0f)
			{
				velocity = new Vector3(velocity.x, 0f - velocity.y);
			}
			else if (base.transform.position.y <= -2.922f && velocity.y < 0f)
			{
				velocity = new Vector3(velocity.x, 0f - velocity.y);
			}
		}
	}

	public void BonBonGoGetEm()
	{
		velocity = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 48f) - base.transform.position;
		moving = true;
	}
}
