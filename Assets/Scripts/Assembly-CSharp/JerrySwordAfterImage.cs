using UnityEngine;

public class JerrySwordAfterImage : MonoBehaviour
{
	private void Update()
	{
		Color color = GetComponent<SpriteRenderer>().color;
		color.a -= 0.05f;
		if (color.a <= 0f)
		{
			Object.Destroy(base.gameObject);
		}
		else
		{
			GetComponent<SpriteRenderer>().color = color;
		}
	}
}
