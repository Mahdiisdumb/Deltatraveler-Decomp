using System;
using UnityEngine;

public class JerrySlashYellow : BulletBase
{
	private float velocity = 1f / 6f;

	private float turnRate = 4f;

	private float minTurnRate = 3f;

	private int turnDecelerationFrames = 30;

	private bool onLeft;

	private float decelerateAngle = -180f;

	private GameObject echo;

	private float turnDeceleration;

	private float angle;

	private float speedMultiplier = 1.5f;

	private bool fall;

	private Vector3 fallDir = Vector3.zero;

	private bool big;

	private int bigHealth = 2;

	private bool red;

	protected override void Awake()
	{
		base.Awake();
		echo = Resources.Load<GameObject>("battle/attacks/bullets/jerry/JerrySlashEcho");
		baseDmg = UnityEngine.Object.FindObjectOfType<Jerry>().GetDamageValue();
		destroyOnHit = true;
		turnDeceleration = minTurnRate / (float)turnDecelerationFrames;
		decelerateAngle = UnityEngine.Random.Range(45, 60);
		aud.pitch = UnityEngine.Random.Range(0.9f, 1.1f);
		sr.color = new Color(1f, 1f, 0f);
	}

	private void Update()
	{
		if (velocity > 0f)
		{
			frames++;
		}
		if (velocity < 1f / 6f)
		{
			velocity += 1f / 30f;
		}
		sr.color = Color.Lerp(new Color(1f, 1f, 0f), Color.white, (float)frames / 10f);
		if (red && frames <= 10)
		{
			base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.Lerp(new Color(1f, 1f, 0f), Color.red, (float)frames / 10f);
		}
		float t = Mathf.Abs(Mathf.Sin((float)(frames * 36) * ((float)Math.PI / 180f)));
		base.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 0.8f, 1f), t);
		if (big)
		{
			if (frames <= 15)
			{
				base.transform.localScale *= Mathf.Lerp(1f, 2f, (float)frames / 15f);
			}
			else
			{
				base.transform.localScale *= ((bigHealth > 1) ? 2f : 1.5f);
			}
		}
		if (!fall && base.transform.position.y < 7f)
		{
			angle += turnRate * speedMultiplier;
			if (angle >= decelerateAngle && turnRate > 0f)
			{
				turnRate -= turnDeceleration * speedMultiplier;
				if (turnRate < 0f)
				{
					turnRate = 0f;
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
		}
		else if (fall)
		{
			Vector3 position = base.transform.position;
			base.transform.position += base.transform.right * velocity * speedMultiplier * (red ? 0.8f : 0.75f) * (big ? 1.15f : 1f);
			if (fallDir == Vector3.zero)
			{
				fallDir = (base.transform.position - position).normalized;
			}
			if (frames % 10 == 0 && velocity > 0f)
			{
				UnityEngine.Object.Instantiate(echo, base.transform.position, base.transform.rotation).GetComponent<JerrySlashEcho>().Activate(velocity * speedMultiplier, fallDir, base.transform.localScale, base.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
			}
			if (base.transform.position.y <= -7f)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
	}

	public override void GetShot(bool bigshot)
	{
		if (!big || bigshot || (big && bigHealth <= 1))
		{
			UnityEngine.Object.Instantiate(echo, base.transform.position, base.transform.rotation).GetComponent<JerrySlashEcho>().Activate(velocity * speedMultiplier, fallDir, base.transform.localScale, base.transform.GetChild(0).GetComponent<SpriteRenderer>().color);
			UnityEngine.Object.FindObjectOfType<SOULGraze>().AddTPBuildup(bigshot ? 0.2f : 0.25f);
			Util.GameManager().PlayGlobalSFX("sounds/snd_bomb");
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			bigHealth--;
			PlaySFX("sounds/snd_mtt_burst");
			UnityEngine.Object.FindObjectOfType<SOULGraze>().AddTPBuildup(0.2f);
			velocity = -1f / 6f;
		}
	}

	public void Activate(bool onLeft, float startAngle, int turnDecelerationFrames, float speedMultiplier, bool big, bool red)
	{
		this.onLeft = onLeft;
		angle = startAngle;
		this.speedMultiplier = speedMultiplier;
		float z = (onLeft ? (180f - angle) : angle);
		base.transform.eulerAngles = new Vector3(0f, 0f, z);
		this.big = big;
		this.red = red;
	}

	public void StartFalling()
	{
		base.transform.right = UnityEngine.Object.FindObjectOfType<SOUL>().transform.position + new Vector3(UnityEngine.Random.Range(-1f, 1f), 0f) - base.transform.position;
		fall = true;
		if (!red)
		{
			canGetShot = true;
		}
	}

	public override void PreSOULHit()
	{
		base.PreSOULHit();
		baseDmg = UnityEngine.Object.FindObjectOfType<Jerry>().GetDamageValue();
	}
}
