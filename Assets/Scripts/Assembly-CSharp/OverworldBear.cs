using UnityEngine;

public class OverworldBear : OverworldBloodEnemyBase
{
	private float warble;

	[SerializeField]
	private Vector2 startLook = Vector2.down;

	protected override void Awake()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(defeatFlagID) == 1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 5)
		{
			CreateDeadEnemy(true);
		}
		base.Awake();
		anim.SetFloat("dirX", startLook.x);
		anim.SetFloat("dirY", startLook.y);
		speed = 4f;
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
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_bear_kill" + text);
		}
		else if (age)
		{
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_bear_kill_age" + text);
		}
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		dif = CalculateDifference(6f);
		SetFaceDirection(false);
	}

	public override void StopRunning()
	{
		base.StopRunning();
		anim.SetBool("isMoving", false);
	}

	protected override void RunAlgorithm()
	{
		base.RunAlgorithm();
		anim.SetBool("isMoving", true);
		SetFaceDirection(runFromPlayer);
	}

	private void SetFaceDirection(bool reverse)
	{
		float num = ((!reverse) ? 1 : (-1));
		anim.SetFloat("dirX", dif.x * num);
		anim.SetFloat("dirY", dif.y * num);
	}
}
