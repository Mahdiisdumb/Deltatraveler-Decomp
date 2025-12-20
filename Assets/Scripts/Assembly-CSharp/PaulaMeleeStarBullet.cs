using System;
using UnityEngine;

public class PaulaMeleeStarBullet : BulletBase
{
	private int angle;

	private Vector3 basePos;

	private Color color = Color.white;

	private bool spinReverse;

	private bool hard;

	private float reachout = 150f;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		destroyOnHit = false;
	}

	private void Update()
	{
		if (!hard)
		{
			frames++;
			Color b = color;
			b.a = 0f;
			float num = (float)frames / 60f;
			float num2 = num;
			num2 -= 1f;
			num2 = num2 * num2 * num2 + 1f;
			angle -= ((!spinReverse) ? 1 : (-1));
			base.transform.localPosition = basePos + Vector3.Lerp(Vector3.zero, new Vector3(Mathf.Cos((float)angle * ((float)Math.PI / 180f)), Mathf.Sin((float)angle * ((float)Math.PI / 180f))) * 2f, num2);
			GetComponent<SpriteRenderer>().color = Color.Lerp(color, b, (float)(frames - 30) / 10f);
			base.transform.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, spinReverse ? (-45) : 45), num);
			if (frames == 35)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else
		{
			frames++;
			float t = Mathf.Sin((float)frames * (180f / reachout) * ((float)Math.PI / 180f));
			float t2 = (float)frames / reachout;
			angle -= (spinReverse ? (-2) : 2);
			base.transform.localPosition = basePos + Vector3.Lerp(Vector3.zero, new Vector3(Mathf.Cos((float)angle * ((float)Math.PI / 180f)), Mathf.Sin((float)angle * ((float)Math.PI / 180f))) * 4f, t);
			base.transform.eulerAngles = Vector3.Lerp(Vector3.zero, new Vector3(0f, 0f, spinReverse ? (-30) : 30), t2);
			base.transform.localScale = Vector3.Lerp(new Vector3(0.65f, 0.65f, 1f), new Vector3(1.25f, 1.25f, 1f), t);
			if ((float)frames == reachout)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public void SetValues(int i, Vector3 basePos, bool spinReverse, bool hard = false)
	{
		this.spinReverse = spinReverse;
		this.basePos = basePos;
		angle = i * 45;
		if (hard)
		{
			this.hard = true;
			baseDmg = 12;
			color = new Color(1f, 1f, 0.5f);
			GetComponent<SpriteRenderer>().color = color;
		}
		if ((bool)UnityEngine.Object.FindObjectOfType<PaulaDeathAttack>())
		{
			baseDmg = 1;
		}
	}
}
