using System.Collections.Generic;
using UnityEngine;

public class FlagUtil : Object
{
	private static Dictionary<int, string> names = new Dictionary<int, string>
	{
		{ 0, "Kris Stat Portrait" },
		{ 1, "Susie Stat Portrait" },
		{ 2, "Noelle Stat Portrait" },
		{ 3, "First Cutscene Played" },
		{ 4, "Flowey cutscene played" },
		{ 5, "Toriel running cutscene" },
		{ 6, "Dummy Win State" },
		{ 7, "Susie's Patience (Dummy)" },
		{ 8, "Confronted Toriel" },
		{ 9, "Susie explained Running" },
		{ 10, "Froggit (Ruins_7) win state" },
		{ 11, "Froggit (Ruins_7) exhaust" },
		{ 12, "OBLIT Base" },
		{ 13, "OBLIT Confirm" },
		{ 14, "Candy Bowl" },
		{ 15, "Susie's Immunity to Cracks" },
		{ 16, "Rock Push Amount (Float)" },
		{ 17, "Toriel asked about flavors" },
		{ 18, "Flavor Preference" },
		{ 19, "Top Rock Push Amount" },
		{ 20, "Middle Rock Push Amount" },
		{ 21, "Bottom Rock \"Persuaded\"" },
		{ 22, "Two Froggits (Ruins_10) win state" },
		{ 23, "Two Froggits (Ruins_10) exhaust" },
		{ 24, "Whimsun (Ruins_9) win state" },
		{ 25, "Whimsun (Ruins_9) exhaust" },
		{ 26, "Froggit+Whimsun (Ruins_11) win state" },
		{ 27, "Froggit+Whimsun (Ruins_11) exhaust" },
		{ 28, "Moldsmal (Ruins_12) win state" },
		{ 29, "Moldsmal (Ruins_12) exhaust" },
		{ 30, "Talked to Napstablook" },
		{ 31, "Loox (Ruins_15) win state" },
		{ 32, "Loox (Ruins_15) exhaust" },
		{ 33, "Doobie Ruins" },
		{ 34, "Fall switch" },
		{ 35, "Vegetoid1 (Ruins_16) win state" },
		{ 36, "Vegetoid1 (Ruins_16) exhaust" },
		{ 37, "Vegetoid2 (Ruins_16) win state" },
		{ 38, "Vegetoid2 (Ruins_16) exhaust" },
		{ 39, "Picked up Faded Ribbon" },
		{ 40, "Spiral Switch 1" },
		{ 41, "Spiral Switch 2" },
		{ 42, "Spiral Switch 3" },
		{ 43, "Two Loox (Ruins_18) win state" },
		{ 44, "Two Loox (Ruins_18) exhaust" },
		{ 45, "Two Vegetoids (Ruins_19) win state" },
		{ 46, "Two Vegetoids (Ruins_19) exhaust" },
		{ 47, "Loox + Vegetoid (Ruins_20) win state" },
		{ 48, "Loox + Vegetoid (Ruins_20) exhaust" },
		{ 49, "Picked up Toy Knife" },
		{ 50, "<color=#00FF00>[MOSS]</color> Ruins" },
		{ 51, "Confronted Toriel outside" },
		{ 52, "Confronted Toriel inside" },
		{ 53, "Saw Dream Sequence" },
		{ 54, "Ruins OBLIT Failsafe" },
		{ 55, "Talked to Toriel in chair" },
		{ 56, "Toriel confronted us" },
		{ 57, "Flowey Boss Cutscene" },
		{ 58, "Flowey win state" },
		{ 59, "Noelle joined the party" },
		{ 60, "Confronted Sans" },
		{ 61, "Toriel called to check up" },
		{ 62, "Susie's Annoyance" },
		{ 63, "Was Given the Egg" },
		{ 64, "Section ID" },
		{ 65, "Transitioned to Earthbound" },
		{ 66, "Doobie PRV" },
		{ 67, "Shopped with Sans" },
		{ 68, "PRV Enemy Group 1 (Mobile Sprout Solo) win state" },
		{ 69, "PRV Enemy Group 1 (Mobile Sprout Solo) exhaust" },
		{ 70, "PRV Enemy Group 2 (Mobile Sprout Duo) win state" },
		{ 71, "PRV Enemy Group 2 (Mobile Sprout Duo) exhaust" },
		{ 72, "PRV Enemy Group 3 (UFO solo) win state" },
		{ 73, "PRV Enemy Group 3 (UFO solo) exhaust" },
		{ 74, "PRV Enemy Group 4 (Spinning Robo) win state" },
		{ 75, "PRV Enemy Group 4 (Spinning Robo) exhaust" },
		{ 76, "PRV Enemy Group 5 (2 UFO, 1 Sprout) win state" },
		{ 77, "PRV Enemy Group 5 (2 UFO, 1 Sprout) exhaust" },
		{ 78, "PRV Enemy Group 6 (Oak + Sprout) win state" },
		{ 79, "PRV Enemy Group 6 (Oak + Sprout) exhaust" },
		{ 80, "PRV Enemy Group 7 (Oak, Robo, UFO) win state" },
		{ 81, "PRV Enemy Group 7 (Oak, Robo, UFO) exhaust" },
		{ 82, "PRV Enemy Group 8 (2 Sprout, 1 oak) win state" },
		{ 83, "PRV Enemy Group 8 (2 Sprout, 1 oak) exhaust" },
		{ 84, "Real mom called us" },
		{ 85, "Pencil Statue Thrown" },
		{ 86, "Mini Party Member ID" },
		{ 87, "OBLIT Rememberance" },
		{ 88, "Quiet Shroom picked up" },
		{ 89, "Coil Snake win state" },
		{ 90, "Noelle spoke up" },
		{ 91, "Hard Hat taken" },
		{ 92, "Sans knows Kris's name" },
		{ 93, "Sans knows Susie's name" },
		{ 94, "TS!Underswap UI" },
		{ 95, "Napstablook OBLIT Failsafe" },
		{ 96, "We know about Paula" },
		{ 97, "Entered HHV" },
		{ 98, "First Cultist win state" },
		{ 99, "Doctor diagnosed Kris" },
		{ 100, "Dual Cultist win state" },
		{ 101, "Dual Cultist exhaust" },
		{ 102, "Concussion" },
		{ 103, "Visited Paula" },
		{ 104, "Confronted Porky" },
		{ 105, "Used a PSI move" },
		{ 106, "Porky Duo Cultist win state" },
		{ 107, "Frisk Mode" },
		{ 108, "Hard Mode" },
		{ 109, "Maze Cultists win state" },
		{ 110, "Cultist 1 moved" },
		{ 111, "Cultist 2 moved" },
		{ 112, "Cultist 3 moved" },
		{ 113, "Cultist 4 moved" },
		{ 114, "Opened the Sandwich box" },
		{ 115, "Paid the Creepy Lady" },
		{ 116, "Carpainter win state" },
		{ 117, "Carpainter is alive" },
		{ 118, "Cave wall blown up" },
		{ 119, "HM First Toriel confrontation" },
		{ 120, "Susie's Concern" },
		{ 121, "HM Water Sausages" },
		{ 122, "HM Discovered Kris' Knife" },
		{ 123, "HM BLADEKNIGHT confronted" },
		{ 124, "HM BLADEKNIGHT win state" },
		{ 125, "HM Kill Counter" },
		{ 126, "HM Traded Candy" },
		{ 127, "Napstablook win state" },
		{ 128, "Hardmode Completed (title death)" },
		{ 129, "Ice Shock Cutscene activated" },
		{ 130, "Hint Man knows you aborted" },
		{ 131, "LSCave Enemy Group 1 (Mole) win state" },
		{ 132, "LSCave Enemy Group 1 (Mole) kill counter" },
		{ 133, "LSCave Enemy Group 2 (Batty) win state" },
		{ 134, "LSCave Enemy Group 2 (Batty) kill counter" },
		{ 135, "LSCave Enemy Group 3 (Mole, Batty) win state" },
		{ 136, "LSCave Enemy Group 3 (Mole, Batty) kill counter" },
		{ 137, "LSCave Enemy Group 4 (Mole, Mole) win state" },
		{ 138, "LSCave Enemy Group 4 (Mole, Mole) kill counter" },
		{ 139, "LSCave Enemy Group 5 (Mole, Mole, Batty) win state" },
		{ 140, "LSCave Enemy Group 5 (Mole, Mole, Batty) kill counter" },
		{ 141, "LSCave Enemy Group 6 (Batty, Batty) win state" },
		{ 142, "LSCave Enemy Group 6 (Batty, Batty) kill counter" },
		{ 143, "LSCave Enemy Group 7 (Bear) win state" },
		{ 144, "LSCave Enemy Group 7 (Bear) kill counter" },
		{ 145, "LSCave Enemy Group 8 (Mole, Bear) win state" },
		{ 146, "LSCave Enemy Group 8 (Mole, Bear) kill counter" },
		{ 147, "LSCave Enemy Group 9 (Mole, Batty, Bear) win state" },
		{ 148, "LSCave Enemy Group 9 (Mole, Batty, Bear) kill counter" },
		{ 149, "Man in the Wall" },
		{ 150, "Mondo Mole win state" },
		{ 151, "Mole Friend Activation" },
		{ 152, "Hintman Egg" },
		{ 153, "Found the Picnic Basket" },
		{ 154, "Porky win state" },
		{ 155, "Eavesdropped on GG!UF!Toriel" },
		{ 156, "Generated Box Contents" },
		{ 157, "Box Item 1" },
		{ 158, "Box Item 2" },
		{ 159, "Box Item 3" },
		{ 160, "Box Item 4" },
		{ 161, "Box Item 5" },
		{ 162, "Box Item 6" },
		{ 163, "Box Item 7" },
		{ 164, "Box Item 8" },
		{ 165, "Box Item 9" },
		{ 166, "Box Item 10" },
		{ 167, "Fish Note Examined" },
		{ 168, "Transitioned to GG!Underfell" },
		{ 169, "Saw UF!Pap and UF!Sans" },
		{ 170, "Shayy Quest 1/2" },
		{ 171, "Shayy Quest 2/2" },
		{ 172, "Depression State" },
		{ 173, "Ness Paula win state" },
		{ 174, "Burger Present opened" },
		{ 175, "Dummy left" },
		{ 176, "Mismatch Check" },
		{ 177, "Last Saved Build" }
	};

	private static Dictionary<int, FlagType> types = new Dictionary<int, FlagType>
	{
		{
			0,
			FlagType.String
		},
		{
			1,
			FlagType.String
		},
		{
			2,
			FlagType.String
		},
		{
			6,
			FlagType.WinState
		},
		{
			10,
			FlagType.WinState
		},
		{
			13,
			FlagType.IntNumber
		},
		{
			14,
			FlagType.IntNumber
		},
		{
			16,
			FlagType.Float
		},
		{
			19,
			FlagType.Float
		},
		{
			20,
			FlagType.Float
		},
		{
			22,
			FlagType.WinState
		},
		{
			24,
			FlagType.WinState
		},
		{
			26,
			FlagType.WinState
		},
		{
			28,
			FlagType.WinState
		},
		{
			31,
			FlagType.WinState
		},
		{
			35,
			FlagType.WinState
		},
		{
			37,
			FlagType.WinState
		},
		{
			43,
			FlagType.WinState
		},
		{
			45,
			FlagType.WinState
		},
		{
			47,
			FlagType.WinState
		},
		{
			58,
			FlagType.WinState
		},
		{
			64,
			FlagType.IntNumber
		},
		{
			68,
			FlagType.WinState
		},
		{
			70,
			FlagType.WinState
		},
		{
			72,
			FlagType.WinState
		},
		{
			74,
			FlagType.WinState
		},
		{
			76,
			FlagType.WinState
		},
		{
			78,
			FlagType.WinState
		},
		{
			80,
			FlagType.WinState
		},
		{
			82,
			FlagType.WinState
		},
		{
			86,
			FlagType.MiniPartyMember
		},
		{
			87,
			FlagType.IntNumber
		},
		{
			89,
			FlagType.WinState
		},
		{
			98,
			FlagType.WinState
		},
		{
			100,
			FlagType.WinState
		},
		{
			106,
			FlagType.WinState
		},
		{
			109,
			FlagType.WinState
		},
		{
			116,
			FlagType.WinState
		},
		{
			124,
			FlagType.WinState
		},
		{
			125,
			FlagType.IntNumber
		},
		{
			127,
			FlagType.WinState
		},
		{
			131,
			FlagType.WinState
		},
		{
			133,
			FlagType.WinState
		},
		{
			135,
			FlagType.WinState
		},
		{
			137,
			FlagType.WinState
		},
		{
			139,
			FlagType.WinState
		},
		{
			141,
			FlagType.WinState
		},
		{
			143,
			FlagType.WinState
		},
		{
			145,
			FlagType.WinState
		},
		{
			147,
			FlagType.WinState
		},
		{
			150,
			FlagType.WinState
		},
		{
			154,
			FlagType.WinState
		},
		{
			157,
			FlagType.Item
		},
		{
			158,
			FlagType.Item
		},
		{
			159,
			FlagType.Item
		},
		{
			160,
			FlagType.Item
		},
		{
			161,
			FlagType.Item
		},
		{
			162,
			FlagType.Item
		},
		{
			163,
			FlagType.Item
		},
		{
			164,
			FlagType.Item
		},
		{
			165,
			FlagType.Item
		},
		{
			166,
			FlagType.Item
		},
		{
			177,
			FlagType.String
		}
	};

	private static string[] pnames = new string[1] { "Flowey Cutscene Fastmode" };

	public static string GetFlagName(int flag)
	{
		if (names.ContainsKey(flag))
		{
			return names[flag];
		}
		return "Unnamed";
	}

	public static FlagType GetFlagType(int flag)
	{
		if (types.ContainsKey(flag))
		{
			return types[flag];
		}
		return FlagType.IntBool;
	}

	public static string GetPFlagName(int flag)
	{
		if (flag < pnames.Length)
		{
			return pnames[flag];
		}
		return "Unnamed";
	}

	public static int GetPFlagType(int flag)
	{
		return 0;
	}

	public static int GetFlagCount()
	{
		return names.Count;
	}
}
