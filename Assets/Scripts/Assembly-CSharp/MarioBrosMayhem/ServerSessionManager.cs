using System.Collections.Generic;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class ServerSessionManager : MonoBehaviour
	{
		private bool inGame;

		private List<int> classicRules = new List<int>(new int[7] { 3, 0, 0, 0, 0, 0, 0 });

		private void Awake()
		{
			MonoBehaviour.print("awake");
		}

		private void Start()
		{
			if ((bool)Util.GameManager())
			{
				MonoBehaviour.print("gamemanager found");
				switch ((int)Util.GameManager().GetSessionFlag(13))
				{
				case 1:
					classicRules[2] = 1;
					classicRules[4] = 1;
					classicRules[5] = 1;
					classicRules[6] = 1;
					break;
				case 2:
					classicRules[0] = 2;
					classicRules[2] = 2;
					classicRules[3] = 1;
					break;
				}
			}
			StartGame();
		}

		public int[] GetRules(int type)
		{
			if (type == 0)
			{
				int[] array = new int[classicRules.Count];
				for (int i = 0; i < classicRules.Count; i++)
				{
					array[i] = classicRules[i];
				}
				return array;
			}
			return null;
		}

		public int GetRuleValue(int type, int id)
		{
			if (type == 0)
			{
				return classicRules[id];
			}
			return 0;
		}

		public int GetGamemode()
		{
			return 0;
		}

		public void StartGame()
		{
			if (!inGame)
			{
				inGame = true;
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/network/MarioBrosNetworkManager"));
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/Player"), GlobalVariables.SPAWN_POS[0], Quaternion.identity).GetComponent<Player>().SetValues((int)Util.GameManager().GetSessionFlag(11), (int)Util.GameManager().GetSessionFlag(12));
			}
		}
	}
}
