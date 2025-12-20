using System.Collections.Generic;
using UnityEngine;

public class UndyneShadow : MonoBehaviour
{
	private List<Transform> players = new List<Transform>();

	private Transform[] shadow = new Transform[3];

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
			shadow[j] = base.transform.GetChild(j);
		}
	}

	private void LateUpdate()
	{
		for (int i = 0; i < 3; i++)
		{
			shadow[i].transform.position = players[i].transform.position + new Vector3(0f, -0.702f);
			if ((bool)players[i].GetComponent<OverworldPartyMember>())
			{
				shadow[i].position -= players[i].GetComponent<OverworldPartyMember>().GetPositionOffset();
			}
		}
	}
}
