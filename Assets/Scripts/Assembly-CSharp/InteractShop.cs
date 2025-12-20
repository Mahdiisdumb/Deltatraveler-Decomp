using UnityEngine;
using UnityEngine.UI;

public class InteractShop : Interactable
{
	[SerializeField]
	public new bool enabled = true;

	[SerializeField]
	private int itemID;

	[SerializeField]
	private int price = 10;

	[SerializeField]
	private string buyText = "Yes";

	[SerializeField]
	private string rejectText = "No";

	[SerializeField]
	protected string[] lines = new string[1] { "* [NO_TEXT]" };

	[SerializeField]
	protected string[] sounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] speed = new int[1];

	[SerializeField]
	protected string[] portraits;

	[SerializeField]
	protected Remark[] remarks;

	[SerializeField]
	protected string[] purchaseLines = new string[1] { "* [NO_TEXT]" };

	[SerializeField]
	protected string[] purchaseSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] purchaseSpeed = new int[1];

	[SerializeField]
	protected string[] purchasePortraits;

	[SerializeField]
	protected Remark[] purchaseRemarks;

	[SerializeField]
	protected string[] rejectLines = new string[0];

	[SerializeField]
	protected string[] rejectSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] rejectSpeed = new int[1];

	[SerializeField]
	protected string[] rejectPortraits;

	[SerializeField]
	protected Remark[] rejectRemarks;

	[SerializeField]
	protected string[] noSpaceLines = new string[1] { "* You are carrying too\n  many items." };

	[SerializeField]
	protected string[] noSpaceSounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] noSpaceSpeed = new int[1];

	[SerializeField]
	protected string[] noSpacePortraits;

	[SerializeField]
	protected Remark[] noSpaceRemarks;

	[SerializeField]
	protected string[] noMoneyLines = new string[1] { "* You didn't have enough\n  gold." };

	[SerializeField]
	protected string[] noMoneySounds = new string[1] { "snd_text" };

	[SerializeField]
	protected int[] noMoneySpeed = new int[1];

	[SerializeField]
	protected string[] noMoneyPortraits;

	[SerializeField]
	protected Remark[] noMoneyRemarks;

	protected bool selectActivated;

	protected UIBackground shopBG;

	[SerializeField]
	private Sprite[] talkSprites;

	[SerializeField]
	private int talkFramerate = 6;

	private int talkFrames;

	protected string talkName = "Talk";

	protected virtual void Update()
	{
		if ((bool)txt)
		{
			HandleTextExist();
			if ((bool)GetComponent<Animator>() && GetComponent<Animator>().enabled)
			{
				if (txt.IsPlaying())
				{
					talkFrames++;
					if (talkFrames == 1 && GetComponent<Animator>().HasState(0, Animator.StringToHash(talkName)))
					{
						GetComponent<Animator>().Play(talkName, 0, 0f);
					}
				}
				else
				{
					if (talkFrames > 0)
					{
						string stateName = (GetComponent<Animator>().HasState(0, Animator.StringToHash("Idle")) ? "Idle" : "idle");
						GetComponent<Animator>().Play(stateName, 0, 0f);
					}
					talkFrames = 0;
				}
			}
			else if (txt.IsPlaying() && talkSprites != null)
			{
				talkFrames++;
				if (talkSprites.Length != 0)
				{
					GetComponent<SpriteRenderer>().sprite = talkSprites[talkFrames / talkFramerate % talkSprites.Length];
				}
			}
			else if (talkSprites != null)
			{
				talkFrames = 0;
				if (talkSprites.Length != 0)
				{
					GetComponent<SpriteRenderer>().sprite = talkSprites[0];
				}
			}
		}
		else if (talkFrames > 0)
		{
			talkFrames = 0;
			if (talkSprites != null && talkSprites.Length != 0)
			{
				GetComponent<SpriteRenderer>().sprite = talkSprites[0];
			}
			if ((bool)GetComponent<Animator>() && GetComponent<Animator>().enabled)
			{
				string stateName2 = (GetComponent<Animator>().HasState(0, Animator.StringToHash("Idle")) ? "Idle" : "idle");
				GetComponent<Animator>().Play(stateName2, 0, 0f);
			}
		}
		if (!txt && (bool)shopBG)
		{
			Object.Destroy(shopBG.gameObject);
		}
	}

	public override void DoInteract()
	{
		if (!txt && enabled)
		{
			CreateTextBox(lines, sounds, speed, false, portraits, remarks);
			shopBG = new GameObject("ShopMenu").AddComponent<UIBackground>();
			shopBG.transform.parent = GameObject.Find("Canvas").transform;
			shopBG.CreateElement("space", new Vector2(189f, 2f), new Vector2(202f, 108f));
			Text component = Object.Instantiate(Resources.Load<GameObject>("ui/SelectionBase"), shopBG.transform).GetComponent<Text>();
			component.gameObject.name = "SpaceInfo";
			component.transform.localScale = new Vector3(1f, 1f, 1f);
			component.transform.localPosition = new Vector3(116f, -71f);
			component.text = "$ - " + Object.FindObjectOfType<GameManager>().GetGold() + "G\nSPACE - " + (8 - Object.FindObjectOfType<GameManager>().NumItemFreeSpace()) + "/8";
			component.lineSpacing = 1.3f;
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
			txt.EnableSelectionAtEnd();
		}
	}

	protected virtual void HandleTextExist()
	{
		if (txt.CanLoadSelection() && !selectActivated)
		{
			selectActivated = true;
			DeltaSelection component = Object.Instantiate(Resources.Load<GameObject>("ui/DeltaSelection"), Vector3.zero, Quaternion.identity, txt.GetUIBox().transform).GetComponent<DeltaSelection>();
			component.SetupChoice(Vector2.left, buyText, Vector3.zero);
			component.SetupChoice(Vector2.right, rejectText, new Vector3(38f, 0f));
			component.Activate(this, 0, txt.gameObject);
		}
	}

	public override int GetEventData()
	{
		return -1;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.left)
		{
			if (Object.FindObjectOfType<GameManager>().NumItemFreeSpace() == 0)
			{
				CreateTextBox(noSpaceLines, noSpaceSounds, noSpaceSpeed, true, noSpacePortraits, noSpaceRemarks);
			}
			else if (Object.FindObjectOfType<GameManager>().GetGold() < price)
			{
				CreateTextBox(noMoneyLines, noMoneySounds, noMoneySpeed, true, noMoneyPortraits, noMoneyRemarks);
			}
			else
			{
				Object.FindObjectOfType<GameManager>().AddItem(itemID);
				Object.FindObjectOfType<GameManager>().RemoveGold(price);
				shopBG.transform.Find("SpaceInfo").GetComponent<Text>().text = "$ - " + Object.FindObjectOfType<GameManager>().GetGold() + "G\nSPACE - " + (8 - Object.FindObjectOfType<GameManager>().NumItemFreeSpace()) + "/8";
				CreateTextBox(purchaseLines, purchaseSounds, purchaseSpeed, true, purchasePortraits, purchaseRemarks);
			}
		}
		else if (index == Vector2.right)
		{
			if (rejectLines.Length != 0)
			{
				CreateTextBox(rejectLines, rejectSounds, rejectSpeed, true, rejectPortraits, rejectRemarks);
			}
			else
			{
				Object.FindObjectOfType<GameManager>().EnablePlayerMovement();
			}
		}
		selectActivated = false;
	}
}
