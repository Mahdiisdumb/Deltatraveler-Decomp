using System;
using UnityEngine;

public class UndereventTrailerIntro : CutsceneBase
{
	private void Update()
	{
		if (state == 0)
		{
			if (kris.transform.position.x <= 10.5f)
			{
				kris.transform.position += new Vector3(0.125f, 0f);
				susie.transform.position += new Vector3(0.125f, 0f);
				noelle.transform.position += new Vector3(0.125f, 0f);
				return;
			}
			frames++;
			if (frames == 1)
			{
				kris.DisableAnimator();
				susie.DisableAnimator();
				noelle.DisableAnimator();
				kris.SetSprite("spr_kr_pose");
				susie.SetSprite("spr_su_pose");
				noelle.SetSprite("spr_no_pose");
			}
			if (frames == 25)
			{
				kris.SetSprite("spr_kr_up_run_0");
				susie.SetSprite("spr_su_up_run_0");
				noelle.SetSprite("spr_no_up_0");
			}
			if (frames == 30)
			{
				kris.EnableAnimator();
				kris.GetComponent<Animator>().Play("Fall");
				susie.EnableAnimator();
				susie.GetComponent<Animator>().Play("FallBack");
				noelle.EnableAnimator();
				noelle.GetComponent<Animator>().Play("Fall");
				state = 1;
				frames = 0;
			}
		}
		else if (state == 1)
		{
			frames++;
			float num = (float)frames / 15f;
			if (num < 1f)
			{
				num = Mathf.Sin(num * (float)Math.PI * 0.5f);
			}
			float num2 = (float)frames / 60f;
			if (num2 < 1f)
			{
				num2 *= num2 * num2;
			}
			noelle.transform.position = new Vector3(Mathf.Lerp(4.5f, -1f, num2), Mathf.Lerp(0f, 1f, num) + Mathf.Lerp(0f, 3f, num2));
			kris.transform.position = new Vector3(Mathf.Lerp(10.5f, 7.5f, num2), Mathf.Lerp(0f, 1f, num) + Mathf.Lerp(0f, -1f, num2));
			susie.transform.position = new Vector3(7.5f, Mathf.Lerp(0f, 1f, num) + Mathf.Lerp(0f, -0.25f, num2));
			if (frames == 15)
			{
				kris.GetComponent<Animator>().Play("FallTurn");
				susie.GetComponent<Animator>().Play("FallTurn");
			}
			GameObject.Find("MAP").transform.position = new Vector3(0f, Mathf.Lerp(-10f, 0f, (float)(frames - 50) / 10f));
			if (frames == 60)
			{
				kris.SetSelfAnimControl(true);
				susie.SetSelfAnimControl(true);
				kris.ChangeDirection(Vector2.down);
				susie.ChangeDirection(Vector2.down);
				kris.GetComponent<Animator>().Play("walk");
				susie.GetComponent<Animator>().Play("walk");
				cam.SetFollowPlayer(true);
				gm.SetPartyMembers(true, false);
				EndCutscene();
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		kris.transform.position = new Vector3(0f, 0f);
		susie.transform.position = new Vector3(-3f, 0f) + susie.GetPositionOffset();
		noelle.transform.position = new Vector3(-6f, 0f) + noelle.GetPositionOffset();
		kris.SetSelfAnimControl(false);
		susie.SetSelfAnimControl(false);
		noelle.SetSelfAnimControl(false);
		kris.GetComponent<Animator>().SetBool("isMoving", true);
		kris.ChangeDirection(Vector2.right);
		susie.GetComponent<Animator>().SetBool("isMoving", true);
		susie.ChangeDirection(Vector2.right);
		noelle.GetComponent<Animator>().SetBool("isMoving", true);
		noelle.ChangeDirection(Vector2.right);
		GameObject.Find("MAP").transform.position = new Vector3(0f, -10f);
		cam.SetFollowPlayer(false);
		cam.transform.position = new Vector3(7.5f, 0f, -10f);
	}
}
