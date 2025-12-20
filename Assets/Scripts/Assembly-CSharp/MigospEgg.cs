using UnityEngine;

public class MigospEgg : Interactable
{
	[SerializeField]
	public new bool enabled = true;

	private bool eggSound;

	private bool migospel;

	private void Awake()
	{
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(53) == 0 || WeirdChecker.HasKilled(Object.FindObjectOfType<GameManager>()))
		{
			Object.Destroy(base.gameObject);
			return;
		}
		migospel = (int)Util.GameManager().GetFlag(108) == 1;
		if (migospel)
		{
			GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("overworld/npcs/spr_migospel");
		}
	}

	private void Update()
	{
		if ((bool)txt && txt.GetCurrentStringNum() == (migospel ? 6 : 9) && !eggSound)
		{
			Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_egg");
			eggSound = true;
		}
	}

	public override void DoInteract()
	{
		if ((bool)txt || !enabled)
		{
			return;
		}
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(63) == 0)
		{
			if (Object.FindObjectOfType<GameManager>().NumItemFreeSpace() > 0)
			{
				txt = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
				if (migospel)
				{
					txt.CreateBox(new string[7] { "* Uhh...^05 who the hell\n  are you?", "* I thought I could hide away\n  so that I wouldn't have to\n  keep up this facade.", "* But alas...^05 you have seen what\n  I desperately did not want\n  you to see.", "* I must give you a\n  consolation gift.", "* Please take this object that\n  a strange man gave me.", "* (You got the Egg.)", "* (Why did they just...\n ^10 take it???)" }, new string[7] { "snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus" }, new int[17], true, new string[7] { "su_annoyed", "", "", "", "", "", "su_inquisitive" });
				}
				else
				{
					txt.CreateBox(new string[10] { "* Hiya~", "* WHO THE HELL ARE\n  YOU???", "* Just standing here alone,^05\n  bein' myself...", "* The idea of fighting you\n  two seemed really awkward.", "* So I didn't!", "* But apparently you two\n  turned out to be really\n  nice.", "* Speaking of awkward,^05 I got\n  this really weird egg from\n  someone.", "* You seem like the type of\n  people that would want it.", "* (You got the Egg.)", "* (Why did Kris just...\n ^10 take it???)" }, new string[10] { "snd_text", "snd_txtsus", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_text", "snd_txtsus" }, new int[17], true, new string[10] { "", "su_wtf", "", "", "", "", "", "", "", "su_inquisitive" });
				}
				GameObject.Find("Pillar").GetComponent<InteractTextBox>().ModifyContents(new string[1] { "* (For just a brief moment,^05 you\n  thought you saw a man\n  behind the pillar.)" }, new string[1] { "snd_text" }, new int[1], new string[1] { "" });
				Object.FindObjectOfType<GameManager>().SetFlag(63, 1);
				Object.FindObjectOfType<GameManager>().AddItem(16);
				Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			}
			else
			{
				TextBox component = new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>();
				if ((int)Util.GameManager().GetFlag(108) == 1)
				{
					component.CreateBox(new string[1] { "* I have something that I\n  don't want anymore,^05 but I\n  see your inventory is full." });
				}
				else
				{
					component.CreateBox(new string[2] { "* Hiya~", "* I have something that I\n  don't want anymore,^05 but I\n  see your inventory is full." });
				}
				Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			}
		}
		else
		{
			new GameObject("InteractTextBox", typeof(TextBox)).GetComponent<TextBox>().CreateBox(new string[1] { migospel ? "* Please do not tell anyone." : "* La la~^05\n* Just be yourself~" });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		Debug.LogError("Tried to make decision in simple textbox interactable");
	}
}
