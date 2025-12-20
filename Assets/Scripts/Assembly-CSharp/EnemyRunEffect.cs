using UnityEngine;

public class EnemyRunEffect : MonoBehaviour
{
	private int frames;

	private void Update()
	{
		frames++;
		for (int i = 0; i < base.transform.childCount; i++)
		{
			base.transform.GetChild(i).GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), ((float)frames - (float)i / 3f) / 5f);
		}
	}
}
