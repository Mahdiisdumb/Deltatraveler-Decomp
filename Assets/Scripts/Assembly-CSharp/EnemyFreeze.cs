using UnityEngine;

public class EnemyFreeze : MonoBehaviour
{
	private int width;

	private int height;

	private int frames;

	private int heightRaiseRate = 8;

	private void Awake()
	{
		SetSprite(GetComponent<SpriteRenderer>().sprite);
	}

	private void Update()
	{
		if (frames * heightRaiseRate >= height)
		{
			return;
		}
		frames++;
		base.transform.Find("Mask").localScale = new Vector3(width, frames * heightRaiseRate);
		if (frames * heightRaiseRate >= height)
		{
			Object.Destroy(base.transform.Find("Mask").gameObject);
			SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
			for (int i = 0; i < componentsInChildren.Length; i++)
			{
				componentsInChildren[i].maskInteraction = SpriteMaskInteraction.None;
			}
		}
	}

	public void SetSprite(Sprite sprite)
	{
		SpriteRenderer[] componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].sprite = sprite;
		}
		width = (sprite.texture.width + 2) * 2;
		height = (sprite.texture.height + 2) * 2;
	}
}
