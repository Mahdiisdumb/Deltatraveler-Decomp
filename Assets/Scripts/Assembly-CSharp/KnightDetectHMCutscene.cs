using UnityEngine;

public class KnightDetectHMCutscene : CutsceneBase
{
	private void Update()
	{
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				GameObject.Find("Goner").GetComponent<Animator>().enabled = true;
			}
			if (frames == 42)
			{
				PlaySFX("sounds/snd_weaponpull");
			}
			if (frames == 70)
			{
				Object.Destroy(GameObject.Find("Goner"));
				kris.InitiateBattle(41);
				EndCutscene(false);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.DisablePlayerMovement(true);
		gm.SetFlag(123, 1);
		GameObject.Find("Goner").transform.parent = GameObject.Find("NPC").transform;
		Object.Destroy(Object.FindObjectOfType<CutsceneStart>().gameObject);
		StartText(new string[2] { "/WDI FEEL THE PRESENCE\nOF THE INTERLOPER", "/WDYOU WILL FEEL MY\nBLADE, FIEND" }, new string[2] { "snd_txtwdc", "snd_txtwdc" }, new int[2], new string[2] { "", "" });
	}
}
