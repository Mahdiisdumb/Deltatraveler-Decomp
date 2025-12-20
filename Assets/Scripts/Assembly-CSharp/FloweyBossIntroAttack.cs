using UnityEngine;

public class FloweyBossIntroAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
	}

	protected override void Update()
	{
		if (isStarted)
		{
			frames++;
			if (frames == 15)
			{
				Util.FindObjectOfType<Flowey>().GetPart("vineLeft").GetComponent<Animator>()
					.enabled = true;
				Util.GameManager().PlayGlobalSFX("sounds/snd_grab");
				Util.FindObjectOfType<BattleCamera>().BlastShake();
			}
			if (frames == 45)
			{
				Util.FindObjectOfType<Flowey>().GetPart("vineRight").GetComponent<Animator>()
					.enabled = true;
				Util.GameManager().PlayGlobalSFX("sounds/snd_grab");
				Util.FindObjectOfType<BattleCamera>().BlastShake();
			}
			if (frames == 70)
			{
				Util.FindObjectOfType<Flowey>().GetPart("vineLeft").GetComponent<Animator>()
					.Play("Idle");
				Util.FindObjectOfType<Flowey>().GetPart("vineRight").GetComponent<Animator>()
					.Play("Idle");
				Util.GameManager().PlayGlobalSFX("sounds/snd_floweylaugh2");
			}
			if (frames > 70)
			{
				Util.FindObjectOfType<Flowey>().SetFace("laugh_" + frames / 2 % 2);
			}
			if (frames == 145)
			{
				Util.FindObjectOfType<Flowey>().SetFace("evil");
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}
}
