using UnityEngine;

namespace MarioBrosMayhem
{
	public class Fighterfly : Enemy
	{
		private readonly float HOP_HEIGHT = 0.5f;

		private readonly float TIME_TO_HOP_APEX = 25f;

		private readonly float BASE_SPEED = 1f;

		private float groundTimer;

		private Sprite[] sprites;

		protected override void Awake()
		{
			base.Awake();
			SetGravityAndSpeed();
			fullRageWhenAlone = false;
			spawnSound = "mariobros/sounds/snd_spawn_fighterfly";
			sprites = Resources.LoadAll<Sprite>("mariobros/sprites/objects/enemies/spr_fighterfly");
			for (int i = 0; i < 3; i++)
			{
				palettes[i] = Resources.Load<Material>("mariobros/materials/enemies/fighterfly-" + i);
			}
			FixSprite();
		}

		private void LateUpdate()
		{
			FixSprite();
		}

		private void FixSprite()
		{
			int num = int.Parse(sprite.sprite.name.Substring(sprite.sprite.name.LastIndexOf("_") + 1));
			sprite.sprite = sprites[num];
		}

		protected override void MoveAlgorithm()
		{
			if (!controller.collisions.down || kicked || biting || respawning || flipped)
			{
				base.MoveAlgorithm();
				if (controller.collisions.down)
				{
					animator.SetBool("Grounded", value: true);
				}
				if (flipped || respawning)
				{
					groundTimer = 0f;
				}
				return;
			}
			groundTimer += Time.fixedDeltaTime;
			velocity.x = 0f;
			if (groundTimer >= (24f - 8f * (GetRageMultiplier() - 1f)) / 60f)
			{
				SetGravityAndSpeed();
				velocity.y = flipVelocity;
				animator.SetBool("Grounded", value: false);
				groundTimer = 0f;
			}
			controller.Move(velocity);
		}

		public override void TurnAround()
		{
			if (!turning && !biting && !flipped && speed != 0f)
			{
				movingRight = !movingRight;
				speed = 0f;
				speedStunned = true;
			}
		}

		private void SetGravityAndSpeed()
		{
			speed = BASE_SPEED * GetRageMultiplier();
			gravity = 2f * HOP_HEIGHT / Mathf.Pow(TIME_TO_HOP_APEX / GetRageMultiplier(), 2f);
			flipVelocity = gravity * (TIME_TO_HOP_APEX / GetRageMultiplier());
			if (!movingRight)
			{
				speed *= -1f;
			}
		}

		public override void IncreaseRage(int rage)
		{
			bool num = flipped;
			base.IncreaseRage(rage);
			SetGravityAndSpeed();
			animator.SetBool("Grounded", value: false);
			if (num)
			{
				velocity.y = flipVelocity;
			}
		}

		public override void ChangeDirection(bool movingRight)
		{
			base.ChangeDirection(movingRight);
			SetGravityAndSpeed();
		}

		public override void Flip(int playerId, int speedMultiplier, bool playAudio = true)
		{
			if (speedMultiplier == 0)
			{
				speedMultiplier = ((!movingRight) ? 1 : (-1));
				if (flipped)
				{
					speedMultiplier *= -1;
				}
			}
			base.Flip(playerId, speedMultiplier, playAudio);
			if (!flipped)
			{
				SetWalkSpeed();
				animator.SetBool("Grounded", value: false);
			}
		}

		private float GetRageMultiplier()
		{
			if (rage == 1)
			{
				return 1.5f;
			}
			if (rage == 2)
			{
				return 2f;
			}
			return 1f;
		}

		protected override void SetWalkSpeed()
		{
			base.SetWalkSpeed();
			SetGravityAndSpeed();
			if (rage == 0)
			{
				animator.SetFloat("Speed", 1f);
			}
			else if (rage == 1)
			{
				animator.SetFloat("Speed", 1.1428572f);
			}
			else
			{
				animator.SetFloat("Speed", 1.3333334f);
			}
		}
	}
}
