using UnityEngine;

public class PorkyGasBullet : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private Vector3 tradjectory;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 9;
		destroyOnHit = false;
		tradjectory = Vector3.MoveTowards(base.transform.position, Object.FindObjectOfType<SOUL>().transform.position, 1f / 24f) - base.transform.position;
		sr.color = new Color(1f, 1f, 1f, 0f);
	}

	private void Update()
	{
		frames++;
		base.transform.localScale = Vector3.Lerp(base.transform.localScale, new Vector3(1f, 1f, 1f), 0.05f);
		sr.color = Color.Lerp(sr.color, Color.white, 0.2f);
		sr.sprite = sprites[frames / 3 % sprites.Length];
		base.transform.position += tradjectory;
		if (base.transform.position.y <= -6f)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
