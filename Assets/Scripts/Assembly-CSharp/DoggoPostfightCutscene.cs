using UnityEngine;

public class DoggoPostfightCutscene : CutsceneBase
{
	private int endState;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (endState == 1 && !txt)
		{
			gm.PlayMusic("zoneMusic");
			kris.ChangeDirection(Vector2.down);
			EndCutscene();
		}
		else if (endState == 2 && !txt)
		{
			frames++;
			GameObject.Find("Doggo").transform.position = new Vector3(2.938f, Mathf.Lerp(-0.878f, -1.794f, (float)frames / 40f));
			if (frames == 60)
			{
				gm.PlayMusic("zoneMusic");
				kris.ChangeDirection(Vector2.down);
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		if (endState == 1)
		{
			Object.Destroy(GameObject.Find("Doggo"));
			if (!gm.SusieInParty() || (int)gm.GetSessionFlag(3) == 0 || ((int)gm.GetSessionFlag(3) == 1 && (int)gm.GetFlag(172) == 1))
			{
				if ((int)gm.GetFlag(12) == 1)
				{
					gm.PlayGlobalSFX("sounds/snd_ominous");
				}
				gm.PlayMusic("zoneMusic");
				EndCutscene();
			}
			else if ((int)gm.GetSessionFlag(5) == 1)
			{
				StartText(new string[2] { "* ...........", "* YOU TWO STOP LOOKING\n  AT ME LIKE THAT!!!" }, new string[2] { "snd_txtnoe", "snd_txtsus" }, new int[2] { 5, 0 }, new string[2] { "no_silent", "su_angry" });
			}
			else
			{
				StartText(new string[3]
				{
					"* Kris,^05 why did we\n  kill him after we\n  gained his trust...?",
					"* Yeah,^05 dude,^05 that's\n  really cruel.",
					((int)gm.GetFlag(87) > 2) ? "* I hope this isn't\n  you falling back into\n  slaughter again." : "* Are you okay?"
				}, new string[3] { "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[2], new string[3] { "no_shocked", "su_pissed", "su_annoyed" });
			}
		}
		else
		{
			if ((int)gm.GetFlag(12) == 1)
			{
				WeirdChecker.Abort(gm);
			}
			GameObject.Find("Doggo").GetComponent<Animator>().Play("doggo_look");
			StartText(new string[4]
			{
				"* I-^05I-^05I made friends with\n  p-people I don't know...!",
				"* And they barely even moved!!!",
				"* M-^05maybe I can make up with\n  the rest of the guard...",
				((int)gm.GetSessionFlag(4) == 1) ? "* I...^10 I need a nap..." : "* I...^10 I need dog treats..."
			}, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
		}
	}
}
