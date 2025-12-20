using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator
{
	private static Dictionary<int, Type[]> enemies = new Dictionary<int, Type[]>
	{
		{
			0,
			new Type[1] { typeof(Kris) }
		},
		{
			1,
			new Type[1] { typeof(FloweyCutscene) }
		},
		{
			2,
			new Type[1] { typeof(Dummy) }
		},
		{
			3,
			new Type[1] { typeof(Froggit) }
		},
		{
			4,
			new Type[2]
			{
				typeof(Froggit),
				typeof(Froggit)
			}
		},
		{
			5,
			new Type[1] { typeof(Whimsun) }
		},
		{
			6,
			new Type[2]
			{
				typeof(Froggit),
				typeof(Whimsun)
			}
		},
		{
			7,
			new Type[3]
			{
				typeof(Moldsmal),
				typeof(Moldsmal),
				typeof(Moldsmal)
			}
		},
		{
			8,
			new Type[1] { typeof(Napstablook) }
		},
		{
			9,
			new Type[1] { typeof(Loox) }
		},
		{
			10,
			new Type[1] { typeof(Vegetoid) }
		},
		{
			11,
			new Type[2]
			{
				typeof(Loox),
				typeof(Loox)
			}
		},
		{
			12,
			new Type[2]
			{
				typeof(Vegetoid),
				typeof(Vegetoid)
			}
		},
		{
			13,
			new Type[2]
			{
				typeof(Loox),
				typeof(Vegetoid)
			}
		},
		{
			14,
			new Type[1] { typeof(Flowey) }
		},
		{
			15,
			new Type[1] { typeof(MobileSprout) }
		},
		{
			16,
			new Type[2]
			{
				typeof(MobileSprout),
				typeof(MobileSprout)
			}
		},
		{
			17,
			new Type[1] { typeof(LilUFO) }
		},
		{
			18,
			new Type[1] { typeof(SpinRobo) }
		},
		{
			19,
			new Type[3]
			{
				typeof(LilUFO),
				typeof(MobileSprout),
				typeof(LilUFO)
			}
		},
		{
			20,
			new Type[2]
			{
				typeof(MobileSprout),
				typeof(ExplosiveOak)
			}
		},
		{
			21,
			new Type[3]
			{
				typeof(SpinRobo),
				typeof(ExplosiveOak),
				typeof(LilUFO)
			}
		},
		{
			22,
			new Type[3]
			{
				typeof(MobileSprout),
				typeof(MobileSprout),
				typeof(ExplosiveOak)
			}
		},
		{
			23,
			new Type[1] { typeof(CoilSnake) }
		},
		{
			24,
			new Type[1] { typeof(BlueCultist) }
		},
		{
			25,
			new Type[2]
			{
				typeof(BlueCultist),
				typeof(BlueCultist)
			}
		},
		{
			26,
			new Type[2]
			{
				typeof(BlueCultist),
				typeof(BlueCultist)
			}
		},
		{
			27,
			new Type[3]
			{
				typeof(BlueCultist),
				typeof(BlueCultist),
				typeof(BlueCultist)
			}
		},
		{
			28,
			new Type[1] { typeof(Carpainter) }
		},
		{
			29,
			new Type[1] { typeof(Toriel) }
		},
		{
			30,
			new Type[1] { typeof(FinalFroggit) }
		},
		{
			31,
			new Type[2]
			{
				typeof(FinalFroggit),
				typeof(FinalFroggit)
			}
		},
		{
			32,
			new Type[1] { typeof(Whimsalot) }
		},
		{
			33,
			new Type[2]
			{
				typeof(FinalFroggit),
				typeof(Whimsalot)
			}
		},
		{
			34,
			new Type[3]
			{
				typeof(Moldessa),
				typeof(Moldessa),
				typeof(Moldessa)
			}
		},
		{
			35,
			new Type[1] { typeof(Astigmatism) }
		},
		{
			36,
			new Type[1] { typeof(Parsnik) }
		},
		{
			37,
			new Type[2]
			{
				typeof(Astigmatism),
				typeof(Astigmatism)
			}
		},
		{
			38,
			new Type[2]
			{
				typeof(Parsnik),
				typeof(Parsnik)
			}
		},
		{
			39,
			new Type[2]
			{
				typeof(Astigmatism),
				typeof(Parsnik)
			}
		},
		{
			40,
			new Type[1] { typeof(Flowey) }
		},
		{
			41,
			new Type[1] { typeof(BladeKnight) }
		},
		{
			42,
			new Type[1] { typeof(RoughMole) }
		},
		{
			43,
			new Type[1] { typeof(MrBatty) }
		},
		{
			44,
			new Type[2]
			{
				typeof(RoughMole),
				typeof(MrBatty)
			}
		},
		{
			45,
			new Type[2]
			{
				typeof(RoughMole),
				typeof(RoughMole)
			}
		},
		{
			46,
			new Type[3]
			{
				typeof(RoughMole),
				typeof(RoughMole),
				typeof(MrBatty)
			}
		},
		{
			47,
			new Type[2]
			{
				typeof(MrBatty),
				typeof(MrBatty)
			}
		},
		{
			48,
			new Type[1] { typeof(MightyBear) }
		},
		{
			49,
			new Type[2]
			{
				typeof(RoughMole),
				typeof(MightyBear)
			}
		},
		{
			50,
			new Type[3]
			{
				typeof(RoughMole),
				typeof(MightyBear),
				typeof(MrBatty)
			}
		},
		{
			51,
			new Type[1] { typeof(MondoMole) }
		},
		{
			52,
			new Type[1] { typeof(Porky) }
		},
		{
			53,
			new Type[2]
			{
				typeof(Ness),
				typeof(Paula)
			}
		},
		{
			54,
			new Type[1] { typeof(Paula) }
		},
		{
			55,
			new Type[1] { typeof(TrainingDummy) }
		},
		{
			56,
			new Type[1] { typeof(Snowdrake) }
		},
		{
			57,
			new Type[1] { typeof(Doggo) }
		},
		{
			58,
			new Type[1] { typeof(IceCap) }
		},
		{
			59,
			new Type[2]
			{
				typeof(Glyde),
				typeof(Jerry)
			}
		},
		{
			60,
			new Type[2]
			{
				typeof(IceCap),
				typeof(Snowdrake)
			}
		},
		{
			61,
			new Type[2]
			{
				typeof(Snowdrake),
				typeof(Snowdrake)
			}
		},
		{
			62,
			new Type[1] { typeof(Feraldrake) }
		},
		{
			63,
			new Type[1] { typeof(Feraldrake) }
		},
		{
			64,
			new Type[1] { typeof(Feraldrake) }
		},
		{
			65,
			new Type[1] { typeof(Feraldrake) }
		},
		{
			66,
			new Type[2]
			{
				typeof(Dogamy),
				typeof(Dogaressa)
			}
		},
		{
			67,
			new Type[2]
			{
				typeof(IceCap),
				typeof(IceCap)
			}
		},
		{
			68,
			new Type[2]
			{
				typeof(LesserDog),
				typeof(SusieLD)
			}
		},
		{
			69,
			new Type[1] { typeof(Gyftrot) }
		},
		{
			70,
			new Type[1] { typeof(Jerry) }
		},
		{
			71,
			new Type[3]
			{
				typeof(IceCap),
				typeof(IceCap),
				typeof(IceCap)
			}
		},
		{
			72,
			new Type[1] { typeof(GreaterDog) }
		},
		{
			73,
			new Type[1] { typeof(Sans) }
		},
		{
			74,
			new Type[1] { typeof(RoughMole) }
		},
		{
			75,
			new Type[1] { typeof(UnoEnemy) }
		}
	};

	private static Dictionary<int, float[]> xValues = new Dictionary<int, float[]>
	{
		{
			0,
			new float[1]
		},
		{
			1,
			new float[1] { 0.063f }
		},
		{
			2,
			new float[1] { -1.03f }
		},
		{
			3,
			new float[1] { -1.06f }
		},
		{
			4,
			new float[2] { -3.1f, 1.07f }
		},
		{
			5,
			new float[1] { -1.05f }
		},
		{
			6,
			new float[2] { -1.06f, 1.05f }
		},
		{
			7,
			new float[3] { -4.39f, -0.26f, 3.87f }
		},
		{
			8,
			new float[1]
		},
		{
			9,
			new float[1] { -1.02f }
		},
		{
			10,
			new float[1] { -1.02f }
		},
		{
			11,
			new float[2] { -3.07f, 1.12f }
		},
		{
			12,
			new float[2] { -3.07f, 1.12f }
		},
		{
			13,
			new float[2] { -3.07f, 1.12f }
		},
		{
			14,
			new float[1]
		},
		{
			15,
			new float[1]
		},
		{
			16,
			new float[2] { -2f, 2f }
		},
		{
			17,
			new float[1]
		},
		{
			18,
			new float[1]
		},
		{
			19,
			new float[3] { -3f, 0f, 3f }
		},
		{
			20,
			new float[2] { -2f, 2f }
		},
		{
			21,
			new float[3] { -3.5f, 0f, 3.5f }
		},
		{
			22,
			new float[3] { -3.5f, 0f, 3.5f }
		},
		{
			23,
			new float[1]
		},
		{
			24,
			new float[1]
		},
		{
			25,
			new float[2] { -2f, 2f }
		},
		{
			26,
			new float[2] { -2f, 2f }
		},
		{
			27,
			new float[3] { -3f, 0f, 3f }
		},
		{
			28,
			new float[1]
		},
		{
			29,
			new float[1] { 0.04f }
		},
		{
			30,
			new float[1] { -1.16f }
		},
		{
			31,
			new float[2] { -3.1f, 1.07f }
		},
		{
			32,
			new float[1] { -1.05f }
		},
		{
			33,
			new float[2] { -3.22f, 3.21f }
		},
		{
			34,
			new float[3] { -4.39f, -0.26f, 3.87f }
		},
		{
			35,
			new float[1] { -1.02f }
		},
		{
			36,
			new float[1] { -1.02f }
		},
		{
			37,
			new float[2] { -3.07f, 1.12f }
		},
		{
			38,
			new float[2] { -3.07f, 1.12f }
		},
		{
			39,
			new float[2] { -3.07f, 1.12f }
		},
		{
			40,
			new float[1]
		},
		{
			41,
			new float[1]
		},
		{
			42,
			new float[1]
		},
		{
			43,
			new float[1]
		},
		{
			44,
			new float[2] { -2f, 2f }
		},
		{
			45,
			new float[2] { -2f, 2f }
		},
		{
			46,
			new float[3] { -3f, 0f, 3f }
		},
		{
			47,
			new float[2] { -2f, 2f }
		},
		{
			48,
			new float[1]
		},
		{
			49,
			new float[2] { -2f, 2f }
		},
		{
			50,
			new float[3] { -3f, 0f, 3f }
		},
		{
			51,
			new float[1]
		},
		{
			52,
			new float[1]
		},
		{
			53,
			new float[2] { -2.5f, 2.5f }
		},
		{
			54,
			new float[1]
		},
		{
			55,
			new float[1]
		},
		{
			56,
			new float[1] { -0.37f }
		},
		{
			57,
			new float[1] { -1.02f }
		},
		{
			58,
			new float[1] { -1.04f }
		},
		{
			59,
			new float[2] { 0f, 100f }
		},
		{
			60,
			new float[2] { -3.1f, 1.24f }
		},
		{
			61,
			new float[2] { -2.12f, 2.12f }
		},
		{
			62,
			new float[1] { -0.37f }
		},
		{
			63,
			new float[1] { -0.37f }
		},
		{
			64,
			new float[1] { -0.37f }
		},
		{
			65,
			new float[1] { -0.37f }
		},
		{
			66,
			new float[2]
		},
		{
			67,
			new float[2] { -2.12f, 2.12f }
		},
		{
			68,
			new float[2]
		},
		{
			69,
			new float[1]
		},
		{
			70,
			new float[1]
		},
		{
			71,
			new float[3] { -3.5f, 0f, 3.5f }
		},
		{
			72,
			new float[1]
		},
		{
			73,
			new float[1]
		},
		{
			74,
			new float[1]
		}
	};

	private static Dictionary<int, object[]> music = new Dictionary<int, object[]>
	{
		{
			0,
			new object[2] { "music/mus_battledelta", 1 }
		},
		{
			1,
			new object[2] { "music/mus_flowey", 1 }
		},
		{
			2,
			new object[2] { "music/mus_prebattle", 1 }
		},
		{
			8,
			new object[2] { "music/mus_ghostbattle", 1 }
		},
		{
			14,
			new object[2] { "music/mus_floweyboss", 1 }
		},
		{
			15,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			16,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			17,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			18,
			new object[2] { "music/mus_machinebattle", 1 }
		},
		{
			19,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			20,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			21,
			new object[2] { "music/mus_machinebattle", 1 }
		},
		{
			22,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			23,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			24,
			new object[2] { "music/mus_unsettling_battle", 1 }
		},
		{
			25,
			new object[2] { "music/mus_unsettling_battle", 1 }
		},
		{
			26,
			new object[2] { "music/mus_unsettling_battle", 1 }
		},
		{
			27,
			new object[2] { "music/mus_unsettling_battle", 1 }
		},
		{
			28,
			new object[2] { "music/mus_otherworldfoe_intro", 1 }
		},
		{
			29,
			new object[2] { "", 1 }
		},
		{
			30,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			31,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			32,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			33,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			34,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			35,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			36,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			37,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			38,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			39,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			40,
			new object[2] { "music/mus_floweyboss", 1.1f }
		},
		{
			41,
			new object[2] { "", 1 }
		},
		{
			42,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			43,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			44,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			45,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			46,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			47,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			48,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			49,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			50,
			new object[2] { "music/mus_battle_eb", 1 }
		},
		{
			51,
			new object[2] { "music/mus_sanctuaryboss_intro", 1 }
		},
		{
			52,
			new object[2] { "music/mus_pokeyboss_intro", 1 }
		},
		{
			53,
			new object[2] { "music/mus_nessboss", 1 }
		},
		{
			54,
			new object[2] { "music/mus_megalovania_frakture", 1 }
		},
		{
			55,
			new object[2] { "music/mus_castle_funk", 1 }
		},
		{
			56,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			57,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			58,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			60,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			61,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			62,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			63,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			64,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			65,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			67,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			68,
			new object[2] { "music/mus_doggers", 0.4f }
		},
		{
			69,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			70,
			new object[2] { "music/mus_jerry_intro", 1 }
		},
		{
			71,
			new object[2] { "music/mus_battle_hard", 1 }
		},
		{
			72,
			new object[2] { "music/mus_doggers", 0.9f }
		},
		{
			73,
			new object[2] { "music/mus_f_wind_intro", 1 }
		},
		{
			75,
			new object[2] { "", 1 }
		}
	};

	private static Dictionary<int, string> approach = new Dictionary<int, string>
	{
		{ 0, "* It's the evil Kris!!!" },
		{ 1, "* You shouldn't be seeing this" },
		{ 2, "* You encountered the Dummy." },
		{ 3, "* Froggit hopped close!" },
		{ 4, "* A pair of Froggits hop\n  towards you." },
		{ 5, "* Whimsun approached meekly!" },
		{ 6, "* Froggit and Whimsun drew near!" },
		{ 7, "* You tripped into a\n  line of Moldsmals." },
		{ 8, "* Here comes Napstablook." },
		{ 9, "* Loox drew near!" },
		{ 10, "* Vegetoid came out of the earth!" },
		{ 11, "* A pair of Loox\n  decided to pick on you!" },
		{ 12, "* A pair of Vegetoids\n  came out of the ground!" },
		{ 13, "* Vegetoid and Loox attacked!" },
		{ 14, "* FLOWEY attacks!" },
		{ 15, "* Mobile Sprout ran into you!" },
		{ 16, "* A couple of sprouts stumbled\n  in the way!" },
		{ 17, "* A tiny li'l UFO zoomed in\n  your sight!" },
		{ 18, "* Spinning Robo spun into view!" },
		{ 19, "* Two UFOs and a sprout\n  block your way!" },
		{ 20, "* Mobile Sprout and its\n  explosive cohort appeared!" },
		{ 21, "* Smorgasbord Strikes Back." },
		{ 22, "* The deities of nature\n  ambush you!" },
		{ 23, "* Coil Snake blocks the way!" },
		{ 24, "* Blue Cultist ambushes you!" },
		{ 25, "* Two cultists come to paint\n  you blue!" },
		{ 26, "* Two cultists are ordered to\n  attack you!" },
		{ 27, "* Three cultists block your\n  way!" },
		{ 28, "* Mr. Carpainter attacks!" },
		{ 29, "* Toriel blocks the way!" },
		{ 30, "* Final Froggit has been\n  expecting you two." },
		{ 31, "* The Final Duo closed in\n  on you!" },
		{ 32, "* Whimsalot rushed in!" },
		{ 33, "* Whimsalot and Final Froggit\n  appeared." },
		{ 34, "* A line of Moldessas block the\n  path." },
		{ 35, "* Astigmatism drew near." },
		{ 36, "* Parsnik slithered out of the\n  earth!" },
		{ 37, "* What an eyesore." },
		{ 38, "* Parsniks hissed out of the\n  earth!" },
		{ 39, "* Not only potatoes have eyes." },
		{ 40, "* FLOWEY attacks!" },
		{ 41, "* BLADEKNIGHT appears." },
		{ 42, "* Rough Mole rushed in!" },
		{ 43, "* Mr. Batty swooped towards\n  you!" },
		{ 44, "* Rough Mole and Mr. Batty came\n  rushing in!" },
		{ 45, "* A pair of moles cornered\n  you!" },
		{ 46, "* The underground deviants come\n  in fiercely!" },
		{ 47, "* Mr. and Mr. Batty accidentally\n  bump into you!" },
		{ 48, "* The Mighty Bear comes forth!" },
		{ 49, "* The small mole and the big bear\n  appeared!" },
		{ 50, "* The cavern's beasts have all\n  come after you!" },
		{ 51, "* Mondo Mole attacks!" },
		{ 52, "* Porky suddenly appears!" },
		{ 53, "* Ness and Paula block the way!" },
		{ 54, "* Paula phase 2 test" },
		{ 55, "* TRAINING MODE" },
		{ 56, "* Chilldrake rushes in!" },
		{ 57, "* Doggo blocks the way!" },
		{ 58, "* Icecap struts into view." },
		{ 59, "* Glyde swooped in!" },
		{ 60, "* Icecap and Chilldrake\n  pose like bad guys." },
		{ 61, "* Chilldrakes flutter forth!" },
		{ 62, "* Feraldrake ambushes you!" },
		{ 63, "* Feraldrake ambushes you from\n  the shadows!" },
		{ 64, "* Feraldrake ambushes you!" },
		{ 65, "* A feral Chilldrake emerges\n  from the shadows!" },
		{ 66, "* Dogi assault you!" },
		{ 67, "* The IceCaps ambush you!^05\n* With their caps!" },
		{ 68, "* You approach the Lesser Dog." },
		{ 69, "* Gyftrot stumbles into you!" },
		{ 70, "* Jerry test" },
		{ 71, "* A gang of Ice_Caps emerge\n  from the snow poff!" },
		{ 72, "* GREATERDOG blocks the way!" },
		{ 74, "* Rough Mole rushed in...?" },
		{ 75, "* UNO Battle" }
	};

	private static Dictionary<int, object[]> bg = new Dictionary<int, object[]>
	{
		{
			0,
			new object[5]
			{
				1,
				0.1f,
				60,
				(Color)new Color32(116, 0, 150, byte.MaxValue),
				false
			}
		},
		{
			1,
			new object[5]
			{
				3,
				0.1f,
				60,
				new Color(0.259f, 0f, 0.259f),
				false
			}
		},
		{
			8,
			new object[5]
			{
				0,
				0,
				0,
				new Color(0.1333f, 0.694f, 0.298f),
				true
			}
		},
		{
			14,
			new object[5]
			{
				3,
				0,
				0,
				new Color(0.1333f, 0.694f, 0.298f),
				true
			}
		},
		{
			15,
			new object[5]
			{
				4,
				1,
				0,
				Color.white,
				false
			}
		},
		{
			16,
			new object[5]
			{
				4,
				1,
				0,
				Color.white,
				false
			}
		},
		{
			17,
			new object[5]
			{
				4,
				2,
				0,
				Color.white,
				false
			}
		},
		{
			18,
			new object[5]
			{
				4,
				3,
				0,
				Color.white,
				false
			}
		},
		{
			19,
			new object[5]
			{
				4,
				2,
				0,
				Color.white,
				false
			}
		},
		{
			20,
			new object[5]
			{
				4,
				1,
				0,
				Color.white,
				false
			}
		},
		{
			21,
			new object[5]
			{
				4,
				3,
				0,
				Color.white,
				false
			}
		},
		{
			22,
			new object[5]
			{
				4,
				1,
				0,
				Color.white,
				false
			}
		},
		{
			23,
			new object[5]
			{
				4,
				1,
				0,
				Color.white,
				false
			}
		},
		{
			24,
			new object[5]
			{
				4,
				5,
				0,
				Color.white,
				false
			}
		},
		{
			25,
			new object[5]
			{
				4,
				5,
				0,
				Color.white,
				false
			}
		},
		{
			26,
			new object[5]
			{
				4,
				5,
				0,
				Color.white,
				false
			}
		},
		{
			27,
			new object[5]
			{
				4,
				5,
				0,
				Color.white,
				false
			}
		},
		{
			28,
			new object[5]
			{
				4,
				6,
				0,
				Color.white,
				false
			}
		},
		{
			29,
			new object[5]
			{
				0,
				0,
				0,
				new Color(0.1333f, 0.694f, 0.298f),
				true
			}
		},
		{
			40,
			new object[5]
			{
				3,
				0,
				0,
				new Color(0.1333f, 0.694f, 0.298f),
				true
			}
		},
		{
			41,
			new object[5]
			{
				1,
				0.1f,
				60,
				(Color)new Color32(123, 123, 123, byte.MaxValue),
				false
			}
		},
		{
			42,
			new object[5]
			{
				4,
				7,
				0,
				Color.white,
				false
			}
		},
		{
			43,
			new object[5]
			{
				4,
				8,
				0,
				Color.white,
				false
			}
		},
		{
			44,
			new object[5]
			{
				4,
				8,
				0,
				Color.white,
				false
			}
		},
		{
			45,
			new object[5]
			{
				4,
				7,
				0,
				Color.white,
				false
			}
		},
		{
			46,
			new object[5]
			{
				4,
				7,
				0,
				Color.white,
				false
			}
		},
		{
			47,
			new object[5]
			{
				4,
				8,
				0,
				Color.white,
				false
			}
		},
		{
			48,
			new object[5]
			{
				4,
				9,
				0,
				Color.white,
				false
			}
		},
		{
			49,
			new object[5]
			{
				4,
				9,
				0,
				Color.white,
				false
			}
		},
		{
			50,
			new object[5]
			{
				4,
				9,
				0,
				Color.white,
				false
			}
		},
		{
			51,
			new object[5]
			{
				4,
				10,
				0,
				Color.white,
				false
			}
		},
		{
			52,
			new object[5]
			{
				4,
				11,
				0,
				Color.white,
				false
			}
		},
		{
			53,
			new object[5]
			{
				4,
				12,
				0,
				Color.white,
				false
			}
		},
		{
			54,
			new object[5]
			{
				4,
				13,
				0,
				Color.white,
				false
			}
		},
		{
			55,
			new object[5]
			{
				1,
				0.1f,
				60,
				(Color)new Color32(0, 107, 183, byte.MaxValue),
				false
			}
		},
		{
			69,
			new object[5]
			{
				3,
				0,
				0,
				new Color(0.1333f, 0.694f, 0.298f),
				true
			}
		},
		{
			73,
			new object[5]
			{
				4,
				14,
				0,
				Color.white,
				false
			}
		}
	};

	private static Dictionary<int, object[]> fallbackBG = new Dictionary<int, object[]>
	{
		{
			1,
			new object[5]
			{
				1,
				0.1f,
				60f,
				(Color)new Color32(239, 239, 23, byte.MaxValue),
				false
			}
		},
		{
			2,
			new object[5]
			{
				1,
				0.8f,
				60f,
				(Color)new Color32(104, 120, 4, byte.MaxValue),
				false
			}
		},
		{
			3,
			new object[5]
			{
				2,
				5f,
				5f,
				(Color)new Color32(192, 56, 152, byte.MaxValue),
				false
			}
		},
		{
			5,
			new object[5]
			{
				1,
				1f,
				60f,
				(Color)new Color32(128, 24, 168, byte.MaxValue),
				false
			}
		},
		{
			6,
			new object[5]
			{
				1,
				0.3f,
				60f,
				(Color)new Color32(0, 0, 196, byte.MaxValue),
				true
			}
		},
		{
			7,
			new object[5]
			{
				2,
				4f,
				60f,
				(Color)new Color32(239, 239, 23, byte.MaxValue),
				false
			}
		},
		{
			8,
			new object[5]
			{
				1,
				1f,
				30f,
				(Color)new Color32(176, 176, 120, byte.MaxValue),
				false
			}
		},
		{
			9,
			new object[5]
			{
				2,
				2f,
				100f,
				(Color)new Color32(176, 176, 120, byte.MaxValue),
				false
			}
		},
		{
			10,
			new object[5]
			{
				2,
				1f,
				80f,
				(Color)new Color32(168, 24, 88, byte.MaxValue),
				true
			}
		},
		{
			11,
			new object[5]
			{
				1,
				1f,
				60f,
				Color.red,
				true
			}
		},
		{
			12,
			new object[5]
			{
				1,
				0.1f,
				60f,
				Color.blue,
				true
			}
		}
	};

	private static string[] bgNames = new string[21]
	{
		"Earthbound/Test", "Earthbound/Sprout", "Earthbound/UFO", "Earthbound/Robo", "Stardust", "Earthbound/BlueBlue", "Earthbound/Carpainter", "Earthbound/Mole", "Earthbound/Bat", "Earthbound/Bear",
		"Earthbound/Mondo", "Earthbound/Porky", "Earthbound/Ness", "Earthbound/Paula", "SansBG", "PapyrusBalls", "UTY/AxisBG", "UTY/CerobaBG", "UTY/DalvBG", "UTY/MartletBG",
		"UTY/StarloBG"
	};

	private static Dictionary<int, object[]> customUnoBGs = new Dictionary<int, object[]>
	{
		{
			2,
			bg[14]
		},
		{
			3,
			new object[5]
			{
				1,
				0.3f,
				60,
				new Color(0.1333f, 0.694f, 0.298f),
				false
			}
		},
		{
			4,
			bg[14]
		},
		{
			5,
			bg[0]
		},
		{
			6,
			bg[0]
		},
		{
			7,
			bg[0]
		},
		{
			8,
			bg[0]
		},
		{
			10,
			bg[24]
		},
		{
			11,
			bg[14]
		},
		{
			12,
			bg[52]
		},
		{
			13,
			bg[73]
		},
		{
			14,
			new object[5]
			{
				4,
				15,
				0,
				Color.white,
				false
			}
		},
		{
			15,
			new object[5]
			{
				0,
				0,
				0,
				(Color)new Color32(byte.MaxValue, 204, 0, byte.MaxValue),
				false
			}
		},
		{
			16,
			new object[5]
			{
				4,
				18,
				0,
				Color.white,
				false
			}
		},
		{
			17,
			new object[5]
			{
				4,
				19,
				0,
				Color.white,
				false
			}
		},
		{
			18,
			new object[5]
			{
				4,
				20,
				0,
				Color.white,
				false
			}
		},
		{
			19,
			new object[5]
			{
				4,
				16,
				0,
				Color.white,
				false
			}
		},
		{
			20,
			new object[5]
			{
				4,
				17,
				0,
				Color.white,
				false
			}
		}
	};

	public static EnemyBase[] GetEnemies(int battleId)
	{
		if (enemies.ContainsKey(battleId))
		{
			int num = enemies[battleId].Length;
			if (num > 3)
			{
				num = 3;
			}
			if (num < xValues[battleId].Length)
			{
				num = xValues[battleId].Length;
			}
			EnemyBase[] array = new EnemyBase[num];
			for (int i = 0; i < num; i++)
			{
				array[i] = new GameObject("Enemy" + (i + 1), enemies[battleId][i]).GetComponent<EnemyBase>();
				array[i].transform.position = new Vector2(xValues[battleId][i], 0f);
			}
			return array;
		}
		return null;
	}

	public static object[] GetMusic(int battleId)
	{
		if (music.ContainsKey(battleId))
		{
			if (music[battleId].Length < 1)
			{
				return new object[2]
				{
					music[battleId][0],
					1
				};
			}
			return music[battleId];
		}
		return new object[2] { "music/mus_battle", 1 };
	}

	public static string GetApproachText(int battleId)
	{
		Dictionary<int, string> serializedClass = Util.PackManager().GetSerializedClass<Dictionary<int, string>>("EnemyGenerator");
		if (approach.ContainsKey(battleId))
		{
			if (serializedClass != null && serializedClass.ContainsKey(battleId))
			{
				return serializedClass[battleId];
			}
			if (battleId == 8 && (int)Util.GameManager().GetFlag(108) == 1)
			{
				return Util.MiscStrings().GetString("napsta_approach_hardmode", 0);
			}
			if (battleId == 56 && (int)Util.GameManager().GetFlag(180) == 0)
			{
				return "* A familiar face rushes\n  in...?";
			}
			return approach[battleId];
		}
		return Util.MiscStrings().GetString("default_enemy_approach", 0);
	}

	public static object[] GetBattleBG(int battleId)
	{
		object[] array = new object[5]
		{
			0,
			0,
			0,
			new Color(0.1333f, 0.694f, 0.298f),
			false
		};
		if (bg.ContainsKey(battleId))
		{
			array = bg[battleId];
		}
		if (battleId == 75 && customUnoBGs.ContainsKey(MusicChooser.musicID))
		{
			array = customUnoBGs[MusicChooser.musicID];
		}
		int num = (int)array[0];
		int key = (int)float.Parse(array[1].ToString());
		if (num == 4 && fallbackBG.ContainsKey(key) && GameManager.GetOptions().lowGraphics.value == 1)
		{
			array = fallbackBG[key];
		}
		return array;
	}

	public static string GetBGName(int index)
	{
		if (index >= 0 && index < bgNames.Length)
		{
			return bgNames[index];
		}
		return "";
	}

	public static int GetEncounterCount()
	{
		return enemies.Count;
	}

	public static string GetEncounterName(int battleId)
	{
		if (enemies[battleId] == null)
		{
			return "EMPTY DO NOT USE";
		}
		List<string> list = new List<string>();
		Type[] array = enemies[battleId];
		foreach (Type type in array)
		{
			list.Add(type.ToString());
		}
		return string.Join(", ", list.ToArray());
	}
}
