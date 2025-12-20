using UnityEngine;

public class FloweyPelletStandard : BulletBase
{
	private int frameOffset;

	private Vector3 velocity = Vector3.zero;

	private bool hardmode;

	private float speed = 6f;

	protected override void Awake()
	{
		base.Awake();
		PlaySFX("sounds/snd_floweypellet");
		baseDmg = 7;
		destroyOnHit = false;
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
		if (hardmode)
		{
			speed = 7.2f;
		}
	}

	private void Update()
	{
		frames++;
		if (frames == (hardmode ? 12 : 15) + frameOffset)
		{
			velocity = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, speed / 48f) - base.transform.position;
		}
		base.transform.position += velocity;
		if (frames % 6 == 3)
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/spr_spinbullet_outline_1");
		}
		else if (frames % 6 == 0)
		{
			sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/spr_spinbullet_outline_0");
		}
	}

	public void SetMovementOffset(int frameOffset)
	{
		this.frameOffset = frameOffset;
	}

	public void SetPremadeVelocity(Vector3 velocity)
	{
		this.velocity = velocity;
		frames = 30;
	}
}
