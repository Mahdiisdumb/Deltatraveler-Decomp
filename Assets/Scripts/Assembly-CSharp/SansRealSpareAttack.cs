using UnityEngine;

public class SansRealSpareAttack : AttackBase
{
	private Sans sans;

	protected override void Awake()
	{
		base.Awake();
		sans = Object.FindObjectOfType<Sans>();
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(0);
		Object.FindObjectOfType<SOUL>().SetFrozen(false);
		isStarted = true;
		sans.SetFace("closed_down_mad");
		sans.Chat(new string[16]
		{
			"...", "...", "you want the truth \nthat badly?", "fine.", "no,^05 she didn't die.", "you know how i know?", "she didn't turn to \ndust immediately.", "i...^10 i...", "i couldn't bring myself \nto kill a child.", "despite how much i've \nfallen into this horrible \nmindset.",
			"despite how much she \nlooked like susie.", "i just couldn't do it.", "i thought it would be \na moment of catharsis...", "to seek some kind of \nrevenge for what \nhappened to me.", "but looking down at \nher,^05 the fear in her \neyes...", "it was a reminder \nof how horrible my \nlife has become."
		}, "snd_txtsans", true, 0);
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
				sans.Chat(new string[9] { "nobody deserves to live \nin a place like this.", "i'd already fallen so far.^10\nthere was no going back \nfor me.", "i figured, the only way \nto make this world \nbetter...^10 was to destroy \nit.", "so i started getting \nstronger.", "killing the weak and \ngaining LOVE...", "all i needed was a \nhuman soul,^05 and i'd \nfinally have the power \nto end everything.", "that way nobody would \nhave to suffer anymore.", "and papyrus...", "he..." }, "snd_txtsans", true, 0);
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
				Object.FindObjectOfType<PartyPanels>().transform.position = new Vector3(100f, 0f);
				Object.FindObjectOfType<TPBar>().transform.localPosition = new Vector3(-500f, 0f);
				Object.FindObjectOfType<DescriptionBox>().Vanish();
			}
			if (frames <= 60)
			{
				BattleButton[] array = Object.FindObjectsOfType<BattleButton>();
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
				Object.FindObjectOfType<BattleManager>().PlayMusic("music/mus_sansspare", 1f, true);
			}
			if (frames == 90)
			{
				sans.Chat(new string[5] { "he just didn't get it.", "he always saw the good \nin what this world \nhad to offer.", "despite all the suffering,^05 \nthe pain...", "and the way i tried to \nget him to see things \nmy way...", "..." }, "snd_txtsans", true, 0);
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
				sans.Chat(new string[5] { "oh, god.^10\nwhat did i do to my \nbrother...?", "i've been so blindsighted \nby my goal that all i've \ndone is make things \nworse.", "for both of us.", "for everyone here in \nsnowdin.", "i just..." }, "snd_txtsans", true, 0);
				state = 3;
				frames = 19;
			}
		}
		else if (state == 3)
		{
			if ((bool)sans.GetTextBubble())
			{
				if (sans.GetTextBubble().GetCurrentStringNum() == 3)
				{
					sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_1");
				}
				else if (sans.GetTextBubble().GetCurrentStringNum() == 5)
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
				sans.Chat(new string[1] { "i thought it would all \nbe worth it." }, "snd_txtsans", Util.GameManager().IsTestMode(), 1);
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
				Object.FindObjectOfType<BattleManager>().StopMusic();
				sans.GetEnemyObject().transform.Find("mainbody").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/enemies/Sans/spr_b_sans_regret_4");
				sans.Chat(new string[1] { "SANS...?^10\nYOU..." }, "LeftSmall", "snd_txtpap", new Vector2(244f, 109f), true, 0);
			}
			if (frames == 45)
			{
				Util.GameManager().AddGold(sans.GetGold() * 2 / 3);
				Object.FindObjectOfType<BattleManager>().FadeEndBattle(2);
			}
		}
	}
}
