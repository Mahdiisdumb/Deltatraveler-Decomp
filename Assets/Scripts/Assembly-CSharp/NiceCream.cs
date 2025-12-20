using UnityEngine;

public class NiceCream : InteractSelectionBase
{
	private int takenNiceCreams;

	private bool openedOnce;

	private void Awake()
	{
		takenNiceCreams = (int)Util.GameManager().GetFlag(193);
		openedOnce = (int)Util.GameManager().GetFlag(194) == 1;
		ReplaceText();
	}

	public override void DoInteract()
	{
		if ((bool)txt || !enabled)
		{
			return;
		}
		if (takenNiceCreams >= 4)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(lines, true);
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			return;
		}
		base.DoInteract();
		if (!openedOnce)
		{
			openedOnce = true;
			ReplaceText();
			Util.GameManager().SetFlag(194, 1);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		selectActivated = false;
		if (index == Vector2.left)
		{
			if (Object.FindObjectOfType<GameManager>().NumItemFreeSpace() > 0)
			{
				Object.FindObjectOfType<GameManager>().AddItem(38);
				takenNiceCreams++;
				Object.FindObjectOfType<GameManager>().SetFlag(193, takenNiceCreams);
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[1] { "* (You got the Nice Cream.)" }, true);
			}
			else
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[1] { "* (You're carrying too much.)" }, true);
			}
		}
		else if (index == Vector2.right)
		{
			Util.GameManager().EnablePlayerMovement();
		}
		ReplaceText();
	}

	private void ReplaceText()
	{
		if (takenNiceCreams >= 4)
		{
			lines = new string[1] { "* (There's no more nice cream\n  in the cart.)" };
		}
		else if (openedOnce)
		{
			lines = new string[1] { "* (There's nice cream in the\n  cart.)\n* (Take one?)" };
		}
		else if (!Util.GameManager().SusieInParty())
		{
			lines[2] = "* (You felt uneasy.)";
			lines[4] = "* (Take one?)";
			sounds = new string[1] { "snd_text" };
			portraits = new string[1] { "" };
		}
		else if ((int)Util.GameManager().GetFlag(172) == 1)
		{
			lines[4] = "* (Take one?)";
			sounds[4] = "snd_text";
			portraits[4] = "";
		}
	}
}
