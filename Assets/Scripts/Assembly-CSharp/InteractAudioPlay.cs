using System;
using UnityEngine;

public class InteractAudioPlay : Interactable
{
	public override void DoInteract()
	{
		GetComponent<AudioSource>().Play();
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
