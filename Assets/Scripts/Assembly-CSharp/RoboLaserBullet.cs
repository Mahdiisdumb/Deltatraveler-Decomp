using System;
using UnityEngine;

public class RoboLaserBullet : BulletBase
{
	private LilUFO ufo;

	private SpinRobo robo;

	private bool setToMove;

	private float angle = 180f;

	private bool activated;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		destroyOnHit = false;
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		if (base.transform.localScale.y < 120f)
		{
			base.transform.localScale += new Vector3(0f, 10f);
		}
		else
		{
			if (!setToMove)
			{
				if ((bool)ufo)
				{
					ufo.SetMovement(canMove: true);
				}
				if ((bool)robo)
				{
					robo.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Spinning Robo/spr_b_robo_body");
				}
				setToMove = true;
			}
			base.transform.position += new Vector3(0f - Mathf.Sin(angle * (MathF.PI / 180f)), Mathf.Cos(angle * (MathF.PI / 180f))) * 10f / 48f;
		}
		if (base.transform.position.y < -5f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Activate(LilUFO ufo)
	{
		this.ufo = ufo;
		base.transform.up = Util.FindObjectOfType<SOUL>().transform.position - base.transform.position;
		angle = base.transform.eulerAngles.z;
		activated = true;
	}

	public void Activate(SpinRobo robo)
	{
		this.robo = robo;
		base.transform.up = Util.FindObjectOfType<SOUL>().transform.position - base.transform.position;
		angle = base.transform.eulerAngles.z;
		activated = true;
	}
}
