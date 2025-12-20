using System.Collections.Generic;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class Fireball : StageObject
	{
		private readonly float[] ROW_POSITIONS = new float[4] { 2f, 0f, -2f, -4f };

		private readonly float MAX_HEIGHT = 1f;

		private readonly float HEIGHT_TIME_SCALE = 0.4f;

		[SerializeField]
		private bool red;

		private bool spawned;

		private bool movingRight;

		private float moveTimer;

		private float bounceTimer;

		private bool revvingUp;

		private float revTimer;

		private int lastAudioPlay;

		private bool die;

		private bool dead;

		private float dieFrames;

		private float rowY;

		private Vector3 velocity = Vector3.zero;

		private bool rage;

		private bool hard;

		private bool hardRage;

		private bool secondFireball;

		private AudioSource dieAud;

		private float spawnTimer = 4.5f;

		private float spawnTime = 4.5f;

		protected override void Awake()
		{
			base.Awake();
			aud = GetComponents<AudioSource>()[0];
			dieAud = GetComponents<AudioSource>()[1];
			hard = Object.FindObjectOfType<ServerSessionManager>().GetRuleValue(0, 3) == 1;
			secondFireball = Object.FindObjectsOfType<Fireball>().Length > 1;
			if (hard && (!secondFireball || (secondFireball && red)))
			{
				spawnTime = 2f;
			}
			else if (hard && secondFireball && !red)
			{
				spawnTime = 3f;
			}
			else
			{
				spawnTime = 4.5f;
			}
		}

		private void FixedUpdate()
		{
			if (!spawned && Object.FindObjectOfType<MarioBrosManager>().GetEnemyCount() > 0)
			{
				spawnTimer += Time.fixedDeltaTime;
				if (spawnTimer >= spawnTime)
				{
					spawned = true;
					spawnTimer = 0f;
					List<Player> list = new List<Player>();
					Player[] array = Object.FindObjectsOfType<Player>();
					foreach (Player player in array)
					{
						if (!player.IsDead())
						{
							list.Add(player);
						}
					}
					Player player2 = null;
					if (list.Count > 0)
					{
						player2 = list[Random.Range(0, list.Count)];
					}
					ServerSpawnIn(player2);
				}
			}
			if (revvingUp)
			{
				revTimer += Time.fixedDeltaTime;
				int num = (int)(revTimer * 60f) / 50;
				if (num > lastAudioPlay)
				{
					aud.Play();
					lastAudioPlay = num;
				}
				if (revTimer >= (hard ? 0.75f : 2.9166667f))
				{
					animator.Play("Fireball");
					revvingUp = false;
					lastAudioPlay = 0;
					GetComponent<Collider2D>().enabled = true;
				}
			}
			else if (!die)
			{
				if (red)
				{
					controller.Move(velocity * Time.fixedDeltaTime);
					if ((controller.collisions.right && velocity.x > 0f) || (controller.collisions.left && velocity.x < 0f))
					{
						velocity.x *= -1f;
						aud.Play();
					}
					else if ((controller.collisions.up && velocity.y > 0f) || (controller.collisions.down && velocity.y < 0f))
					{
						velocity.y *= -1f;
						aud.Play();
					}
				}
				else
				{
					moveTimer += Time.fixedDeltaTime;
					if (moveTimer <= 7.266667f)
					{
						bounceTimer += Time.fixedDeltaTime;
					}
					float num2 = Mathf.Lerp(5f, -5f, moveTimer / (hard ? 3.6f : 7.266667f));
					if (movingRight)
					{
						num2 *= -1f;
					}
					float num3 = Mathf.Lerp(0f, MAX_HEIGHT, bounceTimer / HEIGHT_TIME_SCALE);
					if (bounceTimer >= 1f * HEIGHT_TIME_SCALE)
					{
						if (bounceTimer < 2f * HEIGHT_TIME_SCALE)
						{
							num3 = Mathf.Lerp(0f, MAX_HEIGHT, 2f - bounceTimer / HEIGHT_TIME_SCALE);
							if (lastAudioPlay < 1)
							{
								aud.Play();
								lastAudioPlay++;
							}
						}
						else if (bounceTimer < 2.5f * HEIGHT_TIME_SCALE)
						{
							num3 = Mathf.Lerp(0f, MAX_HEIGHT / 2f, 2f * bounceTimer / HEIGHT_TIME_SCALE - 4f);
							if (lastAudioPlay < 2)
							{
								aud.Play();
								lastAudioPlay++;
							}
						}
						else if (bounceTimer < 3f * HEIGHT_TIME_SCALE)
						{
							num3 = Mathf.Lerp(0f, MAX_HEIGHT / 2f, 6f - 2f * bounceTimer / HEIGHT_TIME_SCALE);
							if (lastAudioPlay < 3)
							{
								aud.Play();
								lastAudioPlay++;
							}
						}
						else
						{
							bounceTimer -= 3f * HEIGHT_TIME_SCALE;
							aud.Play();
							num3 = Mathf.Lerp(0f, MAX_HEIGHT, bounceTimer / HEIGHT_TIME_SCALE);
							lastAudioPlay = 0;
						}
					}
					num3 += rowY;
					base.transform.position = new Vector3(num2, num3);
					if (moveTimer >= (hard ? 3.6f : 7.5f))
					{
						Die();
					}
				}
			}
			if (die && !dead)
			{
				dieFrames += Time.fixedDeltaTime;
				if (dieFrames >= 0.4f)
				{
					animator.Play("Die");
					dead = true;
					spawned = false;
				}
			}
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			OnTriggerStay2D(collision);
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (!die && !dead && (bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract() && !collision.GetComponent<Player>().IsInvincible())
			{
				collision.GetComponent<Player>().Damage(new Vector3((float)((!(base.transform.position.x > collision.transform.position.x)) ? 1 : (-1)) / 15f, 0.15f), 0);
				Die();
			}
		}

		public void Die(bool playSound = false, bool instant = false)
		{
			if (!dead)
			{
				revvingUp = false;
				die = true;
				dead = instant;
				dieFrames = 0f;
				if (rage && hard)
				{
					hardRage = true;
				}
				rage = true;
				GetComponent<Collider2D>().enabled = false;
				if (instant)
				{
					animator.Play("Die");
					spawned = false;
				}
				else
				{
					animator.Play("Small");
				}
				if (playSound)
				{
					aud.Stop();
					dieAud.Play();
				}
			}
		}

		public void Kill(int playerId, bool playSound = true)
		{
			Die(playSound, true);
		}

		public void SpawnIn(Vector3 spawnPosition, bool movingRight, float rowY)
		{
			die = false;
			dead = false;
			this.movingRight = movingRight;
			this.rowY = rowY;
			GetComponent<Collider2D>().enabled = false;
			GetComponent<SpriteRenderer>().flipX = !red && movingRight;
			animator.Play("Spawn");
			revTimer = 0f;
			revvingUp = true;
			lastAudioPlay = 0;
			velocity = new Vector3(movingRight ? 1 : (-1), -1f);
			if (rage)
			{
				velocity *= 2f;
			}
			if (hard)
			{
				velocity *= (hardRage ? 2f : 1.5f);
			}
			moveTimer = 0f;
			bounceTimer = 0f;
			base.transform.position = spawnPosition;
		}

		public void ServerSpawnIn(Player player)
		{
			Vector3 spawnPosition = Vector3.zero;
			if (red)
			{
				movingRight = Random.Range(0, 2) == 0;
				spawnPosition = new Vector3((!movingRight) ? 1 : (-1), 4.25f);
			}
			else
			{
				if ((bool)player)
				{
					for (int i = 0; i < 4; i++)
					{
						if (player.transform.position.y > ROW_POSITIONS[i] - 2f / 3f)
						{
							rowY = ROW_POSITIONS[i];
							break;
						}
					}
					movingRight = player.transform.position.x > 0f;
				}
				else
				{
					movingRight = Random.Range(0, 2) == 0;
					rowY = ROW_POSITIONS[Random.Range(0, ROW_POSITIONS.Length)];
				}
				spawnPosition = new Vector3(movingRight ? (-5) : 5, rowY);
			}
			SpawnIn(spawnPosition, movingRight, rowY);
		}

		public bool IsRed()
		{
			return red;
		}

		public bool CanBeKilled()
		{
			if (revvingUp)
			{
				return !die;
			}
			return true;
		}
	}
}
