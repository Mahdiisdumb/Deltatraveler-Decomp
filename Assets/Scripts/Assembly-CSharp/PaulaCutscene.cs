using System;
using System.Collections.Generic;
using UnityEngine;

public class PaulaCutscene : CutsceneBase
{
	private Animator paula;

	private bool geno;

	private bool paulaKnown;

	private bool selecting;

	private int choice1choice;

	private bool paulaAtFinalPos;

	private int fadeFrames;

	private bool vineBoom;

	private int krisFallFrames;

	private int tripFrames;

	private Vector3 paulaPos = Vector3.zero;

	private Transform table;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("intro_0", new string[1] { "* Who...?" });
		dictionary.Add("intro_1_paulaknown", new string[5] { "* Wait,^05 what???^05\n* A kid???", "* Are you Paula???", "* H-^05how did you know\n  my name?", "* Did someone tell you\n  that I was here...?", "* Well,^05 the masked\n  man didn't seem very\n  smart..." });
		dictionary.Add("intro_1", new string[5] { "* Uhhhh,^05 what???", "* Is that a kid???", "* Oh...^10 hello.^05\n* My name is Paula.", "* I was abducted\n  by a chubby kid\n  and a masked man.", "* They locked me\n  here,^05 and--" });
		dictionary.Add("intro_2", new string[12]
		{
			"* Wait...^05 how long have\n  you BEEN here?", "* You've just been...\n^10  alone,^05 all by yourself?", "* ... And they didn't\n  even give you any\n  FOOD???", "* WHEN I GET MY\n  HANDS ON THEM,\n  I'LL--", "* Susie,^05 shouldn't we\n  help her first?", "* Oh yeah.", "* ...", "* Well,^05 you three aren't\n  who I was hoping\n  would show up...", "* But if you wanna\n  help me,^05 you'll need\n  to get a key from--", "* Key?",
			"* Nah,^05 we don't need\n  that.", "* Susie?"
		});
		dictionary.Add("intro_3", new string[1] { "* Watch THIS!" });
		dictionary.Add("intro_4", new string[3] { "* ...", "* Well...^05 thanks!", "* Don't mention it." });
		dictionary.Add("intro_5", new string[1] { "* So...^05 what now?" });
		dictionary.Add("gaster_rant_0", new string[3] { "\b        WELL DONE,^10 KRIS.", "\b          IT APPEARS\n\b         WE HAVE FOUND\n\b       AN ESSENCE HOLDER.", "\b    TAKE A LOOK AT THE SOULS\n\b          IN THIS ROOM." });
		dictionary.Add("gaster_rant_1", new string[12]
		{
			"\b  NOTICE THE COLOR OF YOUR SOUL,^10\n\b        AS WELL AS HERS.", "\b   THIS INDICATES A CONNECTION.", "\b            THOUGH...\n\b           WITH HER...", "\b          IT APPEARS...\n\b     HER CONNECTION IS NOT\n\b         STRONG ENOUGH.", "\b IT IS SPREAD ACROSS FOUR HUMANS\n\b         IN THIS WORLD.", "\b    AND THEIR WILL TO PROCEED\n\b         IS NOT AS HIGH\n\b          AS YOUR OWN.", "\b              ...", "\b           HOWEVER...", "\b    SHE SEEMS TO BE CARRYING\n\b       AN IMPORTANT OBJECT.", "\b          I WONDER...",
			"\b  WOULD SHE BENEFIT YOUR CAUSE?", "\b           AND IF SO..."
		});
		dictionary.Add("gaster_rant_2", new string[1] { "\b  WOULD YOU ALLOW HER TO DO SO?" });
		dictionary.Add("post_gaster_0", new string[2] { "* No idea.", "* Any ideas,^05 Kris?" });
		dictionary.Add("post_gaster_1", new string[2] { "* ...^10 Kris?", "* Is... something wrong?" });
		dictionary.Add("choices_0", new string[2] { "Nothing", "Paula\nshould\njoin us" });
		dictionary.Add("post_gaster_2_choice0", new string[7] { "* ...", "* Not sure I believe\n  that.", "* Yeah,^05 Kris.", "* You look really\n  worried about\n  something.", "* Say,^05 is there a\n  reason why you came\n  here?", "* Well,^05 we're tryna find\n  a grey door,^05 mostly.", "* Really?" });
		dictionary.Add("post_gaster_2_choice1", new string[5] { "* Huh?", "* Umm,^05 I guess for\n  like...", "* Until we reach a\n  grey door.", "* (I don't think Kris\n  even believes\n  themself...)", "* Oh,^05 you're trying to\n  find a grey door?" });
		dictionary.Add("post_gaster_3", new string[22]
		{
			"* Well,^05 I heard there's\n  one nearby.", "* It's at a place\n  called <color=#FFFF00FF>Lilliput Steps</color>.", "* But...^05 the entrance\n  has been completely\n  sealed.", "* The only way to\n  get past it is\n  to blow it up.", "* Okay...^10 but how?", "* Is there a bomb\n  nearby or something?", "* Yeah.\n^05* Exactly that.", "* Mr. Carpainter has a\n  bomb hidden away at\n  the <color=#FFFF00FF>center of town</color>.", "* Okay,^05 I was just\n  joking.", "* That's like...\n^05  actually stupid.",
			"* But if that's what\n  we need to do,^05\n  we'll just take it.", "* That could be an\n  issue if you go\n  by yourselves.", "* See,^05 he has this\n  lightning power...", "* Because he can cast\n  lightning,^05 you won't\n  be able to get it.", "* This whole thing sounds\n  really tough,^05 haha...", "* Well,^05 it's a good\n  thing I have a solution.", "* I have a Franklin Badge\n  that can <color=#FFFF00FF>reflect\n  lightning</color>!", "* Maybe for the time\n  being,^05 I could join you\n  three.", "* Badge aside,^05 I know\n  some psychic moves\n  that can help you.", "* I dunno.",
			"* Sounds kinda complicated.", "* ..."
		});
		dictionary.Add("paula_injured_0", new string[11]
		{
			"* O-^05ow...", "* I...^10 I think I\n  hurt my leg...", "* Those two were\n  pretty rough...", "* Oh!", "* Umm...^05 Maybe one of\n  us could carry you?", "* Really?\n* That sounds like\n  fun!", "* Umm,^05 sorry but...^05 I...^10\n  don't do carrying.", "* And I mean,^05 she was\n  sitting in a\n  dirty ass cell.", "* And I'm not strong\n  enough to carry\n  anyone...", "* ...^05 Kris?",
			"* Can you carry her?"
		});
		dictionary.Add("choices_1", new string[2] { "Yes", "No" });
		dictionary.Add("paula_injured_1_choice0", new string[2] { "* T-^05thanks Kris!", "* (They could've said it\n  less rudely,^05 I was\n  asking nicely...)" });
		dictionary.Add("paula_injured_1_choice1", new string[2] { "* No?", "* Well too bad,^05 you\n  can't proceed without\n  me!" });
		dictionary.Add("cope_seethe_mald", new string[9] { "* (Paula joined the party.)", "* Wow,^05 it's so tall\n  up here!", "* I...^10 I'd love to\n  also ride piggyback...", "* Well,^05 too bad Kris\n  is already carrying\n  someone.", "* Well,^05 you're not\n  carrying anything...", "* Yeah,^05 well I don't\n  feel like carrying\n  anything right now.", "* ", "* Alright,^05 enough screwing\n  around.", "* Let's go to that\n  <color=#FFFF00FF>center of town</color> place." });
		return dictionary;
	}

	private void Update()
	{
		if (state == 0)
		{
			frames++;
			if (frames < 30)
			{
				kris.transform.position += new Vector3(-1f / 12f, 0f);
				susie.transform.position += new Vector3(-1f / 12f, 0f);
				noelle.transform.position += new Vector3(-1f / 12f, 0f);
			}
			else if (frames == 30)
			{
				paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_firstcut_1");
				kris.GetComponent<Animator>().SetBool("isMoving", false);
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				noelle.GetComponent<Animator>().SetBool("isMoving", false);
				StartText(GetStringArray("intro_0"), new string[1] { "snd_txtpau" }, new int[18], new string[1] { "pau_confused" }, 0);
				state = 1;
				frames = 0;
			}
		}
		if (state == 1)
		{
			if ((frames > 0 && (bool)txt) || !txt)
			{
				frames++;
			}
			if (frames == 1)
			{
				kris.ChangeDirection(Vector2.up);
				susie.ChangeDirection(Vector2.up);
				noelle.ChangeDirection(Vector2.up);
			}
			if (frames == 15)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_surprise_up");
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_surprise_up");
				if (paulaKnown)
				{
					StartText(GetStringArray("intro_1_paulaknown"), new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtpau" }, new int[18], new string[5] { "su_shocked", "su_shocked", "pau_surprised", "pau_neutral", "pau_dejected" }, 0);
				}
				else
				{
					StartText(GetStringArray("intro_1"), new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtpau" }, new int[18], new string[5] { "su_shocked", "su_shocked", "pau_neutral", "pau_dejected", "pau_dejected" }, 0);
				}
			}
			if (frames > 15 && !txt)
			{
				state = 2;
				frames = 0;
				noelle.EnableAnimator();
				noelle.UseUnhappySprites();
				susie.EnableAnimator();
				susie.GetComponent<SpriteRenderer>().flipX = true;
				StartText(GetStringArray("intro_2"), new string[12]
				{
					"snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtsus",
					"snd_txtsus", "snd_txtnoe"
				}, new int[18], new string[12]
				{
					"su_surprised", "su_concerned", "su_pissed", "su_wtf", "no_thinking", "su_smile_sweat", "pau_sussy", "pau_dejected", "pau_smile", "su_surprised",
					"su_annoyed", "no_confused"
				}, 0);
			}
		}
		if (state == 2)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					kris.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.left);
					susie.DisableAnimator();
					susie.SetSprite("spr_su_wtf");
				}
				if (txt.GetCurrentStringNum() == 4)
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
				if (txt.GetCurrentStringNum() == 6)
				{
					if (!paula.GetComponent<SpriteRenderer>().sprite.name.EndsWith("2"))
					{
						paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_firstcut_2");
					}
					susie.EnableAnimator();
					susie.ChangeDirection(Vector2.right);
					susie.UseUnhappySprites();
				}
				if (txt.GetCurrentStringNum() == 7)
				{
					kris.ChangeDirection(Vector2.up);
					susie.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
				}
				if (txt.GetCurrentStringNum() == 8 && !paula.GetComponent<SpriteRenderer>().sprite.name.EndsWith("1"))
				{
					paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_firstcut_1");
				}
				if (txt.GetCurrentStringNum() == 12)
				{
					noelle.ChangeDirection(Vector2.left);
				}
			}
			else if (susie.transform.position.y != -2.96f)
			{
				susie.UseHappySprites();
				susie.ChangeDirection(Vector2.down);
				susie.GetComponent<Animator>().SetBool("isMoving", true);
				susie.GetComponent<Animator>().SetFloat("speed", 2f);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(susie.transform.position.x, -2.96f), 5f / 24f);
			}
			else if (susie.transform.position != new Vector3(-1.65f, -2.96f))
			{
				if (!paula.GetComponent<SpriteRenderer>().sprite.name.EndsWith("3"))
				{
					paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_firstcut_3");
				}
				kris.ChangeDirection(Vector2.down);
				susie.ChangeDirection(Vector2.left);
				susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(-1.65f, -2.96f), 0.25f);
			}
			else
			{
				kris.ChangeDirection(Vector2.left);
				susie.GetComponent<Animator>().SetBool("isMoving", false);
				susie.GetComponent<Animator>().SetFloat("speed", 1f);
				susie.ChangeDirection(Vector2.up);
				susie.DisableAnimator();
				susie.SetSprite("spr_su_throw_ready");
				StartText(GetStringArray("intro_3"), new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_teeth_eyes" }, 0);
				state = 3;
				frames = 0;
			}
		}
		if (state == 3 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				susie.SetSprite("spr_su_smash_0");
				table.GetComponent<SpriteRenderer>().sortingOrder = susie.GetComponent<SpriteRenderer>().sortingOrder + 1;
				table.position = new Vector3(-1.62f, -2.62f);
				PlaySFX("sounds/snd_grab");
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_left_shocked_0");
			}
			if (frames == 15)
			{
				paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_shocked");
				susie.SetSprite("spr_su_smash_1");
				table.position = new Vector3(-1.62f, -2.81f);
				table.eulerAngles = new Vector3(0f, 0f, 2.611f);
			}
			if (frames >= 30)
			{
				int num = (frames - 30) / 2 + 2;
				if (num > 4)
				{
					num = 4;
				}
				susie.SetSprite("spr_su_smash_" + num);
				table.GetComponent<SpriteRenderer>().sortingOrder = susie.GetComponent<SpriteRenderer>().sortingOrder - 1;
				table.position = Vector3.Lerp(new Vector3(-1.62f, -2.81f), new Vector3(-1.6f, -0.59f), (float)(frames - 30) / 6f);
				table.eulerAngles = Vector3.Lerp(new Vector3(0f, 0f, 2.611f), new Vector3(0f, 0f, -84f), (float)(frames - 30) / 6f);
				if (frames == 37)
				{
					UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/RealisticExplosion"), new Vector3(-1.74f, -0.71f), Quaternion.identity).transform.localScale = new Vector3(2f, 2f, 1f);
					GameObject.Find("CellDoor").GetComponent<SpriteRenderer>().enabled = false;
					GameObject.Find("RoomShading").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/eb_objects/spr_cabin_shading_nodoor");
					table.GetComponent<SpriteRenderer>().enabled = false;
				}
			}
			if (frames == 90)
			{
				susie.EnableAnimator();
				StartText(GetStringArray("intro_4"), new string[3] { "snd_txtpau", "snd_txtpau", "snd_txtsus" }, new int[18], new string[3] { "pau_wideeye", "pau_embarrassed", "su_smile" }, 0);
				state = 4;
				frames = 0;
			}
		}
		if (state == 4)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 2)
				{
					paula.enabled = true;
					paula.SetFloat("dirX", -1f);
				}
			}
			else
			{
				frames++;
				if (frames == 1)
				{
					paula.SetBool("isMoving", true);
				}
				if (paula.transform.position.x != -1.65f)
				{
					paula.transform.position = Vector3.MoveTowards(paula.transform.position, new Vector3(-1.65f, 0.29f), 0.125f);
				}
				else if (paula.transform.position.y != -1.33f)
				{
					paula.SetFloat("dirX", 0f);
					paula.SetFloat("dirY", -1f);
					paula.transform.position = Vector3.MoveTowards(paula.transform.position, new Vector3(-1.65f, -1.33f), 0.125f);
				}
				else
				{
					paula.SetBool("isMoving", false);
				}
				if (noelle.transform.position != new Vector3(-0.55f, -2.99f))
				{
					noelle.EnableAnimator();
					noelle.GetComponent<Animator>().SetBool("isMoving", true);
					noelle.ChangeDirection(Vector2.left);
					noelle.transform.position = Vector3.MoveTowards(noelle.transform.position, new Vector3(-0.55f, -2.99f), 1f / 12f);
				}
				else
				{
					noelle.GetComponent<Animator>().SetBool("isMoving", false);
					noelle.ChangeDirection(Vector2.up);
				}
				if (frames == 100)
				{
					StartText(GetStringArray("intro_5"), new string[1] { "snd_txtnoe" }, new int[18], new string[1] { "no_confused_side" }, 0);
					state = 5;
					frames = 0;
				}
			}
		}
		if (state == 5 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic();
				PlaySFX("sounds/snd_noise");
				GameObject.Find("TimeFreeze").GetComponent<SpriteRenderer>().enabled = true;
			}
			if (frames == 60)
			{
				gm.PlayMusic("music/AUDIO_ANOTHERHIM", 0.7f, 0.75f);
				StartText(GetStringArray("gaster_rant_0"), new string[20]
				{
					"", "", "", "", "", "", "", "", "", "",
					"", "", "", "", "", "", "", "", "", ""
				}, new int[22]
				{
					2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					2, 2
				}, new string[0], 0);
				state = 6;
				frames = 0;
				txt.EnableGasterText();
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
			}
		}
		if (state == 6)
		{
			bool flag = true;
			if (geno)
			{
				if (frames >= 60)
				{
					krisFallFrames++;
				}
				bool flag2 = krisFallFrames >= 300;
				if ((bool)txt && txt.GetCurrentStringNum() == 11)
				{
					flag2 = true;
				}
				if (flag2)
				{
					UnityEngine.Object.Destroy(txt);
					state = 12;
					frames = 0;
					flag = false;
				}
			}
			if (!txt && flag)
			{
				frames++;
				if (frames == 1)
				{
					PlaySFX("sounds/snd_shadowpendant");
					SpriteRenderer[] componentsInChildren = GameObject.Find("SOULs").GetComponentsInChildren<SpriteRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].enabled = true;
					}
					GameObject.Find("SOULs").transform.Find("Yours").position = kris.transform.position;
					GameObject.Find("SOULs").transform.Find("SusieS").position = susie.transform.position;
					GameObject.Find("SOULs").transform.Find("NoelleS").position = noelle.transform.position;
					GameObject.Find("SOULs").transform.Find("PaulaS").position = paula.transform.position;
				}
				if (frames <= 30)
				{
					SpriteRenderer[] componentsInChildren = GameObject.Find("SOULs").GetComponentsInChildren<SpriteRenderer>();
					foreach (SpriteRenderer obj in componentsInChildren)
					{
						float b = ((obj.gameObject.name == "soulll") ? 1f : 0.7058824f);
						float a = Mathf.Lerp(0f, b, (float)frames / 30f);
						Color color = obj.color;
						color.a = a;
						obj.color = color;
					}
					GameObject.Find("ActionFade").GetComponent<SpriteRenderer>().color = new Color(0f, 0f, 0f, Mathf.Lerp(0f, 0.5f, (float)frames / 30f));
				}
				if (frames == 60)
				{
					StartText(GetStringArray("gaster_rant_1"), new string[20]
					{
						"", "", "", "", "", "", "", "", "", "",
						"", "", "", "", "", "", "", "", "", ""
					}, new int[22]
					{
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
						2, 2
					}, new string[0], 0);
					txt.EnableGasterText();
					UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
					UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
				}
			}
			if (frames == 61)
			{
				gm.StopMusic();
				kris.GetComponent<SpriteRenderer>().color = Color.white;
				kris.DisableAnimator();
				kris.SetSprite("spr_kr_outline_intimidate");
				susie.GetComponent<SpriteRenderer>().color = Color.black;
				noelle.GetComponent<SpriteRenderer>().color = Color.black;
				paula.GetComponent<SpriteRenderer>().color = Color.black;
				SpriteRenderer[] componentsInChildren = GameObject.Find("SOULs").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				componentsInChildren = GameObject.Find("OBJ").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].color = Color.black;
				}
				GameObject.Find("RoomShading").GetComponent<SpriteRenderer>().enabled = false;
				UnityEngine.Object.Destroy(GameObject.Find("ActionFade"));
			}
			if (frames == 120)
			{
				StartText(GetStringArray("gaster_rant_2"), new string[20]
				{
					"", "", "", "", "", "", "", "", "", "",
					"", "", "", "", "", "", "", "", "", ""
				}, new int[22]
				{
					2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					2, 2, 2, 2, 2, 2, 2, 2, 2, 2,
					2, 2
				}, new string[0], 0);
				txt.EnableGasterText();
				UnityEngine.Object.Destroy(GameObject.Find("menuBorder"));
				UnityEngine.Object.Destroy(GameObject.Find("menuBox"));
				state = 7;
				frames = 0;
			}
		}
		if (state == 5 || state == 6)
		{
			fadeFrames++;
			GameObject.Find("TimeFreeze").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f + (Mathf.Cos((float)(fadeFrames * 3) * ((float)Math.PI / 180f)) - 1f) / 10f);
		}
		if (state == 7)
		{
			if (!txt)
			{
				if (frames == 0)
				{
					gm.PlayMusic("music/mus_paula");
					kris.EnableAnimator();
					susie.GetComponent<SpriteRenderer>().color = Color.white;
					noelle.GetComponent<SpriteRenderer>().color = Color.white;
					paula.GetComponent<SpriteRenderer>().color = Color.white;
					SpriteRenderer[] componentsInChildren = GameObject.Find("OBJ").GetComponentsInChildren<SpriteRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].color = Color.white;
					}
					GameObject.Find("RoomShading").GetComponent<SpriteRenderer>().enabled = true;
					GameObject.Find("TimeFreeze").GetComponent<SpriteRenderer>().enabled = false;
					PlaySFX("sounds/snd_noise");
					susie.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.left);
					StartText(GetStringArray("post_gaster_0"), new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_side_sweat", "su_smile_sweat" }, 0);
					frames++;
				}
				else
				{
					if (frames == 1)
					{
						noelle.ChangeDirection(Vector2.right);
						paula.SetFloat("dirX", 1f);
						paula.SetFloat("dirY", 0f);
					}
					frames++;
					if (frames == 45)
					{
						susie.UseUnhappySprites();
						StartText(GetStringArray("post_gaster_1"), new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_inquisitive", "su_neutral" }, 0);
						txt.EnableSelectionAtEnd();
					}
				}
			}
			else if (txt.CanLoadSelection() && !selecting)
			{
				selecting = true;
				InitiateDeltaSelection();
				select.SetupChoice(Vector2.left, GetString("choices_0", 0), Vector3.zero);
				select.SetupChoice(Vector2.right, GetString("choices_0", 1), new Vector3(-32f, 0f));
				select.Activate(this, 0, txt.gameObject);
			}
		}
		if (state == 8)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 5)
				{
					susie.ChangeDirection(Vector2.up);
					noelle.ChangeDirection(Vector2.up);
					paula.SetFloat("dirX", 0f);
					paula.SetFloat("dirY", -1f);
				}
			}
			else
			{
				StartText(GetStringArray("post_gaster_3"), new string[22]
				{
					"snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtsus", "snd_txtsus",
					"snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtnoe", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtsus",
					"snd_txtsus", "snd_txtpau"
				}, new int[28], new string[22]
				{
					"pau_neutral", "pau_neutral", "pau_dejected", "pau_dejected", "su_neutral", "su_smirk_sweat", "pau_smile", "pau_smile", "su_inquisitive", "su_annoyed",
					"su_confident", "pau_dejected", "pau_neutral", "pau_dejected", "no_weird", "pau_confident", "pau_smile", "pau_smile_side", "pau_happy", "su_annoyed",
					"su_annoyed", "pau_annoyed"
				}, 0);
				state = 9;
			}
		}
		if (state == 9)
		{
			if (!txt)
			{
				if (frames == 0)
				{
					paula.GetComponent<SpriteRenderer>().sortingOrder = 5;
					paula.enabled = true;
					paula.SetFloat("dirX", 1f);
					paula.SetFloat("dirY", 0f);
					paula.SetBool("isMoving", true);
					frames++;
				}
				if (paula.transform.position.x != 0f && frames == 1)
				{
					paula.transform.position = Vector3.MoveTowards(paula.transform.position, new Vector3(0f, -1.33f), 1f / 12f);
				}
				else
				{
					frames++;
					if (frames == 2)
					{
						kris.ChangeDirection(Vector2.up);
						paula.SetBool("isMoving", false);
					}
					if (frames >= 12 && frames <= 15)
					{
						int num2 = ((frames % 2 == 0) ? 1 : (-1));
						int num3 = 15 - frames;
						paula.transform.position = new Vector3(0f, -1.33f) + new Vector3((float)(num3 * num2) / 24f, 0f);
					}
					if (frames >= 27 && frames <= 30)
					{
						if (frames == 27)
						{
							susie.DisableAnimator();
							susie.SetSprite("spr_su_surprise_up");
							noelle.DisableAnimator();
							noelle.SetSprite("spr_no_surprise_up");
							PlaySFX("sounds/snd_noise");
							paula.enabled = false;
							paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_kneel");
						}
						int num4 = ((frames % 2 == 0) ? 1 : (-1));
						int num5 = 30 - frames;
						paula.transform.position = new Vector3(0f, -1.33f) + new Vector3((float)(num5 * num4) / 24f, 0f);
					}
					if (frames == 45)
					{
						StartText(GetStringArray("paula_injured_0"), new string[11]
						{
							"snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtnoe", "snd_txtnoe", "snd_txtpau", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe",
							"snd_txtnoe"
						}, new int[21], new string[11]
						{
							"pau_pain", "pau_pain", "pau_dejected", "no_shocked", "no_confused_side", "pau_surprised", "su_flustered", "su_annoyed", "no_thinking", "no_confused",
							"no_confused_side"
						}, 0);
						txt.EnableSelectionAtEnd();
						state = 10;
						frames = 0;
					}
				}
			}
			else if (txt.GetCurrentStringNum() == 20 && paula.enabled)
			{
				paula.enabled = false;
				paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_down_annoyed");
			}
		}
		if (state == 10)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 5)
				{
					noelle.EnableAnimator();
				}
				if (txt.GetCurrentStringNum() == 6)
				{
					paula.enabled = true;
					paula.SetFloat("dirY", -1f);
					paula.SetFloat("dirX", 0f);
					susie.EnableAnimator();
				}
				if (txt.CanLoadSelection() && !selecting)
				{
					selecting = true;
					InitiateDeltaSelection();
					select.SetupChoice(Vector2.left, "Yes", Vector3.zero);
					select.SetupChoice(Vector2.right, "No", new Vector3(20f, 0f));
					select.Activate(this, 1, txt.gameObject);
				}
			}
			else if (!selecting)
			{
				if (paula.transform.position.x != 2.75f && !paulaAtFinalPos)
				{
					paula.SetFloat("dirX", 1f);
					paula.SetFloat("dirY", 0f);
					paula.SetBool("isMoving", true);
					paula.transform.position = Vector3.MoveTowards(paula.transform.position, new Vector3(2.75f, -1.33f), 0.125f);
					susie.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.right);
				}
				else if (paula.transform.position.y != -2.44f && !paulaAtFinalPos)
				{
					kris.ChangeDirection(Vector2.right);
					paula.SetFloat("dirX", 0f);
					paula.SetFloat("dirY", -1f);
					paula.transform.position = Vector3.MoveTowards(paula.transform.position, new Vector3(2.75f, -2.44f), 0.125f);
				}
				else if (!paulaAtFinalPos)
				{
					paulaAtFinalPos = true;
				}
				else
				{
					frames++;
					if (frames == 1)
					{
						paula.SetFloat("dirX", -1f);
						paula.SetFloat("dirY", 0f);
						paula.SetFloat("speed", 0f);
						PlaySFX("sounds/snd_jump");
					}
					float num6 = (float)frames / 8f;
					paula.transform.position = new Vector3(Mathf.Lerp(2.75f, 2.139f, num6), Mathf.Lerp(-2.44f, -1.931f, Mathf.Sin(num6 * (float)Math.PI * 0.5f)));
					if (frames == 8)
					{
						kris.ChangeDirection(Vector2.left);
						paula.GetComponent<SpriteRenderer>().enabled = false;
						gm.SetMiniPartyMember(1);
						gm.Heal(0, 20);
					}
					if (frames == 35)
					{
						gm.StopMusic();
						gm.PlayGlobalSFX("music/mus_charjoined");
						StartText(GetStringArray("cope_seethe_mald"), new string[9] { "snd_text", "snd_txtpau", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[21], new string[9] { "", "pau_happy", "no_blush", "su_smirk_sweat", "no_happy", "su_annoyed", "no_boom", "su_annoyed", "su_side" }, 0);
						state = 11;
						frames = 0;
					}
				}
			}
		}
		if (state == 11)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					noelle.DisableAnimator();
					noelle.SetSprite("spr_no_blush");
				}
				if (txt.GetCurrentStringNum() == 4)
				{
					susie.ChangeDirection(Vector2.right);
				}
				if (txt.GetCurrentStringNum() == 5)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.left);
					noelle.UseHappySprites();
				}
				if (txt.GetCurrentStringNum() == 7 && !vineBoom)
				{
					vineBoom = true;
					PlaySFX("sounds/snd_vineboom");
					noelle.UseUnhappySprites();
				}
			}
			else
			{
				kris.SetSelfAnimControl(true);
				susie.SetSelfAnimControl(true);
				noelle.SetSelfAnimControl(true);
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
				susie.UseHappySprites();
				if ((int)gm.GetFlag(87) == 0)
				{
					noelle.UseHappySprites();
				}
				gm.PlayMusic("music/mus_paula");
				EndCutscene();
			}
		}
		if (state == 12 && !txt)
		{
			frames++;
			if (frames == 1)
			{
				gm.StopMusic();
				kris.DisableAnimator();
				kris.SetSprite("spr_kr_sit_injured_injured");
				kris.GetComponent<SpriteRenderer>().flipX = true;
				susie.UseUnhappySprites();
				susie.ChangeDirection(Vector2.right);
				noelle.ChangeDirection(Vector2.right);
				paula.SetFloat("dirX", 1f);
				paula.SetFloat("dirY", 0f);
				kris.GetComponent<SpriteRenderer>().color = Color.white;
				susie.GetComponent<SpriteRenderer>().color = Color.white;
				noelle.GetComponent<SpriteRenderer>().color = Color.white;
				paula.GetComponent<SpriteRenderer>().color = Color.white;
				SpriteRenderer[] componentsInChildren = GameObject.Find("SOULs").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].enabled = false;
				}
				GameObject.Find("RoomShading").GetComponent<SpriteRenderer>().enabled = false;
				UnityEngine.Object.Destroy(GameObject.Find("ActionFade"));
				componentsInChildren = GameObject.Find("OBJ").GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].color = Color.white;
				}
				GameObject.Find("RoomShading").GetComponent<SpriteRenderer>().enabled = true;
				GameObject.Find("TimeFreeze").GetComponent<SpriteRenderer>().enabled = false;
				PlaySFX("sounds/snd_noise");
			}
			if (frames == 15)
			{
				susie.DisableAnimator();
				susie.SetSprite("spr_su_surprise_right");
				StartText(new string[1] { "* Kris???" }, new string[1] { "snd_txtsus" }, new int[18], new string[1] { "su_shocked" }, 0);
				noelle.DisableAnimator();
				noelle.SetSprite("spr_no_right_shocked_0");
			}
			if (frames > 15)
			{
				if (frames == 16)
				{
					susie.EnableAnimator();
					susie.GetComponent<Animator>().SetBool("isMoving", true);
					susie.GetComponent<Animator>().SetFloat("speed", 2f);
				}
				if (susie.transform.position.y != -1.58f)
				{
					susie.ChangeDirection(Vector2.up);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(susie.transform.position.x, -1.58f), 5f / 24f);
				}
				else if (susie.transform.position.x != 1.69f)
				{
					susie.ChangeDirection(Vector2.right);
					susie.transform.position = Vector3.MoveTowards(susie.transform.position, new Vector3(1.69f, -1.58f), 5f / 24f);
				}
				else
				{
					state = 13;
					frames = 0;
					susie.GetComponent<Animator>().SetBool("isMoving", false);
					susie.GetComponent<Animator>().SetFloat("speed", 1f);
					susie.DisableAnimator();
					susie.SetSprite("spr_su_kneel_front");
					paulaPos = paula.transform.position;
					StartText(new string[14]
					{
						"* Hey,^05 are you okay\n  dude???", "* God,^05 how the hell\n  are we supposed to\n  keep going like this?", "* Hey,^05 I think I know\n  somewhere you guys can\n  go.", "* It's a place called\n  <color=#FFFF00FF>Lilliput Steps</color>.", "* It has this really\n  powerful healing effect.", "* It's so powerful that\n  it even cures\n  concussions.", "* But the cave was\n  blocked off because\n  a grey door appeared.", "* A grey door?", "* We've actually been\n  trying to find one\n  of those.", "* How can we get\n  there?",
						"* Honestly,^05 I think it'd\n  be best for you guys\n  to hang tight.", "* What??!", "* I'm gonna try to find\n  someone that can help\n  get through.", "* Stay here for a bit."
					}, new string[14]
					{
						"snd_txtsus", "snd_txtsus", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtpau", "snd_txtnoe", "snd_txtnoe", "snd_txtsus",
						"snd_txtpau", "snd_txtsus", "snd_txtpau", "snd_txtpau"
					}, new int[18], new string[14]
					{
						"su_concerned", "su_sus", "pau_confident", "pau_smile", "pau_smile_side", "pau_happy", "pau_neutral", "no_awe", "no_happy", "su_surprised",
						"pau_dejected", "su_wtf", "pau_dejected", "pau_neutral"
					}, 0);
				}
			}
		}
		if (state == 13)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 3)
				{
					noelle.EnableAnimator();
					noelle.ChangeDirection(Vector2.up);
				}
				if (txt.GetCurrentStringNum() == 12)
				{
					susie.GetComponent<SpriteRenderer>().flipX = true;
					susie.SetSprite("spr_su_wtf");
				}
				else if (txt.GetCurrentStringNum() == 13)
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
					susie.ChangeDirection(Vector2.left);
					susie.EnableAnimator();
				}
			}
			else
			{
				Vector3 vector = Vector3.zero;
				tripFrames++;
				if (tripFrames >= 10 && tripFrames <= 16)
				{
					if (tripFrames == 10)
					{
						paula.enabled = false;
						paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_trip");
					}
					int num7 = ((tripFrames % 2 == 0) ? 1 : (-1));
					int num8 = 16 - tripFrames;
					vector = new Vector3((float)(num8 * num7) / 72f, 0f);
				}
				if (tripFrames == 20)
				{
					paula.enabled = true;
				}
				if (paulaPos.x < 3.42f)
				{
					paula.GetComponent<SpriteRenderer>().sortingOrder = 5;
					paula.SetBool("isMoving", true);
					paulaPos = Vector3.MoveTowards(paulaPos, new Vector3(3.42f, paulaPos.y), ((tripFrames >= 10 && tripFrames <= 16) ? 3f : 6f) / 48f);
				}
				else if (paulaPos.y != -2.32f)
				{
					susie.ChangeDirection(Vector2.right);
					noelle.ChangeDirection(Vector2.right);
					paula.SetFloat("dirX", 0f);
					paula.SetFloat("dirY", -1f);
					paulaPos = Vector3.MoveTowards(paulaPos, new Vector3(3.42f, -2.32f), 0.125f);
				}
				else if (paulaPos != new Vector3(7.49f, -2.32f))
				{
					paula.SetFloat("dirX", 1f);
					paula.SetFloat("dirY", 0f);
					paulaPos = Vector3.MoveTowards(paulaPos, new Vector3(7.49f, -2.32f), 0.125f);
				}
				else
				{
					frames++;
					if (frames == 30)
					{
						state = 14;
						frames = 0;
						susie.ChangeDirection(Vector2.left);
						StartText(new string[2] { "* Okay,^05 like hell are\n  we gonna sit around\n  and do NOTHING.", "* Let's get going." }, new string[2] { "snd_txtsus", "snd_txtsus" }, new int[18], new string[2] { "su_annoyed", "su_side_sweat" }, 0);
					}
				}
				paula.transform.position = paulaPos + vector;
			}
		}
		if (state != 14)
		{
			return;
		}
		if ((bool)txt)
		{
			if (txt.GetCurrentStringNum() == 2)
			{
				susie.ChangeDirection(Vector2.down);
			}
			return;
		}
		frames++;
		if (frames == 1)
		{
			susie.DisableAnimator();
			susie.SetSprite("spr_su_kneel_front");
		}
		if (frames == 30)
		{
			gm.PlayGlobalSFX("sounds/snd_wing");
			kris.GetComponent<SpriteRenderer>().flipX = false;
			kris.EnableAnimator();
			kris.ChangeDirection(Vector2.down);
			susie.EnableAnimator();
			kris.SetSelfAnimControl(true);
			susie.SetSelfAnimControl(true);
			noelle.SetSelfAnimControl(true);
			EndCutscene();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (id == 0)
		{
			if (index == Vector2.left)
			{
				StartText(GetStringArray("post_gaster_2_choice0"), new string[7] { "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtnoe", "snd_txtpau", "snd_txtsus", "snd_txtpau" }, new int[18], new string[7] { "su_neutral", "su_side_sweat", "no_curious", "no_thinking", "pau_confused", "su_smirk_sweat", "pau_surprised" }, 0);
			}
			else
			{
				StartText(GetStringArray("post_gaster_2_choice1"), new string[5] { "snd_txtsus", "snd_txtsus", "snd_txtsus", "snd_txtnoe", "snd_txtpau" }, new int[18], new string[5] { "su_surprised", "su_side_sweat", "su_smile_sweat", "no_thinking", "pau_surprised" }, 0);
			}
			state = 8;
			frames = 0;
			selecting = false;
		}
		if (id == 1)
		{
			if (index == Vector2.left)
			{
				StartText(GetStringArray("paula_injured_1_choice0"), new string[2] { "snd_txtnoe", "snd_txtnoe" }, new int[18], new string[2] { "no_happy", "no_thinking" }, 0);
			}
			else
			{
				StartText(GetStringArray("paula_injured_1_choice1"), new string[2] { "snd_txtpau", "snd_txtpau" }, new int[18], new string[2] { "pau_confused", "pau_confident" }, 0);
			}
			selecting = false;
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		gm.SetCheckpoint(53, new Vector3(86.19f, 31.97f));
		paula = GameObject.Find("Paula").GetComponent<Animator>();
		paula.enabled = false;
		paula.GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_paula_firstcut_0");
		paula.transform.position = new Vector3(2.403f, 0.29f);
		table = GameObject.Find("Table").transform;
		table.GetComponent<SpriteRenderer>().enabled = true;
		GameObject.Find("RoomShading").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/eb_objects/spr_cabin_shading_door");
		GameObject.Find("CellDoor").GetComponent<SpriteRenderer>().enabled = true;
		kris.SetSelfAnimControl(false);
		susie.SetSelfAnimControl(false);
		noelle.SetSelfAnimControl(false);
		kris.ChangeDirection(Vector2.left);
		kris.GetComponent<Animator>().SetBool("isMoving", true);
		susie.ChangeDirection(Vector2.left);
		susie.GetComponent<Animator>().SetBool("isMoving", true);
		noelle.ChangeDirection(Vector2.left);
		noelle.GetComponent<Animator>().SetBool("isMoving", true);
		kris.transform.position = new Vector3(4f, -2.305f);
		susie.transform.position = new Vector3(5.25f, -2.14f);
		noelle.transform.position = new Vector3(6.5f, -2.1175f);
		geno = (int)gm.GetFlag(87) >= 5;
		if ((int)gm.GetFlag(12) == 1 && !geno)
		{
			WeirdChecker.Abort(gm);
		}
		paulaKnown = (int)gm.GetFlag(96) == 1;
		gm.SetFlag(103, 1);
	}
}
