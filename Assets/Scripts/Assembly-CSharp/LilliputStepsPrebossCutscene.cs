using System;
using UnityEngine;

public class LilliputStepsPrebossCutscene : CutsceneBase
{
	private MoleFriend mole;

	private AudioSource natureSounds;

	private bool turnAround;

	private bool injuredVariant;

	private bool oblitBoss;

	private int advanceFrames;

	private Animator ness;

	private Animator paula;

	private Animator porky;

	private SpriteRenderer flash;

	private Vector3 krisToPos;

	private Vector3 susieToPos;

	private Vector3 noelleToPos;

	private Vector3 moleToPos;

	private Vector3 camToPos;

	private Vector2 look;

	private int soundState;

	private bool paulaDoFunnyAnim;

	private bool fastVersion;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					kris.ChangeDirection(Vector2.down);
					gm.StopMusic(60f);
				}
				if (frames <= 60)
				{
					natureSounds.volume = (float)(60 - frames) / 120f;
					natureSounds.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (float)frames / 120f);
					if (frames == 60)
					{
						gm.PlayMusic("music/mus_lilliput_steps_melody");
					}
				}
				else if (frames >= 165 && frames <= 225)
				{
					natureSounds.GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, (float)(225 - frames) / 120f);
				}
				else if (frames >= 225 && frames <= 285)
				{
					natureSounds.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)(frames - 225) / 120f);
				}
				if (frames >= 330)
				{
					if (frames == 330)
					{
						kris.SetSelfAnimControl(false);
						susie.SetSelfAnimControl(false);
						noelle.SetSelfAnimControl(false);
						if ((bool)mole)
						{
							mole.SetSelfAnimControl(false);
						}
						cam.SetFollowPlayer(false);
						gm.HealAll(99);
						if (injuredVariant)
						{
							gm.SetFlag(102, 0);
							kris.GetComponent<Animator>().SetFloat("speed", 0f);
							kris.GetComponent<Animator>().Play("RemoveBandage", 0, 0f);
						}
						PlaySFX("sounds/snd_spellcast");
					}
					natureSounds.volume = (float)(frames - 330) / 120f;
					natureSounds.GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, (float)(390 - frames) / 120f);
				}
				if (frames == 390)
				{
					if (!fastVersion)
					{
						if (injuredVariant)
						{
							StartText(new string[4] { "* (For some strange reason,^05\n  for just a brief moment...)", "* (You thought you saw your\n  old,^05 red horns.)", "* (The power of this place\n  made you and your friends feel\n  alive.)", "* (You fully recovered from your\n  concussion.)" }, new string[4] { "snd_text", "snd_text", "snd_text", "snd_text" }, new int[5], new string[4] { "", "", "", "" });
						}
						else
						{
							StartText(new string[3] { "* (For some strange reason,^05\n  for just a brief moment...)", "* (You thought you saw your\n  old,^05 red horns.)", "* (The power of this place\n  made you and your friends feel\n  alive.)" }, new string[3] { "snd_text", "snd_text", "snd_text" }, new int[5], new string[3] { "", "", "" });
						}
						state = 1;
					}
					else
					{
						state = (oblitBoss ? 10 : 9);
					}
					frames = 0;
					gm.PlayMusic("music/mus_sanctuary_post_intro");
				}
			}
			else if (txt.GetCurrentStringNum() == 2 && !turnAround)
			{
				turnAround = true;
				kris.ChangeDirection(-kris.GetDirection());
				noelle.UseUnhappySprites();
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				noelle.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				noelle.GetComponent<Animator>().SetBool("isMoving", true);
				if ((bool)mole)
				{
					mole.GetComponent<Animator>().SetBool("isMoving", true);
				}
				noelleToPos = kris.transform.position + new Vector3(0.97f, -0.58f);
				moleToPos = kris.transform.position + new Vector3(0.75f, 0.77f);
				if (kris.transform.position.y <= -4.566f)
				{
					camToPos = kris.transform.position + new Vector3(0.5f, 0f, -10f);
					susieToPos = kris.transform.position + new Vector3(0.99f, 1.02f);
					look = Vector2.left;
				}
				else
				{
					camToPos = kris.transform.position + new Vector3(0f, -0.5f, -10f);
					susieToPos = kris.transform.position + new Vector3(-0.99f, -0.63f);
					look = Vector2.up;
				}
				susie.ChangeDirection(Vector3.MoveTowards(susie.transform.position, susieToPos, 1f) - susie.transform.position);
				noelle.ChangeDirection(Vector3.MoveTowards(noelle.transform.position, noelleToPos, 1f) - noelle.transform.position);
				if ((bool)mole)
				{
					mole.ChangeDirection(Vector3.MoveTowards(mole.transform.position, moleToPos, 1f) - mole.transform.position);
				}
			}
			bool flag = true;
			if (susie.transform.position != susieToPos)
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, susieToPos, 1f / 12f);
				flag = false;
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.ChangeDirection(look);
			}
			if (noelle.transform.position != noelleToPos)
			{
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, noelleToPos, 1f / 12f);
				flag = false;
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				noelle.ChangeDirection(look);
			}
			if (cam.transform.position != camToPos)
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, camToPos, 1f / 24f);
				flag = false;
			}
			if ((bool)mole)
			{
				if (mole.transform.position != moleToPos)
				{
					mole.transform.position = Vector3.MoveTowards(mole.transform.position, moleToPos, 5f / 48f);
					flag = false;
				}
				else
				{
					mole.GetComponent<Animator>().SetBool("isMoving", false);
					mole.ChangeDirection(Vector2.down);
				}
			}
			if (flag)
			{
				kris.GetComponent<Animator>().Play("idle");
				kris.ChangeDirection(-look);
				if (injuredVariant)
				{
					StartText(new string[7] { "* Kris...?", "* You look a lot\n  better.", "* Guess that concussion\n  finally went away.", "* This place seems to\n  actually have some\n  kind of weird power.", "* I feel...^05 really good\n  right now.", "* Hey,^05 let's not forget\n  what we came here\n  to do.", "* O-^05oh!^05\n* Right!" }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[10], new string[7] { "su_side", "su_smirk", "su_smile", "no_confused", "no_relief", "su_annoyed", "no_awe" });
				}
				else
				{
					StartText(new string[9] { "* Kris...?", "* You seem...^05 really\n  confused.", "* I guess you also\n  felt it.", "* This place seems to\n  actually have some\n  kind of weird power.", "* I feel...^05 really good\n  right now.", "* Hey,^05 let's not forget\n  what we came here\n  to do.", "* Yeah,^05 let's not get\n  carried away.", "* You might turn out\n  like that big mole.", "* O-^05oh!^05\n* Sorry!" }, new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtnoe" }, new int[10], new string[9] { "su_side", "su_neutral", "su_smirk", "no_confused", "no_relief", "su_annoyed", "pau_embarrassed", "pau_confident", "no_awe" });
				}
				state = 2;
				frames = 0;
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				kris.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				noelle.GetComponent<Animator>().SetBool("isMoving", true);
				kris.ChangeDirection(Vector2.right);
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
			}
			bool flag2 = true;
			if (kris.transform.position != new Vector3(11.34f, -4.22f))
			{
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(11.34f, -4.22f), 5f / 48f);
				flag2 = false;
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", false);
				kris.ChangeDirection(Vector2.up);
			}
			if (susie.transform.position != new Vector3(14.07f, -4.02f))
			{
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(14.07f, -4.02f), 5f / 48f);
				flag2 = false;
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.ChangeDirection(Vector2.up);
			}
			if (noelle.transform.position != new Vector3(12.67f, -4.21f))
			{
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(12.67f, -4.21f), 5f / 48f);
				flag2 = false;
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				noelle.ChangeDirection(Vector2.up);
			}
			if (cam.transform.position != new Vector3(12.62f, -3.08f, -10f))
			{
				cam.transform.position = Vector3.MoveTowards(cam.transform.position, new Vector3(12.62f, -3.08f, -10f), 1f / 12f);
				flag2 = false;
			}
			if ((bool)mole)
			{
				if (frames == 15)
				{
					flag2 = false;
					mole.GetComponent<Animator>().SetBool("isMoving", true);
					mole.ChangeDirection(Vector2.right);
				}
				else if (frames > 15)
				{
					if (mole.transform.position != new Vector3(10.33f, -5.86f))
					{
						mole.transform.position = Vector3.MoveTowards(mole.transform.position, new Vector3(10.33f, -5.86f), 0.125f);
						flag2 = false;
					}
					else
					{
						mole.GetComponent<Animator>().SetBool("isMoving", false);
						mole.ChangeDirection(Vector2.up);
					}
				}
			}
			if (flag2)
			{
				advanceFrames++;
				if (advanceFrames >= 30)
				{
					frames = 0;
					advanceFrames = 0;
					if (injuredVariant)
					{
						StartText(new string[7] { "* Finally,^05 we found that\n  grey door.", "* Now we can get\n  back to that forest\n  and continue home.", "* ... Let's just try\n  to forget that any\n  of this happened.", "* That it was all\n  just a bad dream.", "* Y'know,^05 we'll never\n  see this place again.", "* ... I guess you're\n  right,^05 Susie.", "* Let's just go home." }, new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe" }, new int[10], new string[7] { "su_confident", "su_smile", "su_dejected", "su_depressed", "su_side", "no_depressed", "no_relief" });
						state = 4;
					}
					else
					{
						StartText(new string[2] { "* Finally,^05 we found that\n  grey door.", "* Now we can get\n  back to that forest\n  and continue home." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[2], new string[2] { "su_confident", "su_smile" });
						state = 3;
					}
				}
			}
		}
		if (state == 3 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.SetMiniPartyMember(0);
				paula.SetFloat("dirX", -1f);
				PlaySFX("sounds/snd_jump");
			}
			paula.transform.position = new Vector3(Mathf.Lerp(11.343f, 10.325f, (float)frames / 10f), -4.325f + 0.416f * Mathf.Cos((float)(frames * 9) * ((float)Math.PI / 180f)));
			if (frames == 10)
			{
				kris.ChangeDirection(Vector2.left);
				susie.ChangeDirection(Vector2.left);
				noelle.ChangeDirection(Vector2.left);
				paula.SetFloat("dirX", 1f);
				StartText(new string[6] { "* Well,^05 that was fun!", "* But now I've gotta\n  go my own way.", "* I've got another\n  adventure to tag\n  along on.", "* Good luck on your\n  way home!", "* Good luck on your\n  own adventure,^05 Paula!", "* Let's get outta here." }, new string[6] { "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtnoe", "snd_txtsus" }, new int[6], new string[6] { "pau_smile", "pau_smile_side", "pau_confident", "pau_happy", "no_happy", "su_smile" });
				state = 4;
				frames = 0;
			}
		}
		if (state == 4)
		{
			if ((bool)txt)
			{
				if (injuredVariant)
				{
					if (txt.GetCurrentStringNum() == 3)
					{
						susie.ChangeDirection(Vector2.left);
						kris.ChangeDirection(Vector2.right);
						noelle.ChangeDirection(Vector2.right);
					}
					else if (txt.GetCurrentStringNum() == 5)
					{
						susie.ChangeDirection(Vector2.up);
					}
					else if (txt.GetCurrentStringNum() == 6)
					{
						noelle.ChangeDirection(Vector2.up);
					}
					else if (txt.GetCurrentStringNum() == 7)
					{
						kris.ChangeDirection(Vector2.up);
					}
				}
				else if (txt.GetCurrentStringNum() == 5)
				{
					susie.UseHappySprites();
					noelle.UseHappySprites();
				}
				else if (txt.GetCurrentStringNum() == 6)
				{
					kris.ChangeDirection(Vector2.up);
					susie.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					gm.StopMusic(30f);
					kris.GetComponent<Animator>().SetBool("isMoving", true);
					susie.GetComponent<Animator>().SetBool("isMoving", true);
					noelle.GetComponent<Animator>().SetBool("isMoving", true);
				}
				if (!oblitBoss)
				{
					if (frames == 30)
					{
						PlaySFX("sounds/snd_heavyswing");
					}
					if (frames == 40)
					{
						gm.PlayGlobalSFX("sounds/snd_damage");
					}
					if (frames >= 30 && frames <= 40)
					{
						float num = (float)(frames - 30) / 10f;
						num *= num;
						porky.transform.position = new Vector3(12.72f, Mathf.Lerp(6.36f, -3.52f, num));
					}
				}
				if (frames < 40)
				{
					kris.transform.position += Vector3.up / 48f;
					susie.transform.position += Vector3.up / 48f;
					noelle.transform.position += Vector3.up / 48f;
				}
				else if (!oblitBoss)
				{
					if (frames == 40)
					{
						natureSounds.Stop();
						kris.DisableAnimator();
						kris.SetSprite("spr_kr_ko");
						susie.DisableAnimator();
						susie.SetSprite("spr_su_ko");
						noelle.DisableAnimator();
						noelle.SetSprite("spr_no_collapsed");
						noelle.GetComponent<SpriteRenderer>().flipX = true;
						if (!injuredVariant)
						{
							paula.enabled = false;
							paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_shocked");
							paula.GetComponent<SpriteRenderer>().flipX = true;
						}
						krisToPos = kris.transform.position;
						susieToPos = susie.transform.position;
						noelleToPos = noelle.transform.position;
						if ((bool)mole)
						{
							mole.ChangeDirection(Vector2.down);
							mole.GetComponent<Animator>().SetBool("isMoving", true);
							mole.GetComponent<Animator>().SetFloat("speed", 2f);
						}
					}
					if ((bool)mole)
					{
						mole.transform.position -= new Vector3(0f, 5f / 24f);
					}
					if (frames <= 45)
					{
						if (!injuredVariant)
						{
							paula.transform.position -= new Vector3(0.125f, 0f);
						}
						cam.transform.position = new Vector3(12.62f, -3.8f, -10f) + new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) * (45 - frames) / 48f;
					}
					float num2 = (float)(frames - 40) / 35f;
					if (num2 <= 1f)
					{
						num2 = Mathf.Sin(num2 * (float)Math.PI * 0.5f);
						kris.transform.position = Vector3.Lerp(krisToPos, new Vector3(8.31f, -5.11f), num2);
						susie.transform.position = Vector3.Lerp(susieToPos, new Vector3(7.54f, -6.96f), num2);
						noelle.transform.position = Vector3.Lerp(noelleToPos, new Vector3(10.73f, -7.46f), num2);
					}
					if (frames == 100)
					{
						gm.PlayMusic("music/mus_gallery");
						state = 5;
						frames = 0;
						StartText(new string[5] { "* Heheheh...^05\n  look who it is!", "* The interdimensional\n  losers!", "* ...^05 What the hell...?", "* YOU???!", "* Don't look so surprised,^05\n  you stupid little\n  weakling." }, new string[5] { "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtsus", "snd_txtpor" }, new int[5], new string[5] { "por_suit_smile", "por_suit_smile", "su_depressed", "su_wtf", "por_suit_smile" });
					}
				}
				else if (oblitBoss)
				{
					if (frames == 40)
					{
						natureSounds.Stop();
						GameObject.Find("Black2").GetComponent<SpriteRenderer>().enabled = true;
						ness.GetComponent<AudioSource>().Play();
						krisToPos = kris.transform.position;
						susieToPos = susie.transform.position;
						noelleToPos = noelle.transform.position;
					}
					if (frames >= 70)
					{
						if (frames == 70)
						{
							PlaySFX("sounds/snd_pkflash");
							Util.GameManager().PlayGlobalSFX("sounds/snd_damage");
							flash.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y);
							flash.enabled = true;
							kris.DisableAnimator();
							kris.SetSprite("spr_kr_sit");
							susie.DisableAnimator();
							susie.SetSprite("spr_su_kneel");
							noelle.DisableAnimator();
							noelle.SetSprite("spr_no_kneel_right");
							ness.transform.position = new Vector3(11.98f, -3.27f);
							paula.transform.position = new Vector3(13.44f, -3.27f);
							ness.GetComponent<SpriteRenderer>().sortingOrder = 102;
							paula.GetComponent<SpriteRenderer>().sortingOrder = 102;
						}
						flash.transform.localScale = new Vector3(Mathf.Lerp(1f, 640f, (float)(frames - 70) / 4f), Mathf.Lerp(1f, 440f + Mathf.Sin((float)(frames * 36) * ((float)Math.PI / 180f)) * 20f, (float)(frames - 70) / 8f));
						Color color = Color.Lerp(Color.black, Color.white, (float)(frames - 100) / 45f);
						kris.GetComponent<SpriteRenderer>().color = color;
						susie.GetComponent<SpriteRenderer>().color = color;
						noelle.GetComponent<SpriteRenderer>().color = color;
						ness.GetComponent<SpriteRenderer>().color = color;
						paula.GetComponent<SpriteRenderer>().color = color;
						flash.color = new Color(1f, 1f, 1f, 1f - (float)(frames - 100) / 45f);
						GameObject.Find("Black2").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f - (float)(frames - 100) / 45f);
						float num3 = (float)(frames - 70) / 35f;
						if (num3 <= 1f)
						{
							num3 = Mathf.Sin(num3 * (float)Math.PI * 0.5f);
							kris.transform.position = Vector3.Lerp(krisToPos, krisToPos - new Vector3(0f, 2f), num3);
							susie.transform.position = Vector3.Lerp(susieToPos, susieToPos - new Vector3(0f, 2f), num3);
							noelle.transform.position = Vector3.Lerp(noelleToPos, noelleToPos - new Vector3(0f, 2f), num3);
						}
						if (frames == 165)
						{
							UnityEngine.Object.Destroy(flash.gameObject);
							StartText(new string[17]
							{
								"* (You dropped the Franklin\n  Badge.)", "* Stop right there,^05\n  murderers!", "* What...^10 are you\n  doing?", "* My name is Ness,^05\n  and I'm going to put\n  an end to this!", "* ?!?!", "* Get away!^05\n* You're gonna get\n  killed!", "* Threats aren't gonna\n  work on me.", "* We're trying to get\n  home,^05 you idiots!", "* This isn't bravery,^05 this\n  is just stupid!!!", "* I won't let you\n  murder more people in\n  more worlds.",
								"* Your path ends here!", "* ...", "* Paula!\n^05* Tell him to get\n  away!!", "* ...", "* Ugh!!!", "* Kris,^05 if what you've\n  been saying is true...", "* For the love of\n  GOD,^05 PLEASE DON'T KILL\n  THEM."
							}, new string[17]
							{
								"", "snd_txtness", "snd_txtsus", "snd_txtness", "snd_txtnoe", "snd_txtnoe", "snd_txtness", "snd_txtsus", "snd_txtsus", "snd_txtness",
								"snd_txtness", "snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtsus", "snd_txtsus", "snd_txtsus"
							}, new int[5] { 1, 0, 0, 0, 0 }, new string[17]
							{
								"", "ness_mad", "su_depressed", "ness_mad", "no_shocked", "no_afraid", "ness_mad", "su_annoyed_sweat", "su_annoyed_sweat", "ness_rage",
								"ness_rage", "su_concerned", "su_worried", "pau_defiant", "su_annoyed_sweat", "su_depressed", "su_panic"
							});
							state = 8;
							frames = 0;
						}
					}
				}
			}
		}
		if (state == 5)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3 && soundState == 0)
				{
					PlaySFX("sounds/snd_wing");
					susie.SetSprite("spr_su_kneel");
					soundState++;
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					soundState = 0;
					porky.enabled = true;
					if ((bool)mole)
					{
						mole.transform.position = new Vector3(8.45f, -10.97f);
						mole.ChangeDirection(Vector2.up);
						mole.GetComponent<Animator>().SetFloat("speed", 1f);
						mole.GetComponent<Animator>().SetBool("isMoving", false);
					}
					if (!injuredVariant)
					{
						paula.enabled = true;
						paula.GetComponent<SpriteRenderer>().flipX = false;
						paula.SetFloat("dirX", -1f);
						paula.SetFloat("speed", 2f);
						paula.SetBool("isMoving", true);
					}
				}
				if (frames == 15)
				{
					kris.SetSprite("spr_kr_sit_injured");
					noelle.UseUnhappySprites();
					noelle.SetSprite("spr_no_kneel_right");
					noelle.GetComponent<SpriteRenderer>().flipX = false;
					PlaySFX("sounds/snd_wing");
				}
				if (frames == 35)
				{
					noelle.SetSprite("spr_no_surprise_up");
					PlaySFX("sounds/snd_noelle_scared");
					noelle.ChangeDirection(Vector2.left);
					noelle.GetComponent<Animator>().SetFloat("speed", 2f);
				}
				if (frames > 45)
				{
					if (noelle.transform.position.x != 5.86f)
					{
						noelle.EnableAnimator();
						noelle.transform.position = new Vector3(Mathf.MoveTowards(noelle.transform.position.x, 5.86f, 0.25f), noelle.transform.position.y);
					}
					else if (noelle.GetComponent<Animator>().enabled)
					{
						noelle.GetComponent<Animator>().SetFloat("speed", 1f);
						noelle.GetComponent<Animator>().SetBool("isMoving", false);
						noelle.DisableAnimator();
						noelle.SetSprite("spr_no_right_shocked_0");
					}
				}
				porky.transform.position = Vector3.Lerp(new Vector3(12.72f, -3.52f), new Vector3(11.69f, -6.03f), (float)frames / 60f);
				cam.transform.position = Vector3.Lerp(new Vector3(12.62f, -3.8f, -10f), new Vector3(10.29f, -6.27f, -10f), (float)frames / 70f);
				if (frames == 66)
				{
					porky.enabled = false;
				}
				if (!injuredVariant)
				{
					if (paula.transform.position.x != 6.16f)
					{
						paula.transform.position = new Vector3(Mathf.MoveTowards(paula.transform.position.x, 6.16f, 1f / 6f), paula.transform.position.y);
					}
					else if (paula.transform.position.y != -5.18f)
					{
						paula.SetFloat("dirX", 0f);
						paula.transform.position = new Vector3(6.16f, Mathf.MoveTowards(paula.transform.position.y, -5.18f, 1f / 6f));
					}
					else if (!paulaDoFunnyAnim)
					{
						paula.SetFloat("dirX", 1f);
						paula.SetBool("isMoving", false);
						paula.Play("DrawWeapon", 0, 0f);
						paulaDoFunnyAnim = true;
					}
				}
				if (frames == 75)
				{
					if (injuredVariant)
					{
						StartText(new string[13]
						{
							"* Y'know,^05 I almost\n  thought you three were\n  the murderers.", "* But you couldn't even\n  kill that big,^05 stupid\n  mole!", "* I'm surprised you losers\n  even survived that\n  thing!", "* I bet IT was responsible\n  for the murders!", "* Then...^05 why the hell\n  are you attacking US???", "* You see,^05 you three\n  jumping through these\n  stupid grey doors...", "* It's causing all sorts\n  of dimensional\n  destruction!", "* And Giygas's plans\n  cannot be disrupted by\n  such ridiculousness!", "* So I've come to kill\n  you!", "* Is that an issue for\n  you or something?",
							"* Is that an issue...?", "* Heh...", "* Well,^05 I've got some\n  news for you."
						}, new string[13]
						{
							"snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtpor",
							"snd_txtsus", "snd_txtsus", "snd_txtsus"
						}, new int[14], new string[13]
						{
							"por_suit_neutral", "por_suit_smile", "por_suit_smile", "por_suit_smile", "su_annoyed_sweat", "por_suit_neutral", "por_suit_neutral", "por_suit_smile", "por_suit_smile", "por_suit_smile",
							"su_depressed", "su_depressed_smile", "su_teeth"
						});
					}
					else
					{
						StartText(new string[14]
						{
							"* What the heck are\n  you even doing??!", "* Get away!", "* Oh,^05 but you don't\n  understand the\n  circumstances.", "* Not only did these\n  three ruin my plans\n  in Happy Happy Village...", "* But they're also the\n  source of the time-\n  space destabilization!", "* What the hell are\n  you even talking\n  about???", "* You see,^05 you three\n  jumping through these\n  stupid grey doors...", "* It's causing all sorts\n  of dimensional\n  destruction!", "* And Giygas's plans\n  cannot be disrupted by\n  such ridiculousness!", "* So I've come to kill\n  you!",
							"* Is that an issue for\n  you or something?", "* Is that an issue...?", "* Heh...", "* Well,^05 I've got some\n  news for you."
						}, new string[14]
						{
							"snd_txtpau", "snd_txtpau", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtsus", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtpor",
							"snd_txtpor", "snd_txtsus", "snd_txtsus", "snd_txtsus"
						}, new int[14], new string[14]
						{
							"pau_mad_sweat", "pau_mad_sweat", "por_suit_neutral", "por_suit_neutral", "por_suit_neutral", "su_annoyed_sweat", "por_suit_neutral", "por_suit_neutral", "por_suit_smile", "por_suit_smile",
							"por_suit_smile", "su_depressed", "su_depressed_smile", "su_teeth"
						});
					}
					state = 6;
					frames = 0;
				}
			}
		}
		if (state == 6 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				if (!injuredVariant)
				{
					gm.SetMiniPartyMember(1);
				}
				PlaySFX("sounds/snd_weaponpull");
				susie.EnableAnimator();
				susie.GetComponent<Animator>().Play("AttackStick", 0, 0f);
				string text = "Attack";
				switch (gm.GetWeapon(0))
				{
				case 8:
					text += "Ring";
					break;
				case 13:
					text += "ToyKnife";
					break;
				case 21:
					text += "Bat";
					break;
				case 31:
					text += "AlBat";
					break;
				default:
					text += "Pencil";
					break;
				}
				kris.EnableAnimator();
				kris.GetComponent<Animator>().Play(text, 0, 0f);
				string text2 = "Attack";
				switch (gm.GetWeapon(2))
				{
				case 8:
					text2 += "Ring";
					break;
				case 13:
					text2 += "ToyKnife";
					break;
				case 21:
					text2 += "Bat";
					break;
				case 31:
					text2 += "AlBat";
					break;
				default:
					text2 += "Pencil";
					break;
				}
				noelle.EnableAnimator();
				noelle.GetComponent<Animator>().Play(text2, 0, 0f);
			}
			if (frames == 30)
			{
				StartText(new string[5] { "* I'll launch you right\n  into space.", "* Oh YEAH???", "* My mech is way more\n  powerful than you\n  three could ever be!", "* You pathetic \"heroes\" of\n  so-called \"justice\"...", "* I'll crush you to\n  MUSH!!!" }, new string[5] { "snd_txtsus", "snd_txtpor", "snd_txtpor", "snd_txtpor", "snd_txtpor" }, new int[14], new string[5] { "su_teeth_eyes", "por_suit_smile", "por_suit_neutral", "por_suit_neutral", "por_suit_smile" });
				state = 7;
				frames = 0;
			}
		}
		if (state == 7 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic();
				PlaySFX("sounds/snd_break2");
			}
			if (frames == 30)
			{
				if ((bool)mole)
				{
					mole.GetComponent<SpriteRenderer>().enabled = false;
				}
				kris.InitiateBattle(52);
				EndCutscene(false);
			}
		}
		if (state == 8)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 5 && !paulaDoFunnyAnim)
				{
					gm.PlayMusic("music/d");
					PlaySFX("sounds/snd_wing");
					susie.SetSprite("spr_su_surprise_up");
					susie.GetComponent<SpriteRenderer>().flipX = true;
					noelle.SetSprite("spr_no_surprise_up");
					paulaDoFunnyAnim = true;
				}
				else if (txt.GetCurrentStringNum() == 8 && susie.GetComponent<SpriteRenderer>().flipX)
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
					susie.SetSprite("spr_su_point_up_0");
				}
				else if (txt.GetCurrentStringNum() == 12 && !susie.GetComponent<Animator>().enabled)
				{
					PlaySFX("sounds/snd_wing");
					kris.EnableAnimator();
					kris.GetComponent<Animator>().SetBool("isMoving", false);
					susie.GetComponent<Animator>().SetBool("isMoving", false);
					susie.EnableAnimator();
				}
				else if (txt.GetCurrentStringNum() == 15 && susie.GetComponent<Animator>().enabled)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wtf");
				}
				else if (txt.GetCurrentStringNum() == 16 && !susie.GetComponent<Animator>().enabled)
				{
					noelle.EnableAnimator();
					noelle.GetComponent<Animator>().SetBool("isMoving", false);
					noelle.ChangeDirection(Vector2.left);
					susie.EnableAnimator();
					susie.ChangeDirection(Vector2.left);
					kris.ChangeDirection(Vector2.right);
				}
				else if (txt.GetCurrentStringNum() == 17)
				{
					noelle.ChangeDirection(Vector2.up);
					susie.ChangeDirection(Vector2.up);
					kris.ChangeDirection(Vector2.up);
				}
			}
			else
			{
				kris.InitiateBattle(53);
				UnityEngine.Object.FindObjectOfType<NessPaulaSouls>().Activate();
				EndCutscene(false);
			}
		}
		if (state == 9)
		{
			frames++;
			if (frames == 1)
			{
				porky.enabled = true;
				if (injuredVariant)
				{
					kris.GetComponent<Animator>().SetFloat("speed", 1.5f);
				}
				else
				{
					kris.GetComponent<Animator>().SetBool("isMoving", true);
				}
				kris.ChangeDirection(Vector2.right);
				susie.ChangeDirection(Vector2.right);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				noelle.ChangeDirection(Vector2.right);
				noelle.GetComponent<Animator>().SetBool("isMoving", true);
				camToPos = cam.transform.position;
				susieToPos = susie.transform.position;
				krisToPos = kris.transform.position;
				noelleToPos = noelle.transform.position;
				if ((bool)mole)
				{
					mole.ChangeDirection(Vector2.right);
					mole.GetComponent<Animator>().SetBool("isMoving", true);
					moleToPos = mole.transform.position;
				}
			}
			if (frames == 2)
			{
				porky.enabled = false;
			}
			if (frames <= 60)
			{
				if (frames == 20 && injuredVariant)
				{
					kris.GetComponent<Animator>().SetBool("isMoving", true);
					kris.GetComponent<Animator>().Play("walk");
				}
				cam.transform.position = Vector3.Lerp(camToPos, new Vector3(10.29f, -6.27f, -10f), (float)frames / 60f);
				kris.transform.position = Vector3.Lerp(krisToPos, new Vector3(8.31f, -5.11f), injuredVariant ? ((float)(frames - 20) / 40f) : ((float)frames / 60f));
				susie.transform.position = Vector3.Lerp(susieToPos, new Vector3(7.54f, -6.96f), (float)frames / 60f);
				noelle.transform.position = Vector3.Lerp(noelleToPos, new Vector3(5.86f, -7.46f), (float)frames / 60f);
				porky.transform.position = Vector3.Lerp(new Vector3(11.69f, 5f), new Vector3(11.69f, -6.03f), (float)(frames - 50) / 10f);
				if ((bool)mole)
				{
					mole.transform.position = Vector3.Lerp(moleToPos, new Vector3(8.45f, -5.5f), (float)frames / 60f);
				}
				if (frames == 50)
				{
					PlaySFX("sounds/snd_heavyswing");
				}
				if (frames == 60)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_surprise_right");
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_surprise");
					kris.GetComponent<Animator>().SetBool("isMoving", false);
					if ((bool)mole)
					{
						mole.ChangeDirection(Vector2.down);
					}
					gm.StopMusic();
					natureSounds.Stop();
					gm.PlayGlobalSFX("sounds/snd_crash");
				}
			}
			else
			{
				if (frames <= 65)
				{
					cam.transform.position = new Vector3(10.29f, -6.27f, -10f) + new Vector3(UnityEngine.Random.Range(-1, 2), UnityEngine.Random.Range(-1, 2)) * (65 - frames) / 48f;
				}
				if ((bool)mole)
				{
					mole.transform.position = Vector3.Lerp(new Vector3(8.45f, -5.5f), new Vector3(8.45f, -10.97f), (float)(frames - 60) / 10f);
				}
				if (frames == 70 && (bool)mole)
				{
					mole.ChangeDirection(Vector2.up);
					mole.GetComponent<Animator>().SetBool("isMoving", false);
				}
				if (frames == 90)
				{
					if ((bool)mole)
					{
						mole.GetComponent<SpriteRenderer>().enabled = false;
					}
					kris.InitiateBattle(52);
					EndCutscene(false);
				}
			}
		}
		if (state != 10)
		{
			return;
		}
		frames++;
		if (frames == 1)
		{
			kris.GetComponent<Animator>().SetFloat("speed", 1.5f);
			kris.ChangeDirection(Vector2.right);
			susie.ChangeDirection(Vector2.right);
			susie.GetComponent<Animator>().SetBool("isMoving", true);
			noelle.ChangeDirection(Vector2.right);
			noelle.GetComponent<Animator>().SetBool("isMoving", true);
			camToPos = cam.transform.position;
			susieToPos = susie.transform.position;
			krisToPos = kris.transform.position;
			noelleToPos = noelle.transform.position;
		}
		if (frames <= 70)
		{
			if (frames == 20 && injuredVariant)
			{
				kris.GetComponent<Animator>().SetBool("isMoving", true);
				kris.GetComponent<Animator>().Play("walk");
			}
			cam.transform.position = Vector3.Lerp(camToPos, new Vector3(12.62f, -3.08f, -10f), (float)frames / 70f);
			kris.transform.position = Vector3.Lerp(krisToPos, new Vector3(11.34f, -4.22f), injuredVariant ? ((float)(frames - 20) / 50f) : ((float)frames / 70f));
			susie.transform.position = Vector3.Lerp(susieToPos, new Vector3(14.07f, -4.02f), (float)frames / 70f);
			noelle.transform.position = Vector3.Lerp(noelleToPos, new Vector3(12.67f, -4.21f), (float)frames / 70f);
			return;
		}
		int num4 = frames - 30;
		if (num4 == 41)
		{
			gm.StopMusic();
			natureSounds.Stop();
			GameObject.Find("Black2").GetComponent<SpriteRenderer>().enabled = true;
			ness.GetComponent<AudioSource>().Play();
			krisToPos = kris.transform.position;
			susieToPos = susie.transform.position;
			noelleToPos = noelle.transform.position;
		}
		if (num4 >= 70)
		{
			if (num4 == 70)
			{
				PlaySFX("sounds/snd_pkflash");
				Util.GameManager().PlayGlobalSFX("sounds/snd_damage");
				flash.transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y);
				flash.enabled = true;
				kris.DisableAnimator();
				kris.SetSprite("spr_kr_sit");
				susie.DisableAnimator();
				susie.SetSprite("spr_su_kneel");
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_kneel_right");
				ness.transform.position = new Vector3(11.98f, -3.27f);
				paula.transform.position = new Vector3(13.44f, -3.27f);
				ness.GetComponent<SpriteRenderer>().sortingOrder = 102;
				paula.GetComponent<SpriteRenderer>().sortingOrder = 102;
			}
			flash.transform.localScale = new Vector3(Mathf.Lerp(1f, 640f, (float)(num4 - 70) / 4f), Mathf.Lerp(1f, 440f + Mathf.Sin((float)(num4 * 36) * ((float)Math.PI / 180f)) * 20f, (float)(num4 - 70) / 8f));
			Color color2 = Color.Lerp(Color.black, Color.white, (float)(num4 - 100) / 45f);
			kris.GetComponent<SpriteRenderer>().color = color2;
			susie.GetComponent<SpriteRenderer>().color = color2;
			noelle.GetComponent<SpriteRenderer>().color = color2;
			ness.GetComponent<SpriteRenderer>().color = color2;
			paula.GetComponent<SpriteRenderer>().color = color2;
			flash.color = new Color(1f, 1f, 1f, 1f - (float)(num4 - 100) / 45f);
			GameObject.Find("Black2").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, 1f - (float)(num4 - 100) / 45f);
			float num5 = (float)(num4 - 70) / 35f;
			if (num5 <= 1f)
			{
				num5 = Mathf.Sin(num5 * (float)Math.PI * 0.5f);
				kris.transform.position = Vector3.Lerp(krisToPos, krisToPos - new Vector3(0f, 1.1666666f), num5);
				susie.transform.position = Vector3.Lerp(susieToPos, susieToPos - new Vector3(0f, 1.1666666f), num5);
				noelle.transform.position = Vector3.Lerp(noelleToPos, noelleToPos - new Vector3(0f, 1.1666666f), num5);
			}
			if (num4 == 165)
			{
				UnityEngine.Object.Destroy(flash.gameObject);
				kris.InitiateBattle(53);
				UnityEngine.Object.FindObjectOfType<NessPaulaSouls>().Activate();
				EndCutscene(false);
			}
		}
	}

	private void LateUpdate()
	{
		if ((state == 4 && oblitBoss && frames >= 70) || (state == 10 && frames >= 100))
		{
			kris.GetComponent<SpriteRenderer>().sortingOrder += 100;
			susie.GetComponent<SpriteRenderer>().sortingOrder += 100;
			noelle.GetComponent<SpriteRenderer>().sortingOrder += 100;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetCheckpoint(71);
		if ((bool)UnityEngine.Object.FindObjectOfType<MoleFriend>())
		{
			mole = UnityEngine.Object.FindObjectOfType<MoleFriend>();
		}
		ness = GameObject.Find("Ness").GetComponent<Animator>();
		paula = GameObject.Find("Paula").GetComponent<Animator>();
		porky = GameObject.Find("Porky").GetComponent<Animator>();
		natureSounds = GameObject.Find("NatureSounds").GetComponent<AudioSource>();
		susie.UseUnhappySprites();
		flash = GameObject.Find("Flash").GetComponent<SpriteRenderer>();
		if ((int)gm.GetSessionFlag(1) == 1 || gm.IsTestMode())
		{
			fastVersion = true;
		}
		else
		{
			gm.SetSessionFlag(1, 1);
		}
		if (fastVersion)
		{
			noelle.UseUnhappySprites();
		}
		injuredVariant = (int)gm.GetFlag(102) == 1;
		oblitBoss = (int)gm.GetFlag(13) >= 6;
		if (injuredVariant)
		{
			gm.SetFlag(0, "neutral");
		}
		if (!oblitBoss)
		{
			paula.enabled = true;
		}
		else
		{
			gm.SetMiniPartyMember(0);
		}
		if (!fastVersion)
		{
			StartText(new string[5] { "* ... Hold on.", "* Susie?^05\n* What's wrong?", "* Does anyone else hear\n  that?", "* It sounds like...^05\n  music...", "* ... Is it coming from\n  these steps...?" }, new string[5] { "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[5], new string[5] { "su_neutral", "no_confused", "su_side", "su_side_sweat", "no_thinking" });
		}
	}
}
