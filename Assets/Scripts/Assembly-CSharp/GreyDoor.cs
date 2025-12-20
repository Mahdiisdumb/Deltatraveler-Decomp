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
			Util.FindObjectOfType<Fade>().FadeOut(10);
		}
		else if (frames >= 31 && !Util.FindObjectOfType<Fade>().IsPlaying())
		{
			Util.GameManager().EnablePlayerMovement();
			Vector2 dir = (vertical ? Vector2.up : Vector2.right);
			if (downOrLeft)
			{
				dir *= -1f;
			}
			Util.GameManager().LoadArea(newScene, fadeIn: true, newPos, dir);
			activated = false;
		}
	}

	public override void DoInteract()
	{
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
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
