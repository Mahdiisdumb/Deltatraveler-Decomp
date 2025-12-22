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
		OverworldPlayer overworldPlayer = Util.OverworldPlayer();
		BattleManager battleManager = Util.FindObjectOfType<BattleManager>();
		SOUL sOUL = null;
		SOUL[] array = Util.FindObjectsOfType<SOUL>();
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
			text = "SOUL POS: " + sOUL.transform.position.ToString() + "\nSOUL MODE: " + sOUL.GetSOULMode() + "\nSOUL CANMOVE: " + sOUL.IsControllable() + "\nINV FRAMES: " + sOUL.GetInvFrames() + "\nSOUL GRABBED: " + sOUL.IsGrabbed();
			EnemyBase[] array2 = Util.FindObjectsOfType<EnemyBase>();
			foreach (EnemyBase enemyBase in array2)
			{
				text = text + "\n" + enemyBase.GetName() + " HP: " + enemyBase.GetHP() + " / " + enemyBase.GetMaxHP() + "\n" + enemyBase.GetName() + " BUFFS (ATK/DEF): " + enemyBase.GetBuff(0) + " / " + enemyBase.GetBuff(1);
			}
		}
		else if ((bool)overworldPlayer)
		{
			text = "PLAYER POS: " + overworldPlayer.transform.position.ToString() + "\nPLAYER SORT: " + overworldPlayer.GetComponent<SpriteRenderer>().sortingOrder + "\nPLAYER CANMOVE: " + overworldPlayer.CanMove() + "\nNOCLIP: " + overworldPlayer.GetNoclip();
		}
		text = text + "\nSCENE: " + SceneManager.GetActiveScene().buildIndex + " - " + SceneManager.GetActiveScene().name;
		text = text + "\nREFRESH RATE: " + Util.GameManager().GetRefreshRate();
		base.transform.GetChild(0).GetComponent<Text>().text = text;
		base.transform.GetChild(1).GetComponent<Text>().text = text;
	}
}
