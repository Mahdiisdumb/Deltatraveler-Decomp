using System.Collections.Generic;
using UnityEngine;

public class WaterRippleHandler : MonoBehaviour
{
	private int frames;

	[SerializeField]
	private Sprite[] sprites;

	private List<Transform> players = new List<Transform>();

	private SpriteRenderer[] sr = new SpriteRenderer[3];

	private bool[] inWater = new bool[3];

	private void Awake()
	{
		players.Add(Object.FindObjectOfType<OverworldPlayer>().transform);
		OverworldPartyMember[] array = Object.FindObjectsOfType<OverworldPartyMember>();
		foreach (OverworldPartyMember overworldPartyMember in array)
		{
			players.Add(overworldPartyMember.transform);
		}
		for (int j = 0; j < 3; j++)
		{
			sr[j] = base.transform.GetChild(j).GetComponent<SpriteRenderer>();
		}
	}

	private void LateUpdate()
	{
		frames++;
		for (int i = 0; i < 3; i++)
		{
			sr[i].sprite = sprites[frames / 5 % 4];
			sr[i].transform.position = players[i].transform.position + new Vector3(0f, -0.76f);
			if ((bool)players[i].GetComponent<OverworldPartyMember>())
			{
				sr[i].transform.position -= players[i].GetComponent<OverworldPartyMember>().GetPositionOffset();
			}
			sr[i].color = new Color(1f, 1f, 1f, inWater[i] ? 1 : 0);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			collision.GetComponent<OverworldPlayer>().EnableStepSounds("sounds/snd_splash");
			inWater[0] = true;
		}
		else if ((bool)collision.GetComponent<OverworldPartyMember>())
		{
			inWater[(collision.transform == players[1].transform) ? 1 : 2] = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			collision.GetComponent<OverworldPlayer>().DisableStepSounds();
			inWater[0] = false;
		}
		else if ((bool)collision.GetComponent<OverworldPartyMember>())
		{
			inWater[(collision.transform == players[1].transform) ? 1 : 2] = false;
		}
	}
}
