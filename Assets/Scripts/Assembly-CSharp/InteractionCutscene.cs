using System;
using UnityEngine;

public class InteractionCutscene : Interactable
{
	[SerializeField]
	private int flag = -1;

	[SerializeField]
	private int cutsceneId;

	[SerializeField]
	private bool destroyOnInteract;

	private bool started;

	private void Awake()
	{
		if (flag > -1 && (int)UnityEngine.Object.FindObjectOfType<GameManager>().GetFlag(flag) >= 1)
		{
			UnityEngine.Object.Destroy(base.gameObject);
		}
	}

	public override void DoInteract()
	{
		if (!started)
		{
			CutsceneHandler.GetCutscene(cutsceneId).StartCutscene();
			started = true;
			if (destroyOnInteract)
			{
				UnityEngine.Object.Destroy(base.gameObject);
			}
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
