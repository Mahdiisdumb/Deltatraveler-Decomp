using UnityEngine;

public class BigFistAttack : PlayerAttackAnimation
{
	public override void AssignValues(EnemyBase enemy, int partyMember, float targetExcellence, int partySize)
	{
		float num = 0.35f;
		float num2 = (0f - num) * ((float)partySize / 2f);
		base.transform.position = enemy.GetEnemyObject().transform.Find("atkpos").position + new Vector3(num2 + num * (float)partyMember + 0.15f, 0f);
		GetComponent<SpriteRenderer>().color = PartyPanels.defaultColors[partyMember];
		if (targetExcellence >= 20f)
		{
			GetComponent<SpriteRenderer>().color += new Color(0.6f, 0.6f, 0.3f);
		}
		if ((int)Util.GameManager().GetFlag(107) == 1)
		{
			GetComponent<SpriteRenderer>().color = Color.white;
			if (targetExcellence >= 20f)
			{
				GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 0.5f);
			}
		}
	}
}
