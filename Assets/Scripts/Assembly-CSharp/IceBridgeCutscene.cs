using UnityEngine;

public class IceBridgeCutscene : CutsceneBase
{
	private int count;

	private Transform mask;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				state = 1;
				frames = 0;
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if (frames % 15 == 0)
			{
				count++;
				if (count == 6)
				{
					gm.ResumeMusic();
					EndCutscene();
				}
				else
				{
					mask.localScale = new Vector3(80f, count * 16, 1f);
					PlaySFX("sounds/snd_floweypellet");
				}
			}
			frames++;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.PauseMusic();
		gm.DisablePlayerMovement(false);
		mask = GameObject.Find("IceBridge").transform.GetChild(0);
	}
}
