using System;
using UnityEngine;

public class GauntletBallBullet : ActionBulletBase
{
	private bool raising;

	private float lowerVelocity = 1f;

	private AudioSource chainAud;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 8;
		chainAud = GetComponents<AudioSource>()[1];
	}

	protected override void Update()
	{
		base.Update();
		if (!raising)
		{
			lowerVelocity += 0.75f;
			base.transform.localPosition += new Vector3(0f, (0f - lowerVelocity) / 48f);
			if (base.transform.localPosition.y <= 8.21f)
			{
				raising = true;
				lowerVelocity = 0f;
				chainAud.pitch = 1f;
				chainAud.volume = 0.3f;
			}
			return;
		}
		if (frames % 40 == 0 && base.transform.localPosition.y < 17.77f)
		{
			PlaySFX("sounds/snd_heavyswing");
		}
		frames++;
		base.transform.eulerAngles = new Vector3(0f, 0f, Mathf.Lerp(-25f, 25f, (Mathf.Sin((float)frames * 4.5f * ((float)Math.PI / 180f)) + 1f) / 2f));
		if (frames <= 180)
		{
			float num = Mathf.Abs((1f - Mathf.Cos((float)(frames * 2) * ((float)Math.PI / 180f))) / 2f);
			base.transform.localPosition = new Vector3(0f, Mathf.Lerp(8.21f, 16.68f, num));
			float t = (0.5f - Mathf.Abs(0.5f - num)) * 2f;
			chainAud.volume = Mathf.Lerp(0.3f, 0.6f, t);
		}
		else
		{
			if (lowerVelocity < 6f)
			{
				lowerVelocity += 0.2f;
			}
			base.transform.localPosition += new Vector3(0f, lowerVelocity / 48f);
			if (base.transform.localPosition.y < 17.77f)
			{
				chainAud.volume = Mathf.Lerp(0.3f, 0.6f, lowerVelocity / 6f);
			}
			else
			{
				chainAud.volume = (19.3f - base.transform.localPosition.y) / 3.06f;
			}
		}
		AfterImage afterImage = new GameObject("BallAfterImage" + frames, typeof(SpriteRenderer)).AddComponent<AfterImage>();
		afterImage.CreateAfterImage(sr.sprite, base.transform.position);
		afterImage.transform.eulerAngles = base.transform.eulerAngles;
		afterImage.GetComponent<SpriteRenderer>().sortingOrder = sr.sortingOrder - 1;
		if (base.transform.localPosition.y >= 19.3f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
