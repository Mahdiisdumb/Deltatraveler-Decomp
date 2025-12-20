using System.Collections.Generic;
using UnityEngine.SceneManagement;

public class MapInfo
{
	private static Dictionary<int, string> validSavePoints = new Dictionary<int, string>
	{
		{ 4, "Test Area" },
		{ 5, "Test Room - Tunnel" },
		{ 9, "Ruins - Entrance" },
		{ 15, "Ruins - Leaf Pile" },
		{ 21, "Ruins - Mouse Hole" },
		{ 37, "Ruins - Home" },
		{ 49, "Snowdin - Box Road" },
		{ 51, "Twoson Caves" },
		{ 56, "Happy Happy Village" },
		{ 63, "???" },
		{ 66, "LOSTCORE - End" },
		{ 74, "Snowdin - Box Road" },
		{ 86, "Snowdin - Spaghetti" },
		{ 53, "Peaceful Rest Valley" },
		{ 70, "Lilliput Steps Cave" },
		{ 91, "Snowdin - Dogi Checkpoint" },
		{ 88, "Snowdin - Bunny House" },
		{ 95, "Snowdin - Dog House" },
		{ 99, "Snowdin - Cave Entrance" },
		{ 107, "Snowdin - Snow Pile" },
		{ 108, "Snowdin - Under Bridge" },
		{ 111, "Snowdin - Town" },
		{ 121, "Waterfall - Checkpoint" }
	};

	private static Dictionary<int, string> savePlatforms = new Dictionary<int, string>
	{
		{ 4, "waterfall" },
		{ 5, "waterfall" },
		{ 9, "ruins" },
		{ 15, "ruins" },
		{ 21, "ruins" },
		{ 37, "ruins" },
		{ 49, "snowdin" },
		{ 51, "eb" },
		{ 56, "eb" },
		{ 66, "lostcore" },
		{ 74, "snowdin_gguf" },
		{ 86, "snowdin_gguf" },
		{ 53, "eb" },
		{ 70, "eb" },
		{ 91, "snowdin_gguf" },
		{ 88, "snowdin_gguf" },
		{ 95, "snowdin_gguf" },
		{ 99, "snowdin_gguf" },
		{ 108, "snowdin_gguf" },
		{ 111, "snowdin" },
		{ 121, "waterfall" }
	};

	public static string GetMapName(int bIndex)
	{
		if (validSavePoints.ContainsKey(bIndex))
		{
			return validSavePoints[bIndex];
		}
		return "";
	}

	public static string GetMapSavePlatform(int bIndex)
	{
		if (savePlatforms.ContainsKey(bIndex))
		{
			return savePlatforms[bIndex];
		}
		return "";
	}

	public static bool IsValidMapSpawn(int map)
	{
		return validSavePoints.ContainsKey(map);
	}

	public static World GetMapWorld(int room)
	{
		if (room >= 7 && room <= 49)
		{
			return World.Undertale;
		}
		if ((room >= 50 && room <= 62) || (room >= 70 && room <= 71))
		{
			return World.Earthbound;
		}
		if ((room >= 72 && room <= 76) || (room >= 79 && room <= 102) || (room >= 105 && room <= 109))
		{
			return World.Underfell;
		}
		if ((room >= 110 && room <= 127) || room == 130)
		{
			return World.UTIntermission1;
		}
		if ((room >= 63 && room <= 69) || room == 101 || room == 102)
		{
			return World.LOSTCORE;
		}
		return World.None;
	}

	public static List<string> GetMapNames()
	{
		return new List<string>(validSavePoints.Values);
	}

	public static List<int> GetMapIDs()
	{
		return new List<int>(validSavePoints.Keys);
	}

	public static World GetCurrentWorld()
	{
		return GetMapWorld(SceneManager.GetActiveScene().buildIndex);
	}
}
