using UnityEngine;

namespace MarioBrosMayhem
{
	public class Freezie : MovingObject
	{
		private Vector3 origin = Vector3.zero;

		private float spawnCountDown = 12f;

		private bool biting;

		private float biteTimer;

		private bool freezing;

		private bool freezeCall;

		private float freezeTimer;

		private int freezeSpot;

		private bool die;

		private float dieTimer;

		private bool vanish;

		protected override void Awake()
		{
			base.Awake();
			spawnSound = "mariobros/sounds/snd_spawn";
			sprite.sortingOrder = -4;
			speed = 1.75f;
			gravity = 1.3333334f / Mathf.Pow(16.666666f, 2f);
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			spawnCountDown -= Time.fixedDeltaTime;
			if (spawnCountDown <= 0f && !startedSpawn && Object.FindObjectOfType<MarioBrosManager>().GetEnemyCount() > 0)
			{
				if (origin == Vector3.zero)
				{
					origin = base.transform.position;
				}
				die = false;
				respawning = false;
				biting = false;
				freezing = false;
				sprite.sortingOrder = -4;
				animator.Play("Walk");
				animator.SetFloat("Speed", 1f);
				base.transform.position = origin;
				startedSpawn = true;
				SpawnFromNearestPipe(true);
			}
		}

		public override bool IsPerformingAction()
		{
			if (!freezing && !biting && !die && spawned)
			{
				return respawning;
			}
			return true;
		}

		protected override void MoveAlgorithm()
		{
			if (!IsPerformingAction())
			{
				if (controller.collisions.down && velocity.y <= 0f)
				{
					velocity.y = 0f;
				}
				velocity.y -= gravity;
				velocity.x = speed * Time.fixedDeltaTime;
				sprite.flipX = !movingRight;
				controller.Move(velocity);
			}
			else if (die && !vanish)
			{
				dieTimer += Time.fixedDeltaTime;
				if (dieTimer <= 1f / 6f)
				{
					base.transform.position += new Vector3(0f, 2f * Time.fixedDeltaTime);
				}
			}
			else if (freezing)
			{
				freezeTimer += Time.fixedDeltaTime;
				if (freezeTimer >= 5f / 6f && !freezeCall)
				{
					freezeCall = true;
					FinishFreeze(freezeSpot);
				}
			}
			else if (respawning)
			{
				respawnTimer += Time.fixedDeltaTime;
				base.transform.position = new Vector3(base.transform.position.x + speed * Time.fixedDeltaTime, Mathf.Lerp(-4.3333335f, -3.9166667f, respawnTimer * 4f));
			}
			else if (biting)
			{
				sprite.flipX = movingRight;
				biteTimer += Time.fixedDeltaTime;
				if (biteTimer >= 1f)
				{
					biting = false;
					animator.SetFloat("Speed", 1f);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!respawning && spawned)
			{
				if ((bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract() && !collision.GetComponent<Player>().IsInvincible())
				{
					OnTriggerStay2D(collision);
				}
				if (((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>())
				{
					OnTriggerStay2D(collision);
				}
				if (!respawning && (bool)collision.GetComponent<EnterPipe>() && ((base.transform.position.x > 0f && movingRight) || (base.transform.position.x < 0f && !movingRight)))
				{
					EnterPipe(true);
				}
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (!respawning && spawned)
			{
				if ((bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract() && !collision.GetComponent<Player>().IsInvincible())
				{
					collision.GetComponent<Player>().Damage(new Vector3((float)((!(base.transform.position.x > collision.transform.position.x)) ? 1 : (-1)) / 24f, 0.05f), 1);
					BitePlayer(0, base.transform.position.x < collision.transform.position.x);
				}
				if ((((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>()) && ((collision.transform.position.x > base.transform.position.x && movingRight) || (collision.transform.position.x < base.transform.position.x && !movingRight)))
				{
					ChangeDirection(!movingRight);
				}
			}
		}

		public void BitePlayer(int playerId, bool playerOnRight)
		{
			biting = true;
			biteTimer = 0f;
			movingRight = !playerOnRight;
			speed = Mathf.Abs(speed);
			if (!movingRight)
			{
				speed *= -1f;
			}
			sprite.flipX = movingRight;
			animator.SetFloat("Speed", 0f);
		}

		public override void EnterPipe(bool serverCall)
		{
			base.EnterPipe(serverCall);
			startedSpawn = false;
			spawnCountDown = 5f;
		}

		public void Die(int playerId)
		{
			if (!die)
			{
				freezing = false;
				biting = false;
				vanish = playerId == -1;
				die = true;
				dieTimer = 0f;
				startedSpawn = false;
				spawnCountDown = 5f;
				controller.DisableCollisions();
				animator.Play(vanish ? "Vanish" : "Die");
				if (!vanish)
				{
					PlaySFX("sounds/snd_freezie_die");
					Object.FindObjectOfType<MarioBrosNetworkManager>().KillFreezie(Object.FindObjectOfType<Player>(), base.transform.position);
				}
			}
		}

		public void StartFreezing(Vector3 position, int freezeSpot)
		{
			if (!freezing || this.freezeSpot == freezeSpot)
			{
				freezing = true;
				freezeCall = false;
				freezeTimer = 0f;
				this.freezeSpot = freezeSpot;
				base.transform.position = position;
				PlaySFX("sounds/snd_freeze");
				animator.SetFloat("Speed", 0f);
			}
		}

		public void FinishFreeze(int freezeSpot)
		{
			Die(-1);
			FreezeSpot[] array = Object.FindObjectsOfType<FreezeSpot>();
			foreach (FreezeSpot freezeSpot2 in array)
			{
				if (freezeSpot2.GetSpotID() == freezeSpot)
				{
					freezeSpot2.GetPlatform().Freeze();
				}
			}
		}

		public override void ChangeDirection(bool right)
		{
			base.ChangeDirection(right);
			sprite.flipX = !movingRight;
		}

		public override void SetNewAction(int playerId, int action, Vector3 position, int extraArg)
		{
			base.transform.position = position;
			switch (action)
			{
			case 0:
				BitePlayer(playerId, extraArg == 1);
				break;
			case 1:
				EnterPipe(false);
				break;
			case 2:
				Die(playerId);
				break;
			case 3:
				StartFreezing(position, extraArg);
				break;
			case 4:
				FinishFreeze(extraArg);
				break;
			}
		}

		public bool Grounded()
		{
			return controller.collisions.down;
		}
	}
}
