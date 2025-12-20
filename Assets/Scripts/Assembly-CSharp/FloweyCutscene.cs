using System;
using UnityEngine;

public class FloweyCutscene : EnemyBase
{
	protected override void Awake()
	{
		base.Awake();
		enemyName = "FloweyCutscene";
		fileName = "flowey";
		checkDesc = "* STOP PEEKING";
		maxHp = 999999999;
		hp = maxHp;
		hpPos = new Vector2(103f, 162f);
	}

	protected override void Update()
	{
		if (gotHit)
		{
			frames++;
			float num = Mathf.Sin((float)Math.PI / 180f * Mathf.Lerp(0f, 135f, (float)frames / 20f)) * 2f;
			Vector3 localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), Vector3.zero, (float)frames / 20f);
			GetEnemyObject().transform.position = new Vector3(0.063f, 1.294f + num);
			GetEnemyObject().transform.localScale = localScale;
			GetEnemyObject().transform.rotation = Quaternion.Euler(0f, 0f, Mathf.Lerp(0f, 90f, (float)frames / 20f));
			if (frames == 10)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_ehurt1");
			}
		}
	}

	public override void Hit(int partyMember, float rawDmg, bool playSound)
	{
		gotHit = true;
		aud.clip = Resources.Load<AudioClip>("sounds/snd_damage");
		aud.Play();
		CombineParts();
		obj.transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/" + enemyName.Replace(".", "") + "/spr_b_" + fileName + "_dmg");
	}

	public SpriteRenderer GetBody()
	{
		return GetEnemyObject().transform.Find("parts").Find("literallyjusttheirbodylol").GetComponent<SpriteRenderer>();
	}

	public SpriteRenderer GetFakeSusie()
	{
		return GetEnemyObject().transform.Find("fakesusie").GetComponent<SpriteRenderer>();
	}
}
