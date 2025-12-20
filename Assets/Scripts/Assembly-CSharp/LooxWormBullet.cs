using System;
using UnityEngine;

public class LooxWormBullet : BulletBase
{
	private Vector3 basePosition = Vector3.zero;

	private Vector3 velocity = Vector3.zero;

	private Vector3 origPosition = Vector3.zero;

	private int level;

	[SerializeField]
	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 5;
	}

	private void Update()
	{
		frames++;
		basePosition += velocity;
		base.transform.position = basePosition + new Vector3(0f, Mathf.Cos((float)(12 * frames) * ((float)Math.PI / 180f)) / 7f);
		if (frames == 4 && level < 2)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/ruins/LooxWormBullet"), origPosition, Quaternion.identity, base.transform.parent).GetComponent<LooxWormBullet>().Activate(origPosition, velocity, level + 1);
		}
		if (Mathf.Abs(base.transform.position.x) > 1.8f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public void Activate(Vector3 basePosition, Vector3 velocity, int level)
	{
		this.basePosition = basePosition;
		base.transform.position = basePosition;
		origPosition = basePosition;
		this.velocity = velocity;
		this.level = level;
		GetComponent<SpriteRenderer>().sprite = sprites[level];
	}
}
