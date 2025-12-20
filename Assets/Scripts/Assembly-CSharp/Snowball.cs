using UnityEngine;

public class Snowball : MonoBehaviour
{
	private Vector3 direction = Vector3.zero;

	private float speed;

	private bool hit;

	private float size = 2f;

	private int timer;

	private bool completed;

	private void Awake()
	{
		if ((int)Util.GameManager().GetFlag(87) >= 9)
		{
			Object.Destroy(base.gameObject);
		}
	}

	private void Update()
	{
		GetComponent<Rigidbody2D>().velocity = Vector2.zero;
		if (hit)
		{
			timer++;
			if (speed > 0f)
			{
				speed -= 0.1f;
			}
			else if (speed < 0f)
			{
				speed = 0f;
			}
			if (speed == 0f)
			{
				size -= 0.01f;
			}
			if (completed)
			{
				size -= 0.05f;
			}
			if (size < 0.2f)
			{
				Object.Destroy(base.gameObject);
			}
			else
			{
				GetComponent<Rigidbody2D>().MovePosition(base.transform.position + direction * speed / 24f);
				base.transform.Rotate(new Vector3(0f, 0f, (direction.x + direction.y * speed * 30f) / (size * 10f)));
				base.transform.localScale = new Vector3(size, size, 0f);
			}
		}
		GetComponent<SpriteRenderer>().sortingOrder = Mathf.RoundToInt(base.transform.position.y * -5f);
	}

	private void OnCollisionEnter2D(Collision2D collision)
	{
		if (completed)
		{
			return;
		}
		if ((bool)collision.gameObject.GetComponent<OverworldPlayer>())
		{
			float num = collision.gameObject.GetComponent<OverworldPlayer>().GetSpeed();
			if (num < 6f)
			{
				num = 6f;
			}
			speed = num * 2f / 3f + (1f / size + 0.1f);
			if (!hit)
			{
				Object.FindObjectOfType<SnowballHole>().ResetFlag();
			}
			hit = true;
			direction = (Vector2)base.transform.position - collision.GetContact(0).point;
			direction.Normalize();
		}
		else if ((bool)collision.gameObject.GetComponent<SnowballHole>() || (bool)collision.gameObject.GetComponent<OverworldVegetoid>())
		{
			if ((int)Util.GameManager().GetFlag(191) == 1)
			{
				completed = true;
				speed = 0f;
				Util.GameManager().PlayGlobalSFX("sounds/snd_won");
				Object.FindObjectOfType<SnowballHole>().RaiseFlag(timer, size);
			}
			else
			{
				GetComponent<AudioSource>().Play();
				speed = 8f + (1f / size + 0.1f);
				hit = true;
				direction = (Vector2)base.transform.position - collision.GetContact(0).point;
				direction.Normalize();
			}
		}
		else if (collision.gameObject.layer == 15)
		{
			Vector2 vector = collision.GetContact(0).point - (Vector2)base.transform.position;
			if (speed <= 0.06f)
			{
				speed = 0f;
			}
			else
			{
				speed -= 0.05f;
			}
			if (Mathf.Abs(vector.x) > Mathf.Abs(vector.y))
			{
				direction.x *= -1f;
				base.transform.position += new Vector3((float)((vector.x > 0f) ? (-2) : 2) / 24f, 0f);
			}
			else
			{
				direction.y *= -1f;
				base.transform.position += new Vector3(0f, (float)((vector.y > 0f) ? (-2) : 2) / 24f);
			}
		}
	}

	public void OnDestroy()
	{
		if ((bool)Object.FindObjectOfType<SnowballSpawner>() && (int)Util.GameManager().GetFlag(87) < 9)
		{
			Object.FindObjectOfType<SnowballSpawner>().SpawnSnowball();
		}
	}
}
