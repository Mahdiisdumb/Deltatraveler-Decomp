using UnityEngine;

public class PorkyShooterBullet : BulletBase
{
	[SerializeField]
	private Sprite[] walkSprites;

	[SerializeField]
	private Sprite[] shootSprites;

	private int spawnRate;

	private bool activated;

	private Vector3 toPos;

	private float yTradjectory = 2f;

	private float xTradjectory;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
		spawnRate = Random.Range(10, 24);
		if ((bool)Object.FindObjectOfType<PorkyHeliShooterAttack>())
		{
			spawnRate *= 2;
		}
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		if (state == 0)
		{
			base.transform.position = Vector3.MoveTowards(base.transform.position, toPos, 1f / 12f);
			sr.sprite = walkSprites[frames / 6 % 4];
			if (base.transform.position == toPos)
			{
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if (frames % (spawnRate * 2) == spawnRate)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyShooterBeamBullet"), base.transform.position + new Vector3(sr.flipX ? (-0.204f) : 0.204f, sr.flipY ? (-0.019f) : 0.019f), Quaternion.identity, base.transform.parent);
				sr.sprite = shootSprites[1];
			}
			else if (frames % (spawnRate * 2) == 0)
			{
				sr.sprite = shootSprites[0];
			}
		}
		else if (state == 2)
		{
			base.transform.position += new Vector3(xTradjectory, yTradjectory / 48f);
			yTradjectory -= 0.25f;
			if (xTradjectory > 0f)
			{
				base.transform.localEulerAngles -= new Vector3(0f, 0f, 2f);
			}
			else
			{
				base.transform.localEulerAngles += new Vector3(0f, 0f, 2f);
			}
		}
	}

	public void Activate(int i, int total, bool flip = false)
	{
		toPos = new Vector3(Mathf.Lerp(-1.322f, 1.322f, (float)i / ((float)total - 1f)), flip ? (-0.576f) : (-2.744f));
		base.transform.position = toPos - new Vector3(flip ? (-3.172f) : 3.172f, 0f);
		sr.flipX = flip;
		sr.flipY = flip;
		activated = true;
	}

	public override void Parry()
	{
		base.Parry();
		if (state == 0)
		{
			xTradjectory = 1f / 12f;
		}
		else
		{
			xTradjectory = Random.Range(-1f / 12f, 1f / 12f);
		}
		state = 2;
		sr.sortingOrder = 198;
		sr.maskInteraction = SpriteMaskInteraction.None;
	}
}
