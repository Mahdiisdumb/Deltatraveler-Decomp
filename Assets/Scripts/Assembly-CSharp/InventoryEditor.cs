using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class InventoryEditor : SelectableBehaviour
{
	private int[] items;

	private bool holding;

	private int dir;

	private int partyMember;

	private int[] weapons = new int[3];

	private int[] armors = new int[3];

	private Transform textParent;

	private OverworldPlayer player;

	private Selection sel;

	private AudioSource aud;

	private int curIndex;

	private void Awake()
	{
		player = Util.OverworldPlayer();
		holding = false;
		dir = 0;
		curIndex = 0;
		base.transform.SetParent(GameObject.Find("Canvas").transform);
		base.gameObject.AddComponent<UIBackground>().CreateElement("InventoryEditorBG", new Vector2(0f, 0f), new Vector2(412f, 462f));
		textParent = Object.Instantiate(Resources.Load<GameObject>("ui/debug/InventoryEditorText"), base.transform).transform;
		sel = base.gameObject.AddComponent<Selection>();
		items = new int[10];
		for (int i = 0; i < items.Length; i++)
		{
			switch (i)
			{
			case 8:
				items[i] = Util.GameManager().GetWeapon(partyMember);
				break;
			case 9:
				items[i] = Util.GameManager().GetArmor(partyMember);
				break;
			default:
				items[i] = Util.GameManager().GetItem(i);
				break;
			}
		}
		for (int j = 0; j < 3; j++)
		{
			weapons[j] = Util.GameManager().GetWeapon(j);
			armors[j] = Util.GameManager().GetArmor(j);
		}
		aud = base.gameObject.AddComponent<AudioSource>();
		aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
		aud.Play();
		UpdateInfo();
	}

	private void Start()
	{
		Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
	}

	private void Update()
	{
		if ((bool)player && player.CanMove())
		{
			Util.GameManager().DisablePlayerMovement(deactivatePartyMembers: false);
		}
		int num = curIndex;
		curIndex = (int)sel.GetIndex()[0];
		if (curIndex != num)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == -1f && items[curIndex] > -1 && !holding)
		{
			aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
			aud.Play();
			items[curIndex]--;
			if (curIndex == 8)
			{
				while (Items.ItemType(items[curIndex]) != 1 && Items.ItemType(items[curIndex]) != -1)
				{
					items[curIndex]--;
				}
				weapons[partyMember] = items[curIndex];
			}
			else if (curIndex == 9)
			{
				while (Items.ItemType(items[curIndex]) != 2 && Items.ItemType(items[curIndex]) != -1)
				{
					items[curIndex]--;
				}
				armors[partyMember] = items[curIndex];
			}
			holding = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == 1f && items[curIndex] < Items.NumOfItems() - 1 && !holding)
		{
			if (curIndex == 8)
			{
				if (items[curIndex] != Items.GetHighestWeaponIndex())
				{
					aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
					aud.Play();
					items[curIndex]++;
					while (Items.ItemType(items[curIndex]) != 1 && Items.ItemType(items[curIndex]) != -1 && items[curIndex] < Items.GetHighestWeaponIndex())
					{
						items[curIndex]++;
					}
				}
			}
			else if (curIndex == 9)
			{
				if (items[curIndex] != Items.GetHighestArmorIndex())
				{
					aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
					aud.Play();
					items[curIndex]++;
					while (Items.ItemType(items[curIndex]) != 2 && Items.ItemType(items[curIndex]) != -1 && items[curIndex] < Items.GetHighestWeaponIndex())
					{
						items[curIndex]++;
					}
				}
			}
			else
			{
				aud.clip = Resources.Load<AudioClip>("sounds/snd_menumove");
				aud.Play();
				items[curIndex]++;
			}
			holding = true;
			UpdateInfo();
		}
		else if (UTInput.GetAxis("Horizontal") == 0f && holding)
		{
			holding = false;
		}
		else if (UTInput.GetButtonDown("Z"))
		{
			partyMember++;
			if (partyMember >= 3)
			{
				partyMember = 0;
			}
			items[8] = weapons[partyMember];
			items[9] = armors[partyMember];
			UpdateInfo();
		}
		else
		{
			if (!UTInput.GetButtonDown("C"))
			{
				return;
			}
			while (Util.GameManager().FirstFreeItemSpace(equipment: false) != 0)
			{
				Util.GameManager().RemoveItem(0);
			}
			for (int i = 0; i < 8; i++)
			{
				if (items[i] != -1)
				{
					Util.GameManager().AddItem(items[i]);
				}
			}
			for (int j = 0; j < 3; j++)
			{
				Util.GameManager().ForceWeapon(j, weapons[j]);
				Util.GameManager().ForceArmor(j, armors[j]);
			}
			if ((bool)player)
			{
				Util.GameManager().LoadArea(SceneManager.GetActiveScene().buildIndex, fadeIn: true, player.transform.position, player.GetDirection());
			}
			else
			{
				SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
			}
		}
	}

	private void UpdateInfo()
	{
		string text = (new string[3] { "KRIS", "SUSIE", "NOELLE" })[partyMember];
		string[,] sels = new string[10, 1]
		{
			{ items[0] + " - " + Items.ItemName(items[0]) },
			{ items[1] + " - " + Items.ItemName(items[1]) },
			{ items[2] + " - " + Items.ItemName(items[2]) },
			{ items[3] + " - " + Items.ItemName(items[3]) },
			{ items[4] + " - " + Items.ItemName(items[4]) },
			{ items[5] + " - " + Items.ItemName(items[5]) },
			{ items[6] + " - " + Items.ItemName(items[6]) },
			{ items[7] + " - " + Items.ItemName(items[7]) },
			{ text + "WEAPON: " + items[8] + " - " + Items.ItemName(items[8]) },
			{ text + "ARMOR: " + items[9] + " - " + Items.ItemName(items[9]) }
		};
		Vector2 selection = Vector2.zero;
		if (sel.IsEnabled())
		{
			selection = sel.GetIndex();
			sel.Reset();
		}
		sel.CreateSelections(sels, new Vector2(-150f, 48f), new Vector2(0f, -32f), new Vector2(-15f, 94f), "DTM-Sans", useSoul: true, makeSound: false, this, 1);
		sel.SetSelection(selection);
		textParent.Find("LEFT").GetComponent<Text>().enabled = true;
		if (items[curIndex] == -1)
		{
			textParent.Find("LEFT").GetComponent<Text>().enabled = false;
		}
		textParent.Find("RIGHT").GetComponent<Text>().enabled = true;
		if (items[curIndex] == Items.NumOfItems() - 1 || (curIndex == 8 && items[curIndex] == Items.GetHighestWeaponIndex()) || (curIndex == 8 && items[curIndex] == Items.GetHighestArmorIndex()))
		{
			textParent.Find("RIGHT").GetComponent<Text>().enabled = false;
		}
		dir = 0;
	}

	public override void MakeDecision(Vector2 index, int id)
	{
	}
}
