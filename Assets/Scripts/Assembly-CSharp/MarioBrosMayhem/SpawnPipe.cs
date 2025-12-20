using System.Collections.Generic;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class SpawnPipe : MonoBehaviour
	{
		private List<MovingObject> spawnList = new List<MovingObject>();

		private bool spawning;

		private float timer;

		private bool rightSide;

		private void Awake()
		{
			if (base.transform.position.x > 0f)
			{
				rightSide = true;
			}
		}

		private void Update()
		{
			if (!spawning)
			{
				return;
			}
			if (timer == 0f)
			{
				spawnList[0].ChangeDirection(!rightSide);
			}
			timer += Time.deltaTime;
			int num = (rightSide ? 1 : (-1));
			if (spawnList.Count == 0 || !spawnList[0])
			{
				Debug.LogWarning("SpawnPipe: object in pipe was destroyed while exiting pipe.  if not exiting game, then this is an issue.");
				spawning = false;
				return;
			}
			spawnList[0].transform.position = new Vector3(Mathf.Lerp(3.75f, 2.75f, timer) * (float)num, 2.75f);
			if (timer >= 1f)
			{
				PlaySFX(spawnList[0].GetSpawnSound());
				spawnList[0].StartMoving(!rightSide);
				timer = 0f;
				spawnList.RemoveAt(0);
				if (spawnList.Count == 0)
				{
					spawning = false;
				}
				else
				{
					GetComponent<Animator>().Play("Exit", 0, 0f);
				}
			}
		}

		public void AddObjectToSpawn(MovingObject obj)
		{
			if (!spawning)
			{
				timer = 0f;
				GetComponent<Animator>().Play("Exit", 0, 0f);
			}
			spawnList.Add(obj);
			spawning = true;
		}

		public void PlaySFX(string sfx, float pitch = 1f)
		{
			GetComponent<AudioSource>().clip = Resources.Load<AudioClip>(sfx);
			GetComponent<AudioSource>().Play();
		}
	}
}
