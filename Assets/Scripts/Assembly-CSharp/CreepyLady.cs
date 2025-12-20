using UnityEngine;
using UnityEngine.UI;

public class CreepyLady : OverworldPartyMember
{
	protected bool selectActivated;

	private int detectFrames;

	private bool detecting;

	private bool following;

	protected UIBackground shopBG;

	private string prefix = "";

	protected override void Awake()
	{
		base.Awake();
		isPlayer = false;
	}

	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(116) != 0)
		{
			prefix = "_normal";
		}
	}

	protected override void Update()
	{
		if ((bool)txt)
		{
			HandleTextExist();
		}
		if (!txt && (bool)shopBG)
		{
			Object.Destroy(shopBG.gameObject);
		}
		if (detecting)
		{
			detectFrames++;
			if (detectFrames == 30)
			{
				base.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = false;
				DoInteract();
			}
		}
		else
		{
			base.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = false;
		}
		base.Update();
	}

	private void LateUpdate()
	{
		if (GetComponent<SpriteRenderer>().sprite.name != curSpriteName && prefix != "")
		{
			Sprite sprite = Resources.Load<Sprite>("overworld/npcs/hhvillage/" + GetComponent<SpriteRenderer>().sprite.name + prefix);
			if (sprite != null)
			{
				GetComponent<SpriteRenderer>().sprite = sprite;
			}
		}
		curSpriteName = GetComponent<SpriteRenderer>().sprite.name;
	}

	public override void DoInteract()
	{
		GetComponent<Animator>().SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
		GetComponent<Animator>().SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
		if ((bool)txt || !base.enabled)
		{
			return;
		}
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(115) == 2)
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* I should pour the funds\n  from the religion into\n  charity organizations." });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
		else if ((int)Object.FindObjectOfType<GameManager>().GetFlag(116) != 0)
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			if ((int)Object.FindObjectOfType<GameManager>().GetFlag(115) == 0)
			{
				if (Object.FindObjectOfType<GameManager>().NumItemFreeSpace() == 0)
				{
					txt.CreateBox(new string[3] { "* Finally,^05 I have a chance\n  to apologize.", "* ...^05 Umm...^05 I'm sorry for\n  being creepy.", "* I'd give you what I\n  would've given you,^05 but you're\n  carrying too much." });
				}
				else
				{
					txt.CreateBox(new string[4] { "* Finally,^05 I have a chance\n  to apologize.", "* ...^05 Umm...^05 I'm sorry for\n  being creepy.", "* You can have this weird\n  postcard as an apology.", "* (You got the Punch Card.)" });
					Util.GameManager().AddItem(24);
					Util.GameManager().SetFlag(115, 2);
				}
			}
			else
			{
				txt.CreateBox(new string[4] { "* Finally,^05 I have a chance\n  to apologize.", "* ...^05 Umm...^05 I'm sorry for\n  being creepy.", "* You can have your money back.", "* (You got 1 GOLD.)" });
				Util.GameManager().AddGold(1);
				Util.GameManager().SetFlag(115, 2);
			}
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
		else if ((int)Object.FindObjectOfType<GameManager>().GetFlag(115) == 0)
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[3] { "* Excuse me, tourists.", "* I'm collecting donations to\n  help protect the world from\n  contaminants.", "* Donate whatever you can." }, false);
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
		else
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* Thank you for your patronage." });
			Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.right || (index == Vector2.left && Object.FindObjectOfType<GameManager>().GetGold() == 0))
		{
			following = true;
			string[] array = new string[2] { "* Screw off,^05 weirdo.", "* ...\n^10* Then I shall be your\n  shadow." };
			if (index == Vector2.left)
			{
				array[0] = "* We don't have any\n  money,^10 weirdo.";
			}
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(array, new string[2] { "snd_txtsus", "snd_text" }, new int[2], true, new string[2] { "su_annoyed", "" });
			GameObject.Find("Noelle").GetComponent<OverworldPartyMember>().UseUnhappySprites();
		}
		else
		{
			following = false;
			Deactivate();
			if (Object.FindObjectOfType<GameManager>().NumItemFreeSpace() == 0)
			{
				txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[6] { "* Your good deed will be\n  rewarded.", "* Here's a strange card for\n  you...", "* Wait,^05 you don't have any\n  free space.", "* I'll bother you later,^05\n  then...", "* Take your money back.", "* (You got 1 GOLD.)" }, true);
			}
			else
			{
				txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[3] { "* Your good deed will be\n  rewarded.", "* Here's a strange card for\n  you.", "* (You got the Punch Card.)" }, true);
				Object.FindObjectOfType<GameManager>().RemoveGold(1);
				Object.FindObjectOfType<GameManager>().AddItem(24);
				Object.FindObjectOfType<GameManager>().SetFlag(115, 1);
				shopBG.transform.Find("SpaceInfo").GetComponent<Text>().text = "$ - " + Object.FindObjectOfType<GameManager>().GetGold() + "G\nSPACE - " + (8 - Object.FindObjectOfType<GameManager>().NumItemFreeSpace()) + "/8";
			}
			GameObject.Find("Noelle").GetComponent<OverworldPartyMember>().UseHappySprites();
		}
		selectActivated = false;
	}

	public bool IsFollowing()
	{
		return following;
	}

	protected virtual void HandleTextExist()
	{
		if (txt.CanLoadSelection() && !selectActivated)
		{
			selectActivated = true;
			DeltaSelection component = Object.Instantiate(Resources.Load<GameObject>("ui/DeltaSelection"), Vector3.zero, Quaternion.identity, txt.GetUIBox().transform).GetComponent<DeltaSelection>();
			component.SetupChoice(Vector2.left, "Give 1G", Vector3.zero);
			component.SetupChoice(Vector2.right, "No", new Vector3(20f, 0f));
			component.Activate(this, 0, txt.gameObject);
		}
	}

	public void DetectPlayer()
	{
		detecting = true;
		Object.FindObjectOfType<GameManager>().DisablePlayerMovement(false);
		base.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = true;
		base.transform.Find("Exclaim").GetComponent<AudioSource>().Play();
		GetComponent<Animator>().SetFloat("dirX", Object.FindObjectOfType<OverworldPlayer>().transform.position.x - base.transform.position.x);
		GetComponent<Animator>().SetFloat("dirY", Object.FindObjectOfType<OverworldPlayer>().transform.position.y - base.transform.position.y);
	}
}
