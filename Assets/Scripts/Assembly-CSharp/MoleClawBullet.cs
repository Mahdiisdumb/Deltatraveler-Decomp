using System;
using UnityEngine;

public class MoleClawBullet : BulletBase
{
	private bool behind = true;

	private bool activated;

	private bool right;

	private bool hugMode;

	private int difficultyLevel;

	private float speed;

	private float tracking;

	private int trackingFrames;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 7;
		if (UnityEngine.Object.FindObjectOfType<MondoMole>().IsLectured())
		{
			baseDmg = 6;
		}
		destroyOnHit = false;
		GetComponent<BoxCollider2D>().enabled = false;
		tpGrazeValue = 1.5f;
	}

	private void Update()
	{
		if (behind)
		{
			grazed = false;
		}
		if (state != 2)
		{
			GetComponent<BoxCollider2D>().enabled = !behind;
		}
		else
		{
			GetComponent<BoxCollider2D>().enabled = false;
		}
		sr.sortingOrder = (behind ? 50 : 199);
		if (state == 0)
		{
			frames++;
			if (frames == 1 || frames == 16)
			{
				sr.enabled = true;
			}
			float num = (float)frames / 30f;
			num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			Vector3 b = (right ? new Vector3(0.9f, -4.24f) : new Vector3(-0.9f, 0.22f));
			if (hugMode)
			{
				b = new Vector3(right ? 2.61f : (-2.61f), -1.66f);
			}
			base.transform.position = Vector3.Lerp(new Vector3(0f, -1.66f), b, num);
			if (frames == 30)
			{
				frames = 0;
				if (!hugMode)
				{
					behind = right;
				}
				else
				{
					behind = false;
					if (difficultyLevel >= 3 && right)
					{
						trackingFrames = -(16 - 2 * difficultyLevel);
					}
				}
				state = 1;
			}
		}
		else if (state == 1)
		{
			if (hugMode)
			{
				float num2 = 16 - 2 * difficultyLevel;
				if (difficultyLevel >= 3)
				{
					num2 *= 2f;
				}
				if ((float)trackingFrames <= num2)
				{
					trackingFrames++;
					tracking += 1f / (num2 - 2f);
					base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(right ? 2.61f : (-2.61f), UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y), tracking);
					if ((float)trackingFrames == num2)
					{
						grazed = false;
					}
				}
				else
				{
					float num3 = ((difficultyLevel >= 3) ? 12 : 8);
					base.transform.position -= new Vector3((right ? num3 : (0f - num3)) / 48f, 0f);
					if ((difficultyLevel >= 3) ? (Mathf.Abs(base.transform.position.x) >= 2.65f) : (Mathf.Abs(base.transform.position.x) <= 0.9f))
					{
						tracking = 0f;
						trackingFrames = 0;
						if (difficultyLevel >= 3)
						{
							right = !right;
							sr.flipX = right;
						}
					}
				}
			}
			else
			{
				base.transform.position += new Vector3(0f, speed) / (behind ? 48f : (-48f));
				if (base.transform.position.y <= -4.24f && !behind)
				{
					behind = true;
				}
				else if (base.transform.position.y >= -0.22f && behind)
				{
					behind = false;
				}
			}
		}
		else if (state == 2)
		{
			base.transform.position = Vector3.Lerp(base.transform.position, new Vector3(right ? 0.9f : (-0.9f), 0.22f), 0.5f);
		}
		GetComponent<BoxCollider2D>().offset = new Vector2(Mathf.Abs(GetComponent<BoxCollider2D>().offset.x) * (float)(right ? 1 : (-1)), GetComponent<BoxCollider2D>().offset.y);
	}

	public void Grabbed()
	{
		state = 2;
		GetComponent<BoxCollider2D>().enabled = false;
	}

	public void Activate(bool right, bool hugMode, int difficultyLevel)
	{
		this.difficultyLevel = difficultyLevel;
		speed = 8 + difficultyLevel;
		activated = true;
		this.right = right;
		sr.flipX = right;
		this.hugMode = hugMode;
		if (!hugMode)
		{
			frames = 15;
		}
	}
}
