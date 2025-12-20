using System.Collections.Generic;
using UnityEngine;

public class GreaterDogDefeatCutscene : CutsceneBase
{
	private Animator gdog;

	private Animator qc;

	private string oblitEnd = "";

	private int endState;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 30 && endState == 1)
			{
				frames = 0;
				state = 1;
			}
			else if (frames >= 30)
			{
				if (MoveTo(gdog, new Vector3(0.7f, -0.73f), 4f))
				{
					gdog.SetFloat("speed", 1f);
					LookAt(kris, gdog, new Vector3(0f, -0.75f));
					LookAt(susie, gdog, new Vector3(0f, -0.75f));
					LookAt(noelle, gdog, new Vector3(0f, -0.75f));
				}
				else
				{
					Object.Destroy(gdog.gameObject);
					frames = 0;
					state = 1;
				}
			}
		}
		else if (state == 1)
		{
			if (!MoveTo(qc, new Vector3(7.3f, -0.87f), 6f))
			{
				frames++;
				if (frames == 1)
				{
					LookAt(kris, qc);
					LookAt(susie, qc);
					LookAt(noelle, qc);
					SetMoveAnim(qc, isMoving: false);
				}
				if (frames == 5)
				{
					List<string> list = new List<string>();
					List<string> list2 = new List<string>();
					List<string> list3 = new List<string>();
					if (endState == 2)
					{
						list.AddRange(new string[2] { "* Goodness me,^05 only y'all could\n  do something like that.", "* Maybe the dogs'll stop arguing\n  now that he isn't \"cold\n  and stern\" anymore." });
						list2.AddRange(new string[2] { "snd_text", "snd_text" });
						list3.AddRange(new string[2] { "", "" });
					}
					else if ((int)Util.GameManager().GetFlag(280) == 1)
					{
						list.AddRange(new string[6] { "* Did y'all really need to,^05\n  uhh...^05 y'know...", "* No,^05 they didn't.", "* ...", "* Well,^05 I suppose letting him\n  go could lead him to being\n  RETRAINED to kill.", "* So I guess he was doomed\n  either way.", "* ...^05 Sure." });
						list2.AddRange(new string[6] { "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_txtsus" });
						list3.AddRange(new string[6] { "", "su_disappointed", "", "", "", "su_annoyed" });
					}
					else if ((int)Util.GameManager().GetFlag(279) <= 6)
					{
						list.AddRange(new string[3] { "* Yeesh,^05 y'all hit REALLY hard.^05\n* He barely stood a chance!", "* I wouldn't wanna be on\n  your bad side,^05 heh heh.", "* (...)" });
						list2.AddRange(new string[3] { "snd_text", "snd_text", "snd_txtnoe" });
						list3.AddRange(new string[3] { "", "", "no_afraid" });
					}
					else
					{
						list.AddRange(new string[3] { "* Guess there wasn't any\n  other way around him...", "* Prolly could've waited a few\n  hours,^05 but the guard has been\n  getting worse by the day.", "* Y'all stopped it before it\n  could get worse." });
						list2.AddRange(new string[3] { "snd_text", "snd_text", "snd_text" });
						list3.AddRange(new string[3] { "", "", "" });
					}
					list.AddRange(new string[3] { "* Anyway,^05 what's important is\n  that the way isn't blocked!", "* I REALLY need to get to\n  my shop.", "* See ya!" });
					list2.AddRange(new string[1] { "snd_text" });
					list3.AddRange(new string[1] { "" });
					StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
					state = 2;
					frames = 0;
				}
			}
			else if (!qc.enabled)
			{
				qc.enabled = true;
				SetMoveAnim(qc, isMoving: true);
				ChangeDirection(qc, Vector2.right);
				qc.GetComponent<SpriteRenderer>().sortingOrder = 5;
			}
		}
		else if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(qc, isMoving: true);
			}
			qc.transform.position += new Vector3(0.125f, 0f);
			LookAt(kris, qc);
			LookAt(susie, qc);
			LookAt(noelle, qc);
			if (qc.transform.position.x > 20f)
			{
				state = 3;
				frames = 0;
				if (Util.GameManager().GetFlagInt(87) < 7)
				{
					LookAt(kris, susie);
					LookAt(noelle, susie);
					StartText(new string[11]
					{
						"* We've gotta be getting\n  close to the end of\n  the forest by now.", "* All these dogs and\n  enemies are getting\n  boring.", "* And there's still\n  whatever the hell\n  Papyrus has left.", "* I'm sure it'll be\n  fine!", "* Kris has been doing\n  very well against\n  his puzzles.", "* You sure?", "* He seemed pretty\n  desperate last time.", "* Bet he's set to\n  kill us quickly.", "* I don't think so.", "* He's so proud of\n  himself that I doubt\n  he'd sink that low.",
						"* We'll see..."
					}, new string[11]
					{
						"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe",
						"snd_txtsus"
					}, new int[1], new string[11]
					{
						"su_side", "su_annoyed", "su_smirk_sweat", "no_happy", "no_tease_finger", "su_inquisitive", "su_side_sweat", "su_smirk_sweat", "no_confused", "no_confused_side",
						"su_smile_sweat"
					});
				}
			}
		}
		else
		{
			if (state != 3)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					LookAt(susie, noelle);
				}
				if (AtLine(11))
				{
					ChangeDirection(susie, Vector2.right);
				}
			}
			else
			{
				ChangeDirection(kris, Vector2.down);
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(-5.58f, 0.73f), Quaternion.identity);
				EndOfObliterationHandle();
				state = 4;
			}
		}
	}

	private void EndOfObliterationHandle()
	{
		RestorePlayerControl();
		WeirdChecker.RoomModifications(gm);
		if (oblitEnd != "")
		{
			new GameObject("LesserDogDefeatCutsceneOblitEnd").AddComponent<TextBox>().CreateBox(new string[2] { "* (...)", oblitEnd }, giveBackControl: true);
			gm.PlayMusic("zoneMusic");
			EndCutscene(enablePlayerMovement: false);
		}
		else
		{
			gm.PlayMusic("zoneMusic");
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gdog = GameObject.Find("GreaterDog").GetComponent<Animator>();
		gdog.GetComponent<BoxCollider2D>().enabled = false;
		qc = GameObject.Find("QC").GetComponent<Animator>();
		gm.StopMusic();
		susie.EnableAnimator();
		ChangeDirection(kris, Vector2.right);
		ChangeDirection(susie, Vector2.right);
		ChangeDirection(noelle, Vector2.right);
		endState = int.Parse(par[0].ToString());
		if (endState == 1)
		{
			Object.Destroy(gdog.gameObject);
			if ((int)gm.GetFlag(12) == 1)
			{
				gm.PlayGlobalSFX("sounds/snd_ominous");
				oblitEnd = EndBattleHandler.GetSnowdinSecondHalfString();
			}
			return;
		}
		gdog.enabled = true;
		if ((int)gm.GetFlag(12) == 1)
		{
			WeirdChecker.Abort(gm);
		}
		gm.SetFlag(1, "side_sweat");
		gm.SetFlag(2, "surprised_happy");
	}
}
