using UnityEngine;

public class FloweyDefeatCutscene : CutsceneBase
{
	private int endState;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (endState == 2 && frames == 30)
			{
				if ((int)gm.GetFlag(13) == 3)
				{
					StartText(new string[7] { "* I guess...^10 that settles\n  that for now.", "* ...", "* Kris...?", "* You look...^10 relieved.", "* ...", "* Well,^05 you don't have\n  to talk about that\n  right now.", "* Let's get outta here." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[7] { "su_side", "su_neutral", "su_side", "su_side", "su_side", "su_smirk", "su_smile" });
				}
				else
				{
					StartText(new string[7] { "* I guess...^10 that settles\n  that for now.", "* ...", "* I'm kinda wondering\n  what that flower's\n  deal was.", "* \"Finish me off\"...", "* That's kinda extreme,^05\n  don't you think,^05 Kris?", "* ...", "* Well,^05 we might as\n  well get outta here." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[7] { "su_side", "su_side", "su_smile_side", "su_dejected", "su_dejected", "su_side", "su_smile" });
				}
				state = 1;
			}
			else if (endState == 1 && frames == 30)
			{
				StartText(new string[4] { "* Yeesh.", "* That's...^10 really damn\n  brutal.", "* ...", "* Let's get outta\n  here,^05 Kris." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_side_sweat", "su_side_sweat", "su_dejected", "su_neutral" });
				gm.SetFlag(1, "side_sweat");
				state = 1;
			}
		}
		if (state == 1 && !txt)
		{
			cam.SetFollowPlayer(follow: true);
			kris.ChangeDirection(Vector2.down);
			if (endState == 2 && (int)gm.GetFlag(13) == 3)
			{
				WeirdChecker.Abort(gm);
			}
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		Object.Destroy(GameObject.Find("BigFlower"));
		if (endState == 1)
		{
			GameObject.Find("DeadFlowey").transform.position = new Vector3(0f, 0.172f);
		}
		kris.GetComponent<Animator>().Play("idle");
		susie.GetComponent<Animator>().Play("idle");
		kris.SetSelfAnimControl(setAnimControl: true);
		susie.SetSelfAnimControl(setAnimControl: true);
		kris.ChangeDirection(Vector2.up);
		susie.ChangeDirection(Vector2.up);
	}
}
