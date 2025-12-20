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

	private OverworldPlayer player;

	private void Awake()
	{
		position = base.transform.position;
		if ((int)Util.GameManager().GetFlag(21) == 1 && (flag == 19 || flag == 20))
		{
			position = new Vector3(xLimit, position.y);
			base.transform.position = position;
		}
		else if (flag != -1 && float.Parse(Util.GameManager().GetFlag(flag).ToString()) != 0f)
		{
			position = new Vector3(float.Parse(Util.GameManager().GetFlag(flag).ToString()), position.y);
			base.transform.position = position;
		}
		player = Object.FindAnyObjectByType<OverworldPlayer>();
	}

	private void LateUpdate()
	{
		if (base.transform.position.y == position.y && base.transform.position.x > position.x)
		{
			base.transform.position = position + new Vector3(player.GetSpeed() / 48f, 0f);
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
			Util.GameManager().SetFlag(flag, position.x);
		}
		GetComponent<Rigidbody2D>().linearVelocityX = 0f;
	}
}
