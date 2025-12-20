using System;
using UnityEngine;

public class SillyStickAttack : PlayerAttackAnimation
{
	private int frames;

	protected override void Update()
	{
		frames++;
		base.transform.GetChild(0).localPosition = new Vector3(0f, Mathf.Lerp(-3.12f, 0.3f, Mathf.Sin((float)(frames * 9) * ((float)Math.PI / 180f))));
		base.transform.GetChild(0).eulerAngles = new Vector3(0f, 0f, frames);
		base.transform.GetChild(0).localScale = Vector3.Lerp(new Vector3(1.3f, 1.3f, 1f), new Vector3(1f, 1f, 1f), (float)frames / 12f);
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = frames <= 15;
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0f, 1f, 1f - (float)(frames - 12) / 3f);
		if (frames > 12)
		{
			base.transform.GetChild(0).localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(1.2f, 1.2f, 1.2f), (float)(frames - 12) / 3f);
		}
		if (!GetComponent<AudioSource>().isPlaying && frames > 15)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void AssignValues(EnemyBase enemy, int partyMember, float targetExcellence, int partySize)
	{
		float num = 0.35f;
		float num2 = (0f - num) * ((float)partySize / 2f);
		base.transform.position = enemy.GetEnemyObject().transform.Find("atkpos").position + new Vector3(num2 + num * (float)partyMember + 0.15f, 0f);
	}
}
