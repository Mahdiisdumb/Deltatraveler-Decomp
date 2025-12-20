using UnityEngine;

public class OverworldBloodEnemyBase : OverworldEnemyBase
{
	[SerializeField]
	protected GameObject deadPrefab;

	protected GameObject dead;

	public virtual void CreateDeadEnemy(bool age = false)
	{
		dead = Object.Instantiate(deadPrefab, base.transform.position, Quaternion.identity);
		dead.transform.parent = base.transform.parent;
	}
}
