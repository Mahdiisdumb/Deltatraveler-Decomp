using System;
using UnityEngine;

public class OverworldGyftrot : OverworldEnemyBase
{
	private Vector3 tradjectory;

	private float maxSpeed;

	private float acceleration;

	private int hangFrames;

	private int maxHangFrames;

	private bool speedup;

	private int waitFrames;

	private int maxWaitFrames;

	[SerializeField]
	private Vector2 minBounds = Vector2.zero;

	[SerializeField]
	private Vector2 maxBounds = Vector2.zero;

	protected override void Awake()
	{
		base.Awake();
		ResetMovement();
	}

	protected override void Update()
	{
		base.Update();
		if (disabled || initiateBattle)
		{
			return;
		}
		if (speedup)
		{
			if (speed < maxSpeed)
			{
				speed += acceleration;
			}
			if (speed >= maxSpeed)
			{
				speed = maxSpeed;
				hangFrames++;
				if (hangFrames >= maxHangFrames)
				{
					speedup = false;
				}
			}
			if ((tradjectory.x > 0f && base.transform.position.x >= maxBounds.x) || (tradjectory.x < 0f && base.transform.position.x <= minBounds.x) || (tradjectory.y > 0f && base.transform.position.y >= maxBounds.y) || (tradjectory.y < 0f && base.transform.position.y <= minBounds.y))
			{
				speedup = false;
			}
		}
		else
		{
			if (speed > 0f)
			{
				speed -= acceleration;
			}
			if (speed <= 0f)
			{
				speed = 0f;
				waitFrames++;
				if (waitFrames >= maxWaitFrames)
				{
					ResetMovement();
				}
			}
		}
		rigidbody2D.MovePosition(base.transform.position + tradjectory * speed);
		anim.SetFloat("Speed", speed * 48f / 4f);
	}

	public override void InstantSpareRespawn()
	{
		base.InstantSpareRespawn();
		if ((bool)respawned)
		{
			if ((int)Util.GameManager().GetFlag(267) == 2)
			{
				respawned.GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* What the fuck is your\n  problem." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				respawned.GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* Kris,^05 stop bothering\n  them.^05\n* You've done enough." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_annoyed" });
			}
			else if ((int)Util.GameManager().GetFlag(266) == 1)
			{
				respawned.GetComponent<InteractTextBox>().ModifyContents(new string[3] { "* That was so gracious of\n  you,^05 deer girl!", "* To remove that GARBAGE off\n  of me!", "* (This seems kinda\n  contrived,^05 not gonna\n  lie.)" }, new string[3] { "", "", "snd_txtsus" }, new int[1], new string[3] { "", "", "su_smirk_sweat" });
				respawned.GetComponent<InteractTextBox>().ModifySecondaryContents(new string[1] { "* (Maybe we should leave\n  them alone.)" }, new string[1] { "snd_txtnoe" }, new int[1], new string[1] { "no_thinking" });
			}
			else if ((int)Util.GameManager().GetFlag(268) == 1)
			{
				respawned.GetComponent<InteractTextBox>().ModifyContents(new string[4] { "* GET AWAY FROM ME,^05 TEENAGE\n  HUMAN!!!", "* See what you did,^05\n  Kris?", "* No,^05 not you two!^05\n* You two are the good\n  ones!", "* ???????" }, new string[4] { "snd_text", "snd_txtsus", "snd_text", "snd_txtnoe" }, new int[1], new string[4] { "", "su_annoyed", "", "no_confused" });
			}
		}
	}

	public void ResetMovement()
	{
		speedup = true;
		maxSpeed = UnityEngine.Random.Range(3f, 6f) / 48f;
		acceleration = UnityEngine.Random.Range(1f, 3f) / 48f / 8f;
		maxHangFrames = UnityEngine.Random.Range(4, 12);
		maxWaitFrames = UnityEngine.Random.Range(5, 20);
		float num = UnityEngine.Random.Range(0f, 360f);
		tradjectory = new Vector3(Mathf.Cos(num * ((float)Math.PI / 180f)), Mathf.Sin(num * ((float)Math.PI / 180f)));
		if ((tradjectory.x > 0f && base.transform.position.x >= maxBounds.x) || (tradjectory.x < 0f && base.transform.position.x <= minBounds.x))
		{
			tradjectory.x *= -1f;
		}
		if ((tradjectory.y > 0f && base.transform.position.y >= maxBounds.y) || (tradjectory.y < 0f && base.transform.position.y <= minBounds.y))
		{
			tradjectory.y *= -1f;
		}
		hangFrames = 0;
		waitFrames = 0;
	}
}
