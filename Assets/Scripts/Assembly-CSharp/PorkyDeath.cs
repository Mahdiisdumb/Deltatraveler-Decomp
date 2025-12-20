using UnityEngine;

public class PorkyDeath : AttackBase
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
		if (isStarted)
		{
			frames++;
			if (frames == 150)
			{
				Util.GameManager().AddEXP(250);
				Util.FindObjectOfType<PartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
				Util.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<Porky>().Explode();
	}
}
