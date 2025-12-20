using UnityEngine;

public class RalseiDoobiePRVTroll : MonoBehaviour
{
	private void Awake()
	{
		if (Util.GameManager().GetPartyMember(3) == 3)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision && (bool)collision.GetComponent<OverworldPlayer>())
		{
			if ((bool)Util.FindObjectOfType<RalseiSmokinAFatOne>() && Util.GameManager().GetPartyMember(3) != 3)
			{
				Util.FindObjectOfType<RalseiSmokinAFatOne>().transform.position = new Vector3(89.96f, 47.71f);
			}
			Object.Destroy(base.gameObject);
		}
	}
}
