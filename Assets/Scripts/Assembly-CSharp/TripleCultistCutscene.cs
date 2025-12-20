using UnityEngine;

public class TripleCultistCutscene : CutsceneBase
{
	private InteractWanderingNPC[] cultists = new InteractWanderingNPC[3];

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
				cultists[0].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = false;
				state = 1;
				frames = 0;
				StartText(new string[3]
				{
					"* It was you!^05\n* You ruined our meditation\n  session!",
					"* Now you'll pay for sure!",
					((int)gm.GetFlag(13) >= 5) ? "* Can you guys like...^05\n  STOP???" : "* ...^05 Whatever."
				}, new string[3] { "snd_text", "snd_text", "snd_txtsus" }, new int[14], new string[3]
				{
					"",
					"",
					((int)gm.GetFlag(13) >= 5) ? "su_pissed" : "su_annoyed"
				});
			}
		}
		if (state != 1 || (bool)txt)
		{
			return;
		}
		frames++;
		for (int i = 0; i < 3; i++)
		{
			cultists[i].GetComponent<Animator>().SetBool("isMoving", value: true);
			float num = 4 + i * 4;
			if (i == 2)
			{
				num += 12f;
			}
			cultists[i].transform.position = Vector3.MoveTowards(cultists[i].transform.position, kris.transform.position, num / 48f);
		}
		if (frames == 30)
		{
			kris.InitiateBattle(27);
			EndCutscene(enablePlayerMovement: false);
		}
	}

	public override void StartCutscene(params object[] par)
	{
		if ((int)gm.GetFlag(87) >= 5)
		{
			base.StartCutscene(par);
			for (int i = 0; i < 3; i++)
			{
				cultists[i] = GameObject.Find("CultistNPC-" + i).GetComponent<InteractWanderingNPC>();
				cultists[i].StopMoving();
				cultists[i].ChangeDirection(Vector2.right);
			}
			cultists[0].transform.GetChild(0).GetComponent<SpriteRenderer>().enabled = true;
			PlaySFX("sounds/snd_encounter");
		}
		else
		{
			Object.Destroy(base.gameObject);
		}
	}
}
