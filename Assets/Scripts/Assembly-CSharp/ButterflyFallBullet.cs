using UnityEngine;

public class ButterflyFallBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 4;
	}

	private void Update()
	{
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 4 % 2];
		velocity += Random.Range(0.0015f, 0.007f);
		base.transform.position += new Vector3(0f, velocity);
		if (base.transform.position.y > -0.406f || Mathf.Abs(base.transform.position.x) >= 1.555f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
