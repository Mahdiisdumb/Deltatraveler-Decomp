using UnityEngine;

namespace MarioBrosMayhem
{
	public class MarioBrosNetworkManager : MonoBehaviour
	{
		private int phase = GlobalVariables.DEBUG_PHASE - 1;

		private Phase phaseInfo;

		private int[] curPowLevels = new int[2] { 3, 3 };

		private bool roundStarted;

		private float spawnTimer;

		private int spawnID;

		private bool spawnOnRight;

		private bool doneSettingUp;

		private bool[] spawnedCoins;

		private bool spawnedGreen;

		private bool spawnedRed;

		private bool hardFireballs;

		private bool spawnedSecondGreen;

		private bool spawnedFreezie;

		private bool bigMode;

		private bool spawnMushrooms;

		private int credits = 1;

		private bool playingResults;

		private void Awake()
		{
			hardFireballs = Object.FindObjectOfType<ServerSessionManager>().GetRuleValue(0, 3) == 1;
			bigMode = Object.FindObjectOfType<ServerSessionManager>().GetRuleValue(0, 2) == 2;
			if (!bigMode)
			{
				spawnMushrooms = Object.FindObjectOfType<ServerSessionManager>().GetRuleValue(0, 6) == 1;
			}
		}

		private void Update()
		{
			if (roundStarted)
			{
				HandleRoundBehavior();
			}
			else
			{
				if (doneSettingUp)
				{
					return;
				}
				if (PhaseInfo.GetPhase(phase + 1).SpawnIcicle())
				{
					int num = (phase + 2) / 10;
					if (num > 4)
					{
						num = 4;
					}
					for (int i = 0; i < num; i++)
					{
						Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Icicle"));
					}
				}
				doneSettingUp = true;
				Object.FindObjectOfType<MarioBrosManager>().StartNewRound();
			}
		}

		public void HandleRoundBehavior()
		{
			if (!Object.FindObjectOfType<MarioBrosManager>().IsEndingRound())
			{
				spawnTimer += Time.deltaTime;
			}
			if (!phaseInfo.IsSpecialStage())
			{
				while (spawnID < phaseInfo.GetEnemyCount() && spawnTimer >= phaseInfo.GetEnemySpawnTime(spawnID))
				{
					Enemy enemy = null;
					enemy = ((phaseInfo.GetEnemyType(spawnID) != 1) ? ((phaseInfo.GetEnemyType(spawnID) != 2) ? Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/enemies/Koopa"), new Vector3((spawnID + 1) * (spawnOnRight ? 1 : (-1)), -10f), Quaternion.identity).GetComponent<Enemy>() : Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/enemies/Fighterfly"), new Vector3((spawnID + 1) * (spawnOnRight ? 1 : (-1)), -10f), Quaternion.identity).GetComponent<Enemy>()) : Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/enemies/Sidestepper"), new Vector3((spawnID + 1) * (spawnOnRight ? 1 : (-1)), -10f), Quaternion.identity).GetComponent<Enemy>());
					enemy.SetEnemyNumber(spawnID);
					spawnID++;
					spawnOnRight = !spawnOnRight;
				}
				if (!spawnedGreen && spawnTimer >= (hardFireballs ? (phaseInfo.GetGreenFireSpawnTime() / 2f) : phaseInfo.GetGreenFireSpawnTime()))
				{
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/GreenFireball")).GetComponent<Fireball>();
					spawnedGreen = true;
				}
				if (hardFireballs && !spawnedSecondGreen && spawnTimer >= phaseInfo.GetGreenFireSpawnTime() / 2f + 2f)
				{
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/GreenFireball")).GetComponent<Fireball>();
					spawnedSecondGreen = true;
				}
				if (!spawnedRed && spawnTimer >= (hardFireballs ? (phaseInfo.GetRedFireSpawnTime() / 2f) : phaseInfo.GetRedFireSpawnTime()))
				{
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/RedFireball")).GetComponent<Fireball>();
					spawnedRed = true;
				}
			}
		}

		public void StartNewRound()
		{
			roundStarted = false;
			spawnTimer = 0f;
			Enemy[] array = Object.FindObjectsOfType<Enemy>();
			for (int i = 0; i < array.Length; i++)
			{
				Object.Destroy(array[i].gameObject);
			}
			Coin[] array2 = Object.FindObjectsOfType<Coin>();
			for (int j = 0; j < array2.Length; j++)
			{
				Object.Destroy(array2[j].gameObject);
			}
			Fireball[] array3 = Object.FindObjectsOfType<Fireball>();
			for (int k = 0; k < array3.Length; k++)
			{
				Object.Destroy(array3[k].gameObject);
			}
			Freezie[] array4 = Object.FindObjectsOfType<Freezie>();
			for (int l = 0; l < array4.Length; l++)
			{
				Object.Destroy(array4[l].gameObject);
			}
			Icicle[] array5 = Object.FindObjectsOfType<Icicle>();
			for (int m = 0; m < array5.Length; m++)
			{
				Object.Destroy(array5[m].gameObject);
			}
			Mushroom[] array6 = Object.FindObjectsOfType<Mushroom>();
			for (int n = 0; n < array6.Length; n++)
			{
				Object.Destroy(array6[n].gameObject);
			}
			doneSettingUp = false;
			SetUpNewRoundClientRpc();
			if (PhaseInfo.GetPhase(phase + 1).IsSpecialStage())
			{
				BonusCoinSetup();
			}
		}

		public void BonusCoinSetup()
		{
			for (int i = 0; i < 10; i++)
			{
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/bonuscoins/BonusCoin" + i + " Variant"));
			}
		}

		public void StartRound(int phase)
		{
			roundStarted = true;
			spawnOnRight = false;
			doneSettingUp = false;
			this.phase = phase;
			phaseInfo = PhaseInfo.GetPhase(phase);
			spawnTimer = 0f;
			spawnID = 0;
			if (!phaseInfo.IsSpecialStage())
			{
				spawnedCoins = new bool[phaseInfo.GetEnemyCount()];
				for (int i = 0; i < spawnedCoins.Length; i++)
				{
					spawnedCoins[i] = false;
				}
			}
			else
			{
				curPowLevels[0] = 3;
				curPowLevels[1] = 3;
			}
			spawnedGreen = false;
			spawnedRed = false;
			spawnedFreezie = false;
			spawnedSecondGreen = false;
		}

		public void SetUpNewRoundClientRpc()
		{
			Object.FindObjectOfType<MarioBrosManager>().SetUpNewRound();
		}

		public void HitPowBlock(int powId)
		{
			curPowLevels[powId]--;
		}

		public int GetPowLevel(int powId)
		{
			return curPowLevels[powId];
		}

		public void PickUpPowBlock(int powId)
		{
			curPowLevels[powId] = 0;
		}

		public void FlipOverEnemyFromPlatform(Player player)
		{
			player.AddPoints(10);
		}

		public void KillFireball(Player player, bool red, Vector3 position)
		{
			int points = (red ? 1000 : 200);
			player.AddPoints(points);
			CreateScoreGraphic(points, position);
		}

		public void KillFreezie(Player player, Vector3 position)
		{
			int points = 500;
			player.AddPoints(points);
			CreateScoreGraphic(points, position);
		}

		public void EnemyDefeatedServerRpc(Player player, int enemyId, int multikick, Vector3 position)
		{
			if (spawnedCoins[enemyId])
			{
				return;
			}
			spawnedCoins[enemyId] = true;
			if (phaseInfo.SpawnFreezer() && !spawnedFreezie)
			{
				spawnedFreezie = true;
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Freezie"), new Vector3((phase % 2 != 0) ? 1 : (-1), -11f), Quaternion.identity).GetComponent<Freezie>();
			}
			int num = 800 * (multikick + 1);
			if (num > 3200)
			{
				num = 3200;
			}
			player.AddPoints(num);
			CreateScoreGraphic(num, position, multikick > 0);
			if (multikick >= 4)
			{
				player.AddLives(1);
				CreateScoreGraphic(1, position + new Vector3(0f, 2f / 3f), multikick > 0);
			}
			if (Object.FindObjectOfType<MarioBrosManager>().GetEnemyCount() > 0)
			{
				if (multikick == 2 && spawnMushrooms)
				{
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Mushroom"), new Vector3((enemyId + 1) * ((enemyId % 2 == 0) ? 1 : (-1)), -11f), Quaternion.identity).GetComponent<Mushroom>();
				}
				else
				{
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Coin"), new Vector3((enemyId + 1) * ((enemyId % 2 == 0) ? 1 : (-1)), -11f), Quaternion.identity).GetComponent<Coin>();
				}
			}
		}

		public void CreateScoreGraphic(int points, Vector3 occurrencePosition, bool big = false)
		{
			Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/score/" + (big ? "big/" : "") + points), occurrencePosition + new Vector3(0f, 0.25f), Quaternion.identity);
		}

		public void RevivePlayer(int playerId)
		{
			Player playerObject = GetPlayerObject(playerId);
			Object.FindObjectOfType<GameOverContinue>().Deactivate();
			if (credits > 0)
			{
				credits--;
				playerObject.ResetPoints();
				playerObject.Revive(3);
			}
			else
			{
				Object.FindObjectOfType<MarioBrosManager>().TrueGameOver();
			}
		}

		public Player GetPlayerObject(int playerId)
		{
			if (playerId == -1)
			{
				return null;
			}
			return Object.FindObjectOfType<Player>();
		}

		public int GetCredits()
		{
			return credits;
		}
	}
}
