using UnityEngine;

public class PaintBlobAttack : AttackBase
{
	private bool gottenHit;

	private int count;

	private int worryRate = 1;

	protected override void Awake()
	{
		base.Awake();
		BlueCultist[] array = Util.FindObjectsOfType<BlueCultist>();
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
		maxFrames = 230;
		bbSize = new Vector2(260f, 140f);
		soulPos = new Vector2(-0.055f, -2.83f);
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(1);
		attackAllTargets = false;
	}

	protected override void Update()
	{
		base.Update();
		if (!isStarted)
		{
			return;
		}
		int num = (15 + (count - 1) * 10) / worryRate;
		if (frames % num == 1)
		{
			for (int i = 0; i < count; i++)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PaintBlobBullet"), base.transform);
			}
		}
	}

	private void OnDestroy()
	{
		BlueCultist[] array = Util.FindObjectsOfType<BlueCultist>();
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
		BlueCultist[] array = Util.FindObjectsOfType<BlueCultist>();
		foreach (BlueCultist blueCultist in array)
		{
			if (!blueCultist.IsDone() && blueCultist.LookingForHit())
			{
				string text = "";
				if ((int)Util.GameManager().GetFlag(102) == 1)
				{
					text = "_injured";
				}
				Util.FindObjectOfType<PartyPanels>().SetSprite(0, "spr_kr_paintedblue" + text);
				Util.FindObjectOfType<PartyPanels>().SetSprite(2, "spr_no_paintedblue");
				Util.FindObjectOfType<PartyPanels>().SetSprite(3, "spr_paula_paintedblue");
				blueCultist.RewardLooking();
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/eb/PaintBlobPlatforms"), base.transform);
		bb.StartMovement(new Vector2(260f, 160f));
	}
}
