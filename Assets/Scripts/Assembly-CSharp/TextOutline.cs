using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;
using UnityEngine.UI;

public class TextOutline : Outline
{
	[Serializable]
	public enum Direction
	{
		None = 0,
		All = -1,
		Up = 1,
		Down = 2,
		Left = 4,
		Right = 8,
		UpLeft = 16,
		UpRight = 32,
		DownLeft = 64,
		DownRight = 128,
		Sides = 15,
		DiagonalLeftRight = 144,
		DiagonalRightLeft = 96
	}

	[SerializeField]
	private float m_Extent = 2f;

	[SerializeField]
	private Direction m_Directions = Direction.All;

	public override void ModifyMesh(VertexHelper vh)
	{
		if (!IsActive())
		{
			return;
		}
		List<UIVertex> list = CollectionPool<List<UIVertex>, UIVertex>.Get();
		vh.GetUIVertexStream(list);
		int num = list.Count * 5;
		if (list.Capacity < num)
		{
			list.Capacity = num;
		}
		int num2 = 0;
		for (int i = 0; i < 8; i++)
		{
			int num3 = 1 << i;
			float x = 0f;
			float y = 0f;
			if (((uint)m_Directions & (uint)num3) == (uint)num3)
			{
				switch (i)
				{
				case 0:
					y = m_Extent;
					break;
				case 1:
					y = 0f - m_Extent;
					break;
				case 2:
					x = 0f - m_Extent;
					break;
				case 3:
					x = m_Extent;
					break;
				case 4:
					y = m_Extent;
					x = 0f - m_Extent;
					break;
				case 5:
					y = m_Extent;
					x = m_Extent;
					break;
				case 6:
					y = 0f - m_Extent;
					x = 0f - m_Extent;
					break;
				case 7:
					y = 0f - m_Extent;
					x = m_Extent;
					break;
				}
				int start = num2;
				num2 = list.Count;
				ApplyShadowZeroAlloc(list, base.effectColor, start, list.Count, x, y);
			}
		}
		vh.Clear();
		vh.AddUIVertexTriangleStream(list);
		CollectionPool<List<UIVertex>, UIVertex>.Release(list);
	}
}
