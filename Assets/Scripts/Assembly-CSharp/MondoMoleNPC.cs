using System.Collections.Generic;
using UnityEngine;

public class MondoMoleNPC : InteractSelectionBase
{
	private int state;

	private int frames;

	private bool playingCutscene;

	private int musicPlayAt = 6;

	private int musicPlayState;

	private OverworldPlayer kris;

	private OverworldPartyMember susie;

	private OverworldPartyMember noelle;

	private CameraController cam;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(150) == 1 && (int)Util.GameManager().GetFlag(13) >= 6)
		{
			CreateDeadEnemy(age: true);
		}
		if ((int)Util.GameManager().GetFlag(150) != 0)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		GetComponent<BoxCollider2D>().enabled = true;
		Util.FindObjectOfType<SAVEPoint>().transform.position = new Vector3(100f, 0.368f);
	}

	public void CreateDeadEnemy(bool age)
	{
		Debug.Log("mondo");
		string text = ((GameManager.GetOptions().contentSetting.value == 1) ? "_tw" : "");
		GameObject gameObject = Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/enemies/npc_replace/DeadMondoMole"));
		if (text != "" && !age)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_mondo_mole_kill" + text);
		}
		else if (age)
		{
			gameObject.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_mondo_mole_kill_age" + text);
		}
		gameObject.transform.SetParent(base.transform.parent);
	}

	protected override void Update()
	{
		base.Update();
		if (!playingCutscene)
		{
			return;
		}
		kris.SetCollision(onoff: false);
		if (state == 0)
		{
			bool flag = true;
			if (kris.transform.position != new Vector3(52.5f, 1.32f))
			{
				flag = false;
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(52.5f, 1.32f), 1f / 12f);
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (susie.transform.position != new Vector3(53.739f, 1.29f))
			{
				flag = false;
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(53.739f, 1.29f), 1f / 12f);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (noelle.transform.position != new Vector3(55.14f, 1.51f))
			{
				flag = false;
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(55.14f, 1.51f), 1f / 12f);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (cam.transform.position != new Vector3(53.739f, 0f, -10f))
			{
				flag = false;
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(53.739f, 0f, -10f), 1f / 24f);
			}
			if (flag)
			{
				frames++;
				if (frames == 10)
				{
					List<string> list = new List<string> { "* Okay,^05 and?", "* We need to get\n  to the grey door.", "* They just want to\n  get home.", "* So you're gonna\n  let us through,^05 or\n  else.", "* ... Very well then.", "* I am the guardian of this\n  \"Your Sanctuary\" location.^05\n* It is my power.", "* Take it from me,^05 if you\n  dare..." };
					List<string> list2 = new List<string> { "snd_txtsus", "snd_txtnoe", "snd_txtpau", "snd_txtsus", "snd_text", "snd_text", "snd_text" };
					List<string> list3 = new List<string> { "su_neutral", "no_happy", "pau_dejected", "su_teeth_eyes", "", "", "" };
					if (Util.GameManager().GetPartyMember(3) != 3)
					{
						list.RemoveAt(2);
						list2.RemoveAt(2);
						list3.RemoveAt(2);
						musicPlayAt--;
					}
					txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
					txt.CreateBox(list.ToArray(), list2.ToArray(), new int[7], giveBackControl: false, list3.ToArray());
					state = 1;
					frames = 0;
				}
			}
		}
		if (state != 1)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == musicPlayAt - 1 && musicPlayState == 0)
			{
				Util.GameManager().StopMusic();
				musicPlayState++;
			}
			else if (txt.GetCurrentStringNum() == musicPlayAt && musicPlayState == 1)
			{
				Util.GameManager().PlayMusic("music/mus_sanctuary_challenge");
				musicPlayState++;
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			GetComponent<AudioSource>().Play();
		}
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)frames / 30f);
		base.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)(30 - frames) / 30f);
		if (frames == 60)
		{
			playingCutscene = false;
			kris.InitiateBattle(51);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			Util.GameManager().SetCheckpoint(56, new Vector3(50f, -12.83f));
			OverworldEnemyBase[] array = Util.FindObjectsOfType<OverworldEnemyBase>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].StopRunning();
			}
			kris = Util.OverworldPlayer();
			susie = kris.GetPartyMemberByID(1);
			noelle = kris.GetPartyMemberByID(2);
			cam = Util.FindObjectOfType<CameraController>();
			susie.Deactivate();
			noelle.Deactivate();
			kris.SetSelfAnimControl(setAnimControl: false);
			kris.ChangeDirection(Vector2.up);
			susie.SetSelfAnimControl(setAnimControl: false);
			susie.ChangeDirection(Vector2.up);
			noelle.SetSelfAnimControl(setAnimControl: false);
			noelle.ChangeDirection(Vector2.up);
			cam.SetFollowPlayer(follow: false);
			kris.GetComponent<Animator>().SetBool("isMoving", value: true);
			kris.GetComponent<Animator>().SetFloat("speed", 1f);
			susie.GetComponent<Animator>().SetBool("isMoving", value: true);
			susie.GetComponent<Animator>().SetFloat("speed", 1f);
			noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
			noelle.GetComponent<Animator>().SetFloat("speed", 1f);
			playingCutscene = true;
		}
		else
		{
			Util.GameManager().EnablePlayerMovement();
		}
	}
}
