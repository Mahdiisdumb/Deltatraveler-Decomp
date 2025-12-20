using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GreyDoorSection1 : CutsceneBase
{
	private SpriteRenderer greyDoor;

	private int shakeFrames;

	private float velocity;

	private SpriteRenderer krisBW;

	private SpriteRenderer susieBW;

	private SpriteRenderer noelleBW;

	private TextBoxEB txtEB;

	private int ki;

	private int si;

	private int ni;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.ChangeDirection(Vector2.up);
				kris.GetComponent<Animator>().SetBool("isMoving", value: true);
				kris.GetComponent<Animator>().SetFloat("speed", 0.7f);
				susie.ChangeDirection(Vector2.up);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.GetComponent<Animator>().SetFloat("speed", 0.7f);
				noelle.ChangeDirection(Vector2.up);
				noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
				cam.SetFollowPlayer(follow: false);
			}
			if (cam.transform.position != new Vector3(2.083f, 0f, -10f))
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(2.083f, 0f, -10f), 1f / 24f);
			}
			if (kris.transform.position != new Vector3(3.08f, 1.11f))
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(3.08f, 1.11f), 0.0625f);
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (susie.transform.position != new Vector3(0.98f, 1.26f))
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(0.98f, 1.26f), 0.0625f);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (noelle.transform.position != new Vector3(2.09f, 1.74f))
			{
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(2.09f, 1.74f), 5f / 48f);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			MonoBehaviour.print(frames);
			if (frames == 100)
			{
				gm.PlayMusic("music/mus_creepydoor");
				StartText(new string[2] { "* It's a grey door.", "* Who the hell put\n  a door out here???" }, new string[2] { "snd_txtnoe", "snd_txtsus" }, new int[2], new string[2] { "no_curious", "su_side_sweat" }, 1);
				state = 1;
				frames = 0;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.sprite = Resources.Load<Sprite>("overworld/spr_grey_door_1");
				PlaySFX("sounds/snd_elecdoor_shutheavy");
			}
			if (frames == 30)
			{
				StartText(new string[3] { "* It...^05 opened on its\n  own...", "* Maybe it's a prank?", "* I wouldn't put it\n  past that Sans guy." }, new string[3] { "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[3], new string[3] { "no_shocked", "su_smirk_sweat", "su_annoyed" }, 1);
				state = 2;
				frames = 0;
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
				noelle.GetComponent<Animator>().SetFloat("speed", 0.4f);
			}
			if (noelle.transform.position != new Vector3(2.09f, 2.83f))
			{
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(2.09f, 2.83f), 1f / 48f);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (frames == 90)
			{
				StartText(new string[3] { "* It's completely dark\n  in here.", "* Hello...?", "* ...!" }, new string[3] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe" }, new int[3], new string[3] { "no_confused", "no_confused", "no_shocked" }, 1);
				state = 3;
				frames = 0;
			}
		}
		if (state == 3)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_surprise_up");
					PlaySFX("sounds/snd_wing");
					gm.StopMusic();
				}
				if (frames <= 3)
				{
					int num = ((frames % 2 == 0) ? 1 : (-1));
					int num2 = 3 - frames;
					noelle.transform.position = new Vector3(2.09f, 2.83f) + new Vector3((float)(num2 * num) / 24f, 0f);
				}
				if (frames == 30)
				{
					StartText(new string[2] { "* Noelle???", "* I'm...^10 losing balance!!!" }, new string[2] { "snd_txtsus", "snd_txtnoe" }, new int[3], new string[2] { "su_surprised", "no_scared" }, 1);
				}
				if (frames == 31)
				{
					noelle.EnableAnimator();
					noelle.GetComponent<Animator>().Play("Fall");
					noelle.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
					PlaySFX("sounds/snd_noelle_scared");
					velocity = 1f / 24f;
					susie.DisableAnimator();
					susie.SetSprite("spr_su_surprise_up");
				}
				if (frames >= 31)
				{
					velocity -= 1f / 72f;
					noelle.transform.position += new Vector3(0f, velocity);
				}
				if (frames == 60)
				{
					StartText(new string[1] { "* NOELLE!!" }, new string[1] { "snd_txtsus" }, new int[3], new string[1] { "su_shocked" }, 1);
					state = 4;
					frames = 0;
				}
			}
			else if (frames == 30)
			{
				shakeFrames = (shakeFrames + 1) % 30;
				if (shakeFrames <= 3)
				{
					int num3 = ((shakeFrames % 2 == 0) ? 1 : (-1));
					int num4 = 3 - shakeFrames;
					noelle.transform.position = new Vector3(2.09f, 2.83f) + new Vector3((float)(num4 * num3) / 24f, 0f);
				}
			}
		}
		if (state == 4 && !txt)
		{
			frames++;
			if (frames <= 10)
			{
				susie.EnableAnimator();
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.GetComponent<Animator>().SetFloat("speed", 3f);
				susie.transform.position = Vector3.Lerp(new Vector3(0.98f, 1.26f), new Vector3(2.09f, 2.83f), (float)frames / 10f);
				if (frames == 10)
				{
					susie.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.VisibleInsideMask;
					susie.GetComponent<Animator>().Play("FallBack");
					PlaySFX("sounds/snd_jump");
					velocity = 1f / 6f;
				}
			}
			else
			{
				velocity -= 1f / 72f;
				susie.transform.position += new Vector3(0f, velocity);
			}
			if (frames >= 15 && frames <= 35)
			{
				kris.ChangeDirection(Vector2.left);
				kris.GetComponent<Animator>().SetBool("isMoving", value: true);
				kris.GetComponent<Animator>().SetFloat("speed", 0.7f);
				kris.transform.position = Vector3.Lerp(new Vector3(3.08f, 1.11f), new Vector3(2.083f, 1.11f), (float)(frames - 15) / 20f);
			}
			else if (frames >= 35 && frames < 45)
			{
				kris.ChangeDirection(Vector2.up);
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			else if (frames >= 45 && frames <= 65)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 0f);
				kris.GetComponent<Animator>().Play("RunUp", 0, 0f);
				kris.transform.position = Vector3.Lerp(new Vector3(2.083f, 1.11f), new Vector3(2.083f, 0.25f), Mathf.Sin((float)((frames - 45) * 9) * (MathF.PI / 180f)));
			}
			else if (frames >= 65 && frames <= 75)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				kris.transform.position = Vector3.Lerp(new Vector3(2.083f, 1.11f), new Vector3(2.083f, 2.83f), (float)(frames - 65) / 10f);
			}
			if (frames == 75)
			{
				kris.GetComponent<Animator>().Play("Fall", 0, 0f);
				state = 5;
				frames = 0;
			}
		}
		if (state == 5)
		{
			frames++;
			if (frames == 1)
			{
				greyDoor.GetComponent<AudioSource>().Play();
			}
			if (frames <= 15)
			{
				float num5 = (float)frames / 15f;
				num5 = Mathf.Sin(num5 * MathF.PI * 0.5f);
				kris.transform.position = Vector3.Lerp(new Vector3(2.083f, 2.83f), new Vector3(2.083f, 3.5f), num5);
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
					susie.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
					noelle.GetComponent<SpriteRenderer>().maskInteraction = SpriteMaskInteraction.None;
					susie.transform.position = new Vector3(4.34f, -6.55f);
					noelle.transform.position = new Vector3(1.85f, -6.24f);
				}
				if (frames < 240)
				{
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(0f, 1f, (float)(frames - 50) / 60f);
				}
				else
				{
					if (frames == 240)
					{
						PlaySFX("sounds/snd_dtrans_lw");
						fade.FadeOut(60, Color.white);
					}
					greyDoor.GetComponent<AudioSource>().volume = Mathf.Lerp(1f, 0f, (float)(frames - 240) / 30f);
				}
				if (velocity < 0.5f)
				{
					velocity += 0.01f;
				}
				kris.transform.position += new Vector3(-5f / 192f, -1f / 32f) * velocity;
				susie.transform.position += new Vector3(1f / 96f, 1f / 24f) * velocity;
				noelle.transform.position += new Vector3(0f, 5f / 96f) * velocity;
			}
			if (frames >= 40 && frames % 15 == 1)
			{
				SpriteRenderer component = new GameObject("GreyDoorBGSquare", typeof(SpriteRenderer), typeof(GreyDoorBGSquare)).GetComponent<SpriteRenderer>();
				component.sprite = Resources.Load<Sprite>("spr_pixel");
				component.transform.position = new Vector3(cam.transform.position.x, 0f);
			}
			if (frames == 300)
			{
				state = 6;
				frames = 0;
			}
		}
		if (state == 6)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeIn(60, Color.white);
				GameObject.Find("Black").transform.position = Vector3.zero;
			}
			if (frames == 120)
			{
				Util.GameManager().ForceLoadArea(50);
			}
		}
		if (state == 7 && !txtEB)
		{
			frames++;
			if (frames == 1)
			{
				Util.GameManager().SetFramerate(30);
			}
			if (frames == 60)
			{
				SceneManager.LoadScene(6);
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 5)
		{
			kris.GetComponent<SpriteRenderer>().sortingOrder = 500;
			susie.GetComponent<SpriteRenderer>().sortingOrder = 500;
			noelle.GetComponent<SpriteRenderer>().sortingOrder = 500;
			Color color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)(frames - 150) / 60f);
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
		gm.StopMusic(60f);
		gm.SetFlag(95, 0);
		kris.SetSelfAnimControl(setAnimControl: false);
		susie.SetSelfAnimControl(setAnimControl: false);
		noelle.SetSelfAnimControl(setAnimControl: false);
		kris.GetComponent<Animator>().SetBool("isMoving", value: false);
		susie.GetComponent<Animator>().SetBool("isMoving", value: false);
		noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
		krisBW = GameObject.Find("KrisBW").GetComponent<SpriteRenderer>();
		susieBW = GameObject.Find("SusieBW").GetComponent<SpriteRenderer>();
		noelleBW = GameObject.Find("NoelleBW").GetComponent<SpriteRenderer>();
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
		StartText(new string[1] { "* Hey,^05 what the hell\n  is that...?" }, new string[1] { "snd_txtsus" }, new int[1], new string[1] { "su_side" }, 0);
	}
}
