using UnityEngine;

public class FloorBoneBullet : ActionBulletBase
{
	[SerializeField]
	private Sprite[] sprites;

	protected override void Awake()
	{
		base.Awake();
		destroyOnHit = false;
		baseDmg = 5;
	}

	protected override void Update()
	{
		base.Update();
		SpriteRenderer[] componentsInChildren;
		if (frames < 30)
		{
			frames++;
			if (frames >= 15)
			{
				int num = (frames - 15) / 2;
				if (num > 3)
				{
					num = 3;
				}
				componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
				for (int i = 0; i < componentsInChildren.Length; i++)
				{
					componentsInChildren[i].sprite = sprites[num];
				}
				if (frames == 15)
				{
					PlaySFX("sounds/snd_spearrise");
				}
			}
		}
		componentsInChildren = GetComponentsInChildren<SpriteRenderer>();
		for (int i = 0; i < componentsInChildren.Length; i++)
		{
			componentsInChildren[i].color = new Color(1f, 1f, 1f, Mathf.Lerp(0f, 1f, (float)frames / 15f));
		}
	}
}
