using UnityEngine;

public class NapstablookFinalAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	protected override void Update()
	{
		if (!isStarted || state != 0)
		{
			return;
		}
		if (Object.FindObjectOfType<Napstablook>().GetNapEndState() == 2 && frames == 1)
		{
			Object.FindObjectOfType<GameManager>().AddEXP(25);
			Object.FindObjectOfType<PartyPanels>().UpdateHP(Object.FindObjectOfType<GameManager>().GetHPArray());
			Object.FindObjectOfType<Napstablook>().TurnToDust();
		}
		else
		{
			Object.FindObjectOfType<Napstablook>().GetPart("body").GetComponent<SpriteRenderer>()
				.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 30f);
		}
		if (frames == ((Object.FindObjectOfType<Napstablook>().GetNapEndState() == 2) ? 90 : 30))
		{
			if (Object.FindObjectOfType<Napstablook>().GetNapEndState() == 2)
			{
				Object.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
			else
			{
				Object.FindObjectOfType<BattleManager>().EndNormalFight(false, "");
			}
			Object.Destroy(base.gameObject);
		}
		else
		{
			frames++;
		}
	}
}
