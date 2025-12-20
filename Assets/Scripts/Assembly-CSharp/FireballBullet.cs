using System;
using UnityEngine;

public class FireballBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private int spriteFrames;

	private Vector3 basePos;

	private float xVelocity;

	private float yVelocity;

	private float yStrength;

	private int framesToFreefall;

	private Vector3 tradjectory;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
		sr.color = new Color(1f, 1f, 1f, 0f);
	}

	private void Start()
	{
		basePos = base.transform.position;
	}

	private void Update()
	{
		spriteFrames++;
		sr.sprite = sprites[spriteFrames / 2 % 3];
		_ = base.transform.position;
		if (state == 0)
		{
			frames++;
			if (frames > 10)
			{
				base.transform.eulerAngles = Vector3.zero;
			}
			else
			{
				base.transform.position = new Vector3(0f, 0f, 180f);
			}
			if (frames <= 5)
			{
				GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)frames / 5f);
			}
			base.transform.position = new Vector3(basePos.x, basePos.y + Mathf.Sin((float)(frames * 9) * (MathF.PI / 180f)));
			if (frames == 20)
			{
				state = 1;
				frames = 0;
				tradjectory = Vector3.MoveTowards(base.transform.position, Util.FindObjectOfType<SOUL>().transform.position, 5f / 24f) - base.transform.position;
				base.transform.up = -tradjectory;
			}
		}
		else if (state == 1)
		{
			base.transform.position += tradjectory;
		}
		if (Mathf.Abs(base.transform.position.y) > 6f || Mathf.Abs(base.transform.position.x) > 8f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
