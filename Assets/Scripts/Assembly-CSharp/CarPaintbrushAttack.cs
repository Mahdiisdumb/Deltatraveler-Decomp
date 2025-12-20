using UnityEngine;

public class CarPaintbrushAttack : AttackBase
{
	private bool gottenHit;

	protected override void Awake()
	{
		base.Awake();
		maxFrames = 180;
		bbSize = new Vector2(200f, 140f);
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
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
		if ((bool)Util.FindObjectOfType<Carpainter>() && !gottenHit && Util.FindObjectOfType<Carpainter>().LookingForAvoid())
		{
			Util.FindObjectOfType<Carpainter>().AddActPoints(25);
			if (Util.FindObjectOfType<Carpainter>().GetSatisfactionLevel() >= 100)
			{
				Util.FindObjectOfType<Carpainter>().Spare();
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
