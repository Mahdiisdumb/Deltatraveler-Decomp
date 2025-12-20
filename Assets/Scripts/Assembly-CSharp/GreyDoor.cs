using System;
using UnityEngine;

public class GreyDoor : Interactable
{
	[SerializeField]
	private int newScene = 2;

	[SerializeField]
	private Vector2 newPos = Vector2.zero;

	[SerializeField]
	private bool vertical = true;

	[SerializeField]
	private bool downOrLeft = true;

	private bool activated;

	private int frames;

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		if (frames == 30)
		{
			UnityEngine.Object.FindObjectOfType<Fade>().FadeOut(10);
		}
		else if (frames >= 31 && !UnityEngine.Object.FindObjectOfType<Fade>().IsPlaying())
		{
			UnityEngine.Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			Vector2 dir = (vertical ? Vector2.up : Vector2.right);
			if (downOrLeft)
			{
				dir *= -1f;
			}
			UnityEngine.Object.FindObjectOfType<GameManager>().LoadArea(newScene, true, newPos, dir);
			activated = false;
		}
	}

	public override void DoInteract()
	{
		Util.GameManager().DisablePlayerMovement(true);
		GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/spr_grey_door_open");
		GetComponent<AudioSource>().Play();
		activated = true;
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
