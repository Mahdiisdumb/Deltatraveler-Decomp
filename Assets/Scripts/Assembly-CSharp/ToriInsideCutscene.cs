using UnityEngine;

public class ToriInsideCutscene : CutsceneBase
{
	private Animator toriel;

	private void Update()
	{
		if (state == 0 && !txt)
		{
			state = 1;
			gm.EnablePlayerMovement();
			toriel.Play("WalkRight");
			toriel.SetFloat("speed", 1f);
		}
		if (state == 1)
		{
			toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(10f, -1.62f), 1f / 6f);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(52) == 1)
		{
			EndCutscene();
			return;
		}
		kris.GetComponent<BoxCollider2D>().enabled = false;
		base.StartCutscene(par);
		gm.SetFlag(52, 1);
		StartText(new string[5] { "* Welcome home, you two.", "* Whoa.", "* (This is way bigger\n  than Kris's house.)", "* The surprise is at the\n  end of the hallway\n  to the right.", "* I will meet you two\n  there." }, new string[5] { "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18], new string[5] { "tori_happy", "su_wideeye", "su_smirk_sweat", "tori_neutral", "tori_neutral" }, 0);
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		toriel.transform.position = new Vector3(0f, -1.62f);
		toriel.Play("WalkDown");
	}
}
