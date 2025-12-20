using System.Collections.Generic;
using UnityEngine;

public class MazeChaseStartCutscene : CutsceneBase
{
	private int activate;

	private OverworldBoneBullet[] bullets = new OverworldBoneBullet[3];

	private Vector3[] positions = new Vector3[3]
	{
		new Vector3(0.53f, 0.25f),
		new Vector3(2.44f, 2.13f),
		new Vector3(4.53f, 0.78f)
	};

	private GameObject prefab;

	private void Update()
	{
		if (state == 0 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic();
			}
			if (frames == 30)
			{
				ChangeDirection(kris, Vector2.right);
				ChangeDirection(susie, Vector2.right);
				ChangeDirection(noelle, Vector2.right);
				susie.UseUnhappySprites();
				noelle.UseUnhappySprites();
			}
			for (int i = 0; i < 3; i++)
			{
				if ((bool)bullets[i])
				{
					bullets[i].transform.position = Vector3.Lerp(bullets[i].transform.position, positions[i], 0.2f);
				}
			}
			if (frames % 45 == 1 && frames / 45 < 3)
			{
				bullets[frames / 45] = Object.Instantiate(prefab, new Vector3(positions[frames / 45].x, -6.09f), Quaternion.identity).GetComponent<OverworldBoneBullet>();
				bullets[frames / 45].transform.GetChild(0).GetComponent<SpriteRenderer>().color = Color.white;
				PlaySFX("sounds/snd_spearappear");
			}
			if (frames > 45)
			{
				if (MoveTo(susie, (kris.transform.position.y < 1.213f) ? new Vector3(-4.641f, 1.552f) : new Vector3(-5.54f, 0.728f), 4f))
				{
					SetMoveAnim(susie, isMoving: true);
				}
				else
				{
					SetMoveAnim(susie, isMoving: false);
				}
				if (MoveTo(noelle, (kris.transform.position.y > 0.18f) ? new Vector3(-4.63f, 0.129f) : new Vector3(-5.54f, 0.728f), 4f))
				{
					SetMoveAnim(susie, isMoving: true);
				}
				else
				{
					SetMoveAnim(susie, isMoving: false);
				}
			}
			if (frames == 150)
			{
				state = 1;
				List<string> list = new List<string> { "* What the hell is going\n  on out there?" };
				List<string> list2 = new List<string> { "snd_txtsus" };
				List<string> list3 = new List<string> { "su_side" };
				if ((int)gm.GetFlag(87) >= 8)
				{
					list.AddRange(new string[3] { "* I have no clue.", "* Do you think it's\n  related to Sans or\n  his brother...?", "* WAIT A MINUTE." });
					list2.AddRange(new string[3] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus" });
					list3.AddRange(new string[3] { "no_thinking", "no_curious", "su_wideeye" });
					activate = 6;
				}
				else
				{
					list.Add("* Is that...???");
					list2.Add("snd_txtnoe");
					list3.Add("no_shocked");
					activate = 4;
				}
				list.AddRange(new string[2] { "* KRIS.", "* START RUNNING!!!" });
				list2.AddRange(new string[1] { "snd_txtsus" });
				list3.AddRange(new string[2] { "su_sus_forward", "su_wtf" });
				StartText(list.ToArray(), list2.ToArray(), new int[1], list3.ToArray());
			}
		}
		else
		{
			if (state != 1)
			{
				return;
			}
			if ((bool)txt)
			{
				if (AtLine(activate - 1))
				{
					Vector2 vector = susie.transform.position - kris.transform.position;
					ChangeDirection(direction: (!(Mathf.Abs(vector.x) > Mathf.Abs(vector.y))) ? ((vector.y > 0f) ? Vector2.up : Vector2.down) : ((vector.x > 0f) ? Vector2.right : Vector2.left), obj: kris);
					ChangeDirection(susie, kris.transform.position - susie.transform.position);
					ChangeDirection(noelle, kris.transform.position - noelle.transform.position);
				}
				else if (AtLine(activate))
				{
					OverworldBoneBullet[] array = bullets;
					for (int j = 0; j < array.Length; j++)
					{
						array[j].StartSpinning(modifyColor: false);
					}
					Util.FindObjectOfType<ActionBulletHandler>().enabled = true;
					kris.SetCollision(onoff: true);
				}
			}
			else
			{
				for (int k = 0; k < 3; k++)
				{
					bullets[k].StartMoving(k == 0);
				}
				RestorePlayerControl();
				ChangeDirection(kris, Vector2.down);
				gm.PlayMusic("music/mus_creepychase");
				Util.FindObjectOfType<DeepMazeEventHandler>().StartChase();
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		StartText(new string[2] { "* I think this might\n  be the last one.", "* Let's go see Q.C. and\n  see if we need to\n  find any more." }, new string[1] { "snd_txtnoe" }, new int[1], new string[2] { "no_curious", "no_neutral" });
		RevokePlayerControl();
		prefab = Resources.Load<GameObject>("overworld/bullets/OverworldBoneBullet");
	}
}
