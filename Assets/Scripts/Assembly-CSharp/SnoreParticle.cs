using UnityEngine;

public class SnoreParticle : MonoBehaviour
{
	private SpriteRenderer snooze;

	private int frames;

	private Vector2 guranteedPosition;

	private bool active;

	private float snoreSize;

	private void Update()
	{
		if (active)
		{
			frames++;
			float num = 1f - (float)frames / 30f;
			snooze.color = new Color(1f, 1f, 1f, num);
			guranteedPosition += new Vector2(-0.08f * num, 0.06f * num);
			base.transform.position = guranteedPosition + new Vector2(Mathf.Sin((float)frames / 4f) / 4f, 0f);
			if (frames == 30)
			{
				Object.Destroy(base.gameObject);
			}
		}
	}

	public SnoreParticle CreateSnore(Vector2 firstPosition, float size)
	{
		snooze = base.gameObject.AddComponent<SpriteRenderer>();
		snooze.sprite = Util.PackManager().GetTranslatedSprite(Resources.Load<Sprite>("ui/spr_snooze"), "ui/spr_snooze");
		base.transform.localScale = new Vector2(size, size);
		guranteedPosition = firstPosition;
		base.transform.position = firstPosition;
		snooze.color = new Color(1f, 1f, 1f, 0.5f);
		active = true;
		return this;
	}
}
