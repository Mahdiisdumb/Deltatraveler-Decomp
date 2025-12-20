using UnityEngine;

public class GlydeBullet : BulletBase
{
	private Vector3 direction = Vector3.down;

	private float speed;

	private bool spinRight;

	private SpriteRenderer otherSr;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 8;
		if (Object.FindObjectOfType<Glyde>().StrongerAttack())
		{
			baseDmg += 2;
		}
		speed = Random.Range(6.5f, 7.5f) / 48f;
		spinRight = Random.Range(0, 2) == 0;
		otherSr = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
	}

	private void Update()
	{
		base.transform.position += direction * speed;
		if (base.transform.position.y < -6f)
		{
			Object.Destroy(base.gameObject);
		}
		frames++;
		int num = frames % 20;
		if (num <= 10)
		{
			otherSr.color = new Color(1f, 1f, 1f, (float)(num - 5) / 5f);
			sr.color = new Color(1f, 1f, 1f, 1f - otherSr.color.a);
		}
		else
		{
			sr.color = new Color(1f, 1f, 1f, (float)(num - 15) / 5f);
			otherSr.color = new Color(1f, 1f, 1f, 1f - sr.color.a);
		}
		base.transform.eulerAngles += new Vector3(0f, 0f, (!spinRight) ? 1 : (-1));
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.5f, 1.5f, 1f), (float)frames / 10f);
	}

	public void Activate(Vector3 direction)
	{
		this.direction = direction;
	}
}
