using System;
using UnityEngine;

public class BaseballBullet : BulletBase
{
	private Vector3 basePos;

	private float xVelocity;

	private float yVelocity;

	private float yStrength;

	private int framesToFreefall;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = (UnityEngine.Object.FindObjectOfType<Ness>().ReduceDamage() ? 6 : 8);
		destroyOnHit = false;
		sr.color = new Color(1f, 1f, 1f, 0f);
	}

	private void Start()
	{
		basePos = base.transform.position;
		yStrength = UnityEngine.Random.Range(1f, 2f);
		framesToFreefall = Mathf.RoundToInt(20f * yStrength);
		xVelocity = (UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x - base.transform.position.x) / (float)framesToFreefall;
	}

	private void Update()
	{
		Vector3 position = base.transform.position;
		if (state == 0)
		{
			frames++;
			if (frames <= 5)
			{
				GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)frames / 5f);
			}
			base.transform.position = new Vector3(basePos.x, basePos.y + Mathf.Sin((float)(frames * 9) * ((float)Math.PI / 180f)));
			if (frames == 20)
			{
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames <= framesToFreefall)
			{
				float y = basePos.y + Mathf.Sin((float)frames * (180f / (float)framesToFreefall) * ((float)Math.PI / 180f)) * yStrength;
				base.transform.position = new Vector3(base.transform.position.x + xVelocity, y);
				if (frames == framesToFreefall)
				{
					yVelocity = base.transform.position.y - position.y;
				}
			}
			else
			{
				base.transform.position += new Vector3(xVelocity, yVelocity);
			}
		}
		if (base.transform.position.y < -6f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
