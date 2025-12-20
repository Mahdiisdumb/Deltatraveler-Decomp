using UnityEngine;

public class SmallFistAttack : PlayerAttackAnimation
{
	private void Awake()
	{
		isPlaying = false;
	}

	protected override void Update()
	{
		base.transform.position += new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) / 48f;
		base.Update();
	}

	public override void AssignValues(EnemyBase enemy, int partyMember, float targetExcellence, int partySize, int slot)
	{
		base.AssignValues(enemy, partyMember, targetExcellence, partySize, slot);
		base.transform.position = enemy.GetEnemyObject().transform.Find("atkpos").position + new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f));
		if (enemy.GetName() == "Jerry" && ((Jerry)enemy).InFinale())
		{
			Util.FindObjectOfType<BattleCamera>().BlastShake();
			Object.Instantiate(Resources.Load<GameObject>("battle/RudePunchEffect"), base.transform.position, Quaternion.identity);
			if (!Util.FindObjectOfType<JerryFinaleHPBar>())
			{
				JerryFinaleHPBar component = Object.Instantiate(Resources.Load<GameObject>("battle/enemies/JerryFinaleHP"), GameObject.Find("BattleCanvas").transform).GetComponent<JerryFinaleHPBar>();
				component.gameObject.name = "EnemyHP";
				component.transform.localScale = new Vector2(1f, 1f);
				component.transform.localPosition = new Vector2(0f, 160f);
			}
			Util.FindObjectOfType<JerryFinaleHPBar>().StartHP(1, enemy.GetHP(), enemy.GetMaxHP(), 0, 202, mercy: false);
			JerryFinaleHPText component2 = Object.Instantiate(Resources.Load<GameObject>("battle/enemies/JerryFinaleHPText"), GameObject.Find("BattleCanvas").transform).GetComponent<JerryFinaleHPText>();
			component2.transform.position = new Vector3((float)Mathf.RoundToInt(base.transform.position.x * 48f) / 48f, (float)Mathf.RoundToInt(base.transform.position.y * 48f + 10f) / 48f);
			component2.StartHP(1, enemy.GetHP(), enemy.GetMaxHP(), partyMember, mercy: false, null);
		}
	}
}
