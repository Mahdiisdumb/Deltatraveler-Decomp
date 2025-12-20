using UnityEngine;

public class KrisDefeatCutscene : CutsceneBase
{
	private int soulState;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (soulState == 1)
			{
				StartText(new string[3] { "* Damn...^05 you killed me.", "* ...", "* Well,^10 good testing with you!" }, new string[1] { "snd_txtkrs" }, new int[3], new string[3]);
				state = 1;
			}
			else
			{
				StartText(new string[2] { "* You spared me!", "* Did you make the good choice?\n\n         Yes         No" }, new string[1] { "snd_txtkrs" }, new int[2], new string[2]);
				state = 3;
			}
		}
		if (state == 1 && txt == null)
		{
			if (soulState == 1)
			{
				EndCutscene();
			}
			else
			{
				EndCutscene();
			}
		}
		if (state == 2 && txt == null)
		{
			EndCutscene();
		}
		if (state == 3 && txt != null && txt.AtLastText() && !txt.IsPlaying() && sels == null)
		{
			StartSelection("Yes", "No", 4, -1);
		}
		if (state == 4)
		{
			StartText(new string[2] { "* I'm glad you think that!", "* Good testing with you!" }, new string[1] { "snd_txtkrs" }, new int[2], new string[2]);
			state = 1;
		}
		if (state == 5)
		{
			StartText(new string[3] { "* Oh...", "* ...", "* Well,^10 good testing with you!" }, new string[1] { "snd_txtkrs" }, new int[3], new string[3]);
			state = 1;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		soulState = int.Parse(par[0].ToString());
		if (soulState == 0)
		{
			Object.Destroy(GameObject.Find("NPC_kris"));
			EndCutscene();
		}
		else
		{
			base.StartCutscene(par);
		}
	}
}
