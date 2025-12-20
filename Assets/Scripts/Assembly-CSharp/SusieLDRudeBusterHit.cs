using System;
using UnityEngine;

public class SusieLDRudeBusterHit : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private float velocity = 25f;

	private float decayRate = 0.75f;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 18;
		tpGrazeValue = 2f;
	}

	private void Update()
	{
		frames++;
		sr.sprite = sprites[frames / 2 % sprites.Length];
		baseDmg = Mathf.RoundToInt(Mathf.Lerp(18f, 14f, (float)(frames - 5) / 5f));
		velocity *= decayRate;
		base.transform.position += new Vector3(Mathf.Cos(base.transform.eulerAngles.z * ((float)Math.PI / 180f)), Mathf.Sin(base.transform.eulerAngles.z * ((float)Math.PI / 180f))) * velocity / 48f;
		base.transform.localScale = new Vector3(base.transform.localScale.x * 0.8f, 1f);
		sr.color = new Color(1f, 1f, 1f, sr.color.a - 0.05f);
		if (sr.color.a <= 0.2f)
		{
			GetComponent<Collider2D>().enabled = false;
		}
		if (sr.color.a <= 0f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void SetDecayRate(float decayRate)
	{
		this.decayRate = decayRate + 0.02f;
	}
}
