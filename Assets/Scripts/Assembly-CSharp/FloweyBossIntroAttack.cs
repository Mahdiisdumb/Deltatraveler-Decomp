using UnityEngine;

public class FloweyBossIntroAttack : AttackBase
{
	protected override void Update()
	{
		if (isStarted)
		{
			frames++;
			if (frames == 15)
			{
				Object.FindObjectOfType<Flowey>().GetPart("vineLeft").GetComponent<Animator>()
					.enabled = true;
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_grab");
				Object.FindObjectOfType<BattleCamera>().BlastShake();
			}
			if (frames == 45)
			{
				Object.FindObjectOfType<Flowey>().GetPart("vineRight").GetComponent<Animator>()
					.enabled = true;
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_grab");
				Object.FindObjectOfType<BattleCamera>().BlastShake();
			}
			if (frames == 70)
			{
				Object.FindObjectOfType<Flowey>().GetPart("vineLeft").GetComponent<Animator>()
					.Play("Idle");
				Object.FindObjectOfType<Flowey>().GetPart("vineRight").GetComponent<Animator>()
					.Play("Idle");
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_floweylaugh2");
			}
			if (frames > 70)
			{
				Object.FindObjectOfType<Flowey>().SetFace("laugh_" + frames / 2 % 2);
			}
			if (frames == 145)
			{
				Object.FindObjectOfType<Flowey>().SetFace("evil");
				Object.Destroy(base.gameObject);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}
}
