using UnityEngine;

public class NapstablookFinalAttack : AttackBase
{
	protected override void Awake()
	{
		base.Awake();
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
	}

	protected override void Update()
	{
		if (!isStarted || state != 0)
		{
			return;
		}
		if (Util.FindObjectOfType<Napstablook>().GetNapEndState() == 2 && frames == 1)
		{
			Util.GameManager().AddEXP(25);
			Util.FindObjectOfType<PartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
			Util.FindObjectOfType<Napstablook>().TurnToDust();
		}
		else
		{
			Util.FindObjectOfType<Napstablook>().GetPart("body").GetComponent<SpriteRenderer>()
				.color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 30f);
		}
		if (frames == ((Util.FindObjectOfType<Napstablook>().GetNapEndState() == 2) ? 90 : 30))
		{
			if (Util.FindObjectOfType<Napstablook>().GetNapEndState() == 2)
			{
				Util.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
			else
			{
				Util.FindObjectOfType<BattleManager>().EndNormalFight(customMessage: false, "");
			}
			Object.Destroy(base.gameObject);
		}
		else
		{
			frames++;
		}
	}
}
