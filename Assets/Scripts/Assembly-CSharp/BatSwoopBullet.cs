using System;
using UnityEngine;

public class BatSwoopBullet : BulletBase
{
	private int multi = 1;

	[SerializeField]
	private Sprite[] sprites;

	private float fromY;

	private float yMulti;

	private float fromX;

	private float toX;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		destroyOnHit = false;
		sr.color = new Color(1f, 1f, 1f, 0f);
	}

	private void Update()
	{
		frames++;
		if (frames <= 10)
		{
			sr.color = new Color(1f, 1f, 1f, (float)frames / 5f);
			base.transform.position += new Vector3(1f / 12f * (float)multi, (float)frames / 96f);
		}
		if (frames >= 20)
		{
			if (frames == 20)
			{
				Vector3 position = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position;
				fromX = base.transform.position.x;
				fromY = base.transform.position.y;
				toX = base.transform.position.x + Mathf.Abs(position.x - base.transform.position.x) * (float)multi * 2f;
				yMulti = position.y - base.transform.position.y;
			}
			base.transform.position = new Vector3(Mathf.Lerp(fromX, toX, (float)(frames - 20) / 60f), Mathf.Sin((float)((frames - 20) * 3) * ((float)Math.PI / 180f)) * yMulti + fromY);
			if (frames >= 75)
			{
				sr.color = new Color(1f, 1f, 1f, (float)(80 - frames) / 5f);
			}
		}
		sr.sprite = sprites[frames / 2 % sprites.Length];
		if (frames == 75)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Activate(bool swoopFromLeft)
	{
		if (!swoopFromLeft)
		{
			multi = -1;
		}
		base.transform.position = new Vector3(-3.45f * (float)multi, -0.5f);
	}
}
