using UnityEngine;

public class GyftrotPelletBullet : BulletBase
{
	private float fallVelocity = 1f / 24f;

	private bool reverseDirection;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 11;
		reverseDirection = Random.Range(0, 2) == 0;
	}

	private void Update()
	{
		fallVelocity += 1f / 128f;
		base.transform.position += new Vector3((float)((!reverseDirection) ? 1 : (-1)) / 48f, 0f - fallVelocity);
		if (base.transform.position.y <= -3.2f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
