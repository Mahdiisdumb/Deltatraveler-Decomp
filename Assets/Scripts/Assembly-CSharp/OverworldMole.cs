using UnityEngine;

public class OverworldMole : OverworldBloodEnemyBase
{
	private bool startRunning;

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
		SetFaceDirection(false);
	}

	public override void StopRunning()
	{
		base.StopRunning();
		anim.SetBool("isMoving", false);
	}

	public override void OnCollisionEnter2D(Collision2D collision)
	{
		base.OnCollisionEnter2D(collision);
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>() && !disabled && canDetectPlayer)
		{
			anim.SetBool("isMoving", false);
		}
	}

	protected override void RunAlgorithm()
	{
		if (runFromPlayer)
		{
			if (Vector3.Distance(base.transform.position, Object.FindObjectOfType<OverworldPlayer>().transform.position) < 2f)
			{
				startRunning = true;
			}
			if (Vector3.Distance(base.transform.position, Object.FindObjectOfType<OverworldPlayer>().transform.position) < 4f && startRunning)
			{
				speed = Object.FindObjectOfType<OverworldPlayer>().GetSpeed() + 2f;
				base.RunAlgorithm();
				anim.SetBool("isMoving", true);
				SetFaceDirection(true);
			}
			else
			{
				startRunning = false;
				dif = CalculateDifference(6f);
				anim.SetBool("isMoving", false);
				SetFaceDirection(false);
			}
		}
		else
		{
			if (UTInput.GetAxis("Horizontal") != 0f || UTInput.GetAxis("Vertical") != 0f)
			{
				speed = Object.FindObjectOfType<OverworldPlayer>().GetSpeed() + 2f;
				anim.SetBool("isMoving", true);
			}
			else
			{
				speed = 0f;
				anim.SetBool("isMoving", false);
			}
			base.RunAlgorithm();
			if (speed != 0f)
			{
				SetFaceDirection(false);
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
