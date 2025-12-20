using System.Collections.Generic;

namespace MarioBrosMayhem
{
	public struct Phase
	{
		private KeyValuePair<int, float>[] enemyTypesAndTime;

		private float greenFireSpawn;

		private float redFireSpawn;

		private int backgroundType;

		private int platformType;

		private bool spawnFreezer;

		private int music;

		private bool spawnIcicle;

		private bool specialStage;

		private int specialStageLevel;

		public Phase(KeyValuePair<int, float>[] enemyTypesAndTime, float greenFireSpawn, float redFireSpawn, int backgroundType = 0, int platformType = 0, bool spawnFreezer = false, int music = 0, bool spawnIcicle = false)
		{
			this.enemyTypesAndTime = enemyTypesAndTime;
			this.greenFireSpawn = greenFireSpawn;
			this.redFireSpawn = redFireSpawn;
			this.backgroundType = backgroundType;
			this.platformType = platformType;
			this.spawnFreezer = spawnFreezer;
			this.music = music;
			this.spawnIcicle = spawnIcicle;
			specialStage = false;
			specialStageLevel = 0;
		}

		public Phase(bool specialStage, int specialStageLevel = 0)
		{
			this.specialStage = specialStage;
			this.specialStageLevel = specialStageLevel;
			enemyTypesAndTime = null;
			greenFireSpawn = -1f;
			redFireSpawn = -1f;
			backgroundType = 4;
			platformType = 3;
			spawnFreezer = false;
			spawnIcicle = false;
			music = 2;
		}

		public KeyValuePair<int, float>[] GetEnemies()
		{
			return enemyTypesAndTime;
		}

		public int GetEnemyCount()
		{
			return enemyTypesAndTime.Length;
		}

		public int GetEnemyType(int enemyNo)
		{
			if (enemyTypesAndTime == null || enemyTypesAndTime.Length == 0 || enemyTypesAndTime.Length <= enemyNo)
			{
				return 0;
			}
			return enemyTypesAndTime[enemyNo].Key;
		}

		public float GetEnemySpawnTime(int enemyNo)
		{
			if (enemyTypesAndTime == null || enemyTypesAndTime.Length == 0 || enemyTypesAndTime.Length <= enemyNo)
			{
				return 0f;
			}
			return enemyTypesAndTime[enemyNo].Value;
		}

		public float GetGreenFireSpawnTime()
		{
			return greenFireSpawn;
		}

		public float GetRedFireSpawnTime()
		{
			return redFireSpawn;
		}

		public int GetBackgroundType()
		{
			return backgroundType;
		}

		public int GetPlatformType()
		{
			if (specialStageLevel == 1)
			{
				return 4;
			}
			return platformType;
		}

		public int GetMusic()
		{
			return music;
		}

		public bool IsSpecialStage()
		{
			return specialStage;
		}

		public int SpecialStageLevel()
		{
			return specialStageLevel;
		}

		public bool SpawnFreezer()
		{
			return spawnFreezer;
		}

		public bool SpawnIcicle()
		{
			return spawnIcicle;
		}
	}
}
