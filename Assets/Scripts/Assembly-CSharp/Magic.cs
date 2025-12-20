using System.Collections.Generic;
using UnityEngine;

public class Magic : MonoBehaviour
{
	public struct Spell
	{
		private string name;

		private string longDesc;

		private string shortDesc;

		private int tp;

		private bool targetAll;

		private bool targetEnemy;

		private bool isAttackMagic;

		public Spell(string name, string ld, string sd, int tp, bool targetAll, bool targetEnemy, bool isAttackMagic)
		{
			this.name = name;
			longDesc = ld;
			shortDesc = sd;
			this.tp = tp;
			this.targetAll = targetAll;
			this.targetEnemy = targetEnemy;
			this.isAttackMagic = isAttackMagic;
		}

		public string GetName()
		{
			return name;
		}

		public string GetLongDescription()
		{
			return longDesc;
		}

		public string GetShortDescription()
		{
			return shortDesc;
		}

		public int GetTPCost()
		{
			return tp;
		}

		public bool TargetsEveryone()
		{
			return targetAll;
		}

		public bool TargetsEnemies()
		{
			return targetEnemy;
		}

		public bool IsAttackMagic()
		{
			return isAttackMagic;
		}
	}

	public enum ID
	{
		None = -1,
		ACT = 0,
		MiniACT = 1,
		RudeBuster = 2,
		UltimateHeal = 3,
		SleepMist = 4,
		HealPrayer = 5,
		IceShock = 6,
		PSILifeup = 7,
		PSIShield = 8,
		PKFreeze = 9,
		PKFire = 10
	}

	private static readonly Spell[] spells = new Spell[11]
	{
		new Spell("ACT", "* This allows you to do many\n  kinds of things.\n* Don't confuse it with magic.", "BITCH!", 0, targetAll: false, targetEnemy: true, isAttackMagic: false),
		new Spell("Mini-ACT", "* Allows {0} to ACT on\n  {1} own.", "", 0, targetAll: false, targetEnemy: true, isAttackMagic: false),
		new Spell("Rude Buster", "* Powerful magical spell that\n  deals RUDE damage to one\n  enemy.", "Deals RUDE Damage", 50, targetAll: false, targetEnemy: true, isAttackMagic: true),
		new Spell("UltimateHeal", "* Really good and awesome\n  healing attack. (LOSERS!)", "The best healing", 100, targetAll: false, targetEnemy: false, isAttackMagic: false),
		new Spell("Sleep Mist", "* A very sleepy spell that\n  pacifies all TIRED enemies.\n* Needs elemental weapon to cast.", "Spares TIRED Enemies", 32, targetAll: true, targetEnemy: true, isAttackMagic: false),
		new Spell("HealPrayer", "* A LIGHT spell that heals\n  one party member.", "Uses LIGHT to heal", 32, targetAll: false, targetEnemy: false, isAttackMagic: false),
		new Spell("Ice Shock", "* Powerful ICE spell that deals\n  ICE damage to one enemy.\n* Requires ICE weapon to cast.", "Deals ICE Damage", 24, targetAll: false, targetEnemy: true, isAttackMagic: true),
		new Spell("Lifeup", "* Psychic healing move that heals\n  15 HP to one party member.", "Heals 15 HP", 24, targetAll: false, targetEnemy: false, isAttackMagic: false),
		new Spell("Shield", "* Creates a LIGHT shield around\n  your SOUL, reducing damage\n  received by 33%. Lasts 15 hits.", "Creates a LIGHT shield", 50, targetAll: true, targetEnemy: false, isAttackMagic: false),
		new Spell("PK Freeze", "* Powerful psychic ICE moves that\n  deals ICE damage to one enemy.", "Deals ICE Damage", 24, targetAll: false, targetEnemy: true, isAttackMagic: true),
		new Spell("PK Fire", "* Effective psychic FIRE moves\n  that deals FIRE damage to all\n  enemies.", "Deals all FIRE Damage", 36, targetAll: true, targetEnemy: true, isAttackMagic: true)
	};

	public static Spell GetSpell(int id)
	{
		return spells[id];
	}

	public static Spell GetSpell(ID spellId)
	{
		return GetSpell((int)spellId);
	}

	public static Spell GetSpell(int partyMember, int index)
	{
		return GetSpell(GetSpellList(partyMember)[index]);
	}

	public static string[] UseMagic(ID spellID, EnemyBase[] enemies, int user, int target, int devious = -1, int miniACTId = 0)
	{
		GameManager gameManager = Util.GameManager();
		BattleManager battleManager = Util.FindObjectOfType<BattleManager>();
		Spell spell = GetSpell(spellID);
		switch (spellID)
		{
		case ID.MiniACT:
			return enemies[target].PerformAssistAct(user, miniACTId);
		case ID.RudeBuster:
		{
			if (!enemies[target].PartyMemberAcceptAttack(user, 1) || devious == 1)
			{
				return new string[1] { "* Susie cast RUDE BUSTER...^10\n  onto the wall." };
			}
			RudeBusterEffect component2 = Object.Instantiate(Resources.Load<GameObject>("battle/RudeBuster")).GetComponent<RudeBusterEffect>();
			int num2 = target;
			if (enemies[num2].IsDone())
			{
				for (int j = 0; j < enemies.Length; j++)
				{
					if (!enemies[j].IsDone() && j != num2)
					{
						num2 = j;
						break;
					}
				}
			}
			component2.AssignEnemy(enemies[num2]);
			if (devious == 2 || devious == 3)
			{
				component2.SetDevious(devious == 2);
			}
			string[] array = new string[1] { "* Susie cast RUDE BUSTER!" };
			if (battleManager.IsSusieDevious())
			{
				array[0] = BattleManager.DEVIOUS_STRING + array[0];
			}
			return array;
		}
		case ID.UltimateHeal:
		{
			int num3 = -1;
			if (battleManager.IsSusieDevious() && (devious == 4 || Random.Range(0, 1) == 0))
			{
				if (Random.Range(0, 5) == 0)
				{
					if (devious != 4)
					{
						MonoBehaviour.print("SUSIE DEVIOUS HEAL: random enemy");
					}
					for (int k = 0; k < enemies.Length; k++)
					{
						if (!enemies[k].IsDone())
						{
							num3 = k;
							break;
						}
					}
				}
				else
				{
					if (devious != 4)
					{
						MonoBehaviour.print("SUSIE DEVIOUS HEAL: random party member");
					}
					target = Random.Range(0, battleManager.GetPartySize());
				}
			}
			string text = ((user == target) ? "herself." : (PartyMembers.GetMemberName(target, gameManager.GetPartyMember(0) == target, useCase: false) + "."));
			if (num3 >= 0)
			{
				text = enemies[num3].GetName() + ".";
			}
			int num4 = Mathf.FloorToInt(PartyMembers.GetMagicRaw(1) + (float)PartyMembers.GetMagicEquipment(1) / 2f);
			if (battleManager.IsSeriousMode())
			{
				num4 += 3;
			}
			string[] result;
			if (battleManager.IsSusieDevious())
			{
				string text2 = BattleManager.DEVIOUS_STRING + "* Susie cast ULTIMATE HEAL\n  onto " + text;
				result = ((num3 < 0) ? new string[2]
				{
					text2,
					Items.GetRecoveryString(target, num4)
				} : new string[1] { text2 });
			}
			else
			{
				result = new string[1] { "* Susie cast ULTIMATE HEAL\n  onto " + text + "\n" + Items.GetRecoveryString(target, num4) };
			}
			if (num3 >= 0)
			{
				enemies[num3].Hit(1, -num4, playSound: true);
			}
			else
			{
				PartyMembers.Heal(target, num4);
				gameManager.PlayTimedHealSound();
			}
			battleManager.PlaySound2("sounds/snd_spell_cure_slight_smaller");
			return result;
		}
		case ID.SleepMist:
			if (Items.GetItemElement(PartyMembers.GetWeapon(2)) == 1)
			{
				SleepMist component4 = Object.Instantiate(Resources.Load<GameObject>("battle/SleepMist")).GetComponent<SleepMist>();
				string[] array2 = new string[1] { "* Noelle cast SLEEP MIST!" };
				bool flag = false;
				int num7 = 0;
				for (int m = 0; m < enemies.Length; m++)
				{
					if (!enemies[m].IsDone())
					{
						num7++;
					}
					if (enemies[m].IsTired() && !enemies[m].IsDone())
					{
						enemies[m].Spare(sleepMist: true);
						if (flag)
						{
							enemies[m].GetComponent<AudioSource>().Stop();
						}
						flag = true;
					}
					else if (!enemies[m].CanSpare() && !enemies[m].IsDone())
					{
						enemies[m].AttemptedSpare();
					}
				}
				string text3 = "* But none of the enemies\n  were <color=#00A2E8FF>TIRED</color>...";
				if (num7 == 1)
				{
					text3 = "* But the enemy wasn't\n  <color=#00A2E8FF>TIRED</color>...";
				}
				if (!flag)
				{
					ref string reference = ref array2[0];
					reference = reference + "\n" + text3;
				}
				else
				{
					component4.GetComponents<AudioSource>()[0].Play();
				}
				return array2;
			}
			return new string[1] { "* Noelle tried SLEEP MIST,^05\n  but wasn't able to..." };
		case ID.HealPrayer:
		{
			string text4 = ((user == target) ? "herself." : (PartyMembers.GetMemberName(target, gameManager.GetPartyMember(0) == target, useCase: false) + "."));
			int num8 = PartyMembers.GetMaxHP(2) / 4 + Mathf.FloorToInt((PartyMembers.GetMagicRaw(2) + (float)Items.GetItemMagic(PartyMembers.GetArmor(2))) / 2f);
			if (Items.GetItemElement(PartyMembers.GetWeapon(2)) == 1)
			{
				int num9 = Items.GetItemMagic(PartyMembers.GetWeapon(2));
				if (Items.GetWeaponType(PartyMembers.GetWeapon(2)) == 4)
				{
					num9 /= 2;
				}
				num8 += num9;
			}
			string[] result2 = new string[1] { "* Noelle cast HEAL PRAYER\n  onto " + text4 + "\n" + Items.GetRecoveryString(target, num8) };
			PartyMembers.Heal(target, num8);
			gameManager.PlayTimedHealSound();
			battleManager.PlaySound2("sounds/snd_spellcast");
			return result2;
		}
		case ID.IceShock:
			if (Items.GetItemElement(PartyMembers.GetWeapon(2)) == 1)
			{
				if (!enemies[target].PartyMemberAcceptAttack(user, 1))
				{
					gameManager.PlayGlobalSFX("sounds/snd_hurt");
					PartyMembers.Damage(2, 5);
					return new string[1] { "* Noelle cast ICE SHOCK...^10\n  onto herself." };
				}
				IceShock component3 = Object.Instantiate(Resources.Load<GameObject>("battle/IceShock")).GetComponent<IceShock>();
				int num6 = target;
				if (enemies[num6].IsDone())
				{
					for (int l = 0; l < enemies.Length; l++)
					{
						if (!enemies[l].IsDone())
						{
							num6 = l;
							break;
						}
					}
				}
				component3.AssignEnemy(enemies[num6]);
				return new string[1] { "* Noelle cast ICE SHOCK!" };
			}
			return new string[1] { "* Noelle tried ICE SHOCK,^05\n  but wasn't able to..." };
		case ID.PSILifeup:
		{
			int num5 = 15;
			PartyMembers.Heal(target, num5);
			gameManager.PlayTimedHealSound();
			battleManager.PlaySound2("sounds/snd_psi");
			return new string[1] { "* Paula tried LIFEUP...\n" + Items.GetRecoveryString(target, num5) };
		}
		case ID.PSIShield:
			Util.FindObjectOfType<SOUL>().ActivateLightShield();
			battleManager.PlaySound2("sounds/snd_psi_shield");
			return new string[1] { "* Paula tried SHIELD...\n* Your SOUL was protected by\n  a LIGHT shield for 15 hits!" };
		case ID.PKFreeze:
		{
			PKFreezeEffect component = Object.Instantiate(Resources.Load<GameObject>("battle/PKFreeze")).GetComponent<PKFreezeEffect>();
			int num = target;
			if (enemies[num].IsDone())
			{
				for (int i = 0; i < enemies.Length; i++)
				{
					if (!enemies[i].IsDone())
					{
						num = i;
						break;
					}
				}
			}
			component.AssignEnemy(enemies[num]);
			battleManager.PlaySound2("sounds/snd_psi");
			battleManager.MiniPartyMemberSpellToMainFight(num);
			return new string[1] { "* Paula tried PK FREEZE..." };
		}
		case ID.PKFire:
			Object.Instantiate(Resources.Load<GameObject>("battle/PKFire")).GetComponent<PKFireEffect>();
			battleManager.PlaySound2("sounds/snd_psi");
			battleManager.MiniPartyMemberSpellToMainFight();
			return new string[1] { "* Paula tried PK FIRE..." };
		default:
			return new string[1] { "* " + spell.GetName() + "\n  doesn't work?????\n* You are  broke" };
		}
	}

	public static ID[] GetSpellList(int partyMember)
	{
		switch ((PartyMembers.ID)partyMember)
		{
		case PartyMembers.ID.Kris:
			return new ID[1];
		case PartyMembers.ID.Chara:
		case PartyMembers.ID.Frisk:
			return new ID[1];
		case PartyMembers.ID.Susie:
			return new ID[3]
			{
				ID.MiniACT,
				ID.RudeBuster,
				ID.UltimateHeal
			};
		case PartyMembers.ID.Noelle:
			return new ID[4]
			{
				ID.MiniACT,
				ID.SleepMist,
				ID.HealPrayer,
				ID.IceShock
			};
		case PartyMembers.ID.Paula:
			return new ID[4]
			{
				ID.PSILifeup,
				ID.PSIShield,
				ID.PKFreeze,
				ID.PKFire
			};
		case PartyMembers.ID.Sans:
			return new ID[1] { ID.MiniACT };
		default:
			return new ID[0];
		}
	}

	public static ID[] GetSpellListWithoutACT(int partyMember)
	{
		List<ID> list = new List<ID>(GetSpellList(partyMember));
		list.Remove(ID.ACT);
		return list.ToArray();
	}

	public static bool HasACTAbility(int partyMember)
	{
		if (partyMember == -1)
		{
			return false;
		}
		return GetSpellList(partyMember)[0] == ID.ACT;
	}

	public static string GetNameOfMagicMenu(int partyMember)
	{
		return partyMember switch
		{
			0 => "Abilities", 
			3 => "PSI", 
			_ => "Magic", 
		};
	}

	public static bool CanCastSpell(ID spellID, int partyMember, int tp)
	{
		if (tp < GetSpell(spellID).GetTPCost())
		{
			return false;
		}
		if (Items.GetItemElement(PartyMembers.GetWeapon(partyMember)) != 1 && (spellID == ID.SleepMist || spellID == ID.IceShock))
		{
			return false;
		}
		return true;
	}
}
