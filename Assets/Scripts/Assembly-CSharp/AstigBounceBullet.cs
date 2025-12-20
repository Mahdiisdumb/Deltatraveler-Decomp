using UnityEngine;

public class AstigBounceBullet : BulletBase
{
	private bool lookingForRight;

	private bool lookingForUp;

	private Vector3 tradjectory;

	private float speed = 1f;

	private bool hitWall;

	private int postWallFrames;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
		sr.color = new Color(1f, 1f, 1f, 0f);
		lookingForRight = Random.Range(0, 2) == 1;
		lookingForUp = Random.Range(0, 2) == 1;
		tradjectory = new Vector3(lookingForRight ? 1 : (-1), lookingForUp ? 1 : (-1));
		if (Random.Range(0, 2) == 1)
		{
			float x = Random.Range(-2.7f, -0.57f) * (float)(lookingForRight ? 1 : (-1));
			float y = (lookingForUp ? (-4.92f) : 1.59f);
			base.transform.position = new Vector3(x, y);
		}
		else
		{
			float x2 = (lookingForRight ? (-4.1f) : 4.1f);
			float y2 = (lookingForUp ? Random.Range(-4.71f, -3.01f) : Random.Range(-0.31f, 2f));
			base.transform.position = new Vector3(x2, y2);
		}
	}

	private void Update()
	{
		if (frames < 10)
		{
			frames++;
			sr.color = new Color(1f, 1f, 1f, (float)frames / 10f);
		}
		base.transform.position += tradjectory * speed / 48f;
		speed += 0.1f;
		if (!hitWall)
		{
			if ((lookingForRight && base.transform.position.x > 1.614f) || (!lookingForRight && base.transform.position.x < -1.614f))
			{
				HitWall(vertical: false);
			}
			else if ((lookingForUp && base.transform.position.y > -0.524f) || (!lookingForUp && base.transform.position.y < -2.8f))
			{
				HitWall(vertical: true);
			}
			return;
		}
		postWallFrames++;
		float a = (float)(80 - postWallFrames) / 20f;
		sr.color = new Color(1f, 1f, 1f, a);
		if (postWallFrames == 80)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void HitWall(bool vertical)
	{
		hitWall = true;
		speed = 2f;
		Vector3 position = base.transform.position;
		if (vertical)
		{
			tradjectory = new Vector3(0f, (!lookingForUp) ? 1 : (-1));
			if (position.y > -0.524f)
			{
				position.y = -0.524f;
			}
			else if (position.y < -2.8f)
			{
				position.y = -2.8f;
			}
		}
		else
		{
			tradjectory = new Vector3((!lookingForRight) ? 1 : (-1), 0f);
			if (position.x > 1.614f)
			{
				position.x = 1.614f;
			}
			else if (position.x < -1.614f)
			{
				position.x = -1.614f;
			}
		}
		base.transform.position = position;
	}
}
