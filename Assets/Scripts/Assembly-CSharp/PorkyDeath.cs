using UnityEngine;

public class PorkyDeath : AttackBase
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
		if (isStarted)
		{
			frames++;
			if (frames == 150)
			{
				Object.FindObjectOfType<GameManager>().AddEXP(250);
				Object.FindObjectOfType<PartyPanels>().UpdateHP(Object.FindObjectOfType<GameManager>().GetHPArray());
				Object.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<Porky>().Explode();
	}
}
