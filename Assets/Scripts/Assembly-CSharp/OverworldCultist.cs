using UnityEngine;

public class OverworldCultist : OverworldBloodEnemyBase
{
	protected override void Awake()
	{
		speed = 12f;
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(defeatFlagID) == 1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 5)
		{
			CreateDeadEnemy(true);
		}
		base.Awake();
		runFromPlayer = true;
		anim.SetFloat("speed", 0.5f);
		anim.SetBool("isMoving", true);
		anim.SetFloat("dirY", 1f);
	}

	protected override void Update()
	{
		base.Update();
		if (initiateBattle)
		{
			anim.SetBool("isMoving", false);
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
		anim.SetBool("isMoving", false);
		anim.SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
	}

	protected override void RunAlgorithm()
	{
		runFromPlayer = false;
		anim.SetFloat("speed", 2f);
		anim.SetBool("isMoving", true);
		anim.SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
		anim.SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
		base.RunAlgorithm();
	}
}
