using UnityEngine;

public class LilypadBell : Interactable
{
	private bool activated;

	private int frames;

	[SerializeField]
	private Sprite[] sprites;

	private Transform lilypads;

	private void Awake()
	{
		lilypads = GameObject.Find("LilyPods").transform;
	}

	private void Update()
	{
		if (!activated)
		{
			return;
		}
		frames++;
		GetComponent<SpriteRenderer>().sprite = sprites[frames / 5 % 4];
		if (frames > 15 && frames < 42)
		{
			float num = (float)(frames - 15) / 12f;
			SpriteRenderer[] componentsInChildren = lilypads.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = new Color(1f, 1f, 1f, 1f - num);
			}
		}
		else if (frames > 42)
		{
			float num2 = (float)(frames - 42) / 12f;
			SpriteRenderer[] componentsInChildren = lilypads.GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].color = new Color(1f, 1f, 1f, num2);
			}
			if (num2 >= 1f)
			{
				activated = false;
				GetComponent<SpriteRenderer>().sprite = sprites[0];
			}
		}
	}

	public override void DoInteract()
	{
		if (!activated)
		{
			GetComponent<AudioSource>().Play();
			activated = true;
			frames = 0;
		}
	}
}
