using System.Collections.Generic;
using UnityEngine;

public class HHFirstCutscene : CutsceneBase
{
	private Animator cultist;

	private bool susiePose;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("part_0", new string[7] { "* Uhh...", "* What the hell is\n  the deal with\n  this place?", "* I know,^05 right?", "* Who's going around\n  painting the trees\n  blue?", "* That's not what\n  I'm worried about.", "* Just...^10 this place\n  feels...", "* Eerie." });
		dictionary.Add("part_1", new string[9] { "* ...", "* So...^05 who are you lot?", "* Uhh...^05 what's with\n  the get-up?", "* You gonna paint some\n  trees or something?", "* Are you telling me that\n  you're against painting the\n  world blue?", "* Why would you paint\n  everything blue?", "* It's bad for the\n  environment,^05 and--", "* I think you three need to\n  be taught a thorough lesson!", "{0}" });
		dictionary.Add("susie_reactions", new string[2] { "* You wanna try me?^10\n* Bring it on,^05 punk!", "* Hey,^05 wait a sec!" });
		return dictionary;
	}

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			cam.transform.position = Vector3.Lerp(new Vector3(7.99f, -7.66f), new Vector3(1.64f, -2.43f, -10f), (float)frames / 60f);
			kris.transform.position = Vector3.Lerp(new Vector3(0.05f, -1.02f), new Vector3(0.05f, -2.25f), (float)frames / 60f);
			susie.transform.position = Vector3.Lerp(new Vector3(0.03f, -0.61f), new Vector3(1.91f, -1.21f), (float)frames / 75f);
			noelle.transform.position = Vector3.Lerp(new Vector3(0.03f, -0.65f), new Vector3(2.29f, -3.06f), (float)frames / 75f);
			if (frames == 60)
			{
				kris.GetComponent<Animator>().SetBool("isMoving", false);
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
			}
			if (frames == 75)
			{
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				noelle.GetComponent<Animator>().SetFloat("speed", 1f);
			}
			if (frames == 80)
			{
				kris.ChangeDirection(Vector2.right);
			}
			if (frames == 95)
			{
				susie.ChangeDirection(Vector2.down);
				noelle.ChangeDirection(Vector2.right);
			}
			if (frames == 110)
			{
				susie.ChangeDirection(Vector2.up);
				noelle.ChangeDirection(Vector2.left);
			}
			if (frames == 140)
			{
				susie.ChangeDirection(Vector2.down);
				noelle.ChangeDirection(Vector2.up);
				frames = 0;
				state = 1;
				StartText(GetStringArray("part_0"), new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[7] { "su_side_sweat", "su_smile_sweat", "no_confused", "no_happy", "su_annoyed", "su_side", "su_side" }, 1);
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				cultist.SetFloat("dirX", -1f);
				cultist.SetBool("isMoving", true);
			}
			cultist.transform.position = Vector3.Lerp(new Vector3(9.44f, -4.42f), new Vector3(5.9f, -3.47f), (float)frames / 45f);
			if (frames == 30)
			{
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
			}
			if (frames == 45)
			{
				cultist.SetFloat("speed", 0f);
				cultist.transform.Find("Exclaim").GetComponent<AudioSource>().Play();
				cultist.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames == 65)
			{
				cultist.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = false;
				cultist.SetFloat("speed", 1f);
				cultist.SetBool("isMoving", false);
			}
			if (frames == 75)
			{
				state = 2;
				frames = 0;
				string text = GetString("susie_reactions", 0);
				string text2 = "su_angry";
				if ((int)gm.GetFlag(13) == 4)
				{
					text = GetString("susie_reactions", 1);
					text2 = "su_shocked";
				}
				StartText(GetStringArrayFormatted("part_1", text), new string[9] { "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_text", "snd_txtnoe", "snd_txtnoe", "snd_text", "snd_txtsus" }, new int[18], new string[9] { "", "", "su_inquisitive", "su_annoyed", "", "no_confused", "no_thinking", "", text2 }, 1);
			}
		}
		if (state != 2)
		{
			return;
		}
		if (!txt)
		{
			kris.InitiateBattle(24);
			EndCutscene(false);
		}
		else if (txt.GetCurrentStringNum() == 9 && !susiePose)
		{
			susiePose = true;
			susie.DisableAnimator();
			if ((int)gm.GetFlag(13) == 4)
			{
				susie.SetSprite("spr_su_surprise_right");
				return;
			}
			susie.SetSprite("spr_su_threaten_stick");
			PlaySFX("sounds/snd_weaponpull");
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetCheckpoint(55, new Vector3(15.87f, 0.37f));
		cultist = GameObject.Find("CultistCutscene").GetComponent<Animator>();
		kris.SetSelfAnimControl(false);
		susie.SetSelfAnimControl(false);
		noelle.SetSelfAnimControl(false);
		kris.GetComponent<Animator>().SetBool("isMoving", true);
		kris.GetComponent<Animator>().SetFloat("speed", 0.5f);
		susie.ChangeDirection(Vector2.right);
		susie.GetComponent<Animator>().SetBool("isMoving", true);
		susie.GetComponent<Animator>().SetFloat("speed", 0.5f);
		noelle.GetComponent<Animator>().SetBool("isMoving", true);
		noelle.GetComponent<Animator>().SetFloat("speed", 0.5f);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		cam.SetFollowPlayer(false);
	}
}
