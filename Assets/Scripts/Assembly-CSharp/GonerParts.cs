using UnityEngine;

public class GonerParts : MonoBehaviour
{
	private SpriteRenderer sr;

	private SpriteRenderer head;

	private SpriteRenderer body;

	private string headPrefix = "";

	private string bodyPrefix = "";

	private bool flippedLegs;

	public bool spriteRendererEnabled = true;

	private bool forceRerender;

	private void Awake()
	{
		sr = GetComponent<SpriteRenderer>();
		sr.enabled = false;
		head = base.transform.GetChild(0).GetComponent<SpriteRenderer>();
		body = base.transform.GetChild(1).GetComponent<SpriteRenderer>();
		SetPrefixes();
	}

	private void LateUpdate()
	{
		if (spriteRendererEnabled)
		{
			head.enabled = true;
			body.enabled = true;
			head.sortingOrder = sr.sortingOrder + 1;
			body.sortingOrder = sr.sortingOrder;
			if ((!(GetSpriteEnding(sr.sprite.name) != GetSpriteEnding(head.sprite.name)) || !(GetSpriteEnding(sr.sprite.name) != GetSpriteEnding(body.sprite.name))) && !forceRerender)
			{
				return;
			}
			forceRerender = false;
			Sprite sprite = null;
			sprite = Resources.Load<Sprite>("player/GONER/heads/" + headPrefix + GetSpriteEnding(sr.sprite.name));
			if (sprite != null)
			{
				head.sprite = sprite;
			}
			bool flag = true;
			if (flippedLegs)
			{
				sprite = Resources.Load<Sprite>("player/GONER/bodies/" + bodyPrefix + "_flip" + GetSpriteEnding(sr.sprite.name));
				if (sprite != null)
				{
					body.sprite = sprite;
					flag = false;
				}
			}
			if (flag)
			{
				sprite = Resources.Load<Sprite>("player/GONER/bodies/" + bodyPrefix + GetSpriteEnding(sr.sprite.name));
				if (sprite != null)
				{
					body.sprite = sprite;
				}
			}
		}
		else
		{
			head.enabled = false;
			body.enabled = false;
		}
	}

	private string GetSpriteEnding(string spriteName)
	{
		return spriteName.Substring(spriteName.IndexOf("_", 4));
	}

	public void SetPrefixes()
	{
		int num = (int)Util.GameManager().GetFlag(215);
		int num2 = (int)Util.GameManager().GetFlag(216);
		flippedLegs = (int)Util.GameManager().GetFlag(217) == 1;
		headPrefix = "spr_gh_h" + num;
		bodyPrefix = "spr_gh_b" + num2;
		forceRerender = true;
	}
}
