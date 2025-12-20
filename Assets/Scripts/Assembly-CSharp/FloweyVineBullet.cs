using UnityEngine;

public class FloweyVineBullet : BulletBase
{
	private bool activated = true;

	private Vector3 startDir = Vector3.down;

	private Vector3 midDir = Vector3.zero;

	private float velocity;

	private Vector3 midStartSpot = Vector3.zero;

	private bool reverse;

	private SpriteRenderer line;

	private bool hardmode;

	protected override void Awake()
	{
		base.Awake();
		PlaySFX("sounds/snd_spearappear");
		baseDmg = 8;
		destroyOnHit = false;
		GetComponent<BoxCollider2D>().enabled = false;
		sr.color = new Color32(192, 192, 192, byte.MaxValue);
		hardmode = (int)Util.GameManager().GetFlag(108) == 1;
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		if (!reverse)
		{
			frames++;
			if (frames >= (hardmode ? 12 : 15))
			{
				if (frames == (hardmode ? 12 : 15))
				{
					line.enabled = false;
					sr.color = Color.white;
					GetComponent<BoxCollider2D>().enabled = true;
					PlaySFX("sounds/snd_wallclaw");
					velocity = 1f;
				}
				base.transform.position += midDir * velocity;
				if (velocity < (float)(hardmode ? 24 : 20))
				{
					velocity += (hardmode ? 1.2f : 1f);
				}
				if (Mathf.Abs(base.transform.position.x) > 2.26f || base.transform.position.y < 0f || base.transform.position.y > 4f)
				{
					frames = 0;
					reverse = true;
				}
			}
			else
			{
				base.transform.position = Vector3.Lerp(base.transform.position, midStartSpot, hardmode ? 0.3f : 0.25f);
			}
		}
		else
		{
			base.transform.position -= midDir * (hardmode ? 7.2f : 6f);
		}
	}

	public void Activate(Vector2 startDir)
	{
		activated = true;
		this.startDir = startDir;
		midDir = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 48f) - base.transform.position;
		base.transform.right = base.transform.position - Object.FindObjectOfType<SOUL>().transform.position;
		midStartSpot = base.transform.position + midDir * 32f;
		line = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		line.transform.parent = base.transform.parent;
	}
}
