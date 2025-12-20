using UnityEngine;

public class PorkyHelicopterBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private int dropFrames;

	private int dropRate;

	private bool reverse;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		destroyOnHit = false;
		sr.color = new Color(1f, 1f, 1f, 0f);
		dropRate = Random.Range(35, 45);
		dropFrames = Random.Range(0, dropRate);
		base.transform.position = new Vector3(-2.75f, Random.Range(-0.34f, 0.136f));
	}

	private void Update()
	{
		frames++;
		dropFrames++;
		if (Mathf.Abs(base.transform.position.x) <= 1.455f && dropFrames % dropRate == 0)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyNukeBullet"), base.transform.position, Quaternion.identity, base.transform.parent);
		}
		sr.sprite = sprites[frames / 2 % 2];
		base.transform.position += new Vector3(1f / 24f * (float)((!reverse) ? 1 : (-1)), 0f);
		if (frames < 10)
		{
			sr.color = new Color(1f, 1f, 1f, (float)frames / 10f);
			return;
		}
		sr.color = new Color(1f, 1f, 1f, 3.72f - Mathf.Abs(base.transform.position.x));
		if (Mathf.Abs(base.transform.position.x) >= 3.72f)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void Reverse()
	{
		reverse = true;
		sr.flipX = true;
		base.transform.position = new Vector3(0f - base.transform.position.x, base.transform.position.y);
	}

	public override void Parry()
	{
		base.Parry();
		for (int i = 0; i < 12; i++)
		{
			int angle = i * 30;
			float distance = 2f;
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyFlameBullet"), base.transform.position, Quaternion.identity, base.transform.parent).GetComponent<PorkyFlameBullet>().Activate(angle, distance, i < 2);
		}
		Object.Destroy(base.gameObject);
	}
}
