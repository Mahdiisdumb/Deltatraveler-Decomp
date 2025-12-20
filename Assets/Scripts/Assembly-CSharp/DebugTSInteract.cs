public class DebugTSInteract : InteractTextBox
{
	public override void DoInteract()
	{
		if ((int)Util.GameManager().GetFlag(94) == 0)
		{
			talkedToBefore = false;
			Util.GameManager().SetFlag(94, 1);
		}
		else
		{
			talkedToBefore = true;
			Util.GameManager().SetFlag(94, 0);
		}
		base.DoInteract();
	}
}
