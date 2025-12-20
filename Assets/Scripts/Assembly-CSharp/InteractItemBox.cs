using UnityEngine;

public class InteractItemBox : InteractSelectionBase
{
	[SerializeField]
	private int flag = -1;

	[SerializeField]
	private int itemID;

	[SerializeField]
	protected string[] purchaseLines = new string[1] { "* (You got the [ITEM].)" };

	[SerializeField]
	protected string[] purchaseSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] purchaseSpeed = new int[1];

	[SerializeField]
	protected string[] purchasePortraits;

	[SerializeField]
	protected string[] rejectLines = new string[0];

	[SerializeField]
	protected string[] rejectSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] rejectSpeed = new int[1];

	[SerializeField]
	protected string[] rejectPortraits;

	[SerializeField]
	protected string[] noSpaceLines = new string[1] { "* (You are carrying too\n  many items.)" };

	[SerializeField]
	protected string[] noSpaceSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] noSpaceSpeed = new int[1];

	[SerializeField]
	protected string[] noSpacePortraits;

	[SerializeField]
	private Sprite emptySprite;

	[SerializeField]
	protected string[] emptyLines = new string[1] { "* (The box is empty.)" };

	[SerializeField]
	protected string[] emptySounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] emptySpeed = new int[1];

	[SerializeField]
	protected string[] emptyPortraits;

	protected bool empty;

	private void Awake()
	{
		if (flag > -1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(flag) == 1)
		{
			empty = true;
			GetComponent<SpriteRenderer>().sprite = emptySprite;
		}
	}

	public override void DoInteract()
	{
		if (!txt && enabled)
		{
			txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			if (!empty)
			{
				txt.CreateBox(lines, sounds, speed, false, portraits);
				txt.EnableSelectionAtEnd();
			}
			else
			{
				txt.CreateBox(emptyLines, emptySounds, emptySpeed, true, emptyPortraits);
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			if (Object.FindObjectOfType<GameManager>().NumItemFreeSpace() == 0)
			{
				txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(noSpaceLines, noSpaceSounds, noSpaceSpeed, true, noSpacePortraits);
			}
			else
			{
				Object.FindObjectOfType<GameManager>().PlayGlobalSFX("sounds/snd_item");
				Object.FindObjectOfType<GameManager>().AddItem(itemID);
				if (flag > -1)
				{
					Object.FindObjectOfType<GameManager>().SetFlag(flag, 1);
				}
				txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(purchaseLines, purchaseSounds, purchaseSpeed, true, purchasePortraits);
				empty = true;
				GetComponent<SpriteRenderer>().sprite = emptySprite;
			}
		}
		else if (index == Vector2.right)
		{
			if (rejectLines.Length != 0)
			{
				txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(rejectLines, rejectSounds, rejectSpeed, true, rejectPortraits);
			}
			else
			{
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			}
		}
		selectActivated = false;
	}
}
