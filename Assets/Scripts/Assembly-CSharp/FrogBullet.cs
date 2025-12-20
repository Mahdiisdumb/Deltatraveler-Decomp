using System;
using UnityEngine;

public class FrogBullet : BulletBase
{
	[SerializeField]
	private Sprite sprite;

	private int waitFrames;

	private int waitMaxFrames;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		waitMaxFrames = UnityEngine.Random.Range(30, 60);
	}

	private void Update()
	{
		if (waitFrames >= waitMaxFrames)
		{
			frames++;
			if (frames == 1)
			{
				GetComponent<SpriteRenderer>().sprite = sprite;
			}
			base.transform.position = new Vector3(Mathf.Lerp(1.18f, -1.22f, (float)frames / 17f), Mathf.Sin(Mathf.Lerp(0f, 90f, (float)frames / 17f) * ((float)Math.PI / 180f)) * 0.83f - 2.6f);
			if (frames == 18)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
		}
		else
		{
			waitFrames++;
		}
	}
}
