using System;
using UnityEngine;

namespace MarioBrosMayhem
{
	public class CharacterController2D : MonoBehaviour
	{
		private struct RaycastOrigins
		{
			public Vector2 topLeft;

			public Vector2 topRight;

			public Vector2 bottomLeft;

			public Vector2 bottomRight;
		}

		public struct CollisionInfo
		{
			public bool up;

			public bool down;

			public bool left;

			public bool right;

			public bool climbingSlope;

			public bool descendingSlope;

			public float slopeAngle;

			public float slopeAngleOld;

			public Vector3 velocityOld;

			public void Reset()
			{
				up = (down = false);
				left = (right = false);
				climbingSlope = false;
				descendingSlope = false;
				slopeAngleOld = slopeAngle;
				slopeAngle = 0f;
			}
		}

		public LayerMask collisionMask;

		private LayerMask bottomCollisionMask;

		private LayerMask defaultCollisionMask;

		private LayerMask defaultBottomCollisionMask;

		private const float skinWidth = 0.015f;

		public int horizontalRayCount = 4;

		public int verticalRayCount = 4;

		private float maxClimbAngle = 75f;

		private float maxDescendAngle = 75f;

		private float horizontalRaySpacing;

		private float verticalRaySpacing;

		private BoxCollider2D collider;

		private RaycastOrigins raycastOrigins;

		public CollisionInfo collisions;

		private void Awake()
		{
			collider = GetComponent<BoxCollider2D>();
			defaultCollisionMask = -65601;
			defaultBottomCollisionMask = defaultCollisionMask;
			if ((bool)GetComponent<Enemy>() || (bool)GetComponent<Coin>() || (bool)GetComponent<Freezie>() || (bool)GetComponent<HeldPowBlock>())
			{
				defaultCollisionMask = -65609;
				defaultBottomCollisionMask = defaultCollisionMask;
				if ((bool)GetComponent<HeldPowBlock>())
				{
					defaultCollisionMask = 0;
				}
			}
			else if ((bool)GetComponent<Fireball>())
			{
				defaultCollisionMask = 64;
				defaultBottomCollisionMask = defaultCollisionMask;
			}
			EnableCollisions();
			CalculateRaySpacing();
			collisions.Reset();
		}

		public void Move(Vector3 velocity)
		{
			UpdateRaycastOrigins();
			collisions.Reset();
			collisions.velocityOld = velocity;
			if (velocity.y < 0f)
			{
				DescendSlope(ref velocity);
			}
			if (velocity.x != 0f)
			{
				HorizontalCollisions(ref velocity);
			}
			if (velocity.y != 0f)
			{
				VerticalCollisions(ref velocity);
			}
			base.transform.Translate(velocity);
		}

		private void HorizontalCollisions(ref Vector3 velocity)
		{
			float num = Mathf.Sign(velocity.x);
			float num2 = Mathf.Abs(velocity.x) + 0.015f;
			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 vector = ((num == -1f) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight) + Vector2.up * (horizontalRaySpacing * (float)i);
				RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.right * num, num2, collisionMask);
				Debug.DrawRay(vector, Vector2.right * num * num2, Color.red);
				if (!raycastHit2D)
				{
					continue;
				}
				float num3 = Vector2.Angle(raycastHit2D.normal, Vector2.up);
				if (i == 0 && num3 <= maxClimbAngle)
				{
					if (collisions.descendingSlope)
					{
						collisions.descendingSlope = false;
						velocity = collisions.velocityOld;
					}
					float num4 = 0f;
					if (num3 != collisions.slopeAngleOld)
					{
						num4 = raycastHit2D.distance - 0.015f;
						velocity.x -= num4 * num;
					}
					ClimbSlope(ref velocity, num3);
					velocity.x += num4 * num;
					float num5 = velocity.x / collisions.velocityOld.x;
					velocity.x /= num5;
					velocity.y /= num5;
				}
				if (!collisions.climbingSlope || num3 > maxClimbAngle)
				{
					velocity.x = (raycastHit2D.distance - 0.015f) * num;
					num2 = raycastHit2D.distance;
					if (collisions.climbingSlope)
					{
						velocity.y = Mathf.Tan(collisions.slopeAngle * ((float)Math.PI / 180f)) * Mathf.Abs(velocity.x);
					}
					collisions.left = num == -1f;
					collisions.right = num == 1f;
				}
			}
		}

		private void VerticalCollisions(ref Vector3 velocity)
		{
			float num = Mathf.Sign(velocity.y);
			float num2 = Mathf.Abs(velocity.y) + 0.015f;
			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 vector = ((num == -1f) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft) + Vector2.right * (verticalRaySpacing * (float)i + velocity.x);
				RaycastHit2D raycastHit2D = Physics2D.Raycast(vector, Vector2.up * num, num2, (num == -1f) ? bottomCollisionMask : collisionMask);
				Debug.DrawRay(vector, Vector2.up * num * num2, Color.red);
				if ((bool)raycastHit2D)
				{
					velocity.y = (raycastHit2D.distance - 0.015f) * num;
					num2 = raycastHit2D.distance;
					if (collisions.climbingSlope)
					{
						velocity.x = velocity.y / Mathf.Tan(collisions.slopeAngle * ((float)Math.PI / 180f)) * Mathf.Abs(velocity.x);
						float num3 = velocity.x / collisions.velocityOld.x;
						velocity.x /= num3;
						velocity.y /= num3;
					}
					collisions.down = num == -1f;
					collisions.up = num == 1f;
				}
			}
			if (!collisions.climbingSlope)
			{
				return;
			}
			float num4 = Mathf.Sign(velocity.x);
			num2 = Mathf.Abs(velocity.x) + 0.015f;
			RaycastHit2D raycastHit2D2 = Physics2D.Raycast((num4 == -1f) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight, Vector2.right * num4, num2, collisionMask);
			if ((bool)raycastHit2D2)
			{
				float num5 = Vector2.Angle(raycastHit2D2.normal, Vector2.up);
				if (num5 != collisions.slopeAngle)
				{
					velocity.x = (raycastHit2D2.distance - 0.015f) * num4;
					collisions.slopeAngle = num5;
				}
			}
		}

		public RaycastHit2D[] GetHorizontalHits(ref Vector3 velocity)
		{
			float num = Mathf.Sign(velocity.x);
			float distance = Mathf.Abs(velocity.x) + 0.015f;
			RaycastHit2D[] array = new RaycastHit2D[horizontalRayCount];
			for (int i = 0; i < horizontalRayCount; i++)
			{
				Vector2 origin = ((num == -1f) ? raycastOrigins.bottomLeft : raycastOrigins.bottomRight);
				origin += Vector2.up * (horizontalRaySpacing * (float)i);
				array[i] = Physics2D.Raycast(origin, Vector2.right * num, distance, collisionMask);
			}
			return array;
		}

		public RaycastHit2D[] GetVerticalHits(ref Vector3 velocity)
		{
			float num = Mathf.Sign(velocity.y);
			float distance = Mathf.Abs(velocity.y) + 0.015f;
			RaycastHit2D[] array = new RaycastHit2D[verticalRayCount];
			for (int i = 0; i < verticalRayCount; i++)
			{
				Vector2 origin = ((num == -1f) ? raycastOrigins.bottomLeft : raycastOrigins.topLeft);
				origin += Vector2.right * (verticalRaySpacing * (float)i + velocity.x);
				array[i] = Physics2D.Raycast(origin, Vector2.up * num, distance, (num == -1f) ? bottomCollisionMask : collisionMask);
			}
			return array;
		}

		private void ClimbSlope(ref Vector3 velocity, float slopeAngle)
		{
			float num = Mathf.Abs(velocity.x);
			float num2 = Mathf.Sin(slopeAngle * ((float)Math.PI / 180f)) * num;
			if (velocity.y <= num2)
			{
				velocity.y = Mathf.Sin(slopeAngle * ((float)Math.PI / 180f)) * num;
				velocity.x = Mathf.Cos(slopeAngle * ((float)Math.PI / 180f)) * num * Mathf.Sign(velocity.x);
				collisions.down = true;
				collisions.climbingSlope = true;
				collisions.slopeAngle = slopeAngle;
			}
		}

		private void DescendSlope(ref Vector3 velocity)
		{
			float num = Mathf.Sign(velocity.x);
			RaycastHit2D raycastHit2D = Physics2D.Raycast((num == -1f) ? raycastOrigins.bottomRight : raycastOrigins.bottomLeft, -Vector2.up, float.PositiveInfinity, collisionMask);
			if ((bool)raycastHit2D)
			{
				float num2 = Vector2.Angle(raycastHit2D.normal, Vector2.up);
				if (num2 != 0f && num2 <= maxDescendAngle && Mathf.Sign(raycastHit2D.normal.x) == num && raycastHit2D.distance - 0.015f <= Mathf.Tan(num2 * ((float)Math.PI / 180f)) * Mathf.Abs(velocity.x))
				{
					float num3 = Mathf.Abs(velocity.x);
					float num4 = Mathf.Sin(num2 * ((float)Math.PI / 180f)) * num3;
					velocity.x = Mathf.Cos(num2 * ((float)Math.PI / 180f)) * num3 * Mathf.Sign(velocity.x);
					velocity.y -= num4;
					float num5 = velocity.x / collisions.velocityOld.x;
					velocity.x /= num5;
					velocity.y /= num5;
					collisions.slopeAngle = num2;
					collisions.descendingSlope = true;
					collisions.down = true;
				}
			}
		}

		private void UpdateRaycastOrigins()
		{
			Bounds bounds = collider.bounds;
			bounds.Expand(-0.03f);
			raycastOrigins.bottomLeft = new Vector2(bounds.min.x, bounds.min.y);
			raycastOrigins.bottomRight = new Vector2(bounds.max.x, bounds.min.y);
			raycastOrigins.topLeft = new Vector2(bounds.min.x, bounds.max.y);
			raycastOrigins.topRight = new Vector2(bounds.max.x, bounds.max.y);
		}

		public void CalculateRaySpacing()
		{
			Bounds bounds = collider.bounds;
			bounds.Expand(-0.03f);
			horizontalRayCount = Mathf.Clamp(horizontalRayCount, 2, int.MaxValue);
			verticalRayCount = Mathf.Clamp(verticalRayCount, 2, int.MaxValue);
			horizontalRaySpacing = bounds.size.y / (float)(horizontalRayCount - 1);
			verticalRaySpacing = bounds.size.x / (float)(verticalRayCount - 1);
		}

		public void EnableCollisions()
		{
			collisionMask = defaultCollisionMask;
			bottomCollisionMask = defaultBottomCollisionMask;
			collider.enabled = true;
		}

		public void DisableCollisions()
		{
			collisionMask = 0;
			bottomCollisionMask = 0;
			collider.enabled = false;
		}

		public void GhostCollisions()
		{
			defaultCollisionMask = -65609;
			defaultBottomCollisionMask = -65609;
			if (collider.enabled)
			{
				EnableCollisions();
			}
			else
			{
				DisableCollisions();
			}
		}

		public void PlayerCollisions()
		{
			defaultCollisionMask = -65601;
			defaultBottomCollisionMask = defaultCollisionMask;
			if (collider.enabled)
			{
				EnableCollisions();
			}
			else
			{
				DisableCollisions();
			}
		}
	}
}
