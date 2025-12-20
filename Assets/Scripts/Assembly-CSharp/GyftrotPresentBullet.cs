using System;
using UnityEngine;

public class GyftrotPresentBullet : BulletBase
{
	private float degreeOffset;

	private float degree;

	private int typeSet;

	private int fakeTypeSet;

	private bool activated;

	private bool upSpin;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 10;
		destroyOnHit = false;
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		float num = (upSpin ? ((float)(frames - 45) / 50f) : ((float)(frames - 65) / 30f));
		if (num < 0f)
		{
			num = 0f;
		}
		num *= num;
		if (frames < 25)
		{
			if (frames % 10 == 0)
			{
				ChangeType(0);
			}
			else if (frames % 10 == 5)
			{
				ChangeType(fakeTypeSet);
			}
		}
		else if (upSpin)
		{
			if (frames <= 45)
			{
				degree = Mathf.Lerp(0f, 180f, (float)(frames - 25) / 20f);
			}
			else
			{
				degree = Mathf.Lerp(180f, 360f, (float)(frames - 45) / 50f);
			}
			if (frames == 46)
			{
				ChangeType(typeSet);
			}
		}
		else
		{
			if (frames <= 65)
			{
				degree = Mathf.Lerp(0f, 360f, (float)(frames - 25) / 40f);
			}
			if (frames == 66)
			{
				ChangeType(typeSet);
			}
		}
		float num2 = Mathf.Sin((degreeOffset + degree) * ((float)Math.PI / 180f));
		sr.sortingOrder = 150 + (int)(num2 * 10f);
		base.transform.position = new Vector3(-1.405f * num2, Mathf.Lerp(-2.396f, 1.33f, num));
	}

	public void SetParameters(int typeSet, float degreeOffset, bool upSpin, int fakeTypeSet = -1)
	{
		this.typeSet = typeSet;
		this.degreeOffset = degreeOffset;
		this.upSpin = upSpin;
		if (fakeTypeSet == -1)
		{
			this.fakeTypeSet = typeSet;
		}
		else
		{
			this.fakeTypeSet = fakeTypeSet;
		}
	}

	public void Activate()
	{
		activated = true;
	}
}
