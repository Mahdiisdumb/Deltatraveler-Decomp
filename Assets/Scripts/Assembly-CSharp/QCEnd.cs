using System;
using UnityEngine;

public class QCEnd : Interactable
{
	private bool startedCutscene;

	private void Update()
	{
		if (!startedCutscene)
		{
			GetComponent<Animator>().SetFloat("dirX", 0f - (base.transform.position.x - Util.OverworldPlayer().transform.position.x));
			GetComponent<Animator>().SetFloat("dirY", 0f - (base.transform.position.y - Util.OverworldPlayer().transform.position.y));
		}
	}

	private void LateUpdate()
	{
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f);
	}

	public override void DoInteract()
	{
		if ((int)Util.GameManager().GetFlag(179) == 3 || (int)Util.GameManager().GetFlag(178) == 1)
		{
			startedCutscene = true;
			CutsceneHandler.GetCutscene(79).StartCutscene();
			return;
		}
		txt = new GameObject("InteractQCEnd", typeof(TextBox)).GetComponent<TextBox>();
		txt.CreateBox(new string[3] { "* Hey,^05 y'all!", "* I...^05 don't have my lighter,^05\n  so would y'all be willing\n  to bring the torch over here?", "* It'd be a bit too difficult\n  to build this thing in the\n  dark." }, giveBackControl: true);
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
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
