using System;
using UnityEngine;

public class BearClawBullet : BulletBase
{
	private bool behind = true;

	private bool activated;

	private bool right;

	private bool hugMode;

	private float speed;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 11;
		if (UnityEngine.Object.FindObjectOfType<MightyBear>().IsLectured())
		{
			baseDmg = 8;
		}
		destroyOnHit = false;
		GetComponent<BoxCollider2D>().enabled = false;
		tpGrazeValue = 1.5f;
	}

	private void Update()
	{
		if (behind)
		{
			grazed = false;
		}
		GetComponent<BoxCollider2D>().enabled = !behind;
		sr.sortingOrder = (behind ? 50 : 199);
		if (state == 0)
		{
			frames++;
			if (frames == 1 || frames == 16)
			{
				sr.enabled = true;
			}
			float num = (float)frames / 30f;
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			Vector3 b = (right ? new Vector3(0.9f, -4.24f) : new Vector3(-0.9f, 0.22f));
			if (hugMode)
			{
				b = new Vector3(right ? 2.61f : (-2.61f), -1.66f);
			}
			base.transform.position = Vector3.Lerp(new Vector3(0f, -1.66f), b, num);
			if (frames == 30)
			{
				if (!hugMode)
				{
					behind = right;
				}
				else
				{
					behind = false;
				}
				state = 1;
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if (hugMode)
			{
				base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(right ? 0.9f : (-0.9f), -1.66f), 0.25f);
				return;
			}
			base.transform.position += new Vector3(0f, speed) / (behind ? 48f : (-48f));
			if (base.transform.position.y <= -4.24f && !behind)
			{
				behind = true;
			}
			else if (base.transform.position.y >= -0.22f && behind)
			{
				behind = false;
			}
		}
	}

	public void Activate(bool right, bool hugMode, float speed)
	{
		this.speed = speed;
		if (UnityEngine.Object.FindObjectOfType<MightyBear>().IsLectured())
		{
			this.speed *= 0.75f;
		}
		activated = true;
		this.right = right;
		sr.flipX = right;
		this.hugMode = hugMode;
		if (!hugMode)
		{
			frames = 15;
		}
	}
}
