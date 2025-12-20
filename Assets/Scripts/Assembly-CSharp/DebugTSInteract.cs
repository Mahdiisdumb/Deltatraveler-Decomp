using UnityEngine;

public class DebugTSInteract : InteractTextBox
{
	public override void DoInteract()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 0)
		{
			talkedToBefore = false;
			Object.FindObjectOfType<GameManager>().SetFlag(94, 1);
		}
		else
		{
			talkedToBefore = true;
			Object.FindObjectOfType<GameManager>().SetFlag(94, 0);
		}
		base.DoInteract();
	}
}
