using System.Collections.Generic;
using UnityEngine;

public class NapstablookCutscene : CutsceneBase
{
	private bool geno;

	private bool formerGeno;

	private bool selecting;

	private bool hardmode;

	private void Update()
	{
		if (state == 0)
		{
			if ((bool)txt)
			{
				if (geno && txt.CanLoadSelection() && !selecting)
				{
					selecting = true;
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "Yes", Vector3.zero);
					select.SetupChoice(Vector2.right, "No", new Vector3(38f, 0f));
					select.Activate(this, 0, txt.gameObject);
				}
			}
			else if (!geno)
			{
				state = 1;
			}
		}
		if (state == 1 && geno && !txt)
		{
			kris.InitiateBattle(8);
			EndCutscene(enablePlayerMovement: false);
		}
		if (state == 1 && !geno)
		{
			frames++;
			GameObject.Find("Napstablook").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)frames / 20f);
			GameObject.Find("Napstablook").GetComponent<BoxCollider2D>().enabled = false;
			if (frames == 30)
			{
				if (formerGeno)
				{
					List<string> list = new List<string> { "* Huh.", "* Sweet." };
					if (hardmode)
					{
						list.Add("* (Seems that somebody didn't get\n  the memo...)");
					}
					StartText(list.ToArray(), new string[3] { "snd_txtsus", "snd_txtsus", "snd_text" }, new int[15], new string[3] { "su_side", "su_smile", "" });
				}
				else
				{
					List<string> list2 = new List<string> { "* Uhh...^05 cool." };
					if (hardmode)
					{
						list2.Add("* (Seems that somebody didn't get\n  the memo...)");
					}
					StartText(list2.ToArray(), new string[2] { "snd_txtsus", "snd_text" }, new int[15], new string[2] { "su_smile_sweat", "" });
				}
				state = 2;
			}
		}
		if (state == 2 && !txt)
		{
			gm.SetCheckpoint(22);
			EndCutscene();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			StartText(new string[2]
			{
				hardmode ? "* Whoa, whoa, wait a\n  sec!" : "* Uhh Kris...?",
				"* Um,^05 why don't you\n  let me talk to them."
			}, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[15], new string[2] { "su_concerned", "su_side_sweat" });
			state = 1;
		}
		else if (index == Vector2.right)
		{
			WeirdChecker.Abort(gm);
			geno = false;
			formerGeno = true;
			StartText(new string[4] { "* oh, sorry", "* i thought i heard that\n  there was a human beating\n  up people", "* but it must have been\n  my imagination", "* i'll get out of your way" }, new string[8] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text" }, new int[15], new string[8] { "", "", "", "", "", "", "", "" });
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		geno = (int)gm.GetFlag(13) >= 2;
		if (geno)
		{
			gm.SetCheckpoint(22);
		}
		gm.SetFlag(30, 1);
		hardmode = (int)gm.GetFlag(108) == 1;
		if (geno)
		{
			StartText(new string[4] { "* zzzzzzzzzzzzzzz...\n^10* zzzzzzzzzzzzzz...", "* Uhh,^05 hello?", "* oh... it's you", "* are you here to beat me up?" }, new string[8] { "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_text" }, new int[15], new string[8] { "", "su_side", "", "", "", "", "", "" });
			txt.EnableSelectionAtEnd();
			return;
		}
		if ((int)gm.GetFlag(12) == 1)
		{
			gm.SetFlag(12, 0);
			if ((int)gm.GetFlag(13) == 0)
			{
				gm.SetFlag(95, 1);
			}
			else
			{
				WeirdChecker.Abort(gm);
			}
		}
		StartText(new string[8] { "* zzzzzzzzzzzzzzz...\n^10* zzzzzzzzzzzzzz...", "* Uhh,^05 hello?", "* oh...\n^10* was i not convincing?", "* Aren't you a cop?", "* i would never", "* oh,^05 am i in the way?", "* Yeah.", "* sorry,^05 i'll get out of\n  your way." }, new string[8] { "snd_text", "snd_txtsus", "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_txtsus", "snd_text" }, new int[15], new string[8] { "", "su_side", "", "su_side_sweat", "", "", "su_annoyed", "" });
	}
}
