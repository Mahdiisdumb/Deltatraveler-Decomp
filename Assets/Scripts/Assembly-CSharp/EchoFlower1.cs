using UnityEngine;

public class EchoFlower1 : InteractTextBox
{
	public override void DoInteract()
	{
		if (talkedToBefore && Util.GameManager().GetFlagInt(304) == 1)
		{
			Util.GameManager().SetFlag(304, 2);
		}
		else if (Util.GameManager().GetFlagInt(304) == 2)
		{
			txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[2] { "* Well it's a good thing we\n  don't live near a swamp,^05 faha!", "* (It's still so...^05\n  creepy.)" }, new string[2] { "snd_txtnoe", "snd_txtsus" }, new int[1], true, new string[2] { "", "su_side_sweat" });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			return;
		}
		base.DoInteract();
	}
}
