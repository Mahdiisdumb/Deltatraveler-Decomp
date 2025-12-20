using System.Collections.Generic;
using UnityEngine;

public class JerryDefeatCutscene : CutsceneBase
{
	private int endState;

	private bool attackWhenSpare;

	private bool ditched;

	private bool vigilante;

	private bool abort;

	private bool gotItem;

	private Transform jerry;

	private int killEndVariant;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == -1)
		{
			frames++;
			if (frames == 30)
			{
				frames = 0;
				state = 1;
				if (endState == 2)
				{
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
					StartText(new string[3]
					{
						"* Did we,^05 uhh...",
						abort ? "* Really just end our\n  killstreak by doing\n  THAT???" : "* Really need to do that?",
						"* I'm still here,^05 you know!!!"
					}, new string[3] { "snd_txtsus", "snd_txtsus", "snd_text" }, new int[1], new string[3]
					{
						"su_neutral",
						abort ? "su_inquisitive" : "su_side",
						""
					});
				}
				else
				{
					state = 6;
					StartText(new string[3] { "* Yeesh,^05 that was...^05\n* Something.", "* You good,^05 Kris?", "* Hey,^05 his sword is\n  still here." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[3] { "su_inquisitive", "su_neutral", "no_curious" });
				}
			}
		}
		else if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				SetSprite(jerry, "overworld/npcs/underfell/spr_jerry_uf_postbattle_putawaysword_0");
			}
			if (frames == 20)
			{
				PlaySFX("sounds/snd_smallswing");
				SetSprite(jerry, "overworld/npcs/underfell/spr_jerry_uf_postbattle_putawaysword_1");
			}
			if (frames == 40)
			{
				SetSprite(jerry, "overworld/npcs/underfell/spr_jerry_uf_postbattle");
				frames = 0;
				state = 2;
				StartText(new string[4] { "* I won't kill you,^05 human.", "* In fact,^05 I think I owe you\n  something.", "* ...^05 No,^05 not the sword.^05\n* I need it to survive.", "* But..." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			}
		}
		else if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				ChangeDirection(kris, Vector2.up);
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
			}
			jerry.transform.position = new Vector3(Mathf.Lerp(8.16f, 0.019f, (float)frames / 30f), 14.665f);
			if (frames == 40)
			{
				frames = 0;
				state = 0;
				StartText(new string[3] { "* You know,^05 that could've gotten\n  you killed!", "* But...^05 you showed me a better\n  future.", "* So I'll forgive you for\n  doing that." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			}
		}
		else if (state == 2 && !txt)
		{
			frames++;
			jerry.position = Vector3.Lerp(new Vector3(0.019f, 14.665f), new Vector3(-1.74f, 12.93f), (float)frames / 45f);
			if (frames == 35)
			{
				ChangeDirection(susie, Vector2.left);
				ChangeDirection(noelle, Vector2.left);
			}
			if (frames == 55)
			{
				frames = 0;
				state = 3;
				List<string> list = new List<string> { "* This...^05 bandana that I've been\n  wearing.", "* It possesses a special power\n  that has let me build up\n  more power in battle.", "* I probably don't need this\n  thing anymore.", "* My swordsmanship has gotten a\n  lot more...^05 epic.", "* So take it." };
				if (gm.NumItemFreeSpace() > 0)
				{
					gotItem = true;
					gm.AddItem(40);
					list.AddRange(new string[1] { "* (You got the " + Items.ItemName(40) + ".)" });
				}
				else
				{
					gm.SetFlag(272, 2);
					list.AddRange(new string[3] { "* ... You don't have room?", "* Well...", "* I'll just put it down,^05 and\n  you can pick it up later.^05\n* Heh." });
				}
				StartText(list.ToArray(), new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (AtLine(6) && gotItem)
				{
					PlaySFX("sounds/snd_item");
					SetSprite(jerry, "overworld/npcs/underfell/spr_jerry_uf_0");
				}
				else if (AtLine(8) && !gotItem)
				{
					Object.FindObjectOfType<JerryObjectHandler>().PlaceObject(false);
					SetSprite(jerry, "overworld/npcs/underfell/spr_jerry_uf_0");
				}
				return;
			}
			frames++;
			if (frames == 10)
			{
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
			}
			jerry.position = Vector3.Lerp(new Vector3(-1.74f, 12.93f), new Vector3(0.019f, 14.665f), (float)frames / 45f);
			if (frames == 55)
			{
				vigilante = (int)gm.GetFlag(87) >= 8;
				if (vigilante)
				{
					gm.SetFlag(275, 1);
				}
				List<string> list2 = new List<string> { "* So I thought about how\n  I could turn my life\n  around.", "* But I think you guys are\n  the perfect model for how\n  I could do that!", "* So...^05 I think..." };
				List<string> list3 = new List<string> { "snd_text", "snd_text", "snd_text" };
				List<string> list4 = new List<string> { "", "", "" };
				if (!vigilante)
				{
					list2.AddRange(new string[7] { "* I'm gonna be the nicest guy\n  to ever grace this place!", "* Everyone's gonna get a\n  little bit of help\n  from me!", "* Uhhhh", "* What about the whale\n  guy you just killed?", "* ...", "* What they don't know\n  won't hurt them.", "* Fair point...?" });
					list3.AddRange(new string[7] { "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_text", "snd_text", "snd_txtsus" });
					list4.AddRange(new string[7] { "", "", "su_annoyed", "su_side", "", "", "su_inquisitive" });
				}
				else
				{
					list2.AddRange(new string[4] { "* I'm gonna rid this place\n  of all the terrible people\n  making life worse!", "* Starting with...", "<color=#FF0000FF>* SANS THE SKELETON!!!</color>", "* Wait don't---!" });
					list3.AddRange(new string[4] { "snd_text", "snd_text", "snd_text", "snd_txtsus" });
					list4.AddRange(new string[4] { "", "", "", "su_concerned" });
				}
				list2.AddRange(new string[2] { "* Well,^05 I'm off to make a\n  difference on the world!", "* Seeeeeeee^03 ya!" });
				list3.Add("snd_text");
				list4.Add("");
				StartText(list2.ToArray(), list3.ToArray(), new int[1], list4.ToArray());
				state = 4;
				frames = 0;
			}
		}
		else if (state == 4)
		{
			if ((bool)txt && ((vigilante && txt.GetCurrentStringNum() < 9) || (!vigilante && txt.GetCurrentStringNum() < 12)))
			{
				if (vigilante)
				{
					if (AtLine(6))
					{
						SetSprite(kris, "spr_kr_surprise_upright");
						SetSprite(susie, "spr_su_surprise_up");
						SetSprite(noelle, "spr_no_surprise_up");
					}
					else if (AtLine(8))
					{
						kris.EnableAnimator();
						susie.EnableAnimator();
						noelle.EnableAnimator();
					}
				}
			}
			else if (frames < 15)
			{
				frames++;
				jerry.position = new Vector3(0f, Mathf.Lerp(14.665f, 9.43f, (float)frames / 15f));
				if (jerry.position.y <= 12.15f)
				{
					jerry.GetComponent<SpriteRenderer>().sortingOrder = 0;
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.down);
					ChangeDirection(noelle, Vector2.left);
				}
				if (frames == 15)
				{
					jerry.position = new Vector3(11f, 9.5f);
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(susie, Vector2.down);
					ChangeDirection(noelle, Vector2.down);
				}
			}
			else
			{
				if ((bool)txt)
				{
					return;
				}
				frames++;
				if (frames == 45)
				{
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
					state = 5;
					frames = 0;
					if (vigilante)
					{
						StartText(new string[8] { "* He's dead.", "* Maybe if we get\n  there first...?", "* Nah,^05 you see how\n  quick the little guy\n  is?", "* He's definitely gonna\n  beat us to him.", "* Even when trying to\n  help someone,^05 we\n  kill them.", "* I'm sure it isn't\n  like that,^05 Susie.", "* Whatever.", "* Let's go." }, new string[8] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], new string[8] { "su_neutral", "no_thinking", "su_annoyed", "su_side", "su_dejected", "no_happy", "su_depressed", "su_neutral" });
					}
					else
					{
						StartText(new string[7] { "* He's gonna annoy the\n  hell out of everyone,^05\n  isn't he?", "* I'm sure someone will\n  appreciate the help!", "* And if no one\n  does...?", "* Well,^05 he offered that\n  bandana to us,^05 right?", "* I think that's worth\n  something.", "* I guess.", "* Let's just get\n  going." }, new string[7] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], new string[7] { "su_annoyed", "no_happy", "su_smirk", "no_confused_side", "no_confused", "su_side", "su_neutral" });
					}
				}
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (vigilante)
				{
					if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(8))
					{
						ChangeDirection(susie, Vector2.left);
					}
				}
				else if (AtLine(7))
				{
					ChangeDirection(susie, Vector2.left);
				}
			}
			else if (!MoveTo(cam, cam.GetClampedPos(), 3f))
			{
				gm.PlayMusic("zoneMusic");
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				EndCutscene();
			}
		}
		else if (state == 6)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(susie, Vector2.left);
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				kris.EnableAnimator();
				PlaySFX("sounds/snd_wing");
				ChangeDirection(susie, Vector2.up);
			}
			if (frames == 40)
			{
				if ((int)Util.GameManager().GetFlag(84) > 1 && !WeirdChecker.HasCommittedBloodshed(gm))
				{
					killEndVariant = 1;
					StartText(new string[7] { "* Huh.", "* Well,^05 uhh...", "* Sure.", "* Is something wrong,^05\n  Susie?", "* No,^05 but...", "* Nevermind.", "* I guess you can\n  take it,^05 Kris." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[7] { "su_side", "su_side_sweat", "su_annoyed", "no_confused", "su_side", "su_dejected", "su_neutral" });
				}
				else if ((int)Util.GameManager().GetFlag(12) == 0 && WeirdChecker.HasCommittedBloodshed(gm))
				{
					killEndVariant = 2;
					StartText(new string[6] { "* Huh.", "* Well,^05 uhh...", "* Sure.", "* Prolly not gonna\n  make much of a\n  dent,^05 or...", "* A cut.", "* But sure." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[6] { "su_side", "su_side_sweat", "su_annoyed", "su_annoyed", "su_depressed", "su_dejected" });
				}
				else if ((int)Util.GameManager().GetFlag(12) == 1)
				{
					killEndVariant = 3;
					StartText(new string[7] { "* Huh.", "* Well,^05 uhh...", "* Sure.", "* (Susie,^05 are you sure\n  that isn't going to...)", "* (I don't think it's\n  anywhere as powerful\n  as Paula's pan.)", "* (Okay...)", "* I...^05 I guess it's\n  your choice,^05 Kris." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[7] { "su_side", "su_side_sweat", "su_annoyed", "no_shocked", "su_smile_sweat", "no_depressed_side", "su_neutral" });
				}
				else
				{
					StartText(new string[4] { "* Oh shoot!", "* You should totally\n  nab it,^05 Kris.", "* You like swords,^05 right?", "* Bet you could do\n  some cool shit like\n  that guy did." }, new string[1] { "snd_txtsus" }, new int[1], new string[4] { "su_surprised", "su_smile", "su_smile_side", "su_happy" });
				}
				frames = 0;
				state = 7;
			}
		}
		else
		{
			if (state != 7)
			{
				return;
			}
			if ((bool)txt)
			{
				if (killEndVariant == 0)
				{
					if (AtLine(2))
					{
						ChangeDirection(susie, Vector2.left);
						susie.UseHappySprites();
						ChangeDirection(kris, Vector2.right);
					}
				}
				else if (killEndVariant == 1)
				{
					if (AtLine(4))
					{
						ChangeDirection(noelle, Vector2.left);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
					}
					else if (AtLine(6))
					{
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(7))
					{
						ChangeDirection(susie, Vector2.left);
						ChangeDirection(kris, Vector2.right);
					}
				}
				else if (killEndVariant == 3)
				{
					if (AtLine(4))
					{
						ChangeDirection(noelle, Vector2.left);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
					}
					else if (AtLine(7))
					{
						ChangeDirection(susie, Vector2.left);
						ChangeDirection(kris, Vector2.right);
					}
				}
			}
			else if (!MoveTo(cam, cam.GetClampedPos(), 3f))
			{
				gm.PlayMusic("zoneMusic");
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		RevokePlayerControl();
		Object.FindObjectOfType<StepEncounterer>().enabled = false;
		gm.StopMusic();
		gm.SetPartyMembers(true, true);
		attackWhenSpare = (int)Util.GameManager().GetFlag(269) == 1;
		ditched = (int)Util.GameManager().GetFlag(271) == 1;
		kris.transform.position = new Vector3(-1.7f, 12.43f);
		susie.transform.position = new Vector3(0f, 12.63f);
		noelle.transform.position = new Vector3(1.7f, 12.65f);
		cam.transform.position = new Vector3(0f, 14.167f, -10f);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		ChangeDirection(kris, Vector2.up);
		ChangeDirection(susie, Vector2.up);
		ChangeDirection(noelle, Vector2.up);
		if (endState == 2)
		{
			jerry = GameObject.Find("Jerry").transform;
			abort = (int)Util.GameManager().GetFlag(12) == 1;
			if (abort)
			{
				WeirdChecker.Abort(gm);
			}
			if (!ditched)
			{
				jerry.position = new Vector3(0.019f, 14.665f);
				StartText(new string[3] { "* ...", "* I've...^05 made up my mind.", "* You showed me a better future." }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
			}
			else
			{
				state = -1;
			}
		}
		else
		{
			state = -1;
			gm.SetFlag(272, 1);
			Object.FindObjectOfType<JerryObjectHandler>().PlaceObject(true);
			SetSprite(kris, "spr_kr_sit");
		}
		PlayerPrefs.SetInt("JerryDefeated", 1);
	}
}
