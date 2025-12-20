public class KidnapGuy : InteractWanderingNPC
{
	private bool playTone;

	protected override void Awake()
	{
		base.Awake();
		if ((int)Util.GameManager().GetFlag(96) == 1 || Util.GameManager().GetPartyMember(3) == 3)
		{
			talkedToBefore = true;
		}
	}

	protected override void Update()
	{
		base.Update();
		if (!txt && playTone)
		{
			Util.GameManager().SetFlag(96, 1);
			Util.GameManager().PlayGlobalSFX("sounds/snd_creepyjingle");
			playTone = false;
		}
	}

	public override void DoInteract()
	{
		base.DoInteract();
		if (!Util.GameManager().PartySlotFilled(3) && (bool)txt)
		{
			playTone = true;
		}
	}
}
