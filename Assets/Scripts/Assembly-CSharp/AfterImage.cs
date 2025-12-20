using UnityEngine;

public class AfterImage : MonoBehaviour
{
	private void Update()
	{
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, GetComponent<SpriteRenderer>().color.a - 1f / 24f);
		if (((Color32)GetComponent<SpriteRenderer>().color).a == 0)
		{
			Object.Destroy(base.gameObject);
		}
	}

	public void CreateAfterImage(Sprite sprite, Vector3 position)
	{
		base.transform.position = position;
		GetComponent<SpriteRenderer>().sprite = sprite;
		GetComponent<SpriteRenderer>().color = new Color(1f, 1f, 1f, 0.5f);
		GetComponent<SpriteRenderer>().sortingOrder = -500;
	}
}
