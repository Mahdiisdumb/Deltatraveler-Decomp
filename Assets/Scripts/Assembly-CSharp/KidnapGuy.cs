using UnityEngine;

public class KidnapGuy : InteractWanderingNPC
{
	private bool playTone;

	protected override void Awake()
	{
		base.Awake();
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(96) == 1 || Object.FindObjectOfType<GameManager>().GetMiniPartyMember() == 1)
		{
			talkedToBefore = true;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!txt && playTone)
		{
			Object.FindObjectOfType<GameManager>().SetFlag(96, 1);
			Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_creepyjingle");
			playTone = false;
		}
	}

	public override void DoInteract()
	{
		base.DoInteract();
		if (Object.FindObjectOfType<GameManager>().GetMiniPartyMember() == 0 && (bool)txt)
		{
			playTone = true;
		}
	}
}
