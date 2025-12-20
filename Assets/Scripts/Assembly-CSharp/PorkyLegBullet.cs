using UnityEngine;

public class PorkyLegBullet : BulletBase
{
	private bool activated;

	private bool playSounds;

	private bool goingDown = true;

	private bool parried;

	private int leg;

	private Vector3 dir;

	private float velocity;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		if (goingDown)
		{
			frames++;
			if (frames < 15)
			{
				return;
			}
			if (frames == 15)
			{
				base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				if (playSounds)
				{
					PlaySFX("sounds/snd_spearrise");
				}
			}
			velocity += 1f;
			base.transform.position += dir * velocity / 48f;
			if ((dir.y == 0f && Mathf.Abs(base.transform.position.x) >= 1.629f) || (dir.x == 0f && Mathf.Abs(base.transform.position.y + 1.66f) >= 1.36f))
			{
				goingDown = false;
				GetComponent<AudioSource>().volume = 0.5f;
				PlaySFX("sounds/snd_crash");
				Object.FindObjectOfType<BattleCamera>().BlastShake();
			}
		}
		else
		{
			base.transform.position -= dir * 10f / 48f;
			if ((dir.y == 0f && Mathf.Abs(base.transform.position.x) >= 2f) || (dir.x == 0f && Mathf.Abs(base.transform.position.y + 1.66f) >= 2f))
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	private void LateUpdate()
	{
		if (activated)
		{
			GetComponent<SpriteRenderer>().color = Object.FindObjectOfType<Porky>().GetLegColor(leg);
		}
	}

	public void Activate(int leg, bool playSounds)
	{
		this.leg = leg;
		this.playSounds = playSounds;
		if (playSounds)
		{
			PlaySFX("sounds/snd_encounter");
		}
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
		if (Random.Range(0, 2) == 0)
		{
			dir = ((Random.Range(0, 2) == 0) ? Vector3.right : Vector3.left);
			base.transform.position = new Vector3(-1.619f * dir.x, Random.Range(-2.873f, -0.493f));
			base.transform.localEulerAngles = new Vector3(0f, 0f, 90f * dir.x);
		}
		else
		{
			dir = ((Random.Range(0, 2) == 0) ? Vector3.up : Vector3.down);
			base.transform.position = new Vector3(Random.Range(-1.392f, 1.392f), 1.35f * (0f - dir.y) - 1.66f);
			base.transform.localEulerAngles = new Vector3(0f, 0f, (dir.y != -1f) ? 180 : 0);
		}
		baseDmg = 9 - Object.FindObjectOfType<Porky>().GetLegStates()[leg];
		activated = true;
	}

	public override void Parry()
	{
		if (!parried)
		{
			parried = true;
			GetComponent<PolygonCollider2D>().enabled = false;
			Object.FindObjectOfType<Porky>().ToppleSpecificLeg(leg);
			Object.FindObjectOfType<TPBar>().AddTP(2);
		}
	}
}
