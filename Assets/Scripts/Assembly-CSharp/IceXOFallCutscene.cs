using UnityEngine;

public class IceXOFallCutscene : CutsceneBase
{
	private bool moveKris = true;

	private bool moveSusie = true;

	private bool moveNoelle = true;

	private bool playFullCutscene;

	private Transform bunny;

	private bool bunnyRun;

	private int bunnyFrames;

	private void Update()
	{
		if (isPlaying)
		{
			if (state == 0)
			{
				frames++;
				bool flag = true;
				if (frames > 0 && moveKris)
				{
					if (MoveTo(kris, new Vector3(-1.707f, -1.3295f), 10f))
					{
						Vector2[] array = new Vector2[4]
						{
							Vector2.down,
							Vector2.right,
							Vector2.up,
							Vector2.left
						};
						ChangeDirection(kris, array[frames / 4 % 4]);
						flag = false;
					}
					else
					{
						moveKris = false;
						ChangeDirection(kris, Vector2.down);
						if (!playFullCutscene)
						{
							kris.SetCollision(onoff: true);
							kris.SetSelfAnimControl(setAnimControl: true);
							kris.SetMovement(newMove: true, activateMembers: false);
						}
					}
				}
				if (frames > 10 && moveSusie)
				{
					if (MoveTo(susie, new Vector3(-0.432f, -1.1645f), 10f))
					{
						susie.GetComponent<SpriteRenderer>().flipX = frames / 4 % 2 == 0;
						flag = false;
					}
					else
					{
						moveSusie = false;
						ChangeDirection(susie, Vector2.down);
						susie.EnableAnimator();
						susie.GetComponent<SpriteRenderer>().flipX = false;
						if (!playFullCutscene)
						{
							susie.SetSelfAnimControl(setAnimControl: true);
							susie.ResetPathLists();
							susie.Activate();
						}
						else
						{
							susie.UseUnhappySprites();
						}
					}
				}
				if (frames > 20 && moveNoelle)
				{
					if (MoveTo(noelle, new Vector3(0.82f, -1.142f), 10f))
					{
						flag = false;
					}
					else
					{
						moveNoelle = false;
						ChangeDirection(noelle, Vector2.down);
						noelle.EnableAnimator();
						if (!playFullCutscene)
						{
							noelle.SetSelfAnimControl(setAnimControl: true);
							noelle.ResetPathLists();
							noelle.Activate();
						}
						else
						{
							noelle.UseUnhappySprites();
						}
					}
				}
				if (flag && frames > 30)
				{
					if (playFullCutscene && frames >= 75)
					{
						StartText(new string[5] { "* So like...", "* Why the hell are\n  we following you while\n  you do this?", "* I mean,^05 I figured\n  we should so that\n  WE don't slip off...", "* ... When Kris finishes\n  the puzzle.", "* Okay,^05 but they didn't." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[5] { "su_side", "su_annoyed", "no_thinking", "no_curious", "su_inquisitive" });
						state = 1;
						ChangeDirection(kris, Vector2.right);
						ChangeDirection(susie, Vector2.up);
						ChangeDirection(noelle, Vector2.left);
					}
					else if (!playFullCutscene)
					{
						EndCutscene(enablePlayerMovement: false);
					}
				}
			}
			else if (state == 1)
			{
				if ((bool)txt)
				{
					if (AtLine(2))
					{
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(3))
					{
						ChangeDirection(susie, Vector2.right);
					}
				}
				else
				{
					frames = 0;
					state = (((Random.Range(0, 5) == 0 || Util.GameManager().GetPlayerName() == "SHAYY") && (int)Util.GameManager().GetFlag(12) == 0) ? 2 : 3);
				}
			}
			else if (state == 2)
			{
				frames++;
				SetSprite(bunny, "overworld/npcs/spr_bunny_walk_" + frames / 10 % 2, flipX: true);
				if (!MoveTo(bunny, new Vector3(-5f, 3.81f), 1f) && frames >= 60)
				{
					SetSprite(bunny, "overworld/npcs/spr_bunny", flipX: true);
					StartText(new string[5] { "* its cuz i didnt feel like\n  making them stay in a single\n  room,^05 cuz that's hard lmao", "* WHAT?????", "* Who the heck is\n  that???", "* I dunno.^05\n* Seems pretty annoying\n  though.", "* Anyway..." }, new string[5] { "snd_text", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[1], new string[5] { "", "su_angry", "no_confused", "su_annoyed", "no_thinking" }, 1);
					state = 3;
				}
			}
			else if (state == 3)
			{
				if ((bool)txt)
				{
					if (AtLine(2))
					{
						bunnyRun = true;
						SetSprite(susie, "spr_su_point_up_0");
						ChangeDirection(kris, Vector2.up);
						ChangeDirection(noelle, Vector2.up);
					}
					else if (AtLine(3))
					{
						ChangeDirection(noelle, Vector2.left);
					}
					else if (AtLine(4))
					{
						susie.EnableAnimator();
						ChangeDirection(kris, Vector2.right);
					}
					else if (AtLine(5))
					{
						ChangeDirection(noelle, Vector2.up);
					}
				}
				else
				{
					StartText(new string[6]
					{
						bunnyRun ? "* It seems like it'd\n  be easier to just\n  follow Kris." : "* Well,^05 it seems like it'd\n  be easier to just\n  follow Kris.",
						"* ...^05 Fine.",
						"* Kris,^05 prolly try\n  planning where to slide\n  next time.",
						"* ...^05 Also,^05 should we\n  mention...?",
						"* Nah,^05 this seems par\n  for the course.",
						"* Let's just climb back\n  up."
					}, new string[5] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[6] { "no_curious", "su_annoyed", "su_neutral", "no_thinking", "su_smirk_sweat", "su_annoyed" }, bunnyRun ? 1 : (-1));
					ChangeDirection(noelle, Vector2.left);
					state = 4;
				}
			}
			else if (state == 4)
			{
				if ((bool)txt)
				{
					if (AtLine(2))
					{
						ChangeDirection(susie, Vector2.up);
					}
					else if (AtLine(3) || AtLine(6))
					{
						ChangeDirection(susie, Vector2.left);
					}
					else if (AtLine(4))
					{
						ChangeDirection(noelle, Vector2.up);
					}
					else if (AtLine(5))
					{
						ChangeDirection(susie, Vector2.right);
					}
				}
				else
				{
					ChangeDirection(kris, Vector2.down);
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(noelle, Vector2.left);
					if (!WeirdChecker.HasCommittedBloodshed(gm))
					{
						noelle.UseHappySprites();
					}
					RestorePlayerControl();
					EndCutscene();
				}
			}
		}
		if (bunnyRun)
		{
			bunnyFrames++;
			if (MoveTo(bunny, new Vector3(-5f, 6f), 12f))
			{
				SetSprite(bunny, "overworld/npcs/spr_bunny_walk_" + bunnyFrames % 2, flipX: true);
			}
		}
	}

	public void OnDestroy()
	{
		if ((bool)Util.GameManager())
		{
			Util.GameManager().SetSessionFlag(10, 0);
			Util.GameManager().UnlockMenu();
		}
		if (bunnyRun && (bool)bunny)
		{
			Object.Destroy(bunny.gameObject);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)Util.GameManager().GetSessionFlag(10) == 0)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.StartCutscene(par);
		RevokePlayerControl();
		kris.transform.position = new Vector3(-1.707f, 6f);
		susie.transform.position = new Vector3(-0.432f, 6f);
		noelle.transform.position = new Vector3(0.82f, 6f);
		bunny = GameObject.Find("Bunny").transform;
		SetSprite(susie, "spr_su_freaked");
		SetSprite(noelle, "spr_no_surprise");
		if ((int)Util.GameManager().GetFlag(262) == 0 && (int)Util.GameManager().GetFlag(265) == 0)
		{
			playFullCutscene = true;
		}
		Util.GameManager().SetFlag(265, 1);
	}
}
