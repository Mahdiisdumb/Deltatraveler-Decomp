using System;
using UnityEngine;

public class MovableMound : Interactable
{
	private void Awake()
	{
		if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(14) == 1)
		{
			base.transform.parent.position = new Vector2(3.584f, -0.53f);
			base.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = 2;
		}
	}

	public override void DoInteract()
	{
		if ((int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(14) != 1)
		{
			base.transform.parent.GetComponent<SpriteRenderer>().sortingOrder = 2;
			CutsceneHandler.GetCutscene(7).StartCutscene();
		}
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
