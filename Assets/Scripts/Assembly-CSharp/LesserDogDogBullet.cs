using System;
using UnityEngine;

public class LesserDogDogBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private bool runRight;

	private Vector3 velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
	}

	private void Update()
	{
		frames++;
		if (state == 0)
		{
			if (frames == 10)
			{
				PlaySFX("sounds/snd_doghurt1");
				runRight = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x < base.transform.position.x;
				sr.flipX = !runRight;
				sr.sprite = sprites[1];
			}
			if (frames < 25)
			{
				return;
			}
			sr.flipX = runRight;
			int num = (frames - 25) % 2;
			num += 2;
			sr.sprite = sprites[num];
			float num2 = 0.125f;
			base.transform.position += new Vector3(runRight ? num2 : (0f - num2), 0f);
			if (Mathf.Abs(base.transform.position.x) >= 1.4f)
			{
				PlaySFX("sounds/snd_jump");
				sr.sprite = sprites[4];
				float num3 = UnityEngine.Random.Range(25f, 75f);
				velocity = new Vector3(Mathf.Cos(num3 * ((float)Math.PI / 180f)), Mathf.Sin(num3 * ((float)Math.PI / 180f))) * num2;
				if (!runRight)
				{
					velocity.x *= -1f;
				}
				state = 1;
			}
		}
		else if (state == 1)
		{
			if ((velocity.x > 0f && base.transform.position.x >= 2.4f) || (velocity.x < 0f && base.transform.position.x <= -2.4f))
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
				sr.sprite = sprites[5];
				sr.flipX = !sr.flipX;
				PlaySFX("sounds/snd_crash");
				velocity.x *= -1f;
				velocity *= 1.06f;
			}
			if ((velocity.y > 0f && base.transform.position.y >= -1.1f) || (velocity.y < 0f && base.transform.position.y <= -3.07f))
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, UnityEngine.Random.Range(0, 360));
				sr.sprite = sprites[5];
				sr.flipY = !sr.flipY;
				PlaySFX("sounds/snd_crash");
				velocity.y *= -1f;
				velocity *= 1.06f;
			}
			base.transform.position += velocity;
		}
	}
}
