using System.Collections.Generic;
using UnityEngine;

public class QCFirstCutscene : CutsceneBase
{
	private Animator qc;

	private bool noellePresent;

	private bool blood;

	private bool alt;

	private int susieMoveState;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			frames++;
			if (qc.enabled)
			{
				if (!MoveTo(qc, new Vector3(-0.938f, 1.125f), 4f))
				{
					qc.enabled = false;
					SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit");
					PlaySFX("sounds/snd_wing");
				}
				else
				{
					ChangeDirection(qc, Vector2.up);
					SetMoveAnim(qc, isMoving: true);
				}
			}
			if (frames >= 10)
			{
				if (!MoveTo(kris, new Vector3(0.79f, -1.18f), 3f))
				{
					SetMoveAnim(kris, isMoving: false);
				}
				else
				{
					SetMoveAnim(kris, isMoving: true, 0.6f);
				}
			}
			if (frames >= 15 && noellePresent)
			{
				if (!MoveTo(noelle, new Vector3(-0.69f, -0.7f), 4f))
				{
					SetMoveAnim(noelle, isMoving: false);
				}
				else
				{
					SetMoveAnim(noelle, isMoving: true);
				}
			}
			if (frames >= 20)
			{
				if (frames == 20)
				{
					SetMoveAnim(susie, isMoving: true);
				}
				if (susieMoveState == 0 && !MoveTo(susie, new Vector3(1.33f, -2.4f), 4f))
				{
					susieMoveState = 1;
				}
				else if (susieMoveState == 1 && !MoveTo(susie, new Vector3(1.33f, -0.14f), 6f))
				{
					susieMoveState = 2;
					ChangeDirection(susie, Vector2.right);
				}
				else if (susieMoveState == 2 && !MoveTo(susie, new Vector3(1.96f, -0.14f), 6f) && susie.GetComponent<SpriteRenderer>().enabled)
				{
					susie.GetComponent<SpriteRenderer>().enabled = false;
					ChangeDirection(susie, Vector2.left);
					SetMoveAnim(susie, isMoving: false);
					string text = (noellePresent ? "0" : "1");
					SetSprite(GameObject.Find("toplayer").transform, "overworld/snow_objects/spr_bnuy_home_0_susie_" + text);
				}
			}
			if (frames != 90)
			{
				return;
			}
			List<string> list = new List<string>
			{
				"* Much nicer than the dark\n  forest,^05 huh?",
				"* Yeah.",
				"* A bit cramped,^05\n  though.",
				"* Why do you guys\n  even live down here?",
				"* Well,^05 we all lived in\n  Snowdin Town a long\n  time ago.",
				"* Wasn't the best place,^05\n  given how the Underground\n  is...",
				"* But it was nice enough.",
				"* That was until the two brothers\n  snapped one day...",
				"* They started going on a\n  murderous rampage.",
				"* I know killing isn't too\n  uncommon here,^05 but this was\n  just a massacre.",
				"* Got to the point where we\n  had to hide ourselves\n  somewhere to avoid them.",
				"* I still run my shop there\n  to keep up on supplies.",
				"* Well,^05 up until recently.",
				"* What happened?",
				"* A bunch of wild snowdrakes\n  attacked me on my way\n  down here a week ago.",
				"* Broke the ladder into\n  pieces!",
				"* I got most of the pieces,^05\n  but I'm still missing a\n  few of them.",
				"* Left my axe at the shop,^05\n  too...",
				"* I've been stuck here ever\n  since,^05 trying to find them\n  before I try and tempt fate.",
				noellePresent ? "* Well,^05 we happened to\n  get one of them\n  just now." : "* Oh,^05 umm...^10\n* I think I'm holding\n  onto one of them.",
				"* Apparently one of the\n  snowdrakes was holding\n  onto it.",
				"* Really???",
				"* That sounds wonderful,^05\n  actually!",
				"* I've never been able to\n  get near 'em,^05 let alone\n  put a dent into them.",
				"* Would you mind helping find\n  those pieces?",
				"* Well,^05 uhh...",
				"* It's the only way\n  we can go forward.",
				"* We're tryna get to\n  Hotland.",
				"* Well,^05 then we need to get\n  those pieces!",
				"* But I imagine that might\n  be a harrowing task..."
			};
			List<string> list2 = new List<string>
			{
				"snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text",
				"snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus",
				"snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_text"
			};
			List<string> list3 = new List<string>
			{
				"",
				"su_smile",
				"su_smile_sweat",
				"su_neutral",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"",
				"su_surprised",
				"",
				"",
				"",
				"",
				"",
				noellePresent ? "su_surprised" : "su_inquisitive",
				noellePresent ? "su_smile_side" : "su_smirk_sweat",
				"",
				"",
				"",
				"",
				"su_side_sweat",
				"su_smile_sweat",
				"su_neutral",
				"",
				""
			};
			if (noellePresent)
			{
				list.AddRange(new string[15]
				{
					"* ...^05 Say,^05 I just noticed...^05\n* Is that you,^05 Ms. <color=#FFFF00FF>Noel</color>?",
					"* Huh???",
					"* Well,^05 yeah...",
					"* You've grown up a lot faster\n  than I expected...",
					"* Tell you what?\n* Since y'all're helping me out,^05\n  I'll let y'all stay the night.",
					"* Heck,^05 I insist you take a rest\n  right now before you get going!",
					blood ? "* Hey,^05 we needed to\n  take a break\n  anyway." : "* Umm,^05 I guess we\n  can.",
					"* I think we only have two\n  bedrooms available to sleep\n  in...",
					"* Two of y'all might need to\n  share a bed.",
					alt ? "* ..." : "* ...!!!",
					alt ? "* Kris,^05 you should\n  take the single bed." : "* Kris,^05 you should\n  probably take the\n  single bed.",
					"* WHAT???",
					"* But I don't want to\n  share a bed!!!",
					alt ? "* Susie..." : "* Do you want Kris\n  to steal all the\n  blankets?",
					alt ? "* ...^05 Oh,^05 right...^05\n* Guess I will,^05 then." : "* ...^05 Okay,^05 fine.^05\n* I'll share with Noelle."
				});
				list2.AddRange(new string[15]
				{
					"snd_text", "snd_txtnoe", "snd_txtnoe", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_txtnoe",
					"snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus"
				});
				list3.AddRange(new string[15]
				{
					"",
					"no_shocked",
					"no_weird",
					"",
					"",
					"",
					blood ? "su_smile" : "su_inquisitive",
					"",
					"",
					alt ? "no_depressed" : "no_surprised_happy",
					alt ? "no_thinking" : "no_confused_side",
					"su_wtf",
					"su_angry",
					alt ? "no_mad" : "no_tease_finger",
					alt ? "su_inquisitive" : "su_flustered"
				});
				if (blood)
				{
					list.AddRange(new string[4] { "* But uhh,^05 Kris.", "* Don't think I forgot\n  about that talk.", "* We're talking after\n  we wake up.", "* Now let's go." });
					list2.AddRange(new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
					list3.AddRange(new string[4] { "su_neutral", "su_side", "su_annoyed", "su_side" });
				}
			}
			else
			{
				list.AddRange(new string[7] { "* Ms. <color=#FFFF00FF>Noel</color> is already upstairs\n  sleeping in the large bedroom.", "* One of y'all will need to\n  share with her.", "* Umm...", "* ...", "* I guess I will.", "* Kris,^05 we're gonna have\n  that talk after we\n  wake up.", "* So you better sleep\n  well." });
				list2.AddRange(new string[7] { "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
				list3.AddRange(new string[7] { "", "", "su_surprised", "su_side_sweat", "su_flustered", "su_neutral", "su_annoyed" });
			}
			StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray(), 1);
			state = 1;
			frames = 0;
			SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_4");
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() <= 30)
				{
					if (AtLine(5) || AtLine(8) || AtLine(20) || AtLine(29))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_1");
					}
					else if (AtLine(6) || AtLine(11) || AtLine(17))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_3");
					}
					else if (AtLine(9) || AtLine(28))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_2");
					}
					else if (AtLine(10) || AtLine(18))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_5");
					}
					else if (AtLine(15))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_8");
					}
					else if (AtLine(21))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_9");
					}
					else if (AtLine(23))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_4");
					}
					else if (AtLine(14))
					{
						SetSprite(GameObject.Find("toplayer").transform, "overworld/snow_objects/spr_bnuy_home_0_susie_1");
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_1");
					}
				}
				else if (noellePresent)
				{
					if (AtLine(31))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_6");
					}
					else if (AtLine(35))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_7");
					}
					else if (AtLine(36))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_4");
					}
					else if (AtLine(38))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_1");
					}
					else if (AtLine(39))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_3");
					}
					else if (AtLine(41))
					{
						ChangeDirection(noelle, Vector2.right);
						ChangeDirection(kris, Vector2.left);
					}
					else if (AtLine(42))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_9");
						ChangeDirection(kris, Vector2.right);
						SetSprite(GameObject.Find("toplayer").transform, "overworld/snow_objects/spr_bnuy_home_0_toplayer");
						susie.GetComponent<SpriteRenderer>().enabled = true;
						SetSprite(susie, "spr_su_wtf", flipX: true);
					}
					else if (AtLine(44))
					{
						if (alt)
						{
							SetSprite(noelle, "spr_no_blush");
						}
						else
						{
							ChangeDirection(noelle, Vector2.up);
						}
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_3");
					}
					else if (AtLine(45))
					{
						SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_1");
						susie.GetComponent<SpriteRenderer>().flipX = false;
						PlayAnimation(susie, "Embarrassed");
						ChangeDirection(susie, Vector2.up);
					}
					if (blood && AtLine(46))
					{
						susie.UseUnhappySprites();
						noelle.EnableAnimator();
						noelle.UseUnhappySprites();
						PlayAnimation(susie, "idle");
						ChangeDirection(susie, Vector2.left);
						ChangeDirection(kris, Vector2.right);
						ChangeDirection(noelle, Vector2.right);
					}
				}
				else if (AtLine(31))
				{
					SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_2");
				}
				else if (AtLine(32))
				{
					SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_sit_1");
				}
				else if (AtLine(33))
				{
					ChangeDirection(kris, Vector2.right);
					SetSprite(GameObject.Find("toplayer").transform, "overworld/snow_objects/spr_bnuy_home_0_toplayer");
					susie.GetComponent<SpriteRenderer>().enabled = true;
				}
				else if (AtLine(34))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(35))
				{
					PlayAnimation(susie, "Embarrassed");
				}
				else if (AtLine(36))
				{
					PlayAnimation(susie, "idle");
					ChangeDirection(susie, Vector2.left);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					PlayAnimation(susie, "walk");
					ChangeDirection(susie, Vector2.up);
					ChangeDirection(noelle, Vector2.right);
					noelle.EnableAnimator();
					SetMoveAnim(kris, isMoving: true);
					SetMoveAnim(susie, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
				}
				MoveTo(kris, new Vector3(3.48f, 1.44f), 3f);
				MoveTo(susie, new Vector3(4.3f, 3.44f), 3f);
				if (noellePresent)
				{
					MoveTo(noelle, new Vector3(3.38f, 1.27f), 3f);
				}
				if (frames == 30)
				{
					fade.FadeOut(15);
				}
				if (frames == 45)
				{
					gm.LoadArea(89, fadeIn: true, new Vector2(-3.32f, -2.46f), Vector2.up);
				}
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		gm.SetFlag(172, 0);
		noellePresent = Util.GameManager().NoelleInParty();
		blood = WeirdChecker.HasCommittedBloodshed(gm);
		alt = (int)gm.GetFlag(87) >= 7;
		qc = GameObject.Find("QC").GetComponent<Animator>();
		qc.enabled = true;
		qc.transform.position = new Vector3(-0.13f, -0.48f);
		Object.Destroy(Util.FindObjectOfType<SAVEPoint>().gameObject);
		StartText(new string[1] { "* Welcome inside!" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
	}
}
