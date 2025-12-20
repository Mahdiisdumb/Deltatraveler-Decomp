using UnityEngine;

public class PorkyLaunch : MonoBehaviour
{
	[SerializeField]
	private Sprite[] otherSprites;

	private int frames;

	private void Update()
	{
		frames++;
		if (frames < 15)
		{
			base.transform.position = Vector3.Lerp(new Vector3(0f, 2.43f), new Vector3(-2.31f, 7.39f), (float)frames / 7f);
			if (frames == 10)
			{
				GetComponent<SpriteRenderer>().enabled = false;
			}
			return;
		}
		if (frames < 90)
		{
			if (frames == 45)
			{
				GetComponent<SpriteRenderer>().sprite = otherSprites[0];
				GetComponent<SpriteRenderer>().enabled = true;
			}
			base.transform.position = Vector3.Lerp(new Vector3(-1.45f, 5.27f), new Vector3(-1.12f, 4.2f), (float)(frames - 45) / 45f);
			return;
		}
		int num = (frames - 90) / 3 + 1;
		if (num >= otherSprites.Length)
		{
			num = otherSprites.Length - 1;
		}
		GetComponent<SpriteRenderer>().sprite = otherSprites[num];
		if (frames == 90)
		{
			GetComponent<AudioSource>().Play();
		}
	}
}
