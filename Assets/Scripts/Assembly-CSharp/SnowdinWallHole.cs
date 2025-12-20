using UnityEngine;

public class SnowdinWallHole : MonoBehaviour
{
	private int frames;

	[SerializeField]
	private Sprite[] eyeSprites;

	private SpriteRenderer eyes;

	private void Awake()
	{
		eyes = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		frames = Random.Range(0, 20);
		if (GetComponent<SpriteRenderer>().flipX)
		{
			frames += 310;
			eyes.flipX = true;
		}
	}

	private void Update()
	{
		frames++;
		eyes.transform.localPosition = new Vector3(Mathf.Sin((float)frames / 7f), Mathf.Cos((float)frames / 8f)) / 24f;
		int num = (int)Mathf.Abs(Mathf.Sin((float)frames / 15f) * 2.8f);
		if (num > 2)
		{
			num = 2;
		}
		eyes.sprite = eyeSprites[num];
		float num2 = 0f;
		num2 = ((frames > 150) ? Mathf.Lerp(1f, 0f, (float)(frames - 300) / 50f) : Mathf.Lerp(0f, 1f, (float)(frames - 100) / 50f));
		eyes.color = new Color(1f, 1f, 1f, num2);
		if (frames >= 350)
		{
			frames = 0;
		}
	}
}
