using UnityEngine;
using UnityEngine.UI;

public class ShopUISansSnowdin2 : ShopUISansBase
{
	private Sprite[] glowSprites;

	private bool fadingOut;

	protected override void Awake()
	{
		base.Awake();
		if ((int)Util.GameManager().GetFlag(87) >= 8)
		{
			item1price *= 5;
			item2price *= 4;
			item3price += 25;
			item4price += 30;
		}
		string text = "ui/shop/sans/spr_sans_shop_body";
		glowSprites = new Sprite[3]
		{
			Resources.Load<Sprite>(text + "_light"),
			Resources.Load<Sprite>(text + "_point_0_light"),
			Resources.Load<Sprite>(text + "_point_1_light")
		};
		sellMenuEnabled = true;
		Util.GameManager().PlayMusic("music/mus_shop", 0.95f);
		if (Util.GameManager().GetFlagInt(281) == 1)
		{
			if (Util.GameManager().GetFlagInt(282) == 1)
			{
				topic4lines = new string[13]
				{
					"closed`* looks like you guys took\n  care of evil me.", "sad`* sorry you had to deal with\n  him.", "concerned`* it's hard to even imagine what\n  kinda place he was in when\n  you met him.", "closed`* but, uhh...^10 on an unrelated\n  note...", "neutral`* i think it's a bad idea\n  to abuse ice magic.", "rolleye`* i mean,^05 plenty of folks here\n  have frozen themselves to the\n  floor and had to call the guard\n  to help.", "closed`* but also...", "closed`* it's really powerful to master.", "neutral`* power that requires incredible\n  responsibility to handle.", "closed`* i doubt you'd be able to\n  even get close to mastering\n  ice magic on your journey.",
					"wink`* but in your dreams,^05 you\n  could do anything.", "side`* actually,^05 uhh...", "closed`* ...nevermind."
				};
			}
			else
			{
				topic4lines = new string[4] { "closed`* looks like you guys took\n  care of evil me.", "sad`* sorry you had to deal with\n  him.", "concerned`* it's hard to even imagine what\n  kinda place he was in when\n  you met him.", "closed`* ... but it's not hard for\n  me to see how that could've\n  started." };
			}
		}
		else if (Util.GameManager().GetFlagInt(318) == 1)
		{
			topic4lines = new string[6] { "closed`* looks like you guys took\n  care of evil me.", "side`* that turned out better than \n  i expected.", "sad`* not gonna lie,^05 i was thinking\n  you guys'd have to make \n  him fall asleep.", "wink`* glad to see talking it out \n  actually worked,^05 though.", "closed`* that being said,^05 i feel if you went\n  any other route,^05 that wouldn't\n  be possible.", "wink`* but that's not something\n  to worry about anymore.^10\n* what's been done, ^10\n  's been done." };
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
				base.transform.Find("Sans").Find("Light").GetComponent<Image>()
					.sprite = glowSprites[num];
			}
		}
		else if (bodyMoveFrames > 0)
		{
			bodyMoveFrames--;
			base.transform.Find("Sans").Find("Light").GetComponent<Image>()
				.sprite = glowSprites[0];
		}
		if (state == 5 && !fadingOut)
		{
			Util.GameManager().StopMusic(10f);
			fadingOut = true;
		}
	}

	protected override void ToSellMenu()
	{
		if (Util.GameManager().GetFlagInt(284) == 0)
		{
			Util.GameManager().SetFlag(284, 1);
			endToState = 6;
			StartFullTalk(new string[5] { "closed`* okay,^05 i talked with papyrus...", "wink`* under a heavy disguise...", "rolleye`* says he's pretty bored from\n  polishing a cannon.", "neutral`* so i can buy <color=#FFFF00FF>three things</color>\n  off of you for now.", "wink`* be sure to choose what\n  you wanna sell wisely." });
		}
		else
		{
			base.transform.Find("Separator").GetComponent<Image>().enabled = true;
			base.transform.Find("Gold").GetComponent<Text>().enabled = true;
			base.transform.Find("Space").GetComponent<Text>().enabled = true;
			base.ToSellMenu();
		}
	}

	protected override void HandleExit(bool enableMovement)
	{
		Util.GameManager().PlayMusic("zoneMusic");
		base.HandleExit(enableMovement);
	}
}
