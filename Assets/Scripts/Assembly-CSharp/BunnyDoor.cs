using UnityEngine;

public class BunnyDoor : InteractTextBox
{
	private bool done;

	public override void DoInteract()
	{
		if ((int)Util.GameManager().GetSessionFlag(6) == 1)
		{
			GetComponent<BoxCollider2D>().enabled = false;
			CutsceneHandler.GetCutscene(74).StartCutscene();
		}
		else if ((int)Util.GameManager().GetFlag(204) == 1)
		{
			done = true;
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			Util.GameManager().SetPartyMembers(susie: true, noelle: true);
			Util.GameManager().SetCheckpoint(89);
			Util.FindObjectOfType<Fade>().FadeOut(15);
		}
		else
		{
			base.DoInteract();
		}
	}

	protected override void Update()
	{
		if (done && !Util.FindObjectOfType<Fade>().IsPlaying())
		{
			Util.GameManager().LoadArea(88, fadeIn: true, new Vector3(4.15f, 1.41f), Vector2.down);
		}
	}
}
