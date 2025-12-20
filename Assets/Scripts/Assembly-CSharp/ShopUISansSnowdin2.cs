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
					"closed`*\tlooks like you guys took\n\tcare of evil me.", "sad`*\tsorry you had to deal with\n\thim.", "concerned`*\tit's hard to even imagine what\n\tkinda place he was in when\n\tyou met him.", "closed`*\tbut, uhh...^10 on an unrelated\n\tnote...", "neutral`*\ti think it's a bad idea\n\tto abuse ice magic.", "rolleye`*\ti mean,^05 plenty of folks here\n\thave frozen themselves to the\n\tfloor and had to call the guard\n\tto help.", "closed`*\tbut also...", "closed`*\tit's really powerful to master.", "neutral`*\tpower that requires incredible\n\tresponsibility to handle.", "closed`*\ti doubt you'd be able to\n\teven get close to mastering\n\tice magic on your journey.",
					"wink`*\tbut in your dreams,^05 you\n\tcould do anything.", "side`*\tactually,^05 uhh...", "closed`*\t...nevermind."
				};
			}
			else
			{
				topic4lines = new string[4] { "closed`*\tlooks like you guys took\n\tcare of evil me.", "sad`*\tsorry you had to deal with\n\thim.", "concerned`*\tit's hard to even imagine what\n\tkinda place he was in when\n\tyou met him.", "closed`*\t... but it's not hard for\n\tme to see how that could've\n\tstarted." };
			}
		}
		else if (Util.GameManager().GetFlagInt(318) == 1)
		{
			topic4lines = new string[6] { "closed`*\tlooks like you guys took\n\tcare of evil me.", "side`*\tthat turned out better than \n\ti expected.", "sad`*\tnot gonna lie,^05 i was thinking\n\tyou guys'd have to make \n\thim fall asleep.", "wink`*\tglad to see talking it out \n\tactually worked,^05 though.", "closed`*\tthat being said,^05 i feel if you went\n\tany other route,^05 that wouldn't\n\tbe possible.", "wink`*\tbut that's not something\n\tto worry about anymore.^10\n*\twhat's been done, ^10\n\t's been done." };
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
			StartFullTalk(new string[5] { "closed`*\tokay,^05 i talked with papyrus...", "wink`*\tunder a heavy disguise...", "rolleye`*\tsays he's pretty bored from\n\tpolishing a cannon.", "neutral`*\tso i can buy <color=#FFFF00FF>three things</color>\n\toff of you for now.", "wink`*\tbe sure to choose what\n\tyou wanna sell wisely." });
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
