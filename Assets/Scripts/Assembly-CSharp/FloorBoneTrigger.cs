using UnityEngine;

public class FloorBoneTrigger : MonoBehaviour
{
	[SerializeField]
	private Vector2 direction = Vector2.zero;

	[SerializeField]
	private Vector3 origin = Vector3.zero;

	[SerializeField]
	private GameObject objectToSpawn;

	[SerializeField]
	private GameObject[] objectToDespawn;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!collision.GetComponent<OverworldPlayer>())
		{
			return;
		}
		Object.Instantiate(objectToSpawn, origin + (Vector3)(direction * (collision.GetComponent<OverworldPlayer>().GetSpeed() * 18f) / 48f), Quaternion.identity, base.transform.parent);
		if (objectToDespawn.Length != 0)
		{
			for (int i = 0; i < objectToDespawn.Length; i++)
			{
				if ((bool)objectToDespawn[i])
				{
					Object.Destroy(objectToDespawn[i]);
				}
			}
		}
		Object.Destroy(base.gameObject);
	}
}
