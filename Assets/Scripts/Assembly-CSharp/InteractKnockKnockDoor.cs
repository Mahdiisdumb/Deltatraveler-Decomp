public class InteractKnockKnockDoor : InteractTextBox
{
	public override void DoInteract()
	{
		base.DoInteract();
		Util.GameManager().PlayGlobalSFX("sounds/snd_knock");
	}
}
