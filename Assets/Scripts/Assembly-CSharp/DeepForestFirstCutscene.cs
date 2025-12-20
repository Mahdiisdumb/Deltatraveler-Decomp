using System.Collections.Generic;
using UnityEngine;

public class DeepForestFirstCutscene : CutsceneBase
{
	private bool oblit;

	private bool selecting;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames >= 30)
			{
				if (frames == 30)
				{
					SetMoveAnim(susie, isMoving: true);
					SetMoveAnim(noelle, isMoving: true);
				}
				if (!MoveTo(susie, new Vector3(20.889f, 1.66f), 4f))
				{
					SetMoveAnim(susie, isMoving: false);
				}
				if (!MoveTo(noelle, new Vector3(19.138f, 1.612f), 4f))
				{
					SetMoveAnim(noelle, isMoving: false);
				}
			}
			if (frames == 90)
			{
				StartText(new string[4]
				{
					"* This is uhh...",
					"* Kinda eerie.",
					oblit ? "* I can only like...^05\n  barely see anything\n  in this forest." : "* You can barely see\n  into the darkness...",
					oblit ? "* Anyone else?" : "* Can you see anything,^05\n  Kris?"
				}, new string[4]
				{
					"snd_txtsus",
					"snd_txtsus",
					oblit ? "snd_txtsus" : "snd_txtnoe",
					oblit ? "snd_txtsus" : "snd_txtnoe"
				}, new int[1], new string[4]
				{
					"su_neutral",
					"su_side",
					oblit ? "su_smirk_sweat" : "no_silent",
					oblit ? "su_neutral" : "no_confused_side"
				});
				txt.EnableSelectionAtEnd();
				frames = 0;
				state = 1;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					ChangeDirection(susie, oblit ? Vector2.left : Vector2.up);
					ChangeDirection(noelle, oblit ? Vector2.right : Vector2.up);
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					selecting = true;
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "I can see\nfine", Vector3.zero);
					select.SetupChoice(Vector2.right, "I can't see\nanything", new Vector3(-46f, 0f));
					select.Activate(this, 0, txt.gameObject);
				}
			}
		}
		else if (state == 2 && !txt)
		{
			RestorePlayerControl();
			EndCutscene();
		}
		else if (state == 3 && !txt && !MoveTo(susie, new Vector3(20f, 1f), 6f))
		{
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(susie, isMoving: false);
			}
			if (frames == 5)
			{
				StartText(new string[9] { "* Kris,^05 we just found\n  this weird house with\n  a bunch of rabbits.", "* They seem a lot nicer\n  than anyone else we've\n  met here.", "* Not sure if I trust\n  them entirely...", "* Noelle decided to go\n  inside,^05 though.", "* (She also insisted on\n  walking in the dark\n  for some reason...)", "* Take like...^05 two rights\n  from here to get\n  there.", "* ...^05 The hell is with\n  that look?", "* Can you not see\n  down here?", "* Prolly take that torch\n  with us when we\n  go there." }, new string[1] { "snd_txtsus" }, new int[1], new string[9] { "su_sus", "su_smile_sweat", "su_neutral", "su_side", "su_inquisitive", "su_smirk", "su_annoyed", "su_smirk_sweat", "su_neutral" });
				state = 2;
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selecting = false;
		ChangeDirection(susie, Vector2.up);
		ChangeDirection(noelle, Vector2.up);
		List<string> list = new List<string>();
		List<string> list2 = new List<string>();
		List<string> list3 = new List<string>();
		if (index == Vector2.left)
		{
			if (oblit)
			{
				list.AddRange(new string[3] { "* Yeah,^05 bullshit.", "* You don't sound\n  confident about it at\n  all.", "* You should prolly grab\n  that torch." });
				list2.Add("snd_txtsus");
				list3.AddRange(new string[3] { "su_annoyed", "su_side", "su_neutral" });
			}
			else
			{
				list.AddRange(new string[3] { "* Are you sure?", "* I've read somewhere that\n  humans have worse sight\n  than most monsters.", "* Maybe it would be\n  a good idea to bring\n  that torch with us." });
				list2.AddRange(new string[3] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" });
				list3.AddRange(new string[3] { "no_confused", "no_thinking", "no_curious" });
			}
		}
		else
		{
			list.Add("* ... Should prolly grab\n  that torch then.");
			list2.Add("snd_txtsus");
			list3.Add("su_inquisitive");
		}
		list.AddRange(new string[2] { "* Hell,^05 I'd prolly say\n  to grab it even if\n  you CAN see.", "* It's REALLY dark." });
		list2.AddRange(new string[2] { "snd_txtsus", "snd_txtsus" });
		list3.AddRange(new string[2] { "su_annoyed", "su_side" });
		StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
		state = 2;
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		oblit = (int)gm.GetFlag(172) == 1;
		RevokePlayerControl();
		cam.SetFollowPlayer(follow: true);
		gm.SetFlag(203, 1);
		gm.SetFlag(1, "side_sweat");
		if (!Util.GameManager().SusieInParty())
		{
			LoadingZone[] array = Util.FindObjectsOfType<LoadingZone>();
			foreach (LoadingZone obj in array)
			{
				obj.SetForceActivationTrigger(forceActivationTrigger: true);
				obj.ModifyContents("* Kris,^05 let's head over\n  to that house.", "snd_txtsus", "su_annoyed");
			}
			Util.GameManager().SetPartyMembers(susie: true, noelle: false);
			StartText(new string[1] { "* KRIS!!!" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "" });
			GameObject.Find("LOL").transform.position = new Vector3(36.22f, -16.38f);
			susie.transform.position = new Vector3(20f, -2.54f);
			SetMoveAnim(susie, isMoving: true);
			ChangeDirection(susie, Vector2.up);
			state = 3;
		}
		else if (!oblit)
		{
			gm.SetFlag(2, "silent");
		}
	}
}
