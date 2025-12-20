using UnityEngine;

public class DummyLeavingAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		Util.FindObjectOfType<BattleManager>().StartText("* Dummy tires of your\n  aimless shenanigans.", new Vector2(-4f, -134f), "snd_txtbtl");
		if (UTInput.GetButton("X") || UTInput.GetButton("C"))
		{
			Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
		}
		Util.GameManager().PlayGlobalSFX("sounds/snd_slidewhist");
		Util.FindObjectOfType<Dummy>().SetLeaving();
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted)
		{
			if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && Util.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Util.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			else if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !Util.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Util.FindObjectOfType<BattleManager>().GetBattleText().DestroyOldText();
				Util.FindObjectOfType<BattleManager>().EndNormalFight(customMessage: false, "");
				Util.GameManager().SetFlag(175, 1);
				Object.Destroy(base.gameObject);
			}
		}
	}
}
