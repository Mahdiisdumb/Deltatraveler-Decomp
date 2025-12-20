using UnityEngine;

public class SpikedFloorSoft : MonoBehaviour
{
	[SerializeField]
	private Sprite[] spikedGraphics;

	private bool krisOnSpikes;

	private bool susieOnSpikes;

	private void Update()
	{
		GetComponent<SpriteRenderer>().sprite = spikedGraphics[(krisOnSpikes || susieOnSpikes) ? 1 : 0];
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		MonoBehaviour.print("GOT DAMN");
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			krisOnSpikes = true;
		}
		if ((bool)collision.GetComponent<OverworldPartyMember>())
		{
			susieOnSpikes = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			krisOnSpikes = false;
		}
		if ((bool)collision.GetComponent<OverworldPartyMember>())
		{
			susieOnSpikes = false;
		}
	}
}
