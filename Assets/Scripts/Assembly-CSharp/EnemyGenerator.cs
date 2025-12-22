using System;
using System.Collections.Generic;
using UnityEngine;

public class EnemyGenerator
{
	public struct Enemy
	{
		public Type type;

		public float xPos;

		public Enemy(Type type, float xPos)
		{
			this.type = type;
			this.xPos = xPos;
		}
	}

	public struct Encounter
	{
		public Enemy[] enemies;

		public string approachText;

		public BackgroundID background;

		public string music;

		public float musicPitch;

		public bool isBoss;

		public int introAttack;

		public int stateFlag;

		public int endCutscene;

		public Encounter(Enemy[] enemies, BackgroundID background = BackgroundID.None, string approachText = "", string music = "music/mus_battle", float musicPitch = 1f, bool isBoss = false, int introAttack = -1, int stateFlag = -1, int endCutscene = -1)
		{
			this.enemies = enemies;
			this.background = background;
			this.approachText = approachText;
			this.music = music;
			this.musicPitch = musicPitch;
			this.isBoss = isBoss;
			this.introAttack = introAttack;
			this.stateFlag = stateFlag;
			this.endCutscene = endCutscene;
		}
	}

	public enum ID
	{
		None = -1,
		Kris = 0,
		FloweyIntro = 1,
		Dummy = 2,
		Froggit = 3,
		TwoFroggits = 4,
		Whimsun = 5,
		FroggitAndWhimsun = 6,
		Moldsmals = 7,
		Napstablook = 8,
		Loox = 9,
		Vegetoid = 10,
		TwoLoox = 11,
		TwoVegetoids = 12,
		LooxAndVegetoid = 13,
		Flowey = 14,
		MobileSprout = 15,
		TwoMobileSprouts = 16,
		LilUFO = 17,
		SpinningRobo = 18,
		UFOsAndSprout = 19,
		SproutAndOak = 20,
		Smorgasbord = 21,
		TwoSproutsAndOak = 22,
		CoilSnake = 23,
		FirstCultist = 24,
		CaveCultists = 25,
		CabinCultists = 26,
		MazeCultists = 27,
		Carpainter = 28,
		Toriel = 29,
		FinalFroggit = 30,
		TwoFinalFroggits = 31,
		Whimsalot = 32,
		FFroggitAndWhimsalot = 33,
		Moldessas = 34,
		Astigmatism = 35,
		Parsnik = 36,
		TwoAstigmatisms = 37,
		TwoParsniks = 38,
		AstigmatismAndParsnik = 39,
		HardmodeFlowey = 40,
		HardmodeBladeKnight = 41,
		RoughMole = 42,
		MrBatty = 43,
		MoleAndBat = 44,
		TwoMoles = 45,
		MolesAndBat = 46,
		MrAndMrBatty = 47,
		MightyBear = 48,
		MoleAndBear = 49,
		CavernBeasts = 50,
		MondoMole = 51,
		Porky = 52,
		NessAndPaula = 53,
		PaulaTest = 54,
		TrainingMode = 55,
		Snowdrake = 56,
		Doggo = 57,
		IceCap = 58,
		Jerry = 59,
		IceCapAndSnowdrake = 60,
		TwoSnowdrakes = 61,
		Feraldrake1 = 62,
		Feraldrake2 = 63,
		Feraldrake3 = 64,
		FeralChilldrake = 65,
		Dogi = 66,
		IceCaps = 67,
		LesserDog = 68,
		Gyftrot = 69,
		JerryTest = 70,
		Ice_Caps = 71,
		GreaterDog = 72,
		Sans = 73,
		WaterfallMole = 74,
		UNOBattle = 75
	}

	public enum BackgroundID
	{
		None = -1,
		EarthboundTest = 0,
		EarthboundSprout = 1,
		EarthboundUFO = 2,
		EarthboundRobo = 3,
		TSStardust = 4,
		EarthboundBlueBlue = 5,
		EarthboundCarpainter = 6,
		EarthboundMole = 7,
		EarthboundBat = 8,
		EarthboundBear = 9,
		EarthboundMondo = 10,
		EarthboundPorky = 11,
		EarthboundNess = 12,
		EarthboundPaula = 13,
		UFSans = 14,
		UNOFrankness = 15,
		UTYAxis = 16,
		UTYCeroba = 17,
		UTYDalv = 18,
		UTYMartlet = 19,
		UTYStarlo = 20,
		UTBase = 21,
		UTBoss = 22,
		Deltarune = 23,
		LOSTCORE = 24,
		TrainingMode = 25,
		UTUndyne = 26,
		UTYDunes = 27
	}

	private static Encounter[] encounters = new Encounter[76]
	{
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Kris), 0f),
			new Enemy(typeof(Gringus), 3f)
		}, BackgroundID.Deltarune, "* It's the evil Kris!!!", "music/mus_battledelta", 1f, isBoss: false, -1, 3, 0),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(FloweyCutscene), 0.063f)
		}, BackgroundID.None, "* You shouldn't be seeing this", "music/mus_flowey", 1f, isBoss: false, 3, -1, 3),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Dummy), -1.03f)
		}, BackgroundID.UTBase, "* You encountered the Dummy.", "music/mus_prebattle", 1f, isBoss: false, -1, 6, 6),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Froggit), -1.06f)
		}, BackgroundID.UTBase, "* Froggit hopped close!"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Froggit), -3.1f),
			new Enemy(typeof(Froggit), 1.07f)
		}, BackgroundID.UTBase, "* A pair of Froggits hop\n  towards you."),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Whimsun), -1.05f)
		}, BackgroundID.UTBase, "* Whimsun approached meekly!"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Froggit), -1.06f),
			new Enemy(typeof(Whimsun), 1.05f)
		}, BackgroundID.UTBase, "* Froggit and Whimsun drew near!"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(Moldsmal), -4.39f),
			new Enemy(typeof(Moldsmal), -0.26f),
			new Enemy(typeof(Moldsmal), 3.87f)
		}, BackgroundID.UTBase, "* You tripped into a\n  line of Moldsmals."),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Napstablook), 0f)
		}, BackgroundID.UTBoss, "* Here comes Napstablook.", "music/mus_ghostbattle", 1f, isBoss: false, -1, 127, 12),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Loox), -1.02f)
		}, BackgroundID.UTBase, "* Loox drew near!"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Vegetoid), -1.02f)
		}, BackgroundID.UTBase, "* Vegetoid came out of the earth!"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Loox), -3.07f),
			new Enemy(typeof(Loox), 1.12f)
		}, BackgroundID.UTBase, "* A pair of Loox\n  decided to pick on you!"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Vegetoid), -3.07f),
			new Enemy(typeof(Vegetoid), 1.12f)
		}, BackgroundID.UTBase, "* A pair of Vegetoids\n  came out of the ground!"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Loox), -3.07f),
			new Enemy(typeof(Vegetoid), 1.12f)
		}, BackgroundID.UTBase, "* Vegetoid and Loox attacked!"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Flowey), 0f)
		}, BackgroundID.None, "* FLOWEY attacks!", "music/mus_floweyboss", 1f, isBoss: true, 22, 58, 21),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(MobileSprout), 0f)
		}, BackgroundID.EarthboundSprout, "* Mobile Sprout ran into you!", "music/mus_battle_eb"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(MobileSprout), -2f),
			new Enemy(typeof(MobileSprout), 2f)
		}, BackgroundID.EarthboundSprout, "* A couple of sprouts stumbled\n  in the way!", "music/mus_battle_eb"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(LilUFO), 0f)
		}, BackgroundID.EarthboundUFO, "* A tiny li'l UFO zoomed in\n  your sight!", "music/mus_battle_eb"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(SpinRobo), 0f)
		}, BackgroundID.EarthboundRobo, "* Spinning Robo spun into view!", "music/mus_machinebattle"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(LilUFO), -3f),
			new Enemy(typeof(MobileSprout), 0f),
			new Enemy(typeof(LilUFO), 3f)
		}, BackgroundID.EarthboundUFO, "* Two UFOs and a sprout\n  block your way!", "music/mus_battle_eb"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(MobileSprout), -2f),
			new Enemy(typeof(ExplosiveOak), 2f)
		}, BackgroundID.EarthboundSprout, "* Mobile Sprout and its\n  explosive cohort appeared!", "music/mus_battle_eb"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(SpinRobo), -3.5f),
			new Enemy(typeof(ExplosiveOak), 0f),
			new Enemy(typeof(LilUFO), 3.5f)
		}, BackgroundID.EarthboundRobo, "* Smorgasbord Strikes Back.", "music/mus_machinebattle"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(MobileSprout), -3.5f),
			new Enemy(typeof(MobileSprout), 0f),
			new Enemy(typeof(ExplosiveOak), 3.5f)
		}, BackgroundID.EarthboundSprout, "* The deities of nature\n  ambush you!", "music/mus_battle_eb"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(CoilSnake), 0f)
		}, BackgroundID.EarthboundSprout, "* Coil Snake blocks the way!", "music/mus_battle_eb", 1f, isBoss: false, -1, 89, 32),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(BlueCultist), 0f)
		}, BackgroundID.EarthboundBlueBlue, "* Blue Cultist ambushes you!", "music/mus_unsettling_battle", 1f, isBoss: false, -1, 97, 34),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(BlueCultist), -2f),
			new Enemy(typeof(BlueCultist), 2f)
		}, BackgroundID.EarthboundBlueBlue, "* Two cultists come to paint\n  you blue!", "music/mus_unsettling_battle"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(BlueCultist), -2f),
			new Enemy(typeof(BlueCultist), 2f)
		}, BackgroundID.EarthboundBlueBlue, "* Two cultists are ordered to\n  attack you!", "music/mus_unsettling_battle", 1f, isBoss: false, -1, 106, 37),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(BlueCultist), -3f),
			new Enemy(typeof(BlueCultist), 0f),
			new Enemy(typeof(BlueCultist), 3f)
		}, BackgroundID.EarthboundBlueBlue, "* Three cultists block your\n  way!", "music/mus_unsettling_battle", 1f, isBoss: false, -1, 109, 52),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Carpainter), 0f)
		}, BackgroundID.EarthboundCarpainter, "* Mr. Carpainter attacks!", "music/mus_otherworldfoe_intro", 1f, isBoss: false, 37, 116, 40),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Toriel), 0.04f)
		}, BackgroundID.UTBoss, "* Toriel blocks the way!", "", 1f, isBoss: true, 40, -1, 44),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(FinalFroggit), -1.16f)
		}, BackgroundID.UTBase, "* Final Froggit has been\n  expecting you two.", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(FinalFroggit), -3.1f),
			new Enemy(typeof(FinalFroggit), 1.07f)
		}, BackgroundID.UTBase, "* The Final Duo closed in\n  on you!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Whimsalot), -1.05f)
		}, BackgroundID.UTBase, "* Whimsalot rushed in!", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(FinalFroggit), -3.22f),
			new Enemy(typeof(Whimsalot), 3.21f)
		}, BackgroundID.UTBase, "* Whimsalot and Final Froggit\n  appeared.", "music/mus_battle_hard"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(Moldessa), -4.39f),
			new Enemy(typeof(Moldessa), -0.26f),
			new Enemy(typeof(Moldessa), 3.87f)
		}, BackgroundID.UTBase, "* A line of Moldessas block the\n  path.", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Astigmatism), -1.02f)
		}, BackgroundID.UTBase, "* Astigmatism drew near.", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Parsnik), -1.02f)
		}, BackgroundID.UTBase, "* Parsnik slithered out of the\n  earth!", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Astigmatism), -3.07f),
			new Enemy(typeof(Astigmatism), 1.12f)
		}, BackgroundID.UTBase, "* What an eyesore.", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Parsnik), -3.07f),
			new Enemy(typeof(Parsnik), 1.12f)
		}, BackgroundID.UTBase, "* Parsniks hissed out of the\n  earth!", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Astigmatism), -3.07f),
			new Enemy(typeof(Parsnik), 1.12f)
		}, BackgroundID.UTBase, "* Not only potatoes have eyes.", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Flowey), 0f)
		}, BackgroundID.None, "* FLOWEY attacks!", "music/mus_floweyboss", 1.1f, isBoss: true, 22, 58, 46),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(BladeKnight), 0f)
		}, BackgroundID.LOSTCORE, "* BLADEKNIGHT appears.", "", 1f, isBoss: false, -1, 124),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(RoughMole), 0f)
		}, BackgroundID.EarthboundMole, "* Rough Mole rushed in!", "music/mus_battle_eb"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(MrBatty), 0f)
		}, BackgroundID.EarthboundBat, "* Mr. Batty swooped towards\n  you!", "music/mus_battle_eb"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(RoughMole), -2f),
			new Enemy(typeof(MrBatty), 2f)
		}, BackgroundID.EarthboundBat, "* Rough Mole and Mr. Batty came\n  rushing in!", "music/mus_battle_eb"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(RoughMole), -2f),
			new Enemy(typeof(RoughMole), 2f)
		}, BackgroundID.EarthboundMole, "* A pair of moles cornered\n  you!", "music/mus_battle_eb"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(RoughMole), -3f),
			new Enemy(typeof(RoughMole), 0f),
			new Enemy(typeof(MrBatty), 3f)
		}, BackgroundID.EarthboundMole, "* The underground deviants come\n  in fiercely!", "music/mus_battle_eb"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(MrBatty), -2f),
			new Enemy(typeof(MrBatty), 2f)
		}, BackgroundID.EarthboundBat, "* Mr. and Mr. Batty accidentally\n  bump into you!", "music/mus_battle_eb"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(MightyBear), 0f)
		}, BackgroundID.EarthboundBear, "* The Mighty Bear comes forth!", "music/mus_battle_eb"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(RoughMole), -2f),
			new Enemy(typeof(MightyBear), 2f)
		}, BackgroundID.EarthboundBear, "* The small mole and the big bear\n  appeared!", "music/mus_battle_eb"),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(RoughMole), -3f),
			new Enemy(typeof(MightyBear), 0f),
			new Enemy(typeof(MrBatty), 3f)
		}, BackgroundID.EarthboundBear, "* The cavern's beasts have all\n  come after you!", "music/mus_battle_eb"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(MondoMole), 0f)
		}, BackgroundID.EarthboundMondo, "* Mondo Mole attacks!", "music/mus_sanctuaryboss_intro", 1f, isBoss: false, -1, 150, 49),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Porky), 0f)
		}, BackgroundID.EarthboundPorky, "* Porky suddenly appears!", "music/mus_pokeyboss_intro", 1f, isBoss: true, -1, 154, 53),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Ness), -2.5f),
			new Enemy(typeof(Paula), 2.5f)
		}, BackgroundID.EarthboundNess, "* Ness and Paula block the way!", "music/mus_nessboss", 1f, isBoss: true, -1, 173, 56),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Paula), 0f)
		}, BackgroundID.EarthboundPaula, "* Paula phase 2 test", "music/mus_megalovania_frakture", 1f, isBoss: true, -1, 173, 56),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(TrainingDummy), 0f)
		}, BackgroundID.TrainingMode, "* TRAINING MODE", "music/mus_castle_funk"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Snowdrake), -0.37f)
		}, BackgroundID.UTBase, "* Chilldrake rushes in!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Doggo), -1.02f)
		}, BackgroundID.UTBase, "* Doggo blocks the way!", "music/mus_battle_hard", 1f, isBoss: false, -1, 185, 62),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(IceCap), -1.04f)
		}, BackgroundID.UTBase, "* Icecap struts into view.", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Glyde), 0f),
			new Enemy(typeof(Jerry), 100f)
		}, BackgroundID.UTBase, "* Glyde swooped in!", "music/mus_battle", 1f, isBoss: false, -1, 270, 90),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(IceCap), -3.1f),
			new Enemy(typeof(Snowdrake), 1.24f)
		}, BackgroundID.UTBase, "* Icecap and Chilldrake\n  pose like bad guys.", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Snowdrake), -2.12f),
			new Enemy(typeof(Snowdrake), 2.12f)
		}, BackgroundID.UTBase, "* Chilldrakes flutter forth!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Feraldrake), -0.37f)
		}, BackgroundID.UTBase, "* Feraldrake ambushes you!", "music/mus_battle_hard", 1f, isBoss: false, -1, 205, 70),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Feraldrake), -0.37f)
		}, BackgroundID.UTBase, "* Feraldrake ambushes you from\n  the shadows!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Feraldrake), -0.37f)
		}, BackgroundID.UTBase, "* Feraldrake ambushes you!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Feraldrake), -0.37f)
		}, BackgroundID.UTBase, "* A feral Chilldrake emerges\n  from the shadows!", "music/mus_battle_hard", 1f, isBoss: false, -1, 209, 80),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(Dogamy), 0f),
			new Enemy(typeof(Dogaressa), 0f)
		}, BackgroundID.UTBase, "* Dogi assault you!", "music/mus_battle", 1f, isBoss: false, -1, 241, 86),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(IceCap), -2.12f),
			new Enemy(typeof(IceCap), 2.12f)
		}, BackgroundID.UTBase, "* The IceCaps ambush you!^05\n* With their caps!", "music/mus_battle_hard"),
		new Encounter(new Enemy[2]
		{
			new Enemy(typeof(LesserDog), 0f),
			new Enemy(typeof(SusieLD), 0f)
		}, BackgroundID.UTBase, "* You approach the Lesser Dog.", "music/mus_doggers", 0.4f, isBoss: false, -1, 253, 87),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Gyftrot), 0f)
		}, BackgroundID.None, "* Gyftrot stumbles into you!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Jerry), 0f)
		}, BackgroundID.UTBase, "* Jerry test", "music/mus_jerry_intro", 1f, isBoss: false, -1, 270, 90),
		new Encounter(new Enemy[3]
		{
			new Enemy(typeof(IceCap), -3.5f),
			new Enemy(typeof(IceCap), 0f),
			new Enemy(typeof(IceCap), 3.5f)
		}, BackgroundID.UTBase, "* A gang of Ice_Caps emerge\n  from the snow poff!", "music/mus_battle_hard"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(GreaterDog), 0f)
		}, BackgroundID.UTBase, "* GREATERDOG blocks the way!", "music/mus_doggers", 0.9f, isBoss: false, -1, 245, 93),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(Sans), 0f)
		}, BackgroundID.UFSans, "", "music/mus_f_wind_intro", 1f, isBoss: true, 108, 281, 97),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(RoughMole), 0f)
		}, BackgroundID.UTBase, "* Rough Mole rushed in...?"),
		new Encounter(new Enemy[1]
		{
			new Enemy(typeof(UnoEnemy), 0f)
		}, BackgroundID.None, "", "", 1f, isBoss: false, -1, -1, 101)
	};

	private static string[] bgNames = new string[28]
	{
		"Earthbound/Test", "Earthbound/Sprout", "Earthbound/UFO", "Earthbound/Robo", "Stardust", "Earthbound/BlueBlue", "Earthbound/Carpainter", "Earthbound/Mole", "Earthbound/Bat", "Earthbound/Bear",
		"Earthbound/Mondo", "Earthbound/Porky", "Earthbound/Ness", "Earthbound/Paula", "SansBG", "PapyrusBalls", "UTY/AxisBG", "UTY/CerobaBG", "UTY/DalvBG", "UTY/MartletBG",
		"UTY/StarloBG", "Undertale/Base", "Undertale/Boss", "Undertale/Deltarune", "Undertale/LOSTCORE", "Undertale/TrainingMode", "Undertale/Undyne", "UTY/DunesBG"
	};

	private static List<BackgroundID> needsFallback = new List<BackgroundID>
	{
		BackgroundID.EarthboundSprout,
		BackgroundID.EarthboundUFO,
		BackgroundID.EarthboundRobo,
		BackgroundID.EarthboundBlueBlue,
		BackgroundID.EarthboundCarpainter,
		BackgroundID.EarthboundMole,
		BackgroundID.EarthboundBat,
		BackgroundID.EarthboundBear,
		BackgroundID.EarthboundMondo,
		BackgroundID.EarthboundPorky,
		BackgroundID.EarthboundNess
	};

	private static Dictionary<int, BackgroundID> customUnoBGs = new Dictionary<int, BackgroundID>
	{
		{
			0,
			BackgroundID.UTBase
		},
		{
			1,
			BackgroundID.UTBase
		},
		{
			2,
			BackgroundID.UTBase
		},
		{
			3,
			BackgroundID.UTUndyne
		},
		{
			5,
			BackgroundID.Deltarune
		},
		{
			6,
			BackgroundID.Deltarune
		},
		{
			7,
			BackgroundID.Deltarune
		},
		{
			8,
			BackgroundID.Deltarune
		},
		{
			9,
			BackgroundID.UTBase
		},
		{
			10,
			BackgroundID.EarthboundBlueBlue
		},
		{
			12,
			BackgroundID.EarthboundPorky
		},
		{
			13,
			BackgroundID.UFSans
		},
		{
			14,
			BackgroundID.UNOFrankness
		},
		{
			15,
			BackgroundID.UTYDunes
		},
		{
			16,
			BackgroundID.UTYDalv
		},
		{
			17,
			BackgroundID.UTYMartlet
		},
		{
			18,
			BackgroundID.UTYStarlo
		},
		{
			19,
			BackgroundID.UTYAxis
		},
		{
			20,
			BackgroundID.UTYCeroba
		}
	};

	public static EnemyBase[] GetEnemies(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			int num = encounters[battleId].enemies.Length;
			if (num > 3)
			{
				num = 3;
			}
			EnemyBase[] array = new EnemyBase[num];
			for (int i = 0; i < num; i++)
			{
				Type type = encounters[battleId].enemies[i].type;
				float xPos = encounters[battleId].enemies[i].xPos;
				array[i] = new GameObject("Enemy" + (i + 1), type).GetComponent<EnemyBase>();
				array[i].transform.position = new Vector2(xPos, 0f);
			}
			return array;
		}
		return null;
	}

	public static string GetMusic(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			return encounters[battleId].music;
		}
		return "music/mus_battle";
	}

	public static float GetMusicPitch(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			return encounters[battleId].musicPitch;
		}
		return 1f;
	}

	public static string GetApproachText(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			if (battleId == 8 && (int)Util.GameManager().GetFlag(108) == 1)
			{
				return "* Here comes Napstablook.^05\n* Same as usual.";
			}
			if (battleId == 56 && (int)Util.GameManager().GetFlag(180) == 0)
			{
				return "* A familiar face rushes\n  in...?";
			}
			return encounters[battleId].approachText;
		}
		return "* Enemy approaches!";
	}

	public static GameObject GetBattleBG(int battleId)
	{
		int num = -1;
		if (battleId > -1 && battleId < encounters.Length)
		{
			num = (int)encounters[battleId].background;
		}
		if (battleId == 75 && customUnoBGs.ContainsKey(MusicChooser.musicID))
		{
			num = (int)customUnoBGs[MusicChooser.musicID];
		}
		if (num > -1)
		{
			return UnityEngine.Object.Instantiate(Resources.Load<GameObject>("vfx/BattleBGEffect/" + GetBGName((BackgroundID)num)));
		}
		return null;
	}

	public static bool IsBossEncounter(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			return encounters[battleId].isBoss;
		}
		return false;
	}

	public static int GetIntroAttack(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			return encounters[battleId].introAttack;
		}
		return -1;
	}

	public static int GetStateFlag(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			return encounters[battleId].stateFlag;
		}
		return -1;
	}

	public static int GetEndCutscene(int battleId)
	{
		if (battleId > -1 && battleId < encounters.Length)
		{
			return encounters[battleId].endCutscene;
		}
		return -1;
	}

	public static string GetBGName(BackgroundID index)
	{
		if (needsFallback.Contains(index) && GameManager.GetOptions().lowGraphics.value == 1)
		{
			return "Fallback/" + bgNames[(int)index];
		}
		return bgNames[(int)index];
	}

	public static int GetEncounterCount()
	{
		return encounters.Length;
	}

	public static string GetEncounterName(int battleId)
	{
		if (battleId < 0 || battleId >= encounters.Length)
		{
			return "EMPTY DO NOT USE";
		}
		List<string> list = new List<string>();
		Enemy[] enemies = encounters[battleId].enemies;
		for (int i = 0; i < enemies.Length; i++)
		{
			Enemy enemy = enemies[i];
			list.Add(enemy.type.ToString());
		}
		return string.Join(", ", list.ToArray());
	}
}
