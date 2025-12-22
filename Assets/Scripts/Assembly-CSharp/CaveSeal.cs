using System;
using System.Collections.Generic;
using UnityEngine;

public class CaveSeal : InteractTextBox
{
	private int frames;

	private bool activated;

	private bool started;

	private OverworldPlayer kris;

	private OverworldPartyMember susie;

	private OverworldPartyMember noelle;

	private CameraController cam;

	private Transform bomb;

	protected override void Awake()
	{
		if ((int)Util.GameManager().GetFlag(118) == 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
		else
		{
			GameObject.Find("CaveLoadingZone").transform.position = new Vector3(1000f, -11.956f);
		}
		List<string> list = new List<string>();
		list.AddRange(lines);
		if ((int)Util.GameManager().GetFlag(116) == 0)
		{
			if (Util.GameManager().GetPartyMember(3) == 3)
			{
				list.Add("* This is where that\n  bomb would be used.");
				list.Add("* We need to go to\n  the center of town\n  to get it.");
				sounds[2] = "snd_txtpau";
				sounds[3] = "snd_txtpau";
				portraits[2] = "pau_dejected";
				portraits[3] = "pau_neutral";
			}
			else
			{
				list.Add("* It's...^05 really tight\n  on there.");
				list.Add("* How the hell are\n  we supposed to get\n  through?");
			}
		}
		else
		{
			list.Add("* Aight,^05 move aside.");
			portraits[2] = (((int)Util.GameManager().GetFlag(13) >= 5) ? "su_annoyed" : "su_confident");
		}
		lines = list.ToArray();
		base.Awake();
	}

	public override void DoInteract()
	{
		base.DoInteract();
		if ((int)Util.GameManager().GetFlag(116) != 0)
		{
			cam = Util.FindObjectOfType<CameraController>();
			kris = Util.OverworldPlayer();
			susie = Util.OverworldPlayer().GetPartyMemberByID(1);
			noelle = Util.OverworldPlayer().GetPartyMemberByID(2);
			bomb = GameObject.Find("Bomb").transform;
			Util.GameManager().SetFlag(118, 1);
			activated = true;
		}
	}

	protected override void Update()
	{
		if (!activated || (bool)txt)
		{
			return;
		}
		if (!started)
		{
			started = true;
			cam.SetFollowPlayer(follow: false);
			kris.SetSelfAnimControl(setAnimControl: false);
			susie.SetSelfAnimControl(setAnimControl: false);
			noelle.SetSelfAnimControl(setAnimControl: false);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			kris.GetComponent<Animator>().SetBool("isMoving", value: true);
			kris.GetComponent<Animator>().SetFloat("speed", 1f);
			kris.ChangeDirection(Vector2.right);
			susie.GetComponent<Animator>().SetBool("isMoving", value: true);
			susie.GetComponent<Animator>().SetFloat("speed", 1f);
			susie.ChangeDirection(Vector2.up);
			noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
			noelle.GetComponent<Animator>().SetFloat("speed", 1f);
			noelle.ChangeDirection(Vector2.right);
		}
		if (frames == 0)
		{
			bool flag = true;
			if (kris.transform.position.x > 48.04f)
			{
				kris.transform.position = new Vector3(Mathf.MoveTowards(kris.transform.position.x, 48.04f, 1f / 12f), kris.transform.position.y);
				flag = false;
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (susie.transform.position != new Vector3(50f, -14.06f))
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(50f, -14.06f), 1f / 12f);
				flag = false;
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (noelle.transform.position.x > 48.04f && noelle.transform.position.y > -15.33f)
			{
				noelle.transform.position = new Vector3(Mathf.MoveTowards(noelle.transform.position.x, 48.04f, 1f / 12f), noelle.transform.position.y);
				flag = false;
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (flag)
			{
				frames++;
			}
			return;
		}
		frames++;
		if (frames == 20)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
			susie.GetComponent<Animator>().SetFloat("speed", 0f);
			susie.GetComponent<Animator>().Play("Kick", 0, 0f);
		}
		if (frames == 45)
		{
			susie.GetComponent<Animator>().SetFloat("speed", 1f);
		}
		if (frames >= 20 && frames <= 50)
		{
			bomb.position = new Vector3(50f, -14.056f + Mathf.Sin((float)((frames - 20) * 6) * (MathF.PI / 180f)));
		}
		if (frames == 50)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_kick_bomb");
		}
		if (frames > 50 && frames <= 60)
		{
			bomb.position = new Vector3(50f, Mathf.Lerp(-14.056f, -11.838f, (float)(frames - 50) / 5f));
		}
		if (frames == 70)
		{
			bomb.position = new Vector3(1000f, 0f);
			UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/RealisticExplosion"), new Vector3(50f, -11.385f), Quaternion.identity).transform.localScale = new Vector3(2f, 2f, 1f);
			susie.GetComponent<Animator>().Play("idle");
			GetComponent<SpriteRenderer>().enabled = false;
			GameObject.Find("CaveLoadingZone").transform.position = new Vector3(50f, -11.956f);
		}
		if (frames >= 110)
		{
			if (cam.transform.position != cam.GetClampedPos())
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 0.0625f);
				return;
			}
			cam.SetFollowPlayer(follow: true);
			kris.SetSelfAnimControl(setAnimControl: true);
			susie.SetSelfAnimControl(setAnimControl: true);
			noelle.SetSelfAnimControl(setAnimControl: true);
			kris.ChangeDirection(Vector2.down);
			susie.ChangeDirection(Vector2.left);
			noelle.ChangeDirection(Vector2.up);
			Util.GameManager().EnablePlayerMovement();
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}
}
