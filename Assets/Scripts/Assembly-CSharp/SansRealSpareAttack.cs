using UnityEngine;

public class SansRealSpareAttack : AttackBase
{
	private Sans sans;

	protected override void Awake()
	{
		base.Awake();
		sans = Util.FindObjectOfType<Sans>();
		Util.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		Util.FindObjectOfType<SOUL>().SetFrozen(boo: false);
		isStarted = true;
		sans.SetFace("closed_down_mad");
		sans.Chat(new string[16]
		{
			"...", "...", "you want the \ntruth that badly?", "fine.", "no,^05 she didn't \ndie.", "you know how i \nknow?", "she didn't turn to \ndust immediately.", "i...^10 i...", "i couldn't bring \nmyself to kill a \nchild.", "despite how much \ni've fallen into \nthis horrible \nmindset.",
			"despite how much \nshe looked like \nsusie.", "i just couldn't \ndo it.", "i thought it would \nbe a moment of \ncatharsis...", "to seek some \nkind of revenge \nfor what happened \nto me.", "but looking down \nat her,^05 the fear \nin her eyes...", "it was a reminder \nof how horrible \nmy life had become."
		}, "snd_txtsans", canSkip: true, 0);
		Util.GameManager().SetFlag(318, 1);
	}

	protected override void Update()
	{
		if (state == 0)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 3)
				{
					sans.SetFace("glare");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 8 || sans.GetTextBubble().GetCurrentStringNum() == 12)
				{
					sans.SetFace("cold");
					sans.SetSweat(1);
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 9 || sans.GetTextBubble().GetCurrentStringNum() == 16)
				{
					sans.SetFace("closed_unhappy");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 13)
				{
					sans.SetFace("realsad_side");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 15)
				{
					sans.SetFace("realsad");
				}
				return;
			}
			frames++;
			if (frames == 75)
			{
				sans.SetSweat(0);
				sans.Chat(new string[9] { "nobody deserves to \nlive in a place \nlike this.", "i'd already fallen \nso far.^10\nthere was no going \nback for me.", "i figured,^10 the only \nway to make this \nworld better...^10 was \nto destroy it.", "so i started \ngetting stronger.", "killing the weak \nand gaining LOVE...", "all i needed was a \nhuman soul,^05 and i'd \nfinally have the \npower to end it all.", "that way nobody \nwould have to \nsuffer anymore.", "and papyrus...", "he..." }, "snd_txtsans", canSkip: true, 0);
				sans.SetFace("closed_unhappy");
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 8)
				{
					sans.SetSweat(-1);
					sans.SetFace("realsad");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 9)
				{
					sans.SetFace("realsad_side");
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				Util.FindObjectOfType<PartyPanels>().transform.position = new Vector3(100f, 0f);
				Util.FindObjectOfType<TPBar>().transform.localPosition = new Vector3(-500f, 0f);
				Util.FindObjectOfType<DescriptionBox>().Vanish();
			}
			if (frames <= 60)
			{
				BattleButton[] array = Util.FindObjectsOfType<BattleButton>();
				foreach (BattleButton battleButton in array)
				{
					battleButton.GetComponent<SpriteRenderer>().color = new Color(battleButton.GetComponent<SpriteRenderer>().color.r, battleButton.GetComponent<SpriteRenderer>().color.g, battleButton.GetComponent<SpriteRenderer>().color.b, 1f - (float)frames / 60f);
				}
			}
			if (frames == 20)
			{
				sans.Bump();
			}
			if (frames == 40)
			{
				Util.FindObjectOfType<BattleManager>().PlayMusic("music/mus_sansspare", 1f, hasIntro: true);
			}
			if (frames == 90)
			{
				sans.Chat(new string[5] { "he just didn't \nget it.", "he always saw the \ngood in what this \nworld had to offer.", "despite all the \nsuffering,^05 the \npain...", "and the way i \ntried to get him \nto see things \nmy way...", "..." }, "snd_txtsans", canSkip: true, 0);
				sans.SetFace("realsad");
				state = 2;
				frames = 0;
			}
		}
		else if (state == 2)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 2)
				{
					sans.SetFace("realsad_side");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 3)
				{
					sans.SetFace("closed_unhappy");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 5)
				{
					sans.SetFace("realsad");
				}
				return;
			}
			frames++;
			if (frames == 20)
			{
				sans.ForceCombineParts();
				sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_0");
				sans.Bump();
			}
			if (frames == 50)
			{
				sans.Chat(new string[6] { "oh, god.^10\nwhat did i do to my \nbrother...?", "i've been so \nblindsighted by my \ngoal...", "that all i've done \nis make things \nworse.", "for both of us.", "for everyone here \nin snowdin.", "i just..." }, "snd_txtsans", canSkip: true, 0);
				state = 3;
				frames = 19;
			}
		}
		else if (state == 3)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 4)
				{
					sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_1");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 6)
				{
					sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_2");
				}
				return;
			}
			frames++;
			if (frames == 20 || frames == 50 || frames == 70 || frames == 85 || frames == 95 || frames == 102 || frames == 107 || frames == 103 || frames == 106 || frames == 109)
			{
				sans.Bump();
			}
			if (frames == 112)
			{
				sans.Unhostile();
				sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_3");
			}
			if (frames == 160)
			{
				sans.Chat(new string[1] { "i thought it would \nall be worth it." }, "snd_txtsans", Util.GameManager().IsTestMode(), 1);
				sans.GetTextBubble().GetComponent<ShakingText>().StartShake(5, "sans");
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4 && !sans.GetTextBubble())
		{
			frames++;
			if (frames == 30)
			{
				Util.FindObjectOfType<BattleManager>().StopMusic();
				sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_4");
				sans.Chat(new string[1] { "SANS...?^10\nYOU..." }, "LeftSmall", "snd_txtpap", new Vector2(244f, 109f), canSkip: true, 0);
			}
			if (frames == 45)
			{
				Util.GameManager().AddGold(sans.GetGold() * 2 / 3);
				Util.FindObjectOfType<BattleManager>().FadeEndBattle(2);
			}
		}
	}
}
