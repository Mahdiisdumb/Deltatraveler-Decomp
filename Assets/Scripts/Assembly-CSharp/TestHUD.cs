using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class TestHUD : MonoBehaviour
{
	private void Awake()
	{
		Object.DontDestroyOnLoad(base.gameObject);
	}

	private void Update()
	{
		OverworldPlayer overworldPlayer = Object.FindObjectOfType<OverworldPlayer>();
		BattleManager battleManager = Object.FindObjectOfType<BattleManager>();
		SOUL sOUL = null;
		SOUL[] array = Object.FindObjectsOfType<SOUL>();
		foreach (SOUL sOUL2 in array)
		{
			if (sOUL2.IsPlayer())
			{
				sOUL = sOUL2;
				break;
			}
		}
		string text = "NO PLAYER FOUND";
		if ((bool)battleManager && (bool)sOUL)
		{
			text = string.Concat("SOUL POS: ", sOUL.transform.position, "\nSOUL MODE: ", sOUL.GetSOULMode(), "\nSOUL CANMOVE: ", sOUL.IsControllable().ToString(), "\nINV FRAMES: ", sOUL.GetInvFrames(), "\nSOUL GRABBED: ", sOUL.IsGrabbed().ToString());
			EnemyBase[] array2 = Object.FindObjectsOfType<EnemyBase>();
			foreach (EnemyBase enemyBase in array2)
			{
				text = text + "\n" + enemyBase.GetName() + " HP: " + enemyBase.GetHP() + " / " + enemyBase.GetMaxHP() + "\n" + enemyBase.GetName() + " BUFFS (ATK/DEF): " + enemyBase.GetBuff(0) + " / " + enemyBase.GetBuff(1);
			}
		}
		else if ((bool)overworldPlayer)
		{
			text = string.Concat("PLAYER POS: ", overworldPlayer.transform.position, "\nPLAYER SORT: ", overworldPlayer.GetComponent<SpriteRenderer>().sortingOrder, "\nPLAYER CANMOVE: ", overworldPlayer.CanMove().ToString(), "\nNOCLIP: ", overworldPlayer.GetNoclip().ToString());
		}
		text = text + "\nSCENE: " + SceneManager.GetActiveScene().buildIndex + " - " + SceneManager.GetActiveScene().name;
		base.transform.GetChild(0).GetComponent<Text>().text = text;
		base.transform.GetChild(1).GetComponent<Text>().text = text;
	}
}
