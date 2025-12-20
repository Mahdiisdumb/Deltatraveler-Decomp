using System;
using UnityEngine;

public class LightningBullet : BulletBase
{
	[SerializeField]
	private Sprite[] warningSprites;

	private float angleOffset;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 15;
		destroyOnHit = false;
		angleOffset = UnityEngine.Random.Range(0, 360);
		aud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		tpGrazeValue = 2f;
		tpBuildRate = 0.25f;
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().flipX = UnityEngine.Random.Range(0, 2) == 0;
	}

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			base.transform.GetChild(1).GetComponent<SpriteRenderer>().sprite = warningSprites[frames / 4 % 2];
			if (frames == 20)
			{
				UnityEngine.Object.FindObjectOfType<BattleCamera>().BlastShake();
				base.transform.GetChild(1).GetComponent<SpriteRenderer>().enabled = false;
				base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
				base.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>()
					.enabled = true;
				GetComponent<BoxCollider2D>().enabled = true;
				aud.Play();
				frames = -1;
				state = 1;
			}
		}
		if (state == 1)
		{
			frames++;
			angleOffset += 12f;
			Color color = Color.Lerp(new Color(1f, 1f, 0f), new Color(0f, 1f, 1f), (Mathf.Sin(angleOffset * ((float)Math.PI / 180f)) + 1f) / 2f);
			color.a = Mathf.Lerp(182f, 0f, (float)(frames - 10) / 8f) / 255f;
			base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = color;
			base.transform.GetChild(0).localPosition = new Vector3(UnityEngine.Random.Range(-0.125f, 0.125f), UnityEngine.Random.Range(-0.125f, 0.125f));
			if (frames == 10)
			{
				base.transform.GetChild(0).GetChild(0).GetComponent<SpriteRenderer>()
					.enabled = false;
				GetComponent<BoxCollider2D>().enabled = false;
			}
			if (frames >= 18 && !aud.isPlaying)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}
}
