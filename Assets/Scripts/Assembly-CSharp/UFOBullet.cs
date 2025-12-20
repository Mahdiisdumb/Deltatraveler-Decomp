using UnityEngine;

public class UFOBullet : BulletBase
{
	private bool left = true;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		destroyOnHit = false;
		left = Random.Range(0, 2) == 0;
		float num = 7.1f;
		if (left)
		{
			num *= -1f;
		}
		float y = Random.Range(-2.9f, -0.41f);
		base.transform.position = new Vector3(num, y);
		if (Random.Range(0, 50) == 0)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/attacks/bullets/eb/spr_ufo_bullet_cute");
		}
	}

	private void Update()
	{
		float num = (left ? 1 : (-1));
		base.transform.position += new Vector3(1f / 6f * num, 0f);
		if (Mathf.Abs(base.transform.position.x) > 8f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
