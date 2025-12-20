using UnityEngine;

namespace MarioBrosMayhem
{
	public class MovingObject : StageObject
	{
		protected int spawnLife;

		protected bool spawned;

		protected bool startedSpawn;

		protected bool movingRight;

		protected bool respawning;

		protected float respawnTimer;

		protected Vector3 velocity = Vector3.zero;

		protected float speed = 1.3333334f;

		protected float gravity = 0.01f;

		protected string spawnSound;

		protected override void Awake()
		{
			animator = GetComponentInChildren<Animator>();
			sprite = GetComponentInChildren<SpriteRenderer>();
			aud = GetComponent<AudioSource>();
			controller = GetComponent<CharacterController2D>();
		}

		protected virtual void FixedUpdate()
		{
			spawnLife++;
			if (spawned)
			{
				MoveAlgorithm();
			}
		}

		public virtual bool IsPerformingAction()
		{
			return false;
		}

		protected virtual void ClientResync()
		{
		}

		protected virtual void MoveAlgorithm()
		{
		}

		public virtual void EnterPipe(bool serverCall)
		{
			respawning = true;
			respawnTimer = 0f;
			controller.DisableCollisions();
			sprite.sortingOrder = -4;
			EnterPipe[] array = Object.FindObjectsOfType<EnterPipe>();
			foreach (EnterPipe enterPipe in array)
			{
				if (Mathf.Sign(enterPipe.transform.position.x) == Mathf.Sign(base.transform.position.x))
				{
					enterPipe.PlayEnterAnimation();
					break;
				}
			}
		}

		public virtual void StartMoving(bool movingRight)
		{
			spawned = true;
			velocity.y = 0f;
			controller.EnableCollisions();
			ChangeDirection(movingRight);
			sprite.sortingOrder = -1;
		}

		public virtual void ChangeDirection(bool right)
		{
			movingRight = right;
			speed = Mathf.Abs(speed);
			if (!movingRight)
			{
				speed = Mathf.Abs(speed) * -1f;
			}
		}

		protected void PlaySFX(string sfx, float pitch = 1f)
		{
			sfx = "mariobros/" + sfx;
			aud.clip = Resources.Load<AudioClip>(sfx);
			aud.pitch = pitch;
			aud.Play();
		}

		public string GetSpawnSound()
		{
			return spawnSound;
		}

		protected void SpawnFromNearestPipe(bool disableCollisions)
		{
			SpawnPipe[] array = Object.FindObjectsOfType<SpawnPipe>();
			foreach (SpawnPipe spawnPipe in array)
			{
				if (Mathf.Sign(base.transform.position.x) == Mathf.Sign(spawnPipe.transform.position.x))
				{
					if (disableCollisions)
					{
						controller.DisableCollisions();
					}
					spawnPipe.AddObjectToSpawn(this);
					break;
				}
			}
		}

		public virtual void SetNewAction(int playerId, int action, Vector3 position, int extraArg)
		{
		}
	}
}
