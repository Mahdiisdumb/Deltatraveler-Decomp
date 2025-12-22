using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PartyMembers : Object
{
	public struct PartyMember
	{
		public string name;

		public string shortName;

		public string pronoun;

		public Color neonColor;

		public int statPortraitFlag;

		public int starterWeapon;

		public int starterArmor;

		public bool[] allowedSlots;

		public PartyMember(string name, string shortName, string pronoun, Color neonColor, int statPortraitFlag, int starterWeapon, int starterArmor, bool[] allowedSlots)
		{
			this.name = name;
			this.shortName = shortName;
			this.pronoun = pronoun;
			this.neonColor = neonColor;
			this.statPortraitFlag = statPortraitFlag;
			this.starterWeapon = starterWeapon;
			this.starterArmor = starterArmor;
			this.allowedSlots = allowedSlots;
		}
	}

	public enum ID
	{
		None = -1,
		Kris = 0,
		Susie = 1,
		Noelle = 2,
		Paula = 3,
		Chara = 4,
		Sans = 5,
		Frisk = 6
	}

	private static readonly PartyMember[] partyMembers = new PartyMember[7]
	{
		new PartyMember("Kris", "kr", "they", Color.cyan, 0, 3, 4, new bool[6] { true, false, false, false, false, false }),
		new PartyMember("Susie", "su", "she", Color.magenta, 1, 6, 4, new bool[6] { false, true, false, false, false, false }),
		new PartyMember("Noelle", "no", "she", new Color(1f, 1f, 0f), 2, 8, 9, new bool[6] { false, true, true, false, false, false }),
		new PartyMember("Paula", "pau", "she", Color.red, -1, 20, 46, new bool[6] { false, false, false, true, false, false }),
		new PartyMember("Chara", "ch", "they", Color.green, -1, -1, -1, new bool[6] { true, false, false, true, false, false }),
		new PartyMember("Sans", "sans", "he", SOUL.SOUL_COLORS[1], -1, 47, 48, new bool[6] { false, false, true, false, false, false }),
		new PartyMember("Frisk", "fr", "they", Color.cyan, 0, 25, 4, new bool[6] { true, false, false, true, false, false })
	};

	private static int[] hp;

	private static int[] weapon;

	private static int[] armor;

	public static int GetNumPartyMembers()
	{
		return partyMembers.Length;
	}

	public static PartyMember GetPartyMember(int id)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return partyMembers[0];
		}
		return partyMembers[id];
	}

	public static PartyMember GetPartyMember(ID memberId)
	{
		return GetPartyMember((int)memberId);
	}

	public static string GetMemberName(int id, bool isSelfPOV = false, bool useCase = true)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			if (!useCase)
			{
				return "nobody";
			}
			return "Nobody";
		}
		if (isSelfPOV)
		{
			if (id != 4)
			{
				if (!useCase)
				{
					return "you";
				}
				return "You";
			}
			if (!useCase)
			{
				return "me";
			}
			return "I";
		}
		return partyMembers[id].name;
	}

	public static string GetMemberNamePossessive(int id, bool isSelfPOV = false, bool useCase = false)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			if (!useCase)
			{
				return "nobody's";
			}
			return "Nobody's";
		}
		if (isSelfPOV)
		{
			if (id != 4)
			{
				if (!useCase)
				{
					return "your";
				}
				return "Your";
			}
			if (!useCase)
			{
				return "my";
			}
			return "My";
		}
		return partyMembers[id].name + "'s";
	}

	public static string GetMemberPronoun(int id, bool isSelfPOV = false)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return "they";
		}
		if (isSelfPOV)
		{
			if (id != 4)
			{
				return "you";
			}
			return "I";
		}
		return partyMembers[id].pronoun;
	}

	public static Color GetMemberNeonColor(int id, bool allowOverride = true)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return Color.white;
		}
		if (allowOverride && id == 6 && Util.GameManager().GetFlagInt(108) == 1)
		{
			return UIBackground.borderColors[Util.GameManager().GetFlagInt(223)];
		}
		return partyMembers[id].neonColor;
	}

	public static string GetMemberNeonColorMenu(int id)
	{
		return ColorUtility.ToHtmlStringRGB(GetMemberNeonColor(id, allowOverride: false) + Color.white * 0.41f);
	}

	public static bool IsMemberAllowedInSlot(int id, int slot)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return true;
		}
		return partyMembers[id].allowedSlots[slot];
	}

	public static string GetMemberOWSpriteSuffix(int id, string customSuffix)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return customSuffix;
		}
		GameManager gameManager = Util.GameManager();
		if ((bool)Util.FindObjectOfType<UndyneShadow>())
		{
			return "undynes";
		}
		if (SceneManager.GetActiveScene().buildIndex == 123)
		{
			if (customSuffix == "unhappy")
			{
				return "unhappy_hd";
			}
			return "hd";
		}
		switch ((ID)id)
		{
		case ID.Kris:
			if (gameManager.GetPartyMember(3) == 3)
			{
				return partyMembers[3].shortName;
			}
			if (gameManager.GetSessionFlagInt(6) == 1)
			{
				return "depressed";
			}
			if (gameManager.GetFlagInt(102) == 1)
			{
				return "injured";
			}
			if (gameManager.GetFlagInt(204) == 1 && gameManager.GetFlagInt(178) == 1)
			{
				return "eyehold";
			}
			if (gameManager.GetFlagInt(204) == 1)
			{
				return "eye";
			}
			if (gameManager.GetFlagInt(178) == 1)
			{
				return "hold";
			}
			break;
		case ID.Susie:
			if (customSuffix == "unhappy" && gameManager.GetFlagInt(172) == 2)
			{
				return "depressed";
			}
			break;
		case ID.Noelle:
			if (customSuffix == "unhappy" && gameManager.GetFlagInt(172) >= 1)
			{
				return "depressed";
			}
			break;
		case ID.Frisk:
			if (gameManager.GetFlagInt(108) == 1 && gameManager.GetFlagInt(13) >= 2 && gameManager.GetFlagInt(127) == 1)
			{
				return "g";
			}
			break;
		}
		return customSuffix;
	}

	public static string GetMemberSpritePath(int id)
	{
		if (id == 3)
		{
			return "overworld/npcs/";
		}
		return "player/" + GetMemberName(id) + "/";
	}

	public static string GetMemberPanelSprite(int id, bool serious = false)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return "spr_kr_down_0";
		}
		GameManager gameManager = Util.GameManager();
		serious = serious || gameManager.GetFlagInt(13) >= 5 || (gameManager.GetFlagInt(13) == 4 && gameManager.GetFlagInt(87) == 4);
		switch ((ID)id)
		{
		case ID.Kris:
			if (gameManager.GetFlagInt(102) == 1)
			{
				return "injured/spr_kr_down_0_injured";
			}
			if (gameManager.GetFlagInt(204) == 1)
			{
				return "eye/spr_kr_down_0_eye";
			}
			break;
		case ID.Susie:
			if (serious)
			{
				return "unhappy/spr_su_down_0_unhappy";
			}
			break;
		case ID.Noelle:
			if (serious || gameManager.GetFlagInt(87) >= 4)
			{
				return "unhappy/spr_no_down_0_unhappy";
			}
			break;
		case ID.Frisk:
			if (gameManager.GetFlagInt(13) >= 2 && gameManager.GetFlagInt(127) == 1)
			{
				return "g/spr_fr_down_0_g";
			}
			break;
		}
		return "spr_" + partyMembers[id].shortName + "_down_0";
	}

	public static string GetMemberStatPortrait(int id)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return "portrait_default";
		}
		string text = Util.GameManager().GetFlagString(partyMembers[id].statPortraitFlag);
		if (text == "0")
		{
			text = "neutral";
		}
		return partyMembers[id].shortName + "_" + text;
	}

	public static string GetResponsibilityString(int firstMember, int secondMember)
	{
		int partyMember = Util.GameManager().GetPartyMember(0);
		int num = ((secondMember < 0) ? firstMember : ((firstMember < 0) ? secondMember : (-1)));
		if (num > -1)
		{
			return GetMemberName(num, num == partyMember);
		}
		int num2 = ((secondMember == partyMember) ? secondMember : ((firstMember == partyMember) ? firstMember : (-1)));
		int num3 = ((num2 == firstMember) ? secondMember : ((num2 == secondMember) ? firstMember : (-1)));
		if (num2 > -1 && num3 > -1)
		{
			if (num2 == 4)
			{
				return GetMemberName(num3) + " and I";
			}
			return "You and " + GetMemberName(num3, isSelfPOV: false, useCase: false);
		}
		return GetMemberName(firstMember) + " and " + GetMemberName(secondMember, isSelfPOV: false, useCase: false);
	}

	public static void SetDefaultValues()
	{
		hp = new int[partyMembers.Length];
		weapon = new int[partyMembers.Length];
		armor = new int[partyMembers.Length];
		for (int i = 0; i < partyMembers.Length; i++)
		{
			InitializePartyMember((ID)i);
		}
	}

	public static int GetMemberStarterWeapon(int id)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return -1;
		}
		return partyMembers[id].starterWeapon;
	}

	public static int GetMemberStarterArmor(int id)
	{
		if (id < 0 || id >= partyMembers.Length)
		{
			return -1;
		}
		return partyMembers[id].starterArmor;
	}

	public static int[] GetStarterWeaponArray()
	{
		int[] array = new int[partyMembers.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = partyMembers[i].starterWeapon;
		}
		return array;
	}

	public static int[] GetStarterArmorArray()
	{
		int[] array = new int[partyMembers.Length];
		for (int i = 0; i < array.Length; i++)
		{
			array[i] = partyMembers[i].starterArmor;
		}
		return array;
	}

	public static void InitializePartyMember(ID partyMember)
	{
		hp[(int)partyMember] = GetMaxHP((int)partyMember);
		weapon[(int)partyMember] = GetMemberStarterWeapon((int)partyMember);
		armor[(int)partyMember] = GetMemberStarterArmor((int)partyMember);
		int statPortraitFlag = GetPartyMember((int)partyMember).statPortraitFlag;
		if (statPortraitFlag > -1)
		{
			Util.GameManager().SetFlag(statPortraitFlag, "neutral");
		}
	}

	public static void SetAllHP(int[] n_hp, bool maxPartyMembers = false)
	{
		int num = ((n_hp.Length < partyMembers.Length) ? n_hp.Length : partyMembers.Length);
		List<int> list = new List<int>(Util.GameManager().GetParty());
		for (int i = 0; i < num; i++)
		{
			hp[i] = n_hp[i];
			if (maxPartyMembers && list.Contains(i) && hp[i] < GetMaxHP(i))
			{
				hp[i] = GetMaxHP(i);
			}
		}
	}

	public static void SetAllWeapon(int[] n_weapon)
	{
		int num = ((n_weapon.Length < partyMembers.Length) ? n_weapon.Length : partyMembers.Length);
		for (int i = 0; i < num; i++)
		{
			weapon[i] = n_weapon[i];
		}
	}

	public static void SetAllArmor(int[] n_armor)
	{
		int num = ((n_armor.Length < partyMembers.Length) ? n_armor.Length : partyMembers.Length);
		for (int i = 0; i < num; i++)
		{
			armor[i] = n_armor[i];
		}
	}

	public static int GetHP(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		return hp[partyMember];
	}

	public static int[] GetAllHP()
	{
		return hp;
	}

	public static int GetMaxHP(int partyMember)
	{
		return GetMaxHP(partyMember, Util.GameManager().GetEXP());
	}

	public static int GetMaxHP(int partyMember, int exp)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		int lV = Util.GameManager().GetLV(exp);
		return (ID)partyMember switch
		{
			ID.Kris => 20 + 4 * (lV - 1), 
			ID.Frisk => 20 + 4 * (lV - 1), 
			ID.Susie => 30 + 5 * (lV - 1), 
			ID.Noelle => 20 + Mathf.FloorToInt(3.3333333f * (float)(lV - 1)), 
			ID.Paula => 20, 
			ID.Sans => 10, 
			_ => 20, 
		};
	}

	public static int GetWeapon(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		return weapon[partyMember];
	}

	public static int GetArmor(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		return armor[partyMember];
	}

	public static int[] GetAllWeapon()
	{
		return weapon;
	}

	public static int[] GetAllArmor()
	{
		return armor;
	}

	public static void SetWeapon(int partyMember, int i)
	{
		if (partyMember >= 0 && partyMember < partyMembers.Length)
		{
			weapon[partyMember] = i;
		}
	}

	public static void SetArmor(int partyMember, int i)
	{
		if (partyMember >= 0 && partyMember < partyMembers.Length)
		{
			armor[partyMember] = i;
		}
	}

	public static void Heal(int partyMember, int heal)
	{
		if (partyMember >= 0 && partyMember < partyMembers.Length && hp[partyMember] <= GetMaxHP(partyMember))
		{
			hp[partyMember] += heal;
			if (hp[partyMember] > GetMaxHP(partyMember))
			{
				hp[partyMember] = GetMaxHP(partyMember);
			}
		}
	}

	public static void HealAll(int heal, bool includeOutOfParty = true)
	{
		List<int> list = new List<int>(Util.GameManager().GetParty());
		for (int i = 0; i < partyMembers.Length; i++)
		{
			if (includeOutOfParty || list.Contains(i))
			{
				Heal(i, heal);
			}
		}
	}

	public static void Damage(int partyMember, int dmg)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return;
		}
		int num = hp[partyMember];
		hp[partyMember] -= dmg;
		if ((bool)Util.FindObjectOfType<BattleManager>() && num > 0)
		{
			bool flag = partyMember == 0 || partyMember == 6 || partyMember == 4;
			if ((Util.FindObjectOfType<BattleManager>().IsSeriousMode() && hp[partyMember] <= 0 && flag && num > 1) || (Util.FindObjectOfType<BattleManager>().GetState() < 3 && hp[partyMember] <= 0))
			{
				hp[partyMember] = 1;
			}
		}
		if (hp[partyMember] <= 0)
		{
			hp[partyMember] = 0;
		}
		Util.GameManager().DetermineDeath();
	}

	public static void SetHP(int partyMember, int newHP, bool forceOverheal = false)
	{
		if (partyMember >= 0 && partyMember < partyMembers.Length && (!forceOverheal || newHP <= GetMaxHP(partyMember) || newHP >= hp[partyMember]))
		{
			hp[partyMember] = newHP;
			if (hp[partyMember] > GetMaxHP(partyMember) && !forceOverheal)
			{
				hp[partyMember] = GetMaxHP(partyMember);
			}
			if (hp[partyMember] <= 0)
			{
				hp[partyMember] = 0;
			}
			Util.GameManager().DetermineDeath();
		}
	}

	public static int GetATK(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		int num = Items.ItemValue(GetWeapon(partyMember), partyMember);
		return GetATKRaw(partyMember) + num;
	}

	public static int GetATKRaw(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		int lV = Util.GameManager().GetLV();
		return (ID)partyMember switch
		{
			ID.Kris => (lV - 1) * 2 - 6 * Util.GameManager().GetFlagInt(102), 
			ID.Frisk => (lV - 1) * 2, 
			ID.Susie => 2 + (lV - 1) * 2 + Mathf.FloorToInt((float)lV / 4f), 
			ID.Noelle => Mathf.RoundToInt((float)((lV - 1) * 4) / 3f), 
			ID.Paula => 3, 
			_ => 0, 
		};
	}

	public static int GetDEF(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		return GetDEFRaw(partyMember) + Items.ItemValue(GetArmor(partyMember));
	}

	public static int GetDEFRaw(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		int lV = Util.GameManager().GetLV();
		int num = 0;
		switch ((ID)partyMember)
		{
		case ID.Kris:
			num -= 6 * Util.GameManager().GetFlagInt(102);
			goto case ID.Susie;
		case ID.Susie:
		case ID.Noelle:
		case ID.Frisk:
			num += Mathf.FloorToInt((float)lV / 5f);
			break;
		}
		return num;
	}

	public static float GetMagic(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0f;
		}
		return GetMagicRaw(partyMember) + (float)GetMagicEquipment(partyMember);
	}

	public static int GetMagicEquipment(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0;
		}
		return Items.GetItemMagic(GetWeapon(partyMember)) + Items.GetItemMagic(GetArmor(partyMember));
	}

	public static float GetMagicRaw(int partyMember)
	{
		if (partyMember < 0 || partyMember >= partyMembers.Length)
		{
			return 0f;
		}
		int lV = Util.GameManager().GetLV();
		return (ID)partyMember switch
		{
			ID.Susie => 1f + (float)lV / 5f, 
			ID.Noelle => lV, 
			ID.Paula => 6f, 
			ID.Sans => 10f, 
			_ => 0f, 
		};
	}

	public static List<string> GetMemberNames(bool includeNobody = false)
	{
		List<string> list = new List<string>();
		if (includeNobody)
		{
			list.Add("Nobody");
		}
		for (int i = 0; i < GetNumPartyMembers(); i++)
		{
			list.Add(GetMemberName(i));
		}
		return list;
	}

	public static List<int> GetMemberIndices(bool includeNobody = false)
	{
		List<int> list = new List<int>();
		if (includeNobody)
		{
			list.Add(-1);
		}
		for (int i = 0; i < GetNumPartyMembers(); i++)
		{
			list.Add(i);
		}
		return list;
	}
}
