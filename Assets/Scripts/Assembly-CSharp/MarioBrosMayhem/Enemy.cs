using UnityEngine;

namespace MarioBrosMayhem
{
	public class Enemy : MovingObject
	{
		public enum SyncableActions
		{
			None = 0,
			FlipOver = 1,
			FlipBack = 2,
			Kick = 3,
			Respawn = 4,
			Bite = 5
		}

		protected readonly float FLIP_HEIGHT = 2f / 3f;

		protected readonly float TIME_TO_FLIP_APEX = 16.666666f;

		protected float[] walkSpeeds = new float[3]
		{
			43f / 48f,
			1.3333334f,
			2.6666667f
		};

		protected float flipVelocity;

		protected int rage;

		protected int walkRage;

		protected Material[] palettes = new Material[3];

		protected bool inPipe;

		protected bool biting;

		protected float biteTimer;

		protected bool flipped;

		protected float flipTimer;

		protected Vector3 flipLandPos = Vector3.zero;

		protected bool kicked;

		protected bool kickRight;

		protected float kickTimer;

		protected float spin;

		protected bool splashed;

		protected bool hostRegisterKick;

		protected bool turning;

		protected bool turned;

		protected float turnTimer;

		protected bool speedStunned;

		protected bool fullRageWhenAlone = true;

		protected int enemyNo = -1;

		protected override void Awake()
		{
			base.Awake();
			spawnSound = "mariobros/sounds/snd_spawn_shellcreeper";
			speed = walkSpeeds[0];
			sprite.sortingOrder = -4;
			SetWalkSpeed();
			gravity = 2f * FLIP_HEIGHT / Mathf.Pow(TIME_TO_FLIP_APEX, 2f);
			flipVelocity = gravity * TIME_TO_FLIP_APEX;
			for (int i = 0; i < 3; i++)
			{
				palettes[i] = Resources.Load<Material>("mariobros/materials/enemies/koopa-" + i);
			}
		}

		private void Start()
		{
			SpawnFromNearestPipe(disableCollisions: true);
		}

		private void Update()
		{
			if ((bool)Util.FindObjectOfType<MarioBrosManager>() && !kicked && !flipped && Util.FindObjectOfType<MarioBrosManager>().GetEnemyCount() == 1 && rage < 2 && fullRageWhenAlone)
			{
				IncreaseRage(2);
			}
		}

		public override bool IsPerformingAction()
		{
			if (!flipped && !kicked)
			{
				return biting;
			}
			return true;
		}

		protected override void ClientResync()
		{
			speed = walkSpeeds[rage + walkRage];
			if (!movingRight)
			{
				speed *= -1f;
			}
		}

		protected override void MoveAlgorithm()
		{
			if (kicked)
			{
				if (splashed)
				{
					return;
				}
				kickTimer += Time.fixedDeltaTime;
				if (kickTimer >= 0.25f && Mathf.Abs(speed) > 0f)
				{
					if (Mathf.Abs(speed) - 0.042f <= 0f)
					{
						speed = 0f;
					}
					else
					{
						speed = (Mathf.Abs(speed) - 0.042f) * Mathf.Sign(speed);
					}
				}
				spin += 675f * Time.fixedDeltaTime * (float)((!kickRight) ? 1 : (-1));
				sprite.transform.localRotation = Quaternion.Euler(0f, 0f, (int)spin / 45 * 45);
				if (Mathf.Abs(base.transform.position.x) >= 5.8333335f)
				{
					int num = ((!(base.transform.position.x > 0f)) ? 1 : (-1));
					base.transform.position += new Vector3(11.5f * (float)num, 0f);
				}
				if (base.transform.position.y < -6f && !splashed)
				{
					splashed = true;
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Splash"), new Vector3(base.transform.position.x, -4.6666665f), Quaternion.identity);
				}
				velocity.y -= gravity;
				velocity.x = speed * Time.fixedDeltaTime;
				sprite.flipX = !movingRight;
				controller.Move(velocity);
				return;
			}
			if (turning)
			{
				turnTimer += Time.fixedDeltaTime;
				if (turnTimer >= 0.2f && !turned)
				{
					movingRight = !movingRight;
					turned = true;
				}
				if (turnTimer >= 0.4f)
				{
					turning = false;
					animator.Play("Walk");
					speed = walkSpeeds[rage + walkRage];
					if (!movingRight)
					{
						speed *= -1f;
					}
					SetWalkSpeed();
				}
			}
			if (biting)
			{
				sprite.flipX = movingRight;
				biteTimer += Time.fixedDeltaTime;
				if (!(biteTimer >= 1f))
				{
					return;
				}
				biting = false;
				animator.Play("Walk");
			}
			if (respawning)
			{
				respawnTimer += Time.fixedDeltaTime;
				base.transform.position = new Vector3(base.transform.position.x + speed * Time.fixedDeltaTime, Mathf.Lerp(-4.3333335f, -3.9166667f, respawnTimer * 4f));
				if (respawnTimer >= 1.9166666f)
				{
					SpawnFromNearestPipe(disableCollisions: false);
					respawning = false;
					spawned = false;
				}
				return;
			}
			if (flipped && velocity.y <= 0f && controller.collisions.down)
			{
				flipTimer += Time.fixedDeltaTime;
				if (flipTimer >= 5.55f)
				{
					int num2 = Mathf.RoundToInt(flipTimer * 60f) - 333;
					if (rage < 2)
					{
						sprite.material = palettes[rage + 1];
					}
					sprite.transform.localPosition = new Vector2((num2 % 8 >= 4) ? (1f / 24f) : 0f, 0f);
				}
				if (flipTimer >= 7.55f)
				{
					IncreaseRage(rage + 1);
				}
			}
			if (controller.collisions.down && velocity.y <= 0f)
			{
				velocity.y = 0f;
				if (speedStunned && !flipped && !turning)
				{
					speedStunned = false;
					speed = walkSpeeds[rage + walkRage] * (float)(movingRight ? 1 : (-1));
					SetWalkSpeed();
				}
				else if (flipped)
				{
					speed = 0f;
					animator.SetFloat("Speed", 1f);
				}
			}
			velocity.y -= gravity;
			velocity.x = speed * Time.fixedDeltaTime;
			sprite.flipX = !movingRight;
			controller.Move(velocity);
		}

		private void OnTriggerEnter2D(Collider2D collision)
		{
			if ((bool)collision && !kicked && spawned)
			{
				if ((bool)collision.GetComponent<Player>())
				{
					OnTriggerStay2D(collision);
				}
				if (((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>())
				{
					OnTriggerStay2D(collision);
				}
				if (!respawning && (bool)collision.GetComponent<EnterPipe>() && !turning && ((base.transform.position.x > 0f && movingRight) || (base.transform.position.x < 0f && !movingRight)))
				{
					EnterPipe(serverCall: true);
				}
			}
		}

		private void OnTriggerStay2D(Collider2D collision)
		{
			if (kicked || !spawned)
			{
				return;
			}
			if ((bool)collision.GetComponent<Player>() && collision.GetComponent<Player>().CanInteract())
			{
				if (flipped)
				{
					collision.GetComponent<Player>().KickEnemy(base.transform.position.x > collision.transform.position.x);
					Kick(0, base.transform.position.x > collision.transform.position.x);
				}
				else if (!collision.GetComponent<Player>().IsInvincible())
				{
					collision.GetComponent<Player>().Damage(new Vector3((float)((!(base.transform.position.x > collision.transform.position.x)) ? 1 : (-1)) / 24f, 0.05f));
					BitePlayer(0, base.transform.position.x < collision.transform.position.x);
				}
			}
			if ((((bool)collision.GetComponent<Enemy>() && !collision.GetComponent<Enemy>().Kicked()) || (bool)collision.GetComponent<MovingObject>()) && ((collision.transform.position.x > base.transform.position.x && movingRight) || (collision.transform.position.x < base.transform.position.x && !movingRight)))
			{
				TurnAround();
			}
		}

		public void BitePlayer(int playerId, bool playerOnRight)
		{
			turning = false;
			biting = true;
			biteTimer = 0f;
			movingRight = !playerOnRight;
			speed = walkSpeeds[rage + walkRage];
			if (!movingRight)
			{
				speed *= -1f;
			}
			sprite.flipX = movingRight;
			animator.Play("Bite");
		}

		public void Flip(int playerId, Vector3 hitPosition)
		{
			if (Mathf.Abs(base.transform.position.x - hitPosition.x) < 1f / 6f && !GetComponent<Fighterfly>())
			{
				Flip(playerId, 0);
			}
			else
			{
				Flip(playerId, (!(base.transform.position.x < hitPosition.x)) ? 1 : (-1));
			}
		}

		public virtual void Flip(int playerId, int speedMultiplier, bool playAudio = true)
		{
			if (!kicked && !respawning && spawned)
			{
				animator.SetFloat("Speed", 0f);
				turning = false;
				biting = false;
				velocity.y = flipVelocity;
				flipped = !flipped;
				flipTimer = 0f;
				if ((bool)Util.FindObjectOfType<MarioBrosManager>() && Util.FindObjectOfType<MarioBrosManager>().GetPhaseNumber() >= 23)
				{
					flipTimer = 3.7666667f;
				}
				speedStunned = true;
				if (flipped)
				{
					speed = walkSpeeds[rage + walkRage] * (float)speedMultiplier;
					animator.Play("Flip", 0, (flipTimer > 0f) ? 0.5f : 0f);
				}
				else
				{
					speed = 0f;
					animator.Play("Walk");
				}
				sprite.transform.localPosition = Vector2.zero;
				sprite.material = palettes[rage];
				if (playAudio)
				{
					PlaySFX("sounds/snd_enemy_flip");
				}
				if (!playAudio)
				{
					speedMultiplier = ((speedMultiplier != 0) ? (speedMultiplier * 2) : 3);
				}
			}
		}

		public void Kick(int playerId, bool kickRight)
		{
			kicked = true;
			speed = walkSpeeds[2] * 1.4f;
			this.kickRight = kickRight;
			if (!kickRight)
			{
				speed *= -1f;
			}
			animator.Play("Kick");
			sprite.transform.localPosition = new Vector3(0f, 0.25f);
			gravity = 2f * FLIP_HEIGHT / Mathf.Pow(TIME_TO_FLIP_APEX, 2f);
			flipVelocity = gravity * TIME_TO_FLIP_APEX;
			velocity.y = flipVelocity;
			PlaySFX("sounds/snd_enemy_downed");
			controller.DisableCollisions();
			Util.FindObjectOfType<MarioBrosNetworkManager>().EnemyDefeatedServerRpc(Util.FindObjectOfType<Player>(), enemyNo, Util.FindObjectOfType<MarioBrosManager>().GetMyPlayer().GetMultiKicks(), base.transform.position);
			Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/KickEffect"), Vector3.MoveTowards(base.transform.position, Util.FindObjectOfType<Player>().transform.position, Vector3.Distance(base.transform.position, Util.FindObjectOfType<Player>().transform.position) / 2f) + new Vector3(0f, 0.1f), Quaternion.identity);
			if ((bool)Util.FindObjectOfType<MarioBrosManager>())
			{
				Util.FindObjectOfType<MarioBrosManager>().DefeatEnemy(enemyNo);
			}
		}

		public virtual void IncreaseRage(int rage)
		{
			if (flipped)
			{
				flipped = false;
				movingRight = !movingRight;
			}
			if (rage > 2)
			{
				rage = 2;
			}
			this.rage = rage;
			speedStunned = true;
			sprite.material = palettes[rage];
			animator.Play("Walk");
			SetWalkSpeed();
		}

		public virtual void TurnAround()
		{
			if (!turning && !biting && !flipped && speed != 0f)
			{
				turning = true;
				turned = false;
				turnTimer = 0f;
				animator.Play("Turning");
				speed = 0f;
			}
		}

		protected virtual void SetWalkSpeed()
		{
			if (rage == 0)
			{
				animator.SetFloat("Speed", 0.9230769f);
			}
			else if (rage == 1)
			{
				animator.SetFloat("Speed", 1.0909091f);
			}
			else
			{
				animator.SetFloat("Speed", 1.3333334f);
			}
		}

		public override void ChangeDirection(bool movingRight)
		{
			speed = walkSpeeds[rage + walkRage];
			base.ChangeDirection(movingRight);
			sprite.flipX = !movingRight;
		}

		public void SetEnemyNumber(int enemyNo)
		{
			this.enemyNo = enemyNo;
		}

		public bool Grounded()
		{
			return controller.collisions.down;
		}

		public bool Kicked()
		{
			return kicked;
		}

		public bool IsFlipped()
		{
			return flipped;
		}

		public int GetEnemyNumber()
		{
			return enemyNo;
		}
	}
}
