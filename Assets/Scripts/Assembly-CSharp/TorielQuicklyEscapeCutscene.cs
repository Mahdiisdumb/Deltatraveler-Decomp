using UnityEngine;

public class TorielQuicklyEscapeCutscene : CutsceneBase
{
	private Animator toriel;

	private bool hardmode;

	private Vector3 torielToPos = Vector3.zero;

	private int posID;

	private int freezeFrames;

	private Vector3[] positions = new Vector3[7]
	{
		new Vector3(3.13f, -0.29f),
		new Vector3(3.13f, 1.37f),
		new Vector3(1.43f, 1.37f),
		new Vector3(1.43f, 2.18f),
		new Vector3(2.04f, 2.18f),
		new Vector3(-0.72f, 2.18f),
		new Vector3(-0.72f, 0.41f)
	};

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0 && ((frames <= 20 && (bool)txt) || !txt))
		{
			frames++;
			if (frames <= 15)
			{
				toriel.transform.position = Vector3.Lerp(new Vector3(2.1f, 2.18f), new Vector3(-0.811f, 2.18f), (float)frames / 15f);
			}
			else if (frames <= 20)
			{
				toriel.transform.position = Vector3.Lerp(new Vector3(-0.811f, 2.18f), new Vector3(-0.811f, 2.18f), (float)(frames - 15) / 5f);
			}
			if (frames == 16)
			{
				toriel.Play("WalkUp");
			}
			if (frames == 21)
			{
				toriel.GetComponent<SpriteRenderer>().enabled = false;
			}
			if (frames >= 10 && frames <= 20)
			{
				susie.transform.position = Vector3.Lerp(new Vector3(0f, -3.09f), new Vector3(0f, -0.42f), (float)(frames - 10) / 10f);
				if (frames == 10)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.GetComponent<Animator>().Play("walk");
					susie.GetComponent<Animator>().SetFloat("speed", 3f);
				}
				else if (frames == 15)
				{
					StartText(new string[1] { "* Wait!!!\n* Don't you wanna see..." }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_shocked" }, 1);
				}
				else if (frames == 20)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					susie.GetComponent<Animator>().Play("idle");
					susie.GetComponent<Animator>().SetFloat("speed", 1f);
				}
			}
			if (frames == 30)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_wtf");
				StartText(new string[1] { "* WHY THE HELL IS SHE\n  RUNNING AWAY?!?" }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_wtf" }, 0);
				state = 1;
			}
		}
		if (state == 1 && !txt)
		{
			susie.EnableAnimator();
			susie.ChangeDirection(Vector2.down);
			EndCutscene();
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				toriel.Play("WalkRight");
				toriel.SetFloat("speed", 1f);
			}
			if (posID == 5)
			{
				freezeFrames++;
				if (freezeFrames == 20)
				{
					toriel.Play("WalkRight");
					toriel.SetFloat("speed", 1f);
					toriel.GetComponent<SpriteRenderer>().flipX = true;
				}
			}
			if (posID != 5 || (posID == 5 && freezeFrames >= 20))
			{
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, positions[posID], 0.125f);
			}
			if (toriel.transform.position == positions[posID])
			{
				posID++;
				if (posID == 1 || posID == 3)
				{
					toriel.Play("WalkUp");
					toriel.GetComponent<SpriteRenderer>().flipX = false;
				}
				else if (posID == 2 || posID == 4)
				{
					toriel.Play("WalkRight");
					toriel.GetComponent<SpriteRenderer>().flipX = posID == 2;
				}
				else if (posID == 5)
				{
					GameObject.Find("Door").GetComponent<SpriteRenderer>().enabled = false;
					GameObject.Find("WallSwitch").GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("overworld/ruins_objects/spr_ruins_wallswitch")[1];
					PlaySFX("sounds/snd_noise");
					toriel.Play("WalkUp", 0, 0f);
					toriel.SetFloat("speed", 0f);
				}
				else if (posID == 6)
				{
					toriel.Play("WalkDown");
					toriel.GetComponent<SpriteRenderer>().flipX = false;
				}
			}
			if (posID == 7)
			{
				toriel.Play("WalkDown", 0, 0f);
				toriel.SetFloat("speed", 0f);
				StartText(new string[4] { "* The RUINS are full of\n  puzzles.", "* Ancient fusions between\n  diversions and doorkeys.", "* One must solve them\n  to move from room to\n  room.", "* Please adjust yourself\n  to the sight of them." }, new string[4] { "snd_txttor", "snd_txttor", "snd_txttor", "snd_txttor" }, new int[18], new string[4] { "tori_neutral", "tori_neutral", "tori_neutral", "tori_neutral" }, 0);
				state = 3;
				freezeFrames = 0;
				frames = 0;
			}
			if (frames == 7)
			{
				GameObject.Find("StepBL").GetComponent<StepSwitch>().StepOn();
			}
			else if (frames == 20)
			{
				GameObject.Find("StepBR").GetComponent<StepSwitch>().StepOn();
			}
			else if (frames == 38)
			{
				GameObject.Find("StepUR").GetComponent<StepSwitch>().StepOn();
			}
			else if (frames == 51)
			{
				GameObject.Find("StepUL").GetComponent<StepSwitch>().StepOn();
			}
		}
		if (state == 3 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				toriel.Play("WalkUp");
				toriel.SetFloat("speed", 1f);
			}
			if (toriel.transform.position != new Vector3(-0.72f, 2.18f))
			{
				toriel.transform.position = Vector3.MoveTowards(toriel.transform.position, new Vector3(-0.72f, 2.18f), 0.125f);
			}
			else
			{
				toriel.GetComponent<SpriteRenderer>().enabled = false;
				freezeFrames++;
				if (freezeFrames == 10)
				{
					StartText(new string[3] { "* It's all full of\n  puzzles?", "* I wish she was like...\n^05  frantically solving them\n  for us instead.", "* That would be much\n  easier." }, new string[3] { "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[3] { "su_inquisitive", "su_side", "su_smile" }, 0);
					state = 4;
				}
			}
		}
		if (state == 4 && !txt)
		{
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		toriel = GameObject.Find("Toriel").GetComponent<Animator>();
		if ((int)gm.GetFlag(108) == 1)
		{
			GameObject.Find("Door").GetComponent<SpriteRenderer>().enabled = true;
			toriel.transform.position = new Vector3(-0.08f, -0.29f);
			StartText(new string[6] { "* Welcome to your new\n  home,^05 innocent one.", "* God,^05 I'd love to go\n  home.", "* What did you say?", "* Uhh,^05 hey,^05 can I\n  just...", "* Do not interrupt me.", "* Anyways,^05 allow me to\n  educate you in the\n  operation of the RUINS." }, new string[6] { "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txtsus", "snd_txttor", "snd_txttor" }, new int[18], new string[6] { "tori_neutral", "su_side", "tori_mad", "su_shocked", "tori_mad", "tori_neutral" }, 0);
			hardmode = true;
			state = 2;
		}
		else
		{
			PlaySFX("sounds/snd_noise");
			toriel.Play("WalkLeftFaceless");
			toriel.SetFloat("speed", 1f);
			GameObject.Find("StepBL").GetComponent<StepSwitch>().StepOn(sound: false);
			GameObject.Find("StepBR").GetComponent<StepSwitch>().StepOn(sound: false);
			GameObject.Find("StepUL").GetComponent<StepSwitch>().StepOn(sound: false);
			GameObject.Find("StepUR").GetComponent<StepSwitch>().StepOn(sound: false);
			GameObject.Find("WallSwitch").GetComponent<SpriteRenderer>().sprite = Resources.LoadAll<Sprite>("overworld/ruins_objects/spr_ruins_wallswitch")[1];
		}
		base.StartCutscene(par);
		gm.SetFlag(5, 1);
	}
}
