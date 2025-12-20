using UnityEngine;

public class TripleCultistDefeat : CutsceneBase
{
	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(87) >= 5)
		{
			int num = (int)par[0];
			if ((int)gm.GetFlag(13) >= 5 && num == 1)
			{
				gm.PlayGlobalSFX("sounds/snd_ominous");
				Object.FindObjectOfType<CultistWallGenerator>().CreateDeadEnemies(false);
			}
			else
			{
				Object.FindObjectOfType<CultistWallGenerator>().EliminateEverything();
				if ((int)gm.GetFlag(12) == 1)
				{
					WeirdChecker.Abort(gm);
				}
			}
		}
		else if ((int)gm.GetFlag(12) == 1)
		{
			WeirdChecker.Abort(gm);
		}
		Util.GameManager().EnablePlayerMovement();
		Object.Destroy(base.gameObject);
	}
}
