using System.Collections.Generic;
using UnityEngine;

public class MondoMoleDeath : AttackBase
{
	private MondoMole mondo;

	private bool booby;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("killed", new string[6] { "You have done \nexcellently to \ndefeat me.", "It is a shame \nthat you murdered \nme.", "Because now you \nwill need to make \na difficult decision.", "...", "I will let you \nfind that out \nyourself.", "Goodbye." });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
		maxFrames = 5000;
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		mondo = Object.FindObjectOfType<MondoMole>();
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				mondo.Chat(GetStringArray("killed"), "RightWide", "snd_text", new Vector2(178f, 117f), true, 1);
				state = 1;
				frames = 0;
			}
		}
		if (state != 1)
		{
			return;
		}
		if ((bool)mondo.GetTextBubble())
		{
			if (mondo.GetTextBubble().GetCurrentStringNum() == 2 && !booby)
			{
				booby = true;
				mondo.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Mondo Mole/spr_b_mondo_die_1");
			}
			else if (mondo.GetTextBubble().GetCurrentStringNum() == 4 && booby)
			{
				mondo.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Mondo Mole/spr_b_mondo_die_0");
			}
			return;
		}
		frames++;
		if (frames < 30)
		{
			mondo.GetEnemyObject().transform.position = new Vector3(0f, -0.14f) + new Vector3(Random.Range(-1, 2), Random.Range(-1, 2)) / 48f;
		}
		else if (frames == 30)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
			Object.FindObjectOfType<BattleCamera>().BlastShake();
			mondo.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Mondo Mole/spr_b_mondo_die_2");
			mondo.GetEnemyObject().transform.position = new Vector3(0f, -0.14f);
			Object.Instantiate(Resources.Load<GameObject>("vfx/EnemyBlood"), Vector3.zero, Quaternion.identity);
		}
		if (frames == 120)
		{
			Object.FindObjectOfType<BattleManager>().EndNormalFight(false, "");
		}
	}
}
