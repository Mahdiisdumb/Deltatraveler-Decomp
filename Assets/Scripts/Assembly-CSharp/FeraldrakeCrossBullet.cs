using System;
using UnityEngine;

public class FeraldrakeCrossBullet : BulletBase
{
	private bool activated;

	private bool sound;

	private Vector3 dir = Vector2.up;

	private float distance = 1.1875f;

	private Vector3 origin;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 8;
		destroyOnHit = false;
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		if (frames % 2 == 0)
		{
			base.transform.rotation = Quaternion.Euler(0f, 0f, base.transform.rotation.eulerAngles.z - 90f);
		}
		if (frames <= 15)
		{
			float num = (float)frames / 15f;
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			distance = Mathf.Lerp(57f, 77f, num) / 48f;
			sr.color = new Color(1f, 1f, 1f, num);
		}
		else if (frames < 35)
		{
			float num2 = (float)(frames - 15) / 20f;
			num2 *= num2;
			float num3 = distance;
			distance = Mathf.Lerp(77f, -57f, num2) / 48f;
			if (sound && num3 > 0f && distance <= 0f)
			{
				PlaySFX("sounds/snd_shakerbreaker");
			}
			else if (num3 < 0f && distance < 0f)
			{
				GetComponent<CircleCollider2D>().enabled = false;
			}
		}
		else
		{
			float num4 = (float)(frames - 35) / 10f;
			num4 = Mathf.Sin(num4 * (float)Math.PI * 0.5f);
			distance = Mathf.Lerp(-57f, -77f, num4) / 48f;
			sr.color = new Color(1f, 1f, 1f, 1f - num4);
		}
		base.transform.position = origin + dir * distance;
		if (frames >= 45)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Activate(Vector3 origin, Vector2 dir, bool sound)
	{
		this.dir = dir;
		this.sound = sound;
		this.origin = origin;
		base.transform.position = origin + this.dir * distance;
		sr.color = new Color(1f, 1f, 1f, 0f);
		activated = true;
	}
}
