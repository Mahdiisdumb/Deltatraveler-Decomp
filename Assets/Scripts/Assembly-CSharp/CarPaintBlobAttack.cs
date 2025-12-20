using System.Collections.Generic;
using UnityEngine;

public class CarPaintBlobAttack : AttackBase
{
	private bool gottenHit;

	private int count;

	private int worryRate = 1;

	private List<Transform> rightSidePlatforms = new List<Transform>();

	private List<Transform> leftSidePlatforms = new List<Transform>();

	private readonly float[] yPos = new float[3] { -2.37f, -1.685f, -0.99f };

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 250;
		bbSize = new Vector2(260f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		if (frames % 12 == 1)
		{
			Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PaintBlobBullet"), base.transform);
		}
		if (frames % 40 == 1)
		{
			for (int i = 0; i < 3; i++)
			{
				int num = ((i % 2 != 1) ? 1 : (-1));
				Platform component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/ShortPlatform"), new Vector3(num * 3, yPos[i]), Quaternion.identity, base.transform).GetComponent<Platform>();
				component.ChangeSize(35);
				component.DisableGainTPOnLand();
				SpriteRenderer[] componentsInChildren = component.GetComponentsInChildren<SpriteRenderer>();
				for (int j = 0; j < componentsInChildren.Length; j++)
				{
					componentsInChildren[j].maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
				}
				if (num == -1)
				{
					leftSidePlatforms.Add(component.transform);
				}
				else
				{
					rightSidePlatforms.Add(component.transform);
				}
			}
		}
		List<Transform> list = new List<Transform>();
		foreach (Transform rightSidePlatform in rightSidePlatforms)
		{
			rightSidePlatform.transform.position -= new Vector3(1f / 24f, 0f);
			if (rightSidePlatform.transform.position.x < -3f)
			{
				list.Add(rightSidePlatform);
			}
		}
		foreach (Transform leftSidePlatform in leftSidePlatforms)
		{
			leftSidePlatform.transform.position += new Vector3(1f / 24f, 0f);
			if (leftSidePlatform.transform.position.x > 3f)
			{
				list.Add(leftSidePlatform);
			}
		}
		int num2 = list.Count;
		for (int k = 0; k < num2; k++)
		{
			rightSidePlatforms.Remove(list[0]);
			leftSidePlatforms.Remove(list[0]);
			Object.Destroy(list[0].gameObject);
			list.RemoveAt(0);
		}
	}

	private void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<Carpainter>() && !gottenHit && Object.FindObjectOfType<Carpainter>().LookingForAvoid())
		{
			Object.FindObjectOfType<Carpainter>().AddActPoints(25);
			if (Object.FindObjectOfType<Carpainter>().GetSatisfactionLevel() >= 100)
			{
				Object.FindObjectOfType<Carpainter>().Spare();
			}
		}
	}

	public void GetHit()
	{
		gottenHit = true;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(260f, 160f));
	}
}
