using UnityEngine;

public class GauntletFireball : ActionBulletBase
{
	private readonly float SPEED = 1f / 3f;

	[SerializeField]
	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 7;
	}

	protected override void Update()
	{
		base.Update();
		frames++;
		sr.sprite = sprites[frames / 3 % 3];
		sr.color = new Color(1f, 1f, 1f, (float)frames / 5f);
		GetComponent<Rigidbody2D>().MovePosition(base.transform.position + Vector3.down * SPEED);
		if (base.transform.localPosition.y < -6f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
