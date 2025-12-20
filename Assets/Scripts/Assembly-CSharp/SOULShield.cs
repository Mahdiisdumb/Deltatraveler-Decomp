using UnityEngine;

public class SOULShield : MonoBehaviour
{
	private Sprite[] sprites;

	private Vector2 dir = Vector2.up;

	private Transform face;

	private SpriteRenderer sprite;

	private AudioSource[] aud;

	private int frames = 10;

	private bool destroyAsap;

	private void Awake()
	{
		face = base.transform.parent.Find("face");
		sprite = base.transform.parent.Find("ShieldSprite").GetComponent<SpriteRenderer>();
	}

	private void Start()
	{
		sprites = new Sprite[2]
		{
			Resources.Load<Sprite>("battle/attacks/spr_soul_shield"),
			Resources.Load<Sprite>("battle/attacks/spr_soul_shield_protect")
		};
		aud = GetComponents<AudioSource>();
	}

	private void Update()
	{
		if (!destroyAsap)
		{
			if (frames < 5)
			{
				frames++;
				if (frames == 5)
				{
					sprite.sprite = sprites[0];
				}
			}
			if ((UTInput.GetAxis("Vertical") == 0f && UTInput.GetAxis("Horizontal") != 0f) || (UTInput.GetAxis("Vertical") != 0f && UTInput.GetAxis("Horizontal") == 0f))
			{
				if (UTInput.GetAxis("Vertical") == 1f && dir != Vector2.up)
				{
					dir = Vector2.up;
				}
				else if (UTInput.GetAxis("Horizontal") == 1f && dir != Vector2.right)
				{
					dir = Vector2.right;
				}
				else if (UTInput.GetAxis("Vertical") == -1f && dir != Vector2.down)
				{
					dir = Vector2.down;
				}
				else if (UTInput.GetAxis("Horizontal") == -1f && dir != Vector2.left)
				{
					dir = Vector2.left;
				}
			}
			face.localPosition = dir;
			Quaternion rotation = sprite.transform.rotation;
			base.transform.up = face.position - base.transform.position;
			if (base.transform.rotation.eulerAngles.y != 0f)
			{
				base.transform.rotation = Quaternion.Euler(0f, 0f, 180f);
			}
			sprite.transform.rotation = Quaternion.Lerp(rotation, base.transform.rotation, 0.75f);
		}
		else if (!aud[0].isPlaying && !aud[1].isPlaying && !aud[2].isPlaying)
		{
			Object.Destroy(base.transform.parent.gameObject);
		}
	}

	public void SetToDestroy()
	{
		sprite.enabled = false;
		base.transform.parent.Find("Boundary").GetComponent<SpriteRenderer>().enabled = false;
		destroyAsap = true;
	}

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (!(collision.tag == "Bullet"))
		{
			return;
		}
		if (!collision.GetComponent<GasterBlaster>())
		{
			collision.gameObject.layer = 2;
			Object.Destroy(collision.gameObject);
		}
		if (!collision.GetComponent<GasterBlaster>() || ((bool)collision.GetComponent<GasterBlaster>() && collision.gameObject.layer == 2))
		{
			sprite.sprite = sprites[1];
			frames = 0;
			if (!aud[0].isPlaying)
			{
				aud[0].Play();
			}
			else if (!aud[1].isPlaying)
			{
				aud[1].Play();
			}
			else if (!aud[2].isPlaying)
			{
				aud[2].Play();
			}
			else
			{
				aud[0].Play();
			}
		}
	}

	private void OnTriggerStay2D(Collider2D collision)
	{
		if ((bool)collision.GetComponent<GasterBlaster>() && collision.gameObject.layer == 2)
		{
			sprite.sprite = sprites[1];
			frames = 0;
		}
	}

	public Vector2 GetFaceDirection()
	{
		return dir;
	}
}
