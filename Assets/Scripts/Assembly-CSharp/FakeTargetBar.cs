using UnityEngine;

public class FakeTargetBar : MonoBehaviour
{
	private Color baseColor = Color.white;

	private Color fadeColor;

	private bool quadSounds;

	private float toX;

	private bool activated;

	private bool hit;

	private int frames;

	private float speed = 0.2f;

	private void Awake()
	{
		baseColor = GetComponent<SpriteRenderer>().color;
	}

	public void Activate(float toX, float startX, bool quadSounds = false, float speed = 0.2f)
	{
		base.transform.localScale = new Vector3(1f, 1f, 1f);
		base.transform.localPosition = new Vector3(startX, base.transform.localPosition.y);
		GetComponent<SpriteRenderer>().color = baseColor;
		this.toX = toX;
		this.quadSounds = quadSounds;
		this.speed = speed;
		activated = true;
	}

	private void Update()
	{
		if (activated)
		{
			base.transform.localPosition -= new Vector3(speed, 0f);
			if (base.transform.localPosition.x <= toX)
			{
				frames = 0;
				base.transform.localPosition = new Vector3(toX, base.transform.localPosition.y);
				activated = false;
				hit = true;
				if (quadSounds)
				{
					GetComponent<AudioSource>().clip = Resources.Load<AudioClip>("sounds/snd_quad_" + ((toX == 0f) ? "crit" : "hit"));
					GetComponent<AudioSource>().Play();
				}
				fadeColor = ((toX == 0f) ? new Color(1f, 0.6f, 0f, 0f) : baseColor);
			}
		}
		else if (hit)
		{
			frames++;
			Color color = Color.Lerp(baseColor, fadeColor, (float)frames / 6f);
			color.a = 1f - (float)frames / 12f;
			GetComponent<SpriteRenderer>().color = color;
			base.transform.localScale *= 1.1f;
			if (frames == 12)
			{
				hit = false;
			}
		}
	}

	public bool IsActivated()
	{
		return activated;
	}

	public bool IsDoingHitAnim()
	{
		return hit;
	}
}
