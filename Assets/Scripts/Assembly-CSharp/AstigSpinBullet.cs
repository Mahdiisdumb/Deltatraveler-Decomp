using System;
using UnityEngine;

public class AstigSpinBullet : BulletBase
{
	private int angleFrames;

	private bool reverseSpin;

	private bool isChild;

	private float distanceFromCenter;

	private AstigSpinBullet parent;

	private int waveFrames;

	private Vector3 tradjectory;

	private Vector3 centerPos;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
		destroyOnHit = false;
		angleFrames = UnityEngine.Random.Range(0, 90);
		reverseSpin = UnityEngine.Random.Range(0, 2) == 0;
		sr.color = new Color(1f, 1f, 1f, 0f);
		tradjectory = new Vector3(UnityEngine.Random.Range(-3f, -4f) / 48f, UnityEngine.Random.Range(-0.75f, 0.75f) / 48f);
		centerPos = base.transform.position;
	}

	private void Update()
	{
		if (frames < 10)
		{
			frames++;
			sr.color = new Color(1f, 1f, 1f, (float)frames / 10f);
		}
		if (isChild)
		{
			return;
		}
		angleFrames += ((!reverseSpin) ? 1 : (-1));
		waveFrames++;
		centerPos += tradjectory;
		base.transform.position = centerPos + new Vector3(0f, Mathf.Sin(15.652174f * (float)waveFrames * ((float)Math.PI / 180f)) / 24f);
		bool flag = false;
		float a = 1f;
		if ((double)base.transform.position.x < -2.48)
		{
			flag = true;
			a = (base.transform.position.x + 3.85f) / 1.37f;
			sr.color = new Color(1f, 1f, 1f, a);
		}
		for (int i = 0; i < base.transform.childCount; i++)
		{
			AstigSpinBullet component = base.transform.GetChild(i).GetComponent<AstigSpinBullet>();
			component.transform.localPosition = new Vector3(Mathf.Cos((float)(angleFrames * 4) * ((float)Math.PI / 180f)), Mathf.Sin((float)(angleFrames * 4) * ((float)Math.PI / 180f))) * component.GetDistanceFromCenter();
			if (flag)
			{
				component.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, a);
			}
		}
		if (base.transform.position.x < -3.85f)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public float GetDistanceFromCenter()
	{
		return distanceFromCenter;
	}

	public void SetAsChild(int i, float distanceFromCenter, AstigSpinBullet parent)
	{
		isChild = true;
		this.distanceFromCenter = distanceFromCenter;
		this.parent = parent;
		sr.sprite = Resources.Load<Sprite>("battle/attacks/bullets/hardmode/spr_astigbull_" + i);
		sr.sortingOrder = 150 - i;
		GetComponent<BoxCollider2D>().size = new Vector2(1f / (float)(2 + i), 1f / (float)(2 + i));
	}

	public void GenerateChildren()
	{
		for (int i = 0; i < 2; i++)
		{
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/hardmode/AstigSpinBullet"), base.transform).GetComponent<AstigSpinBullet>().SetAsChild(i + 1, (i == 0) ? 0.45f : 0.78f, this);
		}
	}
}
