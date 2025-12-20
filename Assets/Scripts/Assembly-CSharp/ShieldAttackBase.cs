using System.Collections.Generic;
using UnityEngine;

public class ShieldAttackBase : AttackBase
{
	protected object[][] bulletInfo;

	protected List<SpearArrowBullet> spearBullets = new List<SpearArrowBullet>();

	protected List<GasterBlaster> gasterBlasters = new List<GasterBlaster>();

	protected int spawnRate;

	protected int spawnPerRate;

	protected override void Awake()
	{
		base.Awake();
		bbSize = new Vector2(85f, 140f);
		bbPos = new Vector2(0.05f, -1.66f);
		Object.FindObjectOfType<SOUL>().ChangeSOULMode(2);
		soulPos = new Vector3(0f, -1.63f);
		spawnRate = 10;
		spawnPerRate = 1;
		bb.transform.Find("Background").GetComponent<SpriteMask>().enabled = false;
	}

	protected virtual void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<SOULShield>())
		{
			Object.FindObjectOfType<SOULShield>().SetToDestroy();
		}
		if ((bool)Object.FindObjectOfType<BattleManager>())
		{
			try
			{
				Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().sortingOrder = 199;
				Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeIn(8, new Color32(0, 0, 0, 132));
				bb.transform.Find("Background").GetComponent<SpriteMask>().enabled = true;
				Object.FindObjectOfType<PartyPanels>().GetComponent<Canvas>().sortingOrder = 99;
				GameObject.Find("BattleCanvas").GetComponent<Canvas>().sortingOrder = 400;
			}
			catch
			{
				Debug.Log("lol you died on the green attack");
			}
		}
	}

	protected override void Update()
	{
		if (!isStarted)
		{
			return;
		}
		if (frames < 3)
		{
			Object.FindObjectOfType<SOUL>().transform.position = Vector3.Lerp(soulPos, new Vector3(0f, -0.042f), (float)(frames + 1) / 3f);
		}
		int num = Mathf.FloorToInt((float)frames / (float)spawnRate) * spawnPerRate;
		for (int i = num; i < num + spawnPerRate; i++)
		{
			if (i >= bulletInfo.Length || frames % spawnRate != 0 || bulletInfo[i] == null)
			{
				continue;
			}
			if ((int)bulletInfo[i][0] == 0 || (int)bulletInfo[i][0] == 1)
			{
				SpearArrowBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/SpearArrowBullet")).GetComponent<SpearArrowBullet>();
				component.Activate((int)bulletInfo[i][1], (Vector3)bulletInfo[i][2], !ContainsSpearBullets(), this, (Vector3)bulletInfo[i][3], (int)bulletInfo[i][0] == 1);
				spearBullets.Add(component);
			}
			if ((int)bulletInfo[i][0] == 2)
			{
				GasterBlaster component2 = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/GasterBlaster")).GetComponent<GasterBlaster>();
				Vector3 vector = (Vector3)bulletInfo[i][2];
				float num2 = ((!(vector == Vector3.down)) ? 180 : 0);
				if (vector == Vector3.left || vector == Vector3.right)
				{
					num2 = 90f * vector.x;
				}
				component2.transform.rotation = Quaternion.Euler(0f, 0f, num2 - 180f);
				component2.transform.position = new Vector3(0f, -0.042f) - vector * 10f;
				component2.Activate((int)bulletInfo[i][4], (int)bulletInfo[i][5], num2, new Vector2(0f, -0.042f) - (Vector2)vector * 3f + (Vector2)(Vector3)bulletInfo[i][3], (int)bulletInfo[i][1], true);
				gasterBlasters.Add(component2);
			}
		}
		if (num >= bulletInfo.Length && !ContainsSpearBullets() && !ContainsBlasters())
		{
			Object.Destroy(base.gameObject);
		}
		frames++;
	}

	public override void StartAttack()
	{
		base.StartAttack();
		Object.FindObjectOfType<SOUL>().GetComponent<SpriteRenderer>().sortingOrder = 402;
		Object.FindObjectOfType<PartyPanels>().GetComponent<Canvas>().sortingOrder = 402;
		GameObject.Find("BattleCanvas").GetComponent<Canvas>().sortingOrder = 402;
		Object.Instantiate(Resources.Load<GameObject>("battle/attacks/SOULShield"));
		bb.StartMovement(new Vector2(85f, 85f), new Vector2(0.05f, 0.525f));
		Object.FindObjectOfType<BattleManager>().GetBattleFade().FadeOut(8, new Color32(0, 0, 0, 132));
	}

	public void SetNewLeadBullet(GameObject prevGameObject)
	{
		for (int i = 0; i < spearBullets.Count; i++)
		{
			if (spearBullets[i] != null && !(spearBullets[i].gameObject == prevGameObject))
			{
				spearBullets[i].SetAsLead();
				break;
			}
		}
	}

	public bool ContainsSpearBullets()
	{
		if (spearBullets.Count == 0)
		{
			return false;
		}
		foreach (SpearArrowBullet spearBullet in spearBullets)
		{
			if (spearBullet != null)
			{
				return true;
			}
		}
		return false;
	}

	public bool ContainsBlasters()
	{
		if (gasterBlasters.Count == 0)
		{
			return false;
		}
		foreach (GasterBlaster gasterBlaster in gasterBlasters)
		{
			if (gasterBlaster != null && gasterBlaster.IsBlasting())
			{
				return true;
			}
		}
		return false;
	}
}
