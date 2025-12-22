using UnityEngine;
using UnityEngine.UI;

public class Section1EndCutscene : CutsceneBase
{
	private bool hardmode;

	private bool hardmodeJingle;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !fade.IsPlaying())
		{
			fade.FadeIn(0);
			state = 1;
			GameObject.Find("Black").GetComponent<SpriteRenderer>().enabled = true;
		}
		if (state != 1)
		{
			return;
		}
		frames++;
		if (frames == 75 && !hardmodeJingle && hardmode)
		{
			frames = 60;
			gm.PlayGlobalSFX("sounds/snd_mode");
			hardmodeJingle = true;
			GameObject.Find("HARDMODETEXT").GetComponent<Text>().enabled = true;
		}
		if (frames % 140 != 1)
		{
			return;
		}
		string[] array = new string[3] { "Title", "CreditPage1", null };
		if (frames / 140 < array.Length)
		{
			PlaySFX("music/mus_intronoise");
			for (int i = 0; i < array.Length - 1; i++)
			{
				GameObject.Find(hardmode ? "Hardmode" : "Normal").transform.Find(array[i]).localPosition = new Vector3((i != frames / 140) ? 1000 : 0, 0f);
			}
		}
		else
		{
			gm.LoadArea(47, fadeIn: true, new Vector2(-4.35f, -0.82f), Vector2.right);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		hardmode = (int)gm.GetFlag(108) == 1;
		if (PersistentSAVE.GetInt("completion", 0) < 1 && !hardmode)
		{
			PersistentSAVE.SetInt("completion", 1);
			PersistentSAVE.SetInt("last-saved-pm-0", 0);
			PersistentSAVE.SetInt("last-saved-pm-0", 1);
			PersistentSAVE.SetInt("last-saved-pm-0", 2);
		}
		Util.GameManager().SetFlag(286, 0);
		Util.FindObjectOfType<Fade>().UTFadeOut();
	}
}
