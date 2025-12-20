using UnityEngine;

public class OverworldCultist : OverworldBloodEnemyBase
{
	protected override void Awake()
	{
		speed = 12f;
		if ((int)Util.GameManager().GetFlag(defeatFlagID) == 1 && (int)Util.GameManager().GetFlag(13) >= 5)
		{
			CreateDeadEnemy(age: true);
		}
		base.Awake();
		runFromPlayer = true;
		anim.SetFloat("speed", 0.5f);
		anim.SetBool("isMoving", value: true);
		anim.SetFloat("dirY", 1f);
	}

	protected override void Update()
	{
		base.Update();
		if (initiateBattle)
		{
			anim.SetBool("isMoving", value: false);
		}
	}

	public override void CreateDeadEnemy(bool age = false)
	{
		base.CreateDeadEnemy(age);
		string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
		if (text != "" && !age)
		{
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill" + text);
		}
		else if (age)
		{
			dead.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/spr_cultist_kill_age" + text);
		}
	}

	public override void DetectPlayer()
	{
		base.DetectPlayer();
		anim.SetBool("isMoving", value: false);
		anim.SetFloat("dirX", Util.OverworldPlayer().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Util.OverworldPlayer().transform.position.y - base.transform.position.y);
	}

	protected override void RunAlgorithm()
	{
		runFromPlayer = false;
		anim.SetFloat("speed", 2f);
		anim.SetBool("isMoving", value: true);
		anim.SetFloat("dirX", Util.OverworldPlayer().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Util.OverworldPlayer().transform.position.y - base.transform.position.y);
		base.RunAlgorithm();
	}
}
