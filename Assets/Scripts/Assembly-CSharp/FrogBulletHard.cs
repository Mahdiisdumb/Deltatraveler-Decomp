using System;
using UnityEngine;

public class FrogBulletHard : BulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	private bool onCeil;

	private bool onWalls;

	private bool jumping;

	private float angle;

	private bool increaseTrajectory;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
		tpGrazeValueReuse = 1f;
	}

	private void Update()
	{
		if (!jumping)
		{
			frames++;
			if (frames == 20)
			{
				base.transform.right = base.transform.position - UnityEngine.Object.FindObjectOfType<SOUL>().transform.position;
				angle = base.transform.eulerAngles.z;
				if (onWalls)
				{
					base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
				}
				else
				{
					base.transform.eulerAngles = Vector3.zero;
					increaseTrajectory = angle > 270f;
				}
				if (onCeil)
				{
					base.transform.eulerAngles += new Vector3(0f, 0f, 180f);
				}
				sr.sprite = sprites[1];
				jumping = true;
			}
			return;
		}
		if (onWalls)
		{
			if (UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.y >= 0f)
			{
				increaseTrajectory = true;
			}
			else
			{
				increaseTrajectory = false;
			}
		}
		else if (UnityEngine.Object.FindObjectOfType<SOUL>().transform.position.x >= 0f)
		{
			increaseTrajectory = true;
		}
		else
		{
			increaseTrajectory = false;
		}
		if (onCeil)
		{
			increaseTrajectory = !increaseTrajectory;
		}
		Vector3 vector = new Vector3(0f - Mathf.Cos(angle * ((float)Math.PI / 180f)), 0f - Mathf.Sin(angle * ((float)Math.PI / 180f))) * 6.5f / 48f;
		angle += (increaseTrajectory ? 1 : (-1));
		base.transform.position += vector;
		if (base.transform.position.y <= -2.6f || base.transform.position.y >= -0.74f)
		{
			onWalls = false;
			onCeil = base.transform.position.y > -1f;
			HandleLandFloor();
		}
		else if (Mathf.Abs(base.transform.position.x) >= 1.3f)
		{
			onWalls = true;
			onCeil = base.transform.position.x > 0f;
			HandleLandFloor();
		}
	}

	private void HandleLandFloor()
	{
		jumping = false;
		frames = 12;
		if (onWalls)
		{
			base.transform.eulerAngles = new Vector3(0f, 0f, -90f);
		}
		else
		{
			base.transform.eulerAngles = Vector3.zero;
		}
		if (onCeil)
		{
			base.transform.eulerAngles += new Vector3(0f, 0f, 180f);
		}
		float num = base.transform.position.x;
		if (num > 1.3f)
		{
			num = 1.3f;
		}
		else if (num < -1.3f)
		{
			num = -1.3f;
		}
		float num2 = base.transform.position.y;
		if (num2 > -0.74f)
		{
			num2 = -0.74f;
		}
		else if (num2 < -2.6f)
		{
			num2 = -2.6f;
		}
		base.transform.position = new Vector3(num, num2);
		sr.sprite = sprites[0];
	}
}
