using UnityEngine;

public class OverworldFroggit : OverworldEnemyBase
{
	private int hopFrames;

	private bool hopping;

	private bool hardMode;

	private Vector3 prevPos;

	private Vector3 projectedPos;

	protected override void Awake()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(108) == 1)
		{
			hardMode = true;
		}
		base.Awake();
		sortingOrderOffset = -2;
		if (hardMode)
		{
			anim.Play("HardIdle");
		}
	}

	protected override void RunAlgorithm()
	{
		frames++;
		if (frames == 1)
		{
			prevPos = base.transform.position;
			float maxDistanceDelta = 5f / 6f;
			projectedPos = Vector3.MoveTowards(prevPos, Object.FindObjectOfType<OverworldPlayer>().transform.position, maxDistanceDelta);
			Vector3 vector = projectedPos - prevPos;
			if (runFromPlayer)
			{
				projectedPos = base.transform.position - vector;
				vector *= -1f;
			}
			if (hardMode)
			{
				anim.Play("HardHop", 0, 0f);
			}
			else if (vector.y > 0f)
			{
				anim.Play("HopBack", 0, 0f);
			}
			else
			{
				anim.Play("Hop", 0, 0f);
			}
		}
		if (!hopping)
		{
			return;
		}
		if (!hardMode)
		{
			hopFrames++;
			float num = (runFromPlayer ? 3 : 6);
			Vector3 vector2 = Vector3.Lerp(prevPos, projectedPos, (float)hopFrames / num);
			rigidbody2D.MovePosition(vector2);
			if ((float)hopFrames >= num)
			{
				hopping = false;
				frames = 0;
			}
		}
		else
		{
			if (speed < 12f)
			{
				speed += 0.25f;
			}
			base.RunAlgorithm();
		}
	}

	public override void InstantSpareRespawn()
	{
		base.InstantSpareRespawn();
		if (respawned != null && respawned.GetComponent<Animator>().runtimeAnimatorController == anim.runtimeAnimatorController && hardMode)
		{
			respawned.GetComponent<Animator>().Play("HardIdle");
		}
	}

	public void Hop()
	{
		GetComponents<AudioSource>()[2].Play();
		hopping = true;
		hopFrames = 0;
	}

	public override void StopRunning()
	{
		base.StopRunning();
		hopping = false;
		if (!hardMode)
		{
			anim.Play("IdleNormal", 0, 0f);
		}
		else
		{
			anim.Play("HardIdle", 0, 0f);
		}
	}
}
