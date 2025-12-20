using UnityEngine;

public class BigShot : MonoBehaviour
{
	private int frames;

	private float velocity = 8f;

	private void Update()
	{
		base.transform.position += new Vector3(0f, velocity / 48f);
		velocity += 0.2f;
		base.transform.localScale = Vector3.Lerp(new Vector3(2f, 0.1f, 1f), new Vector3(1f, 1f, 1f), (float)frames / 6f);
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, Mathf.Lerp(0.5f, 1f, (float)frames / 4f));
		frames++;
		if (base.transform.position.y > 5f && !GetComponent<AudioSource>().isPlaying)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<BulletBase>() && collision.GetComponent<BulletBase>().CanGetShot())
		{
			collision.GetComponent<BulletBase>().GetShot(true);
		}
	}
}
