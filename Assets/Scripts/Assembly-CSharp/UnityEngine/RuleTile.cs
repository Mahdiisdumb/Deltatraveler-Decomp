using System;
using System.Collections.Generic;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	public class RuleTile<T> : RuleTile
	{
		public sealed override Type m_NeighborType
		{
			get
			{
				return typeof(T);
			}
		}
	}
	[Serializable]
	[CreateAssetMenu(fileName = "New Rule Tile", menuName = "Tiles/Rule Tile")]
	public class RuleTile : TileBase
	{
		[Serializable]
		public class TilingRule
		{
			public class Neighbor
			{
				public const int DontCare = 0;

				public const int This = 1;

				public const int NotThis = 2;
			}

			public enum Transform
			{
				Fixed = 0,
				Rotated = 1,
				MirrorX = 2,
				MirrorY = 3
			}

			public enum OutputSprite
			{
				Single = 0,
				Random = 1,
				Animation = 2
			}

			public int[] m_Neighbors;

			public Sprite[] m_Sprites;

			public GameObject m_GameObject;

			public float m_AnimationSpeed;

			public float m_PerlinScale;

			public Transform m_RuleTransform;

			public OutputSprite m_Output;

			public Tile.ColliderType m_ColliderType;

			public Transform m_RandomTransform;

			public TilingRule()
			{
				m_Output = OutputSprite.Single;
				m_Neighbors = new int[NeighborCount];
				m_Sprites = new Sprite[1];
				m_GameObject = null;
				m_AnimationSpeed = 1f;
				m_PerlinScale = 0.5f;
				m_ColliderType = Tile.ColliderType.Sprite;
				for (int i = 0; i < m_Neighbors.Length; i++)
				{
					m_Neighbors[i] = 0;
				}
			}
		}

		private static readonly int[,] RotatedOrMirroredIndexes = new int[5, 8]
		{
			{ 2, 4, 7, 1, 6, 0, 3, 5 },
			{ 7, 6, 5, 4, 3, 2, 1, 0 },
			{ 5, 3, 0, 6, 1, 7, 4, 2 },
			{ 2, 1, 0, 4, 3, 7, 6, 5 },
			{ 5, 6, 7, 3, 4, 0, 1, 2 }
		};

		private static readonly int NeighborCount = 8;

		public Sprite m_DefaultSprite;

		public GameObject m_DefaultGameObject;

		public Tile.ColliderType m_DefaultColliderType = Tile.ColliderType.Sprite;

		protected TileBase[] m_CachedNeighboringTiles = new TileBase[NeighborCount];

		private TileBase m_OverrideSelf;

		private Quaternion m_GameObjectQuaternion;

		[HideInInspector]
		public List<TilingRule> m_TilingRules;

		public virtual Type m_NeighborType
		{
			get
			{
				return typeof(TilingRule.Neighbor);
			}
		}

		public virtual int neighborCount
		{
			get
			{
				return NeighborCount;
			}
		}

		public TileBase m_Self
		{
			get
			{
				if (!m_OverrideSelf)
				{
					return this;
				}
				return m_OverrideSelf;
			}
			set
			{
				m_OverrideSelf = value;
			}
		}

		public override bool StartUp(Vector3Int location, ITilemap tilemap, GameObject instantiateedGameObject)
		{
			if (instantiateedGameObject != null)
			{
				instantiateedGameObject.transform.position = location + tilemap.GetComponent<Tilemap>().tileAnchor;
				instantiateedGameObject.transform.rotation = m_GameObjectQuaternion;
			}
			return true;
		}

		public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
		{
			TileBase[] neighboringTiles = null;
			GetMatchingNeighboringTiles(tilemap, position, ref neighboringTiles);
			Matrix4x4 identity = Matrix4x4.identity;
			tileData.sprite = m_DefaultSprite;
			tileData.gameObject = m_DefaultGameObject;
			tileData.colliderType = m_DefaultColliderType;
			tileData.flags = TileFlags.LockTransform;
			tileData.transform = identity;
			foreach (TilingRule tilingRule in m_TilingRules)
			{
				Matrix4x4 transform = identity;
				if (!RuleMatches(tilingRule, ref neighboringTiles, ref transform))
				{
					continue;
				}
				switch (tilingRule.m_Output)
				{
				case TilingRule.OutputSprite.Single:
				case TilingRule.OutputSprite.Animation:
					tileData.sprite = tilingRule.m_Sprites[0];
					break;
				case TilingRule.OutputSprite.Random:
				{
					int num = Mathf.Clamp(Mathf.FloorToInt(GetPerlinValue(position, tilingRule.m_PerlinScale, 100000f) * (float)tilingRule.m_Sprites.Length), 0, tilingRule.m_Sprites.Length - 1);
					tileData.sprite = tilingRule.m_Sprites[num];
					if (tilingRule.m_RandomTransform != TilingRule.Transform.Fixed)
					{
						transform = ApplyRandomTransform(tilingRule.m_RandomTransform, transform, tilingRule.m_PerlinScale, position);
					}
					break;
				}
				}
				tileData.transform = transform;
				tileData.gameObject = tilingRule.m_GameObject;
				tileData.colliderType = tilingRule.m_ColliderType;
				m_GameObjectQuaternion = Quaternion.LookRotation(new Vector3(transform.m02, transform.m12, transform.m22), new Vector3(transform.m01, transform.m11, transform.m21));
				break;
			}
		}

		protected static float GetPerlinValue(Vector3Int position, float scale, float offset)
		{
			return Mathf.PerlinNoise(((float)position.x + offset) * scale, ((float)position.y + offset) * scale);
		}

		public override bool GetTileAnimationData(Vector3Int position, ITilemap tilemap, ref TileAnimationData tileAnimationData)
		{
			TileBase[] neighboringTiles = null;
			Matrix4x4 identity = Matrix4x4.identity;
			foreach (TilingRule tilingRule in m_TilingRules)
			{
				if (tilingRule.m_Output == TilingRule.OutputSprite.Animation)
				{
					Matrix4x4 transform = identity;
					GetMatchingNeighboringTiles(tilemap, position, ref neighboringTiles);
					if (RuleMatches(tilingRule, ref neighboringTiles, ref transform))
					{
						tileAnimationData.animatedSprites = tilingRule.m_Sprites;
						tileAnimationData.animationSpeed = tilingRule.m_AnimationSpeed;
						return true;
					}
				}
			}
			return false;
		}

		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			if (m_TilingRules != null && m_TilingRules.Count > 0)
			{
				for (int i = -1; i <= 1; i++)
				{
					for (int j = -1; j <= 1; j++)
					{
						base.RefreshTile(location + new Vector3Int(j, i, 0), tileMap);
					}
				}
			}
			else
			{
				base.RefreshTile(location, tileMap);
			}
		}

		protected virtual bool RuleMatches(TilingRule rule, ref TileBase[] neighboringTiles, ref Matrix4x4 transform)
		{
			for (int i = 0; i <= ((rule.m_RuleTransform == TilingRule.Transform.Rotated) ? 270 : 0); i += 90)
			{
				if (RuleMatches(rule, ref neighboringTiles, i))
				{
					transform = Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -i), Vector3.one);
					return true;
				}
			}
			if (rule.m_RuleTransform == TilingRule.Transform.MirrorX && RuleMatches(rule, ref neighboringTiles, true, false))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(-1f, 1f, 1f));
				return true;
			}
			if (rule.m_RuleTransform == TilingRule.Transform.MirrorY && RuleMatches(rule, ref neighboringTiles, false, true))
			{
				transform = Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, -1f, 1f));
				return true;
			}
			return false;
		}

		protected virtual Matrix4x4 ApplyRandomTransform(TilingRule.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
		{
			float perlinValue = GetPerlinValue(position, perlinScale, 200000f);
			switch (type)
			{
			case TilingRule.Transform.MirrorX:
				return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(((double)perlinValue < 0.5) ? 1f : (-1f), 1f, 1f));
			case TilingRule.Transform.MirrorY:
				return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, ((double)perlinValue < 0.5) ? 1f : (-1f), 1f));
			case TilingRule.Transform.Rotated:
			{
				int num = Mathf.Clamp(Mathf.FloorToInt(perlinValue * 4f), 0, 3) * 90;
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -num), Vector3.one);
			}
			default:
				return original;
			}
		}

		public virtual bool RuleMatch(int neighbor, TileBase tile)
		{
			switch (neighbor)
			{
			case 1:
				return tile == m_Self;
			case 2:
				return tile != m_Self;
			default:
				return true;
			}
		}

		protected bool RuleMatches(TilingRule rule, ref TileBase[] neighboringTiles, int angle)
		{
			for (int i = 0; i < neighborCount; i++)
			{
				int rotatedIndex = GetRotatedIndex(i, angle);
				TileBase tileBase = neighboringTiles[rotatedIndex];
				if (tileBase is RuleOverrideTile)
				{
					tileBase = (tileBase as RuleOverrideTile).m_RuntimeTile.m_Self;
				}
				if (!RuleMatch(rule.m_Neighbors[i], tileBase))
				{
					return false;
				}
			}
			return true;
		}

		protected bool RuleMatches(TilingRule rule, ref TileBase[] neighboringTiles, bool mirrorX, bool mirrorY)
		{
			for (int i = 0; i < neighborCount; i++)
			{
				int mirroredIndex = GetMirroredIndex(i, mirrorX, mirrorY);
				TileBase tileBase = neighboringTiles[mirroredIndex];
				if (tileBase is RuleOverrideTile)
				{
					tileBase = (tileBase as RuleOverrideTile).m_RuntimeTile.m_Self;
				}
				if (!RuleMatch(rule.m_Neighbors[i], tileBase))
				{
					return false;
				}
			}
			return true;
		}

		protected virtual void GetMatchingNeighboringTiles(ITilemap tilemap, Vector3Int position, ref TileBase[] neighboringTiles)
		{
			if (neighboringTiles != null)
			{
				return;
			}
			if (m_CachedNeighboringTiles == null || m_CachedNeighboringTiles.Length < neighborCount)
			{
				m_CachedNeighboringTiles = new TileBase[neighborCount];
			}
			int num = 0;
			for (int num2 = 1; num2 >= -1; num2--)
			{
				for (int i = -1; i <= 1; i++)
				{
					if (i != 0 || num2 != 0)
					{
						Vector3Int position2 = new Vector3Int(position.x + i, position.y + num2, position.z);
						m_CachedNeighboringTiles[num++] = tilemap.GetTile(position2);
					}
				}
			}
			neighboringTiles = m_CachedNeighboringTiles;
		}

		protected virtual int GetRotatedIndex(int original, int rotation)
		{
			switch (rotation)
			{
			case 0:
				return original;
			case 90:
				return RotatedOrMirroredIndexes[0, original];
			case 180:
				return RotatedOrMirroredIndexes[1, original];
			case 270:
				return RotatedOrMirroredIndexes[2, original];
			default:
				return original;
			}
		}

		protected virtual int GetMirroredIndex(int original, bool mirrorX, bool mirrorY)
		{
			if (mirrorX && mirrorY)
			{
				return RotatedOrMirroredIndexes[1, original];
			}
			if (mirrorX)
			{
				return RotatedOrMirroredIndexes[3, original];
			}
			if (mirrorY)
			{
				return RotatedOrMirroredIndexes[4, original];
			}
			return original;
		}
	}
}
