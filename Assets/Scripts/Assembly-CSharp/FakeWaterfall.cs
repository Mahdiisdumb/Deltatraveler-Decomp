using System.Collections.Generic;
using UnityEngine;

public class FakeWaterfall : MonoBehaviour
{
	private int frames;

	private int raiseFrames;

	[SerializeField]
	private Sprite[] sprites;

	private List<Transform> players = new List<Transform>();

	private SpriteRenderer sr;

	private void Awake()
	{
		players.Add(Object.FindObjectOfType<OverworldPlayer>().transform);
		OverworldPartyMember[] array = Object.FindObjectsOfType<OverworldPartyMember>();
		foreach (OverworldPartyMember overworldPartyMember in array)
		{
			players.Add(overworldPartyMember.transform);
		}
		sr = GetComponentInChildren<SpriteRenderer>();
	}

	private void Update()
	{
		frames++;
		bool flag = false;
		foreach (Transform player in players)
		{
			Vector3 b = player.position + new Vector3(0f, -0.76f);
			if (!flag)
			{
				flag = Vector3.Distance(base.transform.position, b) <= 0.75f;
			}
		}
		sr.sprite = sprites[frames / 2 % 3];
		if (flag && raiseFrames < 4)
		{
			raiseFrames += 2;
		}
		else if (!flag && raiseFrames > 0)
		{
			raiseFrames--;
		}
		sr.transform.localPosition = new Vector3(0f, Mathf.Lerp(0f, 1.25f, (float)raiseFrames / 4f));
	}
}
