using UnityEngine;

public class PreChilldrakeCutscene : CutsceneBase
{
	private Animator qc;

	private Animator feraldrake;

	private bool revoked;

	private void Update()
	{
		if (state == 0 && !txt)
		{
			bool flag = MoveTo(qc, new Vector3(38f, -17.6f), 4f);
			if (!revoked)
			{
				RevokePlayerControl();
				cam.SetFollowPlayer(follow: true);
				revoked = true;
			}
			if (flag)
			{
				ChangeDirection(qc, Vector2.right);
				SetMoveAnim(qc, isMoving: true);
				Vector2 vector = qc.transform.position - kris.transform.position;
				ChangeDirection(direction: (!(Mathf.Abs(vector.x) > Mathf.Abs(vector.y))) ? ((vector.y > 0f) ? Vector2.up : Vector2.down) : ((vector.x > 0f) ? Vector2.right : Vector2.left), obj: kris);
				if (susie.transform.position.y > -16.91f)
				{
					ChangeDirection(susie, Vector2.down);
					SetMoveAnim(susie, isMoving: true);
					MoveTo(susie, new Vector3(susie.transform.position.x, -16.91f), 6f);
				}
				else if (susie.transform.position.y < -18.83f)
				{
					ChangeDirection(susie, Vector2.up);
					SetMoveAnim(susie, isMoving: true);
					MoveTo(susie, new Vector3(susie.transform.position.x, -18.83f), 6f);
				}
				else
				{
					ChangeDirection(susie, qc.transform.position - susie.transform.position);
					SetMoveAnim(susie, isMoving: false);
				}
				if (noelle.transform.position.y > -16.05f)
				{
					ChangeDirection(noelle, Vector2.down);
					SetMoveAnim(noelle, isMoving: true);
					MoveTo(noelle, new Vector3(noelle.transform.position.x, -16.05f), 6f);
				}
				else if (noelle.transform.position.y < -19.27f)
				{
					ChangeDirection(noelle, Vector2.up);
					SetMoveAnim(noelle, isMoving: true);
					MoveTo(noelle, new Vector3(noelle.transform.position.x, -19.27f), 6f);
				}
				else
				{
					ChangeDirection(noelle, qc.transform.position - noelle.transform.position);
					SetMoveAnim(noelle, isMoving: false);
				}
			}
			else
			{
				SetMoveAnim(qc, isMoving: false);
			}
			if (flag)
			{
				return;
			}
			bool num = MoveTo(kris, new Vector3(36.93f, -16.91f), 4f);
			bool flag2 = MoveTo(susie, new Vector3(36.817f, -17.626f), 4f);
			bool flag3 = MoveTo(noelle, new Vector3(36.94f, -18.356f), 4f);
			if (num)
			{
				ChangeDirection(kris, Vector2.right);
				SetMoveAnim(kris, isMoving: true);
			}
			else
			{
				SetMoveAnim(kris, isMoving: false);
			}
			if (flag2)
			{
				ChangeDirection(susie, Vector2.right);
				SetMoveAnim(susie, isMoving: true);
			}
			else
			{
				SetMoveAnim(susie, isMoving: false);
			}
			if (flag3)
			{
				ChangeDirection(noelle, Vector2.right);
				SetMoveAnim(noelle, isMoving: true);
			}
			else
			{
				SetMoveAnim(noelle, isMoving: false);
			}
			if (!num && !flag2 && !flag3 && !flag)
			{
				frames++;
				if (frames == 12)
				{
					cam.SetFollowPlayer(follow: false);
					gm.StopMusic();
					PlaySFX("sounds/snd_hurtdragon");
					SetSprite(kris, "spr_kr_surprise");
					SetSprite(susie, "spr_su_surprise_right");
					SetSprite(noelle, "spr_no_surprise");
					SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_fear");
				}
				if (frames == 60)
				{
					SetSprite(qc, "overworld/npcs/underfell/spr_qc_uf_pissed");
					StartText(new string[1] { "* It's behind us,^05 isn't it?" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
					state = 1;
					frames = 0;
				}
			}
		}
		else if (state == 1 && !txt)
		{
			MoveTo(cam, new Vector3(35.84f, -17.99f, -10f), 2f);
			frames++;
			if (frames == 1)
			{
				kris.EnableAnimator();
				susie.EnableAnimator();
				noelle.EnableAnimator();
				qc.enabled = true;
				ChangeDirection(kris, Vector2.left);
				ChangeDirection(susie, Vector2.left);
				ChangeDirection(noelle, Vector2.left);
				ChangeDirection(qc, Vector2.left);
				ChangeDirection(feraldrake, Vector2.right);
				PlayAnimation(feraldrake, "Walk", 0.5f);
			}
			if (frames <= 30)
			{
				feraldrake.transform.position = new Vector3(Mathf.Lerp(32.01f, 33.11f, (float)frames / 30f), -17.8f);
				if (frames == 30)
				{
					PlayAnimation(feraldrake, "Walk", 0f);
				}
			}
			else if (frames <= 90)
			{
				if (frames == 60)
				{
					PlayAnimation(feraldrake, "Walk", 0.5f);
				}
				feraldrake.transform.position = new Vector3(Mathf.Lerp(33.11f, 34.21f, (float)(frames - 60) / 30f), -17.8f);
				if (frames == 90)
				{
					PlayAnimation(feraldrake, "Walk", 0f);
				}
			}
			else if (frames == 110)
			{
				feraldrake.enabled = false;
				SetSprite(feraldrake, "overworld/npcs/enemies/spr_snowdrake_right_1_chillred");
				PlaySFX("sounds/snd_hurtdragon");
			}
			else if (frames >= 112 && frames < 134)
			{
				feraldrake.transform.position = new Vector3(34.21f + (float)((frames % 2 != 0) ? 1 : (-1)) / 24f, -17.8f);
			}
			else if (frames == 134)
			{
				feraldrake.transform.position = new Vector3(34.21f, -17.8f);
			}
			else if (frames == 136)
			{
				SetSprite(feraldrake, "overworld/npcs/enemies/spr_snowdrake_right_0_chillred");
			}
			else if (frames == 150)
			{
				ChangeDirection(qc, Vector2.up);
				StartText(new string[6]
				{
					"* ...^05 Damn it...",
					"* I'm sorry to ask this of\n  y'all,^05 but can you take care\n  of this last Snowdrake?",
					((int)Util.GameManager().GetFlag(178) == 1) ? "* I can take that torch from\n  you if it'll make things\n  easier." : "* I'll try to finish this ladder\n  before you get done with it.",
					"* I guess so.",
					"* We've got no real choice\n  anyway.",
					"* Here we go."
				}, new string[6] { "snd_text", "snd_text", "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[1], new string[6] { "", "", "", "su_annoyed", "su_side", "su_neutral" });
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
			if ((bool)txt)
			{
				if (AtLine(2))
				{
					ChangeDirection(qc, Vector2.left);
					ChangeDirection(kris, Vector2.right);
					ChangeDirection(susie, Vector2.right);
					ChangeDirection(noelle, Vector2.right);
				}
				else if (AtLine(6))
				{
					ChangeDirection(kris, Vector2.left);
					ChangeDirection(susie, Vector2.left);
					ChangeDirection(noelle, Vector2.left);
					SetSprite(susie, "spr_su_shrug_unhappy");
				}
				return;
			}
			if ((int)Util.GameManager().GetFlag(178) == 1)
			{
				TorchHolder[] array = Util.FindObjectsOfType<TorchHolder>();
				foreach (TorchHolder torchHolder in array)
				{
					if (torchHolder.GetHolderID() == 3)
					{
						torchHolder.MakeDecision(Vector2.left, 0);
					}
				}
			}
			susie.EnableAnimator();
			kris.InitiateBattle(65);
			EndCutscene(enablePlayerMovement: false);
		}
	}

	private void LateUpdate()
	{
		if ((bool)feraldrake && feraldrake.enabled)
		{
			Sprite sprite = Resources.Load<Sprite>("overworld/npcs/enemies/" + feraldrake.GetComponent<SpriteRenderer>().sprite.name + "_chillred");
			if (sprite != null)
			{
				feraldrake.GetComponent<SpriteRenderer>().sprite = sprite;
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		qc = Util.FindObjectOfType<QCEnd>().GetComponent<Animator>();
		feraldrake = GameObject.Find("Chilldrake").GetComponent<Animator>();
		StartText(new string[2]
		{
			((int)Util.GameManager().GetFlag(178) == 1) ? "* Hey,^05 y'all brought a torch!" : "* Hey,^05 you put the torch over\n  there.^05\n* That's a pretty good idea!",
			"* Now let's go ahead and start\n  building that ladder and get\n  the hell outta here."
		}, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
	}
}
