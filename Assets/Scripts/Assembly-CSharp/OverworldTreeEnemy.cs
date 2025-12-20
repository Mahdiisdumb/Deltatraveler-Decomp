using UnityEngine;

public class OverworldTreeEnemy : OverworldEnemyBase
{
	protected override void RunAlgorithm()
	{
		if (speed < 15f)
		{
			speed += 0.2f;
		}
		base.RunAlgorithm();
	}

	public override void StopRunning()
	{
		base.StopRunning();
		anim.SetFloat("speed", 0f);
		speed = 0f;
	}

	public override void DetectPlayer()
	{
		if (!runFromPlayer)
		{
			anim.SetFloat("speed", 1f);
			running = true;
			speed = 0f;
		}
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		if (initiateBattle)
		{
			anim.enabled = false;
			sr.sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_tree_enemy_catch");
		}
	}
}
