using UnityEngine;

public class NapstablookStreamBullet : BulletBase
{
	private Vector3 direction = new Vector3(0.1f, -0.9f);

	private float velocity = 1f / 48f;

	private bool isRight = true;

	[SerializeField]
	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		baseDmg = 6;
	}

	private void Update()
	{
		frames++;
		base.transform.position += direction * velocity;
		velocity += 1f / 192f;
		if (velocity > 1f / 6f)
		{
			velocity = 1f / 6f;
		}
		Vector3 vector = (isRight ? Vector3.right : Vector3.left);
		float x = Object.FindObjectOfType<SOUL>().transform.position.x;
		if (direction.y < 0f && base.transform.position.y <= -2.92f)
		{
			base.transform.position = new Vector3(base.transform.position.x, -2.92f);
			velocity = 1f / 48f;
			direction = vector;
			base.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
		}
		else if (direction.x == vector.x && Mathf.Abs(base.transform.position.x) >= 1.53f)
		{
			base.transform.position = new Vector3(vector.x * 1.53f, -2.92f);
			velocity = 1f / 48f;
			direction = Vector3.up;
			base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		else if (direction.y > 0f && base.transform.position.y >= -0.44f)
		{
			base.transform.position = new Vector3(vector.x * 1.53f, -0.44f);
			velocity = 1f / 48f;
			direction = -vector;
			base.transform.rotation = Quaternion.Euler(0f, 0f, 90f);
		}
		else if (0f - direction.x == vector.x && ((base.transform.position.x - x >= 0f && !isRight) || (base.transform.position.x - x <= 0f && isRight)))
		{
			base.transform.position = new Vector3(x, -0.44f);
			velocity = 1f / 48f;
			direction = Vector3.down;
			base.transform.rotation = Quaternion.Euler(0f, 0f, 0f);
		}
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 4 % 2];
	}

	public void SetToLeft()
	{
		isRight = false;
	}
}
