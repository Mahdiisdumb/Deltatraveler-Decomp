using UnityEngine;

public class FoundKrisKnifeCutscene : CutsceneBase
{
	private bool oblit;

	private bool playItemSound;

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
				kris.SetSelfAnimControl(false);
				susie.SetSelfAnimControl(false);
				kris.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetFloat("speed", 1.5f);
				cam.SetFollowPlayer(false);
			}
			if (susie.transform.position != new Vector3(0f, 0.54f) && frames <= 76)
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(0f, 0.54f), 0.125f);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", false);
			}
			if (kris.transform.position != new Vector3(-1.05f, 0.19f))
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(-1.05f, 0.19f), 1f / 12f);
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", false);
			}
			if (cam.transform.position != new Vector3(0f, 0f, -10f))
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(0f, 0f, -10f), 1f / 24f);
			}
			if (frames > 70 && frames <= 90)
			{
				float t = Mathf.Abs((float)(frames - 80) / 10f);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.transform.position = Vector3.Lerp(new Vector3(0f, 0.3f), new Vector3(0f, 0.54f), t);
				if (frames == 80)
				{
					GameObject.Find("Knife").GetComponent<SpriteRenderer>().enabled = false;
				}
			}
			if (frames == 90)
			{
				kris.ChangeDirection(Vector2.right);
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.DisableAnimator();
				susie.SetSprite("spr_su_knife");
			}
			if (frames == 110)
			{
				bool flag = gm.NumItemFreeSpace() == 0;
				if (!oblit)
				{
					if (!flag)
					{
						gm.AddItem(26);
					}
					else
					{
						gm.ForceWeapon(0, 26);
					}
				}
				StartText(new string[7]
				{
					"* It's...^05 a knife.",
					"* This seems like the\n  kind of thing that\n  Kris would keep hidden.",
					"* Maybe something happened\n  to them!",
					"* We didn't see them\n  on our way here...\n* Maybe they're ahead?",
					"* ... I don't have much\n  use for this,^05 so I\n  guess you can use it.",
					flag ? "* (You equipped Kris's Knife.)" : "* (You got Kris's Knife.)",
					"* Quick,^05 let's hurry!"
				}, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_text", "snd_txtsus" }, new int[18], new string[7] { "su_surprised", "su_side", "su_concerned", "su_smirk_sweat", "su_neutral", "", "su_sus" }, 0);
				state = 1;
				frames = 0;
			}
		}
		if (state == 1)
		{
			if ((bool)txt)
			{
				if (oblit && txt.GetCurrentStringNum() >= 2)
				{
					frames++;
					if (frames == 30 || txt.GetCurrentStringNum() > 2)
					{
						Object.Destroy(txt);
						susie.SetSprite("spr_su_knife_wideeye");
						state = 2;
						frames = 0;
						PlaySFX("sounds/snd_item");
						bool flag2 = gm.NumItemFreeSpace() == 0;
						if (!flag2)
						{
							gm.AddItem(27);
						}
						else
						{
							gm.ForceWeapon(0, 27);
						}
						StartText(new string[6]
						{
							flag2 ? "* (You took the knife from \n Susie and equipped it.)" : "* (You took the knife from\n  Susie.)",
							"* ...",
							"* The guts that you have\n  to just steal a weapon\n  from a ferocious beast.",
							"* You are actually\n  psychotic.",
							"* Knock it off before\n  I knock the life out\n  of you.",
							"* Got it?"
						}, new string[6] { "snd_text", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[6] { "", "su_wideeye", "su_inquisitive", "su_pissed", "su_annoyed", "su_annoyed" }, 0);
					}
				}
				else if (!oblit)
				{
					if (txt.GetCurrentStringNum() == 3)
					{
						susie.SetSprite("spr_su_freaked");
					}
					if (txt.GetCurrentStringNum() == 4)
					{
						susie.EnableAnimator();
						susie.ChangeDirection(Vector2.up);
					}
					if (txt.GetCurrentStringNum() == 5)
					{
						susie.ChangeDirection(Vector2.left);
					}
					if (txt.GetCurrentStringNum() == 6 && !playItemSound)
					{
						playItemSound = true;
						PlaySFX("sounds/snd_item");
					}
				}
			}
			else if (cam.transform.position != cam.GetClampedPos())
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 1f / 24f);
			}
			else
			{
				cam.SetFollowPlayer(true);
				kris.ChangeDirection(Vector2.down);
				kris.SetSelfAnimControl(true);
				susie.SetSelfAnimControl(true);
				EndCutscene();
			}
		}
		if (state != 2)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 3)
			{
				susie.SetSprite("spr_su_knife_sus");
			}
			if (txt.GetCurrentStringNum() == 4)
			{
				susie.EnableAnimator();
				susie.ChangeDirection(Vector2.up);
			}
			if (txt.GetCurrentStringNum() == 6)
			{
				susie.ChangeDirection(Vector2.left);
			}
		}
		else if (cam.transform.position != cam.GetClampedPos())
		{
			cam.transform.position = Vector3.MoveTowards(cam.transform.position, cam.GetClampedPos(), 1f / 24f);
		}
		else
		{
			cam.SetFollowPlayer(true);
			kris.ChangeDirection(Vector2.down);
			kris.SetSelfAnimControl(true);
			susie.SetSelfAnimControl(true);
			EndCutscene();
		}
	}

	public override void StartCutscene(params object[] par)
	{
		oblit = (int)gm.GetFlag(13) >= 3;
		if ((int)gm.GetFlag(108) == 0 || (int)gm.GetFlag(53) == 0 || (int)gm.GetFlag(122) == 1)
		{
			Object.Destroy(base.gameObject);
			return;
		}
		base.StartCutscene(par);
		gm.SetFlag(122, 1);
		susie.UseUnhappySprites();
		StartText(new string[1] { "* Hey,^05 hey,^05 wait a\n  second." }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[18], new string[1] { "su_surprised" }, 0);
	}
}
