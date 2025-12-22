using UnityEngine;

public class UFLetter : InteractSelectionBase
{
	[SerializeField]
	private Sprite[] sprites;

	private bool empty;

	private GameObject prefab;

	private bool createFakeBones;

	private int frames;

	private void Awake()
	{
		prefab = Resources.Load<GameObject>("overworld/bullets/FakeOverworldBoneBullet");
		if (!Util.GameManager().NoelleInParty())
		{
			lines[2] = "* Good thing Noelle isn't\n  here to like... ^10tell\n  you to open it.";
			sounds[2] = "snd_txtsus";
			portraits[2] = "su_smirk_sweat";
		}
		else if ((int)Util.GameManager().GetFlag(172) == 1)
		{
			if (Util.GameManager().GetFlagInt(184) == 1)
			{
				lines[2] = "* Kris,^05 please don't.";
				portraits[2] = "no_depressedx";
			}
			else
			{
				lines[2] = "* ... But I'm a little\n  curious what's in it.";
				portraits[2] = "no_depressedx_smile";
			}
		}
	}

	public override void DoInteract()
	{
		if (empty && !txt && enabled)
		{
			if (Util.GameManager().SusieInParty())
			{
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				if ((int)Util.GameManager().GetFlag(198) == 1)
				{
					txt.CreateBox(new string[3] { "* How the hell did\n  he fit a bunch of\n  bones in there???", "* ... The hell is with\n  that look,^05 Kris?", "* It's not like it'll\n  have any other surprises\n  in it." }, new string[1] { "snd_txtsus" }, new int[1], giveBackControl: true, new string[3] { "su_side_sweat", "su_annoyed", "su_smirk_sweat" });
				}
				else
				{
					txt.CreateBox(new string[1] { "* How the hell did\n  he fit a bunch of\n  bones in there???" }, new string[1] { "snd_txtsus" }, new int[1], giveBackControl: true, new string[1] { "su_side_sweat" });
				}
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			}
			else
			{
				Util.GameManager().SetFlag(198, 1);
				txt = new GameObject("InteractTextBoxSelection", typeof(TextBox)).GetComponent<TextBox>();
				txt.CreateBox(new string[3] { "* (You noticed that there was\n  an actual message in the\n  letter.)", "* (It reads...)", "* \"I KNOW WHAT YOU ARE\"" }, giveBackControl: true);
				Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
			}
		}
		else
		{
			base.DoInteract();
		}
	}

	private void LateUpdate()
	{
		if (createFakeBones)
		{
			frames++;
			if (frames % 2 == 1)
			{
				Object.Instantiate(prefab, new Vector3(0f, -10f), Quaternion.identity);
			}
		}
		if (empty)
		{
			if (createFakeBones)
			{
				base.transform.GetChild(0).position = new Vector3(Random.Range(-4, 5), Random.Range(-4, 5)) / 48f;
			}
			else
			{
				base.transform.GetChild(0).position = Vector2.zero;
			}
		}
	}

	public override void MakeDecision(Vector2 index, int id)
	{
		base.MakeDecision(index, id);
		if (index == Vector2.left)
		{
			Util.GameManager().SetFlag(199, 1);
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: true);
			CutsceneHandler.GetCutscene(66).StartCutscene();
		}
		else
		{
			Util.GameManager().EnablePlayerMovement();
		}
	}

	public void SetSprite(int i)
	{
		GetComponentInChildren<SpriteRenderer>().sortingOrder = ((i == 0) ? 100 : (-400));
		GetComponentInChildren<SpriteRenderer>().sprite = sprites[i];
	}

	public void StartGeneratingBones()
	{
		createFakeBones = true;
	}

	public void StopGeneratingBones()
	{
		createFakeBones = false;
	}

	public void MakeLetterEmpty()
	{
		if (!empty)
		{
			base.transform.parent = base.transform.parent.parent;
			SetSprite(1);
			empty = true;
		}
	}
}
