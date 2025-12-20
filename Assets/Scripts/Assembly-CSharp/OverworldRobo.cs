using UnityEngine;

public class OverworldRobo : OverworldEnemyBase
{
	protected override void Awake()
	{
		base.Awake();
		speed = 12f;
	}

	protected override void Update()
	{
		base.Update();
		if (initiateBattle)
		{
			anim.SetFloat("speed", 0f);
		}
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		anim.SetFloat("speed", 0f);
	}

	public override void StopRunning()
	{
		base.StopRunning();
		anim.SetFloat("speed", 0f);
	}

	protected override void RunAlgorithm()
	{
		anim.SetFloat("speed", 1f);
		base.RunAlgorithm();
		GetComponent<SpriteRenderer>().flipX = dif.x > 0f;
		if (runFromPlayer)
		{
			GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
		}
	}
}
