using System;
using System.Collections.Generic;

[Serializable]
public class SAVEFile
{
	public string zoneName;

	public bool susieActive;

	public bool noelleActive;

	public string name;

	public int exp;

	public List<int> items;

	public List<int> equipItems;

	public List<int> boxItems;

	public int[] party;

	public int[] hp;

	public int[] weapon;

	public int[] armor;

	public int playTime;

	public int zone;

	public int gold;

	public object[] flags;

	public object[] persFlags;

	public int deaths;

	public void UpdateCharacterInfo(string name, int exp, List<int> items, List<int> equipItems, List<int> boxItems, int[] party, int[] hp, int[] weapon, int[] armor, int playTime, int zone, int gold, object[] flags)
	{
		this.name = name;
		this.exp = exp;
		this.items = new List<int>(items);
		this.equipItems = new List<int>(equipItems);
		this.boxItems = new List<int>(boxItems);
		this.party = (int[])party.Clone();
		this.hp = (int[])hp.Clone();
		this.weapon = (int[])weapon.Clone();
		this.armor = (int[])armor.Clone();
		this.playTime = playTime;
		this.zone = zone;
		this.gold = gold;
		this.flags = (object[])flags.Clone();
	}

	public void UpdateDeathCount(int deaths)
	{
		this.deaths = deaths;
	}

	public void UpdatePersistentFlags(object[] persFlags)
	{
		this.persFlags = persFlags;
	}
}
