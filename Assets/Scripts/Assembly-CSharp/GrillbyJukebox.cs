using UnityEngine;

public class GrillbyJukebox : InteractSelectionBase
{
	private bool broken = true;

	private void Awake()
	{
		if (Util.GameManager().GetPlayerName() == "SHAYY" && Util.GameManager().GetFlagInt(291) == 0)
		{
			broken = false;
		}
	}

	public override void DoInteract()
	{
		if (!broken)
		{
			base.DoInteract();
			return;
		}
		txt = new GameObject("JukeboxBrokenBitch", typeof(TextBox)).GetComponent<TextBox>();
		txt.CreateBox(new string[1] { "* (The jukebox is broken.)^05\n" + ((Util.GameManager().GetFlagInt(291) == 0) ? "* (It feels like a curse at\n  this point.)" : "* (This time it's your fault.)") });
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			broken = true;
			Util.GameManager().SetFlag(291, 1);
			GetComponent<AudioSource>().Play();
			Util.GameManager().PlayMusic("zoneMusic");
			txt = new GameObject("JukeboxBrokenBitch", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[2] { "* (You select the next song.)^05\n* (It immediately starts playing,^05\n  and then breaks.)", "* (What have you done.)" });
		}
		else
		{
			txt = new GameObject("JukeboxRemainsAlive", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* (You live in your own hell.)" });
		}
	}
}
