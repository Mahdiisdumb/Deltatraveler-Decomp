using UnityEngine;

namespace MarioBrosMayhem
{
	public class Mushroom : MovingObject
	{
		private bool collected;

		private bool hostCheck;

		protected override void Awake()
		{
			base.Awake();
			spawnSound = "mariobros/sounds/snd_spawn";
			sprite.sortingOrder = -4;
			gravity = 1.3333334f / Mathf.Pow(16.666666f, 2f);
			speed = 2f;
		}

		protected override void FixedUpdate()
		{
			base.FixedUpdate();
			if (spawnLife >= 100 && !spawned && !startedSpawn && !collected)
			{
				startedSpawn = true;
				SpawnFromNearestPipe(true);
			}
		}

		protected override void MoveAlgorithm()
		{
			if (collected)
			{
				return;
			}
			if (!respawning)
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
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if (!respawning && spawned)
			{
				if ((bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract())
				{
					Collect(0);
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
			if (!respawning && spawned && (((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>()) && ((collision.transform.position.x > base.transform.position.x && movingRight) || (collision.transform.position.x < base.transform.position.x && !movingRight)))
			{
				ChangeDirection(!movingRight);
			}
		}

		public void Collect(int playerId)
		{
			if ((respawning && playerId == -1) || collected)
			{
				return;
			}
			collected = true;
			spawnSound = "";
			controller.DisableCollisions();
			base.transform.position += new Vector3(0f, 0.25f);
			Player playerObject = Object.FindObjectOfType<MarioBrosNetworkManager>().GetPlayerObject(playerId);
			if ((bool)playerObject)
			{
				if (playerObject.IsBig())
				{
					PlaySFX("sounds/snd_player_grow");
				}
				bool num = !playerObject.AtMaxHealth();
				playerObject.IncreaseHealth();
				int points = ((!num) ? 1200 : 800);
				playerObject.AddPoints(points);
				Object.FindObjectOfType<MarioBrosNetworkManager>().CreateScoreGraphic(points, base.transform.position);
			}
			sprite.enabled = false;
		}

		public override void EnterPipe(bool serverCall)
		{
			base.EnterPipe(serverCall);
		}

		public void HitFromBelow(int playerId)
		{
			velocity.y = 0.1f;
		}

		public bool Grounded()
		{
			return controller.collisions.down;
		}
	}
}
