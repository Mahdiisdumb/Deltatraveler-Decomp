using UnityEngine;

public class RalseiDoobiePRVTroll : MonoBehaviour
{
	private void Awake()
	{
		if (Object.FindObjectOfType<GameManager>().GetMiniPartyMember() == 1)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			if ((bool)Object.FindObjectOfType<RalseiSmokinAFatOne>() && Object.FindObjectOfType<GameManager>().GetMiniPartyMember() != 1)
			{
				Object.FindObjectOfType<RalseiSmokinAFatOne>().transform.position = new Vector3(89.96f, 47.71f);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
