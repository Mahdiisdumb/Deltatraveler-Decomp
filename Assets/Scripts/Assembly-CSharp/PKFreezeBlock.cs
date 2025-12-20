using UnityEngine;

public class PKFreezeBlock : BulletBase
{
	private bool forming = true;

	private PKFreezeBlock top;

	private PKFreezeBlock bot;

	private int id;

	private float velocity;

	private bool spawnSnow;

	private int snowFrames;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
		sr.color = new Color(1f, 1f, 1f, 0f);
		spawnSnow = !Object.FindObjectOfType<Ness>();
		snowFrames = Random.Range(0, 17);
		if (!spawnSnow && (bool)Object.FindObjectOfType<Ness>())
		{
			spawnSnow = Object.FindObjectOfType<Ness>().IsKilled();
		}
		if (spawnSnow && (bool)Object.FindObjectOfType<PaulaRandomPatternsAttack>())
		{
			spawnSnow = false;
		}
	}

	private void OnDestroy()
	{
		if (top != null && bot != null)
		{
			top.SetBottomBlock(bot, id);
			bot.SetTopBlock(top);
		}
		else if (top == null && bot != null)
		{
			bot.SetTopBlock(null);
		}
		else if (top != null && bot == null)
		{
			top.SetBottomBlock(null, 0);
		}
	}

	private void Update()
	{
		if (forming)
		{
			frames++;
			sr.color = new Color(1f, 1f, 1f, (float)frames / 10f);
			if (frames == 10)
			{
				forming = false;
			}
			return;
		}
		float num = -2.76f + 0.52f * (float)id;
		if (!(base.transform.position.y > num))
		{
			return;
		}
		if (spawnSnow)
		{
			snowFrames++;
			if (snowFrames % 24 == 0)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/nesspaula/PKFreezeSnowBullet"), base.transform.position, Quaternion.identity, base.transform.parent);
			}
		}
		base.transform.position -= new Vector3(0f, velocity / 48f);
		velocity += 0.25f;
		if (base.transform.position.y <= num)
		{
			velocity = 0f;
			base.transform.position = new Vector3(base.transform.position.x, num);
			aud.Play();
		}
	}

	public bool IsForming()
	{
		return forming;
	}

	public void SetTopBlock(PKFreezeBlock top)
	{
		this.top = top;
	}

	public void SetBottomBlock(PKFreezeBlock bot, int id = -1)
	{
		this.bot = bot;
		if (id != -1)
		{
			SetThisAndTopID(id);
		}
	}

	public void SetThisAndTopID(int id)
	{
		this.id = id;
		if (top != null)
		{
			top.SetThisAndTopID(id + 1);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<PKFireFlame>() || (bool)collision.GetComponent<LightningBullet>() || (bool)collision.GetComponent<FireballBullet>())
		{
			Object.Destroy(base.gameObject);
		}
	}
}
