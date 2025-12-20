using UnityEngine;

public class DogHeartSpawner : MonoBehaviour
{
	private int frames;

	[SerializeField]
	private Sprite[] sprites;

	[SerializeField]
	private Sprite[] sadSprites;

	private bool spawner;

	private bool sad;

	private bool alternate;

	private void Awake()
	{
		sad = Object.FindObjectOfType<Dogaressa>().IsKilled();
		frames = ((!sad) ? 65 : 0);
		if (sad)
		{
			GetComponent<SpriteRenderer>().sprite = sadSprites[0];
		}
	}

	private void Update()
	{
		if (sad)
		{
			frames++;
			if (frames == 60)
			{
				Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DogiHeartBullet"), base.transform.parent);
				GetComponent<SpriteRenderer>().sprite = sadSprites[1];
			}
			else if (frames == 90)
			{
				GetComponent<SpriteRenderer>().sprite = sadSprites[0];
			}
			return;
		}
		frames = (frames + 1) % 75;
		if (frames <= 12)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[2];
		}
		else
		{
			GetComponent<SpriteRenderer>().sprite = sprites[(frames - 12) / 5 % 2];
		}
		if (frames != 4 || !spawner)
		{
			return;
		}
		for (int i = 0; i < 8; i++)
		{
			float degree = 45 * i;
			DogiHeartBullet component = Object.Instantiate(Resources.Load<GameObject>("battle/attacks/bullets/snowdin/DogiHeartBullet"), base.transform.parent).GetComponent<DogiHeartBullet>();
			component.SetDegree(degree);
			bool flag = i % 2 == 1;
			if (alternate)
			{
				flag = !flag;
			}
			component.ChangeType(flag ? 1 : 0);
		}
	}

	public void SetAsSpawner()
	{
		spawner = true;
		GetComponent<SpriteRenderer>().flipX = true;
	}
}
