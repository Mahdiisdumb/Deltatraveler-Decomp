using UnityEngine;

public class VegCarrotBullet : BulletBase
{
	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
		if ((bool)Util.FindObjectOfType<Parsnik>())
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/hardmode/spr_snakebullet_fall");
			baseDmg = 6;
		}
		if ((bool)Util.FindObjectOfType<CoilSnake>())
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/hardmode/spr_snakebullet_fall");
			baseDmg = 4;
		}
	}

	private void Update()
	{
		base.transform.position += new Vector3(0f, velocity);
		velocity -= 0.004166667f;
		if (base.transform.position.y < -3.5f)
		{
			Object.Destroy(base.gameObject);
		}
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
		Vegetoid[] array = Util.FindObjectsOfType<Vegetoid>();
		foreach (Vegetoid vegetoid in array)
		{
			if (vegetoid.ExpectingGreensEaten() && base.gameObject.tag == "GreenBullet")
			{
				vegetoid.EatGreens();
			}
		}
		Parsnik[] array2 = Util.FindObjectsOfType<Parsnik>();
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
