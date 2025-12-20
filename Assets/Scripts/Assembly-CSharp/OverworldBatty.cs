using System;
using UnityEngine;

public class OverworldBatty : OverworldBloodEnemyBase
{
	private float warble;

	protected override void Awake()
	{
		sortingOrderOffset = -2;
		if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(defeatFlagID) == 1 && (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(13) >= 5)
		{
			CreateDeadEnemy(true);
		}
		base.Awake();
		if ((int)Util.GameManager().GetFlag(13) >= 5)
		{
			runFromPlayer = true;
		}
	}

	public override void CreateDeadEnemy(bool age = false)
	{
		base.CreateDeadEnemy(age);
		string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
		if (text != "" && !age)
		{
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_batty_kill" + text);
		}
		else if (age)
		{
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_batty_kill_age" + text);
		}
		if (dead.transform.position.y < -54.71f)
		{
			dead.GetComponent<BoxCollider2D>().enabled = false;
		}
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		frames = 0;
	}

	public override void StopRunning()
	{
		base.StopRunning();
		anim.Play("Idle");
	}

	protected override void RunAlgorithm()
	{
		speed = UnityEngine.Object.FindObjectOfType<OverworldPlayer>().GetSpeed() + 2f;
		if (speed < 6f)
		{
			speed = 6f;
		}
		anim.Play("Fly");
		Vector3 vector = Vector3.MoveTowards(base.transform.position, UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position, speed / 48f);
		frames++;
		warble = Mathf.Sin((float)(frames * 12) * ((float)Math.PI / 180f)) / 4f;
		dif = vector - base.transform.position;
		if (runFromPlayer)
		{
			vector = base.transform.position - dif;
		}
		vector.x += warble;
		if ((bool)rigidbody2D)
		{
			rigidbody2D.MovePosition(vector);
		}
	}
}
