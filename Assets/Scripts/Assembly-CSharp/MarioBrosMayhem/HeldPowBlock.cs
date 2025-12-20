using UnityEngine;

namespace MarioBrosMayhem
{
	public class HeldPowBlock : HeldObject
	{
		private readonly float[] LEVEL_HEIGHTS = new float[3]
		{
			1f / 3f,
			0.5f,
			2f / 3f
		};

		[SerializeField]
		private Sprite[] sprites;

		private float speed;

		private float yVelocity;

		private bool thrown;

		private float timer;

		private int level;

		private int playerId = -1;

		private int powId;

		private bool detectGround;

		private CharacterController2D controller;

		private void Awake()
		{
			controller = GetComponent<CharacterController2D>();
		}

		private void FixedUpdate()
		{
			if (!thrown)
			{
				return;
			}
			yVelocity -= 0.01f;
			controller.Move(new Vector3(speed, yVelocity));
			timer += Time.fixedDeltaTime;
			if (timer >= 0.1f && !detectGround)
			{
				detectGround = true;
				controller.EnableCollisions();
			}
			else
			{
				if (!detectGround || !controller.collisions.down || !(yVelocity < 0f))
				{
					return;
				}
				PowBlock[] array = Object.FindObjectsOfType<PowBlock>();
				foreach (PowBlock powBlock in array)
				{
					if (powBlock.GetPowId() == powId)
					{
						powBlock.PowToLevel(playerId, 0, true, true);
						break;
					}
				}
				Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/PowDie"), base.transform.position + new Vector3(2f / 3f, 1f / 6f - (LEVEL_HEIGHTS[level] - 1f / 3f) / 2f), Quaternion.identity);
				Object.Destroy(base.gameObject);
			}
		}

		public void SetLevel(int level, int powId, int playerId)
		{
			this.playerId = playerId;
			this.powId = powId;
			this.level = level;
			GetComponent<SpriteRenderer>().sprite = sprites[level];
			GetComponent<BoxCollider2D>().size = new Vector2(2f / 3f, LEVEL_HEIGHTS[level]);
			if ((bool)Object.FindObjectOfType<BattleManager>())
			{
				GetComponent<SpriteRenderer>().material = Resources.Load<Material>("mariobros/materials/objects/pow-block-red");
			}
			controller.CalculateRaySpacing();
			controller.DisableCollisions();
		}

		public override void Throw(bool facingRight, Vector3 velocity)
		{
			base.Throw(facingRight, velocity);
			thrown = true;
			speed = 0.075f;
			if (!facingRight)
			{
				speed *= -1f;
			}
			speed += velocity.x;
			yVelocity = velocity.y;
			if (yVelocity > 0f)
			{
				controller.EnableCollisions();
			}
		}

		public override Vector3 GetPositionOffset()
		{
			return new Vector3(0f, (LEVEL_HEIGHTS[level] - 2f / 3f) / 2f);
		}
	}
}
