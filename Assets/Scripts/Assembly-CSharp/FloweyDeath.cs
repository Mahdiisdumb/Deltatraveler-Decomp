using UnityEngine;

public class FloweyDeath : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: true, susie: false, noelle: false);
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
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
				Util.FindObjectOfType<Flowey>().SetFace("die_" + num);
			}
		}
		if (frames >= 90)
		{
			int num2 = (frames - 90) / 15 + 5;
			if (num2 < 8)
			{
				Util.FindObjectOfType<Flowey>().SetFace("die_" + num2);
			}
		}
		if (frames == 150 && (int)Util.GameManager().GetFlag(108) == 1)
		{
			Util.FindObjectOfType<Flowey>().TriggerKrisFalling();
		}
		if (frames == 180)
		{
			Util.GameManager().AddEXP(150);
			Util.FindObjectOfType<PartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
			Util.FindObjectOfType<BattleManager>().FadeEndBattle(1);
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}
}
