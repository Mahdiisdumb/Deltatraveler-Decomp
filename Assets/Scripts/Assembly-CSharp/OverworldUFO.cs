using System;
using UnityEngine;

public class OverworldUFO : OverworldEnemyBase
{
	private int wanderingFrames;

	private float velocity;

	private Vector3 targetLocation;

	private bool wanderingForward = true;

	protected override void Awake()
	{
		base.Awake();
		sortingOrderOffset = -2;
	}

	protected override void Update()
	{
		if (!disabled && canDetectPlayer && !detecting && !running)
		{
			if (wanderingFrames == 0)
			{
				wanderingFrames++;
				wanderingForward = true;
				float f = UnityEngine.Random.Range(0f, (float)Math.PI * 2f);
				targetLocation = base.transform.position + new Vector3(Mathf.Cos(f), Mathf.Sin(f)) * 20f;
				if (targetLocation.x <= base.transform.position.x)
				{
					anim.Play("Left");
				}
				else
				{
					anim.Play("Right");
				}
			}
			if (velocity < 4f && wanderingForward)
			{
				velocity += 0.2f;
			}
			else if (velocity >= 4f && wanderingForward)
			{
				wanderingFrames++;
				if (wanderingFrames == 20)
				{
					wanderingForward = false;
				}
			}
			else if (velocity > 0f && !wanderingForward)
			{
				velocity -= 0.2f;
			}
			else if (velocity <= 0f && !wanderingForward)
			{
				velocity = 0f;
				wanderingFrames++;
				if (wanderingFrames == 40)
				{
					wanderingFrames = 0;
				}
			}
			rigidbody2D.MovePosition(Vector3.MoveTowards(base.transform.position, targetLocation, velocity / 48f));
		}
		base.Update();
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		velocity = 0f;
	}

	protected override void RunAlgorithm()
	{
		if (frames == 0)
		{
			targetLocation = Vector3.MoveTowards(base.transform.position, UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position, 20f) - base.transform.position;
			targetLocation *= 100f;
			if ((targetLocation.x <= base.transform.position.x && !runFromPlayer) || (targetLocation.x > base.transform.position.x && runFromPlayer))
			{
				anim.Play("Left");
			}
			else
			{
				anim.Play("Right");
			}
		}
		frames++;
		if (frames < 7)
		{
			velocity -= 0.8f;
		}
		else if (frames < 20)
		{
			velocity += 1.5f;
		}
		else if (velocity >= 0f)
		{
			velocity -= 0.4f;
		}
		if (frames >= 20 && velocity <= 0f)
		{
			velocity = 0f;
			frames = 0;
		}
		int num = ((!runFromPlayer) ? 1 : (-1));
		rigidbody2D.MovePosition(Vector3.MoveTowards(base.transform.position, targetLocation, velocity / 48f * (float)num));
	}

	public override void StopRunning()
	{
		base.StopRunning();
		wanderingFrames = 0;
		frames = 0;
	}
}
