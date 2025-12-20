using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TrainingMenu : SpecialACT
{
	private int[] weapons = new int[3];

	private int miniPartyMember;

	private readonly List<int> allowedKrisWeapons = new List<int> { -1, 3, 8, 13, 20, 21, 31, 32, 34 };

	private readonly List<int> allowedSusieWeapons = new List<int> { -1, 6, 15 };

	private readonly List<int> allowedNoelleWeapons = new List<int> { -1, 3, 8, 13, 20, 21, 31, 32, 34 };

	private readonly string[] miniPartyMembers = new string[2] { "None", "Paula" };

	private bool holdingAxis;

	private int curIndex;

	private void Awake()
	{
		PlayerPrefs.GetInt("JerryDefeated", 0);
		int num = 1;
		allowedKrisWeapons.Insert(9, 41);
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		if (holdingAxis && UTInput.GetAxis("Horizontal") == 0f && UTInput.GetAxis("Vertical") == 0f)
		{
			holdingAxis = false;
		}
		else if (!holdingAxis)
		{
			if (UTInput.GetAxis("Horizontal") != 0f)
			{
				if (curIndex <= 2)
				{
					List<int> list = new List<int>((curIndex == 1) ? allowedSusieWeapons : allowedKrisWeapons);
					if (curIndex == 2)
					{
						list = allowedNoelleWeapons;
					}
					int num = ((curIndex == 0) ? 3 : (-1));
					int num2 = list[list.Count - 1];
					MonoBehaviour.print(UTInput.GetAxis("Horizontal"));
					if ((UTInput.GetAxis("Horizontal") > 0f && weapons[curIndex] < num2) || (UTInput.GetAxis("Horizontal") < 0f && weapons[curIndex] > num))
					{
						int num3 = list.IndexOf(weapons[curIndex]);
						weapons[curIndex] = list[num3 + (int)UTInput.GetAxis("Horizontal")];
						if (weapons[curIndex] == -1)
						{
							base.transform.Find(curIndex.ToString()).GetComponent<Text>().text = "None";
						}
						else
						{
							base.transform.Find(curIndex.ToString()).GetComponent<Text>().text = Items.ItemName(weapons[curIndex]);
						}
					}
				}
				else if (curIndex == 3 && ((UTInput.GetAxis("Horizontal") > 0f && miniPartyMember < miniPartyMembers.Length - 1) || (UTInput.GetAxis("Horizontal") < 0f && miniPartyMember > 0)))
				{
					miniPartyMember += (int)UTInput.GetAxis("Horizontal");
					base.transform.Find(curIndex.ToString()).GetComponent<Text>().text = miniPartyMembers[miniPartyMember];
				}
				holdingAxis = true;
			}
			else if (UTInput.GetAxis("Vertical") != 0f)
			{
				curIndex -= (int)UTInput.GetAxis("Vertical");
				if (curIndex < 0)
				{
					curIndex = 4;
				}
				else if (curIndex > 4)
				{
					curIndex = 0;
				}
				base.transform.Find("SOUL").localPosition = new Vector3(-246f, 58 - curIndex * 30);
				holdingAxis = true;
			}
		}
		if (!UTInput.GetButtonDown("Z") || curIndex != 4)
		{
			return;
		}
		Util.GameManager().SetPartyMembers(weapons[1] != -1, weapons[2] != -1);
		Util.GameManager().SetMiniPartyMember(miniPartyMember);
		for (int i = 0; i < 3; i++)
		{
			if (weapons[i] > -1)
			{
				Util.GameManager().ForceWeapon(i, weapons[i]);
			}
			Util.GameManager().SetHP(i, Util.GameManager().GetMaxHP(i));
		}
		Object.FindObjectOfType<BattleManager>().UpdatePartyMembers();
		Object.FindObjectOfType<BattleManager>().AdvanceToEnemyTurn();
		Object.Destroy(base.gameObject);
	}

	public override void Activate()
	{
		base.Activate();
		for (int i = 0; i < 3; i++)
		{
			weapons[i] = Util.GameManager().GetWeapon(i);
			MonoBehaviour.print(weapons[i]);
			if ((i == 1 && !Util.GameManager().SusieInParty()) || (i == 2 && !Util.GameManager().NoelleInParty()))
			{
				weapons[i] = -1;
			}
			if (weapons[i] == -1)
			{
				base.transform.Find(i.ToString()).GetComponent<Text>().text = "None";
			}
			else
			{
				base.transform.Find(i.ToString()).GetComponent<Text>().text = Items.ItemName(weapons[i]);
			}
		}
		miniPartyMember = Util.GameManager().GetMiniPartyMember();
		base.transform.Find("3").GetComponent<Text>().text = miniPartyMembers[miniPartyMember];
		Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(1);
		Object.FindObjectOfType<BattleManager>().SkipPartyMemberTurn(2);
		Image[] componentsInChildren = GetComponentsInChildren<Image>();
		for (int j = 0; j < componentsInChildren.Length; j++)
		{
			componentsInChildren[j].enabled = true;
		}
		Text[] componentsInChildren2 = GetComponentsInChildren<Text>();
		for (int j = 0; j < componentsInChildren2.Length; j++)
		{
			componentsInChildren2[j].enabled = true;
		}
	}
}
