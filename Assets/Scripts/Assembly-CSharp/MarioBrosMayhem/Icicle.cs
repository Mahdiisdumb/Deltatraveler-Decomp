using UnityEngine;

namespace MarioBrosMayhem
{
	public class Icicle : StageObject
	{
		private readonly float GRAVITY = 0.005f;

		private bool spawned;

		private bool spikey;

		private float sparkleTimer;

		private bool revvingUp;

		private float revTimer;

		private Vector3 basePos = Vector3.zero;

		private float velocity;

		private float spawnTimer = 2f;

		private SpriteRenderer sparkles;

		protected override void Awake()
		{
			base.Awake();
			aud = GetComponent<AudioSource>();
			sparkles = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		}

		private void FixedUpdate()
		{
			if (!spawned && Object.FindObjectOfType<MarioBrosManager>().GetEnemyCount() > 0)
			{
				spawnTimer += Time.fixedDeltaTime;
				if (spawnTimer >= 3f)
				{
					ServerSpawnIn();
				}
			}
			if (spikey)
			{
				sparkleTimer += Time.fixedDeltaTime;
				sparkles.flipX = (int)(sparkleTimer * 60f) / 4 % 2 == 0;
			}
			if (revvingUp)
			{
				revTimer += Time.fixedDeltaTime;
				base.transform.position = basePos;
				if ((revTimer >= 2.0666666f && revTimer < 2.2f) || (revTimer >= 2.3333333f && revTimer < 2.3833334f))
				{
					base.transform.position += new Vector3(1f / 12f, 0f);
				}
				if (revTimer >= 2.3333333f && !spikey)
				{
					spikey = true;
					sparkles.enabled = true;
					sparkleTimer = 0f;
				}
				if (revTimer >= 2.5f)
				{
					revvingUp = false;
				}
			}
			else if (spawned)
			{
				velocity -= GRAVITY;
				base.transform.position += new Vector3(0f, velocity);
				if (base.transform.position.y < -6f)
				{
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Splash"), new Vector3(base.transform.position.x, -4.6666665f), Quaternion.identity);
					Die();
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnTriggerStay2D(collision);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (spawned && (bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract() && (!collision.GetComponent<Player>().IsInvincible() || !spikey) && (!revvingUp || !(revTimer < 1f)))
			{
				if (spikey)
				{
					collision.GetComponent<Player>().Damage(new Vector3(0f, velocity), revvingUp ? 1 : 2);
				}
				Die(!spikey);
			}
		}

		public void Die(bool playSound = false)
		{
			if (spawned)
			{
				revvingUp = false;
				GetComponent<Collider2D>().enabled = false;
				sparkles.enabled = false;
				spikey = false;
				animator.Play("Die");
				if (playSound)
				{
					aud.Play();
				}
				spawned = false;
				spawnTimer = Random.Range(0f, 1.5f);
			}
		}

		public void Kill(int playerId, bool playSound = true)
		{
			Die(playSound);
		}

		public void SpawnIn(Vector3 spawnPosition)
		{
			spawned = true;
			GetComponent<Collider2D>().enabled = true;
			animator.enabled = true;
			animator.Play("Icicle", 0, 0f);
			revTimer = 0f;
			revvingUp = true;
			velocity = 0f;
			base.transform.position = spawnPosition;
			basePos = spawnPosition;
		}

		public void ServerSpawnIn()
		{
			bool num = Random.Range(0, 10) == 0;
			Vector3 zero = Vector3.zero;
			zero = ((!num) ? new Vector3((float)(124 - Random.Range(0, 13) * 8) / 24f, 1.3333334f) : new Vector3(3.6666667f, 2.75f));
			if (Random.Range(0, 2) == 0)
			{
				zero.x *= -1f;
			}
			SpawnIn(zero);
		}

		public bool CanBeKilled()
		{
			return !spikey;
		}
	}
}
