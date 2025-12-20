using System;
using UnityEngine;

public class OverworldIceCap : OverworldEnemyBase
{
	private bool startSpin;

	private float distance;

	private float angle;

	private float acceleration;

	protected override void Awake()
	{
		base.Awake();
		speed = 9f;
	}

	public override void InstantSpareRespawn()
	{
		base.InstantSpareRespawn();
		if ((bool)respawned && counterFlagID == 188 && !Util.GameManager().NoelleInParty())
		{
			respawned.GetComponent<InteractTextBox>().ShrinkLines(4);
		}
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		GetComponent<SpriteRenderer>().flipX = base.transform.position.x - UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position.x > 0f;
		anim.SetFloat("speed", 0f);
	}

	protected override void RunAlgorithm()
	{
		anim.SetFloat("speed", 1f);
		if (runFromPlayer)
		{
			base.RunAlgorithm();
			GetComponent<SpriteRenderer>().flipX = dif.x > 0f;
		}
		else if (!startSpin)
		{
			startSpin = true;
			Vector3 vector = base.transform.position - UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position;
			distance = vector.magnitude;
			angle = Mathf.Atan2(vector.normalized.y, vector.normalized.x) * 57.29578f;
			MonoBehaviour.print("icecap: starting spin at " + distance + " distance and " + angle + " angle!!!!!!!!!");
		}
		else
		{
			acceleration += 0.1f;
			distance -= 1f / 24f * acceleration;
			angle -= 10f * acceleration;
			base.transform.position = Vector3.Lerp(base.transform.position, UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position + new Vector3(Mathf.Cos(angle * ((float)Math.PI / 180f)), Mathf.Sin(angle * ((float)Math.PI / 180f))) * distance, acceleration);
			GetComponent<SpriteRenderer>().flipX = base.transform.position.x - UnityEngine.Object.FindObjectOfType<OverworldPlayer>().transform.position.x > 0f;
		}
	}
}
