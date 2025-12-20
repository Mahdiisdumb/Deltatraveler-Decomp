using System.Collections.Generic;
using UnityEngine;

public class DummyLeavingAttack : AttackBase
{
	private string[] diag;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("diag", new string[1] { "* Dummy tires of your\n  aimless shenanigans." });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
		diag = GetStringArray("diag");
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		Object.FindObjectOfType<BattleManager>().StartText(diag[0], new Vector2(-4f, -134f), "snd_txtbtl");
		if (UTInput.GetButton("X") || UTInput.GetButton("C"))
		{
			Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
		}
		Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_slidewhist");
		Object.FindObjectOfType<Dummy>().SetLeaving();
	}

	protected override void Update()
	{
		base.Update();
		if (isStarted)
		{
			if ((UTInput.GetButton("X") || UTInput.GetButton("C")) && Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Object.FindObjectOfType<BattleManager>().GetBattleText().SkipText();
			}
			else if ((UTInput.GetButtonDown("Z") || UTInput.GetButton("C")) && !Object.FindObjectOfType<BattleManager>().GetBattleText().IsPlaying())
			{
				Object.FindObjectOfType<BattleManager>().GetBattleText().DestroyOldText();
				Object.FindObjectOfType<BattleManager>().EndNormalFight(false, "");
				Object.FindObjectOfType<GameManager>().SetFlag(175, 1);
				Object.Destroy(base.gameObject);
			}
		}
	}
}
