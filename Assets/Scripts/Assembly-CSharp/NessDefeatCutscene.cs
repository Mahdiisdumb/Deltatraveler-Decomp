using System;
using UnityEngine;

public class NessDefeatCutscene : CutsceneBase
{
	private SpriteRenderer greyDoor;

	private Animator paula;

	private Animator ness;

	private float velocity;

	private SpriteRenderer krisBW;

	private SpriteRenderer susieBW;

	private SpriteRenderer noelleBW;

	private int ki;

	private int si;

	private int ni;

	private bool spare;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 25 && spare)
			{
				StartText(new string[9] { "* ...", "* You know,^05 I...", "* This doesn't excuse\n  what you've done at\n  all...", "* But that feeling of\n  being unable to control\n  your fate.", "* It...^05 gets to me\n  sometimes.", "* ...", "* There are people in\n  Twoson that want to\n  know what's happening.", "* We're gonna say that\n  the mole did it\n  and bury it.", "* Just go." }, new string[9] { "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness", "snd_txtness" }, new int[5], new string[9] { "ness_sad", "ness_sad", "ness_annoyed", "ness_sad", "ness_sad", "ness_sad", "ness_annoyed", "ness_annoyed", "ness_rage" });
				state = 2;
				frames = 0;
			}
			if (frames == 60 && !spare)
			{
				StartText(new string[2] { "* ...", "* Let's just go." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[5], new string[2] { "su_depressed", "su_depressed" });
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !txt)
		{
			if (cam.transform.position != cam.GetClampedPos())
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
			}
			else
			{
				kris.ChangeDirection(Vector2.down);
				kris.SetSelfAnimControl(true);
				susie.SetSelfAnimControl(true);
				noelle.SetSelfAnimControl(true);
				cam.SetFollowPlayer(true);
				EndCutscene();
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 4)
				{
					ness.SetFloat("dirY", 1f);
					paula.SetFloat("dirX", -1f);
				}
				if (txt.GetCurrentStringNum() == 7)
				{
					ness.SetFloat("dirY", 0f);
					paula.SetFloat("dirX", 0f);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					ness.SetFloat("dirX", -1f);
					paula.SetFloat("dirX", -1f);
					paula.SetBool("isMoving", true);
					ness.SetBool("isMoving", true);
				}
				if (frames == 40)
				{
					StartText(new string[3] { "* Thank you,^05 Kris.", "* ...", "* Let's get outta here." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[5], new string[3] { "su_relieved", "su_side", "su_dejected" });
					susie.ChangeDirection(Vector2.left);
					noelle.ChangeDirection(Vector2.right);
					kris.ChangeDirection(Vector2.right);
					state = 5;
					frames = 0;
				}
			}
		}
		if (state == 5)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					susie.ChangeDirection(Vector2.up);
				}
				if (txt.GetCurrentStringNum() == 3)
				{
					kris.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
				}
			}
			else
			{
				frames++;
				if (frames == 20)
				{
					GameObject.Find("NatureSounds").GetComponent<AudioSource>().Stop();
					greyDoor.sprite = Resources.Load<Sprite>("overworld/spr_grey_door_1");
					PlaySFX("sounds/snd_elecdoor_shutheavy");
				}
				if (frames >= 45 && frames <= 65)
				{
					kris.GetComponent<Animator>().SetFloat("speed", 0f);
					kris.GetComponent<Animator>().Play("RunUp", 0, 0f);
					susie.GetComponent<Animator>().SetFloat("speed", 0f);
					susie.GetComponent<Animator>().Play("RunUp", 0, 0f);
					kris.transform.position = Vector3.Lerp(new Vector3(11.34f, -5.3866663f), new Vector3(11.34f, -6.2466664f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
					susie.transform.position = Vector3.Lerp(new Vector3(14.07f, -5.1866665f), new Vector3(14.07f, -6.0466666f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
					noelle.transform.position = Vector3.Lerp(new Vector3(12.67f, -5.376667f), new Vector3(12.67f, -6.236667f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
				}
				else if (frames >= 65 && frames <= 75)
				{
					kris.GetComponent<Animator>().SetFloat("speed", 1f);
					susie.GetComponent<Animator>().SetFloat("speed", 1f);
					kris.transform.position = Vector3.Lerp(new Vector3(11.34f, -5.3866663f), new Vector3(11.92f, -2.82f), (float)(frames - 65) / 10f);
					susie.transform.position = Vector3.Lerp(new Vector3(14.07f, -5.1866665f), new Vector3(13.35f, -2.6f), (float)(frames - 65) / 10f);
					noelle.transform.position = Vector3.Lerp(new Vector3(12.67f, -5.376667f), new Vector3(12.64f, -2.9f), (float)(frames - 65) / 10f);
				}
				if (frames == 75)
				{
					kris.GetComponent<Animator>().Play("Fall", 0, 0f);
					susie.GetComponent<Animator>().Play("FallBack", 0, 0f);
					noelle.GetComponent<Animator>().Play("Fall", 0, 0f);
					state = 6;
					frames = 0;
				}
			}
		}
		if (state == 6)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.GetComponent<AudioSource>().Play();
			}
			if (frames <= 15)
			{
				float num = (float)frames / 15f;
				num = Mathf.Sin(num * (float)Math.PI * 0.5f);
				kris.transform.position = Vector3.Lerp(new Vector3(11.92f, -2.82f), new Vector3(10.92f, -2.1499999f), num);
				susie.transform.position = Vector3.Lerp(new Vector3(13.35f, -2.6f), new Vector3(14.35f, -1.9299998f), num);
				noelle.transform.position = Vector3.Lerp(new Vector3(12.64f, -2.9f), new Vector3(12.64f, -2.23f), num);
			}
			if (frames < 50)
			{
				greyDoor.transform.GetChild(0).localScale = new Vector3(1f, 1f, 1f) * Mathf.Lerp(0f, 20f, (float)frames / 40f);
				greyDoor.GetComponent<AudioSource>().pitch = Mathf.Lerp(0.8f, 1.15f, (float)frames / 10f);
			}
			else if (frames > 50)
			{
				if (frames == 51)
				{
					velocity = 0f;
					greyDoor.GetComponent<AudioSource>().pitch = 0.8f;
					greyDoor.GetComponent<AudioSource>().volume = 0f;
					greyDoor.GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_dtrans_drone");
					greyDoor.GetComponent<AudioSource>().loop = true;
					greyDoor.GetComponent<AudioSource>().Play();
				}
				if (frames < 120)
				{
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, (float)(frames - 50) / 60f);
				}
				else
				{
					if (frames == 120)
					{
						PlaySFX("sounds/snd_dtrans_lw");
						fade.FadeOut(60, Color.white);
					}
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, (float)(frames - 120) / 30f);
				}
				if (velocity < 0.5f)
				{
					velocity += 0.01f;
				}
				kris.transform.position += new Vector3(-1f / 96f, -1f / 32f) * velocity;
				susie.transform.position += new Vector3(1f / 96f, -1f / 32f) * velocity;
				noelle.transform.position += new Vector3(0f, -1f / 32f) * velocity;
			}
			if (frames >= 40 && frames % 15 == 1)
			{
				SpriteRenderer component = new GameObject("GreyDoorBGSquare", typeof(SpriteRenderer), typeof(GreyDoorBGSquare)).GetComponent<SpriteRenderer>();
				component.sprite = Resources.Load<Sprite>("spr_pixel");
				component.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y);
			}
			if (frames == 180)
			{
				state = 7;
				frames = 0;
			}
		}
		if (state == 7)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeIn(60, Color.white);
				GameObject.Find("Black").transform.position = Vector3.zero;
			}
			if (frames == 120)
			{
				UnityEngine.Object.FindObjectOfType<GameManager>().ForceLoadArea(74);
			}
		}
		if ((state == 2 && !txt) || state >= 3)
		{
			ness.transform.position -= new Vector3(1f / 12f, 0f);
			paula.transform.position -= new Vector3(1f / 12f, 0f);
		}
	}

	private void LateUpdate()
	{
		if (state == 6)
		{
			kris.GetComponent<SpriteRenderer>().sortingOrder = 500;
			susie.GetComponent<SpriteRenderer>().sortingOrder = 500;
			noelle.GetComponent<SpriteRenderer>().sortingOrder = 500;
			Color color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)(frames - 50) / 45f);
			int num = int.Parse(kris.GetComponent<SpriteRenderer>().sprite.name.Substring(kris.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			int num2 = int.Parse(susie.GetComponent<SpriteRenderer>().sprite.name.Substring(susie.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			int num3 = int.Parse(noelle.GetComponent<SpriteRenderer>().sprite.name.Substring(noelle.GetComponent<SpriteRenderer>().sprite.name.Length - 1));
			if (ki != num)
			{
				ki = num;
				krisBW.sprite = Resources.Load<Sprite>("player/Kris/spr_kr_fall_bw_" + ki);
			}
			if (si != num2)
			{
				si = num2;
				susieBW.sprite = Resources.Load<Sprite>("player/Susie/spr_su_fall_back_bw_" + si);
			}
			if (ni != num3)
			{
				ni = num3;
				noelleBW.sprite = Resources.Load<Sprite>("player/Noelle/spr_no_fall_bw_" + ni);
			}
			krisBW.transform.position = kris.transform.position;
			susieBW.transform.position = susie.transform.position;
			noelleBW.transform.position = noelle.transform.position;
			krisBW.color = color;
			susieBW.color = color;
			noelleBW.color = color;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		if (par.Length != 0)
		{
			spare = (int)par[0] == 2;
		}
		else
		{
			spare = true;
			if (!spare)
			{
				gm.SetFlag(172, 2);
			}
		}
		if (!spare)
		{
			gm.SetFlag(84, 6);
			WeirdChecker.AdvanceTo(gm, 7, false);
			gm.SetFlag(2, "depressedx");
			gm.SetFlag(0, "g_1");
			GameObject obj = UnityEngine.Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/enemies/npc_replace/DeadEBHeroes"));
			string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
			obj.transform.Find("Ness").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_ness_kill" + text);
			obj.transform.Find("Paula").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_kill" + text);
		}
		else
		{
			WeirdChecker.Abort(gm);
		}
		UnityEngine.Object.FindObjectOfType<LoadingZone>().SetForceActivationTrigger(true);
		gm.StopMusic();
		paula = GameObject.Find("Paula").GetComponent<Animator>();
		ness = GameObject.Find("Ness").GetComponent<Animator>();
		if (spare)
		{
			ness.GetComponent<SpriteRenderer>().sortingOrder = 0;
			paula.GetComponent<SpriteRenderer>().sortingOrder = 0;
			ness.transform.position = new Vector3(11.98f, -3.27f);
			paula.transform.position = new Vector3(13.44f, -3.27f);
			ness.enabled = true;
			paula.enabled = true;
		}
		else
		{
			ness.transform.position = new Vector3(500f, 0f);
			paula.transform.position = new Vector3(500f, 0f);
		}
		GameObject.Find("NatureSounds").GetComponent<AudioSource>().Play();
		kris.transform.position = new Vector3(11.34f, -5.3866663f);
		kris.SetSelfAnimControl(false);
		kris.GetComponent<Animator>().SetBool("isMoving", false);
		kris.GetComponent<Animator>().Play("idle");
		kris.ChangeDirection(Vector2.up);
		kris.EnableAnimator();
		susie.UseUnhappySprites();
		susie.EnableAnimator();
		susie.transform.position = new Vector3(14.07f, -5.1866665f);
		susie.SetSelfAnimControl(false);
		susie.GetComponent<Animator>().SetBool("isMoving", false);
		susie.GetComponent<Animator>().Play("idle");
		susie.ChangeDirection(Vector2.up);
		noelle.transform.position = new Vector3(12.67f, -5.376667f);
		noelle.EnableAnimator();
		noelle.UseUnhappySprites();
		noelle.SetSelfAnimControl(false);
		noelle.GetComponent<Animator>().SetBool("isMoving", false);
		noelle.GetComponent<Animator>().Play("idle");
		noelle.ChangeDirection(Vector2.up);
		cam.SetFollowPlayer(false);
		cam.transform.position = new Vector3(12.62f, -3.08f, -10f);
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
		krisBW = GameObject.Find("KrisBW").GetComponent<SpriteRenderer>();
		susieBW = GameObject.Find("SusieBW").GetComponent<SpriteRenderer>();
		noelleBW = GameObject.Find("NoelleBW").GetComponent<SpriteRenderer>();
	}
}
