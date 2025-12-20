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
			Util.GameManager().DisablePlayerMovement(true);
			Util.GameManager().SetPartyMembers(true, true);
			Util.GameManager().SetCheckpoint(89);
			Object.FindObjectOfType<Fade>().FadeOut(15);
		}
		else
		{
			base.DoInteract();
		}
	}

	protected override void Update()
	{
		if (done && !Object.FindObjectOfType<Fade>().IsPlaying())
		{
			Util.GameManager().LoadArea(88, true, new Vector3(4.15f, 1.41f), Vector2.down);
		}
	}
}
