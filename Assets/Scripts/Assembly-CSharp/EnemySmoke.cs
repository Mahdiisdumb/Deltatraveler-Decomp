using UnityEngine;

public class EnemySmoke : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private int frames;

	private float rotation;

	private float velocity;

	private void Awake()
	{
		rotation = Random.Range(-8f, 8f);
	}

	private void Update()
	{
		frames++;
		if (velocity < 1f)
		{
			velocity += 0.05f;
		}
		base.transform.position += new Vector3(-1f / 12f, 1f / 12f) * velocity;
		base.transform.eulerAngles += new Vector3(0f, 0f, rotation);
		base.transform.localScale += new Vector3(0.02f, 0.02f);
		GetComponent<SpriteRenderer>().color = Color.Lerp(Color.white, new Color(1f, 1f, 1f, 0f), (float)(frames - 30) / 40f);
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 5 % 2];
		if (frames >= 70)
		{
			Object.Destroy(base.gameObject);
		}
	}
}
