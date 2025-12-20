using UnityEngine;

public class OverworldWhimsun : OverworldEnemyBase
{
	private bool hardMode;

	private int chargeFrames;

	private bool startRunning;

	protected override void Awake()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(108) == 1)
		{
			hardMode = true;
		}
		base.Awake();
		if (hardMode)
		{
			hardMode = true;
			speed = 8f;
			anim.Play("Whimsalot Idle");
		}
		else
		{
			runFromPlayer = true;
			speed = 6f;
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
		chargeFrames = 0;
		if (hardMode)
		{
			anim.Play("Whimsalot Idle");
		}
	}

	protected override void RunAlgorithm()
	{
		if (hardMode)
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
					SetFaceDirection(true);
				}
				else
				{
					startRunning = false;
					dif = CalculateDifference(6f);
					SetFaceDirection(false);
				}
			}
			else if (chargeFrames < 5)
			{
				chargeFrames++;
				anim.Play("Whimsalot Precharge");
				if (chargeFrames == 5)
				{
					anim.Play("Whimsalot Charge", 0, 0f);
				}
			}
			else
			{
				base.RunAlgorithm();
				SetFaceDirection(false);
			}
		}
		else
		{
			base.RunAlgorithm();
			GetComponent<SpriteRenderer>().flipX = dif.x > 0f;
		}
	}

	public override void InstantSpareRespawn()
	{
		base.InstantSpareRespawn();
		if (hardMode)
		{
			respawned.GetComponent<Animator>().Play("Whimsalot Idle");
		}
	}

	private void SetFaceDirection(bool reverse)
	{
		float num = ((!reverse) ? 1 : (-1));
		anim.SetFloat("dirX", dif.x * num);
		anim.SetFloat("dirY", dif.y * num);
	}
}
