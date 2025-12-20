using UnityEngine;

public class TorielFightEndAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: false, susie: false, noelle: false);
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		Util.FindObjectOfType<BattleManager>().FadeEndBattle(2);
	}
}
