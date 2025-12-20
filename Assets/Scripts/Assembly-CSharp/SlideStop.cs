using UnityEngine;

public class SlideStop : OverworldManipulator
{
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && collision.GetComponent<OverworldPlayer>().IsSliding())
		{
			collision.GetComponent<OverworldPlayer>().StopSliding();
		}
	}
}
