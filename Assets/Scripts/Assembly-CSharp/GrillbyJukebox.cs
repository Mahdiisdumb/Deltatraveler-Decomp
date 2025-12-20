using UnityEngine;

public class GrillbyJukebox : InteractSelectionBase
{
	private bool broken = true;

	public override void DoInteract()
	{
		if (!broken)
		{
			base.DoInteract();
			return;
		}
		txt = new GameObject("JukeboxBrokenBitch", typeof(TextBox)).GetComponent<TextBox>();
		txt.CreateBox(new string[1] { "* (The jukebox is broken.)^05\n" + ((Util.GameManager().GetFlagInt(291) == 0) ? "* (It feels like a curse at\n  this point.)" : "* (This time it's your fault.)") });
		Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
	}
}
