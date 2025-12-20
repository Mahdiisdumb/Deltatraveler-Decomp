using System;
using UnityEngine;

public class JerrySlashRed : BulletBase
{
	private float velocity = 1f / 6f;

	private float turnRate = 5f;

	private float minTurnRate = 3f;

	private int turnDecelerationFrames = 30;

	private bool onLeft;

	private float decelerateAngle = -180f;

	private GameObject echo;

	private float turnDeceleration;

	private float angle;

	private float speedMultiplier = 1.5f;

	protected override void Awake()
	{
		base.Awake();
		echo = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashEcho");
		baseDmg = UnityEngine.Object.FindObjectOfType<Jerry>().GetDamageValue();
		destroyOnHit = true;
		turnDeceleration = minTurnRate / (float)turnDecelerationFrames;
		decelerateAngle = UnityEngine.Random.Range(-145f, -190f);
		aud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		sr.color = Color.red;
	}

	private void Update()
	{
		frames++;
		sr.color = Color.Lerp(Color.red, Color.white, (float)frames / 10f);
		float t = Mathf.Abs(Mathf.Sin((float)(frames * 36) * ((float)Math.PI / 180f)));
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 0.8f, 1f), t);
		angle -= turnRate * speedMultiplier;
		if (angle <= 0f && turnRate > 3f)
		{
			turnRate -= turnDeceleration * speedMultiplier;
		}
		else if (angle <= decelerateAngle && turnRate > 0.5f)
		{
			turnRate -= turnDeceleration * speedMultiplier;
			if (turnRate < 0.5f)
			{
				turnRate = 0.5f;
			}
		}
		float num = (onLeft ? (180f - angle) : angle);
		base.transform.eulerAngles = new Vector3(0f, 0f, num);
		Vector3 vector = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)));
		base.transform.position += vector * velocity * speedMultiplier;
		if (frames % 10 == 0)
		{
			UnityEngine.Object.Instantiate(echo, base.transform.position, base.transform.rotation).GetComponent<JerrySlashEcho>().Activate(velocity * speedMultiplier, vector, base.transform.localScale, base.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
		}
		if ((onLeft && base.transform.position.x > 7f) || (!onLeft && base.transform.position.x < -7f) || (angle <= decelerateAngle && base.transform.position.y >= 6f))
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Activate(bool onLeft, float startAngle, float minTurnRate, int turnDecelerationFrames, float speedMultiplier)
	{
		this.onLeft = onLeft;
		angle = startAngle;
		this.minTurnRate = minTurnRate;
		turnRate = minTurnRate * ((angle - 60f) / 30f);
		if (turnRate < minTurnRate)
		{
			turnRate = minTurnRate;
		}
		turnRate *= 1.5f;
		this.speedMultiplier = speedMultiplier;
		float z = (onLeft ? (180f - angle) : angle);
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
	}

	public override void PreSOULHit()
	{
		base.PreSOULHit();
		baseDmg = UnityEngine.Object.FindObjectOfType<Jerry>().GetDamageValue();
	}
}
