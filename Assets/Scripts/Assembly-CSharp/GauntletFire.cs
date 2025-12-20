using System;
using UnityEngine;

public class GauntletFire : ActionBulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private GameObject prefab;

	private float yVelocity;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 7;
		prefab = Resources.Load<GameObject>("overworld/bullets/GauntletFireball");
	}

	protected override void Update()
	{
		base.Update();
		if (state == 0)
		{
			frames++;
			base.transform.localPosition = new Vector3(0f, Mathf.Lerp(6.134f, 5.42f, (float)frames / 30f));
			if (frames == 30)
			{
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 40)
			{
				frames = 0;
				state = 1;
				PlaySFX("sounds/snd_rocket");
			}
		}
		if (state != 1)
		{
			return;
		}
		frames++;
		int num = frames / 3;
		if (num > 4)
		{
			num = frames / 3 % 4 + 1;
		}
		if (num > 0)
		{
			GetComponent<BoxCollider2D>().enabled = true;
			base.transform.localPosition = new Vector3(Mathf.Lerp(-5f, 5f, (Mathf.Sin((float)((frames - 3) * 3) * ((float)Math.PI / 180f)) + 1f) / 2f), base.transform.localPosition.y);
			if (frames % 6 == 3)
			{
				UnityEngine.Object.Instantiate(prefab, base.transform.position + Vector3.down, Quaternion.identity, base.transform.parent);
			}
			if (frames >= 180)
			{
				yVelocity += 0.05f;
			}
			base.transform.localPosition += new Vector3(0f, yVelocity / 48f);
		}
		sr.sprite = sprites[num];
		if (frames == 270)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
