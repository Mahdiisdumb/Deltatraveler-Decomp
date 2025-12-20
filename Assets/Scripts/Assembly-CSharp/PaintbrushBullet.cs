using UnityEngine;

public class PaintbrushBullet : BulletBase
{
	private Vector3 velocity = Vector3.zero;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
		velocity = ((Random.Range(0, 2) == 0) ? Vector3.right : Vector3.up);
		velocity *= (float)((Random.Range(0, 2) != 0) ? 1 : (-1));
		velocity *= 1f / 12f;
		if (velocity.y == 0f)
		{
			if (velocity.x > 0f)
			{
				base.transform.position = new Vector3(-3.28f, Random.Range(-1.82f, 1.1f));
			}
			else
			{
				base.transform.position = new Vector3(3.28f, Random.Range(-3.53f, -0.48f));
				base.transform.eulerAngles = new Vector3(0f, 0f, 180f);
			}
		}
		else if (velocity.y > 0f)
		{
			base.transform.position = new Vector3(Random.Range(-2.55f, 0.82f), -4.32f);
			base.transform.eulerAngles = new Vector3(0f, 0f, 90f);
		}
		else
		{
			base.transform.position = new Vector3(Random.Range(-0.82f, 2.55f), 2.32f);
			base.transform.eulerAngles = new Vector3(0f, 0f, 270f);
		}
		if ((bool)Object.FindObjectOfType<Carpainter>())
		{
			base.transform.right = Object.FindObjectOfType<SOUL>().transform.position - base.transform.GetChild(0).position;
			velocity = Vector3.MoveTowards(base.transform.GetChild(0).position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 12f) - base.transform.GetChild(0).position;
		}
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = new Color(1f, 1f, 1f, 0f);
		}
	}

	private void Update()
	{
		frames++;
		if (frames <= 6)
		{
			Color color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)frames / 6f);
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = color;
			}
		}
		else if (frames > 70)
		{
			BoxCollider2D[] componentsInChildren2 = GetComponentsInChildren<BoxCollider2D>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			Color color2 = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)(frames - 70) / 10f);
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = color2;
			}
		}
		base.transform.position += velocity;
		if (frames == 80)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public override void SOULHit()
	{
		if ((bool)Object.FindObjectOfType<PaintbrushAttack>())
		{
			Object.FindObjectOfType<PaintbrushAttack>().GetHit();
		}
		if ((bool)Object.FindObjectOfType<CarPaintbrushAttack>())
		{
			Object.FindObjectOfType<CarPaintbrushAttack>().GetHit();
		}
	}
}
