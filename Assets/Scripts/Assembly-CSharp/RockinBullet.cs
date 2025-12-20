using UnityEngine;

public class RockinBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private bool right;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = (Object.FindObjectOfType<Ness>().ReduceDamage() ? 7 : 9);
		destroyOnHit = false;
		ChangeType(Random.Range(1, 3));
		right = Random.Range(0, 2) == 1;
		if (right)
		{
			base.transform.localScale = new Vector3(-1f, 1f, 1f);
		}
		bool flag = Object.FindObjectOfType<PSIMagnetBullet>();
		base.transform.position = new Vector3(right ? (-6.76f) : 6.76f, Random.Range(flag ? 8.5f : 7.905f, 2.25f));
	}

	private void Update()
	{
		frames++;
		base.transform.position += new Vector3(right ? 1 : (-1), -1f) * 8f / 48f;
		if (base.transform.position.y < -8f)
		{
			Object.Destroy(base.gameObject);
		}
		sr.sprite = sprites[frames / 2 % 3];
	}
}
