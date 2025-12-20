using System;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ToriForceHandCutscenePt2 : CutsceneBase
{
	private Animator toriel;

	private Transform soul;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (kris.transform.position.x != -0.49f)
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(-0.49f, 0f), 0.125f);
			}
			else
			{
				state = 1;
				kris.GetComponent<Animator>().Play("idle");
				toriel.transform.position = new Vector3(0.457f, 0.465f);
				StartText(new string[5] { "* My child,^05 I am very\n  worried about that\n  monster.", "* She appears to be very\n  hostile,^05 given her short\n  temper.", "* It may be that she is\n  trying to trick you.", "* Many monsters here are\n  after human SOULs,^05 and\n  she might be one of them.", "* For your own safety,\n^10  it would be wise to\n  avoid her." }, new string[5] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[5] { "tori_sad", "tori_annoyed", "tori_sad", "tori_sad", "tori_worry" }, 1);
			}
		}
		if (state == 1 && !txt)
		{
			if (susie.transform.position.x < -3.68f)
			{
				susie.SetSelfAnimControl(setAnimControl: false);
				susie.GetComponent<Animator>().Play("RunRight");
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-3.68f, 0.31f), 0.25f);
				susie.GetComponent<Animator>().SetFloat("speed", 1.5f);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_kneel");
					PlaySFX("sounds/snd_wing");
					kris.ChangeDirection(Vector2.left);
				}
				if (frames < 20)
				{
					float num = (float)frames / 20f;
					susie.transform.position = new Vector3(Mathf.Lerp(-3.68f, -1.78f, Mathf.Sin(num * MathF.PI * 0.5f)), 0.31f);
				}
				else
				{
					if (frames == 20)
					{
						PlaySFX("sounds/snd_whip_hard");
						susie.SetSprite("spr_su_throw_ready");
						toriel.enabled = true;
						toriel.Play("WalkLeftMad");
					}
					if (frames <= 23 && frames >= 20)
					{
						int num2 = ((frames % 2 == 0) ? 1 : (-1));
						int num3 = 23 - frames;
						susie.transform.position = new Vector3(-1.78f, 0.31f) + new Vector3((float)(num3 * num2) / 24f, 0f);
					}
					if (frames == 35)
					{
						StartText(new string[4] { "* COME ON!!!", "* Can you at least\n  just tell me why\n  you hate me???", "* Well,^05 if you are so\n  desperate for an\n  answer,^05 then I shall.", "* Move aside,^05 human." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18], new string[4] { "su_pissed", "su_concerned", "tori_mad", "tori_annoyed" }, 1);
						state = 2;
						frames = 0;
					}
				}
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					susie.EnableAnimator();
					susie.GetComponent<Animator>().Play("idle");
				}
			}
			else
			{
				if (kris.transform.position.y != 0.79f)
				{
					kris.ChangeDirection(Vector2.down);
					kris.GetComponent<Animator>().SetBool("isMoving", value: true);
					kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(-0.49f, 0.79f), 0.125f);
				}
				else
				{
					kris.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
				if (toriel.transform.position.x != -0.75f)
				{
					toriel.SetFloat("speed", 0.5f);
					toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(-0.75f, 0.465f), 1f / 24f);
				}
				else
				{
					frames++;
					if (frames == 1)
					{
						toriel.SetFloat("speed", 0f);
						toriel.Play("WalkLeftMad", 0, 0f);
					}
					if (frames == 20)
					{
						StartText(new string[9] { "* There is a reason why\n  I have secluded myself\n  from the outside world.", "* All of you monsters\n  abide by the disgusting\n  King ASGORE.", "* He is the perpetrator\n  of this so called \"war\"\n  against humanity.", "* You and everyone else\n  are following in the\n  footsteps of bloodshed.", "* Wh....^10 what...?", "* Your failure to listen\n  to me leads me to\n  believe...", "* You are ALSO after\n  this human's SOUL.", "* As the guardian of this\n  human child,^05 I cannot\n  allow you to do so.", "* HEY,^05 JUST WAIT A\n  SECOND!!!" }, new string[9] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor", "snd_txttor", "snd_txtsus" }, new int[18], new string[9] { "tori_annoyed", "tori_mad", "tori_mad", "tori_mad", "su_worried", "tori_mad", "tori_mad", "tori_mad", "su_shocked" }, 1);
						state = 3;
						frames = 0;
					}
				}
			}
		}
		if (state != 3)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_right_worried_0");
			}
			if (txt.GetCurrentStringNum() == 7)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_freaked");
			}
			if (txt.GetCurrentStringNum() == 9)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_surprise_right");
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			Util.FindObjectOfType<InteractionTrigger>().GetComponent<BoxCollider2D>().enabled = false;
			SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = false;
			}
			BoxCollider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<BoxCollider2D>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = false;
			}
			EdgeCollider2D[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<EdgeCollider2D>();
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = false;
			}
			AudioSource[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
			foreach (AudioSource audioSource in componentsInChildren4)
			{
				if (audioSource.isPlaying)
				{
					audioSource.enabled = false;
				}
			}
			TilemapRenderer[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
			for (int i = 0; i < componentsInChildren5.Length; i++)
			{
				componentsInChildren5[i].enabled = false;
			}
			kris.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (frames == 1 || frames == 5 || frames == 9)
		{
			PlaySFX("sounds/snd_noise");
			soul.GetComponent<SpriteRenderer>().enabled = true;
		}
		if (frames == 3 || frames == 7)
		{
			soul.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (frames == 11)
		{
			susie.GetComponent<SpriteRenderer>().enabled = false;
			PlaySFX("sounds/snd_battlestart");
		}
		if (frames > 11)
		{
			soul.transform.position = Vector3.Lerp(new Vector3(-1.77f, 0.13f), new Vector3(-0.055f, -1.63f), (float)(frames - 11) / 10f);
		}
		if (frames == 25)
		{
			gm.StartBattle(29);
			EndCutscene(enablePlayerMovement: false);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(108) == 1)
		{
			base.StartCutscene(par);
			toriel = GameObject.Find("Toriel").GetComponent<Animator>();
			soul = GameObject.Find("SusieSOUL").transform;
			soul.position = new Vector3(-1.77f, 0.13f);
			gm.StopMusic();
			kris.ChangeDirection(Vector2.right);
			kris.SetSelfAnimControl(setAnimControl: false);
			kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			susie.transform.position = new Vector3(-7.49f, 0.31f);
			susie.UseUnhappySprites();
			susie.SetSelfAnimControl(setAnimControl: false);
			susie.ChangeDirection(Vector2.right);
			susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			kris.GetComponent<Animator>().Play("ToriRight");
		}
		else
		{
			EndCutscene();
		}
		if (gm.IsTestMode())
		{
			kris.InitiateBattle(29);
			EndCutscene(enablePlayerMovement: false);
		}
	}
}
