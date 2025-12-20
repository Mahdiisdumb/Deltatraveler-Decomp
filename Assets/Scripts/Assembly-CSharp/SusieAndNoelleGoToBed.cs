using System;
using UnityEngine;

public class SusieAndNoelleGoToBed : CutsceneBase
{
	private int animType;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (animType != 3)
		{
			if (state == 0)
			{
				frames++;
				if (frames < 20)
				{
					return;
				}
				if (frames <= 50)
				{
					if (!MoveTo(susie, new Vector3(2.08f, -1.42f), 4f))
					{
						SetMoveAnim(susie, isMoving: false);
					}
					else
					{
						SetMoveAnim(susie, isMoving: true);
					}
				}
				else if (frames == 51)
				{
					SetSprite(susie, "spr_su_up_run_0");
				}
				else if (frames >= 55 && frames <= 75)
				{
					if (frames == 55)
					{
						susie.GetComponent<SpriteRenderer>().flipX = true;
						PlayAnimation(susie, "FallBack");
						PlaySFX("sounds/snd_jump");
					}
					susie.transform.position = Vector3.Lerp(new Vector3(2.08f, -1.42f), new Vector3(2.477f, 0.208f), (float)(frames - 55) / 20f) + new Vector3(0f, Mathf.Sin((float)((frames - 55) * 9) * (MathF.PI / 180f)));
					if (frames == 75)
					{
						SetSprite(susie, "spr_su_chilling_0");
						gm.PlayGlobalSFX("sounds/snd_heavyswing");
					}
				}
				if (noelle.transform.position.y < -1.51f)
				{
					MoveTo(noelle, new Vector3(2.99f, -1.51f), 4f);
					SetMoveAnim(noelle, isMoving: true);
					return;
				}
				if (noelle.transform.position.x < 4.61f)
				{
					MoveTo(noelle, new Vector3(4.61f, -1.51f), 5f);
					ChangeDirection(noelle, Vector2.right);
					return;
				}
				if (MoveTo(noelle, new Vector3(4.61f, 2.67f), 6f))
				{
					ChangeDirection(noelle, Vector2.up);
					return;
				}
				SetMoveAnim(noelle, isMoving: false);
				if (frames >= 75)
				{
					noelle.UseUnhappySprites();
					frames = 0;
					state = 1;
				}
			}
			else if (state == 1)
			{
				frames++;
				if (frames == 10)
				{
					ChangeDirection(noelle, Vector2.down);
				}
				if (frames == 35)
				{
					SetSprite(susie, "spr_su_chilling_1");
				}
				if (animType == 0)
				{
					if (frames == 50)
					{
						PlayAnimation(noelle, "Laugh");
					}
					if (frames == 80)
					{
						PlayAnimation(noelle, "idle");
						noelle.UseHappySprites();
					}
				}
				else if (animType == 1)
				{
					if (frames == 80)
					{
						SetSprite(susie, "spr_su_chilling_oblit_2");
					}
				}
				else if (animType == 2 && frames == 65)
				{
					SetSprite(noelle, "spr_no_down_depressed_reject");
					SetSprite(susie, "spr_su_chilling_oblit_2");
				}
				if (frames == 100)
				{
					PlayAnimation(noelle, "idle");
					ChangeDirection(noelle, Vector2.up);
				}
				if (frames == 120)
				{
					GameObject.Find("RAGE").GetComponent<SpriteRenderer>().enabled = true;
					GameObject.Find("LightsOff").GetComponent<SpriteRenderer>().enabled = true;
					PlaySFX("sounds/snd_noise");
				}
				if (frames < 135)
				{
					return;
				}
				if (frames == 135)
				{
					SetMoveAnim(noelle, isMoving: true);
					ChangeDirection(noelle, Vector2.left);
					GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>().enabled = true;
					if (animType == 2)
					{
						SetSprite(susie, "spr_su_chilling_oblit_0");
					}
				}
				else if (frames == 155 && animType != 2)
				{
					SetSprite(susie, "spr_su_passed_out");
					PlaySFX("sounds/snd_bump");
				}
				if (!MoveTo(noelle, new Vector3(1.773f, 2.936f), 4f))
				{
					ChangeDirection(noelle, Vector2.down);
					SetMoveAnim(noelle, isMoving: false);
					state = 2;
					frames = 0;
				}
			}
			else
			{
				if (state != 2)
				{
					return;
				}
				frames++;
				if (animType == 2)
				{
					if (frames == 15)
					{
						noelle.GetComponent<SpriteRenderer>().enabled = false;
						SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_0_oblit");
					}
					else if (frames == 25)
					{
						PlaySFX("sounds/snd_wing");
						SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_1_oblit");
					}
					else if (frames == 40)
					{
						noelle.GetComponent<SpriteRenderer>().enabled = true;
						SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_2");
					}
					else if (frames >= 60 && frames < 75)
					{
						SetSprite(noelle, "spr_no_climb_to_bed_oblit");
						MoveTo(noelle, new Vector3(1.773f, 3.5f), 1.25f);
						if (frames == 60)
						{
							PlaySFX("sounds/snd_wing");
						}
					}
					else if (frames == 75)
					{
						noelle.GetComponent<SpriteRenderer>().enabled = false;
						SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_3_oblit");
					}
					else if (frames == 85)
					{
						PlaySFX("sounds/snd_wing");
						SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_6");
					}
					else if (frames == 115)
					{
						SetSprite(susie, "spr_su_chilling_oblit_1");
					}
					else if (frames == 205)
					{
						SetSprite(susie, "spr_su_passed_out");
						PlaySFX("sounds/snd_bump");
						state = 3;
					}
				}
				else if (frames == 15)
				{
					noelle.GetComponent<SpriteRenderer>().enabled = false;
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_0");
				}
				else if (frames == 25)
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_1");
				}
				else if (frames == 40)
				{
					noelle.GetComponent<SpriteRenderer>().enabled = true;
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_2");
				}
				else if (frames >= 60 && frames < 75)
				{
					SetSprite(noelle, "spr_no_climb_to_bed");
					MoveTo(noelle, new Vector3(1.773f, 3.5f), 1.25f);
					if (frames == 60)
					{
						PlaySFX("sounds/snd_wing");
					}
				}
				else if (frames == 75)
				{
					noelle.GetComponent<SpriteRenderer>().enabled = false;
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_3");
				}
				else if (frames == 85)
				{
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_4");
					PlaySFX("sounds/snd_wing");
				}
				else if (frames == 145)
				{
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_5");
				}
				else if (frames == 215)
				{
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_4");
				}
				else if (frames == 305)
				{
					PlaySFX("sounds/snd_wing");
					SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_6");
					state = 3;
				}
			}
		}
		else if (state == 0)
		{
			frames++;
			if (frames >= 60 && frames < 110)
			{
				if (!MoveTo(susie, new Vector3(2.49f, -1.05f), 4f))
				{
					SetMoveAnim(susie, isMoving: false);
				}
				else
				{
					SetMoveAnim(susie, isMoving: true);
				}
			}
			else if (frames == 110)
			{
				SetSprite(susie, "spr_su_climb_to_bed");
				susie.transform.position = new Vector3(2.407f, -0.569f);
				PlaySFX("sounds/snd_wing");
			}
			else if (frames == 130)
			{
				susie.transform.position = new Vector3(2.477f, 0.208f);
				SetSprite(susie, "spr_su_chilling_oblit_0");
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			frames++;
			if (frames == 120)
			{
				SetSprite(susie, "spr_su_chilling_oblit_1");
			}
			else if (frames == 180)
			{
				SetSprite(susie, "spr_su_chilling_oblit_2");
			}
			else if (frames == 300)
			{
				SetSprite(susie, "spr_su_chilling_oblit_0");
			}
			else if (frames == 450)
			{
				SetSprite(susie, "spr_su_passed_out");
				PlaySFX("sounds/snd_bump");
				state = 2;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		if (!gm.NoelleInParty())
		{
			animType = 3;
		}
		else if ((int)gm.GetFlag(87) >= 7)
		{
			animType = 2;
		}
		else if (WeirdChecker.HasCommittedBloodshed(gm))
		{
			animType = 1;
		}
		gm.SetPartyMembers(susie: false, noelle: false);
		RevokePlayerControl();
		kris.SetSelfAnimControl(setAnimControl: true);
		gm.EnablePlayerMovement();
		ChangeDirection(susie, Vector2.up);
		ChangeDirection(noelle, Vector2.up);
		if (animType == 3)
		{
			susie.transform.position = new Vector3(2.49f, -2.2725f);
			GameObject.Find("RAGE").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("LightsOff").GetComponent<SpriteRenderer>().enabled = true;
			GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>().enabled = true;
			SetSprite(GameObject.Find("NoelleBedSide").GetComponent<SpriteRenderer>(), "overworld/snow_objects/spr_bed_noelle_6");
			noelle.GetComponent<SpriteRenderer>().enabled = false;
		}
		else
		{
			susie.transform.position = new Vector3(2.08f, -2.2725f);
			noelle.transform.position = new Vector3(2.99f, -2.25f);
		}
	}
}
