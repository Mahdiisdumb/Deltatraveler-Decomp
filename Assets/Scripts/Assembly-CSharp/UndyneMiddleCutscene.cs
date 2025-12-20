using System;
using UnityEngine;

public class UndyneMiddleCutscene : CutsceneBase
{
	private Animator undyne;

	private int undyneFrames;

	private bool noleRun;

	private float landPos;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (susie.transform.position.x < 4.44f)
			{
				susie.transform.position += new Vector3(1f / 6f, 0f);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					SetSprite(susie, "spr_su_dragkrisnole_1");
					PlaySFX("sounds/snd_jump");
				}
				if (frames <= 30)
				{
					float num = Mathf.Sin((float)(frames * 6) * ((float)Math.PI / 180f));
					susie.transform.position = new Vector3(susie.transform.position.x + 1f / 6f, -0.59f + num);
				}
				else if (frames <= 60)
				{
					float num2 = (float)(frames - 30) / 30f;
					num2 = Mathf.Sin(num2 * (float)Math.PI * 0.5f);
					if (frames == 31)
					{
						PlaySFX("sounds/snd_noise");
						SetSprite(kris, "spr_kr_sit");
						SetSprite(susie, "spr_su_kneel");
						SetSprite(noelle, "spr_no_kneel_right");
						landPos = susie.transform.position.x;
						kris.GetComponent<SpriteRenderer>().enabled = true;
					}
					kris.transform.position = Vector3.Lerp(new Vector3(landPos, -1.07f), new Vector3(11.15f, -1.29f), num2);
					susie.transform.position = new Vector3(Mathf.Lerp(landPos, 12.27f, num2), -0.59f);
					noelle.transform.position = Vector3.Lerp(new Vector3(9.32f, 0f), new Vector3(11.2f, 0.43f), num2);
				}
				else
				{
					if (frames == 61)
					{
						SetMoveAnim(susie, true);
						ChangeDirection(susie, Vector2.right);
						PlayAnimation(susie, "run", 1.5f);
					}
					if (susie.transform.position.x < 15.97f)
					{
						susie.transform.position += new Vector3(5f / 24f, 0f);
					}
					else
					{
						SetSprite(noelle, "spr_no_panic_right");
						StartText(new string[1] { "* Susie,^05 wait!!" }, new string[1] { "snd_txtnoe" }, new int[1], new string[1] { "no_afraid_open" });
						state = 1;
						frames = 0;
					}
				}
			}
			if (frames <= 30 && !txt)
			{
				kris.transform.position = susie.transform.position;
			}
		}
		else if (state == 1 && !txt)
		{
			gm.LockMenu();
			gm.SetPartyMembers(false, false);
			RestorePlayerControl();
			gm.EnablePlayerMovement();
			kris.EnableAnimator();
			ChangeDirection(kris, Vector2.down);
			noelle.EnableAnimator();
			noelle.UseUnhappySprites();
			ChangeDirection(noelle, Vector2.right);
			noelle.SetSelfAnimControl(false);
			SetMoveAnim(noelle, true, 1.5f);
			if (noleRun)
			{
				PlayAnimation(noelle, "run", 1.5f);
			}
			state = 2;
			UnityEngine.Object.FindObjectOfType<ActionPartyPanels>().UpdatePanels();
			UnityEngine.Object.FindObjectOfType<UndyneSpearSpawner>().Activate();
			if (Util.GameManager().GetHP(0) < Util.GameManager().GetMaxHP(0))
			{
				UnityEngine.Object.FindObjectOfType<ActionPartyPanels>().Raise();
				UnityEngine.Object.FindObjectOfType<ActionPartyPanels>().UpdateHP(gm.GetHPArray());
			}
		}
		else if (state == 2)
		{
			noelle.transform.position += new Vector3(1f / 6f, 0f);
			frames++;
			int frame = frames;
			int num3 = 900;
		}
		if (!MoveTo(undyne, new Vector3(3.2f, -0.43f), 8f))
		{
			undyneFrames++;
			if (undyneFrames == 1)
			{
				SetMoveAnim(undyne, false);
			}
			if (undyneFrames == 30)
			{
				ChangeDirection(undyne, Vector2.up);
			}
			if (undyneFrames == 60)
			{
				ChangeDirection(undyne, Vector2.right);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		RevokePlayerControl();
		cam.SetFollowPlayer(true);
		undyne = GameObject.Find("Undyne").GetComponent<Animator>();
		ChangeDirection(undyne, Vector2.right);
		SetMoveAnim(undyne, true);
		undyne.transform.position = new Vector3(-11.49f, -0.43f);
		susie.transform.position = new Vector3(-7.32f, -0.59f);
		PlayAnimation(susie, "DragKrisNoelle");
		kris.GetComponent<SpriteRenderer>().enabled = false;
		noleRun = GameManager.GetOptions().runAnimations.value == 1;
	}
}
