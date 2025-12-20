using UnityEngine;

public class RockThatMoves : MonoBehaviour
{
	[SerializeField]
	private float xLimit;

	private Vector3 position;

	[SerializeField]
	private int flag = -1;

	[SerializeField]
	private BoxCollider2D barrier;

	[SerializeField]
	private Sprite barrierNewSprite;

	private void Awake()
	{
		position = base.transform.position;
		if ((int)Object.FindObjectOfType<GameManager>().GetFlag(21) == 1 && (flag == 19 || flag == 20))
		{
			position = new Vector3(xLimit, position.y);
			base.transform.position = position;
		}
		else if (flag != -1 && float.Parse(Object.FindObjectOfType<GameManager>().GetFlag(flag).ToString()) != 0f)
		{
			position = new Vector3(float.Parse(Object.FindObjectOfType<GameManager>().GetFlag(flag).ToString()), position.y);
			base.transform.position = position;
		}
	}

	private void LateUpdate()
	{
		if (base.transform.position.y == position.y && base.transform.position.x > position.x)
		{
			position = base.transform.position;
		}
		if (position.x >= xLimit)
		{
			position = new Vector3(xLimit, position.y);
			base.transform.parent.GetComponent<BoxCollider2D>().size = new Vector2(5f / 6f, 0.75f);
			if ((bool)barrier)
			{
				barrier.enabled = false;
				barrier.GetComponent<SpriteRenderer>().sprite = barrierNewSprite;
			}
		}
		base.transform.parent.position = position;
		base.transform.localPosition = Vector3.zero;
		if (flag != -1)
		{
			Object.FindObjectOfType<GameManager>().SetFlag(flag, position.x);
		}
	}
}
