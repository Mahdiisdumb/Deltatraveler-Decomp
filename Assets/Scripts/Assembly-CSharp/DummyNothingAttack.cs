using UnityEngine;

public class DummyNothingAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}
}
