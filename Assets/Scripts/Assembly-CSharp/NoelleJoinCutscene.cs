using UnityEngine;
using UnityEngine.Tilemaps;

public class NoelleJoinCutscene : CutsceneBase
{
	private bool musicPlayed;

	private bool hardmode;

	private bool geno;

	private bool knife;

	private Animator krisNPC;

	private Animator goner;

	private int krisAnimState;

	private SpriteRenderer fakeSOUL;

	private SpriteRenderer krisSOUL;

	private Vector3 soulPos;

	private Vector3 krisSoulPos;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			frames++;
			if (frames < 45)
			{
				kris.transform.position += new Vector3(1f / 24f, 0f);
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (frames < 25)
			{
				susie.transform.position += new Vector3(1f / 24f, 0f);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (frames == 75)
			{
				kris.ChangeDirection(Vector2.left);
				StartText(new string[3]
				{
					hardmode ? "* ..." : "* Hey Kris...",
					"* There's a person on\n  the ground over there.",
					hardmode ? "* It kinda looks like..." : "* Is it just me,^05 or\n  does that look like..."
				}, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[15], new string[3] { "su_side_sweat", "su_side_sweat", "su_smirk_sweat" }, 0);
				cam.SetFollowPlayer(follow: false);
				frames = 0;
				state = 1;
			}
		}
		if (state == 1 && !txt)
		{
			frames++;
			cam.transform.position = Vector3.Lerp(Vector3.zero, new Vector3(5f, 0f), (float)frames / 60f);
			float y = (hardmode ? (-2.19f) : (-1.97f));
			if (kris.transform.position != new Vector3(4.04f, y))
			{
				kris.ChangeDirection(Vector2.right);
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(4.04f, y), 0.125f);
				kris.GetComponent<Animator>().SetBool("isMoving", value: true);
			}
			else
			{
				kris.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (susie.transform.position != new Vector3(5.42f, -1.87f))
			{
				susie.ChangeDirection(Vector2.right);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(5.42f, -1.87f), 0.125f);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			MonoBehaviour.print(frames);
			if (frames == 120)
			{
				PlaySFX("sounds/snd_whip_hard");
				susie.DisableAnimator();
				susie.SetSprite(hardmode ? "spr_su_wtf_kr" : "spr_su_wtf");
				susie.GetComponent<SpriteRenderer>().flipX = true;
				StartText(new string[1] { "* WHAT THE HELL IS\n  NOELLE DOING HERE???" }, new string[4] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[15], new string[1] { "su_wtf" }, 0);
				state = 2;
				frames = 0;
			}
		}
		if (state == 2 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				noelle.SetSprite("spr_no_collapsed_wake");
				PlaySFX("sounds/snd_wing", 0.8f);
			}
			if (frames <= 3)
			{
				int num = ((frames % 2 == 0) ? 1 : (-1));
				int num2 = 3 - frames;
				noelle.transform.position = new Vector3(7.65f, -1.89f) + new Vector3((float)(num2 * num) / 24f, 0f);
			}
			if (frames == 7)
			{
				StartText(new string[2] { "* W-^05w-^05w-^05huh???", "* Whuh...?" }, new string[4] { "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[15], new string[2] { "no_speechless", "su_surprised" }, 0);
				frames = 0;
				state = 3;
			}
		}
		if (state == 3)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_wing", 0.8f);
				}
				if (frames <= 3)
				{
					int num3 = ((frames % 2 == 0) ? 1 : (-1));
					int num4 = 3 - frames;
					noelle.transform.position = new Vector3(7.65f, -1.89f) + new Vector3((float)(num4 * num3) / 24f, 0f);
				}
				if (frames == 20)
				{
					noelle.SetSprite("spr_no_surprise");
					PlaySFX("sounds/snd_wing", 0.9f);
				}
				if (frames >= 20 && frames <= 23)
				{
					int num5 = ((frames % 2 == 0) ? 1 : (-1));
					int num6 = 23 - frames;
					noelle.transform.position = new Vector3(7.65f, -1.89f) + new Vector3((float)(num6 * num5) / 24f, 0f);
				}
				if (frames == 30)
				{
					StartText(new string[5]
					{
						"* W-^05where am I???",
						hardmode ? "* SUSIE????" : "* SUSIE????^10 KRIS????",
						hardmode ? "* WHAT'S HAPPENING???^05\n* IS KRIS OKAY!?!??!" : "* WHAT ON EARTH IS\n  HAPPENING???????",
						"* Hey, chill out for\n  a sec!!",
						"* O-^05Okay!!!"
					}, new string[5] { "snd_txtnoe", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[15], new string[5] { "no_awe", "no_scared", "no_scared", "su_pissed", "no_awe" }, 0);
					state = 4;
					frames = 0;
				}
			}
			else if (txt.GetCurrentStringNum() == 2)
			{
				susie.GetComponent<SpriteRenderer>().flipX = false;
				susie.EnableAnimator();
			}
		}
		if (state == 4)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					if (!hardmode)
					{
						susie.SetSprite("unhappy/spr_su_right_0_unhappy");
					}
					else
					{
						susie.EnableAnimator();
					}
					noelle.UseUnhappySprites();
					noelle.ChangeDirection(Vector2.left);
					noelle.EnableAnimator();
				}
				if (frames == 30)
				{
					state = (hardmode ? 6 : 5);
					frames = 0;
					if (hardmode)
					{
						StartText(new string[8] { "* Okay,^05 so this'll sound\n  really weird,^05 but...", "* We're in a different\n  world.", "* Huh...?", "* Wait,^05 am I dreaming\n  again?", "* ...^15 No.", "* Everything is still\n  real and boring.", "* But apparently we need\n  to find--", "* Wait a minute,^05\n  who's THAT behind you?" }, new string[8] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe" }, new int[16], new string[8] { "su_smirk_sweat", "su_smile_sweat", "no_shocked", "no_tease_finger", "su_smirk_sweat", "su_annoyed", "su_side_sweat", "no_confused" }, 0);
					}
					else
					{
						StartText(new string[16]
						{
							"* Okay,^05 so this'll sound\n  really weird,^05 but...", "* We're in a different\n  world.", "* Huh...?", "* Wait,^05 am I dreaming\n  again?", "* ...^15 No.", "* Everything is still\n  real and boring.", "* But apparently we need\n  to find some scientist\n  guy to get home.", "* We need to go to like...^10\n  someplace called\n  Hotland.", "* Okay...?", "* It sounds weird but\n  that's what we need\n  to do!!!",
							"* Are you coming with\n  or not?!", "* (thump thump...)", "* I-I'll come!", "* Sweet.", "* (Noelle joined the party.)", "* Let's go,^05 Kris."
						}, new string[16]
						{
							"snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus",
							"snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_text", "snd_txtsus"
						}, new int[16], new string[16]
						{
							"su_smirk_sweat", "su_smile_sweat", "no_shocked", "no_tease_finger", "su_smirk_sweat", "su_annoyed", "su_side_sweat", "su_smirk_sweat", "no_confused", "su_wtf",
							"su_angry", "no_silent", "no_surprised_happy", "su_smirk", "", "su_smile"
						}, 0);
					}
				}
			}
			else
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					susie.DisableAnimator();
					susie.SetSprite(hardmode ? "spr_su_surprise_right_kr" : "spr_su_surprise_right");
				}
				if (txt.GetCurrentStringNum() == 4)
				{
					susie.DisableAnimator();
					susie.SetSprite(hardmode ? "spr_su_wtf_kr" : "spr_su_wtf");
				}
			}
		}
		if (state == 5)
		{
			if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					gm.ResumeMusic(15);
				}
				cam.transform.position = Vector3.Lerp(new Vector3(5f, 0f), cam.GetClampedPos(), (float)frames / 15f);
				if (frames == 15)
				{
					gm.SetFlag(1, "smile");
					susie.SetSelfAnimControl(setAnimControl: true);
					kris.SetSelfAnimControl(setAnimControl: true);
					kris.EnableAnimator();
					kris.ChangeDirection(Vector2.down);
					cam.SetFollowPlayer(follow: true);
					gm.SetPartyMembers(susie: true, noelle: true);
					EndCutscene();
				}
			}
			else
			{
				if (txt.GetCurrentStringNum() == 10)
				{
					susie.SetSprite("spr_su_wtf");
				}
				if (txt.GetCurrentStringNum() == 12)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.up);
				}
				if (txt.GetCurrentStringNum() == 13)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.left);
				}
				if (txt.GetCurrentStringNum() == 14)
				{
					susie.EnableAnimator();
				}
				if (txt.GetCurrentStringNum() == 15 && !musicPlayed)
				{
					musicPlayed = true;
					gm.PauseMusic();
					PlaySFX("music/mus_charjoined");
				}
				if (txt.GetCurrentStringNum() == 16)
				{
					susie.ChangeDirection(Vector2.left);
				}
			}
		}
		if (state == 6 && !txt)
		{
			frames++;
			if (susie.transform.position != new Vector3(5.42f, -1.22f))
			{
				susie.ChangeDirection(Vector2.down);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(5.42f, -1.22f), 1f / 12f);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
			}
			else
			{
				susie.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (noelle.transform.position != new Vector3(5.11f, -1.89f))
			{
				noelle.SetSelfAnimControl(setAnimControl: false);
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(5.11f, -1.89f), 1f / 12f);
				noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
			}
			if (frames == 60)
			{
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_laugh_0");
				if (geno)
				{
					StartText(new string[4] { "* Aww,^05 they're so\n  cute!", "* They're like a\n  little Kris!", "* Uhh,^05 you might wanna\n  get away from them.", "* Huh?^10\n* Why?" }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[16], new string[4] { "no_happy", "no_blush", "su_concerned", "no_confused_side" }, 0);
				}
				else
				{
					StartText(new string[4] { "* Aww,^05 they're so\n  cute!", "* They're like a\n  little Kris!", "* Wait,^05 what...?", "* Speaking of,^05 did\n  something happen to\n  them?" }, new string[4] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_txtnoe" }, new int[16], new string[4] { "no_happy", "no_blush", "su_side", "no_confused_side" }, 0);
				}
				state = 7;
				frames = 0;
			}
		}
		if (state == 7)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 4 && !noelle.GetComponent<Animator>().enabled)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.up);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					kris.ChangeDirection(Vector2.up);
					PlaySFX("sounds/snd_wing");
				}
				if (frames <= 3)
				{
					int num7 = ((frames % 2 == 0) ? 1 : (-1));
					float num8 = 3 - frames;
					susie.transform.position = new Vector3(5.42f, -1.22f) + new Vector3(num8 * (float)num7 / 24f, 0f);
				}
				if (frames == 15)
				{
					StartText(new string[1] { geno ? "* Great timing." : "* Hey,^05 perfect timing.\n^05* They're awake." }, new string[1] { "snd_txtsus" }, new int[16], new string[1] { geno ? "su_inquisitive" : "su_smile" }, 0);
				}
				if (frames == 16)
				{
					if (geno)
					{
						kris.ChangeDirection(Vector2.right);
					}
					susie.ChangeDirection(Vector2.right);
				}
				if (frames == 30)
				{
					PlaySFX("sounds/snd_wing");
					susie.SetCustomSpritesetPrefix("");
					susie.DisableAnimator();
					susie.SetSprite("spr_su_kneel");
					krisNPC.transform.position = new Vector3(6.41f, -1.38f);
				}
				if (frames == 45)
				{
					susie.EnableAnimator();
					susie.UseUnhappySprites();
				}
				if (frames == 60)
				{
					StartText(new string[1] { "* You all good,^05 Kris?" }, new string[1] { "snd_txtsus" }, new int[16], new string[1] { "su_smile_sweat" }, 0);
					state = 8;
					frames = 0;
				}
			}
		}
		if (state == 8 && !txt)
		{
			frames++;
			if (geno)
			{
				if (frames == 1)
				{
					kris.DisableAnimator();
					kris.SetSprite("g/spr_fr_right_1_g");
				}
				else if (frames == 40)
				{
					kris.EnableAnimator();
				}
				kris.transform.position = new Vector3(Mathf.Lerp(4.04f, 4.24f, (float)frames / 40f), -2.19f);
			}
			if (frames == 40)
			{
				PlaySFX("sounds/snd_wing");
				krisNPC.GetComponent<SpriteRenderer>().flipX = false;
				krisNPC.enabled = true;
				krisNPC.SetFloat("dirX", -1f);
			}
			if (frames == 75)
			{
				StartText(new string[2]
				{
					"* ...^15 Yeah.",
					geno ? "* Hey,^05 cool.^05\n* Anyways..." : "* Hey,^05 that's good."
				}, new string[2] { "snd_txtkrs", "snd_txtsus" }, new int[16], new string[2]
				{
					"kr_neutral",
					geno ? "su_side_sweat" : "su_smirk"
				}, 0);
				state = 9;
				frames = 0;
				noelle.UseHappySprites();
			}
		}
		if (state == 9 && !txt)
		{
			frames++;
			if (geno)
			{
				if (kris.transform.position.x != 5.4f)
				{
					kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(5.4f, -2.19f), 1f / 12f);
					kris.GetComponent<Animator>().SetBool("isMoving", value: true);
				}
				else
				{
					kris.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
				if (frames == 10)
				{
					susie.ChangeDirection(Vector2.down);
				}
				if (frames == 20)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_freaked");
					GameObject.Find("Exclaim").transform.position = new Vector3(5.43f, 0f);
				}
			}
			if (noelle.transform.position.x != 7.62f)
			{
				kris.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
				noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(7.62f, -1.89f), 0.125f);
			}
			else if (noelle.transform.position.y != -1.23f)
			{
				noelle.ChangeDirection(Vector2.up);
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(7.62f, -1.23f), 0.125f);
			}
			else
			{
				noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
				noelle.ChangeDirection(Vector2.left);
				krisNPC.SetFloat("dirX", 1f);
				if (geno)
				{
					StartText(new string[4]
					{
						"* I'm so glad that\n  you're okay,^05 Kris!",
						"* Okay,^05 no.^05\n* We are NOT having\n  this shit.",
						"* S-Susie?^10\n* What's happening?",
						knife ? "* Are you not seeing\n  the knife that they're\n  holding???" : "* Are you not seeing\n  the weapon that\n  they're holding???"
					}, new string[4] { "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus" }, new int[16], new string[4] { "no_happy", "su_annoyed", "no_awe", "su_pissed" }, 0);
					state = 12;
					frames = 0;
				}
				else
				{
					StartText(new string[5] { "* I'm so glad that\n  you're okay,^05 Kris!", "* ...^05 Do you guys mind\n  if I tag along...?", "* Uhh,^10 sure,^05 I guess?", "* (Kris and Noelle joined the\n  party.)", "* Aight,^05 we should really\n  get going now." }, new string[5] { "snd_txtnoe", "snd_txtnoe", "snd_txtsus", "snd_text", "snd_txtsus" }, new int[16], new string[5] { "no_happy", "no_blush", "su_annoyed", "", "su_smirk_sweat" }, 0);
					state = 10;
					frames = 0;
				}
			}
		}
		if (state == 10)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2 && noelle.GetComponent<Animator>().enabled)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_blush");
				}
				else if (txt.GetCurrentStringNum() == 3)
				{
					krisNPC.SetFloat("dirX", -1f);
				}
				else if (txt.GetCurrentStringNum() >= 4)
				{
					if (krisNPC.enabled)
					{
						gm.StopMusic();
						PlaySFX("music/mus_charjoined");
						krisNPC.enabled = false;
						krisNPC.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("player/Kris/spr_kr_pose");
						noelle.SetSprite("spr_no_pose");
					}
					State10();
				}
			}
			else
			{
				State10();
			}
		}
		if (state == 11 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				fade.FadeOut(1);
			}
			if (frames == 60)
			{
				gm.SetPartyMembers(susie: false, noelle: false);
				gm.EnablePlayerMovement();
				gm.SetCheckpoint(63);
				gm.LoadArea(63, fadeIn: true, Vector2.zero, Vector2.down);
			}
		}
		if (state == 12)
		{
			bool flag = !txt;
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() >= 2 && frames < 10)
				{
					flag = true;
				}
				if (txt.GetCurrentStringNum() == 4)
				{
					gm.StopMusic();
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wtf");
				}
			}
			if (flag)
			{
				frames++;
				if (frames == 1)
				{
					GameObject.Find("Exclaim").transform.position = new Vector3(1000f, 0f);
					susie.EnableAnimator();
					susie.ChangeDirection(Vector2.down);
					susie.GetComponent<Animator>().SetBool("isMoving", value: true);
					kris.GetComponent<Animator>().SetBool("isMoving", value: true);
					krisNPC.SetFloat("dirX", -1f);
				}
				susie.transform.position = Vector3.Lerp(new Vector3(5.42f, -1.22f), new Vector3(5.42f, -1.83f), (float)frames / 10f);
				kris.transform.position = Vector3.Lerp(new Vector3(5.4f, -2.19f), new Vector3(4.21f, -2.19f), (float)frames / 10f);
				if (frames == 10)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					kris.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
				if (frames == 11)
				{
					gm.PlayMusic("music/mus_prebattle1", 0.25f);
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_surprise");
					susie.EnableAnimator();
					susie.ChangeDirection(Vector2.right);
					GameObject.Find("Exclaim").transform.position = new Vector3(7.65f, 0f);
				}
				if (frames == 31)
				{
					GameObject.Find("Exclaim").transform.position = new Vector3(1000f, 0f);
					StartText(new string[6]
					{
						"* Y-^05you mean to say\n  that they were about\n  to...",
						"* (Why do they look\n  like...)",
						knife ? "* Is that...^10 my knife?" : "* Susie.",
						knife ? "* You didn't just\n  let them...?" : "* Why didn't you\n  stop them?",
						knife ? "* They literally snatched\n  it from me!" : "* They are literally\n  a sociopath!",
						"* And I wasn't about\n  to be gutted or\n  anything like that."
					}, new string[6] { "snd_txtnoe", "snd_txtkrs", "snd_txtkrs", "snd_txtkrs", "snd_txtsus", "snd_txtsus" }, new int[16], new string[6] { "no_shocked", "kr_scared_noeye", "kr_scared_noeye", "kr_scared_noeye", "su_angry", "su_annoyed" }, 0);
					state = 13;
					frames = 0;
				}
			}
		}
		if (state == 13)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 5 && susie.GetComponent<Animator>().enabled)
				{
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wtf");
				}
				else if (txt.GetCurrentStringNum() == 6 && !susie.GetComponent<Animator>().enabled)
				{
					susie.EnableAnimator();
				}
			}
			else if (kris.transform.position != new Vector3(5.82f, -2.19f))
			{
				if (!noelle.GetComponent<Animator>().enabled)
				{
					noelle.EnableAnimator();
					noelle.UseUnhappySprites();
					noelle.GetComponent<Animator>().SetBool("isMoving", value: true);
				}
				kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(5.82f, -2.19f), 1f / 12f);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(100f, -1.83f), 1f / 12f);
				noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(20f, -4f), 1f / 24f);
				susie.GetComponent<Animator>().SetBool("isMoving", value: true);
				kris.GetComponent<Animator>().SetBool("isMoving", value: true);
				susie.ChangeDirection(Vector2.left);
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					susie.GetComponent<Animator>().SetBool("isMoving", value: false);
					kris.GetComponent<Animator>().SetBool("isMoving", value: false);
					noelle.GetComponent<Animator>().SetBool("isMoving", value: false);
				}
				if (frames == 30)
				{
					susie.ChangeDirection(Vector2.up);
					krisNPC.SetFloat("dirX", 0f);
					krisNPC.SetFloat("dirY", -1f);
					StartText(new string[9] { "* Kris?", "* You thinkin' what\n  I'm thinkin'?", "* I think so.", "* Get them to stay\n  away,^05 else their heart\n  shatter?", "* ...^05 I wouldn't have\n  said THAT,^05 but yeah.", "* ...^10 Alright,^05 freak.", "* You have two choices.", "* You either leave us\n  alone,^05 or we mutilate\n  you.", "* Take your pick." }, new string[9] { "snd_txtsus", "snd_txtsus", "snd_txtkrs", "snd_txtkrs", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus" }, new int[16], new string[9] { "su_neutral", "su_smile", "kr_neutral", "kr_violent", "su_inquisitive", "su_depressed", "su_depressed", "su_depressed_smile", "su_teeth" }, 0);
					state = 14;
					frames = 0;
				}
			}
		}
		if (state == 14)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 4 && krisAnimState == 0)
				{
					krisAnimState++;
					krisNPC.GetComponent<SpriteRenderer>().flipX = true;
					krisNPC.Play("DR Attack", 0, 0f);
					krisNPC.transform.position = new Vector3(6.02f, -1.38f);
					PlaySFX("sounds/snd_attack");
				}
				else if (txt.GetCurrentStringNum() == 6 && krisAnimState == 1)
				{
					krisAnimState++;
					krisNPC.Play("DR Idle", 0, 0f);
					susie.DisableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = true;
					susie.SetSprite("spr_su_throw_ready");
				}
				else if (txt.GetCurrentStringNum() == 9 && krisAnimState == 2)
				{
					krisAnimState++;
					susie.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().flipX = false;
					susie.ChangeDirection(Vector2.left);
				}
				if (krisAnimState > 0)
				{
					if (kris.transform.position != new Vector3(4.49f, -2.19f))
					{
						kris.GetComponent<Animator>().SetBool("isMoving", value: true);
						kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(4.49f, -2.19f), 0.125f);
					}
					else
					{
						kris.GetComponent<Animator>().SetBool("isMoving", value: false);
					}
				}
			}
			else
			{
				frames++;
				if (frames < 25)
				{
					if (frames == 1)
					{
						kris.DisableAnimator();
						kris.SetSprite("g/spr_fr_right_1_g");
					}
					kris.transform.position = Vector3.MoveTowards(kris.transform.position, new Vector3(4.81f, -2.19f), 1f / 96f);
				}
				else
				{
					if (frames == 25)
					{
						kris.EnableAnimator();
						SetMoveAnim(kris, isMoving: false);
					}
					if (frames == 40)
					{
						StartText(new string[2] { "* Fine then.", "* Time to die." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[16], new string[2] { "su_confident", "su_teeth" }, 0);
						susie.DisableAnimator();
						susie.SetSprite("spr_su_shrug");
						state = 15;
						frames = 0;
					}
				}
			}
		}
		if (state != 15)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2 && krisAnimState == 3)
			{
				krisAnimState++;
				susie.EnableAnimator();
				susie.GetComponent<SpriteRenderer>().flipX = true;
				susie.GetComponent<Animator>().Play("DR Attack", 0, 0f);
				susie.transform.position = new Vector3(6.53f, -1.43f);
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_surprise");
				PlaySFX("sounds/snd_weaponpull");
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			gm.StopMusic();
			PlaySFX("sounds/snd_break2");
			susie.GetComponent<Animator>().Play("DR Idle", 0, 0f);
			susie.transform.position = new Vector3(6.9f, -1.88f);
			krisSOUL = GameObject.Find("KrisFakeSOUL").GetComponent<SpriteRenderer>();
		}
		if (frames >= 30 && frames < 45)
		{
			int num9 = frames - 30;
			if (num9 == 0)
			{
				OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
				for (int i = 0; i < array.Length; i++)
				{
					array[i].HideSprite();
				}
				SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				BoxCollider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<BoxCollider2D>();
				for (int i = 0; i < componentsInChildren2.Length; i++)
				{
					componentsInChildren2[i].enabled = false;
				}
				EdgeCollider2D[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<EdgeCollider2D>();
				for (int i = 0; i < componentsInChildren3.Length; i++)
				{
					componentsInChildren3[i].enabled = false;
				}
				AudioSource[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
				foreach (AudioSource audioSource in componentsInChildren4)
				{
					if (audioSource.isPlaying)
					{
						audioSource.enabled = false;
					}
				}
				TilemapRenderer[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
				for (int i = 0; i < componentsInChildren5.Length; i++)
				{
					componentsInChildren5[i].enabled = false;
				}
				fakeSOUL.transform.parent = kris.transform;
				fakeSOUL.transform.localPosition = new Vector3(0f, -0.254f);
				krisNPC.GetComponent<SpriteRenderer>().enabled = true;
				krisSOUL.transform.position = krisNPC.transform.position;
				soulPos = fakeSOUL.transform.position;
			}
			if (num9 == 1 || num9 == 5 || num9 == 9)
			{
				PlaySFX("sounds/snd_noise");
				fakeSOUL.GetComponent<SpriteRenderer>().enabled = true;
				krisSOUL.GetComponent<SpriteRenderer>().enabled = true;
			}
			if (num9 == 3 || num9 == 7)
			{
				fakeSOUL.GetComponent<SpriteRenderer>().enabled = false;
				krisSOUL.GetComponent<SpriteRenderer>().enabled = false;
			}
			if (num9 == 11)
			{
				kris.GetComponent<SpriteRenderer>().enabled = false;
				krisNPC.GetComponent<SpriteRenderer>().enabled = false;
				PlaySFX("sounds/snd_battlestart");
			}
			if (num9 >= 11)
			{
				fakeSOUL.transform.position = Vector3.Lerp(soulPos, new Vector3(-5.646f, -4.48f) + new Vector3(cam.transform.position.x, cam.transform.position.y), ((float)num9 - 11f) / 13f);
				krisSOUL.transform.position = Vector3.Lerp(krisNPC.transform.position, new Vector3(5f, 2f), ((float)num9 - 11f) / 13f);
			}
		}
		if (frames == 45)
		{
			SpriteRenderer[] componentsInChildren = GameObject.Find("MAP").GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].enabled = true;
			}
			BoxCollider2D[] componentsInChildren2 = GameObject.Find("MAP").GetComponentsInChildren<BoxCollider2D>();
			for (int i = 0; i < componentsInChildren2.Length; i++)
			{
				componentsInChildren2[i].enabled = true;
			}
			EdgeCollider2D[] componentsInChildren3 = GameObject.Find("MAP").GetComponentsInChildren<EdgeCollider2D>();
			for (int i = 0; i < componentsInChildren3.Length; i++)
			{
				componentsInChildren3[i].enabled = true;
			}
			AudioSource[] componentsInChildren4 = GameObject.Find("MAP").GetComponentsInChildren<AudioSource>();
			for (int i = 0; i < componentsInChildren4.Length; i++)
			{
				componentsInChildren4[i].enabled = true;
			}
			TilemapRenderer[] componentsInChildren5 = GameObject.Find("MAP").GetComponentsInChildren<TilemapRenderer>();
			foreach (TilemapRenderer tilemapRenderer in componentsInChildren5)
			{
				if (((Behaviour)(object)tilemapRenderer.GetComponent<Tilemap>()).enabled)
				{
					tilemapRenderer.enabled = true;
				}
			}
			OverworldPartyMember[] array = Util.FindObjectsOfType<OverworldPartyMember>();
			for (int i = 0; i < array.Length; i++)
			{
				array[i].ShowSprite();
			}
			kris.GetComponent<SpriteRenderer>().enabled = true;
			fakeSOUL.enabled = false;
			krisSOUL.enabled = false;
			PlaySFX("sounds/snd_grab");
			goner.enabled = false;
			goner.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_frg_grasp");
		}
		if (frames <= 48 && frames >= 45)
		{
			int num10 = ((frames % 2 == 0) ? 1 : (-1));
			float num11 = 48 - frames;
			kris.transform.position = new Vector3(4.9f, -2.19f) + new Vector3(num11 * (float)num10 / 24f, 0f);
			goner.transform.position = new Vector3(4.68f, -2.04f) + new Vector3(num11 * (float)num10 / 24f, 0f);
		}
		if (frames == 55)
		{
			gm.PlayGlobalSFX("sounds/snd_mysterygo");
			kris.GetComponent<SpriteRenderer>().enabled = false;
			goner.GetComponent<SpriteRenderer>().enabled = false;
			susie.DisableAnimator();
			susie.SetSprite("spr_su_surprise_right");
		}
		if (frames == 130)
		{
			susie.transform.position = new Vector3(7.08667f, -1.83f);
			susie.SetSprite("spr_su_shrug");
			susie.GetComponent<SpriteRenderer>().flipX = false;
			krisNPC.GetComponent<SpriteRenderer>().flipX = false;
			krisNPC.Play("idle", 0, 0f);
			krisNPC.transform.position = new Vector3(6.19f, -1.38f);
			PlaySFX("sounds/snd_smallswing");
			noelle.EnableAnimator();
			StartText(new string[1] { "* Well, that took care\n  of itself." }, new string[1] { "snd_txtsus" }, new int[16], new string[1] { "su_confident" }, 0);
			state = 11;
			frames = 0;
		}
	}

	private void State10()
	{
		frames++;
		if (frames <= 25)
		{
			goner.transform.position = new Vector3(Mathf.Lerp(-2.23f, 3.803f, (float)frames / 25f), -2.045f);
			return;
		}
		if (frames == 26)
		{
			if ((bool)txt)
			{
				Object.Destroy(txt);
			}
			PlaySFX("sounds/snd_grab");
			goner.enabled = false;
			goner.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_frg_grasp");
			susie.DisableAnimator();
			susie.SetSprite("spr_su_surprise_right");
			susie.GetComponent<SpriteRenderer>().flipX = true;
			noelle.DisableAnimator();
			noelle.SetSprite("spr_no_surprise");
			krisNPC.enabled = true;
			krisNPC.SetBool("isMoving", value: true);
		}
		if (frames <= 28)
		{
			int num = ((frames % 2 == 0) ? 1 : (-1));
			float num2 = 28 - frames;
			kris.transform.position = new Vector3(4.04f, -2.19f) + new Vector3(num2 * (float)num / 24f, 0f);
			goner.transform.position = new Vector3(3.803f, -2.045f) + new Vector3(num2 * (float)num / 24f, 0f);
			susie.transform.position = new Vector3(5.42f, -1.22f) + new Vector3(num2 * (float)num / 24f, 0f);
			noelle.transform.position = new Vector3(7.62f, -1.23f) + new Vector3(num2 * (float)num / 24f, 0f);
		}
		if (frames == 35)
		{
			gm.PlayGlobalSFX("sounds/snd_mysterygo");
			kris.GetComponent<SpriteRenderer>().enabled = false;
			goner.GetComponent<SpriteRenderer>().enabled = false;
		}
		if (krisNPC.transform.position.x != 6.73f)
		{
			krisNPC.transform.position = Vector3.MoveTowards(krisNPC.transform.position, new Vector3(6.73f, -1.38f), 1f / 12f);
		}
		else
		{
			krisNPC.SetBool("isMoving", value: false);
		}
		if (frames == 110)
		{
			krisNPC.SetFloat("dirX", 0f);
			krisNPC.SetFloat("dirY", -1f);
			StartText(new string[1] { "* ...^10 What?" }, new string[1] { "snd_txtkrs" }, new int[16], new string[1] { "kr_scared_noeye" }, 0);
			state = 11;
			frames = 0;
		}
		MonoBehaviour.print(frames);
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetFlag(59, 1);
		kris.transform.position = new Vector3(-4.35f, -0.82f);
		susie.transform.position = new Vector3(-4.88f, -1.48f);
		noelle.transform.position = new Vector3(7.65f, -1.89f);
		kris.ChangeDirection(Vector2.right);
		kris.SetSelfAnimControl(setAnimControl: false);
		kris.GetComponent<Animator>().SetBool("isMoving", value: true);
		susie.ChangeDirection(Vector2.right);
		susie.SetSelfAnimControl(setAnimControl: false);
		susie.GetComponent<Animator>().SetBool("isMoving", value: true);
		hardmode = (int)gm.GetFlag(108) == 1;
		geno = (int)gm.GetFlag(13) == 3;
		if (hardmode)
		{
			susie.SetCustomSpritesetPrefix("kr");
		}
		krisNPC = GameObject.Find("Kris").GetComponent<Animator>();
		goner = GameObject.Find("Goner").GetComponent<Animator>();
		knife = gm.GetWeapon(0) == 27;
		noelle.DisableAnimator();
		noelle.SetSprite("spr_no_collapsed");
		fakeSOUL = GameObject.Find("FakeSOUL").GetComponent<SpriteRenderer>();
	}
}
