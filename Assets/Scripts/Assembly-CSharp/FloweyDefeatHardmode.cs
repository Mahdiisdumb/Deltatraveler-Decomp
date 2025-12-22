using System.Collections.Generic;
using UnityEngine;

public class FloweyDefeatHardmode : CutsceneBase
{
	private int endState;

	private bool geno;

	private int lastDiag;

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames == 30)
			{
				List<string> collection = new List<string> { "* ...", "* Oh yeah,^05 Kris!" };
				List<string> collection2 = new List<string> { "su_side", "su_shocked" };
				List<string> collection3;
				List<string> collection4;
				if (endState == 2)
				{
					if (geno)
					{
						collection3 = new List<string> { "* So...^10 Frisk,^05 huh.", "* I guess something about\n  that flower snapped\n  you out of it." };
						collection4 = new List<string> { "su_side", "su_smirk" };
					}
					else
					{
						collection3 = new List<string> { "* I guess...^10 that settles\n  that for now." };
						collection4 = new List<string> { "su_side" };
					}
					geno = false;
				}
				else if (geno)
				{
					collection3 = new List<string> { "* Yeesh.", "* That's...^10 really damn\n  brutal.", "* ...^10 Frisk,^05 huh.", "* Did you...^10 know that\n  flower or something?" };
					collection4 = new List<string> { "su_side_sweat", "su_side_sweat", "su_neutral", "su_neutral" };
				}
				else
				{
					collection3 = new List<string> { "* Yeesh.", "* That's...^10 really damn\n  brutal." };
					collection4 = new List<string> { "su_side_sweat", "su_side_sweat" };
				}
				List<string> list = new List<string>();
				list.AddRange(collection3);
				list.AddRange(collection);
				List<string> list2 = new List<string>();
				list2.AddRange(collection4);
				list2.AddRange(collection2);
				StartText(list.ToArray(), new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], list2.ToArray());
				lastDiag = list.Count;
				state = 1;
				frames = 0;
			}
		}
		if (state == 1)
		{
			if (!txt)
			{
				if (susie.transform.position.y != 0.85f)
				{
					susie.EnableAnimator();
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.GetComponent<Animator>().SetFloat("speed", 2f);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(1f, 0.85f), 5f / 24f);
				}
				else if (susie.transform.position.x != 2.76f)
				{
					kris.ChangeDirection(Vector2.up);
					susie.ChangeDirection(Vector2.right);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(2.76f, 0.85f), 5f / 24f);
				}
				else
				{
					frames++;
					if (frames == 1)
					{
						susie.GetComponent<Animator>().SetBool("isMoving", value: false);
						susie.GetComponent<Animator>().SetFloat("speed", 1f);
						susie.DisableAnimator();
						susie.SetSprite("spr_su_kneel_front");
					}
					if (frames == 20)
					{
						StartText(new string[4] { "* Kris?^10\n* Are you okay?", "* ...", "* Thank GOD,^05 their heart\n  is still beating...", "* I guess that flower\n  knocked them out\n  cold." }, new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[4] { "su_concerned", "su_wideeye", "su_relieved", "su_sus" });
						state = 2;
						frames = 0;
					}
				}
			}
			else if (txt.GetCurrentStringNum() == lastDiag)
			{
				lastDiag = 0;
				if (!geno)
				{
					kris.ChangeDirection(Vector2.right);
				}
				susie.DisableAnimator();
				susie.SetSprite("spr_su_freaked");
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				susie.ChangeDirection(Vector2.down);
				susie.UseHappySprites();
				susie.SetCustomSpritesetPrefix("kr");
				PlaySFX("sounds/snd_wing");
				susie.EnableAnimator();
				GameObject.Find("Kris").GetComponent<SpriteRenderer>().enabled = false;
			}
			if (frames > 15)
			{
				if (susie.transform.position != new Vector3(0f, -0.65f))
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(0f, -0.65f), 1f / 12f);
				}
				else
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					if (geno)
					{
						StartText(new string[2] { "* ...^05 Can you quit\n  looking at me like\n  that?", "* Let's just go." }, new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_annoyed", "su_annoyed" });
					}
					else
					{
						StartText(new string[1] { "* Let's get outta here." }, new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[1] { "su_smile_sweat" });
					}
					state = 3;
				}
			}
		}
		if (state == 3 && !txt)
		{
			cam.SetFollowPlayer(follow: true);
			kris.ChangeDirection(Vector2.down);
			kris.SetSelfAnimControl(setAnimControl: true);
			susie.SetSelfAnimControl(setAnimControl: true);
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		endState = int.Parse(par[0].ToString());
		Object.Destroy(GameObject.Find("BigFlower"));
		Object.Destroy(GameObject.Find("FloweyVine"));
		if (endState == 1)
		{
			GameObject.Find("DeadFlowey").transform.position = new Vector3(0f, 0.172f);
		}
		GameObject.Find("LoadingZone").GetComponent<LoadingZone>().SetForceActivationTrigger(forceActivationTrigger: true);
		geno = (int)gm.GetFlag(13) == 3;
		if (endState == 2 && geno)
		{
			WeirdChecker.Abort(gm);
		}
		susie.EnableAnimator();
		kris.SetSelfAnimControl(setAnimControl: false);
		susie.SetSelfAnimControl(setAnimControl: false);
		kris.GetComponent<Animator>().SetBool("isMoving", value: false);
		susie.GetComponent<Animator>().SetBool("isMoving", value: false);
		kris.GetComponent<Animator>().Play("idle");
		susie.GetComponent<Animator>().Play("idle");
		kris.ChangeDirection(Vector2.up);
		susie.ChangeDirection(Vector2.up);
		kris.transform.position = new Vector3(-1f, -1.9f);
		susie.transform.position = new Vector3(1f, -1.53f);
		GameObject.Find("Kris").transform.position = new Vector3(2.78f, 0.52f);
		susie.UseUnhappySprites();
	}
}
