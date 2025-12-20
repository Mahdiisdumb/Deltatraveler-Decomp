using UnityEngine;

public class UndyneSpearSpawner : MonoBehaviour
{
	private int alarm = 10;

	private float impact;

	private int dif;

	private GameObject bonePrefab;

	private bool activated;

	private void Awake()
	{
		bonePrefab = Resources.Load<GameObject>("overworld/bullets/UndyneSpearBullet");
		dif = Util.GameManager().GetSessionFlagInt(18);
		if (dif > 0)
		{
			activated = true;
		}
	}

	private void Start()
	{
		if (activated && Util.GameManager().GetHP(0) < Util.GameManager().GetMaxHP(0))
		{
			Object.FindObjectOfType<ActionPartyPanels>().Raise();
			Object.FindObjectOfType<ActionPartyPanels>().UpdateHP(Util.GameManager().GetHPArray());
		}
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		alarm--;
		if (alarm > 0)
		{
			return;
		}
		if (dif < 35)
		{
			impact += 0.5f;
			if (dif > 13)
			{
				impact += 0.2f;
			}
			int num = (int)impact;
			impact -= num;
			dif += num;
			if (dif > 35)
			{
				dif = 35;
			}
			Util.GameManager().SetSessionFlag(18, dif);
		}
		alarm = 48 - dif;
		GetComponent<AudioSource>().Play();
		for (int i = 0; i < 3; i++)
		{
			float num2 = Random.Range(-2.5f, 2.5f);
			bool num3 = Random.Range(0, 2) == 0;
			bool flag = Random.Range(0, 2) == 0;
			Vector3 vector = Vector3.zero;
			vector = ((!num3) ? new Vector3(flag ? (-3.5f) : 3.5f, num2) : new Vector3(num2, flag ? (-3.5f) : 3.5f));
			OverworldBoneBullet component = Object.Instantiate(bonePrefab, Object.FindObjectOfType<OverworldPlayer>().transform.position + vector, Quaternion.identity, GameObject.Find("OBJ").transform).GetComponent<OverworldBoneBullet>();
			component.StartSpinning();
			component.StartTracking(vector);
			component.StartMoving(i == 0);
		}
	}

	public void Activate()
	{
		activated = true;
		if (dif < 1)
		{
			dif = 1;
			Util.GameManager().SetSessionFlag(18, dif);
		}
	}
}
