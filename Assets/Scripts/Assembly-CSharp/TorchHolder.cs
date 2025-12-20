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
		if ((!Object.FindObjectOfType<Torch>().IsAttachedToPlayer() && hasTorch) || Object.FindObjectOfType<Torch>().IsAttachedToPlayer())
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			if (!hasTorch)
			{
				txt.CreateBox(new string[1] { "* (Put the torch here?)" }, false);
			}
			else
			{
				txt.CreateBox(new string[1] { "* (Take the torch?)" }, false);
			}
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			txt.EnableSelectionAtEnd();
			return;
		}
		txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
		if (Vector3.Distance(Object.FindObjectOfType<OverworldPlayer>().transform.position, Object.FindObjectOfType<Torch>().transform.position) >= 7f)
		{
			playFunnySound = false;
			txt.CreateBox(new string[3] { "* (You stuck your hand inside\n  of something.)", "* (You found one of the torch\n  sconces!)", "* (... But there isn't a torch\n  here,^05 so you get nothing.)" }, true);
		}
		else
		{
			txt.CreateBox(new string[1] { "* (There's nothing here.)" }, true);
		}
		Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
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
			Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
		}
		else if (!hasTorch)
		{
			Util.GameManager().SetFlag(179, holderID);
			Util.GameManager().SetFlag(178, 0);
			Object.FindObjectOfType<Torch>().AttachToSconce(holderID);
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
			Object.FindObjectOfType<Torch>().AttachToPlayer();
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
