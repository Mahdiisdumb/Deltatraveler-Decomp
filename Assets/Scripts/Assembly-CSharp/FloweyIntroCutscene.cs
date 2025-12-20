using UnityEngine;
using UnityEngine.UI;

public class FloweyIntroCutscene : CutsceneBase
{
	private SpriteRenderer flowey;

	private Sprite[] floweySprites;

	private Vector3 krisOrigPos;

	private Vector3 susieOrigPos;

	private Vector3 floweyOrigPos;

	private Transform fakeBattleUI;

	private Transform fakeTPBar;

	private bool susieSlash;

	private bool hardmode;

	private void Update()
	{
		if (!isPlaying)
		{
			return;
		}
		if (state == 0)
		{
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 13)
				{
					gm.StopMusic();
				}
			}
			else if (!txt)
			{
				frames++;
				if (frames == 1)
				{
					if (hardmode)
					{
						kris.ChangeDirection(Vector2.right);
					}
					else
					{
						kris.DisableAnimator();
						kris.SetSprite("spr_kr_right_0");
					}
					susie.DisableAnimator();
					susie.SetSprite("spr_su_crossed_right");
				}
				if (frames <= 10)
				{
					new GameObject("KrisAfterImage", typeof(SpriteRenderer), typeof(AfterImage)).GetComponent<AfterImage>().CreateAfterImage(kris.GetComponent<SpriteRenderer>().sprite, kris.transform.position);
					new GameObject("SusieAfterImage", typeof(SpriteRenderer), typeof(AfterImage)).GetComponent<AfterImage>().CreateAfterImage(susie.GetComponent<SpriteRenderer>().sprite, susie.transform.position);
					kris.transform.position = Vector3.Lerp(krisOrigPos, new Vector3(-4.6f, 2.126f), (float)frames / 10f);
					susie.transform.position = Vector3.Lerp(susieOrigPos, new Vector3(-4.479f, 0.292f), (float)frames / 10f);
					flowey.transform.position = Vector3.Lerp(floweyOrigPos, new Vector3(4.03f, 0.7f), (float)frames / 10f);
				}
				if (frames == 10)
				{
					PlaySFX("sounds/snd_weaponpull_fast");
					susie.transform.position = new Vector3(-3.733f, 0.772f);
					if (!hardmode)
					{
						kris.transform.position = new Vector3(-4.221f, 2.044f);
						kris.EnableAnimator();
						kris.SetSelfAnimControl(false);
						kris.GetComponent<Animator>().Play("DR Attack", 0, 0f);
					}
					susie.EnableAnimator();
					susie.SetSelfAnimControl(false);
					susie.GetComponent<Animator>().Play("DR Attack", 0, 0f);
				}
				if (frames == 24)
				{
					gm.PlayMusic("music/mus_battledelta");
					if (!hardmode)
					{
						kris.transform.position = new Vector3(-4.417f, 2.125f);
						kris.GetComponent<Animator>().Play("DR Idle", 0, 0f);
						StartText(new string[7] { "* What.", "* The hell are you doing?", "* We're...^10 in a battle?", "* How do you not know this???", "* No one's gonna know what\n  you're doing if you fight\n  like this,^05 idiot.", "* FINE!!!", "* If it'll get you to\n  SHUT UP,^05 then hurry up and\n  show us how YOU do it." }, new string[7] { "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtsus", "snd_txtsus" }, new int[10], new string[7] { "flowey_confused", "flowey_confused", "su_inquisitive", "su_angry", "flowey_sassy", "su_wtf", "su_angry" }, 1);
					}
					else
					{
						StartText(new string[7] { "* What.", "* The hell are you doing?", "* We're...^10 in a battle?", "* How do you not know this???", "* I'm pretty sure that your\n  friend there doesn't know\n  what's happening,^05 either.", "* FINE!!!", "* If it'll get you to\n  SHUT UP,^05 then hurry up and\n  show us how YOU do it." }, new string[7] { "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtsus", "snd_txtsus" }, new int[10], new string[7] { "flowey_confused", "flowey_confused", "su_inquisitive", "su_angry", "flowey_sassy", "su_wtf", "su_angry" }, 1);
					}
					susie.transform.position = new Vector3(-4.167f, 0.311f);
					susie.GetComponent<Animator>().Play("DR Idle", 0, 0f);
					Object.Destroy(GameObject.Find("menuBorder"));
					Object.Destroy(GameObject.Find("menuBox"));
					frames = 0;
					state = 1;
				}
			}
		}
		if (state == 1)
		{
			fakeBattleUI.localPosition = Vector3.Lerp(fakeBattleUI.localPosition, Vector3.zero, 0.4f);
			fakeTPBar.localPosition = Vector3.Lerp(fakeTPBar.localPosition, Vector3.zero, 0.4f);
			if ((bool)txt)
			{
				if (txt.GetCurrentStringNum() == 6 && !susieSlash)
				{
					susie.transform.position = new Vector3(-3.733f, 0.772f);
					susie.GetComponent<Animator>().Play("DR Attack", 0, 0f);
					PlaySFX("sounds/snd_attack");
					susieSlash = true;
				}
			}
			else
			{
				StartText(new string[1] { "* Don't worry,^05 this won't\n  take long at all." }, new string[1] { "snd_txtflw" }, new int[1], new string[1] { "flowey_sassy" }, 1);
				gm.StopMusic();
				state = 2;
			}
		}
		if (state == 2)
		{
			fakeBattleUI.localPosition = Vector3.Lerp(fakeBattleUI.localPosition, new Vector3(0f, -198f), 0.4f);
			fakeTPBar.localPosition = Vector3.Lerp(fakeTPBar.localPosition, new Vector3(-116f, 0f), 0.4f);
			if (!txt)
			{
				Object.Destroy(flowey.gameObject);
				Object.Destroy(fakeBattleUI.gameObject);
				Object.Destroy(fakeTPBar.gameObject);
				kris.InitiateBattle(1, new Vector3(-0.055f, -1.63f), 18);
				EndCutscene(false);
				state = 3;
			}
		}
	}

	private void LateUpdate()
	{
		if (state == 0 && (bool)txt && (bool)txt.GetPortrait() && txt.GetPortrait().sprite.name.Contains("flowey"))
		{
			flowey.sprite = floweySprites[int.Parse(txt.GetPortrait().sprite.name.Substring(txt.GetPortrait().sprite.name.Length - 1))];
		}
		if (state == 1 && (bool)txt)
		{
			if ((bool)txt.GetTextUT() && (bool)txt.GetTextUT().GetGameObject())
			{
				txt.GetTextUT().GetGameObject().transform.localPosition = new Vector3(78f, -203f);
				txt.GetTextUT().GetGameObject().GetComponent<Text>()
					.lineSpacing = 0.9f;
			}
			if ((bool)txt.GetPortrait())
			{
				txt.GetPortrait().transform.localPosition = new Vector3(-259f, -182f);
			}
		}
	}

	public override void StartCutscene(params object[] par)
	{
		base.StartCutscene(par);
		cam.SetFollowPlayer(false);
		krisOrigPos = kris.transform.position;
		susieOrigPos = susie.transform.position;
		if ((int)gm.GetFlag(108) == 1)
		{
			hardmode = true;
			StartText(new string[15]
			{
				"* Howdy!^15\n* I'm <color=#FFFF00FF>FLOWEY</color>.^15\n* <color=#FFFF00FF>FLOWEY</color> the <color=#FFFF00FF>FLOWER</color>!", "* Hmm...", "* You're new to the\n  UNDERGROUND,^15 aren'tcha?", "* Uhh...", "* Golly,^10 you must be\n  so confused.", "* Someone ought to teach\n  you how things work\n  around here!", "* Someone oughtta teach\n  you to mind your damn\n  business.", "* Hey,^10 that's no way to\n  talk to a kind-hearted\n  stranger!", "* So what?^10\n* Leave us alone and\n  get outta the way.", "* But...^10 don't you wanna\n  know about your SOUL?^15\n* About LOVE?",
				"* Dude,^10 why do you wanna\n  show us this stuff so\n  badly?", "* Do you WANT our SOULs\n  or something?", "* !!!^15\n* H-^05how'd you...?!", "* I didn't know.^10\n* YOU'RE the one who\n  said it.", "* C'mon,^05 let's teach this\n  idiot a lesson."
			}, new string[15]
			{
				"snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtflw", "snd_txtsus", "snd_txtflw",
				"snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtsus", "snd_txtsus"
			}, new int[18], new string[15]
			{
				"flowey_neutral", "flowey_neutral", "flowey_neutral", "su_inquisitive", "flowey_neutral", "flowey_neutral", "su_side", "flowey_sassy", "su_neutral", "flowey_earnest",
				"su_annoyed", "su_annoyed", "flowey_fear", "su_confident", "su_smile"
			}, 0);
		}
		else
		{
			StartText(new string[15]
			{
				"* Howdy!^15\n* I'm <color=#FFFF00FF>FLOWEY</color>.^15\n* <color=#FFFF00FF>FLOWEY</color> the <color=#FFFF00FF>FLOWER</color>!", "* Hmm...", "* You're new to the\n  UNDERGROUND,^15 aren'tcha?", "* Uhh...", "* Golly,^10 you must be\n  so confused.", "* Someone ought to teach\n  you how things work\n  around here!", "* Someone oughtta teach\n  you to mind your damn\n  business.", "* Hey,^10 that's no way to\n  talk to a kind-hearted\n  stranger!", "* So what?^10\n* Leave us alone and\n  get outta the way.", "* But...^10 don't you wanna\n  know about your SOUL?^15\n* About LOVE?",
				"* Dude,^10 why do you wanna\n  show us this stuff so\n  badly?", "* Do you WANT our SOULs\n  or something?", "* !!!^15\n* H-^05how'd you...?!", "* I didn't know.^10\n* YOU'RE the one who\n  said it.", "* Kris,^05 let's teach this\n  idiot a lesson."
			}, new string[15]
			{
				"snd_txtflw", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtflw", "snd_txtflw", "snd_txtsus", "snd_txtflw", "snd_txtsus", "snd_txtflw",
				"snd_txtsus", "snd_txtsus", "snd_txtflw", "snd_txtsus", "snd_txtsus"
			}, new int[18], new string[15]
			{
				"flowey_neutral", "flowey_neutral", "flowey_neutral", "su_inquisitive", "flowey_neutral", "flowey_neutral", "su_side", "flowey_sassy", "su_neutral", "flowey_earnest",
				"su_annoyed", "su_annoyed", "flowey_fear", "su_confident", "su_smile"
			}, 0);
		}
		gm.PlayMusic("music/mus_flowey");
		flowey = GameObject.Find("Flowey").GetComponent<SpriteRenderer>();
		floweyOrigPos = flowey.transform.position;
		floweySprites = new Sprite[2]
		{
			Resources.Load<Sprite>("overworld/npcs/spr_flowey_0"),
			Resources.Load<Sprite>("overworld/npcs/spr_flowey_1")
		};
		fakeBattleUI = GameObject.Find("FakeBattleUI").transform;
		if (hardmode)
		{
			fakeBattleUI.GetComponent<Image>().sprite = Resources.Load<Sprite>("battle/spr_fake_battle_ui_hard");
			fakeBattleUI.GetChild(0).GetComponent<Image>().color = UIBackground.borderColors[(int)gm.GetFlag(223)];
		}
		fakeTPBar = GameObject.Find("FakeTP").transform;
	}
}
