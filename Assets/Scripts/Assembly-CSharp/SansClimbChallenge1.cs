using UnityEngine;

public class SansClimbChallenge1 : AttackBase
{
	private Transform course;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 470;
		bbSize = new Vector2(375f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(1);
		course = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/sans/SansClimbChallenge1"), base.transform).transform;
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted)
		{
			course.position -= new Vector3(1f / 12f, 0f);
			if (frames == 106)
			{
				bb.StartMovement(new Vector2(375f, 300f));
			}
			if (course.position.x < -31f)
			{
				course.Find("TheBoneThatCould").localPosition -= new Vector3(1f / 24f, 0f);
			}
		}
	}
}
