using UnityEngine;

public class PorkyFinalAttack : AttackBase
{
	private SpriteRenderer ness;

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
				Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_gallery", 1f);
				Util.FindObjectOfType<Porky>().Chat(new string[5] { "Did you think that \nI'd just RUN AWAY???", "This thing still \nhas enough energy for \nme to kill you in \nONE FELL SWOOP!!!", "I just wanted to \nmake you fight this \nmech that DIDN'T want \nto even fight!", "Hahahaha...", "SO LONG,^05 LOSERS!" }, "RightWide", "snd_txtpor", new Vector2(182f, 126f), canSkip: true, 0);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1 && !Util.FindObjectOfType<TextBubble>())
		{
			frames++;
			if (frames == 1)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/porky/PorkyFinalBeam"));
			}
			if (frames == 60)
			{
				Util.FindObjectOfType<BattleManager>().StopMusic();
				Util.FindObjectOfType<Porky>().GetPart("mech").Find("head")
					.GetComponent<SpriteRenderer>()
					.sprite = Resources.Load<Sprite>("battle/enemies/Porky/spr_b_porky_head_nohp");
				Object.Instantiate(Resources.Load<GameObject>("battle/Bash")).GetComponent<PlayerAttackAnimation>().AssignValues(Util.FindObjectOfType<Porky>(), 5, 20f, 1, 0);
				Object.Destroy(Util.FindObjectOfType<PorkyFinalBeam>().gameObject);
			}
			if (frames == 120)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
				Util.FindObjectOfType<BattleCamera>().BlastShake();
				Util.FindObjectOfType<Porky>().Hit(0, 10f, playSound: true);
			}
			if (frames >= 210 && !Util.FindObjectOfType<Porky>().IsShaking())
			{
				state = 2;
				frames = 0;
				Util.FindObjectOfType<Porky>().Chat(new string[5] { "W-^05what did you...?", "...NESS???\n^05How'd you...?!", "This isn't over,^05 \nlosers!", "Ness,^05 I'll be seeing \nyou and Paula very \nsoon...", "You'll pay for this!!!" }, "RightWide", "snd_txtpor", new Vector2(182f, 126f), canSkip: true, 0);
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
			if (!Util.FindObjectOfType<TextBubble>())
			{
				frames++;
				if (frames == 1)
				{
					Util.FindObjectOfType<Porky>().Explode();
				}
				if (frames == 150)
				{
					Util.FindObjectOfType<BattleManager>().FadeEndBattle(2);
				}
			}
			else if (Util.FindObjectOfType<TextBubble>().GetCurrentStringNum() == 2 && !ness)
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
		Util.GameManager().PlayGlobalSFX("sounds/snd_crash");
		bb.StartMovement(new Vector2(29f, 29f), new Vector2(0f, -0.52f), instant: true);
		Util.FindObjectOfType<SOUL>().transform.position = bbPos;
	}
}
