using UnityEngine;

public class ElectricMazeWall : MonoBehaviour
{
	public void OnCollisionEnter2D(Collision2D collision)
	{
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>())
		{
			Object.FindObjectOfType<ElectricMazeHandler>().WallCollision();
		}
	}

	public void OnCollisionStay2D(Collision2D collision)
	{
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>())
		{
			Object.FindObjectOfType<ElectricMazeHandler>().WallCollision();
		}
	}
}
