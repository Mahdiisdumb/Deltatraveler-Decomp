using UnityEngine;

public class OverworldMole : OverworldBloodEnemyBase
{
	private bool startRunning;

	[SerializeField]
	private Vector2 startLook = Vector2.down;

	protected override void Awake()
	{
		if ((int)Util.GameManager().GetFlag(defeatFlagID) == 1 && (int)Util.GameManager().GetFlag(13) >= 5)
		{
			CreateDeadEnemy(age: true);
		}
		base.Awake();
		anim.SetFloat("dirX", startLook.x);
		anim.SetFloat("dirY", startLook.y);
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
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_mole_kill" + text);
		}
		else if (age)
		{
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_mole_kill_age" + text);
		}
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		dif = CalculateDifference(6f);
		SetFaceDirection(reverse: false);
	}

	public override void StopRunning()
	{
		base.StopRunning();
		anim.SetBool("isMoving", value: false);
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>() && !disabled && canDetectPlayer)
		{
			anim.SetBool("isMoving", value: false);
		}
	}

	protected override void RunAlgorithm()
	{
		if (runFromPlayer)
		{
			if (Vector3.Distance(base.transform.position, Util.OverworldPlayer().transform.position) < 2f)
			{
				startRunning = true;
			}
			if (Vector3.Distance(base.transform.position, Util.OverworldPlayer().transform.position) < 4f && startRunning)
			{
				speed = Util.OverworldPlayer().GetSpeed() + 2f;
				base.RunAlgorithm();
				anim.SetBool("isMoving", value: true);
				SetFaceDirection(reverse: true);
			}
			else
			{
				startRunning = false;
				dif = CalculateDifference(6f);
				anim.SetBool("isMoving", value: false);
				SetFaceDirection(reverse: false);
			}
		}
		else
		{
			if (UTInput.GetAxis("Horizontal") != 0f || UTInput.GetAxis("Vertical") != 0f)
			{
				speed = Util.OverworldPlayer().GetSpeed() + 2f;
				anim.SetBool("isMoving", value: true);
			}
			else
			{
				speed = 0f;
				anim.SetBool("isMoving", value: false);
			}
			base.RunAlgorithm();
			if (speed != 0f)
			{
				SetFaceDirection(reverse: false);
			}
		}
	}

	private void SetFaceDirection(bool reverse)
	{
		float num = ((!reverse) ? 1 : (-1));
		anim.SetFloat("dirX", dif.x * num);
		anim.SetFloat("dirY", dif.y * num);
	}
}
