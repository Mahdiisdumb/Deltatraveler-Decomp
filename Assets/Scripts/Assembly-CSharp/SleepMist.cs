using UnityEngine;

public class SleepMist : MonoBehaviour
{
	private bool isPlaying = true;

	private int frames;

	private void Update()
	{
		if (isPlaying)
		{
			frames++;
			for (int i = 0; i < 4; i++)
			{
				float num = frames - i * 5;
				float t = num / 10f;
				if (num > 15f)
				{
					t = 1f - (num - 15f) / 10f;
				}
				if (i % 2 == 0)
				{
					base.transform.GetChild(0).GetChild(i).localPosition += new Vector3(1f / 48f, 0f);
				}
				else
				{
					base.transform.GetChild(0).GetChild(i).localPosition -= new Vector3(1f / 48f, 0f);
				}
				base.transform.GetChild(0).GetChild(i).GetComponent<SpriteRenderer>()
					.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, t);
			}
			for (int j = 0; j < 6; j++)
			{
				float num2 = frames - j * 4;
				float t2 = num2 / 10f;
				if (num2 > 10f)
				{
					t2 = 1f - (num2 - 10f) / 10f;
				}
				base.transform.GetChild(1).GetChild(j).GetComponent<SpriteRenderer>()
					.color = Color.Lerp(new Color(1f, 1f, 1f, 0f), Color.white, t2);
			}
			if (frames == 60)
			{
				isPlaying = false;
			}
		}
		if (!isPlaying && !GetComponent<AudioSource>().isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
