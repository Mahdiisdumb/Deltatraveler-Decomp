using UnityEngine;

public class FlyBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private Vector3 pos1;

	private Vector3 pos2;

	private Vector3 velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 4;
	}

	private void Update()
	{
		frames++;
		if (frames == 1)
		{
			velocity = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 24f) - base.transform.position;
		}
		if (frames == 45)
		{
			velocity = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 24f) - base.transform.position;
		}
		if (frames > 45 || frames <= 30)
		{
			base.transform.position += velocity;
		}
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 2 % 2];
	}
}
