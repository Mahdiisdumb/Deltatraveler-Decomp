using System.Collections.Generic;

namespace MarioBrosMayhem
{
	public class PhaseInfo
	{
		public enum Background
		{
			Sewer = 0,
			Underground = 1,
			Dungeon = 2,
			Fortress = 3,
			Snowy = 4
		}

		public enum Platform
		{
			Normal = 0,
			Underground = 1,
			Dungeon = 2,
			Special = 3,
			Frozen = 4
		}

		public enum EnemyType
		{
			Shellcreeper = 0,
			Sidestepper = 1,
			Fighterfly = 2
		}

		public static readonly Phase[] PHASES = new Phase[17]
		{
			new Phase(new KeyValuePair<int, float>[3]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 4f),
				new KeyValuePair<int, float>(0, 9f)
			}, 41.5f, 52f),
			new Phase(new KeyValuePair<int, float>[4]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 4f),
				new KeyValuePair<int, float>(0, 9f),
				new KeyValuePair<int, float>(0, 13f)
			}, 41.5f, 52f),
			new Phase(new KeyValuePair<int, float>[6]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 4f),
				new KeyValuePair<int, float>(0, 10f),
				new KeyValuePair<int, float>(0, 11f),
				new KeyValuePair<int, float>(0, 15.5f),
				new KeyValuePair<int, float>(0, 15.5f)
			}, 41.5f, 52f),
			new Phase(specialStage: true),
			new Phase(new KeyValuePair<int, float>[4]
			{
				new KeyValuePair<int, float>(1, 1f),
				new KeyValuePair<int, float>(1, 3f),
				new KeyValuePair<int, float>(1, 10f),
				new KeyValuePair<int, float>(1, 12f)
			}, 31f, 52f, 1, 1, spawnFreezer: false, 1),
			new Phase(new KeyValuePair<int, float>[6]
			{
				new KeyValuePair<int, float>(1, 1f),
				new KeyValuePair<int, float>(1, 3f),
				new KeyValuePair<int, float>(0, 11f),
				new KeyValuePair<int, float>(0, 12f),
				new KeyValuePair<int, float>(1, 24f),
				new KeyValuePair<int, float>(1, 25f)
			}, 31f, 52f, 1, 1, spawnFreezer: false, 1),
			new Phase(new KeyValuePair<int, float>[4]
			{
				new KeyValuePair<int, float>(2, 2f),
				new KeyValuePair<int, float>(2, 4f),
				new KeyValuePair<int, float>(2, 12.5f),
				new KeyValuePair<int, float>(2, 14.5f)
			}, 31f, 52f, 2, 2, spawnFreezer: false, 1),
			new Phase(new KeyValuePair<int, float>[6]
			{
				new KeyValuePair<int, float>(2, 2f),
				new KeyValuePair<int, float>(2, 4f),
				new KeyValuePair<int, float>(1, 12.5f),
				new KeyValuePair<int, float>(1, 13.5f),
				new KeyValuePair<int, float>(2, 19f),
				new KeyValuePair<int, float>(2, 27f)
			}, 10.5f, 52f, 2, 2, spawnFreezer: false, 1),
			new Phase(specialStage: true, 1),
			new Phase(new KeyValuePair<int, float>[5]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 2.75f),
				new KeyValuePair<int, float>(2, 11f),
				new KeyValuePair<int, float>(0, 12f),
				new KeyValuePair<int, float>(0, 20f)
			}, 10.5f, 52f, 4, 3, spawnFreezer: true),
			new Phase(new KeyValuePair<int, float>[5]
			{
				new KeyValuePair<int, float>(1, 1f),
				new KeyValuePair<int, float>(1, 2.5f),
				new KeyValuePair<int, float>(2, 11f),
				new KeyValuePair<int, float>(1, 12f),
				new KeyValuePair<int, float>(1, 20f)
			}, 10.5f, 52f, 4, 3, spawnFreezer: true),
			new Phase(new KeyValuePair<int, float>[6]
			{
				new KeyValuePair<int, float>(2, 2f),
				new KeyValuePair<int, float>(1, 4f),
				new KeyValuePair<int, float>(1, 13f),
				new KeyValuePair<int, float>(1, 14f),
				new KeyValuePair<int, float>(2, 22f),
				new KeyValuePair<int, float>(1, 25f)
			}, 10.5f, 52f, 4, 3, spawnFreezer: true),
			new Phase(new KeyValuePair<int, float>[5]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 2.75f),
				new KeyValuePair<int, float>(2, 11f),
				new KeyValuePair<int, float>(0, 12f),
				new KeyValuePair<int, float>(0, 20f)
			}, 5f, 52f, 4, 3, spawnFreezer: true),
			new Phase(new KeyValuePair<int, float>[5]
			{
				new KeyValuePair<int, float>(1, 1f),
				new KeyValuePair<int, float>(1, 2.5f),
				new KeyValuePair<int, float>(2, 11f),
				new KeyValuePair<int, float>(1, 12f),
				new KeyValuePair<int, float>(1, 20f)
			}, 5f, 52f, 4, 3, spawnFreezer: true),
			new Phase(new KeyValuePair<int, float>[6]
			{
				new KeyValuePair<int, float>(2, 2f),
				new KeyValuePair<int, float>(1, 4f),
				new KeyValuePair<int, float>(1, 13f),
				new KeyValuePair<int, float>(1, 14f),
				new KeyValuePair<int, float>(2, 22f),
				new KeyValuePair<int, float>(1, 25f)
			}, 5f, 52f, 4, 3, spawnFreezer: true),
			new Phase(specialStage: true, 2),
			new Phase(new KeyValuePair<int, float>[5]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 2.75f),
				new KeyValuePair<int, float>(1, 11f),
				new KeyValuePair<int, float>(0, 12f),
				new KeyValuePair<int, float>(0, 13f)
			}, 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true)
		};

		public static readonly Phase[] ROUNDS_EASY = new Phase[1]
		{
			new Phase(new KeyValuePair<int, float>[2]
			{
				new KeyValuePair<int, float>(0, 1f),
				new KeyValuePair<int, float>(0, 2.75f)
			}, 31f, 52f, 3)
		};

		public static Phase GetPhase(int i)
		{
			if (i < PHASES.Length)
			{
				return PHASES[i];
			}
			Phase[] array = new Phase[7]
			{
				new Phase(PHASES[10].GetEnemies(), 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true),
				new Phase(PHASES[11].GetEnemies(), 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true),
				new Phase(PHASES[9].GetEnemies(), 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true),
				new Phase(PHASES[10].GetEnemies(), 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true),
				new Phase(PHASES[11].GetEnemies(), 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true),
				new Phase(specialStage: true, 3),
				new Phase(PHASES[9].GetEnemies(), 5f, 52f, 4, 3, spawnFreezer: true, 1, spawnIcicle: true)
			};
			return array[(i - PHASES.Length) % array.Length];
		}

		public static Phase GetRound(int i, int difficulty)
		{
			return ROUNDS_EASY[i % 4];
		}
	}
}
