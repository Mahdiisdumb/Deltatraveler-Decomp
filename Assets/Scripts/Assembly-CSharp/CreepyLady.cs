using UnityEngine;

public class CreepyLady : InteractShop
{
	private OverworldPartyMember follower;

	private int detectFrames;

	private bool detecting;

	private void Awake()
	{
		follower = GetComponent<OverworldPartyMember>();
		follower.Deactivate();
		follower.SetSpritePath("overworld/npcs/hhvillage/");
	}

	private void Start()
	{
		if ((int)Util.GameManager().GetFlag(116) != 0)
		{
			follower.SetCustomSpritesetPrefix("normal");
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

	public override void DoInteract()
	{
		GetComponent<Animator>().SetFloat("dirX", Util.OverworldPlayer().transform.position.x - base.transform.position.x);
		GetComponent<Animator>().SetFloat("dirY", Util.OverworldPlayer().transform.position.y - base.transform.position.y);
		if ((bool)txt || !enabled)
		{
			return;
		}
		if ((int)Util.GameManager().GetFlag(115) == 2)
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* I should pour the funds\n  from the religion into\n  charity organizations." });
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		else if ((int)Util.GameManager().GetFlag(116) != 0)
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			if ((int)Util.GameManager().GetFlag(115) == 0)
			{
				if (Util.GameManager().NumItemFreeSpace(equipment: false) == 0)
				{
					txt.CreateBox(new string[3] { "* Finally,^05 I have a chance\n  to apologize.", "* ...^05 Umm...^05 I'm sorry for\n  being creepy.", "* I'd give you what I\n  would've given you,^05 but you're\n  carrying too much." });
				}
				else
				{
					txt.CreateBox(new string[4] { "* Finally,^05 I have a chance\n  to apologize.", "* ...^05 Umm...^05 I'm sorry for\n  being creepy.", "* You can have this weird\n  postcard as an apology.", "* (You got the Postcard.)\n* (It was added to your ITEMs.)" });
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
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		else if ((int)Util.GameManager().GetFlag(115) == 0)
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[3] { "* Excuse me, tourists.", "* I'm collecting donations to\n  help protect the world from\n  contaminants.", "* Donate whatever you can." }, giveBackControl: false);
			shopBG = Object.Instantiate(Resources.Load<GameObject>("ui/MiniShopUI"), GameObject.Find("Canvas").transform).GetComponent<MiniShopUI>();
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			txt.EnableSelectionAtEnd();
		}
		else
		{
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(new string[1] { "* Thank you for your patronage." });
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		if (index == Vector2.right || (index == Vector2.left && Util.GameManager().GetGold() == 0))
		{
			follower.Activate();
			Util.OverworldPlayer().AddPartyMember(follower);
			string[] array = new string[2] { "* Screw off,^05 weirdo.", "* ...\n^10* Then I shall be your\n  shadow." };
			if (index == Vector2.left)
			{
				array[0] = "* We don't have any\n  money,^10 weirdo.";
			}
			txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
			txt.CreateBox(array, new string[2] { "snd_txtsus", "snd_text" }, new int[2], giveBackControl: true, new string[2] { "su_annoyed", "" });
			Util.OverworldPlayer().GetPartyMemberByID(2).UseUnhappySprites();
		}
		else
		{
			follower.Deactivate();
			Util.OverworldPlayer().RemovePartyMember(follower);
			if (Util.GameManager().NumItemFreeSpace(equipment: false) == 0)
			{
				txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[6] { "* Your good deed will be\n  rewarded.", "* Here's a strange card for\n  you...", "* Wait,^05 you don't have any\n  free space.", "* I'll bother you later,^05\n  then...", "* Take your money back.", "* (You got 1 GOLD.)" }, giveBackControl: true);
			}
			else
			{
				txt = new GameObject("CreepyLadyInteract", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[3] { "* Your good deed will be\n  rewarded.", "* Here's a strange card for\n  you.", "* (You got the Postcard.)\n* (It was added to your ITEMs.)" }, giveBackControl: true);
				Util.GameManager().RemoveGold(1);
				Util.GameManager().AddItem(24);
				Util.GameManager().SetFlag(115, 1);
				shopBG.UpdateText();
			}
			Util.OverworldPlayer().GetPartyMemberByID(2).UseHappySprites();
		}
		selectActivated = false;
	}

	public void DetectPlayer()
	{
		detecting = true;
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		base.transform.Find("Exclaim").GetComponent<SpriteRenderer>().enabled = true;
		base.transform.Find("Exclaim").GetComponent<AudioSource>().Play();
		GetComponent<Animator>().SetFloat("dirX", Util.OverworldPlayer().transform.position.x - base.transform.position.x);
		GetComponent<Animator>().SetFloat("dirY", Util.OverworldPlayer().transform.position.y - base.transform.position.y);
	}
}
