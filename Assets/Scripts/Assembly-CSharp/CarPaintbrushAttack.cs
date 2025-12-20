using UnityEngine;

public class CarPaintbrushAttack : AttackBase
{
	private bool gottenHit;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 180;
		bbSize = new Vector2(200f, 140f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		attackAllTargets = false;
	}

	protected override void Update()
	{
		if (!bb.IsPlaying())
		{
			base.Update();
			if (isStarted && frames % 25 == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PaintbrushBullet"), new Vector3(10f, 0f), Quaternion.identity, base.transform);
			}
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
		bb.StartMovement(new Vector2(200f, 200f));
	}
}
