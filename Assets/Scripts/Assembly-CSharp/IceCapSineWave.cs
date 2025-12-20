using System;
using UnityEngine;

public class IceCapSineWave : MonoBehaviour
{
	private float velocity = 3f;

	private Transform top;

	private Transform bot;

	private float verticalDistance;

	private bool activated;

	private void Awake()
	{
		top = base.transform.GetChild(0);
		bot = base.transform.GetChild(1);
	}

	private void Update()
	{
		if (activated)
		{
			top.localPosition = Vector3.Lerp(top.localPosition, new Vector3(0f, verticalDistance / 2f / 48f, 0f), 0.05f);
			bot.localPosition = Vector3.Lerp(bot.localPosition, new Vector3(0f, (0f - verticalDistance) / 2f / 48f, 0f), 0.05f);
			base.transform.position += new Vector3(velocity / 48f, 0f);
		}
	}

	public void Activate(float centerY, float verticalDistance, float horizontalDistance, int maxHeight, int lowToHighCount, float velocity = 3f)
	{
		this.velocity = velocity;
		this.verticalDistance = verticalDistance + 200f;
		activated = true;
		base.transform.position = new Vector3(1.5f, centerY);
		for (int i = 0; i < top.childCount; i++)
		{
			float num = Mathf.Sin(180f * ((float)i / (float)lowToHighCount) * ((float)Math.PI / 180f));
			top.GetChild(i).localScale = new Vector3(1f, 0f - (100f + (num + 1f) / 2f * (float)maxHeight), 1f);
			bot.GetChild(i).localScale = new Vector3(1f, 100f + (0f - num + 1f) / 2f * (float)maxHeight, 1f);
			top.GetChild(i).localPosition = new Vector3((0f - horizontalDistance) * (float)i / 48f, 0f, 0f);
			bot.GetChild(i).localPosition = new Vector3((0f - horizontalDistance) * (float)i / 48f, 0f, 0f);
		}
	}
}
