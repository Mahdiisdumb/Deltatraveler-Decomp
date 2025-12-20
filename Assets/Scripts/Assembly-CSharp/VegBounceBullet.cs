using UnityEngine;

public class VegBounceBullet : BulletBase
{
	private float velocity;

	private float xMulti = 1f;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
		xMulti = ((Random.Range(0, 2) == 1) ? 1 : (-1));
		if ((bool)Object.FindObjectOfType<Parsnik>())
		{
			baseDmg = 6;
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/hardmode/spr_snakebullet_bounce");
			sr.flipX = Random.Range(0, 2) == 1;
			sr.flipY = Random.Range(0, 10) == 1;
		}
		else
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/ruins/spr_vegbullet_" + Random.Range(0, 6));
		}
	}

	private void Update()
	{
		if (base.transform.position.x >= 1.5f)
		{
			xMulti = -1f;
		}
		if (base.transform.position.x <= -1.5f)
		{
			xMulti = 1f;
		}
		base.transform.position += new Vector3(0.0625f * xMulti, velocity);
		if (base.transform.position.y <= -2.76f && velocity < 0f)
		{
			velocity *= -1f;
		}
		velocity -= 0.004166667f;
	}

	public override void ChangeType(string tag)
	{
		base.ChangeType(tag);
		if (tag == "GreenBullet")
		{
			baseDmg = 1;
		}
	}

	public override void SOULHit()
	{
		Vegetoid[] array = Object.FindObjectsOfType<Vegetoid>();
		foreach (Vegetoid vegetoid in array)
		{
			if (vegetoid.ExpectingGreensEaten() && base.gameObject.tag == "GreenBullet")
			{
				vegetoid.EatGreens();
			}
		}
		Parsnik[] array2 = Object.FindObjectsOfType<Parsnik>();
		foreach (Parsnik parsnik in array2)
		{
			if (parsnik.ExpectingGreensEaten() && base.gameObject.tag == "GreenBullet")
			{
				parsnik.EatGreens();
			}
		}
		base.SOULHit();
	}
}
