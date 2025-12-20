using UnityEngine;

public class EnemyExplosion : MonoBehaviour
{
	private int frames;

	private void Update()
	{
		frames++;
		if (frames % 4 >= 2)
		{
			base.transform.localScale -= new Vector3(0.05f, 0.05f);
		}
		else
		{
			base.transform.localScale += new Vector3(0.1f, 0.1f);
		}
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)(frames - 10) / 60f);
		if (frames >= 70 && !GetComponent<AudioSource>().isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
