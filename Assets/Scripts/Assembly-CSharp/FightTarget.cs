using UnityEngine;

public class FightTarget : MonoBehaviour
{
	private EnemyBase[] enemies = new EnemyBase[3];

	private FightTargetBar[] bars;

	private bool[] attacking = new bool[4] { true, false, false, false };

	private int startFrames;

	private int endFrames;

	private bool done;

	private int miniPartyMember;

	private int partyMembers;

	private int deviousType = -1;

	private void Awake()
	{
		bars = new FightTargetBar[4];
		miniPartyMember = Object.FindObjectOfType<GameManager>().GetMiniPartyMember();
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(94) == 1)
		{
			base.transform.Find("Target").GetComponent<SpriteRenderer>().sprite = Resources.Load<Sprite>("battle/spr_target_1_ts");
		}
		if (Object.FindObjectOfType<BattleManager>().IsSusieDevious())
		{
			deviousType = Random.Range(0, 5);
			MonoBehaviour.print("SUSIE DEVIOUS ATTACK: " + deviousType);
			if (deviousType == 4 && Util.GameManager().GetWeapon(1) == -1)
			{
				deviousType = 1;
			}
		}
		for (int i = 0; i < 4; i++)
		{
			if (i < 3)
			{
				if (Items.GetWeaponType(Object.FindObjectOfType<GameManager>().GetWeapon(i)) == 2)
				{
					bars[i] = Object.Instantiate(Resources.Load<GameObject>("battle/reticles/Quad"), base.transform).GetComponent<FightTargetBar>();
				}
				else if (Items.GetWeaponType(Object.FindObjectOfType<GameManager>().GetWeapon(i)) == 3)
				{
					bars[i] = Object.Instantiate(Resources.Load<GameObject>("battle/reticles/Bash"), base.transform).GetComponent<FightTargetBar>();
				}
				else if (Items.GetWeaponType(Object.FindObjectOfType<GameManager>().GetWeapon(i)) == 5)
				{
					bars[i] = Object.Instantiate(Resources.Load<GameObject>("battle/reticles/Katana"), base.transform).GetComponent<FightTargetBar>();
				}
				else
				{
					bars[i] = Object.Instantiate(Resources.Load<GameObject>("battle/reticles/Standard"), base.transform).GetComponent<FightTargetBar>();
				}
			}
			else if (miniPartyMember == 1)
			{
				bars[i] = Object.Instantiate(Resources.Load<GameObject>("battle/reticles/Quad"), base.transform).GetComponent<FightTargetBar>();
			}
			else
			{
				bars[i] = Object.Instantiate(Resources.Load<GameObject>("battle/reticles/Standard"), base.transform).GetComponent<FightTargetBar>();
			}
			SpriteRenderer[] componentsInChildren = bars[i].GetComponentsInChildren<SpriteRenderer>();
			foreach (SpriteRenderer spriteRenderer in componentsInChildren)
			{
				if (i < 3)
				{
					spriteRenderer.color = PartyPanels.defaultColors[i];
				}
				else if (miniPartyMember > 0)
				{
					spriteRenderer.color = PartyPanels.defaultColors[miniPartyMember + 2];
				}
			}
		}
	}

	private void Update()
	{
		bool flag = false;
		for (int i = 0; i < 3; i++)
		{
			if ((bool)enemies[i] && enemies[i].IsShaking())
			{
				flag = true;
			}
		}
		if (!done && !flag && Object.FindObjectsOfType<PlayerAttackAnimation>().Length == 0 && startFrames >= 5)
		{
			bool flag2 = false;
			FightTargetBar[] array = bars;
			for (int j = 0; j < array.Length; j++)
			{
				if (!array[j].IsCompleted())
				{
					flag2 = true;
				}
			}
			if (!flag2)
			{
				done = true;
			}
		}
		if (startFrames < 5)
		{
			startFrames++;
			if (startFrames != 5)
			{
				return;
			}
			int num = 50;
			for (int k = 0; k < 4; k++)
			{
				if (k < 3)
				{
					if (attacking[k])
					{
						if (partyMembers == 1)
						{
							bars[k].Activate(12);
						}
						else
						{
							bars[k].Activate();
						}
						if (bars[k].GetLastFrames() < num)
						{
							num = bars[k].GetLastFrames();
						}
					}
				}
				else if (k == 3 && miniPartyMember > 0 && attacking[k])
				{
					if (attacking[0])
					{
						bars[k].Activate(num - Random.Range(4, 6) * 2);
					}
					else
					{
						bars[k].Activate();
					}
				}
			}
			return;
		}
		bool num2 = UTInput.GetButton("C") && Object.FindObjectOfType<GameManager>().IsTestMode();
		bool flag3 = false;
		if (num2)
		{
			for (int l = 0; l < 4; l++)
			{
				if (attacking[l] && bars[l].GetCurFrames() == 40 && !bars[l].IsCompleted())
				{
					flag3 = true;
				}
			}
		}
		if (UTInput.GetButtonDown("Z") || flag3)
		{
			int num3 = 0;
			FightTargetBar[] array = bars;
			foreach (FightTargetBar fightTargetBar in array)
			{
				if (fightTargetBar.GetCurFrames() > num3 && fightTargetBar.CanPushZ())
				{
					num3 = fightTargetBar.GetCurFrames();
				}
			}
			for (int m = 0; m < 4; m++)
			{
				int num4 = m;
				int num5 = m;
				if (m == 3)
				{
					num4 = 0;
					num5 = miniPartyMember + 2;
				}
				if (!attacking[m] || !bars[m].CanPushZ() || bars[m].GetCurFrames() != num3)
				{
					continue;
				}
				bool flag4 = true;
				if (enemies[num4].GetPredictedHP() <= 0)
				{
					flag4 = false;
					EnemyBase[] array2 = Object.FindObjectOfType<BattleManager>().GetEnemies();
					foreach (EnemyBase enemyBase in array2)
					{
						if (enemyBase.GetPredictedHP() > 0)
						{
							bars[m].AssignValues(enemyBase, num5);
							enemies[num4] = enemyBase;
							flag4 = true;
							break;
						}
					}
				}
				MonoBehaviour.print("Member" + num5 + " Enemy" + num4 + " PredHP" + enemies[num4].GetPredictedHP() + " CanHit" + flag4.ToString());
				bool flag5 = bars[m].PushZ(flag4);
				if (flag4 && flag5)
				{
					PlayHitAnimation(enemies[num4], num5, bars[m].GetSuccessRate(), m);
				}
			}
		}
		if (done)
		{
			endFrames++;
			base.transform.Find("Target").localScale = Vector2.Lerp(new Vector2(1f, 1f), new Vector2(0.278125f, 1f), (float)endFrames / 11f);
			base.transform.Find("Target").GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 1f - (float)endFrames / 11f);
			if (endFrames == 11)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public void PlayHitAnimation(EnemyBase enemy, int partyMember, float successRate, int barIndex)
	{
		if (partyMember != 1 || deviousType != 1)
		{
			PlayerAttackAnimation playerAttackAnimation = ((deviousType == 4 && partyMember == 1) ? Object.Instantiate(Resources.Load<GameObject>("battle/SillyStick")).GetComponent<SillyStickAttack>() : ((Items.GetWeaponType(Object.FindObjectOfType<GameManager>().GetWeapon(partyMember)) == 1) ? Object.Instantiate(Resources.Load<GameObject>("battle/Smack")).GetComponent<PlayerAttackAnimation>() : ((Items.GetWeaponType(Object.FindObjectOfType<GameManager>().GetWeapon(partyMember)) == 3) ? Object.Instantiate(Resources.Load<GameObject>("battle/Bash")).GetComponent<PlayerAttackAnimation>() : ((Object.FindObjectOfType<GameManager>().GetWeapon(partyMember) == 20) ? Object.Instantiate(Resources.Load<GameObject>("battle/PanAttack")).GetComponent<PanAttackAnimation>() : ((Util.GameManager().GetWeapon(partyMember) == 32 || (partyMember != 2 && Util.GameManager().GetWeapon(partyMember) == -1)) ? Object.Instantiate(Resources.Load<GameObject>("battle/BigFist")).GetComponent<BigFistAttack>() : ((Util.GameManager().GetWeapon(partyMember) != 41) ? Object.Instantiate(Resources.Load<GameObject>("battle/Slice")).GetComponent<PlayerAttackAnimation>() : Object.Instantiate(Resources.Load<GameObject>("battle/KSlice")).GetComponent<KSliceAnimation>()))))));
			playerAttackAnimation.AssignValues(enemy, partyMember, successRate, Object.FindObjectOfType<PartyPanels>().NumOfActivePartyMembers());
			if (partyMember == 1 && (deviousType == 2 || deviousType == 3))
			{
				playerAttackAnimation.transform.position = new Vector3(Object.FindObjectOfType<PartyPanels>().transform.Find((deviousType == 2) ? "KrisSprite" : "SusieSprite").localPosition.x / 48f, 0f);
			}
		}
	}

	public void PlayMiniHitAnimation(EnemyBase enemy, int partyMember)
	{
		PlayerAttackAnimation playerAttackAnimation = null;
		if (Util.GameManager().GetWeapon(partyMember) == 32)
		{
			playerAttackAnimation = Object.Instantiate(Resources.Load<GameObject>("battle/SmallFist")).GetComponent<SmallFistAttack>();
		}
		if (playerAttackAnimation != null)
		{
			playerAttackAnimation.AssignValues(enemy, partyMember, 1f, Object.FindObjectOfType<PartyPanels>().NumOfActivePartyMembers());
		}
	}

	public void SetEnemies(EnemyBase krisTarget, EnemyBase susieTarget, EnemyBase noelleTarget)
	{
		enemies = new EnemyBase[3] { krisTarget, susieTarget, noelleTarget };
	}

	public void SetAttackers(bool kris, bool susie, bool noelle, int partyMembers, int specialMiniState)
	{
		attacking = new bool[4]
		{
			kris && Object.FindObjectOfType<GameManager>().KrisInControl(),
			susie,
			noelle,
			kris && miniPartyMember > 0
		};
		if (specialMiniState == 1)
		{
			attacking[3] = false;
		}
		if (specialMiniState == 2)
		{
			attacking[0] = false;
		}
		if (partyMembers == 1)
		{
			this.partyMembers = partyMembers;
			if (attacking[0])
			{
				if (!attacking[3])
				{
					SpriteRenderer[] componentsInChildren = bars[0].GetComponentsInChildren<SpriteRenderer>();
					for (int i = 0; i < componentsInChildren.Length; i++)
					{
						componentsInChildren[i].color = Color.white;
					}
				}
				bars[0].AssignValues(enemies[0], 0);
			}
			if (attacking[3])
			{
				bars[3].AssignValues(enemies[0], miniPartyMember + 2);
			}
			return;
		}
		for (int j = 0; j < 4; j++)
		{
			if (j < 3 && attacking[j])
			{
				this.partyMembers++;
			}
			if (j == 0 && (int)Object.FindObjectOfType<GameManager>().GetFlag(107) == 1 && (int)Object.FindObjectOfType<GameManager>().GetFlag(108) == 1)
			{
				SpriteRenderer[] componentsInChildren = bars[0].GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].color = UIBackground.borderColors[(int)Util.GameManager().GetFlag(223)];
				}
			}
			bars[j].transform.localScale = new Vector3(1f, (partyMembers == 2) ? 0.5f : (1f / 3f), 1f);
			float num = ((partyMembers == 2) ? 0.66f : 0.88f);
			if (j > 0)
			{
				num *= -1f;
			}
			if (partyMembers != 3 || j != 1)
			{
				bars[j].transform.localPosition = new Vector3(bars[j].transform.localPosition.x, num);
			}
			if (j < 3)
			{
				if (!attacking[j])
				{
					continue;
				}
				bars[j].AssignValues(enemies[j], j);
				if (j == 1)
				{
					if (deviousType == 1)
					{
						bars[j].SetScoreMultiplier(0f);
					}
					else if (deviousType == 2 || deviousType == 3)
					{
						bars[j].SetDeviousMode(deviousType == 2);
					}
					else if (deviousType == 4)
					{
						bars[j].SetScoreMultiplier(0.25f);
					}
				}
			}
			else
			{
				bars[j].transform.localPosition = new Vector3(bars[j].transform.localPosition.x, bars[0].transform.localPosition.y);
				if (attacking[j])
				{
					bars[j].AssignValues(enemies[0], miniPartyMember + 2);
				}
			}
		}
	}

	public int GetNumAttackingMembers()
	{
		int num = 0;
		for (int i = 0; i < 4; i++)
		{
			if (attacking[i])
			{
				num++;
			}
		}
		if (attacking[0] && attacking[3])
		{
			num--;
		}
		return num;
	}

	public bool[] GetAttackingMembers()
	{
		return attacking;
	}

	public EnemyBase[] GetEnemies()
	{
		return enemies;
	}

	public bool IsGoing()
	{
		return !done;
	}
}
