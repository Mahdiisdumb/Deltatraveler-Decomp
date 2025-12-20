using System;
using UnityEngine;

public class FlyBulletHard : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private Vector3 pos1;

	private Vector3 pos2;

	private Vector3 idlePos;

	private Vector3 velocity;

	private int multiX = 1;

	private int multiY = 1;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		multiX = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
		multiY = ((UnityEngine.Random.Range(0, 2) != 0) ? 1 : (-1));
	}

	private void Update()
	{
		frames++;
		if (frames == 1)
		{
			velocity = Vector3.MoveTowards(base.transform.position, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position, 1f / 12f) - base.transform.position;
		}
		if (frames == 25)
		{
			velocity = Vector3.MoveTowards(base.transform.position, UnityEngine.Object.FindObjectOfType<SOUL>().transform.position, 1f / 12f) - base.transform.position;
		}
		if (frames > 25 || frames < 8)
		{
			base.transform.position += velocity;
		}
		else
		{
			if (frames == 8)
			{
				idlePos = base.transform.position;
			}
			float num = (0f - Mathf.Cos((float)(frames - 8) * 10.588235f * ((float)Math.PI / 180f)) + 1f) * 2f / 48f;
			float num2 = Mathf.Sin((float)(frames - 8) * 10.588235f * ((float)Math.PI / 180f)) * 4f / 48f;
			base.transform.position = idlePos + new Vector3(num2 * (float)multiX, num * (float)multiY);
		}
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 5 % 2];
	}
}
