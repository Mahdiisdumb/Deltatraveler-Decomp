using System;
using UnityEngine;

public class GreyDoorSection2Oblit : CutsceneBase
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

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			bool flag = true;
			if (kris.transform.position != new Vector3(11.34f, -4.22f))
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(11.34f, -4.22f), 1f / 12f);
				flag = false;
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", false);
				kris.ChangeDirection(Vector2.up);
			}
			if (susie.transform.position != new Vector3(14.07f, -4.02f))
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(14.07f, -4.02f), 1f / 12f);
				flag = false;
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.ChangeDirection(Vector2.up);
			}
			if (noelle.transform.position != new Vector3(12.67f, -4.21f))
			{
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(12.67f, -4.21f), 1f / 12f);
				flag = false;
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				noelle.ChangeDirection(Vector2.up);
			}
			if (cam.transform.position != new Vector3(12.62f, -3.08f, -10f))
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(12.62f, -3.08f, -10f), 1f / 12f);
				flag = false;
			}
			if (flag)
			{
				state = 5;
				frames = 0;
			}
		}
		if (state == 5)
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
				kris.transform.position = Vector3.Lerp(new Vector3(11.34f, -4.22f), new Vector3(11.34f, -5.08f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
				susie.transform.position = Vector3.Lerp(new Vector3(14.07f, -4.02f), new Vector3(14.07f, -4.88f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
				noelle.transform.position = Vector3.Lerp(new Vector3(12.67f, -4.21f), new Vector3(12.67f, -5.07f), Mathf.Sin((float)((frames - 45) * 9) * ((float)Math.PI / 180f)));
			}
			else if (frames >= 65 && frames <= 75)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				kris.transform.position = Vector3.Lerp(new Vector3(11.34f, -4.22f), new Vector3(11.92f, -2.82f), (float)(frames - 65) / 10f);
				susie.transform.position = Vector3.Lerp(new Vector3(14.07f, -4.02f), new Vector3(13.35f, -2.6f), (float)(frames - 65) / 10f);
				noelle.transform.position = Vector3.Lerp(new Vector3(12.67f, -4.21f), new Vector3(12.64f, -2.9f), (float)(frames - 65) / 10f);
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
		kris.SetSelfAnimControl(false);
		kris.GetComponent<Animator>().SetFloat("speed", 1f);
		kris.GetComponent<Animator>().SetBool("isMoving", true);
		kris.ChangeDirection(Vector2.up);
		susie.UseUnhappySprites();
		susie.SetSelfAnimControl(false);
		susie.GetComponent<Animator>().SetFloat("speed", 1f);
		susie.GetComponent<Animator>().SetBool("isMoving", true);
		susie.GetComponent<Animator>().Play("idle");
		susie.ChangeDirection(Vector2.up);
		noelle.SetSelfAnimControl(false);
		noelle.GetComponent<Animator>().SetFloat("speed", 1f);
		noelle.GetComponent<Animator>().SetBool("isMoving", true);
		noelle.GetComponent<Animator>().Play("idle");
		noelle.ChangeDirection(Vector2.up);
		cam.SetFollowPlayer(false);
		greyDoor = GameObject.Find("GreyDoor").GetComponent<SpriteRenderer>();
		krisBW = GameObject.Find("KrisBW").GetComponent<SpriteRenderer>();
		susieBW = GameObject.Find("SusieBW").GetComponent<SpriteRenderer>();
		noelleBW = GameObject.Find("NoelleBW").GetComponent<SpriteRenderer>();
	}
}
