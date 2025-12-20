using UnityEngine;

public class GreaterDogDogBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private bool runRight;

	private Vector3 velocity;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 12;
		destroyOnHit = false;
		tpGrazeValueReuse = tpGrazeValue;
	}

	private void Update()
	{
		frames++;
		if (state == 0)
		{
			sr.flipX = Object.FindObjectOfType<SOUL>().transform.position.x > base.transform.position.x;
			sr.sprite = sprites[(frames < 30) ? (frames / 10 % 2) : (frames / 2 % 2)];
			if (frames == 40)
			{
				sr.sprite = sprites[2];
				PlaySFX("sounds/snd_jump");
				base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
				velocity = (Object.FindObjectOfType<SOUL>().transform.position - base.transform.position).normalized * 0.1875f;
				state = 1;
			}
		}
		else if (state == 1)
		{
			if ((velocity.x > 0f && base.transform.position.x >= 2f) || (velocity.x < 0f && base.transform.position.x <= -2f))
			{
				frames = 0;
				state = 2;
				base.transform.rotation = Quaternion.Euler(0f, 0f, 90f * Mathf.Sign(base.transform.position.x));
				sr.sprite = sprites[1];
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
			}
			else if ((velocity.y > 0f && base.transform.position.y >= -0.65f) || (velocity.y < 0f && base.transform.position.y <= -2.6f))
			{
				frames = 0;
				state = 2;
				base.transform.rotation = Quaternion.Euler(0f, 0f, (!(base.transform.position.y < -2f)) ? 180 : 0);
				sr.sprite = sprites[1];
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
			}
			base.transform.position += velocity;
		}
		else if (state == 2)
		{
			frames++;
			sr.sprite = sprites[frames / 2 % 2];
			if (frames == 6)
			{
				state = 0;
				frames = 39;
			}
		}
	}
}
