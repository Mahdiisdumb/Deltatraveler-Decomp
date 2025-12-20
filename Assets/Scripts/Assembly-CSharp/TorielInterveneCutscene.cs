using UnityEngine;

public class TorielInterveneCutscene : CutsceneBase
{
	private Animator toriel;

	private bool hardmode;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				toriel.SetFloat("speed", 1f);
				kris.ChangeDirection(Vector2.down);
				susie.DisableAnimator();
				susie.SetSprite("spr_su_down_unhappy_0");
				toriel.Play("WalkUp", 0, 0f);
			}
			toriel.transform.position = Vector3.Lerp(new Vector3(0f, -7f), new Vector3(0f, -2.5f), (float)frames / 30f);
			if (frames == 30)
			{
				toriel.SetFloat("speed", 0f);
				toriel.Play("WalkUp", 0, 0f);
			}
			if (frames == 45)
			{
				if (hardmode)
				{
					StartText(new string[8] { "* Huh...?", "* I apologize,^05 but I need\n  to warn you of the\n  monsters out there.", "* What.", "* I am sure you\n  remember what I said\n  earlier today.", "* That the monsters here\n  are hostile and\n  dangerous.", "* I am sure that you\n  have struggled on your\n  journey here.", "* My child,^05 if you do\n  not want to endure\n  the road ahead...", "* I ask that you\n  stay with me." }, new string[10] { "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[8] { "su_surprised", "tori_worry", "su_inquisitive", "tori_worry", "tori_sad", "tori_sad", "tori_worry", "tori_worry" }, 0);
				}
				else
				{
					StartText(new string[10] { "* Huh...?", "* I forgot to warn\n  you...^10 of the monsters\n  that live here.", "* What.", "* I can only assume that\n  in your world...", "* Humans and monsters\n  live in peace.", "* That is not so in\n  this world.", "* Where us monsters\n  have been banished\n  underground.", "* Where the tyrannous\n  <color=#FF0000FF>ASGORE</color> has sworn to\n  take any human SOUL.", "* If you change your minds\n  on your journey...", "* ..." }, new string[10] { "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[10] { "su_surprised", "tori_worry", "su_inquisitive", "tori_worry", "tori_worry", "tori_sad", "tori_sad", "tori_sad", "tori_worry", "tori_worry" }, 0);
				}
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 30)
			{
				if (hardmode)
				{
					StartText(new string[5] { "* Ms. Dreemurr,^05 I'm sure\n  they appreciate it...", "* But I really don't\n  think they want to\n  stay here,^05 either.", "* I dunno if you've\n  noticed,^05 but I'm the one\n  following them around.", "* They know where they\n  want to be.", "* And where they want\n  to be is not here." }, new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[5] { "su_side", "su_annoyed", "su_side", "su_neutral", "su_annoyed" }, 0);
				}
				else
				{
					StartText(new string[1] { "* If we change our minds,^10\n  then...?" }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_inquisitive" }, 0);
				}
			}
			if (frames == 80)
			{
				toriel.Play("WalkDownSad", 0, 0f);
			}
			if (frames == 110)
			{
				StartText(new string[4]
				{
					hardmode ? "* I understand." : "* Nevermind.",
					"* I wish you the best\n  of luck.",
					"* Be good,^15 won't you?",
					"* My child..."
				}, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18]
				{
					0, 0, 1, 2, 0, 0, 0, 0, 0, 0,
					0, 0, 0, 0, 0, 0, 0, 0
				}, new string[4] { "tori_sad", "tori_sad", "tori_sad", "tori_sad" }, 0);
				frames = 0;
				state = 2;
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				toriel.SetFloat("speed", 1f);
			}
			toriel.transform.position = Vector3.Lerp(new Vector3(0f, -2.5f), new Vector3(0f, -7f), (float)frames / 30f);
			if (frames == 45)
			{
				susie.EnableAnimator();
				kris.ChangeDirection(-susie.GetDirection());
				if (hardmode)
				{
					StartText(new string[5] { "* ...", "* Now I feel bad.", "* But I guess it just\n  has to be this way.", "* I mean,^05 it seems really\n  pointless to stop here.", "* Let's get going." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[6] { "su_side", "su_dejected", "su_dejected", "su_side", "su_neutral", "su_smile" }, 0);
				}
				else
				{
					StartText(new string[6] { "* Kris..?", "* That was really weird.", "* Do you think she was\n  about to ask us to\n  stay?", "* ...", "* Well,^10 we shouldn't let\n  her down if she gave\n  us up like that.", "* Let's go home." }, new string[6] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[6] { "su_neutral", "su_side_sweat", "su_dejected", "su_dejected", "su_smirk", "su_smile" }, 0);
				}
				state = 3;
			}
		}
		if (state == 3 && !txt)
		{
			kris.ChangeDirection(Vector2.up);
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		base.StartCutscene(par);
		gm.SetFlag(56, 1);
		hardmode = (int)gm.GetFlag(108) == 1;
		StartText(new string[1] { "* Wait a moment!" }, new string[4] { "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[1] { "" }, 0);
	}
}
