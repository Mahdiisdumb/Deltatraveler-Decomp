using UnityEngine;

public class PaintbrushAttack : AttackBase
{
	private bool gottenHit;

	private int count;

	private int worryRate = 1;

	protected override void Awake()
	{
		base.Awake();
		BlueCultist[] array = Object.FindObjectsOfType<BlueCultist>();
		foreach (BlueCultist obj in array)
		{
			if (!obj.IsDone())
			{
				count++;
			}
			if (obj.IsWorried())
			{
				worryRate = 2;
			}
		}
		maxFrames = 160;
		bbSize = new Vector2(200f, 140f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		attackAllTargets = false;
	}

	protected override void Update()
	{
		if (bb.IsPlaying())
		{
			return;
		}
		base.Update();
		if (!isStarted)
		{
			return;
		}
		int num = (20 + (count - 1) * 12) / worryRate;
		if (frames % num == 1)
		{
			for (int i = 0; i < count; i++)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PaintbrushBullet"), new Vector3(10f, 0f), Quaternion.identity, base.transform);
			}
		}
	}

	private void OnDestroy()
	{
		BlueCultist[] array = Object.FindObjectsOfType<BlueCultist>();
		foreach (BlueCultist blueCultist in array)
		{
			if (!blueCultist.IsDone())
			{
				if (!gottenHit && blueCultist.LookingForAvoid())
				{
					blueCultist.RewardLooking();
				}
				blueCultist.ResetLookVariables();
			}
		}
	}

	public void GetHit()
	{
		gottenHit = true;
		BlueCultist[] array = Object.FindObjectsOfType<BlueCultist>();
		foreach (BlueCultist blueCultist in array)
		{
			if (!blueCultist.IsDone() && blueCultist.LookingForHit())
			{
				string text = "";
				if ((int)Object.FindObjectOfType<GameManager>().GetFlag(102) == 1)
				{
					text = "_injured";
				}
				Object.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_paintedblue" + text);
				Object.FindObjectOfType<PartyPanels>().SetSprite(2, "spr_no_paintedblue");
				Object.FindObjectOfType<PartyPanels>().SetSprite(3, "spr_paula_paintedblue");
				blueCultist.RewardLooking();
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		bb.StartMovement(new Vector2(200f, 200f));
	}
}
