using UnityEngine;

public class FirstFeraldrakeCutscene : CutsceneBase
{
	private Animator feraldrake;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			cam.SetFollowPlayer(follow: false);
			if (!MoveTo(cam, new Vector3(20f, -4.6f, -10f), 2f))
			{
				frames++;
				if (frames == 20)
				{
					StartText(new string[2]
					{
						(!Util.GameManager().NoelleInParty()) ? "* Didn't see this\n  person on my way\n  back." : "* The hell is a\n  snowdrake doing down\n  here?",
						"* HEY!!!\n* What're you doing???"
					}, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_smirk_sweat", "su_angry" });
					state = 1;
					frames = 0;
				}
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				feraldrake.transform.GetChild(0).GetComponent<Animator>().enabled = true;
			}
			else if (frames == 60)
			{
				Object.Destroy(feraldrake.transform.GetChild(0).gameObject);
				SetSprite(feraldrake, "overworld/npcs/enemies/spr_snowdrake_down_1_feral");
				SetSprite(noelle, "spr_no_surprise");
				PlaySFX("sounds/snd_hurtdragon");
			}
			else if (frames >= 62 && frames < 84)
			{
				feraldrake.transform.position = new Vector3(20f + (float)((frames % 2 != 0) ? 1 : (-1)) / 24f, -5.55f);
			}
			else if (frames == 84)
			{
				feraldrake.transform.position = new Vector3(20f, -5.55f);
			}
			else if (frames == 86)
			{
				SetSprite(feraldrake, "overworld/npcs/enemies/spr_snowdrake_down_0_feral");
			}
			else if (frames == 120)
			{
				if (Util.GameManager().NoelleInParty())
				{
					StartText(new string[4] { "* The hell was that???", "* This isn't funny,^05\n  dude.", "* S-^05Susie,^10 I don't think\n  that's a normal\n  snowdrake...", "* Huh...?" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[1], new string[4] { "su_pissed", "su_annoyed", "no_shocked", "su_side_sweat" });
				}
				else
				{
					StartText(new string[2] { "* The hell was that???", "* This isn't funny,^05\n  dude." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_pissed", "su_annoyed" });
				}
				state = 2;
				frames = 0;
			}
		}
		else
		{
			if (state != 2 || ((bool)txt && (!txt || frames <= 0)))
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				feraldrake.enabled = true;
				ChangeDirection(feraldrake, Vector2.up);
				PlaySFX("sounds/snd_encounter");
				feraldrake.transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			}
			else
			{
				if (frames < 15)
				{
					return;
				}
				if (frames == 15)
				{
					Object.Destroy(feraldrake.transform.GetChild(0).gameObject);
					PlaySFX("sounds/snd_hurtdragon");
					feraldrake.SetFloat("speed", 1.25f);
				}
				if (frames == 22)
				{
					SetSprite(susie, "spr_su_freaked");
					StartText(new string[1] { "* WAIT WHAT THE HELL" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_wideeye" });
				}
				if (!MoveTo(feraldrake, kris.transform.position, 4f) || Vector3.Distance(feraldrake.transform.position, kris.transform.position) < 1f)
				{
					if ((bool)txt)
					{
						Object.Destroy(txt);
					}
					kris.InitiateBattle(62);
					EndCutscene(enablePlayerMovement: false);
				}
			}
		}
	}

	private void LateUpdate()
	{
		if (feraldrake.enabled)
		{
			Sprite sprite = Resources.Load<Sprite>("overworld/npcs/enemies/" + feraldrake.GetComponent<SpriteRenderer>().sprite.name + "_feral");
			if (sprite != null)
			{
				feraldrake.GetComponent<SpriteRenderer>().sprite = sprite;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		SetMoveAnim(kris, isMoving: false);
		SetMoveAnim(susie, isMoving: false);
		SetMoveAnim(noelle, isMoving: false);
		cam.SetFollowPlayer(follow: true);
		ChangeDirection(kris, Vector2.down);
		ChangeDirection(susie, Vector2.down);
		ChangeDirection(noelle, Vector2.down);
		if ((int)gm.GetFlag(178) == 0)
		{
			StartText(new string[4] { "* Wait,^05 hold on.", "* Huh?", "* Heh,^05 so you CAN'T\n  see in the dark.", "* There's something in\n  front of us." }, new string[1] { "snd_txtsus" }, new int[1], new string[4] { "su_neutral", "su_surprised", "su_confident", "su_smile_sweat" });
		}
		else
		{
			StartText(new string[1] { "* Wait,^05 hold on." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_neutral" });
		}
		if (gm.NoelleInParty())
		{
			gm.SetCheckpoint(87, new Vector3(20f, 1.85f));
		}
		feraldrake = GameObject.Find("Feraldrake").GetComponent<Animator>();
	}
}
