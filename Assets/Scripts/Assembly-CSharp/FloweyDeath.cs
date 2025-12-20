using UnityEngine;

public class FloweyDeath : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(true, false, false);
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		frames++;
		if (frames >= 30 && frames < 60)
		{
			int num = (frames - 30) / 3;
			if (num < 5)
			{
				Object.FindObjectOfType<Flowey>().SetFace("die_" + num);
			}
		}
		if (frames >= 90)
		{
			int num2 = (frames - 90) / 15 + 5;
			if (num2 < 8)
			{
				Object.FindObjectOfType<Flowey>().SetFace("die_" + num2);
			}
		}
		if (frames == 150 && (int)Util.GameManager().GetFlag(108) == 1)
		{
			Object.FindObjectOfType<Flowey>().TriggerKrisFalling();
		}
		if (frames == 180)
		{
			Object.FindObjectOfType<GameManager>().AddEXP(150);
			Object.FindObjectOfType<PartyPanels>().UpdateHP(Object.FindObjectOfType<GameManager>().GetHPArray());
			Object.FindObjectOfType<BattleManager>().FadeEndBattle(1);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}
}
