using UnityEngine;

public class OverworldSprout : OverworldEnemyBase
{
	protected override void Awake()
	{
		base.Awake();
		sortingOrderOffset = -2;
	}

	protected override void Update()
	{
		base.Update();
		if (initiateBattle)
		{
			anim.SetFloat("speed", 0f);
		}
	}

	protected override void RunAlgorithm()
	{
		if (UTInput.GetAxis("Horizontal") != 0f || UTInput.GetAxis("Vertical") != 0f)
		{
			speed = Object.FindObjectOfType<OverworldPlayer>().GetSpeed() + 2f;
			anim.SetFloat("speed", 1f);
		}
		else
		{
			speed = 0f;
			anim.SetFloat("speed", 0f);
		}
		base.RunAlgorithm();
		GetComponent<SpriteRenderer>().flipX = dif.x > 0f;
		if (runFromPlayer)
		{
			GetComponent<SpriteRenderer>().flipX = !GetComponent<SpriteRenderer>().flipX;
		}
	}
}
