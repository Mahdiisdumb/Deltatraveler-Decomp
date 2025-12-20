using System.Collections.Generic;
using UnityEngine;

public class PorkyFinalAttack : AttackBase
{
	private SpriteRenderer ness;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("spared_0", new string[5] { "Did you think that \nI'd just RUN AWAY???", "This thing still \nhas enough energy for \nme to kill you in \nONE FELL SWOOP!!!", "I just wanted to \nmake you fight this \nmech that DIDN'T want \nto even fight!", "Hahahaha...", "SO LONG,^05 LOSERS!" });
		dictionary.Add("spared_1", new string[5] { "W-^05what did you...?", "...NESS???\n^05How'd you...?!", "This isn't over,^05 \nlosers!", "Ness,^05 I'll be seeing \nyou and Paula very \nsoon...", "You'll pay for this!!!" });
		return dictionary;
	}

	protected override void Awake()
	{
		base.Awake();
		SetStrings(GetDefaultStrings(), GetType());
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
				Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_gallery", 1f);
				Object.FindObjectOfType<Porky>().Chat(GetStringArray("spared_0"), "RightWide", "snd_txtpor", new Vector2(182f, 126f), true, 0);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1 && !Object.FindObjectOfType<TextBubble>())
		{
			frames++;
			if (frames == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyFinalBeam"));
			}
			if (frames == 60)
			{
				Object.FindObjectOfType<BattleManager>().StopMusic();
				Object.FindObjectOfType<Porky>().GetPart("mech").Find("head")
					.GetComponent<SpriteRenderer>()
					.sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_head_nohp");
				Object.Instantiate(Resources.Load<GameObject>("battle/Bash")).GetComponent<PlayerAttackAnimation>().AssignValues(Object.FindObjectOfType<Porky>(), 5, 20f, 10);
				Object.Destroy(Object.FindObjectOfType<PorkyFinalBeam>().gameObject);
			}
			if (frames == 120)
			{
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_crash");
				Object.FindObjectOfType<BattleCamera>().BlastShake();
				Object.FindObjectOfType<Porky>().Hit(0, 10f, true);
			}
			if (frames >= 210 && !Object.FindObjectOfType<Porky>().IsShaking())
			{
				state = 2;
				frames = 0;
				Object.FindObjectOfType<Porky>().Chat(GetStringArray("spared_1"), "RightWide", "snd_txtpor", new Vector2(182f, 126f), true, 0);
			}
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			if ((bool)ness)
			{
				ness.transform.position = Vector3.Lerp(ness.transform.position, new Vector3(-5f, -0.13f), 0.2f);
			}
			if (!Object.FindObjectOfType<TextBubble>())
			{
				frames++;
				if (frames == 1)
				{
					Object.FindObjectOfType<Porky>().Explode();
				}
				if (frames == 150)
				{
					Object.FindObjectOfType<BattleManager>().FadeEndBattle(2);
				}
			}
			else if (Object.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && !ness)
			{
				ness = new GameObject("Ness", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
				ness.transform.position = new Vector3(-8f, -0.13f);
				ness.sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_ness");
				ness.sortingOrder = 35;
				ness.flipX = true;
			}
		}
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_crash");
		bb.StartMovement(new Vector2(29f, 29f), new Vector2(0f, -0.52f), true);
		Object.FindObjectOfType<SOUL>().transform.position = bbPos;
	}
}
