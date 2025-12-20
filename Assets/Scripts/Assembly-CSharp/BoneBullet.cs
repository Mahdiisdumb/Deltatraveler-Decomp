using UnityEngine;

public class BoneBullet : BulletBase
{
	[SerializeField]
	private float adjustableHeight = 1f;

	private float height = 1f;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 8;
		karmaImpact = 6;
		ChangeHeight(adjustableHeight);
	}

	private void Update()
	{
		if (height != adjustableHeight)
		{
			ChangeHeight(adjustableHeight);
		}
	}

	public override void ChangeType(string tag)
	{
		base.ChangeType(tag);
		base.transform.Find("top").GetComponent<SpriteRenderer>().color = sr.color;
		base.transform.Find("bot").GetComponent<SpriteRenderer>().color = sr.color;
		base.transform.Find("body").GetComponent<SpriteRenderer>().color = sr.color;
	}

	public void ChangeHeight(float height)
	{
		this.height = height;
		adjustableHeight = height;
		GetComponent<BoxCollider2D>().size = new Vector2(0.1f, 0.125f * height);
		base.transform.Find("top").localPosition = new Vector3(0f, 0.0625f * height);
		base.transform.Find("bot").localPosition = new Vector3(0f, -0.0625f * height);
		base.transform.Find("body").localScale = new Vector3(1f, height, 1f);
	}
}
