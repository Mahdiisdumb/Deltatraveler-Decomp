using UnityEngine;

public class SansSpareAbortAttack : AttackBase
{
	private Sans sans;

	private int sleepFrames;

	protected override void Awake()
	{
		base.Awake();
		bbPos = new Vector2(0f, -2.37f);
		bbSize = new Vector2(575f, 140f);
		Util.FindObjectOfType<PartyPanels>().DeactivateTargets();
		Util.FindObjectOfType<PartyPanels>().RaiseHeads(kris: false, susie: false, noelle: false);
		Util.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().enabled = false;
		sans = Util.FindObjectOfType<Sans>();
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
				sans.Chat(new string[8] { "what...?", "after the massacre \ni saw you commit...", "after the torture \nyou put your \nteammates through...", "you're sparing the \none person that you \nhad every right to \nkill.", "...", "i...^10 realize how \nmuch i've gone off \nthe deep end.", "but compared to \nyou...?", "only a demented god \nwould have the gall \nto do something \nlike this." }, "snd_txtsans", canSkip: true, 1);
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 5)
				{
					sans.SetFace("closed");
					sans.GetPart("body").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_torso");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 7)
				{
					sans.SetFace("closed_down");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 8)
				{
					sans.SetFace("glare");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				sans.SetFace("closed_down");
			}
			if (frames == 30)
			{
				sans.SetFace("closed_sleep");
				sans.PlaySFX("sounds/snd_wing");
				sans.ResetBreatheAnimation();
				sans.StopBreatheAnimation();
				sans.GetPart("legs").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_legs");
			}
			if (frames == 75)
			{
				sans.Chat(new string[2] { "i'm gonna go catch \nsome z's elsewhere.", "you three can wallow \nin your pathetic \nexistence." }, "snd_txtsans", canSkip: true, 1);
				state = 2;
				frames = 0;
			}
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 2)
				{
					sans.SetFace("empty_down");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				sans.SetFace("closed_sleep");
				sans.PlaySFX("sounds/snd_escaped");
			}
			sans.transform.position += new Vector3(1f / 12f, 0f);
			if (frames == 90)
			{
				Util.GameManager().AddGold(sans.GetGold() / 2);
				Util.FindObjectOfType<BattleManager>().FadeEndBattle(2);
			}
		}
	}
}
