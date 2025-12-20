using UnityEngine;

public class ScreenBoundaries : MonoBehaviour
{
	private bool activated;

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>() && !activated)
		{
			activated = true;
			GetComponent<AudioSource>().Play();
			GetComponent<Animator>().enabled = true;
		}
	}
}
