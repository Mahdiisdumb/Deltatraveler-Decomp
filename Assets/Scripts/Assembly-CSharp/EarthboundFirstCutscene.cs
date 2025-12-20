using System.Collections.Generic;
using UnityEngine;

public class EarthboundFirstCutscene : CutsceneBase
{
	private TextBoxEB txtEB;

	private int textState;

	private int ringState;

	private bool needToPlayEquipText;

	private string oldWeapon = "Pencil";

	private int glowFrames;

	private List<SpriteRenderer> glowObjects = new List<SpriteRenderer>();

	private bool playNoelleGlow;

	private bool playKrisGlow;

	private SOUL soul;

	public override Dictionary<string, string[]> GetDefaultStrings()
	{
		Dictionary<string, string[]> dictionary = new Dictionary<string, string[]>();
		dictionary.Add("dark", new string[3] { "Ugh...", "...", "What the hell...?" });
		dictionary.Add("the_line_eb", new string[1] { "GOD DAMN IT KRIS\nWHERE THE HELL ARE\nWE NOW" });
		dictionary.Add("intro_0", new string[2] { "That grey door...", "It lead us to another world?" });
		dictionary.Add("intro_1", new string[2] { "I guess so.", "Wait a minute..." });
		dictionary.Add("intro_2", new string[1] { "WHERE'S THE DOOR??" });
		dictionary.Add("intro_3", new string[1] { "W-well,^05 we did fall for\na while." });
		dictionary.Add("intro_4", new string[2] { "Guess you're right.", "..." });
		dictionary.Add("intro_5", new string[2] { "Hey,^10 does anyone else feel...^30\nweird?", "Like,^05 there's something about\nthis place that feels...^20 really\noff." });
		dictionary.Add("intro_6", new string[1] { "Now that you mention it,^05\nI do feel a bit strange." });
		dictionary.Add("intro_7", new string[1] { "What about you,^10 Kris?" });
		dictionary.Add("ring_0", new string[3] { "...^20 My ring is glowing?", "What the heck???", "It IS glowing...!" });
		dictionary.Add("ring_1", new string[2] { "...^20 Wait,^05 why is my ring\nglowing on you...?", "Can we switch for a\nsecond?" });
		dictionary.Add("ring_2", new string[3] { "...^20 Hey,^05 what's that thing\nthat's glowing in your\npocket?", "... My ring???\n^20Why is it glowing?", "Can you give it\nto me?" });
		dictionary.Add("ring_3", new string[3] { "...^20 My ring is glowing?", "Wait,^05 I thought you took\nthis from me!", "What happened to the\n{0} that you\ngave me...?" });
		dictionary.Add("ring_swap", new string[1] { "(You and Noelle swapped\nweapons.)" });
		dictionary.Add("ring_equip", new string[1] { "(Noelle equipped the\nSnow Ring.)" });
		dictionary.Add("lightclear_0", new string[2] { "Kris...?", "What's with that glowing\naura that's coming out of\nyou?" });
		dictionary.Add("lightclear_1", new string[3] { "WAIT.", "Are we able to cast\nmagic here???", "Kris,^10 let's try casting\nsomething to make us\nfeel less weird!" });
		dictionary.Add("lightclear_2", new string[1] { "(Your SOUL shined its power\nonto Noelle.)" });
		dictionary.Add("lightclear_3", new string[1] { "(Noelle shined her light\nonto your SOUL.)" });
		dictionary.Add("lightclear_4", new string[1] { "You and Noelle cast LightClear!" });
		return dictionary;
	}

	private void Update()
	{
		glowFrames = (glowFrames + 1) % 30;
		List<SpriteRenderer> list = new List<SpriteRenderer>();
		if (playNoelleGlow && glowFrames == 0)
		{
			SpriteRenderer component = new GameObject("noelleGlow", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
			component.sprite = Resources.Load<Sprite>("player/Noelle/eb/spr_no_ring_look_glow");
			component.sortingOrder = 100;
			component.transform.position = new Vector3(3.076f, -1.473f);
			glowObjects.Add(component);
		}
		if (playKrisGlow && (glowFrames == 0 || glowFrames == 15))
		{
			SpriteRenderer component2 = new GameObject("krisGlow", typeof(SpriteRenderer)).GetComponent<SpriteRenderer>();
			component2.sprite = Resources.Load<Sprite>("player/Kris/eb/spr_kr_right_glow");
			component2.flipX = kris.GetComponent<SpriteRenderer>().flipX;
			component2.color = new Color(1f, 0f, 0f, 0.75f);
			component2.sortingOrder = -50;
			component2.transform.position = kris.transform.position;
			glowObjects.Add(component2);
		}
		for (int i = 0; i < glowObjects.Count; i++)
		{
			glowObjects[i].color = new Color(glowObjects[i].color.r, glowObjects[i].color.g, glowObjects[i].color.b, glowObjects[i].color.a - 0.025f);
			if (glowObjects[i].gameObject.name.StartsWith("kris"))
			{
				glowObjects[i].transform.localScale += new Vector3(0.015f, 0.015f, 0.015f);
			}
			else
			{
				glowObjects[i].transform.localScale += new Vector3(0.1f, 0.1f, 0.1f);
			}
			if (glowObjects[i].color.a <= 0f)
			{
				list.Add(glowObjects[i]);
			}
		}
		int count = list.Count;
		for (int j = 0; j < count; j++)
		{
			glowObjects.Remove(list[0]);
			Object.Destroy(list[0].gameObject);
		}
		if (state == 0 && !txtEB)
		{
			frames++;
			if (frames == 1)
			{
				gm.PlayMusic("music/mus_cave");
			}
			GameObject.Find("Black").GetComponent<SpriteRenderer>().color = Color.Lerp(Color.black, new Color(0f, 0f, 0f, 0f), (float)frames / 180f);
			if (frames == 200)
			{
				PlaySFX("sounds/snd_whip_hard");
				susie.SetSprite("eb/spr_su_pissed");
				noelle.SetSprite("eb/spr_no_right");
				noelle.GetComponent<SpriteRenderer>().flipX = true;
			}
			if (frames == 245)
			{
				state = 1;
				frames = 0;
				StartTextEB(GetStringArray("the_line_eb"), "snd_txtsus");
			}
		}
		if (state == 1)
		{
			if ((bool)txtEB)
			{
				return;
			}
			if (textState == 0)
			{
				StartTextEB(GetStringArray("intro_0"), "snd_txtnoe");
				textState++;
			}
			else if (textState == 1)
			{
				kris.SetSprite("eb/spr_kr_up");
				susie.SetSprite("eb/spr_su_right");
				StartTextEB(GetStringArray("intro_1"), "snd_txtsus");
				textState++;
			}
			else if (textState == 2)
			{
				frames++;
				if (frames == 15)
				{
					susie.GetComponent<SpriteRenderer>().flipX = true;
				}
				else if (frames == 30)
				{
					susie.GetComponent<SpriteRenderer>().flipX = false;
				}
				else if (frames == 45)
				{
					susie.SetSprite("eb/spr_su_down");
				}
				else if (frames == 60)
				{
					susie.SetSprite("eb/spr_su_up");
				}
				if (frames == 90)
				{
					frames = 0;
					StartTextEB(GetStringArray("intro_2"), "snd_txtsus");
					textState++;
				}
			}
			else if (textState == 3)
			{
				noelle.SetSprite("eb/spr_no_up");
				StartTextEB(GetStringArray("intro_3"), "snd_txtnoe");
				textState++;
			}
			else if (textState == 4)
			{
				StartTextEB(GetStringArray("intro_4"), "snd_txtsus");
				textState++;
			}
			else if (textState == 5)
			{
				susie.SetSprite("eb/spr_su_right");
				noelle.SetSprite("eb/spr_no_right");
				noelle.GetComponent<SpriteRenderer>().flipX = true;
				StartTextEB(GetStringArray("intro_5"), "snd_txtsus");
				textState++;
			}
			else if (textState == 6)
			{
				noelle.SetSprite("eb/spr_no_right");
				noelle.GetComponent<SpriteRenderer>().flipX = false;
				StartTextEB(GetStringArray("intro_6"), "snd_txtnoe");
				textState++;
			}
			else if (textState == 7)
			{
				frames++;
				if (frames == 1)
				{
					noelle.SetSprite("eb/spr_no_down");
				}
				noelle.transform.position = Vector3.Lerp(new Vector3(3.16f, -0.56f), new Vector3(3.16f, -1.12f), (float)frames / 60f);
				if (frames % 20 == 1)
				{
					noelle.GetComponent<SpriteRenderer>().flipX = !noelle.GetComponent<SpriteRenderer>().flipX;
				}
				if (frames == 60)
				{
					kris.SetSprite("eb/spr_kr_right");
					noelle.SetSprite("eb/spr_no_right");
					noelle.GetComponent<SpriteRenderer>().flipX = true;
					StartTextEB(GetStringArray("intro_7"), "snd_txtnoe");
					textState = 8;
					frames = 0;
				}
			}
			else if (textState == 8)
			{
				state = 2;
				textState = 0;
				needToPlayEquipText = true;
				StartTextEB(GetStringArrayFormatted("ring_" + ringState, oldWeapon), "snd_txtnoe");
			}
		}
		else
		{
			if (state != 2)
			{
				return;
			}
			if (!txtEB)
			{
				if (textState == 0)
				{
					if (needToPlayEquipText)
					{
						playNoelleGlow = true;
						needToPlayEquipText = false;
						PlaySFX("sounds/snd_equip_eb");
						noelle.SetSprite("eb/spr_no_ring_look");
						noelle.GetComponent<SpriteRenderer>().flipX = false;
						StartTextEB(GetStringArray((ringState == 1) ? "ring_swap" : "ring_equip"), "snd_txteb");
					}
					else
					{
						playKrisGlow = true;
						kris.GetComponent<SpriteRenderer>().flipX = true;
						StartTextEB(GetStringArray("lightclear_0"), "snd_txtsus");
						textState++;
					}
				}
				else if (textState == 1)
				{
					noelle.SetSprite("eb/spr_no_ring_look_shocked");
					kris.GetComponent<SpriteRenderer>().flipX = false;
					StartTextEB(GetStringArray("lightclear_1"), "snd_txtnoe");
					textState++;
				}
				else if (textState == 2)
				{
					playKrisGlow = false;
					kris.SetSprite("eb/spr_kr_point");
					Object.Instantiate(Resources.Load<GameObject>("vfx/SOULShine"), kris.transform.position - new Vector3(0f, 0.1f), Quaternion.identity);
					StartTextEB(GetStringArray("lightclear_2"), "snd_txteb");
					textState++;
				}
				else if (textState == 3)
				{
					noelle.GetComponent<SpriteRenderer>().flipX = false;
					soul = new GameObject("SOUL").AddComponent<SOUL>();
					soul.transform.position = new Vector3(1.9f, -1.49f);
					GameObject.Find("LightBarBase").transform.position = new Vector3(1.9f, -1.49f);
					soul.CreateSOUL(new Color(1f, 0f, 0f), false, false);
					soul.GetComponent<SpriteRenderer>().sortingOrder = 300;
					soul.Emanate(true);
					noelle.SetSprite("eb/spr_no_cast");
					StartTextEB(GetStringArray("lightclear_3"), "snd_txteb");
					textState++;
				}
				else if (textState == 4)
				{
					StartTextEB(GetStringArray("lightclear_4"), "snd_txteb");
					textState++;
					frames = 0;
				}
				else
				{
					if (textState != 5)
					{
						return;
					}
					frames++;
					if (frames == 1)
					{
						soul.Emanate(false);
						gm.PlayGlobalSFX("sounds/snd_revival");
					}
					SpriteRenderer[] componentsInChildren = GameObject.Find("LightBarBase").GetComponentsInChildren<SpriteRenderer>();
					foreach (SpriteRenderer spriteRenderer in componentsInChildren)
					{
						if (spriteRenderer.gameObject.name != "LightBarBase")
						{
							spriteRenderer.transform.localScale = new Vector3(1f, spriteRenderer.transform.localScale.y / 1.0005f, 1f);
						}
						else
						{
							spriteRenderer.transform.localScale *= 1.05f;
						}
						spriteRenderer.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, (float)frames / 180f);
					}
					if (frames == 150)
					{
						fade.FadeOut(60, Color.white);
					}
					if (frames == 220)
					{
						gm.ForceLoadArea(51);
					}
				}
			}
			else if (textState == 0 && ringState != 1 && ringState != 2)
			{
				if (needToPlayEquipText && txtEB.GetCurrentStringNum() == 1)
				{
					playNoelleGlow = true;
					needToPlayEquipText = false;
					noelle.SetSprite("eb/spr_no_ring_look");
					noelle.GetComponent<SpriteRenderer>().flipX = false;
				}
			}
			else if (textState == 2 && txtEB.GetCurrentStringNum() == 2 && playNoelleGlow)
			{
				gm.StopMusic(30f);
				noelle.SetSprite("eb/spr_no_right");
				noelle.GetComponent<SpriteRenderer>().flipX = true;
				playNoelleGlow = false;
			}
		}
	}

	private void StartTextEB(string[] dialogue, string sound)
	{
		txtEB = Object.Instantiate(Resources.Load<GameObject>("ui/TextBoxEB"), GameObject.Find("Canvas").transform).GetComponent<TextBoxEB>();
		txtEB.SetSound(sound);
		txtEB.StartTextBox(dialogue, false);
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		kris.DisableAnimator();
		susie.DisableAnimator();
		noelle.DisableAnimator();
		kris.SetSprite("eb/spr_kr_down");
		susie.SetSprite("eb/spr_su_down");
		noelle.SetSprite("eb/spr_no_down");
		fade.FadeIn(1, Color.black);
		if (gm.GetWeapon(2) == 8)
		{
			ringState = 0;
		}
		else if (gm.GetWeapon(0) == 8)
		{
			gm.ForceWeapon(0, gm.GetWeapon(2));
			gm.ForceWeapon(2, 8);
			ringState = 1;
		}
		else
		{
			bool flag = false;
			for (int i = 0; i < gm.GetItemList().Count; i++)
			{
				if (gm.GetItem(i) == 8)
				{
					gm.ChangeWeapon(2, i);
					flag = true;
					break;
				}
			}
			if (flag)
			{
				ringState = 2;
			}
			else
			{
				ringState = 3;
				oldWeapon = Items.ItemName(gm.GetWeapon(2));
				if (gm.NumItemFreeSpace() > 0)
				{
					gm.AddItem(gm.GetWeapon(2));
				}
				gm.ForceWeapon(2, 8);
			}
		}
		int num = gm.GetEBTextColorID();
		if ((int)gm.GetFlag(223) == 0)
		{
			gm.SetFlag(223, num);
		}
		else
		{
			num = (int)gm.GetFlag(223);
		}
		if (num > 0)
		{
			string text = "";
			switch (num)
			{
			case 1:
				text = "_mint";
				break;
			case 2:
				text += "_strawberry";
				break;
			case 3:
				text += "_banana";
				break;
			case 4:
				text += "_buttspie";
				break;
			case 5:
				text += "_blueberry";
				break;
			case 6:
				text += "_cinnamon";
				break;
			case 7:
				text += "_moss";
				break;
			case 8:
				text += "_cottoncandy";
				break;
			case 9:
				text += "_milkshake";
				break;
			case 10:
				text += "_eggplant";
				break;
			}
			SetSprite(GameObject.Find("Border").transform, "ui/spr_eb_border" + text);
		}
		gm.SetFlag(64, 1);
		gm.SetFramerate(60);
		StartTextEB(GetStringArray("dark"), "snd_txtsus");
	}
}
