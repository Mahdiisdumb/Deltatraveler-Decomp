using UnityEngine;

public class CreepyLadyDetectionRange : MonoBehaviour
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			GetComponentInParent<CreepyLady>().DetectPlayer();
			Object.Destroy(base.gameObject);
		}
	}
}
