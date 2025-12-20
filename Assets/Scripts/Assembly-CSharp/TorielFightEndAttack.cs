using UnityEngine;

public class TorielFightEndAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		Object.FindObjectOfType<BattleManager>().FadeEndBattle(2);
	}
}
