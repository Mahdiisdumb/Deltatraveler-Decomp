using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
	public enum Type
	{
		Heal = 0,
		Weapon = 1,
		Armor = 2,
		Other = 3,
		AllHeal = 4
	}

	public struct Item
	{
		public string name;

		public string shortName;

		public string seriousName;

		public string desc;

		public Type type;

		public int value;

		public int sellPrice;

		public Item(string name, string shortName, string seriousName, string desc, Type type, int value, int sellPrice = -1)
		{
			this.name = name;
			this.shortName = shortName;
			this.seriousName = seriousName;
			this.desc = desc;
			this.type = type;
			this.value = value;
			this.sellPrice = sellPrice;
		}
	}

	public enum ID
	{
		None = -1,
		RedactedFood = 0,
		RedactedWeapon = 1,
		RedactedArmor = 2,
		Pencil = 3,
		Bandage = 4,
		ButterscotchPie = 5,
		BigPencil = 6,
		BandageHeal = 7,
		SnowRing = 8,
		Wristwatch = 9,
		MonsterCandy = 10,
		SpiderDonut = 11,
		SpiderCider = 12,
		ToyKnife = 13,
		FadedRibbon = 14,
		HeavyBranch = 15,
		OldEgg = 16,
		ChocolateCandy = 17,
		QuietShroom = 18,
		HardHat = 19,
		CleanPan = 20,
		CrackedBat = 21,
		SkipSandwich = 22,
		Hamburger = 23,
		Postcard = 24,
		Stick = 25,
		AntiqueKnife = 26,
		RealKnife = 27,
		SnailPie = 28,
		Bandana = 29,
		BoiledEgg = 30,
		AluminumBat = 31,
		ToughGlove = 32,
		ManlyBandana = 33,
		Permacicle = 34,
		Bisicle = 35,
		Unisicle = 36,
		CinnamonBun = 37,
		NiceCream = 38,
		Carrot = 39,
		BloodBandana = 40,
		BlusterBlade = 41,
		PapyrusCharm = 42,
		OldTutu = 43,
		Spaghetti = 44,
		WildReverseCard = 45,
		BigRibbon = 46,
		UntoughGloves = 47,
		PinkSlippers = 48
	}

	public enum WeaponType
	{
		Slash = 0,
		IceRing = 1,
		Quad = 2,
		Bash = 3,
		IceSlash = 4,
		Katana = 5
	}

	private static Item[] items = new Item[49]
	{
		new Item("[REDACTED]", "REDACTED", "REDACTED", "* Poison.", Type.AllHeal, -999),
		new Item("[REDACTED]", "REDACTED", "REDACTED", "* Useless.", Type.Weapon, -50),
		new Item("[REDACTED]", "REDACTED", "REDACTED", "* It feels like a million\n  shards of glass.", Type.Armor, -50),
		new Item("Pencil", "Pencil", "Pencil", "* Mightier than a sword?\n* Maybe equal at best.", Type.Weapon, 1, 30),
		new Item("Bandage", "Bandage", "Bandage", "* It has cartoon characters on\n  it.", Type.Armor, 1, 30),
		new Item("Butterscotch Pie", "ButtsPie", "Pie", "* Butterscotch-cinnamon\n  pie, one slice.", Type.Heal, 999, 100),
		new Item("Big Pencil", "BigPencil", "BigPencil", "* The eraser end is completely\n  bitten off.", Type.Weapon, 2, 1),
		new Item("Bandage", "Bandage", "Bandage", "* It has cartoon characters on\n  it.", Type.Heal, 10, 30),
		new Item("Snow Ring", "Snow Ring", "Snow Ring", "* For some reason, it feels\n  really cold in your hands.", Type.Weapon, 1),
		new Item("Wristwatch", "Wrstwatch", "Watch", "* Maybe an expensive antique.\n* Stuck before half past noon.", Type.Armor, 5, 200),
		new Item("Monster Candy", "MnstrCndy", "MnstrCndy", "* Has a distinct,\n^10  non-licorice flavor.", Type.Heal, 10, 15),
		new Item("Spider Donut", "SpidrDont", "SpdrDonut", "* A donut made with Spider\n  Cider in the batter.", Type.Heal, 12, 25),
		new Item("Spider Cider", "SpidrCidr", "SpdrCider", "* Made with whole spiders,\n  not just the juice.", Type.Heal, 24, 40),
		new Item("Toy Knife", "Toy Knife", "Toy Knife", "* Made of plastic.\n* A rarity nowadays.", Type.Weapon, 3, 50),
		new Item("Faded Ribbon", "Ribbon", "Ribbon", "* If you're cuter,^10 monsters\n  won't hit you as hard.", Type.Armor, 5, 50),
		new Item("Heavy Branch", "HevyBrnch", "Branch", "* A big branch straight\n  off the Snowdin trees.", Type.Weapon, 8, 2),
		new Item("Egg", "Egg", "Egg", "* Not too important, not\n  too unimportant.", Type.Other, 0),
		new Item("Chocolate Candy", "ChocCandy", "Chocolate", "* A rich,^05 dark chocolate treat.", Type.Heal, 26, 66),
		new Item("Quiet Shroom", "Shhhhroom", "QuiShroom", "* A reformed mushroom that is\n  neither ramblin' nor evil.", Type.Heal, 18, 5),
		new Item("Hard Hat", "Hard Hat", "Hard Hat", "* Construction cap intended for\n  construction sites.", Type.Armor, 8, 60),
		new Item("Clean Pan", "Clean Pan", "Clean Pan", "* A powerful,^05 non-burnt pan.\n* Has no passive effects.", Type.Weapon, 12),
		new Item("Cracked Bat", "CrackdBat", "CrackdBat", "* A light,^05 wooden bat with\n  a noticable crack on it.", Type.Weapon, 8, 60),
		new Item("Skip Sandwich", "SkpSndwch", "SkpSndwch", "* A sea-based sandwich that\n  increases SPEED in battle.}* Additionally,^05 eating it out of\n  battle will increase your\n  base speed for the room.", Type.Heal, 10, 27),
		new Item("Hamburger", "Hamburger", "Hamburger", "* A burger,^05 cooked to perfection.\n* Likely made of magic.", Type.Heal, 24, 50),
		new Item("Postcard", "Postcard", "Postcard", "A sight so awful\n  that Susie rips it up in anger,\n  temporarily giving her +10 AT.}* You can also view the card by\n  using it outside of battle.", Type.Other, 0, 5),
		new Item("Stick", "Stick", "Stick", "* Its bark is worse than\n  its bite.", Type.Weapon, 1),
		new Item("Antique Knife", "A. Knife", "AntqKnife", "* A well-worn blade belonging\n  to Susie's lost friend.", Type.Weapon, 15),
		new Item("Real Knife", "RealKnife", "RealKnife", "* Here we are!", Type.Weapon, 15),
		new Item("Snail Pie", "Snail Pie", "Snail Pie", "* An acquired taste.", Type.Heal, 999),
		new Item("Banana", "Banana", "Banana", "* Potassium.", Type.Heal, 25, 69),
		new Item("Boiled Egg", "BoiledEgg", "BoiledEgg", "* Finally,^05 an egg that you\n  can eat.", Type.Heal, 18, 6),
		new Item("Aluminum Bat", "Al Bat", "AlmnumBat", "* A powerful bat that's also\n  lightweight.", Type.Weapon, 8, 80),
		new Item("Tough Glove", "TuffGlove", "Glove", "* A worn pink leather glove.\n* For five-fingered folk.", Type.Weapon, 5, 75),
		new Item("Manly Bandanna", "Mandanna", "Bandanna", "* It has seen some wear.\n* It has abs drawn on it.}* The lower the wearer's HP goes,^05\n  the higher damage they'll deal.}* However,^05 they will deal less\n  damage when closer to full\n  health than normal.", Type.Armor, 3, 75),
		new Item("Permacicle", "Prmacicle", "Icicle", "* A magical icicle that doesn't\n  melt or smoke away.", Type.Weapon, 3, 20),
		new Item("Bisicle", "Bisicle", "Bisicle", "* It's a two-pronged popsicle,^05\n  so you can eat it twice.", Type.Heal, 11, 40),
		new Item("Unisicle", "Unisicle", "Unisicle", "* It's a SINGLE-pronged popsicle.\n* Wait,^05 that's just normal...", Type.Heal, 11, 20),
		new Item("Cinnamon Bun", "CinnaBun", "C. Bun", "* A cinnamon roll in the shape\n  of a bunny.", Type.Heal, 22, 30),
		new Item("Nice Cream", "NiceCream", "NiceCream", "* Instead of a joke,^05 the\n  wrapper says something nice.", Type.Heal, 15, 60),
		new Item("Carrot", "Carrot", "Carrot", "* Orange plant object.\n* Presumably worn by a snowman.", Type.Heal, 8, 8),
		new Item("Blood Bandana", "Bloodana", "BBandana", "* A bright red bandana that gives\n  off the essence of blood.}* Wearing this bandana will\n  increase the range bullets\n  increase tension.", Type.Armor, 5, 150),
		new Item("Bluster Blade", "BustBlade", "Katana", "* A katana infused with WIND\n  magic.", Type.Weapon, 12, 150),
		new Item("Papyrus Charm", "PapyCharm", "Charm", "* A shiny pendant bearing\n  Papyrus's likeness.}* If worn,^05 the wearer won't take\n  any damage when hit for the\n  first time in battle.}* This effect does not work in\n  overworld bullet segments.", Type.Armor, 3),
		new Item("Old Tutu", "Old Tutu", "Tutu", "* Its age is what makes it\n  so protective.", Type.Armor, 10, 100),
		new Item("Spaghetti", "Spaghetti", "Spaghett", "* A large pasta pot,^05 enough\n  for three servings.", Type.AllHeal, 15, 150),
		new Item("WILD REVERSE CARD", "REVRSCARD", "SkipCard", "* A strange,^05 non-standard UNO\n  card.}* In UNO,^05 it reverses player\n  order and changes color.^05\n* Skips enemy turn in battle.", Type.Other, 0, 250),
		new Item("Big Ribbon", "BigRibbon", "BigRibbon", "* A giant bow that makes\n  you cuter and more psychic.", Type.Armor, 5, 75),
		new Item("Untough Gloves", "UntufGlov", "Mittens", "* This isn't yours.", Type.Weapon, 0),
		new Item("Pink Slippers", "PinkSlips", "Slippers", "* This isn't yours.", Type.Armor, 1)
	};

	private static Dictionary<int, int> weaponTypes = new Dictionary<int, int>
	{
		{ 8, 1 },
		{ 20, 2 },
		{ 21, 3 },
		{ 32, 2 },
		{ 34, 4 },
		{ 41, 5 }
	};

	private static Dictionary<int, int> magicValue = new Dictionary<int, int>
	{
		{ 8, 5 },
		{ 34, 4 },
		{ 42, 3 },
		{ 46, 2 }
	};

	public static string ItemName(int i)
	{
		if (i == -1)
		{
			return "None";
		}
		return items[i].name;
	}

	public static string ShortItemName(int i, bool isBoss)
	{
		if (!isBoss)
		{
			return items[i].shortName;
		}
		return items[i].seriousName;
	}

	public static string ShortItemName(int i)
	{
		return ShortItemName(i, isBoss: false);
	}

	public static string ItemDescription(int i)
	{
		string text = "";
		if (ItemType(i) == 0)
		{
			text = ((i == 28) ? "Heals Some HP" : ((ItemValue(i) < 99) ? ("Heals " + ItemValue(i) + " HP") : "All HP"));
		}
		else if (ItemType(i) == 1)
		{
			text = ((GetItemMagic(i) <= 0) ? ("Weapon AT " + ItemValue(i)) : ("Wpn AT " + ItemValue(i) + " MG " + GetItemMagic(i)));
		}
		else if (ItemType(i) == 2)
		{
			text = ((GetItemMagic(i) <= 0) ? ("Armor DF " + ItemValue(i)) : ("Amr DF " + ItemValue(i) + " MG " + GetItemMagic(i)));
		}
		else if (ItemType(i) == 4)
		{
			text = ((ItemValue(i) != -999) ? ("Heals " + ItemValue(i) + " HP Each") : "-999 HP Each");
		}
		string text2 = items[i].desc;
		if (i == 7 && Util.GameManager().GetPartyMember(0) == 6)
		{
			text2 = "* It has already been used\n  several times.";
		}
		string text3 = "* \"" + ItemName(i) + "\" - " + text + "\n" + text2;
		if (text == "")
		{
			text3 = "* \"" + ItemName(i) + "\" - " + text2;
		}
		if (ItemType(i) == 1)
		{
			if (GetWeaponType(i) == 0)
			{
				text3 += "}* This weapon is a SLASH\n  type weapon.\n* One bar, standard damage.";
			}
			else if (GetWeaponType(i) == 1)
			{
				text3 += "}* This ICERING allows Noelle to\n  cast ICE spells when equipped.";
			}
			else if (GetWeaponType(i) == 2)
			{
				text3 += "}* This QUAD-type weapon uses\n  four bars instead of one.\n* More crits means more damage.";
				if (i == 32)
				{
					text3 += "}* However,^05 this specific weapon\n  has less incremental crit\n  damage than other QUAD-types.";
				}
			}
			else if (GetWeaponType(i) == 3)
			{
				text3 += "}* This is a BASH type weapon.\n* The bar progressively gets\n  faster with time.";
			}
			else if (GetWeaponType(i) == 4)
			{
				text3 += "}* This weapon is a ICESLASH\n  type weapon.\n* One bar, standard ICE damage.}* If equipped to Noelle,^05 she will\n  be able to cast ICE spells.}* However,^05 its effect on HEAL\n  PRAYER will be weaker than\n  other MAGIC weapons.";
			}
			else if (GetWeaponType(i) == 5)
			{
				text3 += "}* This blade has two bars.^05\n* One aims vertically,^05 the other\n  horizontally.}* Lining them both up perfectly\n  will damage all enemies on\n  screen.}* However,^05 hitting off-target is\n  penalized with less damage\n  than SLASH weapons.";
			}
		}
		if (i == 16)
		{
			text3 = "* \"Egg\" - Not too important, not\n  too unimportant.";
		}
		if (i == 45)
		{
			text3 = "* \"WILD REVERSE CARD\" - Card\n" + text2;
		}
		return text3;
	}

	public static int ItemType(int i)
	{
		if (i == -1)
		{
			return -1;
		}
		return (int)items[i].type;
	}

	public static int ItemValue(int i, int partyMember = 0)
	{
		switch (i)
		{
		case -1:
			return 0;
		case 23:
			if (partyMember == 2)
			{
				return items[i].value / 2;
			}
			break;
		}
		return items[i].value;
	}

	public static string ItemUse(int i, int from_slot, int to_slot, bool serious)
	{
		int partyMember = Util.GameManager().GetPartyMember(from_slot);
		int partyMember2 = Util.GameManager().GetPartyMember(to_slot);
		string memberName = PartyMembers.GetMemberName(partyMember, from_slot == 0);
		string memberName2 = PartyMembers.GetMemberName(partyMember2, to_slot == 0);
		string memberName3 = PartyMembers.GetMemberName(partyMember2, to_slot == 0, useCase: false);
		string memberPronoun = PartyMembers.GetMemberPronoun(partyMember2, to_slot == 0);
		string text = "* " + memberName;
		if (i == -21 || i == -22)
		{
			if (i == -21)
			{
				string text2 = "* " + memberName + " plays";
				if (from_slot == 0)
				{
					text2 = "* " + memberName + " play";
				}
				string[] array = new string[6] { "RED", "BLUE", "GREEN", "ORANGE", "CYAN", "YELLOW" };
				return text2 + " a WILD REVERSE.\n* Turn order has been reversed!\n* The color changes to " + array[Util.FindObjectOfType<SOUL>().GetSOULMode()] + "!";
			}
			string text3 = "* " + memberName + " tries to play";
			if (from_slot == 0)
			{
				text3 = "* " + memberName + " try to play";
			}
			return text3 + " the\n  WILD REVERSE, but the enemy\n  cannot be skipped!";
		}
		string text4 = ItemName(i);
		if ((partyMember2 == 1 || partyMember2 == 2) && (int)Util.GameManager().GetFlag(172) == 2)
		{
			serious = true;
		}
		if (partyMember2 == 2 && (int)Util.GameManager().GetFlag(172) == 1)
		{
			serious = true;
		}
		string text5 = "* " + memberName2 + " declined to equip the\n  " + ItemName(i) + ".";
		if (partyMember != partyMember2)
		{
			if (partyMember == 2 && i == 5)
			{
				text4 = "Pie";
			}
			text = text + " gave the " + text4 + "\n  to " + memberName3;
		}
		if (ItemType(i) == 0)
		{
			if (i == 38)
			{
				text = (new string[7] { "* You're just great!\n", "* You look nice today!\n", "* Are those claws natural?\n", "* You're super spiffy!\n", "* Have a wonderful day!\n", "* Is this as sweet as you?\n", "* Love yourself! I love you!\n" })[Random.Range(0, 7)];
				if (!serious)
				{
					switch (partyMember2)
					{
					case 1:
						text += "* Susie thought it was dumb.\n";
						break;
					case 2:
						if ((int)Util.GameManager().GetFlag(172) == 0)
						{
							text += "* Noelle's HAPPINESS increased!\n";
						}
						break;
					}
				}
			}
			else if (partyMember != partyMember2)
			{
				text = i switch
				{
					7 => text + " and reapplied it.\n", 
					12 => text + " and " + memberPronoun + " drank it.\n", 
					35 => "* " + memberName2 + " ate one half of the\n  Bisicle.\n", 
					_ => text + " and " + memberPronoun + " ate it.\n", 
				};
			}
			else
			{
				text = i switch
				{
					7 => text + " re-applied the bandage.\n", 
					12 => text + " drank the " + ItemName(i) + ".\n", 
					35 => text + " ate one half of the\n  Bisicle.\n", 
					_ => text + " ate the " + ItemName(i) + ".\n", 
				};
				if (partyMember == 0 && i == 5)
				{
					text += "* It tastes like home.\n";
				}
			}
			int num = ItemValue(i, partyMember2);
			if (i == 28 && partyMember2 != 1)
			{
				num = PartyMembers.GetMaxHP(partyMember2) - PartyMembers.GetHP(partyMember2) - 1;
			}
			if (i == 39 && partyMember2 == 2)
			{
				num = 999;
			}
			text = ((i != 28 || num >= 1) ? (text + GetRecoveryString(partyMember2, num)) : (text + "^10* ...^10 Gained no HP."));
		}
		else if (ItemType(i) == 1)
		{
			text = ((partyMember != partyMember2) ? (text + " and " + memberPronoun + " equipped it.\n") : ((ItemName(i).Length <= 13 && (ItemName(i).Length <= 11 || partyMember2 != 1) && (ItemName(i).Length <= 10 || partyMember2 != 2)) ? (text + " equipped the " + ItemName(i) + ".") : (text + " equipped the\n  " + ItemName(i) + ".")));
			if (i == 27)
			{
				text = "* How convenient.";
			}
			if (partyMember2 == 1)
			{
				text = i switch
				{
					20 => "su_depressed`snd_txtsus`* ...I'm not taking\n  this.", 
					27 => serious ? "su_pissed`snd_txtsus`* Stop pointing that\n  thing at me!!!" : "su_side_sweat`snd_txtsus`* (Can they stop pointing\n  that at me...?)", 
					_ => serious ? text5 : ((PartyMembers.GetWeapon(1) == -1) ? "su_annoyed`snd_txtsus`* No,^05 I'm NOT gonna\n  take anything." : "su_annoyed`snd_txtsus`* Umm,^05 I'm gonna keep\n  MY weapon."), 
				};
			}
			if (partyMember2 == 2 && i == 41)
			{
				text = (serious ? text5 : "no_happy`snd_txtnoe`* S-sorry Kris,^05 but that's\n  too heavy for me\n  to use...");
			}
			if (partyMember2 == 3)
			{
				text = (serious ? text5 : "pau_confused`snd_txtpau`* ... Why would I want\n  to use that instead\n  of my pan?");
			}
		}
		else if (ItemType(i) == 2)
		{
			text = ((partyMember != partyMember2) ? (text + " and " + memberPronoun + " equipped it.\n") : ((ItemName(i).Length <= 13 && (ItemName(i).Length <= 11 || partyMember2 != 1) && (ItemName(i).Length <= 10 || partyMember2 != 2)) ? (text + " equipped the " + ItemName(i) + ".") : (text + " equipped the\n  " + ItemName(i) + ".")));
			if (CanEquipItem((PartyMembers.ID)partyMember2, i) && PartyMembers.GetArmor(partyMember2) == 4 && Util.GameManager().NumItemFreeSpace(equipment: false) == 0)
			{
				text += "}* In trying to unequip the\n  Bandage,^05 you found that your\n  ITEMS are full...}* So you opted to place the\n  Bandage in your EQUIPMENT and\n  ruin your perfect organization.";
			}
			if (partyMember2 == 1 && !CanEquipItem((PartyMembers.ID)partyMember2, i))
			{
				text = (serious ? text5 : (i switch
				{
					43 => "su_annoyed`snd_txtsus`* Over my dead body.", 
					46 => "su_pissed`snd_txtsus`* Just because it's BIGGER\n  doesn't mean that I'd\n  want to wear it!!!", 
					14 => "su_annoyed`snd_txtsus`* No way.", 
					_ => text5, 
				}));
			}
			if (partyMember2 == 3 && !CanEquipItem((PartyMembers.ID)partyMember2, i) && i == 14 && !serious)
			{
				text = "pau_confident`snd_txtpau`* Sorry,^05 that ribbon's not\n  iconic enough.";
			}
		}
		else if (ItemType(i) == 4)
		{
			text = "* Everyone ate the " + ItemName(i) + "\n  and recovered " + ItemValue(i) + " HP each!";
		}
		else if (i == 24)
		{
			if (!Util.GameManager().SusieInParty())
			{
				text += " took out the card,^05\n  but Susie wasn't here\n  to destroy it...";
			}
			else
			{
				text = (((partyMember == 1 || partyMember2 == 1) && (partyMember != 1 || partyMember2 == 1)) ? "" : "* Susie glimpsed at the photo...\n");
				text += "* OOOORAAAAA!!!\n* Susie rips up the postcard!}* Susie's AT increased by 10!";
			}
		}
		else
		{
			text = text + " used the " + ItemName(i) + ".";
			if (ItemType(i) == 3)
			{
				text += "\n* Something occurred.";
			}
		}
		return text;
	}

	public static string ItemDrop(int i)
	{
		return "* The " + ItemName(i) + " was\n  thrown away.";
	}

	public static int NumOfItems()
	{
		return items.Length;
	}

	public static int GetHighestWeaponIndex()
	{
		int num;
		for (num = items.Length - 1; num >= 0; num--)
		{
			if (items[num].type == Type.Weapon)
			{
				return num;
			}
		}
		return num;
	}

	public static int GetHighestArmorIndex()
	{
		int num;
		for (num = items.Length - 1; num >= 0; num--)
		{
			if (items[num].type == Type.Armor)
			{
				return num;
			}
		}
		return num;
	}

	public static string GetRecoveryString(int partyMember, int hp)
	{
		bool isSelfPOV = Util.GameManager().GetPartyMember(0) == partyMember;
		string memberName = PartyMembers.GetMemberName(partyMember, isSelfPOV);
		string memberNamePossessive = PartyMembers.GetMemberNamePossessive(partyMember, isSelfPOV, useCase: true);
		if (PartyMembers.GetHP(partyMember) + hp >= PartyMembers.GetMaxHP(partyMember))
		{
			return "* " + memberNamePossessive + " HP was maxed out.";
		}
		return "* " + memberName + " recovered " + hp + " HP!";
	}

	public static int GetWeaponType(int i)
	{
		if (weaponTypes.ContainsKey(i))
		{
			return weaponTypes[i];
		}
		return 0;
	}

	public static string GetWeaponTypeName(int i)
	{
		string[] array = new string[6] { "SLASH", "ICERING", "QUAD", "BASH", "ICESLASH", "KATANA" };
		int weaponType = GetWeaponType(i);
		if (weaponType < array.Length)
		{
			return array[weaponType];
		}
		return "UNKNOWN (" + i + ")";
	}

	public static int GetItemMagic(int i)
	{
		if (magicValue.ContainsKey(i))
		{
			return magicValue[i];
		}
		return 0;
	}

	public static int GetItemElement(int i)
	{
		if (GetWeaponType(i) == 4 || GetWeaponType(i) == 1)
		{
			return 1;
		}
		return 0;
	}

	public static string GetBattleDescription(int i)
	{
		if (i < 0)
		{
			return "";
		}
		if (ItemType(i) == 0)
		{
			string text = ItemValue(i).ToString();
			switch (i)
			{
			case 5:
				text = "all";
				break;
			case 28:
				text = "the";
				break;
			}
			return "Heals " + text + " HP to one member";
		}
		if (ItemType(i) == 1)
		{
			return GetWeaponTypeName(i) + " Weapon (" + ItemValue(i) + " AT)";
		}
		if (ItemType(i) == 2)
		{
			return "Armor (" + ItemValue(i) + " DF)";
		}
		if (ItemType(i) == 4)
		{
			string text2 = ItemValue(i).ToString();
			return "Heals " + text2 + " HP to each member";
		}
		return i switch
		{
			24 => "Increases Susie's AT by 10", 
			45 => "Skips enemy turn", 
			_ => "", 
		};
	}

	public static List<Remark> GetUseRemarks(int item, int member)
	{
		List<Remark> list = new List<Remark>();
		GameManager gameManager = Util.GameManager();
		if (gameManager.GetFlagInt(87) >= 5)
		{
			return list;
		}
		switch (item)
		{
		case 3:
			if (member == 2)
			{
				list.Add(new Remark(1, "no_confused_side", "(It has bite marks\non it...)", new Vector2(400f, 50f)));
			}
			break;
		case 7:
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_side", "Guess this works...", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_thinking", "(This is so gross.)", new Vector2(400f, 66f)));
				break;
			case 3:
				list.Add(new Remark(1, "pau_dejected", "Why do you guys\neven have this...?", "bl"));
				list.Add(new Remark(1, "su_side", "I mean, it works.", "br"));
				break;
			}
			break;
		case 8:
			if (member == 0)
			{
				if (gameManager.SusieInParty())
				{
					list.Add(new Remark(1, "su_annoyed", "?????", new Vector2(160f, 50f)));
				}
				if (gameManager.NoelleInParty())
				{
					list.Add(new Remark(1, "no_confused", "... Do you know magic\nthat I don't???", new Vector2(400f, 50f)));
				}
			}
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_smirk_sweat", "Also, don't think \nthis'd fit.", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_thinking", "(Th-this was mine...)", new Vector2(400f, 50f)));
				break;
			}
			break;
		case 9:
			if (member == 0 && gameManager.NoelleInParty())
			{
				list.Add(new Remark(1, "no_thinking", "... Kris...?", new Vector2(400f, 50f)));
			}
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_smile", "It's clobbering time.", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_thinking", "(Th-this was mine...)", new Vector2(400f, 50f)));
				break;
			case 3:
				list.Add(new Remark(1, "pau_confused", "Am I supposed to\nlike... use this as\na shield or something?", new Vector2(380f, 50f)));
				break;
			}
			break;
		case 13:
			if (member == 2)
			{
				list.Add(new Remark(1, "no_playful", "Do you want me to\nbe like you or \nsomething??", new Vector2(400f, 50f)));
			}
			break;
		case 14:
			if (member == 2)
			{
				list.Add(new Remark(1, "no_blush", "Well...?\nHow do I look?", new Vector2(400f, 50f)));
			}
			else if (member == 0 && gameManager.NoelleInParty())
			{
				list.Add(new Remark(1, "no_silent_side", "...", new Vector2(400f, 50f)));
			}
			else if (member == 3)
			{
				list.Add(new Remark(1, "su_inquisitive", "... the hell is that \nsupposed to mean?", "br"));
			}
			break;
		case 19:
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_teeth_eyes", "No one will bash\nMY head in.", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_weird", "(It looks so silly\nsitting on my\nantlers...)", new Vector2(400f, 50f)));
				break;
			case 3:
				list.Add(new Remark(1, "pau_happy", "I feel so much\nmore protected!", "br"));
				break;
			}
			break;
		case 21:
		case 31:
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_smile_sweat", "(But damn, it does\nkinda look sick.)", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_playful", "Guess you could call\nme Dess with this\nthing, faha!", new Vector2(400f, 50f)));
				break;
			}
			break;
		case 27:
			if (member == 0 && gameManager.SusieInParty())
			{
				list.Add(new Remark(1, "su_side_sweat", "(Why're they gripping\nit so hard...?)", "br"));
			}
			break;
		case 32:
			if (member == 2)
			{
				list.Add(new Remark(1, "no_confused_side", "I'll... try my best!", new Vector2(400f, 50f)));
			}
			break;
		case 33:
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_angry", "I can already feel\nthe POWER!", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_mad", "...", new Vector2(400f, 50f)));
				break;
			}
			break;
		case 34:
			switch (member)
			{
			case 2:
				list.Add(new Remark(1, "no_thinking", "I can feel its\nchill flowing...", new Vector2(400f, 50f)));
				break;
			case 0:
				if (gameManager.NoelleInParty())
				{
					list.Add(new Remark(1, "no_curious", "(They look uncomfortable\nholding that.)", new Vector2(360f, 50f)));
				}
				break;
			}
			break;
		case 40:
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_teeth", "Get ready for some\nREAL tension.", "br"));
				break;
			case 2:
				list.Add(new Remark(1, "no_thinking", "(Why do I feel\ncolder wearing this?)", new Vector2(400f, 50f)));
				break;
			}
			break;
		case 41:
			if (member == 0)
			{
				if (gameManager.SusieInParty())
				{
					list.Add(new Remark(1, "su_smile_side", "Gotta say, that looks\nreal sick.", new Vector2(160f, 50f)));
				}
				if (gameManager.NoelleInParty())
				{
					list.Add(new Remark(1, "no_shocked", "I can feel it\nblowing this way.", new Vector2(400f, 50f)));
				}
			}
			break;
		case 42:
			switch (member)
			{
			case 1:
				list.Add(new Remark(1, "su_surprised", "Wait, this thing kinda\nfeels powerful, actually.", new Vector2(360f, 68f)));
				break;
			case 2:
				list.Add(new Remark(1, "no_happy", "Umm... nyeh heh heh?", new Vector2(400f, 50f)));
				break;
			}
			break;
		case 43:
			if (member == 0 && gameManager.SusieInParty())
			{
				list.Add(new Remark(1, "su_annoyed", "Looks as bad as\nI thought it would.", "br"));
			}
			else if (member == 2)
			{
				list.Add(new Remark(1, "no_weird", "This thing definitely\nneeds a wash.", new Vector2(400f, 50f)));
			}
			break;
		case 46:
			switch (member)
			{
			case 2:
			{
				bool flag2 = gameManager.GetPartyMember(3) == 3;
				if (gameManager.SusieInParty())
				{
					list.Add(new Remark(1, "su_flustered", "...", new Vector2(flag2 ? 64 : 160, 50f)));
				}
				list.Add(new Remark(1, "no_blush", "Well...?\nHow do I look?", new Vector2(flag2 ? 202 : 400, 50f)));
				if (flag2)
				{
					list.Add(new Remark(1, "pau_happy", "You looked wrapped\nup like a christmas\npresent, haha!", new Vector2(400f, 50f)));
				}
				break;
			}
			case 3:
				list.Add(new Remark(1, "pau_happy", "Right where it \nbelongs!", "br"));
				break;
			case 0:
			{
				bool flag = gameManager.GetPartyMember(3) == 3;
				if (gameManager.SusieInParty())
				{
					list.Add(new Remark(1, "su_annoyed", "Not gonna lie, doesn't\nreally fit you.", new Vector2(flag ? 64 : 160, 50f)));
				}
				if (flag)
				{
					list.Add(new Remark(1, "pau_annoyed", "You just don't\nsee the vision,\npurple lady.", new Vector2(290f, 50f)));
				}
				if (gameManager.NoelleInParty())
				{
					list.Add(new Remark(1, "no_silent_side", "...", new Vector2(flag ? 489 : 400, 50f)));
				}
				break;
			}
			}
			break;
		}
		return list;
	}

	public static int GetSellPrice(int i)
	{
		if (i >= items.Length || i < 0)
		{
			return -1;
		}
		return items[i].sellPrice;
	}

	public static bool IsEquipment(int item)
	{
		if (ItemType(item) != 1)
		{
			return ItemType(item) == 2;
		}
		return true;
	}

	public static List<int> GetItemsByType(int type, bool includeNone = false)
	{
		List<int> list = new List<int>();
		if (includeNone)
		{
			list.Add(-1);
		}
		for (int i = 0; i < NumOfItems(); i++)
		{
			if (items[i].type == (Type)type || type == -1)
			{
				list.Add(i);
			}
		}
		return list;
	}

	public static List<string> GetItemNamesByType(int type, bool includeNone = false)
	{
		List<string> list = new List<string>();
		if (includeNone)
		{
			list.Add("None");
		}
		for (int i = 0; i < NumOfItems(); i++)
		{
			if (items[i].type == (Type)type || type == -1)
			{
				list.Add(ItemName(i));
			}
		}
		return list;
	}

	public static bool CanEquipItem(PartyMembers.ID partyMember, int item)
	{
		if (ItemType(item) == 1)
		{
			switch (partyMember)
			{
			case PartyMembers.ID.Susie:
				return false;
			case PartyMembers.ID.Paula:
				return false;
			case PartyMembers.ID.Noelle:
				return item != 41;
			}
		}
		else if (ItemType(item) == 2)
		{
			switch (partyMember)
			{
			case PartyMembers.ID.Susie:
				if (item != 14 && item != 43)
				{
					return item != 46;
				}
				return false;
			case PartyMembers.ID.Paula:
				return item != 14;
			}
		}
		return true;
	}
}
