using UnityEngine;

public class OverworldCoilSnake : OverworldBloodEnemyBase
{
	protected override void Awake()
	{
		speed = 6f;
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(defeatFlagID) == 1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(13) == 4)
		{
			CreateDeadEnemy();
		}
		base.Awake();
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
		anim.SetFloat("speed", 1f);
		base.RunAlgorithm();
	}

	public override void CreateDeadEnemy(bool age = false)
	{
		float num = base.transform.position.y;
		if (num > -0.85f)
		{
			num = -0.85f;
		}
		if (num < -1.21f)
		{
			num = -1.21f;
		}
		GameObject gameObject = Object.Instantiate(deadPrefab, new Vector3(base.transform.position.x, num), Quaternion.identity);
		gameObject.transform.parent = base.transform.parent;
		string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
		if (text != "")
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/enemies/spr_coil_snake_kill" + text);
		}
	}
}
