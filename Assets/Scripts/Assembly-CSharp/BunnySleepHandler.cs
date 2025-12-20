using UnityEngine;

public class BunnySleepHandler : MonoBehaviour
{
	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(210) == 0)
		{
			CutsceneHandler.GetCutscene(72).StartCutscene();
		}
	}
}
