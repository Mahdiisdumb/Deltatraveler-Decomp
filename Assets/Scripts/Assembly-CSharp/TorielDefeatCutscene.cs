using UnityEngine;

public class TorielDefeatCutscene : CutsceneBase
{
	private Animator toriel;

	private bool playSound;

	private void Update()
	{
		if (state == 0)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					susie.ChangeDirection(Vector2.right);
				}
				if (txt.GetCurrentStringNum() == 28 && !playSound)
				{
					playSound = true;
					PlaySFX("sounds/snd_item");
				}
				if (txt.GetCurrentStringNum() == 33)
				{
					susie.ChangeDirection(Vector2.up);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic(60f);
					susie.ChangeDirection(Vector2.right);
					toriel.SetFloat("speed", 1f);
					toriel.GetComponent<SpriteRenderer>().flipX = false;
				}
				if (toriel.transform.position.x != 7.73f)
				{
					toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(7.73f, 0.3f), 0.125f);
				}
				else
				{
					toriel.GetComponent<SpriteRenderer>().enabled = false;
					kris.ChangeDirection(Vector2.down);
					susie.ChangeDirection(Vector2.up);
					StartText(new string[4] { "* So,^05 you've gotta get\n  outta here...", "* And I've gotta find\n  this <color=#FFFF00FF>ROYAL SCIENTIST</color>\n  guy.", "* This should be pretty\n  fun.", "* Let's get going." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_neutral", "su_side", "su_confident", "su_smile" }, 0);
					state = 1;
				}
			}
		}
		if (state == 1 && !txt)
		{
			kris.SetSelfAnimControl(setAnimControl: true);
			susie.SetSelfAnimControl(setAnimControl: true);
			gm.PlayMusic("zoneMusic");
			gm.SetCheckpoint(14, new Vector3(91.8f, 0.6f));
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetFlag(8, 1);
		Object.Destroy(GameObject.Find("SusieSOUL"));
		kris.transform.position = new Vector3(-0.65f, 0.88f);
		kris.ChangeDirection(Vector2.right);
		kris.GetComponent<Animator>().Play("idle");
		susie.transform.position = new Vector3(-1.16f, -0.35f);
		susie.EnableAnimator();
		susie.ChangeDirection(Vector2.up);
		susie.UseHappySprites();
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		toriel.enabled = true;
		toriel.Play("WalkRight", 0, 0f);
		toriel.GetComponent<SpriteRenderer>().flipX = true;
		toriel.transform.position = new Vector3(1.47f, 0.3f);
		gm.PlayMusic("music/mus_toriel", 0.75f);
		StartText(new string[33]
		{
			"* ...^10 Thanks.", "* Well,^05 uhh,^05 the name's\n  Susie.", "* Uhh... do you know\n  anyone by the name\n  of Kris?", "* Kris...", "* I have never heard\n  that name before.", "* Huh.", "* Well,^05 you're their mom\n  where I'm from.", "* Really?", "* Well,^05 I do not know\n  how that could be.", "* Unless...",
			"* Unless...?", "* Could it be that\n  you...^10 are from\n  another world?", "* I guess that explains\n  what's going on.", "* Do you know how\n  I can get outta\n  here?", "* Possibly.", "* You could perhaps meet\n  with the UNDERGROUND's\n  <color=#FFFF00FF>ROYAL SCIENTIST</color>.", "* He is always doing...\n  ^15interesting experiments.", "* He may be able to help\n  you travel home.", "* And how can I find\n  this guy?", "* You will have to\n  travel all the way to\n  HOTLAND.",
			"* It is pretty far away,\n^10  but I'm sure that\n  you can make it.", "* And my child,^05 that\n  look in your eye...", "* You wish to accompany\n  her,^05 do you not?", "* The two of you\n  together seem to be\n  very strong.", "* So I am not very\n  concerned about you\n  two leaving.", "* However,^05 before you\n  leave,^05 I will give\n  you a CELL PHONE.", "* In the case that\n  you need assistance,^05\n  you may call me.", "* (You got the CELL PHONE.)", "* I will get something\n  prepared for you.", "* I will see you\n  two very soon.",
			"* My child,^05 be good,\n ^05 alright?", "* And Susie,^05 watch over\n  and protect this child.", "* I will,^05 don't worry."
		}, new string[33]
		{
			"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor",
			"snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor",
			"snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_text", "snd_txttor", "snd_txttor",
			"snd_txttor", "snd_txttor", "snd_txtsus"
		}, new int[33], new string[33]
		{
			"su_depressed", "su_neutral", "su_smile_sweat", "tori_neutral", "tori_worry", "su_inquisitive", "su_annoyed", "tori_blush", "tori_neutral", "tori_worry",
			"su_smile_sweat", "tori_worry", "su_inquisitive", "su_neutral", "tori_worry", "tori_neutral", "tori_worry", "tori_neutral", "su_surprised", "tori_worry",
			"tori_happy", "tori_worry", "tori_worry", "tori_neutral", "tori_happy", "tori_neutral", "tori_neutral", "", "tori_neutral", "tori_neutral",
			"tori_worry", "tori_worry", "su_smirk_sweat"
		}, 0);
	}
}
