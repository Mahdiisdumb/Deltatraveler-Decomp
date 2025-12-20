using UnityEngine;

namespace MarioBrosMayhem
{
	public static class GlobalVariables
	{
		public static readonly string[] SKIN_NAMES = new string[4] { "Mario", "Luigi", "Kris", "Susie" };

		public static readonly string[] SKIN_FILENAMES = new string[4] { "mario", "luigi", "kris", "susie" };

		public static readonly int[][] SKIN_HEIGHTS_SMALL = new int[4][]
		{
			new int[32]
			{
				20, 21, 20, 20, 21, 15, 20, 21, 21, 20,
				18, 22, 21, 24, 18, 15, 20, 21, 20, 20,
				20, 15, 20, 20, 0, 0, 0, 0, 0, 0,
				0, 0
			},
			new int[32]
			{
				24, 25, 24, 24, 25, 15, 24, 25, 23, 22,
				18, 23, 23, 24, 19, 15, 24, 25, 24, 24,
				24, 15, 24, 24, 0, 0, 0, 0, 0, 0,
				0, 0
			},
			new int[32]
			{
				20, 21, 20, 20, 21, 15, 20, 21, 21, 20,
				18, 22, 21, 24, 18, 15, 20, 21, 20, 20,
				20, 15, 20, 20, 0, 0, 0, 0, 0, 0,
				0, 0
			},
			new int[32]
			{
				24, 25, 24, 24, 25, 15, 24, 25, 23, 22,
				18, 23, 23, 24, 19, 15, 24, 25, 24, 24,
				24, 15, 24, 24, 0, 0, 0, 0, 0, 0,
				0, 0
			}
		};

		public static readonly int[][] SKIN_HEIGHTS_BIG = new int[4][]
		{
			new int[32]
			{
				28, 29, 28, 28, 29, 17, 28, 29, 32, 28,
				25, 31, 30, 25, 24, 17, 28, 29, 28, 28,
				28, 17, 28, 28, 0, 0, 0, 0, 0, 0,
				0, 0
			},
			new int[32]
			{
				32, 33, 32, 32, 33, 17, 32, 33, 33, 28,
				25, 33, 33, 25, 26, 18, 32, 33, 32, 32,
				32, 17, 32, 32, 0, 0, 0, 0, 0, 0,
				0, 0
			},
			new int[32]
			{
				28, 29, 28, 28, 29, 17, 28, 29, 32, 28,
				25, 31, 30, 25, 24, 17, 28, 29, 28, 28,
				28, 17, 28, 28, 0, 0, 0, 0, 0, 0,
				0, 0
			},
			new int[32]
			{
				32, 33, 32, 32, 33, 17, 32, 33, 33, 28,
				25, 33, 33, 25, 26, 18, 32, 33, 32, 32,
				32, 17, 32, 32, 0, 0, 0, 0, 0, 0,
				0, 0
			}
		};

		public static readonly int SKIN_COUNT = SKIN_NAMES.Length;

		public static readonly bool[] HAS_VOICE = new bool[4] { true, true, false, true };

		public static readonly Vector3[] SPAWN_POS = new Vector3[8]
		{
			new Vector3(-3f, -4.315f),
			new Vector3(3f, -4.315f),
			new Vector3(-1f, -4.315f),
			new Vector3(1f, -4.315f),
			new Vector3(-4f, -2.3333333f),
			new Vector3(4f, -2.3333333f),
			new Vector3(-2f, -2.3333333f),
			new Vector3(2f, -2.3333333f)
		};

		public static readonly Vector3[] HUD_POS = new Vector3[8]
		{
			new Vector3(-96f, -104f),
			new Vector3(96f, -104f),
			new Vector3(-38f, -104f),
			new Vector3(38f, -104f),
			new Vector3(-96f, 100f),
			new Vector3(96f, 100f),
			new Vector3(-38f, 100f),
			new Vector3(38f, 100f)
		};

		public static readonly string[] BACKGROUND_NAMES = new string[5] { "SewerBG", "UndergroundBG", "DungeonBG", "BattleFortressBG", "SnowyBG" };

		public static readonly string[] PLATFORM_NAMES = new string[6] { "standard", "sidestepper", "fighterfly", "bonus", "frozen", "bonus_cold" };

		public static readonly string[] MUSIC_STAGE_NAMES = new string[3] { "mus_stage", "mus_stage_underground", "mus_bonus" };

		public static readonly int DEBUG_PHASE = ((Application.platform == RuntimePlatform.WindowsEditor) ? 0 : 0);

		public static Material GetNamedPaletteMaterial(string skin, string name, int id)
		{
			Material material = Resources.Load<Material>(string.Format("mariobros/materials/{0}/{1}/{0}-{1}-{2}", skin, name, id));
			if (!material)
			{
				return Resources.Load<Material>(string.Format("mariobros/materials/{0}/{1}/{0}-{1}-0", skin, name));
			}
			return material;
		}

		public static Material GetNamedPaletteMaterial(int skin, string name, int id)
		{
			return GetNamedPaletteMaterial(SKIN_FILENAMES[skin], name, id);
		}

		public static Material GetPaletteMaterial(string skin, int id)
		{
			return GetNamedPaletteMaterial(skin, "sprite", id);
		}

		public static Material GetPaletteMaterial(int skin, int id)
		{
			return GetNamedPaletteMaterial(skin, "sprite", id);
		}

		public static Material GetHUDPaletteMaterial(string skin, int id)
		{
			return GetNamedPaletteMaterial(skin, "hud", id);
		}

		public static Material GetHUDPaletteMaterial(int skin, int id)
		{
			return GetNamedPaletteMaterial(skin, "hud", id);
		}
	}
}
