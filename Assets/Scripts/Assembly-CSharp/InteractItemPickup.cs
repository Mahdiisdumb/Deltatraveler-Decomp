using UnityEngine;

public class InteractItemPickup : InteractSelectionBase
{
	[SerializeField]
	private int flag = -1;

	[SerializeField]
	private int itemID;

	[SerializeField]
	protected string[] purchaseLines = new string[1] { "* [NO_TEXT]" };

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
	protected string[] noSpaceLines = new string[1] { "* You are carrying too\n  many items." };

	[SerializeField]
	protected string[] noSpaceSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] noSpaceSpeed = new int[1];

	[SerializeField]
	protected string[] noSpacePortraits;

	private void Awake()
	{
		if (flag > -1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(flag) == 1)
		{
			Object.Destroy(base.gameObject);
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
				Object.FindObjectOfType<GameManager>().AddItem(itemID);
				if (flag > -1)
				{
					Object.FindObjectOfType<GameManager>().SetFlag(flag, 1);
				}
				txt = new GameObject("InteractTextBoxItem", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(purchaseLines, purchaseSounds, purchaseSpeed, true, purchasePortraits);
				Object.Destroy(base.gameObject);
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

	public void ModifyPurchaseContents(string[] lines, string[] sounds, int[] speed, string[] portraits)
	{
		purchaseLines = lines;
		purchaseSounds = sounds;
		purchaseSpeed = speed;
		purchasePortraits = portraits;
	}
}
