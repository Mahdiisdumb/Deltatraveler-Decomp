using UnityEngine;
using UnityEngine.UI;

public class ShopUISansBase : ShopUIBase
{
	protected int bodyMoveFrames;

	protected Sprite[] bodySprites;

	private bool photobookCutscene;

	protected override void Awake()
	{
		base.Awake();
		string text = "ui/shop/sans/spr_sans_shop_body";
		bodySprites = new Sprite[3]
		{
			Resources.Load<Sprite>(text),
			Resources.Load<Sprite>(text + "_point_0"),
			Resources.Load<Sprite>(text + "_point_1")
		};
		if (Util.GameManager().GetFlagInt(293) != 1)
		{
			return;
		}
		if (Util.GameManager().GetFlagInt(303) == 0)
		{
			topic1 = "<color=#FFFF00FF>Photobook</color>";
			if (Util.GameManager().GetFlagInt(315) == 1)
			{
				topic1lines = new string[17]
				{
					"closed`* ...", "closed`* heh...", "wink`* you know me, kris.^10\n* i'm not much of a talker.", "side`* and when it comes to THAT\n  stuff,^05 well...", "closed`* ... let's just say i'm not\n  feeling any more chatty.", "concerned`* especially with susie and\n  noelle around.", "sad`* admittedly, it's...^15 pretty\n  tiring not rushing things here.^10\n* after so long.", "closed`* but the last thing any of us\n  need is for things to get\n  more complicated.", "neutral`* we've all dealt with enough\n  stress for one lifetime.", "closed`* and besides...",
					"empty`* maybe don't stick your nose\n  where it doesn't belong.", "empty`* you don't know what kind of\n  dark secrets you'll find.", "wink`* ... and regret knowing about.", "neutral`* now gimme back my key.", "neutral`* huh?^10\n* someone stole it from\n  you?", "empty`* well why didnt'cha kill\n  'em like you always\n  do?", "wink`* oops,^05 getting ahead of\n  myself."
				};
			}
			else
			{
				topic1lines = new string[15]
				{
					"closed`* ...", "closed`* heh...", "wink`* you know me, kris.^10\n* i'm not much of a talker.", "side`* and when it comes to THAT\n  stuff,^05 well...", "closed`* ... let's just say i'm not\n  feeling any more chatty.", "concerned`* especially with susie and\n  noelle around.", "sad`* admittedly, it's...^15 pretty\n  tiring not rushing things here.^10\n* after so long.", "closed`* but the last thing any of us\n  need is for things to get\n  more complicated.", "neutral`* we've all dealt with enough\n  stress for one lifetime.", "closed`* and besides...",
					"empty`* maybe don't stick your nose\n  where it doesn't belong.", "empty`* you don't know what kind of\n  dark secrets you'll find.", "wink`* ... and regret knowing about.", "neutral`* now gimme back my key.", "wink`* thank you."
				};
			}
		}
		else
		{
			SetToNah();
		}
	}

	protected override void Update()
	{
		base.Update();
		if (((state == 1 || state == 2) && index < 4) || (state == 6 && index < 8))
		{
			if (bodyMoveFrames < 15)
			{
				int num = bodyMoveFrames / 3;
				if (num > 2)
				{
					num = 2;
				}
				base.transform.Find("Sans").GetComponent<Image>().sprite = bodySprites[num];
				bodyMoveFrames++;
				base.transform.Find("Sans").localPosition = new Vector3(Mathf.Lerp(-1f, -69f, (float)bodyMoveFrames / 15f), 88f);
			}
		}
		else if (bodyMoveFrames > 0)
		{
			bodyMoveFrames--;
			base.transform.Find("Sans").localPosition = new Vector3(Mathf.Lerp(-1f, -69f, (float)bodyMoveFrames / 15f), 88f);
			base.transform.Find("Sans").GetComponent<Image>().sprite = bodySprites[0];
		}
	}

	protected override void StartFullTalk(string[] diag)
	{
		if (state == 3)
		{
			if (topic1 == "About yourself" && diag == topic1lines)
			{
				if ((int)Util.GameManager().GetFlag(92) == 0)
				{
					Util.GameManager().SetFlag(92, 1);
				}
				else
				{
					diag = new string[2] { "neutral`* there isn't much that i can\n  really tell you about me right\n  now.", "wink`* for all intents and purposes,^05i'm\n  just the convenient store guy." };
				}
			}
			else if (topic1.Contains("Photobook") && diag == topic1lines && Util.GameManager().GetFlagInt(303) == 0)
			{
				Util.GameManager().SetFlag(303, 1);
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
				Util.GameManager().PauseMusic();
				base.transform.Find("Vignette").GetComponent<Image>().enabled = true;
				photobookCutscene = true;
				SetToNah();
				OrganizeArrays();
			}
		}
		base.StartFullTalk(diag);
	}

	protected override void StartText(string txt)
	{
		int num = 0;
		string[] array = txt.Split('`');
		if (array.Length > 1)
		{
			num = 1;
			base.transform.Find("Sans").Find("Head").GetComponent<Image>()
				.enabled = true;
			base.transform.Find("Sans").Find("Head").GetComponent<Image>()
				.sprite = Resources.Load<Sprite>("ui/shop/sans/spr_sans_shop_face_" + array[0]);
		}
		else
		{
			base.transform.Find("Sans").Find("Head").GetComponent<Image>()
				.enabled = false;
		}
		if (photobookCutscene)
		{
			if (curString == 10)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_noise");
				Util.GameManager().ResumeMusic();
				base.transform.Find("Vignette").GetComponent<Image>().enabled = false;
			}
			if (curString == 14 && diag.Length == 15)
			{
				Util.GameManager().PlayGlobalSFX("sounds/snd_item");
			}
		}
		base.StartText(array[num]);
	}

	private void SetToNah()
	{
		topic1 = "Photobook";
		topic1lines = new string[1] { "empty`* nah." };
	}

	protected override string GetSellDenyText(int itemID)
	{
		switch (itemID)
		{
		case 0:
		case 1:
		case 2:
			return "empty`never seen \nthis, you \ndirty \nhacker.";
		case 8:
			return "closed`you should,\nuhh...\nkeep that.";
		case 16:
			return "side`you should \ngive this \nto papyrus \npersonally.";
		case 20:
			return "empty`i'm not \ntaking this \ncrap.";
		case 26:
		case 27:
			return "closed`pretty \nsure this \nis yours.";
		case 42:
			return "side`really???\nyou'll just\ndo that to\nhim?";
		default:
			if (!willBuyItems)
			{
				return "i'm not \ntaking any \nmore stuff.";
			}
			return base.GetSellDenyText(itemID);
		}
	}

	protected override void DetermineWillBuy(bool justSold)
	{
		if (Util.GameManager().GetFlagInt(285) < 0)
		{
			Util.GameManager().SetFlag(285, 99999);
		}
		if (justSold)
		{
			Util.GameManager().SetFlag(285, Util.GameManager().GetFlagInt(285) + 1);
		}
		int num = 3;
		willBuyItems = Util.GameManager().GetFlagInt(285) < num;
	}
}
