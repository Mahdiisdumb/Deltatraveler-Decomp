using System.Collections.Generic;
using UnityEngine;

public class WaterfallKelp : MonoBehaviour
{
	[SerializeField]
	private Sprite[] sprites;

	private List<GameObject> objects = new List<GameObject>();

	private void Awake()
	{
		base.transform.position = new Vector3((float)Mathf.RoundToInt(base.transform.position.x * 48f) / 48f, (float)Mathf.RoundToInt(base.transform.position.y * 48f) / 48f);
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (objects.Count == 0)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[1];
		}
		if (!objects.Contains(collision.gameObject))
		{
			objects.Add(collision.gameObject);
			GetComponent<AudioSource>().Play();
		}
	}

	private void OnTriggerExit2D(Collider2D collision)
	{
		if (objects.Contains(collision.gameObject))
		{
			objects.Remove(collision.gameObject);
		}
		if (objects.Count == 0)
		{
			GetComponent<SpriteRenderer>().sprite = sprites[0];
		}
	}
}
