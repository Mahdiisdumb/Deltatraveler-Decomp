using UnityEngine;

public class DRBattleManager : MonoBehaviour
{
	public enum States
	{
		None = -1,
		Intro = 0,
		PlayerTurn = 1,
		EnemyStart = 2,
		EnemyAction = 3,
		EnemyEnd = 4
	}
}
