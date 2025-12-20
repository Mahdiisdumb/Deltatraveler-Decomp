using System;
using UnityEngine;

public class InteractBattle : Interactable
{
	[SerializeField]
	private int battleID;

	private void Awake()
	{
		if ((int)EndBattleHandler.GetFlagFromId(battleID) == 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void DoInteract()
	{
		if ((int)EndBattleHandler.GetFlagFromId(battleID) == 0)
		{
			UnityEngine.Object.FindObjectOfType<OverworldPlayer>().InitiateBattle(2);
		}
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		throw new NotImplementedException();
	}
}
