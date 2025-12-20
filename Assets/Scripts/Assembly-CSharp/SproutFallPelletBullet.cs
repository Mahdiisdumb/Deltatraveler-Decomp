using UnityEngine;

public class SproutFallPelletBullet : BulletBase
{
	private float fallVelocity = 1f / 32f;

	private float xVelocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		xVelocity = Random.Range(-3f, 3f) / 48f;
	}

	private void Update()
	{
		fallVelocity += 1f / 160f;
		base.transform.position += new Vector3(xVelocity, 0f - fallVelocity);
		if (base.transform.position.y < -5f)
		{
			Object.Destroy(base.gameObject);
		}
		if (xVelocity > 0f)
		{
			base.transform.GetChild(0).eulerAngles -= new Vector3(0f, 0f, 9f);
		}
		else
		{
			base.transform.GetChild(0).eulerAngles += new Vector3(0f, 0f, 9f);
		}
	}
}
