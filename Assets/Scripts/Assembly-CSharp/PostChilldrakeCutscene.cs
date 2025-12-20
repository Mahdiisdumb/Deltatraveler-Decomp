using UnityEngine;

public class PostChilldrakeCutscene : CutsceneBase
{
	private Animator qc;

	private int endState;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				ChangeDirection(kris, Vector2.right);
				ChangeDirection(susie, Vector2.right);
				ChangeDirection(noelle, Vector2.right);
				StartText(new string[4]
				{
					(endState == 2) ? "* Wow.^05\n* I...^05 didn't expect that." : "* Hopefully that's the last\n  one of those Snowdrakes.",
					"* ... Anyway,^05 I finally got\n  the ladder built,^05 so I'm\n  getting outta here.",
					"* Again,^05 thank y'all so much\n  for helping us out!",
					"* It's our pleasure,^05\n  Q.C.!"
				}, new string[4] { "snd_text", "snd_text", "snd_text", "snd_txtnoe" }, new int[1], new string[4] { "", "", "", "no_happy" });
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			if ((bool)txt)
			{
				if (AtLine(4))
				{
					susie.UseHappySprites();
					noelle.UseHappySprites();
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				SetMoveAnim(qc, true);
				ChangeDirection(qc, Vector2.right);
			}
			float x = Mathf.Lerp(38f, 38.518f, (float)frames / 6f);
			float y = Mathf.Lerp(-17.6f, -8.52f, (float)(frames - 6) / 90f);
			qc.transform.position = new Vector3(x, y);
			if (frames == 6)
			{
				PlaySFX("sounds/snd_escaped");
				PlayAnimation(qc, "Climb");
			}
			if (frames == 90)
			{
				ChangeDirection(kris, Vector2.down);
				ChangeDirection(susie, Vector2.up);
				ChangeDirection(noelle, Vector2.up);
				SetSprite(susie, "spr_su_pose");
				StartText(new string[2] { "* And hey,^05 that means\n  we can also get\n  going,^05 too.", "* So...^05 let's get going." }, new string[1] { "snd_txtsus" }, new int[1], new string[2] { "su_confident", "su_smile_side" });
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
					susie.EnableAnimator();
				}
				return;
			}
			frames++;
			if (frames == 1)
			{
				Object.FindObjectOfType<DeepMazeEventHandler>().EndOfEpisode5(false);
			}
			if (!MoveTo(cam, cam.GetClampedPos(), 2f))
			{
				Object.Destroy(qc.gameObject);
				ChangeDirection(kris, Vector2.down);
				RestorePlayerControl();
				gm.PlayMusic("zoneMusic");
				Object.Instantiate(Resources.Load<GameObject>("overworld/npcs/StalkerFlowey"), new Vector3(20f, -1f), Quaternion.identity);
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		qc = Object.FindObjectOfType<QCEnd>().GetComponent<Animator>();
		Object.Destroy(qc.GetComponent<QCEnd>());
		RevokePlayerControl();
		endState = int.Parse(par[0].ToString());
		if ((int)gm.GetFlag(12) == 1)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_ominous");
		}
		Object.Destroy(GameObject.Find("Chilldrake"));
		Object.Instantiate(Resources.Load<GameObject>("overworld/snow_objects/LadderBuilt"));
	}
}
