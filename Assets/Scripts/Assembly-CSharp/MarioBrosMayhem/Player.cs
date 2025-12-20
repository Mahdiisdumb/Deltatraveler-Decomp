using UnityEngine;

namespace MarioBrosMayhem
{
	public class Player : StageObject
	{
		private readonly float MAX_JUMP_HEIGHT = 2.3333333f;

		private readonly float CHARGE_JUMP_HEIGHT = 3.25f;

		private readonly float TIME_TO_JUMP_APEX = 21.667f;

		private readonly float MIN_JUMP_VELOCITY = 0.16567536f;

		private readonly float MAX_SPEED_WALK = 0.05f;

		private readonly float MAX_SPEED_RUN = 0.065f;

		private readonly float ACCELERATION_WALK = 1f / 120f;

		private readonly float ACCELERATION_RUN = 0.010833333f;

		private readonly float DECELERATION_RATE = 0.0039999997f;

		private readonly int[] WALK_SOUND_PATTERN = new int[8] { 1, 2, 1, 2, 1, 2, 1, 0 };

		private readonly float ITIMER_CLASSIC_MODE = 2f;

		private readonly float ITIMER_BATTLE_MODE = 5f;

		private readonly float ICE_ACCELERATION_RATE = 0.000942029f;

		private readonly Vector2 HITBOX_SIZE = new Vector2(1f / 3f, 0.5f);

		private readonly float MULTIKICK_TIME = 1.15f;

		[SerializeField]
		private bool dummyPlayer;

		private bool bigMode;

		private int maxHealth = 2;

		private bool startSuper = true;

		private bool iFramesAfterDeath = true;

		private int stunTime = -1;

		private float speed;

		private float gravity;

		private float jumpVelocity;

		private float jumpSpeedIncrease;

		private float animSpeed;

		private Vector3 velocity;

		private bool skidding;

		private float skidEffectTimer;

		private bool onIce;

		private bool jumpButtonHolding;

		private bool facingRight = true;

		private bool canMove;

		private bool forceMove;

		private bool frozen = true;

		private bool pickupButtonHolding;

		private float yVelocity;

		private bool hitCeiling;

		private bool crouching;

		private int crouchCharge;

		private bool crouchCharged;

		private float chargedJump;

		private bool doingChargedJump;

		private float stepFrames;

		private int steps;

		private int glowFrames;

		private bool stomped;

		private bool stompHoldAxis;

		private int stompFrames;

		private int lives = 3;

		private bool dead;

		private bool dieFall;

		private float dieTimer;

		private bool splashed;

		private bool quit;

		private int dieCondition = -1;

		private Material[] diePalettes = new Material[2];

		private float reviveTimer;

		private bool reviving;

		private AngelPlatform angelPlatform;

		private LivesBubble livesBubble;

		private Transform headItems;

		private SpriteRenderer identifier;

		private bool kicking;

		private bool kickingRight;

		private float kickTimer;

		private float multikickTimer;

		private int multikicks;

		private AudioSource voice;

		private bool playVoices = true;

		private Sprite[] smallSprites;

		private Sprite[] bigSprites;

		private bool isBig;

		private BoxCollider2D collider;

		private bool invulnerable;

		private float iTimer;

		private float iMaxTimer = 2f;

		private float iFlashTimer;

		private int health = 1;

		private bool changingSize;

		private float changeSizeTimer;

		private bool pickingUp;

		private float pickupTimer;

		private bool holdingObject;

		private Transform heldObject;

		private bool throwing;

		private bool throwToRight;

		private float throwTimer;

		private bool thrown;

		private float thrownTimer;

		private bool beingHeld;

		private Vector3 pickupPosition;

		private int escapeInputs;

		private bool bumpedPlayerThisUpdate;

		private bool pauselock;

		private bool battlemode;

		private int skin;

		public int palette;

		private int points;

		private string skinName = "mario";

		private bool nPlayerGroundedBefore;

		private readonly int[] POINTS_MILESTONES = new int[2] { 20000, 100000 };

		protected override void Awake()
		{
			base.Awake();
			aud = GetComponents<AudioSource>()[0];
			voice = GetComponents<AudioSource>()[1];
			angelPlatform = GetComponentInChildren<AngelPlatform>();
			livesBubble = GetComponentInChildren<LivesBubble>();
			headItems = base.transform.Find("HeadItems");
			identifier = headItems.Find("Identifier").GetComponent<SpriteRenderer>();
			collider = GetComponent<BoxCollider2D>();
			gravity = (0f - 2f * MAX_JUMP_HEIGHT) / Mathf.Pow(TIME_TO_JUMP_APEX, 2f);
			jumpVelocity = (0f - gravity) * TIME_TO_JUMP_APEX;
			chargedJump = CHARGE_JUMP_HEIGHT / MAX_JUMP_HEIGHT * jumpVelocity;
			jumpSpeedIncrease = (chargedJump - jumpVelocity) * 0.35f;
			ServerSessionManager serverSessionManager = Object.FindObjectOfType<ServerSessionManager>();
			if (serverSessionManager.GetGamemode() == 0)
			{
				lives = serverSessionManager.GetRuleValue(0, 0);
				bigMode = serverSessionManager.GetRuleValue(0, 2) == 2;
				maxHealth = (bigMode ? 1 : 2);
				startSuper = serverSessionManager.GetRuleValue(0, 2) == 1;
				iFramesAfterDeath = serverSessionManager.GetRuleValue(0, 5) == 1;
				iMaxTimer = ITIMER_CLASSIC_MODE;
			}
			ResetHealth();
			if (isBig)
			{
				ResizeHitbox();
			}
			controller.collisions.down = true;
		}

		public void Start()
		{
			SetAppearance();
			MonoBehaviour.print("player start");
		}

		private void Update()
		{
			if (!pauselock && !frozen && (!Object.FindObjectOfType<GameOverContinue>() || !Object.FindObjectOfType<GameOverContinue>().IsActive()) && lives >= 0 && ((UTInput.GetButtonDown("C") && canMove) || (!Object.FindObjectOfType<PauseMenu>().Paused() && !canMove)))
			{
				if (canMove)
				{
					canMove = false;
					StopLoopingSFX();
					Object.FindObjectOfType<PauseMenu>().Pause();
				}
				else
				{
					if (!dummyPlayer)
					{
						canMove = true;
					}
					jumpButtonHolding = true;
				}
			}
			else if (pauselock)
			{
				pauselock = false;
			}
			if (Time.timeScale != 0f && skidding && !frozen)
			{
				skidEffectTimer += Time.deltaTime;
				if (skidEffectTimer >= 1f / 15f)
				{
					skidEffectTimer -= 1f / 15f;
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/fx/Skid"), new Vector3(base.transform.position.x + 0.375f * (float)((!facingRight) ? 1 : (-1)), base.transform.position.y), Quaternion.identity);
				}
			}
		}

		private void FixedUpdate()
		{
			if (multikickTimer > 0f)
			{
				multikickTimer -= Time.fixedDeltaTime;
				if (multikickTimer <= 0f)
				{
					multikickTimer = 0f;
					multikicks = 0;
				}
			}
			if (frozen)
			{
				return;
			}
			if (pickingUp)
			{
				pickupTimer += Time.fixedDeltaTime;
				if ((bool)heldObject)
				{
					if (heldObject.gameObject.name.Contains("Pow"))
					{
						heldObject.transform.position = base.transform.position + new Vector3(0f, Mathf.Lerp(-1f / 3f, 1.0416666f + heldObject.GetComponent<HeldPowBlock>().GetPositionOffset().y, pickupTimer / (1f / 3f)));
					}
					else if (heldObject.gameObject.name.Contains("Player"))
					{
						heldObject.transform.position = base.transform.position + new Vector3(pickupPosition.x, Mathf.Lerp(pickupPosition.y, 0.375f, pickupTimer / (1f / 3f)));
					}
				}
				if (!(pickupTimer >= 1f / 3f))
				{
					return;
				}
				pickingUp = false;
				if (playVoices)
				{
					PlayVoice("pickup_1");
				}
				animator.Play("HoldJump");
			}
			if (!pickingUp && (bool)heldObject && !heldObject.parent)
			{
				heldObject.parent = base.transform.Find("HeadItems");
				if (heldObject.gameObject.name.Contains("Pow"))
				{
					heldObject.localPosition = new Vector3(0f, 5f / 24f) + heldObject.GetComponent<HeldPowBlock>().GetPositionOffset();
				}
			}
			if (!heldObject && holdingObject)
			{
				holdingObject = false;
				if (stomped)
				{
					animator.Play("Stomped");
				}
				else if (crouching)
				{
					animator.Play("Crouched");
				}
				else
				{
					animator.Play("Idle");
				}
			}
			if (invulnerable)
			{
				iTimer += Time.fixedDeltaTime;
				if (iTimer >= iMaxTimer)
				{
					iTimer = 0f;
					invulnerable = false;
					controller.PlayerCollisions();
				}
			}
			if (dead)
			{
				HandleDieMovement();
				return;
			}
			if (reviving)
			{
				HandleReviveMovement();
				if (reviving)
				{
					return;
				}
			}
			if (stomped)
			{
				stompFrames++;
				if (stompFrames > stunTime && controller.collisions.down)
				{
					animator.SetBool("Stomped", false);
					stomped = false;
					skidding = false;
					stompFrames = 0;
				}
			}
			if (stomped)
			{
				HandleStompedMovement();
			}
			else
			{
				HandleMovement();
			}
			WallDetection();
			VerticalDetection();
			bumpedPlayerThisUpdate = false;
			if (dead || reviving)
			{
				return;
			}
			if (animSpeed > 0.2f && !skidding && !stomped && !kicking && controller.collisions.down)
			{
				stepFrames += ((animSpeed > 1f) ? 1f : animSpeed);
				if (stepFrames >= 8f)
				{
					stepFrames = 0f;
					PlaySFX("mariobros/sounds/snd_player_step" + WALK_SOUND_PATTERN[steps % WALK_SOUND_PATTERN.Length]);
					steps++;
				}
			}
			if (crouchCharged)
			{
				if (glowFrames % 2 == 0)
				{
					float brightness = (new float[4] { 1.1f, 1.2f, 1.1f, 1f })[glowFrames / 2 % 4];
					SetBrightness(brightness);
				}
				glowFrames++;
			}
		}

		private void LateUpdate()
		{
			FixSprite();
		}

		private void HandleMovement()
		{
			if (!UTInput.GetButton("Z") && jumpButtonHolding)
			{
				jumpButtonHolding = false;
				if (yVelocity > MIN_JUMP_VELOCITY + JumpSpeedIncrease() && !doingChargedJump)
				{
					yVelocity = MIN_JUMP_VELOCITY + JumpSpeedIncrease();
				}
			}
			if (UTInput.GetButton("Z") && !jumpButtonHolding && controller.collisions.down && canMove)
			{
				DoAction();
			}
			crouching = UTInput.GetAxis("Vertical") < 0f && controller.collisions.down && canMove;
			if (crouching)
			{
				skidding = false;
				animator.Play(holdingObject ? "HoldCrouch" : "Crouch");
				crouchCharge++;
			}
			else if (!crouching && crouchCharge < 42)
			{
				ResetCrouchCharge();
			}
			if (crouchCharge == 42 && !crouchCharged)
			{
				crouchCharged = true;
				PlaySFX("mariobros/sounds/snd_player_charged");
				if (playVoices)
				{
					PlayVoice("charged");
				}
			}
			float num = UTInput.GetAxis("Horizontal");
			if (crouching)
			{
				num = 0f;
			}
			bool flag = num > 0f;
			bool flag2 = num != 0f;
			bool button = UTInput.GetButton("X");
			if (!flag2 && forceMove && !skidding)
			{
				flag = facingRight;
			}
			if (!thrown)
			{
				if ((flag2 || forceMove) && canMove)
				{
					if (speed == 0f)
					{
						facingRight = flag;
						if (crouchCharged)
						{
							ResetCrouchCharge();
						}
					}
					if (!controller.collisions.down && facingRight != flag)
					{
						facingRight = flag;
						skidding = false;
					}
					float num2 = (flag ? 1 : (-1));
					float num3 = ((button && !holdingObject) ? ACCELERATION_RUN : ACCELERATION_WALK);
					if ((!button || flag != facingRight) && onIce)
					{
						num3 = ICE_ACCELERATION_RATE;
					}
					float num4 = ((button && !holdingObject) ? MAX_SPEED_RUN : MAX_SPEED_WALK);
					if (!skidding && controller.collisions.down)
					{
						skidding = flag != facingRight && (Mathf.Abs(speed) > MAX_SPEED_WALK / 2f || onIce);
					}
					else if ((skidding && flag == facingRight) || !controller.collisions.down)
					{
						skidding = false;
					}
					if (Mathf.Abs(speed) < num4 || Mathf.Sign(speed) != num2 || flag != facingRight)
					{
						speed += num3 * num2;
						if (forceMove && Mathf.Abs(speed) < MAX_SPEED_WALK / 2f)
						{
							speed = MAX_SPEED_WALK / 2f * Mathf.Sign(speed);
						}
						if (Mathf.Abs(speed) > num4 && !skidding)
						{
							speed = num4 * num2;
						}
						else if ((speed > 0f && !facingRight) || (speed < 0f && facingRight))
						{
							facingRight = flag;
							skidding = false;
						}
					}
					else if (Mathf.Abs(speed) > num4 && !skidding)
					{
						float num5 = Mathf.Abs(speed) - DECELERATION_RATE;
						if (num5 <= num4)
						{
							speed = num4 * num2;
						}
						else
						{
							speed = num5 * Mathf.Sign(speed);
						}
					}
				}
				else if (speed != 0f && ((!controller.collisions.down && Mathf.Abs(speed) > MAX_SPEED_WALK) || (controller.collisions.down && Mathf.Abs(speed) > 0f)))
				{
					float num6 = DECELERATION_RATE;
					if (onIce)
					{
						num6 = ICE_ACCELERATION_RATE;
						if (!skidding && !crouching)
						{
							animator.Play("Walk", 0, 0.75f);
						}
					}
					float num7 = Mathf.Abs(speed) - num6;
					if (num7 <= 0f)
					{
						speed = 0f;
						if (skidding)
						{
							facingRight = !facingRight;
							skidding = false;
						}
					}
					else
					{
						speed = num7 * Mathf.Sign(speed);
					}
				}
			}
			else
			{
				thrownTimer += Time.fixedDeltaTime;
				if (controller.collisions.down)
				{
					MonoBehaviour.print("escape thrown");
					thrown = false;
					thrownTimer = 0f;
				}
			}
			if (skidding && !controller.collisions.down)
			{
				skidding = false;
				facingRight = !facingRight;
			}
			if (aud.loop && !skidding)
			{
				StopLoopingSFX();
			}
			if (holdingObject)
			{
				if (UTInput.GetButton("X") && !pickupButtonHolding)
				{
					ThrowHeldObject();
				}
				else if (!UTInput.GetButton("X") && pickupButtonHolding)
				{
					pickupButtonHolding = false;
				}
			}
			yVelocity += gravity;
			velocity.x = speed;
			velocity.y = ((hitCeiling && yVelocity > 0f) ? 0f : yVelocity);
			sprite.flipX = !facingRight;
			if (holdingObject && skidding)
			{
				sprite.flipX = !sprite.flipX;
			}
			if (doingChargedJump && !holdingObject)
			{
				sprite.flipX = false;
			}
			animator.SetBool("Grounded", controller.collisions.down);
			animator.SetBool("IsMoving", speed != 0f);
			animator.SetBool("Skidding", skidding);
			animator.SetBool("Crouched", crouching);
			animator.SetBool("ChargeJump", doingChargedJump);
			if (skidding && !aud.isPlaying && !aud.loop && controller.collisions.down)
			{
				PlaySFX("mariobros/sounds/snd_player_skid", 1f, true);
			}
			animSpeed = Mathf.Abs(speed) * 20f;
			if (animSpeed > MAX_SPEED_WALK && button)
			{
				animSpeed = 1f + (animSpeed - 1f) * 3f;
			}
			if (button && holdingObject)
			{
				animSpeed *= 2f;
			}
			if (onIce)
			{
				if (flag2)
				{
					animSpeed = ((!button) ? 1 : 2);
				}
				else
				{
					animSpeed = 0f;
				}
			}
			animator.SetFloat("Speed", Mathf.Abs(animSpeed));
			controller.Move(velocity);
		}

		private void HandleStompedMovement()
		{
			if (UTInput.GetAxis("Horizontal") != 0f && !stompHoldAxis && canMove)
			{
				stompHoldAxis = true;
				if (Mathf.Abs(speed) < MAX_SPEED_WALK)
				{
					speed = MAX_SPEED_WALK * UTInput.GetAxis("Horizontal");
				}
				facingRight = !facingRight;
				sprite.flipX = !facingRight;
			}
			else if (UTInput.GetAxis("Horizontal") == 0f && stompHoldAxis)
			{
				stompHoldAxis = false;
			}
			if (controller.collisions.down)
			{
				float num = DECELERATION_RATE;
				if (onIce)
				{
					num = ICE_ACCELERATION_RATE;
				}
				float num2 = Mathf.Abs(speed) - num;
				if (num2 < 0f)
				{
					speed = 0f;
				}
				else
				{
					speed = num2 * Mathf.Sign(speed);
				}
			}
			if (!UTInput.GetButton("Z") && jumpButtonHolding)
			{
				jumpButtonHolding = false;
				if (yVelocity > MIN_JUMP_VELOCITY)
				{
					yVelocity = MIN_JUMP_VELOCITY;
				}
			}
			if (UTInput.GetButton("Z") && !jumpButtonHolding && controller.collisions.down && canMove)
			{
				DoAction();
			}
			yVelocity += gravity;
			velocity.x = speed;
			velocity.y = ((hitCeiling && yVelocity > 0f) ? 0f : yVelocity);
			controller.Move(velocity);
		}

		private void HandleDieMovement()
		{
			if (lives < 0)
			{
				return;
			}
			dieTimer += Time.fixedDeltaTime;
			if (!dieFall && dieTimer >= 13f / 15f)
			{
				sprite.flipX = false;
				dieFall = true;
				PlaySFX("mariobros/sounds/snd_player_die");
				animator.Play("Die");
				velocity = new Vector3(0f, MIN_JUMP_VELOCITY * 0.8f);
				if (playVoices)
				{
					PlayVoice("die");
				}
			}
			if (dieFall)
			{
				if (base.transform.position.y > -6f)
				{
					base.transform.position += velocity;
					velocity.y += gravity * 0.6f;
				}
				if (!splashed && base.transform.position.y <= -6f)
				{
					splashed = true;
					Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/Splash"), new Vector3(base.transform.position.x, -4.6666665f), Quaternion.identity);
				}
			}
			if (dieTimer >= (battlemode ? 8f : 4.1666665f))
			{
				lives--;
				if (lives >= 0)
				{
					Revive();
				}
				else if ((bool)Object.FindObjectOfType<MarioBrosManager>())
				{
					Object.FindObjectOfType<MarioBrosManager>().GameOver();
				}
			}
		}

		public void HandleReviveMovement()
		{
			if (!reviving)
			{
				return;
			}
			reviveTimer += Time.fixedDeltaTime;
			base.transform.position = new Vector3(base.transform.position.x, Mathf.Lerp(2.7083333f, 0f, reviveTimer / 2.1666667f) + 4f);
			if (reviveTimer >= 2.1666667f)
			{
				if (UTInput.GetButton("Z") && canMove)
				{
					FallOffRevivePlatform(true);
					DoAction();
				}
				else if (UTInput.GetAxis("Horizontal") != 0f && canMove)
				{
					speed = MAX_SPEED_WALK * Mathf.Sign(UTInput.GetAxis("Horizontal"));
					facingRight = speed > 0f;
					FallOffRevivePlatform(true);
				}
				else if (reviveTimer >= 5.3333335f)
				{
					FallOffRevivePlatform(true);
				}
			}
			if (reviving)
			{
				angelPlatform.SetVisual(reviveTimer);
			}
		}

		private void WallDetection()
		{
			if ((!(speed > MAX_SPEED_WALK / 4f) || !controller.collisions.right) && (!(speed < MAX_SPEED_WALK / 4f) || !controller.collisions.left))
			{
				return;
			}
			bool flag = false;
			if (!dead && !invulnerable)
			{
				RaycastHit2D[] horizontalHits = controller.GetHorizontalHits(ref velocity);
				for (int i = 0; i < horizontalHits.Length; i++)
				{
					if ((bool)horizontalHits[i])
					{
						flag = true;
						break;
					}
				}
			}
			if (flag && (!thrown || (thrown && yVelocity <= 0f)))
			{
				speed = 0f;
			}
		}

		private void VerticalDetection()
		{
			if (!controller.collisions.down)
			{
				onIce = false;
			}
			if (yVelocity < 0f && controller.collisions.down)
			{
				bool flag = false;
				bool flag2 = false;
				if (UTInput.GetButton("X") && !pickupButtonHolding)
				{
					flag2 = true;
					pickupButtonHolding = true;
				}
				else if (!UTInput.GetButton("X") && pickupButtonHolding)
				{
					pickupButtonHolding = false;
				}
				RaycastHit2D[] verticalHits = controller.GetVerticalHits(ref velocity);
				for (int i = 0; i < verticalHits.Length; i++)
				{
					RaycastHit2D raycastHit2D = verticalHits[i];
					if (!raycastHit2D)
					{
						continue;
					}
					if (!flag && !dead && !invulnerable && (bool)raycastHit2D.collider.GetComponent<Player>() && !raycastHit2D.collider.GetComponent<Player>().IsDead())
					{
						raycastHit2D.collider.GetComponent<Player>();
						Vector3 vector = (Vector3)raycastHit2D.point - raycastHit2D.transform.position;
						if (vector.y > ((BoxCollider2D)raycastHit2D.collider).size.y / 2f && Mathf.Abs(vector.x) < ((BoxCollider2D)raycastHit2D.collider).size.x / 3f)
						{
							flag = true;
						}
						continue;
					}
					if ((bool)raycastHit2D.collider.GetComponent<PowBlock>() && flag2 && !holdingObject && !stomped)
					{
						Pickup(false, raycastHit2D.collider.GetComponent<PowBlock>().GetPowId(), raycastHit2D.collider.GetComponent<PowBlock>().GetPowLevel());
						raycastHit2D.collider.GetComponent<PowBlock>().VanishPowBlock();
						break;
					}
					if ((bool)raycastHit2D.collider.GetComponent<Platform>())
					{
						onIce = raycastHit2D.collider.GetComponent<Platform>().Slippery();
					}
				}
				if (yVelocity <= 0f && controller.collisions.down)
				{
					yVelocity = 0f;
					hitCeiling = false;
					doingChargedJump = false;
				}
			}
			else
			{
				if (!(yVelocity > 0f) || !controller.collisions.up || !canMove || !controller.collisions.up || hitCeiling)
				{
					return;
				}
				hitCeiling = true;
				RaycastHit2D[] verticalHits = controller.GetVerticalHits(ref velocity);
				for (int i = 0; i < verticalHits.Length; i++)
				{
					RaycastHit2D raycastHit2D2 = verticalHits[i];
					if ((bool)raycastHit2D2)
					{
						if ((bool)raycastHit2D2.collider.GetComponent<Platform>() && !holdingObject)
						{
							raycastHit2D2.collider.GetComponent<Platform>().HitCeiling(this, base.transform.position.x);
							MonoBehaviour.print("BITCH!!!");
							break;
						}
						if ((bool)raycastHit2D2.collider.GetComponent<PowBlock>() && !holdingObject)
						{
							raycastHit2D2.collider.GetComponent<PowBlock>().Hit(0);
							break;
						}
					}
				}
				if (hitCeiling && doingChargedJump)
				{
					yVelocity = 0f;
				}
			}
		}

		private void Pickup(bool isPlayer, int id, int level = 0)
		{
			pickingUp = true;
			pickupTimer = 0f;
			holdingObject = true;
			yVelocity = 0f;
			speed = 0f;
			stomped = false;
			skidding = false;
			heldObject = Object.Instantiate(Resources.Load<GameObject>("mariobros/prefabs/objects/HeldPowBlock"), base.transform.position + new Vector3(0f, -1f / 3f), Quaternion.identity).transform;
			heldObject.GetComponent<HeldPowBlock>().SetLevel(level - 1, id, 0);
			animator.Play("Pickup");
			if (playVoices)
			{
				PlayVoice("pickup_0");
			}
			else
			{
				PlaySFX("mariobros/sounds/snd_player_pickup");
			}
		}

		public void ThrowHeldObject(bool forceThrow = false)
		{
			throwTimer = 0f;
			throwToRight = facingRight;
			kicking = false;
			holdingObject = false;
			heldObject.transform.parent = null;
			heldObject.GetComponent<HeldObject>().Throw(facingRight, new Vector3(speed, yVelocity));
			heldObject = null;
			if (!forceThrow)
			{
				PlaySFX("mariobros/sounds/snd_player_throw");
				if (playVoices)
				{
					PlayVoice("throw");
				}
				throwing = true;
			}
			throwTimer = 0f;
			animator.Play("Idle");
			pickupButtonHolding = true;
		}

		public void Revive(int setLives = -1)
		{
			if (setLives > -1)
			{
				lives = setLives;
				pauselock = true;
			}
			dead = false;
			dieFall = false;
			dieCondition = -1;
			splashed = false;
			velocity = Vector3.zero;
			yVelocity = 0f;
			speed = 0f;
			skidding = false;
			hitCeiling = false;
			stomped = false;
			ResetHealth();
			if (!frozen)
			{
				reviving = true;
				livesBubble.transform.parent.GetComponent<Canvas>().enabled = true;
				livesBubble.UpdateContents(lives, skin, palette);
				animator.SetBool("Reviving", true);
				animator.Play("Revive");
				base.transform.position = new Vector3(GlobalVariables.SPAWN_POS[0].x, 5.4166665f);
				reviveTimer = 0f;
				PlaySFX("mariobros/sounds/snd_player_respawn");
			}
			sprite.material = GlobalVariables.GetPaletteMaterial(skinName, palette);
		}

		private void ResetHealth()
		{
			if (maxHealth > 1)
			{
				if (startSuper)
				{
					health = maxHealth;
				}
				else
				{
					health = 1;
				}
				isBig = health > 1;
			}
			else
			{
				isBig = bigMode;
				health = 1;
			}
		}

		private void FallOffRevivePlatform(bool activateIFrames)
		{
			reviving = false;
			livesBubble.transform.parent.GetComponent<Canvas>().enabled = false;
			animator.SetBool("Reviving", false);
			angelPlatform.Hide();
			controller.EnableCollisions();
			if (activateIFrames && iFramesAfterDeath)
			{
				ActivateIFrames();
			}
		}

		private void ActivateIFrames()
		{
			iTimer = 0f;
			iFlashTimer = 0f;
			invulnerable = true;
			controller.GhostCollisions();
		}

		public void ResetForNextRound()
		{
			animator.SetFloat("Speed", 1f);
			animator.SetBool("Grounded", true);
			animator.SetBool("IsMoving", false);
			animator.SetBool("Skidding", false);
			animator.SetBool("Crouched", false);
			animator.SetBool("ChargeJump", false);
			animator.SetBool("Stomped", false);
			animator.SetBool("Reviving", false);
			ResetCrouchCharge();
			speed = 0f;
			yVelocity = 0f;
			velocity = Vector3.zero;
			skidding = false;
			animSpeed = 0f;
			hitCeiling = false;
			crouching = false;
			doingChargedJump = false;
			stomped = false;
			FallOffRevivePlatform(false);
			invulnerable = false;
			controller.PlayerCollisions();
			if (dead && lives > 0)
			{
				lives--;
				dead = false;
				dieFall = false;
				dieTimer = 0f;
				splashed = false;
				ResetHealth();
			}
			kicking = false;
			multikicks = 0;
			if (!dead)
			{
				base.transform.position = GlobalVariables.SPAWN_POS[0];
				animator.Play("Idle");
			}
			facingRight = base.transform.position.x < 0f;
			sprite.flipX = !facingRight;
			forceMove = false;
			controller.collisions.down = true;
			onIce = false;
			pickingUp = false;
			holdingObject = false;
			if ((bool)heldObject)
			{
				Object.Destroy(heldObject.gameObject);
			}
			throwing = false;
			stepFrames = 0f;
			dieCondition = 0;
			sprite.material = GlobalVariables.GetPaletteMaterial(skinName, palette);
			thrown = false;
		}

		public void DoAction()
		{
			if (stomped)
			{
				yVelocity = 0.05f;
			}
			else
			{
				yVelocity = (crouchCharged ? chargedJump : (jumpVelocity + JumpSpeedIncrease()));
				doingChargedJump = crouchCharged;
				ResetCrouchCharge();
				PlaySFX("mariobros/sounds/snd_player_jump");
				if (doingChargedJump && !holdingObject)
				{
					animator.Play("Charge Jump");
					if (playVoices)
					{
						PlayVoice("chargejump");
					}
				}
				animator.SetBool("ChargeJump", doingChargedJump);
			}
			jumpButtonHolding = true;
		}

		private void ResetCrouchCharge()
		{
			glowFrames = 0;
			crouchCharged = false;
			crouchCharge = 0;
			SetBrightness(1f);
		}

		public void SetBrightness(float brightness)
		{
			sprite.material.SetFloat("_Brightness", brightness);
		}

		public void PlaySFX(string sfx, float pitch = 1f, bool loop = false)
		{
			if (!kicking && !throwing && (!aud.isPlaying || (!aud.clip.name.Contains("damage") && !aud.clip.name.Contains("shrink") && !aud.clip.name.Contains("grow"))))
			{
				aud.clip = Resources.Load<AudioClip>(sfx);
				aud.pitch = pitch;
				aud.loop = loop;
				aud.Play();
			}
		}

		public void PlayVoice(string sfx)
		{
			if (skin == 0 && sfx == "die" && Random.Range(0, 100) == 0)
			{
				sfx = "die_alt";
			}
			AudioClip audioClip = Resources.Load<AudioClip>("mariobros/sounds/voices/snd_" + skinName + "_" + sfx);
			if ((bool)audioClip)
			{
				voice.clip = audioClip;
				voice.Play();
			}
		}

		public void Freeze()
		{
			frozen = true;
			animSpeed = 0f;
			animator.SetFloat("Speed", 0f);
			StopLoopingSFX();
		}

		public void SetMovement(bool canMove)
		{
			this.canMove = canMove;
			if (dummyPlayer)
			{
				this.canMove = false;
			}
			if (stunTime == -1)
			{
				int num = Object.FindObjectsOfType<Player>().Length;
				if (num > 4)
				{
					stunTime = (int)Mathf.Lerp(80f, 25f, (float)(num - 5) / 3f);
				}
				else
				{
					stunTime = 106;
				}
			}
			frozen = false;
		}

		public void SetForceMove(bool forceMove)
		{
			this.forceMove = forceMove;
		}

		private void BumpPlayer(float speed)
		{
			if (!thrown)
			{
				ResetCrouchCharge();
				this.speed = speed;
				velocity.x = speed;
				facingRight = speed > 0f;
				if (controller.collisions.down)
				{
					skidding = true;
				}
				bumpedPlayerThisUpdate = true;
			}
		}

		private void StompPlayer()
		{
			if (reviving)
			{
				if (!(reviveTimer >= 2.1666667f))
				{
					return;
				}
				FallOffRevivePlatform(true);
			}
			if (yVelocity > 0f)
			{
				yVelocity = 0f;
			}
			ResetCrouchCharge();
			animator.SetBool("Stomped", true);
			animator.Play(holdingObject ? "HoldStomped" : "Stomped");
			stomped = true;
			skidding = false;
			StopLoopingSFX();
			stompFrames = 0;
		}

		public void HitFromPlatform()
		{
			if (controller.collisions.down)
			{
				StompPlayer();
				yVelocity = MIN_JUMP_VELOCITY;
			}
		}

		public void HitFromPow()
		{
			if (controller.collisions.down)
			{
				StompPlayer();
				yVelocity = MIN_JUMP_VELOCITY * 0.6f;
			}
		}

		public void Damage(Vector3 damageTrajectory, int condition = -1)
		{
			thrown = false;
			ResetCrouchCharge();
			health--;
			if (health > 0)
			{
				if (health == 1)
				{
					ChangeSize(false);
				}
				else
				{
					PlaySFX("mariobros/sounds/snd_player_damage");
					if (condition == 0 && playVoices)
					{
						PlayVoice("burned");
					}
				}
				if (hitCeiling && damageTrajectory.y < 0f && yVelocity > 0f)
				{
					yVelocity = 0f;
				}
				else if (yVelocity <= 0f && yVelocity + damageTrajectory.y > 0f)
				{
					hitCeiling = false;
				}
				speed += damageTrajectory.x;
				yVelocity += damageTrajectory.y;
				if (condition == 2)
				{
					StompPlayer();
				}
				ActivateIFrames();
			}
			else
			{
				if (condition == 2)
				{
					condition = 1;
				}
				Die(condition);
			}
		}

		public void IncreaseHealth()
		{
			health++;
			if (health > maxHealth)
			{
				health = maxHealth;
			}
			if (!isBig)
			{
				ChangeSize(true);
			}
		}

		public void ChangeSize(bool grow)
		{
			isBig = grow;
			PlaySFX(grow ? "mariobros/sounds/snd_player_grow" : "mariobros/sounds/snd_player_shrink");
			if (!grow && playVoices)
			{
				PlayVoice("shrink");
			}
			changingSize = true;
			changeSizeTimer = 0f;
		}

		public void Die(int condition = -1)
		{
			ResetCrouchCharge();
			kicking = false;
			stomped = false;
			dead = true;
			dieTimer = 0f;
			skidding = false;
			controller.DisableCollisions();
			PlaySFX("mariobros/sounds/snd_player_hit");
			animator.Play("Hit");
			pickingUp = false;
			throwing = false;
			sprite.color = Color.white;
			if ((bool)heldObject)
			{
				holdingObject = false;
				heldObject.transform.parent = null;
				heldObject.GetComponent<HeldObject>().Throw(facingRight, Vector3.zero);
				heldObject = null;
			}
			switch (condition)
			{
			case 0:
				dieCondition = 0;
				if (playVoices)
				{
					PlayVoice("burned");
				}
				break;
			case 1:
				dieCondition = 1;
				break;
			default:
				dieCondition = -1;
				break;
			}
			if (dieCondition >= 0 && dieCondition < diePalettes.Length)
			{
				sprite.material = diePalettes[dieCondition];
			}
		}

		public void KickEnemy(bool kickingRight)
		{
			if (kicking)
			{
				kicking = false;
			}
			if (multikickTimer > 0f)
			{
				multikicks++;
				if (multikicks > 4)
				{
					multikicks = 4;
				}
			}
			else
			{
				multikicks = 0;
			}
			multikickTimer = MULTIKICK_TIME;
			PlaySFX("mariobros/sounds/snd_player_kick_" + multikicks);
			if (playVoices)
			{
				PlayVoice("throw");
			}
			StopLoopingSFX();
			kicking = true;
			throwing = false;
			this.kickingRight = kickingRight;
			kickTimer = 0f;
		}

		public void StopLoopingSFX()
		{
			aud.loop = false;
		}

		public void SetAppearance(bool revealPlayer = true)
		{
			sprite.enabled = true;
			skinName = GlobalVariables.SKIN_FILENAMES[skin];
			sprite.material = GlobalVariables.GetPaletteMaterial(skinName, palette);
			bigSprites = Resources.LoadAll<Sprite>("mariobros/sprites/player/spr_" + skinName + "_big");
			smallSprites = Resources.LoadAll<Sprite>("mariobros/sprites/player/spr_" + skinName + "_small");
			diePalettes[0] = GlobalVariables.GetNamedPaletteMaterial(skinName, "burned", palette);
			diePalettes[1] = GlobalVariables.GetNamedPaletteMaterial(skinName, "frozen", palette);
			facingRight = base.transform.position.x < 0f;
			sprite.flipX = !facingRight;
			playVoices = GlobalVariables.HAS_VOICE[skin];
			FixSprite();
		}

		public void ResizeHitbox()
		{
			int num = int.Parse(sprite.sprite.name.Substring(sprite.sprite.name.LastIndexOf("_") + 1));
			if (collider.enabled)
			{
				Vector2 size = collider.size;
				if (!isBig || num == 5 || num == 15)
				{
					collider.size = HITBOX_SIZE;
				}
				else
				{
					collider.size = new Vector2(HITBOX_SIZE.x, HITBOX_SIZE.y * 1.8f);
				}
				collider.offset = new Vector2(0f, collider.size.y / 2f);
				if (size != collider.size)
				{
					controller.CalculateRaySpacing();
				}
			}
		}

		private void FixSprite()
		{
			int num = int.Parse(sprite.sprite.name.Substring(sprite.sprite.name.LastIndexOf("_") + 1));
			bool flag = isBig;
			if (changingSize)
			{
				changeSizeTimer += Time.deltaTime;
				int num2 = (int)(changeSizeTimer * 60f);
				flag = num2 / 4 % 2 == ((!isBig) ? 1 : 0);
				if (num2 >= 24)
				{
					changingSize = false;
					flag = isBig;
				}
			}
			sprite.sprite = (flag ? bigSprites[num] : smallSprites[num]);
			ResizeHitbox();
			if (dead || reviving)
			{
				kicking = false;
				return;
			}
			if (kicking)
			{
				int num3 = (holdingObject ? 17 : 7);
				sprite.sprite = (flag ? bigSprites[num3] : smallSprites[num3]);
				sprite.flipX = !kickingRight;
				if (!frozen)
				{
					kickTimer += Time.deltaTime;
					if (kickTimer >= 0.35f)
					{
						kicking = false;
					}
				}
			}
			if (throwing)
			{
				sprite.sprite = (flag ? bigSprites[20] : smallSprites[20]);
				sprite.flipX = !throwToRight;
				if (!frozen)
				{
					throwTimer += Time.deltaTime;
					if (throwTimer >= 0.5f)
					{
						throwing = false;
					}
				}
			}
			float y = (float)(flag ? GlobalVariables.SKIN_HEIGHTS_BIG[skin][num] : GlobalVariables.SKIN_HEIGHTS_SMALL[skin][num]) / 24f;
			headItems.localPosition = new Vector3(0f, y, 0f);
			if (invulnerable && !dead)
			{
				iFlashTimer += Time.deltaTime;
				sprite.color = new Color(1f, 1f, 1f, ((int)(iFlashTimer * 60f) / 6 % 2 == 0) ? (2f / 3f) : (1f / 3f));
			}
			else
			{
				sprite.color = Color.white;
			}
		}

		public void AddPoints(int points)
		{
			int[] pOINTS_MILESTONES = POINTS_MILESTONES;
			foreach (int num in pOINTS_MILESTONES)
			{
				if (points + this.points >= num && this.points < num)
				{
					AddLives(1);
				}
			}
			if (this.points < 999990)
			{
				this.points += points;
				if (this.points > 999990)
				{
					this.points = 999990;
				}
			}
		}

		public void ResetPoints()
		{
			points = 0;
		}

		public void AddLives(int lives)
		{
			this.lives += lives;
			Object.FindObjectOfType<GameManager>().PlayGlobalSFX("mariobros/sounds/snd_1up");
		}

		public int GetPoints()
		{
			return points;
		}

		public void SetValues(int skin, int palette)
		{
			this.skin = skin;
			this.palette = palette;
			SetAppearance();
		}

		private float JumpSpeedIncrease()
		{
			float num = Mathf.Abs(velocity.x * 2f) / MAX_SPEED_WALK;
			if (num > 1f)
			{
				num = 1f;
			}
			if (isBig && num > 0.75f)
			{
				num = 0.75f;
			}
			return jumpSpeedIncrease * num;
		}

		public bool IsReviving()
		{
			return reviving;
		}

		public float GetSpeed()
		{
			return speed;
		}

		public int GetSkin()
		{
			return skin;
		}

		public int GetPalette()
		{
			return palette;
		}

		public int GetLives()
		{
			return lives;
		}

		public bool CanInteract()
		{
			return true;
		}

		public bool IsInvincible()
		{
			if (!reviving)
			{
				return invulnerable;
			}
			return true;
		}

		public int GetMultiKicks()
		{
			return multikicks;
		}

		public bool IsDead()
		{
			return dead;
		}

		public bool CurrentlyDying()
		{
			if (dead)
			{
				return lives >= 0;
			}
			return false;
		}

		public bool IsBig()
		{
			return isBig;
		}

		public bool BigMode()
		{
			return bigMode;
		}

		public bool CanBePickedUp()
		{
			if (!holdingObject)
			{
				return !beingHeld;
			}
			return false;
		}

		public bool IsFacingRight()
		{
			return facingRight;
		}

		public float GetAnimSpeed()
		{
			return animSpeed;
		}

		public bool AtMaxHealth()
		{
			return health >= maxHealth;
		}

		public Transform GetHeldObject()
		{
			return heldObject;
		}
	}
}
