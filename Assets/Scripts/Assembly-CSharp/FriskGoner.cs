using UnityEngine;

public class FriskGoner : MonoBehaviour
{
	private bool vanish;

	private float vanishFloat;

	private void Update()
	{
		if (vanish && vanishFloat > 0f)
		{
			vanishFloat -= 0.1f;
		}
		else if (!vanish && vanishFloat < 1f)
		{
			vanishFloat += 0.1f;
		}
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, vanishFloat);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			vanish = true;
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<OverworldPlayer>())
		{
			vanish = false;
		}
	}
}
