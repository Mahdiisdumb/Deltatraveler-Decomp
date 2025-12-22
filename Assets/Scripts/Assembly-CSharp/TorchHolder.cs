using UnityEngine;

public class TorchHolder : InteractSelectionBase
{
	[SerializeField]
	private int holderID;

	private bool hasTorch;

	private bool playFunnySound = true;

	private void Start()
	{
		hasTorch = (int)Util.GameManager().GetFlag(179) == holderID && (int)Util.GameManager().GetFlag(178) == 0;
	}

	public override void DoInteract()
	{
		if ((bool)txt || !enabled)
		{
			return;
		}
		if ((!Util.FindObjectOfType<Torch>().IsAttachedToPlayer() && hasTorch) || Util.FindObjectOfType<Torch>().IsAttachedToPlayer())
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			if (!hasTorch)
			{
				txt.CreateBox(new string[1] { "* (Put the torch here?)" }, giveBackControl: false);
			}
			else
			{
				txt.CreateBox(new string[1] { "* (Take the torch?)" }, giveBackControl: false);
			}
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			txt.EnableSelectionAtEnd();
			return;
		}
		txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
		if (Vector3.Distance(Util.OverworldPlayer().transform.position, Util.FindObjectOfType<Torch>().transform.position) >= 7f)
		{
			playFunnySound = false;
			txt.CreateBox(new string[3] { "* (You stuck your hand inside\n  of something.)", "* (You found one of the torch\n  sconces!)", "* (... But there isn't a torch\n  here,^05 so you get nothing.)" }, giveBackControl: true);
		}
		else
		{
			txt.CreateBox(new string[1] { "* (There's nothing here.)" }, giveBackControl: true);
		}
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
	}

	private void LateUpdate()
	{
		if ((bool)txt && txt.GetCurrentStringNum() == 2 && !playFunnySound)
		{
			Util.GameManager().PlayGlobalSFX("sounds/snd_won");
			playFunnySound = true;
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.right)
		{
			Util.GameManager().EnablePlayerMovement();
		}
		else if (!hasTorch)
		{
			Util.GameManager().SetFlag(179, holderID);
			Util.GameManager().SetFlag(178, 0);
			Util.FindObjectOfType<Torch>().AttachToSconce(holderID);
			if (selectActivated)
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[1] { "* (You put the torch inside.)" });
			}
			hasTorch = true;
		}
		else
		{
			Util.GameManager().SetFlag(178, 1);
			Util.FindObjectOfType<Torch>().AttachToPlayer();
			if (selectActivated)
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[1] { "* (You took the torch.)" });
			}
			hasTorch = false;
		}
		selectActivated = false;
	}

	public int GetHolderID()
	{
		return holderID;
	}
}
