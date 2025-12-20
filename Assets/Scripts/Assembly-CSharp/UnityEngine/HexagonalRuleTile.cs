using System;
using UnityEngine.Tilemaps;

namespace UnityEngine
{
	public class HexagonalRuleTile<T> : HexagonalRuleTile
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
	[CreateAssetMenu(fileName = "New Hexagonal Rule Tile", menuName = "Tiles/Hexagonal Rule Tile")]
	public class HexagonalRuleTile : RuleTile
	{
		private static readonly int[,] RotatedOrMirroredIndexes = new int[5, 6]
		{
			{ 3, 2, 1, 0, 5, 4 },
			{ 0, 5, 4, 3, 2, 1 },
			{ 3, 4, 5, 0, 1, 2 },
			{ 0, 5, 4, 3, 2, 1 },
			{ 3, 2, 1, 0, 5, 4 }
		};

		private static readonly Vector3Int[,] PointedTopNeighborOffsets = new Vector3Int[2, 6]
		{
			{
				new Vector3Int(1, 0, 0),
				new Vector3Int(0, -1, 0),
				new Vector3Int(-1, -1, 0),
				new Vector3Int(-1, 0, 0),
				new Vector3Int(-1, 1, 0),
				new Vector3Int(0, 1, 0)
			},
			{
				new Vector3Int(1, 0, 0),
				new Vector3Int(1, -1, 0),
				new Vector3Int(0, -1, 0),
				new Vector3Int(-1, 0, 0),
				new Vector3Int(0, 1, 0),
				new Vector3Int(1, 1, 0)
			}
		};

		private static readonly Vector3Int[,] FlatTopNeighborOffsets = new Vector3Int[2, 6]
		{
			{
				new Vector3Int(1, 0, 0),
				new Vector3Int(0, 1, 0),
				new Vector3Int(-1, 1, 0),
				new Vector3Int(-1, 0, 0),
				new Vector3Int(-1, -1, 0),
				new Vector3Int(0, -1, 0)
			},
			{
				new Vector3Int(1, 0, 0),
				new Vector3Int(1, 1, 0),
				new Vector3Int(0, 1, 0),
				new Vector3Int(-1, 0, 0),
				new Vector3Int(0, -1, 0),
				new Vector3Int(1, -1, 0)
			}
		};

		private static readonly int NeighborCount = 6;

		public bool m_FlatTop;

		public override int neighborCount
		{
			get
			{
				return NeighborCount;
			}
		}

		public override void RefreshTile(Vector3Int location, ITilemap tileMap)
		{
			if (m_TilingRules != null && m_TilingRules.Count > 0)
			{
				for (int i = 0; i < neighborCount; i++)
				{
					base.RefreshTile(location + GetOffsetPosition(location, i), tileMap);
				}
			}
			base.RefreshTile(location, tileMap);
		}

		protected override bool RuleMatches(TilingRule rule, ref TileBase[] neighboringTiles, ref Matrix4x4 transform)
		{
			for (int i = 0; i <= ((rule.m_RuleTransform == TilingRule.Transform.Rotated) ? 300 : 0); i += 60)
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

		protected override Matrix4x4 ApplyRandomTransform(TilingRule.Transform type, Matrix4x4 original, float perlinScale, Vector3Int position)
		{
			float perlinValue = RuleTile.GetPerlinValue(position, perlinScale, 200000f);
			switch (type)
			{
			case TilingRule.Transform.MirrorX:
				return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(((double)perlinValue < 0.5) ? 1f : (-1f), 1f, 1f));
			case TilingRule.Transform.MirrorY:
				return original * Matrix4x4.TRS(Vector3.zero, Quaternion.identity, new Vector3(1f, ((double)perlinValue < 0.5) ? 1f : (-1f), 1f));
			case TilingRule.Transform.Rotated:
			{
				int num = Mathf.Clamp(Mathf.FloorToInt(perlinValue * (float)neighborCount), 0, neighborCount - 1) * (360 / neighborCount);
				return Matrix4x4.TRS(Vector3.zero, Quaternion.Euler(0f, 0f, -num), Vector3.one);
			}
			default:
				return original;
			}
		}

		protected override void GetMatchingNeighboringTiles(ITilemap tilemap, Vector3Int position, ref TileBase[] neighboringTiles)
		{
			if (neighboringTiles == null)
			{
				if (m_CachedNeighboringTiles == null || m_CachedNeighboringTiles.Length < neighborCount)
				{
					m_CachedNeighboringTiles = new TileBase[neighborCount];
				}
				for (int i = 0; i < neighborCount; i++)
				{
					Vector3Int position2 = position + GetOffsetPosition(position, i);
					m_CachedNeighboringTiles[i] = tilemap.GetTile(position2);
				}
				neighboringTiles = m_CachedNeighboringTiles;
			}
		}

		protected override int GetRotatedIndex(int original, int rotation)
		{
			return (original + rotation / 60) % neighborCount;
		}

		protected override int GetMirroredIndex(int original, bool mirrorX, bool mirrorY)
		{
			if (mirrorX && mirrorY)
			{
				return RotatedOrMirroredIndexes[2, original];
			}
			if (mirrorX)
			{
				return RotatedOrMirroredIndexes[m_FlatTop ? 3 : 0, original];
			}
			if (mirrorY)
			{
				return RotatedOrMirroredIndexes[(!m_FlatTop) ? 1 : 4, original];
			}
			return original;
		}

		private Vector3Int GetOffsetPosition(Vector3Int location, int direction)
		{
			int num = location.y & 1;
			if (!m_FlatTop)
			{
				return PointedTopNeighborOffsets[num, direction];
			}
			return FlatTopNeighborOffsets[num, direction];
		}
	}
}
