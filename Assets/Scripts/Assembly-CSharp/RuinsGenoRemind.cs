using UnityEngine;

public class RuinsGenoRemind : CutsceneBase
{
	private void Update()
	{
		if (isPlaying && state == 0 && !txt)
		{
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(13) == 1 || (int)gm.GetFlag(13) == 2)
		{
			gm.SetFlag(54, 1);
			base.StartCutscene(par);
			if ((int)gm.GetFlag(108) == 1)
			{
				StartText(new string[1] { $"<color=#FF0000FF>* Strongly felt {WeirdChecker.GetRemainingHardModeEnemies(gm)} left.\n* Shouldn't proceed yet.</color>" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[0]);
			}
			else
			{
				StartText(new string[2] { "* (You get the feeling that\n  continuing on would be better\n  for the people here.)", "* (After all,^05 you're already\n  pretty strong.)" }, new string[2] { "snd_text", "snd_text" }, new int[2], new string[2] { "", "" });
			}
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
