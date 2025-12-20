using UnityEngine;

public class InteractKnockKnockDoor : InteractTextBox
{
	public override void DoInteract()
	{
		base.DoInteract();
		Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_knock");
	}
}
