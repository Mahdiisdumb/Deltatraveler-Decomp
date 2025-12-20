using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class KeyItems : MonoBehaviour
{
	public struct KeyItem
	{
		private string name;

		private string description;

		private string dropText;

		public KeyItem(string name, string description, string dropText = "* (You felt that you shouldn't\n  drop this.)")
		{
			this.name = name;
			this.description = description;
			this.dropText = dropText;
		}

		public string GetName()
		{
			return name;
		}

		public string GetDescription()
		{
			return description;
		}

		public string GetDropText()
		{
			return dropText;
		}
	}

	public enum ID
	{
		Cellphone = 0,
		Cards = 1,
		Egg = 2,
		FranklinBadge = 3,
		Bomb = 4,
		LadderFirst = 5,
		LadderDeadEnd = 6,
		LadderGuard = 7,
		LadderChase = 8,
		SilverKey = 9,
		KrisKnife = 10
	}

	private static readonly KeyItem[] keyItems = new KeyItem[11]
	{
		new KeyItem("Cell Phone", "* \"Cell Phone\" - It allows you \n  to make and receive calls.", "* (But what if your mom calls?^05\n  You can't throw it away!)"),
		new KeyItem("Cards", "* \"Cards\" - The Jack of Spades,^05\n  and the Rules Card.", "* (You fumbled and caught them.^05\n  You can't throw these away!)"),
		new KeyItem("Egg", "* \"Egg\" - Not too important, not\n  too unimportant.", "* What Egg?"),
		new KeyItem("Franklin Badge", "* \"Franklin Badge\" - A unique\n  button badge that can reflect\n  lightning.", "pau_shocked`snd_txtpau`* This isn't yours?????"),
		new KeyItem("Bomb", "* \"Bomb\" - Bomb\n* It can blow up the seal east\n  of Happy Happy Village.}* ... You still wonder why Mr.\n  Carpainter had this.", "su_angry`snd_txtsus`* Hey,^05 hands off!!!"),
		new KeyItem("Ladder Piece", "* \"Ladder Piece\" - A removable\n  ladder piece.\n* A snowdrake was carrying this.", "su_angry`snd_txtsus`* Hey,^05 hands off!!!"),
		new KeyItem("Ladder Piece", "* \"Ladder Piece\" - This one was\n  found at a dead end.", "su_angry`snd_txtsus`* Hey,^05 hands off!!!"),
		new KeyItem("Ladder Piece", "* \"Ladder Piece\" - This one was\n  found being guarded by a\n  Feraldrake.", "su_angry`snd_txtsus`* Hey,^05 hands off!!!"),
		new KeyItem("Ladder Piece", "* \"Ladder Piece\" - This one was\n  sitting at the edge of\n  a cliff.", "su_angry`snd_txtsus`* Hey,^05 hands off!!!"),
		new KeyItem("Silver Key", "* \"Silver Key\" - A strange light\n  seems to radiate from its\n  silver body.", "* (Despite not knowing where\n  this goes,^05 you felt that you\n  shouldn't drop it.)"),
		new KeyItem("Antique Knife", "* \"Antique Knife\" - Weapon 20 AT\n* A reliable, well-worn blade.\n* One of your prized possessions.", "* You can't drop this!^10\n* It's too precious!")
	};

	private static readonly int[] callFlags = new int[7] { -1, 327, 328, 329, 330, 331, 332 };

	public static KeyItem GetKeyItem(int id)
	{
		if (id < 0)
		{
			return new KeyItem("[REDACTED]", "* You felt nauseous.");
		}
		return keyItems[id];
	}

	public static KeyItem GetKeyItem(ID id)
	{
		return GetKeyItem((int)id);
	}

	public static string[] UseItem(ID id, TextBox txt = null)
	{
		switch (id)
		{
		case ID.Cellphone:
		{
			if (Util.GameManager().GetSessionFlagInt(6) == 1)
			{
				return new string[1] { "* (You couldn't bring yourself\n  to dial.)" };
			}
			if (MapInfo.GetCurrentWorld() == World.LOSTCORE)
			{
				return new string[1] { "* (The phone won't turn on.)" };
			}
			List<string> list = new List<string> { "* You pushed buttons on the\n  Cell Phone randomly." };
			List<int> possiblePhoneCalls = GetPossiblePhoneCalls();
			int num = possiblePhoneCalls[Random.Range(0, possiblePhoneCalls.Count)];
			if (num > 0 && Util.GameManager().GetFlagInt(callFlags[num]) == 0)
			{
				Util.GameManager().SetFlag(callFlags[num], 1);
			}
			else
			{
				num = 0;
			}
			int num2 = 0;
			while (Localizer.HasText($"phone_random_{num}_{num2}"))
			{
				list.Add(Localizer.GetText($"phone_random_{num}_{num2++}"));
			}
			return list.ToArray();
		}
		case ID.Cards:
			return new string[1] { "* You held the Cards.^05\n* They felt flimsy between your\n  fingers." };
		case ID.Egg:
			Util.GameManager().PlayGlobalSFX("sounds/snd_egg");
			break;
		case ID.FranklinBadge:
			if (Util.GameManager().GetPartyMember(3) != 3)
			{
				return new string[1] { "* You admire the Franklin Badge.^05\n* The lightning insignia on it\n  shines brightly." };
			}
			return new string[2] { "pau_confused`snd_txtpau`* Hey,^05 the badge doesn't\n  really do anything.", "pau_neutral`snd_txtpau`* It's basically a shield\n  against lightning." };
		case ID.Bomb:
		{
			OverworldPlayer overworldPlayer = Util.OverworldPlayer();
			RaycastHit2D raycastHit2D = Physics2D.Raycast(overworldPlayer.transform.position, Util.OverworldPlayer().GetDirection(), 0.5f, -8197);
			if ((bool)raycastHit2D && (bool)raycastHit2D.collider.GetComponent<CaveSeal>())
			{
				raycastHit2D.collider.GetComponent<CaveSeal>().SetTalkable(txt);
				raycastHit2D.collider.GetComponent<CaveSeal>().DoInteract();
				string text = (((int)Util.GameManager().GetFlag(13) >= 5) ? "su_annoyed" : "su_confident");
				return new string[2]
				{
					"su_side`snd_txtsus`* Seems like the right\n  spot for this.",
					text + "`snd_txtsus`* Aight,^05 move aside."
				};
			}
			if (SceneManager.GetActiveScene().buildIndex == 54 && overworldPlayer.transform.position.x <= 5f)
			{
				return new string[2] { "su_side`snd_txtsus`* Y'know,^05 as much as\n  I'd like to know\n  what's over there...", "su_annoyed`snd_txtsus`* I think getting to\n  the grey door is\n  more important." };
			}
			return new string[1] { "* (The bomb shouldn't be set off\n  here.)" };
		}
		case ID.LadderFirst:
		case ID.LadderDeadEnd:
		case ID.LadderGuard:
		case ID.LadderChase:
			if (Util.GameManager().SusieInParty())
			{
				return new string[1] { "su_annoyed`snd_txtsus`* The hell do you\n  need this for?" };
			}
			return new string[1] { "* (Susie has the ladder piece.)" };
		case ID.SilverKey:
			return new string[1] { "* (Doesn't look like the key\n  can be used here.)" };
		case ID.KrisKnife:
			if (Util.GameManager().GetFlagInt(13) < 7)
			{
				return new string[1] { "* It's too sentimental to equip\n  it for battle." };
			}
			return new string[1] { "* The thought of using this to\n  fight fills you with an\n  anxious dread." };
		}
		return new string[1] { $"* You used the {GetName(id)}." };
	}

	public static string GetName(ID id)
	{
		return GetKeyItem(id).GetName();
	}

	public static string GetDescription(ID id)
	{
		if (id == ID.FranklinBadge && Util.GameManager().GetFlagInt(87) >= 5 && Util.GameManager().GetFlagInt(116) == 0)
		{
			return "* \"Franklin Badge\" - You can tell\n  it's called that because it's\n  written on the badge.";
		}
		if (id == ID.SilverKey && MapInfo.GetCurrentWorld() != World.UTIntermission1 && Util.GameManager().GetFlagInt(294) == 0)
		{
			return "* \"Silver Key\" - You get the odd\n  feeling that you won't get\n  any use out of this now.";
		}
		return GetKeyItem(id).GetDescription();
	}

	public static string GetDropText(ID id)
	{
		if (id == ID.Egg)
		{
			Util.GameManager().SetFlag(286, 0);
		}
		if (id == ID.FranklinBadge && Util.GameManager().GetFlagInt(87) >= 5)
		{
			if (Util.GameManager().GetFlagInt(154) != 0)
			{
				Util.GameManager().SetFlag(323, 1);
				return "* (You unpinned the Franklin Badge\n  from your shirt and threw it\n  away.)";
			}
			return "* (You felt that you shouldn't\n  drop this.)";
		}
		switch (id)
		{
		case ID.SilverKey:
			if (Util.GameManager().GetFlagInt(294) == 1 || Util.GameManager().GetFlagInt(299) == 1)
			{
				return "* (You felt it would be too\n  rude to throw this away.)";
			}
			Util.GameManager().SetFlag(292, 0);
			Util.GameManager().SetFlag(324, 1);
			return "* You throw away the Silver Key,^05\n  somehow knowing you have no\n  use for it anymore.";
		case ID.Cellphone:
			if (Util.GameManager().GetPartyMember(0) == 6)
			{
				return "* (You felt that you shouldn't\n  drop this.)";
			}
			break;
		}
		if ((id == ID.LadderChase || id == ID.LadderDeadEnd || id == ID.LadderFirst || id == ID.LadderGuard) && !Util.GameManager().SusieInParty())
		{
			return "* (Susie has the ladder piece.)";
		}
		return GetKeyItem(id).GetDropText();
	}

	public static List<ID> GetListOfKeyItems()
	{
		List<ID> list = new List<ID>();
		GameManager gameManager = Util.GameManager();
		if (gameManager.GetFlagInt(108) == 0)
		{
			list.Add(ID.Cellphone);
			list.Add(ID.Cards);
		}
		else if (gameManager.GetFlagInt(8) == 1)
		{
			list.Add(ID.Cellphone);
		}
		if (gameManager.GetFlagInt(286) == 1 && MapInfo.GetCurrentWorld() == World.Undertale)
		{
			list.Add(ID.Egg);
		}
		if (gameManager.GetPartyMember(3) == 3 || (gameManager.GetFlagInt(87) >= 5 && gameManager.GetFlagInt(106) != 0 && gameManager.GetFlagInt(173) == 0 && gameManager.GetFlagInt(323) == 0))
		{
			list.Add(ID.FranklinBadge);
		}
		if (gameManager.GetFlagInt(116) != 0 && gameManager.GetFlagInt(118) == 0)
		{
			list.Add(ID.Bomb);
		}
		if (gameManager.GetFlagInt(286) == 1 && MapInfo.GetCurrentWorld() == World.Earthbound)
		{
			list.Add(ID.Egg);
		}
		if (gameManager.GetFlagInt(205) != 0 && gameManager.GetFlagInt(209) == 0)
		{
			list.Add(ID.LadderFirst);
		}
		if (gameManager.GetFlagInt(227) == 1 && gameManager.GetFlagInt(209) == 0)
		{
			list.Add(ID.LadderDeadEnd);
		}
		if (gameManager.GetFlagInt(228) == 1 && gameManager.GetFlagInt(209) == 0)
		{
			list.Add(ID.LadderGuard);
		}
		if (gameManager.GetFlagInt(208) >= 1 && gameManager.GetFlagInt(209) == 0)
		{
			list.Add(ID.LadderChase);
		}
		if (gameManager.GetFlagInt(281) != 0)
		{
			list.Add(ID.KrisKnife);
		}
		if (gameManager.GetFlagInt(286) == 1 && MapInfo.GetCurrentWorld() == World.Underfell)
		{
			list.Add(ID.Egg);
		}
		if (gameManager.GetFlagInt(292) == 1 && gameManager.GetFlagInt(303) == 0 && gameManager.GetFlagInt(315) == 0)
		{
			list.Add(ID.SilverKey);
		}
		return list;
	}

	private static List<int> GetPossiblePhoneCalls()
	{
		List<int> list = new List<int> { 0 };
		if (MapInfo.GetCurrentWorld() == World.Undertale && Util.GameManager().GetFlagInt(60) == 0)
		{
			list.Add(1);
		}
		if (MapInfo.GetCurrentWorld() == World.Earthbound)
		{
			if (Util.GameManager().GetFlagInt(13) == 7)
			{
				list.Add(4);
			}
			else if (Util.GameManager().GetFlagInt(13) >= 6)
			{
				list.Add(5);
			}
			else
			{
				list.Add(2);
			}
			if (Util.GameManager().GetFlagInt(13) < 7)
			{
				list.Add(6);
			}
		}
		if (MapInfo.GetCurrentWorld() < World.Underfell)
		{
			list.Add(3);
		}
		return list;
	}
}
