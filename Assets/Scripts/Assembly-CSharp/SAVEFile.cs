using System;
using System.Collections.Generic;

[Serializable]
public class SAVEFile
{
	public string name;

	public int exp;

	public List<int> items;

	public int[] weapon;

	public int[] armor;

	public bool susieActive;

	public bool noelleActive;

	public int playTime;

	public int zone;

	public int gold;

	public object[] flags;

	public object[] persFlags;

	public string zoneName;

	public int deaths;

	public void UpdateCharacterInfo(string name, int exp, List<int> items, int[] weapon, int[] armor, bool susieActive, bool noelleActive, int playTime, int zone, int gold, string zoneName, object[] flags)
	{
		this.name = name;
		this.exp = exp;
		this.items = new List<int>(items);
		this.weapon = (int[])weapon.Clone();
		this.armor = (int[])armor.Clone();
		this.susieActive = susieActive;
		this.noelleActive = noelleActive;
		this.playTime = playTime;
		this.zone = zone;
		this.gold = gold;
		this.zoneName = zoneName;
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
