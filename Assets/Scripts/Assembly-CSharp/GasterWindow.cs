using System;
using UnityEngine;

public class GasterWindow : MonoBehaviour
{
	private Color color = new Color(1f, 0f, 0f, 0.5f);

	private const float COLOR_CHANGE_RATE = 1f / 60f;

	private const float SIZE_CHANGE_RATE = 0.02f;

	private int focus;

	private void Update()
	{
		int num = focus - 1;
		int num2 = focus + 1;
		if (num < 0)
		{
			num = 2;
		}
		if (num2 > 2)
		{
			num2 = 0;
		}
		if (color[num] > 0f)
		{
			color[num] -= 1f / 60f;
		}
		else if (color[num2] < 1f)
		{
			color[num2] += 1f / 60f;
		}
		else
		{
			focus = (focus + 1) % 3;
		}
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = color;
		}
		for (int j = 0; j < 2; j++)
		{
			Predicate<float> predicate = (float x) => x <= 8f;
			if (j == 0)
			{
				predicate = (float x) => x >= 20f;
			}
			float num3 = ((j != 1) ? 1 : (-1));
			SpriteRenderer component = base.transform.GetChild(j).GetComponent<SpriteRenderer>();
			component.size += new Vector2(0.02f, -0.02f) * num3;
			if (predicate(component.size.x))
			{
				component.size -= new Vector2(10f, -10f) * num3;
			}
		}
	}
}
