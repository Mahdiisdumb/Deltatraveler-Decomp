using System.Collections.Generic;
using UnityEngine;

public class DogiCutscene : CutsceneBase
{
	private bool selecting;

	private int selIndex;

	private bool oblit;

	private int oblitDistance;

	private int oblitDif;

	private InteractTextBox dogamy;

	private InteractTextBox dogaressa;

	private string[] noelleDiag = new string[3] { "* W-^05we just helped a\n  rabbit family escape\n  from the deep f-^05forest!", "* We had to f-^05fight a\n  few wild S-^05Snowdrakes\n  down there,^05 so...", "* M-^05maybe it's that...?" };

	private string[] noelleSounds = new string[3] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" };

	private int[] noelleSpeed = new int[3];

	private string[] noellePortrats = new string[3] { "no_confused", "no_confused_side", "no_weird" };

	private string[] dogamyDiag = new string[5] { "* .................", "* Yeah,^05 that makes sense.", "* (Get back to town,^05 all of you.)", "* (It's dangerous out here,^05\n  especially with a human\n  prowling around.)", "* Okay...^05 got it." };

	private string[] dogamySounds = new string[5] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus" };

	private int[] dogamySpeed = new int[2] { 4, 0 };

	private string[] dogamyPortrats = new string[5] { "", "", "", "", "su_inquisitive" };

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			if (frames == 0)
			{
				kris.EnableAnimator();
				susie.EnableAnimator();
				noelle.EnableAnimator();
				SetMoveAnim(kris, true, 1.5f);
				SetMoveAnim(susie, true, 1.5f);
				SetMoveAnim(noelle, true, 1.5f);
				ChangeDirection(kris, Vector2.left);
				ChangeDirection(susie, (susie.transform.position.x > 8.402f) ? Vector2.left : Vector2.right);
				ChangeDirection(noelle, (noelle.transform.position.x > 9.494f) ? Vector2.left : Vector2.right);
			}
			frames++;
			if (kris.transform.position.x != 7.302f)
			{
				MoveTo(kris, new Vector3(7.302f, kris.transform.position.y), 8f);
			}
			else if (MoveTo(kris, new Vector3(7.302f, -1.63f), 8f))
			{
				ChangeDirection(kris, Vector2.down);
			}
			else
			{
				ChangeDirection(kris, Vector2.up);
				SetMoveAnim(kris, false);
			}
			if (susie.transform.position.x != 8.402f)
			{
				MoveTo(susie, new Vector3(8.402f, susie.transform.position.y), 8f);
			}
			else if (MoveTo(susie, new Vector3(8.402f, -1.47f), 8f))
			{
				ChangeDirection(susie, Vector2.down);
			}
			else
			{
				ChangeDirection(susie, Vector2.up);
				SetMoveAnim(susie, false);
			}
			if (noelle.transform.position.x != 9.494f)
			{
				MoveTo(noelle, new Vector3(9.494f, noelle.transform.position.y), 8f);
			}
			else if (MoveTo(noelle, new Vector3(9.494f, -1.468f), 8f))
			{
				ChangeDirection(noelle, Vector2.down);
			}
			else
			{
				ChangeDirection(noelle, Vector2.up);
				SetMoveAnim(noelle, false);
			}
			if (!MoveTo(dogamy.transform.parent, new Vector3(8.43f, 0.27f), 8f))
			{
				dogamy.GetComponent<SpriteRenderer>().flipX = false;
				PlayAnimation(dogamy, "Idle");
				PlayAnimation(dogaressa, "Idle");
			}
			if (frames == 90)
			{
				dogamy.GetComponent<Animator>().enabled = false;
				dogaressa.GetComponent<Animator>().enabled = false;
				List<string> list = new List<string> { "* We've heard multiple reports of\n  a human wandering around with\n  a couple of monsters.", "* (Have you three seen this\n  human?)", "* Uhhhhhhhhhh", "* ...", "* Nope.", "* I uhh...^05 have no\n  idea what a human\n  looks like.", "* Are you sure...?", "* (You three smell strange...)", "* Well,^05 uhh...", "* We just,^05 umm..." };
				List<string> list2 = new List<string> { "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus" };
				List<int> list3 = new List<int> { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 };
				List<string> list4 = new List<string> { "", "", "su_side_sweat", "su_neutral", "su_side", "su_smirk_sweat", "", "", "su_wideeye", "su_sus" };
				if (!oblit)
				{
					list.AddRange(noelleDiag);
					list2.AddRange(noelleSounds);
					list3.AddRange(noelleSpeed);
					list4.AddRange(noellePortrats);
					list.AddRange(dogamyDiag);
					list2.AddRange(dogamySounds);
					list3.AddRange(dogamySpeed);
					list4.AddRange(dogamyPortrats);
				}
				StartText(list.ToArray(), list2.ToArray(), list3.ToArray(), list4.ToArray());
				dogamy.SetTalkable(txt);
				state = 1;
				frames = 0;
				if (oblit)
				{
					txt.EnableSelectionAtEnd();
				}
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (txt.CanLoadSelection() && !selecting)
				{
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "...", Vector3.zero);
					select.SetupChoice(Vector2.right, "I'm actually\na human.", new Vector3(-70f, 0f));
					select.Activate(this, 1, txt.gameObject);
					selecting = true;
				}
				else if (!selecting)
				{
					if (AtLine(2 - oblitDif) || AtLine(8 - oblitDif) || AtLine(16 - oblitDif))
					{
						dogamy.SetTalkable(null);
						dogaressa.SetTalkable(txt);
					}
					else if (AtLine(3 - oblitDif) || AtLine(18 - oblitDif))
					{
						dogaressa.SetTalkable(null);
					}
					else if (AtLine(4 - oblitDif))
					{
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(5 - oblitDif))
					{
						ChangeDirection(susie, Vector2.up);
						SetSprite(kris, "spr_kr_confused_to_side");
						SetSprite(noelle, "spr_no_confused_to_side");
					}
					else if (AtLine(6 - oblitDif) || AtLine(10 - oblitDif))
					{
						PlayAnimation(susie, "Embarrassed");
					}
					else if (AtLine(7 - oblitDif))
					{
						PlayAnimation(susie, "idle");
						kris.EnableAnimator();
						noelle.EnableAnimator();
						dogamy.SetTalkable(txt);
					}
					else if (AtLine(9 - oblitDif))
					{
						dogaressa.SetTalkable(null);
						ChangeDirection(susie, Vector2.right);
					}
					else if (AtLine(11 - oblitDif))
					{
						PlayAnimation(susie, "idle");
					}
					else if (AtLine(12 - oblitDif))
					{
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(14 - oblitDif))
					{
						dogamy.SetTalkable(txt);
					}
				}
			}
			else
			{
				if (selecting)
				{
					return;
				}
				if (frames == 0)
				{
					dogamy.GetComponent<Animator>().enabled = true;
					dogaressa.GetComponent<Animator>().enabled = true;
					PlayAnimation(dogamy, "Walk");
					PlayAnimation(dogaressa, "Walk");
					frames++;
				}
				if (dogamy.transform.parent.position.y != 2.02f)
				{
					MoveTo(dogamy.transform.parent, new Vector3(8.43f, 2.02f), 6f);
					return;
				}
				if (MoveTo(dogamy.transform.parent, new Vector3(-1.04f, 2.02f), 6f))
				{
					dogamy.GetComponent<SpriteRenderer>().flipX = true;
					return;
				}
				frames++;
				if (frames == 30)
				{
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(noelle, Vector2.left);
					StartText(new string[7]
					{
						(oblitDistance > 0) ? "* Thank goodness.^05\n* I thought you were\n  gonna kill them,^05 Kris." : "* Humans...^10 dangerous?",
						(oblitDistance > 0) ? "* But I wonder why\n  they think humans\n  are all dangerous." : "* That's a little\n  presumptuous,^05 don't you\n  think?",
						"* And how come they didn't\n  recognize Kris as a\n  human?",
						"* Kris,^05 remember what your\n  mom said?",
						"* Or...^05 what your mom\n  from the purple cave\n  thing said?",
						"* They wanted to like...^05\n  take your soul or\n  whatever.",
						"* I mean,^05 I dunno,^05 what\n  do you think is up?"
					}, new string[7] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[7]
					{
						(oblitDistance > 0) ? "no_weird" : "no_curious",
						"no_thinking",
						"no_confused",
						"su_neutral",
						"su_smirk_sweat",
						"su_smile_sweat",
						"su_inquisitive"
					});
					txt.EnableSelectionAtEnd();
					state = 2;
				}
			}
		}
		else if (state == 2)
		{
			if ((bool)txt)
			{
				if (AtLine(4) || AtLine(6))
				{
					ChangeDirection(susie, Vector2.left);
				}
				else if (AtLine(5))
				{
					ChangeDirection(susie, Vector2.up);
				}
				else if (AtLine(7))
				{
					PlayAnimation(susie, "Embarrassed");
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "Just as\nconfused as\nyou", Vector3.zero);
					select.SetupChoice(Vector2.right, "Something\nhappened to\nthem", new Vector3(-64f, 0f));
					select.SetupChoice(Vector2.up, "I agree\nwith them", new Vector3(-14f, 0f));
					select.SetupChoice(Vector2.down, "...", Vector3.zero);
					select.SetCenterOffset(new Vector2(0f, -22f));
					select.Activate(this, 0, txt.gameObject);
					selecting = true;
				}
			}
		}
		else if (state == 3)
		{
			if ((bool)txt)
			{
				if (selIndex == 0)
				{
					if (AtLine(3))
					{
						susie.UseHappySprites();
						noelle.UseHappySprites();
					}
				}
				else if (selIndex == 1)
				{
					if (AtLine(3))
					{
						ChangeDirection(susie, Vector2.right);
					}
					else if (AtLine(8))
					{
						SetSprite(susie, "spr_su_wtf");
					}
					else if (AtLine(9))
					{
						SetSprite(susie, "spr_su_throw_ready", true);
					}
				}
				else if (selIndex == 2)
				{
					if (AtLine(2))
					{
						ChangeDirection(susie, Vector2.right);
						susie.GetComponent<SpriteRenderer>().flipX = false;
						susie.EnableAnimator();
					}
					else if (AtLine(8))
					{
						ChangeDirection(susie, Vector2.left);
					}
				}
				return;
			}
			if (frames == 0)
			{
				susie.GetComponent<SpriteRenderer>().flipX = false;
				susie.EnableAnimator();
				ChangeDirection(susie, Vector2.left);
				frames++;
			}
			if (!MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				gm.PlayMusic("zoneMusic");
				bool flag = false;
				if (oblit && oblitDistance == 0)
				{
					flag = true;
					new GameObject("LOL").AddComponent<TextBox>().CreateBox(new string[3] { "* ...", "* Oh shoot,^05 we didn't\n  fight them!", "* Looks like things\n  should de-escalate." }, new string[1] { "snd_txtsus" }, new int[1], true, new string[3] { "su_side", "su_surprised", "su_smile" });
					WeirdChecker.Abort(gm);
				}
				gm.SetFlag(214, 1);
				EndCutscene(!flag);
			}
		}
		else if (state == 4)
		{
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
				else if (AtLine(3))
				{
					noelle.EnableAnimator();
				}
				else if (AtLine(5))
				{
					ChangeDirection(susie, Vector2.up);
					PlayAnimation(susie, "idle");
				}
				else if (AtLine(6))
				{
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(noelle, Vector2.left);
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "Yeah sorry", Vector3.zero);
					select.SetupChoice(Vector2.right, "I'm going to\nkill you", new Vector3(-64f, 0f));
					select.Activate(this, 2, txt.gameObject);
					selecting = true;
				}
			}
		}
		else if (state == 5)
		{
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
					noelle.EnableAnimator();
					ChangeDirection(susie, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
					dogamy.SetTalkable(txt);
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "I was joking,\nsorry", Vector3.zero);
					select.SetupChoice(Vector2.right, "Proceed", new Vector3(-40f, 0f));
					select.Activate(this, 3, txt.gameObject);
					selecting = true;
				}
			}
		}
		else
		{
			if (state != 6)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(3))
				{
					susie.EnableAnimator();
					ChangeDirection(susie, Vector2.up);
					ChangeDirection(noelle, Vector2.up);
					dogaressa.SetTalkable(txt);
				}
			}
			else
			{
				kris.InitiateBattle(66);
				gm.SetFlag(214, 1);
				EndCutscene(false);
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		switch (id)
		{
		case 0:
		{
			Object.Destroy(dogamy.transform.parent.gameObject);
			List<string> list4 = new List<string>();
			List<string> list5 = new List<string>();
			List<string> list6 = new List<string>();
			PlayAnimation(susie, "idle");
			if (index == Vector2.down)
			{
				selIndex = 0;
				list4.AddRange(new string[4] { "* ...", "* Well,^05 if you don't\n  wanna say,^05 then you\n  don't have to.", "* I'm just glad we didn't\n  have to waste time\n  fighting them.", "* Let's go." });
				list5.AddRange(new string[1] { "snd_txtsus" });
				list6.AddRange(new string[4] { "su_side", "su_smirk", "su_smile_side", "su_smile" });
			}
			else if (index == Vector2.up)
			{
				selIndex = 2;
				SetSprite(susie, "spr_su_wtf", true);
				list4.AddRange(new string[8] { "* The hell does that\n  mean,^05 Kris???", "* I heard that humans\n  have a long history\n  of prejudice.", "* Deeply rooted conflicts\n  between themselves over\n  their differences.", "* I can't imagine what\n  that's like,^05 let alone\n  how they'd treat us.", "* I imagine Kris knows\n  more about this than\n  the rest of us.", "* We don't need to\n  be getting deep about\n  this stuff right now.", "* Save that for when\n  we get home,^05 in school.", "* Let's just get going." });
				list5.AddRange(new string[8] { "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" });
				list6.AddRange(new string[8] { "su_angry", "no_curious", "no_thinking", "no_curious", "no_weird", "su_annoyed", "su_side", "su_annoyed" });
			}
			else
			{
				selIndex = 1;
				if (index == Vector2.left)
				{
					list4.AddRange(new string[2] { "* Eh...^05 you don't look\n  like it.", "* What,^05 d'you think\n  they like...^05 went to\n  war or something?" });
				}
				else
				{
					list4.AddRange(new string[2] { "* So like,^05 what do\n  you think?", "* That they went to\n  war or something?" });
				}
				list5.AddRange(new string[2] { "snd_txtsus", "snd_txtsus" });
				list6.AddRange(new string[2] { "su_annoyed", "su_smirk_sweat" });
				list4.AddRange(new string[7] { "* I wonder if in this\n  underground world...", "* Humans and monsters were\n  much more violent\n  towards one another.", "* Which is...^05 a little\n  worrying given what we\n  know about human power.", "* Wait,^05 what?", "* Just some boring history\n  stuff,^05 faha...", "* Huh?^05\n* Then let's stop wasting\n  time!!!", "* Let's get going!" });
				list5.AddRange(new string[7] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" });
				list6.AddRange(new string[7] { "no_curious", "no_curious", "no_weird", "su_side_sweat", "no_happy", "su_angry", "su_pissed" });
			}
			gm.SetFlag(233, selIndex);
			StartText(list4.ToArray(), list5.ToArray(), new int[1], list6.ToArray());
			state = 3;
			frames = 0;
			selecting = false;
			break;
		}
		case 1:
		{
			List<string> list7 = new List<string>();
			List<string> list8 = new List<string>();
			List<int> list9 = new List<int>();
			List<string> list10 = new List<string>();
			if (index == Vector2.left)
			{
				list7.AddRange(noelleDiag);
				list8.AddRange(noelleSounds);
				list9.AddRange(noelleSpeed);
				list10.AddRange(noellePortrats);
				list7.AddRange(dogamyDiag);
				list8.AddRange(dogamySounds);
				list9.AddRange(dogamySpeed);
				list10.AddRange(dogamyPortrats);
				oblitDif = 10;
			}
			else
			{
				state = 4;
				SetSprite(susie, "spr_su_surprise_right", true);
				SetSprite(noelle, "spr_no_surprise_left");
				list7.AddRange(new string[6] { "* K-^05KRIS???", "* Oh,^05 uhh...", "* My buddy here is a\n  bit loopy after,^05 uhh...", "* Fending off the\n  Snowdrakes in the\n  forest down there...", "* Which is uhhh,^05\n  why we smell weird.", "* Right,^05 Kris...?" });
				list8.AddRange(new string[2] { "snd_txtnoe", "snd_txtsus" });
				list9.Add(0);
				list10.AddRange(new string[6] { "no_scared", "su_wideeye", "su_side_sweat", "su_smile_sweat", "su_inquisitive", "su_worriedsmile" });
			}
			StartText(list7.ToArray(), list8.ToArray(), list9.ToArray(), list10.ToArray());
			if (state == 4)
			{
				txt.EnableSelectionAtEnd();
			}
			selecting = false;
			break;
		}
		case 2:
		{
			List<string> list11 = new List<string>();
			List<string> list12 = new List<string>();
			List<int> list13 = new List<int>();
			List<string> list14 = new List<string>();
			if (index == Vector2.left)
			{
				state = 1;
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
				list11.AddRange(dogamyDiag);
				list12.AddRange(dogamySounds);
				list13.AddRange(dogamySpeed);
				list14.AddRange(dogamyPortrats);
				oblitDif = 13;
				WeirdChecker.Abort(gm);
				oblitDistance = 1;
			}
			else
			{
				state = 5;
				SetSprite(susie, "spr_su_wtf", true);
				SetSprite(noelle, "spr_no_surprise_left");
				list11.AddRange(new string[3] { "* KRIS,^05 SHUT UP!!!", "* We are NOT gonna\n  help you if you're\n  doing this.", "* T-^05they're not joking...???" });
				list12.AddRange(new string[3] { "snd_txtsus", "snd_txtsus", "snd_text" });
				list13.Add(0);
				list14.AddRange(new string[3] { "su_wtf", "su_pissed", "" });
			}
			StartText(list11.ToArray(), list12.ToArray(), list13.ToArray(), list14.ToArray());
			if (state == 5)
			{
				txt.EnableSelectionAtEnd();
			}
			selecting = false;
			break;
		}
		case 3:
		{
			List<string> list = new List<string>();
			List<string> list2 = new List<string>();
			List<string> list3 = new List<string>();
			if (index == Vector2.left)
			{
				state = 1;
				list.AddRange(new string[4] { "* Okay,^05 thank god.", "* For a second,^05 we almost thought\n  you WERE a human.", "* (Be careful with that dark\n  humor.)^05\n* (It could get you killed.)", "* (Now you three get back to\n  town.)" });
				list2.Add("snd_text");
				list3.Add("");
				oblitDif = 13;
				WeirdChecker.Abort(gm);
				oblitDistance = 1;
			}
			else
			{
				state = 6;
				SetSprite(susie, "spr_su_shrug");
				ChangeDirection(noelle, Vector2.left);
				gm.SetCheckpoint(91, new Vector3(-2.8f, 2.57f));
				gm.SetPartyMembers(false, false);
				list.AddRange(new string[4] { "* Alright,^05 well we talked\n  about this.", "* We'll just step away.", "* (So you WERE a human this\n  whole time!!!)", "* (It's time to eliminate you!)" });
				list2.AddRange(new string[3] { "snd_txtsus", "snd_txtsus", "snd_text" });
				list3.AddRange(new string[3] { "su_smile_sweat", "su_smirk", "" });
			}
			StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
			selecting = false;
			break;
		}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		SetSprite(kris, "spr_kr_surprise");
		SetSprite(susie, "spr_su_surprise_right");
		SetSprite(noelle, "spr_no_surprise");
		dogamy = GameObject.Find("Dogamy").GetComponent<InteractTextBox>();
		dogaressa = GameObject.Find("Dogaressa").GetComponent<InteractTextBox>();
		PlayAnimation(dogamy, "Walk", 1.5f);
		PlayAnimation(dogaressa, "Walk", 1.5f);
		dogamy.transform.parent.position = new Vector3(17.13f, 0.27f);
		gm.StopMusic(30f);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		oblit = (int)Util.GameManager().GetFlag(13) >= 9;
		StartText(new string[2] { "* Stop right there!!!", "* (Step to the side,^05 all of you!)" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
	}
}
