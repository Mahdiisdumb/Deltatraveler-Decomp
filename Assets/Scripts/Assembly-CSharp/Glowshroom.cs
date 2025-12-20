using System;
using UnityEngine;

public class Glowshroom : Interactable
{
	private bool glowing = true;

	[SerializeField]
	private Sprite[] sprites;

	public override void DoInteract()
	{
		glowing = !glowing;
		if (glowing)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = sprites[1];
		}
		GetComponent<AudioSource>().Play();
	}

	public override int GetEventData()
	{
		throw new NotImplementedException();
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		throw new NotImplementedException();
	}
}
