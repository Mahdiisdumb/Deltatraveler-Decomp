using UnityEngine;

public class SusieLDEnd : AttackBase
{
	private SusieLD susie;

	private bool quoteOnQuoteAttackStart;

	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Object.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Object.FindObjectOfType<PartyPanels>().RaiseHeads(false, false, false);
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		susie = Object.FindObjectOfType<SusieLD>();
	}

	protected override void Update()
	{
		if (state == 0)
		{
			if ((bool)susie.GetTextBubble())
			{
				if (!quoteOnQuoteAttackStart && (susie.GetTextBubble().GetCurrentStringNum() == 3 || (susie.GetTextBubble().GetCurrentStringNum() == 2 && !susie.GetTextBubble().IsPlaying())))
				{
					quoteOnQuoteAttackStart = true;
					bb.StartMovement(new Vector2(165f, 140f));
					Object.FindObjectOfType<PartyPanels>().SetTargets(true, false, false);
					Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = true;
				}
				if (susie.GetTextBubble().GetCurrentStringNum() == 1)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_hurt_rage");
				}
				if (susie.GetTextBubble().GetCurrentStringNum() == 3)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_hurt_lookdown");
					susie.GetTextBubble().GetComponent<ShakingText>().StartShake(20, "speechbubble");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_hurt_lookdown_1");
				Object.FindObjectOfType<SOUL>().SetControllable(true);
			}
			if (frames == 45)
			{
				state = 1;
				frames = 0;
				susie.Chat(new string[6] { "That,^05 umm...", "Happens when someone \nisn't dying...", "...", "So I'm NOT dying.", "...", "OKAY I'M SPARING \nMYSELF" }, "RightWide", "snd_txtsus", Vector2.zero, true, 0);
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if ((bool)susie.GetTextBubble())
			{
				if (susie.GetTextBubble().GetCurrentStringNum() == 4)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_hurt_lookup");
				}
				if (susie.GetTextBubble().GetCurrentStringNum() == 6)
				{
					susie.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Susie/spr_b_susie_hurt_spare");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				susie.Spare();
			}
			if (frames == 30)
			{
				Util.GameManager().SetHP(1, Util.GameManager().GetMaxHP(1) / 5);
				Util.GameManager().AddEXP(Object.FindObjectOfType<LesserDog>().GetFinalEXP());
				Util.GameManager().AddGold(Object.FindObjectOfType<LesserDog>().GetGold());
				Object.FindObjectOfType<BattleManager>().FadeEndBattle(1);
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().SetControllable(false);
		susie.Chat(new string[3] { "WHAT...^10 THE \nHELL...!", "YOU DOUBLE CROSSING,^10 \nBACKSTABBING PIECE \nOF...", "...^10 Uhhh..." }, "RightWide", "snd_txtsus", Vector2.zero, true, 1);
		susie.GetTextBubble().gameObject.AddComponent<ShakingText>().StartShake(0, "speechbubble");
	}
}
