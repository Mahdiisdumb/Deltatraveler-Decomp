using UnityEngine;

namespace MarioBrosMayhem
{
	public class Coin : MovingObject
	{
		[SerializeField]
		private bool stationary;

		private bool collected;

		private bool collectRegistered;

		private float collectTimer;

		private bool collectedByPlayer;

		private bool showScore;

		protected override void Awake()
		{
			base.Awake();
			spawnSound = "mariobros/sounds/snd_spawn";
			sprite.sortingOrder = -4;
			gravity = 1.3333334f / Mathf.Pow(16.666666f, 2f);
			if (stationary)
			{
				spawned = true;
				sprite.sortingOrder = -1;
			}
			GetComponent<Animator>().Play("Coin", 0, (float)Random.Range(0, 5) / 5f);
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (spawnLife >= 100 && !spawned && !startedSpawn && !collected && !stationary)
			{
				startedSpawn = true;
				SpawnFromNearestPipe(disableCollisions: true);
			}
		}

		public override bool IsPerformingAction()
		{
			return collected;
		}

		protected override void MoveAlgorithm()
		{
			if (!collected && !respawning && !stationary)
			{
				if (controller.collisions.down && velocity.y <= 0f)
				{
					velocity.y = 0f;
				}
				velocity.y -= gravity;
				velocity.x = speed * Time.fixedDeltaTime;
				controller.Move(velocity);
			}
			else if (respawning)
			{
				respawnTimer += Time.fixedDeltaTime;
				base.transform.position = new Vector3(base.transform.position.x + speed * Time.fixedDeltaTime, Mathf.Lerp(-4.3333335f, -3.9166667f, respawnTimer * 4f));
			}
			else if (collected)
			{
				collectTimer += Time.fixedDeltaTime;
				if (collectTimer < 17f / 60f)
				{
					base.transform.position += new Vector3(0f, 2.5f * Time.fixedDeltaTime);
				}
				if (!stationary && collectedByPlayer && !showScore && collectTimer >= 2f / 3f)
				{
					showScore = true;
					Util.FindObjectOfType<MarioBrosNetworkManager>().CreateScoreGraphic(800, base.transform.position);
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if ((bool)collision && !respawning && !collected && spawned)
			{
				if ((bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract())
				{
					CollectCoin(0);
				}
				if (((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>())
				{
					OnTriggerStay2D(collision);
				}
				if (!respawning && (bool)collision.GetComponent<EnterPipe>() && ((base.transform.position.x > 0f && movingRight) || (base.transform.position.x < 0f && !movingRight)))
				{
					EnterPipe(serverCall: true);
				}
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (!respawning && !collected && spawned && (((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>()) && ((collision.transform.position.x > base.transform.position.x && movingRight) || (collision.transform.position.x < base.transform.position.x && !movingRight)))
			{
				ChangeDirection(!movingRight);
			}
		}

		public void CollectCoin(int playerId)
		{
			if ((respawning && playerId == -1) || collected)
			{
				return;
			}
			spawnSound = "";
			if (playerId != -1)
			{
				aud.Play();
				collectedByPlayer = true;
				if (!stationary)
				{
					Util.FindObjectOfType<Player>().AddPoints(800);
				}
			}
			collected = true;
			controller.DisableCollisions();
			base.transform.position += new Vector3(0f, 0.25f);
			if (!spawned)
			{
				sprite.enabled = false;
			}
			else
			{
				animator.Play((playerId == -1) ? "Vanish" : "Collect", 0, 0f);
			}
		}

		public override void EnterPipe(bool serverCall)
		{
			base.EnterPipe(serverCall);
		}

		public bool IsCollected()
		{
			return collected;
		}

		public bool Grounded()
		{
			return controller.collisions.down;
		}

		public int GetCollector()
		{
			if (!collectedByPlayer)
			{
				return -1;
			}
			return 0;
		}
	}
}
