using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InteractHintMan : InteractSelectionBase
{
	[SerializeField]
	private Sprite[] sprites;

	private bool talkingFromAbove;

	private bool playEggSound;

	protected UIBackground shopBG;

	protected override void Update()
	{
		base.Update();
		if (!txt && (bool)shopBG)
		{
			Object.Destroy(shopBG.gameObject);
		}
		else if ((bool)txt && txt.GetCurrentStringNum() == 6 && playEggSound)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_egg");
			playEggSound = false;
		}
		if (!selectActivated && !txt && talkingFromAbove)
		{
			talkingFromAbove = false;
			GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
	}

	public override void DoInteract()
	{
		if ((bool)txt || !enabled)
		{
			return;
		}
		txt = new GameObject("InteractHintMan", typeof(TextBox)).GetComponent<TextBox>();
		if ((int)Util.GameManager().GetFlag(152) == 1)
		{
			txt.CreateBox(new string[3] { "* To be honest,^05 I started this\n  job in the hopes to see\n  something interesting.", "* You may have given me the\n  best reward that I could\n  ask for.", "* But don't you have anything\n  better that you could be\n  doing?" });
			Util.GameManager().DisablePlayerMovement(false);
		}
		else if ((int)Util.GameManager().GetFlag(151) == 1)
		{
			List<string> list = new List<string> { "* Now what's this...?", "* You came over here to show\n  me this little cave mole?", "* I never even told you that\n  I liked moles...", "* I suppose I could reward you\n  with something..." };
			if (Util.GameManager().NumItemFreeSpace() == 0)
			{
				list.Add("* But you don't have any\n  free space,^05 so come back\n  later,^05 okay?");
				txt.CreateBox(list.ToArray());
			}
			else
			{
				Util.GameManager().SetFlag(152, 1);
				playEggSound = true;
				list.AddRange(new string[3] { "* Something that a strange man\n  wanted to pay me with for\n  a hint.", "* (You got the Egg.)", "* (... It's just an\n  egg?)" });
				if ((int)Util.GameManager().GetFlag(153) == 1)
				{
					list.AddRange(new string[2] { "* But didn't we see\n  a bunch of eggs\n  earlier?", "* This isn't really\n  all that groundbreaking." });
				}
				txt.CreateBox(list.ToArray(), new string[9] { "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtnoe", "snd_txtsus", "snd_txtsus" }, new int[1], true, new string[9] { "", "", "", "", "", "", "no_confused", "su_smirk_sweat", "su_annoyed" });
				Util.GameManager().AddItem(16);
			}
			Util.GameManager().DisablePlayerMovement(false);
		}
		else
		{
			string[] array = new string[3] { "* Now wait a minute,^05 youngster!", "* I could give you a great\n  hint for just 30G.", "* You'd like a hint,^05 wouldn't\n  you?" };
			talkingFromAbove = Object.FindObjectOfType<OverworldPlayer>().transform.position.y > base.transform.position.y;
			if (talkingFromAbove)
			{
				GetComponent<SpriteRenderer>().sprite = sprites[1];
				array[0] = "* How unorthodox...^05\n* You must REALLY need a\n  hint.";
				array[1] = "* I could give you a great\n  hint for just 20G.";
			}
			shopBG = new GameObject("ShopMenu").AddComponent<UIBackground>();
			shopBG.transform.parent = GameObject.Find("Canvas").transform;
			shopBG.CreateElement("space", new Vector2(189f, 2f), new Vector2(202f, 108f));
			Text component = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), shopBG.transform).GetComponent<Text>();
			component.gameObject.name = "SpaceInfo";
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			component.transform.localPosition = new Vector3(116f, -71f);
			component.text = "$ - " + Object.FindObjectOfType<GameManager>().GetGold() + "G\nSPACE - " + (8 - Object.FindObjectOfType<GameManager>().NumItemFreeSpace()) + "/8";
			component.lineSpacing = 1.3f;
			txt.CreateBox(array, false);
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			txt.EnableSelectionAtEnd();
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			int num = (talkingFromAbove ? 20 : 30);
			if (Object.FindObjectOfType<GameManager>().GetGold() < num)
			{
				List<string> list = new List<string>();
				list.Add("* You don't have enough\n  money for one!");
				list.Add("* Though...^05 a young person\n  like you is very unusual\n  these days.");
				list.Add("* If you happen to need a\n  hint and have enough money,^05\n  c'mon back!");
				list.Add("* I'm here all the time...");
				if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 3)
				{
					list.Add("* ...Unless you're able to\n  draw blood.");
				}
				txt = new GameObject("InteractHintMan", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(list.ToArray(), true);
			}
			else
			{
				GameManager gameManager = Util.GameManager();
				gameManager.RemoveGold(num);
				shopBG.transform.Find("SpaceInfo").GetComponent<Text>().text = "$ - " + gameManager.GetGold() + "G\nSPACE - " + (8 - gameManager.NumItemFreeSpace()) + "/8";
				bool flag = (int)gameManager.GetFlag(87) >= 5 && (int)gameManager.GetFlag(13) == 0;
				List<string> list2 = new List<string>();
				string[] array = new string[1] { "* Error i wasted your\n  monie" };
				array = (((int)gameManager.GetFlag(13) >= 3) ? new string[4] { "* Have you ever considered\n  sparing enemies in battle?", "* One of the easiest ways\n  of doing so is using\n  Sleep Mist on a Mobile Sprout.", "* Or you could convince an\n  Explosive Oak to not fight and\n  it'll leave the battle.", "* A lot of the Peaceful Rest\n  Valley creatures respond well\n  to peace." } : (((int)gameManager.GetFlag(118) == 1) ? new string[2] { "* The way forward was already\n  blown away.", "* What are you doing here?^05\n* Go on and return home!" } : (((int)gameManager.GetFlag(116) != 0) ? new string[2] { "* Perhaps there's a sealed cave\n  somewhere that could be\n  blown away around the village.", "* After all,^05 you just got a\n  bomb,^05 didn't you...?" } : (((int)gameManager.GetFlag(103) == 1) ? (flag ? new string[5] { "* I just saw Paula run by,^05\n  warning me about murderers\n  on the loose.", "* I didn't have the heart\n  to tell her...", "* That perhaps explains the\n  <color=#FFFF00FF>Franklin Badge</color> that you're\n  wearing.", "* It can reflect lightning,^05 and\n  may offer as a help in\n  your travels.", "* Do you remember a place\n  where lightning strikes?" } : new string[2] { "* You know,^05 I'm happy to\n  have customers,^05 but don't\n  you have something to do?", "* You shouldn't be wasting\n  time here." }) : (((int)gameManager.GetPersistentFlag(0) == 1) ? new string[2] { "* Do you remember seeing a\n  cabin on the way through\n  Peaceful Rest Valley?", "* There must be a cave in\n  Happy Happy Village that\n  leads to that cabin." } : (((int)Util.GameManager().GetFlag(97) != 0) ? new string[3] { "* I've heard around that a\n  girl named Paula went missing.", "* Rumor has it that she\n  was taken across the\n  Peaceful Rest Valley.", "* Perhaps you'll find something\n  in Happy Happy Village...?" } : new string[3] { "* Have you been to the center\n  of Happy Happy Village?", "* Or are you just offput by\n  the blue color?", "* Because you look like the\n  kind of person to really\n  like blue." }))))));
				if (flag && (int)gameManager.GetFlag(130) == 0)
				{
					gameManager.SetFlag(130, 1);
					list2.AddRange(new string[5] { "* Oh ho!^05\n* You've redeemed yourselves.", "* By sparing your enemies,^05\n  you decided to make ammends.", "* You've lost the ability to\n  draw blood.", "* But that's not what's\n  important right now.", "* You want a hint,^05 so I'll\n  give you one!" });
				}
				list2.AddRange(array);
				txt = new GameObject("InteractHintMan", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(list2.ToArray(), true);
			}
		}
		else if (index == Vector2.right)
		{
			List<string> list3 = new List<string>();
			if (talkingFromAbove)
			{
				list3.Add("* Then why did you come\n  behind me??!");
				list3.Add("* You're either awfully confident\n  or 20G is too much to pay.");
			}
			else
			{
				list3.Add("* So you're telling me that\n  you don't want a hint?");
				list3.Add("* You're either awfully confident\n  or 30G is too much to pay.");
			}
			list3.Add("* If you happen to need a\n  hint,^05 c'mon back!");
			list3.Add("* I'm here all the time...");
			if ((int)Object.FindObjectOfType<GameManager>().GetFlag(13) >= 3)
			{
				list3.Add("* ...Unless you're able to\n  draw blood.");
			}
			txt = new GameObject("InteractHintMan", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(list3.ToArray(), true);
		}
		selectActivated = false;
	}
}
