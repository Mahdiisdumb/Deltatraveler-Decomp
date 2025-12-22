using UnityEngine;

public class GreaterDogIntroCutscene : CutsceneBase
{
	private bool depressed;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if ((bool)txt)
		{
			if (depressed)
			{
				if (AtLine(2))
				{
					kris.EnableAnimator();
					susie.EnableAnimator();
					noelle.EnableAnimator();
				}
			}
			else if (AtLine(2))
			{
				kris.EnableAnimator();
				noelle.EnableAnimator();
			}
			else if (AtLine(9))
			{
				SetSprite(susie, "spr_su_shrug", flipX: true);
			}
			else if (AtLine(11))
			{
				susie.EnableAnimator();
				susie.GetComponent<SpriteRenderer>().flipX = false;
			}
			else if (AtLine(13))
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_weaponpull");
				SetSprite(susie, "spr_su_threaten_stick");
			}
		}
		else
		{
			kris.InitiateBattle(72);
			EndCutscene(enablePlayerMovement: false);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		SetMoveAnim(kris, isMoving: false);
		SetMoveAnim(susie, isMoving: false);
		SetMoveAnim(noelle, isMoving: false);
		SetSprite(kris, "spr_kr_surprise");
		SetSprite(noelle, "spr_no_surprise");
		ChangeDirection(kris, Vector2.right);
		ChangeDirection(susie, Vector2.right);
		ChangeDirection(noelle, Vector2.right);
		susie.UseUnhappySprites();
		noelle.UseUnhappySprites();
		depressed = Util.GameManager().GetFlagInt(87) >= 7;
		if (depressed)
		{
			StartText(new string[7] { "* HALT.^05\n* State your business.", "* Okay,^05 we don't have\n  time for this.", "* Are you gonna try\n  to fight us if we\n  go anyway?", "* That would violate the lockdown\n  associated with PROTOCOL 727.", "* Such a violation would be\n  punishable by force.", "* Then just get this\n  over with.", "* Understood,^05 human sympathizers." }, new string[7] { "snd_txtmtt", "snd_txtsus", "snd_txtsus", "snd_txtmtt", "snd_txtmtt", "snd_txtsus", "snd_txtmtt" }, new int[1], new string[7] { "", "su_annoyed", "su_side", "", "", "su_annoyed", "" });
		}
		else
		{
			StartText(new string[13]
			{
				"* HALT.^05\n* State your business.", "* Umm...", "* Why don't YOU move\n  out of the way?", "* This area is on lockdown.", "* A human has been sighted in\n  the forest and we cannot\n  allow it to get past Snowdin.", "* PROTOCOL 727 - ^10Any human\n  sightings must be dealt with\n  immediately.", "* Its SOUL must be taken at\n  all costs.", "* What the hell did\n  humans even do to\n  you people?", "* You act like they\n  trapped you down here\n  to starve or something.", "* Perhaps you are a group of\n  human sympathizers.",
				"* I have no tolerance for scum\n  like you.", "* I will deal with you\n  PERSONALLY.", "* Alright,^05 punk,^05 it's\n  ON!!!"
			}, new string[13]
			{
				"snd_txtmtt", "snd_txtsus", "snd_txtsus", "snd_txtmtt", "snd_txtmtt", "snd_txtmtt", "snd_txtmtt", "snd_txtsus", "snd_txtsus", "snd_txtmtt",
				"snd_txtmtt", "snd_txtmtt", "snd_txtsus"
			}, new int[1], new string[13]
			{
				"", "su_side", "su_annoyed", "", "", "", "", "su_annoyed", "su_smirk_sweat", "",
				"", "", "su_angry"
			});
		}
	}
}
