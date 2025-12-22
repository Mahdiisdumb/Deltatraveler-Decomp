using UnityEngine;

public class SusiePullKrisAwayCutscene : CutsceneBase
{
	private bool noelleMoving;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (!MoveTo(susie, kris.transform.position + new Vector3(-0.788f, 0.165f), 4f))
			{
				frames++;
				if (frames == 1)
				{
					kris.GetComponent<SpriteRenderer>().enabled = false;
					SetSprite(susie, "spr_su_sillykriskpickup_0");
					PlaySFX("sounds/snd_grab");
				}
				else if (frames == 5)
				{
					SetSprite(susie, "spr_su_sillykriskpickup_1");
				}
				else if (frames == 8)
				{
					SetSprite(noelle, "spr_no_surprise");
					SetSprite(susie, "spr_su_sillykriskpickup_2");
				}
				else if (frames == 11)
				{
					SetSprite(susie, "spr_su_sillykriskpickup_3");
				}
				else if (frames == 40)
				{
					StartText(new string[1] { "* No." }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_annoyed" });
					state = 1;
					frames = 0;
				}
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			frames++;
			if (frames == 1)
			{
				noelle.EnableAnimator();
			}
			susie.transform.position += new Vector3(0.125f, 0f);
			int num = frames / 6 % 4;
			switch (num)
			{
			case 2:
				num = 0;
				break;
			case 3:
				num = 2;
				break;
			}
			SetSprite(susie, "spr_su_sillykriskpickup_walk_" + num);
			if (!noelleMoving)
			{
				ChangeDirection(noelle, susie.transform.position - noelle.transform.position);
				if (Vector3.Distance(noelle.transform.position, susie.transform.position) > 5f)
				{
					ChangeDirection(noelle, Vector2.right);
					SetMoveAnim(noelle, isMoving: true);
					noelleMoving = true;
				}
			}
			else
			{
				noelle.transform.position += new Vector3(0.125f, 0f);
			}
			if (frames == 50)
			{
				fade.FadeOut(15);
			}
			else if (frames == 65)
			{
				gm.LoadArea(96, fadeIn: true, new Vector3(-5.91f, -0.13f), Vector2.right);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		ChangeDirection(susie, kris.transform.position + new Vector3(-0.788f, 0.165f) - susie.transform.position);
		SetMoveAnim(susie, isMoving: true);
	}
}
